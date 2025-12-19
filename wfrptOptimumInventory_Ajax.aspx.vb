Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Public Class wfrptOptimumInventory_Ajax
    Inherits System.Web.UI.Page

#Region " Variables "
    Dim mCompanyDetail As New CompanyDetail
    Public mCategoryList As CategoryList
    Public PartNo As String = ""
    Public Description As String = ""
    Public SerialNo As String = ""
    Public EventLogDetails As String = String.Empty
    Dim email As Thread
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
            End Select
        End If
    End Sub
    Private Sub Display()
        lblCategory1.Visible = True
        upnlSerachCriteria.Update()
    End Sub
    Private Sub SetValues()
        PartNo = IIf(PartNo <> "", PartNo, "")
        Description = IIf(Description <> "", Description, "")
        lblCategory1.Text = "Category : " & IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, "All")
        EventLogDetails = lblCategory1.Text
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False, Optional ByVal ByMail As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As OptimumInventory

        If chkCategoryWiseReport.Checked Then
            myReport = New crptOptimumInventoryCategorywise
        Else
            If cmbTypeOfInventory.SelectedIndex = 0 Then 'Optimum Inventory Amount
                myReport = New crptOptimumInventoryAll
            ElseIf cmbTypeOfInventory.SelectedIndex = 1 Then 'Optimum Inventory Amount
                myReport = New crptOptimumInventory
            ElseIf cmbTypeOfInventory.SelectedIndex = 2 Then 'Excess Inventory Amount
                myReport = New crptOptimumInventorySortByAccessAmount
            ElseIf cmbTypeOfInventory.SelectedIndex = 3 Then 'Below Optimum Inventory Amount
                myReport = New crptOptimumInventorySortByBelowOptimumAmount
            End If
        End If
        
        rpt = OptimumInventory.GetOptimumInventory(CategoryID:=cmbCategory.SelectedValue.ToString, TypeOfInvnetory:=CInt(cmbTypeOfInventory.SelectedValue), ShowOneTimePurchase:=CInt(IIf(chkOTPOnly.Checked, 1, 0)))

        If ByMail = False Then
            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1392)
                MarkLog(Util.Action.Print, "OptimumInventory", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            End If
        End If
        If (ByMail = True And rpt.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "9.B. OPTIMUM INVENTORY", "9.B. OPTIMUM INVENTORY", "There is no record for this search criteria.", _
                "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                ReportGeneratedBy:=Session("ReportGenratedBy"), _
                SmtpHost:=mModuleList.Item("OptimumInventory").SmtpHost, SmtpPort:=mModuleList.Item("OptimumInventory").SmtpPort, _
                SmtpUser:=mModuleList.Item("OptimumInventory").SmtpUser, SmtpPassword:=mModuleList.Item("OptimumInventory").SmtpPassword)
            Exit Sub
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
       mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
       mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        "", IIf(cmbTypeOfInventory.SelectedIndex > 0, cmbTypeOfInventory.SelectedItem.Text, ""), Description, SerialNo, SearchStr4:="", SearchStr5:=IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, ""), _
        ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:=chkOTPOnly.Checked.ToString, SearchStr7:="", SearchStr8:="", SearchStr9:="", _
        SearchStr10:=AppSettings("Logo"), SearchStr11:="")
        If IsExcel = False Then     'PDF format
            Dim ds As New dsOptimumInventory
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, rpt)
            da.Fill(ds, Report)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "9.B. OPTIMUM INVENTORY", "9.B. OPTIMUM INVENTORY", "", "", _
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                          ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                          SmtpHost:=mModuleList.Item("OptimumInventory").SmtpHost, SmtpPort:=mModuleList.Item("OptimumInventory").SmtpPort, _
                                          SmtpUser:=mModuleList.Item("OptimumInventory").SmtpUser, SmtpPassword:=mModuleList.Item("OptimumInventory").SmtpPassword)
            End If
        ElseIf IsExcel = True Then  'Excel format
            Dim ds As New dsOptimumInventory
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, rpt)

            Dim columnToRemove2 As String() = {"SearchStr1", "SearchStr2", "SearchStr3", "SearchStr4", "ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "ShortName", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next
            Dim columnToRemove As String() = {"AccessAmount", "BelowAmount"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("OptimumInventory").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("OptimumInventory").Columns.Remove(columnToRemove(i))
                End If
            Next

            If ds.Tables("OptimumInventory").Columns.Contains("ItemName") Then
                ds.Tables("OptimumInventory").Columns("ItemName").ColumnName = "Part No."
            End If
            If ds.Tables("OptimumInventory").Columns.Contains("ItemDescription") Then
                ds.Tables("OptimumInventory").Columns("ItemDescription").ColumnName = "Description"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Category"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()
            ds.Tables("ReportData").TableName = "Searching Criteria"
			ds.Tables("OptimumInventory").TableName = "9.B. OPTIMUM INVENTORY"
			Session("ExcelFileName") = "9.B. OPTIMUM INVENTORY"
			dsNew = ds
			Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryList = CategoryList.GetCategoryList("(All)", False)
        cmbCategory.DataSource = mCategoryList
        cmbCategory.DataBind()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
        End If
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetValues()
        SetReport(False)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetValues()
        SetReport(True)
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        '    Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
        Session("UserEmailID") = mModuleList.Item("OptimumInventory").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("OptimumInventory").SendCCMailID
        '--------------------------

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", "OpenByMaiWindow();", True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            email = New Thread(Sub() SetReport(False, True))
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
    Private Sub chkCategoryWiseReport_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkCategoryWiseReport.CheckedChanged
        If chkCategoryWiseReport.Checked Then
            cmbTypeOfInventory.ClearSelection()
            cmbTypeOfInventory.Enabled = False
        Else
            cmbTypeOfInventory.Enabled = True
        End If
    End Sub
#End Region

#Region " Service Methods "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim itemlist As ItemListAutoComplete
        itemlist = ItemListAutoComplete.GetItemList(prefixText, False)
        If count = 0 Then
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).ToArray
        Else
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetSerialNo(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mSerialNoListAutoComplete As SerialNoListAutoComplete = SerialNoListAutoComplete.GetSerialNoList(prefixText)
        If count = 0 Then
            Return (From c As SerialNoListAutoComplete.SerialNoListAutoCompleteInfo In mSerialNoListAutoComplete Select c.SerialNo).ToArray
        Else
            Return (From c As SerialNoListAutoComplete.SerialNoListAutoCompleteInfo In mSerialNoListAutoComplete
               Select c.SerialNo).Take(count).ToArray
        End If
    End Function
#End Region

    
End Class