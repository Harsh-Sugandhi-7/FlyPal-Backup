Partial Class wfSearchPartListForAlternatePart
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mItems As Items
    Dim mItem As Item
    Dim mLookInTypeId As Int16
    Dim mName As String
    Dim Type As Int16
    Dim mDescription As String
    Dim AlternateType As Integer 'Added BY Vikrant on 28-03-2012
#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents CheckBox1 As System.Web.UI.WebControls.CheckBox


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
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub GetSession()
        mItems = Session("mItems")
        mItem = Session("mAltItem")
        mLookInTypeId = Session("mLookInTypeId")
        mName = Session("mName")
        Type = Session("Type")
        mDescription = Session("mDescription")
    End Sub
    Private Sub SetSession()
        Session("mItems") = mItems
        Session("mAltItem") = mItem
        Session("mLookInTypeId") = mLookInTypeId
        Session("mName") = mName
        Session("Type") = Type
        Session("mDescription") = mDescription
    End Sub
    Private Sub ControlVisibility()
        btnOk.Visible = IIf(Type = 2, True, False)
        dgPartList.Columns(1).Visible = IIf(Type = 2, True, False)
        dgPartList.Columns(5).Visible = IIf(Type = 1, True, False)
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
    Private Sub DataFieldBind(Optional ByVal LookInTypeId As Int16 = 0, Optional ByVal Name As String = "", Optional ByVal Description As String = "")
        mItems = Flypal.Items.GetItems(LookInTypeId, Name, Description, "", "", "", "")
        dgPartList.DataSource = mItems
        Session("mItems") = mItems
        If Type = 2 Then
            setSelected()
        End If
        DataBind()
    End Sub
    Private Sub SetObject()
        Dim chkSelect As CheckBox
        Dim item As DataGridItem
        Dim Recordno, PageItems As Integer
        ' Dim I As Integer
        PageItems = dgPartList.Items.Count - 1

        For I As Integer = 0 To PageItems
            Recordno = I + dgPartList.PageSize * dgPartList.CurrentPageIndex
            item = dgPartList.Items(I)
            chkSelect = CType(item.FindControl("chkSelect"), CheckBox)
            mItems(Recordno).IsSelected = chkSelect.Checked
            mItems(Recordno).MarkClean()
        Next
        For I As Integer = 0 To PageItems 'mItems.Count - 1
            Recordno = I + dgPartList.PageSize * dgPartList.CurrentPageIndex
            item = dgPartList.Items(I)
            chkSelect = CType(item.FindControl("chkSelect"), CheckBox)
            'chkSelect = CType(dgPartList.Items(I).FindControl("chkSelect"), CheckBox)
            'If mItems(Recordno).IsSelected And Not mItems(Recordno).ID.Equals(mItem.ID) Then 'Commented by Prashant 20-Oct-2009
            'Added by Prashant 20-Oct-2009
            If mItems(Recordno).IsSelected And Not mItems(Recordno).ID.Equals(mItem.ID) And Not mItems(Recordno).LinkID.Equals(mItem.LinkID) Then
                mItem.AlternatePartNos.Add(mItems(Recordno).LinkID, mItem.LinkID)
                mItem.AlternatePartNos.CurrentItem.PartName = mItems(Recordno).Name
                mItem.AlternatePartNos.CurrentItem.PartDescription = mItems(Recordno).Description
                mItem.AlternatePartNos.CurrentItem.AltTypeName = mItems(Recordno).AltTypeName
                'mItem.AlternatePartNos.CurrentItem.IsSelected = True
            ElseIf mItems(Recordno).IsSelected And Not mItems(Recordno).IsDirty And Not chkSelect.Checked Then
                For J As Integer = mItem.AlternatePartNos.Count - 1 To 0 Step -1
                    If mItems(Recordno).ID.Equals(mItem.AlternatePartNos(J).AlternatePartID) Then
                        mItem.AlternatePartNos.Remove(mItem.AlternatePartNos(J))
                        Exit For
                    End If
                Next
            End If
        Next
        'For I As Integer = 0 To dgPartList.Items.Count - 1
        '    chkSelect = CType(dgPartList.Items(I).FindControl("chkSelect"), CheckBox)
        '    If mItems(I).IsSelected And chkSelect.Checked Then
        '        mItems(I).IsSelected = chkSelect.Checked
        '        mItems(I).MarkClean()
        '    ElseIf mItems(I).IsSelected And Not chkSelect.Checked Then
        '        mItems(I).IsSelected = Not chkSelect.Checked
        '        mItems(I).MarkClean()
        '    Else
        '        mItems(I).IsSelected = chkSelect.Checked
        '    End If
        'Next
        'For I As Integer = 0 To mItems.Count - 1
        '    chkSelect = CType(dgPartList.Items(I).FindControl("chkSelect"), CheckBox)
        '    If mItems(I).IsSelected And mItems(I).IsDirty And Not mItems(I).ID.Equals(mItem.ID) Then

        '        mItem.AlternatePartNos.Add(mItems(I).LinkID, mItem.LinkID)
        '        mItem.AlternatePartNos.CurrentItem.PartName = mItems(I).Name
        '        mItem.AlternatePartNos.CurrentItem.PartDescription = mItems(I).Description
        '        mItem.AlternatePartNos.CurrentItem.AltTypeName = mItems(I).AltTypeName
        '        'mItem.AlternatePartNos.CurrentItem.IsSelected = True

        '    ElseIf mItems(I).IsSelected And Not mItems(I).IsDirty And Not chkSelect.Checked Then
        '        For J As Integer = mItem.AlternatePartNos.Count - 1 To 0 Step -1
        '            If mItems(I).ID.Equals(mItem.AlternatePartNos(J).AlternatePartID) Then
        '                mItem.AlternatePartNos.Remove(mItem.AlternatePartNos(J))
        '                Exit For
        '            End If
        '        Next
        '    End If
        'Next
        Session("mAltItem") = mItem
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If txtPartNo.Enabled = True Then
                setFocus(txtPartNo)
            End If

            Dim Name As String = CType(Request.QueryString("Name"), String)
            Dim LookinTypeId As Int16 = Val(Request.QueryString("LookinTypeId"))
            Session("mLookInTypeId") = LookinTypeId
            If LookinTypeId = 1 Or LookinTypeId = 0 Then
                lblPartNo.Text = "Part No."
            Else
                lblPartNo.Text = "Description"
            End If
            Type = Val(Request.QueryString("Type"))
            Session("Type") = Type
            txtPartNo.Text = Name
            DataFieldBind(LookinTypeId, txtPartNo.Text, txtPartNo.Text)
        End If
        ControlVisibility()
        lblResult.Text = "List of Parts : " & mItems.Count & " Record(s) found."
        AlternateType = Val(Request.QueryString("AlternateType")) 'Added BY Vikrant on 28-03-2012
    End Sub
    Private Sub dgPartList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPartList.ItemCommand
        Dim Index As Integer = e.Item.ItemIndex + dgPartList.CurrentPageIndex * dgPartList.PageSize
        Select Case e.CommandName
            Case "Select"
                Dim mId As New Guid(e.Item.Cells(0).Text)
                mItem = Item.GetItem(mId)
                Session("mAltItem") = mItem
                Session.Remove("mItems")
                Response.Redirect(Request.QueryString("BackPage"))
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session.Remove("mItems")
        Session("mAltItem") = mItem ''
        Session("IsReturnedFromPartList") = "True" ''
        'Response.Redirect(Request.QueryString("BackPage"))
        If Request.QueryString("BackPage1") Is Nothing Then
            Response.Redirect(Request.QueryString("BackPage"))
        Else
            Dim str As String
            If AlternateType = 3 Then
                str = "<script language='javascript'>openledgersame('" & Request.QueryString("BackPage1") & "?BackPage=wfReceiptCumInvoice.aspx&ChildPage1=wfAlternatePartListForRCI.aspx&AlternateType=3');</script>"
            ElseIf AlternateType = 4 Then
                str = "<script language='javascript'>openledgersame('" & Request.QueryString("BackPage1") & "?BackPage=wfReceipt.aspx&ChildPage1=wfAlternatePOPartList.aspx&AlternateType=4');</script>"
            End If
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        End If
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        SetObject()
        Session.Remove("mItems")
        Session("IsReturnedFromPartList") = "True" ''
        Session("DoNotSelectAgain") = "DoNotSelectAgain"  'Added By Prashant 20-Oct-2009
        'Response.Redirect(Request.QueryString("BackPage"))
        If Request.QueryString("BackPage1") Is Nothing Then ''
            Response.Redirect(Request.QueryString("BackPage"))
        Else
            Dim str As String
            If AlternateType = 3 Then
                str = "<script language='javascript'>openledgersame('" & Request.QueryString("BackPage1") & "?BackPage=wfReceiptCumInvoice.aspx&ChildPage1=wfAlternatePartListForRCI.aspx&AlternateType=3');</script>"
            ElseIf AlternateType = 4 Then
                str = "<script language='javascript'>openledgersame('" & Request.QueryString("BackPage1") & "?BackPage=wfReceipt.aspx&ChildPage1=wfAlternatePOPartList.aspx&AlternateType=4');</script>"
            End If
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartList.CurrentPageIndex = 0
        If mLookInTypeId = 0 Then
            mLookInTypeId = 1
        End If
        DataFieldBind(mLookInTypeId, txtPartNo.Text, txtPartNo.Text)
        ControlVisibility()
        lblResult.Text = "List of Parts : " & mItems.Count & " Record(s) found."
    End Sub
    Private Sub dgPartList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgPartList.PageIndexChanged
        dgPartList.CurrentPageIndex = e.NewPageIndex
        mItems = Session("mItems")
        dgPartList.DataSource = mItems
        Session("mMachineList") = mItems
        dgPartList.DataBind()
    End Sub
    Private Sub dgPartList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgPartList.SortCommand
        mItems = Session("mItems")
        mItems.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMachineList") = mItems
        dgPartList.DataSource = mItems
        dgPartList.DataBind()
    End Sub
#End Region
End Class
