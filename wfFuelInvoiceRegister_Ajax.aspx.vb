Public Class wfFuelInvoiceRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mVendorList As VendorList
    Public mOrderTextList As DistinctTextListForOrder
    Public FromDate As String = ""
    Public ToDate As String = ""
    Public Supplier As String = ""
    Public OrdText As String = ""
    Public OrdNo As String = ""
    Public IntOrderNo As String = ""
    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim email As Thread
    Dim EventLogID As Guid

    'Added By Abhishek on 10-OCT-2017
    Dim objReg As FuelInvoiceRegister
    Dim da As New CSLA.Data.ObjectAdapter
    Dim dsFuelInvoiceRegister As New dsFuelInvoiceRegister
    Dim mCompanyDetail As New CompanyDetail
    Dim mModuleList As ModuleList    'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Business Properties and Methods "
    Private Sub GetSession()
        mVendorList = CType(Session("mVendorlist"), VendorList)
        mModuleList = Session("mModuleList")    'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mVendorlist") = mVendorList
     End Sub
    Private Sub RemoveSession()
        Session.Remove("mVendorlist")
       End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblVendor.Visible = True
        lblFuelInvoiceNumber.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblVendor.Visible = False
        lblFuelInvoiceNumber.Visible = False
    End Sub
    Private Sub SetValues()
        FromDate = txtFromDate.Text.ToString
        ToDate = txtToDate.Text.ToString
        lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText
        Supplier = txtSupplier.Text.Trim
        lblVendor.Text = "Supplier :  " & Supplier

        OrdText = IIf(txtFuelInvoicTextList.Text <> "", Trim(txtFuelInvoicTextList.Text), "")
        OrdNo = txtFuelInvoicNo.Text.Trim
        lblFuelInvoiceNumber.Text = "Fuel Invoice No.: " & IIf(OrdText + OrdNo <> "", OrdText + "-" + OrdNo, "All")
        mCompleteSearchingCriteria = lblDateRangeFrom.Text + ", " + lblVendor.Text + ", " + lblFuelInvoiceNumber.Text
    End Sub
    Public Sub SetReport(Optional ByVal ByMail As Boolean = False)
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objReg As FuelInvoiceRegister
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsFuelInvoiceRegister As New dsFuelInvoiceRegister
        Dim mCompanyDetail As New CompanyDetail
        SetValues()

        myReport = New crptFuelInvoiceRegister
        objReg = FuelInvoiceRegister.GetFuelInvoiceRegister(FromDate:=FromDate, ToDate:=ToDate, VendorName:=Supplier, FuelInvoiceText:=OrdText, FuelInvoiceNo:=OrdNo)

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
           mCompanyDetail.WebSite, "Fuel Invoice Register", SearchStr1:=FromDate, SearchStr2:=ToDate, SearchStr3:=Supplier, SearchStr4:=IIf(OrdText + OrdNo <> "", OrdText + "-" + OrdNo, ""), SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"), SearchStr11:="")
        If ByMail = False Then
            If objReg.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1338)
            End If
        End If
        If (ByMail = True And objReg.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "Fuel Invoice Register", "Fuel Invoice Register", "There is no record for this search criteria.", _
                "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                  SmtpHost:=mModuleList.Item("FuelInvoiceRegister").SmtpHost, SmtpPort:=mModuleList.Item("FuelInvoiceRegister").SmtpPort, SmtpUser:=mModuleList.Item("FuelInvoiceRegister").SmtpUser, SmtpPassword:=mModuleList.Item("FuelInvoiceRegister").SmtpPassword)
            Exit Sub
        End If
        dsFuelInvoiceRegister.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(dsFuelInvoiceRegister)
            da.Fill(dsFuelInvoiceRegister, mrptImage)
        da.Fill(dsFuelInvoiceRegister, objReg)
        da.Fill(dsFuelInvoiceRegister, Report)
        myReport.SetDataSource(dsFuelInvoiceRegister)
        Session("CrystalReport") = myReport

        MarkLog(Util.Action.Print, "FuelInvoiceRegister", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        If ByMail = False Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        Else
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Fuel Invoice Register", "Fuel Invoice Register", " For " + lblDateRangeFrom.Text, _
                                      "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                      ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                        SmtpHost:=mModuleList.Item("FuelInvoiceRegister").SmtpHost, SmtpPort:=mModuleList.Item("FuelInvoiceRegister").SmtpPort, SmtpUser:=mModuleList.Item("FuelInvoiceRegister").SmtpUser, SmtpPassword:=mModuleList.Item("FuelInvoiceRegister").SmtpPassword)
        End If

    End Sub
    Private Sub addAttributes()
        txtFuelInvoicNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtFuelInvoicNo').value,event)")
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
    Private Sub SetDatePeroid(ByVal Index As Int32)
        txtFromDate.Text = Today.Date
        txtToDate.Text = Today.Date
        txtFromDate.Text = Format(CDate(txtFromDate.Text), AppSettings("DateFormat"))
        txtToDate.Text = Format(CDate(txtToDate.Text), AppSettings("DateFormat"))
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            RemoveSession()
            DataFieldBind()
            SetDatePeroid(6)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid Then
            ControlVisibility2()
            SetValues()
            upnlDisplaySearchCriteria.Update()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetReport(ByMail:=False)
        Else
            upnlValidations.Update()
        End If
    End Sub
   Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click

        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        ' Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
        Session("UserEmailID") = mModuleList.Item("FuelInvoiceRegister").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("FuelInvoiceRegister").SendCCMailID
        '--------------------------

        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            email = New Thread(Sub() SetReport(ByMail:=True))
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
#End Region
    'Added By Abhishek on 10-OCT-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()
            GenerateXLSXFile(CreateDataTable())
        End If
    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("Fuel Invoice Register")
        Dim conString As String = AppSettings("DB:FlyPal")
        Dim con = New SqlConnection(conString)
        con.Open()
        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ExcelFuelInvoiceRegisterFetch"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@FromDate", FromDate)
        cmd.Parameters.AddWithValue("@ToDate", ToDate)
        cmd.Parameters.AddWithValue("@VendorName", Supplier)
        cmd.Parameters.AddWithValue("@InvoiceText", OrdText)
        cmd.Parameters.AddWithValue("@InvoiceNo", OrdNo)

        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()


        'dataTable.Columns.Remove("Rem1")
        'dataTable.Columns.Remove("Rem2")
        'dataTable.Columns.Remove("Rem3")
        Return dataTable
    End Function
    Private Sub GenerateXLSXFile(ByVal tbl As DataTable)   
        objReg = FuelInvoiceRegister.GetFuelInvoiceRegister(FromDate:=FromDate, ToDate:=ToDate, VendorName:=Supplier, FuelInvoiceText:=OrdText, FuelInvoiceNo:=OrdNo)
       
        da.Fill(dsFuelInvoiceRegister, objReg)
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
         mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
         mCompanyDetail.WebSite, "Fuel Invoice Register", SearchStr1:=FromDate, SearchStr2:=ToDate, SearchStr3:=Supplier, SearchStr4:=IIf(OrdText + OrdNo <> "", OrdText + "-" + OrdNo, ""), SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"), SearchStr11:="")
        da.Fill(dsFuelInvoiceRegister, "ReportData", Report)
        Dim columnToRemove As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ReportName", "SearchStr5", "ProductVersion", "SINote", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr14", "SearchStr13", "SearchStr12", "SearchStr11", "CurrencyName", "CurrencySymbol", "ShortName"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If dsFuelInvoiceRegister.Tables("ReportData").Columns.Contains(columnToRemove(i)) Then
                dsFuelInvoiceRegister.Tables("ReportData").Columns.Remove(columnToRemove(i))
            End If
        Next
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
   
        Dim dsNew As New DataSet
        dsNew.Clear()
        dsNew.Merge(dsFuelInvoiceRegister.Tables("ReportData"))
        dsNew.Merge(tbl)

        Session("dsNew") = dsNew
        If dsNew.Tables("ReportData").Columns.Contains("SearchStr1") Then
            dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
        End If
        If dsNew.Tables("ReportData").Columns.Contains("SearchStr2") Then
            dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
        End If
        If dsNew.Tables("ReportData").Columns.Contains("SearchStr3") Then
            dsNew.Tables("ReportData").Columns("SearchStr3").ColumnName = "Supplier"
        End If
        If dsNew.Tables("ReportData").Columns.Contains("SearchStr4") Then
            dsNew.Tables("ReportData").Columns("SearchStr4").ColumnName = "Fuel Invoice No."
        End If
		dsNew.Tables("ReportData").TableName = "Searching Criteria"
		Session("ExcelFileName") = "Fuel Invoice Register"
		'Session("DataTable") = tbl
		'Session("ReportName") = "RCI Register"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "FuelInvoiceRegister", "Export To excel " + mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
End Class