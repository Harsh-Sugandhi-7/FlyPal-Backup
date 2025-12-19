Public Class StickyNotejs
    Inherits System.Web.UI.Page

#Region "Variable Declaration "
    Private mAlertCount As AlertCount
    Private mAlertList As AlertList  'Added by Saylee on 3-May-2010
    Private mAlert As Alert
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        mAlertCount = AlertCount.GetAlertCountList()
        mAlertList = AlertList.GetAlertList()
        For i As Integer = 0 To mAlertList.Count - 1
            mAlert = Alert.GetChildAlert(mAlertList(i).ID)
            Try
                mAlert.DateTime = Today.Date.ToString
                Select Case mAlertList(i).SrNo
                    Case 1
                        mAlert.Count = mAlertCount.OMRCount
                        mAlert.Save()
                    Case 2
                        mAlert.Count = mAlertCount.DueFCICount
                        mAlert.Save()
                    Case 3
                        mAlert.Count = mAlertCount.ExpiredItemsCount
                        mAlert.Save()
                    Case 4
                        mAlert.Count = mAlertCount.ExpiringItemsCount
                        mAlert.Save()
                    Case 5
                        mAlert.Count = mAlertCount.CoreUnitDueCount
                        mAlert.Save()
                End Select
            Catch ex As Exception
                Throw ex
            End Try

        Next
        '******************************************************

        mAlertList = AlertList.GetAlertList()
        If mAlertList.Count > 0 Then
            lnkPendingOrder.Text = mAlertList(0).DescCount
            lnkCalibrationDueReport.Text = mAlertList(1).DescCount
            lnkExpiredItems.Text = mAlertList(2).DescCount
            lnkItemsToExpire.Text = mAlertList(3).DescCount
            lnkCoreUnitDue.Text = mAlertList(4).DescCount

        End If
        Dim str As String
        str = "displyStickyNote();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str, True)
    End Sub
#End Region
End Class