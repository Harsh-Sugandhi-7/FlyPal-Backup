Public Class wfCalibrationFrequencyChange_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCalibrationItemsListForFrequencyChange As CalibrationItemsListForFrequencyChange
    Public mOpenState As Boolean
    Dim PartNo As String
    Dim Location, SearchIndex, PartType, PartNoLocation As String
    Public mCurrentLocation As String
    Public mReceiptItemID As Guid

    Dim EventLogID As Guid
    Public mPartName As String
    Public mItemID As Guid
    Public mBenchmarkMonths As Integer
    Public mCalibrationPeriodIn As String
    Public mCalibrationPeriodInID As Integer

    Public mCalibrationPeriodInList As CalibrationPeriodInList
    Public SerialNo As String
    Dim mCompanyDetail As New CompanyDetail
    Public mCategoryLists As CategoryList
#End Region

#Region " Helper Methods "
    Private Sub GetSessionForLocation()
        mCurrentLocation = CType(Session("mCurrentLocation"), String)
    End Sub
    Private Sub RemoveSessionForPartStore()
        Session.Remove("mItemTypeList")
        Session.Remove("ChangeItemTypeID")
        Session.Remove("ChangeStore")
        Session.Remove("IsStoreChangeble")
        Session.Remove("ChangeStoreID")
        Session.Remove("ChangeStoreList")
    End Sub
    Private Sub RemoveSessionForLocation()
        Session.Remove("mCurrentLocation")
    End Sub
    Private Sub GetSession()
        mItemID = CType(Session("mItemID"), Guid)
        mPartName = Session("mPartName")
        mBenchmarkMonths = Session("mBenchmarkMonths")
        mCalibrationPeriodIn = Session("mCalibrationPeriodIn")
        mCalibrationPeriodInID = Session("mCalibrationPeriodInID")
        mCalibrationItemsListForFrequencyChange = Session("mCalibrationItemsListForFrequencyChangeForGrid")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemID")
        Session.Remove("mPartName")
        Session.Remove("mBenchmarkMonths")
        Session.Remove("mCalibrationPeriodIn")
        Session.Remove("mCalibrationPeriodInID")
        Session.Remove("mCalibrationItemsListForFrequencyChangeForGrid")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfCalibrationFrequencyChange_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility1(ByVal SearchIndex As Int32)
        txtSearchFor.Visible = True
    End Sub
    Private Sub ClearControls()
        txtSearchFor.Text = ""
    End Sub
    Private Sub ResetValues()
        PartNo = ""
        Location = ""
    End Sub
    Private Sub FindNow(Optional ByVal ItemName As String = "")
        gdPartSearch.DataSource = Nothing
        mCalibrationItemsListForFrequencyChange = Nothing
        'Get List From the Database as per Criteria
        mCalibrationItemsListForFrequencyChange = CalibrationItemsListForFrequencyChange.GetCalibrationItemsListForFrequencyChange(PartNo)
        'Set DataSource of the Grid
        gdPartSearch.DataSource = mCalibrationItemsListForFrequencyChange
        Session("mCalibrationItemsListForFrequencyChangeForGrid") = mCalibrationItemsListForFrequencyChange
    End Sub
    Public Sub SetControl()
        SearchIndex = Session("SearchIndex")
        PartNo = Session("PartNo")
        Location = Session("Location")
        PartType = Session("PartType")

        FindNow(PartNo)
        PartSearchGridBind()

        ControlVisibility1(SearchIndex)
        lblResult.Text = "List of Parts : " & mCalibrationItemsListForFrequencyChange.Count & " Record(s) found. "
    End Sub
    Private Sub PartSearchGridBind()
        gdPartSearch.DataBind()
        upnlgrid.Update()
    End Sub
    Private Sub UpdateSearchPanel()
        upnlSearch.Update()
    End Sub
    Private Sub CalibrationPeriodBind()
        mCalibrationPeriodInList = CalibrationPeriodInList.GetCalibrationPeriodInList("(SELECT)")
        Session("mCalibrationPeriodInList") = mCalibrationPeriodInList
        cmbCalibrationPeriodIn.DataSource = mCalibrationPeriodInList
        cmbCalibrationPeriodIn.DataBind()
    End Sub
    Private Sub BindValueForChangeLocation()
        'txtCurrentLocation.Text = mCurrentLocation
        'If txtChangedLocation.Enabled = True Then
        '    setFocus(txtChangedLocation)
        'End If
        'upnlLocation.Update()
    End Sub
    Private Sub ClearLocationControls()
        'txtChangedLocation.Text = ""
    End Sub
    Private Sub ClearChangePartStoreControls()
        txtBenchmarkMonths.Text = ""
        cmbCalibrationPeriodIn.ClearSelection()
    End Sub
    Private Sub ControlVisibilityForChangePartStore(ByVal enableStore As Boolean)
        'cmbChangeStore.Enabled = IIf(enableStore, True, False)
    End Sub
    'End
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "SaveConfirmation" Then 'Added by Shital on 03-Aug-2021
                        Try
                            CalibrationItemsFrequencyChange()
                            ClearChangePartStoreControls()
                            mdlPopUpChangeCalibrationItemFrequency.Hide()
                            RemoveSessionForPartStore()
                        Catch ex As Exception
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "SaveConfirmation" Then
                        '
                        mdlPopUpChangeCalibrationItemFrequency.Show()
                    End If
                Case MsgBoxResult.Ok

            End Select
        ElseIf Result1 = -1 Then

        ElseIf Result1 = 0 Then   ' 

        End If
    End Sub
    Private Sub CalibrationItemsFrequencyChange()
        CalibrationItemsListForFrequencyChange.ChangeCalibrationItemFrequency(mItemID, Val(txtBenchmarkMonths.Text.Trim),
                                                                                      CInt(cmbCalibrationPeriodIn.SelectedValue))
        MarkLog(Util.Action.Save, "ChangeCalibrationItemFrequency", "Part: " + mPartName + " Old Freq: " + txtCurrentCalibrationItemFrequency.Text +
                " New Freq.: " + txtBenchmarkMonths.Text.Trim + " Old Freq In: " + txtCurrentCalibrationItemFrequencyIn.Text +
                " New Freq. In: " + cmbCalibrationPeriodIn.SelectedItem.Text,
                Util.ErrorType.NoError, Guid.Empty, EventLogID)

        mdlPopUpChangeCalibrationItemFrequency.Hide()
        RemoveSessionForPartStore()
        ClearChangePartStoreControls()
        SetControl()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCalibrationItemsListForFrequencyChange = CalibrationItemsListForFrequencyChange.GetCalibrationItemsListForFrequencyChange("")
        gdPartSearch.DataSource = mCalibrationItemsListForFrequencyChange
        Session("mCalibrationItemsListForFrequencyChangeForGrid") = mCalibrationItemsListForFrequencyChange
        lblResult.Text = "List of Parts : " & mCalibrationItemsListForFrequencyChange.Count & " Record(s) found "
        PartSearchGridBind()
        UpdateSearchPanel()
    End Sub
    Private Sub DataFieldBindForChangePartStore()
        'Dim mItemTypeList As ItemTypeList
        'If Session("mItemTypeList") Is Nothing Then
        '    mItemTypeList = ItemTypeList.GetItemTypeList()
        '    cmbPT.DataSource = mItemTypeList
        '    Session("mItemTypeList") = mItemTypeList
        'Else
        '    cmbPT.DataSource = CType(Session("mItemTypeList"), ItemTypeList)
        'End If
        'cmbPT.DataBind()

        'If Session("ChangeStoreList") Is Nothing Then
        '    mStoreList = StoreList.GetStoreList(0, "", False, True)
        '    cmbChangeStore.DataSource = mStoreList
        '    Session("ChangeStoreList") = mStoreList
        'Else
        '    cmbChangeStore.DataSource = CType(Session("ChangeStoreList"), StoreList)
        'End If
        'cmbChangeStore.DataBind()
    End Sub
    Private Sub addAttributes()
        txtBenchmarkMonths.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtBenchmarkMonths').value,event)")
    End Sub
#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfCalibrationFrequencyChange_Ajax.aspx?"
            CalibrationPeriodBind()
            DataFieldBind()
        End If
    End Sub
    Protected Sub gdPartSearch_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gdPartSearch.RowCommand
        Select Case e.CommandName
            Case "ChangeCalibrationItemFrequency"
                Dim index As Integer = CInt(e.CommandArgument) + gdPartSearch.PageIndex * gdPartSearch.PageSize
                mItemID = mCalibrationItemsListForFrequencyChange(index).ItemID
                mPartName = mCalibrationItemsListForFrequencyChange(index).ItemName
                mBenchmarkMonths = mCalibrationItemsListForFrequencyChange(index).BenchmarkMonths
                mCalibrationPeriodIn = mCalibrationItemsListForFrequencyChange(index).CalibrationPeriodIn
                mCalibrationPeriodInID = mCalibrationItemsListForFrequencyChange(index).CalibrationPeriodInID

                Session("mItemID") = mItemID
                Session("mPartName") = mPartName
                Session("mBenchmarkMonths") = mBenchmarkMonths
                Session("mCalibrationPeriodIn") = mCalibrationPeriodIn
                Session("mCalibrationPeriodInID") = mCalibrationPeriodInID

                lblPartNumber.Text = mPartName
                lblPartDescription.Text = mCalibrationItemsListForFrequencyChange(index).ItemDescription
                txtCurrentCalibrationItemFrequency.Text = mBenchmarkMonths
                txtCurrentCalibrationItemFrequencyIn.Text = mCalibrationPeriodIn

                mdlPopUpChangeCalibrationItemFrequency.Show()
                gdPartSearch.DataSource = mCalibrationItemsListForFrequencyChange
                Session("mCalibrationItemsListForFrequencyChangeForGrid") = mCalibrationItemsListForFrequencyChange
                PartSearchGridBind()
                upnlChangeCalibrationItemFrequency.Update()
        End Select
    End Sub
    Private Sub btnSearch_Click(sender As Object, e As ImageClickEventArgs) Handles btnSearch.Click
        gdPartSearch.PageIndex = 0
        PartNo = Trim(txtSearchFor.Text)
        Session("PartNo") = PartNo
        FindNow(PartNo)
        lblResult.Text = "List of Parts : " & mCalibrationItemsListForFrequencyChange.Count & " Record(s) found "
        PartSearchGridBind()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mCalibrationItemsListForFrequencyChange = Nothing
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub gdPartSearch_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdPartSearch.Sorting
        mCalibrationItemsListForFrequencyChange.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCalibrationItemsListForFrequencyChangeForGrid") = mCalibrationItemsListForFrequencyChange
        gdPartSearch.DataSource = mCalibrationItemsListForFrequencyChange
        PartSearchGridBind()
    End Sub
    Protected Sub gdPartSearch_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdPartSearch.PageIndexChanging
        gdPartSearch.PageIndex = e.NewPageIndex
        gdPartSearch.DataSource = mCalibrationItemsListForFrequencyChange
        Session("mCalibrationItemsListForFrequencyChangeForGrid") = mCalibrationItemsListForFrequencyChange
        PartSearchGridBind()
    End Sub
#End Region

#Region "Change Part / Store"
    Protected Sub btnChangePartOk_Click(sender As Object, e As EventArgs) Handles btnChangePartOk.Click
        Try
            If Val(txtBenchmarkMonths.Text) > 0 And cmbCalibrationPeriodIn.SelectedIndex > 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, "Calibration Interval will get change to all serial no of this Part", MsgBoxStyle.YesNo, "SaveConfirmation")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Enter Interval and Period", MsgBoxStyle.OkOnly, "")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnChangePartClose_Click(sender As Object, e As EventArgs) Handles btnChangePartClose.Click
        ClearChangePartStoreControls()
        mdlPopUpChangeCalibrationItemFrequency.Hide()
        RemoveSessionForPartStore()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class