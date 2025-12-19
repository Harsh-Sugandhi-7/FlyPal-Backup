'AJAX Conversion by vikrant on 04-Aug-2015

Public Class wfMaintenanceInvoice_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMaintenanceInvoice As MaintenanceInvoice              'Code 18-FR03-VA06
    Public mMaintenanceInvoiceList As MaintenanceInvoiceList
    Public mVendorList As VendorList
    Public mItemList As ItemList
    Public mChargesForList As ChargesForList
    Dim EventLogID As Guid 'Added By Utkarsh On 22-Jul-2011 For All19072011
    Dim MIDetail As String 'Added By Utkarsh On 22-Jul-2011 For All19072011
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mMaintenanceInvoice = CType(Session("mMaintenanceInvoice"), MaintenanceInvoice)
        mMaintenanceInvoiceList = Session("mMaintenanceInvoiceList")
        mVendorLIst = Session("mVendorList")
        mItemList = Session("mItemList")
        mChargesForList = Session("mChargesForList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMaintenanceInvoice")
        Session.Remove("mMaintenanceInvoiceList")
        Session.Remove("mVendorList")
        Session.Remove("mItemList")
        Session.Remove("mChargesForList")
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtNo').value,event)")
        txtRefNo.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtRefNo').value,event)")
        txtQuantity.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtQuantity').value,event)")
        txtRate.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtRate').value,event)")
    End Sub
    Private Sub SetPage()
        If mMaintenanceInvoice.IsNew Then
            lblTitle.Text = "Maintenance Invoice Details [New]"
        Else
            lblTitle.Text = "Maintenance Invoice [" & mMaintenanceInvoice.InvoiceText & "-" & mMaintenanceInvoice.InvoiceNo & "]"
        End If
        upnlTitle.Update()
    End Sub
    Private Sub setObject()
        mMaintenanceInvoice.Date1 = CDate(txtMaintenanceInvoiceDate.Text)
        mMaintenanceInvoice.VendorID = New Guid(cmbVendorList.SelectedValue)
        mMaintenanceInvoice.VendorInvoiceNo = txtVendorInvoiceNo.Text
        If txtVendorInvoiceDate.Text <> "" Then
            mMaintenanceInvoice.VendorInvoiceDate = CDate(txtVendorInvoiceDate.Text)
        Else
            mMaintenanceInvoice.VendorInvoiceDate = System.DBNull.Value
        End If
        ' mMaintenanceInvoice.VendorInvoiceDate = txtVendorInvoiceDate.Text
        mMaintenanceInvoice.ItemID = New Guid(cmbPartNo.SelectedValue)
        mMaintenanceInvoice.SerialNo = txtSerialNo.Text
        If cmbChargesFor.SelectedValue <> "(All)" Then
            mMaintenanceInvoice.ChargeFor = cmbChargesFor.SelectedValue
        Else
            mMaintenanceInvoice.ChargeFor = txtChargeFor.Text
        End If
        mMaintenanceInvoice.Quantity = txtQuantity.Text
        mMaintenanceInvoice.Rate = Val(txtRate.Text)
        mMaintenanceInvoice.OtherCharges = Val(txtOtherCharges.Text)
        mMaintenanceInvoice.Remark = txtRemark.Text
        mMaintenanceInvoice.InvoiceText = txtText.Text
        mMaintenanceInvoice.InvoiceNo = Val(txtNo.Text)

        Session("mMaintenanceInvoice") = mMaintenanceInvoice
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
                        Session("sender") = ""
                        Page.Validate("1")
                        If Page.IsValid Then
                            Session.Remove("IsValid")
                            '=======
                            'Authentication
                            If Not mMaintenanceInvoice.Date1 Is System.DBNull.Value Then
                                Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
                                If mCheck.WebAuthentication = True Then
                                    Dim mDays As Integer = 0

                                    'Changes by Kalpesh in 13-3-2013
                                    'These lines commented
                                    '
                                    'Dim strOutString As String = ReadXMLFile()
                                    'strOutString = strOutString.Split(CChar("$"))(1)
                                    'mDays = CInt(strOutString) - mCheck.ElapsedDays


                                    'Changes by Kalpesh in 13-3-2013
                                    'These lines commented
                                    '
                                    mDays = mCheck.Number("Days")
                                    mDays = mDays - mCheck.ElapsedDays
                                    '---------------------------------

                                    'If DateDiff(DateInterval.Day, CDate(mEnquiry.Date), mCheck.MaxAllowableDate("Authority.dll")) < 0 Then
                                    If mDays < 0 Then
                                        MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Maintenance Invoice.", MsgBoxStyle.OkOnly, "")
                                        Exit Sub
                                    End If
                                End If
                            End If
                            'Authentication
                            '========
                            Dim MaintenanceInvoiceClone As MaintenanceInvoice
                            MaintenanceInvoiceClone = mMaintenanceInvoice.Clone
                            Try
                                If (Not User.IsInRole("MaintenanceInvoiceNew") And Not User.IsInRole("MaintenanceInvoiceEdit")) Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                End If
                                mMaintenanceInvoice.Save()
                                Session("mMaintenanceInvoice") = mMaintenanceInvoice
                                RemoveSession()
                                Response.Redirect("Index.aspx")
                            Catch ex As SqlClient.SqlException
                                Session("MaintenanceInvoiceClone") = MaintenanceInvoiceClone
                                If ex.Number = 8114 Or ex.Number = 8115 Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                                ElseIf ex.Number = 8145 Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                ElseIf ex.Number = 2627 Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                ElseIf ex.Number = 547 Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                End If
                            Finally
                                MaintenanceInvoiceClone = Nothing
                                'Added By Utkarsh On 22-Jul-2011 For All19072011
                                MIDetail = mMaintenanceInvoice.InvoiceText & "-" & mMaintenanceInvoice.InvoiceNo & " Dated : " + mMaintenanceInvoice.Date1Formatted + " from " + mVendorList(mMaintenanceInvoice.VendorID).Name
                                MarkLog(Util.Action.Save, "Maintenance Invoice", User.Identity.Name & " is not Authorized User to save " & MIDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                                'End
                            End Try
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        RemoveSession()
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    Else
                        Session("Sender") = ""
                    End If
                Case MsgBoxResult.Ok And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    'Added by Utkarsh On 16-Dec-2013 For TransTextSeries
                Case MsgBoxResult.Ok And CType(Session("sender"), String) = "MaintenanceInvoiceTransTextSeriesAlert"
                    Session("sender") = ""
                    Session("AddTransTextSeries") = "True"
                    'DataFieldBind()
                    Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    'ENd
                Case Else
                    Session("Sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mVendorList = VendorList.GetVendortList(0, "", "", "", "", "", True, False, True)
        cmbVendorList.DataSource = mVendorList
        Session("mVendorList") = mVendorList

        mItemList = ItemList.GetItemList(0, "", "", "", "", "", "", True)
        cmbPartNo.DataSource = mItemList
        Session("mItemList") = mItemList

        mChargesForList = ChargesForList.getChargeForList(True, "(All)")
        cmbChargesFor.DataSource = mChargesForList
        Session("mChargesForList") = mChargesForList

        txtMaintenanceInvoiceDate.Text = mMaintenanceInvoice.Date1Formatted
        txtVendorInvoiceDate.Text = mMaintenanceInvoice.VendorInvoiceDateFormatted.ToString
        DataBind()
        If mMaintenanceInvoice.ChargeFor <> "" Then
            If cmbChargesFor.Items.Contains(New ListItem(mMaintenanceInvoice.ChargeFor, mMaintenanceInvoice.ChargeFor)) Then
                cmbChargesFor.SelectedValue = mMaintenanceInvoice.ChargeFor
            End If
        End If
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtOtherCharges" Then
            If Val(txtOtherCharges.Text) < 0 Then
                custValidator.ErrorMessage = "Other Charges can not be Negative"
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtQuantity" Then
           
            Dim errMsg As String = ""
            If Val(txtQuantity.Text) <= 0 Then
                errMsg = "Quantity must be greater than zero"
                custValidator.ErrorMessage = errMsg
                e.IsValid = False
            End If
            Dim mItem As Item
            Dim id As New Guid(cmbPartNo.SelectedValue)
            If id.Equals(Guid.Empty) Then Exit Sub
            mItem = Item.GetItem(id)
            If mItem.SerialisedStatus = True Then
                If Val(txtQuantity.Text) > 1 Then
                    errMsg = errMsg & " Serialized Qty must be One"
                    custValidator.ErrorMessage = errMsg
                    e.IsValid = False
                End If
                If txtSerialNo.Text = "" Then
                    If errMsg <> "" Then
                        errMsg = errMsg & " and Serial No. Required"
                    Else
                        errMsg = " Enter the Serial No"
                    End If
                    custValidator.ErrorMessage = errMsg
                    e.IsValid = False
                End If
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 22-Jul-2011 For All19072011
        addAttributes()
        If Not IsPostBack Then
            'Added by Utkarsh on 16-Dec-2013 for Trans Text Series
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If mMaintenanceInvoice.IsNew Then
                    mMaintenanceInvoice.InvoiceText = Session("TransText_ForTransSeries")
                    txtText.Text = mMaintenanceInvoice.InvoiceText
                    Session("mMaintenanceInvoice") = mMaintenanceInvoice
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If
            End If
            'End
            SetPage()
            If Session("Edit") = True Then
                txtMaintenanceInvoiceDate.Enabled = False
            Else
                txtMaintenanceInvoiceDate.Enabled = True
            End If
            DataFieldBind()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Changed By Utkarsh On 22-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, "Maintenance Invoice", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        Session("Edit") = False
        'setObject()
        If mMaintenanceInvoice.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.BackConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            RemoveSession()
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("MaintenanceInvoiceNew") And Not User.IsInRole("MaintenanceInvoiceEdit")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'Authentication
        If Not mMaintenanceInvoice.Date1 Is System.DBNull.Value Then
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

                If DateDiff(DateInterval.Day, CDate(mMaintenanceInvoice.Date1), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Maintenance Invoice. <br> Maintenance Invoice Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        End If
        'Authentication
        If IsValid Then
            setObject()

            'Added by Utkarsh ON 16-Dec-2013 FOr TransTextSeries
            'Check if Text is blank then call TransTextSeries UI

            If (mMaintenanceInvoice.IsNew) And (mMaintenanceInvoice.InvoiceText = "") Then

                Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(Trans.MaintenanceInvoice, mMaintenanceInvoice.Date1Formatted)

                If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(Trans.MaintenanceInvoice) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(Trans.MaintenanceInvoice) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(Trans.MaintenanceInvoice).TransText = "")) Then

                    Dim str = "<script language='javascript'>openledgersame('wfMaintenanceInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"
                    Session("BackPagestr_ForTransSeries") = str

                    Session("TransName_ForTransSeries") = "Maintenance Invoice"
                    Session("TransTypeID_ForTransSeries") = Trans.MaintenanceInvoice
                    Session("TransDate_ForTransSeries") = mMaintenanceInvoice.Date1Formatted

                    MSGBoxCtrl.show("Maintenance Invoice Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "MaintenanceInvoiceTransTextSeriesAlert")
                    Exit Sub
                Else
                    Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                    If mAutoRenewTransTextSeries.IsRenewed Then
                        With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(Trans.MaintenanceInvoice)
                            mMaintenanceInvoice.InvoiceText = .TransText
                            mMaintenanceInvoice.InvoiceNo = .StartingTransNo
                        End With
                    Else
                        Dim str = "<script language='javascript'>openledgersame('wfMaintenanceInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');</script>"
                        Session("BackPagestr_ForTransSeries") = str

                        Session("TransName_ForTransSeries") = "Maintenance Invoice"
                        Session("TransTypeID_ForTransSeries") = Trans.MaintenanceInvoice
                        Session("TransDate_ForTransSeries") = mMaintenanceInvoice.Date1Formatted

                        MSGBoxCtrl.show("Maintenance Invoice Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "MaintenanceInvoiceTransTextSeriesAlert")
                        Exit Sub
                    End If
                End If

            End If

            'End
            mMaintenanceInvoice.Save()
            txtMaintenanceInvoiceDate.Enabled = False
            'Changed By Utkarsh On 22-Jul-2011 For All19072011
            MIDetail = mMaintenanceInvoice.InvoiceText & "-" & mMaintenanceInvoice.InvoiceNo & " Dated : " + mMaintenanceInvoice.Date1Formatted + " from " + mVendorLIst(mMaintenanceInvoice.VendorID).Name
            MarkLog(Util.Action.Save, "Maintenance Invoice", cmbVendorList.SelectedItem.Text, Util.ErrorType.NoError, mMaintenanceInvoice.ID, EventLogID)
            'End
            SetPage()
            DataFieldBind()
            upnlInvoiceDetails.Update()
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    Private Sub txtMaintenanceInvoiceDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtMaintenanceInvoiceDate.TextChanged
        mMaintenanceInvoice.Date1 = txtMaintenanceInvoiceDate.Text
        txtText.Text = mMaintenanceInvoice.InvoiceText
        txtText.DataBind()
        Session("mMaintenanceInvoice") = mMaintenanceInvoice
    End Sub
End Class