'***********************************
'Modified by Harsh Sugandhi On 05th November 2025 => To Retrieve Last Updated by & Created by
'***********************************


Imports System.Linq
Imports System.Collections.Generic
Imports System.Text


Public Class DentAndRepairDetailPage
	Inherits Page


#Region " Enum "

	Private Enum RequestFor

		Supplier = 0
		Customer = 1

	End Enum

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

#End Region

#Region " Variable(s) "

	Dim Flag As Integer
	Dim EventLogID As Guid                                          'Added by Saylee on 19-July-2011
	Dim mOpenFrom As String                         'Added By Vikrant on 13-Oct-2014 For Req Item Status Report
	Dim mModuleName As String                                       'Added by Saylee on 19-July-2011
	Dim IssueDetail As String
	Dim BaseCurrencySymbol As String = ""
	Dim IsAttachmentDeleted As Boolean = False      'Added By Vikrant On 23-Dec-2014 For All23122014-2
	Dim NumberOfIssusDetails As New StringBuilder

	Dim mUser As User
	Public mOrderItem As OrderItem
	Public mFileAttach As FileAttach
	Public mDentBuckle As DentBuckle
	Public mLogList As ReportLogRegister
	Public mLocationList As LocationList
	Public mVendorApprovals As VendorApprovals
	Public mShipToTypeList As BillToShipToTypeList
	Private AttachmentHelper As New AttachmentHelper
	Public mMachineNameValueList As MachineNameValueList
	Public mBillToShipToTypeList As BillToShipToTypeList
	Private AuthorizationHelper As New AuthorizationHelper
	Public mRequisitionItemOrderItems As RequisitionItemOrderItems  'Added by Vikrant For New Requisition

#End Region

#Region " Business Methods "

	Private Sub GetSession()

		mDentBuckle = Session("mDentBuckle")
		mLogList = Session("mLogList")
		mModuleName = Session("mModuleName")
		mFileAttach = Session("mFileAttach")
		IsAttachmentDeleted = Session("IsAttachmentDeleted")
		mMachineNameValueList = Session("mMachineNameValueList")

	End Sub

	Private Sub RemoveSession()

		Session.Remove("mFileAttach")
		Session.Remove("IsAttachmentDeleted")
		Session.Remove("mLogList")
		Session.Remove("mMachineNameValueList")

	End Sub

	Private Sub SetObject()

		Try

			If txtReportDate.Text = "" Then
				mDentBuckle.ReportDate = Today.Date
			Else
				mDentBuckle.ReportDate = CDate(txtReportDate.Text)
			End If

			mDentBuckle.MachineID = New Guid(cmbMachineList.SelectedValue)

			If cmbLogList.Enabled And cmbLogList.SelectedIndex > 0 Then
				mDentBuckle.LogID = New Guid(cmbLogList.SelectedValue)
			End If

			mDentBuckle.UserName = User.Identity.Name
			mDentBuckle.Text = txtText.Text
			mDentBuckle.No = Val(txtNo.Text)

			If txtRevDate.Text = "" Then
				mDentBuckle.RevDate = DBNull.Value
			Else
				mDentBuckle.RevDate = CDate(txtRevDate.Text)
			End If

			mDentBuckle.RevNo = Trim(txtRevNo.Text)

			If Not mFileAttach Is Nothing Then

				If mFileAttach.Size > 0 Then
					mDentBuckle.IsAttachmentAdded = True
				Else
					mDentBuckle.IsAttachmentAdded = False
				End If

			End If

			If Not User.Identity.Name.ToString.Equals("BTPLADMIN", StringComparison.CurrentCultureIgnoreCase) AndAlso
			   mDentBuckle.IsNew = True Then

				mDentBuckle.CreatedBy = User.Identity.Name
			Else
				mDentBuckle.LastUpdatedBy = User.Identity.Name
			End If

			Session("mDentBuckle") = mDentBuckle

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DeleteRecord(Index As Int32)

		MSGBoxCtrl.Show(MSGBox.Message_Title.RemoveItem,
						MSGBox.Message_Text.RemoveItem,
						"",
						MsgBoxStyle.YesNo,
						"Delete")

		mDentBuckle.DentBuckleItems.CurrentIndex = Index - 1
		Session("mDentBuckle") = mDentBuckle

	End Sub

	Private Sub MessageBoxResult()

		Dim MsgBoxResult As MsgBoxResult
		MsgBoxResult = MSGBoxCtrl.Result
		Try

			If MsgBoxResult > 0 Then

				Select Case MsgBoxResult

					Case MsgBoxResult.Yes

						If MSGBoxCtrl.Sender = "Delete" Then

							Try

								Session("Sender") = ""
								Dim mDentBuckle As DentBuckle
								mDentBuckle = CType(Session("mDentBuckle"), DentBuckle)
								mDentBuckle.DentBuckleItems.Remove(mDentBuckle.DentBuckleItems.CurrentItem)
								Session("mDentBuckle") = mDentBuckle
								DataFieldBind()
								upnlDetails.Update()
								upnlItems.Update()

							Catch ex As SqlException
								ScriptManager.RegisterStartupScript(Me, [GetType], "OpenScript", MessageBox.Show(ex.Message, False), True)
								Exit Sub
							End Try

						End If

						If MSGBoxCtrl.Sender = "Close" Then

							Session("sender") = ""

							If mDentBuckle.IsValid = True Then

								Session.Remove("IsValid")
								DataFieldBind()

								If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																				Action:={Action.[New], Action.Edit},
																				MSGBoxCtrl:=MSGBoxCtrl,
																				ModuleName:="Dent&RepairChart") Then

									Exit Sub

								End If

								If Save() Then
									RemoveSession()
									Response.Redirect("Index.aspx")
								Else
									Exit Sub
								End If

							Else
								upnlValidationsummary.Update()
								Exit Sub
							End If

						End If

						If MSGBoxCtrl.Sender = "Status" Then

							Session("sender") = ""

							If Session("IsValid") Then

								Session.Remove("IsValid")
								mDentBuckle.StatusID = 2
								DataFieldBind()

								If Save() = True Then
									UpdatePanel()
									upnlItems.Update()
								End If

							Else
								Session.Remove("IsValid")
							End If

						End If

					Case MsgBoxResult.No

						If MSGBoxCtrl.Sender = "Close" Then

							Session.Remove("IsValid")
							Session("Sender") = ""
							If mDentBuckle.IsNew Then Session.Remove("mDentBuckle")
							RemoveSession()
							Response.Redirect("Index.aspx")

						End If

						If MSGBoxCtrl.Sender = "Status" Then

							Session("Sender") = ""
							Session.Remove("IsValid")
							UpdatePanel()
							upnlItems.Update()

						End If

						If (MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "StatusCancel") Then

							Session("Sender") = ""
							Session.Remove("IsValid")
							Session("mDentBuckle") = mDentBuckle
							DataFieldBind()
							UpdatePanel()
							upnlItems.Update()

						End If

					Case MsgBoxResult.Ok

						If MSGBoxCtrl.Sender = "Status" Then

							Session("sender") = ""

							If mDentBuckle.StatusID = 2 Then
								mDentBuckle.StatusID = 1
							ElseIf mDentBuckle.StatusID = 3 Or mDentBuckle.StatusID = 4 Then
								mDentBuckle.StatusID = 2
							End If

							Session("mDentBuckle") = mDentBuckle
							Session("NotEqualsQty") = "NotEqualsQty"
							DataFieldBind()
							UpdatePanel()
							upnlItems.Update()

						End If


						If MSGBoxCtrl.Sender = "IssueTransTextSeriesAlert" Then
							Session("AddTransTextSeries") = "True"
							Session("sender") = "IssueCreate" 'Need to set again
							Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
						End If

				End Select

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Function CheckDateForTransactionLock(TransDate As Date) As Boolean 'Added By Vikrant On 28-July-2014 For BA24072014

		Try

			Dim FirstDayOfLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
			Dim FirstDayOfMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)

			If (TransDate >= FirstDayOfLastMonth) Then

				If (TransDate < FirstDayOfMonth) And (Day(Today.Date) > 10) Then
					Return True
				Else
					Return False
				End If

			Else
				Return True
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function 'End

	Private Sub ShowMessage(mOrderItem As OrderItem,
							mIssue As Issue,
							Optional IssueDetail As String = "")

		Dim str1 As String = ""
		Dim Count As Integer = 0
		Dim Count1 As Integer = 0
		Try

			For Each mOrderItem In mDentBuckle.DentBuckleItems

				If (mOrderItem.ReceiptItemID.Equals(Guid.Empty) Or
				   (Not mOrderItem.ReceiptItemID.Equals(Guid.Empty) And
					mOrderItem.EROQty = (0 Or 0.0))) Then
					Count += 1
				Else
					Count += 1
				End If

			Next

			If (Count > 0 And Count1 > 0) Then

				str1 = str1 + ("<span class=""clsLabelAuto"">Issue(s) Created Successfully! <BR>" + IssueDetail + "</BR></span>")
				str1 = str1 + ("<p><span class=""clsLabelAuto"">Automated issue will not be created for following items. As source receipt not selected/Qty. is zero " + "</span></p>")
				str1 = str1 + ("<TABLE width =""100%"" BORDER=1 CELLSPACING=0 CELLPADING=0 ID=""Table2"">")
				str1 = str1 + ("<tr>" & "<td WIDTH=60px align=""left"">" & "<font face=""Calibri""><b>Sr. No. </b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Part No.</b>" & "</font>" & "</td><td WIDTH=100px align=""right"">" & "<font face=""Calibri""><b>Qty.</b>" & "</font>" & "</td></tr>")

				For Each mOrderItem In mDentBuckle.DentBuckleItems

					If (mOrderItem.ReceiptItemID.Equals(Guid.Empty) Or (Not mOrderItem.ReceiptItemID.Equals(Guid.Empty) And mOrderItem.EROQty = (0 Or 0.0))) Then

						str1 = str1 + ("<TR>")
						str1 = str1 + ("<TD WIDTH=60px align=""left"">")
						str1 = str1 + ("<font face=""Calibri"">")
						str1 = str1 + CStr(mOrderItem.SrNo)
						str1 = str1 + ("</font>")
						str1 = str1 + ("</TD>")

						str1 = str1 + ("<TD align=""left"">")
						str1 = str1 + ("<font face=""Calibri"">")
						str1 = str1 + mOrderItem.ItemName
						str1 = str1 + ("</font>")
						str1 = str1 + ("</TD>")

						str1 = str1 + ("<TD WIDTH=100px align=""right"">")
						str1 = str1 + ("<font face=""Calibri"">")
						str1 = str1 + CStr(mOrderItem.EROQty)
						str1 = str1 + ("</font>")
						str1 = str1 + ("</TD>")

						str1 = str1 + ("</TR>")

					End If

				Next

				str1 = str1 + ("</TABLE>")

			ElseIf (Count > 0) Then

				str1 = str1 + ("<p><span class=""clsLabelAuto"">Automated issue will not be created for following items. As source receipt not selected/Qty. is zero " + "</span></p>")
				str1 = str1 + ("<TABLE width =""100%"" BORDER=1 CELLSPACING=0 CELLPADING=0 ID=""Table2"">")
				str1 = str1 + ("<tr>" & "<td WIDTH=60px align=""left"">" & "<font face=""Calibri""><b>Sr. No. </b>" & "</font>" & "</td><td align=""left"">" & "<font face=""Calibri""><b>Part No.</b>" & "</font>" & "</td><td WIDTH=100px align=""right"">" & "<font face=""Calibri""><b>Qty.</b>" & "</font>" & "</td></tr>")

				For Each mOrderItem In mDentBuckle.DentBuckleItems

					If (mOrderItem.ReceiptItemID.Equals(Guid.Empty) Or (Not mOrderItem.ReceiptItemID.Equals(Guid.Empty) And mOrderItem.EROQty = (0 Or 0.0))) Then

						str1 = str1 + ("<TR>")
						str1 = str1 + ("<TD WIDTH=60px align=""left"">")
						str1 = str1 + ("<font face=""Calibri"">")
						str1 = str1 + CStr(mOrderItem.SrNo)
						str1 = str1 + ("</font>")
						str1 = str1 + ("</TD>")

						str1 = str1 + ("<TD align=""left"">")
						str1 = str1 + ("<font face=""Calibri"">")
						str1 = str1 + mOrderItem.ItemName
						str1 = str1 + ("</font>")
						str1 = str1 + ("</TD>")

						str1 = str1 + ("<TD WIDTH=100px align=""right"">")
						str1 = str1 + ("<font face=""Calibri"">")
						str1 = str1 + CStr(mOrderItem.EROQty)
						str1 = str1 + ("</font>")
						str1 = str1 + ("</TD>")

						str1 = str1 + ("</TR>")

					End If

				Next

				str1 = str1 + ("</TABLE>")

			ElseIf (Count1 > 0) Then
				str1 = str1 + ("<span class=""clsLabelAuto"">Issue(s) Created Successfully! <BR>" + IssueDetail + "</BR></span>")
			End If

			Session.Remove("IssueDetail")
			Session("IssueCreate") = ""
			MSGBoxCtrl.Show("Alert!", str1, "", MsgBoxStyle.OkOnly, "IssueCreated")

			Exit Sub

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub

	Private Sub SetLogDetails()

		Try

			If cmbMachineList.SelectedIndex > 0 Then

				mDentBuckle.MachineID = New Guid(cmbMachineList.SelectedValue)
				mLogList = ReportLogRegister.GetRectifiedLog(StartDate:=mDentBuckle.ReportDateFormatted.ToString,
															 EndDate:=mDentBuckle.ReportDateFormatted.ToString,
															 AssemblyID:=mDentBuckle.AFAssemblyID.ToString,
															 MachineID:=mDentBuckle.MachineID.ToString,
															 CalculateTotal:=False, ,
															 StatusSelectLog:=1, , , ,
															 AddTopItem:="(SELECT)", , ,
															 SkipVoidLog:=True)

				Session("mLogList") = mLogList
				cmbLogList.Enabled = CType(IIf(mDentBuckle.StatusID >= 2, False, True), Boolean)
				cmbLogList.DataSource = mLogList
				cmbLogList.DataBind()

			Else
				cmbLogList.ClearSelection()
				cmbLogList.Enabled = False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibility()

		Try

			txtText.Enabled = CType(IIf(mDentBuckle.StatusID >= 2, False, True), Boolean)
			txtNo.Enabled = CType(IIf(mDentBuckle.StatusID >= 2, False, True), Boolean)
			cmbMachineList.Enabled = CType(IIf(mDentBuckle.StatusID >= 2, False, True), Boolean)
			txtReportDate.Enabled = (CType(IIf(mDentBuckle.StatusID >= 2, False, True), Boolean) And mDentBuckle.DentBuckleItems.Count = 0) Or (mDentBuckle.DentBuckleItems.Count = 0)
			txtRevDate.Enabled = CType(IIf(mDentBuckle.StatusID >= 2, False, True), Boolean)
			btnAuthorized.Visible = (Not mDentBuckle.DentBuckleItems.Count = 0) And (Not mDentBuckle.IsNew) And (mDentBuckle.StatusID = 1)
			btnSave.Visible = IIf(mDentBuckle.StatusID > 1, False, True)

			If Not User.IsInRole($"Dent&RepairChartAuthorized") Then
				btnAuthorized.Enabled = False
				btnAuthorized.ToolTip = "You are not an Authorized User."
			End If

			btnSelectFile.Disabled = IIf(mDentBuckle.StatusID >= 2, True, False)
			dgItems.Columns(8).Visible = IIf(mDentBuckle.StatusID > 1, False, True)
			btnAdd.Enabled = IIf(mDentBuckle.StatusID > 1, False, True)
			lnkViewChart.Enabled = IIf(mDentBuckle.MachineID.Equals(Guid.Empty), False, True)

			CreatedByUpdatedByDetails.Visible = IIf(mDentBuckle.LastUpdatedBy <> "" Or mDentBuckle.CreatedBy <> "", True, False)

			ControlVisibilityForAttachment()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibilityForAttachment()

		Try

			If mDentBuckle.IsAttachmentAdded Then
				ImageButton1.Visible = True
				btnDelAttach.Enabled = IIf(mDentBuckle.StatusID >= 2, False, True)
			Else
				ImageButton1.Visible = False
				btnDelAttach.Enabled = False
			End If

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
						ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
					End Try

				Else

					If (Not mDentBuckle.IsNew) And IsAttachmentDeleted Then
						FileAttach.DeleteAttachment(mFileAttach.ID, mDentBuckle.ID)
					End If

					IsAttachmentDeleted = False
					Session("IsAttachmentDeleted") = IsAttachmentDeleted

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ViewImage()

		Try

			If mDentBuckle.IsAttachmentAdded And mFileAttach Is Nothing Then
				mFileAttach = FileAttach.GetAttachment(ReferenceID:=mDentBuckle.ID)
			End If

			AttachmentHelper.DownloadAttachmentWithName(AttachmentObject:=mFileAttach)

			ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetTitle()

		Try

			Dim mTransTypeList As TransactionList = TransactionList.GetTransactionList()

			If mDentBuckle.IsNew Then
				lblTitle.Text = $"Dent & Repair Chart [ NEW ]"
			Else
				lblTitle.Text = $"Dent & Repair Chart [ {mDentBuckle.Text} - {CType(mDentBuckle.No, String)} ]"
			End If

			mModuleName = "Dent&RepairChart"
			Session("mModuleName") = mModuleName

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Function Save() As Boolean

		'Authentication
		If mDentBuckle.ReportDate IsNot DBNull.Value Then

			Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))

			If mCheck.WebAuthentication = True Then

				Dim mDays As Integer = 0
				mDays = mCheck.Number("Days")
				Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)

				If DateDiff(DateInterval.Day, CDate(mDentBuckle.ReportDate), maxAllowableDate) < 0 Then
					ScriptManager.RegisterStartupScript(Me, [GetType], "OpenScript", MessageBox.Show(" Your subscription has been expired. can not save Order. <br> Order Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), False), True)
					Exit Function
				End If

			End If

		End If

		If mMachineNameValueList(mDentBuckle.MachineID).IsReadOnly Then
			MSGBoxCtrl.Show("Alert!", "<b>" & cmbMachineList.SelectedItem.ToString & "</b> is marked <b>ReadOnly</b>", "You cannot save.", MsgBoxStyle.OkOnly, "")
			Exit Function
		End If

		Dim DentBuckleClone As DentBuckle
		DentBuckleClone = mDentBuckle.Clone

		Try

			If Not mDentBuckle.DentBuckleItems.Count = 0 Then

				SetObject()

				mDentBuckle.ApplyEdit()

				If (mDentBuckle.IsNew) And (mDentBuckle.Text = "") Then   'Added by Utkarsh on 14-Nov-2013 for Trans Text Series, 'Check if OrderText is blank then call TransTextSeries UI

					Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(TransTypeID:=mDentBuckle.TransTypeID,
																												 TransDate:=mDentBuckle.ReportDateFormatted.ToString)

					If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And
					   (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mDentBuckle.TransTypeID) = False) Or
					   (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mDentBuckle.TransTypeID) = True AndAlso
					   mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mDentBuckle.TransTypeID).TransText = "")) Then

						Dim str = "<script language='javascript'>openledgersame('wfDentAndRepair_Ajax.aspx');</script>"
						Session("BackPagestr_ForTransSeries") = str
						Session("TransName_ForTransSeries") = "DentAndRepair"
						Session("TransTypeID_ForTransSeries") = mDentBuckle.TransTypeID
						Session("TransDate_ForTransSeries") = mDentBuckle.ReportDateFormatted
						Session("AddTransTextSeries") = "True"

						Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")

					Else

						Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(Prev_TransTextSeries:=mPreviousTransTextSeries)

						If mAutoRenewTransTextSeries.IsRenewed Then

							With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mDentBuckle.TransTypeID)
								mDentBuckle.Text = .TransText
								mDentBuckle.No = .StartingTransNo
							End With

						Else

							Dim str = "<script language='javascript'>openledgersame('wfDentAndRepair_Ajax.aspx');</script>"
							Session("BackPagestr_ForTransSeries") = str
							Session("TransName_ForTransSeries") = "DentAndRepair"
							Session("TransTypeID_ForTransSeries") = mDentBuckle.TransTypeID
							Session("TransDate_ForTransSeries") = mDentBuckle.ReportDateFormatted
							Session("AddTransTextSeries") = "True"

							Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")

						End If

					End If

				End If

				mDentBuckle.Save()
				SaveAttachment()
				'Changed by Utkarsh ON 09-Aug-2012  
				Dim OrderDetail As String = mDentBuckle.TextNo + " Dated : " + mDentBuckle.ReportDateFormatted + " Created By : " & mDentBuckle.UserName 'Added by Saylee on 19-July-2011 

				If mDentBuckle.StatusID = 2 Then
					MarkLog(Action.Authorize, mModuleName, OrderDetail & " Authorized By : " & mDentBuckle.AuthorizedBy, ErrorType.NoError, mDentBuckle.ID, EventLogID)
				Else
					MarkLog(Action.Save, mModuleName, OrderDetail, ErrorType.NoError, mDentBuckle.ID, EventLogID)
				End If

				mDentBuckle.MarkClean()
				lblTitle.Text = "Dent Buckle Chart ( Saved ...)"
				Session("mDentBuckle") = mDentBuckle
				Return True

			Else
				ScriptManager.RegisterStartupScript(Me, [GetType], "OpenScript", MessageBox.Show("Dent Buckle Chart can not be saved without Item.", False), True)
				Exit Function
			End If

		Catch ex As SqlException

			Session("DentBuckleClone") = DentBuckleClone

			If ex.Number = 8114 Or ex.Number = 8115 Then
				MSGBoxCtrl.Show(MSGBox.Message_Title.NumericOverFlow, MSGBox.Message_Text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				Exit Function
			ElseIf ex.Number = 8145 Then
				MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				Exit Function
			ElseIf ex.Number = 2627 Then
				MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate, MSGBox.Message_Text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
				Exit Function
			ElseIf ex.Number = 50000 Then

				If ex.State = 2 Then
					MSGBoxCtrl.Show("Alert!", "Can Not Save ! " + "</br>" + ex.Message, "", MsgBoxStyle.OkOnly, "Status")
					Exit Function
				Else
					MSGBoxCtrl.Show("Alert!", "Can Not Save ! " + "</br>" + ex.Message, "", MsgBoxStyle.OkOnly, "Status")
					Exit Function
				End If

			End If

			mDentBuckle = DentBuckleClone
			Session("mDentBuckle") = mDentBuckle

		Catch ex As Exception
			MSGBoxCtrl.Show(MSGBox.Message_Title.NumericOverFlow, MSGBox.Message_Text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
			Exit Function
		Finally
			DentBuckleClone = Nothing
		End Try

	End Function

	Private Sub UpdatePanel()

		Try

			ControlsDataBind()

			upnlStatusName.Update()
			upnlDetails.Update()
			upnlButtons.Update()

			ControlVisibility()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			mMachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:=mDentBuckle.ReportDateFormatted.ToString, , , , , , ,
																		IsTagRequired:=True,
																		TagText:="(SELECT)")

			Session("mMachineNameValueList") = mMachineNameValueList
			cmbMachineList.DataSource = mMachineNameValueList
			dgItems.DataSource = mDentBuckle.DentBuckleItems
			txtReportDate.Text = mDentBuckle.ReportDateFormatted.ToString
			txtRevDate.Text = mDentBuckle.RevDateFormatted.ToString
			cmbMachineList.SelectedValue = mDentBuckle.MachineID.ToString

			DataBind()
			SetLogDetails()

			If mLogList IsNot Nothing Then

				If mLogList.Contains(mDentBuckle.LogID, "") Then
					cmbLogList.SelectedValue = mDentBuckle.LogID.ToString
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlsDataBind()

		Try

			dgItems.DataBind()

			upnlStatusName.DataBind()
			upnlDetails.DataBind()
			upnlButtons.DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ItemDataGrid()

		Try

			dgItems.DataSource = mDentBuckle.DentBuckleItems

			dgItems.DataBind()
			upnlItems.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)

		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		Try

			If custValidator.ControlToValidate = "txtReportDate" Then

				If txtReportDate.Text.ToString = "" Then
					custValidator.ErrorMessage = "Select Report Date."
					e.IsValid = False
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			GetSession()
			AddAttributes()
			EventLogID = CType(Session("EventLogID"), Guid)

			If Not IsPostBack Then

				If AppSettings("AutoCompleteTransText") <> "True" Then

					If txtText.Enabled Then
						txtText.Focus()
					End If

				End If

				If CType(Session("AddTransTextSeries"), String) = "True" AndAlso
				   (Session("TransText_ForTransSeries") IsNot Nothing) Then

					If mDentBuckle.IsNew Then

						mDentBuckle.Text = Session("TransText_ForTransSeries")
						txtText.Text = mDentBuckle.Text
						Session("mDentBuckle") = mDentBuckle
						Session("AddTransTextSeries") = "False"
						Session.Remove("TransName_ForTransSeries")
						Session.Remove("TransText_ForTransSeries")
						Session.Remove("TransNo_ForTransSeries")

					End If

				End If

				DataFieldBind()
				SetTitle()

				If mDentBuckle.StatusID = 1 And mDentBuckle.IsNew = False Then
					lblStatus.Text = "OPENED"
				End If

				ControlVisibility()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddRecord(sender As Object, e As EventArgs) Handles btnAdd.Click

		Try

			SetObject()

			mDentBuckle.DentBuckleItems.Add(DentBuckleID:=mDentBuckle.ID)
			Session("mDentBuckle") = mDentBuckle

			ScriptManager.RegisterStartupScript(Me, [GetType], "OpenWindow", "OpenItemsWindow();", True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub dgOrderItems_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgItems.RowCommand

		Try

			Dim Index As Integer = CInt(e.CommandArgument)

			Select Case e.CommandName
				Case "EditView"

					Session("Edit") = True
					SetObject()
					mDentBuckle.DentBuckleItems.CurrentIndex = Index - 1
					Session("mDentBuckle") = mDentBuckle
					Session("Edit") = True

					Dim mDentBuckleClone As DentBuckle = mDentBuckle.Clone
					Session("mDentBuckleClone") = mDentBuckleClone

					ScriptManager.RegisterStartupScript(Me, [GetType], "OpenWindow", "OpenItemsWindow();", True)

				Case "DeleteRecord"
					DeleteRecord(Index)
			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SaveRecord(sender As Object, e As EventArgs) Handles btnSave.Click

		Try

			If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
															Action:={Action.[New], Action.Edit},
															MSGBoxCtrl:=MSGBoxCtrl,
															ModuleName:="Dent&RepairChart") Then

				Exit Sub

			End If

			SetObject()

			If IsValid Then

				If Save() Then

					DataFieldBind()
					ControlVisibility()
					upnlStatusName.Update()
					upnlItems.Update()
					upnlDetails.Update()
					upnlButtons.Update()
					SetTitle()
					upnlTitle.Update()

					MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully,
									MSGBox.Message_Text.SavedSuccessFully,
									"",
									MsgBoxStyle.OkOnly,
									"")

				End If

			Else
				upnlValidationsummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnBack.Click

		MarkLog(Action.Close, mModuleName, "", ErrorType.NoError, Guid.Empty, EventLogID)
		Try

			SetObject()

			If mDentBuckle.IsDirty Then
				Session("IsValid") = "True"
				MSGBoxCtrl.Show(MSGBox.Message_Title.CloseConfirm, MSGBox.Message_Text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
			Else

				If mDentBuckle.IsNew Then
					Session.Remove("mDentBuckle")
				End If

				RemoveSession()
				Response.Redirect("Index.aspx")

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnPrint.Click
		PrintWithPDF()
	End Sub

	Public Sub PrintWithPDF()

		Try

			If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
															Action:={Action.Print},
															MSGBoxCtrl:=MSGBoxCtrl,
															ModuleName:="Dent&RepairChart") Then

				Exit Sub

			End If

			Dim dataAdapter As New ObjectAdapter
			Dim crystalReport As Engine.ReportClass
			Dim companyDetail As New CompanyDetail
			Dim dataSet As New dsDentBuckle
			Dim DentBuckle As DentBuckle
			Dim DentBuckleItems As DentBuckleItems

			crystalReport = New crDentnBuckleChart

			DentBuckle = DentBuckle.GetDentBuckle(ID:=DentBuckle.ID)
			DentBuckle = Session("mDentBuckle")

			DentBuckleItems = DentBuckle.DentBuckleItems

			Dim DentBuckleNo As String = $"DBC - {DentBuckle.No} - "
			Dim ReportName As String = "Dent & Repair Chart"

			Dim Report As New ReportData(companyDetail.CompanyName,
										 companyDetail.Address,
										 companyDetail.Tel1,
										 companyDetail.Tel2,
										 companyDetail.Fax,
										 companyDetail.Email,
										 companyDetail.WebSite,
										 ReportName, "", "", "",
										 AppSettings("ClientCode"), "",
										 AppSettings("Product Version"),
										 AppSettings("SINote"), "", "", "",
										 AppSettings("Government Authority"),
										 AppSettings("Logo"))

			Dim companyLogo As rptImage = rptImage.GetImage(dataSet)

			dataAdapter.Fill(dataSet, DentBuckle)
			dataAdapter.Fill(dataSet, DentBuckle.DentBuckleItems)
			dataAdapter.Fill(dataSet, Report)
			dataAdapter.Fill(dataSet, companyLogo)

			crystalReport.SetDataSource(dataSet)

			Session("CrystalReport") = crystalReport

			Dim PDFNo As Integer = 1
			Dim PDFNoChild As Integer = 1
			Dim Number As Integer
			Dim RandomNumber As New Random

			Number = RandomNumber.Next

			Dim MyFile = $"C:\Temp\{DentBuckleNo}{Number}{PDFNo}.pdf"

			crystalReport = CType(Session("CrystalReport"), Engine.ReportClass)

			Dim myExportOption As ExportOptions
			Dim myDiskOption As DiskFileDestinationOptions

			myDiskOption = New DiskFileDestinationOptions With {
				.DiskFileName = MyFile
			}
			myExportOption = crystalReport.ExportOptions

			With myExportOption
				.DestinationOptions = myDiskOption
				.ExportDestinationType = ExportDestinationType.DiskFile
				.ExportFormatType = ExportFormatType.PortableDocFormat
			End With

			crystalReport.Export()
			crystalReport.Close()
			crystalReport.Dispose()
			GC.Collect()

			Dim pageCount As Integer = 0

			Dim pdfList As New ArrayList From {
				MyFile
			}
			PDFNo += 1

			'Attachment
			Dim FileAttachment As FileAttach
			FileAttachment = FileAttach.GetAttachment(ReferenceID:=DentBuckle.ID)

			If FileAttachment.Size > 0 And FileAttachment.Extension = ".pdf" Then

				Dim FileAttachmentPath As String = $"C:\Temp\{DentBuckleNo}{PDFNoChild}{FileAttachment.Extension}"
				Dim fs As FileStream

				If File.Exists("C:\Temp\") = False Then

					File.Delete(FileAttachmentPath)

					fs = File.Create(FileAttachmentPath)
					fs.Write(FileAttachment.ImageFile, 0, FileAttachment.ImageFile.Length)
					fs.Close()

					pdfList.Add(FileAttachmentPath)                               '2. TaskCardAttachment attachment

					PDFNo += 1
					PDFNoChild += 1

				End If

			End If

			FileAttachment = Nothing

			' //********************************************Send Files for Merging****************************************************//
			Dim MergedPath As String = $"C:\Temp\temp_myMergedPdf.pdf"
			Dim MergedPath_WM As String = $"C:\Temp\temp_myMergedPdf_WM.pdf"

			Dim filesByte As New List(Of Byte())()
			For Each file__1 As String In pdfList 'files
				filesByte.Add(File.ReadAllBytes(file__1))
			Next

			File.WriteAllBytes(MergedPath, PDFMergers.MergeFiles(filesByte))

			AddWatermarkText(MergedPath, MergedPath_WM, DentBuckle.No, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount)

			''//********************************************Set Sessions*********************************************************//
			Session("CrystalReport") = MergedPath_WM
			Session("PrintReportWithAttachment") = "True"
			'//*******************************************Delete created file*********************************************************//

			Dim DeleteThis As String = DentBuckleNo
			Dim Files As String() = Directory.GetFiles("C:\Temp\")

			For Each currentFile As String In Files

				If currentFile.ToUpper().Contains(DeleteThis.ToUpper()) Then
					File.Delete(currentFile)
				End If

			Next
			'End

			ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Protected Sub ReportDateChanged(sender As Object, e As EventArgs)

		Try

			mDentBuckle = Session("mDentBuckle")
			mDentBuckle.ReportDate = txtReportDate.Text
			txtText.Text = mDentBuckle.Text
			Session("mDentBuckle") = mDentBuckle
			SetLogDetails()
			cmbLogList.SelectedValue = mDentBuckle.LogID.ToString

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub 'End

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub

	Private Sub HdnBtnFileUpload_Click(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click

		mDentBuckle.IsAttachmentAdded = True
		ControlVisibilityForAttachment()
		upnlFileupload.Update()

	End Sub

	Private Sub RemoveAttachment(sender As Object, e As EventArgs) Handles btnDelAttach.Click

		Dim fileSize1 As Integer = 0
		Dim file1(fileSize1) As Byte

		If mDentBuckle.IsAttachmentAdded And mFileAttach Is Nothing Then
			mFileAttach = FileAttach.GetAttachment(ReferenceID:=mDentBuckle.ID)
		End If

		mFileAttach.ImageFile = file1
		mFileAttach.Size = 0

		ImageButton1.Visible = False
		btnDelAttach.Enabled = False
		IsAttachmentDeleted = True
		mDentBuckle.IsAttachmentAdded = False
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
		Session("mFileAttach") = mFileAttach
		Session("mDentBuckle") = mDentBuckle

	End Sub

	Private Sub ViewAttachment(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
		ViewImage()
	End Sub

	Private Sub SelectFile(sender As Object, e As EventArgs) Handles btnSelectFile.ServerClick

		Try

			If mDentBuckle.IsAttachmentAdded Then
				mFileAttach = FileAttach.GetAttachment(mDentBuckle.ID)
			Else

				If IsAttachmentDeleted Then

					If (Not mDentBuckle.IsNew) Then

						mFileAttach = FileAttach.GetAttachment(mDentBuckle.ID)

						If mFileAttach IsNot Nothing Then

							Dim fileSize1 As Integer = 0
							Dim file1(fileSize1) As Byte

							mFileAttach.ImageFile = file1
							mFileAttach.Size = 0

							GoTo CodeBlock

						End If

					End If

				End If
				mFileAttach = FileAttach.NewAttachment(ID:=Guid.NewGuid, ReferenceID:=mDentBuckle.ID)
			End If

CodeBlock:
			Session("mFileAttach") = mFileAttach

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub HdnImgBtnItems_Click(sender As Object, e As EventArgs) Handles hdnimgBtnItems.Click
		ItemDataGrid()
		upnlDetails.Update()
	End Sub

	Private Sub MachineChanged(sender As Object, e As EventArgs) Handles cmbMachineList.SelectedIndexChanged
		SetLogDetails()
		ControlVisibility()
	End Sub

	Private Sub ViewChart(sender As Object, e As EventArgs) Handles lnkViewChart.Click

		Try

			Dim FileAttach As FileAttach
			Dim mMachine As Machine
			mMachine = Machine.GetMachine(MachineID:=New Guid(cmbMachineList.SelectedValue))

			If mMachine.IsAttachmentAddedForDentBuckleChart Then
				FileAttach = FileAttach.GetAttachment(ReferenceID:=mDentBuckle.MachineID)
			Else

				MSGBoxCtrl.Show("Dent & Repair Chart Missing",
								"The Dent & Repair Chart has not been added for this specific model in the Aircraft Master system",
								"",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			End If

			AttachmentHelper.DownloadAttachmentWithName(AttachmentObject:=FileAttach)

			ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Status "

	Private Sub Authorize(sender As Object, e As EventArgs) Handles btnAuthorized.Click

		Try

			If IsValid Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.StatusAuthorized,
								MSGBox.Message_Text.StatusAuthorized,
								"<Strong> Dent & Repair Chart </Strong>",
								MsgBoxStyle.YesNo,
								"Status")

				Session("IsValid") = IsValid
				Session("mDentBuckle") = mDentBuckle

			Else
				upnlValidationsummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Broken Rule(s) "

	Public Sub ObjectValidation(s As Object, e As ServerValidateEventArgs)

		If Flag = 1 Then Exit Sub

		Dim strMsg As String = ""
		Dim CustValidator As CustomValidator
		CustValidator = CType(s, CustomValidator)
		Try

			SetObject()

			If Not mDentBuckle.IsValid Then

				For i As Integer = 0 To mDentBuckle.GetBrokenRulesCollection.Count - 1
					strMsg += mDentBuckle.GetBrokenRulesCollection(i).Description + "<Br>"
				Next

			End If

			Dim mOrderItem As OrderItem

			If Not mDentBuckle.DentBuckleItems.IsValid Then

				For Each mOrderItem In mDentBuckle.DentBuckleItems

					For i As Integer = 0 To mOrderItem.GetBrokenRulesCollection.Count - 1
						strMsg += $"{mOrderItem.ItemName} : {mOrderItem.GetBrokenRulesCollection(i).Description} <Br>"
					Next

				Next

			End If

			If strMsg.Trim <> "" Then
				CustValidator.ErrorMessage = strMsg
				e.IsValid = False
			End If

			Flag = 1

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Service Method(s) "

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetDistinctTextListAutoComplete(prefixText As String, count As Integer, contextKey As String) As String()

		Dim mDistinctTextAutoComplete As DistinctTextListAutoComplete
		Try

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

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class