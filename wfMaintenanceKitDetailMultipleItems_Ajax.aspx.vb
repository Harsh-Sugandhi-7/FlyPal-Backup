Imports System.Text
'Added By Vikrant On 13-Feb-2020

Public Class wfMaintenanceKitDetailMultipleItems_Ajax
    Inherits System.Web.UI.Page


#Region " Variable declaration"
    Public mMaintenanceKit As MaintenanceKit
    Public mItemList As ItemList
    Public mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    Dim PartsCopied, PartsNotCopied As New StringBuilder
    Public mCategoryLists As CategoryList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMaintenanceKit = Session("mMaintenanceKit")
        mItemList = Session("mItemList")
        mMaintenanceTaskAndKit = Session("mMaintenanceTaskAndKit")
        mCategoryLists = Session("mCategoryLists")
    End Sub
    Private Sub setSession()
        Session("mMaintenanceKit") = mMaintenanceKit
        Session("mItemList") = mItemList
        Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemList")
        Session.Remove("mCategoryLists")
    End Sub
    Private Sub CancelRecord()
        mMaintenanceKit.MaintenanceKitDetails.RemoveAt(mMaintenanceKit.MaintenanceKitDetails.CurrentIndex)
        Session("mMaintenanceKit") = mMaintenanceKit
    End Sub
    Private Sub SetGrid()
        Dim chkBox As CheckBox
        Dim txtQty, txtNotes, txtRemarks As TextBox
        For i As Integer = 0 To dgItemsList.Rows.Count - 1
            If mMaintenanceKit.MaintenanceKitDetails.Contains(mItemList(i).Name) Then
                chkBox = CType(dgItemsList.Rows(i).FindControl("chkSelect"), CheckBox)
                txtQty = CType(dgItemsList.Rows(i).FindControl("txtQuantity"), TextBox)
                txtNotes = CType(dgItemsList.Rows(i).FindControl("txtNote"), TextBox)
                txtRemarks = CType(dgItemsList.Rows(i).FindControl("txtRemark"), TextBox)

                chkBox.Checked = True
                chkBox.Enabled = False
                txtQty.ReadOnly = True
                txtQty.BackColor = Color.Gainsboro
                txtNotes.ReadOnly = True
                txtNotes.BackColor = Color.Gainsboro
                txtRemarks.ReadOnly = True
                txtRemarks.BackColor = Color.Gainsboro
                txtQty.Text = mMaintenanceKit.MaintenanceKitDetails(mItemList(i).ID, "").Qty.ToString
                txtNotes.Text = mMaintenanceKit.MaintenanceKitDetails(mItemList(i).ID, "").Note
                txtRemarks.Text = mMaintenanceKit.MaintenanceKitDetails(mItemList(i).ID, "").Remark
            End If
        Next
    End Sub
    Private Sub SetPage()
        'Commented by Saylee on 23-July-2013 for BA22072013 
        '''If mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Name <> "" Then
        '''    lblTitle.Text = "Maintenance Kit Item [" & mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Name & "]"
        '''Else
        '''    lblTitle.Text = "Maintenance Kit Item [New]"
        '''End If

        'Added by Saylee on 23-July-2013 for BA22072013 
        If mMaintenanceKit.IsForTool = False Then
            'If mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Name <> "" Then
            '    lblTitle.Text = "Maintenance Spares Item [" & mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Name & "]"
            'Else
            '    lblTitle.Text = "Maintenance Spares Item [New]"
            'End If
            lblTitle.Text = "Maintenance Spares Item"
            lblSpareList.InnerText = "Spare's List for Maintenance Activity"
        ElseIf mMaintenanceKit.IsForTool = True Then
            'If mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Name <> "" Then
            '    lblTitle.Text = "Maintenance Tools Item [" & mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Name & "]"
            'Else
            '    lblTitle.Text = "Maintenance Tools Item [New]"
            'End If
            lblTitle.Text = "Maintenance Tools Item"
            'lblPartInfo.InnerText = "Select Part to Add as a Tool"
            lblSpareList.InnerText = "Tool's List for Maintenance Activity"
        End If
    End Sub
    Private Sub setObject()
        Dim chkBox As CheckBox
        Dim txtQty, txtNotes, txtRemarks As TextBox
        For i As Integer = 0 To dgItemsList.Rows.Count - 1
            chkBox = CType(dgItemsList.Rows(i).FindControl("chkSelect"), CheckBox)
            If chkBox.Checked And chkBox.Enabled Then
                txtQty = CType(dgItemsList.Rows(i).FindControl("txtQuantity"), TextBox)
                txtNotes = CType(dgItemsList.Rows(i).FindControl("txtNote"), TextBox)
                txtRemarks = CType(dgItemsList.Rows(i).FindControl("txtRemark"), TextBox)

                mMaintenanceKit.MaintenanceKitDetails.Add(mMaintenanceKit.ID)
                'mMaintenanceKit.BeginEdit()
                mMaintenanceKit.MaintenanceKitDetails.CurrentItem.SrNo = mMaintenanceKit.MaintenanceKitDetails.CurrentIndex + 1
                mMaintenanceKit.MaintenanceKitDetails.CurrentItem.ItemID = mItemList(i).ID
                mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Qty = Val(txtQty.Text)
                mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Note = Trim(txtNotes.Text)
                mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Remark = Trim(txtRemarks.Text) 'Added By Vikrant On 04-Apr-2014 For ALL04042014
                '  If Session("EditKit") = False Then
                If mMaintenanceKit.MaintenanceKitDetails.Contains(mMaintenanceKit.MaintenanceKitDetails.CurrentItem) Then
                    PartsNotCopied.Append(mItemList(i).Name + ",")
                Else
                    PartsCopied.Append(mItemList(i).Name + ",")
                End If
            End If
        Next
    End Sub
    Private Sub Search()
        mItemList = ItemList.GetItemList(7, txtSearch.Text, , , IIf(cmbCategory.SelectedIndex = 0, "", cmbCategory.SelectedItem.ToString), , , False)
        dgItemsList.DataSource = mItemList
        Session("mItemList") = mItemList
        dgItemsList.DataBind()
        lblresult.Text = "Item(s) List: " + IIf(mItemList.Count = 100, "100 record(s) found. (For Fast Performance list shows first 100 records only)", mItemList.Count.ToString + " record(s) found.")
    End Sub
    Private Sub addAttributes()
        'txtQuantity.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQuantity').value,event)")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfMaintenanceKitDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&BackPage5=" & Request.QueryString("BackPage5"))
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryLists = CategoryList.GetCategoryList("All")
        cmbCategory.DataSource = mCategoryLists
        Session("mCategoryLists") = mCategoryLists

        If mCategoryLists.Count > 1 Then
            mItemList = ItemList.GetItemList(7, Trim(txtSearch.Text), "", , IIf(mCategoryLists.Count > 1, mCategoryLists(1).Name, ""), , , False)
        Else
            mItemList = ItemList.GetItemList(7, Trim(txtSearch.Text), "", , "", , , False)
        End If

        dgItemsList.DataSource = mItemList
        Session("mItemList") = mItemList

        dgKitList.DataSource = mMaintenanceKit.MaintenanceKitDetails

        DataBind()
        If cmbCategory.Items.Count > 0 Then
            cmbCategory.SelectedIndex = 1
        End If
        lblresult.Text = "Item(s) List: " + IIf(mItemList.Count = 100, "100 record(s) found. (For Fast Performance list shows first 100 records only)", mItemList.Count.ToString + " record(s) found.")
    End Sub
    'Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
    '    Dim custValidator As CustomValidator
    '    custValidator = CType(s, CustomValidator)
    '    If custValidator.ControlToValidate = "txtSearch" Then
    '        Dim txtQty, txtNt, txtRem As TextBox
    '        For i As Integer = 0 To dgItemsList.Rows.Count - 1
    '            txtQty = CType(dgItemsList.Rows(i).FindControl("txtQuantity"), TextBox)
    '            txtNt = CType(dgItemsList.Rows(i).FindControl("txtNote"), TextBox)
    '            txtRem = CType(dgItemsList.Rows(i).FindControl("txtRemark"), TextBox)
    '            If (txtLicNo.Text.Trim.IndexOf("[") > 0 And txtLicNo.Text.Trim.IndexOf("]") > 0) Or (txtLicNo.Text.Trim.IndexOf("[") < 0 And txtLicNo.Text.Trim.IndexOf("]") < 0) Then
    '                e.IsValid = True
    '            Else
    '                custValidator.ErrorMessage = "Enter Correct License No."
    '                e.IsValid = False
    '                Exit For
    '            End If
    '        Next
    '    End If
    'End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        If Not IsPostBack Then
            txtSearch.Focus()
            DataFieldBind()
            SetGrid()
            SetPage()
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        'If Not Session("EditKit") Then Session.Remove("EditKit") : mMaintenanceKit.MaintenanceKitDetails.Remove(mMaintenanceKit.MaintenanceKitDetails.CurrentItem)
        'Session("EditKit") = False
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect(Request.QueryString("BackPage5") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4"))
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Search()
        SetGrid()
        upnlgrid.Update()
        upnlTitle.Update()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveTop.Click
        If IsValid Then
            'mMaintenanceKit.MaintenanceKitDetails.Add(mMaintenanceKit.ID)
            setObject()
            'mMaintenanceKit.ApplyEdit()
            mMaintenanceKit.Save()
            If mMaintenanceKit.IsForTool = True Then 'Added by Saylee on 23-July-2013 for BA22072013 
                mMaintenanceTaskAndKit.MaintenanceToolID = mMaintenanceKit.ID
            Else
                mMaintenanceTaskAndKit.MaintenanceKitID = mMaintenanceKit.ID
            End If
            setSession()
            Session("EditKit") = False
            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            SetGrid()
            dgKitList.DataSource = mMaintenanceKit.MaintenanceKitDetails
            dgKitList.DataBind()
            upnlgrid.Update()
            upnlKitList.Update()
            'Dim mopenas As String = Request.QueryString("Type")
            'If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            '    Exit Sub
            'End If
            'Response.Redirect(Request.QueryString("BackPage5") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4"))
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Protected Sub txtQuantity_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtQty As TextBox
        Dim chkBox As CheckBox
        Dim IsFirstItem As Boolean = True
        Dim FirstRowQty As Decimal
        For i As Integer = 0 To dgItemsList.Rows.Count - 1
            txtQty = CType(dgItemsList.Rows(i).FindControl("txtQuantity"), TextBox)
            chkBox = CType(dgItemsList.Rows(i).FindControl("chkSelect"), CheckBox)
            If chkBox.Enabled Then
                If IsFirstItem Then
                    FirstRowQty = Val(txtQty.Text)
                    IsFirstItem = False
                End If
                txtQty.Text = FirstRowQty.ToString
            End If
        Next
    End Sub
    Protected Sub txtNote_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtNote As TextBox
        Dim FirstRowNote As String
        Dim chkBox As CheckBox
        Dim IsFirstItem As Boolean = True

        For i As Integer = 0 To dgItemsList.Rows.Count - 1
            txtNote = CType(dgItemsList.Rows(i).FindControl("txtNote"), TextBox)
            chkBox = CType(dgItemsList.Rows(i).FindControl("chkSelect"), CheckBox)
            If chkBox.Enabled Then
                If IsFirstItem Then
                    FirstRowNote = txtNote.Text
                    IsFirstItem = False
                End If
                txtNote.Text = FirstRowNote
            End If
        Next
    End Sub
    Protected Sub txtRemark_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtRemark As TextBox
        Dim FirstRowRemark As String
        Dim chkBox As CheckBox
        Dim IsFirstItem As Boolean = True

        For i As Integer = 0 To dgItemsList.Rows.Count - 1
            txtRemark = CType(dgItemsList.Rows(i).FindControl("txtRemark"), TextBox)
            chkBox = CType(dgItemsList.Rows(i).FindControl("chkSelect"), CheckBox)
            If chkBox.Enabled Then
                If IsFirstItem Then
                    FirstRowRemark = txtRemark.Text
                    IsFirstItem = False
                End If
                txtRemark.Text = FirstRowRemark
            End If
        Next
    End Sub
#End Region

    

End Class