Imports System.IO
Imports System.Globalization

Public Class wfHangarPlanning
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mhangarlist As HangarList
    Public mhangar As Hanger
    Dim mFileAttach As FileAttach
    Dim mh As New HangarList.hangarlistinfo
    Dim mAirCraftMasterList As AirCraftMasterList
    Dim mHangerMasterList As HangerMasterList
    Public mDistinctGood As DistinctGood
    Public mDistinctAircraftListForHangar As DistinctAircraftListForHangar
    Public mHangarPlanningUniqueEntry As HangarPlanningUniqueEntry

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mFileAttach = Session("mFileAttach")
        mhangar = Session("mhanger")
        mHangerMasterList = CType(Session("mHangerMasterList"), HangerMasterList)
    End Sub
    Private Sub SetSession()
        Session("mFileAttach") = mFileAttach

    End Sub

    Private Sub ControlVisibilityForExpCalibration()
        If mhangar.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub

    Private Sub ControlVisibility()
        If Not mhangar.IsNew Then
            txtText.Enabled = False
            txtNo.Enabled = False
        End If

    End Sub
    Private Sub SetObject()

        mhangar.Text = txtText.Text
        mhangar.No = txtNo.Text
        mhangar.AirCraftID = New Guid(DropDownList2.SelectedValue)
        mhangar.HangarID = New Guid(DropDownList1.SelectedValue)
        mhangar.Hdatetimefrom = Txtdatetimefrom.Text
        mhangar.Hdatetimeto = Txtdatetimeto.Text
        mhangar.Hremark = Txtattach.Text
    End Sub
    Private Sub GetAttachment()

        If mhangar.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mhangar.HID)
            'Session("mFileAttach") = mFileAttach
        End If
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim DestinationName As String = String.Empty
                        Try
                            ' Dim mHangerMaster As HangerMaster
                            Session("sender") = ""
                            mhangar = CType(Session("Hanger"), Hanger)


                            mhangar.Delete()
                            mhangar.Save()
                            DataFieldBind()
                            ' SetControl()
                            ControlVisibilityForExpCalibration()

                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, "", MsgBoxStyle.OkOnly, "")
                            End If

                            msgCount = ex.Errors.Count
                        Finally

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
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub

    'Private Sub ViewImage()
    '    Dim No As New Random
    '    Dim StrName As String = "abc" & No.Next.ToString
    '    GetAttachment()
    '    If mFileAttach.Size > 0 Then
    '        Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
    '        Dim fs As FileStream
    '        If File.Exists(AppSettings("DOCPath")) = False Then
    '            'Delete File if exist
    '            System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
    '            ' Create the file.
    '            fs = File.Create(path)
    '            '' Add some information to the file.
    '            fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
    '            fs.Close()
    '            Session("DOCPath") = path
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
    '        End If
    '    End If
    'End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        If mhangar.IsAttachmentAdded = True Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mhangar.FileAttachments(0).Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mhangar.FileAttachments(0).Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mhangar.FileAttachments(0).ImageFile, 0, mhangar.FileAttachments(0).ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFilel();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel();", Str, True)
            End If
        End If
    End Sub
    '#Region " Show BrokenRules "
    '    Public Sub Customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
    '        Dim CustValid As CustomValidator
    '        CustValid = CType(s, CustomValidator)
    '        'If CustValid.ControlToValidate = "Txtdatetimefrom" Then
    '        '    If mhangar.Hdatetimefrom >= mhangar.Hdatetimeto Then
    '        '        e.IsValid = False
    '        '        CustValid.ErrorMessage = "From Date Should Be Greater Than To Date "
    '        '    Else
    '        '        e.IsValid = True
    '        '    End If
    '        'End If
    '        ' GetSession()
    '        SetObject()
    '        If CustValid.ControlToValidate = "Txtdatetimefrom" Then
    '            If IsDBNull(mhangar.Hdatetimeto) Then
    '                e.IsValid = True

    '            Else
    '                If mhangar.Hdatetimefrom >= mhangar.Hdatetimeto Then
    '                    e.IsValid = False
    '                    CustValid.ErrorMessage = "Hangar Planning Exists already Exists in this Date Periods "
    '                Else
    '                    e.IsValid = True
    '                End If
    '            End If
    '        End If


    '    End Sub
    '#End Region





#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'mAirCraftMasterList = AirCraftMasterList.GetHangarList(AddTopItem:="(SELECT)")
        'DropDownList2.DataSource = mAirCraftMasterList
        'DropDownList2.DataBind()
        mDistinctAircraftListForHangar = DistinctAircraftListForHangar.GetDistinctText("2", 0, True, AddTopItem:="(SELECT)")
        DropDownList2.DataSource = mDistinctAircraftListForHangar
        DropDownList2.DataBind()
        'mHangerMasterList = HangerMasterList.GetHangarList(AddTopItem:="(SELECT)")
        'DropDownList1.DataSource = mHangerMasterList
        'DropDownList1.DataBind()
        'mDistinctHangarListForHangar = DistinctHangarListForHangar.GetDistinctText("3", 0, True)
        'DropDownList1.DataSource = mDistinctHangarListForHangar
        'DropDownList1.DataBind()
        mDistinctGood = DistinctGood.GetDistinctText("3", 0, True, AddTopItem:="(SELECT)")
        DropDownList1.DataSource = mDistinctGood
        DropDownList1.DataBind()
        '' txtdatetime.Text = mhangar.HDateFormatted.ToString

        Txtdatetimefrom.Text = mhangar.HedatetimeromFormatted.ToString
        Txtdatetimeto.Text = mhangar.ToDateFormatted.ToString
        DataBind()
        'If mFileAttach Is Nothing Then
        '    If mhangar.IsAttachmentAdded = True Then
        '        mFileAttach = FileAttach.GetAttachment(mhangar.HID)
        '    Else
        '        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mhangar.HID)
        '    End If
        '    Session("mFileAttach") = mFileAttach
        'End If

    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'custValidator.ControlToValidate = "txtsearch"

        If custValidator.ControlToValidate = "txtText" Then
            'Dim dtf As Date = DateTime.ParseExact(Txtdatetimefrom.Text, AppSettings("DateTimeF"), CultureInfo.InvariantCulture)
            'Dim dtt As Date = DateTime.ParseExact(Txtdatetimeto.Text, AppSettings("DateTimeF"), CultureInfo.InvariantCulture)
            If IsDate(Txtdatetimefrom.Text) AndAlso IsDate(Txtdatetimeto.Text) Then
                If (DateAndTime.Hour(Txtdatetimefrom.Text) > 60 Or DateAndTime.Hour(Txtdatetimeto.Text) > 60) And (DateAndTime.Minute(Txtdatetimefrom.Text) > 60 Or DateAndTime.Minute(Txtdatetimeto.Text) > 60) Then
                    custValidator.ErrorMessage = "Enter proper date and time."
                    e.IsValid = False
                ElseIf CDate(Txtdatetimefrom.Text) > CDate(Txtdatetimeto.Text) Then
                    custValidator.ErrorMessage = "From Date Should be Less Than To Date.."
                    e.IsValid = False
                End If
            Else
                custValidator.ErrorMessage = "Enter proper date and time."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        upnlTitle.Update()
        If Not IsPostBack Then
            DataFieldBind()
            'Txtdatetimefrom.Text = DateTime.Today.Date.ToString(AppSettings("DateFormat"))
            'Txtdatetimeto.Text = DateTime.Today.Date.ToString(AppSettings("DateFormat"))
            upnlCityDetails.Update()
            ControlVisibilityForExpCalibration()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mhangar.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachmentChild(mhangar.HID)
        Else
            mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mhangar.HID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        'Dim fileSize1 As Integer = 0
        'Dim file1(fileSize1) As Byte
        'GetAttachment()
        'mhangar.AttachFile = file1
        'mhangar.Size = 0

        'ImageButton1.Visible = False
        'btnDelAttach.Enabled = False
        'mhangar.IsAttachmentAdded = False
        'mhangar.
        'Session("mhangar") = mhangar
        'ControlVisibilityForExpCalibration()
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        GetAttachment()

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        mhangar.IsAttachmentAdded = False
        mhangar.FileAttachments.Remove(mhangar.HID)
        Session("mhangar") = mhangar
    End Sub
    'Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
    '    If IsValid Then
    '        mhangar.Text = txtText.Text
    '        mhangar.No = txtNo.Text
    '        mhangar.AirCraftID = New Guid(DropDownList2.SelectedValue)
    '        mhangar.HangarID = New Guid(DropDownList1.SelectedValue)
    '        mhangar.Hdatetimefrom = Txtdatetimefrom.Text
    '        mhangar.Hdatetimeto = Txtdatetimeto.Text
    '        mhangar.Hremark = Txtattach.Text
    '        Session("mhangar") = mhangar         
    '        mhangar.Save()

    '        ControlVisibilityForExpCalibration()
    '        ' afte save call new object by doing newhanger function
    '        ' mhangar = Hanger.NewHangar()
    '        Dim mopenas As String = Request.QueryString("Type")
    '        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
    '            Exit Sub
    '        End If
    '    Else
    '        upnlTitle.Update()

    '    End If

    'End Sub
    'Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
    '    If IsValid Then
    '        mhangar.Text = txtText.Text
    '        mhangar.No = txtNo.Text
    '        mhangar.AirCraftID = New Guid(DropDownList2.SelectedValue)
    '        mhangar.HangarID = New Guid(DropDownList1.SelectedValue)
    '        mhangar.Hdatetimefrom = Txtdatetimefrom.Text
    '        mhangar.Hdatetimeto = Txtdatetimeto.Text
    '        mhangar.Hremark = Txtattach.Text
    '        Session("mhangar") = mhangar
    '        mhangar.Save()
    '        ControlVisibilityForExpCalibration()
    '        ' afte save call new object by doing newhanger function
    '        ' mhangar = Hanger.NewHangar()
    '        Dim mopenas As String = Request.QueryString("Type")
    '        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
    '            Exit Sub
    '        End If
    '    Else
    '        upnlTitle.Update()

    '    End If

    'End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        'SetObject()
        If IsValid Then

            mhangar.Text = txtText.Text
            mhangar.No = txtNo.Text
            mhangar.AirCraftID = New Guid(DropDownList2.SelectedValue)
            mhangar.HangarID = New Guid(DropDownList1.SelectedValue)
            mhangar.Hdatetimefrom = Txtdatetimefrom.Text
            mhangar.Hdatetimeto = Txtdatetimeto.Text
            mhangar.Hremark = Txtattach.Text
            Session("mhangar") = mhangar

            mHangarPlanningUniqueEntry = HangarPlanningUniqueEntry.GetUniqueAircraft(mhangar.Hdatetimefrom, mhangar.Hdatetimeto, mhangar.AirCraftID.ToString(), mhangar.HID.ToString())

            If mHangarPlanningUniqueEntry.ToDateUniqueEntry Is DBNull.Value Then

                'If IsDBNull(mHangarPlanningUniqueEntry.ToDateUniqueEntry) Then
                '    'do nothihg
                'Else
                '    If mHangarPlanningUniqueEntry.ToDateUniqueEntry <= mhangar.Hdatetimefrom Then
                '        mhangar.Save()
                '        ControlVisibilityForExpCalibration()
                '    Else
                '        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Record Already Exists in Hangar Planning so it cannot be Deleted", MsgBoxStyle.OkOnly, "")
                '    End If
                'End If

                mhangar.Save()
                upnlCityDetails.Update()
                ControlVisibilityForExpCalibration()
                ' afte save call new object by doing newhanger function
                ' mhangar = Hanger.NewHangar()
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Record Already Exists in Hangar Planning", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            upnlTitle.Update()
        Else
            upnlTitle.Update()

        End If
        'upnlTitle.Update()
    End Sub
    'Private Sub Messagebox(ByVal Message As String)
    '    Dim lblMessageBox As New Label()
    '    lblMessageBox.Text = "<script language='javascript'>" + Environment.NewLine & "window.alert('" & Message & "')</script>"
    '    Page.Controls.Add(lblMessageBox)
    'End Sub
    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button3.Click
        'close redirect wfHganerPlanning
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Protected Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles hdnBtnFileUpload.Click
        'onclick attach the file
        'mFileAttach = Session("mFileAttach")
        'mhangar.IsAttachmentAdded = True
        'ControlVisibilityForExpCalibration()
        'upnlAttachment.Update()
        If mhangar.IsAttachmentAdded Then
            mhangar.FileAttachments(0).Size = mFileAttach.Size
            mhangar.FileAttachments(0).ImageFile = mFileAttach.ImageFile
            mhangar.FileAttachments(0).Extension = mFileAttach.Extension
        Else
            mhangar.IsAttachmentAdded = True
            mhangar.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
        End If
        ControlVisibilityForExpCalibration()
        upnlAttachment.Update()
        'AttachMyFile()
    End Sub
    Private Sub AttachMyFile()
        ' prepare attachfile,size,fileextension object
        Try
            mhangar.AttachFile = CType(Session("FileUpload.FileContent"), Byte())
            mhangar.Size = Session("FileUpload.FileSize")
            mhangar.FileExtension = Session("FileUpload.FileExtension")
            Session("mhangar") = mhangar
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
            ControlVisibilityForExpCalibration()
        Catch ex As Exception
            MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try
    End Sub
    Protected Sub AddAirCraft_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AddAirCraft.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAircraftMasterWindow", "OpenAircraftMasterWindow();", True)
    End Sub
    Protected Sub AddHanger_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AddHanger.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenHangerMasterWindow", "OpenHangerMasterWindow();", True)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub hdnBtnAircraftMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnAircraftMaster.Click
        'mAirCraftMasterList = AirCraftMasterList.GetHangarList(AddTopItem:="(SELECT)")
        'DropDownList2.DataSource = mAirCraftMasterList
        'DropDownList2.DataBind()
        mDistinctAircraftListForHangar = DistinctAircraftListForHangar.GetDistinctText("2", 0, True, AddTopItem:="(SELECT)")
        DropDownList2.DataSource = mDistinctAircraftListForHangar
        DropDownList2.DataBind()
        upnlCityDetails.DataBind()
        upnlCityDetails.Update()
    End Sub
    Private Sub hdnBtnHangerMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnHangerMaster.Click
        'mAirCraftMasterList = AirCraftMasterList.GetHangarList(AddTopItem:="(SELECT)")
        'DropDownList2.DataSource = mAirCraftMasterList
        'DropDownList2.DataBind()
        mDistinctGood = DistinctGood.GetDistinctText("3", 0, True, AddTopItem:="(SELECT)")
        DropDownList1.DataSource = mDistinctGood
        DropDownList1.DataBind()
        upnlCityDetails.DataBind()
        upnlCityDetails.Update()
    End Sub
#End Region

    'Protected Sub Txtdatetimefrom_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Txtdatetimefrom.TextChanged
    '    If Not IsDate(Txtdatetimefrom.Text) Then
    '        Txtdatetimefrom.Text = ""
    '        upnlTitle.Update()
    '        upnlCityDetails.Update()

    '    End If
    'End Sub

    'Protected Sub Txtdatetimeto_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Txtdatetimeto.TextChanged
    '    If Not IsDate(Txtdatetimeto.Text) Then
    '        Txtdatetimeto.Text = ""
    '        upnlTitle.Update()
    '        upnlCityDetails.Update()

    '    End If
    'End Sub

End Class