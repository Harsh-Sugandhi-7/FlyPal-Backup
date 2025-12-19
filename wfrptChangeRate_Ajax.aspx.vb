'Added by Utkarsh on 06-Feb-2014

Public Class wfrptChangeRate_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mInvoiceItemList As InvoiceItemList
    Public mCRate As Decimal

    Public mInvoiceID As Guid
    Public mInvoiceItemID As Guid

    Dim PartNo As String
    Dim Location As String
    Dim SearchIndex1 As String
    'Added by Vikrant on 4-AUG-2011
    Dim EventLogID As Guid
    Public mIsReturnFromOHRepair As Boolean
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mInvoiceItemList = CType(Session("mInvoiceItemList"), InvoiceItemList)
        mCRate = CType(Session("mCRate"), Decimal)
        PartNo = IIf(IsNothing(Session("PartNo")), "", Session("PartNo"))
        Location = IIf(IsNothing(Session("Location")), "", Session("Location"))
        SearchIndex1 = Session("SearchIndex1")
    End Sub
    Private Sub SetSession()
        Session("mInvoiceItemList") = mInvoiceItemList
        Session("PartNo") = PartNo
        Session("Location") = Location
        Session("SearchIndex1") = SearchIndex1

    End Sub
    Private Sub RemoveSession()
        Session.Remove("mInvoiceItemList")
        Session.Remove("PartNo")
        Session.Remove("Location")
        Session.Remove("SearchIndex1")
    End Sub
    Private Sub ChangeRate(ByVal mCRate As Decimal, ByVal mInvoiceID As Guid, ByVal mInvoiceItemID As Guid, ByVal mIsReturnFromOHRepair As Boolean)
        Session("mInvoiceID") = mInvoiceID
        Session("mInvoiceItemID") = mInvoiceItemID
        Session("mCRate") = mCRate
        Session("mIsReturnFromOHRepair") = mIsReturnFromOHRepair

    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ClearControls()
        txtSearchFor.Text = ""
    End Sub
    Private Sub ResetValues()
        PartNo = ""
        Location = ""
    End Sub
    Private Sub FindNow(ByVal LookinType As Integer, Optional ByVal ItemName As String = "", Optional ByVal Location As String = "")
        'This step is Imp when details form  is opened dirctly.
        If LookinType = -1 Then
            LookinType = 0
        End If

        dgPartSearch.DataSource = Nothing
        mInvoiceItemList = Nothing

        'Get List From the Database as per Criteria
        mInvoiceItemList = InvoiceItemList.GetInvoiceItemList(ItemName, Location)

        'Set DataSource of the Grid
        Session("mInvoiceItemList") = mInvoiceItemList
        BindGrid()
    End Sub
    Public Sub SetControl()
        'SearchIndex1 = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex) 'Added by Prashant 12/11/07
        'PartNo = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
        'Location = IIf(cmbSearch.SelectedIndex = 2, Trim(txtSearchFor.Text), "")
        FindNow(SearchIndex1, PartNo, Location)
        'End If
    End Sub
    Private Sub BindGrid()
        dgPartSearch.DataSource = mInvoiceItemList
        dgPartSearch.DataBind()
        lblResult.Text = "List of Parts :" & mInvoiceItemList.Count & " Record(s) found. "
        upnlGrid.Update()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                Case MsgBoxResult.No
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mInvoiceItemList = InvoiceItemList.GetInvoiceItemList("", "")
        Session("mInvoiceItemList") = mInvoiceItemList
        BindGrid()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Vikrant on 4-AUG-2011
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptChangeRate_Ajax.aspx"
            ' RemoveSession()
            If cmbSearch.Enabled = True Then
                SetFocus(cmbSearch)
            End If
            DataFieldBind()
            SetControl()
        End If
    End Sub
    Private Sub dgPartSearch_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartSearch.PageIndexChanging
        dgPartSearch.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub
    Private Sub dgPartSearch_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartSearch.RowCommand
        Select Case e.CommandName
            Case "ChangeRate"
                Dim index As Integer = CInt(e.CommandArgument) + dgPartSearch.PageIndex * dgPartSearch.PageSize
                If AppSettings("LockBackDatedTransaction") = "True" Then
                    Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
                    Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
                    If (CDate(mInvoiceItemList(index).DateFormatted.ToString) >= FirstDayofLastMonth) Then
                        If (CDate(mInvoiceItemList(index).DateFormatted.ToString) < FirstDayofMonth) And (Day(Today.Date) > 10) Then
                            MSGBoxCtrl.Show("Alert!", "Previous Months transactions rate can only be change until " & DateSerial(Year(CDate(mInvoiceItemList(index).DateFormatted.ToString).AddMonths(1)), Month(CDate(mInvoiceItemList(index).DateFormatted.ToString).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                    Else
                        MSGBoxCtrl.Show("Alert!", "Previous Months transactions rate can only be change until " & DateSerial(Year(CDate(mInvoiceItemList(index).DateFormatted.ToString).AddMonths(1)), Month(CDate(mInvoiceItemList(index).DateFormatted.ToString).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                        End If
                    End If
                    Dim mPart As String
                mCRate = mInvoiceItemList(index).CRate
                mPart = mInvoiceItemList(index).ItemName
                Dim mInvoiceID As Guid = mInvoiceItemList(index).InvoiceID
                Dim mInvoiceItemID As Guid = mInvoiceItemList(index).InvoiceItemID
                Dim mIsReturnFromOHRepair As Boolean = mInvoiceItemList(index).IsReturnFromOHRepair
                ChangeRate(mCRate, mInvoiceID, mInvoiceItemID, mIsReturnFromOHRepair)
                'Added by Vikrant on 4-AUG-2011
                MarkLog(Util.Action.Edit, "ChangePartRate", "Part : " + mPart + " Rate : " + mCRate.ToString, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                BindValueForChangeRate()
                pnlRate.Visible = True
                upnlChangeRate.Update()
                mdlPopUpChangeRate.Show()
                BindGrid()
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartSearch.PageIndex = 0

        SearchIndex1 = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex) 'Added by Prashant 12/11/07
        PartNo = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
        Location = IIf(cmbSearch.SelectedIndex = 2, Trim(txtSearchFor.Text), "")

        Session("PartNo") = PartNo
        Session("Location") = Location
        Session("SearchIndex1") = SearchIndex1

        FindNow(cmbSearch.SelectedIndex, PartNo, Location)
        BindGrid()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "ChangePartRate", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        mInvoiceItemList = Nothing
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    'Added By Prashant 22-June-2009 for grid sorting 
    Private Sub dgPartSearch_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartSearch.Sorting
        mInvoiceItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mInvoiceItemList") = mInvoiceItemList
        BindGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#Region "Rate"
#Region "Business Methods"
    Private Sub BindValueForChangeRate()
        txtCurrentRate.Text = mCRate.ToString
        If txtChangeRate.Enabled = True Then
            setFocus(txtChangeRate)
        End If
    End Sub
    Private Sub GetSessionForRate()
        mInvoiceID = CType(Session("mInvoiceID"), Guid)
        mInvoiceItemID = CType(Session("mInvoiceItemID"), Guid)
        mCRate = CType(Session("mCRate"), Decimal)
        mIsReturnFromOHRepair = CBool(Session("mIsReturnFromOHRepair"))
    End Sub
    Private Sub RemoveSessionForRate()
        Session.Remove("mInvoiceID")
        Session.Remove("mInvoiceItemID")
        Session.Remove("mCRate")
    End Sub
    Private Sub ClearControlsForRate()
        txtChangeRate.Text = ""
    End Sub
#End Region

#Region "Events"
    Private Sub btnRateOk_Click(sender As Object, e As System.EventArgs) Handles btnRateOk.Click
        If IsValid Then
            Try
                GetSessionForRate()
                Dim mInvoice As Invoice
                Dim mInvoiceItem As InvoiceItem
                mInvoice = Invoice.GetInvoice(mInvoiceID)
                mInvoiceItem = mInvoice.InvoiceItems.Item(mInvoiceItemID)
                mInvoice.InvoiceItems.UpdateItemsConversionFactore(mInvoice.ConversionFactor)
                mInvoiceItem.CRate = CDec(Val(txtChangeRate.Text.Trim))
                If (mInvoice.TransTypeID = 10 Or mInvoice.TransTypeID = 48 Or mInvoice.TransTypeID = 54 Or (mInvoice.TransTypeID = 67 AndAlso mIsReturnFromOHRepair)) Then  'Added By Prashant 17-Feb-2014 ALL17022014
                    'mInvoiceItem.CEffRate = mInvoiceItem.CEffRate + (CDec(Val(txtChangeRate.Text.Trim)) - CDec(Val(txtCurrentRate.Text.Trim)))
                    mInvoiceItem.CEffRate = CDec(Val(txtChangeRate.Text.Trim))
                    mInvoiceItem.CCommercialRate = CDec(Val(txtChangeRate.Text.Trim))
                End If
                mInvoice.CalculateTotal()
                mInvoice.Save()
                MarkLog(Util.Action.Save, "Rate", "Old Rate : " + mCRate.ToString + "New Rate : " + txtChangeRate.Text.Trim, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                RemoveSessionForRate()
                ClearControlsForRate()
                mdlPopUpChangeRate.Hide()
                pnlRate.Visible = False
                upnlChangeRate.Update()
                SetControl()
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub btnRateClose_Click(sender As Object, e As System.EventArgs) Handles btnRateClose.Click
        MarkLog(Util.Action.Close, "Rate", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionForRate()
        ClearControlsForRate()
        mdlPopUpChangeRate.Hide()
        pnlRate.Visible = False
        upnlChangeRate.Update()
    End Sub
#End Region
#End Region

End Class