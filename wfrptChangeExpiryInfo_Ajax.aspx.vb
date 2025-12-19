'AJAX Conversion By Vikrant On 10-Feb-2014

Public Class wfrptChangeExpiryInfo_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStockItemList As StockItemList
    Dim PartNo As String
    Dim SearchIndex, PartNoLocation As String

    Public mReceiptItemID As Guid
    'Added by Vikrant on 4-AUG-2011
    Dim EventLogID As Guid

    Public mReceiptInfo As StockItemList.ReceiptInfo
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mStockItemList = CType(Session("StockItemList"), StockItemList)
        PartNo = IIf(IsNothing(Session("PartNo")), "", Session("PartNo"))
        mReceiptItemID = CType(Session("mReceiptItemID"), Guid)
        SearchIndex = IIf(IsNothing(Session("SearchIndex")), "", Session("SearchIndex"))
        PartNoLocation = Session("PartNoLocation")
        mReceiptInfo = Session("mReceiptInfo")
    End Sub
    Private Sub SetSession()
        Session("StockItemList") = mStockItemList
        Session("PartNo") = PartNo
        Session("mReceiptItemID") = mReceiptItemID
        Session("SearchIndex") = SearchIndex
        Session("PartNoLocation") = PartNoLocation
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("SearchIndex")
        Session.Remove("PartNoLocation")
        Session.Remove("StockItemList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptChangeExpiryInfo_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Sub ChangeItemType(ByVal mReceiptItemID As Guid, ByVal mItemTypeID As Integer, ByVal Name As String)
        Session("mReceiptItemID") = mReceiptItemID
        Session("mItemTypeID") = mItemTypeID
        Session("Name") = Name
    End Sub
    Private Sub ChangeExpiryInfo(ByVal mReceiptItemID As Guid)
        Session("mReceiptItemID") = mReceiptItemID
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    'Private Sub ControlVisibility1(ByVal SearchIndex As Int32)
    '    If SearchIndex = 0 Then
    '        lblFor.Visible = False
    '        txtSearchFor.Visible = False
    '        cmbPartType.Visible = False
    '        cmbPartType.SelectedIndex = 0
    '    ElseIf SearchIndex = 1 Then
    '        lblFor.Visible = True
    '        txtSearchFor.Visible = True
    '        txtSearchFor.Text = PartNo
    '        cmbPartType.Visible = False
    '        cmbPartType.SelectedIndex = 0
    '    ElseIf SearchIndex = 2 Then
    '        lblFor.Visible = True
    '        txtSearchFor.Visible = True
    '        txtSearchFor.Text = Location
    '        cmbPartType.Visible = False
    '        cmbPartType.SelectedIndex = 0
    '    ElseIf SearchIndex = 3 Then
    '        lblFor.Visible = False
    '        txtSearchFor.Visible = False
    '        cmbPartType.Visible = True
    '    End If
    'End Sub
    'Private Sub ClearControls()
    '    txtSearchFor.Text = ""
    'End Sub
    Private Sub ResetValues()
        PartNo = ""
    End Sub
    Private Sub FindNow(Optional ByVal ItemName As String = "")
        'This step is Imp when details form  is opened dirctly.
        'If LookinType = -1 Then
        '    LookinType = 0
        'End If

        dgPartSearch.DataSource = Nothing
        mStockItemList = Nothing

        'Get List From the Database as per Criteria
        mStockItemList = StockItemList.GetStockItemList(PartNo, "", 0)

        'Set DataSource of the Grid
        dgPartSearch.DataSource = mStockItemList
        Session("StockItemList") = mStockItemList
    End Sub
    Public Sub SetControl()
        SearchIndex = Session("SearchIndex")
        PartNo = Session("PartNo")

        FindNow(PartNo)
        dgPartSearch.DataBind()

        cmbSearch.SelectedIndex = CInt(Val(SearchIndex))

        'ControlVisibility1(CInt(Val(SearchIndex)))
        lblResult.Text = "List of Parts : " & mStockItemList.Count & " Record(s) found. "
        upnlGrid.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mStockItemList = StockItemList.GetStockItemList("", "", 0)
        dgPartSearch.DataSource = mStockItemList
        Session("StockItemList") = mStockItemList

        DataBind()

        SearchIndex = Session("SearchIndex")
        PartNo = Session("PartNo")
        lblResult.Text = "List of Parts : " & mStockItemList.Count & " Record(s) found "
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'RemoveSession()
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Vikrant on 4-AUG-2011
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfrptChangeExpiryInfo_Ajax.aspx"
            If cmbSearch.Enabled = True Then
                SetFocus(cmbSearch)
            End If
            DataFieldBind()
            SetControl()
        End If
    End Sub
    Private Sub dgPartSearch_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartSearch.RowCommand
        Select Case e.CommandName
            Case "ChangeExpiryInfo"    'Added Code
                dgPartSearch.DataSource = mStockItemList
                dgPartSearch.DataBind()
                'Dim index As Integer = CInt(e.CommandArgument) + dgPartSearch.PageIndex * dgPartSearch.PageSize
                Dim mReceiptItemID As Guid = New Guid(dgPartSearch.DataKeys(CInt(e.CommandArgument)).Value.ToString())
                SetSession()
                ChangeExpiryInfo(mReceiptItemID)
                mReceiptInfo = mStockItemList.Item(mReceiptItemID)
                Session("mReceiptInfo") = mReceiptInfo

                'Added by Vikrant on 4-AUG-2011
                Dim mCureDate As String = mReceiptInfo.StartDateFormatted.ToString
                Dim mExpiryDate As String = mReceiptInfo.ExpiryDateFormatted.ToString
                Dim mCureQtr As String = mReceiptInfo.CureQtrYear
                Dim mExpiryQtr As String = mReceiptInfo.ExpQtrYear
                MarkLog(Util.Action.Edit, "Change Expiry Info", "Cure Date :" + mCureDate + " Expiry Date : " + mExpiryDate + " Cure Qtr. : " + mCureQtr + " Expiry Qtr. : " + mExpiryQtr, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                BindValueForChangeExpiryInfo()
                pnlExpiryInfo.Visible = True
                upnlChangeExpiryInfo.Update()
                mdlPopUpChangeExpiryInfo.Show()
                'BindGrid()
        End Select
    End Sub
    'Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
    '    Dim Index As Int16 = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
    '    ClearControls()
    '    ControlVisibility1(cmbSearch.SelectedIndex)
    '    If cmbSearch.Enabled = True Then
    '        SetFocus(cmbSearch)
    '    End If
    'End Sub
    Private Sub btnSearch_Click(sender As Object, e As ImageClickEventArgs) Handles btnSearch.Click
        dgPartSearch.PageIndex = 0

        SearchIndex = cmbSearch.SelectedIndex
        PartNo = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")

        Session("SearchIndex") = SearchIndex
        Session("PartNo") = PartNo

        FindNow(PartNo)
        dgPartSearch.DataBind()

        lblResult.Text = "List of Parts : " & mStockItemList.Count & " Record(s) found "
        upnlGrid.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "Change Expiry Info", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        mStockItemList = Nothing
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    'Added By Prashant 22-June-2009 for grid sorting
    Private Sub dgPartSearch_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartSearch.Sorting
        mStockItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("StockItemList") = mStockItemList
        dgPartSearch.DataSource = mStockItemList
        dgPartSearch.DataBind()
    End Sub
    '-----------------------------------------------
    Private Sub dgPartSearch_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartSearch.PageIndexChanging
        dgPartSearch.PageIndex = e.NewPageIndex
        Session("StockItemList") = mStockItemList
        dgPartSearch.DataSource = mStockItemList
        dgPartSearch.DataBind()
    End Sub
#End Region

#Region "Expiry Info"

#Region "Methods"
    Private Sub BindValueForChangeExpiryInfo()
        If txtStartDate.Enabled = True Then
            setFocus(txtStartDate)
        End If
        txtStartDate.Text = mReceiptInfo.StartDateFormatted.ToString
        txtExpiryDate.Text = mReceiptInfo.ExpiryDateFormatted.ToString
        upnlChangeExpiryInfo.DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtExpiryDate" Then
            'If IsDate(txtExpiryDate.Value) Then
            ''If txtExpiryDate.Value.ToString <> "" And txtStartDate.Value.ToString = "" Then
            ''    'custValidator.ErrorMessage = "Start Date. Required"
            ''    'e.IsValid = False
            ''End If
            If (Not txtExpiryDate.Text = "" And txtStartDate.Text = "") And ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And ((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And ((mReceiptInfo.ExpiryMonth <> 0 Or mReceiptInfo.ExpiryQuarter <> 0)) Then
                custValidator.ErrorMessage = "Select Cure Date "
                e.IsValid = False
            ElseIf IsDate(txtExpiryDate.Text) And IsDate(txtStartDate.Text) Then
                If CDate(txtExpiryDate.Text) < CDate(txtStartDate.Text) Then
                    custValidator.ErrorMessage = "Expiry date should be Later to Cure Date."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtStartDate" Then
            If (txtExpiryDate.Text = "" And Not txtStartDate.Text = "") And ((txtExpYear.Text = "" Or txtExpYear.Text = "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0")) And ((txtCureYear.Text = "" Or txtCureYear.Text = "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0")) And ((mReceiptInfo.ExpiryMonth <> 0 Or mReceiptInfo.ExpiryQuarter <> 0)) Then
                custValidator.ErrorMessage = "Select Expiry Date "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtCureQtrs" Then
            If (Not txtExpiryDate.Text = "" Or Not txtStartDate.Text = "") And (Val(txtCureQtrs.Text) <> 0 Or Val(txtCureYear.Text) <> 0 Or Val(txtExpQrts.Text) <> 0 Or Val(txtExpYear.Text) <> 0) Then
                custValidator.ErrorMessage = "Enter either Cure/Expiry Date or Cure/Expiry Quarters."
                e.IsValid = False
            ElseIf Val(txtCureQtrs.Text) < 0 Or Val(txtCureQtrs.Text) > 4 Then
                custValidator.ErrorMessage = "Cure Quarters should be between 1 to 4"
                e.IsValid = False
            ElseIf (txtExpiryDate.Text = "" And txtStartDate.Text = "") And (txtCureQtrs.Text <> "" And txtCureQtrs.Text <> "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0") Then
                custValidator.ErrorMessage = "Cure Year also required with Cure Qtrs."
                e.IsValid = False
            ElseIf ((txtExpiryDate.Text = "" And txtStartDate.Text = "")) And ((txtExpQrts.Text = "" Or txtExpQrts.Text = "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0")) And ((txtCureQtrs.Text <> "" And txtCureQtrs.Text <> "0") And (txtCureYear.Text <> "" And txtCureYear.Text <> "0")) And ((mReceiptInfo.ExpiryMonth <> 0 Or mReceiptInfo.ExpiryQuarter <> 0)) Then
                custValidator.ErrorMessage = "Expiry Year and Expiry Quarters required."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtExpQrts" Then
            If Val(txtExpQrts.Text) < 0 Or Val(txtExpQrts.Text) > 4 Then
                custValidator.ErrorMessage = "Expiry Quarters should be between 1 to 4"
                e.IsValid = False
            ElseIf (txtExpiryDate.Text = "" And txtStartDate.Text = "") And (txtExpQrts.Text <> "" And txtExpQrts.Text <> "0") And (txtExpYear.Text = "" Or txtExpYear.Text = "0") Then
                custValidator.ErrorMessage = "Expiry Year also required with Expiry Qtrs."
                e.IsValid = False
            ElseIf ((txtExpiryDate.Text = "" And txtStartDate.Text = "")) And ((txtExpQrts.Text <> "" And txtExpQrts.Text <> "0") And (txtExpYear.Text <> "" And txtExpYear.Text <> "0")) And ((txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") And (txtCureYear.Text = "" Or txtCureYear.Text = "0")) And ((mReceiptInfo.ExpiryMonth <> 0 Or mReceiptInfo.ExpiryQuarter <> 0)) Then
                custValidator.ErrorMessage = "Cure Year and Cure Quarters required."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            '------------------------------
        ElseIf custValidator.ControlToValidate = "txtExpYear" Then
            If (txtExpiryDate.Text = "" And txtStartDate.Text = "") And (txtExpiryDate.Text = "" And txtStartDate.Text = "") And (txtExpYear.Text <> "" And txtExpYear.Text <> "0") And (txtExpQrts.Text = "" Or txtExpQrts.Text = "0") Then
                custValidator.ErrorMessage = "Expiry Qtrs also required with Expiry Year."
                e.IsValid = False
            ElseIf txtExpYear.Text <> "0" And txtExpYear.Text <> "" And Len(txtExpYear.Text) < 4 Then
                custValidator.ErrorMessage = "Expiry Year should be not be less than 4 digits"
                e.IsValid = False
            ElseIf txtExpYear.Text <> "0" And txtExpYear.Text <> "" And Val(txtExpYear.Text) < 1753 Or Val(txtExpYear.Text) > 3030 Then
                custValidator.ErrorMessage = "Enter valid Expiry Year"
                e.IsValid = False
            ElseIf (txtCureYear.Text <> "0" And txtExpYear.Text <> "0") And (Val(txtCureYear.Text) > Val(txtExpYear.Text)) Then
                custValidator.ErrorMessage = "Expiry Year should be Later to Cure Year."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtCureYear" Then
            If (txtExpiryDate.Text = "" And txtStartDate.Text = "") And (txtCureYear.Text <> "" And txtCureYear.Text <> "0") And (txtCureQtrs.Text = "" Or txtCureQtrs.Text = "0") Then
                custValidator.ErrorMessage = "Cure Qtrs also required with Cure Year."
                e.IsValid = False
            ElseIf txtCureYear.Text <> "0" And Len(txtCureYear.Text) < 4 Then
                custValidator.ErrorMessage = "Cure Year should be not be less than 4 digits"
                e.IsValid = False
            ElseIf txtCureYear.Text <> "0" And Val(txtCureYear.Text) < 1753 Or Val(txtCureYear.Text) > 3030 Then
                custValidator.ErrorMessage = "Enter valid Cure Year"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region
#Region "Events"
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        'If Len(txtChangedLocation.Text) <> 0 Then
        If IsValid Then
            StockItemList.ChangeExpiryInfo(mReceiptItemID, mReceiptInfo.ExpiryMonth, mReceiptInfo.ExpiryQuarter, IIf(IsDate(txtStartDate.Text), txtStartDate.Text, System.DBNull.Value).ToString, _
                                           IIf(IsDate(txtExpiryDate.Text), txtExpiryDate.Text, System.DBNull.Value).ToString, CInt(Val(txtCureQtrs.Text)), _
                                           CInt(Val(txtCureYear.Text)), CInt(Val(txtExpQrts.Text)), CInt(Val(txtExpYear.Text)), mReceiptInfo.ExpiryMonth, _
                                           mReceiptInfo.ExpiryQuarter, IsExpiryNA:=chkIsExpiryNA.Checked, IsExpiryUnlimited:=chkIsExpiryUnlimited.Checked)
            Dim mStartDate As String = IIf(IsDate(txtStartDate.Text), New SmartDate(txtStartDate.Text).FormattedText, System.DBNull.Value.ToString)
            Dim mExpiryDate As String = IIf(IsDate(txtExpiryDate.Text), New SmartDate(txtExpiryDate.Text).FormattedText, System.DBNull.Value.ToString)
            MarkLog(Util.Action.Save, "Expiry Info", "Cure Date : " + mStartDate + " Expiry Date : " + mExpiryDate + " Cure Qtr. : " + txtCureQtrs.Text + "/" + txtCureYear.Text + " Expiry Qtr. : " + txtExpQrts.Text + "/" + txtExpYear.Text, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'End If
            RemoveSessionForExpiryInfo()
            mdlPopUpChangeExpiryInfo.Hide()
            pnlExpiryInfo.Visible = False
            upnlChangeExpiryInfo.Update()
            SetControl()
        End If
    End Sub
    Private Sub txtStartDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStartDate.TextChanged
            If txtStartDate.Text <> mReceiptInfo.StartDate.ToString Then
                If Not IsDate(txtStartDate.Text) Then
                    mReceiptInfo.StartDate = System.DBNull.Value
                Else
                    mReceiptInfo.StartDate = txtStartDate.Text
                End If
                BindValueForChangeExpiryInfo()
                'CNDC
                'txtStartDate.Text = mReceipt.ReceiptItems.CurrentItem.StartDate.ToString
                'txtExpiryDate.Text = mReceipt.ReceiptItems.CurrentItem.ExpiryDate.ToString

                'txtStartDate.Text = mReceiptInfo.StartDate.ToString
                'txtExpiryDate.Text = mReceiptInfo.ExpiryDate.ToString
            End If
         ControlvisibilityForExpiryInfo()
        upnlChangeExpiryInfo.Update()
    End Sub
    Private Sub txtExpiryDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExpiryDate.TextChanged
        ControlvisibilityForExpiryInfo()
        upnlChangeExpiryInfo.Update()
    End Sub
    Private Sub txtCureQtrs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCureQtrs.TextChanged
        If Val(txtCureQtrs.Text) >= 0 And Val(txtCureQtrs.Text) <= 4 Then
            mReceiptInfo.CureQtrs = Val(txtCureQtrs.Text)
            txtExpQrts.DataBind()
            txtExpYear.DataBind()
        End If
        ControlvisibilityForExpiryInfo()
        upnlChangeExpiryInfo.Update()
    End Sub
    Private Sub txtCureYear_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCureYear.TextChanged
        If Val(txtCureQtrs.Text) >= 0 And Val(txtCureQtrs.Text) <= 4 Then
            mReceiptInfo.CureYear = Val(txtCureYear.Text)
            txtExpQrts.DataBind()
            txtExpYear.DataBind()
        End If
        ControlvisibilityForExpiryInfo()
        upnlChangeExpiryInfo.Update()
    End Sub
    Private Sub txtExpQrts_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtExpQrts.TextChanged
        ControlvisibilityForExpiryInfo()
        upnlChangeExpiryInfo.Update()
    End Sub
    Private Sub txtExpYear_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtExpYear.TextChanged
         ControlvisibilityForExpiryInfo()
        upnlChangeExpiryInfo.Update()
    End Sub
    Private Sub RemoveSessionForExpiryInfo()
        Session.Remove("mReceiptInfo")
    End Sub
    Private Sub btnCloseChangeExpiryInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseChangeExpiryInfo.Click
        RemoveSessionForExpiryInfo()
        mdlPopUpChangeExpiryInfo.Hide()
        pnlExpiryInfo.Visible = False
        upnlChangeExpiryInfo.Update()
    End Sub
    Private Sub chkIsExpiryNA_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsExpiryNA.CheckedChanged
        If chkIsExpiryNA.Checked Then
            chkIsExpiryUnlimited.Checked = False
            chkIsExpiryUnlimited.Enabled = False
        Else
            chkIsExpiryUnlimited.Enabled = True
        End If
        ControlvisibilityForExpiryInfo()
        upnlChangeExpiryInfo.Update()
    End Sub
    Private Sub chkIsExpiryUnlimited_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsExpiryUnlimited.CheckedChanged
        If chkIsExpiryUnlimited.Checked Then
                 chkIsExpiryNA.Checked = False
            chkIsExpiryNA.Enabled = False
        Else
            chkIsExpiryNA.Enabled = True
        End If
        ControlvisibilityForExpiryInfo()
        upnlChangeExpiryInfo.Update()
    End Sub
    Private Sub ControlvisibilityForExpiryInfo()
        If (
          (txtStartDate.Text <> "" Or txtExpiryDate.Text <> "") Or (txtCureQtrs.Text <> "0" And txtCureQtrs.Text <> "") Or _
          (txtCureYear.Text <> "0" And txtCureYear.Text <> "") Or (txtExpQrts.Text <> "0" And txtExpQrts.Text <> "") Or _
          (txtExpYear.Text <> "0" And txtExpYear.Text <> "")
           ) Then
            chkIsExpiryNA.Checked = False
            chkIsExpiryUnlimited.Checked = False
            chkIsExpiryNA.Enabled = False
            chkIsExpiryUnlimited.Enabled = False
        Else
            chkIsExpiryNA.Enabled = True
            chkIsExpiryUnlimited.Enabled = True
        End If
    End Sub 'End
#End Region
#End Region

End Class