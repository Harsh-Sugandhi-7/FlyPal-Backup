Public Class wfOtherChargeInvoices_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mOtherChargeInvoiceList As OtherChargeInvoiceList
    Public mOtherCharge As OtherCharge
    Public mOtherChargeDate As String
#End Region

#Region " Business Properties "
    Private Sub GetSession()
        mOtherCharge = Session("mOtherCharge")
        mOtherChargeInvoiceList = Session("mOtherChargeInvoiceList")
    End Sub
    Private Sub SetSession()
        Session("mOtherCharge") = mOtherCharge
        Session("mOtherChargeInvoiceList") = mOtherChargeInvoiceList
    End Sub
    Private Sub setSelectedOtherChargeInvoiceList()
        Dim item As GridViewRow
        Dim chkBox As CheckBox
        Dim Recordno As Integer
        Dim i As Integer
        'Set Selected Notes value  
        For i = 0 To dgInvoiceList.Rows.Count - 1
            Recordno = i + dgInvoiceList.PageSize * dgInvoiceList.PageIndex
            item = dgInvoiceList.Rows(i)
            chkBox = CType(item.FindControl("chkSelect"), CheckBox)
            If chkBox.Checked = True Then
                If Not mOtherCharge.OtherChargeInvoices.Contains(mOtherChargeInvoiceList(Recordno).ID) Then
                    mOtherCharge.OtherChargeInvoices.Add(mOtherCharge.ID)
                    mOtherCharge.OtherChargeInvoices.CurrentItem.InvoiceID = mOtherChargeInvoiceList(Recordno).ID
                End If
            Else
                mOtherCharge.OtherChargeInvoices.Remove(mOtherChargeInvoiceList(Recordno).ID, "")
            End If
        Next
        Session("mOtherCharge") = mOtherCharge
        Session("mOtherChargeInvoiceList") = mOtherChargeInvoiceList
    End Sub
    Private Function IsZeroValue() As Boolean
        If mOtherCharge.OtherChargeInvoices.Count = 0 Then Return False
        If mOtherCharge.OtherChargeInvoices(0).GrandTotal > 0 Then
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub FindNow(ByVal mIsZeroValue As Boolean)
        dgInvoiceList.PageIndex = 0
        dgInvoiceList.DataSource = Nothing
        'Get List From the Database as per Criteria
        mOtherChargeDate = Request.QueryString("OtherChargeDate")
        mOtherChargeInvoiceList = OtherChargeInvoiceList.GetOtherChargeInvoiceList(mOtherCharge.OtherChargeInvoices, mOtherChargeDate, mIsZeroValue)
        'Set DataSource of the Grid
        dgInvoiceList.DataSource = mOtherChargeInvoiceList
        Session("mOtherChargeInvoiceList") = mOtherChargeInvoiceList
        dgInvoiceList.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            FindNow(IsZeroValue)
            optZeroValue.Checked = IsZeroValue()
            optNotZeroValue.Checked = Not IsZeroValue()
            optZeroValue.Enabled = IsZeroValue()
            optNotZeroValue.Enabled = Not IsZeroValue()
            If mOtherCharge.OtherChargeInvoices.Count = 0 Then
                optZeroValue.Enabled = True
                optNotZeroValue.Enabled = True
            End If
            SetSession()
        End If
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        setSelectedOtherChargeInvoiceList()
        Session("mOtherCharge") = mOtherCharge
        Session("mOtherChargeInvoiceList") = mOtherChargeInvoiceList
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mOtherChargeInvoiceList")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub optZeroValue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optZeroValue.CheckedChanged
        FindNow(True)
    End Sub
    Private Sub optNotZeroValue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optNotZeroValue.CheckedChanged
        FindNow(False)
    End Sub
    Private Sub dgInvoiceList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgInvoiceList.PageIndexChanging
        'Added By Saylee on 13th Dec 2007 to solve Bug-LI2 of Other Charge from Inventory By Pramod
        setSelectedOtherChargeInvoiceList()
        dgInvoiceList.PageIndex = e.NewPageIndex
        dgInvoiceList.DataSource = mOtherChargeInvoiceList
        Session("mOtherChargeInvoiceList") = mOtherChargeInvoiceList
        dgInvoiceList.DataBind()
    End Sub
    'New addition by Rupali on 22-Jun-09 for Sorting Order
    Private Sub dgInvoiceList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgInvoiceList.Sorting
        mOtherChargeInvoiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mOtherChargeInvoiceList") = mOtherChargeInvoiceList
        dgInvoiceList.DataSource = mOtherChargeInvoiceList
        dgInvoiceList.DataBind()
    End Sub
#End Region

End Class