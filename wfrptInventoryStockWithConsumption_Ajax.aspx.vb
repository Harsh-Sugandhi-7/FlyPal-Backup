Imports System.Configuration.ConfigurationManager
Imports System.Text

Public Class wfrptInventoryStockWithConsumption_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCategoryList As CategoryList
    Public mAircraft As Machine
    Public mCategory As Category
    Public FromDate As String
    Public ToDate As String
    Public PartNo As String
    Public Description As String
    Public StrAircraft As String
    Public StrCategory As String
    Public mMachineNameValueWithModelList As MachineNameValueWithModelList
    Dim mAircraftwiseConsumptionSearchingCriteria As String = String.Empty
    Dim aircraftlist As String = ""
    Dim mText As String = ""
    Dim email As Thread
    Dim mModuleList As ModuleList
    Dim AircraftIDXML As New StringBuilder
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCategoryList = CType(Session("mCategoryList"), CategoryList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mMachineNameValueWithModelList = CType(Session("mMachineNameValueWithModelList"), MachineNameValueWithModelList)
        mModuleList = Session("mModuleList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mMachineNameValueWithModelList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlvisibilityForSearchingCriteria(ByVal showlabel As Boolean)
        lblDateRangeFrom.Visible = showlabel
        lblPartNo.Visible = showlabel
        lblDesc.Visible = showlabel
        lblAircraftName.Visible = showlabel
        lblCategoryName.Visible = showlabel
        upnlSearchingCriteria.Update()
    End Sub
    Private Sub SetValues()
        FromDate = txtFromDate.Text.Trim
        ToDate = txtToDate.Text.Trim
        lblDateRangeFrom.Text = "Date Range  : " & New SmartDate(txtFromDate.Text).FormattedText & " To Date : " & New SmartDate(txtToDate.Text.ToString).FormattedText
        If (txtSearch.Text.Trim.IndexOf("[") >= 0 AndAlso txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")

        aircraftlist = hdnAircraftList.Value
        StrAircraft = hdnAircraftNameList.Value
        lblAircraftName.Text = "Aircraft Name : " & IIf(StrAircraft.Length > 0, StrAircraft, "All")

        If aircraftlist.ToString <> "" Then
            AircraftIDXML.Append("<AircraftIDs>")
            For Each value As String In aircraftlist.Split(",")
                AircraftIDXML.Append("<ID>")
                AircraftIDXML.Append(value)
                AircraftIDXML.Append("</ID>")
            Next
            AircraftIDXML.Append("</AircraftIDs>")
        End If

        StrCategory = IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, "")
        Session("mAircraft") = mAircraft
        Session("mCategory") = mCategory
        mAircraftwiseConsumptionSearchingCriteria = lblDateRangeFrom.Text + ", " + lblAircraftName.Text + ", " + lblCategoryName.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", "
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean, Optional ByVal ByMail As Boolean = False)
        Try
            SetValues()
            Dim objSearch As rptSearchingCriteria
            Dim rpt As rptInventoryStockWithConsumption
            rpt = rptInventoryStockWithConsumption.GetrptInventoryStockWithConsumption(FromDate, ToDate, AircraftIDXML.ToString, cmbCategory.SelectedValue, PartNo, Description)
            objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, "", "", StrCategory, "", "", StrAircraft, "", Description, "", 0, "", "", mText, AppSettings("Logo"))
            If ByMail = False Then
                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1502)
                End If
            End If

            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim ds As New dsInventoryStockWithConsumption

            myReport = New crptInventoryStockWithConsumption

            ds.Clear()
            If IsExcel = False Then
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                da.Fill(ds, mrptImage)
            End If
            da.Fill(ds, rpt)
            da.Fill(ds, objSearch)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            If ByMail = False Then
                If IsExcel = False Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
                    MarkLog(Util.Action.Print, "InventoryStockWithConsumption", mAircraftwiseConsumptionSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                Else
                    Dim columnToRemove2 As String() = {"CompanyName", "Store", "FromStore", "Nomenclature", "SupplierName", "BranchName", "KitName", _
                                                       "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "WorkShop", _
                                                       "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search2", "Search3", "Search4", "Search5", _
                                                       "Search6", "Search7", "Search8", "Search9", "Search10", "RelNoteNo"}
                    For i As Integer = 0 To columnToRemove2.Length - 1
                        If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                            ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                        End If
                    Next

                    Dim columnToRemove As String() = {"ItemID"}
                    For i As Integer = 0 To columnToRemove.Length - 1
                        If ds.Tables("rptInventoryStockWithConsumption").Columns.Contains(columnToRemove(i)) Then
                            ds.Tables("rptInventoryStockWithConsumption").Columns.Remove(columnToRemove(i))
                        End If
                    Next
                    If ds.Tables("rptInventoryStockWithConsumption").Columns.Contains("EffRate") Then
                        ds.Tables("rptInventoryStockWithConsumption").Columns("EffRate").ColumnName = "Unit Rate"
                    End If
                    ds.Tables("rptInventoryStockWithConsumption").Columns("ServiceableStockQty").SetOrdinal(2)
                    ds.Tables("rptInventoryStockWithConsumption").Columns("UnserviceableQty").SetOrdinal(3)
                    ds.Tables("rptInventoryStockWithConsumption").Columns("StockQty").SetOrdinal(4)
                    ds.Tables("rptInventoryStockWithConsumption").Columns("Unit Rate").SetOrdinal(5)
                    ds.Tables("rptInventoryStockWithConsumption").Columns("IssueQty").SetOrdinal(6)
                    ds.Tables("rptInventoryStockWithConsumption").Columns("AlternatePart").SetOrdinal(7)
                    Dim dsNew As New DataSet
                    dsNew.Clear()
                    dsNew.Merge(ds.Tables("rptSearchingCriteria"))
                    dsNew.Merge(ds.Tables("rptInventoryStockWithConsumption"))
                    dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
                    dsNew.Tables("rptInventoryStockWithConsumption").TableName = "Inventory Stock With Consumption"
					Session("ExcelFileName") = "Inventory Stock With Consumption"
					Session("dsNew") = dsNew

                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    MarkLog(Util.Action.Print, "InventoryStockWithConsumption", "Export To Excel " + mAircraftwiseConsumptionSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                End If

            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Inventory Stock With Consumption", "Inventory Stock With Consumption", " For " + lblDateRangeFrom.Text, "", _
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                          ReportGeneratedBy:=Session("ReportGenratedBy"), _
                    SmtpHost:=mModuleList.Item("InventoryStockWithConsumption").SmtpHost, SmtpPort:=mModuleList.Item("InventoryStockWithConsumption").SmtpPort, SmtpUser:=mModuleList.Item("InventoryStockWithConsumption").SmtpUser, SmtpPassword:=mModuleList.Item("InventoryStockWithConsumption").SmtpPassword)

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
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryList = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList

        mMachineNameValueWithModelList = MachineNameValueWithModelList.GetMachineList(Today.Date.ToString)
        Session("mMachineNameValueWithModelList") = mMachineNameValueWithModelList
        ChklistAircraft.DataSource = mMachineNameValueWithModelList

        DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
            ControlvisibilityForSearchingCriteria(False)
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport(False, False)
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
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        aircraftlist = hdnAircraftList.Value
        StrAircraft = IIf(aircraftlist = String.Empty, String.Empty, aircraftlist)
        Session("UserEmailID") = mModuleList.Item("InventoryStockWithConsumption").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("InventoryStockWithConsumption").SendCCMailID
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetReport(True, False)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mCategoryList = Nothing
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlvisibilityForSearchingCriteria(True)
        SetValues()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region
End Class