Imports System.Linq


Public Class wfSpareAssemblyStatus
    Inherits System.Web.UI.Page


#Region " Assembly Status "
#Region " Variable Declaration "
    Public mAssemblyStatus As AssemblyStatus
    Public mModelList As ModelList

    Private Flag As Int16
    Public mSelectPeriods As SelectPeriods
    Public mSelectPeriod As SelectPeriod
    Public mATAList As ATAList
    Dim EventLogID As Guid 'Added By Utkarsh On 29-Jul-2011 For All19072011
    Dim MachineDetail As String 'Added By Utkarsh On 29-Jul-2011 For All19072011
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    'MLNo
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Shared UserNameForLicenceList As String
    'End
    Public mAssemblyTypeListForUI As AssemblyTypeListForUI
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mAssemblyTypeListForUI = Session("mAssemblyTypeListForUI")
        mModelList = CType(Session("mModelList"), ModelList)
        mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
        mATAList = CType(Session("mATAList"), ATAList)
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
    End Sub
    Private Sub SetSession()
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mAssemblyTypeListForUI") = mAssemblyTypeListForUI
        Session("mSelectPeriods") = mSelectPeriods
        Session("mModelList") = mModelList
        Session("mATAList") = mATAList
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub GetAttachment()
        If mAssemblyStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mAssemblyStatus.ID
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                    'mFileAttach = Nothing
                    'Session("mFileAttach") = mFileAttach
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mAssemblyStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mAssemblyStatus.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If

    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString

        GetAttachment()

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
    Private Sub ControlVisibilityForAttachment()
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                ImageButton1.Visible = True
                btnDelAttach.Enabled = True
            Else
                ImageButton1.Visible = False
            End If
        Else
            ImageButton1.Visible = False
        End If

    End Sub
    Private Sub NewRecord()
        mAssemblyStatus = AssemblyStatus.NewSpareAssemblyStatus(Val(cmbAssemblyType.SelectedValue.ToString))
        Session("mAssemblyStatus") = mAssemblyStatus
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mModelList")
        Session.Remove("mATAList")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    'Added By Vikrant On 25-Jun-2014
    Private Sub RemoveAllSessionValues()
        Session.Remove("Add")
        Session.Remove("Edit")
        Session.Remove("mAssemblyStatusList")
        Session.Remove("mAssemblyStatus")
        Session.Remove("mAssemblyTypeListForUI")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'MLNo 
        Session.Remove("mAssemblyTypeListForUI")
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    'End
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        Save()
                        NewRecord()
                        DataFieldBind()
                        ControlVisibility()
                        SetRights()
                        SetPage()
                        upnlActionBtn.Update()
                        upnlATADetails.Update()
                        upnlDocumentDetails.Update()

                        upnlInstallationDetails.Update()
                        upnlModelDetails.Update()
                        upnlSinceNew.Update()
                        upnlTitle.Update()

                        'Response.Redirect("wfAssemblystatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
                    End If

                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        NewRecord()
                        'Response.Redirect("wfAssemblyStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
                        DataFieldBind()
                        ControlVisibility()
                        SetRights()  'Added By Utkarsh On 14-Mar-2011
                        SetPage()
                        upnlActionBtn.Update()
                        upnlATADetails.Update()
                        upnlDocumentDetails.Update()

                        upnlInstallationDetails.Update()
                        upnlModelDetails.Update()
                        upnlSinceNew.Update()
                        upnlTitle.Update()
                    ElseIf MSGBoxCtrl.Sender = "NextPage" Then
                        Session("sender") = ""
                        Session.Remove("Flag")
                        Session("mAssemblyStatus") = mAssemblyStatus
                        Response.Redirect("wfAssemblyStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
                    End If
                Case MsgBoxResult.Cancel
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        ' Response.Redirect("wfAssemblyStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
                    End If
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    upnlActionBtn.Update()
                    upnlATADetails.Update()
                    upnlDocumentDetails.Update()

                    upnlInstallationDetails.Update()
                    upnlModelDetails.Update()
                    upnlSinceNew.Update()
                    upnlTitle.Update()
                    '' Response.Redirect("wfAssemblyStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    upnlActionBtn.Update()
                    upnlATADetails.Update()
                    upnlDocumentDetails.Update()

                    upnlInstallationDetails.Update()
                    upnlModelDetails.Update()
                    upnlSinceNew.Update()
                    upnlTitle.Update()
                    'Response.Redirect("wfAssemblyStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfAssemblyStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
    Private Sub SetObject()
        With mAssemblyStatus
            .Assembly.ModelID = New Guid(cmbModel.SelectedValue)
            .ATAID = New Guid(cmbATAChapter.SelectedValue)
            .Assembly.SerialNo = Trim(txtSerialNo.Text)

            'CNDC
           
            'If Not IsDate(calFromDate.Text) Then
            '    .InstalledOn = System.DBNull.Value
            'Else
            '    .InstalledOn = CType(Trim(calFromDate.Text), Object)
            'End If

            .InstallationRemark = Trim(txtNote.Text)
            'Set Peroid


            Dim LicenseNo As String = String.Empty
            Dim EmpName As String = String.Empty
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
            Else
                LicenseNo = Trim(txtLicenceNo.Text)
            End If
            .InstLicenseNo = LicenseNo
            .InstDoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID

            'End 

            .InstPlace = txtPlace.Text.Trim
            .InstallationReason = Trim(txtInstallationReason.Text) 'Added By Vikrant On 09-Apr-2014 For ALL09042014-1


            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    .IsAttachmentAdded = True
                Else
                    .IsAttachmentAdded = False
                End If
            End If

        End With
        Session("mAssemblyStatus") = mAssemblyStatus
    End Sub
    Private Sub SetGridObject()
        Dim txtAssemblyInstallationValue As TextBox
        If mAssemblyStatus.AssemblyTypeID <> 1 Then
           

            For j As Integer = 0 To Me.dgCurrentMachineValue.Rows.Count - 1
                txtAssemblyInstallationValue = CType(Me.dgCurrentMachineValue.Rows(j).FindControl("txtAssemblyInstallationValue"), TextBox)

                'AssemblyInstallationValue
                With mAssemblyStatus.AssemblyStatusPeriods
                    If .Item(j).PeriodID = 2 Then
                        If Not Period.IsDate(txtAssemblyInstallationValue.Text) Then
                            .Item(j).AssemblyInstallationValue = ""
                        Else
                            .Item(j).AssemblyInstallationValueFormatted = Trim(txtAssemblyInstallationValue.Text)
                        End If
                    Else
                        If mAssemblyStatus.AssemblyStatusPeriods(j).PeriodID <> 2 And txtAssemblyInstallationValue.Text.Trim = "" Then 'This If Condition added by vikrant on 19-Jun-2020 to save 0 instead of null if nothing enetered in TextBox
                            .Item(j).AssemblyInstallationValue = New Period(mAssemblyStatus.AssemblyStatusPeriods(j).PeriodID, 0).Value
                        Else
                            .Item(j).AssemblyInstallationValue = Trim(txtAssemblyInstallationValue.Text)
                        End If

                    End If
                End With
               
            Next j
        End If
        Session("mAssemblyStatus") = mAssemblyStatus

    End Sub

    Private Sub SetGridHeader()
        Select Case mAssemblyStatus.AssemblyTypeID
            Case 4
                dgCurrentMachineValue.Columns(2).HeaderText = "A.P.U."

            Case 5
                If (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet" Or AppSettings("ClientCode") = "ACC") Then
                    dgCurrentMachineValue.Columns(2).HeaderText = "A.C."

                ElseIf AppSettings("ClientCode") = "Indamer" Then 'Added by Vikrant on 26-sept-2011 For ALL26092011-1
                    dgCurrentMachineValue.Columns(2).HeaderText = "Air-Conditioning"

                Else
                    dgCurrentMachineValue.Columns(2).HeaderText = "C.G.B."

                End If
                'Added by Saylee on 15-Feb-2013 for UHPL15022013
            Case 8
                If (AppSettings("ClientCode") = "UHPL") Then
                    dgCurrentMachineValue.Columns(2).HeaderText = "M.R.H."

                End If
            Case 9
                If (AppSettings("ClientCode") = "UHPL") Then
                    dgCurrentMachineValue.Columns(2).HeaderText = "S.P.S."

                End If
            Case 10
                If (AppSettings("ClientCode") = "UHPL") Then
                    dgCurrentMachineValue.Columns(2).HeaderText = "S.S.A."

                End If
                'End of Addition by Saylee
            Case Else
                dgCurrentMachineValue.Columns(2).HeaderText = mAssemblyStatus.AssemblyTypeName

        End Select
    End Sub
    Private Sub SetPage()
        'set the captions
        If mAssemblyStatus.IsNew Then
            lblTitle.Text = "Build Assembly [New]"
        Else
            lblTitle.Text = mAssemblyStatus.AssemblyTypeName & " [Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]"
        End If
        If IsDate(mAssemblyStatus.InstalledOn) Then
            'Code Commented and newly added on 28-05-2007 by Saylee ---------------------
            ''calFromDate.TitleText = CDate(mAssemblyStatus.InstalledOn)
            ''calFromDate.DateToday = CDate(mAssemblyStatus.InstalledOn)
            ''calFromDate.SelectedDate = CDate(mAssemblyStatus.InstalledOn)
            ''calFromDate.Text = CDate(mAssemblyStatus.InstalledOn)
            '----------------------------------------------------------------------------
            'ElseIf IsDate(mAssemblyStatus.AsOnDate) Then
            '    'Code Commented and newly added on 28-05-2007 by Saylee ---------------------
            '    ''calFromDate.TitleText = CDate(mAssemblyStatus.AsOnDate)
            '    ''calFromDate.DateToday = CDate(mAssemblyStatus.AsOnDate)
            '    ''calFromDate.SelectedDate = CDate(mAssemblyStatus.AsOnDate)
            '    ' calFromDate.Text = CDate(mAssemblyStatus.AsOnDate)
            '    '----------------------------------------------------------------------------
        End If
        If Not mAssemblyStatus.EnablePanel Then
            txtSerialNo.BackColor = Color.Gainsboro
            
            txtNote.BackColor = Color.Gainsboro
        End If
        'Not mAssemblyStatus.EnablePanel 
        'lblInstallationInfo.InnerText = "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName
        lblInstallationInfo.InnerText = mAssemblyStatus.AssemblyTypeName & " Details"
        lblDocumentationValueCaption.InnerText = "Document Information of the " & mAssemblyStatus.AssemblyTypeName
        lblTSN.InnerText = "Since New Values as on " & mAssemblyStatus.AsOnDateFormatted
    End Sub
    Private Sub ControlVisibility()

        '''''''''  dgCurrentMachineValue.Columns(2).Visible = Not (mAssemblyStatus.AssemblyTypeID = 1)  '(mAssemblyStatus.AssemblyTypeID < 3 And Not mAssemblyStatus.HasLogCount And Not mAssemblyStatus.AssemblyTypeID = 1)
        
        
        btnPrint.Enabled = Not mAssemblyStatus.IsNew
        '
        cmbAssemblyType.Enabled = mAssemblyStatus.IsNew
        ''''''  dgCurrentMachineValue.Columns(4).Visible = ((mAssemblyStatus.AssemblyTypeID <> 1) And Not mAssemblyStatus.HasLogCount = True)

        ''''''' btnAddPeriod.Enabled = ((Not mAssemblyStatus.AssemblyTypeID = 1) And Not mAssemblyStatus.HasLogCount = True)


        If (mAssemblyStatus.HasLogCount = True) Then
            For i As Integer = 0 To dgCurrentMachineValue.Rows.Count - 1
                Dim txtCurrentAssemblyValue As TextBox = CType(Me.dgCurrentMachineValue.Rows(i).FindControl("txtCurrentAssemblyValue"), TextBox)
                CType(Me.dgCurrentMachineValue.Rows(i).FindControl("txtCurrentAssemblyValue"), TextBox).ReadOnly = True
            Next
        End If
        'For i As Integer = 0 To dgInstallationValue.Rows.Count - 1
        '    Dim txtAssemblyInstallationValue As TextBox = CType(Me.dgInstallationValue.Rows(i).FindControl("txtAssemblyInstallationValue"), TextBox)
        '    Dim txtMachineInstallationValue As TextBox = CType(Me.dgInstallationValue.Rows(i).FindControl("txtMachineInstallationValue"), TextBox)
        '    CType(Me.dgInstallationValue.Rows(i).FindControl("txtAssemblyInstallationValue"), TextBox).ReadOnly = Not mAssemblyStatus.EnablePanel
        '    CType(Me.dgInstallationValue.Rows(i).FindControl("txtMachineInstallationValue"), TextBox).ReadOnly = Not mAssemblyStatus.EnablePanel
        'Next
        ControlVisibilityForAttachment()
    End Sub
    Private Sub AddSelectedPeroids()
        Dim mSelectPeriod As SelectPeriod
        If IsNothing(mSelectPeriods) Then
            mSelectPeriods = SelectPeriods.NewSelectPeriods
        End If
        'this is to add the selected periods from the SelectPeriod form
        For Each mSelectPeriod In mSelectPeriods
            If mSelectPeriod.IsSelected Then
                mAssemblyStatus.AssemblyStatusPeriods.Add(AssemblyStatusPeriod.NewChildSpareAssemblyStatusPeriod(mAssemblyStatus.ID, "", mAssemblyStatus.Assembly.Model.AssemblyTypeID, mSelectPeriod.PeriodID, , True))
            End If
        Next
        Session("mAssemblyStatus") = mAssemblyStatus
        Session.Remove("mSelectPeriods")
        mSelectPeriods = Nothing
    End Sub
   
    Private Sub SetPeriods()
        mSelectPeriods = SelectPeriods.NewSelectPeriods
        Dim i As Integer
        Dim mPeriodList As PeriodList
        mPeriodList = PeriodList.GetPeriodList
        If mAssemblyStatus.AssemblyTypeID = 1 Or mAssemblyStatus.AssemblyTypeID = 2 Or mAssemblyStatus.AssemblyTypeID = 4 Then
            While i <= mPeriodList.Count - 1
                If Not mAssemblyStatus.AssemblyStatusPeriods.Contains(mPeriodList(i).ID) Then
                    mSelectPeriods.Add(mPeriodList(i).ID, mPeriodList(i).PeriodName)
                End If
                i = i + 1
            End While
            Session("mSelectPeriods") = mSelectPeriods
        Else
            While i <= mAssemblyStatus.AssemblyStatusPeriods.Count - 1
                If Not mAssemblyStatus.AssemblyStatusPeriods.Contains(mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID) Then
                    mSelectPeriods.Add(mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID, mAssemblyStatus.AssemblyStatusPeriods(i).PeriodName)
                End If
                i = i + 1
            End While
            Session("mSelectPeriods") = mSelectPeriods
        End If
    End Sub


    Private Function Save() As Boolean
        If Not IsValid Then Exit Function
        Dim AssemblyStatusClone As AssemblyStatus
        AssemblyStatusClone = CType(mAssemblyStatus.Clone, AssemblyStatus)
        SetObject()
        SetGridObject()
        If mAssemblyStatus.IsValid = True Then
            Try
                'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
                If Not mAssemblyStatus.InstDoneByID.Equals(Guid.Empty) AndAlso mAssemblyStatus.InstalledOn.ToString.Length > 0 Then
                    Dim Title As String = "Save Alert !"
                    Dim Message As String = ""
                    Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyStatus.InstDoneByID.ToString, mAssemblyStatus.InstalledOn.ToString)
                    If mEmployeeStatus(0).Information <> "" Then
                        Message = mEmployeeStatus(0).Information
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(Title, Message, IsTagRequired:=False), True)
                        Return False
                    End If
                End If
                'End
                mAssemblyStatus.ApplyEdit()
                mAssemblyStatus = CType(mAssemblyStatus.Save(), AssemblyStatus)
                SaveAttachment()
                Session("mAssemblyStatus") = mAssemblyStatus
                'lblTitle.Text = "Assembly (Saved.....)"
                Return True
            Catch ex As SqlException
                Session("AssemblyStatusClone") = AssemblyStatusClone
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfAssemblyStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
                    msg1.Show()
                ElseIf ex.Number = 8145 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfAssemblyStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
                    msg1.Show()
                ElseIf ex.Number = 2627 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfAssemblyStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
                    msg1.Show()
                ElseIf ex.Number = 547 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfAssemblyStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
                    msg1.Show()
                End If
                Return False
            Finally
                AssemblyStatusClone = Nothing
                'Added By Utkarsh On 29-Jul-2011 For All19072011

                MachineDetail = "Model : " & mAssemblyStatus.ModelName & " Type : " & mAssemblyStatus.AssemblyTypeName & " Serial No. : " & mAssemblyStatus.Assembly.SerialNo
                MarkLog(Util.Action.Save, "Assembly Status", MachineDetail, Util.ErrorType.NoError, mAssemblyStatus.ID, EventLogID)
                'End
            End Try
        Else
            Return False
        End If
    End Function

    Private Sub GetAssemblyStatusForModel(ByVal PartIndex As Integer) 'Added by Saylee on 25-Aug-2009

        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)

        Dim mtmpAssemblyListOnModelSelection As tmpAssemblyListOnModelSelection = tmpAssemblyListOnModelSelection.GetAssemblyListOnModelSelection(mAssemblyStatus.AssemblyTypeName, Guid.Empty.ToString, mModelList(New Guid(cmbModel.SelectedValue)).ModelName)

        If mtmpAssemblyListOnModelSelection.Count > 0 Then
            'Dim tmpAssemblyStatus As AssemblyStatus = AssemblyStatus.GetInstallAssemblyStatus(mtmpAssemblyListOnModelSelection(0).ID, mtmpAssemblyListOnModelSelection(0).InstalledOn.ToString)
            Dim tmpAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mtmpAssemblyListOnModelSelection(0).ID)
            mAssemblyStatus.ATAID = tmpAssemblyStatus.ATAID
            mAssemblyStatus.Assembly.Model.ManufacturerID = tmpAssemblyStatus.Assembly.Model.ManufacturerID
            mAssemblyStatus.Assembly.ModelID = tmpAssemblyStatus.Assembly.ModelID

            If mAssemblyStatus.AssemblyStatusPeriods.Count > 0 Then
                For i As Integer = mAssemblyStatus.AssemblyStatusPeriods.Count - 1 To 0 Step -1
                    '-----------ClientCode Checked By Vikrant on 19 Dec 2011 for Buddha Air-------------------
                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                        If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 1 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 3 Then
                            mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatus.AssemblyStatusPeriods(i).ID)
                        End If
                    Else
                        If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 1 Then
                            mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatus.AssemblyStatusPeriods(i).ID)
                        End If
                    End If
                    '--------------------------------------------------------------------------------------------
                Next
                dgCurrentMachineValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
                dgCurrentMachineValue.DataBind()

            End If

            Dim tmpAssemblyStatusPeriod As AssemblyStatusPeriod
            For Each tmpAssemblyStatusPeriod In tmpAssemblyStatus.AssemblyStatusPeriods
                If Not mAssemblyStatus.AssemblyStatusPeriods.Contains(tmpAssemblyStatusPeriod.PeriodID) Then
                    mAssemblyStatus.AssemblyStatusPeriods.Add(AssemblyStatusPeriod.NewChildSpareAssemblyStatusPeriod(mAssemblyStatus.ID, "", mAssemblyStatus.AssemblyTypeID, tmpAssemblyStatusPeriod.PeriodID, , True))
                    mAssemblyStatus.AssemblyStatusPeriods.Item(tmpAssemblyStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted = ""

                End If
            Next
            Session("mAssemblyStatus") = mAssemblyStatus
            dgCurrentMachineValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
            DataBind()
            tmpAssemblyStatus = Nothing
        Else
            If mAssemblyStatus.AssemblyStatusPeriods.Count > 0 Then
                For i As Integer = mAssemblyStatus.AssemblyStatusPeriods.Count - 1 To 0 Step -1
                    '-----------ClientCode Checked By Vikrant on 19 Dec 2011 for Buddha Air-------------------
                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                        If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 1 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 3 Then
                            mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatus.AssemblyStatusPeriods(i).ID)
                        End If
                    Else
                        If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 1 Then
                            mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatus.AssemblyStatusPeriods(i).ID)
                        End If
                    End If
                    '--------------------------------------------------------------------------------------------
                Next
                dgCurrentMachineValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods

                dgCurrentMachineValue.DataBind()

            End If


        End If


        ''Dim mAssemblyStatusList As AssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(mAssemblyStatus.AssemblyID, , mAssemblyStatus.CompID.ToString, mPartlist(PartIndex).ID, mPartlist(PartIndex).Name, mPartlist(PartIndex).Description, , , , True, True, True, , , , , mAssemblyStatus)
    End Sub
    'Added By Utkarsh On 14-Mar-2011
    Private Sub SetRights()
        If (Not User.IsInRole("MachineAssemblyPrint")) Then
            btnPrint.Enabled = False
            btnPrint.ToolTip = "You are not authorized user"
        End If
        If (User.IsInRole("MachineAssemblyNew") Or User.IsInRole("MachineAssemblyEdit")) = False Then
            btnSave.Enabled = False
            btnSave.ToolTip = "You are not authorized user"
        End If
    End Sub
    '*******************************
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList("", "(SELECT)")
        cmbATAChapter.DataSource = mATAList
        Session("mATAList") = mATAList
        dgCurrentMachineValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
       
        SetGridHeader()

        mAssemblyTypeListForUI = AssemblyTypeListForUI.GetAssemblyTypeListForUI("(SELECT)")
        cmbAssemblyType.DataSource = mAssemblyTypeListForUI
        Session("mAssemblyTypeListForUI") = mAssemblyTypeListForUI

      
        mModelList = ModelList.GetModelList(mAssemblyTypeListForUI(0).ID, , , , "(SELECT)")
        cmbModel.DataSource = mModelList
        Session("mModelList") = mModelList

        BindLicenceNo() 'MLNo

        DataBind()



        If Not mAssemblyStatus.IsNew Then
            cmbAssemblyType.SelectedValue = mAssemblyStatus.AssemblyTypeID
            mModelList = ModelList.GetModelList(cmbAssemblyType.SelectedIndex + 1, , , , "(SELECT)")
            cmbModel.DataSource = mModelList
            cmbModel.DataBind()
        End If

        If mFileAttach Is Nothing Then
            If mAssemblyStatus.IsAttachmentAdded = True Then
                mFileAttach = FileAttach.GetAttachment(mAssemblyStatus.ID) 'Sort = 1 - Installation
            Else
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyStatus.ID)
            End If
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub DataBindGrid()
        mAssemblyStatus = Session("mAssemblyStatus")
        dgCurrentMachineValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods

        dgCurrentMachineValue.DataBind()

    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 500 Then
                custValidator.ErrorMessage = "Max. length of Note should be 500 char"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "cmbModel" Then
            If cmbModel.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select Model from the list"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "cmbATAChapter" Then
            If cmbATAChapter.SelectedIndex = 0 And mAssemblyStatus.AssemblyTypeID <> 1 Then
                custValidator.ErrorMessage = "Please select ATA Chapter from the list"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'Added By Utkarsh On 12-Jun-2012 FOR ALL08062012
        ElseIf custValidator.ControlToValidate = "txtLicenceNo" Then
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or (txtLicenceNo.Text.Trim.IndexOf("[") < 0 And txtLicenceNo.Text.Trim.IndexOf("]") < 0) Then
                e.IsValid = True
            Else
                custValidator.ErrorMessage = "Enter Correct License No."
                e.IsValid = False
            End If
            'End

        End If
    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'this is for grid validation
        SetObject()
        SetGridObject()
        Dim str As String = ""
        Dim txtCurrentAssemblyValue As TextBox
        If Not mAssemblyStatus.Assembly.IsValid Then
            For i As Integer = 0 To mAssemblyStatus.Assembly.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyStatus.Assembly.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        If Not mAssemblyStatus.IsValid Then
            For i As Integer = 0 To mAssemblyStatus.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgCurrentMachineValue.Rows.Count - 1)
            txtCurrentAssemblyValue = CType(Me.dgCurrentMachineValue.Rows(i).FindControl("txtCurrentAssemblyValue"), TextBox)
            If Not mAssemblyStatus.AssemblyStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyStatus.AssemblyStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyStatus.AssemblyStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
    Public Function CustomValidate2() As Boolean
        Dim str As String = ""
        For i As Integer = 0 To CShort(dgCurrentMachineValue.Rows.Count - 1)
            If Not mAssemblyStatus.AssemblyStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyStatus.AssemblyStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyStatus.AssemblyStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            cvModelList.ErrorMessage = str
            cvModelList.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region " Event "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        GetSessionComp()
        GetSessionService()
        GetSessionInsp()
        GetSessionMod()
        GetSessionParameter()

        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 29-Jul-2011 For All19072011
        If Not IsPostBack Then
            If cmbATAChapter.Enabled = True Then
                setFocus(cmbATAChapter)
            End If
            AddSelectedPeroids()
            DataFieldBind()
            ControlVisibility()
            SetRights()  'Added By Utkarsh On 14-Mar-2011
            SetPage()
            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End
            TbContInst.ActiveTabIndex = IIf(CType(Session("AssemblyInstTabIndex"), Integer) > 0, CType(Session("AssemblyInstTabIndex"), Integer), 0)
            If CType(Session("AssemblyInstTabIndex"), Integer) > 0 Then
                Call TbContInst_ActiveTabChanged(Nothing, Nothing)
            End If
        End If

    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If IsValid Then
            If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub
            If Save() = True Then
                DataFieldBind()
                ControlVisibility()
                SetRights()
                SetPage()
                upnlActionBtn.Update()
                upnlATADetails.Update()
                upnlDocumentDetails.Update()

                upnlInstallationDetails.Update()
                upnlModelDetails.Update()
                upnlSinceNew.Update()
                upnlTitle.Update()
                upnlTabs.Update()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnDelAttach_Click(sender As Object, e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        GetAttachment()
        'mEmployee.ImageFile = file1
        'mEmployee.ImageSize = 0
        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        ControlVisibilityForAttachment()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mAssemblyStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAssemblyStatus.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Protected Sub txtCurrentAssemblyValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtCurrentAssemblyValue As TextBox
        For i As Integer = 0 To mAssemblyStatus.AssemblyStatusPeriods.Count - 1
            txtCurrentAssemblyValue = CType(Me.dgCurrentMachineValue.Rows(i).FindControl("txtAssemblyInstallationValue"), TextBox)
            If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID = 2 Then
                If Period.IsDate(txtCurrentAssemblyValue.Text) Then
                    mAssemblyStatus.AssemblyStatusPeriods.Item(i).AssemblyInstallationValueFormatted = Trim(txtCurrentAssemblyValue.Text)
                Else
                    mAssemblyStatus.AssemblyStatusPeriods.Item(i).AssemblyInstallationValueFormatted = ""
                End If
            Else
                mAssemblyStatus.AssemblyStatusPeriods.Item(i).AssemblyInstallationValue = Trim(txtCurrentAssemblyValue.Text)
            End If
        Next i
        Session("mAssemblyStatus") = mAssemblyStatus
        DataBindGrid()
        SetGridHeader()
    End Sub
    Private Sub dgCurrentMachineValue_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCurrentMachineValue.RowCommand

        Select Case e.CommandName
            Case "CurrentValue"

            Case "DeleteRec"

                Dim Index As Int32 = CInt(e.CommandArgument) + dgCurrentMachineValue.PageIndex * dgCurrentMachineValue.PageSize
                If mAssemblyStatus.AssemblyStatusPeriods(Index).PeriodID = 1 Or mAssemblyStatus.AssemblyStatusPeriods(Index).PeriodID = 2 Or mAssemblyStatus.AssemblyStatusPeriods(Index).PeriodID = 3 Then
                    SetObject()
                    SetGridObject()
                    SetPeriods()
                    SetSession()
                End If


                If (Not User.IsInRole("MachineAssemblyNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("MachineAssemblyEdit") And Not mAssemblyStatus.IsNew) Then
                    '  ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '*****************************
                If mAssemblyStatus.AssemblyStatusPeriods(Index).HasMonitorCount(mAssemblyStatus.ID, mAssemblyStatus.AssemblyStatusPeriods(Index).PeriodID) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.MonitorExist, MSGBox.Message_text.MonitorExist, "Selected " & mAssemblyStatus.AssemblyTypeName & " Period cannot be removed as monitor entry exist", MsgBoxStyle.OkOnly, "")

                ElseIf mAssemblyStatus.AssemblyStatusPeriods(Index).HasCompStatusPeriod Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ComponentPeriodExist, MSGBox.Message_text.ComponentPeriodExist, "Selected " & mAssemblyStatus.AssemblyTypeName & " Period cannot be removed as Component Period exist", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf mAssemblyStatus.AssemblyStatusPeriods(Index).PeriodID = 1 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.HoursRemove, MSGBox.Message_text.HoursRemove, "Selected " & mAssemblyStatus.AssemblyTypeName & " period can not be removed.Hours Cannot Removed", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf mAssemblyStatus.AssemblyStatusPeriods(Index).PeriodID = 2 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.StartDateRemove, MSGBox.Message_text.StartDateRemove, "Selected " & mAssemblyStatus.AssemblyTypeName & " period can not be removed.Manufacturing Date Cannot Removed", MsgBoxStyle.OkOnly, "")
                    Exit Sub

                Else 'delete
                    mAssemblyStatus.AssemblyStatusPeriods.RemoveAt(Index)
                    ' Response.Redirect("wfAssemblyStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
                    DataBindGrid()
                    SetGridHeader()
                    ControlVisibility()
                    upnlSinceNew.Update()
                End If
        End Select
    End Sub
    Protected Sub txtAssemblyInstallationValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtAssemblyInstallationValue As TextBox
        For j As Integer = 0 To mAssemblyStatus.AssemblyStatusPeriods.Count - 1
            txtAssemblyInstallationValue = CType(Me.dgCurrentMachineValue.Rows(j).FindControl("txtAssemblyInstallationValue"), TextBox)
            If mAssemblyStatus.AssemblyStatusPeriods(j).PeriodID = 2 Then
                If Period.IsDate(txtAssemblyInstallationValue.Text) Then
                    mAssemblyStatus.AssemblyStatusPeriods.Item(j).AssemblyInstallationValueFormatted = Trim(txtAssemblyInstallationValue.Text)
                Else
                    mAssemblyStatus.AssemblyStatusPeriods.Item(j).AssemblyInstallationValueFormatted = ""
                End If
            Else
                mAssemblyStatus.AssemblyStatusPeriods.Item(j).AssemblyInstallationValue = Trim(txtAssemblyInstallationValue.Text)
            End If
        Next j
        DataBindGrid()
        SetGridHeader()
    End Sub
   
    Private Sub imgbtnModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnModel.Click
        SetObject()

        Session("Type") = False
        Session("IsForSpareAssembly") = True

        Session("AssemblyTypeId") = mAssemblyStatus.AssemblyTypeID
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelWindow", "OpenModelWindow();", True)
    End Sub
    Private Sub ImgBtnATAChapter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImgBtnATAChapter.Click
        SetObject()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenATAWindow", "OpenATAWindow();", True)
    End Sub
    Private Sub hdnimgBtnATAChapter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnATAChapter.Click
        mATAList = ATAList.GetATAList(, "(SELECT)")
        cmbATAChapter.DataSource = mATAList
        Session("mATAList") = mATAList
        cmbATAChapter.DataBind()
        upnlATADetails.Update()
    End Sub
    Private Sub hdnBtnModel_Click(sender As Object, e As System.EventArgs) Handles hdnBtnModel.Click
        mModelList = ModelList.GetModelList(mAssemblyStatus.Assembly.Model.AssemblyTypeID, , , , "(SELECT)")
        cmbModel.DataSource = mModelList
        Session("mModelList") = mModelList

        If Not mModelList.Contains(mAssemblyStatus.Assembly.ModelName) Then
            mAssemblyStatus.Assembly.ModelID = Guid.Empty
        End If
        cmbModel.DataBind()
        upnlModelDetails.Update()
    End Sub
    Private Sub cmbModel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbModel.SelectedIndexChanged

        GetAssemblyStatusForModel(cmbModel.SelectedIndex)  'Added by Saylee on 25-Aug-2009
        If cmbModel.SelectedIndex > 0 Then
            Dim mModel As Model = Model.GetModel(New Guid(cmbModel.SelectedValue))
            txtManufacturer.Text = mModel.Manufacturer.Name
        Else
            txtManufacturer.Text = ""
        End If
        If cmbModel.Enabled = True Then
            setFocus(cmbModel)
        End If
        cmbATAChapter.DataBind()
        upnlATADetails.Update()
        upnlSinceNew.Update()

    End Sub
    Private Sub btnAddPeriod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPeriod.Click
        SetObject()
        SetGridObject()
        SetPeriods()
        SetSession()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAddPeriodWindow", "OpenAddPeriodWindow()", True)
    End Sub
    Private Sub hdnAddPeriod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnAddPeriod.Click
        mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
        AddSelectedPeroids()
        DataBindGrid()
        upnlSinceNew.Update()

    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Changed By Utkarsh On 29-Jul-2011 For All19072011

        If mAssemblyStatus.IsNew Then 'Added by Saylee on 8-Aug-2012
            MarkLog(Util.Action.Close, "Assembly Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else
            MachineDetail = " Model : " & mAssemblyStatus.ModelName & " Type : " & mAssemblyStatus.AssemblyTypeName & " Serial No. : " & mAssemblyStatus.Assembly.SerialNo
            MarkLog(Util.Action.Close, "Assembly Status", MachineDetail, Util.ErrorType.NoError, mAssemblyStatus.ID, EventLogID)
        End If

        'End
        RemoveSession()
        mModelList = Nothing
        'Response.Redirect(Request.QueryString("GChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        Session("ActiveTabIndex") = 1
        Response.Redirect("Index.aspx")
    End Sub
  
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        If TbContInst.ActiveTabIndex = 0 Then
            MessageBoxResult()
        ElseIf TbContInst.ActiveTabIndex = 1 Then
            MessageBoxResultComp()
        ElseIf TbContInst.ActiveTabIndex = 2 Then
            MessageBoxResultService()
        ElseIf TbContInst.ActiveTabIndex = 3 Then
            MessageBoxResultInsp()
        ElseIf TbContInst.ActiveTabIndex = 4 Then
            MessageBoxResultMod()
        ElseIf TbContInst.ActiveTabIndex = 5 Then
            MessageBoxResultParameter()
        End If
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mAssemblyStatus.ID
            mMaintenanceDoneByEmployees = mAssemblyStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            Session("MaintenanceDoneOnDate") = mAssemblyStatus.InstalledOn.ToString
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mAssemblyStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mAssemblyStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                'mAssemblyStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mAssemblyStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mAssemblyStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mAssemblyStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mAssemblyStatus.MaintenanceDoneByEmployees(j).ID) Then
                mAssemblyStatus.MaintenanceDoneByEmployees.Remove(mAssemblyStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mAssemblyStatus") = mAssemblyStatus
        BindLicenceNo()
        SetLicenceCount() 'MLNo
        upnlLicenceNo.Update()
    End Sub
    Protected Sub txtLicenceNo_TextChanged(sender As Object, e As System.EventArgs)
        'SetObject()
        If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
            EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            LicenseNo = Trim(txtLicenceNo.Text)
        End If
        DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
        Session("LicenseNo") = LicenseNo
        Session("EmployeeID") = DoneByID
        If Not DoneByID.Equals(Guid.Empty) Then
            If mAssemblyStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mAssemblyStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mAssemblyStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mAssemblyStatus.MaintenanceDoneByEmployees.Add(mAssemblyStatus.ID, 1, DoneByID, LicenseNo, "", EmpName)
            End If

        Else
            If mAssemblyStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mAssemblyStatus") = mAssemblyStatus
        BindLicenceNo()
        SetLicenceCount()
    End Sub
    'End

    Private Sub cmbAssemblyType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAssemblyType.SelectedIndexChanged
        If cmbAssemblyType.SelectedIndex > 0 Then
            cmbModel.SelectedIndex = 0
            mAssemblyStatus.Assembly.ModelID = Guid.Empty

            mModelList = ModelList.GetModelList(cmbAssemblyType.SelectedIndex + 1, , , , "(SELECT)")
            cmbModel.DataSource = mModelList
            cmbModel.DataBind()
            Session("mModelList") = mModelList
            If mAssemblyStatus.IsNew Then
                mAssemblyStatus = AssemblyStatus.NewSpareAssemblyStatus(cmbAssemblyType.SelectedIndex + 1)
                Session("mAssemblyStatus") = mAssemblyStatus
            End If

            SetPage()
            SetGridHeader()
            DataBindGrid()

            upnlActionBtn.Update()
            upnlATADetails.Update()
            upnlDocumentDetails.Update()

            upnlInstallationDetails.Update()
            upnlModelDetails.Update()
            upnlSinceNew.Update()
            upnlTitle.Update()
            upnlTabs.Update()
        End If
    End Sub
#End Region

#Region " Report "
    '    'Created By :- Jyoti
#Region "Report Variable"
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region "Event"
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'Commented By Utkarsh On 14-Mar-2011

        'If (Not User.IsInRole("MachinePrint")) Then
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfAssemblyStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
        '    msg.Show()
        '    Exit Sub
        'End If

        '***********************************
        Rpt = New crDetAssemblyStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Machine Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 4
        RHCount = Me.mAssemblyStatus.AssemblyStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            If mAssemblyStatus.AssemblyTypeID = 1 Then
                ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Manufacturer", _
                           txtManufacturer.Text, , , , , , , , , , , , , , , , , "", _
                          "", , _
                             , ""))
            Else
                ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Manufacturer", _
               txtManufacturer.Text, , , , , , , , , , , , , , , , , "", _
               "", "", _
                 , ""))
            End If
        Else
            ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Manufacturer", _
               txtManufacturer.Text, , , , , , , , , , , , , , , , , "", _
               "", "", , ""))
        End If

        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Model", _
                                     cmbModel.SelectedItem.Text, , , , , , , , , , , , , , , , , "", _
                                        "", _
                                       "", _
                                     , ""))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Model", _
                               cmbModel.SelectedItem.Text, , , , , , , , , , , , , , , , , "", _
                               "", "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Serial No.", _
                                          txtSerialNo.Text, , , , , , , , , , , , , , , , , "", _
                                          "", _
                                          "", _
                                          , ""))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Serial No.", _
                                          txtSerialNo.Text, , , , , , , , , , , , , , , , , "", _
                                          "", "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "", _
                                                          "", , , , , , , , , , , , , , , , , "", _
                                                          "", _
                                                         "", _
                                                          , ""))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "", _
                                                          "", , , , , , , , , , , , , , , , , "", _
                                                          "", "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "", _
                                           "", , , , , , , , , , , , , , , , , "", _
                                          "", _
                                          "", _
                                           , ""))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "", _
                                           "", , , , , , , , , , , , , , , , , "", _
                                           "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "", _
                                                             "", , , , , , , , , , , , , , , , , "", _
                                                             "", _
                                                             "", _
                                                             , ""))
            End If
        Next

        'For Installation Value Grid
        If Me.mAssemblyStatus.AssemblyTypeName <> "Airframe" Then
            ReportDetails.Add(New rptStatus(, 1, "", "ATA", _
                   mAssemblyStatus.ATAChapter, , , , , , , , , , , , , , , , , "Values at Building", _
                        dgCurrentMachineValue.Columns.Item(1).HeaderText, _
                      , dgCurrentMachineValue.Columns.Item(2).HeaderText, ""))

            Dim TotalCount1 As Integer
            TotalCount1 = Me.mAssemblyStatus.AssemblyStatusPeriods.Count
            Dim m As Integer

            For m = 0 To TotalCount1 - 1
                If m = 0 Then
                    ReportDetails.Add(New rptStatus(, 1, "", "Remark", _
                         txtInstallationReason.Text, , , , , , , , , , , , , , , , , "Values at Building", _
                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).PeriodName, String), _
                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).AssemblyInstallationValueFormatted, String), _
                           , ""))
                ElseIf m = 1 Then
                    ReportDetails.Add(New rptStatus(, 1, lblInstallationInfo.InnerText, "Note", _
                                          txtNote.Text, , , , , , , , , , , , , , , , , "Values at Building", _
                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).PeriodName, String), _
                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).AssemblyInstallationValueFormatted, String), _
                                          , ""))
                Else
                    ReportDetails.Add(New rptStatus(, 1, lblInstallationInfo.InnerText, "", _
                                           "", , , , , , , , , , , , , , , , , "Values at Installation", _
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).PeriodName, String), _
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(m).AssemblyInstallationValueFormatted, String), _
                                           , ""))
                End If
            Next
        Else
            ReportDetails.Add(New rptStatus(, 1, lblInstallationInfo.InnerText, "", _
                                     "", , , , , , , , , , , , , , , , , "Values at Building", _
                                     "", "", , ""))
            ReportDetails.Add(New rptStatus(, 1, lblInstallationInfo.InnerText, "Work Order No.", _
                          "", , , , , , , , , , , , , , , , , "Values at Building", _
                          "", "", , ""))
            ReportDetails.Add(New rptStatus(, 1, lblInstallationInfo.InnerText, "Note", _
                            txtNote.Text, , , , , , , , , , , , , , , , , "Values at Building", _
                                                      "", "", , ""))
        End If

        '***********************************************************************************************************************
        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 3
        RHCount2 = Me.mAssemblyStatus.AssemblyStatusPeriods.Count
        If LHCount2 > RHCount2 Then
            TotalCount2 = LHCount2
        Else
            TotalCount2 = RHCount2
        End If

        Dim temp2 As Integer
        temp2 = 0
        'If temp2 < RHCount2 Then
        '    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.", _
        '    txtRevisionNo.Text, , , , , , , , , , , , , , , , , , _
        '   dgInstallationValue.Columns.Item(0).HeaderText, dgInstallationValue.Columns.Item(1).HeaderText, , _
        '   dgInstallationValue.Columns.Item(2).HeaderText, , , , ))
        'Else
        ' ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.", _
        'txtRevisionNo.Text, , , , , , , , , , , , , , , , , , _
        '      "", "", , "", , "", ""))
        ' End If
      

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Assembly Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mRptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mRptImage)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region
#End Region

#Region " Component List "

#Region " Variable Declaration "
    Public mtmpCompStatusList As tmpCompStatusList
    Public mCompStatus As CompStatus
#End Region

#Region " Business Methods "
    Private Sub GetSessionComp()

        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mtmpCompStatusList = CType(Session("mtmpCompStatusList"), tmpCompStatusList)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
    End Sub
    Private Sub SetSessionComp()

        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mtmpCompStatusList") = mtmpCompStatusList
        Session("mCompStatus") = mCompStatus
    End Sub
    Private Sub RemoveSessionComp()
        Session.Remove("mtmpCompStatusList")
        Session.Remove("LookInComp")
        Session.Remove("txtForComp")
        Session.Remove("txtCodeComp")
        Session.Remove("SearchForComp")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    'Added By Vikrant On 26-Jun-2014
    Private Sub RemoveAllSessionValuesComp()
        Session.Remove("mModelList")
        Session.Remove("mATAList")
        Session.Remove("Add")
        Session.Remove("Edit")
        Session.Remove("mAssemblyStatusList")
        Session.Remove("mAssemblyStatus")
        Session.Remove("mAssemblyTypeListForUI")
        Session.Remove("mCompStatus")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    'End
    Private Sub SetPageComp()
        If mAssemblyStatus.IsNew Then
            lblTitle.Text = "Build Assembly  [New]"
        Else
            lblTitle.Text = mAssemblyStatus.AssemblyTypeName & " [Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]"
        End If
        'CNDC
        'lblComponentText.Text = "List of all the Components on the " & mMachine.RegNo & " as of Date: " & CDate(mAssemblyStatus.AsOnDate).ToShortDateString & ". The Time Since New values of all the Components will be as of Date: " & SmartDate.StringToDate((mAssemblyStatus.AsOnDate).ToString).ToShortDateString & "."
        lblComponentText.Text = "List of all the Components as of Date: " & mAssemblyStatus.AsOnDateFormatted & ". The Time Since New values of all the Components will be as of Date: " & mAssemblyStatus.AsOnDateFormatted & "."
        If Not IsNothing(mtmpCompStatusList) Then
            lblResultComponentList.Text = "List of Component: " & mtmpCompStatusList.Count & " Record(s)."
        End If
    End Sub
    Private Sub NewRecordComp()
        mCompStatus = CompStatus.NewCompStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, CDate(mAssemblyStatus.AsOnDate).ToShortDateString, mAssemblyStatus.HourType)
        Session("mCompStatus") = mCompStatus
        MarkLog(Util.Action.[New], "Assembly Component Status", "", Util.ErrorType.NoError, mCompStatus.ID, EventLogID) 'Changed By Utkarsh On 1-Aug-2011 For All19072011
        Session.Remove("mtmpCompStatusList")
        'RemoveSession()
        Session("IsOpenedFromAssembly") = "True"
        Response.Redirect("wfCompStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfSpareAssemblyStatus.aspx")
    End Sub
    Private Sub EditRecordComp(ByVal mId As Guid)
        mCompStatus = CompStatus.GetCompStatus(mId, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate.ToString)
        mCompStatus.BeginEdit()
        Session("mCompStatus") = mCompStatus
        Session.Remove("mtmpCompStatusList")
        'RemoveSession()
        Session("Edit") = True
        'Added By Utkarsh On 1-Aug-2011 For All19072011
        MachineDetail = "Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.PartName + " " + mCompStatus.Description + " " + mCompStatus.SerialNo
        MarkLog(Util.Action.Edit, "Assembly Component Status", MachineDetail, Util.ErrorType.NoError, mCompStatus.ID, EventLogID)
        'End
        Session("IsOpenFromMaster") = True 'Added By Vikrant On 26-Jun-2014
        Session("IsOpenedFromAssembly") = "True"

        Response.Redirect("wfCompStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfSpareAssemblyStatus.aspx")
    End Sub
    Private Sub DeleteRecordComp(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mtmpCompStatusList.CurrentIndex = Index
        Session("mtmpCompStatusList") = mtmpCompStatusList
    End Sub
    Private Sub GridBindComp()
        dgCompStatusList.DataSource = mtmpCompStatusList
        dgCompStatusList.DataBind()
    End Sub
    Private Sub MessageBoxResultComp()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Dim CompStatusID As Guid
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mtmpCompStatusList = CType(Session("mtmpCompStatusList"), tmpCompStatusList)
                            CompStatusID = mtmpCompStatusList.Item(mtmpCompStatusList.CurrentIndex).ID
                            MachineDetail = "Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mtmpCompStatusList(mtmpCompStatusList.CurrentIndex).PartName + " " + mtmpCompStatusList(mtmpCompStatusList.CurrentIndex).PartDescription + " " + mtmpCompStatusList(mtmpCompStatusList.CurrentIndex).SerialNo
                            CompStatus.DeleteCompStatus(mtmpCompStatusList.CurrentItem.ID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate.ToString)
                            DataFieldBindComp()
                            SetPageComp()
                            ControlVisibility()
                            upnlGridComponentList.Update()
                            upnlActionBtn.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                'Changed By Utkarsh On 1-Aug-2011 For All19072011
                                MarkLog(Util.Action.Delete, "Assembly Component Status", "Can't delete : " & MachineDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                                'End
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed By Utkarsh On 1-Aug-2011 For All19072011
                                MarkLog(Util.Action.Delete, "Assembly Component Status", MachineDetail, Util.ErrorType.NoError, CompStatusID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub DisplayControlsComp(ByVal Index As Integer)
        txtForComponentList.Text = IIf(Index = 2 Or Index = 3 Or Index = 4, txtForComponentList.Text, "")
        txtCodeComponentList.Text = IIf(Index = 1, txtCodeComponentList.Text, "")
        txtCodeComponentList.Visible = IIf(Index = 1, True, False)
        txtForComponentList.Visible = IIf(Index = 2 Or Index = 3 Or Index = 4 Or Index = 5, True, False)
        lblForComponentList.Visible = (Index > 0 And Index < 5)

    End Sub
    Private Sub addAttributesComp()
        txtCodeComponentList.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCodeComponentList').value,event)")
    End Sub
    Private Sub ControlVisibilityComp()
        If Not IsNothing(mtmpCompStatusList) Then
            btnPrint.Enabled = mtmpCompStatusList.Count > 0
            btnPrintTopComponentList.Enabled = mtmpCompStatusList.Count > 0
            btnPrintComponentList.Enabled = mtmpCompStatusList.Count > 0
        End If
    End Sub
    Private Sub SetControlsComp()
        'Function added By Saylee on 28-th-Jan-2008 for bug-Service List (SL3)
        txtForComponentList.Text = Session("txtForComp")
        txtCodeComponentList.Text = Session("txtCodeComp")
        cmbLookInComponentList.SelectedIndex = Session("LookInComp")
        DisplayControlsComp(cmbLookInComponentList.SelectedIndex)
        'FindNow()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub FindNowComp()
        'Binding CompStatus Grid
        Select Case cmbLookInComponentList.SelectedIndex
            Case 0
                REM ALL
                mtmpCompStatusList = tmpCompStatusList.GetCompStatusList(mAssemblyStatus.AssemblyID, "", "", , , , IIf(mAssemblyStatus.IsSpareAssembly = False, mAssemblyStatus.MachineID.ToString, Guid.Empty.ToString), mAssemblyStatus.ID.ToString, mAssemblyStatus.IsSpareAssembly)
            Case 1
                REM ATACode
                If txtCodeComponentList.Text = "" Then
                    mtmpCompStatusList = tmpCompStatusList.GetCompStatusList(mAssemblyStatus.AssemblyID, "", "", , 0, , IIf(mAssemblyStatus.IsSpareAssembly = False, mAssemblyStatus.MachineID.ToString, Guid.Empty.ToString), mAssemblyStatus.ID.ToString, mAssemblyStatus.IsSpareAssembly)
                Else
                    mtmpCompStatusList = tmpCompStatusList.GetCompStatusList(mAssemblyStatus.AssemblyID, "", "", , Val(txtCodeComponentList.Text), , IIf(mAssemblyStatus.IsSpareAssembly = False, mAssemblyStatus.MachineID.ToString, Guid.Empty.ToString), mAssemblyStatus.ID.ToString, mAssemblyStatus.IsSpareAssembly)
                End If
            Case 2
                REM Part Name
                mtmpCompStatusList = tmpCompStatusList.GetCompStatusList(mAssemblyStatus.AssemblyID, Trim(txtForComponentList.Text), "", , , , IIf(mAssemblyStatus.IsSpareAssembly = False, mAssemblyStatus.MachineID.ToString, Guid.Empty.ToString), mAssemblyStatus.ID.ToString, mAssemblyStatus.IsSpareAssembly)
            Case 3
                REM Part Desription
                mtmpCompStatusList = tmpCompStatusList.GetCompStatusList(mAssemblyStatus.AssemblyID, "", "", Trim(txtForComponentList.Text), , , IIf(mAssemblyStatus.IsSpareAssembly = False, mAssemblyStatus.MachineID.ToString, Guid.Empty.ToString), mAssemblyStatus.ID.ToString, mAssemblyStatus.IsSpareAssembly)
            Case 4
                REM Part Serial No
                mtmpCompStatusList = tmpCompStatusList.GetCompStatusList(mAssemblyStatus.AssemblyID, "", Trim(txtForComponentList.Text), , , , IIf(mAssemblyStatus.IsSpareAssembly = False, mAssemblyStatus.MachineID.ToString, Guid.Empty.ToString), mAssemblyStatus.ID.ToString, mAssemblyStatus.IsSpareAssembly)
            Case Else
                REM ALL
                mtmpCompStatusList = tmpCompStatusList.GetCompStatusList(mAssemblyStatus.AssemblyID, "", "", , , , IIf(mAssemblyStatus.IsSpareAssembly = False, mAssemblyStatus.MachineID.ToString, Guid.Empty.ToString), mAssemblyStatus.ID.ToString, mAssemblyStatus.IsSpareAssembly)
        End Select
        Session("mtmpCompStatusList") = mtmpCompStatusList
        dgCompStatusList.DataSource = mtmpCompStatusList
        dgCompStatusList.DataBind()
        lblResultComponentList.Text = "List of Component: " & mtmpCompStatusList.Count & " Record(s)."
        'SearchForComp = IIf(cmbSearchFor.SelectedIndex <= 0, "", cmbSearchFor.SelectedValue)
        Session("LookInComp") = cmbLookInComponentList.SelectedIndex
        Session("txtForComp") = txtForComponentList.Text
        Session("txtCodeComp") = txtCodeComponentList.Text
    End Sub
    Private Sub DataFieldBindComp()
        mtmpCompStatusList = tmpCompStatusList.GetCompStatusList(mAssemblyStatus.AssemblyID, "", "", , , , IIf(mAssemblyStatus.IsSpareAssembly = False, mAssemblyStatus.MachineID.ToString, Guid.Empty.ToString), mAssemblyStatus.ID.ToString, mAssemblyStatus.IsSpareAssembly)
        dgCompStatusList.DataSource = mtmpCompStatusList
        Session("mtmpCompStatusList") = mtmpCompStatusList
        DataBind()
    End Sub
    Private Sub SetRightsComp()
        If mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineComponentPrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
                btnPrintTopComponentList.Enabled = False
                btnPrintTopComponentList.ToolTip = "You are not Authorized user"
                btnPrintComponentList.Enabled = False
                btnPrintComponentList.ToolTip = "You are not Authorized user"
            End If
            If (User.IsInRole("MachineComponentNew")) = False Then
                btnAddComponentList.Enabled = False
                btnAddComponentList.ToolTip = "You are not authorized user"
                btnAddTopComponentList.Enabled = False
                btnAddTopComponentList.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineComponentPrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
                btnPrintTopComponentList.Enabled = False
                btnPrintTopComponentList.ToolTip = "You are not Authorized user"
                btnPrintComponentList.Enabled = False
                btnPrintComponentList.ToolTip = "You are not Authorized user"
            End If
            If (User.IsInRole("MachineComponentNew")) = False Then
                btnAddComponentList.Enabled = False
                btnAddComponentList.ToolTip = "You are not authorized user"
                btnAddTopComponentList.Enabled = False
                btnAddTopComponentList.ToolTip = "You are not authorized user"
            End If
        End If
    End Sub
#End Region

#Region " Event "
    Private Sub dgCompStatusList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCompStatusList.RowCommand
        Dim Index As Int32
        Dim mId As Guid
        Select Case e.CommandName
            Case "EditRec"
                GridBindComp()
                Index = CInt(e.CommandArgument)
                mId = mtmpCompStatusList(Index).ID 'New Guid(dgCompStatusList.DataKeys(Index).Value.ToString)
                'Added By Prashant 14-Mar-2011
                If (User.IsInRole("MachineComponentView") Or User.IsInRole("MachineComponentEdit")) = False Then
                    'Changed By Utkarsh On 1-Aug-2011 For All19072011
                    MachineDetail = "Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mtmpCompStatusList(mId).PartName + " " + mtmpCompStatusList(mId).PartDescription + " " + mtmpCompStatusList(mId).SerialNo
                    MarkLog(Util.Action.Edit, "Assembly Component Status", User.Identity.Name & " is not Authorized User to edit " & MachineDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'End
                EditRecordComp(mId)
            Case "DeleteRec"
                GridBindComp()
                Index = CInt(e.CommandArgument)
                mId = mtmpCompStatusList(Index).ID 'New Guid(dgCompStatusList.DataKeys(Index).Value.ToString)
                'Added By Prashant 14-Mar-2011
                If User.IsInRole("MachineComponentDelete") = False Then
                    'Added By Utkarsh On 1-Aug-2011 For All19072011
                    MachineDetail = "Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mtmpCompStatusList(mId).PartName + " " + mtmpCompStatusList(mId).PartDescription + " " + mtmpCompStatusList(mId).SerialNo
                    MarkLog(Util.Action.Delete, "Assembly Component Status", User.Identity.Name & " is not Authorized User to delete " & MachineDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecordComp(Index)
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNowComponentList.Click
        dgCompStatusList.PageIndex = 0
        FindNowComp()
        ControlVisibilityComp()
        SetRightsComp() 'Added By Utkarsh On 21-Mar-2011
        upnlGridComponentList.Update()
        upnlActionBtn.Update()
        upnlActionBtnComponentList.Update()
    End Sub
    Private Sub cmbLookInComponentList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookInComponentList.SelectedIndexChanged
        DisplayControlsComp(cmbLookInComponentList.SelectedIndex)
        If cmbLookInComponentList.Enabled = True Then
            cmbLookInComponentList.Focus()
        End If
    End Sub
    Private Sub btnAddComponentList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddComponentList.Click, btnAddTopComponentList.Click
        Session("IsOpenFromMaster") = True 'Added By Vikrant On 26-Jun-2014
        NewRecordComp()
    End Sub
    Private Sub btnBackComp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseComp.Click, btnCloseTopComp.Click
        'Changed By Utkarsh On 1-Aug-2011 For All19072011
        MarkLog(Util.Action.Close, "Assembly Component Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        RemoveSessionComp()
        ' Response.Redirect(Request.QueryString("GChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
        TbContInst.ActiveTabIndex = 0
        TbContInst_ActiveTabChanged(Nothing, Nothing)
        upnlTabs.Update()
    End Sub
    Private Sub dgCompStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCompStatusList.Sorting
        mtmpCompStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mtmpCompStatusList") = mtmpCompStatusList
        dgCompStatusList.DataSource = mtmpCompStatusList
        dgCompStatusList.DataBind()
    End Sub
   
#End Region

#Region " Report "
    '    'Created By:- Jyoti
#Region " Event "
    Private Sub btnPrintComponentList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintComponentList.Click, btnPrintTopComponentList.Click
        If (Not User.IsInRole("MachinePrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        GridBindComp()
        Dim Rpt As New crListComponent
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 4
        RHCount = Me.mAssemblyStatus.AssemblyStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            If mAssemblyStatus.AssemblyTypeID = 1 Then
                ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Manufacturer", _
                       Me.mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , "Since New Values as on " & mAssemblyStatus.AsOnDateFormatted, _
                           "Periods", , _
                             , "Airframe"))
            Else
                ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Manufacturer", _
                   Me.mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , "Since New Values as on " & mAssemblyStatus.AsOnDateFormatted, _
                   "Periods", "Engine", _
                     , "Airframe"))
            End If
        End If

        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Model", _
                                         Me.mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , , _
                                            CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                            CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                         , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Model", _
                               Me.mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , , _
                               "", "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Serial No.", _
                                          "", , , , , , , , , , , , , , , , , , _
                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                          , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Serial No.", _
                                          "", , , , , , , , , , , , , , , , , , _
                                          "", "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Position", _
                                                         Me.mAssemblyStatus.Position, , , , , , , , , , , , , , , , , , _
                                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                                          , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Position", _
                                                          Me.mAssemblyStatus.Position, , , , , , , , , , , , , , , , , , _
                                                          "", "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "", _
                                           "", , , , , , , , , , , , , , , , , , _
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "", _
                                           "", , , , , , , , , , , , , , , , , , _
                                           "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, , "", _
                                                    "", , , , , , , , , , , , , , , , , , _
                                                    CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                                    CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                                 , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
            End If
        Next

        'For Component List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , lblComponentText.Text))

        'For Component List
        'ReportDetails.Add(New rptStatus(, 2, _
        '     , , , , dgCompStatusList.Columns.Item(1).HeaderText, , dgCompStatusList.Columns.Item(2).HeaderText, dgCompStatusList.Columns.Item(3).HeaderText, _
        '      dgCompStatusList.Columns.Item(4).HeaderText, dgCompStatusList.Columns.Item(5).HeaderText, dgCompStatusList.Columns.Item(6).HeaderText, dgCompStatusList.Columns.Item(7).HeaderText))
        ReportDetails.Add(New rptStatus(, 3, _
                        , , , , dgCompStatusList.Columns.Item(1).HeaderText, , dgCompStatusList.Columns.Item(2).HeaderText, dgCompStatusList.Columns.Item(3).HeaderText, dgCompStatusList.Columns.Item(4).HeaderText, _
                         dgCompStatusList.Columns.Item(5).HeaderText, dgCompStatusList.Columns.Item(6).HeaderText, dgCompStatusList.Columns.Item(7).HeaderText, , , , , , , _
                         , , , , , , , dgCompStatusList.Columns.Item(8).HeaderText))

        Dim TotalCount1 As Integer
        TotalCount1 = Me.mtmpCompStatusList.Count
        Dim m As Integer

        Dim str(7) As String


        For m = 0 To TotalCount1 - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""
            If Me.dgCompStatusList.Rows(m).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgCompStatusList.Rows(m).Cells(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(2).Text <> "&nbsp;" Then str(1) = Me.dgCompStatusList.Rows(m).Cells(2).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(2) = Me.dgCompStatusList.Rows(m).Cells(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(3) = Me.dgCompStatusList.Rows(m).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(4) = Me.dgCompStatusList.Rows(m).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(5) = Me.dgCompStatusList.Rows(m).Cells(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(6) = Me.dgCompStatusList.Rows(m).Cells(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgCompStatusList.Rows(m).Cells(8).Text <> "&nbsp;" Then str(7) = Me.dgCompStatusList.Rows(m).Cells(8).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 3, , _
                   , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6), , , , , , , , str(7)))
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Component List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If Me.mtmpCompStatusList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Commented By Utkarsh On 1-Aug-2011 For All19072011
        '    MarkLog(Util.Action.Print, "AssemblyMonitorCompStatus", "Assembly Monitor Component Status List Report", Util.ErrorType.NoError, Guid.Empty)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region
#End Region

#Region " Service List "

#Region " Variable Declaration "
    Public mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
    Public mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList
    Public mModelMonitorServiceTypeList As ModelMonitorServiceTypeList
    Public SearchForService As String

    'Added by Saylee on 13th-Oct-2009
    Public mMachineMaintenance As MachineMaintenance

    Dim mModelMaintenanceActivityListCount As ModelMaintenanceActivityListCount
#End Region

#Region " Business Methods "
    Private Sub GetSessionService()

        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mAssemblyMonitorServiceStatusList = CType(Session("mAssemblyMonitorServiceStatusList"), tmpAssemblyMonitorServiceStatusList)
        mModelMonitorServiceTypeList = CType(Session("mModelMonitorServiceTypeList"), ModelMonitorServiceTypeList)
        'SearchFor = Session("SearchFor")
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
    End Sub
    Private Sub RemoveSessionService()
        Session.Remove("mModelMonitorServiceTypeList")
        Session.Remove("mAssemblyMonitorServiceStatusList")
        Session.Remove("LookInService")
        Session.Remove("txtForService")
        Session.Remove("txtCodeService")
        Session.Remove("SearchForService")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    'Added By Vikrant On 25-Jun-2014
    Private Sub RemoveAllSessionValuesService()
        Session.Remove("mModelList")
        Session.Remove("mATAList")
        Session.Remove("Add")
        Session.Remove("Edit")
        Session.Remove("mAssemblyStatusList")
        Session.Remove("mAssemblyStatus")
        Session.Remove("mAssemblyTypeListForUI")
    End Sub
    'End
    'MLNo
    Public Sub SetLicenceCount()
        If mAssemblyStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mAssemblyStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mAssemblyStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mAssemblyStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
    Private Sub NewRecordService()
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate, mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.HourType)
        mAssemblyMonitorServiceStatus.BeginEdit()
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList

        MarkLog(Util.Action.[New], "Assembly Service Status", "", Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)

        'Code  Added By Saylee  on 1/4/2008 suggested by Deven sir
        Session("EditMasterRecord") = "False"
        Session("IsOpenFromMaster") = True

        'Response.Redirect("wfModelMonitorServiceList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfAssemblyMonitorServiceStatusList_Ajax.aspx" & "&GChildPage3=wfAssemblyMonitorServiceStatusList_Ajax.aspx")
        mModelMaintenanceActivityListCount = ModelMaintenanceActivityListCount.GetModelMaintenanceActivityListCount(mAssemblyStatus.Assembly.ModelID)
        If mModelMaintenanceActivityListCount.ModelServiceListCount > 0 Then
            Response.Redirect("wfModelMonitorServiceList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfSpareAssemblyStatus.aspx" & "&GChildPage3=wfSpareAssemblyStatus.aspx")
        Else
            Dim ID As Guid = Guid.NewGuid
            Dim mModelMonitorService As ModelMonitorService
            mModelMonitorService = ModelMonitorService.NewModelMonitorService(ID, mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.HourType, PreviousRefID:=ID)
            Session("mModelMonitorService") = mModelMonitorService
            mModelMonitorService.BeginEdit()
            MarkLog(Util.Action.[New], "Model Monitor Service", " Model : " & mAssemblyStatus.Assembly.ModelName, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'Response.Redirect("wfModelMonitorService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfModelMonitorServiceList_Ajax.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelServiceMasterWindow", "OpenModelServiceMasterWindow()", True)
        End If
        '---------------------
    End Sub
    Private Sub EditRecordService(ByVal mId As Guid)
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mId, mAssemblyStatus.ID, mAssemblyStatus.HourType)
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("IsOpenFromMaster") = True

        MachineDetail = "Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- " & "Description : " & mAssemblyMonitorServiceStatusList(mId).Description & " Monitor Type : " & mAssemblyMonitorServiceStatusList(mId).MonitorType
        'MarkLog(Util.Action.Edit, "Assembly Service Status",  & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Service Type : " + mAssemblyMonitorServiceStatusList.Item(mAssemblyMonitorServiceStatusList.CurrentIndex).MonitorType + " Description : " + mAssemblyMonitorServiceStatusList.Item(mAssemblyMonitorServiceStatusList.CurrentIndex).Description, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
        MarkLog(Util.Action.Edit, "Assembly Service Status", MachineDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
        Session("Edit") = True
        Response.Redirect("wfAssemblyMonitorServiceStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfSpareAssemblyStatus.aspx")
    End Sub

    'code added by Saylee on 1/04/2008 Suggested by Deven sir
    Private Sub EditMasterRecordService(ByVal mMasterId As Guid, ByVal mId As Guid)
        Dim mModelMonitorService As ModelMonitorService
        mModelMonitorService = ModelMonitorService.GetModelMonitorService(mMasterId, mAssemblyStatus.HourType)
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mId, mAssemblyStatus.ID, mAssemblyStatus.HourType)
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus

        Session("mModelMonitorService") = mModelMonitorService
        Session("IsOpenFromMaster") = True

        'RemoveSession()
        'Response.Redirect("wfModelMonitorService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfAssemblyMonitorServiceStatusList_Ajax.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelServiceMasterWindow", "OpenModelServiceMasterWindow()", True)

    End Sub
    '------------------------------------------

    Private Sub DeleteRecordService(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mAssemblyMonitorServiceStatusList.CurrentIndex = Index
        Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
    End Sub
    Private Sub MessageBoxResultService()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Dim ServiceIDForEventLog As Guid
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            MachineDetail = "Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- " & "Description : " & mAssemblyMonitorServiceStatusList.CurrentItem.Description & " Monitor Type : " & mAssemblyMonitorServiceStatusList.CurrentItem.MonitorType
                            mAssemblyMonitorServiceStatusList = CType(Session("mAssemblyMonitorServiceStatusList"), tmpAssemblyMonitorServiceStatusList)
                            ServiceIDForEventLog = mAssemblyMonitorServiceStatusList.Item(mAssemblyMonitorServiceStatusList.CurrentIndex).ID
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorServiceStatusList.CurrentItem.ID, 5) 'Added by Saylee on 13th-Oct-2009
                            AssemblyMonitorServiceStatus.DeleteAssemblyMonitorServiceStatus(mAssemblyMonitorServiceStatusList.CurrentItem.ID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            Session("mMachineMaintenance") = mMachineMaintenance
                            Session("mAircraftInformationBoardList") = Nothing 'Added by Saylee on 16-July-2009
                            SetControlsService()
                            FindNowService()
                            SetPageService()
                            ControlVisibilityService()
                            SetRightsService()
                            upnlGridService.Update()
                            upnlActionBtnService.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Assembly Service Status", "Can't delete : " & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Service Type : " + mAssemblyMonitorServiceStatusList.Item(mAssemblyMonitorServiceStatusList.CurrentIndex).MonitorType + " Description : " + mAssemblyMonitorServiceStatusList.Item(mAssemblyMonitorServiceStatusList.CurrentIndex).Description + " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "Assembly Service Status",   " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Monitor Service Type : " + mAssemblyMonitorServiceStatusList.Item(mAssemblyMonitorServiceStatusList.CurrentIndex).MonitorType + " Description : " + mAssemblyMonitorServiceStatusList.Item(mAssemblyMonitorServiceStatusList.CurrentIndex).Description, Util.ErrorType.NoError, mAssemblyMonitorServiceStatusList.Item(mAssemblyMonitorServiceStatusList.CurrentIndex).ID, EventLogID)
                                MarkLog(Util.Action.Delete, "Assembly Service Status", MachineDetail, Util.ErrorType.NoError, ServiceIDForEventLog, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
    Private Sub SetPageService()
        If mAssemblyStatus.IsNew Then
            lblTitle.Text = "Build Assembly  [New]"
        Else
            lblTitle.Text = mAssemblyStatus.AssemblyTypeName & " [Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]"
        End If
        'CNDC
        'lblServiceText.Text = "List of all the Servicings on the " & mMachine.RegNo & " as of Date: " & SmartDate.StringToDate((mAssemblyStatus.AsOnDate).ToString).ToShortDateString & ". All the values of all the Services will be as of Date: " & SmartDate.StringToDate((mAssemblyStatus.AsOnDate).ToString).ToShortDateString & "."
        lblServiceText.Text = "List of all the Services as of Date: " & mAssemblyStatus.AsOnDateFormatted & ". All the values of all the Services will be as of Date: " & mAssemblyStatus.AsOnDateFormatted & "."
        lblResultService.Text = "List of Services: " & mAssemblyMonitorServiceStatusList.Count & " Record(s)."
    End Sub
    Private Sub addAttributesService()
        txtCodeService.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCodeService').value,event)")
    End Sub
    Private Sub DisplayControlsService(ByVal Index As Integer)
        '---------------Commented and added By Saylee on 28th-Jan-2008-------------
        'txtFor.Text = ""
        'txtCode.Text = ""
        txtForService.Text = IIf(Index = 2 Or Index = 4, txtForService.Text, "")
        txtCodeService.Text = IIf(Index = 1, txtCodeService.Text, "")
        '--------------------------------------------------------------------------
        txtCodeService.Visible = IIf(Index = 1, True, False)
        txtForService.Visible = IIf(Index = 2 Or Index = 4, True, False)
        lblForService.Visible = (Index > 0 And Index <> 5)
        cmbSearchForService.Visible = (Index = 3)
    End Sub
    Private Sub ControlVisibilityService()
        btnPrintService.Enabled = mAssemblyMonitorServiceStatusList.Count > 0
        btnPrintTopService.Enabled = mAssemblyMonitorServiceStatusList.Count > 0

        btnAddTopService.Visible = mAssemblyMonitorServiceStatusList.Count > 10
        btnPrintTopService.Visible = mAssemblyMonitorServiceStatusList.Count > 10
        btnCloseTopService.Visible = mAssemblyMonitorServiceStatusList.Count > 10
    End Sub
    Private Sub GridBindService()
        dgMonitorServiceStatusList.DataSource = mAssemblyMonitorServiceStatusList
        dgMonitorServiceStatusList.DataBind()
    End Sub
    Private Sub SetControlsService()
        '======Function added By Saylee on 28-th-Jan-2008 for bug-Service List (SL3)
        txtForService.Text = Session("txtForService")
        txtCodeService.Text = Session("txtCodeService")
        cmbLookInService.SelectedIndex = Session("LookInService")
        '==========================================================================
        cmbSearchForService.SelectedValue = IIf(SearchForService = "", 0, SearchForService)
        DisplayControlsService(cmbLookInService.SelectedIndex)
        'FindNow()
    End Sub
    'Added By Utkarsh On 14-Mar-2011
    Private Sub SetRightsService()
        If mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineAssemblyServicePrint")) Then
                btnPrintService.Enabled = False
                btnPrintService.ToolTip = "You are not authorized user"
                btnPrintTopService.Enabled = False
                btnPrintTopService.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineAssemblyServiceNew")) = False Then
                btnAddService.Enabled = False
                btnAddService.ToolTip = "You are not authorized user"
                btnAddTopService.Enabled = False
                btnAddTopService.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineAssemblyServicePrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
                btnPrintTopService.Enabled = False
                btnPrintTopService.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineAssemblyServiceNew")) = False Then
                btnAddService.Enabled = False
                btnAddService.ToolTip = "You are not authorized user"
                btnAddTopService.Enabled = False
                btnAddTopService.ToolTip = "You are not authorized user"
            End If
        End If
    End Sub
    '*******************************
#End Region

#Region " Data Binding "
    Private Sub FindNowService()
        dgMonitorServiceStatusList.PageIndex = 0
        Select Case cmbLookInService.SelectedIndex
            Case 0, -1  'All
                mAssemblyMonitorServiceStatusList = tmpAssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 1  'ATA Code
                mAssemblyMonitorServiceStatusList = tmpAssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, Val(txtCodeService.Text), , , , , , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 2  'Description
                mAssemblyMonitorServiceStatusList = tmpAssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , txtForService.Text.Trim, , , , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 3  'Service Type ID
                mAssemblyMonitorServiceStatusList = tmpAssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , CInt(cmbSearchForService.SelectedValue), , , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 4 ' Work Order No.
                mAssemblyMonitorServiceStatusList = tmpAssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , txtForService.Text.Trim, , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 5  'Show In C of A
                mAssemblyMonitorServiceStatusList = tmpAssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , True, mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
        End Select
        Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
        dgMonitorServiceStatusList.DataSource = mAssemblyMonitorServiceStatusList
        dgMonitorServiceStatusList.DataBind()

        'Added By Saylee on 28th-Jan-2008===============
        Session("LookInService") = cmbLookInService.SelectedIndex
        Session("txtForService") = txtForService.Text
        Session("txtCodeService") = txtCodeService.Text
        SearchForService = IIf(cmbSearchForService.SelectedIndex <= 0, "", cmbSearchForService.SelectedValue) 'cmbSearchFor.SelectedIndex
        Session("SearchForService") = SearchForService
        '==================================================
    End Sub
    Private Sub DataFieldBindService()
        'mAssemblyMonitorServiceStatusList = tmpAssemblyMonitorServiceStatusList.GetAssemblyMonitorServiceStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , , mAssemblyStatus.ID.ToString)
        mModelMonitorServiceTypeList = ModelMonitorServiceTypeList.GetModelMonitorServiceTypeList("(All)")
        cmbSearchForService.DataSource = mModelMonitorServiceTypeList
        Session("mModelMonitorServiceTypeList") = mModelMonitorServiceTypeList
        'dgMonitorServiceStatusList.DataSource = mAssemblyMonitorServiceStatusList
        'Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
        DataBind()
        SearchForService = Session("SearchForService")
    End Sub
#End Region

#Region " Event "
    Private Sub dgMonitorServiceStatusList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorServiceStatusList.RowCommand
        Dim Index As Int32
        Dim mId As Guid
        Dim mMasterId As Guid
        Select Case e.CommandName
            Case "EditRec"
                GridBindService()
                Index = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageSize * dgMonitorServiceStatusList.PageIndex
                mId = mAssemblyMonitorServiceStatusList(Index).ID
                If (User.IsInRole("MachineAssemblyServiceView") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                    MarkLog(Util.Action.Edit, "Assembly Service Status", User.Identity.Name & " is not Authorized User to edit " & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Service Type : " + mAssemblyMonitorServiceStatusList.Item(mAssemblyMonitorServiceStatusList.CurrentIndex).MonitorType + " Description : " + mAssemblyMonitorServiceStatusList.Item(mAssemblyMonitorServiceStatusList.CurrentIndex).Description, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                EditRecordService(mId)
                'Added by Saylee on 1/04/2008 Suggested By Deven Sir
            Case "EditMaster"
                GridBindService()
                Index = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageSize * dgMonitorServiceStatusList.PageIndex
                mId = mAssemblyMonitorServiceStatusList(Index).ID
                mMasterId = mAssemblyMonitorServiceStatusList(Index).ModelMonitorServiceID 'New Guid(dgMonitorServiceStatusList.DataKeys(Index).Values("ModelMonitorServiceID").ToString)
                'Added By Utkarsh On 14-Mar-2011
                If (User.IsInRole("MachineAssemblyServiceView") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("EditMasterRecord") = "True"
                EditMasterRecordService(mMasterId, mId)
            Case "DeleteRec"
                GridBindService()
                Index = CInt(e.CommandArgument) + dgMonitorServiceStatusList.PageSize * dgMonitorServiceStatusList.PageIndex
                mId = mAssemblyMonitorServiceStatusList(Index).ID
                'Added By Utkarsh On 14-Mar-2011
                If (User.IsInRole("MachineAssemblyServiceDelete")) = False Then
                    MarkLog(Util.Action.Delete, "Assembly Service Status", "Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecordService(Index)
        End Select
    End Sub
    Private Sub dgMonitorServiceStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorServiceStatusList.Sorting
        mAssemblyMonitorServiceStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
        dgMonitorServiceStatusList.DataSource = mAssemblyMonitorServiceStatusList
        dgMonitorServiceStatusList.DataBind()
    End Sub
    Private Sub btnAddService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddService.Click, btnAddTopService.Click
        NewRecordService()
    End Sub
    Private Sub btnFindNowService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNowService.Click
        FindNowService()
        SetPageService()
        ControlVisibilityService()
        SetRightsService()  'Added by Utkarsh On 21-Mar-2011
        upnlGridService.Update()
        upnlActionBtnTopService.Update()
        upnlActionBtnService.Update()
    End Sub
    Private Sub btnCloseService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseService.Click, btnCloseTopService.Click
        MarkLog(Util.Action.Close, "Assembly Service Status", " Model : " & mAssemblyStatus.ModelName & " Serial No. : " & mAssemblyStatus.Assembly.SerialNo, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionService()
        'Response.Redirect(Request.QueryString("GChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
        TbContInst.ActiveTabIndex = 0
        TbContInst_ActiveTabChanged(Nothing, Nothing)
        upnlTabs.Update()
    End Sub
    Private Sub cmbLookInService_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookInService.SelectedIndexChanged
        cmbSearchForService.SelectedIndex = 0
        DisplayControlsService(cmbLookInService.SelectedIndex)
        If cmbLookInService.Enabled = True Then
            cmbLookInService.Focus()
        End If
    End Sub
#End Region

#Region " Report "
    'Created By:- Jyoti

#Region "Event"

    Private Sub btnPrintService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintService.Click, btnPrintTopService.Click
        GridBindService()
        Dim Rpt As New crListAssemblyMonitorStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 4
        RHCount = Me.mAssemblyStatus.AssemblyStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            If mAssemblyStatus.AssemblyTypeID = 1 Then
                ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Manufacturer", _
                       Me.mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , "Since New Values as on " & (mAssemblyStatus.AsOnDateFormatted).ToString, _
                           "Periods", , _
                             , "Airframe"))
            Else
                ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Manufacturer", _
                   Me.mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , "Since New Values as on " & (mAssemblyStatus.AsOnDateFormatted).ToString, _
                   "Periods", "Engine", _
                     , "Airframe"))
            End If
        End If

        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Model", _
                                         Me.mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , , _
                                            CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                            CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                         , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Model", _
                               Me.mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , , _
                               "", "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Serial No.", _
                                          "", , , , , , , , , , , , , , , , , , _
                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                          , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Serial No.", _
                                          "", , , , , , , , , , , , , , , , , , _
                                          "", "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Position", _
                                                         Me.mAssemblyStatus.Position, , , , , , , , , , , , , , , , , , _
                                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                                          , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Position", _
                                                          Me.mAssemblyStatus.Position, , , , , , , , , , , , , , , , , , _
                                                          "", "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "", _
                                           "", , , , , , , , , , , , , , , , , , _
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "", _
                                           "", , , , , , , , , , , , , , , , , , _
                                           "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, , "", _
                                                    "", , , , , , , , , , , , , , , , , , _
                                                    CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                                    CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                                 , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
            End If
        Next

        'For Assembly Service List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , lblServiceText.Text))

        'For Assembly Monitor Service List
        ReportDetails.Add(New rptStatus(, 2, _
             , , , , dgMonitorServiceStatusList.Columns.Item(3).HeaderText, dgMonitorServiceStatusList.Columns.Item(11).HeaderText, dgMonitorServiceStatusList.Columns.Item(4).HeaderText, dgMonitorServiceStatusList.Columns.Item(5).HeaderText, _
               dgMonitorServiceStatusList.Columns.Item(6).HeaderText, dgMonitorServiceStatusList.Columns.Item(7).HeaderText, dgMonitorServiceStatusList.Columns.Item(8).HeaderText, dgMonitorServiceStatusList.Columns.Item(9).HeaderText, , , , , , , , , , , , , , , dgMonitorServiceStatusList.Columns.Item(10).HeaderText))

        Dim TotalCount1 As Integer
        TotalCount1 = Me.mAssemblyMonitorServiceStatusList.Count
        Dim m As Integer

        Dim str(8) As String


        For m = 0 To TotalCount1 - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""
            str(8) = ""
            'Commented by Saylee on 1/04/2008 Suggested by Deven sir-------------------------------------
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgMonitorServiceStatusList.Rows(m).Cells(1).Text
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(2).Text <> "&nbsp;" Then str(1) = Me.dgMonitorServiceStatusList.Rows(m).Cells(2).Text
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(2) = Me.dgMonitorServiceStatusList.Rows(m).Cells(3).Text
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(3) = Me.dgMonitorServiceStatusList.Rows(m).Cells(4).Text
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(4) = Me.dgMonitorServiceStatusList.Rows(m).Cells(5).Text
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(5) = Me.dgMonitorServiceStatusList.Rows(m).Cells(6).Text
            'If Me.dgMonitorServiceStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(6) = Me.dgMonitorServiceStatusList.Rows(m).Cells(7).Text

            'code added by Saylee on 1/04/2008 Suggested by Deven sir-------------------------------------
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(0) = Me.dgMonitorServiceStatusList.Rows(m).Cells(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(1) = Me.dgMonitorServiceStatusList.Rows(m).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(2) = Me.dgMonitorServiceStatusList.Rows(m).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(3) = Me.dgMonitorServiceStatusList.Rows(m).Cells(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(4) = Me.dgMonitorServiceStatusList.Rows(m).Cells(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(8).Text <> "&nbsp;" Then str(5) = Me.dgMonitorServiceStatusList.Rows(m).Cells(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(9).Text <> "&nbsp;" Then str(6) = Me.dgMonitorServiceStatusList.Rows(m).Cells(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(10).Text <> "&nbsp;" Then str(7) = Me.dgMonitorServiceStatusList.Rows(m).Cells(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorServiceStatusList.Rows(m).Cells(11).Text <> "&nbsp;" Then str(8) = Me.dgMonitorServiceStatusList.Rows(m).Cells(11).Text.Replace("<BR>", vbCrLf)
            '-------------------------------------------------------------------------------------
            ReportDetails.Add(New rptStatus(, 3, , _
                   , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6), , , , , , , str(8), , str(7)))
        Next


        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Assembly Service Status List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If Me.mAssemblyMonitorServiceStatusList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim mrptimage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mrptimage)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "Assembly Monitor Service Status", "Assembly Monitor Service Status List Report", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region

#End Region

#Region " Insp List "

#Region " Variable Declaration "
    Public mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
    Public mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList
    Public mModelMonitorInspTypeList As ModelMonitorInspTypeList
    Public SearchForInsp As String
#End Region

#Region " Business Methods "
    Private Sub GetSessionInsp()

        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mAssemblyMonitorInspStatusList = CType(Session("mAssemblyMonitorInspStatusList"), tmpAssemblyMonitorInspStatusList)
        mModelMonitorInspTypeList = CType(Session("mModelMonitorInspTypeList"), ModelMonitorInspTypeList)
        'SearchFor = Session("SearchFor")
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
    End Sub
    Private Sub RemoveSessionInsp()
        Session.Remove("mModelMonitorInspTypeList")
        Session.Remove("mAssemblyMonitorInspStatusList")
        Session.Remove("LookInInsp")
        Session.Remove("txtForInsp")
        Session.Remove("txtCodeInsp")
        Session.Remove("SearchForInsp")
    End Sub
    'Added By Vikrant On 25-Jun-2014
    Private Sub RemoveAllSessionValuesInsp()
        Session.Remove("mModelList")
        Session.Remove("mATAList")
        Session.Remove("Add")
        Session.Remove("Edit")
        Session.Remove("mAssemblyStatusList")
        Session.Remove("mAssemblyStatus")
        Session.Remove("mAssemblyTypeListForUI")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    'End
    Private Sub NewRecordInsp()
        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate, mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.HourType)
        mAssemblyMonitorInspStatus.BeginEdit()
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList

        MarkLog(Util.Action.[New], "Assembly Insp Status", "", Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)

        'Code  Added By Saylee  on 1/4/2008 suggested by Deven sir
        Session("EditMasterRecord") = "False"
        Session("IsOpenFromMaster") = True
        'Response.Redirect("wfModelMonitorInspList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfAssemblyMonitorInspStatusList_Ajax.aspx" & "&GChildPage3=wfAssemblyMonitorInspStatusList_Ajax.aspx")
        mModelMaintenanceActivityListCount = ModelMaintenanceActivityListCount.GetModelMaintenanceActivityListCount(mAssemblyStatus.Assembly.ModelID)
        If mModelMaintenanceActivityListCount.ModelInspListCount > 0 Then
            Response.Redirect("wfModelMonitorInspList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfSpareAssemblyStatus.aspx" & "&GChildPage3=wfSpareAssemblyStatus.aspx")
        Else
            Dim mModelMonitorInsp As ModelMonitorInsp
            Dim ID As Guid = Guid.NewGuid 'Revise Activity
            mModelMonitorInsp = ModelMonitorInsp.NewModelMonitorInsp(ID, mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.HourType, ID) 'For new records ID,PrevRefID are same
            Session("mModelMonitorInsp") = mModelMonitorInsp
            mModelMonitorInsp.BeginEdit()
            MarkLog(Util.Action.[New], "Model Monitor Insp", " Model : " & mAssemblyStatus.Assembly.ModelName, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'Response.Redirect("wfModelMonitorInsp_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfModelMonitorInspList_Ajax.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelInspMasterWindow", "OpenModelInspMasterWindow()", True)
        End If
        '---------------------
    End Sub
    Private Sub EditRecordInsp(ByVal mId As Guid)
        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mId, mAssemblyStatus.ID, mAssemblyStatus.HourType)
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("IsOpenFromMaster") = True

        MachineDetail = "Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- " & "Description : " & mAssemblyMonitorInspStatusList(mId).Description & " Monitor Type : " & mAssemblyMonitorInspStatusList(mId).MonitorType
        'MarkLog(Util.Action.Edit, "Assembly Insp Status",  & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Insp Type : " + mAssemblyMonitorInspStatusList.Item(mAssemblyMonitorInspStatusList.CurrentIndex).MonitorType + " Description : " + mAssemblyMonitorInspStatusList.Item(mAssemblyMonitorInspStatusList.CurrentIndex).Description, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
        MarkLog(Util.Action.Edit, "Assembly Insp Status", MachineDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
        Session("Edit") = True
        Response.Redirect("wfAssemblyMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfSpareAssemblyStatus.aspx")
    End Sub

    'code added by Saylee on 1/04/2008 Suggested by Deven sir
    Private Sub EditMasterRecordInsp(ByVal mMasterId As Guid, ByVal mId As Guid)
        Dim mModelMonitorInsp As ModelMonitorInsp
        mModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(mMasterId, mAssemblyStatus.HourType)
        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mId, mAssemblyStatus.ID, mAssemblyStatus.HourType)
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus

        Session("mModelMonitorInsp") = mModelMonitorInsp
        Session("IsOpenFromMaster") = True

        'RemoveSession()
        'Response.Redirect("wfModelMonitorInsp_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfAssemblyMonitorInspStatusList_Ajax.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelInspMasterWindow", "OpenModelInspMasterWindow()", True)

    End Sub
    '------------------------------------------

    Private Sub DeleteRecordInsp(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mAssemblyMonitorInspStatusList.CurrentIndex = Index
        Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
    End Sub
    Private Sub MessageBoxResultInsp()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Dim InspIDForEventLog As Guid
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            MachineDetail = "Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- " & "Description : " & mAssemblyMonitorInspStatusList.CurrentItem.Description & " Monitor Type : " & mAssemblyMonitorInspStatusList.CurrentItem.MonitorType
                            mAssemblyMonitorInspStatusList = CType(Session("mAssemblyMonitorInspStatusList"), tmpAssemblyMonitorInspStatusList)
                            InspIDForEventLog = mAssemblyMonitorInspStatusList.Item(mAssemblyMonitorInspStatusList.CurrentIndex).ID
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorInspStatusList.CurrentItem.ID, 5) 'Added by Saylee on 13th-Oct-2009
                            AssemblyMonitorInspStatus.DeleteAssemblyMonitorInspStatus(mAssemblyMonitorInspStatusList.CurrentItem.ID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            Session("mMachineMaintenance") = mMachineMaintenance
                            Session("mAircraftInformationBoardList") = Nothing 'Added by Saylee on 16-July-2009
                            SetControlsInsp()
                            FindNowInsp()
                            SetPageInsp()
                            ControlVisibilityInsp()
                            SetRightsInsp()
                            upnlGridInsp.Update()
                            upnlActionBtnInsp.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Assembly Insp Status", "Can't delete : " & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Insp Type : " + mAssemblyMonitorInspStatusList.Item(mAssemblyMonitorInspStatusList.CurrentIndex).MonitorType + " Description : " + mAssemblyMonitorInspStatusList.Item(mAssemblyMonitorInspStatusList.CurrentIndex).Description + " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "Assembly Insp Status",  & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Monitor Insp Type : " + mAssemblyMonitorInspStatusList.Item(mAssemblyMonitorInspStatusList.CurrentIndex).MonitorType + " Description : " + mAssemblyMonitorInspStatusList.Item(mAssemblyMonitorInspStatusList.CurrentIndex).Description, Util.ErrorType.NoError, mAssemblyMonitorInspStatusList.Item(mAssemblyMonitorInspStatusList.CurrentIndex).ID, EventLogID)
                                MarkLog(Util.Action.Delete, "Assembly Insp Status", MachineDetail, Util.ErrorType.NoError, InspIDForEventLog, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
    Private Sub SetPageInsp()
        If mAssemblyStatus.IsNew Then
            lblTitle.Text = "Build Assembly  [New]"
        Else
            lblTitle.Text = mAssemblyStatus.AssemblyTypeName & " [Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]"
        End If
        'CNDC
        'lblInspText.Text = "List of all the Servicings on the " & mMachine.RegNo & " as of Date: " & SmartDate.StringToDate((mAssemblyStatus.AsOnDate).ToString).ToShortDateString & ". All the values of all the Insps will be as of Date: " & SmartDate.StringToDate((mAssemblyStatus.AsOnDate).ToString).ToShortDateString & "."
        lblInspText.Text = "List of all the Inspections on the " & " as of Date: " & mAssemblyStatus.AsOnDateFormatted & ". All the values of all the Inspections will be as of Date: " & mAssemblyStatus.AsOnDateFormatted & "."
        lblResultInsp.Text = "List of Inspections: " & mAssemblyMonitorInspStatusList.Count & " Record(s)."
    End Sub
    Private Sub addAttributesInsp()
        txtCodeInsp.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCodeInsp').value,event)")
    End Sub
    Private Sub DisplayControlsInsp(ByVal Index As Integer)
        '---------------Commented and added By Saylee on 28th-Jan-2008-------------
        'txtFor.Text = ""
        'txtCode.Text = ""
        txtForInsp.Text = IIf(Index = 2 Or Index = 4, txtForInsp.Text, "")
        txtCodeInsp.Text = IIf(Index = 1, txtCodeInsp.Text, "")
        '--------------------------------------------------------------------------
        txtCodeInsp.Visible = IIf(Index = 1, True, False)
        txtForInsp.Visible = IIf(Index = 2 Or Index = 4, True, False)
        lblForInsp.Visible = (Index > 0 And Index <> 5)
        cmbSearchForInsp.Visible = (Index = 3)
    End Sub
    Private Sub ControlVisibilityInsp()
        btnPrintInsp.Enabled = mAssemblyMonitorInspStatusList.Count > 0
        btnPrintTopInsp.Enabled = mAssemblyMonitorInspStatusList.Count > 0


        btnAddTopInsp.Visible = mAssemblyMonitorInspStatusList.Count > 10
        btnPrintTopInsp.Visible = mAssemblyMonitorInspStatusList.Count > 10
        btnCloseTopInsp.Visible = mAssemblyMonitorInspStatusList.Count > 10

    End Sub
    Private Sub GridBindInsp()
        dgMonitorInspStatusList.DataSource = mAssemblyMonitorInspStatusList
        dgMonitorInspStatusList.DataBind()
    End Sub
    Private Sub SetControlsInsp()
        '======Function added By Saylee on 28-th-Jan-2008 for bug-Insp List (SL3)
        txtForInsp.Text = Session("txtForInsp")
        txtCodeInsp.Text = Session("txtCodeInsp")
        cmbLookInInsp.SelectedIndex = Session("LookInInsp")
        '==========================================================================
        cmbSearchForInsp.SelectedValue = IIf(SearchForInsp = "", 0, SearchForInsp)
        DisplayControlsInsp(cmbLookInInsp.SelectedIndex)
        'FindNow()
    End Sub
    'Added By Utkarsh On 14-Mar-2011
    Private Sub SetRightsInsp()
        If mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineAssemblyInspectionPrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
                btnPrintTopInsp.Enabled = False
                btnPrintTopInsp.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineAssemblyInspectionNew")) = False Then
                btnAddInsp.Enabled = False
                btnAddInsp.ToolTip = "You are not authorized user"
                btnAddTopInsp.Enabled = False
                btnAddTopInsp.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineAssemblyInspectionPrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
                btnPrintTopInsp.Enabled = False
                btnPrintTopInsp.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineAssemblyInspectionNew")) = False Then
                btnAddInsp.Enabled = False
                btnAddInsp.ToolTip = "You are not authorized user"
                btnAddTopInsp.Enabled = False
                btnAddTopInsp.ToolTip = "You are not authorized user"
            End If
        End If
    End Sub
    '*******************************
#End Region

#Region " Data Binding "
    Private Sub FindNowInsp()
        dgMonitorInspStatusList.PageIndex = 0
        Select Case cmbLookInInsp.SelectedIndex
            Case 0, -1  'All
                mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 1  'ATA Code
                mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, Val(txtCodeInsp.Text), , , , , , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 2  'Description
                mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , txtForInsp.Text.Trim, , , , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 3  'Insp Type ID
                mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , CInt(cmbSearchForInsp.SelectedValue), , , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 4 ' Work Order No.
                mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , txtForInsp.Text.Trim, , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 5  'Show In C of A
                mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , True, mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
        End Select
        Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
        dgMonitorInspStatusList.DataSource = mAssemblyMonitorInspStatusList
        dgMonitorInspStatusList.DataBind()

        'Added By Saylee on 28th-Jan-2008===============
        Session("LookInInsp") = cmbLookInInsp.SelectedIndex
        Session("txtForInsp") = txtForInsp.Text
        Session("txtCodeInsp") = txtCodeInsp.Text
        SearchForInsp = IIf(cmbSearchForInsp.SelectedIndex <= 0, "", cmbSearchForInsp.SelectedValue) 'cmbSearchFor.SelectedIndex
        Session("SearchForInsp") = SearchForInsp
        '==================================================
    End Sub
    Private Sub DataFieldBindInsp()
        'mAssemblyMonitorInspStatusList = tmpAssemblyMonitorInspStatusList.GetAssemblyMonitorInspStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , , mAssemblyStatus.ID.ToString)
        mModelMonitorInspTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList("(All)")
        cmbSearchForInsp.DataSource = mModelMonitorInspTypeList
        Session("mModelMonitorInspTypeList") = mModelMonitorInspTypeList
        'dgMonitorInspStatusList.DataSource = mAssemblyMonitorInspStatusList
        'Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
        DataBind()
        SearchForInsp = Session("SearchForInsp")
    End Sub
#End Region

#Region " Event "
    Private Sub dgMonitorInspStatusList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorInspStatusList.RowCommand
        Dim Index As Int32
        Dim mId As Guid
        Dim mMasterId As Guid
        Select Case e.CommandName
            Case "EditRec"
                GridBindInsp()
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageSize * dgMonitorInspStatusList.PageIndex
                mId = mAssemblyMonitorInspStatusList(Index).ID
                If (Not User.IsInRole("BuildSpareAssemblyView") And Not User.IsInRole("BuildSpareAssemblyEdit")) Then
                    MarkLog(Util.Action.Edit, "Assembly Insp Status", User.Identity.Name & " is not Authorized User to edit " & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Insp Type : " + mAssemblyMonitorInspStatusList.Item(mAssemblyMonitorInspStatusList.CurrentIndex).MonitorType + " Description : " + mAssemblyMonitorInspStatusList.Item(mAssemblyMonitorInspStatusList.CurrentIndex).Description, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                EditRecordInsp(mId)
                'Added by Saylee on 1/04/2008 Suggested By Deven Sir
            Case "EditMaster"
                GridBindInsp()
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageSize * dgMonitorInspStatusList.PageIndex
                mId = mAssemblyMonitorInspStatusList(Index).ID
                mMasterId = mAssemblyMonitorInspStatusList(Index).ModelMonitorInspID 'New Guid(dgMonitorInspStatusList.DataKeys(Index).Values("ModelMonitorInspID").ToString)
                'Added By Utkarsh On 14-Mar-2011
                If ((mAssemblyStatus.IsMaster) And (Not User.IsInRole("BuildSpareAssemblyView") And Not User.IsInRole("BuildSpareAssemblyEdit"))) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("EditMasterRecord") = "True"
                EditMasterRecordInsp(mMasterId, mId)
            Case "DeleteRec"
                GridBindInsp()
                Index = CInt(e.CommandArgument) + dgMonitorInspStatusList.PageSize * dgMonitorInspStatusList.PageIndex
                mId = mAssemblyMonitorInspStatusList(Index).ID
                'Added By Utkarsh On 14-Mar-2011
                If (User.IsInRole("BuildSpareAssemblyDelete")) = False Then
                    MarkLog(Util.Action.Delete, "Assembly Insp Status", " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecordInsp(Index)
        End Select
    End Sub
    Private Sub dgMonitorInspStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorInspStatusList.Sorting
        mAssemblyMonitorInspStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
        dgMonitorInspStatusList.DataSource = mAssemblyMonitorInspStatusList
        dgMonitorInspStatusList.DataBind()
    End Sub
    Private Sub btnAddInsp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddInsp.Click, btnAddTopInsp.Click
        NewRecordInsp()
    End Sub
    Private Sub btnFindNowInsp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNowInsp.Click
        FindNowInsp()
        SetPageInsp()
        ControlVisibilityInsp()
        SetRightsInsp()  'Added by Utkarsh On 21-Mar-2011
        upnlGridInsp.Update()
        upnlActionBtnTopInsp.Update()
        upnlActionBtnInsp.Update()
    End Sub
    Private Sub btnCloseInsp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseInsp.Click, btnCloseTopInsp.Click
        MarkLog(Util.Action.Close, "Assembly Insp Status", " Model : " & mAssemblyStatus.ModelName & " Serial No. : " & mAssemblyStatus.Assembly.SerialNo, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionInsp()
        'Response.Redirect(Request.QueryString("GChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
        TbContInst.ActiveTabIndex = 0
        TbContInst_ActiveTabChanged(Nothing, Nothing)
        upnlTabs.Update()
    End Sub
    Private Sub cmbLookInInsp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookInInsp.SelectedIndexChanged
        cmbSearchForInsp.SelectedIndex = 0
        DisplayControlsInsp(cmbLookInInsp.SelectedIndex)
        If cmbLookInInsp.Enabled = True Then
            cmbLookInInsp.Focus()
        End If
    End Sub
#End Region

#Region " Report "
    'Created By:- Jyoti

#Region "Event"

    Private Sub btnPrintInsp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintInsp.Click, btnPrintTopInsp.Click
        GridBindInsp()
        Dim Rpt As New crListAssemblyMonitorStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 4
        RHCount = Me.mAssemblyStatus.AssemblyStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            If mAssemblyStatus.AssemblyTypeID = 1 Then
                ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Manufacturer", _
                       Me.mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , "Since New Values as on " & (mAssemblyStatus.AsOnDateFormatted).ToString, _
                           "Periods", , _
                             , "Airframe"))
            Else
                ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Manufacturer", _
                   Me.mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , "Since New Values as on " & (mAssemblyStatus.AsOnDateFormatted).ToString, _
                   "Periods", "Engine", _
                     , "Airframe"))
            End If
        End If

        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Model", _
                                         Me.mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , , _
                                            CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                            CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                         , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Model", _
                               Me.mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , , _
                               "", "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Serial No.", _
                                          "", , , , , , , , , , , , , , , , , , _
                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                          , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Serial No.", _
                                          "", , , , , , , , , , , , , , , , , , _
                                          "", "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Position", _
                                                         Me.mAssemblyStatus.Position, , , , , , , , , , , , , , , , , , _
                                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                                          , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Position", _
                                                          Me.mAssemblyStatus.Position, , , , , , , , , , , , , , , , , , _
                                                          "", "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "", _
                                           "", , , , , , , , , , , , , , , , , , _
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "", _
                                           "", , , , , , , , , , , , , , , , , , _
                                           "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, , "", _
                                                    "", , , , , , , , , , , , , , , , , , _
                                                    CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                                    CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                                 , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
            End If
        Next

        'For Assembly Insp List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , lblInspText.Text))

        'For Assembly Monitor Insp List
        'ReportDetails.Add(New rptStatus(, 2, _
        '     , , , , dgMonitorInspStatusList.Columns.Item(2).HeaderText, , dgMonitorInspStatusList.Columns.Item(3).HeaderText, dgMonitorInspStatusList.Columns.Item(4).HeaderText, _
        '      dgMonitorInspStatusList.Columns.Item(5).HeaderText, dgMonitorInspStatusList.Columns.Item(6).HeaderText, dgMonitorInspStatusList.Columns.Item(7).HeaderText, dgMonitorInspStatusList.Columns.Item(8).HeaderText, , , , , , , , , , , , , , , dgMonitorInspStatusList.Columns.Item(9).HeaderText))

        ReportDetails.Add(New rptStatus(, 2, _
             , , , , dgMonitorInspStatusList.Columns.Item(3).HeaderText, dgMonitorInspStatusList.Columns.Item(11).HeaderText, dgMonitorInspStatusList.Columns.Item(4).HeaderText, dgMonitorInspStatusList.Columns.Item(5).HeaderText, _
               dgMonitorInspStatusList.Columns.Item(6).HeaderText, dgMonitorInspStatusList.Columns.Item(7).HeaderText, dgMonitorInspStatusList.Columns.Item(8).HeaderText, dgMonitorInspStatusList.Columns.Item(9).HeaderText, , , , , , , , , , , , , , , dgMonitorInspStatusList.Columns.Item(10).HeaderText))


        Dim TotalCount1 As Integer
        TotalCount1 = Me.mAssemblyMonitorInspStatusList.Count
        Dim m As Integer

        Dim str(8) As String


        For m = 0 To TotalCount1 - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""
            str(8) = ""
            'Commented by Saylee on 1/04/2008 Suggested by Deven sir-------------------------------------
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgMonitorInspStatusList.Rows(m).Cells(1).Text
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(2).Text <> "&nbsp;" Then str(1) = Me.dgMonitorInspStatusList.Rows(m).Cells(2).Text
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(2) = Me.dgMonitorInspStatusList.Rows(m).Cells(3).Text
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(3) = Me.dgMonitorInspStatusList.Rows(m).Cells(4).Text
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(4) = Me.dgMonitorInspStatusList.Rows(m).Cells(5).Text
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(5) = Me.dgMonitorInspStatusList.Rows(m).Cells(6).Text
            'If Me.dgMonitorInspStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(6) = Me.dgMonitorInspStatusList.Rows(m).Cells(7).Text

            'code added by Saylee on 1/04/2008 Suggested by Deven sir-------------------------------------
            If Me.dgMonitorInspStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(0) = Me.dgMonitorInspStatusList.Rows(m).Cells(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(1) = Me.dgMonitorInspStatusList.Rows(m).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(2) = Me.dgMonitorInspStatusList.Rows(m).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(3) = Me.dgMonitorInspStatusList.Rows(m).Cells(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(4) = Me.dgMonitorInspStatusList.Rows(m).Cells(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(8).Text <> "&nbsp;" Then str(5) = Me.dgMonitorInspStatusList.Rows(m).Cells(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(9).Text <> "&nbsp;" Then str(6) = Me.dgMonitorInspStatusList.Rows(m).Cells(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(10).Text <> "&nbsp;" Then str(7) = Me.dgMonitorInspStatusList.Rows(m).Cells(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorInspStatusList.Rows(m).Cells(11).Text <> "&nbsp;" Then str(8) = Me.dgMonitorInspStatusList.Rows(m).Cells(11).Text.Replace("<BR>", vbCrLf)
            '-------------------------------------------------------------------------------------
            ReportDetails.Add(New rptStatus(, 3, , _
                     , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6), , , , , , , str(8), , str(7)))
        Next


        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Assembly Insp Status List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If Me.mAssemblyMonitorInspStatusList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim mrptimage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mrptimage)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "Assembly Monitor Insp Status", "Assembly Monitor Insp Status List Report", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region

#End Region

#Region " Mod List "

#Region " Variable Declaration "
    Public mAssemblyMonitorModStatus As AssemblyMonitorModStatus
    Public mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList
    Public mModelMonitorModTypeList As ModelMonitorModTypeList
    Public SearchForMod As String
#End Region

#Region " Business Methods "
    Private Sub GetSessionMod()

        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mAssemblyMonitorModStatusList = CType(Session("mAssemblyMonitorModStatusList"), tmpAssemblyMonitorModStatusList)
        mModelMonitorModTypeList = CType(Session("mModelMonitorModTypeList"), ModelMonitorModTypeList)
        'SearchFor = Session("SearchFor")
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 13th-Oct-2009
    End Sub
    Private Sub RemoveSessionMod()
        Session.Remove("mModelMonitorModTypeList")
        Session.Remove("mAssemblyMonitorModStatusList")
        Session.Remove("LookInMod")
        Session.Remove("txtForMod")
        Session.Remove("txtCodeMod")
        Session.Remove("SearchForMod")
    End Sub
    'Added By Vikrant On 25-Jun-2014
    Private Sub RemoveAllSessionValuesMod()
        Session.Remove("mModelList")
        Session.Remove("mATAList")
        Session.Remove("Add")
        Session.Remove("Edit")
        Session.Remove("mAssemblyStatusList")
        Session.Remove("mAssemblyStatus")
        Session.Remove("mAssemblyTypeListForUI")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    'End
    Private Sub NewRecordMod()
        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, mAssemblyStatus.AsOnDate, mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.HourType)
        mAssemblyMonitorModStatus.BeginEdit()
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList

        MarkLog(Util.Action.[New], "Assembly Mod Status", "", Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)

        'Code  Added By Saylee  on 1/4/2008 suggested by Deven sir
        Session("EditMasterRecord") = "False"
        Session("IsOpenFromMaster") = True

        'Response.Redirect("wfModelMonitorModList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfAssemblyMonitorModStatusList_Ajax.aspx" & "&GChildPage3=wfAssemblyMonitorModStatusList_Ajax.aspx")
        mModelMaintenanceActivityListCount = ModelMaintenanceActivityListCount.GetModelMaintenanceActivityListCount(mAssemblyStatus.Assembly.ModelID)
        If mModelMaintenanceActivityListCount.ModelModListCount > 0 Then
            Response.Redirect("wfModelMonitorModList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfSpareAssemblyStatus.aspx" & "&GChildPage3=wfSpareAssemblyStatus.aspx")
        Else
            Dim mModelMonitorMod As ModelMonitorMod
            Dim ID As Guid = Guid.NewGuid 'Revise Activity
            mModelMonitorMod = ModelMonitorMod.NewModelMonitorMod(ID, mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.HourType, ID)           'mModel.ID)
            Session("mModelMonitorMod") = mModelMonitorMod
            mModelMonitorMod.BeginEdit()
            MarkLog(Util.Action.[New], "Model Monitor Mod", " Model : " & mAssemblyStatus.Assembly.ModelName, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'Response.Redirect("wfModelMonitorMod_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfModelMonitorModList_Ajax.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelModMasterWindow", "OpenModelModMasterWindow()", True)
        End If
        '---------------------
    End Sub
    Private Sub EditRecordMod(ByVal mId As Guid)
        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mId, mAssemblyStatus.ID, mAssemblyStatus.HourType)
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("IsOpenFromMaster") = True

        MachineDetail = "Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- " & "Description : " & mAssemblyMonitorModStatusList(mId).Description & " Monitor Type : " & mAssemblyMonitorModStatusList(mId).MonitorType
        'MarkLog(Util.Action.Edit, "Assembly Mod Status",  & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Mod Type : " + mAssemblyMonitorModStatusList.Item(mAssemblyMonitorModStatusList.CurrentIndex).MonitorType + " Description : " + mAssemblyMonitorModStatusList.Item(mAssemblyMonitorModStatusList.CurrentIndex).Description, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
        MarkLog(Util.Action.Edit, "Assembly Mod Status", MachineDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
        Session("Edit") = True
        Response.Redirect("wfAssemblyMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfSpareAssemblyStatus.aspx")
    End Sub

    'code added by Saylee on 1/04/2008 Suggested by Deven sir
    Private Sub EditMasterRecordMod(ByVal mMasterId As Guid, ByVal mId As Guid)
        Dim mModelMonitorMod As ModelMonitorMod
        mModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mMasterId, mAssemblyStatus.HourType)
        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mId, mAssemblyStatus.ID, mAssemblyStatus.HourType)
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus

        Session("IsOpenFromMaster") = True

        Session("mModelMonitorMod") = mModelMonitorMod
        'RemoveSession()
        'Response.Redirect("wfModelMonitorMod_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfAssemblyMonitorModStatusList_Ajax.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelModMasterWindow", "OpenModelModMasterWindow()", True)

    End Sub
    '------------------------------------------

    Private Sub DeleteRecordMod(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mAssemblyMonitorModStatusList.CurrentIndex = Index
        Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
    End Sub
    Private Sub MessageBoxResultMod()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Dim ModIDForEventLog As Guid
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            MachineDetail = "Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- " & "Description : " & mAssemblyMonitorModStatusList.CurrentItem.Description & " Monitor Type : " & mAssemblyMonitorModStatusList.CurrentItem.MonitorType
                            mAssemblyMonitorModStatusList = CType(Session("mAssemblyMonitorModStatusList"), tmpAssemblyMonitorModStatusList)
                            ModIDForEventLog = mAssemblyMonitorModStatusList.Item(mAssemblyMonitorModStatusList.CurrentIndex).ID
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorModStatusList.CurrentItem.ID, 5) 'Added by Saylee on 13th-Oct-2009
                            AssemblyMonitorModStatus.DeleteAssemblyMonitorModStatus(mAssemblyMonitorModStatusList.CurrentItem.ID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            Session("mMachineMaintenance") = mMachineMaintenance
                            Session("mAircraftInformationBoardList") = Nothing 'Added by Saylee on 16-July-2009
                            SetControlsMod()
                            FindNowMod()
                            SetPageMod()
                            ControlVisibilityMod()
                            SetRightsMod()
                            upnlGridMod.Update()
                            upnlActionBtnMod.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Assembly Mod Status", "Can't delete : " & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Mod Type : " + mAssemblyMonitorModStatusList.Item(mAssemblyMonitorModStatusList.CurrentIndex).MonitorType + " Description : " + mAssemblyMonitorModStatusList.Item(mAssemblyMonitorModStatusList.CurrentIndex).Description + " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            ElseIf ex.Number = 50000 Then 'Added by vikrant on 06-Mar-2020 to prevent deletion if that activity is selected in WO job
                                MSGBoxCtrl.show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "Assembly Mod Status",  & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Monitor Mod Type : " + mAssemblyMonitorModStatusList.Item(mAssemblyMonitorModStatusList.CurrentIndex).MonitorType + " Description : " + mAssemblyMonitorModStatusList.Item(mAssemblyMonitorModStatusList.CurrentIndex).Description, Util.ErrorType.NoError, mAssemblyMonitorModStatusList.Item(mAssemblyMonitorModStatusList.CurrentIndex).ID, EventLogID)
                                MarkLog(Util.Action.Delete, "Assembly Mod Status", MachineDetail, Util.ErrorType.NoError, ModIDForEventLog, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
    Private Sub SetPageMod()
        If mAssemblyStatus.IsNew Then
            lblTitle.Text = "Build Assembly  [New]"
        Else
            lblTitle.Text = mAssemblyStatus.AssemblyTypeName & " [Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]"
        End If
        'CNDC
        'lblModText.Text = "List of all the Servicings on the " & mMachine.RegNo & " as of Date: " & SmartDate.StringToDate((mAssemblyStatus.AsOnDate).ToString).ToShortDateString & ". All the values of all the Mods will be as of Date: " & SmartDate.StringToDate((mAssemblyStatus.AsOnDate).ToString).ToShortDateString & "."
        lblModText.Text = "List of all the Directives" & " as of Date: " & mAssemblyStatus.AsOnDateFormatted & ". All the values of all the Directives will be as of Date: " & mAssemblyStatus.AsOnDateFormatted & "."
        lblResultMod.Text = "List of Directives: " & mAssemblyMonitorModStatusList.Count & " Record(s)."
    End Sub
    Private Sub addAttributesMod()
        txtCodeMod.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCodeMod').value,event)")
    End Sub
    Private Sub DisplayControlsMod(ByVal Index As Integer)
        '---------------Commented and added By Saylee on 28th-Jan-2008-------------
        'txtFor.Text = ""
        'txtCode.Text = ""
        txtForMod.Text = IIf(Index = 2 Or Index = 4 Or Index = 5, txtForMod.Text, "")
        txtCodeMod.Text = IIf(Index = 1, txtCodeMod.Text, "")
        '--------------------------------------------------------------------------
        txtCodeMod.Visible = IIf(Index = 1, True, False)
        txtForMod.Visible = IIf(Index = 2 Or Index = 4 Or Index = 5, True, False)
        lblForMod.Visible = (Index > 0 And Index <> 6)
        cmbSearchForMod.Visible = (Index = 3)
    End Sub
    Private Sub ControlVisibilityMod()
        btnPrintMod.Enabled = mAssemblyMonitorModStatusList.Count > 0
        btnPrintTopMod.Enabled = mAssemblyMonitorModStatusList.Count > 0

        btnAddTopMod.Visible = mAssemblyMonitorModStatusList.Count > 10
        btnPrintTopMod.Visible = mAssemblyMonitorModStatusList.Count > 10
        btnCloseTopMod.Visible = mAssemblyMonitorModStatusList.Count > 10
    End Sub
    Private Sub GridBindMod()
        dgMonitorModStatusList.DataSource = mAssemblyMonitorModStatusList
        dgMonitorModStatusList.DataBind()
    End Sub
    Private Sub SetControlsMod()
        '======Function added By Saylee on 28-th-Jan-2008 for bug-Mod List (SL3)
        txtForMod.Text = Session("txtForMod")
        txtCodeMod.Text = Session("txtCodeMod")
        cmbLookInMod.SelectedIndex = Session("LookInMod")
        '==========================================================================
        cmbSearchForMod.SelectedValue = IIf(SearchForMod = "", 0, SearchForMod)
        DisplayControlsMod(cmbLookInMod.SelectedIndex)
        'FindNow()
    End Sub
    'Added By Utkarsh On 14-Mar-2011
    Private Sub SetRightsMod()
        If mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineAssemblyModificationPrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
                btnPrintTopMod.Enabled = False
                btnPrintTopMod.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineAssemblyModificationNew")) = False Then
                btnAddMod.Enabled = False
                btnAddMod.ToolTip = "You are not authorized user"
                btnAddTopMod.Enabled = False
                btnAddTopMod.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatus.IsMaster Then
            If (Not User.IsInRole("MachineAssemblyModificationPrint")) Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
                btnPrintTopMod.Enabled = False
                btnPrintTopMod.ToolTip = "You are not authorized user"
            End If
            If (User.IsInRole("MachineAssemblyModificationNew")) = False Then
                btnAddMod.Enabled = False
                btnAddMod.ToolTip = "You are not authorized user"
                btnAddTopMod.Enabled = False
                btnAddTopMod.ToolTip = "You are not authorized user"
            End If
        End If
    End Sub
    '*******************************
#End Region

#Region " Data Binding "
    Private Sub FindNowMod()
        dgMonitorModStatusList.PageIndex = 0
        Select Case cmbLookInMod.SelectedIndex
            Case 0, -1  'All
                mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 1  'ATA Code
                mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, Val(txtCodeMod.Text), , , , , , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 2  'Description
                mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , txtForMod.Text.Trim, , , , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 3  'Mod Type ID
                mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , CInt(cmbSearchForMod.SelectedValue), , , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 4 ' Work Order No.
                mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , txtForMod.Text.Trim, , mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
            Case 5  'Directive No.
                mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , , mAssemblyStatus.ID.ToString, txtForMod.Text.Trim, IsSpareAssembly:=True)
            Case 6  'Show In C of A
                mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , True, mAssemblyStatus.ID.ToString, IsSpareAssembly:=True)
        End Select
        Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
        dgMonitorModStatusList.DataSource = mAssemblyMonitorModStatusList
        dgMonitorModStatusList.DataBind()

        'Added By Saylee on 28th-Jan-2008===============
        Session("LookInMod") = cmbLookInMod.SelectedIndex
        Session("txtForMod") = txtForMod.Text
        Session("txtCodeMod") = txtCodeMod.Text
        SearchForMod = IIf(cmbSearchForMod.SelectedIndex <= 0, "", cmbSearchForMod.SelectedValue) 'cmbSearchFor.SelectedIndex
        Session("SearchForMod") = SearchForMod
        '==================================================
    End Sub
    Private Sub DataFieldBindMod()
        'mAssemblyMonitorModStatusList = tmpAssemblyMonitorModStatusList.GetAssemblyMonitorModStatusList(mAssemblyStatus.AsOnDate.ToString, mAssemblyStatus.AssemblyID, mAssemblyStatus.MachineID, True, , , , , , , mAssemblyStatus.ID.ToString)
        mModelMonitorModTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList("(All)")
        cmbSearchForMod.DataSource = mModelMonitorModTypeList
        Session("mModelMonitorModTypeList") = mModelMonitorModTypeList
        'dgMonitorModStatusList.DataSource = mAssemblyMonitorModStatusList
        'Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
        DataBind()
        SearchForMod = Session("SearchForMod")
    End Sub
#End Region

#Region " Event "
    Private Sub dgMonitorModStatusList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorModStatusList.RowCommand
        Dim Index As Int32
        Dim mId As Guid
        Dim mMasterId As Guid
        Select Case e.CommandName
            Case "EditRec"
                GridBindMod()
                Index = CInt(e.CommandArgument) + dgMonitorModStatusList.PageSize * dgMonitorModStatusList.PageIndex
                mId = mAssemblyMonitorModStatusList(Index).ID
                If (User.IsInRole("MachineAssemblyModificationView") Or User.IsInRole("MachineAssemblyModificationEdit")) = False Then
                    MarkLog(Util.Action.Edit, "Assembly Mod Status", User.Identity.Name & " is not Authorized User to edit " & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Mod Type : " + mAssemblyMonitorModStatusList.Item(mAssemblyMonitorModStatusList.CurrentIndex).MonitorType + " Description : " + mAssemblyMonitorModStatusList.Item(mAssemblyMonitorModStatusList.CurrentIndex).Description, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                EditRecordMod(mId)
                'Added by Saylee on 1/04/2008 Suggested By Deven Sir
            Case "EditMaster"
                GridBindMod()
                Index = CInt(e.CommandArgument) + dgMonitorModStatusList.PageSize * dgMonitorModStatusList.PageIndex
                mId = mAssemblyMonitorModStatusList(Index).ID
                mMasterId = mAssemblyMonitorModStatusList(Index).ModelMonitorModID 'New Guid(dgMonitorModStatusList.DataKeys(Index).Values("ModelMonitorModID").ToString)
                'Added By Utkarsh On 14-Mar-2011
                If (User.IsInRole("MachineAssemblyModificationView") Or User.IsInRole("MachineAssemblyModificationEdit")) = False Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("EditMasterRecord") = "True"
                EditMasterRecordMod(mMasterId, mId)
            Case "DeleteRec"
                GridBindMod()
                Index = CInt(e.CommandArgument) + dgMonitorModStatusList.PageSize * dgMonitorModStatusList.PageIndex
                mId = mAssemblyMonitorModStatusList(Index).ID
                'Added By Utkarsh On 14-Mar-2011
                If (User.IsInRole("MachineAssemblyModificationDelete")) = False Then
                    MarkLog(Util.Action.Delete, "Assembly Mod Status", " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecordMod(Index)
        End Select
    End Sub
    Private Sub dgMonitorModStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorModStatusList.Sorting
        mAssemblyMonitorModStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
        dgMonitorModStatusList.DataSource = mAssemblyMonitorModStatusList
        dgMonitorModStatusList.DataBind()
    End Sub
    Private Sub btnAddMod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMod.Click, btnAddTopMod.Click
        NewRecordMod()
    End Sub
    Private Sub btnFindNowMod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNowMod.Click
        FindNowMod()
        SetPageMod()
        ControlVisibilityMod()
        SetRightsMod()  'Added by Utkarsh On 21-Mar-2011
        upnlGridMod.Update()
        upnlActionBtnTopMod.Update()
        upnlActionBtnMod.Update()
    End Sub
    Private Sub btnCloseMod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseMod.Click, btnCloseTopMod.Click
        MarkLog(Util.Action.Close, "Assembly Mod Status", " Model : " & mAssemblyStatus.ModelName & " Serial No. : " & mAssemblyStatus.Assembly.SerialNo, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionMod()
        'Response.Redirect(Request.QueryString("GChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
        TbContInst.ActiveTabIndex = 0
        TbContInst_ActiveTabChanged(Nothing, Nothing)
        upnlTabs.Update()
    End Sub
    Private Sub btnCloseParameter_Click(sender As Object, e As System.EventArgs) Handles btnCloseParameter.Click
        Session.Remove("mParameterList")
        TbContInst.ActiveTabIndex = 0
        TbContInst_ActiveTabChanged(Nothing, Nothing)
        upnlTabs.Update()
    End Sub
    Private Sub cmbLookInMod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookInMod.SelectedIndexChanged
        cmbSearchForMod.SelectedIndex = 0
        DisplayControlsMod(cmbLookInMod.SelectedIndex)
        If cmbLookInMod.Enabled = True Then
            cmbLookInMod.Focus()
        End If
    End Sub
#End Region

#Region " Report "
    'Created By:- Jyoti

#Region "Event"

    Private Sub btnPrintMod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintMod.Click, btnPrintTopMod.Click
        GridBindMod()
        Dim Rpt As New crListAssemblyMonitorStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Detail Section
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 4
        RHCount = Me.mAssemblyStatus.AssemblyStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            If mAssemblyStatus.AssemblyTypeID = 1 Then
                ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Manufacturer", _
                       Me.mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , "Since New Values as on " & (mAssemblyStatus.AsOnDateFormatted).ToString, _
                           "Periods", , _
                             , "Airframe"))
            Else
                ReportDetails.Add(New rptStatus(, 0, "Model and Serial No. of the " & mAssemblyStatus.AssemblyTypeName, "Manufacturer", _
                   Me.mAssemblyStatus.ManufacturerName, , , , , , , , , , , , , , , , , "Since New Values as on " & (mAssemblyStatus.AsOnDateFormatted).ToString, _
                   "Periods", "Engine", _
                     , "Airframe"))
            End If
        End If

        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Model", _
                                         Me.mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , , _
                                            CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                            CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                         , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Model", _
                               Me.mAssemblyStatus.ModelName, , , , , , , , , , , , , , , , , , _
                               "", "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Serial No.", _
                                          "", , , , , , , , , , , , , , , , , , _
                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                          , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Serial No.", _
                                          "", , , , , , , , , , , , , , , , , , _
                                          "", "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "Position", _
                                                         Me.mAssemblyStatus.Position, , , , , , , , , , , , , , , , , , _
                                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                                          CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                                          , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "Position", _
                                                          Me.mAssemblyStatus.Position, , , , , , , , , , , , , , , , , , _
                                                          "", "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, , "", _
                                           "", , , , , , , , , , , , , , , , , , _
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                           CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                           , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, , "", _
                                           "", , , , , , , , , , , , , , , , , , _
                                           "", "", , ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, , "", _
                                                    "", , , , , , , , , , , , , , , , , , _
                                                    CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), _
                                                    CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String), _
                                                 , CType(Me.mAssemblyStatus.AssemblyStatusPeriods(I).MachineCurrentValueFormatted, String)))
            End If
        Next

        'For Assembly Mod List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , lblModText.Text))

        'For Assembly Monitor Mod List
        'ReportDetails.Add(New rptStatus(, 2, _
        '     , , , , dgMonitorModStatusList.Columns.Item(2).HeaderText, , dgMonitorModStatusList.Columns.Item(3).HeaderText, dgMonitorModStatusList.Columns.Item(4).HeaderText, _
        '      dgMonitorModStatusList.Columns.Item(5).HeaderText, dgMonitorModStatusList.Columns.Item(6).HeaderText, dgMonitorModStatusList.Columns.Item(7).HeaderText, dgMonitorModStatusList.Columns.Item(8).HeaderText, , , , , , , , , , , , , , , dgMonitorModStatusList.Columns.Item(9).HeaderText))
        ReportDetails.Add(New rptStatus(, 2, _
             , , , , dgMonitorModStatusList.Columns.Item(3).HeaderText, dgMonitorModStatusList.Columns.Item(11).HeaderText, dgMonitorModStatusList.Columns.Item(4).HeaderText, dgMonitorModStatusList.Columns.Item(5).HeaderText, _
               dgMonitorModStatusList.Columns.Item(6).HeaderText, dgMonitorModStatusList.Columns.Item(7).HeaderText, dgMonitorModStatusList.Columns.Item(8).HeaderText, dgMonitorModStatusList.Columns.Item(9).HeaderText, , , , , , , , , , , , , , , dgMonitorModStatusList.Columns.Item(10).HeaderText))

        Dim TotalCount1 As Integer
        TotalCount1 = Me.mAssemblyMonitorModStatusList.Count
        Dim m As Integer

        Dim str(8) As String


        For m = 0 To TotalCount1 - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""
            str(8) = ""
            'Commented by Saylee on 1/04/2008 Suggested by Deven sir-------------------------------------
            'If Me.dgMonitorModStatusList.Rows(m).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgMonitorModStatusList.Rows(m).Cells(1).Text
            'If Me.dgMonitorModStatusList.Rows(m).Cells(2).Text <> "&nbsp;" Then str(1) = Me.dgMonitorModStatusList.Rows(m).Cells(2).Text
            'If Me.dgMonitorModStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(2) = Me.dgMonitorModStatusList.Rows(m).Cells(3).Text
            'If Me.dgMonitorModStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(3) = Me.dgMonitorModStatusList.Rows(m).Cells(4).Text
            'If Me.dgMonitorModStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(4) = Me.dgMonitorModStatusList.Rows(m).Cells(5).Text
            'If Me.dgMonitorModStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(5) = Me.dgMonitorModStatusList.Rows(m).Cells(6).Text
            'If Me.dgMonitorModStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(6) = Me.dgMonitorModStatusList.Rows(m).Cells(7).Text

            'code added by Saylee on 1/04/2008 Suggested by Deven sir-------------------------------------
            If Me.dgMonitorModStatusList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(0) = Me.dgMonitorModStatusList.Rows(m).Cells(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(1) = Me.dgMonitorModStatusList.Rows(m).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(2) = Me.dgMonitorModStatusList.Rows(m).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(3) = Me.dgMonitorModStatusList.Rows(m).Cells(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(4) = Me.dgMonitorModStatusList.Rows(m).Cells(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(8).Text <> "&nbsp;" Then str(5) = Me.dgMonitorModStatusList.Rows(m).Cells(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(9).Text <> "&nbsp;" Then str(6) = Me.dgMonitorModStatusList.Rows(m).Cells(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(10).Text <> "&nbsp;" Then str(7) = Me.dgMonitorModStatusList.Rows(m).Cells(10).Text.Replace("<BR>", vbCrLf)
            If Me.dgMonitorModStatusList.Rows(m).Cells(11).Text <> "&nbsp;" Then str(8) = Me.dgMonitorModStatusList.Rows(m).Cells(11).Text.Replace("<BR>", vbCrLf)
            '-------------------------------------------------------------------------------------
            ReportDetails.Add(New rptStatus(, 3, , _
                , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6), , , , , , , str(8), , str(7)))
        Next


        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Assembly Mod Status List Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If Me.mAssemblyMonitorModStatusList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim mrptimage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mrptimage)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "Assembly Monitor Mod Status", "Assembly Monitor Mod Status List Report", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region

#End Region

#Region " Parameter List "

#Region " Variable Declaration "
    Public mParameterList As ParameterList
#End Region

#Region " Business Methods "
    Private Sub GetSessionParameter()

        mParameterList = CType(Session("mParameterList"), ParameterList)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
    End Sub
    Private Sub SetSessionParameter()

        Session("mParameterList") = mParameterList
        Session("mAssemblyStatus") = mAssemblyStatus
    End Sub
    Private Sub RemoveSessionParameter()
        Session.Remove("mParameterList")
    End Sub
    Private Sub RemoveAllSessionValuesParameter()
        Session.Remove("mModelList")
        Session.Remove("mATAList")
        Session.Remove("Add")
        Session.Remove("Edit")
        Session.Remove("mAssemblyStatusList")
        Session.Remove("mAssemblyStatus")
        Session.Remove("mAssemblyTypeListForUI")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    Private Sub addAttributesParameters()
        txtMin.Attributes.Add("onKeyPress", "validateText(('ND'),document.getElementById('txtMin').value,event)")
        txtMax.Attributes.Add("onKeyPress", "validateText(('ND'),document.getElementById('txtMax').value,event)")
    End Sub
    Private Sub DataFieldBindParameters()
        mParameterList = ParameterList.GetParameterList("(SELECT)")
        cmbParameterList.DataSource = mParameterList
        Session("mParameterList") = mParameterList
        dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
        Session("mAssemblyStatus") = mAssemblyStatus
        upnlParameters.DataBind()
        txtMin.Text = ""
        txtMax.Text = ""
    End Sub
    Private Sub SetPageParameters()
        lblResultParameters.Text = "List of Parameters: " & mAssemblyStatus.AssemblyParameters.Count & " Record(s)."
    End Sub
    Private Sub ControlVisibilityParameters()
        ' dgParameterList.Columns(6).Visible = Not mMachine.AssemblyStatus.HasLogCount
    End Sub
    Private Sub EditRecordParameters(ByVal Index As Int32)
        mAssemblyStatus.AssemblyParameters.CurrentIndex = Index
        txtMin.Text = mAssemblyStatus.AssemblyParameters.Item(Index).MinValue
        txtMax.Text = mAssemblyStatus.AssemblyParameters.Item(Index).MaxValue
        cmbParameterList.SelectedValue = mAssemblyStatus.AssemblyParameters.Item(Index).ParameterID.ToString
        cmbParameterList.Enabled = False
        Session("mAssemblyStatus") = mAssemblyStatus
    End Sub
    Private Sub NewRecordParameter()
        Dim mParameter As Parameter
        mParameter = Parameter.NewParameter(Guid.NewGuid)
        Session("mParameter") = mParameter
    End Sub
    Private Sub MessageBoxResultParameter()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Dim ParameterIDForEventLog As Guid
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteParameters" Then
                        Try
                            Session("sender") = ""
                            MachineDetail = "Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Monitor Info :- " & "Description : " & mAssemblyStatus.AssemblyParameters(mAssemblyStatus.AssemblyParameters.CurrentIndex).ParameterDescription
                            mAssemblyStatus.AssemblyParameters.Remove(mAssemblyStatus.AssemblyParameters(mAssemblyStatus.AssemblyParameters.CurrentIndex))
                            dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
                            Session("mAssemblyStatus") = mAssemblyStatus
                            dgParameterList.DataBind()
                            upnlParameters.Update()
                            NewRecordParameter()
                            txtMin.Text = ""
                            txtMax.Text = ""
                            cmbParameterList.SelectedIndex = 0
                            SetPageParameters()
                            Session("mInstallAssemblyParametersEdit") = False
                            cmbParameterList.Enabled = True
                            upnlParameters.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Assembly Parameter Status", "Can't delete : " & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Description : " + mAssemblyStatus.AssemblyParameters(mAssemblyStatus.AssemblyParameters.CurrentIndex).ParameterDescription + " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "Assembly Parameter Status",  & " Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo + " Assembly Monitor Parameter Type : " + mAssemblyMonitorParameterStatusList.Item(mAssemblyMonitorParameterStatusList.CurrentIndex).MonitorType + " Description : " + mAssemblyMonitorParameterStatusList.Item(mAssemblyMonitorParameterStatusList.CurrentIndex).Description, Util.ErrorType.NoError, mAssemblyMonitorParameterStatusList.Item(mAssemblyMonitorParameterStatusList.CurrentIndex).ID, EventLogID)
                                MarkLog(Util.Action.Delete, "Assembly Parameter Status", MachineDetail, Util.ErrorType.NoError, ParameterIDForEventLog, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Not IsValid Then Exit Sub
        Dim ParameterID As New Guid(cmbParameterList.SelectedValue.ToString)
        ''If mAssemblyParameterList.Contains(ParameterID) = False Then
        ''    MarkLog(Util.Action.[New], "Assembly", " Parameter ->  " + cmbParameterList.SelectedItem.Text, Util.ErrorType.NoError, ParameterID)
        ''    'mAssemblyParameter = AssemblyParameter.NewChildAssemblyParameter(mAssemblyStatus.AssemblyID, New Guid(cmbParameterList.SelectedValue.ToString))
        ''    mAssemblyParameterList.Add(mAssemblyStatus.AssemblyID, New Guid(cmbParameterList.SelectedValue.ToString))
        ''    dgParameterList.DataSource = mAssemblyParameterList
        ''    dgParameterList.DataBind()

        ''    Session("mAssemblyParameter") = mAssemblyParameter
        ''    Session("mAssemblyParameterList") = mAssemblyParameterList
        ''    'Response.Redirect("wfAssemblyParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        ''Else
        ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Parameter already exists, can not be added.", MsgBoxStyle.OKOnly)
        ''    '   msg.ReplacePage = "wfAssemblyParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
        ''    msg.ReplacePage = "wfAssemblyParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1")

        ''    Session("sender") = "Delete"
        ''    msg.Show()
        ''End If

        If Session("mInstallAssemblyParametersEdit") = False Then
            If mAssemblyStatus.AssemblyParameters.Contains(ParameterID, mAssemblyStatus.AssemblyID) = False Then
                'Changed by Vikrant on 26-July-2011
                MarkLog(Util.Action.[New], "Assembly Installation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                'mAssemblyParameter = AssemblyParameter.NewChildAssemblyParameter(mAssemblyStatus.AssemblyID, New Guid(cmbParameterList.SelectedValue.ToString))
                mAssemblyStatus.AssemblyParameters.Add(mAssemblyStatus.AssemblyID, New Guid(cmbParameterList.SelectedValue.ToString), Val(txtMin.Text), Val(txtMax.Text)) '$$$$$$$$
                dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
                dgParameterList.DataBind()
                Session("mAssemblyStatus") = mAssemblyStatus

                ''Session("mAssemblyParameter") = mAssemblyParameter
                ''Session("mAssemblyParameterList") = mAssemblyParameterList
                'Response.Redirect("wfAssemblyParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Parameter already exists, can not be added.", MsgBoxStyle.OkOnly, "")
            End If

        Else
            mAssemblyStatus.AssemblyParameters.CurrentItem.MinValue = Val(txtMin.Text)
            mAssemblyStatus.AssemblyParameters.CurrentItem.MaxValue = Val(txtMax.Text)

            If mAssemblyStatus.AssemblyParameters.CurrentItem.IsDirty Then
                dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
                dgParameterList.DataBind()
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mInstallAssemblyParametersEdit") = False
            End If
        End If
        mParameterList = ParameterList.GetParameterList("(SELECT)")
        cmbParameterList.DataSource = mParameterList
        Session("mParameterList") = mParameterList
        cmbParameterList.DataBind()
        cmbParameterList.Enabled = True
        txtMin.Text = ""
        txtMax.Text = ""
        SetPageParameters()
        upnlParameters.Update()
    End Sub
    Private Sub dgParameterList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgParameterList.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgParameterList.PageIndex * dgParameterList.PageSize
                If (Not User.IsInRole("AssemblyStatusNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyStatusEdit") And Not mAssemblyStatus.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteParameters")
                mAssemblyStatus.AssemblyParameters.CurrentIndex = Index
                Session("mAssemblyStatus") = mAssemblyStatus
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgParameterList.PageIndex * dgParameterList.PageSize
                Session("mInstallAssemblyParametersEdit") = True
                EditRecordParameters(Index)
        End Select
    End Sub
    Private Sub imgbtnParameter1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnParameter1.Click
        NewRecordParameter()
        Session("mAssemblyStatus") = mAssemblyStatus  '$$$$$$$$$
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenParameterWindow", "OpenParameterWindow()", True)
        'Response.Redirect("wfParameter_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfInstallAssembly_Ajax.aspx")
    End Sub
    Private Sub dgParameterList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgParameterList.Sorting
        mAssemblyStatus.AssemblyParameters.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAssemblyStatus") = mAssemblyStatus
        dgParameterList.DataSource = mAssemblyStatus.AssemblyParameters
        dgParameterList.DataBind()
    End Sub
    Private Sub hdnBtnParameter_Click(sender As Object, e As System.EventArgs) Handles hdnBtnParameter.Click
        'DataFieldBindParameters()
        mParameterList = ParameterList.GetParameterList("(SELECT)")
        cmbParameterList.DataSource = mParameterList
        Session("mParameterList") = mParameterList
        cmbParameterList.DataBind()
        cmbParameterList.SelectedValue = mAssemblyStatus.AssemblyParameters.CurrentItem.ParameterID.ToString
        upnlParameters.Update()
    End Sub
#End Region

#End Region

#Region "Common Events "
    Private Sub TbContInst_ActiveTabChanged(sender As Object, e As System.EventArgs) Handles TbContInst.ActiveTabChanged
        'If Not Session("TabIndex") Is Nothing Then TbContInst.ActiveTabIndex = CType(Session("TabIndex"), Integer) : Session.Remove("TabIndex")
        Session("AssemblyInstTabIndex") = TbContInst.ActiveTabIndex
        Session("mIsSpareAssembly") = 1
        Select Case TbContInst.ActiveTabIndex
            Case 0
                DataBindGrid()
                ControlVisibility()
                SetPage()
                upnlActionBtn.Update()
                upnlATADetails.Update()
                upnlDocumentDetails.Update()

                upnlInstallationDetails.Update()
                upnlModelDetails.Update()
                upnlSinceNew.Update()
                upnlTitle.Update()
            Case 1
                lblTitle.Text = "Airframe Status of [New]"
                upnlTitle.Update()

                GetSessionComp()
                EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 1-Aug-2011 For All19072011
                addAttributesComp()

                If cmbLookInComponentList.Enabled = True Then
                    cmbLookInComponentList.Focus()
                End If
                DataFieldBindComp()
                SetControlsComp() 'Added By Saylee on 28-th-Jan-2008 for bug-Service List (SL3)

                SetPageComp()
                ControlVisibilityComp()
                SetRightsComp()

                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
            Case 2
                GetSessionService()
                addAttributesService()
                EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 22-July-2011

                If cmbLookInService.Enabled = True Then
                    cmbLookInService.Focus()
                End If

                DataFieldBindService()
                SetControlsService() 'Added By Saylee on 28-th-Jan-2008 for bug-Service List (SL3)
                FindNowService()
                SetPageService()
                ControlVisibilityService()
                SetRightsService()  'Added By Utkarsh On 14-Mar-2011

                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
            Case 3

                GetSessionInsp()
                addAttributesInsp()
                EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 22-July-2011

                If cmbLookInInsp.Enabled = True Then
                    cmbLookInInsp.Focus()
                End If
                DataFieldBindInsp()
                SetControlsInsp() 'Added By Saylee on 28-th-Jan-2008 for bug-Service List (SL3)
                FindNowInsp()
                SetPageInsp()
                ControlVisibilityInsp()
                SetRightsInsp()  'Added By Utkarsh On 14-Mar-2011

                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
            Case 4
                GetSessionMod()
                addAttributesMod()
                EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 22-July-2011

                If cmbLookInMod.Enabled = True Then
                    cmbLookInMod.Focus()
                End If
                DataFieldBindMod()
                SetControlsMod() 'Added By Saylee on 28-th-Jan-2008 for bug-Service List (SL3)
                FindNowMod()
                SetPageMod()
                ControlVisibilityMod()
                SetRightsMod()  'Added By Utkarsh On 14-Mar-2011

                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
            Case 5
                GetSessionParameter()
                addAttributesParameters()

                If cmbParameterList.Enabled = True Then
                    setFocus(cmbParameterList)
                End If
                DataFieldBindParameters()

                SetPageParameters()
                ControlVisibilityParameters()
                cmbParameterList.Enabled = True
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
        End Select
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetLicenceList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        'Dim itemlist As ItemListAutoComplete
        'itemlist = ItemListAutoComplete.GetItemList(prefixText, False)

        Dim mLicenses As LicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList(prefixText, "", , , False)
        If count = 0 Then
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).ToArray
        Else
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region


End Class