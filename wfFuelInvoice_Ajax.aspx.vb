Public Class wfFuelInvoice_Ajax
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
    Public mFuelInvoiceLog As FuelInvoiceLog
    Public mFuelInvoice As FuelInvoice
    Public mVendorList As VendorList
    Public mStatusList As StatusList
    Public mCurrencyList As CurrencyList
    Public mUnitList As UnitListMain
    Dim mFileAttach As FileAttach
    Public Flag As Integer
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mFuelInvoice = Session("mFuelInvoice")
        mVendorList = Session("mVendorList")
        mStatusList = Session("mStatusList")
        mCurrencyList = Session("mCurrencyList")
        mFileAttach = Session("mFileAttach")
    End Sub
    Private Sub setSession()
        Session("mFuelInvoice") = mFuelInvoice
        Session("mVendorList") = mVendorList
        Session("mStatusList") = mStatusList
        Session("mCurrencyList") = mCurrencyList
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mFuelInvoice.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = IIf(mFuelInvoice.StatusID > 1, False, True)
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mFuelInvoice.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mFuelInvoice.ID)
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        If mFuelInvoice.IsAttachmentAdded = True Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFuelInvoice.FileAttachments(0).Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFuelInvoice.FileAttachments(0).Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFuelInvoice.FileAttachments(0).ImageFile, 0, mFuelInvoice.FileAttachments(0).ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            End If
        End If
    End Sub
    Private Sub setObject()
        mFuelInvoice.Date = CDate(calFuelInvoiceDate.Text)
        mFuelInvoice.Text = txtText.Text
        mFuelInvoice.No = Val(txtNo.Text)
        mFuelInvoice.InvoiceUnitID = Val(cmbUnit.SelectedValue.ToString)
        mFuelInvoice.UserName = User.Identity.Name
        mFuelInvoice.VendorInvoiceNo = txtVendorInvoiceNo.Text
        If txtVendorInvoiceDate.Text = "" Then
            mFuelInvoice.VendorInvoiceDate = System.DBNull.Value
        Else
            mFuelInvoice.VendorInvoiceDate = CDate(txtVendorInvoiceDate.Text)
        End If
        mFuelInvoice.Remark = txtRemark.Text
        mFuelInvoice.VendorID = New Guid(cmbVendorList.SelectedValue)
        mFuelInvoice.CurrencyID = New Guid(cmbCurrencyList.SelectedValue)
        mFuelInvoice.ConversionFactor = Val(txtConversionFactor.Text)

        Dim txtValue As TextBox
        Dim mFuelInvoiceLog As FuelInvoiceLog
        Dim i As Integer = 0
        For Each mFuelInvoiceLog In mFuelInvoice.FuelInvoiceLogs
            With mFuelInvoiceLog
                txtValue = CType(Me.dgFuelInvoiceLogs.Rows(i).FindControl("txtUpliftedFuelInvUnit"), TextBox)
                .UpliftedFuelInvUnit = CDec(Val(txtValue.Text))

                txtValue = CType(Me.dgFuelInvoiceLogs.Rows(i).FindControl("txtRate"), TextBox)
                .CRate = CDec(Val(txtValue.Text))

                txtValue = CType(Me.dgFuelInvoiceLogs.Rows(i).FindControl("txtRemark"), TextBox)
                .Remark = txtValue.Text.Trim
            End With
            i = i + 1
        Next
        mFuelInvoice.CalculateTotal()
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mFuelInvoice.FuelInvoiceLogs.CurrentIndex = Index
        Session("mFuelInvoice") = mFuelInvoice
    End Sub
    Private Sub DeleteChargeRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveCharge, MSGBox.Message_text.RemoveCharge, "", MsgBoxStyle.YesNo, "DeleteCharge")
        mFuelInvoice.FuelInvoiceCharges.CurrentIndex = Index
        Session("mFuelInvoice") = mFuelInvoice
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
                            Session("Sender") = ""
                            mFuelInvoice = CType(Session("mFuelInvoice"), FuelInvoice)
                            mFuelInvoice.FuelInvoiceLogs.Remove(mFuelInvoice.FuelInvoiceLogs.CurrentItem)
                            mFuelInvoice.CalculateTotal()
                            Session("mFuelInvoice") = mFuelInvoice
                            FuelInvoiceItemDataGrid()
                            ControlVisibility()
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteCharge" Then
                        Try
                            Session("Sender") = ""
                            mFuelInvoice = CType(Session("mFuelInvoice"), FuelInvoice)
                            mFuelInvoice.FuelInvoiceCharges.Remove(mFuelInvoice.FuelInvoiceCharges.CurrentItem)
                            mFuelInvoice.CalculateTotal()
                            Session("mFuelInvoice") = mFuelInvoice
                            FuelInvoiceChargesGrid()
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session("sender") = ""
                        If mFuelInvoice.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
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
                        If mFuelInvoice.IsValid = True Then
                            mFuelInvoice.StatusID = 2
                            DataFieldBind()
                            Save()
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "StatusCancel" Then
                        Session("sender") = ""
                        mFuelInvoice.StatusID = 4
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
                        Session("mFuelInvoice") = mFuelInvoice
                        DataFieldBind()
                        UpdatePanel()
                        upnlFuelInvoiceLogs.Update()
                        upnlFuelInvoiceCharges.Update()
                    End If
            End Select
        End If
    End Sub
    Private Sub addAttributes()
        txtConversionFactor.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtConversionFactor').value,event)")
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Public Sub TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtValue As TextBox
        Dim mFuelInvoiceLog As FuelInvoiceLog
        Dim i As Integer = 0
        For Each mFuelInvoiceLog In mFuelInvoice.FuelInvoiceLogs
            With mFuelInvoiceLog
                Try
                    txtValue = CType(Me.dgFuelInvoiceLogs.Rows(i).FindControl("txtUpliftedFuelInvUnit"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                    txtValue = CType(Me.dgFuelInvoiceLogs.Rows(i).FindControl("txtRate"), TextBox)
                    txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")
                Catch ex As Exception
                End Try
            End With
            i = i + 1
        Next
        upnlFuelInvoiceLogs.Update()
    End Sub
    Private Sub SetControlStatus(ByVal StatusId As Int16)
        btnAdd.Enabled = IIf(StatusId > 1, False, True)
        btnAddCharge.Enabled = IIf(StatusId > 1, False, True)
        btnSave.Visible = IIf(StatusId > 1, False, True)
        dgFuelInvoiceLogs.Columns(14).Visible = IIf(StatusId > 1, False, True)
        dgFuelInvoiceLogs.Columns(16).Visible = IIf(StatusId > 1, False, True)
        dgFuelInvoiceLogs.Columns(17).Visible = IIf(StatusId > 1, False, True)
        dgChargeList.Columns(4).Visible = IIf(StatusId > 1, False, True)
        dgChargeList.Columns(5).Visible = IIf(StatusId > 1, False, True)
    End Sub
    Private Sub SetPage()
        lblTitle.Text = "Fuel Invoice [" & mFuelInvoice.Text + "-" + CType(mFuelInvoice.No, String) + "]"
        upnlTitle.Update()
    End Sub
    Private Sub ControlVisibilityForGrid()
        Dim txtValue As TextBox
        Dim btnImageButton As ImageButton
        For i As Integer = 0 To dgFuelInvoiceLogs.Rows.Count - 1
            txtValue = CType(Me.dgFuelInvoiceLogs.Rows(i).FindControl("txtUpliftedFuelInvUnit"), TextBox)

            txtValue = CType(Me.dgFuelInvoiceLogs.Rows(i).FindControl("txtRate"), TextBox)
            txtValue = CType(Me.dgFuelInvoiceLogs.Rows(i).FindControl("txtRemark"), TextBox)
            txtValue.Enabled = CType(IIf(mFuelInvoice.StatusID >= 2, False, True), Boolean)
            btnImageButton = CType(Me.dgFuelInvoiceLogs.Rows(i).FindControl("btnCopyInTextBox"), ImageButton)
            If i = 0 Then
                btnImageButton.Visible = True
            Else
                btnImageButton.Visible = False
            End If
            btnImageButton.Enabled = CType(IIf(mFuelInvoice.StatusID >= 2, False, True), Boolean)
        Next
    End Sub
    Private Sub ControlVisibility()
        txtText.Enabled = IIf(mFuelInvoice.StatusID >= 2, False, True)
        txtNo.Enabled = IIf(mFuelInvoice.StatusID >= 2, False, True)
        cmbVendorList.Enabled = (CType(IIf(mFuelInvoice.StatusID >= 2, False, True), Boolean) And mFuelInvoice.FuelInvoiceLogs.Count = 0) Or (mFuelInvoice.FuelInvoiceLogs.Count = 0)
        cmbCurrencyList.Enabled = (CType(IIf(mFuelInvoice.StatusID >= 2, False, True), Boolean))
        txtConversionFactor.Enabled = (CType(IIf(mFuelInvoice.StatusID >= 2, False, True), Boolean))
        calFuelInvoiceDate.Enabled = (CType(IIf(mFuelInvoice.StatusID >= 2, False, True), Boolean) And mFuelInvoice.FuelInvoiceLogs.Count = 0) Or (mFuelInvoice.FuelInvoiceLogs.Count = 0)
        btnAuthorized.Visible = (Not mFuelInvoice.IsNew) And (mFuelInvoice.StatusID = 1)
        btnCancel.Visible = (Not mFuelInvoice.IsNew) And (mFuelInvoice.StatusID = 2)
        cmbCurrencyList.Enabled = (CType(IIf(mFuelInvoice.StatusID >= 2, False, True), Boolean))
        btnSelectFile.Disabled = IIf(mFuelInvoice.StatusID > 1, True, False)
        ControlVisibilityForGrid()
        txtVendorInvoiceDate.Enabled = (CType(IIf(mFuelInvoice.StatusID >= 2, False, True), Boolean))
        'If Not IsInRole(Rights.Authorized) Then
        '    btnAuthorized.Enabled = False
        '    btnAuthorized.ToolTip = "You are not authorized user "
        '    btnCancel.Enabled = False
        '    btnCancel.ToolTip = "You are not authorized user "
        'End If
        If mFuelInvoice.FuelInvoiceLogs.Count = 0 Then
            cmbUnit.Enabled = True
            cmbVendorList.Enabled = True
        Else
            cmbUnit.Enabled = False
            cmbVendorList.Enabled = False
        End If
        upnlFuelInvoiceDetails.Update()
        upnlSupplierDetails.Update()
        ControlVisibilityForAttachment()
    End Sub
    Private Sub Save()
        'Authentication
        If Not mFuelInvoice.Date Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------
                If DateDiff(DateInterval.Day, CDate(mFuelInvoice.Date), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save FuelInvoice. <br> FuelInvoice Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        End If
        'Authentication
        Dim FuelInvoiceClone As FuelInvoice
        FuelInvoiceClone = mFuelInvoice.Clone
        Try
            If Not mFuelInvoice.FuelInvoiceLogs.Count = 0 Then
                setObject()

                If mVendorList(mFuelInvoice.VendorID).NotInUse = True Then
                    If CDate(mVendorList(mFuelInvoice.VendorID).NotInUseDate) <= CDate(mFuelInvoice.Date) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Supplier is not applicable since " + mVendorList(mFuelInvoice.VendorID).NotInUseDateFormatted + " <br><br> Select another Supplier from list or select date before " + mVendorList(mFuelInvoice.VendorID).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                End If
                Session("mFuelInvoice") = mFuelInvoice

                Dim mFuelInvoiceCharge As FuelInvoiceCharge
                For Each mFuelInvoiceCharge In mFuelInvoice.FuelInvoiceCharges
                    If (mFuelInvoiceCharge.Sign <> 1 And mFuelInvoiceCharge.CChargeAmount <= 0) Or (Not (mFuelInvoiceCharge.IsValid)) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Percentage FuelInvoice Charge(s) are not allowed if FuelInvoice Amount Is Zero. ", MsgBoxStyle.OkOnly, "")
                        mFuelInvoice.CancelEdit()
                        Exit Sub
                    End If
                Next

                If (mFuelInvoice.IsNew) And (mFuelInvoice.Text = "") Then

                    Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mFuelInvoice.TransTypeID, mFuelInvoice.DateFormatted)

                    If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mFuelInvoice.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mFuelInvoice.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mFuelInvoice.TransTypeID).TransText = "")) Then

                        Dim str = "<script language='javascript'>openledgersame('wfFuelInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

                        Session("BackPagestr_ForTransSeries") = str

                        Session("TransName_ForTransSeries") = "FuelInvoice"
                        Session("TransTypeID_ForTransSeries") = mFuelInvoice.TransTypeID
                        Session("TransDate_ForTransSeries") = mFuelInvoice.DateFormatted
                        Session("AddTransTextSeries") = "True"
                     
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    Else
                        Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                        If mAutoRenewTransTextSeries.IsRenewed Then
                            With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mFuelInvoice.TransTypeID)
                                mFuelInvoice.Text = .TransText
                                mFuelInvoice.No = .StartingTransNo
                            End With
                        Else
                            Dim str = "<script language='javascript'>openledgersame('wfFuelInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"

                            Session("BackPagestr_ForTransSeries") = str

                            Session("TransName_ForTransSeries") = "FuelInvoice"
                            Session("TransTypeID_ForTransSeries") = mFuelInvoice.TransTypeID
                            Session("TransDate_ForTransSeries") = mFuelInvoice.DateFormatted
                            Session("AddTransTextSeries") = "True"
                            Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                        End If
                    End If
                End If

                mFuelInvoice.Save()
                Session.Remove("mFileAttach")
                Dim FuelInvoiceDetail As String = mFuelInvoice.FuelInvoiceNo + " Dated : " + mFuelInvoice.DateFormatted + " from " + mVendorList(mFuelInvoice.VendorID).Name

                If mFuelInvoice.StatusID = 2 Then
                    MarkLog(Util.Action.Authorize, "FuelInvoice", FuelInvoiceDetail, Util.ErrorType.NoError, mFuelInvoice.ID, EventLogID)
                ElseIf mFuelInvoice.StatusID = 3 Then
                    MarkLog(Util.Action.Amend, "FuelInvoice", FuelInvoiceDetail, Util.ErrorType.NoError, mFuelInvoice.ID, EventLogID)
                ElseIf mFuelInvoice.StatusID = 4 Then
                    MarkLog(Util.Action.Cancel, "FuelInvoice", FuelInvoiceDetail, Util.ErrorType.NoError, mFuelInvoice.ID, EventLogID)
                Else
                    MarkLog(Util.Action.Save, "FuelInvoice", FuelInvoiceDetail, Util.ErrorType.NoError, mFuelInvoice.ID, EventLogID)
                End If

                mFuelInvoice.MarkClean()
                Session("mFuelInvoice") = mFuelInvoice
                SetPage()
                UpdatePanel()
                FuelInvoiceItemDataGrid()
                FuelInvoiceChargesGrid()
                SetChargeGrid()
                ControlVisibilityForGrid()
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "FuelInvoice can not be saved without Log.", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Catch ex As SqlClient.SqlException
            Session("FuelInvoiceClone") = FuelInvoiceClone
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Finally
            FuelInvoiceClone = Nothing
        End Try
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        IsInRoleString = "FuelInvoice"
        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.FindNow
                Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
    Private Sub SetChargeGrid()
        For j As Integer = 0 To dgChargeList.Rows.Count - 1
            If (Me.dgChargeList.Rows.Item(j).Cells(1).Text = "Round off (Plus)" Or Me.dgChargeList.Rows.Item(j).Cells(1).Text = "Round off (Minus)") Then
                dgChargeList.Rows.Item(j).Cells(4).Enabled = False
                dgChargeList.Rows.Item(j).Cells(5).Enabled = False
            End If
        Next
    End Sub
    Private Sub UpdatePanel()
        ControlsDataBind()
        upnlStatusName.Update()
        upnlFuelInvoiceDetails.Update()
        upnlSupplierDetails.Update()
        upnlOtherDetails.Update()
        upnlButtons.Update()
        SetControlStatus(mFuelInvoice.StatusID)
        ControlVisibility()
    End Sub
    Private Sub AttachMyFile()
        If mFuelInvoice.IsAttachmentAdded Then
            mFuelInvoice.FileAttachments(0).Size = mFileAttach.Size
            mFuelInvoice.FileAttachments(0).ImageFile = mFileAttach.ImageFile
            mFuelInvoice.FileAttachments(0).Extension = mFileAttach.Extension
        Else
            mFuelInvoice.IsAttachmentAdded = True
            mFuelInvoice.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
        End If
        ControlVisibilityForAttachment()
        upnlAttachFile.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mVendorList = VendorList.GetVendortList(0, , , , , , True, IsCustomer:=False, IsSupplier:=True)
        Session("mVendorList") = mVendorList
        cmbVendorList.DataSource = mVendorList

        mCurrencyList = CurrencyList.GetCurrencyList(, , True)
        Session("mCurrencyList") = mCurrencyList
        cmbCurrencyList.DataSource = mCurrencyList

        mStatusList = StatusList.GetStatusList(mFuelInvoice.StatusID, 1, True)
        Session("mStatusList") = mStatusList

        dgFuelInvoiceLogs.DataSource = mFuelInvoice.FuelInvoiceLogs
        dgChargeList.DataSource = mFuelInvoice.FuelInvoiceCharges

        calFuelInvoiceDate.Text = mFuelInvoice.DateFormatted

        If txtVendorInvoiceDate.Text = "" Then
            txtVendorInvoiceDate.Text = ""
        Else
            txtVendorInvoiceDate.Text = mFuelInvoice.VendorInvoiceDateFormatted
        End If

        mUnitList = UnitListMain.GetUnitList("", "(SELECT)")
        cmbUnit.DataSource = mUnitList
        DataBind()
    End Sub
    Private Sub ControlsDataBind()
        dgFuelInvoiceLogs.DataBind()
        dgChargeList.DataBind()
        upnlStatusName.DataBind()
        upnlFuelInvoiceDetails.DataBind()
        upnlSupplierDetails.DataBind()
        upnlOtherDetails.DataBind()
        upnlButtons.DataBind()
    End Sub
    Private Sub FuelInvoiceItemDataGrid()
        dgFuelInvoiceLogs.DataSource = mFuelInvoice.FuelInvoiceLogs
        dgFuelInvoiceLogs.DataBind()
        upnlFuelInvoiceLogs.Update()
        upnlOtherDetails.Update()
        upnlOtherDetails.DataBind()
    End Sub
    Private Sub FuelInvoiceChargesGrid()
        dgChargeList.DataSource = mFuelInvoice.FuelInvoiceCharges
        dgChargeList.DataBind()
        upnlFuelInvoiceCharges.Update()
        upnlOtherDetails.Update()
        upnlOtherDetails.DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtFuelInvoiceDate" Then
            If calFuelInvoiceDate.Text = "" Then
                custValidator.ErrorMessage = "Select FuelInvoice Date."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbVendorList" Then
            If cmbVendorList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Vendor from the list."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbCurrencyList" Then
            If cmbCurrencyList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Currency from the List."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbUnit" Then
            If cmbUnit.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select Unit."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtConversionFactor" Then
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
        SetControlStatus(mFuelInvoice.StatusID)
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And Session("sender") = "" Then
            If AppSettings("AutoCompleteTransText") = "False" Then
                If txtText.Enabled = True Then
                    setFocus(txtText)
                End If
            End If
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If mFuelInvoice.IsNew Then
                    mFuelInvoice.Text = Session("TransText_ForTransSeries")
                    txtText.Text = mFuelInvoice.Text
                    Session("mFuelInvoice") = mFuelInvoice
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If
            End If 'End
            DataFieldBind()
        End If
        SetPage()
        ControlVisibility()
        TextChanged(sender, e)
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If IsValid = False Then upnlValidationsummary.Update() : Exit Sub
        setObject()
        setSession()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow();", True)
    End Sub
    Private Sub btnAddCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCharge.Click
        If IsValid Then
            setObject()

            mFuelInvoice.FuelInvoiceCharges.Add(mFuelInvoice.ID)
            Session("mFuelInvoice") = mFuelInvoice
            Response.Redirect("wfFuelInvoiceCharge_Ajax.aspx?BackPage=wfFuelInvoice_Ajax.aspx")
        End If
    End Sub
    Private Sub dgFuelInvoiceLogs_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgFuelInvoiceLogs.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim Index As Integer = CInt(e.CommandArgument) + dgFuelInvoiceLogs.PageIndex * dgFuelInvoiceLogs.PageSize
                Session("Edit") = True
                setObject()

                mFuelInvoice.FuelInvoiceLogs.CurrentIndex = Index
                Session("mFuelInvoice") = mFuelInvoice
                Response.Redirect("wfFuelInvoiceItem_Ajax.aspx?BackPage=wfFuelInvoice_Ajax.aspx")
            Case "DeleteRecord"
                Dim Index As Integer = CInt(e.CommandArgument) + dgFuelInvoiceLogs.PageIndex * dgFuelInvoiceLogs.PageSize
                DeleteRecord(Index)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim index As Int32 = CInt(e.CommandArgument) + dgFuelInvoiceLogs.PageIndex * dgFuelInvoiceLogs.PageSize
                mFuelInvoiceLog = mFuelInvoice.FuelInvoiceLogs(index)
                If mFuelInvoiceLog.IsAttachmentAdded Then
                    mFileAttach = FileAttach.GetAttachmentChild(mFuelInvoiceLog.ID)
                    If mFileAttach.Size > 0 Then
                        Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
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
                            Dim Str1 As String
                            Str1 = "openFile();"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str1, True)
                        End If
                    End If
                Else
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            Case "Attach"
                Dim index As Int32 = CInt(e.CommandArgument) + dgFuelInvoiceLogs.PageIndex * dgFuelInvoiceLogs.PageSize
                Session("index") = index
                If mFuelInvoice.FuelInvoiceLogs(index).IsAttachmentAdded = True Then
                    mFileAttach = FileAttach.GetAttachmentChild(mFuelInvoice.FuelInvoiceLogs(index).ID)
                Else
                    mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mFuelInvoice.FuelInvoiceLogs(index).ID)
                End If
                Session("mFileAttach") = mFileAttach
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
            Case "Remove"
                Dim index As Int32 = CInt(e.CommandArgument) + dgFuelInvoiceLogs.PageIndex * dgFuelInvoiceLogs.PageSize
                mFuelInvoice.FuelInvoiceLogs(index).IsAttachmentAdded = False
                mFuelInvoice.FuelInvoiceLogs(index).FileAttachments.RemoveAt(0)
                FuelInvoiceItemDataGrid()
                Session("mFuelInvoice") = mFuelInvoice
        End Select
    End Sub
    Private Sub dgChargeList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgChargeList.RowCommand
        Select Case e.CommandName
            Case "EditCharge"
                Dim Index As Integer = CInt(e.CommandArgument) + dgChargeList.PageIndex * dgChargeList.PageSize
                Session("EditCharge") = True
                setObject()

                mFuelInvoice.FuelInvoiceCharges.CurrentIndex = Index
                Session("mFuelInvoice") = mFuelInvoice
                Response.Redirect("wfFuelInvoiceCharge_Ajax.aspx?BackPage=wfFuelInvoice_Ajax.aspx")
            Case "DeleteCharge"
                Dim Index As Integer = CInt(e.CommandArgument) + dgChargeList.PageIndex * dgChargeList.PageSize
                DeleteChargeRecord(Index)
        End Select
    End Sub
    Private Sub cmbCurrencyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCurrencyList.SelectedIndexChanged
        txtConversionFactor.Text = mCurrencyList(cmbCurrencyList.SelectedIndex).ConversionFactor
        If cmbCurrencyList.Enabled = True Then
            setFocus(cmbCurrencyList)
        End If
        upnlValidationsummary.Update()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If IsValid Then
            Save()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim FuelInvoiceDetail As String
        If cmbVendorList.SelectedIndex = 0 Then
            FuelInvoiceDetail = mFuelInvoice.FuelInvoiceNo + " Dated : " + mFuelInvoice.DateFormatted
        Else
            FuelInvoiceDetail = mFuelInvoice.FuelInvoiceNo + " Dated : " + mFuelInvoice.DateFormatted + " from " + mVendorList(mFuelInvoice.VendorID).Name
        End If
        MarkLog(Util.Action.Close, "FuelInvoice", FuelInvoiceDetail, Util.ErrorType.NoError, mFuelInvoice.ID, EventLogID)
        'Session("IsValid") = IsValid
        setObject()

        If mFuelInvoice.IsDirty Then
            'If IsValid Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
            'End If
        Else
            Session.Remove("mFileAttach")
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter
        Dim rpt As New crptFuelInvoice
        mFuelInvoice = Session("mFuelInvoice")
        Dim ds As New dsFuelInvoice
        Dim mCompanyDetail As New CompanyDetail
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, "Fuel Invoice", SearchStr1:=cmbUnit.SelectedItem.Text, SearchStr2:=cmbCurrencyList.SelectedItem.Text, SearchStr3:="", SearchStr4:="", SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"), SearchStr11:="")
        Dim mrptImage = rptImage.GetImage(ds)
        da.Fill(ds, mFuelInvoice)
        da.Fill(ds, mFuelInvoice.FuelInvoiceLogs)
        da.Fill(ds, mFuelInvoice.FuelInvoiceCharges)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        rpt.SetDataSource(ds)
        Session("CrystalReport") = rpt
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
    Private Sub calFuelInvoiceDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calFuelInvoiceDate.TextChanged
        If Not (New SmartDate(mFuelInvoice.Date.ToString, True).Text = New SmartDate(CType(Trim(calFuelInvoiceDate.Text), Object).ToString, True).Text) Then
            If calFuelInvoiceDate.Text = "" Then
                mFuelInvoice.Date = System.DBNull.Value
            Else
                mFuelInvoice.Date = CDate(calFuelInvoiceDate.Text)
            End If
            txtText.Text = mFuelInvoice.Text
        End If
        upnlFuelInvoiceDetails.DataBind()
    End Sub
    Private Sub hdnBtnFileUpload_Click(sender As Object, e As System.EventArgs) Handles hdnBtnFileUpload.Click
        'AttachMyFile()
        If mFileAttach.ReferenceID.Equals(mFuelInvoice.ID) Then
            If mFuelInvoice.IsAttachmentAdded Then
                mFuelInvoice.FileAttachments(0).Size = mFileAttach.Size
                mFuelInvoice.FileAttachments(0).ImageFile = mFileAttach.ImageFile
                mFuelInvoice.FileAttachments(0).Extension = mFileAttach.Extension
            Else
                mFuelInvoice.IsAttachmentAdded = True
                mFuelInvoice.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
            End If
        Else
            If mFuelInvoice.FuelInvoiceLogs(mFileAttach.ReferenceID).IsAttachmentAdded Then
                mFuelInvoice.FuelInvoiceLogs(CType(Session("index"), Integer)).FileAttachments(0).Size = mFileAttach.Size
                mFuelInvoice.FuelInvoiceLogs(CType(Session("index"), Integer)).FileAttachments(0).ImageFile = mFileAttach.ImageFile
                mFuelInvoice.FuelInvoiceLogs(CType(Session("index"), Integer)).FileAttachments(0).Extension = mFileAttach.Extension
            Else
                mFuelInvoice.FuelInvoiceLogs(mFileAttach.ReferenceID).IsAttachmentAdded = True
                mFuelInvoice.FuelInvoiceLogs(CType(Session("index"), Integer)).FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
            End If
        End If
        FuelInvoiceItemDataGrid()
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mFuelInvoice.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachmentChild(mFuelInvoice.ID)
        Else
            mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mFuelInvoice.ID)
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
        mFuelInvoice.IsAttachmentAdded = False
        mFuelInvoice.FileAttachments.Remove(mFuelInvoice.ID)
        Session("mFuelInvoice") = mFuelInvoice
    End Sub
    Private Sub hdnimgBtnCommonPartList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnCommonPartList.Click
        If CType(Session("AddParts"), String) = "True" Then
            AddMultipleParts()
            Session("AddParts") = "False"
        Else
            Session("AddParts") = "False"
        End If
        FuelInvoiceItemDataGrid()
        ControlVisibility()
        TextChanged(sender, e)
    End Sub
    Protected Sub btnCopyInTextBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        setObject()
        setSession()
        Dim txtRate As TextBox
        Dim currentRow As GridViewRow = CType(sender, ImageButton).Parent.Parent
        txtRate = CType(currentRow.FindControl("txtRate"), TextBox)
        Dim i As Integer = 0
        If txtRate.Text <> "" Then
            For Each mFuelInvoiceLog In mFuelInvoice.FuelInvoiceLogs
                With mFuelInvoiceLog
                    .CRate = CDec(Val(txtRate.Text))
                End With
                i = i + 1
            Next
        End If
        FuelInvoiceItemDataGrid()
        ControlVisibility()
        TextChanged(sender, e)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

#Region " Status "
    ''====================================WO - 2006-2007-1-19
    Private Sub btnAuthorized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
        If IsValid Then

            If mVendorList(mFuelInvoice.VendorID).NotInUse = True Then  'Added by Saylee on 24-Jul-2012
                If CDate(mVendorList(mFuelInvoice.VendorID).NotInUseDate) <= CDate(mFuelInvoice.Date) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Record can not be saved. <br><br> Supplier is not applicable since " + mVendorList(mFuelInvoice.VendorID).NotInUseDateFormatted + " <br><br> Select another Supplier from list or select date before " + mVendorList(mFuelInvoice.VendorID).NotInUseDateFormatted + " & try again", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
            MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<Strong> FuelInvoice </Strong>", MsgBoxStyle.YesNo, "Status")
            Session("mFuelInvoice") = mFuelInvoice
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If IsValid Then
            MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<Strong> FuelInvoice </Strong>", MsgBoxStyle.YesNo, "StatusCancel")
            Session("mFuelInvoice") = mFuelInvoice
        End If
    End Sub
#End Region

#Region " Add Multiple Parts "
    Private Sub AddMultipleParts()
        Dim mFuelLogListPendingForInvoice As FuelLogListPendingForInvoice
        Dim mFuelLogListPendingForInvoices As FuelLogListPendingForInvoices = Session("mFuelLogListPendingForInvoices")
        Dim mUnitConversionList As UnitConversionList
        mUnitConversionList = UnitConversionList.GetUnitConversionList()
        For Each mFuelLogListPendingForInvoice In mFuelLogListPendingForInvoices
            If mFuelLogListPendingForInvoice.IsSelected Then
                If Not mFuelInvoice.FuelInvoiceLogs.Contains(LogFuelID:=mFuelLogListPendingForInvoice.LogFuelID, str:="", str1:="") Then
                    mFuelInvoice.FuelInvoiceLogs.Add(mFuelInvoice.ID)
                    With mFuelInvoice.FuelInvoiceLogs.CurrentItem
                        .LogDate = mFuelLogListPendingForInvoice.Date
                        .LogPageNo = mFuelLogListPendingForInvoice.LogPageNo
                        .RegNo = mFuelLogListPendingForInvoice.RegNo
                        .From = mFuelLogListPendingForInvoice.From
                        .To = mFuelLogListPendingForInvoice.To
                        .LogFuelID = mFuelLogListPendingForInvoice.LogFuelID
                        .LogUpliftedFuel = mFuelLogListPendingForInvoice.FuelUplifted
                        .UnitName = mFuelLogListPendingForInvoice.UnitName
                        .UpliftedFuelInInvUnitToDispaly = mFuelLogListPendingForInvoice.FuelUplifted * mUnitConversionList(FromUnitID:=mFuelLogListPendingForInvoice.UnitID, ToUnitID:=cmbUnit.SelectedValue).ConversionFactor
                        .UpliftedFuelInvUnit = mFuelLogListPendingForInvoice.FuelUplifted * mUnitConversionList(FromUnitID:=mFuelLogListPendingForInvoice.UnitID, ToUnitID:=cmbUnit.SelectedValue).ConversionFactor
                        .CRate = 0
                    End With
                End If
            End If
        Next
        Session.Remove("mFuelLogListPendingForInvoices")
    End Sub
#End Region

#Region " Show BrokenRules "
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        setObject()
        If mFuelInvoice.IsValid = False Then
            For i As Integer = 0 To mFuelInvoice.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mFuelInvoice.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        Dim mFuelInvoiceLog As FuelInvoiceLog
        If mFuelInvoice.FuelInvoiceLogs.IsValid = False Then
            For Each mFuelInvoiceLog In mFuelInvoice.FuelInvoiceLogs
                For i As Integer = 0 To mFuelInvoiceLog.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mFuelInvoiceLog.LogNo + " : " + mFuelInvoiceLog.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If

        If strMsg.Trim <> "" Then
            cvCommon.ErrorMessage = strMsg
            cvCommon.IsValid = False
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

        If mFuelInvoice.IsValid = False Then
            For i As Integer = 0 To mFuelInvoice.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mFuelInvoice.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If

        Dim mFuelInvoiceLog As FuelInvoiceLog
        If mFuelInvoice.FuelInvoiceLogs.IsValid = False Then
            For Each mFuelInvoiceLog In mFuelInvoice.FuelInvoiceLogs
                For i As Integer = 0 To mFuelInvoiceLog.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mFuelInvoiceLog.LogNo + " : " + mFuelInvoiceLog.GetBrokenRulesCollection(i).Description + "<Br>"
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