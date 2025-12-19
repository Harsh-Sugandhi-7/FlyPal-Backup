
Imports System.Linq
Imports System.Collections.Generic
Imports System.Text

Public Class wfSearchCriteriaForMELDueReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mMachineNameValueList As MachineNameValueList 'Changed By Utkarsh On 19-Apr-2011
    Public mMELCategoryList As MELCategoryList
    Public mATAList As ATAList
    Dim StartDate As String
    Dim EndDate As String
    Dim MachineID, ATAID As String
    Dim Aircraft, ATAChapter, MELCategory As String
    Dim mMELDueReportSearchingCriteria As String = String.Empty
    Public mMELSnagCorrectiveActionListForDue As MELSnagCorrectiveActionListForDue
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMELSnagCorrectiveActionListForDue = CType(Session("mMELSnagCorrectiveActionListForDue"), MELSnagCorrectiveActionListForDue)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList) 'Changed By Utkarsh On 19-Apr-2011)
        mATAList = CType(Session("mATAList"), ATAList)
        mMELCategoryList = Session("mMELCategoryList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mMELSnagCorrectiveActionListForDue") = mMELSnagCorrectiveActionListForDue
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mATAList") = mATAList
        Session("mMELCategoryList") = mMELCategoryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMELSnagCorrectiveActionListForDue")
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
#End Region

#Region " Helper Methods "
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDateRangeFrom.Visible = True
        lblATAChapter1.Visible = True
        lblMElCategory1.Visible = True
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtAsOnDate.Text) Then
            StartDate = ""
        Else
            StartDate = txtAsOnDate.Text
        End If
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        ATAChapter = IIf(cmbATAChapter.SelectedIndex > 0, cmbATAChapter.SelectedItem.Text, "")
        MELCategory = IIf(cmbMELCategory.SelectedIndex > 0, cmbMELCategory.SelectedItem.Text, "")
        lblDateRangeFrom.Text = "As On Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        lblAircraft1.Text = "Aircraft : " & Aircraft
        lblATAChapter1.Text = "ATA Chapter : " & ATAChapter
        lblMElCategory1.Text = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Category : ", "MEL Category : ") & MELCategory
        mMELDueReportSearchingCriteria = lblDateRangeFrom.Text.Trim + ", " + lblAircraft1.Text.Trim + ", " + lblATAChapter1.Text.Trim + ", " + lblMElCategory1.Text
    End Sub
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False) 'Parameter Added by Shital on 6-Sep-2016
        SetValues()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsMELSnagCorrectiveActionForDue
        Dim mCompanyDetail As New CompanyDetail
        Dim IsMajorMinor As Integer
        Dim MajorMinor As String
        'Added By Shweta On 30-April-2013 For ALL29042013-3
        Dim IsPirepsDefectType As Integer
        Dim PirepsDefectType As String
        ''

        If rbAll.Checked = True Then
            IsMajorMinor = 0  'ALL MAJOR AND MINOR
            MajorMinor = 0
        ElseIf rbMajor.Checked = True Then
            IsMajorMinor = 1  'MAJOR
            MajorMinor = 1    'To Show on report MAJOR/MINOR/ALL
        Else
            IsMajorMinor = 2  'MINOR
            MajorMinor = 2
        End If
        'Added By Shweta On 30-April-2013 For ALL29042013-3
        If rbAllDefectType.Checked = True Then
            IsPirepsDefectType = 0  'ALL Pireps And Defect Type
            PirepsDefectType = 0
        ElseIf rbIsPireps.Checked = True Then
            IsPirepsDefectType = 1  'Pireps
            PirepsDefectType = 1    'To Show on report Pireps/Defect Type/ALL
        Else
            IsPirepsDefectType = 2  'DEFECT type
            PirepsDefectType = 2
        End If
        ''

        myReport = New crptMELSnagCorrectiveActionForDue
        If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then
            mMELSnagCorrectiveActionListForDue = MELSnagCorrectiveActionListForDue.GetMELSnagCorrectiveActionListForDue(txtAsOnDate.Text, New Guid(cmbAircraft.SelectedValue.ToString), New Guid(cmbATAChapter.SelectedValue.ToString), cmbMELCategory.SelectedValue, IsMajorMinor, "HH:mm", IsPirepsDefectType, SkipIsForInventoryAircraft:=True)
        Else
            mMELSnagCorrectiveActionListForDue = MELSnagCorrectiveActionListForDue.GetMELSnagCorrectiveActionListForDue(txtAsOnDate.Text, New Guid(cmbAircraft.SelectedValue.ToString), New Guid(cmbATAChapter.SelectedValue.ToString), cmbMELCategory.SelectedValue, IsMajorMinor, , IsPirepsDefectType, SkipIsForInventoryAircraft:=True)
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Due Report", "MEL Due Report"), New SmartDate(StartDate).FormattedText, Aircraft, ATAChapter, MELCategory, MajorMinor, AppSettings("Product Version"), AppSettings("SINote"), "", "", AppSettings("MELSnagNomenclature").ToString, PirepsDefectType, AppSettings("Logo"))

        'If case Added By Shital On 6-Sep-2016
        If ByMail = False Then
            If mMELSnagCorrectiveActionListForDue.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
        If (ByMail = True And mMELSnagCorrectiveActionListForDue.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Due Report", "MEL Due Report"), IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Due Report", "MEL Due Report"), "There is no record for this search criteria.", "", _
                Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                      SmtpHost:=mModuleList.Item("MELDueReport").SmtpHost, SmtpPort:=mModuleList.Item("MELDueReport").SmtpPort, _
                                      SmtpUser:=mModuleList.Item("MELDueReport").SmtpUser, SmtpPassword:=mModuleList.Item("MELDueReport").SmtpPassword)
            Exit Sub
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mMELSnagCorrectiveActionListForDue)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        If (ByMail = True) Then
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Due Report", "MEL Due Report"), IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Due Report", "MEL Due Report"), _
                                      " For " + lblDateRangeFrom.Text + ", " + lblAircraft1.Text, , Session("ToSendMailIDs"), Session("CcSendMailIDs"), _
                                      "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                      SmtpHost:=mModuleList.Item("MELDueReport").SmtpHost, SmtpPort:=mModuleList.Item("MELDueReport").SmtpPort, _
                                      SmtpUser:=mModuleList.Item("MELDueReport").SmtpUser, SmtpPassword:=mModuleList.Item("MELDueReport").SmtpPassword)
        Else
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "MELDueReport", mMELDueReportSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If

    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList("", , , , , , , True, "(All)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList

        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList

        mMELCategoryList = MELCategoryList.GetMELCategoryList("(All)")
        cmbMELCategory.DataSource = mMELCategoryList
        Session("mMELCategoryList") = mMELCategoryList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat").ToString)
            rbAllDefectType.Checked = True
            rbAll.Checked = True
            DataFieldBind()
            If cmbAircraft.Enabled = True Then
                setFocus(cmbAircraft)
            End If
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlselection1.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport(False)  '6-Sep-2016
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    'Added by Shital on 6-Sep-2016
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        '   Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

        Session("UserEmailID") = mModuleList.Item("MELDueReport").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("MELDueReport").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgMELBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgMELBtnSendMail.Click
        Dim email As Thread
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
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgMELBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
#End Region
End Class