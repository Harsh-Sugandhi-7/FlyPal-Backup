'Added by Vikrant

Partial Class wfAlternatePart_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
    Public ItemList As ItemList
    Public mAltTypeList As AltTypeList
    Public AltType As Integer
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Public AlternateType As Int32 = 0 'Added BY Vikrant on 28-03-2012
    Dim IsReturnedFromPartList As String
    Dim mItems As Items
    Dim Type As Int16
    Dim mUnitListForConverter As UnitListForConverter

    Public ModuleName As String = "AlternatePart"

#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(sender As System.Object, e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Business Methods "
    Private Overloads Sub setFocus(cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        ' str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        str = "try{document.getElementById('" + cntrl.ClientID + "').focus();}catch (Error) {}"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
    Private Sub GetSession()
        IsReturnedFromPartList = Session("IsReturnedFromPartList") ''
        AltType = Val(Request.QueryString("AltType"))
        'Commented & Added By Vikrant For AJAX Implementation
        'If AltType <> 1 Then
        '    mItem = Session("mAltItem")
        '    AltType = 0
        'End If
        mItem = Session("mAltItem")
        'End
        mAltTypeList = Session("mAltTypeList")
        AlternateType = Session("AlternateType") ''
        '------------Added BY Vikrant on 28-03-2012--------------------------------
        If (AlternateType = 3 Or AlternateType = 4 Or AlternateType = 5) And Session("IsReturnedFromPartList") = "False" Then
            mItem = Session("mItem")
        End If
        '--------------------------------------------------------------------------
        mItems = Session("mItems")
        Type = Session("Type")
    End Sub
    Private Sub SetSession()
        Session("mAltItem") = mItem
        Session("mAltTypeList") = mAltTypeList
    End Sub
    Private Sub SetPage()
        mItem = Session("mAltItem")
        txtSearchPart.ReadOnly = (cmbLookIn.SelectedIndex = 0)
        txtSearchPart.BackColor = IIf(cmbLookIn.SelectedIndex = 0, Color.Silver, Color.White)
        lblResult.Text = "List of alternate parts For : " + mItem.Name
        If Not mItem.IsNew Then
            lblTitle.Text = "Alternate Part For [" + mItem.Name + "]"
        End If
    End Sub
    Private Sub NewRecord()
        mItem = Item.NewItem()
        Session("mAltItem") = mItem
    End Sub
    Private Sub DeleteRecord(Index As Int32)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfAlternatePart_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
        'Session("sender") = "Delete"
        'msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mItem.AlternatePartNos.CurrentIndex = Index
        Session("mAltItem") = mItem
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

                            Session("sender") = ""
                            mItem = Session("mAltItem")
                            mItem.AlternatePartNos.Remove(mItem.AlternatePartNos.CurrentItem)
                            Session("mAltItem") = mItem
                            Session("mItem") = mItem ''
                            Session("DoNotSelectAgain") = "DoNotSelectAgain" ''
                            ControlVisibility()
                            upnlSelectPart.Update()
                            upnlGridView.Update()

                        Catch ex As SqlException

                            If ex.Number = 8145 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.ProcedureError,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 2627 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.Duplicate,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 547 Then

                                MarkLog(Action.Delete, "Alternate Part",
                                        "Can't delete : " & mItem.Name & " This is Currently in use ",
                                        ErrorType.NoError,
                                        mItem.ID,
                                        EventLogID)

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                                MSGBox.Message_text.ReferenceDelete,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            End If

                            DataFieldBind()
                            msgCount = ex.Errors.Count

                        Finally

                            If msgCount = 0 Then

                                DataFieldBind()
                                MarkLog(Action.Delete,
                                        "Alternate Part",
                                        mItem.Name,
                                        ErrorType.NoError,
                                        mItem.ID,
                                        EventLogID)

                            End If

                        End Try

                    End If

                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok 'Code Added

                    Session("sender") = ""
                    DataFieldBind() ''CHK For all OkOnly MsgBox

                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added

                    Session("sender") = ""
                    DataFieldBind() 'CHK For all OkOnly ans Sender Authorization  MsgBox

            End Select

        ElseIf Result1 = -1 Then

            Session("sender") = ""
            DataFieldBind()

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If

        PreserveStateOfFavIcon()

    End Sub

    '------------Added BY Vikrant on 28-03-2012--------------------------------
    Private Sub ControlVisibility()
        If AlternateType = 3 Or AlternateType = 4 Or AlternateType = 5 Then
            cmbLookIn.Enabled = False
            btnSelect.Enabled = False
        End If
    End Sub
    '--------------------------------------------------------------------------
    Private Sub SetObjectForSearchPartList()
        Dim chkSelect As CheckBox
        Dim Recordno, PageItems As Integer
        ' Dim I As Integer
        PageItems = dgPartList.Rows.Count - 1

        For I As Integer = 0 To PageItems
            Recordno = I + dgPartList.PageSize * dgPartList.PageIndex
            chkSelect = CType(dgPartList.Rows(I).FindControl("chkSelect"), CheckBox)
            mItems(Recordno).IsSelected = chkSelect.Checked
            mItems(Recordno).MarkClean()
        Next
        mUnitListForConverter = UnitListForConverter.GetUnitListForConverter
        For I As Integer = 0 To PageItems 'mItems.Count - 1
            Recordno = I + dgPartList.PageSize * dgPartList.PageIndex
            chkSelect = CType(dgPartList.Rows(I).FindControl("chkSelect"), CheckBox)
            If mItems(Recordno).IsSelected And Not mItems(Recordno).CategoryID.Equals(mItem.CategoryID) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Selected Part " + mItems(Recordno).Name + " Category Is Not Matching With Original Part.  Select Another Part.", MsgBoxStyle.Information, "")
                Exit Sub
            End If
            'If mItems(Recordno).IsSelected And Not mItems(Recordno).UnitID.Equals(mItem.UnitID) Then
            If Not mUnitListForConverter.Contains(BaseUnitID:=mItems(Recordno).UnitID, ConvertUnitID:=mItem.UnitID) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Selected Part " + mItems(Recordno).Name + " Unit/Converted Unit Is Not Matching With Original Part.  Select Another Part.", MsgBoxStyle.Information, "")
                Exit Sub
            End If
            If mItem.SerialisedStatus = True And mItems(Recordno).SerialisedStatus = False Then
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Selected Part Is Not Serialised like Original Part.  Select Another Part.", MsgBoxStyle.Information, "")
                Exit Sub
            End If
            If mItem.SerialisedStatus = False And mItems(Recordno).SerialisedStatus = True Then
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Selected Part Is Serialised but Original Part Is Not Serialised.  Select Another Part.", MsgBoxStyle.Information, "")
                Exit Sub
            End If
            'chkSelect = CType(dgPartList.Items(I).FindControl("chkSelect"), CheckBox)
            'If mItems(Recordno).IsSelected And Not mItems(Recordno).ID.Equals(mItem.ID) Then 'Commented by Prashant 20-Oct-2009
            'Added by Prashant 20-Oct-2009
            If mItems(Recordno).IsSelected And Not mItems(Recordno).ID.Equals(mItem.ID) And Not mItems(Recordno).LinkID.Equals(mItem.LinkID) Then

                If Not mItem.AlternatePartNos.Contains(mItems(Recordno).Name) Then
                    mItem.AlternatePartNos.Add(mItems(Recordno).LinkID, mItem.LinkID)
                    mItem.AlternatePartNos.CurrentItem.PartName = mItems(Recordno).Name
                    mItem.AlternatePartNos.CurrentItem.PartDescription = mItems(Recordno).Description
                    mItem.AlternatePartNos.CurrentItem.AltTypeName = mItems(Recordno).AltTypeName
                    'mItem.AlternatePartNos.CurrentItem.IsSelected = True
                End If

            ElseIf mItems(Recordno).IsSelected And Not mItems(Recordno).IsDirty And Not chkSelect.Checked Then
                For J As Integer = mItem.AlternatePartNos.Count - 1 To 0 Step -1
                    If mItems(Recordno).ID.Equals(mItem.AlternatePartNos(J).AlternatePartID) Then
                        mItem.AlternatePartNos.Remove(mItem.AlternatePartNos(J))
                        Exit For
                    End If
                Next
            End If
        Next
        Session("mAltItem") = mItem

    End Sub
    Private Sub ClearControls()
        cmbLookIn.SelectedIndex = 0
        txtSearchPart.Text = ""
        txtSearchPart.ReadOnly = True
        txtSearchPart.BackColor = Color.Silver

        cmbOptions.SelectedIndex = 0
        txtNewPart.ReadOnly = True
        cmbAltType.SelectedIndex = 0
        cmbAltType.Enabled = False
        txtNewPart.BackColor = Color.Silver
        txtNewPart.Text = ""
    End Sub
    Private Sub ReloadPageAfterModalPopUpClose()
        ClearControls()
        DataFieldBind()
        SetPage()
        ControlVisibility()
        upnlTitle.Update()
        upnlGridView.Update()
        upnlSelectPart.Update()
        upnlSelectedPart.Update()
        upnlAlternatePart.Update()
    End Sub
    Private Sub ShowSearchPartList(SearchText As String)

        If txtPartNoSearchPartList.Enabled = True Then
            setFocus(txtPartNoSearchPartList)
        End If

        If (cmbLookIn.SelectedIndex = 0 Or cmbLookIn.SelectedIndex = 1) Then

            lblPartNo.Text = "Part No."
            DataFieldBindForSearchPartList(IIf(cmbLookIn.SelectedIndex = 0,
                                                          1,
                                                          cmbLookIn.SelectedIndex),
                                           SearchText,
                                           "")

        Else

            lblPartNo.Text = "Description"
            DataFieldBindForSearchPartList(IIf(cmbLookIn.SelectedIndex = 0,
                                                          1,
                                                          cmbLookIn.SelectedIndex),
                                           "",
                                           SearchText)

        End If

        ControlVisibilityForSearchPartList()

        lblResultSearchPartList.Text = "List of Parts : " & mItems.Count & " Record(s) found."

    End Sub

    Private Sub DataFieldBindForSearchPartList(Optional LookInTypeId As Int16 = 0,
                                               Optional Name As String = "",
                                               Optional Description As String = "")

        mItems = Flypal.Items.GetItems(LookInTypeId,
                                       Name,
                                       Description,
                                       "",
                                       "",
                                       "",
                                       "")

        Session("mItems") = mItems

        If Type = 2 Then
            setSelected()
        End If

        dgPartList.DataSource = mItems
        txtPartNoSearchPartList.Text = Name
        dgPartList.PageIndex = 0
        dgPartList.DataBind()
        txtPartNoSearchPartList.DataBind()
        upnlSearchPartList.Update()

    End Sub
    Private Sub setSelected()


        For i As Integer = 0 To mItems.Count - 1
            For j As Integer = 0 To mItem.AlternatePartNos.Count - 1
                If mItems(i).ID.Equals(mItem.AlternatePartNos(j).AlternatePartID) Then
                    mItems(i).IsSelected = True
                    mItems(i).MarkClean()
                    Exit For
                End If
            Next
        Next

        Session("mAltItem") = mItem
    End Sub
    Private Sub ControlVisibilityForSearchPartList()
        btnOk.Visible = IIf(Type = 2, True, False)
        dgPartList.Columns(1).Visible = IIf(Type = 2, True, False)
        dgPartList.Columns(5).Visible = IIf(Type = 1, True, False)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgAlternatePartList.DataSource = mItem.AlternatePartNos
        Session("mAltItem") = mItem
        mAltTypeList = AltTypeList.GetAltTypeList(True)
        Session("mAltTypeList") = mAltTypeList
        cmbAltType.DataSource = mAltTypeList

        DataBind()
    End Sub
    'Commented By Prashant 20-Oct-2009
    'Public Sub CustomValidate( s As Object,  e As ServerValidateEventArgs)
    '    Dim custValidator As CustomValidator
    '    custValidator = CType(s, CustomValidator)
    '    If custValidator.ControlToValidate = "cmbOptions" Then
    '        If cmbOptions.SelectedIndex <= 0 Then
    '            custValidator.ErrorMessage = "Select Option from the list."
    '            e.IsValid = False
    '        ElseIf (cmbOptions.SelectedIndex = 1) And ((txtNewPart.Text.Trim = "") Or (cmbAltType.SelectedItem.Text = "<SELECT>")) Then
    '            custValidator.ErrorMessage = "Enter the New Part no. and Part type."
    '            e.IsValid = False
    '        End If
    '    End If
    'End Sub
    '--------------------------------
    'Added By Prashant 20-Oct-2009
    Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbOptions" Then
            If cmbOptions.SelectedIndex <= 0 Then
                If Session("DoNotSelectAgain") <> "DoNotSelectAgain" Then
                    custValidator.ErrorMessage = "Select Option from the list."
                    e.IsValid = False
                ElseIf Session("DoNotSelectAgain") = "DoNotSelectAgain" Then
                    Session.Remove("DoNotSelectAgain")
                End If
            ElseIf (cmbOptions.SelectedIndex = 1) And ((txtNewPart.Text.Trim = "") Or (cmbAltType.SelectedItem.Text = "<SELECT>")) Then
                custValidator.ErrorMessage = "Enter the New Part no. and Part type."
                e.IsValid = False
            End If
        End If
        'If custValidator.ControlToValidate = "txtPartNo" Then
        '    If (txtPartNo.Text.Trim = "") Then
        '        custValidator.ErrorMessage = "Click on Select Part button to Select Part"
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'End If
    End Sub
    '-------------------------------------------------------------------------------
    Private Function customeValidate1() As Boolean
        rfvName.IsValid = Not (txtPartNo.Text.Trim = "")
        cvOptions.IsValid = (mItem.AlternatePartNos.Count > 0)
        Return rfvName.IsValid And cvOptions.IsValid
    End Function
#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011

        If Not IsPostBack And Session("sender") = "" Then

            If cmbLookIn.Enabled = True Then
                setFocus(cmbLookIn)
            End If

            If IsNothing(mItem) Then
                NewRecord()
            End If

            If AlternateType = 3 Then
                Session("MiddleFrame") = "wfReceiptCumInvoiceList_Ajax.aspx?"
            ElseIf AlternateType = 4 Then
                Session("MiddleFrame") = "wfReceiptList_Ajax.aspx?"
            ElseIf AlternateType = 5 Then
                Session("MiddleFrame") = "wfPurchaseOrderList_Ajax.aspx?OrderType=1"
            Else
                Session("MiddleFrame") = "wfAlternatePart_Ajax.aspx?"
            End If

            DataFieldBind()
            SetPage()
            ControlVisibility()

            'Added by Harsh on 15th July 2024 for FLYPAL 1757
            PreserveStateOfFavIcon()

        End If

    End Sub
    Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click
        Session("mAltItem") = mItem
        'AJAX Dim str As String
        'str = "openledgersame('wfSearchPartListForAlternatePart.aspx?BackPage=Index.aspx&LookinTypeId=" & cmbLookIn.SelectedValue & "&Name=" & txtSearchPart.Text & "&Type=1');"
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        mdlPopUpSearchPartListForAlternatePart.Show()
        Type = 1
        Session("Type") = Type

        ShowSearchPartList(txtSearchPart.Text.Trim)
        upnlSearchPartList.Update()
    End Sub
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then
            'setObject()
            SetSession()
            MarkLog(Action.[New], "Alternate Part", "Not Authorized User", ErrorType.HandledError, Guid.Empty, EventLogID)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfAlternatePart_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        MarkLog(Action.[New], "Alternate Part", "", ErrorType.NoError, mItem.ID, EventLogID)
        If IsValid Then
            If IsNothing(mItem) Then Exit Sub
            Dim Index As Int16 = cmbOptions.SelectedIndex
            Select Case Index
                Case 0
                    Exit Sub
                Case 1
                    Try
                        Dim mAlternateItem As Item = Item.GetAlternateItem(mItem.ID)
                        mAlternateItem.Name = txtNewPart.Text.Trim
                        If cmbAltType.SelectedIndex > 0 Then
                            mAlternateItem.AltTypeID = Val(cmbAltType.SelectedValue)
                        End If
                        mAlternateItem.AsOnDate = DBNull.Value
                        mAlternateItem.AttachFile = Nothing
                        mAlternateItem.Size = 0
                        mAlternateItem.FileExtension = String.Empty
                        mAlternateItem.ApplyEdit()
                        If mItem.AlternatePartNos.Contains(mAlternateItem.Name) Then
                            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Alternate Item", MsgBoxStyle.OKOnly)
                            'msg1.ReplacePage = "wfAlternatePart_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
                            'msg1.Show()
                            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Alternate Item", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        mAlternateItem = mAlternateItem.Save
                        'mItem.AlternatePartNos.Add(mAlternateItem.LinkID, mItem.LinkID)
                        'mItem.AlternatePartNos.CurrentItem.PartDescription = mAlternateItem.Description
                        'mItem.AlternatePartNos.CurrentItem.PartName = mAlternateItem.Name
                        'mItem.AlternatePartNos.CurrentItem.IsSelected = True
                        mItem = Item.GetItem(mItem.ID)
                        Session("mAltItem") = mItem
                        DataFieldBind()
                        txtNewPart.Text = ""
                        cmbOptions.SelectedIndex = 0
                        txtNewPart.ReadOnly = Not cmbOptions.SelectedIndex = 1
                        cmbAltType.Enabled = cmbOptions.SelectedIndex = 1
                        txtNewPart.BackColor = IIf(cmbOptions.SelectedIndex = 1, Color.White, Color.Silver)
                        upnlGridView.Update()
                    Catch ex As SqlException
                        If ex.Number = 8145 Then
                            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                            'msg1.ReplacePage = "wfAlternatePart_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
                            'Session("sender") = "Delete"
                            'msg1.Show()
                            MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                        ElseIf ex.Number = 2627 Then
                            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                            'msg1.ReplacePage = "wfAlternatePart_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
                            'Session("sender") = "Delete"
                            'msg1.Show()
                            MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                        ElseIf ex.Number = 547 Then
                            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                            'msg1.ReplacePage = "wfAlternatePart_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
                            'Session("sender") = "Delete"
                            'msg1.Show()
                            MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                        End If
                    End Try
                Case 2
                    Session("mAltItem") = mItem
                    'Dim str As String
                    'If AlternateType = 3 Then
                    '    str = "openledgersame('wfSearchPartListForAlternatePart.aspx?BackPage=wfReceiptCumInvoice.aspx&ChildPage1=wfAlternatePartListForRCI.aspx&BackPage1=wfAlternatePart_Ajax.aspx&LookinTypeId=1&Name=" & txtNewPart.Text & "&Type=2&AlternateType=3');"
                    'ElseIf AlternateType = 4 Then
                    '    str = "openledgersame('wfSearchPartListForAlternatePart.aspx?BackPage=wfReceipt.aspx&ChildPage=wfReceiptItem.aspx&ChildPage1=wfAlternatePOPartList.aspx&BackPage1=wfAlternatePart_Ajax.aspx&Type=2&AlternateType=4');"
                    'Else
                    '    str = "openledgersame('wfSearchPartListForAlternatePart.aspx?BackPage=Index.aspx&LookinTypeId=1&Name=" & txtNewPart.Text & "&Type=2');"
                    'End If
                    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)

                    mdlPopUpSearchPartListForAlternatePart.Show()
                    Type = 2
                    Session("Type") = Type
                    ShowSearchPartList(txtNewPart.Text.Trim)
            End Select
        End If
    End Sub
    Private Sub cmbOptions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbOptions.SelectedIndexChanged
        If cmbOptions.SelectedIndex = 0 Then
            txtNewPart.ReadOnly = True
            cmbAltType.SelectedIndex = 0
            cmbAltType.Enabled = False
            txtNewPart.BackColor = Color.Silver

            txtNewPart.Text = ""
        ElseIf cmbOptions.SelectedIndex = 1 Then
            txtNewPart.ReadOnly = False
            cmbAltType.SelectedIndex = 3
            cmbAltType.Enabled = True
            txtNewPart.BackColor = Color.White

            txtNewPart.Text = ""
        ElseIf cmbOptions.SelectedIndex = 2 Then
            txtNewPart.ReadOnly = False
            cmbAltType.SelectedIndex = 0
            cmbAltType.Enabled = False
            txtNewPart.BackColor = Color.White
            txtNewPart.Text = ""
        End If
        If cmbOptions.Enabled = True Then
            setFocus(cmbOptions)
        End If
    End Sub
    Private Sub cmbLookIn_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLookIn.SelectedIndexChanged
        txtSearchPart.Text = ""
        SetPage()
        If cmbLookIn.Enabled = True Then
            setFocus(cmbLookIn)
        End If
    End Sub
    Private Sub GridView_AlternatePartList_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgAlternatePartList.RowCommand

        Select Case e.CommandName
            Case "Remove"

                If (Not User.IsInRole("PartDelete")) Then

                    SetSession()
                    MarkLog(Action.Delete,
                            "Alternate part",
                            User.Identity.Name & " is not Authorized User to save ",
                            ErrorType.HandledError,
                            Guid.Empty,
                            EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")
                    Exit Sub

                End If

                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 13-Jan-2023
                Dim Index As Int32 = gvr.RowIndex
                DeleteRecord(Index)

        End Select

        PreserveStateOfFavIcon()

    End Sub
    Private Sub SaveRecord(sender As Object, e As EventArgs) Handles btnSave.Click

        If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then

            SetSession()
            MarkLog(Action.Save,
                    "Alternate part",
                    User.Identity.Name & " is not Authorized User to save ",
                    ErrorType.HandledError,
                    Guid.Empty,
                    EventLogID)

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                            MSGBox.Message_text.Authorization,
                            "",
                            MsgBoxStyle.OkOnly,
                            "Authorization")
            Exit Sub

        End If

        If IsValid Then

            Try

                mItem.Save()
                MarkLog(Action.Save,
                        "Alternate Part",
                        mItem.Name,
                        ErrorType.HandledError,
                        mItem.ID,
                        EventLogID)

                lblTitle.Text = "Alternate Part (Saved...)"
                upnlTitle.Update()
                Session("mAltItem") = mItem

            Catch ex As SqlException

                If ex.Number = 8145 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.ProcedureError,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "Delete")

                ElseIf ex.Number = 2627 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.Duplicate,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "Delete")

                ElseIf ex.Number = 547 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                    MSGBox.Message_text.ReferenceDelete,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "Delete")

                End If

            End Try

        End If

        PreserveStateOfFavIcon()

    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        MarkLog(Action.Close, "Alternate Part", "", ErrorType.NoError, Guid.Empty, EventLogID)
        'Session.Remove("mItem")
        mItem = Session("mAltItem") ''
        Session.Remove("mAltItem")
        Session.Remove("IsReturnedFromPartList") ''
        If AlternateType = 3 Then ''
            Session("mItem") = mItem
            Session.Remove("AlternateType")
            'Dim URLForAlternateType3 As String = Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
            Response.Redirect("wfAlternatePartListForRCI_Ajax.aspx?BackPage=wfReceiptcumInvoiceItem_Ajax.aspx")
        ElseIf AlternateType = 4 Then
            Session("mItem") = mItem
            Session.Remove("AlternateType")
            Response.Redirect("wfAlternatePOPartList_Ajax.aspx?BackPage=wfReceipt_Ajax.aspx&ChildPage=wfReceiptItem_Ajax.aspx&mType=1&OpenFrom=1")
        ElseIf AlternateType = 5 Then
            Session("mItem") = mItem
            Session.Remove("AlternateType")
            Response.Redirect("wfAlternatePartListForOrder_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx&ChildPage=wfOrderItem_Ajax.aspx")
        Else
            Session.Remove("AlternateType")
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        End If
    End Sub
    'Added By Prashant 22-June-2009 for grid sorting
    Private Sub GridView_AlternatePartList_Sorting(source As Object, e As GridViewSortEventArgs) Handles dgAlternatePartList.Sorting

        Try

            mItem.AlternatePartNos.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
            dgAlternatePartList.DataSource = mItem.AlternatePartNos
            dgAlternatePartList.DataBind()

            PreserveStateOfFavIcon()

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    '-------------------------------------------------
    Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'Part List OK Button
    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        SetObjectForSearchPartList()
        Session.Remove("mItems")
        Session.Remove("Type")
        Session("IsReturnedFromPartList") = "True" ''
        Session("DoNotSelectAgain") = "DoNotSelectAgain"  'Added By Prashant 20-Oct-2009
        mdlPopUpSearchPartListForAlternatePart.Hide()
        ReloadPageAfterModalPopUpClose()
    End Sub
    Private Sub btnCloseSearchPartList_Click(sender As Object, e As EventArgs) Handles btnCloseSearchPartList.Click
        Session.Remove("Type")
        Session.Remove("mItems")
        mdlPopUpSearchPartListForAlternatePart.Hide()
        ReloadPageAfterModalPopUpClose()
    End Sub
    Private Sub GridView_PartList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgPartList.PageIndexChanging

        Try

            dgPartList.PageIndex = e.NewPageIndex
            dgPartList.DataSource = mItems
            Session("mItems") = mItems
            dgPartList.DataBind()

            PreserveStateOfFavIcon()

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    Private Sub GridView_PartList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgPartList.RowCommand

        Select Case e.CommandName
            Case "Select"

                Dim mId As New Guid
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartList.PageIndex * dgPartList.PageSize

                mId = mItems(Index).ID
                mItem = Item.GetItem(mId)
                Session("mAltItem") = mItem
                Session.Remove("mItems")
                mdlPopUpSearchPartListForAlternatePart.Hide()
                ReloadPageAfterModalPopUpClose()

        End Select

        PreserveStateOfFavIcon()

    End Sub

    Private Sub FindRecord(sender As Object, e As EventArgs) Handles btnFindNow.Click

        If cmbLookIn.SelectedIndex = 0 Then

            DataFieldBindForSearchPartList(1,
                                           txtPartNoSearchPartList.Text,
                                           "")

        Else

            DataFieldBindForSearchPartList(cmbLookIn.SelectedIndex,
                                           txtPartNoSearchPartList.Text,
                                           txtPartNoSearchPartList.Text)

        End If

        ControlVisibilityForSearchPartList()
        lblResultSearchPartList.Text = "List of Parts : " & mItems.Count & " Record(s) found."

    End Sub

    'Added by Harsh on 15th July 2024 for FLYPAL 1757
    Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click

        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, ModuleName)
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click

        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, ModuleName)
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub PreserveStateOfFavIcon()

        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, ModuleName) Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "Mark As Favourite",
                                                "MarkAsFavourite();",
                                                True)

        Else

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "Remove From Favourite",
                                                "RemoveFromFavourite();",
                                                True)

        End If

    End Sub
    'End

#End Region

End Class
