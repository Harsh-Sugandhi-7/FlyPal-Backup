Public Class wfrptLineMaintenanceOrder_Ajax
    Inherits Page

#Region " Variable Declarations "

    Public mVendor As Vendor
    Public mVendorList As VendorList
    Public FromDate As String = ""
    Public ToDate As String = ""
    Public PartNo As String = ""
    Public Description As String = ""
    Public Supplier As String = ""
    Public OrdText As String = ""
    Public OrdNo As String = ""
    Public Status As String = ""
    Public ShowOnlyMSPRecords As Boolean = False

    Dim EventLogID As Guid
    Dim mLineMaintenanceOrderSearchingCriteria As String = String.Empty

#End Region

#Region " Business Properties and Methods "

    Private Sub GetSession()
        mVendorList = CType(Session("mVendorlist"), VendorList)
    End Sub

    Private Sub RemoveSession()
        Session.Remove("mVendorlist")
    End Sub

    Private Overloads Sub SetFocus(control As WebControl)
        If control.Enabled = False Or control.Visible = False Then Exit Sub
        control.Focus()
    End Sub

    Private Sub ControlVisibility(Index As Int16)

        If Index = 6 Then

            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If

    End Sub

    Private Sub ControlVisibilitySearchCriteria()

        lblDateRangeFrom.Visible = True
        lblVendorName.Visible = True
        lblOrderNo.Visible = True
        lblShowOnlyMSPRecords.Visible = True

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

    Private Sub SetDatePeroid(Index As Int32)

        Select Case Index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 3 'Last 1 Quater

                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                End Select

            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 5 'Current Financial Year

                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
                End If

                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
        End Select

    End Sub

    Private Sub SetValues()

        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            FromDate = txtFromDate.Text
            ToDate = txtToDate.Text
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        Supplier = txtSupplierList.Text.Trim
        lblVendorName.Text = "Supplier :  " & Supplier

        OrdText = IIf(txtOrderTextList.Text <> "", Trim(txtOrderTextList.Text), "")

        OrdNo = txtOrderNo.Text.Trim
        lblOrderNo.Text = "Order No.: " & IIf(OrdText + OrdNo <> "", OrdText + "-" + OrdNo, "All")

        ShowOnlyMSPRecords = chkShowOnlyMSPRecords.Checked
        lblShowOnlyMSPRecords.Text = "Show only MSP Records : " + IIf(chkShowOnlyMSPRecords.Checked, "Yes", "No")

        mLineMaintenanceOrderSearchingCriteria = lblDateRangeFrom.Text.Trim + ", " + lblVendorName.Text.Trim + ", " + OrdText + ", " + OrdNo + ", " + lblShowOnlyMSPRecords.Text.Trim

    End Sub

    Public Sub SetReport(IsExcel As Boolean)

        Session("IsExcel") = IsExcel
        Dim myReport As Engine.ReportClass
        Dim objsearch As rptSearchingCriteriaForReceipt
        Dim rpt As rptLineMaintenanceOrderRegister
        Dim da As New ObjectAdapter
        Dim dsLineMaintOrder As New dsLineMaintenanceOrder
        myReport = New crptLineMaintenanceOrderRegister

        SetValues()

        rpt = rptLineMaintenanceOrderRegister.GetLineMaintenanceOrderList(FromDate:=FromDate,
                                                                          ToDate:=ToDate,
                                                                          VendorName:=Supplier,
                                                                          OrderText:=OrdText,
                                                                          OrderNo:=OrdNo,
                                                                          ShowOnlyMSPRecords:=ShowOnlyMSPRecords)

        objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"),
                                                                                  IIf(FromDate = "1-1-1900", "", FromDate),
                                                                                  IIf(ToDate = "1-1-2200", "", ToDate),
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  OrdText,
                                                                                  "",
                                                                                  "",
                                                                                  OrdNo,
                                                                                  "",
                                                                                  Supplier,
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  0,
                                                                                  "",
                                                                                  "",
                                                                                  AppSettings("Logo"))

        If rpt.Count <= 0 Then

            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound,
                            MSGBox.Message_text.NoRecordFound,
                            "There is no record for this search criteria",
                            MsgBoxStyle.OkOnly,
                            "")

            Exit Sub

        ElseIf rpt.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1255)
        End If

        dsLineMaintOrder.Clear()

        If IsExcel = False Then

            Dim mrptImage As rptImage = rptImage.GetImage(dsLineMaintOrder)
            da.Fill(dsLineMaintOrder, mrptImage)

        End If

        da.Fill(dsLineMaintOrder, rpt)
        da.Fill(dsLineMaintOrder, objsearch)
        myReport.SetDataSource(dsLineMaintOrder)

        Session("CrystalReport") = myReport

        Dim Str As String = "openTranDetail();"

        ScriptManager.RegisterStartupScript(Me,
                                            [GetType],
                                            "openTranDetail",
                                            Str,
                                            True)

        MarkLog(Action.Print,
                "LineMaintenanceOrderRegister",
                mLineMaintenanceOrderSearchingCriteria,
                ErrorType.NoError,
                Guid.Empty,
                EventLogID)

    End Sub

    Private Sub AddAttributes()
        txtOrderNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtOrderNo').value,event)")
    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()
        DataBind()
    End Sub

#End Region

#Region " Events "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        GetSession()
        addAttributes()

        If Not IsPostBack Then

            RemoveSession()
            DataFieldBind()
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6

        End If

    End Sub

    Private Sub DateRange_Changed(sender As Object, e As EventArgs) Handles cmbDateRange.SelectedIndexChanged

        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        SetDatePeroid(Index)

        If cmbDateRange.Enabled = True Then
            SetFocus(cmbDateRange)
        End If

    End Sub

    Private Sub ShowSearchCriteria(sender As Object, e As EventArgs) Handles btnCurrentSearchCriteria.Click

        If IsValid Then

            ControlVisibilitySearchCriteria()
            SetValues()
            upnlSelection.Update()

        End If

    End Sub

    Private Sub btnDisplay_Click(sender As Object, e As EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetReport(False)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If IsValid() Then
            SetReport(True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

        MSGBoxCtrl.HideControl()
        MessageBoxResult()

    End Sub

#End Region

End Class