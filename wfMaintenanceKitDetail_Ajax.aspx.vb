
'AJAX Created By Saylee On 15-May-2015

Public Class wfMaintenanceKitDetail_Ajax
    Inherits System.Web.UI.Page


#Region " Variable declaration"
    Public mMaintenanceKit As MaintenanceKit
    Public mItemList As ItemList
    Public mMaintenanceTaskAndKit As MaintenanceTaskAndKit
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMaintenanceKit = Session("mMaintenanceKit")
        mItemList = Session("mItemList")
        mMaintenanceTaskAndKit = Session("mMaintenanceTaskAndKit")
    End Sub
    Private Sub setSession()
        Session("mMaintenanceKit") = mMaintenanceKit
        Session("mItemList") = mItemList
        Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
    End Sub
    Private Sub CancelRecord()
        mMaintenanceKit.MaintenanceKitDetails.RemoveAt(mMaintenanceKit.MaintenanceKitDetails.CurrentIndex)
        Session("mMaintenanceKit") = mMaintenanceKit
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
            If mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Name <> "" Then
                lblTitle.Text = "Maintenance Spares Item [" & mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Name & "]"
            Else
                lblTitle.Text = "Maintenance Spares Item [New]"
            End If
            lblPartInfo.InnerText = "Select Part to Add as a Spare"
        ElseIf mMaintenanceKit.IsForTool = True Then
            If mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Name <> "" Then
                lblTitle.Text = "Maintenance Tools Item [" & mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Name & "]"
            Else
                lblTitle.Text = "Maintenance Tools Item [New]"
            End If
            lblPartInfo.InnerText = "Select Part to Add as a Tool"
        End If

    End Sub
    Private Function setObject() As Boolean
        mMaintenanceKit.BeginEdit()
        mMaintenanceKit.MaintenanceKitDetails.CurrentItem.SrNo = mMaintenanceKit.MaintenanceKitDetails.CurrentIndex + 1
        mMaintenanceKit.MaintenanceKitDetails.CurrentItem.ItemID = New Guid(cmbPartNo.SelectedValue)
        mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Qty = Val(txtQuantity.Text)
        mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Note = Trim(txtNote.Text)
        mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Remark = Trim(txtRemark.Text) 'Added By Vikrant On 04-Apr-2014 For ALL04042014
        '  If Session("EditKit") = False Then
        If mMaintenanceKit.MaintenanceKitDetails.Contains(mMaintenanceKit.MaintenanceKitDetails.CurrentItem) Then
            mMaintenanceKit.CancelEdit()
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Maintenance Kit Item", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfMaintenanceKitDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&BackPage5=" & Request.QueryString("BackPage5")
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Maintenance Kit Item", MsgBoxStyle.OkOnly, "")
            Return False
            ' End If
        Else
            mMaintenanceKit.ApplyEdit()
        End If
        Return True
    End Function
    Private Sub Search()
        mItemList = ItemList.GetItemList(7, txtSearch.Text, , , , , , True)
        cmbPartNo.DataSource = mItemList
        Session("mItemList") = mItemList
        cmbPartNo.DataBind()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub addAttributes()
        txtQuantity.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQuantity').value,event)")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            DataFieldBind()
                            upnlPartInfo.Update()
                            ' Response.Redirect("wfMaintenanceKitDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&BackPage5=" & Request.QueryString("BackPage5"))
                        Catch ex As SqlException
                              If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    DataFieldBind()
                    ' Response.Redirect("wfMaintenanceKitDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&BackPage5=" & Request.QueryString("BackPage5"))
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfMaintenanceKitDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&BackPage5=" & Request.QueryString("BackPage5"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfMaintenanceKitDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&BackPage5=" & Request.QueryString("BackPage5"))
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        Dim Search As String = String.Empty
        If Session("EditKit") = True Then 'Added by Saylee on 11-Mar-2014 
            Search = mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Name
        Else
            Search = Trim(txtSearch.Text)
        End If

        mItemList = ItemList.GetItemList(7, Search, "", , , , , True)
        cmbPartNo.DataSource = mItemList
        Session("mItemList") = mItemList
        DataBind()
        cmbPartNo.SelectedValue = mMaintenanceKit.MaintenanceKitDetails.CurrentItem.ItemID.ToString

    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbPartNo" Then
            If cmbPartNo.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Part No form the list."
                e.IsValid = False
            End If
            'Commented By Vikrant On 04-Apr-2014 For ALL04042014
            'ElseIf custValidator.ControlToValidate = "txtQuantity" Then
            'If Val(txtQuantity.Text) <= 0 Then
            'custValidator.ErrorMessage = "Quantity must be greater than zero."
            'e.IsValid = False
            'End If
            'End
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            If cmbPartNo.Enabled = True Then
                setFocus(cmbPartNo)
            End If
            DataFieldBind()
            SetPage()
        End If
      End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Not Session("EditKit") Then Session.Remove("EditKit") : mMaintenanceKit.MaintenanceKitDetails.Remove(mMaintenanceKit.MaintenanceKitDetails.CurrentItem)
        Session("EditKit") = False

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect(Request.QueryString("BackPage5") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4"))
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Search()
        upnlPartInfo.Update()
        upnlTitle.Update()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            If setObject() Then
                mMaintenanceKit.ApplyEdit()
                mMaintenanceKit.Save()
                If mMaintenanceKit.IsForTool = True Then 'Added by Saylee on 23-July-2013 for BA22072013 
                    mMaintenanceTaskAndKit.MaintenanceToolID = mMaintenanceKit.ID
                Else
                    mMaintenanceTaskAndKit.MaintenanceKitID = mMaintenanceKit.ID
                End If
                setSession()
                Session("EditKit") = False
                Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit

                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
                Response.Redirect(Request.QueryString("BackPage5") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4"))
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Maintenance Kit Item", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
#End Region
End Class