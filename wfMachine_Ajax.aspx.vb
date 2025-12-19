Imports System.Text
'AJAX Conversion by Saylee On 22-Jun-2015


Public Class wfMachine_Ajax
    Inherits System.Web.UI.Page

#Region "Aircraft Det"

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mMachineID As Guid = Guid.Empty
    Public mMachineCategoryList As MachineCategoryList
    '====By Saylee on 19/07/07==========
    Public mCustomerList As VendorList
    '===================================
    Public mUnitList As UnitListMain
    Public mModelList As ModelList
    Public mPeriodUnitList As PeriodUnitList
    Public mSelectPeriods As SelectPeriods = SelectPeriods.NewSelectPeriods
    Private Flag As Int16

    Dim EventLogID As Guid 'Added by Saylee on 22-July-2011
    'D&BChart
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    'End
#End Region

#Region " Business Methods "

    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mMachineID = CType(Session("mMachineID"), Guid)
        mMachineCategoryList = CType(Session("mMachineCategoryList"), MachineCategoryList)
        mUnitList = CType(Session("mUnitList"), UnitListMain)
        mModelList = CType(Session("mModelList"), ModelList)
        mPeriodUnitList = CType(Session("mPeriodUnitList"), PeriodUnitList)
        mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
        mCustomerList = Session("mCustomerList")
        'D&BChart
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        'End
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mMachineID") = mMachineID
        Session("mMachineCategoryList") = mMachineCategoryList
        Session("mUnitList") = mUnitList
        Session("mModelList") = mModelList
        Session("mPeriodUnitList") = mPeriodUnitList
        Session("mSelectPeriods") = mSelectPeriods
        Session("mCustomerList") = mCustomerList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineID")
        Session.Remove("mMachineCategoryList")
        Session.Remove("mUnitList")
        Session.Remove("mModelList")
        Session.Remove("mPeriodUnitList")
        Session.Remove("mCustomerList")
        'D&BChart
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'End
    End Sub
    Private Sub NewRecord()
        mMachine = Machine.NewMachine(Guid.NewGuid)
        Session("mMachine") = mMachine
    End Sub
    Private Sub ControlVisibility()
        btnPrint.Enabled = Not mMachine.IsNew
        btnAddPeroid.Enabled = (mMachine.AssemblyStatus.AssemblyTypeID < 3 And Not mMachine.AssemblyStatus.HasLogCount)
        cmbHourTypeList.Enabled = Not mMachine.AssemblyStatus.HasLogCount
        dgCurrentPeriodValue.Columns(3).Visible = (mMachine.AssemblyStatus.AssemblyTypeID < 3 And Not mMachine.AssemblyStatus.HasLogCount)
        ''cmbCustomer.Enabled = (chkIsCustomerMachine.Checked)  Commented By Rajnish 04-04-2008
        txtWarrantyStartDate.Enabled = (chkIsUnderWarranty.Checked)
        txtWarrantyEndDate.Enabled = (chkIsUnderWarranty.Checked)
        txtNotInUseDate.Enabled = chkNotInUse.Checked

        If Not mMachine.IsNew Then
            Dim mMaintenanceActivityCountOfAircraft As MaintenanceActivityCountOfAircraft
            mMaintenanceActivityCountOfAircraft = MaintenanceActivityCountOfAircraft.GetCount(mMachine.ID, 4, mMachine.AssemblyStatus.AsOnDateFormatted.ToString)
            'Dim mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList
            'mAssemblyMonitorServiceStatusList = tmpAssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatusList(mMachine.AssemblyStatus.AsOnDate.ToString, mMachine.AssemblyStatus.AssemblyID, mMachine.ID, True, , , , , , , mMachine.AssemblyStatus.ID.ToString)

            'Dim mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList
            'mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mMachine.AssemblyStatus.AsOnDate.ToString, mMachine.AssemblyStatus.AssemblyID, mMachine.ID, True, , , , , , , mMachine.AssemblyStatus.ID.ToString)

            'Dim mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList
            'mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mMachine.AssemblyStatus.AsOnDate.ToString, mMachine.AssemblyStatus.AssemblyID, mMachine.ID, True, , , , , , , mMachine.AssemblyStatus.ID.ToString)


            ''If HasFoundMonitorEntry = True or mAssemblyStatus.HasLogCount = True Then
            'If (mAssemblyMonitorServiceStatusList.Count > 0 Or mAssemblyMonitorInspStatusList.Count > 0 Or mAssemblyMonitorModStatusList.Count > 0) Or (mMachine.AssemblyStatus.HasLogCount = True) Then
            '    calFromDate.Enabled = False
            'End If
            If (mMaintenanceActivityCountOfAircraft.MaintActivityCount > 0) Or (mMachine.AssemblyStatus.HasLogCount = True) Then
                calFromDate.Enabled = False
            End If
            'mAssemblyMonitorServiceStatusList = Nothing
            'mAssemblyMonitorInspStatusList = Nothing
            'mAssemblyMonitorModStatusList = Nothing
            mMaintenanceActivityCountOfAircraft = Nothing
        End If

        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer") Then
            lblCustStar.Visible = True
        Else
            lblCustStar.Visible = False
        End If
        txtReadOnlyDate.Enabled = chkIsReadOnly.Checked 'Added By Vikrant On 25-Apr-2014 For ALL07042014

        If Not mMachine.IsNew Then
            If ((Not AppSettings("ShowExtraMasterTabs") Is Nothing) AndAlso AppSettings("ShowExtraMasterTabs") = "False") Then
                tbpnlPreviousRegList.Visible = False
                tbpnlLeaseInfo.Visible = False
                tbpnlMaintPolicy.Visible = False
            End If
        End If
        chkIsUTC.Enabled = Not (mMachine.AssemblyStatus.HasLogCount)
        'Added By Vikrant On 05-Nov-2015 For All05112015
        chkIsReadOnly.Enabled = ((mMachine.IsNew) Or (Not mMachine.IsNew And Not mMachine.IROContext))
        txtReadOnlyDate.Enabled = ((mMachine.IsNew And chkIsReadOnly.Checked) Or (Not mMachine.IsNew And chkIsReadOnly.Enabled And chkIsReadOnly.Checked))
        'End
        tbpnlZone.Visible = False  ''disabled by Saylee on 16-Sep-2016

        rdbMulti.Enabled = Not (mMachine.AssemblyStatus.HasLogCount) 'Added By Saylee On 14-Jun-2018 For ALL14062018
        rdbSingle.Enabled = Not (mMachine.AssemblyStatus.HasLogCount) 'Added By Saylee On 14-Jun-2018 For ALL14062018
        chkAirBorneTime.Enabled = Not (mMachine.AssemblyStatus.HasLogCount) 'Added By Saylee On 2-Sep-2021 for ALL02092021

        ControlVisibilityForAttachment() 'D&BChart
    End Sub
    Private Sub addAttributes()
        txtEmptyWt.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtEmptyWt').value,event)")
        txtAllUpWt.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtAllUpWt').value,event)")
        txtFuelCap.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtFuelCap').value,event)")
        'Added By Shweta On 31-Jan-2012 for ALL30012013
        txtmaxtaxiwt.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtmaxtaxiwt').value,event)")
        txtMaxTakeOffWt.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtmaxtakeoffwt').value,event)")
        txtMaxZeroFuel.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtmaxzerofuel').value,event)")
        txtmaxlandwt.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtmaxlandwt').value,event)")
        txtMaxGrossPayLoad.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtmaxgrosspayload').value,event)")
        'End
    End Sub
    Private Sub SetObject()
        mMachine.RegNo = Trim(txtRegNo.Text)
        mMachine.MachineCategoryID = Val(cmbCategory.SelectedValue.ToString)
        mMachine.Owner = Trim(txtOwner.Text)
        mMachine.AssemblyStatus.Assembly.ModelID = New Guid(cmbModel.SelectedValue)
        mMachine.AssemblyStatus.Assembly.SerialNo = txtSerialNo.Text.ToString
        mMachine.HourType = Val(cmbHourTypeList.SelectedValue)
        mMachine.MaxAllUpWt = Val(txtAllUpWt.Text)
        mMachine.EmptyWt = Val(txtEmptyWt.Text)
        mMachine.FuelCap = Val(txtFuelCap.Text)
        mMachine.UnitID = Val(cmbUnit.SelectedValue.ToString)
        If calFromDate.Text = "" Then
            mMachine.AssemblyStatus.AsOnDate = System.DBNull.Value
        Else
            mMachine.AssemblyStatus.AsOnDate = calFromDate.Text
        End If
        ' mMachine.AssemblyStatus.AsOnDate = CType(Trim(calFromDate.Text), Object)
        '==========================By Saylee on 18/07/07===============================
        mMachine.IsCustomerMachine = chkIsCustomerMachine.Checked
        mMachine.CustomerID = New Guid(cmbCustomer.SelectedValue)
        '===========================================================================
        '=======By Saylee on 7th-Feb-2008(suggested by Kalpesh Sir)=================
        mMachine.IsUnderWarranty = chkIsUnderWarranty.Checked
        If txtWarrantyStartDate.Text = "" Then
            mMachine.WarrantyStartDate = System.DBNull.Value
        Else
            mMachine.WarrantyStartDate = txtWarrantyStartDate.Text
        End If

        If txtWarrantyEndDate.Text = "" Then
            mMachine.WarrantyEndDate = System.DBNull.Value
        Else
            mMachine.WarrantyEndDate = txtWarrantyEndDate.Text
        End If

        If txtNotInUseDate.Text = "" Then
            mMachine.NotInUseDate = System.DBNull.Value
        Else
            mMachine.NotInUseDate = txtNotInUseDate.Text
        End If
        mMachine.NotInUse = chkNotInUse.Checked
        mMachine.IsActive = Not (chkNotInUse.Checked)  'we will set IsActive only from this page, and not from pbh page

        mMachine.ServiceProvider = txtServiceProvider.Text
        '===========================================================================
        'Added By Shweta On 31-Jan-2012 for ALL30012013
        mMachine.MaxTaxiWt = Val(txtmaxtaxiwt.Text)
        mMachine.MaxTakeOffWt = Val(txtMaxTakeOffWt.Text)
        mMachine.MaxZeroFuelWt = Val(txtMaxZeroFuel.Text)
        mMachine.MaxLandingWt = Val(txtmaxlandwt.Text)
        mMachine.MaxGrossPayload = Val(txtMaxGrossPayLoad.Text)
        mMachine.MaxTaxiUnitID = Val(cmbMaxTaxiUnit.SelectedValue.ToString)
        mMachine.MaxTakeOffUnitID = Val(cmbMaxTakeOffUnit.SelectedValue.ToString)
        mMachine.MaxZeroFuelUnitID = Val(cmbMaxZeroFuelUnit.SelectedValue.ToString)
        mMachine.MaxLandingUnitID = Val(cmbMaxLandingUnit.SelectedValue.ToString)
        mMachine.MaxGrossPayloadUnitID = Val(cmbMaxGrossPayLoadUnit.SelectedValue.ToString)
        'End
        'Added By Vikrant On 15-Mar-2013 For ALL15032013-1
        mMachine.EmptyWtUnitID = Val(cmbEmptyWtUnit.SelectedValue.ToString)
        mMachine.UpWtUnitID = Val(cmbAllUpWtUnit.SelectedValue.ToString)
        'End

        mMachine.IsUTC = chkIsUTC.Checked 'Added By Saylee On 12-Feb-2014 For ALL12022014-1
        mMachine.IsReadOnly = chkIsReadOnly.Checked  'Added By Vikrant On 07-Apr-2014 For ALL07042014
        'Added By Vikrant On 25-Apr-2014 For ALL07042014
        If txtReadOnlyDate.Text = "" Then
            mMachine.ReadOnlyDate = System.DBNull.Value
        Else
            mMachine.ReadOnlyDate = CDate(txtReadOnlyDate.Text.ToString)
        End If
        'End
        'D&BChart
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mMachine.IsAttachmentAddedForDentBuckleChart = True
            Else
                mMachine.IsAttachmentAddedForDentBuckleChart = False
            End If
        End If
        'End

        mMachine.IsTLP = IIf(rdbMulti.Checked = True, True, False) 'Added By Saylee On 14-Jun-2018 For ALL14062018
        mMachine.IsLogAirborneEntry = chkAirBorneTime.Checked 'Added By Saylee On 2-Sep-2021 for ALL02092021

        Session("mMachine") = mMachine
    End Sub
    Private Sub SetGridObject()
        Dim i As Integer
        Dim txtValue As TextBox
        For i = 0 To dgCurrentPeriodValue.Rows.Count - 1
            txtValue = CType(Me.dgCurrentPeriodValue.Rows(i).FindControl("txtValue"), TextBox)
            If mMachine.AssemblyStatus.AssemblyStatusPeriods(i).PeriodID = 2 Then
                If Not Period.IsDate(txtValue.Text) Then
                    mMachine.AssemblyStatus.AssemblyStatusPeriods(i).AssemblyCurrentValue = ""
                Else
                    mMachine.AssemblyStatus.AssemblyStatusPeriods(i).AssemblyCurrentValueFormatted = Trim(txtValue.Text)
                End If
            Else
                mMachine.AssemblyStatus.AssemblyStatusPeriods(i).AssemblyCurrentValue = Trim(txtValue.Text)
            End If
        Next i
        Session("mMachine") = mMachine
    End Sub
    Private Sub MessageBoxResult()
        'Dim Result1 As MsgBoxResult
        'If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        '    Result1 = -1
        'Else
        '    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        '    If Session("YouarenotAuthorizeduser") = "You are not Authorized user" Then
        '        Session("YouarenotAuthorizeduser") = ""
        '        Result1 = 0
        '    End If
        'End If
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Session("YouarenotAuthorizeduser") = "You are not Authorized user" Then
            Session("YouarenotAuthorizeduser") = ""
            Result1 = 0
        End If

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    Try
                        If MSGBoxCtrl.Sender = "Delete" Then
                            Session("sender") = ""
                            Dim mMachine As Machine
                            mMachine = CType(Session("mMachine"), Machine)
                            mMachine.AssemblyStatus.AssemblyStatusPeriods.RemoveAt(mMachine.AssemblyStatus.AssemblyStatusPeriods.CurrentIndex)
                            Session("mMachine") = mMachine
                            dgCurrentPeriodValue.DataSource = mMachine.AssemblyStatus.AssemblyStatusPeriods
                            dgCurrentPeriodValue.DataBind()
                            upnlCurrenntValue.Update()
                            'DataFieldBind()
                            'Response.Redirect("wfMachine.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        ElseIf MSGBoxCtrl.Sender = "Save" Then
                            Session("sender") = ""
                            If mMachine.IsDirty Then mMachine.IsSync = 0 'Added by Saylee on 3-June-2010 for Symco bridge
                            mMachine.Save()
                            SaveAttachment() 'D&BChart
                            DataFieldBind()
                            Session("mMachine") = mMachine
                            lblMachine.Text = "Machine (saved.....)"
                            Response.Redirect("index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                            '===========================
                        ElseIf MSGBoxCtrl.Sender = "Authorization" Then
                            Session("sender") = ""
                            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.CancelAircraft, SIMsgBox.Message_text.CancelAircraft, "Once you cancel registration, all the data related to cancel Aircraft will be deleted, and decision of cancellation can not revertable.", MsgBoxStyle.YesNo)
                            'msg.ReplacePage = "wfMachine.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                            'Session("sender") = "SaveMachine"
                            'msg.Show()
                            DataFieldBind()
                            MSGBoxCtrl.show(MSGBox.Message_title.CancelAircraft, MSGBox.Message_text.CancelAircraft, "Once you cancel registration, all the data related to cancel Aircraft will be deleted, and decision of cancellation can not revertable.", MsgBoxStyle.YesNo, "SaveMachine")

                        ElseIf MSGBoxCtrl.Sender = "SaveMachine" Then
                            Session("sender") = ""
                            SetSession()
                            'Added By Utksrsh On 11-Mar-2011
                            If (User.IsInRole("MachineNew") Or User.IsInRole("MachineEdit")) = False Then
                                ' ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not Authorized user."))
                                Session("YouarenotAuthorizeduser") = "You are not Authorized user"
                                DataFieldBind()
                                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "You are not Authorized user.", MsgBoxStyle.OkOnly, "")
                            Else
                                '--------------------------------
                                If mMachine.IsDirty Then mMachine.IsSync = 0 'Added by Saylee on 3-June-2010 for Symco bridge
                                Page.Validate("a")
                                If mMachine.IsValid And IsValid Then
                                    If Session("IsFromBack") = "True" Then
                                        Session.Remove("IsFromBack")
                                        If chkNotInUse.Checked = True Then
                                            SetObject()
                                            MSGBoxCtrl.show(MSGBox.Message_title.CancelAircraft, MSGBox.Message_text.CancelAircraft, "Once you cancel registration, all the data related to cancel Aircraft will be deleted, and decision of cancellation can not revertable.", MsgBoxStyle.YesNo, "Authorization")
                                            DataFieldBind()
                                            Exit Sub
                                        End If
                                        'Added By Vikrant On 05-Nov-2015 For All05112015
                                        If chkIsReadOnly.Checked = True Then
                                            SetObject()
                                            MSGBoxCtrl.show("Save Alert!", "You have taken decision to mark this Aircraft as ReadOnly.<p>Are you Sure ?</p>", "Once you mark Aircraft as ReadOnly, you can not use it in any of the Transactions, and decision of ReadOnly can not revertable.", MsgBoxStyle.YesNo, "SaveMachine")
                                            DataFieldBind()
                                            Exit Sub
                                        End If
                                        'End
                                    End If
                                    mMachine.ApplyEdit()
                                    mMachine.Save()
                                    SaveAttachment() 'D&BChart
                                    DataFieldBind()
                                    Response.Redirect("index.aspx")
                                Else

                                    Session("sender") = ""
                                    Dim strMsg As String = ""
                                    Dim txtValue As TextBox
                                    If Not mMachine.IsValid Then
                                        For i As Integer = 0 To mMachine.GetBrokenRulesCollection.Count - 1
                                            strMsg = strMsg + mMachine.GetBrokenRulesCollection(i).Description + "<Br>"
                                        Next
                                    End If
                                    If Not mMachine.AssemblyStatus.Assembly.IsValid Then
                                        For i As Integer = 0 To mMachine.AssemblyStatus.Assembly.GetBrokenRulesCollection.Count - 1
                                            strMsg = strMsg + mMachine.AssemblyStatus.Assembly.GetBrokenRulesCollection(i).Description + "<Br>"
                                        Next
                                    End If
                                    If Not mMachine.AssemblyStatus.IsValid Then
                                        For i As Integer = 0 To mMachine.AssemblyStatus.GetBrokenRulesCollection.Count - 1
                                            strMsg = strMsg + mMachine.AssemblyStatus.GetBrokenRulesCollection(i).Description + "<Br>"
                                        Next
                                    End If
                                    For i As Integer = 0 To dgCurrentPeriodValue.Rows.Count - 1
                                        txtValue = CType(Me.dgCurrentPeriodValue.Rows(i).FindControl("txtValue"), TextBox)
                                        If Not mMachine.AssemblyStatus.AssemblyStatusPeriods(i).IsValid Then
                                            For j As Integer = 0 To mMachine.AssemblyStatus.AssemblyStatusPeriods(i).GetBrokenRulesCollection.Count - 1
                                                strMsg = strMsg + mMachine.AssemblyStatus.AssemblyStatusPeriods(i).GetBrokenRulesCollection(j).Description + "<Br>"
                                            Next
                                        End If
                                    Next i
                                    If strMsg.Trim <> "" Then
                                        cvModelList.ErrorMessage = strMsg
                                        cvModelList.IsValid = False
                                    End If
                                    DataFieldBind()
                                    upnlValidationSummary.Update()
                                    ''Response.Redirect("wfMachine.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                                End If
                            End If
                            '========================
                        Else
                            Session("sender") = ""
                            'Response.Redirect("wfMachine.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        End If
                    Catch ex As SqlException
                        'If ex.Number = 8145 Then
                        '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                        '    msg1.ReplacePage = "wfMachine.aspx?BackPage=" & Request.QueryString("BackPage")
                        '    msg1.Show()
                        'ElseIf ex.Number = 2627 Then
                        '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                        '    msg1.ReplacePage = "wfMachine.aspx?BackPage=" & Request.QueryString("BackPage")
                        '    msg1.Show()
                        'ElseIf ex.Number = 547 Then
                        '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                        '    msg1.ReplacePage = "wfMachine.aspx?BackPage=" & Request.QueryString("BackPage")
                        '    msg1.Show()
                        'End If
                        If ex.Number = 8114 Or ex.Number = 8115 Then
                            MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                        ElseIf ex.Number = 8145 Then
                            MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        ElseIf ex.Number = 2627 Then
                            MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        ElseIf ex.Number = 547 Then
                            MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        End If
                    Finally
                        'Added By Utkarsh On 2-Aug-2011 For All19072011
                        MarkLog(Util.Action.Save, "Aircraft", mMachine.RegNo, Util.ErrorType.NoError, mMachine.ID, EventLogID)
                        'End
                    End Try
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session("sender") = ""
                        Response.Redirect("index.aspx") '?MsgResult=0&BackPage="index.aspx")
                    ElseIf MSGBoxCtrl.Sender = "Remove" Then
                        Session("sender") = ""
                        ' Response.Redirect("wfMachine.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    ElseIf MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        Response.Redirect("index.aspx")
                    ElseIf MSGBoxCtrl.Sender = "SaveMachine" Then
                        Session("sender") = ""
                        MarkLog(Util.Action.Close, "Aircraft", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        RemoveSession()
                        mMachine = Nothing
                        mMachineCategoryList = Nothing
                        mUnitList = Nothing
                        mModelList = Nothing
                        Response.Redirect("index.aspx")
                    ElseIf MSGBoxCtrl.Sender = "Authorization" Then
                        upnlValidationSummary.Update()
                    Else
                        Session("sender") = ""

                        ' Response.Redirect("wfMachine.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfMachine.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfMachine.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            'Response.Redirect("wfMachine.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Function Save() As Boolean
        If Not IsValid Then Exit Function

        'Authountation
        ''Dim mCheck As New Authenticate.CheckAuthentication(True)
        ''Dim mMachineList As MachineList = MachineList.GetMachineList()
        ''If mMachine.IsNew = True And mMachineList.Count >= mCheck.Number("Aircraft") And mCheck.Number("Aircraft") <> -1 Then
        ''    Dim msg1 As New SIMsgBox(Page, "Authentication", "This version does not supports more than " & mCheck.Number("Aircraft").ToString & " Aircrafts", "", MsgBoxStyle.OKOnly)
        ''    msg1.ReplacePage = "wfMachine.aspx?BackPage=" & Request.QueryString("BackPage")
        ''    msg1.Show()
        ''    Return False
        ''End If
        Dim clnMachine As Machine
        clnMachine = CType(mMachine, Machine)
        SetObject()
        SetGridObject()

        If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Function
        If Not CustomValidateChild() Then upnlValidationsummaryChild.Update() : Exit Function

        If mMachine.IsNew Then Session("ShowUseMachineList") = True 'Added By Utkarsh ON 21-Aug-2013 FOR ALL20082013-1

        If mMachine.IsDirty Then mMachine.IsSync = 0 'Added by Saylee on 3-June-2010 for Symco bridge
        If mMachine.IsValid = True Then
            Try
                mMachine.ApplyEdit()
                mMachine = CType(mMachine.Save(), Machine)
                SaveAttachment() 'D&BChart
                Session("mMachine") = mMachine
                'Added By Utkarsh ON 21-Aug-2013 FOR ALL20082013-1
                If Not Session("ShowUseMachineList") Is Nothing AndAlso CBool(Session("ShowUseMachineList")) Then
                    Dim mUserMachineList As UserMachineList = New UserMachineList
                    If mUserMachineList.ShowUsermachineList() Then
                        Session("MachineID") = mMachine.ID
                        Session("MachineName") = mMachine.RegNo
                        Session("MachineURL") = Request.Url
                        Session.Remove("ShowUseMachineList")
                        Response.Redirect("wfUserMachineList.aspx")
                    End If
                End If
                'End
                DataFieldBind()
                'Commented By Utkarsh On 2-Aug-2011 For All19072011
                'MarkLog(Util.Action.Save, "Aircraft", mMachine.RegNo, Util.ErrorType.NoError, mMachine.ID, EventLogID)
                'End
                Return True
            Catch ex As SqlException
                Session("clnMachine") = clnMachine
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            Catch ex1 As Exception
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Save, SIMsgBox.Message_text.Save, "Invalid , cannot save", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfMachine.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Save, MSGBox.Message_text.Save, "Invalid , cannot save", MsgBoxStyle.OkOnly, "")
                Return False
            Finally
                clnMachine = Nothing
                'Added By Utkarsh On 2-Aug-2011 For All19072011
                MarkLog(Util.Action.Save, "Aircraft", mMachine.RegNo, Util.ErrorType.NoError, mMachine.ID, EventLogID)
                'End
            End Try
        Else
            If Not CustomValidate2() Then upnlValidationSummary.Update()
            Return False
        End If
    End Function
    Private Sub SetPage()
        If mMachine.IsNew Then
            lblMachine.Text = "Aircraft [New]"
        Else
            lblMachine.Text = "Aircraft [" & mMachine.RegNo & "]"
        End If
    End Sub
    Private Sub SetPeroids()
        Dim mPeriodlist As PeriodList
        mSelectPeriods = SelectPeriods.NewSelectPeriods
        mPeriodlist = PeriodList.GetPeriodList
        For i As Integer = 0 To mPeriodlist.Count - 1
            If Not mMachine.AssemblyStatus.AssemblyStatusPeriods.Contains(mPeriodlist(i).ID) Then
                mSelectPeriods.Add(mPeriodlist(i).ID, mPeriodlist(i).PeriodName)
            End If
        Next
        Session("mSelectPeriods") = mSelectPeriods
    End Sub
    Private Sub AddSelectedPeroids()
        Dim mSelectPeriod As SelectPeriod
        If IsNothing(mSelectPeriods) Then
            mSelectPeriods = SelectPeriods.NewSelectPeriods
        End If
        For Each mSelectPeriod In mSelectPeriods
            If mSelectPeriod.IsSelected Then
                mMachine.AssemblyStatus.AssemblyStatusPeriods.Add(AssemblyStatusPeriod.NewChildAssemblyStatusPeriod(mMachine.AssemblyStatus.ID, mMachine.AssemblyStatus.MachineID, mMachine.AssemblyStatus.AsOnDate.ToString, mMachine.AssemblyStatus.Assembly.Model.AssemblyTypeID, mSelectPeriod.PeriodID))
            End If
        Next
        Session("mMachine") = mMachine
        Session.Remove("mSelectPeriods")
        mSelectPeriods = Nothing
    End Sub
    'D&BChart
    Private Sub ControlVisibilityForAttachment()
        If mMachine.IsAttachmentAddedForDentBuckleChart Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mMachine.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mMachine.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString

        If mMachine.IsAttachmentAddedForDentBuckleChart And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mMachine.ID)
        End If
        If mFileAttach.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    'End
    'Added By Utkarsh On 11-Mar-2011
    Private Sub SetRights()
        If (User.IsInRole("MachinePrint")) = False Then
            btnPrint.Enabled = False
            btnPrint.ToolTip = "You are not authorized user"
        End If
        If (User.IsInRole("MachineNew") Or User.IsInRole("MachineEdit")) = False Then
            btnSave.Enabled = False
            btnSave.ToolTip = "You are not authorized user"
        End If
    End Sub
    '*******************************
#End Region

#Region " DataBinding "
    Private Sub DataFieldBind()
        mMachineCategoryList = MachineCategoryList.GetMachineCategoryList("(SELECT)")
        cmbCategory.DataSource = mMachineCategoryList
        Session("mMachineCategoryList") = mMachineCategoryList

        mModelList = ModelList.GetModelList(mMachine.AssemblyStatus.AssemblyTypeID, "", , , "(SELECT)")
        cmbModel.DataSource = mModelList
        Session("mModelList") = mModelList

        mUnitList = UnitListMain.GetUnitList("", "(SELECT)")
        cmbUnit.DataSource = mUnitList
        'Added By Shweta On 31-Jan-2012 for ALL30012013
        cmbMaxTaxiUnit.DataSource = mUnitList
        cmbMaxTakeOffUnit.DataSource = mUnitList
        cmbMaxZeroFuelUnit.DataSource = mUnitList
        cmbMaxLandingUnit.DataSource = mUnitList
        cmbMaxGrossPayLoadUnit.DataSource = mUnitList
        'End
        'Added By Vikrant On 15-Mar-2013 For ALL15032013-1
        cmbEmptyWtUnit.DataSource = mUnitList
        cmbAllUpWtUnit.DataSource = mUnitList
        'End
        Session("mUnitList") = mUnitList
        mPeriodUnitList = PeriodUnitList.GetPeriodUnitList(1, "(SELECT)")
        cmbHourTypeList.DataSource = mPeriodUnitList

        dgCurrentPeriodValue.DataSource = mMachine.AssemblyStatus.AssemblyStatusPeriods
        Session("mMachine") = mMachine

        'Added On 29,May,2007 By Girish
        calFromDate.Text = mMachine.AssemblyStatus.AsOnDateFormatted.ToString
        '==========================By Saylee 19/07/07=========================
        mCustomerList = VendorList.GetVendortList(0, , , , , , True, True, )
        cmbCustomer.DataSource = mCustomerList
        Session("mCustomerList") = mCustomerList
        '=====================================================================

        '==========================By Saylee on 7th-Feb-2008(suggested by Kalpesh Sir)=========
        txtWarrantyStartDate.Text = mMachine.WarrantyStartDateFormatted.ToString
        txtWarrantyEndDate.Text = mMachine.WarrantyEndDateFormatted.ToString
        txtNotInUseDate.Text = mMachine.NIUDContextFormatted.ToString
        '======================================================================================
        txtReadOnlyDate.Text = mMachine.RODContextFormatted.ToString    'Added By Vikrant On 25-Apr-2014 For ALL07042014
        DataBind()
        cmbCategory.SelectedValue = mMachine.MachineCategoryID.ToString
        cmbModel.SelectedValue = mMachine.AssemblyStatus.Assembly.ModelID.ToString
        cmbUnit.SelectedValue = mMachine.UnitID.ToString
        'Added By Vikrant On 30-Nov-2015 For ALL30112015
        chkNotInUse.Checked = mMachine.NIUContext
        chkIsReadOnly.Checked = mMachine.IROContext
        chkAirBorneTime.Checked = mMachine.IsLogAirborneEntry ''Added by Saylee on 1-Sep-2021 for ALL01092021 
        'End
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidate As CustomValidator
        CustValidate = CType(s, CustomValidator)
        If CustValidate.ControlToValidate = "txtRegNo" Then
            If txtRegNo.Text = "0" Then
                CustValidate.ErrorMessage = "Enter Valid Registration No. "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidate.ControlToValidate = "cmbCategory" Then
            If cmbCategory.SelectedIndex <= 0 Then
                CustValidate.ErrorMessage = "Select Aircraft category from the list."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidate.ControlToValidate = "cmbHourTypeList" Then
            If cmbHourTypeList.SelectedIndex <= 0 Then
                CustValidate.ErrorMessage = "Select Hour type from the list."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'Added New validation of Model---- Rajnish
        ElseIf CustValidate.ControlToValidate = "cmbModel" Then
            If cmbModel.SelectedIndex <= 0 Then
                CustValidate.ErrorMessage = "Select Aircraft Model from the list."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidate.ControlToValidate = "txtEmptyWt" Then
            If Val(txtEmptyWt.Text) < 0 Then
                CustValidate.ErrorMessage = "Empty Wt. should be non zero positive numeric value."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidate.ControlToValidate = "txtAllUpWt" Then
            If Val(txtAllUpWt.Text) < 0 Then
                CustValidate.ErrorMessage = "All Up Wt. should be non zero positive numeric value."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            If Val(txtAllUpWt.Text) < Val(txtEmptyWt.Text) Then
                CustValidate.ErrorMessage = "All Up Wt. cannot be less than Empty Wt."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidate.ControlToValidate = "txtFuelCap" Then
            If Val(txtFuelCap.Text) < 0 Then
                CustValidate.ErrorMessage = "Fuel Capacity should be non zero positive numeric value."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        ElseIf CustValidate.ControlToValidate = "calFromDate" Then
            If calFromDate.Text = "" And mMachine.AssemblyStatus.AssemblyTypeID = 1 Then
                CustValidate.ErrorMessage = "As On date required"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'Commented By Rajnish 04-04-2008 
            'Comment Opened By Saylee 07-Sep-2010 
        ElseIf CustValidate.ControlToValidate = "cmbCustomer" Then
            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer") And (cmbCustomer.SelectedIndex <= 0) Then
                CustValidate.ErrorMessage = "Select Customer from the list."
                e.IsValid = False
            End If
        ElseIf CustValidate.ControlToValidate = "txtmaxtaxiwt" Then
            If Val(txtmaxtaxiwt.Text) < 0 Then
                CustValidate.ErrorMessage = "Maximum Taxi Weight should be non zero positive numeric value."
                e.IsValid = False
                'ElseIf Val(txtmaxtaxiwt.Text) > 0 And cmbMaxTaxiUnit.SelectedIndex = 0 Then
                '    CustValidate.ErrorMessage = "Maximum Taxi Unit Required."
                '    e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidate.ControlToValidate = "txtMaxTakeOffWt" Then
            If Val(txtMaxTakeOffWt.Text) < 0 Then
                CustValidate.ErrorMessage = "Maximum Take Off Weight should be non zero positive numeric value."
                e.IsValid = False
                'ElseIf Val(txtMaxTakeOffWt.Text) > 0 And cmbMaxTakeOffUnit.SelectedIndex = 0 Then
                '    CustValidate.ErrorMessage = "Maximum Take Off Unit Required."
                '    e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidate.ControlToValidate = "txtmaxzerofuel" Then
            If Val(txtMaxZeroFuel.Text) < 0 Then
                CustValidate.ErrorMessage = "Maximum Zero Fuel Weight should be non zero positive numeric value."
                e.IsValid = False
                'ElseIf Val(txtMaxZeroFuel.Text) > 0 And cmbMaxZeroFuelUnit.SelectedIndex = 0 Then
                '    CustValidate.ErrorMessage = "Maximum Zero Fuel Unit Required."
                '    e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidate.ControlToValidate = "txtmaxlandwt" Then
            If Val(txtmaxlandwt.Text) < 0 Then
                CustValidate.ErrorMessage = "Maximum Landing wt. should be non zero positive numeric value."
                e.IsValid = False
                'ElseIf Val(txtmaxlandwt.Text) > 0 And cmbMaxLandingUnit.SelectedIndex = 0 Then
                '    CustValidate.ErrorMessage = "Maximum Landing Unit Required."
                '    e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidate.ControlToValidate = "txtmaxgrosspayload" Then
            If Val(txtMaxGrossPayLoad.Text) < 0 Then
                CustValidate.ErrorMessage = "Maximum Gross Pay load should be non zero positive numeric value."
                e.IsValid = False
                'ElseIf Val(txtMaxGrossPayLoad.Text) > 0 And cmbMaxGrossPayLoadUnit.SelectedIndex = 0 Then
                '    CustValidate.ErrorMessage = "Maximum Gross Pay load Unit Required."
                '    e.IsValid = False
            Else
                e.IsValid = True
            End If
            'Added By Vikrant On 25-Apr-2014 For ALL07042014
        ElseIf CustValidate.ControlToValidate = "txtNotInUseDate" Then
            If chkNotInUse.Checked Then
                If txtNotInUseDate.Text = "" Then
                    CustValidate.ErrorMessage = "Enter Not In Use Date."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                e.IsValid = True
            End If
        ElseIf CustValidate.ControlToValidate = "txtReadOnlyDate" Then
            If chkIsReadOnly.Checked Then
                If txtReadOnlyDate.Text = "" Then
                    CustValidate.ErrorMessage = "Enter ReadOnly Date."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                e.IsValid = True
            End If
            'End
        End If

    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        Dim txtValue As TextBox
        Dim strMsg As String = ""
        SetObject()
        SetGridObject()
        If Not mMachine.IsValid Then
            For i As Integer = 0 To mMachine.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mMachine.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If Not mMachine.AssemblyStatus.Assembly.IsValid Then
            For i As Integer = 0 To mMachine.AssemblyStatus.Assembly.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mMachine.AssemblyStatus.Assembly.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If Not mMachine.AssemblyStatus.IsValid Then
            For i As Integer = 0 To mMachine.AssemblyStatus.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mMachine.AssemblyStatus.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        For i As Integer = 0 To dgCurrentPeriodValue.Rows.Count - 1
            txtValue = CType(Me.dgCurrentPeriodValue.Rows(i).FindControl("txtValue"), TextBox)
            If Not mMachine.AssemblyStatus.AssemblyStatusPeriods(i).IsValid Then
                For j As Integer = 0 To mMachine.AssemblyStatus.AssemblyStatusPeriods(i).GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mMachine.AssemblyStatus.AssemblyStatusPeriods(i).GetBrokenRulesCollection(j).Description + "<Br>"
                Next
            End If
        Next i
        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            e.IsValid = False
        End If
        Flag = 1
    End Sub
    Public Function CustomValidate2() As Boolean
        Dim strMsg As String = ""

        If Not mMachine.IsValid Then
            For j As Integer = 0 To mMachine.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mMachine.GetBrokenRulesCollection(j).Description + "<Br>"
            Next
        End If
        'For i As Integer = 0 To mMachine.AssemblyStatus.AssemblyStatusPeriods.Count - 1
        '    If Not mMachine.AssemblyStatus.AssemblyStatusPeriods(i).IsValid Then
        '        For j As Integer = 0 To mMachine.AssemblyStatus.AssemblyStatusPeriods(i).GetBrokenRulesCollection.Count - 1
        '            strMsg = strMsg + mMachine.AssemblyStatus.AssemblyStatusPeriods(i).GetBrokenRulesCollection(j).Description + "<Br>"
        '        Next
        '    End If
        'Next i

        If strMsg.Trim <> "" Then
            cvModelList.ErrorMessage = strMsg
            cvModelList.IsValid = False
            Return False
        End If
        Return True
    End Function
    Public Function CustomValidateChild() As Boolean
        Dim strMsg As String = ""

        For i As Integer = 0 To mMachine.AssemblyStatus.AssemblyStatusPeriods.Count - 1
            If Not mMachine.AssemblyStatus.AssemblyStatusPeriods(i).IsValid Then
                For j As Integer = 0 To mMachine.AssemblyStatus.AssemblyStatusPeriods(i).GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mMachine.AssemblyStatus.AssemblyStatusPeriods(i).GetBrokenRulesCollection(j).Description + "<Br>"
                Next
            End If
        Next i

        If strMsg.Trim <> "" Then
            cvChildList.ErrorMessage = strMsg
            cvChildList.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 22-July-2011
        If Not IsPostBack Then
            'Session("MiddleFrame") = "wfMachineList.aspx?"
            If txtRegNo.Enabled = True Then
                txtRegNo.Focus()
            End If
            AddSelectedPeroids()
            DataFieldBind()
            SetPage()
            ControlVisibility()
            SetRights()
            'TbContInst.ActiveTabIndex = 2

            If CType(Session("ActiveTabIndex"), Integer) > 0 Then
                If Not Session("ActiveTabIndex") Is Nothing Then TbContInst.ActiveTabIndex = CType(Session("ActiveTabIndex"), Integer) : Session.Remove("ActiveTabIndex")
                Call TbContInst_ActiveTabChanged(Nothing, Nothing)
            Else
                TbContInst.ActiveTabIndex = 0
            End If
        End If
        tbpnlZone.Visible = False  ''disabled by Saylee on 16-Sep-2016
        'MessageBoxResult()
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        If TbContInst.ActiveTabIndex = 0 Then
            MessageBoxResult()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Commented By Utkarsh On 11-Mar-2011
        ''If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
        ''    SetObject()
        ''    SetSession()
        ''    MarkLog(Util.Action.Save, "Aircraft", User.Identity.Name & " is not Authorized User to save " + mMachine.RegNo, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
        ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        ''    msg.ReplacePage = "wfMachine.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
        ''    Session("sender") = "Authorization"
        ''    msg.Show()
        ''    Exit Sub
        ''End If
        If IsValid Then
            'If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub

            If chkNotInUse.Checked = True Then
                SetObject()
                MSGBoxCtrl.show(MSGBox.Message_title.CancelAircraft, MSGBox.Message_text.CancelAircraft, "Once you cancel registration, all the data related to cancel Aircraft will be deleted, and decision of cancellation can not revertable.", MsgBoxStyle.YesNo, "Authorization")
                DataFieldBind()
                Exit Sub
            End If
            'Added By Vikrant On 05-Nov-2015 For All05112015
            If chkIsReadOnly.Checked = True Then
                SetObject()
                MSGBoxCtrl.show("Save Alert!", "You have taken decision to mark this Aircraft as ReadOnly.<p>Are you Sure ?</p>", "Once you mark Aircraft as ReadOnly, you can not use it in any of the Transactions, and decision of ReadOnly can not revertable.", MsgBoxStyle.YesNo, "SaveMachine")
                DataFieldBind()
                Exit Sub
            End If
            'End
            If Save() = True Then
                DataFieldBind()
                SetPage()

                ControlVisibility()
                upnlTabs.Update()
                upnlAircraftRegInfo.Update()
                upnlAirframeInfo.Update()
                upnlCurrenntValue.Update()

                upnlTitle.Update()
                'Response.Redirect("wfMachine.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            Else
                upnlValidationSummary.Update()
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgbtnModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnModel.Click
        SetObject()
        Session("mMachine") = mMachine
        'Response.Redirect("wfModel_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage1=wfMachine.aspx&Type=False&AssemblyTypeId=" & mMachine.AssemblyStatus.AssemblyTypeID)
        Session("Type") = False
        Session("AssemblyTypeId") = mMachine.AssemblyStatus.AssemblyTypeID
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelWindow", "OpenModelWindow()", True)
    End Sub
    Private Sub dgCurrentPeriodValue_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCurrentPeriodValue.RowCommand
        ' Dim index As Int32 = e.Item.ItemIndex + dgCurrentPeriodValue.PageIndex * dgCurrentPeriodValue.PageSize
        Dim Index As Integer = CInt(e.CommandArgument) + dgCurrentPeriodValue.PageSize * dgCurrentPeriodValue.PageIndex

        Select Case e.CommandName
            ' To remove the Period from the list
            Case "DeleteRec"
                'Added By Prashant 2-Sep-2010
                If mMachine.AssemblyStatus.AssemblyStatusPeriods.Item(Index).PeriodID = 1 Or mMachine.AssemblyStatus.AssemblyStatusPeriods.Item(Index).PeriodID = 2 Or mMachine.AssemblyStatus.AssemblyStatusPeriods.Item(Index).PeriodID = 7 Then
                    SetPeroids()
                    SetGridObject()
                    SetObject()
                    Session("mMachine") = mMachine
                End If
                '----------------------------

                'Added By Utkarsh On 11-Mar-2011
                If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                '********************************
                'check if the Period is monitored or not
                'Commented and added by Saylee on 11-Mar-2013 for ALL11032013 - 1 --HasMonitorCount condition added
                If mMachine.AssemblyStatus.AssemblyStatusPeriods.Item(Index).HasMonitor = True Or mMachine.AssemblyStatus.AssemblyStatusPeriods(Index).HasMonitorCount(mMachine.AssemblyStatus.ID, mMachine.AssemblyStatus.AssemblyStatusPeriods(Index).PeriodID) = True Then
                    '******need to add the customised message***pending
                    'Dim msg2 As New SIMsgBox(Page, SIMsgBox.Message_title.MachineMonitor, SIMsgBox.Message_text.MachineMonitor, "You are trying to delete " & mMachine.AssemblyStatus.AssemblyTypeName & " Period. Selected " & mMachine.AssemblyStatus.AssemblyTypeName & " period can not be removed as monitor entry exist.", MsgBoxStyle.OkOnly)
                    'msg2.ReplacePage = "wfMachine.aspx?BackPage=" & Request.QueryString("BackPage")
                    'msg2.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.MachineMonitor, MSGBox.Message_text.MachineMonitor, "You are trying to delete " & mMachine.AssemblyStatus.AssemblyTypeName & " Period. Selected " & mMachine.AssemblyStatus.AssemblyTypeName & " period can not be removed as monitor entry exist.", MsgBoxStyle.OkOnly, "")
                    Session("mMachine") = mMachine
                    'Remove Hours and start Date
                ElseIf mMachine.AssemblyStatus.AssemblyStatusPeriods.Item(Index).PeriodID = 1 Or mMachine.AssemblyStatus.AssemblyStatusPeriods.Item(Index).PeriodID = 2 Then
                    'Dim msg2 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to Remove Period." & mMachine.AssemblyStatus.AssemblyStatusPeriods.Item(Index).PeriodName & " Required. Can not remove.", MsgBoxStyle.OkOnly)
                    'msg2.ReplacePage = "wfMachine.aspx?BackPage=" & Request.QueryString("BackPage")
                    'Session("sender") = "Delete"
                    MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to Remove Period." & mMachine.AssemblyStatus.AssemblyStatusPeriods.Item(Index).PeriodName & " Required. Can not remove.", MsgBoxStyle.OkOnly, "Delete")

                ElseIf mMachine.AssemblyStatus.AssemblyStatusPeriods(Index).HasMonitorCountInOtherAssembly(mMachine.ID, mMachine.AssemblyStatus.ID, mMachine.AssemblyStatus.AssemblyStatusPeriods(Index).PeriodID, mMachine.AssemblyStatus.IsMaster) Then
                    'Dim msg2 As New SIMsgBox(Page, SIMsgBox.Message_title.MachineMonitor, SIMsgBox.Message_text.MachineMonitor, "Selected " & mMachine.AssemblyStatus.AssemblyTypeName & " period can not be removed as monitor entry exist in other Assembly", MsgBoxStyle.OkOnly)
                    'msg2.ReplacePage = "wfMachine.aspx?BackPage=" & Request.QueryString("BackPage")
                    'Session("sender") = "Delete"
                    MSGBoxCtrl.show(MSGBox.Message_title.MachineMonitor, MSGBox.Message_text.MachineMonitor, "Selected " & mMachine.AssemblyStatus.AssemblyTypeName & " period can not be removed as monitor entry exist in other Assembly", MsgBoxStyle.OkOnly, "Delete")
                Else
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.Delete, "Remove Aircraft Item.", MsgBoxStyle.YesNo)
                    'msg1.ReplacePage = "wfMachine.aspx?BackPage=" & Request.QueryString("BackPage")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    mMachine.AssemblyStatus.AssemblyStatusPeriods.CurrentIndex = Index
                    MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.Delete, "Remove Aircraft Item.", MsgBoxStyle.YesNo, "Delete")
                    Session("mMachine") = mMachine
                End If
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added By Utkarsh On 11-Mar-20011
        SetObject()
        SetGridObject()
        '******************************
        'If IsValid Then
        If mMachine.IsDirty Then 'Added by Saylee 7-Sep-2009
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.CloseConfirm, SIMsgBox.Message_text.Save, "", MsgBoxStyle.YesNo)
            'msg1.ReplacePage = "wfMachine.aspx?BackPage=" & Request.QueryString("BackPage")
            'Session("sender") = "SaveMachine"
            'msg1.Show()
            Session("IsFromBack") = "True"
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "SaveMachine")
            ''If IsValid Then
            ''    SetObject()
            ''End If

        Else
            Dim MachineDetail As String = "Reg No. : " + mMachine.RegNo + " with Model : " + mMachine.AssemblyStatus.Assembly.ModelName + " and Serial No : " + mMachine.AssemblyStatus.Assembly.SerialNo
            MarkLog(Util.Action.Close, "Aircraft", MachineDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            RemoveSession()
            mMachine = Nothing
            mMachineCategoryList = Nothing
            mUnitList = Nothing
            mModelList = Nothing
            Response.Redirect("index.aspx")
        End If
        'End If
    End Sub
    Private Sub btnAddPeroid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPeroid.Click
        SetPeroids()
        SetObject()
        SetGridObject()
        Session("mMachine") = mMachine
        'Response.Redirect("wfSelectPeriod.aspx?BackPage2=wfMachine.aspx&BackPage=" & Request.QueryString("BackPage"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAddPeriodWindow", "OpenAddPeriodWindow()", True)
    End Sub
    Private Sub cmbModel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbModel.SelectedIndexChanged
        If cmbModel.SelectedIndex > 0 Then
            Dim mId As New Guid(cmbModel.SelectedValue)
            Dim mModel As Model = Model.GetModel(mId)
            txtManufacturer.Text = mModel.ManufacturerName
        Else
            txtManufacturer.Text = ""
        End If
        If cmbModel.Enabled = True Then
            cmbModel.Focus()
        End If
    End Sub
    Private Sub cmbHourTypeList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbHourTypeList.SelectedIndexChanged
        If cmbHourTypeList.SelectedIndex > 0 Then
            SetObject()
            dgCurrentPeriodValue.DataSource = mMachine.AssemblyStatus.AssemblyStatusPeriods
            dgCurrentPeriodValue.DataBind()
        End If
        If cmbHourTypeList.Enabled = True Then
            cmbHourTypeList.Focus()
        End If
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        NewRecord()
        'If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfMachine.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
        '    Session("sender") = "Authorization"
        '    msg.Show()
        '    Exit Sub
        'End If
        Response.Redirect("wfMachine.aspx?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub chkIsCustomerMachine_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsCustomerMachine.CheckedChanged
        If chkIsCustomerMachine.Checked = True Then
            cmbCustomer.Enabled = True
        Else
            cmbCustomer.Enabled = False
            cmbCustomer.SelectedIndex = 0
        End If
        If chkIsCustomerMachine.Enabled = True Then
            chkIsCustomerMachine.Focus()
        End If
    End Sub
    Private Sub chkIsUnderWarranty_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsUnderWarranty.CheckedChanged
        If chkIsUnderWarranty.Checked = True Then
            txtWarrantyStartDate.Enabled = True
            txtWarrantyEndDate.Enabled = True
        Else
            txtWarrantyStartDate.Enabled = False
            txtWarrantyEndDate.Enabled = False
            txtWarrantyStartDate.Text = ""
            txtWarrantyEndDate.Text = ""
        End If
        If chkIsUnderWarranty.Enabled = True Then
            chkIsUnderWarranty.Focus()
        End If
    End Sub

    Private Sub chkNotInUse_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotInUse.CheckedChanged
        txtNotInUseDate.Enabled = chkNotInUse.Checked
        If chkNotInUse.Checked = False Then
            txtNotInUseDate.Text = ""
        Else 'Added By Vikrant On 25-Apr-2014 For ALL07042014
            chkIsReadOnly.Checked = True
            'txtReadOnlyDate.Enabled = ((mMachine.IsNew) Or (Not mMachine.IsNew And Not mMachine.IsReadOnly)) 'True
            txtReadOnlyDate.Enabled = ((mMachine.IsNew And chkIsReadOnly.Checked) Or (Not mMachine.IsNew And chkIsReadOnly.Enabled And chkIsReadOnly.Checked))
        End If
        'End
    End Sub

    'Added By Vikrant On 25-Apr-2014 For ALL07042014
    Private Sub chkIsReadOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsReadOnly.CheckedChanged
        txtReadOnlyDate.Enabled = chkIsReadOnly.Checked
        If chkIsReadOnly.Checked = False Then
            txtReadOnlyDate.Text = ""
        End If
    End Sub
    'Private Sub txtNotInUseDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNotInUseDate.TextChanged
    '    If chkIsReadOnly.Checked And txtNotInUseDate.Value.ToString <> "" Then
    '        txtReadOnlyDate.Value = txtNotInUseDate.Value
    '    End If
    'End Sub
    'End

#End Region

#Region " Report "
    'Created By :- Jyoti
#Region " Report Variable "
    Dim mCompanyDetail As CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        'If (Not User.IsInRole("MachinePrint")) Then
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfMachine.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
        '    msg.Show()
        '    Exit Sub
        'End If
        Rpt = New crDetMachineStatus
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        ReportDetails.Add(New rptStatus(, 0, "Aircraft Registration Details", lblRegNo.Text, _
        txtRegNo.Text, , , , , , , , , , , , , , , , , "Total Weight And Capacity", lblEmptyWt.Text, txtEmptyWt.Text, _
        , IIf(cmbEmptyWtUnit.SelectedIndex <= 0, "---", cmbEmptyWtUnit.SelectedItem.ToString), , , , , ))

        ReportDetails.Add(New rptStatus(, 0, "Aircraft Registration Details", lblCategory.Text, _
        cmbCategory.SelectedItem.Text, , , , , , , , , , , , , , , , , "Total Weight And Capacity", lblAllUpWt.Text, txtAllUpWt.Text, _
        , IIf(cmbAllUpWtUnit.SelectedIndex <= 0, "---", cmbAllUpWtUnit.SelectedItem.ToString), , , , ))

        ReportDetails.Add(New rptStatus(, 0, "Aircraft Registration Details", lblOwner.Text, _
       txtOwner.Text, , , , , , , , , , , , , , , , , "Total Weight And Capacity", lblFuelCap.Text, txtFuelCap.Text, _
       , cmbUnit.SelectedItem.Text, , , , , ))



        '**************************
        'Max Taxi Wt
        ReportDetails.Add(New rptStatus(, 0, "Aircraft Registration Details", "", _
              "", , , , , , , , , , , , , , , , , "Total Weight And Capacity", lblMaxTaxiWt.Text, txtmaxtaxiwt.Text, _
               , IIf(cmbMaxTaxiUnit.SelectedItem.Text = "<SELECT>", "", cmbMaxTaxiUnit.SelectedItem.Text), , , , , ))


        'Max Take Off Wt
        ReportDetails.Add(New rptStatus(, 0, "Aircraft Registration Details", "", _
                      "", , , , , , , , , , , , , , , , , "Total Weight And Capacity", lblMaxTakeOffWt.Text, txtMaxTakeOffWt.Text, _
                       , IIf(cmbMaxTakeOffUnit.SelectedItem.Text = "<SELECT>", "", cmbMaxTakeOffUnit.SelectedItem.Text), , , , , ))

        'Max Zero Fuel
        ReportDetails.Add(New rptStatus(, 0, "Aircraft Registration Details", "", _
                   "", , , , , , , , , , , , , , , , , "Total Weight And Capacity", lblMaxZeroFuel.Text, txtMaxZeroFuel.Text, _
                    , IIf(cmbMaxZeroFuelUnit.SelectedItem.Text = "<SELECT>", "", cmbMaxZeroFuelUnit.SelectedItem.Text), , , , , ))

        'Max Landing
        ReportDetails.Add(New rptStatus(, 0, "Aircraft Registration Details", "", _
                  "", , , , , , , , , , , , , , , , , "Total Weight And Capacity", lblMaxLandingWt.Text, txtmaxlandwt.Text, _
                   , IIf(cmbMaxLandingUnit.SelectedItem.Text = "<SELECT>", "", cmbMaxLandingUnit.SelectedItem.Text), , , , , ))

        'Max Gross PayLoad 
        ReportDetails.Add(New rptStatus(, 0, "Aircraft Registration Details", "", _
                         "", , , , , , , , , , , , , , , , , "Total Weight And Capacity", lblMaxGrossPayLoad.Text, txtMaxGrossPayLoad.Text, _
                          , IIf(cmbMaxGrossPayLoadUnit.SelectedItem.Text = "<SELECT>", "", cmbMaxGrossPayLoadUnit.SelectedItem.Text), , , , , ))


        '*************************



        ReportDetails.Add(New rptStatus(, 1, "Airframe Details", lblManufacturer.Text, _
               txtManufacturer.Text, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft (TSN)", _
               dgCurrentPeriodValue.Columns.Item(0).HeaderText, dgCurrentPeriodValue.Columns.Item(1).HeaderText, _
               , , , , , , ))

        Dim TotalCount As Integer
        TotalCount = Me.mMachine.AssemblyStatus.AssemblyStatusPeriods.Count
        Dim I As Integer

        For I = 0 To TotalCount - 1
            If I = 0 Then
                ReportDetails.Add(New rptStatus(, 1, "Airframe Details", lblModel.Text, _
                       cmbModel.SelectedItem.Text, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft (TSN)", _
                       CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                       , , , , , , ))
            ElseIf I = 1 Then
                ReportDetails.Add(New rptStatus(, 1, "Airframe Details", lblSerialNo.Text, _
                                      txtSerialNo.Text, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft (TSN)", _
                                      CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                      , , , , , , ))
            Else
                ReportDetails.Add(New rptStatus(, 1, "Airframe Details", "", _
                                       "", , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft (TSN)", _
                                       CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                       , , , , , , ))
            End If
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
        mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
        mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        "Aircraft Status Detail Report", CDate(calFromDate.Text).ToString(Flypal.Util.WebDateFormat), "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 29-Feb-2012
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        'Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region

#End Region

#Region "TAB's"
    Private Sub TbContInst_ActiveTabChanged(sender As Object, e As System.EventArgs) Handles TbContInst.ActiveTabChanged
        Select Case TbContInst.ActiveTabIndex
            Case 0
            Case 1
                'If IsValid Then
                If Not mMachine.IsNew Then
                    Session("mMachine") = mMachine
                    Session("mFrom") = 1
                End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallAssemblyList", "CallAssemblyList();", True)
                'Else
                'upnlValidationSummary.Update()
                'End If
            Case 2
                SetObject()
                SetGridObject()
                If Not mMachine.IsNew Then
                    'mMachine = Machine.GetMachine(mMachine.ID)
                    Session("mMachine") = mMachine
                    'Session("mFrom") = 1
                    'Response.Redirect("wfMachineTankList.aspx?ChildPage=wfMachine.aspx&BackPage=" & Request.QueryString("BackPage"))
                    'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenMachineTankWindow", "OpenMachineTankWindow();", True)

                Else
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.AssemblyStatusAcess, SIMsgBox.Message_text.AssemblyStatusAcess, "Invalid , cannot save", MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfMachine.aspx?BackPage=" & Request.QueryString("BackPage")
                    'msg1.Show()
                End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallTankList", "CallTankList();", True)
            Case 3 'CallFeatureList
                'If IsValid Then
                If Not mMachine.IsNew Then
                    Session("mMachine") = mMachine
                    Session("mFrom") = 1
                End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallFeatureList", "CallFeatureList();", True)
                'Else
                'upnlValidationSummary.Update()
                'End If
            Case 4 'CallCertificateList
                'If IsValid Then
                If Not mMachine.IsNew Then
                    Session("mMachine") = mMachine
                    Session("mFrom") = 1
                End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCertificateList", "CallCertificateList();", True)
                'Else
                'upnlValidationSummary.Update()
                'End If
            Case 5 'MEL Tab
                SetObject()
                SetGridObject()
                Session("mMachine") = mMachine
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMELList", "CallMELList();", True)
            Case 6 'Board Info Tab
                SetObject()
                SetGridObject()
                Session("mMachine") = mMachine
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallBoardInfoList", "CallBoardInfoList();", True)
            Case 7 'Prev Reg.
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallPrevRegList", "CallPrevRegList();", True)
            Case 8 'Lease Info Tab
                SetObject()
                SetGridObject()
                Session("mMachine") = mMachine
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallLeaseInfoList", "CallLeaseInfoList();", True)
            Case 9 'Maint. Policy Tab
                SetObject()
                SetGridObject()
                Session("mMachine") = mMachine
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMaintPolicyList", "CallMaintPolicyList();", True)
            Case 10 'Zone Configuration  'Added by bhushan 02-Aug-2016
                SetObject()
                SetGridObject()
                Session("mMachine") = mMachine
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallZoneConfigurationList", "CallZoneConfigurationList();", True)
            Case 11 'MPD/AMP  
                SetObject()
                SetGridObject()
                Session("mMachine") = mMachine
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallMPDAMPRef", "CallMPDAMPRef();", True)
        End Select
    End Sub
    Private Sub hdnBtnModel_Click(sender As Object, e As System.EventArgs) Handles hdnBtnModel.Click
        mModelList = ModelList.GetModelList(mMachine.AssemblyStatus.AssemblyTypeID, "", , , "(SELECT)")
        cmbModel.DataSource = mModelList
        Session("mModelList") = mModelList

        cmbModel.DataBind()
        cmbModel.SelectedValue = mMachine.AssemblyStatus.Assembly.ModelID.ToString
        upnlAirframeInfo.Update()
    End Sub
    Private Sub hdnAddPeriod_Click(sender As Object, e As System.EventArgs) Handles hdnAddPeriod.Click
        mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
        AddSelectedPeroids()
        dgCurrentPeriodValue.DataSource = mMachine.AssemblyStatus.AssemblyStatusPeriods
        dgCurrentPeriodValue.DataBind()
        upnlCurrenntValue.Update()
    End Sub
#End Region

    'D&BChart
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mMachine.IsAttachmentAddedForDentBuckleChart = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mMachine.IsAttachmentAddedForDentBuckleChart And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mMachine.ID)
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mMachine.IsAttachmentAddedForDentBuckleChart = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        Session("mFileAttach") = mFileAttach
        Session("mMachine") = mMachine
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mMachine.IsAttachmentAddedForDentBuckleChart Then
            mFileAttach = FileAttach.GetAttachment(mMachine.ID)
        Else
            If IsAttachmentDeleted Then
                If (Not mMachine.IsNew) Then
                    mFileAttach = FileAttach.GetAttachment(mMachine.ID)
                    If Not mFileAttach Is Nothing Then
                        Dim fileSize1 As Integer = 0
                        Dim file1(fileSize1) As Byte

                        mFileAttach.ImageFile = file1
                        mFileAttach.Size = 0
                        GoTo CodeBlock
                    End If
                End If
            End If
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mMachine.ID)
        End If
CodeBlock:
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
    End Sub
    'End
    Private Sub txtNotInUseDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtNotInUseDate.TextChanged
        If txtNotInUseDate.Text <> "" Then
            Dim mExistingTransactionListForWOAircraft As ExistingTransactionListForWOAircraft
            mExistingTransactionListForWOAircraft = ExistingTransactionListForWOAircraft.GetList(mMachine.ID.ToString, txtNotInUseDate.Text)
            If mExistingTransactionListForWOAircraft.Count > 0 Then
                Dim TransType As New StringBuilder
               
                For i As Integer = 0 To mExistingTransactionListForWOAircraft.Count - 1
                    If Not TransType.ToString.Contains(mExistingTransactionListForWOAircraft(i).TranType) Then
                        TransType.Append(mExistingTransactionListForWOAircraft(i).TranType + ",")
                    End If
                Next
                MSGBoxCtrl.show("Alert!", TransType.ToString.TrimEnd(",") + " Transaction(s) already present for this Aircarft on or after " + txtNotInUseDate.Text + "<Br>Please select diffrent Not In Use date", "", MsgBoxStyle.OkOnly, "NIU")
                txtNotInUseDate.Text = ""
            End If
        End If
    End Sub

    Private Sub rdbSingle_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbSingle.CheckedChanged, rdbMulti.CheckedChanged
        If rdbSingle.Checked Then
            chkAirBorneTime.Visible = True
        ElseIf rdbMulti.Checked Then
            chkAirBorneTime.Visible = False
        End If
        upnlSector.Update()
    End Sub
End Class