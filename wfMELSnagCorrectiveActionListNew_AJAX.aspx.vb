'AJAX Conversion By Saylee On 30-Dec-2014

Public Class wfMELSnagCorrectiveActionListNew_AJAX
    Inherits System.Web.UI.Page
#Region " Variable Declaration "
    Public mMELSnagCorrectiveActionListNew As MELSnagCorrectiveActionListNew
    Public mMELSnagCorrectiveAction As MELSnagCorrectiveAction
    Dim mMachineNameValueList As MachineNameValueList 'Changed By Utkarsh On 19-Apr-2011
    Public BackPage As String
    Public AircraftId As String
    Public AssemblyId As String
    Public ATAChapterId As String
    Dim DateIndex, FromDate, ToDate, MachineID, Name, No, ATANomenclature, DefectType As String
    Dim StatusCode, ATACode, MELSnagCode As Integer
    Public mATAList As ATAList 'Added By Saylee on 12-Aug-2010
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Dim mMELSnagDetail As String
    Dim mAssemblylist As AssemblyList 'Added By Vikrant On 02-Sept-2014 For All04092014
    Dim ExtensionApplied As Integer
    Dim IsInReliability As Integer
    Public mIncidentTypeListForMELSnagCorrectiveActionListNew As IncidentTypeList
    Dim TypeOfIncidentID As Integer '= -1
    Dim ShowNoEntries As String

#End Region

#Region " Helper Methods "
    Private Sub GetSession()

        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        AircraftId = CType(Session("AircraftId"), String)
        StatusCode = Session("StatusCode")
        MELSnagCode = Session("MELSnagCode")
        MachineID = Session("MachineID")
        mMELSnagCorrectiveActionListNew = Session("mMELSnagCorrectiveActionListNew")
        mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList) 'Changed By Utkarsh On 19-Apr-2011
        mATAList = CType(Session("mATAList"), ATAList)
        ATAChapterId = CType(Session("ATAChapterId"), String)
        mAssemblylist = Session("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
        AssemblyId = CType(Session("AssemblyId"), String)
        ExtensionApplied = Session("ExtensionApplied")
        IsInReliability = Session("IsInReliability")
        DefectType = Session("DefectType")
        mIncidentTypeListForMELSnagCorrectiveActionListNew = CType(Session("mIncidentTypeListForMELSnagCorrectiveActionListNew"), IncidentTypeList)
        TypeOfIncidentID = CType(Session("TypeOfIncidentID"), Integer)
        ShowNoEntries = CType(Session("ShowNoEntries"), String)
    End Sub
    Private Sub SetSession()
        Session("mMELSnagCorrectiveActionListNew") = mMELSnagCorrectiveActionListNew
        Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
        Session("MachineID") = MachineID
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mATAList") = mATAList
        Session("AssemblyId") = AssemblyId
        Session("mIncidentTypeListForMELSnagCorrectiveActionListNew") = mIncidentTypeListForMELSnagCorrectiveActionListNew
        Session("ShowNoEntries") = ShowNoEntries
    End Sub
    Private Sub RemoveSession()
        mMachineNameValueList = Nothing
        Session.Remove("mMELSnagCorrectiveActionListNew")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mATAList")
        Session.Remove("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
        Session.Remove("AssemblyId")
        Session.Remove("ExtensionApplied")
        Session.Remove("IsInReliability")
        Session.Remove("DefectType")
        Session.Remove("mIncidentTypeListForMELSnagCorrectiveActionListNew")
        Session.Remove("TypeOfIncidentID")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfMELSnagCorrectiveActionListNew_AJAX.aspx?" Then
            Session.Remove("mMELSnagCorrectiveActionListNew")
            Session.Remove("mMELSnagCorrectiveAction")
            Session.Remove("Name")
            Session.Remove("mMachineNameValueList")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("AircraftId")
            Session.Remove("MachineID")
            Session.Remove("StatusCode")
            Session.Remove("MELSnagCode")
            Session.Remove("ATACode")
            Session.Remove("ATANomenclature")
            Session.Remove("ATAChapterId")
            Session.Remove("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
            Session.Remove("AssemblyId")
            Session.Remove("ExtensionApplied")
            Session.Remove("IsInReliability")
            Session.Remove("DefectType")
            Session.Remove("mIncidentTypeListForMELSnagCorrectiveActionListNew")
            Session.Remove("TypeOfIncidentID")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "FocusScript", str)
    End Sub
    Private Sub NewRecord()
        mMELSnagCorrectiveAction = MELSnagCorrectiveAction.NewMELSnagCorrectiveAction(mAssemblylist(1).AssemblyStatusID.ToString)
        Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction

        Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mMELSnagCorrectiveAction.ID) 'Sort = 1 : Installation
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mMELSnagCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(mID)
        Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
        Session("MachineID") = mMELSnagCorrectiveAction.MachineID.ToString
        Dim mtmpLog As Log = Log.GetLog(mMELSnagCorrectiveAction.LogID)
        Session("tmpLogDate") = mtmpLog.Date
        AircraftId = Session("MachineID")

        If mMELSnagCorrectiveAction.IsAttachmentAdded Then
            Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mMELSnagCorrectiveAction.ID) 'Sort = 1 - Installation
            Session("mFileAttach") = mFileAttach
        Else
            Dim mFileAttach As FileAttach
            mFileAttach = FileAttach.NewAttachment(Guid.Empty, mMELSnagCorrectiveAction.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mMELSnagDetail = mMELSnagCorrectiveAction.DefectNo + " Dated : " + mMELSnagCorrectiveAction.DateOfOccurrenceFormatted + " Log No. " + mMELSnagCorrectiveAction.LogNo
        MarkLog(Util.Action.Edit, "MEL/Snag Defect Corrective Action", mMELSnagDetail, Util.ErrorType.NoError, mID, EventLogID)
        FindNow(txtFromDate.Text.ToString, txtToDate.Text.ToString, cmbAircraft.SelectedValue.ToString, cmbStatus.SelectedValue,
                mATAList(New Guid(cmbATAChapter.SelectedValue)).ATACode, mATAList(New Guid(cmbATAChapter.SelectedValue)).ATANomenclature,
                cmbMELSnag.SelectedValue, cmbAssembly.SelectedValue.ToString, TypeOfIncidentID:=CInt(cmbIncidentType.SelectedValue))

        upnlGridView.Update()
        upnlActionBtnTop.Update()
        upnlResult.Update()

        Dim str As String
        str = "openLedgerSame('wfMELSnagCorrectiveActionNew_AJAX.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        DataFieldBind()
        SetControl()
        SetGrid()
        upnlGridView.Update()
        upnlActionBtnTop.Update()
        upnlResult.Update()

        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, " ", MsgBoxStyle.YesNo, "Delete")
        mMELSnagCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(mID)
        Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
    End Sub
    Private Sub FindNow(Optional ByVal FromDate As String = "1-1-1900", Optional ByVal ToDate As String = "1-1-3300", Optional ByVal MachineID As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional ByVal InvestigationStatus As Integer = 0, Optional ByVal ATACode As Integer = 0, Optional ByVal ATANomenclature As String = "",
                        Optional ByVal MELSnag As Integer = 0, Optional ByVal AssemblyStatusID As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional ByVal ExtensionApplied As Integer = 0, Optional ByVal IsInReliability As Integer = 0,
                        Optional ByVal DefectType As Integer = 0, Optional ByVal TypeOfIncidentID As Integer = -1)
        'Get List From the Database as per Criteria  
        If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then
            mMELSnagCorrectiveActionListNew = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(FromDate, ToDate, MachineID, InvestigationStatus,
                                                                                                               "HH:mm", ATACode, ATANomenclature, MELSnag,
                                                                                                               cmbAssembly.SelectedValue.ToString,
                                                                                                               ExtensionApplied, IsInReliability, DefectType, IncidentTypeID:=TypeOfIncidentID)
        Else
            mMELSnagCorrectiveActionListNew = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(FromDate, ToDate, MachineID, InvestigationStatus, ,
                                                                                                               ATACode, ATANomenclature, MELSnag,
                                                                                                               cmbAssembly.SelectedValue.ToString,
                                                                                                               ExtensionApplied, IsInReliability, DefectType, IncidentTypeID:=TypeOfIncidentID)
        End If
        'Set DataSource of the Grid
        dgSnagCorrectiveActionList.DataSource = mMELSnagCorrectiveActionListNew
        Session("mMELSnagCorrectiveActionListNew") = mMELSnagCorrectiveActionListNew
        dgSnagCorrectiveActionList.DataBind()
        SetGrid()
        lblResult.Text = "List of " + IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect", "MEL / Snag") + " Corrective Action as per criteria : " & mMELSnagCorrectiveActionListNew.Count.ToString & " Record(s) found."
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim TempID As Guid
                        Try

                            Session("sender") = ""
                            mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")
                            TempID = mMELSnagCorrectiveAction.ID
                            mMELSnagDetail = mMELSnagCorrectiveAction.DefectNo + " Dated : " + mMELSnagCorrectiveAction.DateOfOccurrenceFormatted + " Log No. " + mMELSnagCorrectiveAction.LogNo
                            MELSnagCorrectiveAction.DeleteMELSnagCorrectiveAction(mMELSnagCorrectiveAction.ID)
                            MarkLog(Util.Action.Delete, "MEL/Snag Defect Corrective Action", mMELSnagDetail, Util.ErrorType.NoError, TempID, EventLogID)
                            'Added By Utkarsh on 17-sep-2013 for Log_ajax changes
                            Session.Remove("mMELSnagCorrectiveAction")
                            'End
                            DataFieldBind()
                            SetControl()
                            lblResult.Text = "List of " + IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect", "MEL / Snag") + " Corrective Action as per criteria : " + mMELSnagCorrectiveActionListNew.Count.ToString & " Record(s) found."

                            upnlGridView.Update()
                            upnlActionBtnTop.Update()
                            upnlResult.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Util.Action.Delete, "Work Order", "Can't delete : " & mMELSnagDetail & " is Currently in use", Util.ErrorType.NoError, TempID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                            SetControl()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "SnagCorrectiveAction", mMELSnagCorrectiveAction.Name, Util.ErrorType.NoError, mMELSnagCorrectiveAction.ID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub SetControl()
        Name = Session("Name")
        StatusCode = CType(Session("StatusCode"), Integer)
        MELSnagCode = CType(Session("MELSnagCode"), Integer)
        ATACode = CType(Session("ATACode"), Integer)
        ATANomenclature = Session("ATANomenclature")

        ATAChapterId = Session("ATAChapterId")
        'FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "")
        'ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "")
        Dim mMachineID As String
        mMachineID = Session("AircraftId")
        If mMachineID = "" Then mMachineID = Guid.Empty.ToString
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        ExtensionApplied = CType(Session("ExtensionApplied"), Integer)
        IsInReliability = CType(Session("IsInReliability"), Integer)
        DefectType = CType(Session("DefectType"), Integer)
        'TypeOfIncidentID = CType(Session("TypeOfIncidentID"), Integer)

        FindNow(FromDate, ToDate, mMachineID, StatusCode, mATAList(New Guid(ATAChapterId)).ATACode, mATAList(New Guid(ATAChapterId)).ATANomenclature,
                MELSnagCode, AssemblyId, ExtensionApplied, IsInReliability, DefectType, TypeOfIncidentID:=TypeOfIncidentID)

        dgSnagCorrectiveActionList.DataBind()
        txtFromDate.Text = FromDate
        txtToDate.Text = ToDate
        cmbStatus.SelectedValue = StatusCode
        cmbMELSnag.SelectedValue = MELSnagCode
        cmbATAChapter.SelectedValue = ATAChapterId
        cmbAssembly.SelectedValue = AssemblyId
        cmbAircraft.SelectedValue = mMachineID
        cmbExtensionApplied.SelectedValue = ExtensionApplied
        cmbIncidentType.SelectedValue = TypeOfIncidentID
        upnlGridView.Update()
        upnlActionBtnTop.Update()
        'upnlActionBtnBottom.Update()
        upnlResult.Update()
    End Sub
    Private Sub SetGrid()
        Dim P As Boolean
		For j As Integer = 0 To dgSnagCorrectiveActionList.Rows.Count - 1

			P = mMELSnagCorrectiveActionListNew(j).IsAttachmentAdded
			If P = False Then
				dgSnagCorrectiveActionList.Rows.Item(j).Cells(23).Enabled = False
			End If
		Next
		'Sankalp 28-10-25 
		If AppSettings("ClientCode") = "CVA" Then
			dgSnagCorrectiveActionList.Columns(8).Visible = False
		Else
			dgSnagCorrectiveActionList.Columns(8).Visible = True
		End If
	End Sub
#End Region

#Region " DataBinding "
    Public Sub DataFieldBind()

        'Commented By Utkarsh On 19-Apr-2011

        'mMachineNameValueList = tmpMachineList.GetMachineList()
        '***********************************
        'Added By Utkarsh On 19-Apr-2011

        mMachineNameValueList = MachineNameValueList.GetMachineList("", SkipIsForInventoryAircarft:=True)
        '**********************************
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList

        If Not FromDate Is Nothing Then txtFromDate.Text = FromDate
        If Not ToDate Is Nothing Then txtToDate.Text = ToDate

        If mMachineNameValueList.Count <> 0 Then
            If IsNothing(AircraftId) Then AircraftId = mMachineNameValueList(0).ID.ToString Else AircraftId = AircraftId
        Else
            AircraftId = "00000000-0000-0000-0000-000000000000"
        End If


        StatusCode = Session("StatusCode")
        Session("StatusCode") = StatusCode

        MELSnagCode = Session("MELSnagCode")
        Session("MELSnagCode") = MELSnagCode

        ATACode = Session("ATACode")
        Session("ATACode") = ATACode

        ATANomenclature = Session("ATANomenclature")
        Session("ATANomenclature") = ATANomenclature

        ExtensionApplied = Session("ExtensionApplied")
        Session("ExtensionApplied") = ExtensionApplied

        IsInReliability = Session("IsInReliability")
        Session("IsInReliability") = IsInReliability

        DefectType = Session("DefectType")
        Session("DefectType") = DefectType

        Session("AircraftId") = AircraftId

        If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then
            mMELSnagCorrectiveActionListNew = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(txtFromDate.Text.ToString, txtToDate.Text.ToString,
                                                                                                               AircraftId, StatusCode, "HH:mm", ATACode,
                                                                                                               ATANomenclature, MELSnagCode,
                                                                                                               ExtensionApplied:=ExtensionApplied,
                                                                                                               IsInReliability:=IsInReliability, DefectType:=DefectType)
        Else
            mMELSnagCorrectiveActionListNew = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(txtFromDate.Text.ToString, txtToDate.Text.ToString,
                                                                                                               AircraftId, StatusCode, , ATACode,
                                                                                                               ATANomenclature, MELSnagCode,
                                                                                                               ExtensionApplied:=ExtensionApplied,
                                                                                                               IsInReliability:=IsInReliability, DefectType:=DefectType)
        End If
        dgSnagCorrectiveActionList.DataSource = mMELSnagCorrectiveActionListNew

        mATAList = ATAList.GetATAList("", "(All)") 'Added By Saylee on 12-Aug-2010
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList

        Name = Session("Name")
        Session("Name") = Name
        Session("mMELSnagCorrectiveActionListNew") = mMELSnagCorrectiveActionListNew
        If mATAList.Count <> 0 Then
            If IsNothing(ATAChapterId) Then ATAChapterId = mATAList(0).ID.ToString Else ATAChapterId = ATAChapterId
        Else
            ATAChapterId = "00000000-0000-0000-0000-000000000000"
        End If

        'Added By Vikrant On 02-Sept-2014 For All04092014
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, AircraftId, Today.Date.ToString, "(All)", True)
        cmbAssembly.DataSource = mAssemblylist
        Session("mAssemblylist") = mAssemblylist
        'End
        dgSnagCorrectiveActionList.Columns(19).HeaderText = IIf(AppSettings("MELSnagNomenclature") = "True", "Is ADD", "Is MEL") ' Added By Vikrant On 07-Sep-2020 For ALL07092020

        mIncidentTypeListForMELSnagCorrectiveActionListNew = IncidentTypeList.GetIncidentTypeList("(All)")
        cmbIncidentType.DataSource = mIncidentTypeListForMELSnagCorrectiveActionListNew
        Session("mIncidentTypeListForMELSnagCorrectiveActionListNew") = mIncidentTypeListForMELSnagCorrectiveActionListNew

        If mIncidentTypeListForMELSnagCorrectiveActionListNew.Count <> 0 Then
            If Session("TypeOfIncidentID") Is Nothing Then
                TypeOfIncidentID = mIncidentTypeListForMELSnagCorrectiveActionListNew(0).ID.ToString
                Session("TypeOfIncidentID") = TypeOfIncidentID
            Else
                TypeOfIncidentID = CType(Session("TypeOfIncidentID"), Integer)
            End If
        Else
            TypeOfIncidentID = -1
        End If

        DataBind()
        If mMachineNameValueList.Count > 1 And IsNothing(AircraftId) Then cmbAircraft.SelectedIndex = 0 Else cmbAircraft.SelectedValue = AircraftId
        AircraftId = cmbAircraft.SelectedValue
        Session("AircraftId") = AircraftId
        cmbStatus.SelectedValue = StatusCode
        cmbMELSnag.SelectedValue = MELSnagCode
        cmbExtensionApplied.SelectedValue = ExtensionApplied
        cmbIsInReliability.SelectedValue = IsInReliability
        cmbDefectType.SelectedValue = DefectType

        If mATAList.Count > 1 And IsNothing(AircraftId) Then cmbATAChapter.SelectedIndex = 0 Else cmbATAChapter.SelectedValue = ATAChapterId
        ATAChapterId = cmbATAChapter.SelectedValue
        Session("ATAChapterId") = ATAChapterId

        'If mIncidentTypeListForMELSnagCorrectiveActionListNew.Count > 1 And IsNothing(TypeOfIncidentID) Then cmbIncidentType.SelectedIndex = 0 Else cmbIncidentType.SelectedValue = TypeOfIncidentID
        'TypeOfIncidentID = cmbIncidentType.SelectedValue
        'Session("TypeOfIncidentID") = TypeOfIncidentID
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            ClearAll()
            GetSession()

            If Session("ShowNoEntries") Is Nothing Then
                ddlShowEntries.SelectedValue = "4"
                Session("ShowNoEntries") = ddlShowEntries.SelectedValue
                ShowNoEntries = ddlShowEntries.SelectedValue
            End If
            'End

            EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
            If Not IsPostBack And Session("sender") = "" Then
                If cmbAircraft.Enabled = True Then
                    setFocus(cmbAircraft)
                End If
                Session("MiddleFrame") = "wfMELSnagCorrectiveActionListNew_AJAX.aspx?"
                DataFieldBind()
                SetControl()

                'Added By Harsh on 7th Feb 2024
                If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "MELSnagCorrectiveAction") Then
                    ScriptManager.RegisterStartupScript(Me, [GetType], "MarkAsFavourite", "MarkAsFavourite();", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, [GetType], "RemoveFromFavourite", "RemoveFromFavourite();", True)
                End If

            Else
                dgSnagCorrectiveActionList.DataSource = mMELSnagCorrectiveActionListNew
                dgSnagCorrectiveActionList.DataBind()
            End If
            ' Added By Vikrant On 07-Sep-2020 For ALL07092020
            cmbMELSnag.Items(1).Text = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD", "MEL")
            cmbMELSnag.Items(2).Text = IIf(AppSettings("MELSnagNomenclature") = "True", "Defect", "Snag")
            dgSnagCorrectiveActionList.Columns(19).HeaderText = IIf(AppSettings("MELSnagNomenclature") = "True", "Is ADD", "Is MEL") ' Added By Vikrant On 07-Sep-2020 For ALL07092020
            'End
            lblResult.Text = "List of " + IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect", "MEL / Snag") + " Corrective Action as per criteria : " & mMELSnagCorrectiveActionListNew.Count.ToString & " Record(s) found."
            SetGrid()
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    Private Sub btnFindNow_Click(sender As Object, e As ImageClickEventArgs) Handles btnFindNow.Click
        Session("AircraftId") = cmbAircraft.SelectedValue
        Dim mMachineID As New Guid(cmbAircraft.SelectedValue)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "")
        StatusCode = cmbStatus.SelectedValue
        MELSnagCode = cmbMELSnag.SelectedValue
        AssemblyId = cmbAssembly.SelectedValue
        ExtensionApplied = cmbExtensionApplied.SelectedValue
        IsInReliability = cmbIsInReliability.SelectedValue
        DefectType = cmbDefectType.SelectedValue
        TypeOfIncidentID = CInt(cmbIncidentType.SelectedValue)

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("Name") = Name
        Session("ATACode") = mATAList(New Guid(cmbATAChapter.SelectedValue)).ATACode
        Session("ATANomenclature") = mATAList(New Guid(cmbATAChapter.SelectedValue)).ATANomenclature
        Session("ATAChapterId") = mATAList(New Guid(cmbATAChapter.SelectedValue)).ID.ToString
        Session("StatusCode") = StatusCode
        Session("MELSnagCode") = MELSnagCode
        Session("AssemblyId") = AssemblyId
        Session("ExtensionApplied") = ExtensionApplied
        Session("IsInReliability") = IsInReliability
        Session("DefectType") = DefectType
        Session("TypeOfIncidentID") = TypeOfIncidentID

        dgSnagCorrectiveActionList.PageIndex = 0
        FindNow(txtFromDate.Text.ToString, txtToDate.Text.ToString, mMachineID.ToString, StatusCode,
                mATAList(New Guid(cmbATAChapter.SelectedValue)).ATACode, mATAList(New Guid(cmbATAChapter.SelectedValue)).ATANomenclature,
                MELSnagCode, cmbAssembly.SelectedValue.ToString, ExtensionApplied, IsInReliability, DefectType, TypeOfIncidentID)

        upnlGridView.Update()
        upnlActionBtnTop.Update()
        upnlResult.Update()
    End Sub

    Private Sub dgSnagCorrectiveActionList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgSnagCorrectiveActionList.PageIndexChanging
        dgSnagCorrectiveActionList.PageIndex = e.NewPageIndex
        dgSnagCorrectiveActionList.DataSource = mMELSnagCorrectiveActionListNew
        Session("mMELSnagCorrectiveActionListNew") = mMELSnagCorrectiveActionListNew
        dgSnagCorrectiveActionList.DataBind()
        SetGrid()
    End Sub

    Private Sub dgSnagCorrectiveActionList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgSnagCorrectiveActionList.RowCommand
        Try
            Select Case e.CommandName
                Case "EditRec"
                    Dim Idx As Integer = CInt(e.CommandArgument) + dgSnagCorrectiveActionList.PageSize * dgSnagCorrectiveActionList.PageIndex
                    Dim mId As Guid = mMELSnagCorrectiveActionListNew(Idx).ID
                    'Added by Saylee on 8-Apr-2014 for ALL08042014
                    If (Not User.IsInRole("MELSnagCorrectiveActionView") And Not User.IsInRole("MELSnagCorrectiveActionEdit")) Then
                        'setObject()
                        SetSession()
                        MarkLog(Util.Action.Edit, "MELSnagCorrectiveAction", User.Identity.Name & " is not Authorized User to edit ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                        ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                        Exit Sub
                    End If
                    EditRecord(mId)
                Case "AttachRec"
                    Dim Idx As Integer = CInt(e.CommandArgument) + dgSnagCorrectiveActionList.PageSize * dgSnagCorrectiveActionList.PageIndex
                    Dim mId As Guid = mMELSnagCorrectiveActionListNew(Idx).ID
                    'Added by Saylee on 8-Apr-2014 for ALL08042014

                    If (Not User.IsInRole("MELSnagCorrectiveActionView")) Then
                        SetSession()
                        MarkLog(Action.View, "MELSnagCorrectiveAction", User.Identity.Name & " is not Authorized User to view ", ErrorType.HandledError, Guid.Empty, EventLogID)
                        ClientScript.RegisterStartupScript([GetType](), "OpenScript", MessageBox.Show("You are not authorized user"))
                        Exit Sub
                    End If
                    '----------------------------------------------------------------------
                    Dim No As New Random
                    Dim StrName As String = "abc" & No.Next.ToString
                    '----------------------------------------------------------------------
                    Dim mMELSnagCorrectiveAction As MELSnagCorrectiveAction

                    mMELSnagCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(mId)

                    DataFieldBind()
                    SetControl()
                    SetGrid()
                    upnlGridView.Update()
                    upnlActionBtnTop.Update()
                    upnlResult.Update()

                    Dim mFileAttach As FileAttach
                    mFileAttach = FileAttach.GetAttachment(mMELSnagCorrectiveAction.ID)
                    Session("mFileAttach") = mFileAttach

                    If mFileAttach.Size > 0 Then
                        Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                        Dim fs As FileStream
                        If File.Exists(AppSettings("DOCPath")) = False Then
                            'Delete File if exist
                            File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                            ' Create the file.
                            fs = File.Create(path)
                            '' Add some information to the file.
                            fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                            fs.Close()
                            Session("DOCPath") = path
                            Dim Str As String
                            Str = "openFile();"
                            ScriptManager.RegisterStartupScript(Me, [GetType], "openFile", Str, True)
                        End If
                    End If
                Case "PrintRec"
                    'Added by Saylee on 8-Apr-2014 for ALL08042014
                    If Not User.IsInRole("MELSnagCorrectiveActionPrint") Then
                        SetSession()
                        MarkLog(Action.Print, "MELSnagCorrectiveAction", User.Identity.Name & " is not Authorized User to print ", ErrorType.HandledError, Guid.Empty, EventLogID)
                        ClientScript.RegisterStartupScript([GetType](), "OpenScript", MessageBox.Show("You are not authorized user"))
                        Exit Sub
                    End If
                    Dim Idx As Integer = CInt(e.CommandArgument) + dgSnagCorrectiveActionList.PageSize * dgSnagCorrectiveActionList.PageIndex
                    Dim mId As Guid = mMELSnagCorrectiveActionListNew(Idx).ID
                    Dim mMELSnagCorrectiveActionID As Guid = mMELSnagCorrectiveActionListNew(Idx).ID
                    Dim MELTag As String = mMELSnagCorrectiveActionListNew(Idx).IsMEL
                    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
                    Dim ds As New dsMELSnagCorrectiveAction
                    Dim da As New CSLA.Data.ObjectAdapter
                    Dim mCompanyDetail As New CompanyDetail
                    Dim mrptMELSnagCorrectiveAction As rptMELSnagCorrectiveAction
                    mrptMELSnagCorrectiveAction = rptMELSnagCorrectiveAction.GetrptMELSnagCorrectiveAction(mMELSnagCorrectiveActionID.ToString)
                    Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                            mCompanyDetail.WebSite, "PRELIMINARY DEFECT REPORT", "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

                    If MELTag = "Yes" Then
                        Rpt = New crMELDetailReport
                    Else
                        Rpt = New crLogDefectActionList
                    End If
                    '-----------Added by Utkarsh for Report Logo---------------
                    Dim mrptImage As rptImage = rptImage.GetImage(ds)
                    '----------------------------------------------------------
                    da.Fill(ds, mrptMELSnagCorrectiveAction)
                    da.Fill(ds, Report)
                    da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
                    Rpt.SetDataSource(ds)
                    Session("myReport") = Rpt

                    DataFieldBind()
                    SetControl()
                    SetGrid()
                    upnlGridView.Update()
                    upnlActionBtnTop.Update()
                    upnlResult.Update()

                    Dim Str As String
                    Str = "openTranDetail();"
                    ScriptManager.RegisterStartupScript(Me, [GetType](), "openTranDetail", Str, True)
                Case "DeleteRec"
                    Dim Idx As Integer = CInt(e.CommandArgument) + dgSnagCorrectiveActionList.PageSize * dgSnagCorrectiveActionList.PageIndex
                    Dim mId As Guid = mMELSnagCorrectiveActionListNew(Idx).ID
                    'Added by Saylee on 8-Apr-2014 for ALL08042014
                    If Not User.IsInRole("MELSnagCorrectiveActionDelete") Then
                        SetSession()
                        MarkLog(Util.Action.Delete, "MELSnagCorrectiveAction", User.Identity.Name & " is not Authorized User to delete ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                        ClientScript.RegisterStartupScript([GetType](), "OpenScript", MessageBox.Show("You are not authorized user"))

                        Exit Sub
                    End If
                    DeleteRecord(mId)
            End Select
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click

        Try
            'Added by Saylee on 8-Apr-2014 for ALL08042014
            If (Not User.IsInRole("MELSnagCorrectiveActionNew") And Not User.IsInRole("MELSnagCorrectiveActionEdit")) Then
                SetSession()
                MarkLog(Action.New, "MELSnagCorrectiveAction", User.Identity.Name & " is not Authorized User to add ", ErrorType.HandledError, Guid.Empty, EventLogID)
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            NewRecord()
            Session("MachineID") = cmbAircraft.SelectedValue.ToString
            Session("AircraftRegNo") = cmbAircraft.SelectedItem.ToString
            AircraftId = Session("MachineID")
            mMELSnagCorrectiveAction.MachineID = New Guid(AircraftId)
            Session("mMELSnagCorrectiveActionListNew") = mMELSnagCorrectiveActionListNew
            Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
            MarkLog(Action.[New], "MEL/Snag Defect Corrective Action", "", ErrorType.NoError, Guid.Empty, EventLogID)   'Added By Prashant 20-Jul-2011
            Dim str As String
            str = "openLedgerSame('wfMELSnagCorrectiveActionNew_AJAX.aspx?BackPage=index.aspx');"
            ScriptManager.RegisterStartupScript(Me, [GetType], "Open Script", str, True)
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAircraft.SelectedIndexChanged

        Try
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue.ToString, Today.Date.ToString, "(All)", True)
            cmbAssembly.DataSource = mAssemblylist
            Session("mAssemblylist") = mAssemblylist
            cmbAssembly.DataBind()
            Session("AircraftId") = cmbAircraft.SelectedValue
            Dim mMachineID As New Guid(cmbAircraft.SelectedValue)

            FindNow(txtFromDate.Text.ToString, txtToDate.Text.ToString, cmbAircraft.SelectedValue.ToString, cmbStatus.SelectedValue,
                    mATAList(New Guid(cmbATAChapter.SelectedValue)).ATACode, mATAList(New Guid(cmbATAChapter.SelectedValue)).ATANomenclature,
                    cmbMELSnag.SelectedValue, cmbAssembly.SelectedValue.ToString, Val(cmbExtensionApplied.SelectedValue), Val(cmbIsInReliability.SelectedValue),
                    Val(cmbDefectType.SelectedValue), TypeOfIncidentID:=CInt(cmbIncidentType.SelectedValue))

            upnlGridView.Update()
            upnlActionBtnTop.Update()
            upnlResult.Update()
            upnlAvanceSearchContent.Update()
            If cmbAircraft.Enabled = True Then
                setFocus(cmbAircraft)
            End If
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    Private Sub dgSnagCorrectiveActionList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSnagCorrectiveActionList.Sorting
        mMELSnagCorrectiveActionListNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMELSnagCorrectiveActionListNew") = mMELSnagCorrectiveActionListNew
        dgSnagCorrectiveActionList.DataSource = mMELSnagCorrectiveActionListNew
        dgSnagCorrectiveActionList.DataBind()
        SetGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    'Added By on Harsh on 7th Feb 2024
    Protected Sub ddlShowEntriesIndexChanged(sender As Object, e As EventArgs)
        Try
            dgSnagCorrectiveActionList.PageSize = CInt(ddlShowEntries.SelectedItem.ToString)
            dgSnagCorrectiveActionList.DataSource = mMELSnagCorrectiveActionListNew
            dgSnagCorrectiveActionList.DataBind()
            SetControl()
            upnlGridView.Update()
        Catch ex As Exception
            Throw ex.GetBaseException
        End Try
    End Sub
    'Added by Harsh on 7th Feb 2024
    Private Sub HdnBtnMarkFav_Click(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click
        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, "MELSnagCorrectiveAction")
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub

    Private Sub HdnBtnRemoveFav_Click(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click
        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, "MELSnagCorrectiveAction")
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub
    'End

#End Region


End Class