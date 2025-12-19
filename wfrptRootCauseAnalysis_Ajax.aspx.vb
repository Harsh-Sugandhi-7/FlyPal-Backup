Imports System.Collections.Generic
Imports System.Linq
Imports Flypal.RootCauseAnalysis

Public Class wfrptRootCauseAnalysis_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mAuditExecutionAuditNoList As AuditExecutionAuditNoList
    Dim FromDate, ToDate, mSearchingCriteria As String
    Private mRootCauseList As RootCauseList
    Private mSubject As String
    Dim email As Thread
    Public mResponsibleDepartmentList As EmployeeDepartmentList
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mAuditExecutionAuditNoList = Session("mAuditExecutionAuditNoList")
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mAuditExecutionAuditNoList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub SetReport(Optional ByMail As Boolean = False)
        GetSession()

        Dim mRootCauseAnalysis As RootCauseAnalysis
        mRootCauseAnalysis = RootCauseAnalysis.GetRootCauseAnalysisList(txtFromDate.Text, txtToDate.Text, _
                                                                        IIf(cmbAuditInfoList.SelectedIndex > 0, cmbAuditInfoList.SelectedItem.Text, ""), _
                                                                        cmbRootCause.SelectedValue.ToString, , cmbDepartment.selectedValue.Tostring)
        If mRootCauseAnalysis.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsRootCauseAnalysis As New dsRootCauseAnalysis
        Dim templist As New System.Collections.ArrayList

        If rbGraph.Checked Then
            Dim tmpRootCauseAnalysis As Object
            Dim variable As Object
            tmpRootCauseAnalysis = (From c In mRootCauseAnalysis
                              Group By mRootcauseID = c.RootcauseID, mMasterRootCause = c.MasterRootCause, mDepartmentName = c.DepartmentName, mRootcause = c.Rootcause Into Group
                              Order By mMasterRootCause Ascending
                              Select New With {.RootcauseID = mRootcauseID, .MasterRootCause = mMasterRootCause, .DepartmentName = mDepartmentName, .Rootcause = mRootcause, .RootCauseAnalysisCollection = Group})
            Dim Info As New RootCauseAnalysisInfo

            For Each variable In tmpRootCauseAnalysis
                Info = New RootCauseAnalysisInfo
                Info.Rootcause = variable.Rootcause
                Info.RootcauseID = New Guid(variable.RootcauseID.ToString)
                Info.MasterRootCause = variable.MasterRootCause
                Info.RootcauseCount = variable.RootCauseAnalysisCollection.length
                Info.DepartmentName = variable.DepartmentName
                templist.Add(Info)
            Next
            myReport = New crptRootCauseAnalysisGraph
            mSubject = "Root Cause Analysis Graph Report"
        Else
            myReport = New crptRootCauseAnalysis
            mSubject = "Root Cause Analysis Report"
        End If

        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String
        Dim SearchStr4 As String
        Dim SearchStr5 As String

        SearchStr1 = New SmartDate(txtFromDate.Text).FormattedText

        ToDate = txtToDate.Text
        SearchStr2 = New SmartDate(ToDate).FormattedText

        If cmbAuditInfoList.SelectedIndex > 0 Then
            SearchStr3 = cmbAuditInfoList.SelectedItem.Text
        Else
            SearchStr3 = ""
        End If

        If cmbRootCause.SelectedIndex > 0 Then
            SearchStr4 = cmbRootCause.SelectedItem.Text
        Else
            SearchStr4 = ""
        End If

        If cmbDepartment.SelectedIndex > 0 Then
            SearchStr5 = cmbDepartment.SelectedItem.Text
        Else
            SearchStr5 = ""
        End If


        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                                     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                                     mCompanyDetail.WebSite, "Audit Findings Report", SearchStr1:=SearchStr1, SearchStr2:=SearchStr2, SearchStr3:=SearchStr3, _
                                     SearchStr4:=SearchStr4, SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), _
                                     SearchStr6:=SearchStr5, SearchStr7:="", SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(dsRootCauseAnalysis)
        da.Fill(dsRootCauseAnalysis, mRootCauseAnalysis)
        da.Fill(dsRootCauseAnalysis, Report)
        da.Fill(dsRootCauseAnalysis, mrptImage)
        If rbGraph.Checked Then
            da.Fill(dsRootCauseAnalysis, "RootCauseAnalysisForGraph", templist)
        End If
        myReport.SetDataSource(dsRootCauseAnalysis)
        Session("CrystalReport") = myReport
        MarkLog(Util.Action.Print, "RootCauseAnalysis", mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        If ByMail = True Then
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, mSubject, mSubject, _
                                      Info:="Here is the attached " + mSubject, VendorEmailID:="", ToMailID:=Session("ToSendMailIDs"), _
                                      CCMailID:=Session("CcSendMailIDs"), ReportPath:="", ReportByMail:=True, FromAudit:=1, Remark:=Session("SendMailRemark"), _
                                      ReportGeneratedBy:=Session("ReportGenratedBy"))
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If
    End Sub
    Public Sub setValues()
        Dim mAuditNo As String = ""
        Dim mRootCause As String = ""
        Dim mFindingStatus As String = ""

        ToDate = txtToDate.Text
        lblDateRange.Text = "From Date : " & New SmartDate(txtFromDate.Text).FormattedText + " To Date : " & New SmartDate(txtToDate.Text).FormattedText

        If cmbAuditInfoList.SelectedIndex > 0 Then
            mAuditNo = cmbAuditInfoList.SelectedItem.Text
            lblAudit.Text = "Audit No : " & mAuditNo
        Else
            lblAudit.Text = "Audit No : All"
        End If

        If cmbRootCause.SelectedIndex > 0 Then
            mRootCause = cmbRootCause.SelectedItem.Text
            lblRootCause.Text = "Root cause : " & mRootCause
        Else
            mAuditNo = ""
            lblRootCause.Text = "Root cause : All"
        End If
        mSearchingCriteria = lblDateRange.Text + " " + lblAudit.Text + " " + lblRootCause.Text
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        mAuditExecutionAuditNoList = AuditExecutionAuditNoList.GetAuditExecutionAuditNoList("(ALL)")
        cmbAuditInfoList.DataSource = mAuditExecutionAuditNoList
        Session("mAuditExecutionAuditNoList") = mAuditExecutionAuditNoList

        mRootCauseList = RootCauseList.GetRootCauseList("(ALL)")
        cmbRootCause.DataSource = mRootCauseList

        mResponsibleDepartmentList = EmployeeDepartmentList.GetEmployeeDepartmentList("(ALL)") ' Added by Saylee on 17-Jan-2020
        cmbDepartment.DataSource = mResponsibleDepartmentList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
            'txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            setFocus(cmbAuditInfoList)
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        setValues()
        SetReport()
    End Sub
    Private Sub btnSendMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendMail.Click
        Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            email = New Thread(Sub() SetReport(True))
            email.IsBackground = True
            email.Start()
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSessions()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblAudit.Visible = True
        lblRootCause.Visible = True
        lblDateRange.Visible = True
        setValues()
        upnlSelection.Update()
    End Sub
#End Region

End Class