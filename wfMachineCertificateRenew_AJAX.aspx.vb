Imports System.Linq
Imports System.Collections.Generic
Imports System.Web.Script.Serialization
Imports System.Text

'AJAX Conversion By Saylee On 8-Oct-2014

Public Class wfMachineCertificateRenew_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mRenewMachineCertificate As MachineCertificate
    Public mMachineCertificate As MachineCertificate

    Public mBoardInfo As AircraftInformationBoard.BoardInfo 'Added by Saylee on 22-May-2009
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Dim mMachineCertificateDetails As String
    Dim mModuleList As ModuleList    'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mRenewMachineCertificate = CType(Session("mRenewMachineCertificate"), MachineCertificate)
        mMachineCertificate = CType(Session("mMachineCertificate"), MachineCertificate)
        mBoardInfo = Session("mBoardInfo") 'Added by Saylee on 22-May-2009
        mModuleList = Session("mModuleList")    'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mRenewMachineCertificate") = mRenewMachineCertificate
        Session("mMachineCertificate") = mMachineCertificate

        Session("mBoardInfo") = mBoardInfo 'Added by Saylee on 22-May-2009
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mRenewMachineCertificate")
        Session.Remove("mMachineCertificate")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus(); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
           Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        Save()
                        GetSession()
                        DataFieldBind()
                        SetPage()
                        ControlVisibility()
                        upnlValidationsummary.Update()
                        upnlDetails.Update()
                        'Dim mopenas As String = Request.QueryString("Type")
                        'If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                        '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                        '    Exit Sub
                        'End If
                        'End
                        'Response.Redirect("wfMachineCertificateRenew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    ElseIf MSGBoxCtrl.Sender = "SaveBackDated" Then
                        Session("sender") = ""
                        SaveBackDated()
                        GetSession()
                        DataFieldBind()
                        SetPage()
                        ControlVisibility()
                        upnlValidationsummary.Update()
                        upnlDetails.Update()

                        'Dim mopenas As String = Request.QueryString("Type")
                        'If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                        '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                        '    Exit Sub
                        'End If
                        'End

                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Save" Or MSGBoxCtrl.Sender = "SaveBackDated" Then
                        Session("sender") = ""

                        DataFieldBind()
                        SetPage()
                        ControlVisibility()
                        upnlValidationsummary.Update()
                        upnlDetails.Update()

                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                        'End
                        'Response.Redirect("wfMachineCertificateRenew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    End If
                Case MsgBoxResult.Cancel
                    If MSGBoxCtrl.Sender = "Save" Then
                        Session("sender") = ""
                        'Response.Redirect("wfMachineCertificateRenew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    End If
                Case MsgBoxResult.Ok ''And Session("sender") = ""        
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfMachineCertificateRenew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfMachineCertificateRenew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            ' Response.Redirect("wfMachineCertificateRenew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
        ElseIf Result1 = 0 Then
            ' Session("sender") = ""

        End If
    End Sub
    Private Sub SetPage()
        lblTitle.Text = "Certificates  [" & mRenewMachineCertificate.CertificateName & "]"
    End Sub
    Private Sub ControlVisibility()
        'enabledisable buttons
        'btnAdd.Enabled = Not mMachine.AssemblyStatus.HasLogCount
        'dgCertificateList.Columns(2).Visible = Not mMachine.AssemblyStatus.HasLogCount
        'Added By Vikrant On 13-May-2013 For All13052013
        If mRenewMachineCertificate.IsNew Then
            txtNo.ReadOnly = False
        Else
            txtNo.ReadOnly = True
            txtNo.BackColor = System.Drawing.Color.Gainsboro
        End If
        'End
    End Sub
    Private Sub SetObject()

        mRenewMachineCertificate = Session("mRenewMachineCertificate")
        mMachineCertificate = Session("mMachineCertificate")
        With mRenewMachineCertificate
            .CertificateName = txtName.Text.Trim
            .CertificateNo = txtNo.Text
            .IssueDate = txtIssueDate.Text
            .ExpiryDate = txtExpiryDate.Text
            .IsApplicable = chkApplicable.Checked
            .Remark = txtRemark.Text
            .ReferenceID = mMachineCertificate.ID
            .IsDone = False
            .OneTimeCertificate = chkOneTimeCertificate.Checked
            .WarningDays = txtWarningDays.Text.Trim
            If txtEffectiveDate.Text = "" Then
                .EffectiveDate = System.DBNull.Value
            Else
                .EffectiveDate = txtEffectiveDate.Text 'Added By Prashant 19-Jun-2020 ALL18062020-1
            End If
        End With

        With mMachineCertificate
            .IsDone = True
        End With
        Session("mRenewMachineCertificate") = mRenewMachineCertificate
        Session("mMachineCertificate") = mMachineCertificate
    End Sub
    Private Sub AttachMyFile()
        mRenewMachineCertificate = Session("mRenewMachineCertificate")

        Try
            mRenewMachineCertificate.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mRenewMachineCertificate.ImageSize = Session("FileUpload.FileSize")
            mRenewMachineCertificate.FileExtension = Session("FileUpload.FileExtension")
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
            If mRenewMachineCertificate.ImageSize > 0 Then
                ImageButton1.Visible = True
                btnDelAttach.Enabled = True
            Else
                ImageButton1.Visible = False
                btnDelAttach.Enabled = False
            End If
            upnlAttach.Update()

        Catch ex As Exception
            MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try


        Session("mRenewMachineCertificate") = mRenewMachineCertificate
        Session("mMachineCertificate") = mMachineCertificate
    End Sub
    Private Sub SaveBoardInfo() 'Added by Saylee on 22-May-2009
        mBoardInfo = Session("mBoardInfo")
        If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
            mBoardInfo.MonitorID = mRenewMachineCertificate.ID
            mBoardInfo.DueOnValue = mRenewMachineCertificate.ExpiryDateFormatted.ToString
            mBoardInfo.ApplyEdit()
            mBoardInfo.Save()
            Session("mBoardInfo") = mBoardInfo
            Session("mAircraftInformationBoardList") = Nothing
        End If
    End Sub
    Private Sub SaveDailyStatus()
        Dim mAircraftDSCList As DailyStatusList
        Dim mAircraftDSC As DailyStatus

        mAircraftDSCList = DailyStatusList.GetDailyStatusList(mMachineCertificate.MachineID, Guid.Empty.ToString, Guid.Empty.ToString, 7, True)
        If mAircraftDSCList.Contains(mMachineCertificate.ID, "") Then
            mAircraftDSC = DailyStatus.GetChildDailyStatus(mAircraftDSCList(mMachineCertificate.ID, mMachineCertificate.MachineID).ID)
            mAircraftDSC.ModelPartMonitorID = mRenewMachineCertificate.ID
            mAircraftDSC.Save()
        End If
    End Sub
    'Added By Vikrant On 29-Oct-2021
    Public Sub SendPUSHNotification(ByVal tmpMachineCertificate As MachineCertificate)
        Dim PreviousStepStatus As Boolean = False

        'Step # 1: Get User Devices
        Dim mUserDeviceList As APP_UserDeviceList = APP_UserDeviceList.GetUserDeviceList(7) '7:Aircraft Certificate

        If mUserDeviceList.Count = 0 Then
            PreviousStepStatus = False
        Else
            PreviousStepStatus = True
        End If

        If PreviousStepStatus = False Then Exit Sub '----------------------------------------------------------------------------------------------------


        'Step # 2: Record PUSH Notification in the table

        Dim UserIDs(50) As Guid
        UserIDs = (From c As APP_UserDeviceList.UserDeviceinfo In mUserDeviceList
                            Select (c.UserID)).Distinct().ToArray()

        Dim Notifications(UserIDs.Count - 1) As APP_UserNotification

        For i As Integer = 0 To UserIDs.Count - 1

            If UserIDs(i).Equals(Guid.Empty) Then Exit For

            Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.NewAPP_UserNotification(Guid.NewGuid)


            Try
                With mAPP_UserNotification
                    .UserID = UserIDs(i)
                    .SentOn = Now
                    .Message = "Renewal of Certificate:- " + tmpMachineCertificate.CertificateName + " for Aircraft:- " + Session("RegNo") + " done"
                    .ModuleType = 7 'Machine Certificate
                    .ModuleID = tmpMachineCertificate.ID
                End With

                mAPP_UserNotification = CType(mAPP_UserNotification.Save, APP_UserNotification)

                Notifications(i) = mAPP_UserNotification

                PreviousStepStatus = True
            Catch ex As Exception
                PreviousStepStatus = False
            End Try
        Next

        'Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.NewAPP_UserNotification(Guid.NewGuid)

        If PreviousStepStatus = False Then Exit Sub '----------------------------------------------------------------------------------------------------

        For Each Notification As APP_UserNotification In Notifications

            Dim errorcount As Integer = 0

StartStep3:

            'Step # 3: Trigger PUSH Notification

            errorcount = errorcount + 1

            System.Net.ServicePointManager.Expect100Continue = True
            System.Net.ServicePointManager.SecurityProtocol = 3072 'System.Net.SecurityProtocolType.Tls

            Dim request = TryCast(System.Net.WebRequest.Create("https://onesignal.com/api/v1/notifications"), System.Net.HttpWebRequest)

            request.KeepAlive = True
            request.Method = "POST"
            request.ContentType = "application/json; charset=utf-8"

            request.Headers.Add("authorization", "Basic YmE0YTUwZDgtMmJkYS00MjMzLWI5NjgtZTkxZmE5MzQ0NzMw")

            Dim serializer = New JavaScriptSerializer()

            'Forming Notification Detail URL
            '
            '
            Dim index As Integer = HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("wfMachineCertificateRenew_AJAX.aspx")
            Dim urlNotificationDetail As String = ""
            urlNotificationDetail = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, index) + "APP/Launcher.aspx?NotificationID=" + Notification.ID.ToString + "&ModuleID=" + tmpMachineCertificate.ID.ToString + "&username=" + Notification.UserName + "&EventLogSessionID=" + Guid.NewGuid.ToString + "&ModuleTypeID=6"


            Dim filterObject As Object()
            ReDim filterObject(((mUserDeviceList.Count - 1) * 2) + 1)

            Dim idx As Integer = 0
            Dim Ridx As Integer = 0
            For Each info As APP_UserDeviceList.UserDeviceinfo In mUserDeviceList

                If Notification.UserID.Equals(info.UserID) Then
                    If idx = 0 Then
                        filterObject(idx) = New With {Key .field = "tag", Key .key = "DeviceID", Key .value = mUserDeviceList(0).DeviceID.ToString}
                        idx = idx + 1
                    Else
                        Ridx = Ridx + 1

                        filterObject(idx) = New With {Key .[operator] = "OR"}
                        idx = idx + 1

                        filterObject(idx) = New With {Key .field = "tag", Key .key = "DeviceID", Key .value = mUserDeviceList(Ridx).DeviceID.ToString}
                        idx = idx + 1
                    End If

                End If

            Next

            Dim obj = New With {Key .app_id = "f877b4d2-b6e5-4595-a381-87165f6e46a0", Key .contents = New With {Key .en = Notification.Message}, Key .headings = New With {Key .en = "FlyPal"}, Key .filters = filterObject, Key .data = New With {Key .url = urlNotificationDetail.ToString}}

            '---------------------

            Dim param = serializer.Serialize(obj)
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(param)

            Dim responseContent As String = Nothing

            Try

                Using writer = request.GetRequestStream()
                    writer.Write(byteArray, 0, byteArray.Length)
                End Using

                Using response As System.Net.HttpWebResponse = request.GetResponse()

                    Using reader = New System.IO.StreamReader(response.GetResponseStream())

                        responseContent = reader.ReadToEnd()

                    End Using

                End Using

            Catch ex As System.Net.WebException
                System.Diagnostics.Debug.WriteLine(ex.Message)
                System.Diagnostics.Debug.WriteLine(New System.IO.StreamReader(ex.Response.GetResponseStream()).ReadToEnd())

                If errorcount <= 3 Then GoTo StartStep3

            End Try

            System.Diagnostics.Debug.WriteLine(responseContent)
        Next

    End Sub
    'End
    Private Function Save() As Boolean
        SetObject()

        If mRenewMachineCertificate.IsCertificateBetSlot = True And mRenewMachineCertificate.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Issue Date is less than the last renewal (Expiry Date)." & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveBackDated")
            Exit Function
        End If
        If mRenewMachineCertificate.IsValid Then
            Try
                mRenewMachineCertificate.ApplyEdit()
                mRenewMachineCertificate = CType(mRenewMachineCertificate.Save(), MachineCertificate)
                SaveDailyStatus()
                Session("mRenewMachineCertificate") = mRenewMachineCertificate

                mMachineCertificate.ApplyEdit()
                mMachineCertificate = CType(mMachineCertificate.Save(), MachineCertificate)
                SaveBoardInfo() 'Added by Saylee on 22-May-2009
                Session("mMachineCertificate") = mMachineCertificate

                mMachineCertificateDetails = "Reg No. : " + Session("RegNo") & " Name : " & mRenewMachineCertificate.CertificateName & " No.: " & mRenewMachineCertificate.CertificateNo
                SendPUSHNotification(mRenewMachineCertificate) 'Added By Vikrant On 29-Oct-2021
                MarkLog(Util.Action.Save, "Renewal Certificate", mMachineCertificateDetails, Util.ErrorType.NoError, mRenewMachineCertificate.ID, EventLogID)
                'MarkLog(Util.Action.Save, "RenewalCertificate", mRenewMachineCertificate.CertificateName, Util.ErrorType.NoError, mRenewMachineCertificate.ID)

                btnByMail.DataBind() 'added on 14-Apr-2022
                Return True
            Catch ex As SqlException
                Session("mRenewMachineCertificate") = mRenewMachineCertificate
                Session("mMachineCertificate") = mMachineCertificate
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfMachineCertificateRenew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    msg1.Show()
                ElseIf ex.Number = 8145 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfMachineCertificateRenew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    msg1.Show()
                ElseIf ex.Number = 2627 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfMachineCertificateRenew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    msg1.Show()
                ElseIf ex.Number = 547 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfMachineCertificateRenew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    msg1.Show()
                End If
                Return False
            Finally
                mRenewMachineCertificate = Nothing
                mMachineCertificate = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Function SaveBackDated() As Boolean
        SetObject()

        If mRenewMachineCertificate.IsValid Then
            Try
                mRenewMachineCertificate.ApplyEdit()
                mRenewMachineCertificate = CType(mRenewMachineCertificate.Save(), MachineCertificate)
                SaveDailyStatus()
                Session("mRenewMachineCertificate") = mRenewMachineCertificate

                mMachineCertificate.ApplyEdit()
                mMachineCertificate = CType(mMachineCertificate.Save(), MachineCertificate)
                SaveBoardInfo() 'Added by Saylee on 22-May-2009
                Session("mMachineCertificate") = mMachineCertificate

                mMachineCertificateDetails = "Reg No. : " + Session("RegNo") & " Name : " & mRenewMachineCertificate.CertificateName & " No.: " & mRenewMachineCertificate.CertificateNo
                MarkLog(Util.Action.Save, "Renewal Certificate", mMachineCertificateDetails, Util.ErrorType.NoError, mRenewMachineCertificate.ID, EventLogID)
                'MarkLog(Util.Action.Save, "RenewalCertificate", mRenewMachineCertificate.CertificateName, Util.ErrorType.NoError, mRenewMachineCertificate.ID)
                Return True
            Catch ex As SqlException
                Session("mRenewMachineCertificate") = mRenewMachineCertificate
                Session("mMachineCertificate") = mMachineCertificate
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfMachineCertificateRenew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    msg1.Show()
                ElseIf ex.Number = 8145 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfMachineCertificateRenew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    msg1.Show()
                ElseIf ex.Number = 2627 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfMachineCertificateRenew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    msg1.Show()
                ElseIf ex.Number = 547 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfMachineCertificateRenew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    msg1.Show()
                End If
                Return False
            Finally
                mRenewMachineCertificate = Nothing
                mMachineCertificate = Nothing
            End Try
        Else
            Return False
        End If
    End Function
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 200 Then
                custValidator.ErrorMessage = "Max. length of Remark should be 200 char."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Function CustomValidate1() As Boolean
        SetObject()
        Dim strMSG As String = ""
        If Not mRenewMachineCertificate.IsValid Then
            For i As Integer = 0 To mRenewMachineCertificate.GetBrokenRulesCollection.Count - 1
                strMSG = strMSG + mRenewMachineCertificate.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If strMSG.Trim <> "" Then
            cvDate.ErrorMessage = strMSG
            cvDate.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub DataFieldBind()
        txtIssueDate.Text = mRenewMachineCertificate.IssueDateFormatted

        If mRenewMachineCertificate.ExpiryDate.ToString = "" Then
            txtExpiryDate.Text = ""
        Else
            txtExpiryDate.Text = mRenewMachineCertificate.ExpiryDateFormatted
        End If

        If mRenewMachineCertificate.WarningDays = 0 Then
            txtWarningDays.Text = "0"
        End If

        If mRenewMachineCertificate.EffectiveDate.ToString = "" Then
            txtEffectiveDate.Text = ""
        Else
            txtEffectiveDate.Text = mRenewMachineCertificate.EffectiveDateFormatted
        End If

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            If txtName.Enabled = True Then
                setFocus(txtName)
            Else
                setFocus(txtRemark)
            End If
            If mRenewMachineCertificate.ImageSize > 0 Then
                ImageButton1.Visible = True
                btnDelAttach.Enabled = True
            End If
            DataFieldBind()
        End If
        SetPage()
        ControlVisibility()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("RenewalCertificateNew") And Not User.IsInRole("RenewalCertificateEdit")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If Not IsValid Then upnlValidationsummary.Update() : Exit Sub
        If Not CustomValidate1() Then upnlValidationsummary.Update() : Exit Sub

        If IsValid Then
            If Save() Then

                upnlValidationsummary.Update()
                upnlDetails.Update()

                'Dim mopenas As String = Request.QueryString("Type")
                'If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                '    Exit Sub
                'End If
                'End

                ''  Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
            End If
        End If
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
       
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        If mRenewMachineCertificate.ImageSize > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mRenewMachineCertificate.FileExtension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mRenewMachineCertificate.FileExtension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mRenewMachineCertificate.ImageFile, 0, mRenewMachineCertificate.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
            End If
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()

        mMachineCertificateDetails = "Reg No. : " + Session("RegNo") & " Name : " & mRenewMachineCertificate.CertificateName & " No.: " & mRenewMachineCertificate.CertificateNo
        MarkLog(Util.Action.Close, "Renewal Certificate", mMachineCertificateDetails, Util.ErrorType.NoError, mRenewMachineCertificate.ID, EventLogID)

        RemoveSession()
        Session.Remove("RegNo")

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End

        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub

    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        mRenewMachineCertificate.ImageFile = file1
        mRenewMachineCertificate.ImageSize = 0
        Session("mRenewMachineCertificate") = mRenewMachineCertificate

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
    End Sub
    'Added on 11-Apr-2022
    Private Sub hdnimgBtnSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnSendMail.Click

        Try
            Dim No1 As New Random
            Dim path As String
            Dim StrName As String = "abc" & No1.Next.ToString
            If mRenewMachineCertificate.ImageSize > 0 Then
                path = AppSettings("DOCPath") & "\" & StrName & mRenewMachineCertificate.FileExtension
                Dim fs As FileStream
                If File.Exists(AppSettings("DOCPath")) = False Then
                    'Delete File if exist
                    System.IO.File.Delete(AppSettings("DOCPath") & StrName & mRenewMachineCertificate.FileExtension)
                    ' Create the file.
                    fs = File.Create(path)
                    '' Add some information to the file.
                    fs.Write(mRenewMachineCertificate.ImageFile, 0, mRenewMachineCertificate.ImageFile.Length)
                    fs.Close()
                    Session("DOCPath") = path
                End If
            End If

            Dim str As String
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Following Certificate has been renewed.</font></P></br> ")
            str = str + ("<P><font face=""Calibri"">Certificate Name:<b> " + mRenewMachineCertificate.CertificateName.ToString + "</b></font></P></br> ")
            str = str + ("<P><font face=""Calibri"">Certificate Number:<b> " + mRenewMachineCertificate.CertificateNo.ToString + "</b></font></P></br> ")
            str = str + ("<P><font face=""Calibri"">Aircraft Reg. No.:<b> " + Session("RegNo").ToString + "</b></font></P></br> ")
            str = str + ("<P><font face=""Calibri"">Issue Date:<b> " + mRenewMachineCertificate.IssueDateFormatted.ToString + "</b></font></P></br> ")
            str = str + (" <P><font face=""Calibri"">Expiry Date :<b> " + mRenewMachineCertificate.ExpiryDateFormatted.ToString + "</b></font></P></br>")
            str = str + (" <P><font face=""Calibri"">Certificate Remark:<b> " + mRenewMachineCertificate.Remark.ToString + "</b></font></P></br>")
            str = str + ("<p><font face=""Calibri"">")
            str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")
            str = str + ("</body></html>")
            SendMailFile.SendMailFile(Session("CrystalReport"), User.Identity.Name, "Certificate Renew Details - " + mRenewMachineCertificate.CertificateName.ToString, "", _
                                      str.ToString, "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), IIf(path = Nothing Or path = "", "", path), False, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                   SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"), ClientCode:=AppSettings("ClientCode"))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)

        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Protected Sub btnByMail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        Session("UserEmailID") = mModuleList.Item("RenewalCertificate").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("RenewalCertificate").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    '-----
#End Region

End Class