Public Class wfCWPrptPendingWorkShopOrderList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mCWPrptPendingWorkShopOrderList As CWPrptPendingWorkShopOrderList
    Dim mCompanyDetail As New CompanyDetail
    Dim da As New CSLA.Data.ObjectAdapter
    Dim ds As New dsCWPrptPendingWorkShopOrderList
    Public ToDate As String = ""
    Public PartNo As String = ""
    Public Description As String = ""
    Public OrdText As String = ""
    Public OrderNumber As String = ""
    Public OrdNo As Integer = 0
    Public Amend As String = ""
    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
    Dim email As Thread
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Business Properties and Methods "
    Private Sub GetSession()
       PartNo = CType(Session("PartNo"), String)
        Description = CType(Session("Description"), String)
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub SetValues()
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        OrdText = IIf(txtOrderTextList.Text <> "", Trim(txtOrderTextList.Text), "")
        OrdNo = Val(txtOrderNo.Text.Trim)
        Amend = txtAmend.Text.Trim
        ToDate = txtDate.Text.Trim
        If txtOrderTextList.Text = "" And txtOrderNo.Text.Trim = "" And txtAmend.Text.Trim = "" Then
            OrderNumber = ""
        ElseIf Amend.ToString = "" Then
            OrderNumber = OrdText.ToString + "-" + OrdNo.ToString
        Else
            OrderNumber = OrdText.ToString + "-" + OrdNo.ToString & "-" & Amend.ToString
        End If
        PartNo = IIf(PartNo <> "" And Not IsNothing(PartNo), PartNo, "")
        Description = IIf(Description <> "" And Not IsNothing(Description), Description, "")
        Session("PartNo") = PartNo
        Session("Description") = Description
    End Sub
    Public Sub SetReport(Optional ByVal ByMail As Boolean = False)
        SetValues()
        myReport = New crptCWPPendingWorkShopOrderList
        mCWPrptPendingWorkShopOrderList = CWPrptPendingWorkShopOrderList.GetCWPrptPendingWorkShopOrderList(ItemName:=PartNo, ItemDescription:=Description, Text:=OrdText, No:=OrdNo, Amend:=Amend, ToDate:=ToDate)
        If ByMail = False Then
            If mCWPrptPendingWorkShopOrderList.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1332)
            End If
        End If
        If (ByMail = True And mCWPrptPendingWorkShopOrderList.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "Pending WorkShop Order List", "Pending WorkShop Order List", "There is no record for this search criteria.", _
                "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                 SmtpHost:=mModuleList.Item("CWPrptPendingWorkShopOrderList").SmtpHost, SmtpPort:=mModuleList.Item("CWPrptPendingWorkShopOrderList").SmtpPort, _
                SmtpUser:=mModuleList.Item("CWPrptPendingWorkShopOrderList").SmtpUser, SmtpPassword:=mModuleList.Item("CWPrptPendingWorkShopOrderList").SmtpPassword)
            Exit Sub
        End If
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, "Pending WorkShop Order List", SearchStr1:=New SmartDate(txtDate.Text).FormattedText, SearchStr2:=PartNo, SearchStr3:=Description, SearchStr4:=OrderNumber, SearchStr5:="", ProductVersion:=("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"))
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mCWPrptPendingWorkShopOrderList)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        MarkLog(Util.Action.Print, "CWPrptPendingWorkShopOrderList", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        If ByMail = False Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        Else
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Pending WorkShop Order List", "Pending WorkShop Order List", " For " + New SmartDate(txtDate.Text).FormattedText, _
                                      "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                       SmtpHost:=mModuleList.Item("CWPrptPendingWorkShopOrderList").SmtpHost, SmtpPort:=mModuleList.Item("CWPrptPendingWorkShopOrderList").SmtpPort, _
                                      SmtpUser:=mModuleList.Item("CWPrptPendingWorkShopOrderList").SmtpUser, SmtpPassword:=mModuleList.Item("CWPrptPendingWorkShopOrderList").SmtpPassword)
        End If
    End Sub
    Private Sub addAttributes()
        txtOrderNo.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtOrderNo').value,event)")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok

            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
   
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtDate.Text = New SmartDate(Today.Date).FormattedText
            txtDate.DataBind()
         End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
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
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click

        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        '  Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
        Session("UserEmailID") = mModuleList.Item("CWPrptPendingWorkShopOrderList").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("CWPrptPendingWorkShopOrderList").SendCCMailID
        '--------------------------

        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class