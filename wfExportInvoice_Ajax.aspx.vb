Imports System.Linq
Public Class wfExportInvoice_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mExportInvoice As ExportInvoice
    Public mVendorList As VendorList
    Public mStatusList As StatusList
    Public mCurrencyList As CurrencyList
    Dim EventLogID As Guid
    Dim ExpInvDetail As String
    Public mModuleName As String = "ExportInvoice"
    Dim MaxBoxNo As Integer = 0
    Dim MissingSequence As String = String.Empty
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mExportInvoice = Session("mExportInvoice")
        mVendorList = Session("mVendorList")
        mStatusList = Session("mStatusList")
        mCurrencyList = Session("mCurrencyList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mStatusList")
        Session.Remove("mCurrencyList")
        Session.Remove("mExportInvoice")
        Session.Remove("mVendorList")
    End Sub
    Private Sub SetPage()
        If mExportInvoice.IsNew Then
            lblTitle.Text = "Export Invoice [New]"
        Else
            lblTitle.Text = "Export Invoice [ " & mExportInvoice.ExportInvoiceTextNo & " ]"
        End If
        upnlTitle.Update()
    End Sub
    Private Sub ControlVisibility()
        txtExportInvoiceText.Enabled = CType(IIf(mExportInvoice.StatusID >= 2, False, True), Boolean)
        txtExportInvoiceNo.Enabled = CType(IIf(mExportInvoice.StatusID >= 2, False, True), Boolean)
        btnCancel.Visible = (Not mExportInvoice.IsNew) And (mExportInvoice.StatusID = 2)
        btnAuthorized.Visible = (Not mExportInvoice.ExportInvoiceItems.Count = 0) And (Not mExportInvoice.IsNew) And (mExportInvoice.StatusID = 1)
        cmbVendorList.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        cmbBuyer.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)

        txtExportInvoiceText.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtExportInvoiceNo.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtExporterRef.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtBuyerAddress.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtBuyerAttn.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtBuyerOrderNo.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtSupAddress.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtSupplierAttn.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtBuyerOtherReferences.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtBuyerCountryOfOrigin.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtBuyerCountryOfFinal.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtSupPreCarriage.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtSupPlaceofReceipt.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtFlightNo.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtPortOfLoading.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtPortOfDischarge.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtFinalDestination.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtIECCodeNo.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtInvoiceTo.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        txtRemark.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)
        btnExportInvoiceBox.Enabled = IIf(mExportInvoice.StatusID >= 2, False, True)

        chkIsRoundOff.Enabled = (mExportInvoice.StatusID = 1)
        cmbCurrencyList.Enabled = (CType(IIf(mExportInvoice.StatusID >= 2, False, True), Boolean))
        txtConversionFactor.Enabled = (CType(IIf(mExportInvoice.StatusID >= 2, False, True), Boolean))
        If User.IsInRole("ExportInvoiceAuthorized") = False Then
            btnAuthorized.Enabled = False
            btnAuthorized.ToolTip = "You are not authorized user."
            btnCancel.Enabled = False
            btnCancel.ToolTip = "You are not authorized user."
        End If
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mExportInvoice.ExportInvoiceItems.CurrentIndex = Index
        Session("mExportInvoice") = mExportInvoice
    End Sub
    Private Sub DeleteCharge(ByVal index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveCharge, MSGBox.Message_text.RemoveCharge, "", MsgBoxStyle.YesNo, "DeleteCharge")
        mExportInvoice.ExportinvoiceCharges.CurrentIndex = index
        Session("mExportInvoice") = mExportInvoice
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetControlStatus(ByVal StatusId As Int16)
        txtExportInvoiceDate.Enabled = (CType(IIf(mExportInvoice.StatusID = 2 Or mExportInvoice.StatusID = 4, False, True), Boolean) And mExportInvoice.ExportInvoiceItems.Count = 0) Or (mExportInvoice.ExportInvoiceItems.Count = 0)
        btnAddCharge.Enabled = IIf(StatusId > 1, False, True)
        btnAddTerms.Enabled = IIf(StatusId > 1, False, True)
        txtExportInvoiceDate.Enabled = IIf(StatusId > 1, False, True)
        txtBuyerOrderDate.Enabled = IIf(StatusId > 1, False, True)
        btnSave.Visible = IIf(StatusId > 1, False, True)
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
                            mExportInvoice = CType(Session("mExportInvoice"), ExportInvoice)
                            mExportInvoice.ExportInvoiceItems.Remove(mExportInvoice.ExportInvoiceItems.CurrentItem)
                            mExportInvoice.CalculateTotal()
                            If mExportInvoice.IsRoundOff = True Then
                                mExportInvoice.RoundCGrandTotal()
                            End If
                            Session("mExportInvoice") = mExportInvoice
                            ExportInvoicItemDataGrid()
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteCharge" Then
                        Try
                            Session("Sender") = ""
                            mExportInvoice = CType(Session("mExportInvoice"), ExportInvoice)
                            mExportInvoice.ExportinvoiceCharges.Remove(mExportInvoice.ExportinvoiceCharges.CurrentItem)
                            ExportInvoiceChargeDataGrid()
                            mExportInvoice.CalculateTotal()
                            If mExportInvoice.IsRoundOff = True Then
                                SetChargeGrid()
                                mExportInvoice.RoundCGrandTotal()
                            End If
                            Session("mExportInvoice") = mExportInvoice
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session("sender") = ""
                        If mExportInvoice.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not User.IsInRole("ExportInvoiceNew") And Not User.IsInRole("ExportInvoiceEdit")) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            If Save() = True Then
                                Response.Redirect("Index.aspx")
                            End If
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
                        If mExportInvoice.IsValid = True Then
                            mExportInvoice.StatusID = 2
                            DataFieldBind()
                            Save()
                        Else
                            Session.Remove("IsValid")
                            Response.Redirect("wfExportInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "StatusCancel" Then
                        Session("sender") = ""
                        mExportInvoice.StatusID = 4
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
                        Session("mExportInvoice") = mExportInvoice
                        DataFieldBind()
                    End If
            End Select
        End If
    End Sub
    Public Sub TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtValue As TextBox
        Dim mExportInvoiceItem As ExportInvoiceItem
        Dim i As Integer = 0
        For Each mExportInvoiceItem In mExportInvoice.ExportInvoiceItems
            With mExportInvoiceItem
                Try
                    txtValue = CType(Me.dgExportInvoiceItem.Rows(i).FindControl("txtRate"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                    txtValue = CType(Me.dgExportInvoiceItem.Rows(i).FindControl("txtBoxNo"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtValue.ClientID + "').value,event)")
                Catch ex As Exception
                End Try
            End With
            i = i + 1
        Next
        upnlExportInvoiceItem.Update()
    End Sub
    Private Sub addAttributes()
        txtExportInvoiceNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtExportInvoiceNo').value,event)")
        txtConversionFactor.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtConversionFactor').value,event)")
    End Sub
    Private Sub setObject()
        mExportInvoice.ExportInvoiceDate = CDate(txtExportInvoiceDate.Text)
        mExportInvoice.ExportInvoiceText = txtExportInvoiceText.Text
        mExportInvoice.ExportInvoiceNo = Val(txtExportInvoiceNo.Text)

        mExportInvoice.ExporterReference = txtExporterRef.Text.Trim
        mExportInvoice.BuyerID = New Guid(cmbBuyer.SelectedValue)
        mExportInvoice.BuyerAddress = txtBuyerAddress.Text.Trim
        mExportInvoice.BuyerAttention = txtBuyerAttn.Text.Trim
        mExportInvoice.BuyerOrderNo = txtBuyerOrderNo.Text.Trim
        If txtBuyerOrderDate.Text = "" Then
            mExportInvoice.BuyerOrderDate = System.DBNull.Value
        Else
            mExportInvoice.BuyerOrderDate = CDate(txtBuyerOrderDate.Text)
        End If
        mExportInvoice.ConsigneeID = New Guid(cmbVendorList.SelectedValue)
        mExportInvoice.ConsigneeAddress = txtSupAddress.Text.Trim
        mExportInvoice.ConsigneeAttention = txtSupplierAttn.Text.Trim
        mExportInvoice.OtherReference = txtBuyerOtherReferences.Text.Trim
        mExportInvoice.CountryOfOriginOfGoods = txtBuyerCountryOfOrigin.Text.Trim
        mExportInvoice.CountryOfFinalDestination = txtBuyerCountryOfFinal.Text.Trim
        mExportInvoice.PreCarriageBy = txtSupPreCarriage.Text.Trim
        mExportInvoice.PalceOfReceiptPreCarriage = txtSupPlaceofReceipt.Text.Trim
        mExportInvoice.FlightNo = txtFlightNo.Text.Trim
        mExportInvoice.PortOfLoading = txtPortOfLoading.Text.Trim
        mExportInvoice.PortOfDischarge = txtPortOfDischarge.Text.Trim
        mExportInvoice.FinalDestination = txtFinalDestination.Text.Trim
        mExportInvoice.IECCodeNo = txtIECCodeNo.Text.Trim
        mExportInvoice.InvoiceTo = txtInvoiceTo.Text.Trim
        mExportInvoice.Remark = txtRemark.Text.Trim

        'Item
        Dim txtValue As TextBox
        Dim ExportInoviceItem As ExportInvoiceItem
        Dim i As Integer = 0
        For Each ExportInoviceItem In mExportInvoice.ExportInvoiceItems
            With ExportInoviceItem
                txtValue = CType(dgExportInvoiceItem.Rows(i).FindControl("txtRate"), TextBox)
                .CRate = CDec(Val(txtValue.Text.Trim))

                txtValue = CType(dgExportInvoiceItem.Rows(i).FindControl("txtBoxNo"), TextBox)
                .BoxNo = CDec(Val(txtValue.Text.Trim))
            End With
            i = i + 1
        Next
        'End

        'Box
        Dim ExportInoviceBox As ExportInvoiceBox
        i = 0
        For Each ExportInoviceBox In mExportInvoice.ExportInvoiceBoxes
            With ExportInoviceBox
                txtValue = CType(dgExportInvoiceBox.Rows(i).FindControl("txtContainerNo"), TextBox)
                .ContainerNo = (txtValue.Text.Trim)

                txtValue = CType(dgExportInvoiceBox.Rows(i).FindControl("txtDimension"), TextBox)
                .Dimension = (txtValue.Text.Trim)

                txtValue = CType(dgExportInvoiceBox.Rows(i).FindControl("txtNetWeight"), TextBox)
                .NetWeight = (txtValue.Text.Trim)

                txtValue = CType(dgExportInvoiceBox.Rows(i).FindControl("txtGrossWeight"), TextBox)
                .GrossWeight = (txtValue.Text.Trim)

            End With
            i = i + 1
        Next
        mExportInvoice.UserName = User.Identity.Name
        mExportInvoice.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
        mExportInvoice.ConversionFactor = Val(txtConversionFactor.Text)
        mExportInvoice.IsRoundOff = chkIsRoundOff.Checked
        mExportInvoice.CalculateTotal()
    End Sub
    Private Function Save() As Boolean
        'Authentication
        If Not mExportInvoice.ExportInvoiceDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))

            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------
                If DateDiff(DateInterval.Day, CDate(mExportInvoice.ExportInvoiceDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Quotation. <br> Export Invoice Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Return False
                    Exit Function
                End If
            End If
        End If
        'Authentication
        Dim msgCnt As Integer = 0
        Dim InvoiceClone As ExportInvoice
        InvoiceClone = mExportInvoice.Clone
        Try

            If Not mExportInvoice.ExportInvoiceItems.Count = 0 Then
                setObject()
                Dim ExportInvoiceCharge As ExportInvoiceCharge
                For Each ExportInvoiceCharge In mExportInvoice.ExportinvoiceCharges
                    If (ExportInvoiceCharge.Sign <> 1 And ExportInvoiceCharge.CChargeAmount <= 0) Or (Not (ExportInvoiceCharge.IsValid)) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage Invoice Charge(s) are not allowed if Invoice Amount Is Zero. ", MsgBoxStyle.OkOnly, "")
                        mExportInvoice.CancelEdit()
                        Return False
                        Exit Function
                    End If
                Next

                If mExportInvoice.IsRoundOff = True Then
                    mExportInvoice.RoundCGrandTotal()
                End If
                If Not CustomValidate1() = True Then upnlTitle.Update() : Return False : Exit Function
                If ValidateBoxSequence() AndAlso CreateExportInvoiceBoxes() Then
                    'Added by Utkarsh ON 21-Nov-2013 FOr TransTextSeries
                    'Check if Export Invoice is blank then call TransTextSeries UI

                    If (mExportInvoice.IsNew) And (mExportInvoice.ExportInvoiceText = "") Then

                        Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mExportInvoice.ExportTransTypeID, mExportInvoice.ExportInvoiceDateFormatted)

                        If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mExportInvoice.ExportTransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mExportInvoice.ExportTransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mExportInvoice.ExportTransTypeID).TransText = "")) Then

                            Dim str = "<script language='javascript'>openledgersame('wfExportInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

                            Session("BackPagestr_ForTransSeries") = str

                            Session("TransName_ForTransSeries") = "Export Invoice"
                            Session("TransTypeID_ForTransSeries") = mExportInvoice.ExportTransTypeID
                            Session("TransDate_ForTransSeries") = mExportInvoice.ExportInvoiceDateFormatted
                            Session("AddTransTextSeries") = "True"
                            Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                            'Return False
                        Else
                            Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                            If mAutoRenewTransTextSeries.IsRenewed Then
                                With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mExportInvoice.ExportTransTypeID)
                                    mExportInvoice.ExportInvoiceText = .TransText
                                    mExportInvoice.ExportInvoiceNo = .StartingTransNo
                                End With
                            Else
                                Dim str = "<script language='javascript'>openledgersame('wfExportInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

                                Session("BackPagestr_ForTransSeries") = str

                                Session("TransName_ForTransSeries") = "Export Invoice"
                                Session("TransTypeID_ForTransSeries") = mExportInvoice.ExportTransTypeID
                                Session("TransDate_ForTransSeries") = mExportInvoice.ExportInvoiceDateFormatted
                                Session("AddTransTextSeries") = "True"
                                Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                            End If
                        End If

                    End If

                    'End
                    mExportInvoice.Save()
                Else
                    cvCommon.ErrorMessage = "Box No(s) should be in sequence,Ex : 1,2,3,4..."
                    cvCommon.IsValid = False
                    upnlTitle.Update()
                    mExportInvoice = InvoiceClone
                    setObject()
                    Session("mExportInvoice") = mExportInvoice
                    Return False
                End If


                ExpInvDetail = mExportInvoice.ExportInvoiceTextNo + " Dated : " + mExportInvoice.ExportInvoiceDateFormatted + " from " + mVendorList(mExportInvoice.ConsigneeID).Name

                Select Case mExportInvoice.StatusID
                    Case 1
                        MarkLog(Util.Action.Save, mModuleName, ExpInvDetail, Util.ErrorType.NoError, mExportInvoice.ID, EventLogID)
                    Case 2
                        MarkLog(Util.Action.Authorize, mModuleName, ExpInvDetail, Util.ErrorType.NoError, mExportInvoice.ID, EventLogID)
                    Case 3
                        MarkLog(Util.Action.Amend, mModuleName, ExpInvDetail, Util.ErrorType.NoError, mExportInvoice.ID, EventLogID)
                    Case 4
                        MarkLog(Util.Action.Cancel, mModuleName, ExpInvDetail, Util.ErrorType.NoError, mExportInvoice.ID, EventLogID)
                End Select

                'End
                Session("mExportInvoice") = mExportInvoice
                SetPage()
                ControlVisibility()
                SetControlStatus(mExportInvoice.StatusID)
                upnlSupplierDetails.DataBind()
                upnlSupplierDetails.Update()
                upnlExportInvoiceDetails.Update()
                upnlExportInvoiceItem.Update()
                upnlExportInvoiceBox.Update()
                upnlExportInvoiceTerm.Update()
                ExportInvoiceChargeDataGrid()
                SetChargeGrid()
                upnlOtherDetails.DataBind()
                upnlOtherDetails.Update()
                upnlButtons.DataBind()
                upnlButtons.Update()
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Export Invoice can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                Return False
                Exit Function
            End If
        Catch ex As SqlClient.SqlException
            Session("InvoiceClone") = InvoiceClone
            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                Return False
                Exit Function
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Return False
                Exit Function
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Return False
                Exit Function
            ElseIf ex.Number = 547 Then
                If InStr(ex.Message, "FKtabInvoiceChargetabCharge", CompareMethod.Text) Then
                    MSGBoxCtrl.show("Alert!", "Other Charge Deleted! ", "Other Charge Not Avalable<Br><BR>Selected Charge is no longer exist in the Database <BR><BR> Remove Charge and try Again", MsgBoxStyle.OkOnly, "")
                    Return False
                    Exit Function
                ElseIf InStr(ex.Message, "CCtabInvoiceNo", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "No. Required", MsgBoxStyle.OkOnly, "")
                    Exit Function
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Function
                End If
            End If
            mExportInvoice = InvoiceClone
            Session("mExportInvoice") = mExportInvoice
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
        'Dim lbEdit As LinkButton ' 
        'Dim lbRemove As LinkButton ' 
        For j As Integer = 0 To dgExportInvoiceCharge.Rows.Count - 1
            If (Me.dgExportInvoiceCharge.Rows.Item(j).Cells(2).Text = "Round off (Plus)" Or Me.dgExportInvoiceCharge.Rows.Item(j).Cells(2).Text = "Round off (Minus)") Then
                'lbEdit = CType(dgExportInvoiceCharge.Rows.Item(j).Cells(5).FindControl("lnkEdit"), LinkButton)
                'lbEdit.Enabled = False
                'lbRemove = CType(dgExportInvoiceCharge.Rows.Item(j).Cells(6).FindControl("lnkRemove"), LinkButton)
                'lbRemove.Enabled = False
                'dgExportInvoiceCharge.Rows.Item(j).Cells(5).Enabled = False
                'dgExportInvoiceCharge.Rows.Item(j).Cells(6).Enabled = False
                Dim EditView As ImageButton = CType(dgExportInvoiceCharge.Rows(j).FindControl("EditView"), ImageButton)
                Dim DeleteRecord As ImageButton = CType(dgExportInvoiceCharge.Rows(j).FindControl("DeleteRecord"), ImageButton)
                EditView.Enabled = False
                DeleteRecord.Enabled = False

            End If
        Next
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCurrencyList = CurrencyList.GetCurrencyList(, , True)
        mVendorList = VendorList.GetVendortList(0, , , , , , True, True, True, True)

        cmbVendorList.DataSource = mVendorList

        cmbBuyer.DataSource = mVendorList

        cmbCurrencyList.DataSource = mCurrencyList

        Session("mCurrencyList") = mCurrencyList
        Session("mVendorList") = mVendorList
        Session("mStatusList") = mStatusList

        dgExportInvoiceItem.DataSource = mExportInvoice.ExportInvoiceItems
        dgExportInvoiceCharge.DataSource = mExportInvoice.ExportinvoiceCharges
        dgExportInvoiceTerm.DataSource = mExportInvoice.ExportinvoiceTerms
        dgExportInvoiceBox.DataSource = mExportInvoice.ExportInvoiceBoxes

        txtExportInvoiceDate.Text = mExportInvoice.ExportInvoiceDateFormatted.ToString
        txtBuyerOrderDate.Text = mExportInvoice.BuyerOrderDateFormatted.ToString
        DataBind()
    End Sub
    Private Sub ExportInvoicItemDataGrid()
        dgExportInvoiceItem.DataSource = mExportInvoice.ExportInvoiceItems
        dgExportInvoiceItem.DataBind()
        upnlExportInvoiceItem.Update()
        upnlOtherDetails.Update()
    End Sub
    Private Sub ExportInvoiceChargeDataGrid()
        dgExportInvoiceCharge.DataSource = mExportInvoice.ExportinvoiceCharges
        dgExportInvoiceCharge.DataBind()
        upnlExportInvoiceCharge.Update()
        upnlOtherDetails.Update()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtExportInvoiceDate" Then
            If txtExportInvoiceDate.Text = "" Then
                custValidator.ErrorMessage = "Select Export Invoice Date."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbVendorList" Then
            If cmbVendorList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Consignee From the list."
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
    Private Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        If Not mExportInvoice.IsValid Then
            For i As Integer = 0 To mExportInvoice.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mExportInvoice.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If

        Dim mExportInvoiceItem As ExportInvoiceItem
        If Not mExportInvoice.ExportInvoiceItems.IsValid Then
            For Each mExportInvoiceItem In mExportInvoice.ExportInvoiceItems
                For i As Integer = 0 To mExportInvoiceItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mExportInvoiceItem.PartNo + " : " + mExportInvoiceItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If

        Dim mExportInvoicebox As ExportInvoiceBox
        If Not mExportInvoice.ExportInvoiceBoxes.IsValid Then
            For Each mExportInvoicebox In mExportInvoice.ExportInvoiceBoxes
                For i As Integer = 0 To mExportInvoicebox.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + "Box No." + mExportInvoicebox.ExportInvoiceBoxNo + " : " + mExportInvoicebox.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If
        'If Not (ValidateBoxSequence() AndAlso CreateExportInvoiceBoxes()) Then
        '    strMsg = strMsg + "<Br>" + "Box No(s) should be in sequence,Ex : 1,2,3,4..."
        'End If
        If strMsg.Trim <> "" Then
            cvCommon.ErrorMessage = strMsg
            cvCommon.IsValid = False
            Return False
        Else
            Return True
        End If
    End Function
    Private Function ValidateBoxSequence() As Boolean
        Dim NumberSequence As ArrayList = New ArrayList
        'Add each unique box No. in Numbersequence collection
        If mExportInvoice.ExportInvoiceItems.Count > 0 Then
            For i As Integer = 0 To mExportInvoice.ExportInvoiceItems.Count - 1
                If Not NumberSequence.Contains(mExportInvoice.ExportInvoiceItems(i).BoxNo) Then
                    NumberSequence.Add(mExportInvoice.ExportInvoiceItems(i).BoxNo)
                End If
            Next
            'Convert list to array...
            Dim Sequence As Integer() = CType(NumberSequence.ToArray(GetType(Integer)), Integer())

            'Sort the array
            Array.Sort(Sequence)
            'the last number in sorted array must be the max number of Box No.
            If Sequence.Length <> Sequence.GetValue(Sequence.Length - 1) Then
                Return False
            Else
                MaxBoxNo = Sequence.Length
                Return True
            End If
        Else
            Return True
        End If
    End Function
    Private Function CreateExportInvoiceBoxes() As Boolean
        Try
            If mExportInvoice.ExportInvoiceBoxes.Count > 0 Then
                If MaxBoxNo > mExportInvoice.ExportInvoiceBoxes.Count Then
                    For i As Integer = 0 To (MaxBoxNo - mExportInvoice.ExportInvoiceBoxes.Count) - 1
                        mExportInvoice.ExportInvoiceBoxes.Add(mExportInvoice.ID)
                        mExportInvoice.ExportInvoiceBoxes.CurrentItem.ExportInvoiceBoxNo = mExportInvoice.ExportInvoiceBoxes.Count + i
                    Next
                ElseIf MaxBoxNo < mExportInvoice.ExportInvoiceBoxes.Count Then
                    For i As Integer = 0 To (mExportInvoice.ExportInvoiceBoxes.Count - MaxBoxNo) - 1
                        mExportInvoice.ExportInvoiceBoxes.Remove(mExportInvoice.ExportInvoiceBoxes.Count - i)
                    Next
                End If
            Else
                For i As Integer = 1 To MaxBoxNo
                    mExportInvoice.ExportInvoiceBoxes.Add(mExportInvoice.ID)
                    mExportInvoice.ExportInvoiceBoxes.CurrentItem.ExportInvoiceBoxNo = i
                Next
            End If
            mExportInvoice.TotalBox = mExportInvoice.ExportInvoiceBoxes.Count
            dgExportInvoiceBox.DataSource = mExportInvoice.ExportInvoiceBoxes
            dgExportInvoiceBox.DataBind()
            Session("mExportInvoice") = mExportInvoice
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            'Added by Utkarsh on 22-Nov-2013 for Trans Text Series
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If mExportInvoice.IsNew Then
                    mExportInvoice.ExportInvoiceText = Session("TransText_ForTransSeries")
                    txtExportInvoiceText.Text = mExportInvoice.ExportInvoiceText
                    Session("mExportInvoice") = mExportInvoice
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If
            End If
            'End
            DataFieldBind()
        End If
        SetPage()
        ControlVisibility()
        SetControlStatus(mExportInvoice.StatusID)
        TextChanged(sender, e)
        If chkIsRoundOff.Checked = True Then
            SetChargeGrid()
        End If
    End Sub
    'Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
    '    If IsValid Then
    '        setObject()
    '        Session("mExportInvoice") = mExportInvoice
    '        Response.Redirect("wfExportInvoicePendingIssueList.aspx?BackPage=wfExportInvoice.aspx")
    '    End If
    'End Sub
    Private Sub btnAddTerms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTerms.Click
        setObject()
        Session("mExportInvoice") = mExportInvoice
        Response.Redirect("wfExportInvoiceTerm_Ajax.aspx?BackPage=wfExportInvoice_Ajax.aspx&Type=10")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, mModuleName, "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        setObject()
        If mExportInvoice.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
        Else
            RemoveSession()
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("ExportInvoiceNew") And Not User.IsInRole("ExportInvoiceEdit")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If mVendorList(New Guid(cmbVendorList.SelectedValue)).NotInUse = True Then
            If CDate(mVendorList(New Guid(cmbVendorList.SelectedValue)).NotInUseDate) <= CDate(txtExportInvoiceDate.Text) Then
                'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Supplier is not applicable since " + mVendorList(mOrder.VendorID).NotInUseDateFormatted + " <br><br> Select another Supplier from list or select date before " + mVendorList(mOrder.VendorID).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
                'Exit Function
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Consignee is not applicable since " + mVendorList(New Guid(cmbVendorList.SelectedValue)).NotInUseDateFormatted + "\n" + "Select another Consignee from list or select date before " + mVendorList(New Guid(cmbVendorList.SelectedValue)).NotInUseDateFormatted + " & try again", False), True)
                Exit Sub
            End If
        End If

        If mVendorList(New Guid(cmbBuyer.SelectedValue)).NotInUse = True Then
            If CDate(mVendorList(New Guid(cmbBuyer.SelectedValue)).NotInUseDate) <= CDate(txtExportInvoiceDate.Text) Then
                'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Supplier is not applicable since " + mVendorList(mOrder.VendorID).NotInUseDateFormatted + " <br><br> Select another Supplier from list or select date before " + mVendorList(mOrder.VendorID).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
                'Exit Function
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Buyer is not applicable since " + mVendorList(New Guid(cmbBuyer.SelectedValue)).NotInUseDateFormatted + "\n" + "Select another Buyer from list or select date before " + mVendorList(New Guid(cmbBuyer.SelectedValue)).NotInUseDateFormatted + " & try again", False), True)
                Exit Sub
            End If
        End If
        If IsValid Then
            Save()
        Else
            upnlTitle.Update()
        End If
    End Sub
    Private Sub chkIsRoundOff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsRoundOff.CheckedChanged
        Dim Child As ExportInvoiceCharge
        For i As Integer = mExportInvoice.ExportinvoiceCharges.Count - 1 To 0 Step -1
            Child = mExportInvoice.ExportinvoiceCharges(i)
            If Child.ChargeID.Equals(New Guid("{40000000-0000-0000-0000-000000000000}")) Or Child.ChargeID.Equals(New Guid("{50000000-0000-0000-0000-000000000000}")) Then
                mExportInvoice.ExportinvoiceCharges.Remove(Child)
            End If
        Next
        dgExportInvoiceCharge.DataSource = mExportInvoice.ExportinvoiceCharges
        dgExportInvoiceCharge.DataBind()
        upnlExportInvoiceCharge.Update()
    End Sub
    Private Sub btnAuthorized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
        If IsValid Then
            setObject()
            If CustomValidate1() Then
                If ValidateBoxSequence() Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.StatusAuthorized, SIMsgBox.Message_text.StatusAuthorized, "<Strong> Export Invoice </Strong>", MsgBoxStyle.YesNo)
                    'msg1.ReplacePage = "wfExportInvoice.aspx?BackPage=" & Request.QueryString("BackPage")
                    'Session("sender") = "Status"
                    'Session("IsValid") = IsValid
                    'msg1.Show()
                    'mExportInvoice.StatusID = 2
                    'Session("mExportInvoice") = mExportInvoice
                    MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<Strong> Export Invoice </Strong>", MsgBoxStyle.YesNo, "Status")
                    Exit Sub
                Else
                    cvCommon.ErrorMessage = "Box No(s) should be in sequence,Ex : 1,2,3,4..."
                    cvCommon.IsValid = False
                End If
            End If
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If IsValid Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.StatusCanceled, SIMsgBox.Message_text.StatusCanceled, "<Strong> Export Invoice </Strong>", MsgBoxStyle.YesNo)
            'msg1.ReplacePage = "wfExportInvoice.aspx?BackPage=" & Request.QueryString("BackPage")
            'Session("sender") = "Status"
            'Session("IsValid") = IsValid
            'msg1.Show()
            'mExportInvoice.StatusID = 4
            'Session("mExportInvoice") = mExportInvoice
            MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<Strong> Export Invoice </Strong>", MsgBoxStyle.YesNo, "StatusCancel")
            Session("mExportInvoice") = mExportInvoice
        End If
    End Sub
    Private Sub dgExportInvoiceItem_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgExportInvoiceItem.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgExportInvoiceItem.PageIndex * dgExportInvoiceItem.PageSize
                Session("Edit") = True
                setObject()
                mExportInvoice.ExportInvoiceItems.CurrentIndex = Index
                Session("mExportInvoice") = mExportInvoice
                Response.Redirect("wfExportInvoiceItem_Ajax.aspx?BackPage=wfExportInvoice_Ajax.aspx")
            Case "DeleteRecord"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgExportInvoiceItem.PageIndex * dgExportInvoiceItem.PageSize
                DeleteRecord(Index)
        End Select
    End Sub
    Private Sub dgExportInvoiceCharge_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgExportInvoiceCharge.RowCommand
        Select Case e.CommandName
            Case "EditCharge"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgExportInvoiceCharge.PageIndex * dgExportInvoiceCharge.PageSize
                Session("Edit") = True
                setObject()
                mExportInvoice.ExportinvoiceCharges.CurrentIndex = Index
                Session("mExportInvoice") = mExportInvoice
                Response.Redirect("wfExportInvoiceCharge_Ajax.aspx?BackPage=wfExportInvoice_Ajax.aspx")
            Case "DeleteCharge"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgExportInvoiceCharge.PageIndex * dgExportInvoiceCharge.PageSize
                DeleteCharge(Index)
        End Select
    End Sub
    Private Sub dgExportInvoiceTerm_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgExportInvoiceTerm.RowCommand
        Select Case e.CommandName
            Case "DeleteTerm"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgExportInvoiceTerm.PageIndex * dgExportInvoiceTerm.PageSize
                mExportInvoice.ExportinvoiceTerms.CurrentIndex = Index
                mExportInvoice.ExportinvoiceTerms.Remove(mExportInvoice.ExportinvoiceTerms.CurrentItem)
                Session("mExportInvoice") = mExportInvoice
                dgExportInvoiceTerm.DataSource = mExportInvoice.ExportinvoiceTerms
                dgExportInvoiceTerm.DataBind()
                upnlExportInvoiceTerm.Update()
        End Select
    End Sub
    Private Sub cmbCurrencyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
        txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
        If cmbCurrencyList.Enabled = True Then
            setFocus(cmbCurrencyList)
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not User.IsInRole("ExportInvoicePrint") Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

        Dim rpt As rptExportInvoice
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsExportInvoice
        Dim mCompanyDetail As New CompanyDetail

        If Session("PackingList") = "PackingList" Then
            myReport = New crptPackingList
            Session.Remove("PackingList")
        ElseIf Session("ProformaInvoice") = "ProformaInvoice" Then
            myReport = New crptProformaInvoice
            Session.Remove("ProformaInvoice")
        Else
            If AppSettings("ClientCode") = "UHPL" Then
                myReport = New crptExportInvoiceDetailUHPL
            Else
                myReport = New crptExportInvoiceDetail
            End If
        End If

        rpt = rptExportInvoice.GetrptExportInvoice(mExportInvoice.ID)
        Dim mUser As User
        mUser = SI.UTILITY.User.GetUser(User.Identity.Name)

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "", SearchStr1:=mUser.EmployeeName, SearchStr2:=mUser.Mobile, _
               SearchStr3:=AppSettings("HSNACSCodeVisibleInPartMaster"), SearchStr4:="", SearchStr5:="", _
               ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:="", SearchStr7:="", SearchStr8:="", _
               SearchStr9:=AppSettings("ClientCode"), SearchStr10:=AppSettings("Logo"))

        ds.Clear()

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)

        da.Fill(ds, rpt)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)

        Session("CrystalReport") = myReport
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
    Private Sub cmbVendorList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbVendorList.SelectedIndexChanged
        If cmbVendorList.SelectedIndex = 0 Then
            txtSupAddress.Text = ""
        Else
            txtSupAddress.Text = mVendorList(cmbVendorList.SelectedIndex).Address
        End If
    End Sub
    Private Sub cmbBuyer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBuyer.SelectedIndexChanged
        If cmbBuyer.SelectedIndex = 0 Then
            txtBuyerAddress.Text = ""
        Else
            txtBuyerAddress.Text = mVendorList(cmbBuyer.SelectedIndex).Address
        End If
    End Sub
    Private Sub btnAddCharge_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCharge.Click
        setObject()
        mExportInvoice.ExportinvoiceCharges.Add(mExportInvoice.ID)
        Session("mExportInvoice") = mExportInvoice
        Response.Redirect("wfExportInvoiceCharge_Ajax.aspx?BackPage=wfExportInvoice_Ajax.aspx")
    End Sub
    Private Sub btnExportInvoiceBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportInvoiceBox.Click
        setObject()
        If Not (ValidateBoxSequence() AndAlso CreateExportInvoiceBoxes()) Then
            cvCommon.ErrorMessage = "Box No(s) should be in sequence,Ex : 1,2,3,4..."
            cvCommon.IsValid = False
            upnlTitle.Update()
            Exit Sub
        End If
    End Sub
    Private Sub btnPackingList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPackingList.Click
        Session("PackingList") = "PackingList"
        btnPrint_Click(sender, e)
    End Sub
    Private Sub btnProformaInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProformaInvoice.Click
        Session("ProformaInvoice") = "ProformaInvoice"
        btnPrint_Click(sender, e)
    End Sub
    Private Sub txtExportInvoiceDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExportInvoiceDate.TextChanged  'Added by Utkarsh on 14-Nov-2013 for Trans Text Series
        mExportInvoice = Session("mExportInvoice")
        mExportInvoice.ExportInvoiceDate = txtExportInvoiceDate.Text
        txtExportInvoiceText.Text = mExportInvoice.ExportInvoiceText
        txtExportInvoiceText.DataBind()
        Session("mExportInvoice") = mExportInvoice
        upnlSupplierDetails.Update()
    End Sub 'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
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