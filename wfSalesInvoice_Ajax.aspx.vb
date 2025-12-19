Public Class wfSalesInvoice_Ajax
    Inherits Page

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

    Protected mSalesInvoice As SalesInvoice
    Protected mVendorList As VendorList
    Protected mStatusList As StatusList
    Protected mCurrencyList As CurrencyList
    Public Flag As Integer
    Dim EventLogID As Guid

#End Region

#Region " Business Methods "

    Private Sub GetSession()

        mSalesInvoice = Session("mSalesInvoice")
        mVendorList = Session("mVendorList")
        mStatusList = Session("mStatusList")
        mCurrencyList = Session("mCurrencyList")

    End Sub

    Private Sub SetSession()

        Session("mSalesInvoice") = mSalesInvoice
        Session("mVendorList") = mVendorList
        Session("mStatusList") = mStatusList
        Session("mCurrencyList") = mCurrencyList

    End Sub

    Private Sub SetObject()

        mSalesInvoice.SalesInvoiceDate = CDate(txtInvoiceDate.Text)
        mSalesInvoice.Remark = txtRemark.Text
        mSalesInvoice.Text = txtInvoiceText.Text
        mSalesInvoice.No = Val(txtInvoiceNo.Text)
        mSalesInvoice.UserName = User.Identity.Name
        'Added By Vikrant on 29-Jan-2018 For Deccan29012018-1
        Dim mVendor As Vendor
        Dim mSalesInvoiceItem As SalesInvoiceItem
        Dim txtValue As TextBox
        Dim i As Integer = 0

        If AppSettings("IsGSTApplicable") = "True" Then

            mVendor = Vendor.GetVendor(mSalesInvoice.VendorID)

            If mVendor.ClientCountryName.ToUpper = "INDIA" Then

                If mVendor.CountryName.ToUpper = "INDIA" And mSalesInvoice.SalesInvoiceDate >= CDate("01-Jul-2017") Then

                    For Each mSalesInvoiceItem In mSalesInvoice.SalesInvoiceItems

                        With mSalesInvoiceItem
                            Dim mtmpItem As ItemByID = ItemByID.GetItemByID(.ItemID)

                            If Len(mVendor.StateCode) > 0 Then

                                If mVendor.StateCode = mVendor.ClientStateCode Then

                                    txtValue = CType(Me.dgSalesInvoiceItem.Rows(i).FindControl("txtWCGST"), TextBox)
                                    .CGSTPercentage = CDec(Val(txtValue.Text))

                                    txtValue = CType(Me.dgSalesInvoiceItem.Rows(i).FindControl("txtWCGST"), TextBox)
                                    .SGSTPercentage = Val(txtValue.Text.Trim)

                                    .CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
                                    .SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)

                                    .IGSTPercentage = 0
                                    .IGSTCAmount = 0
                                    .HSNACSCode = mtmpItem.HSNACSCode
                                    mSalesInvoice.StateCode = mVendor.StateCode
                                    mSalesInvoice.ClientStateCode = mVendor.ClientStateCode
                                    mSalesInvoice.VendorCountry = mVendor.CountryName
                                    mSalesInvoice.Visibility = 1

                                Else

                                    txtValue = CType(Me.dgSalesInvoiceItem.Rows(i).FindControl("txtWIGST"), TextBox)
                                    .IGSTPercentage = CDec(Val(txtValue.Text))
                                    .IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)
                                    .CGSTPercentage = 0
                                    .SGSTPercentage = 0
                                    .CGSTCAmount = 0
                                    .SGSTCAmount = 0
                                    .HSNACSCode = mtmpItem.HSNACSCode
                                    mSalesInvoice.StateCode = mVendor.StateCode
                                    mSalesInvoice.ClientStateCode = mVendor.ClientStateCode
                                    mSalesInvoice.VendorCountry = mVendor.CountryName
                                    mSalesInvoice.Visibility = 2

                                End If

                            Else

                                mSalesInvoiceItem.CGSTPercentage = 0
                                mSalesInvoiceItem.SGSTPercentage = 0
                                mSalesInvoiceItem.CGSTCAmount = 0
                                mSalesInvoiceItem.SGSTCAmount = 0
                                mSalesInvoiceItem.IGSTPercentage = 0
                                mSalesInvoiceItem.IGSTCAmount = 0
                                mSalesInvoiceItem.HSNACSCode = ""
                                mSalesInvoice.StateCode = mVendor.StateCode
                                mSalesInvoice.ClientStateCode = mVendor.ClientStateCode
                                mSalesInvoice.VendorCountry = mVendor.CountryName
                                mSalesInvoice.Visibility = 3

                            End If

                        End With

                        i = i + 1

                    Next

                End If

            End If

        End If
        'End

        mSalesInvoice.IsRoundOff = chkIsRoundOff.Checked
        mSalesInvoice.CalculateTotal()     'Added By Saylee on 10-Sep-2007

    End Sub

    Private Sub SetVendorDetails()

        mSalesInvoice.VendorID = New Guid(cmbVendorList.SelectedValue)
        mSalesInvoice.DispatchNo = txtDispatchNo.Text

        If txtDispatchDate.Text = "" Then
            mSalesInvoice.DispatchDate = DBNull.Value
        Else
            mSalesInvoice.DispatchDate = CDate(txtDispatchDate.Text)
        End If

        mSalesInvoice.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
        mSalesInvoice.ConversionFactor = Val(txtConversionFactor.Text)

    End Sub

    Private Sub DeleteRecord(Index As Int32)

        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem,
                        MSGBox.Message_text.RemoveItem,
                        "",
                        MsgBoxStyle.YesNo,
                        "Delete")

        mSalesInvoice.SalesInvoiceItems.CurrentIndex = Index
        Session("mSalesInvoice") = mSalesInvoice

    End Sub

    Private Sub DeleteChargeRecord(Index As Int32)

        MSGBoxCtrl.show(MSGBox.Message_title.RemoveCharge,
                        MSGBox.Message_text.RemoveCharge,
                        "",
                        MsgBoxStyle.YesNo,
                        "DeleteCharge")

        mSalesInvoice.SalesInvoiceCharges.CurrentIndex = Index
        Session("mSalesInvoice") = mSalesInvoice

    End Sub

    Private Sub DeleteSalesInvoiceTerms(Index As Int32)

        MSGBoxCtrl.show(MSGBox.Message_title.RemoveTerm,
                        MSGBox.Message_text.RemoveTerm,
                        "",
                        MsgBoxStyle.YesNo,
                        "DeleteSalesInvoiceTerms")

        mSalesInvoice.SalesInvoiceTerms.CurrentIndex = Index
        Session("mSalesInvoice") = mSalesInvoice

    End Sub

    Private Overloads Sub SetFocus(control As WebControl)

        If control.Enabled = False Or control.Visible = False Then Exit Sub
        control.Focus()

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
                            mSalesInvoice = CType(Session("mSalesInvoice"), SalesInvoice)
                            mSalesInvoice.SalesInvoiceItems.Remove(mSalesInvoice.SalesInvoiceItems.CurrentItem)
                            mSalesInvoice.CalculateTotal()             'Added By Saylee on 10-Sep-2007
                            If mSalesInvoice.IsRoundOff = True Then    'ALL25102012
                                mSalesInvoice.RoundCGrandTotal()
                            End If
                            Session("mSalesInvoice") = mSalesInvoice
                            upnlSalesInvoiceDetails.Update()
                            ControlVisibility()

                        Catch ex As SqlException

                            MSGBoxCtrl.show(MSGBox.Message_title.Alert,
                                            MSGBox.Message_text.Alert,
                                            ex.Message,
                                            MsgBoxStyle.OkOnly,
                                            "")

                            Exit Sub

                        End Try

                    End If

                    If MSGBoxCtrl.Sender = "DeleteCharge" Then

                        Try

                            Session("Sender") = ""
                            mSalesInvoice = CType(Session("mSalesInvoice"), SalesInvoice)
                            mSalesInvoice.SalesInvoiceCharges.Remove(mSalesInvoice.SalesInvoiceCharges.CurrentItem)
                            mSalesInvoice.CalculateTotal()             'Added By Saylee on 10-Sep-2007

                            If mSalesInvoice.IsRoundOff = True Then    'ALL25102012
                                mSalesInvoice.RoundCGrandTotal()
                            End If

                            Session("mSalesInvoice") = mSalesInvoice

                        Catch ex As SqlException

                            MSGBoxCtrl.show(MSGBox.Message_title.Alert,
                                            MSGBox.Message_text.Alert,
                                            ex.Message,
                                            MsgBoxStyle.OkOnly,
                                            "")

                            Exit Sub

                        End Try

                    End If

                    If MSGBoxCtrl.Sender = "DeleteSalesInvoiceTerms" Then

                        Try

                            Session("Sender") = ""
                            mSalesInvoice = CType(Session("mSalesInvoice"), SalesInvoice)
                            mSalesInvoice.SalesInvoiceTerms.Remove(mSalesInvoice.SalesInvoiceTerms.CurrentItem)
                            Session("mSalesInvoice") = mSalesInvoice
                            dgSalesInvoiceTerms.DataSource = mSalesInvoice.SalesInvoiceTerms
                            dgSalesInvoiceTerms.DataBind()
                            upnlSalesInvoiceTerms.Update()

                        Catch ex As SqlException

                            MSGBoxCtrl.show(MSGBox.Message_title.Alert,
                                            MSGBox.Message_text.Alert,
                                            ex.Message,
                                            MsgBoxStyle.OkOnly,
                                            "")
                            Exit Sub

                        End Try

                    End If

                    If MSGBoxCtrl.Sender = "Close" Then

                        Session("sender") = ""
                        If mSalesInvoice.IsValid = True Then

                            Session.Remove("IsValid")
                            DataFieldBind()

                            If (Not User.IsInRole("SalesInvoiceNew") And
                                Not User.IsInRole("SalesInvoiceEdit")) Then

                                ScriptManager.RegisterStartupScript(Me,
                                                                    [GetType],
                                                                    "OpenScript",
                                                                    MessageBox.Show("You are not authorized user", False),
                                                                    True)
                                Exit Sub
                            End If

                            Save()

                            Response.Redirect("Index.aspx")

                        Else

                            Session.Remove("IsValid")
                            If CustomValidate1() = False Then

                                upnlValidationsummary.Update()
                                Exit Sub

                            End If

                        End If

                    End If

                    If MSGBoxCtrl.Sender = "Status" Then

                        Session("sender") = ""
                        If mSalesInvoice.IsValid = True Then

                            mSalesInvoice.StatusID = 2
                            Save()
                            DataFieldBind()
                            ControlVisibility()
                            upnlSalesInvoiceTerms.Update()
                            upnlSalesInvoiceItems.Update()

                        Else

                            If CustomValidate1() = False Then

                                upnlValidationsummary.Update()
                                Exit Sub

                            End If

                        End If

                    End If

                    If MSGBoxCtrl.Sender = "StatusCancel" Then

                        Session("sender") = ""
                        mSalesInvoice.StatusID = 4
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

                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        Session("mSalesInvoice") = mSalesInvoice
                        DataFieldBind()
                        UpdatePanel()
                        upnlSalesInvoiceItems.Update()
                        upnlSalesInvoiceCharge.Update()
                        upnlSalesInvoiceTerms.Update()

                    End If

            End Select

        End If

    End Sub

    Private Sub AddAttributes()

        txtInvoiceNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtInvoiceNo').value,event)")
        txtConversionFactor.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtConversionFactor').value,event)")
    End Sub

    Public Sub TextChanged(sender As Object, e As EventArgs)

        Dim txtValue As TextBox
        Dim mSalesInvoiceItem As SalesInvoiceItem
        Dim i As Integer = 0

        For Each mSalesInvoiceItem In mSalesInvoice.SalesInvoiceItems

            With mSalesInvoiceItem

                Try

                    txtValue = CType(Me.dgSalesInvoiceItem.Rows(i).FindControl("txtWCGST"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                    txtValue = CType(Me.dgSalesInvoiceItem.Rows(i).FindControl("txtWIGST"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                    txtValue = CType(Me.dgSalesInvoiceItem.Rows(i).FindControl("txtWSGST"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                Catch ex As Exception
                End Try

            End With

            i = i + 1

        Next

        upnlSalesInvoiceItems.Update()

    End Sub

    Private Sub SetControlStatus(StatusId As Int16)

        btnAdd.Enabled = IIf(StatusId > 1, False, True)
        btnAddCharge.Enabled = IIf(StatusId > 1, False, True)
        btnSave.Visible = IIf(StatusId > 1, False, True)
        dgSalesInvoiceTerms.Columns(2).Visible = IIf(StatusId > 1, False, True) 'Added By Utkarsh ON 19-Dec-2012 FOR ALL19122012-1
        btnAddTerm.Enabled = IIf(StatusId > 1, False, True) 'Added By Utkarsh ON 19-Dec-2012 FOR ALL19122012-1

    End Sub

    Private Sub SetPage()

        If mSalesInvoice.IsNew Then
            lblTitle.Text = "Sales Invoice Details [New]"
        Else
            lblTitle.Text = "Sales Invoice Details [ " & mSalesInvoice.Text & "-" & mSalesInvoice.No & " ]"
        End If

        upnlTitle.Update()

    End Sub

    Private Sub ControlVisibility()

        txtInvoiceText.Enabled = (CType(IIf(mSalesInvoice.StatusID = 2 Or mSalesInvoice.StatusID = 4, False, True), Boolean)) ' And mSalesInvoice.SalesInvoiceItems.Count = 0) Or (mSalesInvoice.SalesInvoiceItems.Count = 0)
        txtInvoiceNo.Enabled = (CType(IIf(mSalesInvoice.StatusID = 2 Or mSalesInvoice.StatusID = 4, False, True), Boolean)) 'And mSalesInvoice.SalesInvoiceItems.Count = 0) Or (mSalesInvoice.SalesInvoiceItems.Count = 0)
        txtInvoiceDate.Enabled = (CType(IIf(mSalesInvoice.StatusID = 2 Or mSalesInvoice.StatusID = 4, False, True), Boolean) And mSalesInvoice.SalesInvoiceItems.Count = 0) Or (mSalesInvoice.SalesInvoiceItems.Count = 0)
        cmbVendorList.Enabled = (CType(IIf(mSalesInvoice.StatusID = 2 Or mSalesInvoice.StatusID = 4, False, True), Boolean) And mSalesInvoice.SalesInvoiceItems.Count = 0) Or (mSalesInvoice.SalesInvoiceItems.Count = 0)
        txtDispatchDate.Enabled = (CType(IIf(mSalesInvoice.StatusID = 2 Or mSalesInvoice.StatusID = 4, False, True), Boolean)) '' And mSalesInvoice.SalesInvoiceItems.Count = 0) Or (mSalesInvoice.SalesInvoiceItems.Count = 0)
        cmbCurrencyList.Enabled = (CType(IIf(mSalesInvoice.StatusID = 2 Or mSalesInvoice.StatusID = 4, False, True), Boolean)) '' And mSalesInvoice.SalesInvoiceItems.Count = 0) Or (mSalesInvoice.SalesInvoiceItems.Count = 0)
        txtConversionFactor.Enabled = (CType(IIf(mSalesInvoice.StatusID = 2 Or mSalesInvoice.StatusID = 4, False, True), Boolean)) '' And mSalesInvoice.SalesInvoiceItems.Count = 0) Or (mSalesInvoice.SalesInvoiceItems.Count = 0)
        txtDispatchNo.Enabled = (CType(IIf(mSalesInvoice.StatusID = 2 Or mSalesInvoice.StatusID = 4, False, True), Boolean)) '' And mSalesInvoice.SalesInvoiceItems.Count = 0) Or (mSalesInvoice.SalesInvoiceItems.Count = 0)
        btnAuthorized.Visible = (Not mSalesInvoice.SalesInvoiceItems.Count = 0) And (Not mSalesInvoice.IsNew) And (mSalesInvoice.StatusID = 1)
        btnCancel.Visible = (Not mSalesInvoice.IsNew) And (mSalesInvoice.StatusID = 2)
        chkIsRoundOff.Enabled = (mSalesInvoice.StatusID = 1)

        'Added By Prashant 17-Aug-2011
        If Not User.IsInRole("SalesInvoiceAuthorized") Then

            btnAuthorized.Enabled = False
            btnAuthorized.ToolTip = "You are not authorized user "
            btnCancel.Enabled = False
            btnCancel.ToolTip = "You are not authorized user "

        End If

        'Added By Vikrant on 29-Jan-2018 For Deccan29012018-1
        If mSalesInvoice.Visibility = 1 Or mSalesInvoice.Visibility = 2 Then

            Dim txtCGSTPercentage As TextBox
            Dim txtSGSTPercentage As TextBox
            Dim txtIGSTPercentage As TextBox

            For i As Integer = 0 To dgSalesInvoiceItem.Rows.Count - 1

                txtCGSTPercentage = CType(Me.dgSalesInvoiceItem.Rows(i).FindControl("txtWCGST"), TextBox)
                txtSGSTPercentage = CType(Me.dgSalesInvoiceItem.Rows(i).FindControl("txtWSGST"), TextBox)
                txtIGSTPercentage = CType(Me.dgSalesInvoiceItem.Rows(i).FindControl("txtWIGST"), TextBox)
                txtCGSTPercentage.ReadOnly = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mSalesInvoice.StatusID >= 2 Or mSalesInvoice.SalesInvoiceItems(i).HSNACSCode = "", True, False) 'CGSTPercentage 
                txtIGSTPercentage.ReadOnly = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mSalesInvoice.StatusID >= 2 Or mSalesInvoice.SalesInvoiceItems(i).HSNACSCode = "", True, False) 'IGSTPercentage 
                txtCGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mSalesInvoice.StatusID >= 2 Or mSalesInvoice.SalesInvoiceItems(i).HSNACSCode = "", Color.Gainsboro, Color.White) 'CGSTPercentage 
                txtIGSTPercentage.BackColor = IIf(AppSettings("ChangeGSTPercentage") <> "True" Or mSalesInvoice.StatusID >= 2 Or mSalesInvoice.SalesInvoiceItems(i).HSNACSCode = "", Color.Gainsboro, Color.White) 'IGSTPercentage 

            Next

        End If
        'End

        If mSalesInvoice.TransTypeID = 74 Then

            dgSalesInvoiceItem.Columns(5).Visible = False 'ItemTypeName 
            dgSalesInvoiceItem.Columns(6).Visible = False 'IssueNumber 
            dgSalesInvoiceItem.Columns(7).Visible = False 'IssueDateFormatted 
            dgSalesInvoiceItem.Columns(8).Visible = False 'ReceiptNumber 
            dgSalesInvoiceItem.Columns(9).Visible = False 'ReceiptDateFormatted 
            dgSalesInvoiceItem.Columns(10).Visible = False 'ReleaseNoteNo 
            dgSalesInvoiceItem.Columns(11).Visible = False 'ReleaseNoteDateFormatted

        End If

        If mSalesInvoice.Visibility = 1 Then

            dgSalesInvoiceItem.Columns(17).Visible = True 'CGSTPercentage 
            dgSalesInvoiceItem.Columns(18).Visible = True 'CGSTCAmount 
            dgSalesInvoiceItem.Columns(19).Visible = True 'SGSTPercentage 
            dgSalesInvoiceItem.Columns(20).Visible = True 'SGSTCAmount 
            dgSalesInvoiceItem.Columns(21).Visible = False 'IGSTPercentage 
            dgSalesInvoiceItem.Columns(22).Visible = False 'IGSTCAmount 
            lblTotalCGST.Visible = True
            txtTotalCGST.Visible = True
            lblTotalSGST.Visible = True
            txtTotalSGST.Visible = True
            lblTotalIGST.Visible = False
            txtTotalIGST.Visible = False

        ElseIf mSalesInvoice.Visibility = 2 Then

            dgSalesInvoiceItem.Columns(17).Visible = False 'CGSTPercentage 
            dgSalesInvoiceItem.Columns(18).Visible = False 'CGSTCAmount 
            dgSalesInvoiceItem.Columns(19).Visible = False 'SGSTPercentage 
            dgSalesInvoiceItem.Columns(20).Visible = False 'SGSTCAmount 
            dgSalesInvoiceItem.Columns(21).Visible = True  'IGSTPercentage 
            dgSalesInvoiceItem.Columns(22).Visible = True 'IGSTCAmount 
            lblTotalCGST.Visible = False
            txtTotalCGST.Visible = False
            lblTotalSGST.Visible = False
            txtTotalSGST.Visible = False
            lblTotalIGST.Visible = True
            txtTotalIGST.Visible = True

        ElseIf mSalesInvoice.Visibility = 3 Then

            dgSalesInvoiceItem.Columns(17).Visible = False 'CGSTPercentage 
            dgSalesInvoiceItem.Columns(18).Visible = False 'CGSTCAmount 
            dgSalesInvoiceItem.Columns(19).Visible = False 'SGSTPercentage 
            dgSalesInvoiceItem.Columns(20).Visible = False 'SGSTCAmount 
            dgSalesInvoiceItem.Columns(21).Visible = False  'IGSTPercentage 
            dgSalesInvoiceItem.Columns(22).Visible = False 'IGSTCAmount 
            dgSalesInvoiceItem.Columns(4).Visible = False 'HSNACSCode 
            lblTotalCGST.Visible = False
            txtTotalCGST.Visible = False
            lblTotalSGST.Visible = False
            txtTotalSGST.Visible = False
            lblTotalIGST.Visible = False
            txtTotalIGST.Visible = False

        End If

    End Sub

    Private Function Save() As Boolean

        'Authentication
        If Not mSalesInvoice.SalesInvoiceDate Is DBNull.Value Then

            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then

                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------
                If DateDiff(DateInterval.Day, CDate(mSalesInvoice.SalesInvoiceDate), maxAllowableDate) < 0 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
                                    MSGBox.Message_text.saveAlert,
                                    " Your subscription has been expired. can not save SalesInvoice. <br> SalesInvoice Date can not be greater than " &
                                    maxAllowableDate.ToString(WebDateFormat),
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Return False

                    Exit Function

                End If

            End If

        End If

        'Authentication
        Dim SalesInvoiceClone As SalesInvoice
        SalesInvoiceClone = mSalesInvoice.Clone

        Try

            If Not mSalesInvoice.SalesInvoiceItems.Count = 0 Then

                SetObject()
                SetVendorDetails()
                '===Added By Saylee on 27th-Dec-2007 =======
                Dim mSalesInvoiceCharge As SalesInvoiceCharge

                For Each mSalesInvoiceCharge In mSalesInvoice.SalesInvoiceCharges

                    If (mSalesInvoiceCharge.Sign <> 1 And mSalesInvoiceCharge.CChargeAmount <= 0) Or
                       (Not (mSalesInvoiceCharge.IsValid)) Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert,
                                        MSGBox.Message_text.ValidationAlert,
                                        "Percentage Sales Invoice Charge(s) are not allowed if SalesInvoice Amount Is Zero. ",
                                        MsgBoxStyle.OkOnly,
                                        "")

                        mSalesInvoice.CancelEdit()

                        Return False

                        Exit Function

                    End If

                Next

                '====================================================================
                If mSalesInvoice.IsRoundOff = True Then  'Added By Prashant on 29-Oct-2012
                    mSalesInvoice.RoundCGrandTotal()
                End If

                'Added by Utkarsh on 20-Nov-2013 FOr TransTextSeries 
                'Check if Sales Invoice is blank then call TransTextSeries UI

                If (mSalesInvoice.IsNew) And (mSalesInvoice.Text = "") Then

                    Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mSalesInvoice.TransTypeID,
                                                                                                                 mSalesInvoice.SalesInvoiceDateFormatted)

                    If (mPreviousTransTextSeries.IsAutoRenew = False) Or
                       ((mPreviousTransTextSeries.IsAutoRenew = True) And
                       (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mSalesInvoice.TransTypeID) = False) Or
                       (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mSalesInvoice.TransTypeID) = True AndAlso
                       mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mSalesInvoice.TransTypeID).TransText = "")) Then

                        Dim str = "<script language='javascript'>openledgersame('wfSalesInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"
                        Session("BackPagestr_ForTransSeries") = str

                        Session("TransName_ForTransSeries") = "Sales Invoice"
                        Session("TransTypeID_ForTransSeries") = mSalesInvoice.TransTypeID
                        Session("TransDate_ForTransSeries") = mSalesInvoice.SalesInvoiceDateFormatted
                        Session("AddTransTextSeries") = "True"
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")

                        Return False

                    Else

                        Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                        If mAutoRenewTransTextSeries.IsRenewed Then

                            With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mSalesInvoice.TransTypeID)

                                mSalesInvoice.Text = .TransText
                                mSalesInvoice.No = .StartingTransNo

                            End With

                        Else

                            Dim str = "<script language='javascript'>openledgersame('wfSalesInvoice_Ajax.aspx?BackPage=" &
                                       Request.QueryString("BackPage") & "');</script>"

                            Session("BackPagestr_ForTransSeries") = str
                            Session("TransName_ForTransSeries") = "Sales Invoice"
                            Session("TransTypeID_ForTransSeries") = mSalesInvoice.TransTypeID
                            Session("TransDate_ForTransSeries") = mSalesInvoice.SalesInvoiceDateFormatted
                            Session("AddTransTextSeries") = "True"
                            Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                            Return False

                        End If

                    End If

                End If
                'End

                mSalesInvoice.Save()

                Dim mInvoiceDetail = mSalesInvoice.SalesInvoiceNo +
                                     " Dated : " + mSalesInvoice.SalesInvoiceDateFormatted + " to " +
                                     mVendorList(mSalesInvoice.VendorID).Name

                If mSalesInvoice.StatusID = 2 Then

                    MarkLog(Action.Authorize,
                            "SalesInvoice",
                            mInvoiceDetail,
                            ErrorType.NoError,
                            mSalesInvoice.ID,
                            EventLogID)

                ElseIf mSalesInvoice.StatusID = 3 Then

                    MarkLog(Action.Amend,
                            "SalesInvoice",
                            mInvoiceDetail,
                            ErrorType.NoError,
                            mSalesInvoice.ID,
                            EventLogID)

                ElseIf mSalesInvoice.StatusID = 4 Then

                    MarkLog(Action.Cancel,
                            "SalesInvoice",
                            mInvoiceDetail,
                            ErrorType.NoError,
                            mSalesInvoice.ID,
                            EventLogID)

                Else

                    MarkLog(Action.Save,
                            "SalesInvoice",
                            mInvoiceDetail,
                            ErrorType.NoError,
                            mSalesInvoice.ID,
                            EventLogID)

                End If

                mSalesInvoice.MarkClean()
                Session("mSalesInvoice") = mSalesInvoice
                SetPage()
                UpdatePanel()
                upnlSalesInvoiceTerms.Update()
                SalesInvoiceChargesGrid()
                SetChargeGrid()
                SetInvoiceItemGrid()

                If mSalesInvoice.StatusID = 2 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.AuthorizedSuccessFully,
                                    MSGBox.Message_text.AuthorizedSuccessFully,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf mSalesInvoice.StatusID = 4 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.CanceledSuccessFully,
                                    MSGBox.Message_text.CanceledSuccessFully,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")

                Else

                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully,
                                    MSGBox.Message_text.SavedSuccessFully,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")

                End If

            Else

                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
                                MSGBox.Message_text.saveAlert,
                                "Sales Invoice can not be saved without Item.",
                                MsgBoxStyle.OkOnly,
                                "")

                Return False

                Exit Function

            End If

        Catch ex As SqlException

            Session("SalesInvoiceClone") = SalesInvoiceClone

            If ex.Number = 8114 Or ex.Number = 8115 Then

                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow,
                                MSGBox.Message_text.NumericOverFlow,
                                " Rate or Qty or Conversion Factor. ",
                                MsgBoxStyle.OkOnly,
                                "")

                Return False
                Exit Function

            ElseIf ex.Number = 8145 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.ProcedureError,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")
                Return False
                Exit Function

            ElseIf ex.Number = 2627 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.Duplicate,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")
                Return False
                Exit Function

            ElseIf ex.Number = 547 Then

                If InStr(ex.Message, "CCtabIssueItemInvoiceBalQty", CompareMethod.Text) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty,
                                    MSGBox.Message_text.PendingQty,
                                    "Qty. Not Available",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Return False
                    Exit Function

                ElseIf InStr(ex.Message, "CCtabSalesInvoiceNo", CompareMethod.Text) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
                                    MSGBox.Message_text.saveAlert,
                                    "No. Required",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Return False
                    Exit Function

                Else

                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                    MSGBox.Message_text.ReferenceDelete,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Return False
                    Exit Function

                End If

            End If

        Finally
            SalesInvoiceClone = Nothing
        End Try

    End Function

    Private Sub SetChargeGrid()

        Try

            For j As Integer = 0 To dgSalesInvoiceCharge.Rows.Count - 1

                If (Me.dgSalesInvoiceCharge.Rows.Item(j).Cells(1).Text = "Round off (Plus)" Or
                    Me.dgSalesInvoiceCharge.Rows.Item(j).Cells(1).Text = "Round off (Minus)") Then

                    dgSalesInvoiceCharge.Rows.Item(j).Cells(5).Enabled = False

                End If

            Next

            For Each column As DataControlField In dgSalesInvoiceCharge.Columns

                Select Case column.HeaderText
                    Case "Action"
                        column.Visible = IIf(mSalesInvoice.StatusID > 1, False, True)
                End Select

            Next

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub SetInvoiceItemGrid()

        Try

            For Each column As DataControlField In dgSalesInvoiceItem.Columns

                Select Case column.HeaderText
                    Case "Action"
                        column.Visible = IIf(mSalesInvoice.StatusID > 1, False, True)
                End Select

            Next

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub UpdatePanel()

        ControlsDataBind()
        upnlStatusName.Update()
        upnlSalesInvoiceDetails.Update()
        upnlSupplierDetails.Update()
        upnlOtherDetails.Update()
        upnlButtons.Update()
        SetControlStatus(mSalesInvoice.StatusID)
        ControlVisibility()

    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        mCurrencyList = CurrencyList.GetCurrencyList(, , True)
        mVendorList = VendorList.GetVendortList(0, , , , , , True, True, False)
        mStatusList = StatusList.GetStatusList(mSalesInvoice.StatusID, , )

        cmbVendorList.DataSource = mVendorList
        cmbCurrencyList.DataSource = mCurrencyList

        Session("mCurrencyList") = mCurrencyList
        Session("mVendorList") = mVendorList
        Session("mStatusList") = mStatusList

        dgSalesInvoiceItem.DataSource = mSalesInvoice.SalesInvoiceItems
        dgSalesInvoiceCharge.DataSource = mSalesInvoice.SalesInvoiceCharges
        dgSalesInvoiceTerms.DataSource = mSalesInvoice.SalesInvoiceTerms 'Added By Utkarsh ON 19-Dec-2012 FOR ALL19122012-1
        txtInvoiceDate.Text = mSalesInvoice.SalesInvoiceDateFormatted.ToString
        txtDispatchDate.Text = mSalesInvoice.DispatchDateFormatted.ToString

        DataBind()

    End Sub

    Private Sub ControlsDataBind()

        upnlStatusName.DataBind()
        upnlSalesInvoiceDetails.DataBind()
        upnlSupplierDetails.DataBind()
        upnlOtherDetails.DataBind()
        upnlButtons.DataBind()

    End Sub

    Private Sub SalesInvoiceChargesGrid()

        dgSalesInvoiceCharge.DataSource = mSalesInvoice.SalesInvoiceCharges
        dgSalesInvoiceCharge.DataBind()
        upnlSalesInvoiceCharge.Update()
        upnlOtherDetails.Update()
        upnlOtherDetails.DataBind()

    End Sub

    Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)

        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If custValidator.ControlToValidate = "txtInvoiceDate" Then

            If txtInvoiceDate.Text = "" Then
                custValidator.ErrorMessage = "Select Invoice Date."
                e.IsValid = False
            End If

        ElseIf custValidator.ControlToValidate = "cmbVendorList" Then

            If cmbVendorList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Customer from the list."
                e.IsValid = False
            End If

        ElseIf custValidator.ControlToValidate = "cmbCurrencyList" Then

            If cmbCurrencyList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Currency from the List."
                e.IsValid = False
            End If

        ElseIf custValidator.ControlToValidate = "txtConversionFactor" Then

            If Val(txtConversionFactor.Text) <= 0 Then
                custValidator.ErrorMessage = "Currency factor must be greater than zero."
                e.IsValid = False
            End If

        ElseIf custValidator.ControlToValidate = "txtAmountInWords" Then

            If Len(txtAmountInWords.Text) > 250 Then
                e.IsValid = False
            End If

        End If

    End Sub

    'GST Changes
    Private Sub SetSalesInvoiceDetails(stateCode As String,
                                       ClientStateCode As String,
                                       CountryName As String,
                                       Visibility As Integer)

        mSalesInvoice.StateCode = stateCode
        mSalesInvoice.ClientStateCode = ClientStateCode
        mSalesInvoice.VendorCountry = CountryName
        mSalesInvoice.Visibility = Visibility

    End Sub
    'End

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GetSession()
        AddAttributes()
        SetPage()
        SetControlStatus(mSalesInvoice.StatusID)

        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 21-July-2011

        TextChanged(sender, e) 'Added By Vikrant on 29-Jan-2018 For Deccan29012018-1

        If Not IsPostBack And Session("sender") = "" Then

            If AppSettings("AutoCompleteTransText") <> "True" Then 'Added by VIkrant For ALL23052012

                If txtInvoiceText.Enabled = True Then
                    SetFocus(txtInvoiceText)
                End If

            End If

            'Added by Utkarsh on 19-Nov-2013 for Trans Text Series
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso
               (Not Session("TransText_ForTransSeries") Is Nothing) Then

                If mSalesInvoice.IsNew Then

                    mSalesInvoice.Text = Session("TransText_ForTransSeries")
                    txtInvoiceText.Text = mSalesInvoice.Text
                    Session("mSalesInvoice") = mSalesInvoice
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")

                End If

            End If
            'End

            DataFieldBind()

        End If

        ControlVisibility()

        If chkIsRoundOff.Checked = True Then  'Added By Prashant on 29-Oct-2012
            SetChargeGrid()
        End If

        SetInvoiceItemGrid()

    End Sub

    Private Sub AddRecord(sender As Object, e As EventArgs) Handles btnAdd.Click

        If IsValid = False Then upnlValidationsummary.Update() : Exit Sub
        SetObject()
        SetVendorDetails()
        mSalesInvoice.SalesInvoiceItems.Add(mSalesInvoice.ID)
        mSalesInvoice.SalesInvoiceItems.CurrentItem.Currency = IIf(cmbCurrencyList.SelectedIndex >= 0, cmbCurrencyList.SelectedItem.Text, "")
        Session("mSalesInvoice") = mSalesInvoice
        Response.Redirect("wfSalesInvoiceItem_Ajax.aspx?BackPage=wfSalesInvoice_Ajax.aspx")

    End Sub

    Private Sub AddCharge(sender As Object, e As EventArgs) Handles btnAddCharge.Click

        If IsValid Then

            SetObject()
            SetVendorDetails()
            mSalesInvoice.SalesInvoiceCharges.Add(mSalesInvoice.ID)
            Session("mSalesInvoice") = mSalesInvoice

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "Open Invoice Charge Window",
                                                "OpenSalesInvoiceChargeWindow()",
                                                True)

        End If

    End Sub

    Private Sub AddTerms(sender As Object, e As EventArgs) Handles btnAddTerm.Click

        If IsValid Then

            SetObject()
            SetVendorDetails()
            Session("mSalesInvoice") = mSalesInvoice

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "Open Invoice Term Window",
                                                "OpenSalesInvoiceTermWindow()",
                                                True)

        End If

    End Sub

    Private Sub GV_SalesInvoiceItems_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgSalesInvoiceItem.RowCommand

        Select Case e.CommandName
            Case "EditView"

                Dim Index As Integer = CInt(e.CommandArgument)
                Session("Edit") = True
                SetObject()
                SetVendorDetails()
                mSalesInvoice.SalesInvoiceItems.CurrentIndex = Index
                Session("mSalesInvoice") = mSalesInvoice
                Response.Redirect("wfSalesInvoiceItem_Ajax.aspx?BackPage=wfSalesInvoice_Ajax.aspx")

            Case "DeleteRecord"
                Dim Index As Integer = CInt(e.CommandArgument)
                DeleteRecord(Index)

        End Select

    End Sub

    Private Sub GV_SalesInvoiceTerms_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgSalesInvoiceTerms.RowCommand

        Select Case e.CommandName

            Case "DeleteRecord"
                Dim Index As Integer = CInt(e.CommandArgument)
                DeleteSalesInvoiceTerms(Index)

        End Select

    End Sub

    Private Sub GV_SalesInvoiceCharge_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgSalesInvoiceCharge.RowCommand

        Select Case e.CommandName
            Case "EditView"

                Dim Index As Integer = CInt(e.CommandArgument)
                Session("Edit") = True
                SetObject()
                SetVendorDetails()
                mSalesInvoice.SalesInvoiceCharges.CurrentIndex = Index
                Session("mSalesInvoice") = mSalesInvoice

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "Open Invoice Charge Window",
                                                    "OpenSalesInvoiceChargeWindow()",
                                                    True)
            Case "DeleteRecord"

                Dim Index As Integer = CInt(e.CommandArgument)
                DeleteChargeRecord(Index)

        End Select

    End Sub

    Private Sub CurrencyList_Changed(sender As Object, e As EventArgs) Handles cmbCurrencyList.SelectedIndexChanged

        txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
        SetVendorDetails()

        If cmbCurrencyList.Enabled = True Then
            SetFocus(cmbCurrencyList)
        End If

        upnlValidationsummary.Update()

    End Sub

    Private Sub Save_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If (Not User.IsInRole("SalesInvoiceNew") And
            Not User.IsInRole("SalesInvoiceEdit")) Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "OpenScript",
                                                MessageBox.Show("You are not authorized user", False),
                                                True)
            Exit Sub

        End If

        If CustomValidate1() = False Then

            upnlValidationsummary.Update()
            Exit Sub

        End If

        If IsValid Then

            If Save() Then

                Response.Redirect("wfSalesInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))

            End If

        Else
            upnlValidationsummary.Update()
        End If

    End Sub

    Private Sub Back_Click(sender As Object, e As EventArgs) Handles btnBack.Click

        Dim mInvoiceDetail = mSalesInvoice.SalesInvoiceNo +
                            " Dated : " + mSalesInvoice.SalesInvoiceDateFormatted +
                            IIf(cmbVendorList.SelectedIndex > 0, " to " + cmbVendorList.SelectedItem.Text, "")

        MarkLog(Action.Close,
                "SalesInvoice",
                mInvoiceDetail,
                ErrorType.NoError,
                Guid.Empty,
                EventLogID)

        SetObject()
        SetVendorDetails()

        If mSalesInvoice.IsDirty Then

            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm,
                            MSGBox.Message_text.CloseConfirm,
                            "",
                            MsgBoxStyle.YesNo,
                            "Close")
        Else
            Response.Redirect("Index.aspx")
        End If

    End Sub

    Private Sub PrintReport(sender As Object, e As EventArgs) Handles btnPrint.Click

        If Not User.IsInRole("SalesInvoicePrint") Then

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                            MSGBox.Message_text.Authorization,
                            "",
                            MsgBoxStyle.OkOnly,
                            "")
            Exit Sub

        End If

        Dim da As New ObjectAdapter
        Dim rpt As Engine.ReportClass
        Dim obj As rptSalesInvoices
        Dim objChilds As rptSalesInvoiceChilds
        Dim letter As rptLetterHead
        Dim ds As New dsSalesInvoice
        obj = rptSalesInvoices.GetSalesInvoices(mSalesInvoice.ID)
        objChilds = rptSalesInvoiceChilds.GetSalesInvoiceChilds(mSalesInvoice.ID)

        If CDate(txtInvoiceDate.Text) <= CDate("30-Jun-2017") Then
            rpt = New crptSalesInvoiceDetailPortrait
        ElseIf mSalesInvoice.Visibility = 3 Then
            rpt = New crptSalesInvoiceDetailPortrait
        Else
            rpt = New crptSalesInvoiceGSTDetail
        End If

        'Added by Archana on 4-nov-2009
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso
           (AppSettings("ClientCode") = "TAAL" Or
            AppSettings("ClientCode") = "GlobalJet") Then

            letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"),
                                                     "Authorized Sales Representative(ASR) in India. For Cessna Citation & Caravan aircraft.",
                                                     "",
                                                     "True")
        Else

            letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"),
                                                     "",
                                                     "",
                                                     AppSettings("Logo"))

        End If

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, obj)
        da.Fill(ds, objChilds)
        da.Fill(ds, letter)
        da.Fill(ds, mrptImage)
        rpt.SetDataSource(ds)
        Session("CrystalReport") = rpt
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me,
                                            [GetType],
                                            "openTranDetail",
                                            Str1,
                                            True)

    End Sub

    Private Sub InvoiceDate_Changed(sender As Object, e As EventArgs) Handles txtInvoiceDate.TextChanged

        If Not (New SmartDate(mSalesInvoice.SalesInvoiceDate.ToString, True).Text = New SmartDate(CType(Trim(txtInvoiceDate.Text), Object).ToString, True).Text) Then

            If txtInvoiceDate.Text = "" Then
                mSalesInvoice.SalesInvoiceDate = DBNull.Value
            Else
                mSalesInvoice.SalesInvoiceDate = CDate(txtInvoiceDate.Text)
            End If

            txtInvoiceText.Text = mSalesInvoice.Text

        End If

        Session("mSalesInvoice") = mSalesInvoice
        upnlSalesInvoiceDetails.Update()

    End Sub

    Private Sub DispatchDate_Changed(sender As Object, e As EventArgs) Handles txtDispatchDate.TextChanged

        If Not IsDate(txtDispatchDate.Text) Then
            txtDispatchDateWatermarkExtender.WatermarkText = AppSettings("DateFormat")
        End If

    End Sub

    Private Sub IsRoundOff_Changed(sender As Object, e As EventArgs) Handles chkIsRoundOff.CheckedChanged

        Dim Child As SalesInvoiceCharge

        For i As Integer = mSalesInvoice.SalesInvoiceCharges.Count - 1 To 0 Step -1

            Child = mSalesInvoice.SalesInvoiceCharges(i)
            If Child.ChargeID.Equals(New Guid("{40000000-0000-0000-0000-000000000000}")) Or Child.ChargeID.Equals(New Guid("{50000000-0000-0000-0000-000000000000}")) Then
                mSalesInvoice.SalesInvoiceCharges.Remove(Child)
            End If

        Next

        mSalesInvoice.IsRoundOff = chkIsRoundOff.Checked
        dgSalesInvoiceCharge.DataSource = mSalesInvoice.SalesInvoiceCharges
        dgSalesInvoiceCharge.DataBind()
        upnlSalesInvoiceCharge.Update()

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

    Private Sub HiddenImageButtons_Click(sender As Object, e As EventArgs) Handles hdnImgBtnSalesInvoiceTerms.Click, hdnImgBtnSalesInvoiceCharges.Click

        dgSalesInvoiceTerms.DataSource = mSalesInvoice.SalesInvoiceTerms
        dgSalesInvoiceTerms.DataBind()
        upnlSalesInvoiceTerms.Update()

        SalesInvoiceChargesGrid()
        SetChargeGrid()

    End Sub

#End Region

#Region " Status "

    Private Sub Authorized(sender As Object, e As EventArgs) Handles btnAuthorized.Click

        If IsValid Then

            SetVendorDetails()

            MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized,
                            MSGBox.Message_text.StatusAuthorized,
                            "<Strong> SalesInvoice </Strong>",
                            MsgBoxStyle.YesNo,
                            "Status")

            Session("mSalesInvoice") = mSalesInvoice

            SetInvoiceItemGrid()

        Else
            upnlValidationsummary.Update()
        End If

    End Sub

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        If IsValid Then

            MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled,
                            MSGBox.Message_text.StatusCanceled,
                            "<Strong> SalesInvoice </Strong>",
                            MsgBoxStyle.YesNo,
                            "StatusCancel")

            Session("mSalesInvoice") = mSalesInvoice

        Else
            upnlValidationsummary.Update()
        End If

    End Sub

#End Region

#Region " Show BrokenRules "

    Public Function CustomValidate1() As Boolean

        Dim strMsg As String = ""

        SetObject()

        If mSalesInvoice.IsValid = False Then

            For i As Integer = 0 To mSalesInvoice.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mSalesInvoice.GetBrokenRulesCollection(i).Description + "<Br>"
            Next

        End If

        Dim mSalesInvoiceItem As SalesInvoiceItem

        If mSalesInvoice.SalesInvoiceItems.IsValid = False Then

            For Each mSalesInvoiceItem In mSalesInvoice.SalesInvoiceItems

                For i As Integer = 0 To mSalesInvoiceItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mSalesInvoiceItem.ItemName + " : " + mSalesInvoiceItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next

            Next

        End If

        If strMsg.Trim <> "" Then

            cvCustomer.ErrorMessage = strMsg
            cvCustomer.IsValid = False
            Return False

        End If

        Return True

    End Function

    Public Sub CustomValidate1(s As Object, e As ServerValidateEventArgs)

        If Flag = 1 Then Exit Sub

        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        Dim strMsg As String = ""

        SetObject()

        If mSalesInvoice.IsValid = False Then

            For i As Integer = 0 To mSalesInvoice.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mSalesInvoice.GetBrokenRulesCollection(i).Description + "<Br>"
            Next

        End If

        Dim mSalesInvoiceItem As SalesInvoiceItem

        If mSalesInvoice.SalesInvoiceItems.IsValid = False Then

            For Each mSalesInvoiceItem In mSalesInvoice.SalesInvoiceItems

                For i As Integer = 0 To mSalesInvoiceItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mSalesInvoiceItem.ItemName + " : " + mSalesInvoiceItem.GetBrokenRulesCollection(i).Description + "<Br>"
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

End Class