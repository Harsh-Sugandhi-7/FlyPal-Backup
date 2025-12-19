Public Class wfrptVendorsValidityRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mVendorsValidityRegisterCriteria As String = String.Empty
    Public mVendorList As VendorList
    Public ToDate As String
    Dim EventLogID As Guid
    Dim NatureOfVendor As String = ""
    Dim email As Thread

    'Added by Abhishek on 10-OCT-2017
    Dim mCompanyDetail As New CompanyDetail
    Dim da As New CSLA.Data.ObjectAdapter
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim ds As New dsVendorsValidityRegister
    Dim mVendorsValidityRegister As VendorsValidityRegister
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 


#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mVendorList = CType(Session("mVendorList"), VendorList)
        'mVendorTypeList = CType(Session("mVendorTypeList"), VendorTypeList)
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mVendorList")
        'Session.Remove("mVendorTypeList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Controlvisibility(ByVal Index As Int16)
        lblVendorName.Visible = False
        lblDateRange.Visible = False
        lblVendorType.Visible = False
    End Sub
    Private Sub SetValues()
        ToDate = txtDate.Text.ToString
        lblDateRange.Text = "As On Date : " & New SmartDate(txtDate.Text.ToString).FormattedText

        lblVendorName.Text = "Vendor : " + IIf(cmbVendor.SelectedIndex > 0, cmbVendor.SelectedItem.Text, "ALL")
        NatureOfVendor = txtNatureOfVendor.Text.Trim
        lblVendorType.Text = "Nature Of Vendor : " + txtNatureOfVendor.Text.Trim
        'If chkSupplier.Checked = True And chkCustomer.Checked = True And chkServiceProvider.Checked = True Then
        '    NatureOfVendor = "Supplier, Customer, Service Provider"
        'ElseIf chkSupplier.Checked = True And chkCustomer.Checked = True And chkServiceProvider.Checked = False Then
        '    NatureOfVendor = "Supplier, Customer"
        'ElseIf chkSupplier.Checked = True And chkCustomer.Checked = False And chkServiceProvider.Checked = True Then
        '    NatureOfVendor = "Supplier, Service Provider"
        'ElseIf chkSupplier.Checked = True And chkCustomer.Checked = False And chkServiceProvider.Checked = False Then
        '    NatureOfVendor = "Supplier"
        'ElseIf chkSupplier.Checked = False And chkCustomer.Checked = True And chkServiceProvider.Checked = True Then
        '    NatureOfVendor = "Customer, Service Provider"
        'ElseIf chkSupplier.Checked = False And chkCustomer.Checked = True And chkServiceProvider.Checked = False Then
        '    NatureOfVendor = "Customer"
        'ElseIf chkSupplier.Checked = False And chkCustomer.Checked = False And chkServiceProvider.Checked = True Then
        '    NatureOfVendor = "Service Provider"
        'ElseIf chkSupplier.Checked = False And chkCustomer.Checked = False And chkServiceProvider.Checked = False Then
        '    NatureOfVendor = ""
        'End If
        mVendorsValidityRegisterCriteria = lblDateRange.Text.Trim + ", " + lblVendorName.Text.Trim + ", " + lblVendorType.Text.Trim + ", " + NatureOfVendor
    End Sub
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False)
        SetValues()
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsVendorsValidityRegister
        Dim mVendorsValidityRegister As VendorsValidityRegister

        myReport = New crptVendorsValidityRegister
        mVendorsValidityRegister = VendorsValidityRegister.GetVendorsValidityRegister(ToDate, cmbVendor.SelectedValue.ToString, 0, False, False, False, CInt(cmbRange.SelectedValue), txtNatureOfVendor.Text.Trim)

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
               mCompanyDetail.WebSite, "Vendors Validity Register", New SmartDate(ToDate).FormattedText,
               cmbVendor.SelectedItem.Text, NatureOfVendor, IIf(cmbRange.SelectedIndex = 0, "", cmbRange.SelectedItem.Text), SearchStr5:=AppSettings("ClientCode"), AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        If ByMail = False Then
            If mVendorsValidityRegister.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1364)
            End If
        End If
        If (ByMail = True And mVendorsValidityRegister.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "Vendors Validity Register", "Vendors Validity Register", "There is no record for this search criteria.", _
                "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                  SmtpHost:=mModuleList.Item("VendorsValidityRegister").SmtpHost, SmtpPort:=mModuleList.Item("VendorsValidityRegister").SmtpPort, _
                    SmtpUser:=mModuleList.Item("VendorsValidityRegister").SmtpUser, SmtpPassword:=mModuleList.Item("VendorsValidityRegister").SmtpPassword)
            Exit Sub
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mVendorsValidityRegister)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        'Dim Str As String
        'Str = "openTranDetail();"
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        If ByMail = False Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            MarkLog(Util.Action.Print, "VendorsValidityRegister", mVendorsValidityRegisterCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Vendors Validity Register", "Vendors Validity Register", _
                                      " For " + IIf(cmbRange.SelectedIndex = 0, "As On Date", cmbRange.SelectedItem.Text), "", _
                                      Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                      ReportGeneratedBy:=Session("ReportGenratedBy"), _
                  SmtpHost:=mModuleList.Item("VendorsValidityRegister").SmtpHost, SmtpPort:=mModuleList.Item("VendorsValidityRegister").SmtpPort, _
                    SmtpUser:=mModuleList.Item("VendorsValidityRegister").SmtpUser, SmtpPassword:=mModuleList.Item("VendorsValidityRegister").SmtpPassword)
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
        mVendorList = VendorList.GetVendorstList(0, , , , , , "(ALL)", True, True, True)
        cmbVendor.DataSource = mVendorList
        Session("mVendorList") = mVendorList

        'mVendorTypeList = VendorTypeList.GetVendorTypeList("(ALL)")
        'cmbVendorType.DataSource = mVendorTypeList
        'Session("mVendorTypeList") = mVendorTypeList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            txtDate.Text = New SmartDate(Now.Date.ToString).FormattedText
            'Ajay 09-Nov-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "VendorsValidityRegister") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If
            '--------------------------
            DataFieldBind()
            Controlvisibility(2)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblDateRange.Visible = True
        lblVendorName.Visible = True
        lblVendorType.Visible = True
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        'Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

        Session("UserEmailID") = mModuleList.Item("VendorsValidityRegister").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("VendorsValidityRegister").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mVendorList = Nothing
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
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
    'Ajay 09-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 08-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "VendorsValidityRegister")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 08-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "VendorsValidityRegister")
    End Sub
    '-----
#End Region
    'Added by Abhishek on 10-OCT-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()
            mVendorsValidityRegister = VendorsValidityRegister.GetVendorsValidityRegister(ToDate, cmbVendor.SelectedValue.ToString, 0, False, False, False, CInt(cmbRange.SelectedValue), txtNatureOfVendor.Text.Trim)

            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                   mCompanyDetail.WebSite, "Vendors Validity Register", New SmartDate(ToDate).FormattedText, cmbVendor.SelectedItem.Text, NatureOfVendor, IIf(cmbRange.SelectedIndex = 0, "", cmbRange.SelectedItem.Text), "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
            ds.Clear()

            da.Fill(ds, "VendorsValidityRegister", mVendorsValidityRegister)
            da.Fill(ds, "ReportData", Report)
            Dim columnToRemove1 As String() = {"ToDate", "VendorType", "FromDate", "FromDateFormatted", "VendorID", "IsSupplier", "IsCustomer", "IsserviceProvider"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("VendorsValidityRegister").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("VendorsValidityRegister").Columns.Remove(columnToRemove1(i))
                End If
            Next


            Dim columnToRemove2 As String() = {"ReportName", "SearchStr10", "SearchStr8", "SearchStr5", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ProductVersion", "SINote", "CurrencySymbol", "CurrencyName", "SearchStr11", "SearchStr7", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr6", "SearchStr9", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next
            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "As On Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Vendors"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Vendors Type"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "between Months"
            End If

            If ds.Tables("VendorsValidityRegister").Columns.Contains("Code") Then
                ds.Tables("VendorsValidityRegister").Columns("Code").ColumnName = "Code No."
            End If

            If ds.Tables("VendorsValidityRegister").Columns.Contains("NatureOfVendor") Then
                ds.Tables("VendorsValidityRegister").Columns("NatureOfVendor").ColumnName = "Nature Of Vendor"
            End If
            If ds.Tables("VendorsValidityRegister").Columns.Contains("VendorName") Then
                ds.Tables("VendorsValidityRegister").Columns("VendorName").ColumnName = "Name"
            End If


            If ds.Tables("VendorsValidityRegister").Columns.Contains("ApprovalNo") Then
                ds.Tables("VendorsValidityRegister").Columns("ApprovalNo").ColumnName = "CAA Approval No."
            End If
            If ds.Tables("VendorsValidityRegister").Columns.Contains("ToDateFormatted") Then
                ds.Tables("VendorsValidityRegister").Columns("ToDateFormatted").ColumnName = "Valid Date"
            End If
            If ds.Tables("VendorsValidityRegister").Columns.Contains("ApprovalName") Then
                ds.Tables("VendorsValidityRegister").Columns("ApprovalName").ColumnName = "FAA/EASA Cert Ref."
            End If
            If ds.Tables("VendorsValidityRegister").Columns.Contains("Remark") Then
                ds.Tables("VendorsValidityRegister").Columns("Remark").ColumnName = "Rating/Capability"
            End If

            If ds.Tables("VendorsValidityRegister").Columns.Contains("IDOfVendor") Then
                ds.Tables("VendorsValidityRegister").Columns("IDOfVendor").ColumnName = "ID"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("VendorsValidityRegister"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("VendorsValidityRegister").TableName = "Vendors validity Register"
			Session("ExcelFileName") = "Vendors validity Register"
			Session("dsNew") = dsNew
			'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
			'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
			'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
			'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "VendorsValidityRegister", "Export To Excel " + mVendorsValidityRegisterCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
End Class