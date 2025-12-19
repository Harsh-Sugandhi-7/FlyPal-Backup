'Created By Utkarsh On 07-Nov-2013

Public Class wfAlternatePartChild_Ajax
    Inherits System.Web.UI.Page

#Region " Variables and Declarations "
    Public mItem As Item
    Public mAltTypeListForAltPart As AltTypeList
    Dim EventLogID As Guid 'Added By Utkarsh On 19-Jul-2011 For All19072011
    Dim mItemName As String 'Added By Utkarsh On 19-Jul-2011 For All19072011
    Dim mAlternateItems As Items
    Dim mAltItem As Item
    Dim PartTypeId As Integer = 0
    Dim partType As String = String.Empty
    Dim mUnitListForConverter As UnitListForConverter
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mItem = Session("mItem")
        mAlternateItems = Session("mAlternateItems")
        mAltItem = Session("mAltItem")
        mAltTypeListForAltPart = Session("mAltTypeListForAltPart")
        PartTypeId = Session("PartTypeId")
        partType = Session("partType")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAlternateItems")
        Session.Remove("mAltItem")
        Session.Remove("mAltTypeListForAltPart")
        Session.Remove("PartTypeId")
        Session.Remove("partType")
    End Sub
    Private Sub SetSession()
        Session("mItem") = mItem
        Session("mAlternateItems") = mAlternateItems
        Session("mAltItem") = mAltItem
        Session("mAltTypeListForAltPart") = mAltTypeListForAltPart
        Session("PartTypeId") = PartTypeId
    End Sub
    Private Sub SetPage()
        If Not mItem.IsNew Then
            lblTitle.Text = "Part Information [" + mItem.Name + "]"
        End If
        upnlTitle.Update()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SearchAltPart(ByVal index As Integer)
        mAltItem = Nothing
        Session("mAltItem") = mAltItem
        mAlternateItems = Nothing
        Session("mAlternateItems") = mAlternateItems
        ControlVisibilityForSearchAltPart(index)
        If index = 2 Then
            ' mAlternateItems = Flypal.Items.GetItems(1, txtAlternatePart.Text.Trim, "", "", "", "", "")
            mAlternateItems = Flypal.Items.GetItems(4, txtAlternatePart.Text.Trim, "", "", mItem.CategoryName, "", "")
            gdvPartList.DataSource = mAlternateItems
            gdvPartList.DataBind()
            Session("mAlternateItems") = mAlternateItems
        End If
        upnlSearchAlt.Update()
    End Sub
    Private Sub DeleteAlternatePart(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "AlternateDelete")
        mItem.AlternatePartNos.CurrentIndex = Index
        Session("mItem") = mItem
    End Sub
    Private Sub setPartTypeComboID(Optional ByVal OnPageLoad As Boolean = False)
        If PartTypeValue.Value = String.Empty Then
            If OnPageLoad Then
                PartTypeId = 0
                Session("PartTypeId") = PartTypeId
            Else
                PartTypeId = Session("PartTypeId")
            End If
        Else
            PartTypeId = CInt(PartTypeValue.Value)
            Session("PartTypeId") = PartTypeId
        End If

        If PartTypeName.Value = String.Empty Then
            If OnPageLoad Then
                partType = ""
                Session("partType") = partType
            Else
                partType = Session("partType")
            End If
        Else
            partType = PartTypeName.Value
            Session("partType") = partType
        End If
    End Sub
    Private Sub ClearComboBoxValues()
        PartTypeId = 0
        PartTypeValue.Value = ""
        Session("PartTypeId") = PartTypeId
        PartTypeName.Value = ""
        partType = ""
        Session("partType") = partType
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "AlternateDelete" Then
                        Session("sender") = ""

                        Dim msgCount As Integer = 0
                        Dim mAltPartName As String = ""
                        Dim mAltPartID As Guid = Guid.Empty
                        Try
                            mAltPartName = mItem.AlternatePartNos.CurrentItem.PartName
                            mAltPartID = mItem.AlternatePartNos.CurrentItem.AlternatePartID

                            mItem = Session("mItem")
                            mItem.AlternatePartNos.Remove(mItem.AlternatePartNos.CurrentItem)
                            Session("mItem") = mItem
                            DataFieldBind()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.Information, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.Information, "")
                            End If
                            DataFieldBind()
                            msgCount = 0
                            'Commented By Utkarsh On 20-Jul-2011 For All19072011
                            'Finally
                            '    If msgCount = 0 Then

                            '        MarkLog(Util.Action.Delete, "AlternatePart", mAltPartName, Util.ErrorType.NoError, mAltPartID)

                            '    End If
                            'End
                        End Try
                    End If
                Case MsgBoxResult.No
                    If Session("sender") = "AlternateDelete" Then
                        Session("sender") = ""
                        DataFieldBind()
                    End If
                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfAlternatePartChild.aspx?BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "SaveNEW"
                    Session("sender") = ""
                    txtAlternatePart.Text = Session("mAltPartNo").ToString
                    cmbAltTypeList.SelectedValue = Val(Session("mAltPartTypeID"))

                    DataFieldBind()
                    'Response.Redirect("wfAlternatePartChild.aspx?BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "SaveEXISTING"
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.Ok And (MSGBoxCtrl.Sender <> "SaveNEW" And MSGBoxCtrl.Sender <> "SaveEXISTING")
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfAlternatePartChild.aspx?BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfAlternatePartChild.aspx?BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 Then
            Session("sender") = ""
            If Not Session("mAltPartNo") Is Nothing Then
                txtAlternatePart.Text = Session("mAltPartNo").ToString
                cmbAltTypeList.SelectedValue = Val(Session("mAltPartTypeID"))

                Session.Remove("mAltPartNo")
                Session.Remove("mAltPartTypeID")
            End If
        End If
    End Sub
    Private Sub ControlVisibilityForSearchAltPart(ByVal index As Integer)
        If index = 1 Then
            lblAltFindPartNo.Visible = True
            txtAlternatePart.Visible = True
            gdvPartList.Visible = False
            txtAlternatePart.Enabled = True
            lblAltTypeList.Visible = True
            cmbAltTypeList.Visible = True
            ' btnFindNow.Visible = False
            btnSearch.Visible = False
            cmbAltTypeList.Visible = True
            btnAlternatePart.Visible = True
            lblAltInfo.Visible = True
            lblAltInfo.Text = "Select Part Type, enter Part No. to add new Part and press Add button."
        ElseIf index = 2 Then
            lblAltFindPartNo.Visible = True
            txtAlternatePart.Visible = True
            lblAltTypeList.Visible = False
            cmbAltTypeList.Visible = False
            gdvPartList.Visible = True
            txtAlternatePart.Enabled = True
            '  btnFindNow.Visible = True
            btnSearch.Visible = True
            cmbAltTypeList.Visible = False
            btnAlternatePart.Visible = True
            lblAltInfo.Visible = True
            lblAltInfo.Text = "Enter Part No. to find Part and press FindNow button.Select Alternate Part form the list below."
        End If
    End Sub
#End Region

#Region "DataField Bind"
    Private Sub DataFieldBind()
        gdvAlternatePartList.DataSource = mItem.AlternatePartNos
        gdvAlternatePartList.DataBind()
        upnlGrid.Update()
    End Sub
    Private Sub LoadTypeCombo(Optional ByVal FetchFromDatabase As Boolean = False)
        If FetchFromDatabase Then
            mAltTypeListForAltPart = AltTypeList.GetAltTypeList(True)
            Session("mAltTypeListForAltPart") = mAltTypeListForAltPart
        End If
        cmbAltTypeList.DataSource = mAltTypeListForAltPart
        cmbAltTypeList.DataBind()
        cmbAltTypeList.SelectedValue = PartTypeId
    End Sub

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 19-Jul-2011 For All19072011
        If Not IsPostBack And Session("sender") = "" Then
            If cmbOptions.Enabled = True Then
                setFocus(cmbOptions)
            End If
            DataFieldBind()
            setPartTypeComboID(True)
            LoadTypeCombo(True)
            SetPage()
            SearchAltPart(cmbOptions.SelectedValue)
            'If Session("PartInfo") = "True" Then 'Added by Prashant 22-Aug-2018 ALL22082018
            '    btnOpeningStock.Visible = False
            '    btnApplicability.Visible = False
            'End If
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gdvPartList.PageIndex = 0
        SearchAltPart(cmbOptions.SelectedValue)
    End Sub
    Protected Sub txtAlternatePart_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        gdvPartList.PageIndex = 0
        SearchAltPart(cmbOptions.SelectedValue)
    End Sub
    Private Sub gdvPartList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvPartList.PageIndexChanging
        gdvPartList.PageIndex = e.NewPageIndex
        gdvPartList.DataSource = mAlternateItems
        gdvPartList.DataBind()
        ControlVisibilityForSearchAltPart(cmbOptions.SelectedValue)
        setPartTypeComboID()
        LoadTypeCombo()
    End Sub
    Private Sub gdvPartList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvPartList.RowCommand
        Select Case e.CommandName
            Case "Select"
                ControlVisibilityForSearchAltPart(cmbOptions.SelectedValue)
                mAlternateItems = Session("mAlternateItems")
                Dim index As Integer = CInt(e.CommandArgument) + gdvPartList.PageIndex * gdvPartList.PageSize
                Dim mAlterNateItemID As Guid = mAlternateItems(index).ID
                Dim mAltItem As Item = Item.GetAlternateItem(mAlterNateItemID)
                Session("mAltItem") = mAltItem
                gdvPartList.Visible = False
                txtAlternatePart.Text = mAltItem.Name
                txtAlternatePart.Enabled = False
                setPartTypeComboID()
                LoadTypeCombo()
        End Select
    End Sub

    Private Sub gdvAlternatePartList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvAlternatePartList.RowCommand
        Select Case e.CommandName
            Case "Remove"
                Dim Index As Int16 = CInt(e.CommandArgument) + gdvAlternatePartList.PageSize * gdvAlternatePartList.PageIndex
                If (Not User.IsInRole("PartDelete")) Then
                    SetSession()
                    'Commented By Utkarsh On 19-Jul-2011 For All19072011
                    'MarkLog(Util.Action.Delete, "AlternatePart", User.Identity.Name & " is not Authorized User to Delete Part : " & mItemName & " " & "AlternatePart : " & mAltName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
                    Exit Sub
                End If
                DeleteAlternatePart(Index)
                DataFieldBind()
        End Select
    End Sub

    Protected Sub btnAlternatePart_Click(sender As Object, e As EventArgs) Handles btnAlternatePart.Click
        If cmbOptions.SelectedValue = 0 Then        '<SELECT>       Add nothing
            '
        ElseIf cmbOptions.SelectedValue = 1 Then    'NEW            Create New Part and also add to Collection
            setPartTypeComboID()
            mAlternateItems = Flypal.Items.GetItems(4, txtAlternatePart.Text.Trim, "", "", mItem.CategoryName, "", "")
            If Not mAlternateItems.Contains(txtAlternatePart.Text.Trim) Then
                Try
                    If Len(txtAlternatePart.Text.Trim) <> 0 And PartTypeId <> 0 Then

                        Dim mAltItem As Item = Item.GetAlternateItem(mItem.ID)

                        'mAltItem = Item.NewItem()

                        mAltItem.Name = txtAlternatePart.Text.Trim
                        mAltItem.AltTypeID = PartTypeId
                        mAltItem.AsOnDate = System.DBNull.Value
                        mAltItem.AttachFile = Nothing
                        mAltItem.Size = 0
                        mAltItem.FileExtension = String.Empty
                        'mAltItem.LinkID = mItem.LinkID

                        'mAltItem.Description = mItem.Description
                        'mAltItem.UnitID = mItem.UnitID
                        'mAltItem.NomenclatureID = mItem.NomenclatureID
                        'mAltItem.CategoryID = mItem.CategoryID
                        'mAltItem.Location = mItem.Location
                        'mAltItem.ABCID = mItem.ABCID
                        'mAltItem.Folio = mItem.Folio
                        'mAltItem.ExpiryMonths = mItem.ExpiryMonths
                        'mAltItem.ExpiryQuaters = mItem.ExpiryQuaters
                        'mAltItem.BenchmarkMonths = mItem.BenchmarkMonths
                        'mAltItem.SerialisedStatus = mItem.SerialisedStatus
                        'mAltItem.ValuationStatus = mItem.ValuationStatus
                        'mAltItem.StockStatus = mItem.StockStatus
                        'mAltItem.MinStockLevel = mItem.MinStockLevel
                        'mAltItem.MinReOrderLevel = mItem.MinReOrderLevel
                        'mAltItem.MaxStockLevel = mItem.MaxStockLevel
                        'mAltItem.BinCardNumber = mItem.BinCardNumber
                        'mAltItem.IPCReference = mItem.IPCReference
                        'mAltItem.CalibrationPeriodInID = mItem.CalibrationPeriodInID


                        mAltItem.ApplyEdit()
                        mAltItem = mAltItem.Save

                        'mItem.AlternatePartNos.Add(mAltItem.LinkID, mItem.LinkID)
                        'mItem.AlternatePartNos.CurrentItem.PartName = mAltItem.Name
                        'mItem.AlternatePartNos.CurrentItem.PartDescription = mAltItem.Description
                        'mItem.AlternatePartNos.CurrentItem.AltTypeName = partType
                        mItem = Item.GetItem(mItem.ID)
                    Else
                        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.ValidationAlert, "Please check new Alternate Part No. or its Alternate Part Type specified or not.", MsgBoxStyle.Information, "")
                        ControlVisibilityForSearchAltPart(cmbOptions.SelectedValue)
                        LoadTypeCombo()
                        Exit Sub
                    End If
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.Information, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
                    ElseIf ex.Number = 547 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.Information, "")
                    End If
                    LoadTypeCombo()
                    ControlVisibilityForSearchAltPart(cmbOptions.SelectedValue)
                    Exit Sub
                End Try
            Else
                'mAltItem = mAlternateItems.Item(txtAlternatePart.Text.Trim)
                'mItem.AlternatePartNos.Add(mAltItem.LinkID, mItem.LinkID)
                'mItem.AlternatePartNos.CurrentItem.PartName = mAltItem.Name
                'mItem.AlternatePartNos.CurrentItem.PartDescription = mAltItem.Description
                'mItem.AlternatePartNos.CurrentItem.AltTypeName = mAltItem.AltTypeName
                MSGBoxCtrl.show("Alert", "Entered Part already exists", "Please enter different Part Name", MsgBoxStyle.OkOnly, "")
                ClearComboBoxValues()
                txtAlternatePart.Text = ""
                LoadTypeCombo()
                mAltItem = Nothing
                Session("mAltItem") = mAltItem
                Exit Sub
            End If
            ClearComboBoxValues()
            LoadTypeCombo()
            Session("mItem") = mItem
            txtAlternatePart.Enabled = True
            txtAlternatePart.Text = ""
            DataFieldBind()

            mAltItem = Nothing
            Session("mAltItem") = mAltItem
            cmbOptions.SelectedIndex = 0
            SearchAltPart(cmbOptions.SelectedValue)
        ElseIf cmbOptions.SelectedValue = 2 Then    'EXISTING       Add to Collection    
            setPartTypeComboID()
            mAltItem = Session("mAltItem")
            mUnitListForConverter = UnitListForConverter.GetUnitListForConverter
            If Not mItem.CategoryID.Equals(mAltItem.CategoryID) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Selected Part Category Is Not Matching With Original Part.  Select Another Part.", MsgBoxStyle.Information, "")
                LoadTypeCombo()
                ControlVisibilityForSearchAltPart(cmbOptions.SelectedValue)
                Exit Sub
            End If
            If Not mUnitListForConverter.Contains(BaseUnitID:=mItem.UnitID, ConvertUnitID:=mAltItem.UnitID) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Selected Part Unit Is Not Matching With Original Part Unit/Converted Unit.  Select Another Part.", MsgBoxStyle.Information, "")
                LoadTypeCombo()
                ControlVisibilityForSearchAltPart(cmbOptions.SelectedValue)
                Exit Sub
            End If
            If mItem.SerialisedStatus = True And mAltItem.SerialisedStatus = False Then
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Selected Part Is Not Serialised like Original Part.  Select Another Part.", MsgBoxStyle.Information, "")
                LoadTypeCombo()
                ControlVisibilityForSearchAltPart(cmbOptions.SelectedValue)
                Exit Sub
            End If
            If mItem.SerialisedStatus = False And mAltItem.SerialisedStatus = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Selected Part Is Serialised but Original Part Is Not Serialised.  Select Another Part.", MsgBoxStyle.Information, "")
                LoadTypeCombo()
                ControlVisibilityForSearchAltPart(cmbOptions.SelectedValue)
                Exit Sub
            End If
            If Not mAltItem Is Nothing Then
                If Not mItem.AlternatePartNos.Contains(mAltItem.Name) Then
                    mItem.AlternatePartNos.Add(mAltItem.LinkID, mItem.LinkID)
                    mItem.AlternatePartNos.CurrentItem.PartName = mAltItem.Name
                    mItem.AlternatePartNos.CurrentItem.PartDescription = mAltItem.Description
                    mItem.AlternatePartNos.CurrentItem.AltTypeName = mAltItem.AltTypeName
                    Session("mItem") = mItem
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.ValidationAlert, "Please check new Alternate Part No. or its Alternate Part Type specified or not.", MsgBoxStyle.Information, "")
                    LoadTypeCombo()
                    ControlVisibilityForSearchAltPart(cmbOptions.SelectedValue)
                    Exit Sub
                End If
                txtAlternatePart.Enabled = True
                txtAlternatePart.Text = ""
                DataFieldBind()
                mAltItem = Nothing
                Session("mAltItem") = mAltItem
                cmbOptions.SelectedIndex = 0
                SearchAltPart(cmbOptions.SelectedValue)
                ClearComboBoxValues()
                LoadTypeCombo()
            End If
            ControlVisibilityForSearchAltPart(cmbOptions.SelectedValue)
        End If
    End Sub
    Private Sub gdvAlternatePartList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvAlternatePartList.Sorting
        mItem.AlternatePartNos.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mItem") = mItem
        DataFieldBind()
    End Sub
    Private Sub cmbOptions_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbOptions.SelectedIndexChanged
        SearchAltPart(cmbOptions.SelectedValue)
        setPartTypeComboID()
        LoadTypeCombo()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Session("mItem") = mItem
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub


#End Region

#Region "Navigation"
    'Private Sub btnPartInformation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPartInformation.Click, btnBack.Click
    '    Session("mItem") = mItem
    '    Response.Redirect(Request.QueryString("BackPage"))
    'End Sub
    'Private Sub btnApplicability_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApplicability.Click
    '    mItem.ItemApplicables.Add(mItem.ID)
    '    mItem.ItemApplicables.CurrentItem.SrNo = mItem.ItemApplicables.Count
    '    mItem.ItemApplicables.CurrentItem.ModelName = ""
    '    For i As Integer = 0 To mItem.ItemApplicables.Count - 1
    '        mItem.ItemApplicables(i).SrNo = i + 1
    '    Next
    '    Session("mItem") = mItem

    '    Response.Redirect("wfApplicableFor_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
    'End Sub
    'Private Sub btnOpeningStock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpeningStock.Click
    '    If mItem.ItemApplicables.Count > 0 Then
    '        For i As Integer = 0 To mItem.ItemApplicables.Count - 1
    '            If mItem.ItemApplicables(i).ModelName = "" Then
    '                mItem.ItemApplicables.Remove(mItem.ItemApplicables(i))
    '            End If
    '        Next
    '    End If
    '    Session("mItem") = mItem
    '    Session.Remove("mModelList")

    '    Response.Redirect("wfOpeningBalanceList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
    'End Sub
#End Region


End Class