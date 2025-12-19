'Ajax Conversion By Vikrant On 30-Jan-2014

Public Class wfSearchCriteriaForCompany_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mCompanyList As CompanyList
    Dim StartDate As String
    Dim EndDate As String
    Dim Company As String

    Dim da As New CSLA.Data.ObjectAdapter
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mCompanyDetail As New CompanyDetail
    Dim objFlyingReg As New ReportCompanyWiseFlyingRegister
    Dim dsFlyingReg As New dsFlyingRegister
    Dim EventLogDetail As String = String.Empty
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCompanyList = CType(Session("mCompanyList"), CompanyList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForCompany_Ajax.aspx?" Then
            Session.Remove("mCompanyList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblCompany1.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtFromDate.Text) Then
            StartDate = ""
        Else
            StartDate = txtFromDate.Text
        End If
        If Not IsDate(txtToDate.Text) Then
            EndDate = ""
        Else
            EndDate = txtToDate.Text
        End If
        Company = IIf(cmbCompany.SelectedIndex > 0, cmbCompany.SelectedItem.Text, "")
        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
        lblCompany1.Text = "Company : " & IIf(Company <> "", Company, "All")
        EventLogDetail = lblDateRangeFrom.Text + "," + lblDateRangeTo.Text + "," + lblCompany1.Text
    End Sub
    Private Sub SetReport()
        SetValues()

        Dim str1 As String = ""

        If chkLogNo.Checked Then
            str1 = "Log No."
        Else
            str1 = ""
        End If
        If chkLogPageNo.Checked = False Then
            '
        ElseIf str1 = "" Then
            str1 = "Log Page No."
        Else
            str1 = str1 + "/" + "Log Page No."
        End If

        If chkFlightNo.Checked = False Then
            '
        ElseIf str1 = "" Then
            str1 = "Flight No."
        Else
            str1 = str1 + "/" + "Flight No."
        End If
        myReport = New crFlyingRegister
        objFlyingReg = ReportCompanyWiseFlyingRegister.GetCompanyPaxRegisterList(StartDate, EndDate, cmbCompany.SelectedValue.ToString, chkLogNo.Checked, chkLogPageNo.Checked, chkFlightNo.Checked)
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Company wise Flying Register", New SmartDate(StartDate).FormattedText, New SmartDate(EndDate).FormattedText, Company, str1, "", AppSettings("Product Version"), AppSettings("SINote"), AppSettings("Logo"))
        'mCompanyDetail.WebSite, "Company wise Flying Register", New SmartDate(StartDate).FormattedText, New SmartDate(EndDate).FormattedText, Company, str1, "", AppSettings("Product Version"), AppSettings("SINote"))

        If objFlyingReg.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
            'Added By Utkarsh On 7-Jun-2011 For All07062011
        ElseIf objFlyingReg.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 625)
            '*******************************
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(dsFlyingReg) 'Added by Shweta on 20-Feb-2012
        da.Fill(dsFlyingReg, objFlyingReg)
        da.Fill(dsFlyingReg, Report)
        da.Fill(dsFlyingReg, mrptImage) 'Added by Shweta on 20-Feb-2012
        myReport.SetDataSource(dsFlyingReg)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "CompanyWiseFlying", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    'Response.Redirect("wfSearchCriteriaForCompany_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            'Response.Redirect("wfSearchCriteriaForCompany_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCompanyList = CompanyList.GetCompanyList(, True)
        cmbCompany.DataSource = mCompanyList
        Session("mCompanyList") = mCompanyList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfSearchCriteriaForCompany_Ajax.aspx?"
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            ''SetFocus(txtFromDate)
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class