'AJAX Conversion By Vikrant On 29-Jun-2015

Public Class wfMEL_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mMELList As MELList
    Public mMELCategoryList As MELCategoryList
    Public mMEL As MEL
    Public strMsg As String = ""
    Public mMachine As Machine
    Dim EventLogID As Guid
    Dim PartTypeId As Guid = Guid.Empty
#End Region

#Region " Business Methods "
    Private Sub SetSession()
        Session("mMEL") = mMEL
        Session("mMELList") = mMELList
    End Sub
    Private Sub GetSession()
        mMEL = Session("mMEL")
        mMELList = Session("mMELList")
        mMachine = Session("mMachine")
        PartTypeId = Session("PartTypeId")
    End Sub
    'Added By Vikrant On 26-Jun-2014
    Private Sub RemoveSession()
        Session.Remove("mMEL")
        Session.Remove("mMELList")
        Session.Remove("PartTypeId")
    End Sub
    'End
    Private Sub NewRecord()
        mMEL = MEL.NewMEL(Guid.NewGuid)
        Session("mMEL") = mMEL
    End Sub
    Private Sub ControlVisibility(Optional ByVal ForMELCategory As Boolean = False, Optional ByVal IsHours As Boolean = False)
        If ForMELCategory Then
            If cmbMELCategory.SelectedIndex = 1 Then
                chkIsInHours.Enabled = True
                txtFrequencyInDay.Enabled = True
                'txtFrequencyInDay.Text = "0"
                txtFrequencyInHours.Enabled = False
            Else
                chkIsInHours.Checked = False
                chkIsInHours.Enabled = False
                txtFrequencyInDay.Enabled = False
                txtFrequencyInHours.Enabled = False
                txtFrequencyInHours.Text = ""
            End If
        End If
        If IsHours Then
            If chkIsInHours.Checked = True Then
                txtFrequencyInDay.Text = "0"
                txtFrequencyInDay.Enabled = False
                txtFrequencyInHours.Enabled = True
            Else
                txtFrequencyInHours.Text = ""
                If cmbMELCategory.SelectedIndex = 1 Then
                    txtFrequencyInDay.Enabled = True
                End If
                'txtFrequencyInDay.Enabled = True
                txtFrequencyInHours.Enabled = False
            End If
        End If
        'Added By Vikrant On 07-Sep-2020 For ALL07092020
        dgMELList.Columns(4).HeaderText = IIf(AppSettings("MELSnagNomenclature") = "True", "Make ADD Qty.", "Make MEL Qty.")
        dgMELList.Columns(5).HeaderText = IIf(AppSettings("MELSnagNomenclature") = "True", "Dispatch ADD Qty.", "Dispatch MEL Qty.")
        'End
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mMEL = MEL.GetMEL(mId)
        Session("mMEL") = mMEL
    End Sub
    Private Sub SetObject()
        mMEL.MachineID = mMachine.ID
        mMEL.PartID = New Guid(cmbComponent.SelectedValue)
        mMEL.MakeMELQty = Val(txtMakeMELQty.Text)
        mMEL.FlyMELQty = Val(txtFlyMELQty.Text)
        mMEL.MELCategoryID = CInt(cmbMELCategory.SelectedValue)
        mMEL.Remark = txtRemark.Text.Trim
        mMEL.IsHours = chkIsInHours.Checked
        'mMEL.FrequencyValueFormatted = txtFrequency.Text.Trim
        mMEL.FrequencyInDays = Val(txtFrequencyInDay.Text)
        mMEL.FrequencyInHours = txtFrequencyInHours.Text.Trim
        'mMEL.IsHours = chkIsInHours.Checked
        Session("mMEL") = mMEL
    End Sub
    Private Sub addAttributes()
        txtMakeMELQty.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtMakeMELQty').value,event)")
        txtFlyMELQty.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtFlyMELQty').value,event)")
        txtFrequencyInDay.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtFrequencyInDay').value,event)")
        'txtFrequencyInHours.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtFrequencyInHours').value)")
        txtFrequencyInHours.Attributes.Add("onkeypress", "var key; if(window.event){ key = event.keyCode;}else if(event.which){ key = event.which;} return (key == 13 || key == 8 || key == 9 || (key >= 48 && key <= 58) )")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim ErrorCount As Integer = 0
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    Dim tmpPartName As String
                    Dim mtmpID As Guid
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mMEL = Session("mMEL")
                            tmpPartName = mMEL.PartName
                            mtmpID = mMEL.ID
                            MEL.DeleteMEL(mMEL.ID)
                            lbltitle.InnerText = "Minimum Equipment Details [New]"
                            NewRecord()
                            ClearValues()
                            DataFieldBind()
                            ControlVisibility(True, False)
                            upnlSave.Update()
                            upnlMELDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "MEl", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "MEl", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "MEl", MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "MEL", "Can't delete : " & tmpPartName & " is Currently in use", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                            End If
                            ErrorCount = ex.Errors.Count
                            NewRecord()
                            DataFieldBind()
                            upnlMELDetails.Update()
                        Finally
                            If ErrorCount = 0 Then
                                MarkLog(Util.Action.Delete, "MEL", tmpPartName, Util.ErrorType.NoError, mtmpID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ' And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub setPartID(Optional ByVal OnPageLoad As Boolean = False)
        If PartID.Value = String.Empty Then
            If OnPageLoad Then
                PartTypeId = Guid.Empty
                Session("PartTypeId") = PartTypeId
            Else
                PartTypeId = Session("PartTypeId")
            End If
        Else
            PartTypeId = New Guid(PartID.Value)
            Session("PartTypeId") = PartTypeId
        End If
    End Sub
    Private Sub ClearComboBoxValues()
        PartTypeId = Guid.Empty
        Session("PartTypeId") = PartTypeId
    End Sub
    Private Sub ClearValues()
        txtMakeMELQty.Text = ""
        txtFlyMELQty.Text = ""
        txtRemark.Text = ""
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        cmbComponent.DataSource = PartList.GetPartList("", "", "(SELECT)")
        'cmbComponent.DataBind()

        cmbMELCategory.DataSource = MELCategoryList.GetMELCategoryList("(SELECT)")
        'cmbMELCategory.DataBind()

        mMELList = MELList.GetMELList(mMachine.ID, Guid.Empty, Today.Date)
        Session("mMELList") = mMELList
        dgMELList.DataSource = mMELList
        'dgMELList.DataBind()

        lblResult.Text = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD List : ", "MEL List : ") & mMELList.Count & " Record(s) Found."
        'Added By Vikrant On 07-Sep-2020 For ALL07092020
        dgMELList.Columns(4).HeaderText = IIf(AppSettings("MELSnagNomenclature") = "True", "Make ADD Qty.", "Make MEL Qty.")
        dgMELList.Columns(5).HeaderText = IIf(AppSettings("MELSnagNomenclature") = "True", "Dispatch ADD Qty.", "Dispatch MEL Qty.")
        'End
        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'If custValidator.ControlToValidate = "txtFrequencyInHours" Then
        '    If chkIsInHours.Checked = True And txtFrequencyInHours.Text = "" Then
        '        custValidator.ErrorMessage = "Frequency In Hours Required."
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'End If
        '    ElseIf custValidator.ControlToValidate = "cmbMELCategory" Then
        '        If cmbMELCategory.SelectedIndex <= 0 Then
        '            custValidator.ErrorMessage = "MEL Category Required"
        '            e.IsValid = False
        '        Else
        '            e.IsValid = True
        '        End If
        '    End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal ByVale As System.EventArgs) Handles MyBase.Load
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If cmbComponent.Enabled = True Then
                cmbComponent.Focus()
            End If
            NewRecord()
            DataFieldBind()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "MEL", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("Sender") = ""
        RemoveSession() 'Added By Vikrant On 26-Jun-2014
        'Response.Redirect("wfMachine.aspx?BackPage=" & Request.QueryString("BackPage"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        SetObject()
        If mMEL.IsValid Then
            Try
                SetObject()
                mMEL.Save()
                cmbComponent.Focus()
                MarkLog(Util.Action.Save, "MEL", mMEL.PartName, Util.ErrorType.NoError, mMEL.ID, EventLogID)
                NewRecord()
                mMELList = Session("mMELList")
                DataFieldBind()
                lblResult.Text = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD List : ", "MEL List : ") & mMELList.Count & " Record(s) Found."

                lbltitle.InnerText = "Minimum Equipment Details [New]"
                txtMakeMELQty.Text = ""
                txtRemark.Text = ""
                txtFlyMELQty.Text = ""
                Session("Sender") = ""
                If cmbMELCategory.SelectedIndex = 1 Then
                    chkIsInHours.Enabled = True
                    txtFrequencyInDay.Enabled = True
                Else
                    If chkIsInHours.Checked = True Then
                        chkIsInHours.Checked = False
                    End If
                    chkIsInHours.Enabled = False
                    txtFrequencyInDay.Enabled = False
                End If
                MarkLog(Util.Action.Save, "MEL", mMEL.PartName, Util.ErrorType.HandledError, mMEL.ID, EventLogID)
                upnlMELDetails.Update()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "MEL", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "MEL", MsgBoxStyle.OkOnly, "")
                End If
            End Try
        Else
            If Not mMEL.IsValid Then
                For j As Integer = 0 To mMEL.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mMEL.GetBrokenRulesCollection(j).Description + "</BR>"
                Next
            End If

            If strMsg.Trim <> "" Then
                cvFrequency.ErrorMessage = strMsg.TrimEnd("</BR>")
                cvFrequency.IsValid = mMEL.IsValid
            End If
            upnlValidationSummary.Update()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        NewRecord()
        mMELList = Session("mMELList")
        DataFieldBind()
        lbltitle.InnerText = "Minimum Equipment Details [New]"
        txtMakeMELQty.Text = ""
        txtRemark.Text = ""
        txtFlyMELQty.Text = ""
        If cmbMELCategory.SelectedIndex = 1 Then
            chkIsInHours.Enabled = True
            txtFrequencyInDay.Enabled = True
        Else
            If chkIsInHours.Checked = True Then
                chkIsInHours.Checked = False
            End If
            chkIsInHours.Enabled = False
            txtFrequencyInDay.Enabled = False
            txtFrequencyInHours.Enabled = False
        End If
        If cmbComponent.Enabled = True Then
            cmbComponent.Focus()
        End If
    End Sub
    Private Sub dgMELList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgMELList.PageIndexChanging
        dgMELList.PageIndex = e.NewPageIndex
        Session("mMELList") = mMELList
        dgMELList.DataSource = mMELList
        dgMELList.DataBind()
    End Sub
    Private Sub dgMELList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMELList.RowCommand
        Dim Index As Integer
        Select Case e.CommandName
            Case "EditRec"
                dgMELList.DataSource = mMELList
                dgMELList.DataBind()

                Index = CInt(e.CommandArgument) + dgMELList.PageIndex * dgMELList.PageSize

                mMEL = MEL.GetMEL(New Guid(dgMELList.DataKeys(Index).Value.ToString))
                Session("mMEL") = mMEL

                cmbComponent.SelectedValue = mMEL.PartID.ToString
                'txtDescription.Text = mMEL.Description
                'chkIsInHours.Checked = mMEL.IsHours
                'txtFrequencyInDay.Text = mMEL.FrequencyInDays
                'txtFrequencyInHours.Text = mMEL.FrequencyInHours
                txtMakeMELQty.Text = mMEL.MakeMELQty
                txtFlyMELQty.Text = mMEL.FlyMELQty
                cmbMELCategory.SelectedValue = mMEL.MELCategoryID.ToString
                txtRemark.Text = mMEL.Remark
                DataBind()

                ControlVisibility(True, mMEL.IsHours)

                lbltitle.InnerText = "Minimum Equipment Details" & " [" & mMEL.PartName & "]"

                If cmbComponent.Enabled = True Then
                    cmbComponent.Focus()
                End If
                MarkLog(Util.Action.Edit, "MEL", mMEL.PartName, Util.ErrorType.NoError, mMEL.ID, EventLogID)

                upnlMELDetails.Update()
                upnlFreq.Update()
            Case "DeleteRec"
                dgMELList.DataSource = mMELList
                dgMELList.DataBind()
                Index = CInt(e.CommandArgument) + dgMELList.PageIndex * dgMELList.PageSize
                Dim mId As Guid = New Guid(dgMELList.DataKeys(Index).Value.ToString)
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub dgMELList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMELList.Sorting
        mMELList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMELList") = mMELList
        dgMELList.DataSource = mMELList
        dgMELList.DataBind()
    End Sub
    Private Sub cmbComponent_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbComponent.SelectedIndexChanged
        mMEL.PartID = New Guid(cmbComponent.SelectedValue.ToString)
        txtDescription.Text = mMEL.Description
        If cmbComponent.SelectedIndex = 0 Then
            txtDescription.Text = ""
        End If
        cmbComponent.Focus()
    End Sub
    Private Sub cmbMELCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMELCategory.SelectedIndexChanged
        mMEL.MELCategoryID = CInt(cmbMELCategory.SelectedValue)
        txtFrequencyInDay.Text = mMEL.FrequencyInDays

        ControlVisibility(True, False)
        If cmbMELCategory.SelectedIndex = 1 Then
            txtFrequencyInDay.Text = "0"
        End If

        cmbMELCategory.Focus()
        upnlFreq.Update()
    End Sub
    Private Sub chkIsInHours_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsInHours.CheckedChanged
        ControlVisibility(False, True)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region



End Class