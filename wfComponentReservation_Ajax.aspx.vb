Imports System.Linq
Public Class wfComponentReservation_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mComponentReservation As ComponentReservation
    Public mMachineNameValueList As MachineNameValueList
    Dim EventLogID As Guid
    Dim ComponentReservationDetail As String
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mComponentReservation = Session("mComponentReservation")
        mMachineNameValueList = Session("mMachineNameValueList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mComponentReservation")
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Sub SetPage()

    End Sub
    Private Sub ControlVisibility()

    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetControlStatus(ByVal StatusId As Int16)

    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session("sender") = ""
                        If mComponentReservation.IsValid = True Then
                            DataFieldBind()
                            If (Not User.IsInRole("ComponentReservationNew") And Not User.IsInRole("ComponentReservationEdit")) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            If Save() = True Then
                                Response.Redirect("Index.aspx")
                            End If
                        Else
                            Dim str As String = ""
                            If mComponentReservation.GetBrokenRulesCollection.Count > 0 Then
                                For i As Integer = 0 To mComponentReservation.GetBrokenRulesCollection.Count - 1
                                    str = str + mComponentReservation.GetBrokenRulesCollection(i).Description + "<BR>"
                                Next
                            End If
                            If str <> "" Then
                                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, str, MsgBoxStyle.OkOnly, "Cannot")
                                Exit Sub
                            End If
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Cannot" Then

                    End If
                    If MSGBoxCtrl.Sender = "ReadOnlyAircraft" Then
                        cmbAircraftList.ClearSelection()
                        upnlComponentReservationDetails.Update()
                    End If
            End Select
        End If
    End Sub
    Private Sub addAttributes()
    End Sub
    Private Sub setObject()
        mComponentReservation.ReserveForDate = CDate(txtReservationDate.Text)
        mComponentReservation.MachineID = New Guid(cmbAircraftList.SelectedValue)
        mComponentReservation.RegNo = cmbAircraftList.SelectedItem.Text
        mComponentReservation.ReserveForRemark = txtRemark.Text.Trim
        mComponentReservation.ReservedBy = User.Identity.Name
        mComponentReservation.IsReserve = True
    End Sub
    Private Function Save() As Boolean
        Dim msgCnt As Integer = 0
        Dim InvoiceClone As ComponentReservation
        InvoiceClone = mComponentReservation.Clone
        Try
            setObject()
            mComponentReservation.Save()
            ComponentReservationDetail = " Dated: " + mComponentReservation.ReserveForDateFormatted + " Reg. No. " + cmbAircraftList.SelectedItem.Text + " Part No. " + mComponentReservation.PartNo + " Serial No. " + mComponentReservation.SerialNo
            MarkLog(Util.Action.Save, "ComponentReservation", ComponentReservationDetail, Util.ErrorType.NoError, mComponentReservation.ID, EventLogID)
            Session("mComponentReservation") = mComponentReservation
            SetPage()
            ControlVisibility()
            'SetControlStatus(mComponentReservation.StatusID)
            upnlComponentReservationDetails.Update()
            upnlButtons.DataBind()
            upnlButtons.Update()
            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            Response.Redirect("Index.aspx")
        Catch ex As SqlClient.SqlException
            Session("InvoiceClone") = InvoiceClone
            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                Return False
                Exit Function
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Return False
                Exit Function
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Return False
                Exit Function
            ElseIf ex.Number = 547 Then
                If InStr(ex.Message, "FKtabInvoiceChargetabCharge", CompareMethod.Text) Then
                    MSGBoxCtrl.show("Alert!", "Other Charge Deleted! ", "Other Charge Not Avalable<Br><BR>Selected Charge is no longer exist in the Database <BR><BR> Remove Charge and try Again", MsgBoxStyle.OkOnly, "")
                    Return False
                    Exit Function
                ElseIf InStr(ex.Message, "CCtabInvoiceNo", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "No. Required", MsgBoxStyle.OkOnly, "")
                    Exit Function
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Function
                End If
            End If
            mComponentReservation = InvoiceClone
            Session("mComponentReservation") = mComponentReservation
            msgCnt = ex.Number
        Finally
            InvoiceClone = Nothing
        End Try
        If msgCnt = 0 Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , IsTagRequired:=True, TagText:="(SELECT)", ForInventory:=True)
        cmbAircraftList.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        txtReservationDate.Text = mComponentReservation.ReserveForDateFormatted
        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text.Trim) > 500 Then
                custValidator.ErrorMessage = "Remark should be 500 characters."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbAircraftList" Then
            If cmbAircraftList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Aircraft."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtReservationDate" Then
            'If CDate(txtReservationDate.Text) < CDate(lbltxtReceiptDate.Text.Trim) Then
            If CDate(txtReservationDate.Text) < Today.Date Then
                custValidator.ErrorMessage = "Reservation date should be greater than or equal to today date."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
        End If
        SetPage()
        ControlVisibility()
        'SetControlStatus(mComponentReservation.StatusID)
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "ComponentReservation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        setObject()
        If mComponentReservation.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
        Else
            RemoveSession()
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("ComponentReservationNew") And Not User.IsInRole("ComponentReservationEdit")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If IsValid Then
            Save()
        Else
            upnlTitle.Update()
        End If
    End Sub
    Private Sub cmbAircraftList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraftList.SelectedIndexChanged
        If mMachineNameValueList(New Guid(cmbAircraftList.SelectedValue)).ROCntxt Then
            Dim str As String = cmbAircraftList.SelectedItem.Text
            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, str & " ReadOnly Aircraft. <BR><BR>Selected aircraft is marked as readonly can not reserve component to it.", MsgBoxStyle.OkOnly, "ReadOnlyAircraft")
            Exit Sub
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

   
End Class