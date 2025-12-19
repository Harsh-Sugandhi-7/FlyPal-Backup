

'AJAX Conversion By Saylee On 8-Oct-2014
Imports System.Web.Services
Imports System.Text
Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq
Imports System
Imports System.IO

Public Class wfrptAircraftCertificationDocument_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Private mMachineNameValueList As MachineNameValueList
    Public mRenewMachineCertificate As MachineCertificate
    Public mAircraftCertificateList As AircraftCertificateDocumentList
    Public mMachineCertificate As MachineCertificate
    Private AircraftId As String
    Public mBoardInfo As AircraftInformationBoard.BoardInfo 'Added by Saylee on 22-May-2009
    'Added by Saylee on 9th-Oct-2009
    Public mMachineMaintenance As MachineMaintenance
    Public mUpdateRenewMachineCertificateList As UpdateRenewMachineCertificateList
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Dim mMachineCertificateDetails As String

    Dim IsReadOnly As Boolean 'Added by Saylee

    Dim mSortedMachineCertificateList As List(Of AircraftCertificateDocumentList.AircraftCertificateDocumentListInfo) = New List(Of AircraftCertificateDocumentList.AircraftCertificateDocumentListInfo)
    Dim CertificateName As String = ""
    Dim mModuleList As ModuleList    'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAircraftCertificateList = CType(Session("mAircraftCertificateList"), AircraftCertificateDocumentList)

        mRenewMachineCertificate = CType(Session("mRenewMachineCertificate"), MachineCertificate)
        mMachineCertificate = CType(Session("mMachineCertificate"), MachineCertificate)
        AircraftId = Session("AircraftId")
        mMachineNameValueList = Session("mMachineNameValueList")

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 9th-Oct-2009
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee
        CertificateName = Session("CertificateName")
        mModuleList = Session("mModuleList")    'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
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
                            Dim mAircraftDSCList As DailyStatusList
                            mAircraftDSCList = DailyStatusList.GetDailyStatusList(mRenewMachineCertificate.MachineID, Guid.Empty.ToString, Guid.Empty.ToString, 7, True)
                            If mAircraftDSCList.Contains(mRenewMachineCertificate.ID, "") Then
                                DataFieldBind()
                                MSGBoxCtrl.show("Reference Delete!", "This certificate is added in Aircraft Daily Status. Please do not delete this Entry.", "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            Session("sender") = ""
                            'Added by Saylee on 28-May-2009
                            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mRenewMachineCertificate.ID)
                            '********************************
                            'Added by Saylee on 9th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mRenewMachineCertificate.ID, 11)
                            '=============================

                            MachineCertificate.DeleteRenawalMachineCertificate(mRenewMachineCertificate.ID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            Session("mMachineMaintenance") = mMachineMaintenance
                            mMachineCertificate.IsDone = False
                            mMachineCertificate.Save()
                            mRenewMachineCertificate.Save()

                            mMachineCertificateDetails = "Reg No. : " + Session("RegNoForDelete") & " Name : " & mRenewMachineCertificate.CertificateName & " No.: " & mRenewMachineCertificate.CertificateNo
                            Session.Remove("RegNoForDelete")
                            MarkLog(Util.Action.Delete, "Renewal Certificate", mMachineCertificateDetails, Util.ErrorType.NoError, mRenewMachineCertificate.ID, EventLogID)

                            'DataFieldBind()
                            'Added by Saylee on 28-May-2009
                            mBoardInfo.IsComplyDelete = True
                            mBoardInfo.ApplyEdit()
                            mBoardInfo.Save()
                            Session("mAircraftInformationBoardList") = Nothing
                            '********************************
                            DataFieldBind()
                            SetGrid()

                            SetPage()
                            upnlGridView.Update()
                            upnlActionBtnBottom.Update()
                            upnlResult.Update()
                            'Response.Redirect("wfMachineCertificateRenewList.aspx?MsgResult=0&BackPage=")
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Message, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Message, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Message, MsgBoxStyle.OkOnly, "")
                                mMachineCertificateDetails = "Reg No. : " + Session("RegNoForDelete") & " Name : " & mRenewMachineCertificate.CertificateName & " No.: " & mRenewMachineCertificate.CertificateNo
                                Session.Remove("RegNoForDelete")
                                MarkLog(Util.Action.Delete, "Renewal Certificate", "Can't delete : " & mMachineCertificateDetails & " is Currently in use", Util.ErrorType.NoError, mRenewMachineCertificate.ID, EventLogID)

                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "RenewalCertificate", mAircraftCertificateList.Item(mAircraftCertificateList.CurrentIndex).CertificateName, Util.ErrorType.NoError, mAircraftCertificateList.Item(mAircraftCertificateList.CurrentIndex).ID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    DataFieldBind()
                    SetGrid()

                    SetPage()
                    upnlGridView.Update()
                    upnlActionBtnBottom.Update()
                    upnlResult.Update()
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    DataFieldBind()
                    SetGrid()

                    SetPage()
                    upnlGridView.Update()
                    upnlActionBtnBottom.Update()
                    upnlResult.Update()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
                    SetGrid()

                    SetPage()
                    upnlGridView.Update()
                    upnlActionBtnBottom.Update()
                    upnlResult.Update()

            End Select
        ElseIf Result1 = -1 Then

        ElseIf Result1 = 0 Then

        End If
    End Sub
  
    Private Sub FindNow()
        dgCertificateList.PageIndex = 0

        Session("AircraftId") = cmbAircraftList.SelectedValue
        Session("CertificateName") = Trim(txtCertificateName.Text)
        mAircraftCertificateList = AircraftCertificateDocumentList.GetMachineCertificateList(New Guid(cmbAircraftList.SelectedValue.ToString), Today.Date.ToString, chkWithoutExpiryDdate.Checked, CertificateName:=Trim(txtCertificateName.Text), ShowNotIsApplicable:=chkApplicable.Checked)

        mSortedMachineCertificateList = (From c As AircraftCertificateDocumentList.AircraftCertificateDocumentListInfo In mAircraftCertificateList
             Order By c.RegNo, c.RemainingDays
             Select c).ToList

        dgCertificateList.DataSource = mSortedMachineCertificateList
        Session("mAircraftCertificateList") = mAircraftCertificateList
        DataBind()
        SetGrid()

    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById ('" + cntrl.ClientID + "').focus(); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetPage()
        mAircraftCertificateList = Session("mAircraftCertificateList")
        lblResult.Text = "List of Certificates as per selected criteria : " & mAircraftCertificateList.Count & " Record(s) found."
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptAircraftCertificationDocument_AJAX.aspx?" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mAircraftCertificateList")
            Session.Remove("AircraftId")
            Session.Remove("mMachineCertificate")
            Session.Remove("mMachineMaintenance") 'Added by Saylee on 9th-Oct-2009
            Session.Remove("CertificateName")
        End If
    End Sub
    Private Sub RenewRecord(ByVal Index As Int32, ByVal ID As Guid)
        mMachineCertificate = MachineCertificate.GetMachineCertificate(mAircraftCertificateList(Index).MachineID, ID)
        mRenewMachineCertificate = MachineCertificate.NewChildRenewalMachineCertificate(mAircraftCertificateList(Index).MachineID, _
                                                                                        mMachineCertificate.CertificateName, _
                                                                                        mMachineCertificate.CertificateNo, _
                                                                                        mMachineCertificate.IssueDate.ToString, _
                                                                                        mMachineCertificate.ExpiryDate.ToString, True, _
                                                                                        mMachineCertificate.Remark, mMachineCertificate.EffectiveDate.ToString)

        mRenewMachineCertificate.WarningDays = mMachineCertificate.WarningDays

        Session("mRenewMachineCertificate") = mRenewMachineCertificate
        Session("mMachineCertificate") = mMachineCertificate

        'Added by Saylee on 22-May-2009
        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mMachineCertificate.ID)
        Session("mBoardInfo") = mBoardInfo
        '**************************************************
        Session("RegNo") = mAircraftCertificateList(Index).RegNo 'cmbAircraftList.SelectedItem.Text
        'Dim str As String
        'str = "<script language='javascript'>openledgersame('wfMachineCertificateRenew.aspx?GChildPage2=Index.aspx'); </script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        SetGrid()

        SetPage()
        upnlGridView.Update()
        upnlActionBtnBottom.Update()
        upnlResult.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenMachineCertificateWindow", "OpenMachineCertificateWindow()", True)

    End Sub
    Private Sub HistoryRecord(ByVal Index As Int32, ByVal ID As Guid, MachineID As Guid)
        mRenewMachineCertificate = MachineCertificate.GetRenewalMachineCertificate(MachineID, ID)
        'If mRenewMachineCertificate.IsMaster Then
        '    'MessageBox.Show("This is a master record and can not be edited from here", "Comply Component Monitor Inspection Status", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        '    Dim msg As New SIMsgBox(Page, "Master Record!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfMachineCertificateRenewList.aspx?BackPage=" & Request.QueryString("BackPage")
        '    msg.Show()
        '    Exit Sub
        'Else
        If Not mRenewMachineCertificate.ReferenceID.Equals(Guid.Empty) Then
            mMachineCertificate = MachineCertificate.GetMachineCertificate(mRenewMachineCertificate.MachineID, mRenewMachineCertificate.ReferenceID)
            Session("mRenewMachineCertificate") = mRenewMachineCertificate
            Session("mMachineCertificate") = mMachineCertificate
        End If
        'Added by Saylee on 28-July-2009
        'mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mMachineCertificate.ID)
        'Session("mBoardInfo") = mBoardInfo
        '**************************************************
        'MarkLog(Util.Action.Edit, "RenewalCertificate", " Certificate -> " + mMachineCertificate.CertificateName, Util.ErrorType.NoError, mMachineCertificate.ID)

        Session("RegNo") = mAircraftCertificateList(Index).RegNo
        Session("AircraftIdForHistory") = MachineID.ToString
        mUpdateRenewMachineCertificateList = UpdateRenewMachineCertificateList.GetRenewMachineCertificateHistoryList(mRenewMachineCertificate.MachineID, Today.Date.ToString, mRenewMachineCertificate)
        Session("mUpdateRenewMachineCertificateList") = mUpdateRenewMachineCertificateList

        ''Dim str As String
        ''str = "<script language='javascript'>openledgersame('wfUpdateRenewMachineCertificateList.aspx?GChildPage2=Index.aspx'); </script>"
        ''ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        'End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenRenewHistoryWindow", "OpenRenewHistoryWindow()", True)


    End Sub
    Private Sub EditRecord(ByVal Index As Int32, ByVal ID As Guid)

        mRenewMachineCertificate = MachineCertificate.GetRenewalMachineCertificate(mAircraftCertificateList(Index).MachineID, ID)
        If mRenewMachineCertificate.IsMaster Then
            'MessageBox.Show("This is a master record and can not be edited from here", "Comply Component Monitor Inspection Status", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.MasterRecordEdit, SIMsgBox.Message_text.MasterRecordEdit, "You are trying to edit the record.This is a master record and can not be edited from here.", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfMachineCertificateRenewList.aspx?BackPage=" & Request.QueryString("BackPage")
            'msg.Show()
            'MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "You are trying to edit the record. This is a master record and can not be edited from here.", MsgBoxStyle.OkOnly, "")
            MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            mMachineCertificate = MachineCertificate.GetMachineCertificate(mRenewMachineCertificate.MachineID, mRenewMachineCertificate.ReferenceID)
            Session("mRenewMachineCertificate") = mRenewMachineCertificate
            Session("mMachineCertificate") = mMachineCertificate

            'Added by Saylee on 28-July-2009
            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mMachineCertificate.ID)
            Session("mBoardInfo") = mBoardInfo
            '**************************************************
            'MarkLog(Util.Action.Edit, "RenewalCertificate", " Certificate -> " + mMachineCertificate.CertificateName, Util.ErrorType.NoError, mMachineCertificate.ID)
            mMachineCertificateDetails = "Reg No. : " + mAircraftCertificateList(Index).RegNo & " Name : " & mMachineCertificate.CertificateName & " No.: " & mMachineCertificate.CertificateNo
            MarkLog(Util.Action.Edit, "Renewal Certificate", mMachineCertificateDetails, Util.ErrorType.NoError, mMachineCertificate.ID, EventLogID)
            Session("RegNo") = mAircraftCertificateList(Index).RegNo
            'Dim str As String
            'str = "<script language='javascript'>openledgersame('wfMachineCertificateRenew_AJAX.aspx?GChildPage2=Index.aspx'); </script>"
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            SetGrid()
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenMachineCertificateWindow", "OpenMachineCertificateWindow()", True)

        End If
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32, ByVal ID As Guid)

        mRenewMachineCertificate = MachineCertificate.GetRenewalMachineCertificate(mAircraftCertificateList(Index).MachineID, ID)
        If mRenewMachineCertificate.IsMaster Then
            'MessageBox.Show("This is a master record and can not be edited from here", "Comply Component Monitor Inspection Status", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            '''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.MasterRecordDelete, SIMsgBox.Message_text.MasterRecordDelete, "You are trying to delete the record.This is a master record and can not be deleted from here.", MsgBoxStyle.OkOnly)
            '''msg1.ReplacePage = "wfMachineCertificateRenewList.aspx?BackPage=" & Request.QueryString("BackPage")
            '''msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordDelete, MSGBox.Message_text.MasterRecordDelete, "You are trying to delete the record.This is a master record and can not be deleted from here.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            Session("RegNoForDelete") = cmbAircraftList.SelectedItem.Text
            mMachineCertificate = MachineCertificate.GetMachineCertificate(mRenewMachineCertificate.MachineID, mRenewMachineCertificate.ReferenceID)
            mAircraftCertificateList.CurrentIndex = Index
            Session("mAircraftCertificateList") = mAircraftCertificateList
            Session("mRenewMachineCertificate") = mRenewMachineCertificate
            Session("mMachineCertificate") = mMachineCertificate

            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
            'msg.ReplacePage = "wfMachineCertificateRenewList.aspx?BackPage="
            'Session("sender") = "Delete"
            MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")

            'msg.Show()
        End If
    End Sub
    Private Sub SetGrid()
        Dim P As Integer
        Dim B As Boolean


        For j As Integer = 0 To dgCertificateList.Rows.Count - 1

            If mMachineNameValueList(Me.dgCertificateList.Rows.Item(j).Cells(2).Text) Is Nothing Then
                IsReadOnly = True
            Else
                IsReadOnly = mMachineNameValueList(Me.dgCertificateList.Rows.Item(j).Cells(2).Text).IsReadOnly 'Added by Saylee - Restrict User from using ReadOnly Aircraft
            End If


        Next

        IsReadOnly = Session("IsReadOnly") 'Added by Saylee
        'Disable AddNew buttons if Aircraft is ReadOnly
        If IsReadOnly = True Then
            lblReadOnly.Visible = True
        Else
            lblReadOnly.Visible = False
        End If
        '*************************

    End Sub
    Private Sub SetReport(Optional byMail As Boolean = False)
        If Not User.IsInRole("RenewalCertificatePrint") Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If
        Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
        If cmbFormat.SelectedIndex = 0 Then
            Rpt = New crMachineRenewCertificatesFormat1
        Else
            Rpt = New crMachineRenewCertificatesFormat2
        End If

        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsMachineCertificatesForDueReport
        Dim rptMachineCertificateList As New AircraftCertificateDocumentList
        Dim mCompanyDetail As New CompanyDetail


        rptMachineCertificateList = AircraftCertificateDocumentList.GetMachineCertificateList(New Guid(cmbAircraftList.SelectedValue.ToString), Today.Date.ToString, chkWithoutExpiryDdate.Checked, CertificateName:=Trim(txtCertificateName.Text), Format:=0, ShowNotIsApplicable:=chkApplicable.Checked)
        mMachineNameValueList = Session("mMachineNameValueList")
        Dim AircraftName As String = cmbAircraftList.SelectedItem.Text 'mMachineList(New Guid(cmbAircraftList.SelectedValue.ToString)).RegNo

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "Aircraft Certificate/Documents Report", AircraftName, Trim(txtCertificateName.Text), "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", AppSettings("Logo"))


        If rptMachineCertificateList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1114)
        End If

        da.Fill(ds, "AircraftCertificateDocumentList", rptMachineCertificateList)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        If byMail Then
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Aircraft Certificate Report", "Aircraft Certificate Report", AircraftName, _
                                      "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                       SmtpHost:=mModuleList.Item("RenewalCertificate").SmtpHost, SmtpPort:=mModuleList.Item("RenewalCertificate").SmtpPort, SmtpUser:=mModuleList.Item("RenewalCertificate").SmtpUser, SmtpPassword:=mModuleList.Item("RenewalCertificate").SmtpPassword)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        Dim mMachineId As Guid = Guid.Empty

        'mMachineList = tmpMachineList.GetMachineList(, , , , , "<SELECT>")

        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, SkipIsForInventoryAircarft:=True, IsTagRequired:=True, TagText:="(ALL)")
        cmbAircraftList.DataSource = mMachineNameValueList

        If mMachineNameValueList.Count >= 1 And (IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString) Then mMachineId = mMachineNameValueList(0).ID Else mMachineId = New Guid(AircraftId)
        'cmbAircraftList.DataSource = tmpMachineList.GetMachineList(, , , , , "<SELECT>")

        mAircraftCertificateList = AircraftCertificateDocumentList.GetMachineCertificateList(mMachineId, Today.Date.ToString, chkWithoutExpiryDdate.Checked, CertificateName:=CertificateName, Format:=0, ShowNotIsApplicable:=chkApplicable.Checked)


        mSortedMachineCertificateList = (From c As AircraftCertificateDocumentList.AircraftCertificateDocumentListInfo In mAircraftCertificateList
             Order By c.RegNo, c.RemainingDays
             Select c).ToList

        dgCertificateList.DataSource = mSortedMachineCertificateList


        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mAircraftCertificateList") = mAircraftCertificateList
        DataBind()

        If IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString Then cmbAircraftList.SelectedIndex = 0 Else cmbAircraftList.SelectedValue = AircraftId
        txtCertificateName.Text = CertificateName
        Session("AircraftId") = cmbAircraftList.SelectedValue
        Session("RegNo") = cmbAircraftList.SelectedItem.Text
        Session("CertificateName") = CertificateName

        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraftList.SelectedValue)).IsReadOnly 'Added by Saylee - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnly") = IsReadOnly
    End Sub

    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custvalid As CustomValidator = CType(s, CustomValidator)
        If custvalid.ControlToValidate = "cmbAircraftList" Then
            If cmbAircraftList.SelectedIndex = 0 Then
                custvalid.ErrorMessage = "Please Select the Aircraft from the list."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            setFocus(cmbAircraftList)
            Session("MiddleFrame") = "wfrptAircraftCertificationDocument_AJAX.aspx?"
            DataFieldBind()
        End If
        SetGrid()

        SetPage()
    End Sub

    Private Sub dgCertificateList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCertificateList.RowCommand
        'If e.Item.ItemIndex = -1 Then Exit Sub
        'Dim Index As Int16 = e.Item.ItemIndex + dgCertificateList.PageSize * dgCertificateList.PageIndex
        'Dim mID As New Guid(e.Item.Cells(0).Text)
        'Dim mName As String = e.Item.Cells(2).Text
        'Dim mNo As String = e.Item.Cells(3).Text
        'mMachineCertificateDetails = "Reg No. : " + cmbAircraftList.SelectedItem.Text & " Name : " & mName & " No.: " & mNo
        Select Case e.CommandName
            Case "RenewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgCertificateList.PageSize * dgCertificateList.PageIndex

                'Dim mID As Guid = mAircraftCertificateList(index).ID
                Dim mID As Guid = New Guid(dgCertificateList.DataKeys(Index).Values("ID").ToString)
                Dim mName As String = mAircraftCertificateList(mID).CertificateName
                Dim mNo As String = mAircraftCertificateList(mID).CertificateNo
                mMachineCertificateDetails = "Reg No. : " + cmbAircraftList.SelectedItem.Text & " Name : " & mName & " No.: " & mNo

                If Not User.IsInRole("RenewalCertificateNew") Then
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfMachineCertificateRenewList.aspx?MsgResult=0&BackPage="
                    'msg.Show()
                    MarkLog(Util.Action.Comply, "Renewal Certificate", User.Identity.Name & " is not Authorized User to renew " & mMachineCertificateDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                RenewRecord(Index, mID)
                MarkLog(Util.Action.Comply, "Renewal Certificate", mMachineCertificateDetails, Util.ErrorType.NoError, mID, EventLogID)
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgCertificateList.PageSize * dgCertificateList.PageIndex
                'Dim mID As Guid = mAircraftCertificateList(Index).ID
                Dim mID As Guid = New Guid(dgCertificateList.DataKeys(Index).Values("ID").ToString)
                Dim mName As String = mAircraftCertificateList(mID).CertificateName
                Dim mNo As String = mAircraftCertificateList(mID).CertificateNo
                mMachineCertificateDetails = "Reg No. : " + cmbAircraftList.SelectedItem.Text & " Name : " & mName & " No.: " & mNo


                If (Not User.IsInRole("RenewalCertificateEdit") And Not User.IsInRole("RenewalCertificateView")) Then
                    'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If

                DataFieldBind()
                SetGrid()
                EditRecord(Index, mID)
                upnlGridView.Update()
                upnlActionBtnBottom.Update()
                upnlResult.Update()
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgCertificateList.PageSize * dgCertificateList.PageIndex
                'Dim mID As Guid = mAircraftCertificateList(Index).ID
                Dim mID As Guid = New Guid(dgCertificateList.DataKeys(Index).Values("ID").ToString)
                Dim mName As String = mAircraftCertificateList(mID).CertificateName
                Dim mNo As String = mAircraftCertificateList(mID).CertificateNo
                mMachineCertificateDetails = "Reg No. : " + cmbAircraftList.SelectedItem.Text & " Name : " & mName & " No.: " & mNo


                If (Not User.IsInRole("RenewalCertificateDelete")) Then
                    'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecord(Index, mID)
            Case "ViewRec"
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                Dim Index As Integer = CInt(e.CommandArgument) + dgCertificateList.PageSize * dgCertificateList.PageIndex
                ' Dim mID As Guid = mAircraftCertificateList(Index).ID
                Dim mID As Guid = New Guid(dgCertificateList.DataKeys(Index).Values("ID").ToString)
                Dim mName As String = mAircraftCertificateList(mID).CertificateName
                Dim mNo As String = mAircraftCertificateList(mID).CertificateNo
                mMachineCertificateDetails = "Reg No. : " + cmbAircraftList.SelectedItem.Text & " Name : " & mName & " No.: " & mNo

                mRenewMachineCertificate = MachineCertificate.GetRenewalMachineCertificate(mAircraftCertificateList(Index).MachineID, mID)
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
            Case "HistoryRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgCertificateList.PageSize * dgCertificateList.PageIndex
                'Dim mID As Guid = mAircraftCertificateList(Index).ID

                Dim mID As Guid = New Guid(dgCertificateList.DataKeys(Index).Values("ID").ToString)
                Dim mName As String = mAircraftCertificateList(mID).CertificateName
                Dim mNo As String = mAircraftCertificateList(mID).CertificateNo
                mMachineCertificateDetails = "Reg No. : " + mAircraftCertificateList(Index).RegNo & " Name : " & mName & " No.: " & mNo

                HistoryRecord(Index, mID, mAircraftCertificateList(Index).MachineID)
        End Select
    End Sub
    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Not (e.Row.Cells(11).Text) = "&nbsp;" Then


                If (CDbl(e.Row.Cells(11).Text) <= 0.0) Then
                    e.Row.Cells(6).BackColor = Color.Red
                ElseIf (CDbl(e.Row.Cells(11).Text) > 0.0) And (CDbl(e.Row.Cells(11).Text) <= 30.0) Then
                    e.Row.Cells(6).BackColor = Color.Yellow
                ElseIf (CDbl(e.Row.Cells(11).Text) > 30.0) And (CDbl(e.Row.Cells(11).Text) <= 60.0) Then
                    e.Row.Cells(6).BackColor = Color.Green
                End If
            End If
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session.Remove("mMachineNameValueList")
        Session.Remove("mMachineCertificate")
        Session.Remove("mAircraftCertificateList")
        Session.Remove("AircraftId")
        Session.Remove("IsReadOnly")
        Session.Remove("CertificateName")
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        SetReport()
    End Sub
    Private Sub btnFindNow_Click(sender As Object, e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
        SetPage()
        upnlGridView.Update()
        upnlActionBtnBottom.Update()
        upnlResult.Update()
    End Sub

    Private Sub dgCertificateList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCertificateList.Sorting
        mAircraftCertificateList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAircraftCertificateList") = mAircraftCertificateList
        dgCertificateList.DataSource = mAircraftCertificateList
        dgCertificateList.DataBind()
        SetGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnMachineCertificate_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMachineCertificate.Click, hdnBtnRenewHistory.Click
        DataFieldBind()
        SetGrid()
        SetPage()
        upnlGridView.Update()
        upnlActionBtnBottom.Update()
        upnlResult.Update()
    End Sub

    Protected Sub dgCertificateList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgCertificateList.Columns(i).HeaderText
            Next
        End If
    End Sub
#End Region

   
  
End Class