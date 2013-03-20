﻿'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2012
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Imports System.Linq
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Localization.Localization
Imports DotNetNuke.Entities.Content.Taxonomy

Namespace Common

 Public Class Globals

#Region " Constants "
  Public Const SharedResourceFileName As String = "~/DesktopModules/Blog/App_LocalResources/SharedResources.resx"
  Public Const glbAppName As String = "Blog"
  Public Const glbImageHandlerPath As String = "~/DesktopModules/Blog/BlogImage.ashx"
  Public Const glbPermittedFileExtensions As String = ".jpg,.png,.gif,.bmp,"
  Public Const glbTemplatesPath As String = "~/DesktopModules/Blog/Templates/"
  Public Const glbServicesPath As String = "~/DesktopModules/Blog/API/"
  Public Const BloggerPermission As String = "BLOGGER"

  Public Enum SummaryType
   PlainTextIndependent = 0
   HtmlIndependent = 1
   HtmlPrecedesPost = 2
  End Enum
#End Region

#Region " Dates "
  Public Shared Function FormatDate([Date] As DateTime, Culture As String, DateFormat As String, TimeZone As TimeZoneInfo) As String
   Return FormatDate([Date], Culture, DateFormat, TimeZone, False)
  End Function
  Public Shared Function FormatDate([Date] As DateTime, Culture As String, DateFormat As String, TimeZone As TimeZoneInfo, ToUniversal As Boolean) As String
   If String.IsNullOrEmpty(Culture) Then Culture = Threading.Thread.CurrentThread.CurrentCulture.Name
   Dim dtf As System.Globalization.DateTimeFormatInfo = New System.Globalization.CultureInfo(Culture, False).DateTimeFormat
   If ToUniversal = True Then
    Dim dto As New DateTimeOffset([Date])
    Return dto.ToUniversalTime.ToString(DateFormat, dtf)
   Else
    Dim dt As Date = AdjustedDate([Date], TimeZone)
    Return dt.ToString(DateFormat, dtf)
   End If
  End Function

  Public Shared Function AdjustedDate([Date] As DateTime, TimeZone As TimeZoneInfo) As Date
   Dim dto As New DateTimeOffset([Date])
   Return TimeZoneInfo.ConvertTime(dto, TimeZone).DateTime
  End Function

  Public Shared Function DateFromUtc([Date] As DateTime, TimeZone As TimeZoneInfo) As Date
   Dim dto As New DateTimeOffset([Date], New TimeSpan(0))
   Return TimeZoneInfo.ConvertTime(dto, TimeZone).DateTime
  End Function

  Public Shared Function ParseDate(DateString As String, Culture As String) As DateTime
   Dim dtf As System.Globalization.DateTimeFormatInfo = New System.Globalization.CultureInfo(Culture, False).DateTimeFormat
   Try
    Return Date.Parse(DateString, dtf)
   Catch ex As Exception
    Return Nothing
   End Try
  End Function

  Public Shared Function IsValidDate(DateString As String, Culture As String) As Boolean
   Dim dtf As System.Globalization.DateTimeFormatInfo = New System.Globalization.CultureInfo(Culture, False).DateTimeFormat
   Try
    Dim oDate As Date = Date.Parse(DateString, dtf)
    Return True
   Catch ex As Exception
    Return False
   End Try
  End Function

  Public Shared Function GetLocalAddedTime(AddedDate As DateTime, PortalId As Integer, user As DotNetNuke.Entities.Users.UserInfo) As DateTime
   Return TimeZoneInfo.ConvertTimeToUtc(AddedDate, user.Profile.PreferredTimeZone)
  End Function
#End Region

#Region " Other "
  Public Shared Function ManifestFilePath(moduleId As Integer) As String
   Return "/DesktopModules/Blog/WLWManifest.aspx?ModuleId=" & moduleId.ToString
  End Function

  Public Shared Function GetAString(Value As Object) As String
   If Value Is Nothing Then
    Return ""
   Else
    If Value Is DBNull.Value Then
     Return ""
    Else
     Return CType(Value, String)
    End If
   End If
  End Function

  Public Shared Function ReadFile(fileName As String) As String
   If Not IO.File.Exists(fileName) Then Return ""
   Using sr As New IO.StreamReader(fileName)
    Return sr.ReadToEnd
   End Using
  End Function

  Public Shared Function FormatBoolean(value As Boolean, format As String) As String
   If String.IsNullOrEmpty(format) Then
    Return value.ToString
   End If
   If format.Contains(";") Then
    If value Then
     Return Left(format, format.IndexOf(";"))
    Else
     Return Mid(format, format.IndexOf(";") + 2)
    End If
   End If
   Return value.ToString
  End Function

  Public Shared Sub SetSummary(ByRef entry As Entities.Entries.EntryInfo, settings As ModuleSettings)
   entry.Summary = entry.Content
   entry.Summary = RemoveHtmlTags(entry.Summary) ' we can't auto shorten HTML as it risks creating invalid HTML
   entry.Summary = Left(entry.Summary, settings.AutoGeneratedSummaryLength)
  End Sub

  Public Shared Function RemoveHtmlTags(inputString As String) As String
   inputString = Regex.Replace(inputString, "<[^>]+>", "")
   Return (New DotNetNuke.Security.PortalSecurity).InputFilter(inputString, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting Or DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup)
  End Function

  Friend Shared Function GetPortalVocabularies(portalId As Integer) As List(Of Vocabulary)
   Dim cntVocab As IVocabularyController = DotNetNuke.Entities.Content.Common.Util.GetVocabularyController()
   Dim colVocabularies As IQueryable(Of Vocabulary) = cntVocab.GetVocabularies()
   Dim portalVocabularies As IQueryable(Of Vocabulary) = From v In colVocabularies Where v.ScopeTypeId = 2 And v.ScopeId = portalId
   Return portalVocabularies.ToList()
  End Function
#End Region

 End Class

End Namespace