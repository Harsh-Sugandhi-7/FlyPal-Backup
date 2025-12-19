Imports System.Linq
Imports System.Text
Public Class wfrptEngineUtilizationReport_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mModelList As ModelList
    Dim ChkModelIDs As String()
    Dim ModelIDs As New StringBuilder
    Dim ChkModelNames As String()
    Dim mModelNames As New StringBuilder
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region "Business Methods"
    Private Sub SetSession()
        Session("mModelList") = mModelList
    End Sub
    Private Sub GetSession()
        mModelList = Session("mModelList")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
    Private Sub AddAttributes()
        btnDisplay.Attributes("onclick") = "javascript: document.body.style.cursor = 'wait';"
    End Sub
#End Region

#Region "Data Binding"
    Private Sub SetCombo()
        If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
            For i As Integer = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If

        For k As Integer = 1 To 12
            Dim mon As String = MonthName(k, False)
            cmbMonth.Items.Add(mon)
        Next
    End Sub
    Private Sub DataFieldBinding()
        mModelList = ModelList.GetModelList(2)
        ListModel.DataSource = mModelList
        Session("mModelList") = mModelList
        ListModel.DataBind()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mModelList")
    End Sub
    Private Sub Display()
        lblSummary.Visible = True
        lblyear1.Visible = True
        lblModel1.Visible = True
    End Sub
    Private Sub SetValues()
        ChkModelIDs = (From c As System.Web.UI.WebControls.ListItem In ListModel.Items
                       Where c.Selected = True
                       Select (c.Value)).ToArray

        ChkModelNames = (From c As System.Web.UI.WebControls.ListItem In ListModel.Items
                      Where c.Selected = True
                      Select (c.Text)).ToArray
        If ChkModelIDs.Length > 0 Then
            ModelIDs.Append("<ModelID>")
            For i As Integer = 0 To ChkModelIDs.Count - 1
                ModelIDs.Append("<id>")
                ModelIDs.Append(ChkModelIDs(i))
                ModelIDs.Append("</id>")


                mModelNames.Append(ChkModelNames(i))
                mModelNames.Append(",")
                mModelNames.Append(" ")
            Next
            ModelIDs.Append("</ModelID>")
        End If


        
        lblyear1.Text = "Month and Year : " & IIf((cmbYear.SelectedIndex >= 0 And cmbMonth.SelectedIndex >= 0), cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text, "")
        '''lblModel1.Text = "Model : " & IIf(cmbModel.SelectedIndex > 0, cmbModel.SelectedItem.Text, "")
        lblModel1.Text = "Model : " & mModelNames.ToString.Trim.TrimEnd(",")

    End Sub
    Private Sub ControlVisibility()
        If chkMonth.Checked = True Then
            cmbMonth.Enabled = True
            cmbYear.Enabled = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
            cmbMonth.Enabled = False
            cmbYear.Enabled = False
        End If
    End Sub
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False)
        Try
            Dim da As New CSLA.Data.ObjectAdapter

            Dim mCompanyDetail As CompanyDetail
            Dim ReportName As String = String.Empty
            Dim ds As New dsEngineUtilization 'dsReliabilityFlyingHoursRecord
            'dsDailyUtilizationGraph   '
            ReportName = "Engine Utilisation Report"
            SetValues()
            Dim searchFromDate As String = ""
            Dim searchToDate As String = ""
            Dim searchMonthYear As String = ""
            Dim mrptEngineUtilizationReport As rptEngineUtilizationReport

            If chkMonth.Checked Then
                mrptEngineUtilizationReport = rptEngineUtilizationReport.GetList(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, IsForMonth:=chkMonth.Checked)
                searchMonthYear = cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text
            Else
                mrptEngineUtilizationReport = rptEngineUtilizationReport.GetList(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, IsForMonth:=chkMonth.Checked, FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text)
                searchFromDate = New SmartDate(txtFromDate.Text).FormattedText
                searchToDate = New SmartDate(txtToDate.Text).FormattedText
            End If

            Dim myReport = New crptEngineUtilization

            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, ReportName, AppSettings("ClientCode"), Trim(txtBottomLine.Text), "", "", "", AppSettings("Product Version"), AppSettings("SINote"), AppSettings("Logo") = "True", "", mModelNames.ToString.Trim.TrimEnd(","), , SearchStr10:=searchMonthYear, SearchStr11:=searchFromDate, SearchStr12:=searchToDate)


            'myReport.SetDataSource(ds)
            If ByMail = False Then
                If mrptEngineUtilizationReport.Count = 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1220)
                End If
            End If
            If (ByMail = True And mrptEngineUtilizationReport.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "", _
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                    ReportGeneratedBy:=Session("ReportGenratedBy"))
                Exit Sub
            End If

            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            ds.Clear()
            da.Fill(ds, mrptImage)
            da.Fill(ds, mrptEngineUtilizationReport)
            da.Fill(ds, Report)

            myReport.SetDataSource(ds)


            Session("CrystalReport") = myReport
            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblyear1.Text + ", " + lblModel1.Text, "", _
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                          ReportGeneratedBy:=Session("ReportGenratedBy"))
            End If
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (SetReport Sub Method): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidate As CustomValidator
        custValidate = CType(s, CustomValidator)
        If custValidate.ControlToValidate = "cmbModel" Then
            'If cmbModel.SelectedIndex <= 0 Then
            '    custValidate.ErrorMessage = "Select the Model"
            '    e.IsValid = False
            'Else
            '    e.IsValid = True
            'End If
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'AddAttributes()
        If Not Page.IsPostBack Then
            SetCombo()
            DataFieldBinding()
            ControlVisibility()
        End If
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetReport(False)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    'Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
    '    If Page.IsValid Then
    '        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    '        'Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
    '        Session("UserEmailID") = mModuleList.Item("EngineUtilisationReport").SendToMailID
    '        Session("UserCcEmailID") = mModuleList.Item("EngineUtilisationReport").SendCCMailID
    '        '--------------------------

    '        Dim Str As String
    '        Str = "OpenByMaiWindow();"
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    '    Else
    '        upnlValidationsummary.Update()
    '    End If
    'End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            Dim email As New Thread(Sub() SetReport(True))
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
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub chkMonth_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMonth.CheckedChanged
        If chkMonth.Checked = True Then
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
            txtFromDate.Text = ""
            txtToDate.Text = ""
            cmbMonth.Enabled = True
            cmbYear.Enabled = True
        Else
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
            cmbMonth.Enabled = False
            cmbYear.Enabled = False
        End If
        upnlMonth.Update()
    End Sub
    Private Sub chkDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        If chkDate.Checked = True Then
            cmbMonth.Enabled = False
            cmbYear.Enabled = False
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        Else
            cmbMonth.Enabled = True
            cmbYear.Enabled = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
            txtFromDate.Text = ""
            txtToDate.Text = ""
        End If
        upnlMonth.Update()
    End Sub
#End Region

End Class