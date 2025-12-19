Public Class wfAuditReSchedule_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mScheduleStartDate As New SmartDate(True)
    Private mAuditSchedule As AuditSchedule
    Private mAuditExecution As AuditExecution
    Private tmpAuditSchedule As AuditSchedule
    Private mComplianceEndDate As New SmartDate(True)
#End Region

#Region " Helper Methods  "
    Public Sub GetSession()
        mScheduleStartDate.Text = CType(Session("mScheduleStartDate"), String)
        mComplianceEndDate.Text = CType(Session("mComplianceEndDate"), String)
        mAuditSchedule = Session("mAuditSchedule")
        mAuditExecution = Session("mAuditExecution")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0


        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                  

                Case MsgBoxResult.No

                Case MsgBoxResult.Ok
                    Dim mopenas As String = Request.QueryString("Type")
                    If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    End If
            End Select
        ElseIf Result1 = -1 Then
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
        End If
    End Sub

    Private Function ReSchedule(ByVal mDate As SmartDate) As Boolean
        Dim mAuditSchedule As AuditSchedule = AuditSchedule.NewAuditSchedule()
        Dim tmpAuditSchedule As AuditSchedule = AuditSchedule.GetAuditSchedule(mAuditExecution.AuditScheduleID)

        mAuditSchedule.AuditID = tmpAuditSchedule.AuditID
        mAuditSchedule.ScheduleDate = mDate.Text  'mAuditExecution.EndDate
        mAuditSchedule.NextAuditDate = DateAdd(DateInterval.Month, tmpAuditSchedule.Frequency, mAuditSchedule.ScheduleDate)

        mAuditSchedule.Note = tmpAuditSchedule.Note
        mAuditSchedule.OtherInformation = tmpAuditSchedule.OtherInformation
        mAuditSchedule.DepartmentID = tmpAuditSchedule.DepartmentID

        mAuditSchedule.ToMailID = tmpAuditSchedule.ToMailID
        mAuditSchedule.CCMailID = tmpAuditSchedule.CCMailID
        mAuditSchedule.AuditOnID = tmpAuditSchedule.AuditOnID

        mAuditSchedule.AircraftID = tmpAuditSchedule.AircraftID
        mAuditSchedule.AuditOnDepartmentID = tmpAuditSchedule.AuditOnDepartmentID
        mAuditSchedule.LocationID = tmpAuditSchedule.LocationID
        mAuditSchedule.StoreID = tmpAuditSchedule.StoreID
        mAuditSchedule.VendorID = tmpAuditSchedule.VendorID
        mAuditSchedule.AuditOnText = tmpAuditSchedule.AuditOnText

        For Each tmpAuditScheduleTask As AuditScheduleTask In tmpAuditSchedule.AuditScheduleTasks
            mAuditSchedule.AuditScheduleTasks.Add(mAuditSchedule.ID)
            mAuditSchedule.AuditScheduleTasks.CurrentItem.AuditTaskID = tmpAuditScheduleTask.AuditTaskID
        Next

        If mAuditSchedule.IsValid Then
            mAuditSchedule.ApplyEdit()
            mAuditSchedule.Save()
            mAuditSchedule = Nothing
            tmpAuditSchedule = Nothing
            Return True
        Else
            Dim strMsg As String = ""
            If Not mAuditSchedule.IsValid Then
                For j As Integer = 0 To mAuditSchedule.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mAuditSchedule.GetBrokenRulesCollection(j).Description + "<BR>"
                Next
            End If

            If strMsg.Trim <> "" Then
                cvDescription.ErrorMessage = strMsg
                cvDescription.IsValid = mAuditSchedule.IsValid
            End If
            upnlValidation.Update()
            Return False
        End If

        Return False
    End Function
#End Region

#Region " Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not IsPostBack Then
            mScheduleStartDate.Text = CType(New SmartDate((DateAdd(DateInterval.Month, mAuditExecution.Frequency, CDate(mScheduleStartDate.ToString)).ToString)).Text, String)
            mComplianceEndDate.Text = CType(New SmartDate((DateAdd(DateInterval.Month, mAuditExecution.Frequency, CDate(mComplianceEndDate.ToString)).ToString)).Text, String)

            lblScheduleDate.Text = "(" + mScheduleStartDate.FormattedText + ")"
            lblComplianceDate.Text = "(" + mComplianceEndDate.FormattedText + ")"

            Session("mScheduleStartDate") = mScheduleStartDate.Text
            Session("mComplianceEndDate") = mComplianceEndDate.Text
            DataBind()
        End If
    End Sub

    Protected Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Dim mDate As SmartDate
        If rdbScheduleStartDate.Checked Then
            mDate = mScheduleStartDate
        Else
            mDate = mComplianceEndDate
        End If
        Try
            If ReSchedule(mDate) = True Then
                MSGBoxCtrl.show("Alert!!!", "This Audit has been Re-Scheduled for every " + mAuditExecution.Frequency.ToString + " months. ", "You can see this audit in Scheduled List.", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                upnlValidation.Update()
            End If
        Catch ex As Exception
        Finally
        
        End Try
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub

#End Region
End Class