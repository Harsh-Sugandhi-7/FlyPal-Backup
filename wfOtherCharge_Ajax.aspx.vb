'Added By Vikrant On 06-Jan-2015
Imports System.Linq
Public Class wfOtherCharge_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mOtherCharge As OtherCharge
    Public mVendorList As VendorList
    Public mStatusList As StatusList

    Dim EventLogID As Guid 'Added By Utkarsh On 22-Jul-2011 For All19072011
    Dim OCDetail As String 'Added By Utkarsh On 22-Jul-2011 For All19072011
    Dim mFileAttach As FileAttach 'Added By Vikrant On 24-Sep-2020 For ALL24092020
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mOtherCharge = Session("mOtherCharge")
        mFileAttach = Session("mFileAttach") 'Added By Vikrant On 24-Sep-2020 For ALL24092020
    End Sub
    Private Sub setSession()
        Session("mOtherCharge") = mOtherCharge
        Session("mFileAttach") = mFileAttach 'Added By Vikrant On 24-Sep-2020 For ALL24092020
    End Sub
    'Added By Vikrant On 24-Sep-2020 For ALL24092020
    Private Sub ControlVisibilityForFileAttachment()
        If mOtherCharge.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
        upnlAttachFile.Update()
    End Sub
    'End

    Private Sub setObject()
        ' mOtherCharge.Date = CDate(txtOtherChargeDate.Text)
        mOtherCharge.Date = CDate(txtOtherChargeDate.Text)
        mOtherCharge.BillEntryNo = txtBillEntryNo.Text
        mOtherCharge.MasterAirwayBillNo = txtMasterAirwayBillNo.Text
        mOtherCharge.HouseAirwayBillNo = txtHouseAirwayBillNo.Text
        If Not IsDate(txtBillEntryDate.Text) Then
            mOtherCharge.BillEntryDate = System.DBNull.Value
        Else
            mOtherCharge.BillEntryDate = CDate(txtBillEntryDate.Text)
        End If
        ''mOtherCharge.BillEntryDate = txtBillEntryDate.Text
        If Not IsDate(txtMasterAirwayBillDate.Text) Then
            mOtherCharge.MasterAirwayBillDate = System.DBNull.Value
        Else
            mOtherCharge.MasterAirwayBillDate = CDate(txtMasterAirwayBillDate.Text)
        End If
        ''mOtherCharge.MasterAirwayBillDate = txtMasterAirwayBillDate.Text
        If Not IsDate(txtHouseAirwayBillDate.Text) Then
            mOtherCharge.HouseAirwayBillDate = System.DBNull.Value
        Else
            mOtherCharge.HouseAirwayBillDate = CDate(txtHouseAirwayBillDate.Text)
        End If
        ''=============================
        mOtherCharge.Text = txtText.Text
        mOtherCharge.No = Val(txtNo.Text)
        Session("mOtherCharge") = mOtherCharge
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveCharge, MSGBox.Message_text.RemoveCharge, "", MsgBoxStyle.YesNo, "Delete")
        mOtherCharge.OtherChargeDetails.CurrentIndex = Index
        Session("mOtherCharge") = mOtherCharge
    End Sub
    Private Sub DeleteInvoice(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "DeleteInvoice")
        mOtherCharge.OtherChargeInvoices.CurrentIndex = Index
        Session("mOtherCharge") = mOtherCharge
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("Sender") = ""
                            Dim mOtherCharge As OtherCharge
                            mOtherCharge = CType(Session("mOtherCharge"), OtherCharge)
                            mOtherCharge.OtherChargeDetails.Remove(mOtherCharge.OtherChargeDetails.CurrentItem)
                            Session("mOtherCharge") = mOtherCharge
                            dgCharges.DataSource = mOtherCharge.OtherChargeDetails
                            dgCharges.DataBind()
                            upnlCharges.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "DeleteInvoice" Then
                        Try
                            Session("Sender") = ""
                            Dim mOtherCharge As OtherCharge
                            mOtherCharge = CType(Session("mOtherCharge"), OtherCharge)
                            mOtherCharge.OtherChargeInvoices.Remove(mOtherCharge.OtherChargeInvoices.CurrentItem)
                            Session("mOtherCharge") = mOtherCharge
                            dgInvoices.DataSource = mOtherCharge.OtherChargeInvoices
                            dgInvoices.DataBind()
                            EnableDisable()
                            upnlInvoices.Update()
                            upnlOthrChargeDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                 MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                               MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                              MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
                        Session("sender") = ""
                        If Session("IsValid") Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not User.IsInRole("OtherChargeNew") And Not User.IsInRole("OtherChargeEdit")) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            Save()
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    Else
                        Session("Sender") = ""
                    End If
                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "OtherChargeTransTextSeriesAlert"
                    Session("sender") = ""
                    Session("AddTransTextSeries") = "True"
                    Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    'End
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetControlStatus()
        'btnAddInvoice.Enabled = IIf(StatusId > 1, False, True)
        'btnAddCharge.Enabled = IIf(StatusId > 1, False, True)
        'dgInvoices.Columns(7).Visible = IIf(StatusId > 1, False, True)
        dgInvoices.Columns(9).Visible = IIf(mOtherCharge.IsNew, True, False)
        'dgCharges.Columns(9).Visible = IIf(StatusId > 1, False, True)
        'dgCharges.Columns(10).Visible = IIf(StatusId > 1, False, True)
    End Sub
    Private Sub SetPage()
        If mOtherCharge.No > 0 Then
            lblTitle.Text = "Other Charge [" & mOtherCharge.Text + "-" + CType(mOtherCharge.No, String) + "]"
        End If
    End Sub
    Private Sub EnableDisable()
        txtOtherChargeDate.Enabled = (mOtherCharge.OtherChargeInvoices.Count = 0)
        If Not mOtherCharge.IsNew Then
            btnPrint.Enabled = True
            btnAddInvoice.Enabled = False
        Else
            btnPrint.Enabled = False
            btnAddInvoice.Enabled = True
        End If
    End Sub
    Private Function Save() As Boolean
        If mOtherCharge.IsValid Then
            'Authentication
            If Not mOtherCharge.Date Is System.DBNull.Value Then
                Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
                If mCheck.WebAuthentication = True Then
                    'Changes by Kalpesh in 13-3-2013
                    'These lines commented
                    '
                    'Dim strOutString As String = ReadXMLFile()
                    'strOutString = strOutString.Split(CChar("$"))(1)
                    'Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, CInt(strOutString), mCheck.SubscriptionDate)

                    'Changes by Kalpesh in 13-3-2013
                    'These lines commented
                    '
                    Dim mDays As Integer = 0
                    mDays = mCheck.Number("Days")

                    Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                    '---------------------------------

                    If DateDiff(DateInterval.Day, CDate(mOtherCharge.Date), maxAllowableDate) < 0 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Other Charge. <br> Other Charge Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                        Exit Function
                    End If
                End If
            End If
            'Authentication
            Dim OtherChargeClone As OtherCharge
            OtherChargeClone = mOtherCharge.Clone
            Try
                If Not mOtherCharge.OtherChargeInvoices.Count = 0 And Not mOtherCharge.OtherChargeDetails.Count = 0 Then
                    setObject()
                    'Added by Utkarsh ON 16-Dec-2013 FOr TransTextSeries
                    'Check if text is blank then call TransTextSeries UI

                    If (mOtherCharge.IsNew) And (mOtherCharge.Text = "") Then

                        Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(Trans.OtherCharge, mOtherCharge.DateFormatted)

                        If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(Trans.OtherCharge) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(Trans.OtherCharge) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(Trans.OtherCharge).TransText = "")) Then

                            Dim str = "<script language='javascript'>openledgersame('wfOtherCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"
                            Session("BackPagestr_ForTransSeries") = str
                            Session("TransName_ForTransSeries") = "Other Charge"
                            Session("TransTypeID_ForTransSeries") = Trans.OtherCharge
                            Session("TransDate_ForTransSeries") = mOtherCharge.DateFormatted

                            MSGBoxCtrl.show("Other Charge Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "OtherChargeTransTextSeriesAlert")
                            Return False
                        Else
                            Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                            If mAutoRenewTransTextSeries.IsRenewed Then
                                With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(Trans.OtherCharge)
                                    mOtherCharge.Text = .TransText
                                    mOtherCharge.No = .StartingTransNo
                                End With
                            Else
                                Dim str = "<script language='javascript'>openledgersame('wfOtherCharge_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"
                                Session("BackPagestr_ForTransSeries") = str
                                Session("TransName_ForTransSeries") = "Other Charge"
                                Session("TransTypeID_ForTransSeries") = Trans.OtherCharge
                                Session("TransDate_ForTransSeries") = mOtherCharge.DateFormatted

                                MSGBoxCtrl.show("Other Charge Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "OtherChargeTransTextSeriesAlert")
                                Return False
                            End If
                        End If

                    End If

                    'End
                    mOtherCharge.ApplyEdit()
                    mOtherCharge.Save()
                    EnableDisable()
                    txtNo.DataBind()
                    'Changed By Utkarsh On 22-Jul-2011 For All19072011
                    OCDetail = mOtherCharge.OtherChargeNo + " Dated : " + mOtherCharge.DateFormatted
                    MarkLog(Util.Action.Save, "Other Charge", OCDetail, Util.ErrorType.NoError, mOtherCharge.ID, EventLogID)
                    'End
                    mOtherCharge.MarkClean()
                    SetPage()
                    Session("mOtherCharge") = mOtherCharge
                    SetControlStatus()
                    upnlTitle.Update()
                    upnlOthrChargeDetails.Update()
                    upnlCharges.Update()
                    upnlInvoices.Update()
                    upnlActionBtn.Update()
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Other Charge can not be saved without Invoice and Charge.", MsgBoxStyle.OkOnly, "")
                    Exit Function
                End If
            Catch ex As SqlClient.SqlException
                Session("OtherChargeClone") = OtherChargeClone
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    'Code Added by DEVEN On 28/12/2007 --------------------------------------
                    If InStr(ex.Message, "FKtabOtherChargeDetailstabCharge", CompareMethod.Text) Then
                        MSGBoxCtrl.show("Other Charge Deleted ! ", "Other Charge Not Avalable<Br><BR>Selected Charge is no longer exist in the Database <BR><BR> Remove Charge and try Again", " ", MsgBoxStyle.OkOnly, "")
                    Else
                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                ElseIf ex.Number = 50000 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Message, MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                OtherChargeClone = Nothing
            End Try
        Else
            MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Chrage Name Required", MsgBoxStyle.OkOnly, "")
            Return False
        End If
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgInvoices.DataSource = mOtherCharge.OtherChargeInvoices
        dgCharges.DataSource = mOtherCharge.OtherChargeDetails
        txtOtherChargeDate.Text = mOtherCharge.DateFormatted.ToString
        txtBillEntryDate.Text = mOtherCharge.BillEntryDateFormatted.ToString
        txtMasterAirwayBillDate.Text = mOtherCharge.MasterAirwayBillDateFormatted.ToString
        txtHouseAirwayBillDate.Text = mOtherCharge.HouseAirwayBillDateFormatted.ToString
        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtOtherChargeDate" Then
            If txtOtherChargeDate.Text = "" Then
                custValidator.ErrorMessage = "Select OtherCharge Date."
                e.IsValid = False
            End If
        End If
        ''If custValidator.ControlToValidate = "txtNo" Then
        ''    If Val(txtNo.Text) <= 0 Then
        ''        custValidator.ErrorMessage = "Number Required."
        ''        e.IsValid = False
        ''    End If
        ''End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 22-Jul-2011 For All19072011
        addAttributes()
        SetControlStatus()
        'If txtOtherChargeDate.Text = "" Then
        '    txtOtherChargeDate.Text = Today.Date
        'End If
        If Not IsPostBack And Session("sender") = "" Then
            'Added by Utkarsh on 16-Dec-2013 for Trans Text Series
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If mOtherCharge.IsNew Then
                    mOtherCharge.Text = Session("TransText_ForTransSeries")
                    txtText.Text = mOtherCharge.Text
                    Session("mOtherCharge") = mOtherCharge
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If
            End If
            'End
            DataFieldBind()
            SetPage()
            EnableDisable()
            ControlVisibilityForFileAttachment() 'Added By Vikrant On 24-Sep-2020 For ALL24092020
        End If
    End Sub
    Private Sub btnAddInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddInvoice.Click
        If (Not User.IsInRole("OtherChargeNew") And mOtherCharge.IsNew) Or (Not User.IsInRole("OtherChargeEdit") And Not mOtherCharge.IsNew) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If IsValid Then
            setObject()
            'mOtherCharge.OtherChargeInvoices.Add(mOtherCharge.ID)
            Session("mOtherCharge") = mOtherCharge
            'Response.Redirect("wfOtherChargeInvoices.aspx?BackPage=wfOtherCharge.aspx&OtherChargeDate='" + mOtherCharge.Date.ToString + "'")
            'Chagned by Utkarsh on 20-Sep-2012 for Framework 4.0 compatibility
            Response.Redirect("wfOtherChargeInvoices_Ajax.aspx?BackPage=wfOtherCharge_Ajax.aspx&OtherChargeDate=" + mOtherCharge.Date.ToString + "")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnAddCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCharge.Click
        If IsValid Then
            setObject()
            Session("EditCharge") = False
            mOtherCharge.OtherChargeDetails.Add(mOtherCharge.ID)
            Session("mOtherCharge") = mOtherCharge
            'Added By Vikrant On 24-Sep-2020 For ALL24092020
            mFileAttach = FileAttach.NewAttachmentChild(Guid.NewGuid, mOtherCharge.OtherChargeDetails.CurrentItem.ID)
            Session("mFileAttach") = mFileAttach
            'End
            Response.Redirect("wfOtherChargeDetails_Ajax.aspx?BackPage=wfOtherCharge_Ajax.aspx")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub dgInvoices_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInvoices.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                DeleteInvoice(CInt(e.CommandArgument))
        End Select
    End Sub
    Private Sub dgCharges_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCharges.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                setObject()
                Session("EditCharge") = True
                mOtherCharge.OtherChargeDetails.CurrentIndex = CInt(e.CommandArgument)
                Session("mOtherCharge") = mOtherCharge
                Response.Redirect("wfOtherChargeDetails_Ajax.aspx?BackPage=wfOtherCharge_Ajax.aspx")
            Case "DeleteRec"
                DeleteRecord(CInt(e.CommandArgument))
        End Select
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("OtherChargeNew") And Not User.IsInRole("OtherChargeEdit")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If IsValid Then
            Save()
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Changed By Utkarsh On 22-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, "Other Charge", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End

        Session("IsValid") = IsValid
        If IsValid Then
            setObject()
        End If
        If mOtherCharge.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            Response.Redirect("index.aspx")
            'Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub txtOtherChargeDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOtherChargeDate.TextChanged
        ''======================================= 
        mOtherCharge.Date = CType(Trim(txtOtherChargeDate.Text), Object)
        txtText.Text = mOtherCharge.Text
        ''========================================
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not User.IsInRole("OtherChargePrint") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter
        Dim rpt As New crptOtherCharge
        Dim obj As rptOtherCharges
        Dim objChilds As rptOtherChargeChilds
        Dim letter As rptLetterHead
        Dim ds As New dsOtherCharge
        obj = rptOtherCharges.GetOtherChargse(mOtherCharge.ID)
        objChilds = rptOtherChargeChilds.GetOtherChargeChilds(mOtherCharge.ID)
        letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", AppSettings("Logo"))
        da.Fill(ds, obj)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, objChilds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, letter)
        rpt.SetDataSource(ds)
        Session("CrystalReport") = rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'Added By Vikrant On 24-Sep-2020 For ALL24092020
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mOtherCharge.IsAttachmentAdded = True Then
            'mFileAttach = FileAttach.GetAttachment(mReceiptCumInvoice.ID)
            mFileAttach = FileAttach.GetAttachmentChild(mOtherCharge.ID)
        Else
            'mFileAttach = FileAttach.NewAttachment(Guid.Empty, mReceiptCumInvoice.ID)
            mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mOtherCharge.ID)
        End If
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        If (Not User.IsInRole("ReceiptPOAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mOtherCharge.IsAttachmentAdded Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mOtherCharge.FileAttachments(0).Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mOtherCharge.FileAttachments(0).Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mOtherCharge.FileAttachments(0).ImageFile, 0, mOtherCharge.FileAttachments(0).ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            Else
                MSGBoxCtrl.show("Attachment!", "No Attach File Present", "", MsgBoxStyle.OkOnly, "")
                ControlVisibilityForFileAttachment()
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        mOtherCharge.IsAttachmentAdded = False
        mOtherCharge.FileAttachments.RemoveAt(0)
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        Session("mOtherCharge") = mOtherCharge
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        If mOtherCharge.IsAttachmentAdded Then
            mOtherCharge.FileAttachments(0).Size = mFileAttach.Size
            mOtherCharge.FileAttachments(0).ImageFile = mFileAttach.ImageFile
            mOtherCharge.FileAttachments(0).Extension = mFileAttach.Extension
        Else
            mOtherCharge.IsAttachmentAdded = True
            mOtherCharge.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
        End If
        Session("mOtherCharge") = mOtherCharge
        ControlVisibilityForFileAttachment()
    End Sub
    'End

#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetDistinctTextListAutoComplete(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mDistinctTextAutoComplete As DistinctTextListAutoComplete
        Dim str As String() = contextKey.Split("¿")
        Dim mTransTypeID As Integer = CInt(str(0).Substring(str(0).IndexOf("=") + 1))
        Dim mOrderDate As String = str(1).Substring(str(1).IndexOf("=") + 1)
        mDistinctTextAutoComplete = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, , True, mTransTypeID, mOrderDate)
        If count = 0 Then
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In mDistinctTextAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
        Else
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In mDistinctTextAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
        End If
    End Function
#End Region
    
End Class