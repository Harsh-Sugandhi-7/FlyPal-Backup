Imports System.Runtime.CompilerServices.RuntimeHelpers

Public Class wfLinkMaintenanceActivity_Ajax
    Inherits System.Web.UI.Page


#Region " Variable Declaration "
    Protected mLinkMaintenanceList As LinkMaintenanceList
    Protected mLinkMaintenanceActionList As LinkMaintenanceActionList
    Protected mLinkMaintenance As LinkMaintenance
    Protected EventLogID As Guid
    Protected MaintActivityID As Guid
    Dim ModelID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mLinkMaintenanceActionList = Session("mLinkMaintenanceActionList")
        mLinkMaintenanceList = Session("mLinkMaintenanceList")
        MaintActivityID = CType(Session("MaintActivityID"), Guid)
        ModelID = Session("ModelIDForMPD")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mLinkMaintenanceActionList") 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        Session.Remove("mLinkMaintenanceList")
        Session.Remove("URL")
        Session.Remove("MaintenanceActivityID") 'End
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
    Private Sub SetLinkMaintenanceGridObject()
        Dim txtRemark As TextBox
        Dim cmbAction As DropDownList

        For i As Integer = 0 To dgLinkedMaintenanceList.Rows.Count - 1
            txtRemark = CType(dgLinkedMaintenanceList.Rows(i).FindControl("txtRemark"), TextBox)
            cmbAction = CType(dgLinkedMaintenanceList.Rows(i).FindControl("cmbLinkMaintActionlist"), DropDownList)
            mLinkMaintenanceList(i).Remark = txtRemark.Text.Trim
            mLinkMaintenanceList(i).MaintenanceActionID = cmbAction.SelectedValue
        Next
        Session("mLinkMaintenanceList") = mLinkMaintenanceList
    End Sub 'End
    Private Sub SaveLinkList()
        If dgLinkedMaintenanceList.Rows.Count > 0 Then
            SetLinkMaintenanceGridObject()
            Dim mLinkMaintenanceListClone As LinkMaintenanceList
            mLinkMaintenanceListClone = CType(mLinkMaintenanceList.Clone, LinkMaintenanceList)
            Try
                mLinkMaintenanceList = CType(mLinkMaintenanceList.Save, LinkMaintenanceList)

            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
                End If
                mLinkMaintenanceList = mLinkMaintenanceListClone
                Session("mLinkMaintenanceList") = mLinkMaintenanceList
            End Try
            dgLinkedMaintenanceList.DataSource = mLinkMaintenanceList
            dgLinkedMaintenanceList.DataBind()
            upnlLinkedMaintenanceList.Update()
        End If
    End Sub
    Public Sub CustomValidate3(ByVal s As Object, ByVal e As ServerValidateEventArgs) 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        Dim CustValidator As CustomValidator = CType(s, CustomValidator)
        Dim counter As Integer

        SetLinkMaintenanceGridObject()
        Dim str As String = ""
        For counter = 0 To dgLinkedMaintenanceList.Rows.Count - 1
            If Not mLinkMaintenanceList(counter).IsValid Then
                For i As Integer = 0 To mLinkMaintenanceList(counter).GetBrokenRulesCollection.Count - 1
                    If mLinkMaintenanceList(counter).GetBrokenRulesCollection(i).Description.Equals("Action Required") Then
                        If Not str.Contains("Action Required") Then
                            str = str + mLinkMaintenanceList(counter).GetBrokenRulesCollection(i).Description + "<BR>"
                        End If
                    End If
                    If mLinkMaintenanceList(counter).GetBrokenRulesCollection(i).Description.Equals("Remark too Long") Then
                        If Not str.Contains("Remark too Long") Then
                            str = str + mLinkMaintenanceList(counter).GetBrokenRulesCollection(i).Description + "<BR>"
                        End If
                    End If

                Next
            End If
        Next
        If str <> "" Then
            CustValidator.ErrorMessage = str
            e.IsValid = False
        Else
            e.IsValid = True
        End If
    End Sub
    Private Sub ControlVisibility()
        If AppSettings("LinkMaintenance") = True Then
            If Not mLinkMaintenanceList Is Nothing Then
                dgLinkedMaintenanceList.Columns(7).Visible = mLinkMaintenanceList.ShowDirectiveNo
            End If
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "DeleteLM" Then
                        Try
                            If Not mLinkMaintenanceList.Count = 1 Then
                                mLinkMaintenanceList.Remove(mLinkMaintenanceList.CurrentItem)
                            Else
                                If mLinkMaintenanceList.CurrentItem.IsNew Then
                                    mLinkMaintenanceList.Remove(mLinkMaintenanceList.CurrentItem)
                                Else
                                    mLinkMaintenanceList.Remove(mLinkMaintenanceList.CurrentItem)
                                    mLinkMaintenanceList.Save()
                                End If

                            End If
                            Session("mLinkMaintenanceList") = mLinkMaintenanceList
                            dgLinkedMaintenanceList.DataSource = mLinkMaintenanceList
                            dgLinkedMaintenanceList.DataBind()
                            lblResult.Text = "List Of Linked Maintenance Activity : " & mLinkMaintenanceList.Count & " Record(s) found."
                            upnlLinkedMaintenanceList.Update()
                        Catch ex As SqlException
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No

                Case MsgBoxResult.Ok

            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'mLinkMaintenanceActionList = LinkMaintenanceActionList.GetLinkMaintActionList(True) 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        If mLinkMaintenanceList Is Nothing Then
            mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(MaintActivityID.ToString)
        End If
        dgLinkedMaintenanceList.DataSource = mLinkMaintenanceList
        'Session("mLinkMaintenanceActionList") = mLinkMaintenanceActionList
        Session("mLinkMaintenanceList") = mLinkMaintenanceList 'End
        DataBind()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If cmbMonitorType.Enabled = True Then
                cmbMonitorType.Focus()
            End If
            DataFieldBind()
            ControlVisibility()
            If AppSettings("ShowMaintenanceForNewClients") = "False" Then
                cmbMonitorType.Items.Add(New ListItem("Service", "1"))
                cmbMonitorType.Items.Add(New ListItem("Inspection", "2"))
                cmbMonitorType.Items.Add(New ListItem("Directive", "3"))

            Else
                cmbMonitorType.Items.Add(New ListItem("MPD", "1"))
                cmbMonitorType.Items.Add(New ListItem("Directive", "3"))
            End If
            SetFocus(cmbMonitorType)
        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        SaveLinkList()
    End Sub
    Private Sub btnAddNewLinkMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewLinkMaintenance.Click
        Dim URL As Stack = New Stack    'STACK to store url of current page
        URL.Push(Request.Url)           'Inserting URL in STACK
        Session("URL") = URL
        Session("MaintenanceActivityID") = MaintActivityID
        Response.Redirect("wfModelMonitorActivityList.aspx?FromType=" & cmbMonitorType.SelectedValue)
    End Sub
    Private Sub dgLinkedMaintenanceList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLinkedMaintenanceList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                MSGBoxCtrl.show(MSGBox.Message_title.DeleteAlert, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteLM")
                Dim Index As Int32 = CInt(e.CommandArgument) + dgLinkedMaintenanceList.PageIndex * dgLinkedMaintenanceList.PageSize
                mLinkMaintenanceList.CurrentIndex = Index
                Session("mLinkMaintenanceList") = mLinkMaintenanceList
        End Select
    End Sub
    Private Sub dgLinkedMaintenanceList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgLinkedMaintenanceList.Sorting
        mLinkMaintenanceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mLinkMaintenanceList") = mLinkMaintenanceList
        dgLinkedMaintenanceList.DataSource = mLinkMaintenanceList
        dgLinkedMaintenanceList.DataBind()
        upnlLinkedMaintenanceList.Update()
    End Sub 'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
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