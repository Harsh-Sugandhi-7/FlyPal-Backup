Public Class wfUpdateMailID_Ajax
    Inherits Page

#Region "Variable Declaration "

    Dim mModuleList As ModuleList
    Dim mTransactionList As TransactionList
    Dim mReportID As Integer = 0
    Dim mMailIDsDetail As String = ""

#End Region

#Region " Helper Methods "

    Private Sub GetSession()
        mModuleList = Session("mModuleList")
        mTransactionList = Session("mTransactionList")
        mReportID = Session("mID")
        mMailIDsDetail = Session("mMailIDsDetail")
    End Sub

    Private Sub SetSession()
        Session("mModuleList") = mModuleList
        Session("mTransactionList") = mTransactionList
    End Sub

    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfUpdateMailID_Ajax.aspx?" Then
            Session.Remove("mFAScsReportList")
        End If
    End Sub

    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub

    Private Sub SetGridObject()
        Session("mModuleList") = mModuleList
        Session("mTransactionList") = mTransactionList
    End Sub

#End Region

#Region " Data Binding "
    Public Sub DataFieldBind()
        mModuleList = ModuleList.GetModuleList(ModuleTypeID:=3, AddTopItem:="Select")
        Session("mModuleList") = mModuleList
        cmbReportNameList.DataSource = mModuleList
        mTransactionList = TransactionList.GetTransactionList("Select")
        Session("mTransactionList") = mTransactionList
        cmbTransactionList.DataSource = mTransactionList
        upnlMailIDs.Update()
        DataBind()
    End Sub

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()

        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            Session("MiddleFrame") = "wfUpdateMailID_Ajax.aspx?"
            DataFieldBind()

        End If
        If rdbReportlist.Checked Then
            placeholder1.Visible = True
            placeholder2.Visible = False

        ElseIf rdbTranslist.Checked Then
            placeholder2.Visible = True
            placeholder1.Visible = False

        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        mModuleList = Nothing
        Session("MiddleFrame") = ""
        Session.Remove("mID")
        mReportID = Nothing
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
        If IsValid Then
         
            SetGridObject()
            If rdbReportlist.Checked Then
                If cmbReportNameList.SelectedIndex = 0 Then
                    MSGBoxCtrl.show("Alert", "Please Select Report Name.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim mUM_csModule As [Module]
                mUM_csModule = [Module].GetModule(mModuleList.Item(cmbReportNameList.SelectedItem.ToString).ModuleID)
                mUM_csModule.SendToMailID = txtEmailIDs.Text.ToString
                mUM_csModule.SendCCMailID = txtCC.Text.ToString
                mUM_csModule.Save()
            Else
                If cmbTransactionList.SelectedIndex = 0 Then
                    MSGBoxCtrl.show("Alert", "Please Select transaction Name.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim mTransaction As Transaction
                mTransaction = Transaction.GetTransaction(cmbTransactionList.SelectedValue)
                mTransaction.ID = CInt(cmbTransactionList.SelectedValue)
                mTransaction.SendToMailID = txtEmailIDs.Text.ToString
                mTransaction.SendCCMailID = txtCC.Text.ToString
                mTransaction.Save()

            End If
            MarkLog(Action.Save, "UpdateMailIDs", mMailIDsDetail, ErrorType.NoError, Guid.Empty, EventLogID)
            Session.Remove("mMailIDsDetail")
            txtEmailIDs.Text = ""
            txtCC.Text = ""
            upnlMailIDs.Update()
            DataFieldBind()
            MSGBoxCtrl.show("Updated Successfully", "Updated Successfully", "", MsgBoxStyle.OkOnly, "")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub cmbReportNameList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbReportNameList.SelectedIndexChanged
        mModuleList = Session("mModuleList")
        If cmbReportNameList.SelectedIndex > 0 Then
            txtEmailIDs.Text = mModuleList.Item(cmbReportNameList.SelectedItem.ToString).SendToMailID
            txtCC.Text = mModuleList.Item(cmbReportNameList.SelectedItem.ToString).SendCCMailID
        Else
            txtEmailIDs.Text = ""
            txtCC.Text = ""

        End If

        Session("mID") = cmbReportNameList.SelectedValue
        upnlMailIDs.Update()
    End Sub

    Private Sub cmbTransactionList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbTransactionList.SelectedIndexChanged
        mTransactionList = Session("mTransactionList")
        If cmbTransactionList.SelectedIndex > 0 Then
            txtEmailIDs.Text = mTransactionList.Item(CInt(cmbTransactionList.SelectedValue)).SendToMailID
            txtCC.Text = mTransactionList.Item(CInt(cmbTransactionList.SelectedValue)).SendCCMailID
        Else
            txtEmailIDs.Text = ""
            txtCC.Text = ""

        End If
        upnlMailIDs.Update()
    End Sub

#End Region

End Class