Imports System.Collections.Generic
Imports System.Linq
Public Class wfWorkInvoice_Ajax
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
        Authorized = 8
    End Enum
    Private Enum RequstFor
        Supplier = 0
        Customer = 1
    End Enum
#End Region

#Region " Variable Declaration "
    Public mWorkInvoice As WorkInvoice
    Public mVendorList As VendorList
    Public mCurrencyList As CurrencyList
    Public Flag As Integer
    Public mTransTypeID As Trans
    Dim EventLogID As Guid
    Private mWorkInvoiceItem As WorkInvoiceItem
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mWorkInvoice = Session("mWorkInvoice")
        mVendorList = Session("mVendorList")
        mCurrencyList = Session("mCurrencyList")
        mTransTypeID = Session("mTransTypeId")
        mFileAttach = Session("mFileAttach")
    End Sub
    Private Sub setObject()
        If txtWorkInvoiceDate.Text = "" Then
            mWorkInvoice.Date = Today.Date
        Else
            mWorkInvoice.Date = CDate(txtWorkInvoiceDate.Text)
        End If
        mWorkInvoice.Text = txtText.Text
        mWorkInvoice.Remark = txtRemark.Text
        mWorkInvoice.No = Val(txtNo.Text)
        mWorkInvoice.UserName = User.Identity.Name
        mWorkInvoice.IsRoundOff = chkIsRoundOff.Checked
        mWorkInvoice.RefNo = txtRefNo.Text
        If txtRefDate.Text = "" Then
            mWorkInvoice.RefDate = System.DBNull.Value
        Else
            mWorkInvoice.RefDate = CDate(txtRefDate.Text)
        End If
        mWorkInvoice.CalculateTotal()
    End Sub
    Private Sub setVendorDetails()
        mWorkInvoice.VendorID = New Guid(cmbVendorList.SelectedValue)
        mWorkInvoice.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
        mWorkInvoice.ConversionFactor = Val(txtConversionFactor.Text)
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mWorkInvoice.WorkInvoiceItems.CurrentIndex = Index
        Session("mWorkInvoice") = mWorkInvoice
    End Sub
    Private Sub DeleteChargeRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveCharge, MSGBox.Message_text.RemoveCharge, "", MsgBoxStyle.YesNo, "DeleteCharge")
        mWorkInvoice.WorkInvoiceCharges.CurrentIndex = Index
        Session("mWorkInvoice") = mWorkInvoice
    End Sub
    Private Sub DeleteWorkInvoiceTools(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Remove, MSGBox.Message_text.Remove, "", MsgBoxStyle.YesNo, "DeleteWorkInvoiceTools")
        mWorkInvoice.WorkInvoiceTools.CurrentIndex = Index
        Session("mWorkInvoice") = mWorkInvoice
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mWorkInvoice = CType(Session("mWorkInvoice"), WorkInvoice)
                            mWorkInvoice.WorkInvoiceItems.Remove(mWorkInvoice.WorkInvoiceItems.CurrentItem)
                            mWorkInvoice.WorkInvoiceItems.CurrentIndex = mWorkInvoice.WorkInvoiceItems.Count - 1
                            For i As Integer = 0 To mWorkInvoice.WorkInvoiceItems.Count - 1
                                mWorkInvoice.WorkInvoiceItems(i).SrNo = i + 1
                            Next
                            dgWorkInvoiceItems.DataSource = mWorkInvoice.WorkInvoiceItems
                            dgWorkInvoiceItems.DataBind()
                            SetGrid()
                            upnlWorkInvoiceItem.Update()
                            mWorkInvoice.CalculateTotal()
                            If mWorkInvoice.IsRoundOff = True Then
                                mWorkInvoice.RoundCGrandTotal()
                            End If
                            upnlOtherDetails.Update()
                            Session("mWorkInvoice") = mWorkInvoice
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteCharge" Then
                        Try
                            mWorkInvoice = CType(Session("mWorkInvoice"), WorkInvoice)
                            mWorkInvoice.WorkInvoiceCharges.Remove(mWorkInvoice.WorkInvoiceCharges.CurrentItem)
                            dgWorkInvoiceCharges.DataSource = mWorkInvoice.WorkInvoiceCharges
                            dgWorkInvoiceCharges.DataBind()
                            upnlWorkInvoiceCharges.Update()
                            mWorkInvoice.CalculateTotal()
                            If mWorkInvoice.IsRoundOff = True Then 'Added By Prashant on 29-Oct-2012 ALL25102012
                                SetChargeGrid()
                                mWorkInvoice.RoundCGrandTotal()
                            End If
                            upnlOtherDetails.Update()
                            Session("mWorkInvoice") = mWorkInvoice
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteWorkInvoiceTools" Then
                        Try
                            mWorkInvoice = CType(Session("mWorkInvoice"), WorkInvoice)
                            mWorkInvoice.WorkInvoiceTools.Remove(mWorkInvoice.WorkInvoiceTools.CurrentItem)
                            dgWorkInvoiceTools.DataSource = mWorkInvoice.WorkInvoiceTools
                            dgWorkInvoiceTools.DataBind()
                            upnlWorkInvoiceTools.Update()
                            Session("mWorkInvoice") = mWorkInvoice
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        If mWorkInvoice.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not User.IsInRole("WorkInvoiceNew") And Not User.IsInRole("WorkInvoiceEdit")) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            Save()
                        Else
                            Session.Remove("IsValid")
                            If CustomValidate1() = False Then
                                upnlTitle.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If mWorkInvoice.IsValid = True Then
                            Session.Remove("IsValid")
                            mWorkInvoice.StatusID = 2
                            DataFieldBind()
                            Save()
                        Else
                            If CustomValidate1() = False Then
                                upnlTitle.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "StatusCancel" Then
                        Session("sender") = ""
                        mWorkInvoice.StatusID = 4
                        DataFieldBind()
                        Save()
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session.Remove("mTypeList")
                        Session.Remove("mModuleName")
                        Session.Remove("mPendingItemList")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                    If (MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "StatusCancel") Then

                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        Session("sender") = ""
                        If mWorkInvoice.StatusID = 2 Then
                            mWorkInvoice.StatusID = 1
                        ElseIf mWorkInvoice.StatusID = 4 Then
                            mWorkInvoice.StatusID = 2
                        End If
                        Session("mWorkInvoice") = mWorkInvoice
                        DataFieldBind()
                    ElseIf MSGBoxCtrl.Sender = "RCITransTextSeriesAlert" Then
                        Session("sender") = ""
                        Session("AddTransTextSeries") = "True"
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    End If
            End Select
        End If
    End Sub
    Private Sub addAttributes()
        txtConversionFactor.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtConversionFactor').value,event)")
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetControlStatus(ByVal StatusId As Int16)
        btnAdd.Enabled = IIf(StatusId > 1, False, True)
        btnWorkInvoiceTools.Enabled = IIf(StatusId > 1, False, True)
        btnAddTerms.Enabled = IIf(StatusId > 1, False, True)
        btnAddCharge.Enabled = IIf(StatusId > 1, False, True)
        btnSave.Visible = IIf(StatusId > 1, False, True)
        dgWorkInvoiceItems.Columns(11).Visible = IIf(StatusId > 1, False, True)
        dgWorkInvoiceItems.Columns(12).Visible = IIf(StatusId > 1, False, True)
        dgWorkInvoiceTools.Columns(6).Visible = IIf(StatusId > 1, False, True)
        dgWorkInvoiceTools.Columns(7).Visible = IIf(StatusId > 1, False, True)
        dgWorkInvoiceCharges.Columns(5).Visible = IIf(StatusId > 1, False, True)
        dgWorkInvoiceCharges.Columns(6).Visible = IIf(StatusId > 1, False, True)
        dgWorkInvoiceTerms.Columns(2).Visible = IIf(StatusId > 1, False, True)
        btnSelectFile.Disabled = IIf(StatusId > 1, True, False)
    End Sub
    Private Sub SetPage()
        If mWorkInvoice.No > 0 Then
            lblTitle.Text = "Work Invoice [" & mWorkInvoice.Text + "-" + CType(mWorkInvoice.No, String) + "]"
        Else
            lblTitle.Text = "Work Invoice [New]"
        End If
    End Sub
    Private Sub ControlVisibility()
        txtText.Enabled = IIf(mWorkInvoice.StatusID >= 2, False, True)
        txtNo.Enabled = IIf(mWorkInvoice.StatusID >= 2, False, True)
        cmbVendorList.Enabled = (CType(IIf(mWorkInvoice.StatusID >= 2, False, True), Boolean) And mWorkInvoice.WorkInvoiceItems.Count = 0) Or (mWorkInvoice.WorkInvoiceItems.Count = 0)
        cmbCurrencyList.Enabled = (CType(IIf(mWorkInvoice.StatusID >= 2, False, True), Boolean))
        txtConversionFactor.Enabled = (CType(IIf(mWorkInvoice.StatusID >= 2, False, True), Boolean))
        txtWorkInvoiceDate.Enabled = (CType(IIf(mWorkInvoice.StatusID >= 2, False, True), Boolean) And mWorkInvoice.WorkInvoiceItems.Count = 0) Or (mWorkInvoice.WorkInvoiceItems.Count = 0)
        btnAuthorized.Visible = (Not mWorkInvoice.IsNew) And (mWorkInvoice.StatusID = 1)
        btnCancel.Visible = (Not mWorkInvoice.IsNew) And (mWorkInvoice.StatusID = 2)
        cmbCurrencyList.Enabled = (CType(IIf(mWorkInvoice.StatusID >= 2, False, True), Boolean))
        txtRefDate.Enabled = (CType(IIf(mWorkInvoice.StatusID >= 2, False, True), Boolean))
        txtRefNo.Enabled = IIf(mWorkInvoice.StatusID >= 2, False, True)
        chkIsRoundOff.Enabled = (mWorkInvoice.StatusID = 1)
        If Not User.IsInRole("WorkInvoiceAuthorized") Then
            btnAuthorized.Enabled = False
            btnAuthorized.ToolTip = "You are not authorized user "
            btnCancel.Enabled = False
            btnCancel.ToolTip = "You are not authorized user "
        End If
        ControlVisibilityForAttachment()
    End Sub
    Private Sub Save()
        'Authentication
        If Not mWorkInvoice.Date Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------
                If DateDiff(DateInterval.Day, CDate(mWorkInvoice.Date), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save  Work Invoice. <br> Work Invoice Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        End If
        Dim WorkInvoiceClone As WorkInvoice
        WorkInvoiceClone = mWorkInvoice.Clone
        Try
            If Not mWorkInvoice.WorkInvoiceItems.Count = 0 Then
                setObject()
                setVendorDetails()
                Dim mWorkInvoiceCharge As WorkInvoiceCharge
                For Each mWorkInvoiceCharge In mWorkInvoice.WorkInvoiceCharges
                    If (mWorkInvoiceCharge.Sign <> 1 And mWorkInvoiceCharge.CChargeAmount <= 0) Or (Not (mWorkInvoiceCharge.IsValid)) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Work Invoice Charge(s) are not allowed if Work Invoice Amount Is Zero ", MsgBoxStyle.OkOnly, "")
                        mWorkInvoice.CancelEdit()
                        Exit Sub
                    End If
                Next
                mWorkInvoice.ApplyEdit()
                If mWorkInvoice.IsRoundOff = True Then  'Added By Prashant on 29-Oct-2012 ALL25102012
                    mWorkInvoice.RoundCGrandTotal()
                End If
                If (mWorkInvoice.IsNew) And (mWorkInvoice.Text = "") Then

                    Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mWorkInvoice.TransTypeID, mWorkInvoice.DateFormatted)

                    If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mWorkInvoice.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mWorkInvoice.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mWorkInvoice.TransTypeID).TransText = "")) Then

                        Dim str = "<script language='javascript'>openledgersame('wfWorkInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

                        Session("BackPagestr_ForTransSeries") = str

                        Session("TransName_ForTransSeries") = "WorkInvoice"
                        Session("TransTypeID_ForTransSeries") = mWorkInvoice.TransTypeID
                        Session("TransDate_ForTransSeries") = mWorkInvoice.DateFormatted
                        Session("AddTransTextSeries") = "True"
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    Else
                        Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                        If mAutoRenewTransTextSeries.IsRenewed Then
                            With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mWorkInvoice.TransTypeID)
                                mWorkInvoice.Text = .TransText
                                mWorkInvoice.No = .StartingTransNo
                            End With
                        Else
                            Dim str = "<script language='javascript'>openledgersame('wfWorkInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

                            Session("BackPagestr_ForTransSeries") = str

                            Session("TransName_ForTransSeries") = "WorkInvoice"
                            Session("TransTypeID_ForTransSeries") = mWorkInvoice.TransTypeID
                            Session("TransDate_ForTransSeries") = mWorkInvoice.DateFormatted
                            Session("AddTransTextSeries") = "True"
                            Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                        End If
                    End If
                End If
                'SaveAttachment()
                mWorkInvoice.Save()
                Session.Remove("mFileAttach")
                Dim WorkInvoiceDetail As String = mWorkInvoice.Text + " Dated : " + New SmartDate(mWorkInvoice.Date.ToString).FormattedText + " from " + mVendorList(mWorkInvoice.VendorID).Name

                If mWorkInvoice.StatusID = 2 Then
                    MarkLog(Util.Action.Authorize, "Work Invoice", WorkInvoiceDetail, Util.ErrorType.NoError, mWorkInvoice.ID, EventLogID)
                ElseIf mWorkInvoice.StatusID = 3 Then
                    MarkLog(Util.Action.Amend, "Work Invoice", WorkInvoiceDetail, Util.ErrorType.NoError, mWorkInvoice.ID, EventLogID)
                ElseIf mWorkInvoice.StatusID = 4 Then
                    MarkLog(Util.Action.Cancel, "Work Invoice", WorkInvoiceDetail, Util.ErrorType.NoError, mWorkInvoice.ID, EventLogID)
                Else
                    MarkLog(Util.Action.Save, "Work Invoice", WorkInvoiceDetail, Util.ErrorType.NoError, mWorkInvoice.ID, EventLogID)
                End If

                mWorkInvoice.MarkClean()
                Session("mWorkInvoice") = mWorkInvoice
                DataFieldBind()
                SetPage()
                ControlVisibility()
                SetGrid()
                SetChargeGrid()
                SetControlStatus(mWorkInvoice.StatusID)
                upnlTitle.Update()
                upnlWorkInvoiceDetails.Update()
                upnlVendorDetails.Update()
                upnlWorkInvoiceItem.Update()
                upnlWorkInvoiceTools.Update()
                upnlWorkInvoiceTerms.Update()
                upnlWorkInvoiceCharges.Update()
                upnlOtherDetails.Update()
                upnlButtons.Update()
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Work Invoice can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                mWorkInvoice = WorkInvoiceClone
                setObject()
                setVendorDetails()
                Session("mWorkInvoice") = mWorkInvoice
                DataFieldBind()
                Exit Sub
            End If
        Catch ex As SqlClient.SqlException
            Session("WorkInvoiceClone") = WorkInvoiceClone
            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 547 Then
                If InStr(ex.Message, "FKtabWorkInvoiceTermtabTerm", CompareMethod.Text) Then
                    Dim msg1 As New SIMsgBox(Page, "Term Deleted! ", "Term Not Avalable<Br><BR>Selected Term is no longer exist in the Database <BR><BR> Remove Term and try Again", " ", MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfWorkInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf InStr(ex.Message, "FKtabWorkInvoiceChargetabCharge", CompareMethod.Text) Then
                    MSGBoxCtrl.show("Alert!", "WorkInvoice Charge Deleted! ", "Work Invoice Charge Not Available<Br><BR>Selected Charge is no longer exist in the Database <BR><BR> Remove Charge and try Again", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        Finally
            WorkInvoiceClone = Nothing
        End Try
    End Sub
    Private Sub SetGrid()
        Dim P As Boolean
        For j As Integer = 0 To dgWorkInvoiceItems.Rows.Count - 1
            P = CType(Me.dgWorkInvoiceItems.Rows.Item(j).Cells(14).Text, Boolean)
            If P = False Then
                dgWorkInvoiceItems.Rows(j).Cells(13).Enabled = False
            End If
        Next
    End Sub
    Private Sub SetChargeGrid()
        For j As Integer = 0 To dgWorkInvoiceCharges.Rows.Count - 1
            If (Me.dgWorkInvoiceCharges.Rows.Item(j).Cells(2).Text = "Round off (Plus)" Or Me.dgWorkInvoiceCharges.Rows.Item(j).Cells(2).Text = "Round off (Minus)") Then
                dgWorkInvoiceCharges.Rows.Item(j).Cells(5).Enabled = False
                dgWorkInvoiceCharges.Rows.Item(j).Cells(6).Enabled = False
            End If
        Next
        upnlWorkInvoiceCharges.Update()
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mWorkInvoice.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = IIf(mWorkInvoice.StatusID > 1, False, True)
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mWorkInvoice.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mWorkInvoice.ID)
        End If
    End Sub
     Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        If mWorkInvoice.IsAttachmentAdded = True Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mWorkInvoice.FileAttachments(0).Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mWorkInvoice.FileAttachments(0).Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mWorkInvoice.FileAttachments(0).ImageFile, 0, mWorkInvoice.FileAttachments(0).ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

        mVendorList = VendorList.GetVendortList(0, , , , , , True, True, True, True)
        Session("mVendorList") = mVendorList
        cmbVendorList.DataSource = mVendorList

        mCurrencyList = CurrencyList.GetCurrencyList(, , True)
        Session("mCurrencyList") = mCurrencyList
        cmbCurrencyList.DataSource = mCurrencyList

        dgWorkInvoiceItems.DataSource = mWorkInvoice.WorkInvoiceItems
        dgWorkInvoiceCharges.DataSource = mWorkInvoice.WorkInvoiceCharges
        dgWorkInvoiceTerms.DataSource = mWorkInvoice.WorkInvoiceTerms
        dgWorkInvoiceTools.DataSource = mWorkInvoice.WorkInvoiceTools

        txtWorkInvoiceDate.Text = mWorkInvoice.DateFormatted.ToString
        txtRefDate.Text = mWorkInvoice.RefDateFormatted.ToString

        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtConversionFactor" Then
            If Val(txtConversionFactor.Text) <= 0 Then
                custValidator.ErrorMessage = "Currency factor must be greater than zero."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        addAttributes()
        SetControlStatus(mWorkInvoice.StatusID)
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then 'Added by Utkarsh on 21-Nov-2013 for Trans Text Series
                If mWorkInvoice.IsNew Then
                    mWorkInvoice.Text = Session("TransText_ForTransSeries")
                    txtText.Text = mWorkInvoice.Text
                    Session("mWorkInvoice") = mWorkInvoice
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If
            End If 'End
            DataFieldBind()
            SetPage()
            ControlVisibility()
            SetGrid()
            If chkIsRoundOff.Checked = True Then   'Added By Prashant on 29-Oct-2012
                SetChargeGrid()
            End If
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If IsValid Then
            setObject()
            setVendorDetails()
            mWorkInvoice.WorkInvoiceItems.Add(mWorkInvoice.ID)
            Session("mWorkInvoice") = mWorkInvoice
            Response.Redirect("wfWorkInvoiceItem_Ajax.aspx?BackPage=wfWorkInvoice_Ajax.aspx")
        Else
            upnlTitle.Update()
        End If
    End Sub
    Private Sub btnAddCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCharge.Click
        If IsValid Then
            setObject()
            setVendorDetails()
            mWorkInvoice.WorkInvoiceCharges.Add(mWorkInvoice.ID)
            Session("mWorkInvoice") = mWorkInvoice
            Response.Redirect("wfWorkInvoiceCharge_Ajax.aspx?BackPage=wfWorkInvoice_Ajax.aspx")
        Else
            upnlTitle.Update()
        End If
    End Sub
    Private Sub btnWorkInvoiceTools_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWorkInvoiceTools.Click
        If IsValid Then
            setObject()
            setVendorDetails()
            mWorkInvoice.WorkInvoiceTools.Add(mWorkInvoice.ID)
            Session("mWorkInvoice") = mWorkInvoice
            Response.Redirect("wfWorkInvoiceTool_Ajax.aspx?BackPage=wfWorkInvoice_Ajax.aspx&Type=6")
        Else
            upnlTitle.Update()
        End If
    End Sub
    Private Sub btnAddTerms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTerms.Click
        If IsValid Then
            setObject()
            setVendorDetails()
            Session("mWorkInvoice") = mWorkInvoice
            Response.Redirect("wfWorkInvoiceTerm_Ajax.aspx?BackPage=wfWorkInvoice_Ajax.aspx&Type=6")
        End If
    End Sub
    Private Sub dgWorkInvoiceItems_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWorkInvoiceItems.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim index As Int32 = CInt(e.CommandArgument) + dgWorkInvoiceItems.PageIndex * dgWorkInvoiceItems.PageSize
                Session("Edit") = True
                setObject()
                setVendorDetails()
                mWorkInvoice.WorkInvoiceItems.CurrentIndex = index
                Session("mWorkInvoice") = mWorkInvoice
                Response.Redirect("wfWorkInvoiceItem_Ajax.aspx?BackPage=wfWorkInvoice_Ajax.aspx")
            Case "DeleteRecord"
                Dim index As Int32 = CInt(e.CommandArgument) + dgWorkInvoiceItems.PageIndex * dgWorkInvoiceItems.PageSize
                DeleteRecord(index)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim index1 As Int32 = CInt(e.CommandArgument) + dgWorkInvoiceItems.PageIndex * dgWorkInvoiceItems.PageSize
                mWorkInvoiceItem = mWorkInvoice.WorkInvoiceItems(index1)
                If mWorkInvoiceItem.IsAttachmentAdded Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mWorkInvoiceItem.FileAttachments.CurrentItem.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mWorkInvoiceItem.FileAttachments.CurrentItem.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mWorkInvoiceItem.FileAttachments.CurrentItem.ImageFile, 0, mWorkInvoiceItem.FileAttachments.CurrentItem.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        Dim Str1 As String
                        Str1 = "openFile();"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str1, True)
                    End If
                Else
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
        End Select
    End Sub
    Private Sub dgWorkInvoiceCharges_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWorkInvoiceCharges.RowCommand
        Select Case e.CommandName
            Case "EditCharge"
                Dim index As Int32 = CInt(e.CommandArgument) + dgWorkInvoiceCharges.PageIndex * dgWorkInvoiceCharges.PageSize
                Session("EditCharge") = True
                setObject()
                setVendorDetails()
                mWorkInvoice.WorkInvoiceCharges.CurrentIndex = index
                Session("mWorkInvoice") = mWorkInvoice
                Response.Redirect("wfWorkInvoiceCharge_Ajax.aspx?BackPage=wfWorkInvoice_Ajax.aspx")
            Case "DeleteCharge"
                Dim index As Int32 = CInt(e.CommandArgument) + dgWorkInvoiceCharges.PageIndex * dgWorkInvoiceCharges.PageSize
                DeleteChargeRecord(index)
        End Select
    End Sub
    Private Sub dgWorkInvoiceTerms_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWorkInvoiceTerms.RowCommand
        Select Case e.CommandName
            Case "DeleteTerm"
                Dim index As Int32 = CInt(e.CommandArgument) + dgWorkInvoiceItems.PageIndex * dgWorkInvoiceItems.PageSize
                mWorkInvoice.WorkInvoiceTerms.CurrentIndex = index
                mWorkInvoice.WorkInvoiceTerms.Remove(mWorkInvoice.WorkInvoiceTerms.CurrentItem)
                Session("mWorkInvoice") = mWorkInvoice
                dgWorkInvoiceTerms.DataSource = mWorkInvoice.WorkInvoiceTerms
                dgWorkInvoiceTerms.DataBind()
                upnlWorkInvoiceTerms.Update()
        End Select
    End Sub
    Private Sub dgWorkInvoiceTools_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWorkInvoiceTools.RowCommand
        Select Case e.CommandName
            Case "EditTool"
                Dim index As Int32 = CInt(e.CommandArgument) + dgWorkInvoiceTools.PageIndex * dgWorkInvoiceTools.PageSize
                Session("EditTools") = True
                setObject()
                setVendorDetails()
                mWorkInvoice.WorkInvoiceTools.CurrentIndex = index
                Session("mWorkInvoice") = mWorkInvoice
                Response.Redirect("wfWorkInvoiceTool_Ajax.aspx?BackPage=wfWorkInvoice_Ajax.aspx")
            Case "DeleteTool"
                Dim index As Int32 = CInt(e.CommandArgument) + dgWorkInvoiceTools.PageIndex * dgWorkInvoiceTools.PageSize
                DeleteWorkInvoiceTools(index)
        End Select
    End Sub
    Private Sub cmbVendorList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbVendorList.SelectedIndexChanged
        txtAddress.Text = mVendorList(cmbVendorList.SelectedIndex).Address
        If cmbVendorList.Enabled = True Then
            setFocus(cmbVendorList)
        End If
    End Sub
    Private Sub cmbCurrencyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
        txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
        If cmbCurrencyList.Enabled = True Then
            setFocus(cmbCurrencyList)
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("WorkInvoiceNew") And Not User.IsInRole("WorkInvoiceEdit")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If IsValid Then
            Save()
        Else
            upnlTitle.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim WorkInvoiceDetail As String = mWorkInvoice.Text + " Dated : " + New SmartDate(mWorkInvoice.Date.ToString).FormattedText + " from " + mVendorList(mWorkInvoice.VendorID).Name
        MarkLog(Util.Action.Close, "Work Invoice", WorkInvoiceDetail, Util.ErrorType.NoError, mWorkInvoice.ID, EventLogID)
        setObject()
        setVendorDetails()
        If mWorkInvoice.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
        Else
            Session.Remove("mSelectList")
            Session.Remove("mFileAttach")
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim rpt As New crptWorkInvoiceDetail
        Dim mCompanyDetail As New CompanyDetail

        Dim obj As rptWorkInvoice
        Dim objChilds As rptWorkInvoiceChilds
        Dim ds As New dsWorkInvoice
        obj = rptWorkInvoice.GetWorkInvoice(mWorkInvoice.ID)
        objChilds = rptWorkInvoiceChilds.GetrptWorkInvoiceChilds(mWorkInvoice.ID)

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, "", "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))


        da.Fill(ds, obj)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, objChilds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        rpt.SetDataSource(ds)
        Session("CrystalReport") = rpt

        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        If mWorkInvoice.IsAttachmentAdded Then
            mWorkInvoice.FileAttachments(0).Size = mFileAttach.Size
            mWorkInvoice.FileAttachments(0).ImageFile = mFileAttach.ImageFile
            mWorkInvoice.FileAttachments(0).Extension = mFileAttach.Extension
        Else
            mWorkInvoice.IsAttachmentAdded = True
            mWorkInvoice.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
        End If
        ControlVisibilityForAttachment()
        upnlAttachFile.Update()
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mWorkInvoice.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachmentChild(mWorkInvoice.ID)
        Else
            mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mWorkInvoice.ID)
        End If
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        GetAttachment()

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        mWorkInvoice.IsAttachmentAdded = False
        mWorkInvoice.FileAttachments.Remove(mWorkInvoice.ID)
        Session("mWorkInvoice") = mWorkInvoice
    End Sub
    Private Sub chkIsRoundOff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsRoundOff.CheckedChanged
        Dim Child As WorkInvoiceCharge
        For i As Integer = mWorkInvoice.WorkInvoiceCharges.Count - 1 To 0 Step -1
            Child = mWorkInvoice.WorkInvoiceCharges(i)
            If Child.ChargeID.Equals(New Guid("{40000000-0000-0000-0000-000000000000}")) Or Child.ChargeID.Equals(New Guid("{50000000-0000-0000-0000-000000000000}")) Then
                mWorkInvoice.WorkInvoiceCharges.Remove(Child)
            End If
        Next
        dgWorkInvoiceCharges.DataSource = mWorkInvoice.WorkInvoiceCharges
        dgWorkInvoiceCharges.DataBind()
        upnlWorkInvoiceCharges.Update()
    End Sub
    Private Sub txtWorkInvoiceDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWorkInvoiceDate.TextChanged
        mWorkInvoice = Session("mWorkInvoice")

        mWorkInvoice.Date = txtWorkInvoiceDate.Text
        txtText.Text = mWorkInvoice.Text
        txtText.DataBind()
        upnlWorkInvoiceDetails.Update()
        Session("mWorkInvoice") = mWorkInvoice
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

#Region " Status "
    Private Sub btnAuthorized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
        If IsValid Then
            MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<Strong>  Work Invoice </Strong>", MsgBoxStyle.YesNo, "Status")
            Session("IsValid") = IsValid
            Session("mWorkInvoice") = mWorkInvoice
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<Strong> Work Invoice </Strong>", MsgBoxStyle.YesNo, "StatusCancel")
        Session("mWorkInvoice") = mWorkInvoice
    End Sub
#End Region

#Region " Show BrokenRules "
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        setObject()
        If Not mWorkInvoice.IsValid Then
            For i As Integer = 0 To mWorkInvoice.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mWorkInvoice.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        Dim mWorkInvoiceItem As WorkInvoiceItem
        If Not mWorkInvoice.WorkInvoiceItems.IsValid Then
            For Each mWorkInvoiceItem In mWorkInvoice.WorkInvoiceItems
                For i As Integer = 0 To mWorkInvoiceItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mWorkInvoiceItem.TaskDescription + " : " + mWorkInvoiceItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If

        If strMsg.Trim <> "" Then
            cvWorkInvoiceDate.ErrorMessage = strMsg
            cvWorkInvoiceDate.IsValid = False
            Return False
        End If
        Return True
    End Function
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub

        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        Dim strMsg As String = ""
        setObject()

        If Not mWorkInvoice.IsValid Then
            For i As Integer = 0 To mWorkInvoice.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mWorkInvoice.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If

        Dim mWorkInvoiceItem As WorkInvoiceItem
        If Not mWorkInvoice.WorkInvoiceItems.IsValid Then
            For Each mWorkInvoiceItem In mWorkInvoice.WorkInvoiceItems
                For i As Integer = 0 To mWorkInvoiceItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mWorkInvoiceItem.TaskDescription + " : " + mWorkInvoiceItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If

        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            e.IsValid = False
        End If

        Flag = 1
    End Sub
#End Region

#Region " Service Methods "
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