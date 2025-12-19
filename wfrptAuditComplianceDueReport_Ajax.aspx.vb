Public Class wfrptAuditComplianceDueReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mAuditExecutionAuditNoList As AuditExecutionAuditNoList
    Private mEmployeeDepartmentList As EmployeeDepartmentList
    Private mFindingStatusList As FindingStatusList
    Dim DateIndex, FromDate, ToDate, mSearchingCriteria As String
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mAuditExecutionAuditNoList = Session("mAuditExecutionAuditNoList")
        mEmployeeDepartmentList = Session("mEmployeeDepartmentList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mAuditExecutionAuditNoList")
        Session.Remove("mEmployeeDepartmentList")
     End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub SetReport(Optional ByMail As Boolean = False)
        GetSession()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mrptAuditComplianceDueList As rptAuditComplianceDue
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsrptAuditComplianceDue As New dsrptAuditComplianceDue

        myReport = New crptAuditComplianceDueList

        Dim SearchStr2 As String
        Dim SearchStr3 As String
        Dim SearchStr4 As String

        ToDate = txtDate.Text
        SearchStr2 = New SmartDate(ToDate).FormattedText

        If cmbAuditInfoList.SelectedIndex > 0 Then
            SearchStr3 = cmbAuditInfoList.SelectedItem.Text
        Else
            SearchStr3 = ""
        End If

        If cmbDepartmentList.SelectedIndex > 0 Then
            SearchStr4 = cmbDepartmentList.SelectedItem.Text
        Else
            SearchStr4 = ""
        End If


        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "Audit Findings Report", SearchStr1:="", SearchStr2:=SearchStr2, SearchStr3:=SearchStr3, SearchStr4:=SearchStr4, SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"))


        mrptAuditComplianceDueList = rptAuditComplianceDue.GetrptAuditComplianceDueList("1/1/1900", txtDate.Text, SearchStr3, cmbDepartmentList.SelectedValue.ToString)

        If mrptAuditComplianceDueList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(dsrptAuditComplianceDue)
        '----------------------------------------------------------
        da.Fill(dsrptAuditComplianceDue, mrptAuditComplianceDueList)
        da.Fill(dsrptAuditComplianceDue, Report)
        da.Fill(dsrptAuditComplianceDue, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(dsrptAuditComplianceDue)
        Session("CrystalReport") = myReport


        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 

        Session("UserEmailID") = mModuleList.Item("AuditComplianceDueReport").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("AuditComplianceDueReport").SendCCMailID
        '--------------------------

        If ByMail = True Then
            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
            ' SendMailFile.SendMailFile(Session("CrystalReport"), User.Identity.Name, "Audit Compliance Due Report", "AuditDue", Info:="Here is the attached Audit Compliance Due Report for " + "As On Date : " + SearchStr2, VendorEmailID:="", ToMailID:=AppSettings("SendToMailID"), FromAudit:=1, Remark:=Session("SendMailRemark"), ReportGenratedBy:=Session("ReportGenratedBy"))
            SendMailFile.SendMailFile(Session("CrystalReport"), User.Identity.Name, "Audit Compliance Due Report", "AuditDue", Info:="Here is the attached Audit Compliance Due Report for " + "As On Date : " + SearchStr2, VendorEmailID:="", ToMailID:=Session("UserEmailID"), FromAudit:=1, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                     SmtpHost:=mModuleList.Item("AuditComplianceDueReport").SmtpHost, SmtpPort:=mModuleList.Item("AuditComplianceDueReport").SmtpPort, SmtpUser:=mModuleList.Item("AuditComplianceDueReport").SmtpUser, SmtpPassword:=mModuleList.Item("AuditComplianceDueReport").SmtpPassword)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If


        MarkLog(Util.Action.Print, "CalibrationDueReport", mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Public Sub setValues()
        Dim mAuditNo As String = ""
        Dim mDepartment As String = ""
        Dim mFindingStatus As String = ""

        ToDate = txtDate.Text
        lblDateRange.Text = "As On Date : " & New SmartDate(ToDate).FormattedText

        If cmbAuditInfoList.SelectedIndex > 0 Then
            mAuditNo = cmbAuditInfoList.SelectedItem.Text
            lblAudit.Text = "Audit No : " & mAuditNo
        Else
            lblAudit.Text = "Audit No : All"
        End If

        If cmbDepartmentList.SelectedIndex > 0 Then
            mDepartment = cmbDepartmentList.SelectedItem.Text
            lblDeparmentName.Text = "Department : " & mDepartment
        Else
            mAuditNo = ""
            lblDeparmentName.Text = "Department : All"
        End If
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        mAuditExecutionAuditNoList = AuditExecutionAuditNoList.GetAuditExecutionAuditNoList("(ALL)")
        cmbAuditInfoList.DataSource = mAuditExecutionAuditNoList
        Session("mAuditExecutionAuditNoList") = mAuditExecutionAuditNoList

        mEmployeeDepartmentList = EmployeeDepartmentList.GetEmployeeDepartmentList("(ALL)")
        cmbDepartmentList.DataSource = mEmployeeDepartmentList
        Session("mEmployeeDepartmentList") = mEmployeeDepartmentList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
            txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            SetFocus(cmbAuditInfoList)
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            setValues()
            SetReport()
        End If
    End Sub
    Private Sub btnSendMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendMail.Click
        If IsValid() Then
            setValues()
            SetReport(True)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSessions()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblAudit.Visible = True
        lblDeparmentName.Visible = True
        lblDateRange.Visible = True
        setValues()
        upnlSelection.Update()
    End Sub
#End Region

End Class