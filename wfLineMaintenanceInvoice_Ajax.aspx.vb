'***********************************
'Modified by Harsh Sugandhi on 22nd April 2025 for FLYPAL 2334 => Facility to attach a file to Service Module. 
'***********************************


Imports System.Linq


Public Class wfLineMaintenanceInvoice_Ajax
    Inherits Page


#Region " Variable Declaration "

    Public mLineMaintInvoice As LineMaintenanceInvoice
    Public mVendorList As VendorList
    Public mCurrencyList As CurrencyList
    Public mMachineNameValueList As MachineNameValueList
    Dim EventLogID As Guid
    Dim InvDetail As String
    Public mModuleName As String = "LineMaintenanceInvoice"
    Public mLocationList As LocationList
    Public mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False

#End Region

#Region " Business Methods "

    Private Sub getSession()
        mLineMaintInvoice = Session("mLineMaintInvoice")
        mVendorList = Session("mVendorList")
        mCurrencyList = Session("mCurrencyList")
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mLocationList = Session("mLocationList")
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
    End Sub

    Private Sub RemoveSession()
        Session.Remove("LineMaintOrderID")
        Session.Remove("mCurrencyList")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mLocationList")
        Session.Remove("mLineMaintInvoice")
        Session.Remove("mVendorList")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub

    Private Sub SetSession()
        Session("mLineMaintInvoice") = mLineMaintInvoice
        Session("mVendorList") = mVendorList
        Session("mCurrencyList") = mCurrencyList
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub

    Private Sub SetPage()
        If mLineMaintInvoice.IsNew Then
            lblTitle.Text = "Service Invoice [New]"
        Else
            lblTitle.Text = "Service Invoice [ " & mLineMaintInvoice.LineMaintInvoiceNo & " ]"
        End If
    End Sub

    Private Sub DeleteRecord(Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentIndex = Index
        Session("mLineMaintInvoice") = mLineMaintInvoice
    End Sub

    Private Sub DeleteCharge(index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveCharge, MSGBox.Message_text.RemoveCharge, "", MsgBoxStyle.YesNo, "DeleteCharge")
        mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentIndex = index
        Session("mLineMaintInvoice") = mLineMaintInvoice
    End Sub

    Private Sub SetControlStatus(StatusId As Int16)

        txtInvoiceDate.Enabled = (CType(IIf(mLineMaintInvoice.StatusID >= 2 Or mLineMaintInvoice.StatusID = 4, False, True), Boolean) And mLineMaintInvoice.LineMaintenanceInvoiceItems.Count = 0) Or (mLineMaintInvoice.LineMaintenanceInvoiceItems.Count = 0)
        txtInvoiceText.Enabled = CType(IIf(mLineMaintInvoice.StatusID >= 2, False, True), Boolean)
        txtInvoiceNo.Enabled = CType(IIf(mLineMaintInvoice.StatusID >= 2, False, True), Boolean)
        txtVendorInvNo.Enabled = CType(IIf(mLineMaintInvoice.StatusID >= 2, False, True), Boolean)
        txtVendorInvDate.Enabled = CType(IIf(mLineMaintInvoice.StatusID >= 2, False, True), Boolean)
        txtConversionFactor.Enabled = CType(IIf(mLineMaintInvoice.StatusID >= 2, False, True), Boolean)
        btnCancel.Visible = (Not mLineMaintInvoice.IsNew) And (mLineMaintInvoice.StatusID = 2)
        btnAuthorized.Visible = (Not mLineMaintInvoice.LineMaintenanceInvoiceItems.Count = 0) And (Not mLineMaintInvoice.IsNew) And (mLineMaintInvoice.StatusID = 1)
        chkIsRoundOff.Enabled = (mLineMaintInvoice.StatusID = 1)
        btnAdd.Enabled = IIf(StatusId > 1, False, True)
        btnAddCharges.Enabled = IIf(StatusId > 1, False, True)
        btnAddTerm.Enabled = IIf(StatusId > 1, False, True)
        btnSave.Visible = IIf(StatusId > 1, False, True)
        dgLineMaintInvoice.Columns(9).Visible = IIf(StatusId > 1, False, True)
        dgLineMaintInvoiceCharge.Columns(4).Visible = IIf(StatusId > 1, False, True)
        dgLineMaintInvoiceTerm.Columns(2).Visible = IIf(StatusId > 1, False, True)

        If User.IsInRole("LineMaintenanceInvoiceAuthorized") = False Then
            btnAuthorized.Enabled = False
            btnAuthorized.ToolTip = "You are not authorized user."
            btnCancel.Enabled = False
            btnCancel.ToolTip = "You are not authorized user."
        End If

        ControlAttachmentICONVisibility(StatusId)

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
                            Dim mLineMaintInvoice As LineMaintenanceInvoice
                            mLineMaintInvoice = CType(Session("mLineMaintInvoice"), LineMaintenanceInvoice)
                            mLineMaintInvoice.LineMaintenanceInvoiceItems.Remove(mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentItem)
                            mLineMaintInvoice.CalculateTotal()
                            If mLineMaintInvoice.IsRoundOff = True Then
                                mLineMaintInvoice.RoundCGrandTotal()
                            End If
                            Session("mLineMaintInvoice") = mLineMaintInvoice
                            SetControlStatus(mLineMaintInvoice.StatusID)
                            dgLineMaintInvoice.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceItems
                            dgLineMaintInvoice.DataBind()
                            dgLineMaintInvoiceCharge.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceCharges
                            dgLineMaintInvoiceCharge.DataBind()
                            SetChargeGrid()
                            upnlInvoiceDetails.Update()
                            'upnlSupplierDetails.Update()
                            upnlInvoiceCharge.Update()
                            upnlOtherDetails.Update()
                            upnlInvoiceItem.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "DeleteCharge" Then
                        Try
                            Session("Sender") = ""
                            Dim mLineMaintInvoice As LineMaintenanceInvoice
                            mLineMaintInvoice = CType(Session("mLineMaintInvoice"), LineMaintenanceInvoice)
                            mLineMaintInvoice.LineMaintenanceInvoiceCharges.Remove(mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentItem)

                            mLineMaintInvoice.CalculateTotal()
                            If mLineMaintInvoice.IsRoundOff = True Then
                                mLineMaintInvoice.RoundCGrandTotal()
                            End If
                            Session("mLineMaintInvoice") = mLineMaintInvoice
                            dgLineMaintInvoiceCharge.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceCharges
                            dgLineMaintInvoiceCharge.DataBind()
                            SetChargeGrid()
                            upnlInvoiceCharge.Update()
                            upnlOtherDetails.DataBind()
                            upnlOtherDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                        '----------------------------------------------------------------
                    ElseIf MSGBoxCtrl.Sender = "Close" Then
                        Session("sender") = ""
                        If Session("IsValid") Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not User.IsInRole("LineMaintenanceInvoiceNew") And Not User.IsInRole("LineMaintenanceInvoiceEdit")) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            If Save() Then
                                RemoveSession()
                                Response.Redirect("Index.aspx")
                            End If
                        Else
                            Session.Remove("IsValid")
                            'Response.Redirect("wfLineMaintenanceInvoice.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If Session("IsValid") Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If Save() Then

                                'Response.Redirect("wfLineMaintenanceInvoice.aspx?BackPage=" & Request.QueryString("BackPage"))
                            End If
                        Else
                            Session.Remove("IsValid")
                            'Response.Redirect("wfLineMaintenanceInvoice.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                        ''========================
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        If mLineMaintInvoice.StatusID = 2 Then
                            mLineMaintInvoice.StatusID = 1
                        ElseIf mLineMaintInvoice.StatusID = 4 Then
                            mLineMaintInvoice.StatusID = 2
                        End If
                        Session("mLineMaintInvoice") = mLineMaintInvoice
                    ElseIf MSGBoxCtrl.Sender = "DeleteCharge" Then
                        dgLineMaintInvoiceCharge.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceCharges
                        dgLineMaintInvoiceCharge.DataBind()
                        SetChargeGrid()
                        upnlInvoiceCharge.Update()
                    Else
                        Session("Sender") = ""
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("Sender") = ""
                        If mLineMaintInvoice.StatusID = 2 Then
                            mLineMaintInvoice.StatusID = 1
                        ElseIf mLineMaintInvoice.StatusID = 4 Then
                            mLineMaintInvoice.StatusID = 2
                        End If
                        Session("mLineMaintInvoice") = mLineMaintInvoice
                        upnlStatus.Update()
                        'upnlInvoiceDetails.Update()
                        'upnlSupplierDetails.Update()
                        'Added by Utkarsh On 22-Nov-2013 For TransTextSeries
                    ElseIf MSGBoxCtrl.Sender = "LineMaintInvoiceransTextSeriesAlert" Then
                        Session("sender") = ""
                        Session("AddTransTextSeries") = "True"
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                        'ENd
                    Else
                        Session("sender") = ""
                        'DataFieldBind()
                        'Response.Redirect("wfLineMaintenanceInvoice.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            'If mLineMaintInvoice.StatusID = 2 And Session("sender") <> "Close" Then
            '    mLineMaintInvoice.StatusID = 1
            'ElseIf mLineMaintInvoice.StatusID = 4 Then
            '    mLineMaintInvoice.StatusID = 2
            'End If
            'Session("sender") = ""
            'Session("mInvoice") = mLineMaintInvoice
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub

    Private Sub AddAttributes()
        txtInvoiceNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtInvoiceNo').value,event)")
    End Sub

    Private Sub SetObject()

        Try

            mLineMaintInvoice.LineMaintenanceInvoiceDate = CDate(txtInvoiceDate.Text)
            mLineMaintInvoice.Text = txtInvoiceText.Text
            mLineMaintInvoice.No = Val(txtInvoiceNo.Text)
            mLineMaintInvoice.UserName = User.Identity.Name
            mLineMaintInvoice.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
            mLineMaintInvoice.ConversionFactor = Val(txtConversionFactor.Text)
            mLineMaintInvoice.IsRoundOff = chkIsRoundOff.Checked
            mLineMaintInvoice.CalculateTotal()
            mLineMaintInvoice.VendorID = New Guid(cmbVendorList.SelectedValue)
            mLineMaintInvoice.MachineID = New Guid(cmbAircraft.SelectedValue)
            mLineMaintInvoice.LocationID = New Guid(cmbLocation.SelectedValue)
            mLineMaintInvoice.VendorInvoiceNo = Trim(txtVendorInvNo.Text)

            If txtVendorInvDate.Text = "" Then
                mLineMaintInvoice.VendorInvoiceDate = System.DBNull.Value
            Else
                mLineMaintInvoice.VendorInvoiceDate = CDate(txtVendorInvDate.Text)
            End If

            If mFileAttach IsNot Nothing Then

                If mFileAttach.Size > 0 Then
                    mLineMaintInvoice.IsAttachmentAdded = True
                Else
                    mLineMaintInvoice.IsAttachmentAdded = False
                End If

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Function Save() As Boolean

        'Authentication
        If Not mLineMaintInvoice.LineMaintenanceInvoiceDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------

                If DateDiff(DateInterval.Day, CDate(mLineMaintInvoice.LineMaintenanceInvoiceDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Your subscription has been expired. can not save Service Invoice." + "\n" + "Invoice Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Return False
                End If
            End If
        End If

        'Authentication
        Dim msgCnt As Integer = 0
        Dim InvoiceClone As LineMaintenanceInvoice
        InvoiceClone = mLineMaintInvoice.Clone

        Try

            If Not mLineMaintInvoice.LineMaintenanceInvoiceItems.Count = 0 Then

                SetObject()

                Dim LinemaintInvoiceCharge As LineMaintenanceInvoiceCharge

                For Each LinemaintInvoiceCharge In mLineMaintInvoice.LineMaintenanceInvoiceCharges

                    If (LinemaintInvoiceCharge.Sign <> 1 And LinemaintInvoiceCharge.CChargeAmount <= 0) Or (Not (LinemaintInvoiceCharge.IsValid)) Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert,
                                        MSGBox.Message_text.ValidationAlert,
                                        "Percentage Invoice Charge(s) are not allowed if Invoice Amount Is Zero. ",
                                        MsgBoxStyle.OkOnly,
                                        "")

                        mLineMaintInvoice.CancelEdit()

                        Return False

                    End If

                Next
                '------------------------------------------------------------------------

                If mLineMaintInvoice.IsRoundOff = True Then  'Added By Prashant on 29-Oct-2012
                    mLineMaintInvoice.RoundCGrandTotal()
                End If

                'Added by Utkarsh ON 22-Nov-2013 FOr TransTextSeries
                'Check if Service Invoice is blank then call TransTextSeries UI

                If (mLineMaintInvoice.IsNew) And (mLineMaintInvoice.Text = "") Then

                    Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mLineMaintInvoice.TransTypeID, mLineMaintInvoice.LineMaintenanceInvoiceDateFormatted)

                    If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mLineMaintInvoice.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mLineMaintInvoice.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mLineMaintInvoice.TransTypeID).TransText = "")) Then

                        Dim str = "<script language='javascript'>openledgersame('wfLineMaintenanceInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

                        Session("BackPagestr_ForTransSeries") = str

                        Session("TransName_ForTransSeries") = "LineMaintInvoice"
                        Session("TransTypeID_ForTransSeries") = mLineMaintInvoice.TransTypeID
                        Session("TransDate_ForTransSeries") = mLineMaintInvoice.LineMaintenanceInvoiceDateFormatted

                        MSGBoxCtrl.Show("Line Maint. Invoice Transaction Series",
                                        "system does not find transaction series for this transaction. 
                                         Click Ok to enter transaction series.",
                                        "",
                                        MsgBoxStyle.OkOnly,
                                        "LineMaintInvoiceransTextSeriesAlert")

                        Exit Function

                    Else

                        Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                        If mAutoRenewTransTextSeries.IsRenewed Then

                            With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mLineMaintInvoice.TransTypeID)
                                mLineMaintInvoice.Text = .TransText
                                mLineMaintInvoice.No = .StartingTransNo
                            End With

                        Else

                            Dim str = "<script language='javascript'>openledgersame('wfLineMaintenanceInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

                            Session("BackPagestr_ForTransSeries") = str
                            Session("TransName_ForTransSeries") = "LineMaintInvoice"
                            Session("TransTypeID_ForTransSeries") = mLineMaintInvoice.TransTypeID
                            Session("TransDate_ForTransSeries") = mLineMaintInvoice.LineMaintenanceInvoiceDateFormatted

                            MSGBoxCtrl.Show("Line Maint. Invoice Transaction Series",
                                            "system does not find transaction series for this transaction. 
                                             Click Ok to enter transaction series.",
                                            "",
                                            MsgBoxStyle.OkOnly,
                                            "LineMaintInvoiceransTextSeriesAlert")

                            Exit Function

                        End If

                    End If

                End If
                'End

                mLineMaintInvoice.Save()
                SaveAttachment()

                InvDetail = mLineMaintInvoice.LineMaintInvoiceNo + " Dated : " + mLineMaintInvoice.LineMaintenanceInvoiceDateFormatted + " from " + mVendorList(mLineMaintInvoice.VendorID).Name

                Select Case mLineMaintInvoice.StatusID
                    Case 1

                        MarkLog(Action.Save,
                                "LineMaintenanceInvoice",
                                InvDetail,
                                ErrorType.NoError,
                                mLineMaintInvoice.ID,
                                EventLogID)

                    Case 2

                        MarkLog(Action.Authorize,
                                "LineMaintenanceInvoice",
                                InvDetail & " Authorized By : " & mLineMaintInvoice.AuthorizedBy,
                                ErrorType.NoError,
                                mLineMaintInvoice.ID,
                                EventLogID)

                    Case 3

                        MarkLog(Action.Amend, "LineMaintenanceInvoice",
                                InvDetail,
                                ErrorType.NoError,
                                mLineMaintInvoice.ID,
                                EventLogID)

                    Case 4

                        MarkLog(Action.Cancel,
                                "LineMaintenanceInvoice",
                                InvDetail,
                                ErrorType.NoError,
                                mLineMaintInvoice.ID,
                                EventLogID)

                End Select
                'End

                SetPage()

                Session("mLineMaintInvoice") = mLineMaintInvoice
                If mLineMaintInvoice.StatusID = 1 And mLineMaintInvoice.IsNew = False Then
                    lblStatus.Text = "OPENED"
                End If

                DataFieldBind()
                SetControlStatus(mLineMaintInvoice.StatusID)

                If chkIsRoundOff.Checked = True Then
                    SetChargeGrid()
                End If

                upnlTitle.Update()
                upnlStatus.Update()
                upnlInvoiceDetails.Update()
                upnlSupplierDetails.Update()
                upnlInvoiceItem.Update()
                upnlInvoiceCharge.Update()
                upnlOtherDetails.Update()
                upnlActionBtn.Update()
                upnlInvoiceTerm.Update()
                upnlFileAttachmentButtons.Update()

                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully,
                                MSGBox.Message_text.SavedSuccessFully,
                                "",
                                MsgBoxStyle.OkOnly,
                                "")

            Else

                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
                                MSGBox.Message_text.saveAlert,
                                "Invoice can not be saved without Item.",
                                MsgBoxStyle.OkOnly,
                                "")

                mLineMaintInvoice = InvoiceClone
                SetObject()
                Session("mLineMaintInvoice") = mLineMaintInvoice

                Return False

            End If

        Catch ex As SqlException

            Session("InvoiceClone") = InvoiceClone

            If ex.Number = 8114 Or ex.Number = 8115 Then

                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow,
                                MSGBox.Message_text.NumericOverFlow,
                                " Rate or Qty or Conversion Factor. ",
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Function

            ElseIf ex.Number = 8145 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.ProcedureError,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Function

            ElseIf ex.Number = 2627 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.Duplicate,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Function

            ElseIf ex.Number = 547 Then

                If InStr(ex.Message, "CCtabReceiptItemInvoiceBalanceQty", CompareMethod.Text) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                    MSGBox.Message_text.PendingQty,
                                    "Qty. Not Available",
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf InStr(ex.Message, "FKtabInvoiceChargetabCharge", CompareMethod.Text) Then

                    MSGBoxCtrl.Show("Other Charge Deleted ! ",
                                    "Other Charge Not Avalable<Br>
                                     <BR>Selected Charge is no longer exist in the Database <BR><BR> Remove Charge and try Again",
                                    " ",
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf InStr(ex.Message, "CCtabInvoiceNo", CompareMethod.Text) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
                                    MSGBox.Message_text.saveAlert,
                                    "No. Required",
                                    MsgBoxStyle.OkOnly,
                                    "")

                Else

                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                    MSGBox.Message_text.ReferenceDelete,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "")

                End If

            End If

            mLineMaintInvoice = InvoiceClone
            Session("mLineMaintInvoice") = mLineMaintInvoice
            msgCnt = ex.Number

        Finally
            InvoiceClone = Nothing
        End Try

        If msgCnt = 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub SetChargeGrid()
        For j As Integer = 0 To dgLineMaintInvoiceCharge.Rows.Count - 1
            If (Me.dgLineMaintInvoiceCharge.Rows(j).Cells(1).Text = "Round off (Plus)" Or Me.dgLineMaintInvoiceCharge.Rows(j).Cells(1).Text = "Round off (Minus)") Then
                dgLineMaintInvoiceCharge.Rows(j).Cells(4).Visible = False
            End If
        Next
    End Sub

    Private Sub ControlAttachmentICONVisibility(StatusId As Integer)

        Try

            btnSelectFile.Disabled = IIf(StatusId = 2, True, False)
            btnRemoveAttach.Enabled = IIf(mLineMaintInvoice.IsAttachmentAdded AndAlso StatusId <> 2, True, False)
            AttachmentIcon.Visible = IIf(mLineMaintInvoice.IsAttachmentAdded, True, False)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SaveAttachment()

        Try

            If mFileAttach IsNot Nothing Then

                If mFileAttach.Size > 0 Then

                    Try

                        mFileAttach.Save()

                    Catch ex As Exception

                        ScriptManager.RegisterClientScriptBlock(Me,
                                                                [GetType],
                                                                "",
                                                                MessageBox.Show(ex.InnerException.ToString, False),
                                                                True)
                    End Try

                Else

                    If (Not mLineMaintInvoice.IsNew) And IsAttachmentDeleted Then

                        FileAttach.DeleteAttachment(ID:=mFileAttach.ID,
                                                    ReferenceID:=mLineMaintInvoice.ID)

                    End If

                    IsAttachmentDeleted = False
                    Session("IsAttachmentDeleted") = IsAttachmentDeleted

                End If

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()
        mCurrencyList = CurrencyList.GetCurrencyList(, , True)
        mVendorList = VendorList.GetVendortList(0, , , , , , True, False, True)

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , True, "(SELECT)", ForInventory:=True, SkipIsForInventoryAircarft:=True)
        cmbAircraft.DataSource = mMachineNameValueList

        mLocationList = LocationList.GetLocationList(0, , , , , , True)
        Session("mLocationList") = mLocationList

        cmbVendorList.DataSource = mVendorList
        cmbCurrencyList.DataSource = mCurrencyList

        cmbLocation.DataSource = mLocationList
        Session("mCurrencyList") = mCurrencyList
        Session("mVendorList") = mVendorList
        Session("mMachineNameValueList") = mMachineNameValueList

        dgLineMaintInvoice.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceItems
        dgLineMaintInvoiceCharge.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceCharges
        dgLineMaintInvoiceTerm.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceTerms
        txtInvoiceDate.Text = mLineMaintInvoice.LineMaintenanceInvoiceDateFormatted

        DataBind()
    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        getSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        AddAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            'Added by Utkarsh on 22-Nov-2013 for Trans Text Series
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If mLineMaintInvoice.IsNew Then
                    mLineMaintInvoice.Text = Session("TransText_ForTransSeries")
                    txtInvoiceText.Text = mLineMaintInvoice.Text
                    Session("mLineMaintInvoice") = mLineMaintInvoice
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If
            End If
            'End
            DataFieldBind()
            If mLineMaintInvoice.StatusID = 1 And mLineMaintInvoice.IsNew = False Then
                lblStatus.Text = "OPENED"
            End If
            SetPage()
            SetControlStatus(mLineMaintInvoice.StatusID)
            If chkIsRoundOff.Checked = True Then
                SetChargeGrid()
            End If
        End If
    End Sub

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        If IsValid Then
            SetObject()
            Session("mLineMaintInvoice") = mLineMaintInvoice

            If mLineMaintInvoice.LineMaintenanceInvoiceItems.Count > 0 Then
                Dim mLineMaintOrderID As Guid = LineMaintenanceOrderID.GetLineMaintenanceOrderID(mLineMaintInvoice.LineMaintenanceInvoiceItems(0).LineMaintOrderItemID).LineMaintenanceOrderID
                Session("LineMaintOrderID") = mLineMaintOrderID
            Else
                Session("LineMaintOrderID") = Guid.Empty
            End If
            Response.Redirect("wfPendingLineMaintenanceOrderList_Ajax.aspx?BackPage=wfLineMaintenanceInvoice_Ajax.aspx")
        End If
    End Sub

    Private Sub btnAddCharge_Click(sender As System.Object, e As System.EventArgs) Handles btnAddCharges.Click
        If IsValid Then
            SetObject()
            mLineMaintInvoice.LineMaintenanceInvoiceCharges.Add(mLineMaintInvoice.ID)
            Session("mLineMaintInvoice") = mLineMaintInvoice
            'Response.Redirect("wfLineMaintenanceInvoiceCharge_Ajax.aspx?BackPage=wfLineMaintenanceInvoice_Ajax.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenLineMaintenanceInvoiceChargeWindow", "OpenLineMaintenanceInvoiceChargeWindow();", True)
        End If
    End Sub

    Private Sub btnAddTerms_Click(sender As System.Object, e As System.EventArgs) Handles btnAddTerm.Click
        SetObject()
        Session("mLineMaintInvoice") = mLineMaintInvoice
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTermWindow", "OpenTermWindow()", True)
        'Response.Redirect("wfLineMaintenanceInvoiceTerm_Ajax.aspx?BackPage=wfLineMaintenanceInvoice_Ajax.aspx&Type=8")
    End Sub

    Private Sub btnBack_Click(sender As System.Object, e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, mModuleName, "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        SetObject()
        Session("IsValid") = IsValid
        If mLineMaintInvoice.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            RemoveSession()
            Response.Redirect("Index.aspx")
        End If
    End Sub

    Private Sub dgLineMaintInvoice_RowCommand(source As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLineMaintInvoice.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "EditView"
                dgLineMaintInvoice.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceItems
                dgLineMaintInvoice.DataBind()
                Index = CInt(e.CommandArgument) + dgLineMaintInvoice.PageIndex * dgLineMaintInvoice.PageSize
                Session("Edit") = True
                SetObject()
                mLineMaintInvoice.LineMaintenanceInvoiceItems.CurrentIndex = Index
                Session("mLineMaintInvoice") = mLineMaintInvoice
                Response.Redirect("wfLineMaintenanceInvoiceItem_Ajax.aspx?BackPage=wfLineMaintenanceInvoice_Ajax.aspx")
            Case "DeleteRecord"
                dgLineMaintInvoice.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceItems
                dgLineMaintInvoice.DataBind()
                Index = CInt(e.CommandArgument) + dgLineMaintInvoice.PageIndex * dgLineMaintInvoice.PageSize
                DeleteRecord(Index)
        End Select
    End Sub

    Private Sub dgLineMaintInvoiceCharge_RowCommand(source As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLineMaintInvoiceCharge.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "EditCharge"
                dgLineMaintInvoiceCharge.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceCharges
                dgLineMaintInvoiceCharge.DataBind()
                Index = CInt(e.CommandArgument) + dgLineMaintInvoiceCharge.PageIndex * dgLineMaintInvoiceCharge.PageSize
                Session("Edit") = True
                SetObject()
                mLineMaintInvoice.LineMaintenanceInvoiceCharges.CurrentIndex = Index
                Session("mLineMaintInvoice") = mLineMaintInvoice
                'Response.Redirect("wfLineMaintenanceInvoiceCharge_Ajax.aspx?BackPage=wfLineMaintenanceInvoice_Ajax.aspx")
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenLineMaintenanceInvoiceChargeWindow", "OpenLineMaintenanceInvoiceChargeWindow();", True)
            Case "DeleteCharge"
                dgLineMaintInvoiceCharge.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceCharges
                dgLineMaintInvoiceCharge.DataBind()
                Index = CInt(e.CommandArgument) + dgLineMaintInvoiceCharge.PageIndex * dgLineMaintInvoiceCharge.PageSize
                DeleteCharge(Index)
        End Select
    End Sub

    Private Sub dgLineMaintInvoiceTerm_RowCommand(source As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLineMaintInvoiceTerm.RowCommand
        Dim Index As Int32

        Select Case e.CommandName
            Case "DeleteTerm"
                dgLineMaintInvoiceTerm.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceTerms
                dgLineMaintInvoiceTerm.DataBind()
                Index = CInt(e.CommandArgument) + dgLineMaintInvoiceTerm.PageIndex * dgLineMaintInvoiceTerm.PageSize
                mLineMaintInvoice.LineMaintenanceInvoiceTerms.CurrentIndex = Index
                mLineMaintInvoice.LineMaintenanceInvoiceTerms.Remove(mLineMaintInvoice.LineMaintenanceInvoiceTerms.CurrentItem)
                Session("mLineMaintInvoice") = mLineMaintInvoice
                dgLineMaintInvoiceTerm.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceTerms
                dgLineMaintInvoiceTerm.DataBind()
        End Select
    End Sub

    Private Sub cmbCurrencyList_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
        txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
        If cmbCurrencyList.Enabled = True Then
            cmbCurrencyList.Focus()
        End If
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("LineMaintenanceInvoiceNew") And Not User.IsInRole("LineMaintenanceInvoiceEdit")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If IsValid Then
            Save()
        End If
    End Sub

    Private Sub chkIsRoundOff_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkIsRoundOff.CheckedChanged
        Dim Child As LineMaintenanceInvoiceCharge
        For i As Integer = mLineMaintInvoice.LineMaintenanceInvoiceCharges.Count - 1 To 0 Step -1
            Child = mLineMaintInvoice.LineMaintenanceInvoiceCharges(i)
            If Child.ChargeID.Equals(New Guid("{40000000-0000-0000-0000-000000000000}")) Or Child.ChargeID.Equals(New Guid("{50000000-0000-0000-0000-000000000000}")) Then
                mLineMaintInvoice.LineMaintenanceInvoiceCharges.Remove(Child)
            End If
        Next
        dgLineMaintInvoiceCharge.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceCharges
        dgLineMaintInvoiceCharge.DataBind()
        SetChargeGrid()
        upnlInvoiceCharge.Update()
    End Sub


    Private Sub btnAuthorized_Click(sender As System.Object, e As System.EventArgs) Handles btnAuthorized.Click
        If (Not User.IsInRole("LineMaintenanceInvoiceAuthorized")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If IsValid Then
            MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<Strong> Service Invoice </Strong>", MsgBoxStyle.YesNo, "Status")
            Session("IsValid") = IsValid
            SetObject()
            mLineMaintInvoice.StatusID = 2
            Session("mLineMaintInvoice") = mLineMaintInvoice
        End If
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        If (Not User.IsInRole("LineMaintenanceInvoiceAuthorized") And Not User.IsInRole("LineMaintenanceInvoiceNew")) Or (Not User.IsInRole("LineMaintenanceInvoiceAuthorized") And Not User.IsInRole("LineMaintenanceInvoiceEdit")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If IsValid Then
            MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<Strong> Service Invoice </Strong>", MsgBoxStyle.YesNo, "Status")
            Session("IsValid") = IsValid
            mLineMaintInvoice.StatusID = 4
            Session("mLineMaintInvoice") = mLineMaintInvoice
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click
        If Not User.IsInRole("LineMaintenanceInvoicePrint") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

        Dim rpt As rptLineMaintenanceInvoice
        Dim letter As rptLetterHead
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsLineMaintInvoice As New dsLineMaintenanceInvoice
        myReport = New crptLineMaintenanceInvoiceDetails

        rpt = rptLineMaintenanceInvoice.GetLineMaintenanceInvoice(mLineMaintInvoice.ID)
        letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", AppSettings("Logo"))

        dsLineMaintInvoice.Clear()

        Dim mrptImage As rptImage = rptImage.GetImage(dsLineMaintInvoice)
        da.Fill(dsLineMaintInvoice, mrptImage)

        da.Fill(dsLineMaintInvoice, rpt)
        da.Fill(dsLineMaintInvoice, letter)
        myReport.SetDataSource(dsLineMaintInvoice)

        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub

    'Added by Utkarsh on 14-Nov-2013 for Trans Text Series
    Private Sub txtInvoiceDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtInvoiceDate.TextChanged
        mLineMaintInvoice = Session("mLineMaintInvoice")

        mLineMaintInvoice.LineMaintenanceInvoiceDate = CDate(txtInvoiceDate.Text)
        txtInvoiceText.Text = mLineMaintInvoice.Text

        Session("mLineMaintInvoice") = mLineMaintInvoice
    End Sub
    'End

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub hdnimgBtnLineMaintenanceInvoiceTerm_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnLineMaintenanceInvoiceTerm.Click
        dgLineMaintInvoiceTerm.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceTerms
        dgLineMaintInvoiceTerm.DataBind()
        upnlInvoiceTerm.Update()
    End Sub

    Private Sub hdnBtnLineMaintenanceInvoiceCharge_Click(sender As Object, e As System.EventArgs) Handles hdnBtnLineMaintenanceInvoiceCharge.Click
        dgLineMaintInvoiceCharge.DataSource = mLineMaintInvoice.LineMaintenanceInvoiceCharges
        dgLineMaintInvoiceCharge.DataBind()
        mLineMaintInvoice.CalculateTotal()
        SetChargeGrid()
        upnlInvoiceCharge.Update()
        upnlOtherDetails.DataBind()
        upnlOtherDetails.Update()
    End Sub

    Private Sub HdnBtnFileAttachment(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click

        mLineMaintInvoice.IsAttachmentAdded = True
        ControlAttachmentICONVisibility(mLineMaintInvoice.StatusID)
        upnlFileAttachmentButtons.Update()

    End Sub

    Private Sub RemoveAttachment(sender As Object, e As EventArgs) Handles btnRemoveAttach.Click

        Dim fileSize As Integer = 0
        Dim file(fileSize) As Byte

        Try

            If mLineMaintInvoice.IsAttachmentAdded And mFileAttach Is Nothing Then
                mFileAttach = FileAttach.GetAttachment(ReferenceID:=mLineMaintInvoice.ID)
            End If

            mFileAttach.ImageFile = file
            mFileAttach.Size = 0

            AttachmentIcon.Visible = False
            btnRemoveAttach.Enabled = False
            IsAttachmentDeleted = True
            mLineMaintInvoice.IsAttachmentAdded = False

            Session("IsAttachmentDeleted") = IsAttachmentDeleted

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub ViewAttachment(sender As Object, e As ImageClickEventArgs) Handles AttachmentIcon.Click

        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString

        Try

            If mLineMaintInvoice.IsAttachmentAdded And mFileAttach Is Nothing Then

                mFileAttach = FileAttach.GetAttachment(ReferenceID:=mLineMaintInvoice.ID)

            End If

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

                    ScriptManager.RegisterStartupScript(Me,
                                                            [GetType],
                                                            "View Attachment",
                                                            "viewAttachment();",
                                                            True)
                End If

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub AttachFile(sender As Object, e As EventArgs) Handles btnSelectFile.ServerClick

        Try

            If mLineMaintInvoice.IsAttachmentAdded Then

                mFileAttach = FileAttach.GetAttachment(ReferenceID:=mLineMaintInvoice.ID)

            Else

                mFileAttach = FileAttach.NewAttachment(ID:=Guid.NewGuid,
                                                       ReferenceID:=mLineMaintInvoice.ID)
            End If

            Session("mFileAttach") = mFileAttach

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Service Methods "

    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetDistinctTextListAutoComplete(prefixText As String, count As Integer, contextKey As String) As String()
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