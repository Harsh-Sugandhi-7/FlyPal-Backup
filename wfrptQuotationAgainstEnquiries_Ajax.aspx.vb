Public Class wfrptQuotationAgainstEnquiries_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mVendor As Vendor
    Public mItemList As ItemList
    Public mVendorList As VendorList
    Public mDistinctTextListForEnquiry As DistinctTextListForEnquiry
    Public EnqText As String = ""
    Public EnqNo As String = ""
    Public FromDate As String = ""
    Public ToDate As String = ""
    Public PartNo As String = ""
    Public Description As String = ""
    Public Supplier As String = ""
    Public Status As String = ""
    Dim EnquiryType1 As String
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
        Session.Remove("mVendorlist")
        Session.Remove("mItemList")
        Session.Remove("PartNo")
        Session.Remove("Description")
     End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
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
    End Sub
    Private Sub ControlVisibility1(ByVal Index As Int16)
        'lblFor.Visible = (Index <> 0)
        txtSearch.Visible = (Index <> 0)
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        ' lblToDate.Visible = True
        lblEnquiryNo.Visible = True
        lblVendor.Visible = True
         lblPartNo.Visible = True
        lblDesc.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblToDate.Visible = False
        lblEnquiryNo.Visible = True
        lblVendor.Visible = False
          lblPartNo.Visible = False
        lblDesc.Visible = False
    End Sub
    Private Sub SetDatePeroid(ByVal Index As Int32)
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
        If txtSupplier.Text = "" Then
            Supplier = ""
            lblVendor.Text = "Supplier : All"
        Else
             Supplier = txtSupplier.Text
             lblVendor.Text = "Supplier :  " & Supplier
        End If
        EnqText = IIf(txtEnquiryText.Text <> "", Trim(txtEnquiryText.Text), "")
        EnqNo = txtOrderNo.Text
         If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        Session("PartNo") = PartNo
        Session("Description") = Description

        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
        lblEnquiryNo.Text = "Enquiry No. : " & IIf(EnqText + EnqNo <> "", EnqText + "-" + EnqNo, "All")
        mCompleteSearchingCriteria = lblDateRange.Text + ", " + lblEnquiryNo.Text + ", " + lblVendor.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text
    End Sub
    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objReg As QuotationAgainstEnquiries
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsEnquiry As New dsQuotationAgainstEnquiries
        Dim ReportDetails As New rptStatusList
        Dim EnquiryText1 As String = ""
        Dim mCompanyDetail As New CompanyDetail

        SetValues()
        EnqNo = IIf(EnqNo <> "", EnqNo, "0")

        myReport = New crptQuotationAgainstEnquiries

        objReg = QuotationAgainstEnquiries.GetQuotationAgainstEnquiriesList(EnqText, EnqNo, FromDate, ToDate, Supplier, PartNo, Description, 0, _
                                                                            CInt(cmbEnquiryType.SelectedValue))
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, "Enquiry Status", SearchStr1:=New SmartDate(FromDate).FormattedText, _
                SearchStr2:=New SmartDate(ToDate).FormattedText, SearchStr3:=IIf(EnqNo = "0", "", EnqText + "-" + EnqNo), _
                SearchStr4:=Supplier, SearchStr5:=PartNo, ProductVersion:=("Product Version"), SINote:=AppSettings("SINote"), _
                SearchStr6:=Description, SearchStr7:=cmbEnquiryType.SelectedItem.Text, SearchStr8:="", _
                SearchStr9:="", SearchStr10:=AppSettings("Logo"), _
                SearchStr11:="", SearchStr12:="", SearchStr13:="")

        If txtEnquiryText.Text <> "" And txtOrderNo.Text <> "" Then
            EnquiryText1 = Trim(txtEnquiryText.Text) + "-" + txtOrderNo.Text
        ElseIf txtEnquiryText.Text <> "" And txtOrderNo.Text = "" Then
            EnquiryText1 = Trim(txtEnquiryText.Text)
        ElseIf txtEnquiryText.Text = "" And txtOrderNo.Text = "" Then
            EnquiryText1 = ""
        End If

        ReportDetails.Add(New rptStatus(, , EnquiryText1))

        If objReg.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf objReg.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1512)
        End If

        dsEnquiry.Clear()

        Dim mrptImage As rptImage = rptImage.GetImage(dsEnquiry)

        da.Fill(dsEnquiry, objReg)
        da.Fill(dsEnquiry, Report)
        da.Fill(dsEnquiry, mrptImage)

        myReport.SetDataSource(dsEnquiry)
        Session("CrystalReport") = myReport

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        If objReg.Count > 0 Then
            MarkLog(Util.Action.Print, "QuotationAgainstEnquiries", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Overloads Sub SetFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            str = "document.getElementById('" + cntrl.ClientID + "').focus();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try
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
    Private Sub addAttributes()
        txtOrderNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtOrderNo').value,event)")
    End Sub
    Private Sub cmbEnquiryTypeFill()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mDistinctTextListForEnquiry = DistinctTextListForEnquiry.GetDistinctTextList("7", 0, True, "(All)") 'Enquiry
        mItemList = ItemList.GetItemList(0, "", "", "", "", "", "", False)
        Session("mDistinctTextListForEnquiry") = mDistinctTextListForEnquiry
        Session("mItemList") = mItemList
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
            ControlVisibility(6)
            SetDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
            mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", False, True)
            Session("mVendorList") = mVendorList
            DataBind()
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        SetDatePeroid(Index)
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
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbOrderTextList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        txtOrderNo.Text = ""
        txtOrderNo.Visible = IIf(txtEnquiryText.Text <> "", True, False)
        If txtEnquiryText.Enabled = True Then
            SetFocus(txtEnquiryText)
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region


End Class