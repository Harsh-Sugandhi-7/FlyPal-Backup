'AJAX Conversion by Vikrant on 01-Jul-2015

Public Class wfMachineLeaseInfo_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Private mCurrencyList As CurrencyList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mCurrencyList = Session("mCurrencyList")
    End Sub
    Private Sub SetObject()
        mMachine.MachineOnLeaseDetails.Item(mMachine.MachineOnLeaseDetails.CurrentIndex).LeasedType = cmbLeasedType.SelectedValue
        mMachine.MachineOnLeaseDetails.Item(mMachine.MachineOnLeaseDetails.CurrentIndex).LeasedStartDate = txtLeaseStartDate.Text
        mMachine.MachineOnLeaseDetails.Item(mMachine.MachineOnLeaseDetails.CurrentIndex).LeasedEndDate = txtLeaseEndDate.Text
        mMachine.MachineOnLeaseDetails.Item(mMachine.MachineOnLeaseDetails.CurrentIndex).MinUtilizationHrs = Val(txtMinHrs.Text)
        mMachine.MachineOnLeaseDetails.Item(mMachine.MachineOnLeaseDetails.CurrentIndex).RateForMinUtilizationHrs = Val(txtRateForMinHrs.Text)
        mMachine.MachineOnLeaseDetails.Item(mMachine.MachineOnLeaseDetails.CurrentIndex).RateBeyondMinHrs = Val(txtRateBeyondMinHrs.Text)
        mMachine.MachineOnLeaseDetails.Item(mMachine.MachineOnLeaseDetails.CurrentIndex).CurrencyID = New Guid(cmbCurrency.SelectedValue)
        mMachine.MachineOnLeaseDetails.Item(mMachine.MachineOnLeaseDetails.CurrentIndex).CurrencyName = IIf(cmbCurrency.SelectedIndex > 0, cmbCurrency.SelectedItem.Text, "")
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
                            mMachine.MachineOnLeaseDetails.Remove(mMachine.MachineOnLeaseDetails(mMachine.MachineOnLeaseDetails.CurrentIndex))
                            For i As Integer = 0 To mMachine.MachineOnLeaseDetails.Count - 1
                                mMachine.MachineOnLeaseDetails(i).SerialNo = i + 1
                            Next
                            Session("mMachine") = mMachine
                            Session("mMachineLeaseInfoEdit") = False
                            dgLeasedInformationList.DataSource = mMachine.MachineOnLeaseDetails
                            dgLeasedInformationList.DataBind()
                            lblResult.Text = "List of Leased Information: " & mMachine.MachineOnLeaseDetails.Count & " Record(s) found"
                            ClearControls()
                            upnlGrid.Update()
                            upnlLeaseInfoDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub addAttributes()
        txtMinHrs.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtMinHrs').value,event)")
        txtRateForMinHrs.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtRateForMinHrs').value,event)")
        txtRateBeyondMinHrs.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtRateBeyondMinHrs').value,event)")
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        cmbLeasedType.SelectedValue = mMachine.MachineOnLeaseDetails.Item(ID).LeasedType
        txtLeaseStartDate.Text = mMachine.MachineOnLeaseDetails.Item(ID).LeasedStartDateFormatted
        txtLeaseEndDate.Text = mMachine.MachineOnLeaseDetails.Item(ID).LeasedEndDateFormatted
        txtMinHrs.Text = mMachine.MachineOnLeaseDetails.Item(ID).MinUtilizationHrs
        txtRateForMinHrs.Text = mMachine.MachineOnLeaseDetails.Item(ID).RateForMinUtilizationHrs
        txtRateBeyondMinHrs.Text = mMachine.MachineOnLeaseDetails.Item(ID).RateBeyondMinHrs
        cmbCurrency.SelectedValue = mMachine.MachineOnLeaseDetails.Item(ID).CurrencyID.ToString
        'cmbCurrency.SelectedItem.Text = mMachine.MachineOnLeaseDetails.Item(ID).CurrencyName

        lblTitle.InnerText = "Aircraft Leased Information Details" & " [" & mMachine.MachineOnLeaseDetails.Item(ID).LeasedType & "]"

        upnlLeaseInfoDetails.DataBind()
    End Sub
    Private Function CustomValidate1() As Boolean
        Dim strMSG As String = ""
        If Not mMachine.IsValid Then
            For i As Integer = 0 To mMachine.MachineOnLeaseDetails.CurrentItem.GetBrokenRulesCollection.Count - 1
                strMSG = strMSG + mMachine.MachineOnLeaseDetails.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If strMSG.Trim <> "" Then
            cvDate.ErrorMessage = strMSG
            cvDate.IsValid = False
            Return False
        End If
        Return True
    End Function
    'Added By Vikrant On 26-Jun-2014
    Private Sub RemoveSession()
        Session.Remove("mCurrencyList")
        Session.Remove("mMachineLeaseInfoEdit")
    End Sub
    'End
    Private Sub ClearControls()
        cmbCurrency.ClearSelection()
        cmbLeasedType.ClearSelection()
        txtLeaseStartDate.Text = ""
        txtLeaseEndDate.Text = ""
        txtMinHrs.Text = "0"
        txtRateBeyondMinHrs.Text = "0"
        txtRateForMinHrs.Text = "0"
        lblTitle.InnerText = "Aircraft Leased Information Details [NEW]"
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgLeasedInformationList.DataSource = mMachine.MachineOnLeaseDetails
        mCurrencyList = CurrencyList.GetCurrencyList("", "", True)
        cmbCurrency.DataSource = mCurrencyList
        Session("mCurrencyList") = mCurrencyList
        DataBind()
        lblResult.Text = "List of Leased Information: " & mMachine.MachineOnLeaseDetails.Count & " Record(s) found"
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If cmbLeasedType.Enabled = True Then
                cmbLeasedType.Focus()
            End If
            DataFieldBind()
            ClearControls()
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If Not IsValid Then
            upnlValidationSummary.Update()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
            Exit Sub
        End If

        If Session("mMachineLeaseInfoEdit") = False Then
            mMachine.MachineOnLeaseDetails.Add(cmbLeasedType.SelectedItem.Text, mMachine.ID, New Guid(cmbCurrency.SelectedValue), IIf(cmbCurrency.SelectedIndex > 0, cmbCurrency.SelectedItem.Text, ""), txtLeaseStartDate.Text, txtLeaseEndDate.Text, Val(txtMinHrs.Text), Val(txtRateForMinHrs.Text), Val(txtRateBeyondMinHrs.Text))
            If Not CustomValidate1() Then
                mMachine.MachineOnLeaseDetails.Remove(mMachine.MachineOnLeaseDetails.CurrentItem)
                upnlValidationSummary.Update()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                Exit Sub
            End If

            For i As Integer = 0 To mMachine.MachineOnLeaseDetails.Count - 1
                mMachine.MachineOnLeaseDetails(i).SerialNo = i + 1
            Next
            Session("mMachine") = mMachine
            cmbLeasedType.Focus()
        Else
            SetObject()
            If Not CustomValidate1() Then
                upnlValidationSummary.Update()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                Exit Sub
            End If
            Session("mMachine") = mMachine
            cmbLeasedType.Focus()
            Session("mMachineLeaseInfoEdit") = False
        End If
        DataFieldBind()
        ClearControls()
        upnlGrid.Update()
        upnlLeaseInfoDetails.Update()

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub dgLeasedInformationList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLeasedInformationList.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgLeasedInformationList.PageIndex * dgLeasedInformationList.PageSize
                If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                mMachine.MachineOnLeaseDetails.CurrentIndex = Index
                Session("mMachine") = mMachine
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgLeasedInformationList.PageIndex * dgLeasedInformationList.PageSize
                mMachine.MachineOnLeaseDetails.CurrentIndex = Index
                Dim mID As New Guid(dgLeasedInformationList.DataKeys(Index).Value.ToString)
                EditRecord(mID)
                Session("mMachineLeaseInfoEdit") = True
                Session("mMachine") = mMachine
                upnlLeaseInfoDetails.Update()
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        RemoveSession() 'Added By Vikrant On 26-Jun-2014
        ' Response.Redirect("wfMachine_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub
    Private Sub dgLeasedInformationList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgLeasedInformationList.Sorting
        mMachine.MachineOnLeaseDetails.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMachine") = mMachine
        dgLeasedInformationList.DataSource = mMachine.MachineOnLeaseDetails
        dgLeasedInformationList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    
End Class