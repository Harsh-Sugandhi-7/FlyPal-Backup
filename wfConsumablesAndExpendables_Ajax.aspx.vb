Imports System.Linq
Imports System.Collections.Generic
Public Class wfConsumablesAndExpendables_Ajax
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
        Authorized = 8
    End Enum
#End Region

#Region " Variable Declaration "
    Public mConsumableAndExpendable As ConsumableAndExpendable
    Public Shared mRequisitionListForCombo As RequisitionListForCombo
    'Public mMachineNameValueList As MachineNameValueList
    Dim EventLogID As Guid
    Dim mEventLogDetail As String
    Public Flag As Integer
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mConsumableAndExpendable = Session("mConsumableAndExpendable")
        mRequisitionListForCombo = Session("mRequisitionListForCombo")
        'mMachineNameValueList = Session("mMachineNameValueList")
    End Sub
    Private Sub setObject()
        If txtDate.Text = "" Then
            mConsumableAndExpendable.TransDate = Today.Date
        Else
            mConsumableAndExpendable.TransDate = CDate(txtDate.Text)
        End If
        mConsumableAndExpendable.Text = txtText.Text.Trim
        mConsumableAndExpendable.No = Val(txtNo.Text)
        mConsumableAndExpendable.UserName = User.Identity.Name
        'mConsumableAndExpendable.ReqID = New Guid(cmbRequisitionText.SelectedValue)
        'mConsumableAndExpendable.MachineID = New Guid(cmbMachine.SelectedValue)

        Dim txtQty, txtText1 As TextBox
        Dim mConsumableAndExpendableItem As ConsumableAndExpendableItem
        Dim i As Integer = 0
        For Each mConsumableAndExpendableItem In mConsumableAndExpendable.ConsumableAndExpendableItems
            With mConsumableAndExpendableItem
                txtQty = CType(Me.dgItems.Rows(i).FindControl("txtUsedQty"), TextBox)
                .UsedQty = CDec(Val(txtQty.Text))

                txtQty = CType(Me.dgItems.Rows(i).FindControl("txtScrapQty"), TextBox)
                .ScrapQty = CDec(Val(txtQty.Text))

                txtText1 = CType(Me.dgItems.Rows(i).FindControl("txtSerialNo"), TextBox)
                .SerialNo = Trim(txtText1.Text)

                txtText1 = CType(Me.dgItems.Rows(i).FindControl("txtPosition"), TextBox)
                .Position = Trim(txtText1.Text)

                txtText1 = CType(Me.dgItems.Rows(i).FindControl("txtReference"), TextBox)
                .Reference = Trim(txtText1.Text)

                txtText1 = CType(Me.dgItems.Rows(i).FindControl("txtNote"), TextBox)
                .Note = Trim(txtText1.Text)

                txtText1 = CType(Me.dgItems.Rows(i).FindControl("txtCostCenter"), TextBox)
                .RegNo = Trim(txtText1.Text)
            End With
            i = i + 1
        Next
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mConsumableAndExpendable.ConsumableAndExpendableItems.CurrentIndex = Index
        Session("mConsumableAndExpendable") = mConsumableAndExpendable
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("Sender") = ""
                            Dim mConsumableAndExpendable As ConsumableAndExpendable
                            mConsumableAndExpendable = CType(Session("mConsumableAndExpendable"), ConsumableAndExpendable)
                            mConsumableAndExpendable.ConsumableAndExpendableItems.Remove(mConsumableAndExpendable.ConsumableAndExpendableItems.CurrentItem)
                            Session("mConsumableAndExpendable") = mConsumableAndExpendable
                            dgItems.DataSource = mConsumableAndExpendable.ConsumableAndExpendableItems
                            dgItems.DataBind()
                            ControlVisibility()
                            AddAttr()
                            upnlDetails.Update()
                            upnlGridView.Update()
                            upnlActionBtn.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
                        Session("sender") = ""
                        Page.Validate("1")
                        If IsValid Then
                            Session.Remove("IsValid")
                            If mConsumableAndExpendable.ConsumableAndExpendableItems.Count = 0 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "C&E can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                                If mConsumableAndExpendable.StatusID = 2 Then
                                    mConsumableAndExpendable.StatusID = 1
                                    Session("mConsumableAndExpendable") = mConsumableAndExpendable
                                End If
                                Exit Sub
                            End If
                            DataFieldBind()
                            If (Not IsInRole(Rights.New)) And (Not IsInRole(Rights.Edit)) Then
                                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            Save()
                            RemoveSessions()
                            Response.Redirect("Index.aspx")
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                        End If
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If Session("IsValid") Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            Save()
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        If mConsumableAndExpendable.StatusID = 2 Then
                            mConsumableAndExpendable.StatusID = 1
                        End If
                        Session("mConsumableAndExpendable") = mConsumableAndExpendable
                        upnlStatus.Update()
                    Else
                        Session("Sender") = ""
                    End If
                Case MsgBoxResult.Ok
                    'Added by Utkarsh On 22-Nov-2013 For TransTextSeries

                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            ''Added New on 9 April RAJNISH
            If mConsumableAndExpendable.StatusID = 2 Then
                mConsumableAndExpendable.StatusID = 1
                'ElseIf mRequisition.StatusID = 4 Then
                '    mRequisition.StatusID = 2
            End If
            Session("mConsumableAndExpendable") = mConsumableAndExpendable
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetPage()
        If mConsumableAndExpendable.No > 0 Then
            lblTitle.Text = "Consumables & Expendables(C&E) Details [" & mConsumableAndExpendable.Text + "-" + CType(mConsumableAndExpendable.No, String) + "]"
        Else
            lblTitle.Text = "Consumables & Expendables(C&E) Details "
        End If
    End Sub
    Private Sub ControlVisibility()
        Dim txtUsedQty, txtScrapQty, txtReference, txtNote, txtSerialNo, txtPosition, txtCostCenter As TextBox
        For i As Integer = 0 To dgItems.Rows.Count - 1
            txtUsedQty = CType(Me.dgItems.Rows(i).FindControl("txtUsedQty"), TextBox)
            txtUsedQty.Enabled = CType(IIf(mConsumableAndExpendable.StatusID > 1, False, True), Boolean)

            txtScrapQty = CType(Me.dgItems.Rows(i).FindControl("txtScrapQty"), TextBox)
            txtScrapQty.Enabled = CType(IIf(mConsumableAndExpendable.StatusID > 1, False, True), Boolean)

            txtSerialNo = CType(Me.dgItems.Rows(i).FindControl("txtSerialNo"), TextBox)
            txtSerialNo.Enabled = CType(IIf(mConsumableAndExpendable.StatusID > 1, False, True), Boolean)

            txtPosition = CType(Me.dgItems.Rows(i).FindControl("txtPosition"), TextBox)
            txtPosition.Enabled = CType(IIf(mConsumableAndExpendable.StatusID > 1, False, True), Boolean)

            txtReference = CType(Me.dgItems.Rows(i).FindControl("txtReference"), TextBox)
            txtReference.Enabled = CType(IIf(mConsumableAndExpendable.StatusID > 1, False, True), Boolean)

            txtNote = CType(Me.dgItems.Rows(i).FindControl("txtNote"), TextBox)
            txtNote.Enabled = CType(IIf(mConsumableAndExpendable.StatusID > 1, False, True), Boolean)

            txtCostCenter = CType(Me.dgItems.Rows(i).FindControl("txtCostCenter"), TextBox)
            txtCostCenter.Enabled = CType(IIf(mConsumableAndExpendable.StatusID > 1 Or mConsumableAndExpendable.RegNo <> "", False, True), Boolean)
        Next
        btnAuthorized.Visible = (Not mConsumableAndExpendable.IsNew) And (mConsumableAndExpendable.StatusID = 1)
        If mConsumableAndExpendable.StatusID > 1 Then
            dgItems.Columns(13).Visible = False
            txtText.Enabled = False
            txtNo.Enabled = False
            txtDate.Enabled = False
            btnAddItem.Enabled = False
            btnSave.Visible = False
            'cmbRequisitionText.Enabled = False
            txtReqTextNo.Enabled = False
            'cmbMachine.Enabled = False
        Else
            dgItems.Columns(13).Visible = True
            txtText.Enabled = True
            txtNo.Enabled = True
            If mConsumableAndExpendable.ConsumableAndExpendableItems.Count > 0 Then
                txtDate.Enabled = False
                'cmbRequisitionText.Enabled = False
                txtReqTextNo.Enabled = False
                txtText.Enabled = False
                txtNo.Enabled = False
            Else
                txtDate.Enabled = True
                'cmbRequisitionText.Enabled = True
                txtReqTextNo.Enabled = True
                txtText.Enabled = True
                txtNo.Enabled = True
            End If
            btnAddItem.Enabled = True
            btnSave.Visible = True
            'cmbRequisitionText.Enabled = True
            'cmbMachine.Enabled = True
        End If

        If Not IsInRole(Rights.Authorized) Then
            btnAuthorized.Enabled = False
            btnAuthorized.ToolTip = "You are not authorized user "
        End If
    End Sub
    Private Sub Save()
        'Authentication
        If Not mConsumableAndExpendable.TransDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------

                If DateDiff(DateInterval.Day, CDate(mConsumableAndExpendable.TransDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Consumables & Expendables(C&E)." + "\n" + "Consumables & Expendables(C&E) Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        End If
        'Authentication

        Dim ConsumableAndExpendableClone As ConsumableAndExpendable
        ConsumableAndExpendableClone = mConsumableAndExpendable.Clone
        Try
            If Not mConsumableAndExpendable.ConsumableAndExpendableItems.Count = 0 Then
                setObject()
                mConsumableAndExpendable.Save()


                mEventLogDetail = "C&E : " + mConsumableAndExpendable.CnETextNo + " Dated : " + mConsumableAndExpendable.TransDateFormatted.ToString '+ " On Aircaft : " + cmbMachine.SelectedItem.ToString

                If mConsumableAndExpendable.StatusID = 2 Then
                    MarkLog(Util.Action.Authorize, "ConsumablesAndExpendables", mEventLogDetail, Util.ErrorType.NoError, mConsumableAndExpendable.ID, EventLogID)
                Else
                    MarkLog(Util.Action.Save, "ConsumablesAndExpendables", mEventLogDetail, Util.ErrorType.NoError, mConsumableAndExpendable.ID, EventLogID)
                End If

                mConsumableAndExpendable.MarkClean()
                lblTitle.Text = "C&E ( Saved ...)"
                Session("mConsumableAndExpendable") = mConsumableAndExpendable
                DataFieldBind()
                SetPage()
                ControlVisibility()
                upnlTitle.Update()
                upnlDetails.Update()
                upnlStatus.Update()
                upnlActionBtn.Update()
                upnlGridView.Update()
                upnlItemAdd.Update()
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "C&E can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                mConsumableAndExpendable = ConsumableAndExpendableClone
                If mConsumableAndExpendable.StatusID = 2 Then
                    mConsumableAndExpendable.StatusID = 1
                End If
                Session("mConsumableAndExpendable") = mConsumableAndExpendable
                Exit Sub
            End If
        Catch ex As SqlClient.SqlException
            Session("ConsumableAndExpendableClone") = ConsumableAndExpendableClone
            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            End If
        Catch ex As Exception
            MSGBoxCtrl.show(MSGBox.Message_title.CheckQty, MSGBox.Message_text.CheckQty, ex.Message, MsgBoxStyle.OkOnly, "ShowMsg")
        Finally
            ConsumableAndExpendableClone = Nothing
        End Try
    End Sub
    Private Sub RemoveSessions()
        'Session.Remove("mMachineNameValueList")
        Session.Remove("mRequisitionListForCombo")
        Session.Remove("mConsumableAndExpendable")
        Session.Remove("EditCE")
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "ConsumablesAndExpendables"

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
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        If Session("EditCE") = "True" Or mConsumableAndExpendable.ConsumableAndExpendableItems.Count > 0 Then
            mRequisitionListForCombo = RequisitionListForCombo.GetRequisitionList("", mConsumableAndExpendable.TransDateFormatted.ToString, StartingDate:=AppSettings("StartingDateForCnEConsideration").ToString())
        Else
            mRequisitionListForCombo = RequisitionListForCombo.GetRequisitionList("", mConsumableAndExpendable.TransDateFormatted.ToString, 1, StartingDate:=AppSettings("StartingDateForCnEConsideration").ToString())
        End If

        Session("mRequisitionListForCombo") = mRequisitionListForCombo
        'Commented and Added By Vikrant On 23-Jul-2018 For BA23072018
        'cmbRequisitionText.DataSource = mRequisitionListForCombo
        txtReqTextNo.Text = mConsumableAndExpendable.ReqTextNo
        'End

        'mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, IsTagRequired:=True, TagText:="(SELECT)")
        'Session("mMachineNameValueList") = mMachineNameValueList
        'cmbMachine.DataSource = mMachineNameValueList

        dgItems.DataSource = mConsumableAndExpendable.ConsumableAndExpendableItems
        txtDate.Text = CDate(mConsumableAndExpendable.TransDateFormatted.ToString).ToString(AppSettings("DateFormat"))

        DataBind()

        AddAttr()
    End Sub
    Private Sub AddAttr()
        Dim txtValue As TextBox
        Dim mConsumableAndExpendableItem As ConsumableAndExpendableItem
        Dim i As Integer = 0
        For Each mConsumableAndExpendableItem In mConsumableAndExpendable.ConsumableAndExpendableItems
            With mConsumableAndExpendableItem
                txtValue = CType(Me.dgItems.Rows(i).FindControl("txtUsedQty"), TextBox)
                txtValue.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
                txtValue = CType(Me.dgItems.Rows(i).FindControl("txtScrapQty"), TextBox)
                txtValue.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")

            End With
            i = i + 1
        Next
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'SetEmpID()
        'Commented and Added By Vikrant On 23-Jul-2018 For BA23072018
        'If custValidator.ControlToValidate = "cmbRequisitionText" Then
        '    If cmbRequisitionText.SelectedIndex <= 0 Then
        '        custValidator.ErrorMessage = "Parts Requisition Sheet No.(PRS) Required."
        '        e.IsValid = False

        '    End If
        'End If
        If custValidator.ControlToValidate = "txtReqTextNo" Then
            If txtReqTextNo.Text = "" Or mConsumableAndExpendable.ReqID.Equals(Guid.Empty) Then
                e.IsValid = False
                custValidator.ErrorMessage = "Parts Requisition Sheet No.(PRS) Required."
            Else
                e.IsValid = True
            End If
        End If
        'End
       
        'If custValidator.ControlToValidate = "cmbMachine" Then
        '    If cmbMachine.SelectedIndex <= 0 Then
        '        custValidator.ErrorMessage = "Aircraft Required."
        '        e.IsValid = False

        '    End If
        'End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()
        If Not IsPostBack Then
            If txtText.Enabled = True Then
                txtText.Focus()
            End If
            DataFieldBind()
            SetPage()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgItems_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgItems.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument)
                Session("Edit") = True
                setObject()
                mConsumableAndExpendable.ConsumableAndExpendableItems.CurrentIndex = Index
                Session("mConsumableAndExpendable") = mConsumableAndExpendable
                Response.Redirect("wfConsumablesAndExpendableItem_Ajax.aspx?BackPage=wfConsumablesAndExpendables_Ajax.aspx")
            Case "DeleteRec"
                Index = CInt(e.CommandArgument)
                DeleteRecord(Index)
        End Select
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not IsInRole(Rights.New)) And (Not IsInRole(Rights.Edit)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If IsValid Then
            Save()
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnAddItem_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnAddItem.Click
        If IsValid Then
            setObject()
            'mConsumableAndExpendable.ConsumableAndExpendableItems.Add(mConsumableAndExpendable.ID)
            'Session("mConsumableAndExpendable") = mConsumableAndExpendable
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow();", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Page.Validate("1")
        Session("IsValid") = IsValid
        setObject()
        If mConsumableAndExpendable.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
            If IsValid Then
                setObject()
            End If
        Else
            RemoveSessions()
            MarkLog(Util.Action.Close, "ConsumablesAndExpendables", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub btnAuthorized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
        If IsValid Then
            Session("IsValid") = IsValid
            MSGBoxCtrl.show(MSGBox.Message_title.StatusSubmitted, MSGBox.Message_text.StatusSubmitted, "<Strong> Consumables & Expendables(C&E) </Strong>", MsgBoxStyle.YesNo, "Status")
            mConsumableAndExpendable.StatusID = 2
            Session("mConsumableAndExpendable") = mConsumableAndExpendable
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Protected Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        mConsumableAndExpendable = Session("mConsumableAndExpendable")
        mConsumableAndExpendable.TransDate = txtDate.Text
        txtText.Text = mConsumableAndExpendable.Text
        If Session("EditCE") = "True" Then
            mRequisitionListForCombo = RequisitionListForCombo.GetRequisitionList("(SELECT)", mConsumableAndExpendable.TransDateFormatted.ToString, StartingDate:=AppSettings("StartingDateForCnEConsideration").ToString())
        Else
            mRequisitionListForCombo = RequisitionListForCombo.GetRequisitionList("(SELECT)", mConsumableAndExpendable.TransDateFormatted.ToString, 1, StartingDate:=AppSettings("StartingDateForCnEConsideration").ToString())
        End If
        Session("mRequisitionListForCombo") = mRequisitionListForCombo
        txtReqTextNo.Text = ""
        mConsumableAndExpendable.ReqID = Guid.Empty
        mConsumableAndExpendable.ReqTextNo = ""
        Session("mConsumableAndExpendable") = mConsumableAndExpendable
    End Sub
    Protected Sub txtReqTextNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If mRequisitionListForCombo.Contains(txtReqTextNo.Text) Then
            mConsumableAndExpendable.ReqID = mRequisitionListForCombo(txtReqTextNo.Text).ID
            mConsumableAndExpendable.ReqTextNo = mRequisitionListForCombo(txtReqTextNo.Text).RequisitionTextNo
        Else
            txtReqTextNo.Text = ""
            mConsumableAndExpendable.ReqID = Guid.Empty
            mConsumableAndExpendable.ReqTextNo = ""
        End If
        Session("mConsumableAndExpendable") = mConsumableAndExpendable
    End Sub
#End Region

#Region " Add Multiple Parts "
    Private Sub AddCEParts()
        Dim mRequisitionItemNew As RequisitionItemNew
        Dim mRequisitionItemsNew As RequisitionItemsNew = Session("mRequisitionItemsNew")
        For Each mRequisitionItemNew In mRequisitionItemsNew
            If mRequisitionItemNew.IsSelect Then
                If Not mConsumableAndExpendable.ConsumableAndExpendableItems.Contains(mRequisitionItemNew.ID) Then
                    mConsumableAndExpendable.ConsumableAndExpendableItems.Add(mConsumableAndExpendable.ID)
                    With mConsumableAndExpendable.ConsumableAndExpendableItems.CurrentItem
                        .ReqItemID = mRequisitionItemNew.ID
                        .ItemID = mRequisitionItemNew.ItemID
                        .ItemName = mRequisitionItemNew.PartNo
                        .ItemDescription = mRequisitionItemNew.Description
                        .DisplayUnitName = mRequisitionItemNew.Unit
                        .UsedQty = mRequisitionItemNew.RemainingCEQty
                        .RequestedQty = mRequisitionItemNew.RequestedQty
                        .IssuedQty = mRequisitionItemNew.IssuedQty
                        .MachineID = mRequisitionItemNew.MachineID
                        .RegNo = mRequisitionItemNew.RegNo
                        .DisplayUnitID = mRequisitionItemNew.UnitID
                        .RemainingQty = mRequisitionItemNew.RemainingCEQty
                    End With
                    If mConsumableAndExpendable.ConsumableAndExpendableItems.CurrentItem.SRNo = 1 Then
                        mConsumableAndExpendable.RequestedBy = mRequisitionItemNew.AuthorizedBy
                        mConsumableAndExpendable.LocationName = mRequisitionItemNew.LocationName
                        mConsumableAndExpendable.EmployeeName = mRequisitionItemNew.EmployeeName
                    End If
                End If

            End If
        Next
    End Sub
#End Region

#Region " Show BrokenRules "
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        Dim strMsg As String = ""
        setObject()
        If Not mConsumableAndExpendable.IsValid Then
            For i As Integer = 0 To mConsumableAndExpendable.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mConsumableAndExpendable.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        Dim mConsumableAndExpendableItem As ConsumableAndExpendableItem
        If Not mConsumableAndExpendable.ConsumableAndExpendableItems.IsValid Then
            For Each mConsumableAndExpendableItem In mConsumableAndExpendable.ConsumableAndExpendableItems
                For i As Integer = 0 To mConsumableAndExpendableItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mConsumableAndExpendableItem.ItemName + " : " + mConsumableAndExpendableItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If
        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            e.IsValid = False
        End If
        Flag = 1
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnimgBtnCEPartList_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnCEPartList.Click
        If CType(Session("AddCEParts"), String) = "True" Then
            AddCEParts()
            Session("AddCEParts") = "False"
            dgItems.DataSource = mConsumableAndExpendable.ConsumableAndExpendableItems
            dgItems.DataBind()
            ControlVisibility()
            AddAttr()
            upnlDetails.Update()
            upnlGridView.Update()
        Else
            Session("AddCEParts") = "False"
        End If
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetReqTextNoList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        If count = 0 Then
            Return (From c As RequisitionListForCombo.RequisitionListForComboInfo In mRequisitionListForCombo
                    Where c.RequisitionTextNo.ToUpper.Contains(prefixText.ToUpper)
                   Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.RequisitionTextNo, c.ID.ToString())).ToArray
        Else
            Return (From c As RequisitionListForCombo.RequisitionListForComboInfo In mRequisitionListForCombo
                    Where c.RequisitionTextNo.ToUpper.Contains(prefixText.ToUpper)
                  Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.RequisitionTextNo, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region


End Class