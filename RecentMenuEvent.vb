'Created By Utkarsh On 3-Jun-2011

Public Class RecentMenuEvent

    Public Shared Sub RecentMenuItemEvent(ByVal UserName As String, ByVal ModuleID As Integer)

        Dim mRecentlyUsedReportList As RecentlyUsedReportList
        Dim mRecentlyUsedReport As RecentlyUsedReport
        Dim mUserList As UserList = UserList.GetUserList(UserName, , UserName)
        Dim mUserId As Guid

        mUserId = mUserList.Item(UserName).UserID()

        mRecentlyUsedReportList = RecentlyUsedReportList.GetRecentlyUsedReportList()

        If mRecentlyUsedReportList.Contains(mUserId, ModuleID) Then

            mRecentlyUsedReport = RecentlyUsedReport.GetRecentlyUsedReport(mRecentlyUsedReportList.Item(ModuleID, "").ID)

            mRecentlyUsedReport.ClickCount = mRecentlyUsedReport.ClickCount + 1
            mRecentlyUsedReport.DateTime = Now.ToString

            Try
                mRecentlyUsedReport.Save()
            Catch ex As Exception
                ex.GetBaseException()
            End Try
        Else
            mRecentlyUsedReport = RecentlyUsedReport.NewRecentlyUsedReport(UserName, ModuleID, Now.ToString, 1)
            Try
                mRecentlyUsedReport.Save()
            Catch ex As Exception
                ex.GetBaseException()
            End Try
        End If
    End Sub

End Class
