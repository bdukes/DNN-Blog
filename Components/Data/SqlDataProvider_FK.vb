Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data

Namespace Data

 Partial Public Class SqlDataProvider

#Region " BlogPermission Methods "
	Public Overrides Function GetBlogPermissionsByBlog(blogID As Int32 , StartRowIndex As Integer, MaximumRows As Integer, OrderBy As String) As IDataReader
		Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "GetBlogPermissionsByBlog", blogID, StartRowIndex, MaximumRows, OrderBy.ToUpper), IDataReader)
	End Function

#End Region

#Region " Blog Methods "

  Public Overrides Function GetBlogsByCreatedByUser(userID As Int32, StartRowIndex As Integer, MaximumRows As Integer, OrderBy As String) As IDataReader
   Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "GetBlogsByCreatedByUser", userID, StartRowIndex, MaximumRows, OrderBy.ToUpper), IDataReader)
  End Function

#End Region

#Region " Comment Methods "
  Public Overrides Function GetCommentsByContentItem(contentItemId As Int32, IncludeNonApproved As Boolean) As IDataReader
   Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "GetCommentsByContentItem", contentItemId, IncludeNonApproved), IDataReader)
  End Function

  Public Overrides Function GetCommentsByModuleId(moduleId As Int32) As IDataReader
   Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "GetCommentsByModuleId", moduleId), IDataReader)
  End Function
#End Region

#Region " Post Methods "
  Public Overrides Function GetPostsByBlog(blogID As Int32, displayLocale As String, pageIndex As Int32, pageSize As Int32, orderBy As String) As IDataReader
   Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & ModuleQualifier & "GetPostsByBlog", blogID, displayLocale, pageIndex, pageSize, orderBy), IDataReader)
  End Function
#End Region

 End Class

End Namespace
