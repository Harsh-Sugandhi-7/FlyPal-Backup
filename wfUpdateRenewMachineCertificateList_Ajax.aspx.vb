

'AJAX Conversion By Saylee On 8-Oct-2014

Public Class wfUpdateRenewMachineCertificateList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Private mMachineList As MachineList
    Public mRenewMachineCertificate As MachineCertificate
    Public mRenewMachineCertificateList As MachineCertificateList
    Public mMachineCertificate As MachineCertificate
    Private AircraftId As String
    Dim Flag As Int16

    Public mUpdateRenewMachineCertificateList As UpdateRenewMachineCertificateList
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Dim mMachineCertificateDetails As String
    Dim RegNo As String = String.Empty
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mUpdateRenewMachineCertificateList = CType(Session("mUpdateRenewMachineCertificateList"), UpdateRenewMachineCertificateList)
        AircraftId = Session("AircraftIdForHistory")
        RegNo = Session("RegNo")
    End Sub

    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mUpdateRenewMachineCertificateList") = mUpdateRenewMachineCertificateList
        Session("AircraftIdForHistory") = AircraftId
    End Sub

    Private Sub MessageBoxResult()
       Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    ''
                Case MsgBoxResult.No
                    Session("sender") = ""
                    '' Response.Redirect("wfMachineCertificateRenewList.aspx?MsgResult=0&BackPage=")
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    ''DataFieldBind()
                    ''Response.Redirect("wfMachineCertificateRenewList.aspx?MsgResult=0&BackPage=")
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    ''DataFieldBind()
                    ''Response.Redirect("wfMachineCertificateRenewList.aspx?MsgResult=0&BackPage=")
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            ''Response.Redirect("wfMachineCertificateRenewList.aspx?MsgResult=0&BackPage=")
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            '  DataFieldBind()
        End If
    End Sub

    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById ('" + cntrl.ClientID + "').focus(); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub

    Public Function Save() As Boolean
        Dim txtRemark As TextBox
        Dim str As String = String.Empty
        Dim mIsSaved As Boolean = False
        Dim j As Int32
        For j = 0 To Me.dgCertificateList.Rows.Count - 1
            txtRemark = CType(Me.dgCertificateList.Rows(j).FindControl("txtRemark"), TextBox)
            Try
                Dim mMachineCertificate As MachineCertificate = MachineCertificate.GetMachineCertificate(New Guid(AircraftId), mUpdateRenewMachineCertificateList(j).ID)
                mMachineCertificate.Remark = Trim(txtRemark.Text)
                If mMachineCertificate.IsValid Then
                    If mMachineCertificate.IsDirty Then
                        mMachineCertificate.ApplyEdit()
                        mMachineCertificate = CType(mMachineCertificate.Save(), MachineCertificate)
                        mMachineCertificateDetails = "Reg No. : " + Session("RegNo") & " Name : " & mMachineCertificate.CertificateName & " No.: " & mMachineCertificate.CertificateNo & " Issue Date: " & mMachineCertificate.IssueDateFormatted
                        MarkLog(Util.Action.Save, "Renewal Certificate", mMachineCertificateDetails, Util.ErrorType.NoError, mMachineCertificate.ID, EventLogID)
                        mIsSaved = True
                    End If
                Else
                    str = str + "Reg No. : " + Session("RegNo") & " Name : " & mMachineCertificate.CertificateName & " No.: " & mMachineCertificate.CertificateNo & " Issue Date: " & mMachineCertificate.IssueDateFormatted + vbCrLf
                    mIsSaved = False
                End If
            Catch ex As Exception
                Throw ex
            End Try
        Next j

        If mIsSaved = True Then
            Return True
        Else
            If str <> "" Then
                cvDate.ErrorMessage = str
                cvDate.IsValid = False
                Return False
            End If
        End If

    End Function

    Private Sub SetGrid()
        'Dim P As Integer
        'For j As Integer = 0 To dgCertificateList.Rows.Count - 1

        '    If Me.dgCertificateList.Rows.Item(j).Cells(14).Text = "" Then
        '        P = 0
        '    Else
        '        P = CType(Me.dgCertificateList.Rows.Item(j).Cells(14).Text, Integer)
        '    End If
        '    If P <= 0 Then
        '        dgCertificateList.Rows.Item(j).Cells(13).Enabled = False
        '    End If
        'Next
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        Dim mMachineId As Guid = Guid.Empty

        ''mMachineList = MachineList.GetMachineListMonitoringStatus(Today.Date.ToShortDateString, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "<SELECT>")
        ''cmbAircraftList.DataSource = mMachineList

        dgCertificateList.DataSource = mUpdateRenewMachineCertificateList

        Session("mMachineList") = mMachineList
        Session("mUpdateRenewMachineCertificateList") = mUpdateRenewMachineCertificateList
        DataBind()

        ''cmbAircraftList.SelectedValue = AircraftId
        txtAircraft.Text = RegNo
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        Dim str As String = ""

        Dim txtRemark As TextBox
        Dim j As Int32
        For j = 0 To Me.dgCertificateList.Rows.Count - 1
            txtRemark = CType(Me.dgCertificateList.Rows(j).FindControl("txtRemark"), TextBox)
            Try
                Dim mMachineCertificate As MachineCertificate = MachineCertificate.GetMachineCertificate(New Guid(AircraftId), mUpdateRenewMachineCertificateList(j).ID)
                mMachineCertificate.Remark = Trim(txtRemark.Text)
                mMachineCertificateDetails = " No.: " & mMachineCertificate.CertificateNo & " Issue Date: " & mMachineCertificate.IssueDateFormatted

                If Not mMachineCertificate.IsValid Then
                    For i As Integer = 0 To mMachineCertificate.GetBrokenRulesCollection.Count - 1
                        str = str + mMachineCertificateDetails + " : " + mMachineCertificate.GetBrokenRulesCollection(i).Description + "<BR>"
                    Next
                End If

            Catch ex As Exception
                Throw ex
            End Try

        Next j

        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            DataFieldBind()
        End If
        SetGrid()
    End Sub

    Private Sub dgCertificateList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCertificateList.PageIndexChanging
        dgCertificateList.PageIndex = e.NewPageIndex
        dgCertificateList.DataSource = mUpdateRenewMachineCertificateList
        Session("mUpdateRenewMachineCertificateList") = mUpdateRenewMachineCertificateList
        dgCertificateList.DataBind()
    End Sub
    Private Sub dgCertificateList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCertificateList.RowCommand
       
        Select Case e.CommandName
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgCertificateList.PageSize * dgCertificateList.PageIndex
                Dim mID As Guid = mUpdateRenewMachineCertificateList(Index).ID

                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------

                dgCertificateList.DataSource = mUpdateRenewMachineCertificateList
                dgCertificateList.DataBind()
                SetGrid()
                upnlGrid.Update()
                upnlButtons.Update()
                upnlResult.Update()
                upnlDetails.Update()

                mRenewMachineCertificate = MachineCertificate.GetRenewalMachineCertificate(New Guid(AircraftId.ToString), mID)
                If mRenewMachineCertificate.ImageSize > 0 Then
                    'Dim path As String = AppSettings("DOCPath") & "\" & StrName & mRenewMachineCertificate.FileExtension
                    Dim path As String = AppSettings("DOCPath") & StrName & mRenewMachineCertificate.FileExtension
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
        End Select
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If ((Not User.IsInRole("RenewalCertificateEdit") And Not User.IsInRole("RenewalCertificateNew"))) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If

        If Not IsValid Then upnlValidationsummary.Update() : Exit Sub

        If IsValid Then
            If Save() Then
                SetGrid()
                upnlGrid.Update()
                upnlButtons.Update()
                upnlResult.Update()
                upnlDetails.Update()


                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
                'End

                Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
            End If

        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        Session.Remove("mRenewMachineCertificate")
        Session.Remove("mMachineCertificate")

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End

        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub dgCertificateList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCertificateList.Sorting
        mUpdateRenewMachineCertificateList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mUpdateRenewMachineCertificateList") = mUpdateRenewMachineCertificateList
        dgCertificateList.DataSource = mUpdateRenewMachineCertificateList
        dgCertificateList.DataBind()
        SetGrid()
    End Sub

#End Region

End Class