Public Class wfrptOtherChargeRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim Fromdate As String = ""
    Dim ToDate As String = ""
    Dim OtherChargeText As String = ""
    Dim OtherChargeNo As String = ""
    Dim mOtherChargeTextList As DistinctTextListForOtherCharge

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mOtherChargeTextList = CType(Session("mOtherChargeTextList"), DistinctTextListForOtherCharge)
    End Sub
    Private Sub setSession()
        Session("mOtherChargeTextList") = mOtherChargeTextList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mOtherChargeTextList")
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

    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        ''txtFromDate.Visible = IIf(Index <> 0, True, False)
        ''txtToDate.Visible = IIf(Index <> 0, True, False)
        ''calFromDate.Visible = IIf(Index = 6, True, False)
        ''calToDate.Visible = IIf(Index = 6, True, False)

        'Added By Saylee on 18-June 2007							
        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
        'Added By Vikrant 27-Mar-2018 For Deccan26032018
        If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
            lblValuedStores.Visible = True
            cmbStoreType.Visible = True
            lblType.Visible = True
        Else
            lblValuedStores.Visible = False
            cmbStoreType.Visible = False
            lblType.Visible = False
            lblStep5.Text = "Step V. Selection of  Report Format"
            lblStepIV.Text = "Step VI. Display Report"
        End If
        'End
    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblOtherChargeNo.Visible = True
        lblReceiptType.Visible = True
        lblStoreType.Visible = IIf(AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ", True, False) ' SPZ Code added by Saylee on 13-Jun-2022 'Added By Vikrant 27-Mar-2018 For Deccan26032018
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblOtherChargeNo.Visible = False
        lblReceiptType.Visible = False
        lblStoreType.Visible = False 'Added By Vikrant 27-Mar-2018 For Deccan26032018
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All   
                txtFromDate.Text = CDate("01-01-1900")
                txtToDate.Text = CDate("01-01-2200")
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6))
                txtToDate.Text = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Text = Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1)
                txtToDate.Text = Today.Date
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date
                txtToDate.Text = Today.Date
        End Select
        txtFromDate.Text = Format(CDate(txtFromDate.Text), AppSettings("DateFormat"))
        txtToDate.Text = Format(CDate(txtToDate.Text), AppSettings("DateFormat"))
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            Fromdate = "1/1/1900"
            ToDate = "1/1/2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            Fromdate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(Fromdate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & ")"
        End If
        OtherChargeText = IIf(txtOtherCharge.Text <> "", txtOtherCharge.Text, "")
        OtherChargeNo = IIf(txtNo.Text <> "", txtNo.Text.Trim, "0")
        If OtherChargeText = "" Then
            lblOtherChargeNo.Text = "Other Charge No. : All "
        Else
            lblOtherChargeNo.Text = "Other Charge No. : " + OtherChargeText + "-" + OtherChargeNo
        End If

        lblReceiptType.Text = IIf(cmbReceiptType.SelectedIndex = 0, "Receipt Type : All", "Receipt Type : " + cmbReceiptType.SelectedItem.Text)
        lblStoreType.Text = "Store Type : " & IIf(cmbStoreType.SelectedIndex > 0, cmbStoreType.SelectedItem.Text, "All") 'Added By Vikrant 27-Mar-2018 For Deccan26032018
        mCompleteSearchingCriteria = lblDateRangeFrom.Text + ", " + lblOtherChargeNo.Text + ", " + IIf(chkDetail.Checked, "Detailed Report", "") + " Format : " + IIf(optLandscape.Checked, "LandScape", "Portrait") + " Receipt Type : " + cmbReceiptType.SelectedItem.Text + ", " + lblStoreType.Text
    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ExcelrptfetchOtherChargeRegister"
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.AddWithValue("@FromDate", Fromdate)
        cmd.Parameters.AddWithValue("@ToDate", ToDate)
        cmd.Parameters.AddWithValue("@Text", OtherChargeText)
        cmd.Parameters.AddWithValue("@No", OtherChargeNo)
        cmd.Parameters.AddWithValue("@ReceiptType", CInt(cmbReceiptType.SelectedValue))
        cmd.Parameters.AddWithValue("@ClientCode", AppSettings("ClientCode"))           'Added By Vikrant 27-Mar-2018 For Deccan26032018

        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()
        dataTable.Columns.Remove("Rem1")
        dataTable.Columns.Remove("Rem2")
        dataTable.Columns.Remove("Rem3")
        Return dataTable
    End Function
    Private Sub GenerateXLSXFile(tbl As DataTable)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsOtherCharge

        Dim objSearch As rptSearchingCriteriaForReceipt
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", OtherChargeText, OtherChargeNo, IIf(cmbStoreType.SelectedIndex > 0, cmbStoreType.SelectedItem.ToString, "All"), "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))


        ds.Clear()
        da.Fill(ds, objSearch)

        Dim columnToRemove As String() = {"ID", "CompanyName", "InternalReceiptNo", "ReleaseNoteNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Aircraft", "Supplier", "Store", "Status", "DCNo", "PartNo", "Description", "Amend", "QuotationNo", "IntOrderNo", "SerialNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ShowLogo", "WorkShop", "WorkOrderText", "WorkOrderNo"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove(i))
            End If
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("rptSearchingCriteriaForReceipt"))
        dsNew.Merge(tbl)

        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("InvText").ColumnName = "Other Charge Text"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("InvNo").ColumnName = "Other Charge No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("FromStore").ColumnName = "Store Type"

        dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
        dsNew.Tables("TMainReport").TableName = "Other Charge Register"
		Session("ExcelFileName") = "Other Charge Register"

		Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "OtherChargeReg", "Export To excel " + mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteriaForReceipt
        Dim objReg As rptOtherChargeRegister
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsOtherCharge As New dsOtherCharge
        SetValues()
        If chkDetail.Checked Then
            If optPortrait.Checked Then
                myReport = New crptOtherChargeRegDetail
            Else
                myReport = New crptOtherChargeRegDetailLandscape
            End If
        Else
            If optPortrait.Checked Then
                myReport = New crptOtherChargeRegSummary
            Else
                myReport = New crptOtherChargeRegSummaryLandscape
            End If
        End If
        objReg = rptOtherChargeRegister.GetOtherChargeRegister(Fromdate, ToDate, OtherChargeText, OtherChargeNo, ReceiptType:=CInt(cmbReceiptType.SelectedValue), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"))
        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", OtherChargeText, OtherChargeNo, IIf(cmbStoreType.SelectedIndex > 0, cmbStoreType.SelectedItem.ToString, ""), "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        If objReg.Count <= 0 Then
             MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
         ElseIf objReg.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 623)
        End If
        dsOtherCharge.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(dsOtherCharge)
        da.Fill(dsOtherCharge, objReg)
        da.Fill(dsOtherCharge, mrptImage)
        da.Fill(dsOtherCharge, objSearch)
        myReport.SetDataSource(dsOtherCharge)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "OtherChargeReg", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) '623
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mOtherChargeTextList = DistinctTextListForOtherCharge.GetDistinctTextList("6", , True, "(All)") 'OtherCharge
        'cmbOtherChargeTextList.DataSource = mOtherChargeTextList
        Session("mOtherChargeTextList") = mOtherChargeTextList
        DataBind()
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then

            RemoveSession()

            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If

            DataFieldBind()

            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6

        End If

    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()
        upnlDisplaySearchCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        SetValues()
        GenerateXLSXFile(CreateDataTable())
    End Sub
    Private Sub cmbOtherChargeTextList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        txtNo.Text = ""
        txtNo.Visible = IIf(txtOtherCharge.Text <> "", True, False)
        If txtOtherCharge.Enabled = True Then
            setFocus(txtOtherCharge)
        End If
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
    Private Sub cmbReceiptType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReceiptType.SelectedIndexChanged
        Select Case cmbReceiptType.SelectedIndex
            Case 0
                lblMessage.Text = ""
            Case 1
                lblMessage.Text = "Considers Supplier(Against PO)-New Records only"
            Case 2
                lblMessage.Text = "Considers Supplier(Against PO)-Ex/OH/Re Records only"
            Case 3
                lblMessage.Text = "Considers other than Supplier(Against PO)-New & Ex/OH/Re Records"
        End Select
        upnlRecType.DataBind()
    End Sub
#End Region

    
End Class