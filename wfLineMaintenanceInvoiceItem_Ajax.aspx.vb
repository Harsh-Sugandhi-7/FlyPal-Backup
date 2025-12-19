Public Class wfLineMaintenanceInvoiceItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Description "
    Public mLineMaintInvoice As LineMaintenanceInvoice
    ''Dim mVendorName As String
    Public Flag As Integer
#End Region

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
    End Enum
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mLineMaintInvoice = Session("mLineMaintInvoice")
        ''mVendorName = Session("VendorName")
    End Sub
    Private Sub setSession()
        Session("mLineMaintInvoice") = mLineMaintInvoice
    End Sub
    Private Sub addAttributes()
        txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value,event)")
        txtRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtRate').value,event)")
    End Sub
    Private Sub SetPage()
        If Session("Edit") Then
            lblTitle.Text = "Line Maintenance Invoice Item... " ''[" & mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem.JobDetails & "]"
        End If
    End Sub
    Private Function setObject() As Boolean
        mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.SrNo = mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentIndex + 1
        mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.JobDetails = Trim(txtJobDetails.Text)
        mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.Qty = Val(txtQty.Text)
        mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.Unit = Trim(txtUnit.Text)
        mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.CRate = Val(txtRate.Text)
        mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.Remark = Trim(txtRemark.Text)
        mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.Note = Trim(txtNote.Text)

        ''If mLineMaintenanceOrder.LineMaintenanceOrderItems.Contains(mLineMaintenanceOrder.LineMaintenanceOrderItems.CurrentItem) Then
        ''    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Line Maintenance Order Item", MsgBoxStyle.OKOnly)
        ''    msg1.ReplacePage = "wfLineMaintenanceOrderItem.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
        ''    msg1.Show()
        ''    mLineMaintenanceOrder.CancelEdit()
        ''    Exit Function
        ''End If
        'End If

        mLineMaintInvoice.CalculateTotal()
        If mLineMaintInvoice.IsRoundOff = True Then
            mLineMaintInvoice.RoundCGrandTotal()
        End If
        Return True
    End Function
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        'Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                Case MsgBoxResult.No
                    Session("Sender") = ""
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "LineMaintenanceInvoice"

        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.FindNow
                Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
        End Select
    End Function
#End Region

#Region " Data Binding "

    'Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
    '    Dim custValidator As CustomValidator
    '    custValidator = CType(s, CustomValidator)
    '    If custValidator.ControlToValidate = "txtQty" Then
    '        If Val(txtQty.Text) <= 0 Then
    '            custValidator.ErrorMessage = "Quantity must be greater than zero."
    '            e.IsValid = False
    '        End If
    '    ElseIf custValidator.ControlToValidate = "txtRate" Then
    '        If Val(txtRate.Text) < 0 Then
    '            custValidator.ErrorMessage = "Rate must be greater than zero."
    '            e.IsValid = False
    '        End If


    '    End If
    'End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub

        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        Dim strMsg As String = ""
        setObject()

        If Not mLineMaintInvoice.IsValid Then
            For i As Integer = 0 To mLineMaintInvoice.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mLineMaintInvoice.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If

        Dim mLineMaintenanceInvoiceItem As LineMaintenanceInvoiceItem
        If Not mLineMaintInvoice.LineMaintenanceInvoiceItems.IsValid Then
            For Each mLineMaintenanceInvoiceItem In mLineMaintInvoice.LineMaintenanceInvoiceItems
                For i As Integer = 0 To mLineMaintenanceInvoiceItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mLineMaintenanceInvoiceItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If

        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            e.IsValid = False
        End If

        Flag = 1
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        addAttributes()
        If Not IsPostBack Then
            If txtJobDetails.Enabled = True Then
                txtJobDetails.Focus()
            End If
            DataBind()
            SetPage()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not IsInRole(Rights.[New]) And mLineMaintInvoice.IsNew) Or (Not IsInRole(Rights.Edit) And Not mLineMaintInvoice.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If IsValid Then
            If setObject() Then
                Session("mLineMaintInvoice") = mLineMaintInvoice
                Session.Remove("Edit")
                Response.Redirect(Request.QueryString("BackPage"))
            End If
        Else
            upnlDetails.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem.IsNew And Not Session("Edit") = True Then mLineMaintInvoice.LineMaintenanceInvoiceItems.Remove(mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem)
        Session.Remove("Edit")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    
End Class