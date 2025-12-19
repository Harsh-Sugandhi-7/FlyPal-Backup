Public Class wfrptSalesOrderRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mVendor As Vendor
    Public mItemList As ItemList
    Public mVendorList As VendorList
    Public mSalesOrder As SalesOrder
    Public mSalesOrderTextList As DistinctTextListForSalesOrder
    Private SalesOrderText As String = ""
    Private SalesOrdNo As String = ""
    Public FromDate As String = ""
    Public ToDate As String = ""
    Public PartNo As String = ""
    Public Description As String = ""
    Public Supplier As String = ""
    Public Status As String = ""

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Business Properties and Methods "
    Private Sub GetSession()
        mVendorList = CType(Session("mVendorlist"), VendorList)
        mItemList = CType(Session("mItemList"), ItemList)
        PartNo = CType(Session("PartNo"), String)
        Description = CType(Session("Description"), String)
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("mVendorlist") = mVendorList
        Session("mItemList") = mItemList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mSalesOrderList")
        Session.Remove("mVendorlist")
        Session.Remove("mItemList")
        Session.Remove("PartNo")
        Session.Remove("Description")
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
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblSalesOrdNo.Visible = True
        lblVendor.Visible = True
        lblStatus1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblToDate.Visible = False
        lblSalesOrdNo.Visible = True
        lblVendor.Visible = False
        lblStatus1.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
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
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        Supplier = txtCustomer.Text
        lblVendor.Text = "Customer :  " & Supplier
        SalesOrderText = IIf(txtSalesOrderText.Text <> "", Trim(txtSalesOrderText.Text), "")
        SalesOrdNo = txtSalesOrderNo.Text
        Status = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")
        PartNo = IIf(PartNo <> "" And Not IsNothing(PartNo), PartNo, "")
        Description = IIf(Description <> "" And Not IsNothing(Description), Description, "")
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblPartNo.Text = "Part No.     : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
        lblSalesOrdNo.Text = "Sales Order No. : " & IIf(SalesOrderText + SalesOrdNo <> "", SalesOrderText + "-" + SalesOrdNo, "All")
        ' lblEnquiryNo.Text = "Enquiry No.: " & IIf(EnqText + EnqNo <> "", EnqText + "-" + EnqNo, "All")
        lblStatus1.Text = "Status : " & IIf(Status <> "", Status, "All")

        mCompleteSearchingCriteria = lblDateRangeFrom.Text + ", " + lblSalesOrdNo.Text + ", " + lblVendor.Text + ", " + lblStatus1.Text + ", " + _
            IIf(chkDetail.Checked, "Detailed Report", "") + " Format : " + IIf(optLandscape.Checked, "LandScape", "Portrait") + ", " + lblPartNo.Text + ", " + lblDesc.Text
    End Sub
     
    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteriaForSalesOrder
        Dim objReg As rptSalesOrderRegister
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsOrder As New dsSalesOrder
        Dim ReportDetails As New rptStatusList
        Dim SalesOrderText1 As String = ""
        Dim SalesOrderNo1 As String = ""
        SetValues()
        SalesOrdNo = IIf(SalesOrdNo <> "", SalesOrdNo, "0")

        If txtSalesOrderText.Text <> "" And txtSalesOrderNo.Text <> "" Then
            SalesOrderText1 = Trim(txtSalesOrderText.Text) + " - " + txtSalesOrderNo.Text
        ElseIf txtSalesOrderText.Text <> "" And txtSalesOrderNo.Text = "" Then
            SalesOrderText1 = Trim(txtSalesOrderText.Text)
        ElseIf txtSalesOrderText.Text = "" And txtSalesOrderNo.Text = "" Then
            SalesOrderText1 = ""
        End If

        'ReportDetails.Add(New rptStatus(, , SalesOrderText1))
        ReportDetails.Add(New rptStatus(, , SalesOrderText1, AppSettings("Logo")))

        If chkDetail.Checked Then
            If optPortrait.Checked Then
                myReport = New crptSalesOrderRegister
            Else
                myReport = New crptSalesOrderRegisterLandscape
            End If
            objReg = rptSalesOrderRegister.GetSalesOrderList(SalesOrderText, SalesOrdNo, FromDate, ToDate, Supplier, PartNo, Description, CInt(cmbStatus.SelectedValue))
            objSearch = rptSearchingCriteriaForSalesOrder.GetSearchingCriteriaForSalesOrder(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), SalesOrderText, SalesOrdNo, FromDate, ToDate, Supplier, PartNo, Description, Status)

            If objReg.Count <= 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfrptSalesOrderRegister.aspx?Backpage="
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf objReg.Count > 0 Then

                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 603)
            End If
            dsOrder.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(dsOrder) 'Added by Shweta on 23-Feb-2012
            da.Fill(dsOrder, objReg)
            da.Fill(dsOrder, objSearch)
            da.Fill(dsOrder, mrptImage) 'Added by Shweta on 23-Feb-2012
            da.Fill(dsOrder, ReportDetails)
            myReport.SetDataSource(dsOrder)
        Else
            If optPortrait.Checked Then
                myReport = New crptSalesOrderRegSummary
            Else
                myReport = New crptSalesOrderRegSummaryLandscape
            End If
            objReg = rptSalesOrderRegister.GetSalesOrderList(SalesOrderText, SalesOrdNo, FromDate, ToDate, Supplier, PartNo, Description, CInt(cmbStatus.SelectedValue))
            objSearch = rptSearchingCriteriaForSalesOrder.GetSearchingCriteriaForSalesOrder(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), SalesOrderText, SalesOrdNo, FromDate, ToDate, Supplier, PartNo, Description, Status)
            If objReg.Count <= 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfrptOrderRegister.aspx?Backpage="
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf objReg.Count > 0 Then

                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 603)
            End If
            dsOrder.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(dsOrder) 'Added by Shweta on 23-Feb-2012
            da.Fill(dsOrder, objReg)
            da.Fill(dsOrder, objSearch)
            da.Fill(dsOrder, mrptImage) 'Added by Shweta on 23-Feb-2012
            da.Fill(dsOrder, ReportDetails)
            myReport.SetDataSource(dsOrder)
        End If
        Session("CrystalReport") = myReport
        Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        MarkLog(Util.Action.Print, "SalesOrderReg", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) '603
    End Sub
    Private Sub addAttributes()
        txtSalesOrderNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtSalesOrderNo').value,event)")
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
            SetFocus(cmbDateRange)
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
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region
End Class