Imports System.Linq


Public Class wfPBHList
    Inherits System.Web.UI.Page


#Region " Variable Declaration "
    Public mPBHList As PBHList
    Public mPBH As PBH
    Private mMachineNameValueList As MachineNameValueList
    Dim EventLogID As Guid
    Public mNewPBH As PBH
    Public mOldPBH As PBH
    Public mRenewPBH As PBH
    Public previousSubscribedHr As String
    Public previousRemHr As String
    Public mExistingPBH As PBH
    Public mIsOnlyHoursExtended As Boolean
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mPBHList = Session("mPBHList")
        previousSubscribedHr = Session("previousSubscribedHr")
        previousRemHr = Session("previousRemHr")
        mIsOnlyHoursExtended = Session("mIsOnlyHoursExtended")
    End Sub
    Private Sub SetSession()
        Session("mPBHList") = mPBHList
        Session("previousSubscribedHr") = previousSubscribedHr
        Session("previousRemHr") = previousRemHr
        Session("mIsOnlyHoursExtended") = mIsOnlyHoursExtended
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPBHList")
        Session.Remove("previousSubscribedHr")
        Session.Remove("previousRemHr")
        Session.Remove("mIsOnlyHoursExtended")
    End Sub
    Private Sub ControlVisibility()
        'For i As Integer = 0 To dgPBHList.Rows.Count - 1
        '    Dim cmbValue As DropDownList

        '    'cmbValue = CType(Me.dgPBHList.Rows(i).FindControl("cmbPrimaryPBHList"), DropDownList)
        '    'If cmbValue.SelectedIndex <= 0 Then
        '    '    btnUpdate.Enabled = False
        '    '    btnUpdateBottom.Enabled = False
        '    '    Exit Sub
        '    'Else
        '    '    btnUpdate.Enabled = True
        '    '    btnUpdateBottom.Enabled = True
        '    'End If
        'Next

        chkIsCombinedHrs.Checked = mPBHList.IsCombinedHrs

        If chkIsCombinedHrs.Checked Then
            cmbAircraftList.Enabled = False
            cmbAircraftList.Enabled = False
            spnAircraftStar.Visible = False
            txtStartHours.Enabled = False
            txtCurrentHours.Enabled = False
            lblCurrentHours1.Visible = False

        Else
            cmbAircraftList.Enabled = True
            spnAircraftStar.Visible = True
            txtStartHours.Enabled = True
            txtCurrentHours.Enabled = True
            lblCurrentHours1.Visible = True

        End If
        chkIsCombinedHrs.Enabled = Not mPBHList.IsCombinedHrs
        dgPBHList.Columns(1).Visible = Not mPBHList.IsCombinedHrs
        dgPBHList.Columns(2).Visible = Not mPBHList.IsCombinedHrs
        dgPBHList.Columns(3).Visible = Not mPBHList.IsCombinedHrs
        dgPBHList.Columns(19).Visible = Not mPBHList.IsCombinedHrs
        btnAdd.Visible = Not mPBHList.IsCombinedHrs
        btnADDBottom.Visible = Not mPBHList.IsCombinedHrs
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim TempLogID As Guid
                        Try
                            mPBH = Session("mPBH")
                            PBH.DeletePBH(mPBH.ID)
                            mPBHList = PBHList.GetList(1)
                            Dim mtmpPBHList = (From c As PBH In mPBHList Order By c.RemainingHoursDec, c.RemainingDays
                                               Select (c))
                            dgPBHList.DataSource = mPBHList
                            Session("mPBHList") = mPBHList
                            dgPBHList.DataBind()
                            SetGridColor()
                            upnlList.Update()
                            ' MarkLog(Util.Action.Delete, "Flight Log", mLogDetail, Util.ErrorType.NoError, mLogList.Item(mLogList.CurrentIndex).ID, EventLogID)

                        Catch ex As SqlException

                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show("Deletion Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
                            End If

                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "Flight Log", "Deleted SuccessFully : " & mLogDetail, Util.ErrorType.NoError, TempLogID, EventLogID)

                                MSGBoxCtrl.show(MSGBox.Message_title.DeletedSuccessFully, MSGBox.Message_text.DeletedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                            End If
                        End Try

                    End If
                    If MSGBoxCtrl.Sender = "CarryForward" Then
                        RenewPBH(True)
                    End If
                Case MsgBoxResult.No
                    ' '' ''Response.Redirect("wfLogList.aspx?MsgResult=0&BackPage=")
                    If MSGBoxCtrl.Sender = "CarryForward" Then
                        RenewPBH(False)
                    End If
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added


                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"  'Code Added

            End Select
        ElseIf Result1 = -1 Then
            ' '' ''Response.Redirect("wfLogList.aspx?MsgResult=0&BackPage=")
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
    Private Sub DeletePBH()
        PBH.DeletePBH(mPBH.ID)
        mPBHList = PBHList.GetList(IsAllRecordsRequired:=1)
        Dim mtmpPBHList = (From c As PBH In mPBHList Order By c.RemainingHoursDec, c.RemainingDays
                           Select (c))
        dgPBHList.DataSource = mPBHList
        Session("mPBHList") = mPBHList
        dgPBHList.DataBind()
        SetGridColor()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtStartDate" Then
            mOldPBH = Session("mOldPBH")
            'If txtStartDate.Text.ToString = "" And txtStartHours.Text = "0:00" Then
            '    custValidator.ErrorMessage = "Enter atleast Start Date or Star Hours"
            '    e.IsValid = False
            If txtStartDate.Text.ToString = "" Then
                custValidator.ErrorMessage = "Enter Start Date"
                e.IsValid = False
            ElseIf txtStartDate.Text.ToString <> "" And txtDaysFreq.Text = "0" Then
                custValidator.ErrorMessage = "Days Frequency required."
                e.IsValid = False
            ElseIf Not mOldPBH Is Nothing Then
                If txtStartDate.Text.ToString <> "" And CDate(mOldPBH.StartDate.ToString) >= CDate(txtStartDate.Text.ToString) And mIsOnlyHoursExtended = False Then
                    custValidator.ErrorMessage = "Start Date should be greater than previous Subscription Date."
                    e.IsValid = False
                ElseIf txtStartDate.Text.ToString <> "" Then
                    If mOldPBH.LastLogDetails.ToString <> "" Then
                        If CDate(mOldPBH.LastLogDetails.ToString) > CDate(txtStartDate.Text.ToString) And mIsOnlyHoursExtended = False Then
                            custValidator.ErrorMessage = "Start Date should be greater than previous Log Date."
                            e.IsValid = False
                        End If
                    End If
                End If

            End If
        ElseIf custValidator.ControlToValidate = "txtStartHours" Then
            If (txtStartHours.Text <> "0:00" And txtHoursFrequency.Text = "0:00") Or (txtHoursFrequency.Text = "0:00" And chkIsCombinedHrs.Checked = True) Then
                custValidator.ErrorMessage = "Hours Frequency required."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbAircraftList" Then
            If cmbAircraftList.SelectedIndex <= 0 And chkIsCombinedHrs.Checked = False Then
                custValidator.ErrorMessage = "Aircraft required."
                e.IsValid = False
            End If
        End If

    End Sub
#End Region

#Region " DataFieldBind "
    Public Sub GridBind()
        mPBHList = PBHList.GetList(IsAllRecordsRequired:=1)


        Dim mtmpPBHList = (From c As PBH In mPBHList Order By c.RemainingHoursDec, c.RemainingDays
                           Select (c))
        dgPBHList.DataSource = mPBHList
        Session("mPBHList") = mPBHList

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, SkipIsForInventoryAircarft:=True, IsTagRequired:=True, TagText:="(SELECT)")
        cmbAircraftList.DataSource = mMachineNameValueList

        cmbAircraftList.DataBind()   'Added Code
        Session("AircraftId") = cmbAircraftList.SelectedValue

        Session("mMachineNameValueList") = mMachineNameValueList
        DataBind()
        SetGridColor()
        lblResult.Text = "List Aircraft(s) on  :" & mPBHList.Count & " Record(s) found."

        cmbMachineList.DataSource = mMachineNameValueList
        cmbMachineList.DataBind()


    End Sub
    Public Sub SetGridColor()
        Dim RemainingDays As Integer = 0
        Dim RemainingHoursDec As Integer = 0
        Dim HoursFrequencyDec As Integer = 0

        For j As Integer = 0 To dgPBHList.Rows.Count - 1


            RemainingDays = CType(dgPBHList.Rows.Item(j).Cells(7).Text, Integer)
            RemainingHoursDec = CType(dgPBHList.Rows.Item(j).Cells(15).Text, Decimal)
            HoursFrequencyDec = CType(dgPBHList.Rows.Item(j).Cells(16).Text, Decimal)

            If RemainingDays <= 0 OrElse (HoursFrequencyDec > 0 And RemainingHoursDec <= 0) Then
                dgPBHList.Rows.Item(j).BackColor = Color.OrangeRed
                dgPBHList.Rows.Item(j).ToolTip = "Subscription Expired"
                dgPBHList.Rows.Item(j).ForeColor = Color.White

            ElseIf RemainingDays < 30 OrElse (HoursFrequencyDec > 0 And RemainingHoursDec < 1800) Then '1800=30 Hrs
                dgPBHList.Rows.Item(j).BackColor = Color.Yellow
                dgPBHList.Rows.Item(j).ToolTip = "Subscription Expiring"
                dgPBHList.Rows.Item(j).ForeColor = Color.Black
            End If


        Next
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            GridBind()
            ControlVisibility()
        End If

    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        ' AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
        MessageBoxResult()
    End Sub
    Private Sub RenewPBH(ByVal IsCarryforward As Boolean, Optional IsOnlyHoursExtended As Boolean = False)



        mOldPBH = Session("mOldPBH")
        Dim mRenewPBH As PBH = PBH.ReNewPBH(mOldPBH, IsOnlyHoursExtended)

        If IsCarryforward Then
            mRenewPBH.CarryForwardHours = mOldPBH.RemainingHours.ToString
            mRenewPBH.RemainingHours = New Period(1, mRenewPBH.HoursFrequencyDec + mRenewPBH.CarryForwardHoursDec, 1).Value

            If mRenewPBH.CarryForwardHoursDec < 0 Then
                mRenewPBH.ElapsedHours = New Period(1, mRenewPBH.HoursFrequencyDec - mRenewPBH.RemainingHoursDec, 1).Value
            End If
        Else
            mRenewPBH.CarryForwardHours = New Period(1, 0, 1).Value
            mRenewPBH.RemainingHours = New Period(1, mRenewPBH.HoursFrequencyDec + mRenewPBH.CarryForwardHoursDec, 1).Value
        End If
        cmbAircraftList.Enabled = False

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, SkipIsForInventoryAircarft:=True, IsTagRequired:=True, TagText:="(SELECT)", IsForPBH:=True) ''IsForPBH=True only when renewal
        cmbAircraftList.DataSource = mMachineNameValueList
        cmbAircraftList.DataBind()

        Session("mRenewPBH") = mRenewPBH

        cmbAircraftList.SelectedValue = mRenewPBH.MachineID.ToString
        txtStartHours.Text = mRenewPBH.StartHours.ToString
        txtHoursFrequency.Text = mRenewPBH.HoursFrequency.ToString
        txtEndHours.Text = mRenewPBH.EndHours.ToString
        txtCurrentHours.Text = mRenewPBH.CurrentHours.ToString
        txtElaspedHrs.Text = mRenewPBH.ElapsedHours.ToString
        txtRemaining.Text = mRenewPBH.RemainingHours.ToString
        txtStartDate.Text = mRenewPBH.StartDateFormatted
        txtDaysFreq.Text = mRenewPBH.DaysFrequency.ToString
        txtCarryForwardHours.Text = mRenewPBH.CarryForwardHours.ToString

        If Not mRenewPBH.EndDateFormatted Is System.DBNull.Value Then
            txtEndDate.Text = mRenewPBH.EndDateFormatted
        End If
        chkIsCombinedHrs.Checked = mRenewPBH.IsCombinedHours
        chkIsCombinedHrs.Enabled = False

        pnlPBH.Visible = True
        upnlChangePBH.Update()
        mdlPopUpPBH.Show()
        'GridBind()
    End Sub
    Private Sub dgPBHList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPBHList.RowCommand

        Select Case e.CommandName
            Case "Renew"
                ' Dim index As Integer = CInt(e.CommandArgument) + dgPBHList.PageIndex * dgPBHList.PageSize
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mOldPBH = mPBHList(mID)
                Session("mOldPBH") = mOldPBH
                mIsOnlyHoursExtended = False
                If mOldPBH.RemainingHoursDec > 0 Then
                    MSGBoxCtrl.Show("Alert..!!", "Do you want to carry forward remaining hours?", "", MsgBoxStyle.YesNo, "CarryForward")
                    Exit Sub
                ElseIf mOldPBH.RemainingHoursDec < 0 Then 'Overflying happens then directly carryforward for further calculation
                    RenewPBH(True)
                Else
                    RenewPBH(False)
                End If

            Case "DeleteRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mPBH = mPBHList(mID)


                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                Session("mPBH") = mPBH
            Case "BorrowRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mPBH = mPBHList(mID)
                Session("mPBH") = mPBH

                txtElapsed.Text = mPBH.ElapsedHours
                txtHourFreq.Text = mPBH.HoursFrequency
                txtRem.Text = mPBH.RemainingHours
                txtElapsed.DataBind()
                lbltitleborrow.Text = "Hour(s) Borrowing for " + mPBH.RegNo
                txtRem.DataBind()
                txtHourFreq.DataBind()
                txtAvlHrs.Text = ""
                txtHours.Text = ""
                lblPbhHeader.Text = "Details of " + mPBH.RegNo + " PBH Aircraft"
                cmbMachineList.SelectedIndex = 0
                pnlBorrowPBH.Visible = True
                pnlBorrowInnerPBH.Visible = True
                upnlBorrowPBH.Update()
                mdlPopUpPBHBorrow.Show()
            Case "ExtensionRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mOldPBH = mPBHList(mID)
                Session("mOldPBH") = mOldPBH
                mIsOnlyHoursExtended = True
                Session("mIsOnlyHoursExtended") = mIsOnlyHoursExtended
                RenewPBH(IsCarryforward:=False, IsOnlyHoursExtended:=True)
        End Select
    End Sub

    Private Sub dgPBHList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPBHList.Sorting
        mPBHList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPBHList") = mPBHList
        Dim mtmpPBHList = (From c As PBH In mPBHList Order By c.RemainingHoursDec, c.RemainingDays
                           Select (c))
        dgPBHList.DataSource = mPBHList
        dgPBHList.DataBind()
        SetGridColor()
        upnlList.Update()
    End Sub
    Private Sub dgPBHList_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgPBHList.PageIndexChanged
        dgPBHList.PageIndex = e.NewPageIndex
        Dim mtmpPBHList = (From c As PBH In mPBHList Order By c.RemainingHoursDec, c.RemainingDays
                           Select (c))
        dgPBHList.DataSource = mPBHList
        Session("mPBHList") = mPBHList
        dgPBHList.DataBind()
        SetGridColor()
        upnlList.Update()
    End Sub
    Private Sub btnPBHClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPBHClose.Click
        pnlPBH.Visible = False
        mIsOnlyHoursExtended = False
        upnlChangePBH.Update()
        mdlPopUpPBH.Hide()
        cmbAircraftList.ClearSelection()
    End Sub

    Private Sub btnPBH_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPBH.Click
        Try
            If Page.IsValid Then


                If Not Session("mRenewPBH") Is Nothing Then

                    mRenewPBH = Session("mRenewPBH")

                    If mRenewPBH.ModelDetails = "" And mRenewPBH.IsCombinedHours = False Then
                        Dim aMac As Machine = Machine.GetMachine(mRenewPBH.MachineID)
                        mRenewPBH.ModelDetails = aMac.AssemblyStatus.Assembly.ModelName + " (" + aMac.AssemblyStatus.Assembly.SerialNo + ")" ' mMachineNameValueList(New Guid(cmbAircraftList.SelectedValue.ToString)).AssemblySerialNo
                    End If

                    If mRenewPBH.IsValid Then
                        mRenewPBH.Save()
                        pnlPBH.Visible = False

                        mdlPopUpPBH.Hide()

                        GridBind()
                        ControlVisibility()
                        upnlChangePBH.Update()
                        upnlList.Update()
                        cmbAircraftList.ClearSelection()
                        Session("mNewPBH") = Nothing
                    Else
                        Dim str As String = ""
                        For i As Integer = 0 To mRenewPBH.GetBrokenRulesCollection.Count - 1
                            str = str + mRenewPBH.GetBrokenRulesCollection(i).Description + vbCrLf
                        Next
                        cvModelList.ErrorMessage = str
                        cvModelList.IsValid = False
                        upnlValidations.Update()
                    End If

                Else

                    mNewPBH = Session("mNewPBH")
                    mNewPBH.IsCombinedHours = chkIsCombinedHrs.Checked

                    If Not chkIsCombinedHrs.Checked Then
                        mNewPBH.MachineID = New Guid(cmbAircraftList.SelectedValue.ToString)
                        Dim aMac As Machine = Machine.GetMachine(mNewPBH.MachineID)

                        mNewPBH.ModelDetails = aMac.AssemblyStatus.Assembly.ModelName + " (" + aMac.AssemblyStatus.Assembly.SerialNo + ")" ' mMachineNameValueList(New Guid(cmbAircraftList.SelectedValue.ToString)).AssemblySerialNo

                    End If


                    mNewPBH.StartHours = txtStartHours.Text

                    mNewPBH.CurrentHours = txtCurrentHours.Text

                    mNewPBH.EndHours = txtEndHours.Text
                    If mNewPBH.IsValid Then

                        mNewPBH.Save()
                        pnlPBH.Visible = False

                        mdlPopUpPBH.Hide()
                        Session("mNewPBH") = Nothing

                        GridBind()
                        ControlVisibility()
                        upnlChangePBH.Update()
                        upnlList.Update()
                        cmbAircraftList.ClearSelection()
                        Session("mRenewPBH") = Nothing
                    Else
                        Dim str As String = ""
                        For i As Integer = 0 To mNewPBH.GetBrokenRulesCollection.Count - 1
                            str = str + mNewPBH.GetBrokenRulesCollection(i).Description + vbCrLf
                        Next
                        cvModelList.ErrorMessage = str
                        cvModelList.IsValid = False
                        upnlValidations.Update()
                    End If
                End If
            Else
                upnlValidations.Update()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnADDBottom.Click
        mNewPBH = PBH.NewPBH()
        Session("mNewPBH") = mNewPBH

        txtEndHours.Text = mNewPBH.EndHours
        txtEndHours.DataBind()

        txtRemaining.Text = mNewPBH.RemainingHours
        txtRemaining.DataBind()

        txtElaspedHrs.Text = mNewPBH.ElapsedHours
        txtElaspedHrs.DataBind()

        txtCurrentHours.Text = mNewPBH.CurrentHours
        txtCurrentHours.DataBind()

        txtStartHours.Text = mNewPBH.StartHours
        txtStartHours.DataBind()

        txtHoursFrequency.Text = mNewPBH.HoursFrequency
        txtHoursFrequency.DataBind()
        'If Not mNewPBH.StartDateFormatted Is System.DBNull.Value Then
        '    txtStartDate.Text = mNewPBH.StartDateFormatted
        'End If
        'txtStartDate.DataBind()
        'If Not mNewPBH.EndDateFormatted Is System.DBNull.Value Then
        '    txtEndDate.Text = mNewPBH.EndDateFormatted
        'End If
        'txtEndDate.DataBind()

        txtDaysFreq.Text = mNewPBH.DaysFrequency
        txtDaysFreq.DataBind()

        txtCarryForwardHours.Text = mNewPBH.CarryForwardHours
        txtCarryForwardHours.DataBind()


        txtStartDate.Text = ""
        txtEndDate.Text = ""

        cmbAircraftList.SelectedIndex = 0
        cmbAircraftList.Enabled = True
        Session("mOldPBH") = Nothing
        Session("mRenewPBH") = Nothing
        pnlPBH.Visible = True
        upnlChangePBH.Update()
        mdlPopUpPBH.Show()
    End Sub


    Private Sub txtHoursFrequency_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHoursFrequency.TextChanged, txtStartHours.TextChanged
        If Not Session("mNewPBH") Is Nothing Then
            mNewPBH = Session("mNewPBH")
            mNewPBH.StartHours = txtStartHours.Text
            mNewPBH.HoursFrequency = txtHoursFrequency.Text

            txtEndHours.Text = mNewPBH.EndHours
            txtEndHours.DataBind()

            txtRemaining.Text = mNewPBH.RemainingHours
            txtRemaining.DataBind()

            txtCurrentHours.Text = mNewPBH.CurrentHours
            txtCurrentHours.DataBind()


            txtElaspedHrs.Text = mNewPBH.ElapsedHours
            txtElaspedHrs.DataBind()


            If Not mNewPBH.IsValid Then
                Dim str As String = ""
                For i As Integer = 0 To mNewPBH.GetBrokenRulesCollection.Count - 1
                    str = str + mNewPBH.GetBrokenRulesCollection(i).Description + vbCrLf
                Next
                cvModelList.ErrorMessage = str
                cvModelList.IsValid = False
                upnlValidations.Update()
            End If
            Session("mNewPBH") = mNewPBH
        Else
            mRenewPBH = Session("mRenewPBH")


            mRenewPBH.StartHours = txtStartHours.Text
            mRenewPBH.HoursFrequency = txtHoursFrequency.Text
            txtEndHours.Text = mRenewPBH.EndHours
            txtEndHours.DataBind()


            If mRenewPBH.IsRenewed Then
                mOldPBH = Session("mOldPBH")

                If mIsOnlyHoursExtended = False Then
                    txtRemaining.Text = New Period(1, mRenewPBH.HoursFrequencyDec + mRenewPBH.CarryForwardHoursDec, 1).Value
                Else
                    Dim mExt As Decimal = 0
                    mExt = New Period(1, mRenewPBH.HoursFrequencyDec - mOldPBH.HoursFrequencyDec, 1).DbValueDec
                    txtRemaining.Text = New Period(1, mOldPBH.RemainingHoursDec + mExt, 1).Value
                End If

                mRenewPBH.RemainingHours = txtRemaining.Text
                If mOldPBH.RemainingHoursDec > 0 Then
                    mRenewPBH.ElapsedHours = New Period(1, (mRenewPBH.HoursFrequencyDec + mRenewPBH.CarryForwardHoursDec) - mRenewPBH.RemainingHoursDec, 1, False, False).Value
                Else
                    mRenewPBH.ElapsedHours = New Period(1, 0, 1, False, False).Value

                End If
                ' mRenewPBH.ElapsedHours = New Period(1, 0, 1, False, False).Value
            Else
                txtRemaining.Text = mRenewPBH.RemainingHours
                mRenewPBH.ElapsedHours = New Period(1, 0, 1, False, False).Value

            End If

            txtRemaining.DataBind()



            txtElaspedHrs.Text = mRenewPBH.ElapsedHours
            txtElaspedHrs.DataBind()

            txtCurrentHours.Text = mRenewPBH.CurrentHours
            txtCurrentHours.DataBind()


            If Not mRenewPBH.IsValid Then
                Dim str As String = ""
                For i As Integer = 0 To mRenewPBH.GetBrokenRulesCollection.Count - 1
                    str = str + mRenewPBH.GetBrokenRulesCollection(i).Description + vbCrLf
                Next
                cvModelList.ErrorMessage = str
                cvModelList.IsValid = False
                upnlValidations.Update()
            End If
            Session("mRenewPBH") = mRenewPBH
        End If



    End Sub

    Private Sub txtDaysFreq_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDaysFreq.TextChanged, txtStartDate.TextChanged


        If Not Session("mNewPBH") Is Nothing Then
            mNewPBH = Session("mNewPBH")
            If Not IsDate(txtStartDate.Text) Then
                mNewPBH.StartDate = System.DBNull.Value
            Else
                mNewPBH.StartDate = txtStartDate.Text
            End If

            mNewPBH.DaysFrequency = txtDaysFreq.Text


            If Not mNewPBH.EndDateFormatted Is System.DBNull.Value Then
                txtEndDate.Text = mNewPBH.EndDateFormatted
            End If
            'txtEndDate.Text = mNewPBH.EndDateFormatted


            mNewPBH.HoursFrequency = txtHoursFrequency.Text
            txtEndHours.Text = mNewPBH.EndHours
            txtEndHours.DataBind()

            txtRemaining.Text = mNewPBH.RemainingHours
            txtRemaining.DataBind()

            txtElaspedHrs.Text = mNewPBH.ElapsedHours
            txtElaspedHrs.DataBind()

            If Not mNewPBH.IsValid Then
                Dim str As String = ""
                For i As Integer = 0 To mNewPBH.GetBrokenRulesCollection.Count - 1
                    str = str + mNewPBH.GetBrokenRulesCollection(i).Description + vbCrLf
                Next
                cvModelList.ErrorMessage = str
                cvModelList.IsValid = False
                upnlValidations.Update()
            End If
            Session("mNewPBH") = mNewPBH

        Else
            mRenewPBH = Session("mRenewPBH")
            If Not IsDate(txtStartDate.Text) Then
                mRenewPBH.StartDate = System.DBNull.Value
            Else
                mRenewPBH.StartDate = txtStartDate.Text
            End If

            mRenewPBH.DaysFrequency = txtDaysFreq.Text


            If Not mRenewPBH.EndDateFormatted Is System.DBNull.Value Then
                txtEndDate.Text = mRenewPBH.EndDateFormatted
            End If
            'txtEndDate.Text = mNewPBH.EndDateFormatted


            mRenewPBH.HoursFrequency = txtHoursFrequency.Text
            txtEndHours.Text = mRenewPBH.EndHours
            txtEndHours.DataBind()

            txtRemaining.Text = mRenewPBH.RemainingHours
            txtRemaining.DataBind()

            txtElaspedHrs.Text = mRenewPBH.ElapsedHours
            txtElaspedHrs.DataBind()

            If Not mRenewPBH.IsValid Then
                Dim str As String = ""
                For i As Integer = 0 To mRenewPBH.GetBrokenRulesCollection.Count - 1
                    str = str + mRenewPBH.GetBrokenRulesCollection(i).Description + vbCrLf
                Next
                cvModelList.ErrorMessage = str
                cvModelList.IsValid = False
                upnlValidations.Update()
            End If
            Session("mRenewPBH") = mRenewPBH
        End If


    End Sub
    Private Sub cmbAircraftList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraftList.SelectedIndexChanged
        If cmbAircraftList.SelectedIndex > 0 And Not Session("mNewPBH") Is Nothing Then
            mPBHList = Session("mPBHList")
            If mPBHList.Contains(New Guid(cmbAircraftList.SelectedValue.ToString), "") Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                cmbAircraftList.SelectedIndex = 0
            End If
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        mIsOnlyHoursExtended = False
        Response.Redirect("index.aspx")
    End Sub
    Private Sub btnBorrowPBHClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBorrowPBHClose.Click
        pnlBorrowPBH.Visible = False
        upnlBorrowPBH.Update()
        mdlPopUpPBHBorrow.Hide()



    End Sub

    Private Sub txtHours_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHours.TextChanged
        mPBH = Session("mPBH")
        mExistingPBH = Session("mExistingPBH")

        mPBH = PBH.GetPBH(mPBH.ID)
        mExistingPBH = PBH.GetPBH(mExistingPBH.ID)

        Dim NewHours As New Period(1, DBNull.Value)
        NewHours.Value = txtHours.Text

        If NewHours.DbValueDec > mExistingPBH.RemainingHoursDec Then
            MSGBoxCtrl.show("Alert..!!", "Please Enter Hours less than available Hours", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        mPBH.HoursBorrowFrequency = txtHours.Text
        mExistingPBH.HoursSubtractFrequency = txtHours.Text

        txtHourFreq.Text = mPBH.HoursFrequency
        txtHourFreq.DataBind()

        txtElapsed.Text = mPBH.ElapsedHours
        txtElapsed.DataBind()

        txtAvlHrs.Text = mExistingPBH.RemainingHours
        txtAvlHrs.DataBind()

        txtRem.Text = mPBH.RemainingHours
        txtRem.DataBind()

        Session("mPBH") = mPBH
        Session("mExistingPBH") = mExistingPBH
    End Sub
    Private Sub cmbMachineList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMachineList.SelectedIndexChanged

        mPBH = Session("mPBH")
        If mPBH Is Nothing Then Exit Sub

        If cmbMachineList.SelectedItem.Text = mPBH.RegNo Then
            MSGBoxCtrl.show("Selection Alert..!!", "Please Select Aircraft other than your PBH Aircraft.", "", MsgBoxStyle.OkOnly, "")
            cmbMachineList.SelectedIndex = 0
            Exit Sub
        End If


        mPBH = PBH.GetPBH(mPBH.ID)
        previousSubscribedHr = mPBH.HoursFrequency
        previousRemHr = mPBH.RemainingHours
        Session("previousSubscribedHr") = previousSubscribedHr
        Session("previousRemHr") = previousRemHr


        mExistingPBH = PBH.GetPBHByMachine(New Guid(cmbMachineList.SelectedValue.ToString), "")
        txtAvlHrs.Text = mExistingPBH.RemainingHours
        txtAvlHrs.DataBind()
        Session("mExistingPBH") = mExistingPBH

        txtHours.Text = ""
        mPBH.HoursBorrowFrequency = txtHours.Text
        mExistingPBH.HoursSubtractFrequency = txtHours.Text

        txtHourFreq.Text = mPBH.HoursFrequency
        txtHourFreq.DataBind()

        txtElapsed.Text = mPBH.ElapsedHours
        txtElapsed.DataBind()

        txtRem.Text = mPBH.RemainingHours
        txtRem.DataBind()

        Session("mPBH") = mPBH
        Session("mExistingPBH") = mExistingPBH

        txtHours.Focus()
    End Sub
    Private Sub btnBorrowPBH_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBorrowPBH.Click
        Try
            mPBH = Session("mPBH")
            mExistingPBH = Session("mExistingPBH")
            If Page.IsValid Then
                If Not Session("mPBH") Is Nothing Then

                    If mPBH.IsValid Then
                        If mPBH.IsNotActive = True And mPBH.RemainingHoursDec > 0 Then
                            mPBH.MachineNotInUse = False
                            mPBH.IsNotActive = False
                            mPBH.NotActiveDate = System.DBNull.Value
                            mPBH.IsBrrowed = True
                        End If

                        mPBH.Save()
                        If mExistingPBH.IsValid Then
                            mExistingPBH.Save()
                        End If
                        pnlBorrowPBH.Visible = False
                        '
                        mdlPopUpPBHBorrow.Hide()
                        GridBind()
                        upnlList.Update()
                        pnlBorrowPBH.Visible = False
                        upnlBorrowPBH.Update()
                        mdlPopUpPBHBorrow.Hide()
                        cmbMachineList.SelectedIndex = 0
                        cmbMachineList.DataBind()
                        previousSubscribedHr = Session("previousSubscribedHr")
                        previousRemHr = Session("previousRemHr")
                        Dim ExtraText As String = mPBH.RegNo + " : <b>previous subscribed hours </b>were <b>" + previousSubscribedHr + "</b> and <b>previous remaining hours </b>were <b>" + previousRemHr + "</b> and now subscribed hours are " + txtHourFreq.Text + " and <b>total remaining hours</b> are <b>" + txtRem.Text + "</b>"

                        MarkLog(Action.Save, "PBHList", "Aircraft " + mPBH.RegNo + " borrowed PBH hours " + txtHours.Text + " from " + mExistingPBH.RegNo + " on " + New SmartDate(Today.Date.ToString).FormattedText + "<br>" + ExtraText, ErrorType.NoError, mPBH.ID, EventLogID)
                        Session("mPBH") = Nothing
                        Session("mExistingPBH") = Nothing
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub chkIsCombinedHrs_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsCombinedHrs.CheckedChanged
        If chkIsCombinedHrs.Checked Then
            cmbAircraftList.Enabled = False
            spnAircraftStar.Visible = False
            txtStartHours.Enabled = False
            txtCurrentHours.Enabled = False
            lblCurrentHours1.Visible = False

        Else
            cmbAircraftList.Enabled = True
            spnAircraftStar.Visible = True
            txtStartHours.Enabled = True
            txtCurrentHours.Enabled = True
            lblCurrentHours1.Visible = True

        End If
    End Sub
#End Region







End Class