Imports System.Linq
Public Class wfInvoice_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mInvoice As Invoice
    Public mVendorList As VendorList
    Public mStatusList As StatusList
    Public mCurrencyList As CurrencyList
    Public mInvoiceItems As InvoiceItems
    Public mInvoiceCharges As InvoiceCharges
    Public Flag As Integer
    Public mTransTypeID As Trans 'Added By Utkarsh On 21-Jul-2011 For All19072011
    Public mModuleName As String
    Dim EventLogID As Guid
    Dim InvDetail As String 'End
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Public mGSTPercentage As GSTPercentage
    Public mVendor As Vendor
    Public mInvoiceItem As InvoiceItem
    Dim email As Thread
    Dim mTransactionList As TransactionList 'Ajay 17-04-2023
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mInvoice = Session("mInvoice")
        mVendorList = Session("mVendorList")
        mStatusList = Session("mStatusList")
        mCurrencyList = Session("mCurrencyList")
        mTransTypeID = CType(Session("mTransTypeID"), Integer) 'Added By Utkarsh On 21-Jul-2011 For All19072011
        mModuleName = Session("mModuleName") 'End
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mTransactionList = Session("mTransactionList") 'Ajay 17-04-2023
    End Sub
    Private Sub setObject()
        If txtInvoiceDate.Text = "" Then
            mInvoice.InvoiceDate = Today.Date
        Else
            mInvoice.InvoiceDate = CDate(txtInvoiceDate.Text)
        End If
        mInvoice.Remark = txtRemark.Text
        mInvoice.Text = txtInvoiceText.Text
        mInvoice.No = Val(txtInvoiceNo.Text)
        mInvoice.UserName = User.Identity.Name
        mInvoice.CurrencyID = New Guid(cmbCurrency.SelectedValue)
        mInvoice.ConversionFactor = Val(txtFactor.Text)

        Dim txtValue As TextBox
        Dim i As Integer = 0
        Try
            '------------------------------------------------------------------
            If AppSettings("IsGSTApplicable") = "True" Then
                For Each mInvoiceItem In mInvoice.InvoiceItems
                    With mInvoiceItem
                        Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)
                        mVendor = Vendor.GetVendor(mInvoice.VendorID)
                        If mVendor.ClientCountryName.ToUpper = "INDIA" Then
                            If mVendor.CountryName.ToUpper = "INDIA" And mInvoice.InvoiceDate >= CDate("01-Jul-2017") Then
                                mGSTPercentage = GSTPercentage.GetPercentage(mInvoice.InvoiceDate, 1, .ItemID.ToString)
                                If Not mGSTPercentage Is Nothing Then
                                    If Len(mVendor.StateCode) > 0 Then
                                        If mVendor.StateCode = mVendor.ClientStateCode Then
                                            txtValue = CType(Me.dgInvoice.Rows(i).FindControl("txtCGSTPer"), TextBox)
                                            .CGSTPercentage = CDec(Val(txtValue.Text))

                                            txtValue = CType(Me.dgInvoice.Rows(i).FindControl("txtSGSTPer"), TextBox)
                                            .SGSTPercentage = CDec(Val(txtValue.Text))

                                            .CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
                                            .SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)

                                            .TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount

                                            .IGSTPercentage = 0
                                            .IGSTCAmount = 0
                                            .HSNACSCode = mtmpItem.HSNACSCode
                                            mInvoice.StateCode = mVendor.StateCode
                                            mInvoice.ClientStateCode = mVendor.ClientStateCode
                                            mInvoice.VendorCountry = mVendor.CountryName
                                            mInvoice.Visibility = 1
                                        Else
                                            txtValue = CType(Me.dgInvoice.Rows(i).FindControl("txtIGSTPer"), TextBox)
                                            .IGSTPercentage = CDec(Val(txtValue.Text))
                                            .IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)

                                            .CGSTPercentage = 0
                                            .SGSTPercentage = 0
                                            .CGSTCAmount = 0
                                            .SGSTCAmount = 0

                                            .TotalCAmount = .CAmount + .IGSTCAmount
                                            .HSNACSCode = mtmpItem.HSNACSCode
                                            mInvoice.StateCode = mVendor.StateCode
                                            mInvoice.ClientStateCode = mVendor.ClientStateCode
                                            mInvoice.VendorCountry = mVendor.CountryName
                                            mInvoice.Visibility = 2
                                        End If
                                    Else
                                        .CGSTPercentage = 0
                                        .SGSTPercentage = 0
                                        .CGSTCAmount = 0
                                        .SGSTCAmount = 0
                                        .IGSTPercentage = 0
                                        .IGSTCAmount = 0
                                        .TotalCAmount = 0
                                        .HSNACSCode = ""
                                        mInvoice.StateCode = mVendor.StateCode
                                        mInvoice.ClientStateCode = mVendor.ClientStateCode
                                        mInvoice.VendorCountry = mVendor.CountryName
                                        mInvoice.Visibility = 3
                                    End If
                                End If
                            Else
                                .CGSTPercentage = 0
                                .SGSTPercentage = 0
                                .CGSTCAmount = 0
                                .SGSTCAmount = 0
                                .IGSTPercentage = 0
                                .IGSTCAmount = 0
                                .TotalCAmount = 0
                                .HSNACSCode = ""
                                mInvoice.StateCode = mVendor.StateCode
                                mInvoice.ClientStateCode = mVendor.ClientStateCode
                                mInvoice.VendorCountry = mVendor.CountryName
                                mInvoice.Visibility = 3
                            End If
                        Else
                            .CGSTPercentage = 0
                            .SGSTPercentage = 0
                            .CGSTCAmount = 0
                            .SGSTCAmount = 0
                            .IGSTPercentage = 0
                            .IGSTCAmount = 0
                            .TotalCAmount = 0
                            .HSNACSCode = ""
                            mInvoice.StateCode = mVendor.StateCode
                            mInvoice.ClientStateCode = mVendor.ClientStateCode
                            mInvoice.VendorCountry = mVendor.CountryName
                            mInvoice.Visibility = 3
                        End If
                    End With
                    i = i + 1
                Next
            Else
                mInvoice.Visibility = 3
            End If
            '------------------------------------------------------------------
        Catch ex As Exception
            Dim a As Integer = 0
        End Try
        '--------------------------------------------
        mInvoice.IsRoundOff = chkIsRoundOff.Checked
        mInvoice.CalculateTotal()               'Added By Saylee on 8-Sep-2007
        mInvoice.DCNO = Trim(txtDCNo.Text)      'Added by Saylee on 20-june-2011
        If txtDCDate.Text = "" Then
            mInvoice.DCDate = System.DBNull.Value
        Else
            mInvoice.DCDate = CDate(txtDCDate.Text)
        End If
        mInvoice.AWBNo = Trim(txtAWBNo.Text)    '****************************************
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mInvoice.IsAttachmentAdded = True
            Else
                mInvoice.IsAttachmentAdded = False
            End If
        End If
    End Sub
    Private Sub setVendorDetails()
        mInvoice.VendorID = New Guid(cmbVendorList.SelectedValue)
        mInvoice.VendorInvoiceNo = txtVendorInvNo.Text
        If txtVendorInvDate.Text = "" Then
            mInvoice.VendorInvoiceDate = System.DBNull.Value
        Else
            mInvoice.VendorInvoiceDate = CDate(txtVendorInvDate.Text)
        End If
    End Sub
    Private Sub SetPage()
        If mInvoice.IsNew Then
            lblTitle.Text = "Purchase Invoice [New]"
        Else
            lblTitle.Text = "Purchase Invoice [" & mInvoice.Text & "-" & mInvoice.No & "]"
        End If
        upnlTitle.Update()
    End Sub
    Private Sub SetControlStatus(ByVal StatusId As Int16)
        btnAdd.Enabled = IIf(StatusId > 1, False, True)
        btnAddCharge.Enabled = IIf(StatusId > 1, False, True)
        btnSave.Visible = IIf(StatusId > 1, False, True) ''=======================WO - 2006-2007-1-15.doc
        'dgInvoice.Columns(26).Visible = IIf(StatusId > 1, False, True)
        'dgInvoice.Columns(27).Visible = IIf(StatusId > 1, False, True)
        dgInvoice.Columns(26).Visible = IIf(StatusId > 1, False, True)
        dgInvoiceCharge.Columns(4).Visible = IIf(StatusId > 1, False, True)
        'dgInvoiceCharge.Columns(5).Visible = IIf(StatusId > 1, False, True)
        btnSelectFile.Disabled = IIf(StatusId > 1, True, False)
        'Other Charge
        dgInvoice.Columns(15).Visible = IIf((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA"), False, True)

        '---------------------------------------------------------------------------
        If mInvoice.Visibility = 1 Then
            dgInvoice.Columns(20).Visible = True 'CGSTPercentage 
            dgInvoice.Columns(21).Visible = True 'CGSTCAmount 
            dgInvoice.Columns(22).Visible = True 'SGSTPercentage 
            dgInvoice.Columns(23).Visible = True 'SGSTCAmount 
            dgInvoice.Columns(24).Visible = False 'IGSTPercentage 
            dgInvoice.Columns(25).Visible = False 'IGSTCAmount 


            lblTotalCGST.Visible = True
            txtTotalCGST.Visible = True
            lblTotalSGST.Visible = True
            txtTotalSGST.Visible = True

            lblTotalIGST.Visible = False
            txtTotalIGST.Visible = False
        ElseIf mInvoice.Visibility = 2 Then
            dgInvoice.Columns(20).Visible = False 'CGSTPercentage 
            dgInvoice.Columns(21).Visible = False 'CGSTCAmount 
            dgInvoice.Columns(22).Visible = False 'SGSTPercentage 
            dgInvoice.Columns(23).Visible = False 'SGSTCAmount 
            dgInvoice.Columns(24).Visible = True  'IGSTPercentage 
            dgInvoice.Columns(25).Visible = True 'IGSTCAmount 

            lblTotalCGST.Visible = False
            txtTotalCGST.Visible = False
            lblTotalSGST.Visible = False
            txtTotalSGST.Visible = False

            lblTotalIGST.Visible = True
            txtTotalIGST.Visible = True
        ElseIf mInvoice.Visibility = 3 Then
            If AppSettings("HSNACSCodeVisibleInPartMaster") = "True" Then
                dgInvoice.Columns(19).Visible = True 'HSNACSCode
            Else
                dgInvoice.Columns(19).Visible = False 'HSNACSCode 
            End If
            dgInvoice.Columns(20).Visible = False 'CGSTPercentage 
            dgInvoice.Columns(21).Visible = False 'CGSTCAmount 
            dgInvoice.Columns(22).Visible = False 'SGSTPercentage 
            dgInvoice.Columns(23).Visible = False 'SGSTCAmount 
            dgInvoice.Columns(24).Visible = False  'IGSTPercentage 
            dgInvoice.Columns(25).Visible = False 'IGSTCAmount 
            lblTotalCGST.Visible = False
            txtTotalCGST.Visible = False
            lblTotalSGST.Visible = False
            txtTotalSGST.Visible = False
            lblTotalIGST.Visible = False
            txtTotalIGST.Visible = False
        End If
        '---------------------------------------------------------------------------
    End Sub
    Private Sub ControlVisibility()
        txtInvoiceText.Enabled = CType(IIf(mInvoice.StatusID >= 2, False, True), Boolean) ''And mInvoice.InvoiceItems.Count = 0) Or (mInvoice.InvoiceItems.Count = 0)
        txtInvoiceNo.Enabled = CType(IIf(mInvoice.StatusID >= 2, False, True), Boolean)  ''And mInvoice.InvoiceItems.Count = 0) Or (mInvoice.InvoiceItems.Count = 0)
        txtInvoiceDate.Enabled = (CType(IIf(mInvoice.StatusID = 2 Or mInvoice.StatusID = 4, False, True), Boolean) And mInvoice.InvoiceItems.Count = 0) Or (mInvoice.InvoiceItems.Count = 0)
        cmbVendorList.Enabled = (CType(IIf(mInvoice.StatusID = 2 Or mInvoice.StatusID = 4, False, True), Boolean) And mInvoice.InvoiceItems.Count = 0) Or (mInvoice.InvoiceItems.Count = 0)
        txtVendorInvDate.Enabled = CType(IIf(mInvoice.StatusID >= 2, False, True), Boolean) '' And mInvoice.InvoiceItems.Count = 0) Or (mInvoice.InvoiceItems.Count = 0)
        txtRemark.Enabled = CType(IIf(mInvoice.StatusID >= 2, False, True), Boolean) '' And mInvoice.InvoiceItems.Count = 0) Or (mInvoice.InvoiceItems.Count = 0)
        btnCancel.Visible = (Not mInvoice.IsNew) And (mInvoice.StatusID = 2)
        btnAuthorized.Visible = (Not mInvoice.IsNew) And (mInvoice.StatusID = 1)
        txtDCDate.Enabled = (mInvoice.StatusID = 1)
        chkIsRoundOff.Enabled = (mInvoice.StatusID = 1)
        If mInvoice.TransTypeID = 10 Then
            dgInvoice.Columns(11).Visible = True
            dgInvoice.Columns(13).Visible = True
            dgInvoice.Columns(10).Visible = False
            dgInvoice.Columns(12).Visible = False
        Else
            dgInvoice.Columns(11).Visible = False
            dgInvoice.Columns(13).Visible = False
            dgInvoice.Columns(10).Visible = True
            dgInvoice.Columns(12).Visible = True
        End If
        If Not User.IsInRole("InvoiceAuthorized") Then
            btnAuthorized.Enabled = False
            btnAuthorized.ToolTip = "You are not authorized user."
            btnCancel.Enabled = False
            btnCancel.ToolTip = "You are not authorized user "
        End If
        If (Not User.IsInRole("InvoiceAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC")) Then
            'btnSelectFile.Disabled = True
            'btnDelAttach.Enabled = False
            'btnDelAttach.ToolTip = "You are not authorized user "
            'ImageButton1.Enabled = False
            'ImageButton1.ToolTip = "You are not authorized user "
        End If
        Dim txtCGSTPer As TextBox
        Dim txtIGSTPer As TextBox
        For i As Integer = 0 To dgInvoice.Rows.Count - 1
            txtCGSTPer = CType(Me.dgInvoice.Rows(i).FindControl("txtCGSTPer"), TextBox)
            txtCGSTPer.Enabled = IIf(mInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False" Or mInvoice.InvoiceItems(i).HSNACSCode = "", False, True)
            txtIGSTPer = CType(Me.dgInvoice.Rows(i).FindControl("txtIGSTPer"), TextBox)
            txtIGSTPer.Enabled = IIf(mInvoice.StatusID >= 2 Or AppSettings("ChangeGSTPercentage") = "False" Or mInvoice.InvoiceItems(i).HSNACSCode = "", False, True)
        Next
        'Added By Vikrant on 12-Jun-2020 For ALL12062020
        'Commented By Vikrant On 10-Aug-2020 as per Heligo requirement as per maill discussed in meeting
        'cmbCurrency.Enabled = (Not (mInvoice.TransTypeID = 21 Or mInvoice.TransTypeID = 10)) And (Not mInvoice.StatusID <> 1)
        'txtFactor.Enabled = (Not (mInvoice.TransTypeID = 21 Or mInvoice.TransTypeID = 10)) And (Not mInvoice.StatusID <> 1)
        'End
        ControlVisibilityForAttachment()
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mInvoice.InvoiceItems.CurrentIndex = Index
        Session("mInvoice") = mInvoice
    End Sub
    Private Sub DeleteCharge(ByVal index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveCharge, MSGBox.Message_text.RemoveCharge, "", MsgBoxStyle.YesNo, "DeleteCharge")
        mInvoice.InvoiceCharges.CurrentIndex = index
        Session("mInvoice") = mInvoice
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
                            mInvoice = CType(Session("mInvoice"), Invoice)
                            mInvoice.InvoiceItems.Remove(mInvoice.InvoiceItems.CurrentItem)
                            dgInvoice.DataSource = mInvoice.InvoiceItems
                            dgInvoice.DataBind()
                            'SetGrid()
                            upnlInvoiceItems.Update()
                            mInvoice.CalculateTotal()            'Added By Saylee on 8-Sep-2007
                            If mInvoice.IsRoundOff = True Then   'Added By Prashant on 29-Oct-2012 ALL25102012
                                mInvoice.RoundCGrandTotal()
                            End If
                            upnlOtherDetails.Update()
                            Session("mInvoice") = mInvoice
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteCharge" Then
                        Try
                            Session("Sender") = ""
                            Dim mInvoice As Invoice
                            mInvoice = CType(Session("mInvoice"), Invoice)
                            mInvoice.InvoiceCharges.Remove(mInvoice.InvoiceCharges.CurrentItem)
                            dgInvoiceCharge.DataSource = mInvoice.InvoiceCharges
                            dgInvoiceCharge.DataBind()
                            upnlInvoiceCharge.Update()
                            mInvoice.CalculateTotal()            'Added By Saylee on 8-Sep-2007
                            If mInvoice.IsRoundOff = True Then   'Added By Prashant on 29-Oct-2012 ALL25102012
                                SetChargeGrid()
                                mInvoice.RoundCGrandTotal()
                            End If
                            upnlOtherDetails.Update()
                            Session("mInvoice") = mInvoice
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
                        Session("sender") = ""
                        If mInvoice.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not User.IsInRole("InvoiceNew")) And (Not User.IsInRole("InvoiceEdit")) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            Save()
                        Else
                            Session.Remove("IsValid")
                            Response.Redirect("wfInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If mInvoice.IsValid = True Then
                            Session.Remove("IsValid")
                            mInvoice.StatusID = 2
                            DataFieldBind()
                            Save()
                        Else
                            Session.Remove("IsValid")
                            Response.Redirect("wfInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "StatusCancel" Then
                        Session("sender") = ""
                        mInvoice.StatusID = 4
                        DataFieldBind()
                        Save()
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                    If (MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "StatusCancel") Then

                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If mInvoice.StatusID = 2 Then
                            mInvoice.StatusID = 1
                        ElseIf mInvoice.StatusID = 4 Then
                            mInvoice.StatusID = 2
                        End If
                        Session("mInvoice") = mInvoice
                        DataFieldBind()
                    ElseIf MSGBoxCtrl.Sender = "PendingQty" Then
                        DataFieldBind()
                    ElseIf MSGBoxCtrl.Sender = "InvoiceTransTextSeriesAlert" Then
                        Session("sender") = ""
                        Session("AddTransTextSeries") = "True"
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    End If
            End Select
        End If
    End Sub
    Private Sub Save()
        'Authentication
        If Not mInvoice.InvoiceDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")
                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                If DateDiff(DateInterval.Day, CDate(mInvoice.InvoiceDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Goods Receipt. <br> Goods Receipt Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        End If
        'Authentication

        Dim InvoiceClone As Invoice
        InvoiceClone = mInvoice.Clone
        Try
            If Not mInvoice.InvoiceItems.Count = 0 Then
                setObject()
                setVendorDetails()
                'Code Added by DEVEN On 28/12/2007 --------------------------------------
                Dim InvoiceCharge As InvoiceCharge
                For Each InvoiceCharge In mInvoice.InvoiceCharges
                    If (InvoiceCharge.Sign <> 1 And InvoiceCharge.CChargeAmount <= 0) Or (Not (InvoiceCharge.IsValid)) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Invoice Charge(s) are not allowed if Invoice Amount Is Zero ", MsgBoxStyle.OkOnly, "")
                        mInvoice.CancelEdit()
                        Exit Sub
                    End If
                Next
                '------------------------------------------------------------------------
                If mInvoice.IsRoundOff = True Then  'Added By Prashant on 29-Oct-2012 ALL25102012
                    mInvoice.RoundCGrandTotal()
                End If
                'Added by Utkarsh on 19-Nov-2013 FOr TransTextSeries 
                'Check if ReceiptCumInvoiceText is blank then call TransTextSeries UI

                If (mInvoice.IsNew) And (mInvoice.Text = "") Then

                    Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mInvoice.TransTypeID, mInvoice.InvoiceDateFormatted)

                    If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mInvoice.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mInvoice.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mInvoice.TransTypeID).TransText = "")) Then

                        Dim str = "<script language='javascript'>openledgersame('wfInvoice_Ajax.aspx?BackPage=index.aspx');</script>"

                        Session("BackPagestr_ForTransSeries") = str

                        Session("TransName_ForTransSeries") = "Purchase Invoice"
                        Session("TransTypeID_ForTransSeries") = mInvoice.TransTypeID
                        Session("TransDate_ForTransSeries") = mInvoice.InvoiceDateFormatted
                        MSGBoxCtrl.show("Invoice Trans Text Series Alert", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "InvoiceTransTextSeriesAlert")
                        Exit Sub
                    Else
                        Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                        If mAutoRenewTransTextSeries.IsRenewed Then
                            With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mInvoice.TransTypeID)
                                mInvoice.Text = .TransText
                                mInvoice.No = .StartingTransNo
                            End With
                        Else
                            Dim str = "<script language='javascript'>openledgersame('wfInvoice_Ajax.aspx?BackPage=index.aspx');</script>"

                            Session("BackPagestr_ForTransSeries") = str

                            Session("TransName_ForTransSeries") = "Purchase Invoice"
                            Session("TransTypeID_ForTransSeries") = mInvoice.TransTypeID
                            Session("TransDate_ForTransSeries") = mInvoice.InvoiceDateFormatted
                            MSGBoxCtrl.show("Invoice Trans Text Series Alert", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "InvoiceTransTextSeriesAlert")
                            Exit Sub
                        End If
                    End If

                End If
                'End
                SaveAttachment()
                mInvoice.Save()
                'Changed By Utkarsh On 21-Jul-2011 For All19072011
                InvDetail = mInvoice.InvoiceNo + " Dated : " + mInvoice.InvoiceDateFormatted + " from " + mVendorList(mInvoice.VendorID).Name
                Select Case mInvoice.StatusID
                    Case 1
                        MarkLog(Util.Action.Save, mModuleName, InvDetail, Util.ErrorType.NoError, mInvoice.ID, EventLogID)
                    Case 2
                        MarkLog(Util.Action.Authorize, mModuleName, InvDetail, Util.ErrorType.NoError, mInvoice.ID, EventLogID)
                    Case 3
                        MarkLog(Util.Action.Amend, mModuleName, InvDetail, Util.ErrorType.NoError, mInvoice.ID, EventLogID)
                    Case 4
                        MarkLog(Util.Action.Cancel, mModuleName, InvDetail, Util.ErrorType.NoError, mInvoice.ID, EventLogID)
                End Select
                'End
                Session("mInvoice") = mInvoice
                DataFieldBind()
                SetPage()
                ControlVisibility()
                SetChargeGrid()
                SetControlStatus(mInvoice.StatusID)
                upnlStatusName.Update()
                upnlInvoiceDetails.Update()
                upnlSupplierDetails.Update()
                upnlInvoiceItems.Update()
                upnlOtherDetails.Update()
                upnlButtons.Update()
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Invoice can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                Exit Sub
                mInvoice = InvoiceClone
                setObject()
                setVendorDetails()
                Session("mInvoice") = mInvoice
                DataFieldBind()
            End If
        Catch ex As SqlClient.SqlException
            Session("InvoiceClone") = InvoiceClone
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
                If InStr(ex.Message, "CCtabReceiptItemInvoiceBalanceQty", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, "Qty. Not Available", MsgBoxStyle.OkOnly, "PendingQty")
                    mInvoice = InvoiceClone
                    Session("mInvoice") = mInvoice
                    Exit Sub
                ElseIf InStr(ex.Message, "FKtabInvoiceChargetabCharge", CompareMethod.Text) Then
                    MSGBoxCtrl.show("Alert!", "Invoice Charge Deleted ! ", "Invoice charge Not Available<Br><BR>Selected Charge is no longer exist in the Database <BR><BR> Remove Charge and try Again", MsgBoxStyle.OkOnly, "")
                    mInvoice = InvoiceClone
                    Session("mInvoice") = mInvoice
                    Exit Sub
                ElseIf InStr(ex.Message, "CCtabInvoiceNo", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "No. Required", MsgBoxStyle.OkOnly, "")
                    mInvoice = InvoiceClone
                    Session("mInvoice") = mInvoice
                    Exit Sub
                End If
            End If
        Finally
            InvoiceClone = Nothing
        End Try
    End Sub
    Private Sub addAttributes()
        txtInvoiceNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtInvoiceNo').value,event)")
    End Sub
    Private Sub SetChargeGrid()
        For j As Integer = 0 To dgInvoiceCharge.Rows.Count - 1
            If (Me.dgInvoiceCharge.Rows.Item(j).Cells(1).Text = "Round off (Plus)" Or Me.dgInvoiceCharge.Rows.Item(j).Cells(1).Text = "Round off (Minus)") Then
                dgInvoiceCharge.Rows.Item(j).Cells(4).Visible = False
                'dgInvoiceCharge.Rows.Item(j).Cells(5).Enabled = False
            End If
        Next
        upnlInvoiceCharge.Update()
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mInvoice.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = IIf(mInvoice.StatusID > 1, False, True)
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mInvoice.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mInvoice.ID)
            Session("mFileAttach") = mFileAttach
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
                If (Not mInvoice.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mInvoice.ID)
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
    Private Sub CGrandTotal() 'Added By Prashant 24-Apr-2015
        Dim SumCEffectiveAmount As Decimal
        Dim mInvoiceDocketCharge As Decimal
        For i As Integer = 0 To mInvoice.InvoiceItems.Count - 1
            If (mInvoice.TransTypeID = 10 Or mInvoice.TransTypeID = 48 Or mInvoice.TransTypeID = 54 Or (mInvoice.TransTypeID = 67 And mInvoice.InvoiceItems(i).IsReturnFromOHRepair = True)) Then
                SumCEffectiveAmount = SumCEffectiveAmount + (mInvoice.InvoiceItems(i).GROCEffRate * mInvoice.InvoiceItems(i).Qty) - (mInvoice.InvoiceItems(i).CGSTCAmount + mInvoice.InvoiceItems(i).SGSTCAmount + mInvoice.InvoiceItems(i).IGSTCAmount)
            Else
                SumCEffectiveAmount = SumCEffectiveAmount + (mInvoice.InvoiceItems(i).CEffRate * mInvoice.InvoiceItems(i).Qty) - (mInvoice.InvoiceItems(i).CGSTCAmount + mInvoice.InvoiceItems(i).SGSTCAmount + mInvoice.InvoiceItems(i).IGSTCAmount)
            End If
        Next
        mInvoiceDocketCharge = SumCEffectiveAmount - mInvoice.CTotalAmount - mInvoice.CTotalCharges 'This is to show Docket charges per invoice.

        Dim mOtherChargeListByInvoiceID As OtherChargeListByInvoiceID
        mOtherChargeListByInvoiceID = OtherChargeListByInvoiceID.GetOtherChargeListByInvoiceID(mInvoice.ID.ToString)
        If mOtherChargeListByInvoiceID.Count <> 0 Then
            lblTotalDocketCharge.Visible = True
            txtInvoiceDocketCharge.Visible = True
            lblInvoiceDocketCharge.Visible = True
            txtInvoiceDocketCharge.Text = CDec(Format(mInvoiceDocketCharge, "##0.00##")).ToString
            lblTotalDocketCharge.Text = "Total Docket Charge : " + mOtherChargeListByInvoiceID.Item(0).CGrandTotal.ToString + " in " + cmbCurrency.SelectedItem.Text
        Else
            lblTotalDocketCharge.Visible = False
            txtInvoiceDocketCharge.Visible = False
            lblTotalDocketCharge.Visible = False
            lblInvoiceDocketCharge.Visible = False
            lblTotalDocketCharge.Text = ""
            txtInvoiceDocketCharge.Text = ""
        End If
    End Sub
    Public Sub TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtValue As TextBox
        Dim txtCGSTPer As TextBox
        Dim mInvoiceItem As InvoiceItem
        Dim i As Integer = 0
        For Each mInvoiceItem In mInvoice.InvoiceItems
            With mInvoiceItem
                Try
                    txtCGSTPer = CType(Me.dgInvoice.Rows(i).FindControl("txtCGSTPer"), TextBox)
                    txtCGSTPer.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtCGSTPer.ClientID + "').value,event)")

                    txtValue = CType(Me.dgInvoice.Rows(i).FindControl("txtSGSTPer"), TextBox)
                    txtValue.Text = Val(txtCGSTPer.Text)

                    txtValue = CType(Me.dgInvoice.Rows(i).FindControl("txtIGSTPer"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")
                Catch ex As Exception
                End Try
            End With
            i = i + 1
        Next
        upnlInvoiceItems.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCurrencyList = CurrencyList.GetCurrencyList(, , True)
        mVendorList = VendorList.GetVendortList(0, , , , , , True, False, True)
        cmbVendorList.DataSource = mVendorList
        cmbCurrency.DataSource = mCurrencyList

        Session("mCurrencyList") = mCurrencyList
        Session("mVendorList") = mVendorList
        Session("mStatusList") = mStatusList

        dgInvoice.DataSource = mInvoice.InvoiceItems
        dgInvoiceCharge.DataSource = mInvoice.InvoiceCharges
        txtInvoiceDate.Text = mInvoice.InvoiceDateFormatted.ToString
        txtVendorInvDate.Text = mInvoice.VendorInvoiceDateFormatted.ToString
        txtDCDate.Text = mInvoice.DCDateFormatted.ToString 'Added by Saylee on 20-june-2011
        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtInvoiceDate" Then
            If txtInvoiceDate.Text = "" Then
                custValidator.ErrorMessage = "Select Invoice Date."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbVendorList" Then
            If cmbVendorList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Supplier From the list."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbCurrency" Then
            If cmbCurrency.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Currency from the List."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtConversionFactor" Then
            If Val(txtFactor.Text) <= 0 Then
                custValidator.ErrorMessage = "Currency factor must be greater than zero."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtAmountInWords" Then
            If Len(txtAmountInWords.Text) > 250 Then
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 100 Then
                custValidator.ErrorMessage = "Remark Too Long"
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 21-Jul-2011 For All19072011
        addAttributes()
        SetControlStatus(mInvoice.StatusID)
        If Not IsPostBack And Session("sender") = "" Then
            'Added by Utkarsh on 19-Nov-2013 for Trans Text Series
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If mInvoice.IsNew Then
                    mInvoice.Text = Session("TransText_ForTransSeries")
                    txtInvoiceText.Text = mInvoice.Text
                    Session("mInvoice") = mInvoice
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If
            End If
            'End
            DataFieldBind()
            SetPage()
            CGrandTotal()
            ControlVisibility()
            If chkIsRoundOff.Checked = True Then  'Added By Prashant on 21-May-2012
                SetChargeGrid()
            End If
        End If
        TextChanged(sender, e)
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If IsValid Then
            setObject()
            setVendorDetails()
            Session("mInvoice") = mInvoice
            Session("OpenFrom") = "2"
            'If (mInvoice.InvoiceItems.Count = 0) Or (mInvoice.InvoiceItems.Count = 1 And mInvoice.IsNew) Then
            If (mInvoice.InvoiceItems.Count = 0) Then
                Session("mPrevTransID") = Guid.Empty
                Session("mOrderTranstypeID") = 0
            Else
                Session("mPrevTransID") = mInvoice.InvoiceItems.Item(mInvoice.InvoiceItems.Count - 1).ItemDetailForInvoice.ReceiptID
                Session("mOrderTranstypeID") = mInvoice.InvoiceItems.Item(mInvoice.InvoiceItems.Count - 1).ItemDetailForInvoice.OrderTranstypeID
            End If
            Session("mTransaction") = 5  'Transaction.Receipt
            Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?ChildPage=wfInvoice_Ajax.aspx&mType=3")
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnAddCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCharge.Click
        If IsValid Then
            setObject()
            setVendorDetails()
            mInvoice.InvoiceCharges.Add(mInvoice.ID)
            Session("mInvoice") = mInvoice
            Response.Redirect("wfInvoiceCharge_Ajax.aspx?BackPage=wfInvoice_Ajax.aspx")
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Changed By Utkarsh On 21-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, mModuleName, "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        Session("IsValid") = IsValid
        setObject()
        If mInvoice.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
            If IsValid Then
                setObject()
                setVendorDetails()
            End If
        Else
            Session.Remove("mSelectList")
            Session.Remove("mFileAttach")
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub dgInvoice_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInvoice.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim index As Int32 = CInt(e.CommandArgument) '+ dgInvoice.PageIndex * dgInvoice.PageSize
                Session("Edit") = True
                Session("PendingReceipt") = "False"
                setObject()
                setVendorDetails()
                mInvoice.InvoiceItems.CurrentIndex = index
                Session("mInvoice") = mInvoice
                Response.Redirect("wfInvoiceItem_Ajax.aspx?BackPage=wfInvoice_Ajax.aspx")
            Case "DeleteRecord"
                Dim index As Int32 = CInt(e.CommandArgument) '+ dgInvoice.PageIndex * dgInvoice.PageSize
                DeleteRecord(index)
        End Select
    End Sub
    Private Sub dgInvoiceCharge_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInvoiceCharge.RowCommand
        Select Case e.CommandName
            Case "EditCharge"
                Dim index As Int32 = CInt(e.CommandArgument) + dgInvoiceCharge.PageIndex * dgInvoiceCharge.PageSize
                Session("Edit") = True
                setObject()
                setVendorDetails()
                mInvoice.InvoiceCharges.CurrentIndex = index
                Session("mInvoice") = mInvoice
                Response.Redirect("wfInvoiceCharge_Ajax.aspx")
            Case "DeleteCharge"
                Dim index As Int32 = CInt(e.CommandArgument) + dgInvoiceCharge.PageIndex * dgInvoiceCharge.PageSize
                DeleteCharge(index)
        End Select
    End Sub
    Private Sub cmbCurrency_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCurrency.SelectedIndexChanged
        txtFactor.Text = mCurrencyList(cmbCurrency.SelectedIndex).ConversionFactor
        If cmbCurrency.Enabled = True Then
            setFocus(cmbCurrency)
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("InvoiceNew")) And (Not User.IsInRole("InvoiceEdit")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If IsValid Then
            Save()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub txtInvoiceDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtInvoiceDate.TextChanged
        mInvoice.InvoiceDate = txtInvoiceDate.Text
        txtInvoiceDate.Text = mInvoice.Text
        txtInvoiceDate.DataBind()
        Session("mInvoice") = mInvoice
    End Sub
    Public Sub SetReport(Optional ByVal ByMail As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
        If CDate(txtInvoiceDate.Text) <= CDate("30-Jun-2017") Or mInvoice.Visibility = 3 Then
            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "RAL") Then
                rpt = New crptGoodInvoiceNote
            Else
                rpt = New crptInvoiceDetailPortrait
            End If
        Else
            rpt = New crptInvoiceGSTDetail
        End If
        Dim obj As rptInvoices
        Dim objChilds As rptInvoiceChilds
        Dim letter As rptLetterHead
        Dim ds As New dsInvoice
        Dim mCompanyInfo As rptSearchingCriteriaForReceipt

        obj = rptInvoices.GetInvoices(mInvoice.ID)
        letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", lblTotalDocketCharge.Text, _
                                                 AppSettings("Logo"), txtInvoiceDocketCharge.Text, ClientCode:=AppSettings("ClientCode"), _
                                                 SearchString4:=AppSettings("HSNACSCodeVisibleInPartMaster"))

        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "RAL") Then
            objChilds = rptInvoiceChilds.GetInvoiceChilds(mInvoice.ID, "RAL", mInvoice.TransTypeID)
            mCompanyInfo = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), "", "", "", "", objChilds.Item(0).ReceiptDate.ToString, cmbCurrency.SelectedItem.Text, objChilds.Item(0).OrderDate.ToString, "", "", objChilds.Item(0).OrderNo, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0)
            da.Fill(ds, mCompanyInfo)
        Else
            objChilds = rptInvoiceChilds.GetInvoiceChilds(mInvoice.ID)
        End If

        da.Fill(ds, obj)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, objChilds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, letter)
        rpt.SetDataSource(ds)
        Session("CrystalReport") = rpt
        If ByMail = True Then
            Dim str As String
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Invoice No.: <b> " & mInvoice.Text + "-" + mInvoice.No.ToString & "</b> Dated: <b> " + mInvoice.InvoiceDateFormatted + "</b> has been Authorized By User: <b> " + Thread.CurrentPrincipal.Identity.Name + " </b> on: <b> " + New SmartDate(Today.Date).FormattedText + "</b>.</font></P> ")
            str = str + ("<p></b> Copy is attached for your information and planning.</font></p>")
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Invoice Details", _
                                     Text:=mInvoice.Text + "-" + mInvoice.No.ToString, Info:=str, VendorEmailID:="", ToMailID:=Session("ToSendMailIDs"), CCMailID:=Session("CcSendMailIDs"), ReportPath:="", _
                                      ReportByMail:=False, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                        SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))

        Else
            Dim Str1 As String
            Str1 = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not User.IsInRole("InvoicePrint") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        SetReport(False)
    End Sub
    Private Sub chkIsRoundOff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsRoundOff.CheckedChanged
        Dim Child As InvoiceCharge
        For i As Integer = mInvoice.InvoiceCharges.Count - 1 To 0 Step -1
            Child = mInvoice.InvoiceCharges(i)
            If Child.ChargeID.Equals(New Guid("{40000000-0000-0000-0000-000000000000}")) Or Child.ChargeID.Equals(New Guid("{50000000-0000-0000-0000-000000000000}")) Then
                mInvoice.InvoiceCharges.Remove(Child)
            End If
        Next
        dgInvoiceCharge.DataSource = mInvoice.InvoiceCharges
        dgInvoiceCharge.DataBind()
        upnlInvoiceCharge.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mInvoice.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        If (Not User.IsInRole("InvoiceAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        GetAttachment()

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mInvoice.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        If (Not User.IsInRole("InvoiceAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        ViewImage()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If (Not User.IsInRole("InvoiceAuthorized") And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If mInvoice.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mInvoice.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mInvoice.ID)
        End If
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    End Sub

    'Ajay 17-04-2023
    Public Sub SetUserEmailID()
        Session("UserEmailID") = mTransactionList.Item(mInvoice.TransTypeID).SendToMailID
        Session("UserCcEmailID") = mTransactionList.Item(mInvoice.TransTypeID).SendCCMailID
        Session("MailsRequire") = mTransactionList.Item(mInvoice.TransTypeID).MailsRequire
        Session("SmtpHost") = mTransactionList.Item(mInvoice.TransTypeID).SmtpHost
        Session("SmtpPort") = mTransactionList.Item(mInvoice.TransTypeID).SmtpPort
        Session("SmtpUser") = mTransactionList.Item(mInvoice.TransTypeID).SmtpUser
        Session("SmtpPassword") = mTransactionList.Item(mInvoice.TransTypeID).SmtpPassword
        Session("FormRevisionNo") = mTransactionList.Item(mInvoice.TransTypeID).FormRevisionNo
        Session("FormRevisionDate") = mTransactionList.Item(mInvoice.TransTypeID).FormRevisionDate
    End Sub
    Private Sub btnSendMail_Click(sender As Object, e As System.EventArgs) Handles btnSendMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        '   Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
        SetUserEmailID()
        '--------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub

    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            email = New Thread(Sub() SetReport(True))
            email.IsBackground = True
            email.Start()
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
    '---------------------
#End Region

#Region " Status "
    Private Sub btnAuthorized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click ''===============================  WO - 2006-2007-1-15.doc
        If IsValid Then
            MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<Strong> Invoice </Strong>", MsgBoxStyle.YesNo, "Status")
            Session("IsValid") = IsValid
            setObject()
            setVendorDetails()
            Session("mInvoice") = mInvoice
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click ''===============================  WO - 2006-2007-1-15.doc
        If IsValid Then
            If (IsInUse.GetIsInUseInvoiceINPayment(mInvoice.ID).IsInUse) Or (IsInUse.GetIsInUseInvoiceINOtherCharge(mInvoice.ID).IsInUse) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Cancel, MSGBox.Message_text.Cancel, "<Strong>Invoice, It is used in Payment Or Other Charge.</Strong>", MsgBoxStyle.OkOnly, "StatusCancel")
                Session("mInvoice") = mInvoice
                Exit Sub
            End If

            MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<Strong> Invoice </Strong>", MsgBoxStyle.YesNo, "StatusCancel")
            Session("IsValid") = IsValid
            Session("mInvoice") = mInvoice
        End If
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