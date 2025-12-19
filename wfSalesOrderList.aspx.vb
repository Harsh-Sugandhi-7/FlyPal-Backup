Partial Class wfSalesOrderList
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Public Enum UserRightsFor
        urfNew = 1
        urfEdit = 2
        urfDelete = 3
        urfView = 4
        urfPrint = 5
        urfSave = 6
    End Enum
#End Region

#Region " Variable Declaration "
    Public mSalesOrderList As SalesOrderList
    Public mSalesOrder As SalesOrder
    Public mSalesOrderTextList As DistinctTextListForSalesOrder
    Public mQuotationTextList As DistinctTextListForQuotation
    Dim objSearch As rptSearchingCriteriaForSalesOrder
    Dim objReg As rptSalesOrderRegister
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, QuotationText, SalesOrderText, Name, No, Amend As String

    Dim EventLogID As Guid 'Added by Vikrant on 21-July-2011
#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtSearch As System.Web.UI.WebControls.TextBox


    ''Added by Saylee on 16-June 2007
    'Protected WithEvents txtFromDate As SIControls.SICalendar
    'Protected WithEvents txtToDate As SIControls.SICalendar


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mSalesOrder = Session("mSalesOrder")
        mSalesOrderList = Session("mSalesOrderList")
        mSalesOrderTextList = Session("mSalesOrderTextList")
        mQuotationTextList = Session("mQuotationTextList")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        SalesOrderText = Session("SalesOrderText")
        QuotationText = Session("QuotationText")
        Name = Session("Name")
        ''  Amend = Session("Amend")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        lblTotal.Text = Session("lblTotalText") '------Added by Vikrant on 23-Dec-2011 FOR-ALL23122011-----
    End Sub
    Private Sub SetSession()
        Session("mSalesOrder") = mSalesOrder
        Session("mSalesOrderList") = mSalesOrderList
        Session("mSalesOrderTextList") = mSalesOrderTextList
        Session("mQuotationTextList") = mQuotationTextList
        Session("lblTotalText") = lblTotal.Text '------Added by Vikrant on 23-Dec-2011 FOR-ALL23122011-----
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mSalesOrder")
        Session.Remove("mSalesOrderList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSalesOrderList.aspx" Then
            Session.Remove("mSalesOrder")
            Session.Remove("mSalesOrderList")
            Session.Remove("mSalesOrderTextList")
            Session.Remove("mQuotationTextList")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("StatusId")
            Session.Remove("SalesOrderText")
            Session.Remove("QuotationText")
            Session.Remove("Name")
            ''  Session.Remove("Amend")
            Session.Remove("No")
            'Added on 22-01-2007
            Session.Remove("mItemList")
        End If
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgSalesOrderList.DataBind()
        cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId
        'Changed By Yogita on 18-Dec-2007 to solve Bug No:- SOD16
        'cmbSalesOrderText.SelectedValue = IIf(SalesOrderText = "", "(All)", SalesOrderText)
        If mSalesOrderTextList.Contains(SalesOrderText) Then
            cmbSalesOrderText.SelectedValue = IIf(SalesOrderText = "", "(All)", SalesOrderText)
        Else
            cmbSalesOrderText.SelectedValue = "(All)"
        End If
        cmbQuotationText.SelectedValue = IIf(QuotationText = "", "(All)", QuotationText)
        txtName.Text = Name
        txtNo.Text = No
        ''  txtAmend.Text = Amend
        ControlVisibility(SearchIndex, DateIndex)
        lblResult.Text = "List of Sales Order as per criteria : " & mSalesOrderList.Count & " Record(s) found."
    End Sub
    Private Sub NewRecord()
        mSalesOrder = SalesOrder.NewSalesOrder
        mSalesOrder.Date = Today.Date
        Session("mSalesOrder") = mSalesOrder
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mSalesOrder = SalesOrder.GetSalesOrder(mId)
        mSalesOrder.MarkClean()
        Session("mSalesOrder") = mSalesOrder
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        Session("sender") = "Delete"
        mSalesOrder = SalesOrder.GetSalesOrder(mId)
        Session("mSalesOrder") = mSalesOrder
    End Sub
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        'Commented and added by Shweta on 19-August-2013 for ALL16082013-1
        'DateIndex = IIf(IsNothing(DateIndex), 2, DateIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        'end
        StatusId = Session("StatusId")
        SalesOrderText = Session("SalesOrderText")
        QuotationText = Session("QuotationText")
        Name = Session("Name")
        mSalesOrderTextList = DistinctTextListForSalesOrder.GetDistinctTextList("9", 0, True, "(All)") 'Sales Order
        cmbSalesOrderText.DataSource = mSalesOrderTextList
        'Changed By Yogita on 18-Dec-2007 to solve Bug No:-LSO4 suggested by Deven Sir
        'mQuotationTextList = DistinctTextListForQuotation.GetDistinctTextList("8", 0, True, "(All)") '7 Quotation
        mQuotationTextList = DistinctTextListForQuotation.GetDistinctTextList("14", 0, True, "(All)") '7 Quotation
        cmbQuotationText.DataSource = mQuotationTextList
        mSalesOrderList = SalesOrderList.GetSalesOrderList(, , , "1/1/1900", "1/1/2200", 0, , , )
        dgSalesOrderList.DataSource = mSalesOrderList
        Session("mSalesOrderList") = mSalesOrderList
        '------Added by Vikrant on 23-Dec-2011 FOR-ALL23122011-----
        Dim mTotal = mSalesOrderList.Count
        'lblTotal.Text = " " & "[ Total No of Record(s):- " & mTotal & " ]"
        lblTotal.Text = " "
        Session("lblTotalText") = lblTotal.Text
        '-----------------------------------------------------------
        DataBind()
        lblResult.Text = "List of Sales Order as per criteria : " & mSalesOrderList.Count & " Record(s) found."
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Function FormLevelRights(ByVal Type As UserRightsFor) As Boolean
        Select Case Type
        End Select
    End Function
    Private Sub EnableDisableButtons()
        'Enables Buttons as per User Rights
        btnAddNew.Enabled = FormLevelRights(UserRightsFor.urfNew)
        dgSalesOrderList.Columns(9).Visible = FormLevelRights(UserRightsFor.urfEdit)
        dgSalesOrderList.Columns(10).Visible = FormLevelRights(UserRightsFor.urfDelete)
        BtnPrint.Enabled = FormLevelRights(UserRightsFor.urfPrint)
    End Sub
    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Dim mSalesOrder As SalesOrder
                            Session("sender") = ""
                            mSalesOrder = CType(Session("mSalesOrder"), SalesOrder)
                            mSalesOrder.Delete()
                            mSalesOrder.Save()
                            Response.Redirect("wfSalesOrderList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                msg1.ReplacePage = "wfSalesOrderList.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                msg1.ReplacePage = "wfSalesOrderList.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                                msg1.ReplacePage = "wfSalesOrderList.aspx?BackPage=" & Request.QueryString("BackPage")
                                'Added by Vikrant on 21-July-2011
                                Dim mOrderDetail As String = mSalesOrder.SalesOrderNo + " Dated : " + mSalesOrder.DateFormatted + " to " + mSalesOrderList(mSalesOrder.ID).VendorName
                                MarkLog(Util.Action.Delete, "Sales Order", "Can't delete :" & mOrderDetail & " is Currently in use", Util.ErrorType.NoError, mSalesOrder.ID, EventLogID)
                                'End
                                msg1.Show()
                            End If
                            DataFieldBind()
                            SetControl()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Added by Vikrant on 21-July-2011
                                Dim mOrderDetail As String = mSalesOrder.SalesOrderNo + " Dated : " + mSalesOrder.DateFormatted + " to " + mSalesOrderList(mSalesOrder.ID).VendorName
                                MarkLog(Util.Action.Delete, "Sales Order", mOrderDetail, Util.ErrorType.NoError, mSalesOrder.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfSalesOrderList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK 'And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfSalesOrderList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
                    Response.Redirect("wfSalesOrderList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfSalesOrderList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub FindNow(Optional ByVal ItemName As String = "", Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal FromDate As String = "1/1/1800", Optional ByVal ToDate As String = "1/1/3050", Optional ByVal StatusID As Integer = 0, Optional ByVal VendorName As String = "", Optional ByVal QuotationText As String = "", Optional ByVal QuotationNo As Int16 = 0)
        mSalesOrderList = Nothing
        dgSalesOrderList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mSalesOrderList = SalesOrderList.GetSalesOrderList(ItemName, Text, No, FromDate, ToDate, StatusID, VendorName, QuotationText, QuotationNo)
        'Set DataSource of the Grid
        Session("mSalesOrderList") = mSalesOrderList
        dgSalesOrderList.DataSource = mSalesOrderList
        lblResult.Text = "List of Sales Order as per criteria : " & mSalesOrderList.Count & " Record(s) found."
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        'If txtNo.Text = "" Or IsNumeric(txtNo.Text) = False Then txtNo.Text = "0"
        Dim SalesOrderText = "", QuotationText As String = ""
        SalesOrderText = IIf(cmbSalesOrderText.SelectedIndex <= 0, "", cmbSalesOrderText.SelectedItem.Text)
        QuotationText = IIf(cmbQuotationText.SelectedIndex <= 0, "", cmbQuotationText.SelectedItem.Text)
        Select Case Index
            Case -1
                Call FindNow("", "", , FromDate, ToDate, 0, "", , )  'for all records
            Case 0  'all
                Call FindNow("", "", , FromDate, ToDate, 0, "", , ) 'for all records
            Case 1 'date
                Call FindNow("", "", , txtFromDate.Text.ToString, txtToDate.Text.ToString, 0, "", , )   'for all records
            Case 2  'Sales Order Teaxt ,No
                Call FindNow("", SalesOrderText, Val(No), FromDate, ToDate, 0, "", , )   'for all records
            Case 3  'ItemName
                Call FindNow(Name, "", , FromDate, ToDate, 0, "", , )
            Case 4 ' Vendor Name
                Call FindNow(, "", , FromDate, ToDate, 0, Name, , )
            Case 5 ' QuotationText 
                Call FindNow(, "", , FromDate, ToDate, 0, Name, QuotationText, Val(No))
            Case 6 ' Status
                Call FindNow(, "", , FromDate, ToDate, CInt(StatusId), , )
        End Select
        dgSalesOrderList.PageIndex = 0   'Added Code on May,25,2007   
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
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
                'Dim Month As Integer
                'Month = Today.Month
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString))
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString))
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        lblFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        lblToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        txtFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        txtToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        'calFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0 And DateIndex = 6, True, False)
        'calToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0 And DateIndex = 6, True, False)
        'Added by Saylee on 16-June 2007**************
        If SearchIndex = 1 And DateIndex = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
        '************************************************
        cmbSalesOrderText.Visible = IIf(SearchIndex = 2, True, False)
        cmbQuotationText.Visible = IIf(SearchIndex = 5, True, False)
        lblNo.Visible = IIf((SearchIndex = 2 Or SearchIndex = 5) And (cmbSalesOrderText.SelectedIndex <> 0 Or cmbQuotationText.SelectedIndex <> 0), True, False)
        txtNo.Visible = IIf((SearchIndex = 2 Or SearchIndex = 5) And (cmbSalesOrderText.SelectedIndex <> 0 Or cmbQuotationText.SelectedIndex <> 0), True, False)
        '' txtAmend.Visible = IIf(SearchIndex = 5 And cmbQuotationText.SelectedIndex <> 0, True, False)
        txtName.Visible = IIf(SearchIndex >= 3 And SearchIndex <= 4, True, False)
        cmbStatus.Visible = IIf(SearchIndex = 6, True, False)
    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
        '' txtAmend.Text = ""
        txtName.Text = ""
    End Sub
    Private Sub CallFindNowReport(ByVal Index As Integer)
        If txtNo.Text = "" Or IsNumeric(txtNo.Text) = False Then txtNo.Text = "0"
        Dim SOText = "", QuoText As String = ""
        SOText = IIf(cmbSalesOrderText.SelectedIndex <= 0, "", cmbSalesOrderText.SelectedItem.Text)
        QuoText = IIf(cmbQuotationText.SelectedIndex <= 0, "", cmbQuotationText.SelectedItem.Text)
        Select Case Index
            Case -1
                objReg = rptSalesOrderRegister.GetSalesOrderList(, , "1/1/1900", "1/1/2200", , , , )
                objSearch = rptSearchingCriteriaForSalesOrder.GetSearchingCriteriaForSalesOrder(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", 0, "1/1/1900", "1/1/2200", "", "", "", "")
            Case 0  'all
                objReg = rptSalesOrderRegister.GetSalesOrderList(, , "1/1/1900", "1/1/2200", , , , )
                objSearch = rptSearchingCriteriaForSalesOrder.GetSearchingCriteriaForSalesOrder(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", 0, "1/1/1900", "1/1/2200", "", "", "", "")
            Case 1 'Quoatation date
                objReg = rptSalesOrderRegister.GetSalesOrderList(, , txtFromDate.Text.ToString, txtToDate.Text.ToString, , , , )
                objSearch = rptSearchingCriteriaForSalesOrder.GetSearchingCriteriaForSalesOrder(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", 0, txtFromDate.Text.ToString, txtToDate.Text.ToString, "", "", "", "")
            Case 2 'SalesOrder text, No 
                objReg = rptSalesOrderRegister.GetSalesOrderList(SOText, txtNo.Text, "1/1/1900", "1/1/2200", , , , )
                objSearch = rptSearchingCriteriaForSalesOrder.GetSearchingCriteriaForSalesOrder(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), SOText, txtNo.Text, "1/1/1900", "1/1/2200", "", "", "", "")
            Case 3  'ItemName
                objReg = rptSalesOrderRegister.GetSalesOrderList(, , "1/1/1900", "1/1/2200", , txtName.Text, , )
                objSearch = rptSearchingCriteriaForSalesOrder.GetSearchingCriteriaForSalesOrder(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", 0, "1/1/1900", "1/1/2200", "", txtName.Text, "", "")
            Case 4 ' Vendor Name
                objReg = rptSalesOrderRegister.GetSalesOrderList(, , "1/1/1900", "1/1/2200", txtName.Text, , , )
                objSearch = rptSearchingCriteriaForSalesOrder.GetSearchingCriteriaForSalesOrder(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", 0, "1/1/1900", "1/1/2200", txtName.Text, "", "", "")
            Case 5 'Quo No
                objReg = rptSalesOrderRegister.GetSalesOrderList(SOText, txtNo.Text, "1/1/1900", "1/1/2200", , , , )
                objSearch = rptSearchingCriteriaForSalesOrder.GetSearchingCriteriaForSalesOrder(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), SOText, txtNo.Text, "1/1/1900", "1/1/2200", "", "", "", "")
            Case 6 ' Status
                objReg = rptSalesOrderRegister.GetSalesOrderList(, , "1/1/1900", "1/1/2200", , , , CInt(cmbStatus.SelectedValue))
                objSearch = rptSearchingCriteriaForSalesOrder.GetSearchingCriteriaForSalesOrder(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", 0, "1/1/1900", "1/1/2200", txtName.Text, "", "", cmbStatus.SelectedItem.Text)
        End Select
    End Sub
    Private Sub setVariables()
        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        QuotationText = IIf(cmbQuotationText.SelectedIndex <= 0, "", cmbQuotationText.SelectedValue)
        SalesOrderText = IIf(cmbSalesOrderText.SelectedIndex <= 0, "", cmbSalesOrderText.SelectedValue)
        Name = txtName.Text.Trim
        No = txtNo.Text.Trim
        '' Amend = txtAmend.Text.Trim
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("QuotationText") = QuotationText
        Session("SalesOrderText") = SalesOrderText
        Session("No") = No
        '' Session("Amend") = Amend
        Session("Name") = Name
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 21-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            If cmbSearch.Enabled = True Then
                setFocus(cmbSearch)
            End If
            Session("MiddleFrame") = "wfSalesOrderList.aspx"
            DataFieldBind()
            SetControl()
        End If
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbDate.SelectedIndex = 0
        cmbSalesOrderText.SelectedIndex = 0
        cmbQuotationText.SelectedIndex = 0
        ClearControls()
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbSearch.Enabled = True Then
            setFocus(cmbSearch)
        End If
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbDate.Enabled = True Then
            setFocus(cmbDate)
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        CallFindNow(SearchIndex)
        dgSalesOrderList.DataBind()
        BtnPrint.Enabled = IIf(mSalesOrderList.Count = 0, False, True)
        lblResult.Text = "List of Sales Order as per criteria : " & mSalesOrderList.Count & " Record(s) found."
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        If (Not User.IsInRole("SalesOrderNew")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If
        NewRecord()
        'Added by Vikrant on 21-July-2011
        MarkLog(Util.Action.[New], "Sales Order", "", Util.ErrorType.NoError, mSalesOrder.ID, EventLogID)
        'End
        Dim str As String
        str = "<script language='javascript'>  openledgersame('wfSalesOrder_Ajax.aspx?BackPage=index.aspx'); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbSalesOrderText_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSalesOrderText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        'setPeriod(DateIndex)
        If cmbSalesOrderText.Enabled = True Then
            setFocus(cmbSalesOrderText)
        End If
    End Sub
    Private Sub cmbQuotationText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbQuotationText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        'setPeriod(DateIndex)
        If cmbQuotationText.Enabled = True Then
            setFocus(cmbQuotationText)
        End If
    End Sub

#End Region

#Region " Report "
    'Created By :- Jyoti
    'Dated On 8/5/2007

#Region "Report Variable Declaration"
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Private SearchStr1 As String
    Private SearchStr2 As String
#End Region

#Region "Event"
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        If Not User.IsInRole("SalesOrderPrint") Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If
        'For Sales Order List
        Dim Rpt As New crSalesOrderList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        If cmbSearch.SelectedIndex = 0 Then
            'All
            SearchStr1 = "The report shows all records till date."
            SearchStr2 = ""
        ElseIf cmbSearch.SelectedIndex = 1 Then
            'Date
            SearchStr1 = "The report shows records filtered by the following criteria"
            If cmbDate.SelectedIndex = 0 Then
                SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text
            ElseIf cmbDate.SelectedIndex = 6 Then
                'SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + txtFromDate.Value.ToString + " " + lblToDate.Text + " " + txtToDate.Value.ToString
                SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text.ToString).FormattedText
            Else
                'SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + txtFromDate.Value.ToString + " " + lblToDate.Text + " " + txtToDate.Value.ToString
                SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text.ToString).FormattedText
            End If
        ElseIf cmbSearch.SelectedIndex = 2 Then
            'Sales Order No.
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbSalesOrderText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        ElseIf cmbSearch.SelectedIndex = 3 Then
            'Part Number
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        ElseIf cmbSearch.SelectedIndex = 4 Then
            'Vendor
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        ElseIf cmbSearch.SelectedIndex = 5 Then
            'Quotation No.
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbQuotationText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        ElseIf cmbSearch.SelectedIndex = 6 Then
            'Status
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
        End If

        ReportDetails.Add(New rptStatus(, 0, ,
              dgSalesOrderList.Columns.Item(1).HeaderText, dgSalesOrderList.Columns.Item(2).HeaderText, dgSalesOrderList.Columns.Item(3).HeaderText,
              dgSalesOrderList.Columns.Item(4).HeaderText, dgSalesOrderList.Columns.Item(5).HeaderText, dgSalesOrderList.Columns.Item(6).HeaderText,
              dgSalesOrderList.Columns.Item(7).HeaderText, dgSalesOrderList.Columns.Item(8).HeaderText))

        'Added by Saylee on 16-June 2007
        Dim TotalCount As Integer
        Dim mCurrentPageindex As Integer = Me.dgSalesOrderList.PageIndex 'Code Added
        TotalCount = Me.dgSalesOrderList.PageCount
        Dim j As Integer
        Dim I As Integer
        Dim str(7) As String

        For j = 0 To TotalCount - 1

            Me.dgSalesOrderList.PageIndex = j
            Me.dgSalesOrderList.DataSource = mSalesOrderList
            Session("mSalesOrderList") = mSalesOrderList
            dgSalesOrderList.DataBind()
            For I = 0 To Me.dgSalesOrderList.PageSize - 1
                If I <= Me.dgSalesOrderList.Rows.Count - 1 Then

                    str(0) = ""
                    str(1) = ""
                    str(2) = ""
                    str(3) = ""
                    str(4) = ""
                    str(5) = ""
                    str(6) = ""
                    str(7) = ""

                    If Me.dgSalesOrderList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgSalesOrderList.Rows(I).Cells.Item(1).Text
                    If Me.dgSalesOrderList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgSalesOrderList.Rows(I).Cells.Item(2).Text
                    If Me.dgSalesOrderList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgSalesOrderList.Rows(I).Cells.Item(3).Text
                    If Me.dgSalesOrderList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgSalesOrderList.Rows(I).Cells.Item(4).Text
                    If Me.dgSalesOrderList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgSalesOrderList.Rows(I).Cells.Item(5).Text
                    If Me.dgSalesOrderList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgSalesOrderList.Rows(I).Cells.Item(6).Text
                    If Me.dgSalesOrderList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgSalesOrderList.Rows(I).Cells.Item(7).Text
                    If Me.dgSalesOrderList.Rows(I).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.dgSalesOrderList.Rows(I).Cells.Item(8).Text

                    ReportDetails.Add(New rptStatus(, 1, , str(0),
                        str(1), str(2), str(3), str(4), str(5), str(6), str(7)))
                End If
            Next
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Sales Order List Report", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mSalesOrderList.Count = 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            msg1.ReplacePage = "wfSalesOrderList.aspx?Backpage="
            msg1.Show()
            Exit Sub
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        Dim Str1 As String
        Str1 = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)

        'Added by Saylee on 16-June 2007
        Me.dgSalesOrderList.PageIndex = mCurrentPageindex
        Me.dgSalesOrderList.DataSource = mSalesOrderList
        Session("mSalesOrderList") = mSalesOrderList
        dgSalesOrderList.DataBind()
    End Sub

    Private Sub dgSalesOrderList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgSalesOrderList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                If (Not User.IsInRole("SalesOrderView") And Not User.IsInRole("SalesOrderEdit")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If


                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                EditRecord(mID)
                'Changed by Vikrant on 21-july-2011
                Dim mOrderDetail As String = mSalesOrder.SalesOrderNo + " Dated : " + mSalesOrder.DateFormatted + " to " + mSalesOrderList(mSalesOrder.ID).VendorName
                MarkLog(Util.Action.Edit, "Sales Order", mOrderDetail, Util.ErrorType.NoError, mSalesOrder.ID, EventLogID)
                'End
                Dim str As String
                str = "openledgersame('wfSalesOrder_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)

            Case "DeleteRecord"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("SalesOrderDelete")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                DeleteRecord(mID)
                ''Added by Vikrant on 21-July-2011
                Dim mOrderDetail As String = mSalesOrder.SalesOrderNo + " Dated : " + mSalesOrder.DateFormatted + " to " + mSalesOrderList(mSalesOrder.ID).VendorName
                MarkLog(Util.Action.Delete, "Sales Order", mOrderDetail, Util.ErrorType.HandledError, mSalesOrder.ID, EventLogID)

        End Select


    End Sub

    Private Sub dgSalesOrderList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgSalesOrderList.Sorting
        mSalesOrderList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mSalesOrderList") = mSalesOrderList
        dgSalesOrderList.DataSource = dgSalesOrderList
        dgSalesOrderList.DataBind()

    End Sub

    Private Sub dgSalesOrderList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgSalesOrderList.PageIndexChanging
        dgSalesOrderList.PageIndex = e.NewPageIndex
        dgSalesOrderList.DataSource = mSalesOrderList
        Session("mSalesOrderList") = mSalesOrderList
        dgSalesOrderList.DataBind()

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

#End Region

#End Region

End Class

