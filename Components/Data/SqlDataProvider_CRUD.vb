Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke.Framework.Providers

Namespace Data

 Partial Public Class SqlDataProvider
  Inherits DataProvider

#Region " Private Members "

  Private Const ProviderType As String = "data"
  Private Const ModuleQualifier As String = "Blog_"

  Private _providerConfiguration As DotNetNuke.Framework.Providers.ProviderConfiguration = DotNetNuke.Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
  Private _connectionString As String
  Private _providerPath As String
  Private _objectQualifier As String
  Private _databaseOwner As String

#End Region

#Region " Constructors "

  Public Sub New()

   ' Read the configuration specific information for this provider
   Dim objProvider As DotNetNuke.Framework.Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Provider)

   'Get Connection string from web.config
   _connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString()

   If _connectionString = "" Then
    ' Use connection string specified in provider
    _connectionString = objProvider.Attributes("connectionString")
   End If

   _providerPath = objProvider.Attributes("providerPath")

   _objectQualifier = objProvider.Attributes("objectQualifier")
   If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
    _objectQualifier += "_"
   End If

   _databaseOwner = objProvider.Attributes("databaseOwner")
   If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
    _databaseOwner += "."
   End If

  End Sub

#End Region

#Region " Properties "

  Public ReadOnly Property ConnectionString() As String
   Get
    Return _connectionString
   End Get
  End Property

  Public ReadOnly Property ProviderPath() As String
   Get
    Return _providerPath
   End Get
  End Property

  Public ReadOnly Property ObjectQualifier() As String
   Get
    Return _objectQualifier
   End Get
  End Property

  Public ReadOnly Property DatabaseOwner() As String
   Get
    Return _databaseOwner
   End Get
  End Property

#End Region

#Region " General Methods "
  Public Overrides Function GetNull(Field As Object) As Object
   Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
  End Function
#End Region


#Region " BlogPermission Methods "

  Public Overrides Function GetBlogPermission(blogId As Int32, permissionId As Int32, roleId As Int32, userId As Int32) As IDataReader
   Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "GetBlogPermission", blogId, permissionId, roleId, userId), IDataReader)
  End Function

  Public Overrides Sub AddBlogPermission(allowAccess As Boolean, blogId As Int32, expires As Date, permissionId As Int32, roleId As Int32, userId As Int32)
   SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "AddBlogPermission", allowAccess, blogId, GetNull(expires), permissionId, roleId, userId)
  End Sub

  Public Overrides Sub UpdateBlogPermission(allowAccess As Boolean, blogId As Int32, expires As Date, permissionId As Int32, roleId As Int32, userId As Int32)
   SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "UpdateBlogPermission", allowAccess, blogId, GetNull(expires), permissionId, roleId, userId)
  End Sub

  Public Overrides Sub DeleteBlogPermission(blogId As Int32, permissionId As Int32, roleId As Int32, userId As Int32)
   SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "DeleteBlogPermission", blogId, permissionId, roleId, userId)
  End Sub

#End Region

#Region " Blog Methods "

  Public Overrides Function AddBlog(moduleID As Int32, copyright As String, description As String, fullLocalization As Boolean, image As String, includeAuthorInFeed As Boolean, includeImagesInFeed As Boolean, locale As String, mustApproveGhostPosts As Boolean, ownerUserId As Int32, publishAsOwner As Boolean, published As Boolean, syndicated As Boolean, syndicationEmail As String, title As String, createdByUser As Int32) As Integer
   Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "AddBlog", moduleID, GetNull(copyright), GetNull(description), fullLocalization, GetNull(image), includeAuthorInFeed, includeImagesInFeed, locale, mustApproveGhostPosts, ownerUserId, publishAsOwner, published, syndicated, GetNull(syndicationEmail), title, createdByUser), Integer)
  End Function

  Public Overrides Sub UpdateBlog(blogID As Int32, moduleID As Int32, copyright As String, description As String, fullLocalization As Boolean, image As String, includeAuthorInFeed As Boolean, includeImagesInFeed As Boolean, locale As String, mustApproveGhostPosts As Boolean, ownerUserId As Int32, publishAsOwner As Boolean, published As Boolean, syndicated As Boolean, syndicationEmail As String, title As String, updatedByUser As Int32)
   SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "UpdateBlog", blogID, moduleID, GetNull(copyright), GetNull(description), fullLocalization, GetNull(image), includeAuthorInFeed, includeImagesInFeed, locale, mustApproveGhostPosts, ownerUserId, publishAsOwner, published, syndicated, GetNull(syndicationEmail), title, updatedByUser)
  End Sub

  Public Overrides Sub DeleteBlog(blogID As Int32)
   SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "DeleteBlog", blogID)
  End Sub

#End Region

#Region " Comment Methods "

  Public Overrides Function GetComment(commentID As Int32) As IDataReader
   Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "GetComment", commentID), IDataReader)
  End Function

  Public Overrides Function AddComment(approved As Boolean, author As String, comment As String, contentItemId As Int32, email As String, parentId As Int32, website As String, createdByUser As Int32) As Integer
   Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "AddComment", approved, GetNull(author), comment, contentItemId, GetNull(email), GetNull(parentId), GetNull(website), createdByUser), Integer)
  End Function

  Public Overrides Sub UpdateComment(approved As Boolean, author As String, comment As String, commentID As Int32, contentItemId As Int32, email As String, parentId As Int32, website As String, updatedByUser As Int32)
   SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "UpdateComment", approved, GetNull(author), comment, commentID, contentItemId, GetNull(email), GetNull(parentId), GetNull(website), updatedByUser)
  End Sub

  Public Overrides Sub DeleteComment(commentID As Int32)
   SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "DeleteComment", commentID)
  End Sub

#End Region

#Region " Post Methods "

  Public Overrides Function GetPost(contentItemId As Int32, moduleId As Int32, locale As String) As IDataReader
   Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "GetPost", contentItemId, moduleId, locale), IDataReader)
  End Function

  Public Overrides Function AddPost(allowComments As Boolean, blogID As Int32, content As String, copyright As String, displayCopyright As Boolean, image As String, locale As String, published As Boolean, publishedOnDate As Date, summary As String, termIds As String, title As String, viewCount As Int32, createdByUser As Int32) As Integer
   Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "AddPost", allowComments, blogID, content, copyright, displayCopyright, image, locale, published, publishedOnDate, summary, termIds, title, viewCount, createdByUser), Integer)
  End Function

  Public Overrides Sub UpdatePost(allowComments As Boolean, blogID As Int32, content As String, contentItemId As Int32, copyright As String, displayCopyright As Boolean, image As String, locale As String, published As Boolean, publishedOnDate As Date, summary As String, termIds As String, title As String, viewCount As Int32, updatedByUser As Int32)
   SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "UpdatePost", allowComments, blogID, content, contentItemId, copyright, displayCopyright, image, locale, published, publishedOnDate, summary, termIds, title, viewCount, updatedByUser)
  End Sub

  Public Overrides Sub DeletePost(contentItemId As Int32)
   SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "DeletePost", contentItemId)
  End Sub

#End Region


 End Class

End Namespace
