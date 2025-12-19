Public Class wfUpdateMailIDsForFAS_Ajax
    Inherits Page

#Region "Variable Declaration "
    Dim mFAScsReportList As FAScsReportList
    Dim mFAScsReportList1 As FAScsReportList
    Dim mReportID As Integer = 0
    Dim mMailIDsDetail As String = ""

#End Region

#Region " Helper Methods "
    Private Sub addAttributes()
        txtDayofMonth.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtDayofMonth').value,event)")
    End Sub
   
    Private Sub GetSession()
        mFAScsReportList = Session("mFAScsReportList")
        mFAScsReportList1 = Session("mFAScsReportList1")
        mReportID = Session("mID")
        mMailIDsDetail = Session("mMailIDsDetail") 'Added By Prashant on 14-Jan-2021
    End Sub
    Private Sub SetSession()
        Session("mFAScsReportList") = mFAScsReportList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfUpdateMailIDsForFAS_Ajax.aspx?" Then
            Session.Remove("mFAScsReportList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetGridObject()
        Session("mFAScsReportList") = mFAScsReportList
    End Sub
#End Region

#Region " Data Binding "
    Public Sub DataFieldBind()
        mFAScsReportList = FAScsReportList.GetFAScsReportList(IsFromUpdateMailIDsForFAS:=1)
        dgReportNameList.DataSource = mFAScsReportList
        Session("mFAScsReportList") = mFAScsReportList

        mFAScsReportList1 = FAScsReportList.GetFAScsReportList("Select", IsFromUpdateMailIDsForFAS:=1)
        cmbReportNameList.DataSource = mFAScsReportList1
        Session("mFAScsReportList1") = mFAScsReportList1

        upnlReportNameListGrid.Update()
        DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            Session("MiddleFrame") = "wfUpdateMailIDsForFAS_Ajax.aspx?"
            DataFieldBind()
        End If
        upnlMailIDs.Update()
    End Sub

    Private Sub dgReportNameList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgReportNameList.PageIndexChanging
        dgReportNameList.PageIndex = e.NewPageIndex
        dgReportNameList.DataSource = mFAScsReportList
        Session("mFAScsReportList") = mFAScsReportList
        dgReportNameList.DataBind()
    End Sub

    Private Sub dgReportNameList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgReportNameList.RowCommand
        Dim idx As Int32
        Dim mId As New Int32
        Select Case e.CommandName
            Case "EditRec"

                If (Not User.IsInRole("UpdateEmailIDsForFASView")) Then
                    SetSession()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If

                idx = CInt(e.CommandArgument) + dgReportNameList.PageIndex * dgReportNameList.PageSize
                Session("EditReportName") = True
                mId = mFAScsReportList(idx).ID
                Session("mID") = mId
                Dim mName As String = mFAScsReportList(idx).ReportName
                cmbReportNameList.SelectedValue = mId
                txtEmailIDs.Text = mFAScsReportList(idx).Emails
                txtCC.Text = mFAScsReportList(idx).cc
                txtBCC.Text = mFAScsReportList(idx).Bcc
                cmbMonday.SelectedValue = mFAScsReportList(idx).Monday
                cmbTuesday.SelectedValue = mFAScsReportList(idx).Tuesday
                cmbWednesday.SelectedValue = mFAScsReportList(idx).Wednesday
                cmbThursday.SelectedValue = mFAScsReportList(idx).Thursday
                cmbFriday.SelectedValue = mFAScsReportList(idx).Friday
                cmbSaturday.SelectedValue = mFAScsReportList(idx).Saturday
                cmbSunday.SelectedValue = mFAScsReportList(idx).Sunday
                txtDayofMonth.Text = mFAScsReportList(idx).DayOfMonth
                IsDaily.Checked = mFAScsReportList(idx).IsDaily
                upnlMailIDs.Update()
                mMailIDsDetail = "Existing EmailIDs: " + txtEmailIDs.Text + " CC MailIDs: " + txtCC.Text + " BCC MailIDs: " + txtBCC.Text + " Monday: " + cmbMonday.SelectedItem.Text + " Tuesday: " + cmbTuesday.SelectedItem.Text + " Wednesday: " + cmbWednesday.SelectedItem.Text + " Thursday: " + cmbThursday.SelectedItem.Text + " Friday: " + cmbFriday.SelectedItem.Text + " Saturday: " + cmbSaturday.SelectedItem.Text + " Sunday: " + cmbSunday.SelectedItem.Text + " DayofMonth: " + txtDayofMonth.Text + " Is Daily: " + IsDaily.Checked.ToString
                Session("mMailIDsDetail") = mMailIDsDetail 'Added By Prashant on 14-Jan-2021
                MarkLog(Action.Edit, "UpdateMailIDsForFAS", mMailIDsDetail, ErrorType.NoError, Guid.Empty, EventLogID) 'Added By Prashant on 14-Jan-2021
        End Select
    End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        mFAScsReportList = Nothing
        Session("MiddleFrame") = ""
        Session.Remove("mID")
        mReportID = Nothing
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
        If IsValid Then
            If cmbReportNameList.SelectedIndex = 0 Or mReportID = 0 Then
                MSGBoxCtrl.show("Alert", "Please Select Report Name.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            SetGridObject()
            FAScsReportList.SaveReport(mReportID, txtEmailIDs.Text, txtCC.Text, txtBCC.Text, cmbMonday.SelectedValue, cmbTuesday.SelectedValue, _
                                       cmbWednesday.SelectedValue, cmbThursday.SelectedValue, cmbFriday.SelectedValue, cmbSaturday.SelectedValue, _
                                       cmbSunday.SelectedValue, txtDayofMonth.Text, IsDaily.Checked)
            'Addd by Shital on 12-jan-2021
            mMailIDsDetail = mMailIDsDetail + " has been Changed to as " + txtEmailIDs.Text + " CC MailIDs: " + txtCC.Text + " BCC MailIDs: " + txtBCC.Text + " Monday: " + cmbMonday.SelectedItem.Text + " Tuesday: " + cmbTuesday.SelectedItem.Text + " Wednesday: " + cmbWednesday.SelectedItem.Text + " Thursday: " + cmbThursday.SelectedItem.Text + " Friday: " + cmbFriday.SelectedItem.Text + " Saturday: " + cmbSaturday.SelectedItem.Text + " Sunday: " + cmbSunday.SelectedItem.Text + " DayofMonth: " + txtDayofMonth.Text + " Is Daily: " + IsDaily.Checked.ToString + " for Report Name :" + cmbReportNameList.SelectedItem.ToString
            MarkLog(Util.Action.Save, "UpdateMailIDsForFAS", mMailIDsDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            Session.Remove("mMailIDsDetail")
            '----

            txtEmailIDs.Text = ""
            txtCC.Text = ""
            txtBCC.Text = ""
            ' txtReportName.Text = ""
            'cmbReportNameList.SelectedIndex = 0
            upnlMailIDs.Update()
            DataFieldBind()
            
            MSGBoxCtrl.show("Updated Successfully", "Updated Successfully", "", MsgBoxStyle.OkOnly, "")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub cmbReportNameList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbReportNameList.SelectedIndexChanged
        If cmbReportNameList.SelectedIndex > 0 Then
            txtEmailIDs.Text = mFAScsReportList1(cmbReportNameList.SelectedItem.ToString).Emails
            txtCC.Text = mFAScsReportList1(cmbReportNameList.SelectedItem.ToString).cc
            txtBCC.Text = mFAScsReportList1(cmbReportNameList.SelectedItem.ToString).Bcc
            cmbMonday.SelectedValue = mFAScsReportList1(cmbReportNameList.SelectedItem.ToString).Monday
            cmbTuesday.SelectedValue = mFAScsReportList1(cmbReportNameList.SelectedItem.ToString).Tuesday
            cmbWednesday.SelectedValue = mFAScsReportList1(cmbReportNameList.SelectedItem.ToString).Wednesday
            cmbThursday.SelectedValue = mFAScsReportList1(cmbReportNameList.SelectedItem.ToString).Thursday
            cmbFriday.SelectedValue = mFAScsReportList1(cmbReportNameList.SelectedItem.ToString).Friday
            cmbSaturday.SelectedValue = mFAScsReportList1(cmbReportNameList.SelectedItem.ToString).Saturday
            cmbSunday.SelectedValue = mFAScsReportList1(cmbReportNameList.SelectedItem.ToString).Sunday
            txtDayofMonth.Text = mFAScsReportList1(cmbReportNameList.SelectedItem.ToString).DayOfMonth
            IsDaily.Checked = mFAScsReportList1(cmbReportNameList.SelectedItem.ToString).IsDaily
        Else
            txtEmailIDs.Text = ""
            txtCC.Text = ""
            txtBCC.Text = ""
        End If

        Session("mID") = cmbReportNameList.SelectedValue
        upnlMailIDs.Update()
    End Sub
#End Region

End Class