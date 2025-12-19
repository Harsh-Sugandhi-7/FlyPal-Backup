Public Class wfPartListForRCIFromAircraftAsCoreUnitReturn_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mReceiptCumInvoice As ReceiptCumInvoice
    Public mPartListForRCIFromAircraftAsCoreUnitReturnList As PartListForRCIFromAircraftAsCoreUnitReturnList
#End Region

#Region "Business Methods"
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub GetSession()
        mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
        mPartListForRCIFromAircraftAsCoreUnitReturnList = CType(Session("mPartListForRCIFromAircraftAsCoreUnitReturnList"), PartListForRCIFromAircraftAsCoreUnitReturnList)
    End Sub
#End Region

#Region "Data Binding"
    Private Sub DataFieldBinding(Optional ByVal PartNo As String = "", Optional ByVal ToDate As String = "")
        mPartListForRCIFromAircraftAsCoreUnitReturnList = PartListForRCIFromAircraftAsCoreUnitReturnList.GetPartListForRCIFromAircraftAsCoreUnitReturn(PartNo, mReceiptCumInvoice.AircraftID.ToString, ToDate)
        lblResult.Text = "List of Parts: " & mPartListForRCIFromAircraftAsCoreUnitReturnList.Count & " Record(s) Found."
        dgPartList.DataSource = mPartListForRCIFromAircraftAsCoreUnitReturnList
        Session("mPartListForRCIFromAircraftAsCoreUnitReturnList") = mPartListForRCIFromAircraftAsCoreUnitReturnList
        dgPartList.DataBind()
        upnlPartList.Update()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If txtDate.Text.ToString = "" Then
            txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End If
        If Not IsPostBack Then
            setFocus(txtName)
            If mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 1 = 0 Then
                txtDate.Enabled = True
            Else
                txtDate.Enabled = False
            End If
            txtDate.Text = mReceiptCumInvoice.RecCumInvDateFormatted
            DataFieldBinding(txtName.Text.Trim, txtDate.Text.ToString)
        End If

    End Sub
    Private Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartList.PageIndex = 0
        DataFieldBinding(txtName.Text.Trim, txtDate.Text.ToString)
    End Sub
    Private Sub dgPartList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartList.RowCommand
        Select Case e.CommandName
            Case "SelectRec"
                Dim index As Integer = CInt(e.CommandArgument) + dgPartList.PageIndex * dgPartList.PageSize
                If txtDate.Text.ToString = "" Then
                    mReceiptCumInvoice.RecCumInvDate = Today.Date
                Else
                    mReceiptCumInvoice.RecCumInvDate = txtDate.Text
                End If

                mReceiptCumInvoice.CurrencyID = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).CurrencyID 'Added By Prashant 18-Jun-2013 ALL18062013-2
                mReceiptCumInvoice.ConversionFactor = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).ConversionFactor 'Added By Prashant 18-Jun-2013 ALL18062013-2

                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 12 'From Aircraft
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = True
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).ItemID
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Part = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).ItemName
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartDescription = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).ItemDescription
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsPartFromListisSerialized = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).SerialisedStatus
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BaseUnitID = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).UnitID
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).UnitID

                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).DisplayQty
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).IssueItemID
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).StartDate
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).ExpiryDate
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpQtrs = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).ExpQtrs
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpYear = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).ExpYear
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureQtrs = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).CureQtrs
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureYear = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).CureYear

				'If mPartListForRCIFromAircraftAsCoreUnitReturnList(index).ExpiryMonths > 0 Then
				'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = mReceiptCumInvoice.Receipt.RecdDate
				'    If Not (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate) Is System.DBNull.Value Then
				'        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate = CDate(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate).AddMonths(mPartListForRCIFromAircraftAsCoreUnitReturnList(index).ExpiryMonths)
				'    End If
				'End If
				'Commented and Added By Prashant 29-Jan-2014
				'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mPartListForRCIFromAircraftAsCoreUnitReturnList(Index).CRate
				'Commented and Added By Prashant 1-Apr-2016 Because otherchages were not coming in rate
				'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).Rate / mPartListForRCIFromAircraftAsCoreUnitReturnList(index).ConversionFactor
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).CEffRate
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).CEffRate 'Added By Prashant 5-Feb-2019 ALL04022019
                'Commented and Added by Prashant 25-Feb-2013 'All20022013
                'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount = mPartListForRCIFromAircraftAsCoreUnitReturnList(Index).CAmount
                'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount = (mPartListForRCIFromAircraftAsCoreUnitReturnList(index).CRate * mPartListForRCIFromAircraftAsCoreUnitReturnList(index).DisplayQty) 'mPartListForRCIFromAircraftAsCoreUnitReturnList(Index).CAmount
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount = (mPartListForRCIFromAircraftAsCoreUnitReturnList(index).CEffRate * mPartListForRCIFromAircraftAsCoreUnitReturnList(index).DisplayQty) 'mPartListForRCIFromAircraftAsCoreUnitReturnList(Index).CAmount
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCAmount = (mPartListForRCIFromAircraftAsCoreUnitReturnList(index).CEffRate * mPartListForRCIFromAircraftAsCoreUnitReturnList(index).DisplayQty) 'Added By Prashant 5-Feb-2019 ALL04022019
                '--------------------------------------------------------
                'Commented and Added By Prashant 29-Jan-2014
                'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CCommercialRate = mPartListForRCIFromAircraftAsCoreUnitReturnList(Index).CCommercialRate
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CCommercialRate = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).CommercialRate / mPartListForRCIFromAircraftAsCoreUnitReturnList(index).ConversionFactor
                '-----------------------------------
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).UnitID
                mReceiptCumInvoice.AircraftID = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).MachineID

                If (mPartListForRCIFromAircraftAsCoreUnitReturnList(index).SerialisedStatus = True And mPartListForRCIFromAircraftAsCoreUnitReturnList(index).PrimaryCategoryID = 2) Then
                    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).PrimaryCategoryID
                    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CodeNo = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).CodeNo
                End If
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagID = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).ItemTagID
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagName = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).ItemTagName
                'Added on  07-Sep-2016 by Shital
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAirworthinss = mPartListForRCIFromAircraftAsCoreUnitReturnList(index).IsAirworthiness

                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                Session("TotalCount") = CDec(IIf(mPartListForRCIFromAircraftAsCoreUnitReturnList(index).SerialisedStatus, 1, 0)).ToString
                Session("mTotalPendingItemQty") = CDec(IIf(mPartListForRCIFromAircraftAsCoreUnitReturnList(index).SerialisedStatus, 1, 0)).ToString
                DataFieldBinding("", txtDate.Text.ToString)
                Session.Remove("mPartListForRCIFromAircraftAsCoreUnitReturnList")
                mPartListForRCIFromAircraftAsCoreUnitReturnList = Nothing

                Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=" & "wfReceiptCumInvoice_Ajax.aspx" & "&ChildPage1=" & "wfPartListForRCIFromAircraftAsCoreUnitReturn_Ajax.aspx")
        End Select
    End Sub
    Private Sub dgPartList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartList.PageIndexChanging
        dgPartList.PageIndex = e.NewPageIndex
        mPartListForRCIFromAircraftAsCoreUnitReturnList = Session("mPartListForRCIFromAircraftAsCoreUnitReturnList")
        DataFieldBinding(txtName.Text.Trim, txtDate.Text.ToString)
    End Sub
    Private Sub dgPartList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartList.Sorting
        mPartListForRCIFromAircraftAsCoreUnitReturnList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartListForRCIFromAircraftAsCoreUnitReturnList") = mPartListForRCIFromAircraftAsCoreUnitReturnList
        dgPartList.DataSource = mPartListForRCIFromAircraftAsCoreUnitReturnList
        dgPartList.DataBind()
        upnlPartList.Update()
        'DataFieldBinding(txtName.Text.Trim, txtDate.Text.ToString)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.Equals(Guid.Empty) Then
            mReceiptCumInvoice.ReceiptCumInvoiceItems.Remove(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem)
        End If
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
        Session.Remove("mPartListForRCIFromAircraftAsCoreUnitReturnList")
        mPartListForRCIFromAircraftAsCoreUnitReturnList = Nothing
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDate.TextChanged
        dgPartList.PageIndex = 0
        DataFieldBinding(txtName.Text.Trim, txtDate.Text.ToString)
    End Sub
#End Region

End Class