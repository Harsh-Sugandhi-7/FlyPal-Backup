'Added By Vikrant On 20-Aug-2015
Imports System.Collections.Generic
Imports System.Text
Public Class wfAuditSchedule_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mAuditSchedule As AuditSchedule
    Protected mAuditScheduleList As AuditScheduleList
    Protected mAuditTypeList As AuditTypeList
    Public strMsg As String = ""

    Protected mAuditExecutionList As AuditExecutionList
    Public mResponsibleDepartmentList As EmployeeDepartmentList
    Public mAuditOnList As AuditOnList
    Public mMachineNameValueList As MachineNameValueList
    Public mStoreList As StoreList
    Public mVendorList As VendorList
    Public mLocationList As LocationList
    Public mAuditOnDepartmentList As AuditDepartmentList
    'Added by Vikrant on 22-July-2011
    Dim EventLogID As Guid
    Dim mScheduleDetail As String
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mAuditSchedule = Session("mAuditSchedule")
        mAuditScheduleList = Session("mAuditScheduleList")
        mAuditTypeList = Session("mAuditTypeList")
        mAuditExecutionList = Session("mAuditExecutionList")
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mResponsibleDepartmentList = Session("mResponsibleDepartmentList")
        mAuditOnList = Session("mAuditOnList")
    End Sub
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        SaveFormToObject()
        If mAuditSchedule.IsValid = False Then
            For i As Integer = 0 To mAuditSchedule.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mAuditSchedule.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
       
        If strMsg.Trim <> "" Then
            cvFrequency.ErrorMessage = strMsg
            cvFrequency.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub SaveFormToObject()
        Try
            If txtScheduleDate.text <> "" Then
                mAuditSchedule.ScheduleDate = CDate(txtScheduleDate.Text)
            Else
                mAuditSchedule.ScheduleDate = System.DBNull.Value
            End If

            mAuditSchedule.Location = txtLocation.Text

            If txtNextAuditDate.Text <> "" Then
                mAuditSchedule.NextAuditDate = CDate(txtNextAuditDate.Text)
            Else
                mAuditSchedule.NextAuditDate = System.DBNull.Value
            End If
            mAuditSchedule.Note = txtNote.Text
            mAuditSchedule.OtherInformation = Trim(txtOtherInformation.Text)
            mAuditSchedule.DepartmentID = New Guid(cmbDepartmentList.SelectedValue.ToString)
            mAuditSchedule.AuditText = txtAuditNo.Text
            mAuditSchedule.ToMailID = Trim(txtToMailID.Text)
            mAuditSchedule.CCMailID = Trim(txtCCMailID.Text)
            mAuditSchedule.AuditOnID = CInt(cmbAuditOnList.SelectedValue)
            mAuditSchedule.AircraftID = New Guid(cmbAircraft.SelectedValue)
            mAuditSchedule.AuditOnDepartmentID = New Guid(cmbAuditOnDepartment.SelectedValue)
            mAuditSchedule.LocationID = New Guid(cmbLocation.SelectedValue)
            mAuditSchedule.StoreID = New Guid(cmbStore.SelectedValue)
            mAuditSchedule.VendorID = New Guid(cmbVendor.SelectedValue)
            mAuditSchedule.AuditOnText = Trim(txtAuditOn.Text)

            mAuditSchedule.NotInUse = chkNotInUse.Checked

            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    mAuditSchedule.IsAttachmentAdded = True
                Else
                    mAuditSchedule.IsAttachmentAdded = False
                End If
            End If
            Session("mAuditSchedule") = mAuditSchedule
        Catch ex As Exception

        End Try
    End Sub
    Private Sub SetAuditOnCombos()
        cmbAircraft.Visible = IIf(cmbAuditOnList.SelectedIndex = 1, True, False)
        cmbAuditOnDepartment.Visible = IIf(cmbAuditOnList.SelectedIndex = 2, True, False)
        cmbLocation.Visible = IIf(cmbAuditOnList.SelectedIndex = 3, True, False)
        cmbStore.Visible = IIf(cmbAuditOnList.SelectedIndex = 4, True, False)
        cmbVendor.Visible = IIf(cmbAuditOnList.SelectedIndex = 5, True, False)
        txtAuditOn.Visible = IIf(cmbAuditOnList.SelectedIndex = 6, True, False)
    End Sub
    Private Sub SetPage()
        If Not mAuditSchedule.IsNew Then
            lblTitle.Text = "Audit Schedule [" + CType(mAuditSchedule.AuditNo, String) + "]"
        Else
            lblTitle.Text = "Audit Schedule [New]"
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString

        If mAuditSchedule.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAuditSchedule.ID)
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
    Private Sub ControlVisibilityForFileAttachment()
        If mAuditSchedule.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = IIf(mAuditSchedule.IsComplied, False, True)
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub Save()
        Try
            mAuditSchedule = Session("mAuditSchedule")

            If mAuditSchedule.IsValid Then
                mAuditSchedule.ApplyEdit()
                mAuditSchedule = mAuditSchedule.Save()
                'Added by Vikrant on 22-July-2011
                mScheduleDetail = "Audit No. :" + mAuditSchedule.AuditNo + " Dated : " + mAuditSchedule.ScheduleDateFormatted
                MarkLog(Util.Action.Save, "Audit Schedule", mScheduleDetail, Util.ErrorType.NoError, mAuditSchedule.ID, EventLogID)

                Session("mAuditSchedule") = mAuditSchedule
                Session("mAuditScheduleList") = mAuditScheduleList
                SetPage()

                DataBind()
            End If
        Catch ex As SqlException
            If ex.Number = 2601 Then
                MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the duplicate entry.", "You can not add duplicate entry in Calibration.", MsgBoxStyle.OkOnly, "")
                Session("ex.Number") = "ex.Number"
            End If
        End Try
       
    End Sub
    Private Sub SetReport()
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter

        Dim dsAuditScheduleDetail As New dsAuditScheduleDetail

        myReport = New crptAuditScheduleDetail

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "Audit Schedule Report", "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(dsAuditScheduleDetail)

        mAuditSchedule = AuditSchedule.GetAuditSchedule(mAuditSchedule.ID)
        da.Fill(dsAuditScheduleDetail, mAuditSchedule)
        da.Fill(dsAuditScheduleDetail, mAuditSchedule.AuditScheduleTasks)
        da.Fill(dsAuditScheduleDetail, Report)
        da.Fill(dsAuditScheduleDetail, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(dsAuditScheduleDetail)
        Session("CrystalReport") = myReport

        If mAuditSchedule.IsAttachmentAdded Then
            Dim PDFNo As Integer = 1
            Dim PDFNoChild As Integer = 1
            Dim tmp As Integer
            Dim a As New Random

            tmp = a.Next

            'Dim MyFile1 = "C:\Temp\" & tmp & PDFNo.ToString & ".pdf"
            Dim MyFile1 = "C:\Temp\" & "AuditDetail" & tmp & PDFNo.ToString & ".pdf"

            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions


            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()

            Dim pageCount As Integer = 0

            Dim pdfList As New System.Collections.ArrayList

            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1

            Dim mTempFileAttachment As FileAttach = FileAttach.GetAttachmentChild(mAuditSchedule.ID)
            If mTempFileAttachment.Size > 0 And mTempFileAttachment.Extension = ".pdf" Then
                Dim ChildAttachment_path As String = "C:\Temp\" & "AuditDetail" & PDFNoChild.ToString & mTempFileAttachment.Extension

                Dim fs As FileStream
                If File.Exists("C:\Temp\") = False Then
                    System.IO.File.Delete(ChildAttachment_path)
                    fs = File.Create(ChildAttachment_path)
                    fs.Write(mTempFileAttachment.ImageFile, 0, mTempFileAttachment.ImageFile.Length)
                    fs.Close()

                    pdfList.Add(ChildAttachment_path)                               '2. TaskCardAttachment attachment
                    PDFNo = PDFNo + 1
                    PDFNoChild = PDFNoChild + 1
                End If
            End If

            ' //********************************************Send Files for Merging****************************************************//
            Dim MergedPath As String = "C:\Temp\" & mAuditSchedule.AuditText.Replace("\", " ").Replace("/", " ") & ".pdf"
            Dim MergedPath_WM As String = "C:\Temp\" & "temp_myMergedPdf_WM.pdf"

            Dim filesByte As New List(Of Byte())()
            For Each file__1 As String In pdfList 'files
                filesByte.Add(File.ReadAllBytes(file__1))
            Next

            File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))

            '''AddWatermarkText(MergedPath, MergedPath_WM, mnWO.WONumber, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount)
            ''//********************************************Set Sessions*********************************************************//
            Session("CrystalReport") = MergedPath

            Dim DeleteThis As String = "AuditDetail"
            Dim Files As String() = Directory.GetFiles("C:\Temp\")

            For Each file__1 As String In Files
                If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
                    File.Delete(file__1)
                End If
            Next
            Session("PrintReportWithAttachment") = "True"
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
                If (Not mAuditSchedule.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mAuditSchedule.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
       
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteAuditScheduleTask" Then
                        Try
                            Session("Sender") = ""
                            mAuditSchedule = CType(Session("mAuditSchedule"), AuditSchedule)
                            mAuditSchedule.AuditScheduleTasks.Remove(mAuditSchedule.AuditScheduleTasks.CurrentItem)
                            Session("mAuditSchedule") = mAuditSchedule
                            dgAuditScheduleTask.DataSource = mAuditSchedule.AuditScheduleTasks
                            dgAuditScheduleTask.DataBind()
                            upnlAuditScheduleTasks.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2601 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                        '----------------Added By Prashant 3-March-2010
                    ElseIf MSGBoxCtrl.Sender = "Close" Then
                        Session("sender") = ""
                        If (Not User.IsInRole("AuditScheduleNew") And Not User.IsInRole("AuditScheduleEdit")) Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                        Page.Validate()
                        If Not Page.IsValid Then
                            upnlValidationSummary.Update()
                            Exit Sub
                        End If
                        If mAuditSchedule.IsValid Then
                            DataFieldBind()
                            'If Save() Then
                            mAuditSchedule = Session("mAuditExecution")
                            SaveFormToObject()
                            Save()
                            SaveAttachment()
                            Session("mAuditSchedule") = mAuditSchedule
                            Session.Remove("mAuditSchedule")
                            Session.Remove("mFileAttach")
                            Session.Remove("IsAttachmentDeleted")
                            DataFieldBind()
                            ContolVisibility()
                            SetPage()
                            upnlTitle.Update()
                            upnlAuditScheduleTasks.Update()
                            upnlAuditScheduleDetail.Update()
                            Response.Redirect(Request.QueryString("BackPage"))
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationSummary.Update()
                                Exit Sub
                            End If


                        End If
                        '----------------------------------------------
                    End If

                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        If mAuditSchedule.IsNew Then Session.Remove("mAuditSchedule")
                        mAuditSchedule = Session("mAuditSchedule")
                        SaveFormToObject()
                        Session("mAuditSchedule") = mAuditSchedule
                        Session.Remove("mAuditSchedule")
                        Session.Remove("mFileAttach")
                        Session.Remove("IsAttachmentDeleted")
                        Response.Redirect(Request.QueryString("BackPage"))
                    Else
                        Session("sender") = ""
                    End If

                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
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
    Private Sub ContolVisibility()
        If mAuditExecutionList.Contains(mAuditSchedule.ID) Then
            txtScheduleDate.Enabled = False
            dgAuditScheduleTask.Columns(7).Visible = False
        End If
        'chkNotInUse.Enabled = IIf(mAuditSchedule.IsComplied, False, True)
        ControlVisibilityForFileAttachment()
        SetAuditOnCombos()
    End Sub
    Private Sub DeleteAuditScheduleTask(ByVal index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Remove, MSGBox.Message_text.Remove, "", MsgBoxStyle.YesNo, "DeleteAuditScheduleTask")
        mAuditSchedule.AuditScheduleTasks.CurrentIndex = index
        Session("mAuditSchedule") = mAuditSchedule
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbAuditOnList" Then
            If cmbAuditOnList.SelectedIndex = 1 Then
                If cmbAircraft.SelectedIndex <= 0 Then
                    custValidator.ErrorMessage = "Please select Aircraft from list"
                    e.IsValid = False
                End If
            ElseIf cmbAuditOnList.SelectedIndex = 2 Then
                If cmbAuditOnDepartment.SelectedIndex <= 0 Then
                    custValidator.ErrorMessage = "Please select Department from list"
                    e.IsValid = False
                End If
            ElseIf cmbAuditOnList.SelectedIndex = 3 Then
                If cmbLocation.SelectedIndex <= 0 Then
                    custValidator.ErrorMessage = "Please select Location from list"
                    e.IsValid = False
                End If
            ElseIf cmbAuditOnList.SelectedIndex = 4 Then
                If cmbStore.SelectedIndex <= 0 Then
                    custValidator.ErrorMessage = "Please select Store from list"
                    e.IsValid = False
                End If
            ElseIf cmbAuditOnList.SelectedIndex = 5 Then
                If cmbVendor.SelectedIndex <= 0 Then
                    custValidator.ErrorMessage = "Please select Vendor from list"
                    e.IsValid = False
                End If
            ElseIf cmbAuditOnList.SelectedIndex = 6 Then
                If Trim(txtAuditOn.Text) = "" Then
                    custValidator.ErrorMessage = "Please enter Audit On text"
                    e.IsValid = False
                End If
            End If

        End If
    End Sub
    Private Sub ClearCombos()
        cmbAircraft.ClearSelection()
        cmbVendor.ClearSelection()
        cmbAuditOnDepartment.ClearSelection()
        cmbLocation.ClearSelection()
        cmbStore.ClearSelection()
        txtAuditOn.Text = ""
    End Sub
#End Region

#Region " DataFieldBind "
    Private Sub DataFieldBind()
        mAuditTypeList = AuditTypeList.GetAuditTypeList("(SELECT)")
        cmbAuditTypeList.DataSource = mAuditTypeList
        Session("mAuditTypeList") = mAuditTypeList

        txtScheduleDate.Text = mAuditSchedule.ScheduleDateFormatted.ToString
        txtNextAuditDate.Text = mAuditSchedule.NextAuditDateFormatted.ToString
        dgAuditScheduleTask.DataSource = mAuditSchedule.AuditScheduleTasks

        mAuditExecutionList = AuditExecutionList.GetAuditExecutionList("")
        Session("mAuditExecutionList") = mAuditExecutionList

        mResponsibleDepartmentList = EmployeeDepartmentList.GetEmployeeDepartmentList("(SELECT)", False)
        cmbDepartmentList.DataSource = mResponsibleDepartmentList
        cmbAuditOnDepartment.DataSource = mResponsibleDepartmentList
        Session("mResponsibleDepartmentList") = mResponsibleDepartmentList

        mAuditOnList = AuditOnList.GetAuditOnList("(SELECT)")
        cmbAuditOnList.DataSource = mAuditOnList
        Session("mAuditOnList") = mAuditOnList

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, IsTagRequired:=True, TagText:="(SELECT)", SkipIsForInventoryAircarft:=True, ForInventory:=True)  ''ForInventory set True by Saylee on 20-Sep-2022 as we need to show all Aircrafts irrespective of user rights
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList

        mStoreList = StoreList.GetStoreList(0, "", "(SELECT)")
        cmbStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList

        mLocationList = LocationList.GetLocationList(0, IsSelectTagRequired:=True)
        cmbLocation.DataSource = mLocationList
        Session("mLocationList") = mLocationList

        mVendorList = VendorList.GetVendortList(0, IsSelectTagRequired:=True)
        cmbVendor.DataSource = mVendorList
        Session("mVendorList") = mVendorList

        DataBind()
    End Sub
    

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)      'Added by Vikrant on 22-July-2011
        If Not Page.IsPostBack Then
            txtLocation.Focus()
            DataFieldBind()
            ContolVisibility()
            SetPage()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If (Not User.IsInRole("AuditScheduleNew") And Not User.IsInRole("AuditScheduleEdit")) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            If Not IsValid Then
                upnlValidationSummary.Update()
                Exit Sub
            End If

            mAuditSchedule = Session("mAuditSchedule")
            SaveFormToObject()

            If mAuditSchedule.IsValid Then
                mAuditSchedule.ApplyEdit()
                mAuditSchedule = mAuditSchedule.Save()
                SaveAttachment()
                'Added by Vikrant on 22-July-2011
                mScheduleDetail = "Audit No. :" + mAuditSchedule.AuditNo + " Dated : " + mAuditSchedule.ScheduleDateFormatted
                MarkLog(Util.Action.Save, "Audit Schedule", mScheduleDetail, Util.ErrorType.NoError, mAuditSchedule.ID, EventLogID)
                'End
                Session("mAuditSchedule") = mAuditSchedule
                Session("mAuditScheduleList") = mAuditScheduleList
                SetPage()
                dgAuditScheduleTask.DataSource = mAuditSchedule.AuditScheduleTasks
                DataBind()
                upnlTitle.Update()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            Else
                If Not mAuditSchedule.IsValid Then
                    For j As Integer = 0 To mAuditSchedule.GetBrokenRulesCollection.Count - 1
                        strMsg = strMsg + mAuditSchedule.GetBrokenRulesCollection(j).Description + "<BR>"
                    Next
                End If

                If strMsg.Trim <> "" Then
                    cvFrequency.ErrorMessage = strMsg
                    cvFrequency.IsValid = mAuditSchedule.IsValid
                End If
                upnlValidationSummary.Update()
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "Audit Schedule", MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "Audit Schedule", MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "Audit Schedule", MsgBoxStyle.OkOnly, "")
            End If
        End Try
    End Sub
    Private Sub txtFrequency_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFrequency.TextChanged
        txtNextAuditDate.Text = DateAdd(DateInterval.Month, Val(txtFrequency.Text), CDate(txtScheduleDate.Text))
    End Sub
    Private Sub txtScheduleDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtScheduleDate.TextChanged
        If (chkIsScheduleNextAudit.Checked) And (txtScheduleDate.Text <> "") Then
            txtNextAuditDate.Text = DateAdd(DateInterval.Month, Val(txtFrequency.Text), CDate(txtScheduleDate.Text)).ToString(AppSettings("DateFormat"))
        End If
    End Sub
    Private Sub btnAddTask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTask.Click
        If (Not User.IsInRole("AuditScheduleNew") And Not User.IsInRole("AuditScheduleEdit")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'If IsValid Then
        SaveFormToObject()
        Session("mAuditSchedule") = mAuditSchedule
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskWindow", "OpenTaskWindow()", True)
        'End If
    End Sub
    Private Sub dgAuditScheduleTask_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAuditScheduleTask.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgAuditScheduleTask.PageIndex * dgAuditScheduleTask.PageSize
                Session("Edit") = True
                SaveFormToObject()
                mAuditSchedule.AuditScheduleTasks.CurrentIndex = Index
                Session("mAuditSchedule") = mAuditSchedule
                Response.Redirect("wfAuditScheduleTask_Ajax.aspx?BackPage1=wfAuditSchedule_Ajax.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
            Case "Remove"
                ' Dim Index As Int32 = CInt(e.CommandArgument) + dgAuditScheduleTask.PageIndex * dgAuditScheduleTask.PageSize
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 29-09-2023
                Dim Index As Int32 = gvr.RowIndex
                DeleteAuditScheduleTask(Index)
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SaveFormToObject()
        If mAuditSchedule.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            mAuditSchedule = Session("mAuditSchedule")
            SaveFormToObject()
            Session("mAuditSchedule") = mAuditSchedule
            Session.Remove("mAuditSchedule")
            Session.Remove("SearchIndex")
            Session.Remove("AuditTypeID")
            Session.Remove("AuditSearchText")
            Session.Remove("mFileAttach")
            Session.Remove("IsAttachmentDeleted")

            'Changed by Vikrant on 22-July-2011
            MarkLog(Util.Action.Close, "Audit Schedule", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            Response.Redirect(Request.QueryString("BackPage"))
        End If
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub dgAuditScheduleTask_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAuditScheduleTask.Sorting
        mAuditSchedule.AuditScheduleTasks.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAuditSchedule") = mAuditSchedule
        dgAuditScheduleTask.DataSource = mAuditSchedule.AuditScheduleTasks
        dgAuditScheduleTask.DataBind()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mAuditSchedule.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAuditSchedule.ID)
            Session("mFileAttach") = mFileAttach
        End If
        'mEmployee.ImageFile = file1
        'mEmployee.ImageSize = 0
        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mAuditSchedule.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(sender As Object, e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mAuditSchedule.IsAttachmentAdded = True
        ControlVisibilityForFileAttachment()
        upnlAttach.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mAuditSchedule.IsAttachmentAdded = True Then
            mFileAttach = FileAttach.GetAttachment(mAuditSchedule.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAuditSchedule.ID)
        End If
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    End Sub
    Private Sub hdnimgBtnTaskMaster_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnTaskMaster.Click
        dgAuditScheduleTask.DataSource = mAuditSchedule.AuditScheduleTasks
        dgAuditScheduleTask.DataBind()
        upnlAuditScheduleTasks.Update()
    End Sub
    Private Sub cmbAuditOnList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAuditOnList.SelectedIndexChanged
        SetAuditOnCombos()
        ClearCombos()
    End Sub
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If (Not User.IsInRole("AuditSchedulePrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        SetReport()
        
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnSendMail_Click(sender As Object, e As System.EventArgs) Handles btnSendMail.Click
        If (Not User.IsInRole("AuditScheduleNew") And Not User.IsInRole("AuditScheduleEdit")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If (mAuditSchedule.ToMailID.Trim = "") And (mAuditSchedule.CCMailID.Trim = "") Then
            MSGBoxCtrl.show("Error", "Please enter at least one To MailID or CC MailID", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If Not IsValid Then
            upnlValidationSummary.Update()
            Exit Sub
        End If

        If mAuditSchedule.IsValid Then
            SetReport()
            Dim str As New StringBuilder
            str.Append("Audit Schedule Details are as follows: ")
            str.Append("<p><b>Audit No.: </b> " & mAuditSchedule.AuditText & "</p>")
            str.Append("<p><b>Schedule Date: </b> " & mAuditSchedule.ScheduleDateFormatted & "</p>")
            str.Append("<p><b>Responsible Department: </b> " & mAuditSchedule.DepartmentName & "</p>")
            str.Append("<p><b>Audit On: </b> " & mAuditSchedule.AuditOnCostCenter & "</p>")

            Try
                If mAuditSchedule.IsAttachmentAdded Then
                    SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Audit Schedule Details", mAuditSchedule.AuditNo.Replace("/", "").Replace("\", ""), Info:=str.ToString, VendorEmailID:="", ToMailID:=mAuditSchedule.ToMailID, CCMailID:=mAuditSchedule.CCMailID, ReportPath:=Session("CrystalReport"), Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
                Else
                    SendMailFile.SendMailFile(Session("CrystalReport"), User.Identity.Name, "Audit Schedule Details", mAuditSchedule.AuditNo.Replace("/", "").Replace("\", ""), Info:=str.ToString, VendorEmailID:="", ToMailID:=mAuditSchedule.ToMailID, CCMailID:=mAuditSchedule.CCMailID, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
                End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", MessageBox.Show("Mail Sent Successfully", False), True)
            Catch ex As Exception
                Dim Title As String = "Error Sending Mail"
                Dim Message As String = ex.InnerException.ToString
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", MessageBox.Show(Title, Message, , False), True)
                Exit Sub
            End Try
        Else
            If Not mAuditSchedule.IsValid Then
                For j As Integer = 0 To mAuditSchedule.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mAuditSchedule.GetBrokenRulesCollection(j).Description + "<BR>"
                Next
            End If

            If strMsg.Trim <> "" Then
                cvFrequency.ErrorMessage = strMsg
                cvFrequency.IsValid = mAuditSchedule.IsValid
            End If
            upnlValidationSummary.Update()
        End If
       
    End Sub
#End Region

    

    
End Class