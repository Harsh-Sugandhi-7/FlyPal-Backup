Public Class wfOtherChargeDocket_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mOtherCharge As OtherCharge
    Public mVendorList As VendorList
    Public mStatusList As StatusList
    Dim EventLogID As Guid 'Added By Utkarsh On 22-Jul-2011 For All19072011
    Dim OCDetail As String 'Added By Utkarsh On 22-Jul-2011 For All19072011
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mOtherCharge = Session("mOtherCharge")
    End Sub
	Private Sub setSession()
		Session("mOtherCharge") = mOtherCharge
	End Sub

	Private Sub SetObject()

		mOtherCharge.Date = CDate(txtOtherChargeDate.Text)
		mOtherCharge.BillEntryNo = txtBillEntryNo.Text
		mOtherCharge.MasterAirwayBillNo = txtMasterAirwayBillNo.Text
		mOtherCharge.HouseAirwayBillNo = txtHouseAirwayBillNo.Text

		If txtBillEntryDate.Text = "" Then
			mOtherCharge.BillEntryDate = System.DBNull.Value
		Else
			mOtherCharge.BillEntryDate = CDate(txtBillEntryDate.Text)
		End If

		If txtMasterAirwayBillDate.Text = "" Then
			mOtherCharge.MasterAirwayBillDate = System.DBNull.Value
		Else
			mOtherCharge.MasterAirwayBillDate = CDate(txtMasterAirwayBillDate.Text)
		End If

		If txtHouseAirwayBillDate.Text = "" Then
			mOtherCharge.HouseAirwayBillDate = System.DBNull.Value
		Else
			mOtherCharge.HouseAirwayBillDate = CDate(txtHouseAirwayBillDate.Text)
		End If

		mOtherCharge.Text = txtText.Text
		mOtherCharge.No = Val(txtNo.Text)
		Session("mOtherCharge") = mOtherCharge

	End Sub

	Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                   If MSGBoxCtrl.Sender = "Close" Then
                        If mOtherCharge.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not User.IsInRole("OtherChargeNew") And Not User.IsInRole("OtherChargeEdit")) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user ", False), True)
                                Exit Sub
                            End If
                            Save()
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                       Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("wfReceiptCumInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                    End If
                   Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
   Private Sub SetPage()
        If mOtherCharge.No > 0 Then
            lblTitle.Text = "Other Charge [" & mOtherCharge.Text + "-" + CType(mOtherCharge.No, String) + "]"
        End If
    End Sub
    Private Sub Save()

		If mOtherCharge.IsValid Then

			'Authentication
			If mOtherCharge.Date IsNot DBNull.Value Then

				Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))

				If mCheck.WebAuthentication = True Then

					Dim mDays As Integer = mCheck.Number("Days")

					Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)

					If DateDiff(DateInterval.Day, CDate(mOtherCharge.Date), maxAllowableDate) < 0 Then
						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.saveAlert, " Your subscription has been expired. can not save Goods Receipt. <br> Goods Receipt Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
						Exit Sub
					End If

				End If

			End If

			'Authentication
			Dim OtherChargeClone As OtherCharge
			OtherChargeClone = mOtherCharge.Clone
			Try

				If Not mOtherCharge.OtherChargeInvoices.Count = 0 And Not mOtherCharge.OtherChargeDetails.Count = 0 Then

					SetObject()
					mOtherCharge.ApplyEdit()
					mOtherCharge.Save()
					OCDetail = $"{mOtherCharge.OtherChargeNo} Dated : {mOtherCharge.DateFormatted}"
					MarkLog(Action.Save,
							"Other Charge",
							OCDetail,
							ErrorType.NoError,
							mOtherCharge.ID,
							EventLogID)

					mOtherCharge.MarkClean()
					SetPage()
					Session("mOtherCharge") = mOtherCharge
					txtText.DataBind()
					txtNo.DataBind()
					upnlTitle.Update()
					upblOtherCharge.Update()

					MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully,
									MSGBox.Message_Text.SavedSuccessFully,
									"",
									MsgBoxStyle.OkOnly,
									"")

				Else

					MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
									MSGBox.Message_Text.saveAlert,
									"Other Charge can not be saved without Invoice and Charge.",
									MsgBoxStyle.OkOnly,
									"")

					mOtherCharge = OtherChargeClone
					SetObject()
					Session("mOtherCharge") = mOtherCharge
					DataFieldBind()

					Exit Sub

				End If

			Catch ex As SqlException
				Session("OtherChargeClone") = OtherChargeClone
				If ex.Number = 8114 Or ex.Number = 8115 Then
					MSGBoxCtrl.Show(MSGBox.Message_Title.NumericOverFlow, MSGBox.Message_Text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
					Exit Sub
				ElseIf ex.Number = 8145 Then
					MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
					Exit Sub
				ElseIf ex.Number = 2627 Then
					MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
					Exit Sub
				ElseIf ex.Number = 547 Then
					'Code Added by DEVEN On 28/12/2007 --------------------------------------
					If InStr(ex.Message, "FKtabOtherChargeDetailstabCharge", CompareMethod.Text) Then
						MSGBoxCtrl.Show("Alert!", "Other Charge Deleted ! ", "The selected charge can’t be found.<Br><BR>It may have been removed or is no longer available. Please delete it from your selection and choose a new charge to continue.", MsgBoxStyle.OkOnly, "")
						Exit Sub
						'------------------------------------------------------------------------
					Else
						MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete, MSGBox.Message_Text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
						Exit Sub
					End If
				ElseIf ex.Number = 50000 Then
					MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
			Finally
				OtherChargeClone = Nothing
			End Try

		Else

			MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
							MSGBox.Message_Text.saveAlert,
							"Charge Name is required",
							MsgBoxStyle.OkOnly,
							"")

			DataFieldBind()

			Exit Sub

		End If

	End Sub
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
                custValidator.ErrorMessage = "Select Other Charge Date."
                e.IsValid = False
            ElseIf (CDate(txtOtherChargeDate.Text) < CDate(mOtherCharge.OtherChargeInvoices.CurrentItem.Date)) Then
                custValidator.ErrorMessage = "Docket Date Should Not Be Less Than Invoice Date."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 22-Jul-2011 For All19072011
        addAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
        End If
        SetPage()
    End Sub
    Private Sub btnAddCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCharge.Click
        If IsValid Then
            setObject()
            Session("EditCharge") = False
            mOtherCharge.OtherChargeDetails.Add(mOtherCharge.ID)
            Session("mOtherCharge") = mOtherCharge
            Response.Redirect("wfOtherChargeDocketDetails_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfOtherChargeDocket_Ajax.aspx")
        Else
            upnlTitle.Update()
            Exit Sub
        End If
    End Sub
    Private Sub dgCharges_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCharges.RowCommand
        Dim Idx As Int32
        Select Case e.CommandName
            Case "EditView"
                'Dim Index As Int32 = CInt(e.CommandArgument) + dgCharges.PageIndex * dgCharges.PageSize
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 21-feb-2023
                Idx = gvr.RowIndex
                setObject()
                Session("EditCharge") = True
                mOtherCharge.OtherChargeDetails.CurrentIndex = Idx 'Ajay on 21-feb-2023
                Session("mOtherCharge") = mOtherCharge
                Response.Redirect("wfOtherChargeDocketDetails_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfOtherChargeDocket_Ajax.aspx")
        End Select
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("OtherChargeNew") And mOtherCharge.IsNew) Or (Not User.IsInRole("OtherChargeEdit") And Not mOtherCharge.IsNew) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user ", False), True)
            Exit Sub
        End If
        If IsValid Then
            Save()
        Else
            upnlTitle.Update()
            Exit Sub
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "Other Charge", "", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Changed By Utkarsh On 22-Jul-2011 For All19072011
        Session("IsValid") = IsValid
        If IsValid Then
            setObject()
        Else
            upnlTitle.Update()
            Exit Sub
        End If
        Dim mReceiptCumInvoice As ReceiptCumInvoice 'Added By Prashant 5-Mar-2014
        mReceiptCumInvoice = Session("mReceiptCumInvoice")
        mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(mReceiptCumInvoice.ID, mOtherCharge.OtherChargeInvoices.CurrentItem.InvoiceID)
        Session("mReceiptCumInvoice") = mReceiptCumInvoice '-----------------------------
        If mOtherCharge.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
        Else
            Response.Redirect("wfReceiptCumInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
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
        Dim str As String
        str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", str, True)
    End Sub
#End Region

End Class