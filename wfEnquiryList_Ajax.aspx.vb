'***********************************
'AJAX Conversion By Vikrant On 27-Jun-2014
'***********************************

Public Class EnquiryListPage
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

	End Enum

#End Region

#Region " Variable Declaration "

	Public Enquiry As Enquiry
	Public EnquiryList As EnquiryList
	Public ReportHelper As New ReportHelper
	Public TransactionListCount As TransactionListCount  'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
	Public RedirectToNewUIHelper As New RedirectToNewUIHelper
	Public DistinctTextListForEnquiry As DistinctTextListForEnquiry

	Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, EnquiryText, Name, No, VendorNo, VendorName As String
	Dim mModuleName As String
	Dim mTransTypeID As Trans
	Dim EventLogID As Guid 'Added By Utkarsh On 20-Jul-2011 For All19072011
	Dim EnquiryDetail As String 'Added By Utkarsh On 20-Jul-2011 For All19072011
	Dim SearchStr1 As String
	Dim SearchStr2 As String

#End Region

#Region " Business Methods "

	Private Sub GetSession()

		Enquiry = Session("mEnquiry")
		EnquiryList = Session("EnquiryList")
		DistinctTextListForEnquiry = Session("mDistinctTextListForEnquiry")
		SearchIndex = Session("SearchIndexEnq")
		DateIndex = Session("DateIndex")
		FromDate = Session("FromDate")
		ToDate = Session("ToDate")
		StatusId = Session("StatusId")
		EnquiryText = Session("EnquiryText")
		Name = Session("Name")
		No = IIf(IsNothing(Session("No")), 0, Session("No"))
		VendorNo = Session("VendorNo")
		TransactionListCount = Session("TransactionListCount") 'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
		mTransTypeID = Session("mTransTypeId")
		mModuleName = Session("mModuleName")
		VendorName = Session("VendorName")

	End Sub

	Private Sub SetSession()

		Session("mEnquiry") = Enquiry
		Session("EnquiryList") = EnquiryList
		Session("mDistinctTextListForEnquiry") = DistinctTextListForEnquiry
		Session("mTransTypeId") = mTransTypeID
		Session("SearchIndexEnq") = SearchIndex
		Session("DateIndex") = DateIndex
		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("StatusId") = StatusId
		Session("EnquiryText") = EnquiryText
		Session("Name") = Name
		Session("No") = No
		Session("VendorNo") = VendorNo
		Session("TransactionListCount") = TransactionListCount 'Added By Vikrant On 19-AUg-2013 For ALL16082013-
		Session("VendorName") = VendorName

	End Sub

	Private Sub RemoveSession()

		Session.Remove("Enquiry")
		Session.Remove("EnquiryList")
		Session.Remove("mDistinctTextListForEnquiry")
		Session.Remove("mTransTypeId")
		Session.Remove("VendorName")
		Session.Remove("TransactionListCount")
		Session("MiddleFrame") = ""

	End Sub

	Private Sub ClearAll()

		mTransTypeID = Session("mTransTypeId")

		If Session("MiddleFrame") <> "wfEnquiryList_Ajax.aspx?TransTypeId=" & mTransTypeID Then

			Session.Remove("Enquiry")
			Session.Remove("EnquiryList")
			Session.Remove("mDistinctTextListForEnquiry")
			Session.Remove("SearchIndexEnq")
			Session.Remove("DateIndex")
			Session.Remove("FromDate")
			Session.Remove("ToDate")
			Session.Remove("StatusId")
			Session.Remove("EnquiryText")
			Session.Remove("Name")
			Session.Remove("No")
			Session.Remove("VendorNo")
			Session.Remove("TransactionListCount") 'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
			Session.Remove("VendorName")

		End If

	End Sub

	Private Sub SetControl()

		Try

			SetPeriod(DateIndex)
			CallFindNow(SearchIndex)

			dgEnqList.DataBind()

			cmbDate.SelectedIndex = DateIndex
			cmbStatus.SelectedValue = StatusId

			If cmbEnquiryText.Items.Contains(New ListItem(EnquiryText)) Then 'Added By Rajnish On 01-01-2008
				cmbEnquiryText.SelectedValue = EnquiryText
			Else
				cmbEnquiryText.SelectedValue = "(ALL)"
			End If

			txtPartNoSearch.Text = Name 'Item Name
			txtEnquiryNo.Text = No
			txtVendorName.Text = VendorName

			ControlVisibility(SearchIndex, DateIndex)
			'=============Added by Saylee on 26th-Dec-2007=======================
			If mTransTypeID = 1 Then
				lblResult.Text = "List of Sales Enquiry as per criteria :" & EnquiryList.Count & " Record(s) found."
			ElseIf (mTransTypeID = 32) Or (mTransTypeID = 34) Or (mTransTypeID = 35) Then
				lblResult.Text = "List of Purchase Enquiry as per criteria :" & EnquiryList.Count & " Record(s) found."
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub NewRecord()

		Try

			Enquiry = Enquiry.NewEnquiry(mTransTypeID)
			Enquiry.Date = Today.Date
			Session("mEnquiry") = Enquiry
			Session("mTransTypeID") = mTransTypeID

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub EditRecord(mId As Guid)

		Try

			Enquiry = Enquiry.GetEnquiry(mId)
			Enquiry.MarkClean()
			Session("mEnquiry") = Enquiry

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DeleteRecord(mId As Guid)

		Try

			MSGBoxCtrl.Show(MSGBox.Message_Title.Delete,
							MSGBox.Message_Text.Delete,
							"",
							MsgBoxStyle.YesNo,
							"Delete")

			Enquiry = Enquiry.GetEnquiry(mId)
			Session("mEnquiry") = Enquiry
			Session("mTransTypeId") = mTransTypeID

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Overloads Sub SetFocus(Control As WebControl)
		Try

			If Control.Enabled = False Or Control.Visible = False Then Exit Sub
			Control.Focus()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MessageBoxResult()

		Dim MsgBoxResult As MsgBoxResult
		Dim msgCount As Integer = 0
		MsgBoxResult = MSGBoxCtrl.Result
		Try

			If MsgBoxResult > 0 Then
				Select Case MsgBoxResult
					Case MsgBoxResult.Yes

						If MSGBoxCtrl.Sender = "Delete" Then

							Dim mSupplierName As String

							Try
								Dim Enquiry As Enquiry
								Session("Sender") = ""
								Enquiry = CType(Session("mEnquiry"), Enquiry)
								mSupplierName = EnquiryList(Enquiry.ID).VendorName
								Enquiry.Delete()
								Enquiry.Save()
								DataFieldBind()
								SetControl()
								SetTitle()
								btnPrint.Enabled = IIf(EnquiryList.Count = 0, False, True)
								upnlGridView.Update()
								upnlActionBtnTop.Update()

							Catch ex As SqlException

								If ex.Number = 8145 Then
									MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
								ElseIf ex.Number = 2627 Then
									MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
								ElseIf ex.Number = 547 Then

									'Changed By Utkarsh On 20-Jul-2011 For All19072011
									SetTitle()
									upnlTitle.Update()
									upnlActionBtnTop.Update()
									EnquiryDetail = Enquiry.EnquiryNo + " Dated : " + Enquiry.DateFormatted + " from " + EnquiryList(Enquiry.ID).VendorName
									MarkLog(Action.Delete, mModuleName, "Can't delete : " & EnquiryDetail & " is Currently in use", ErrorType.NoError, Enquiry.ID, EventLogID)
									'End

									MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete, MSGBox.Message_Text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")

								End If

								DataFieldBind()
								SetControl()

								msgCount = ex.Errors.Count

							Finally

								If msgCount = 0 Then

									'Changed By Utkarsh On 20-Jul-2011 For All19072011
									SetTitle()
									upnlTitle.Update()
									upnlActionBtnTop.Update()
									EnquiryDetail = Enquiry.EnquiryNo + "," + " Dated : " + Enquiry.DateFormatted + "," + " from : " + mSupplierName
									MarkLog(Action.Delete, mModuleName, EnquiryDetail, ErrorType.NoError, Enquiry.ID, EventLogID)
									'End

								End If

							End Try

						End If

					Case MsgBoxResult.No
						Session("Sender") = ""
					Case MsgBoxResult.Ok
						Session("sender") = ""
					Case MsgBoxResult.Ok And Session("sender") = "Authorization"
						Session("sender") = ""
				End Select

			ElseIf MsgBoxResult = -1 Then
				Session("sender") = ""
			ElseIf MsgBoxResult = 0 And Session("sender") = "Authorization" Then
				Session("sender") = ""
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub FindNow(Optional ItemName As String = "",
						Optional Text As String = "",
						Optional No As Integer = 0,
						Optional Amend As String = "",
						Optional IntEnquiryNo As String = "",
						Optional FromDate As String = "1/1/1900",
						Optional ToDate As String = "1/1/2200",
						Optional StatusID As Integer = 0,
						Optional VendorName As String = "",
						Optional VendorNo As String = "")

		Try

			EnquiryList = Nothing
			dgEnqList.DataSource = Nothing

			EnquiryList = EnquiryList.GetEnquiryList(ItemName:=ItemName,
													 Text:=Text,
													 No:=No,
													 FromDate:=FromDate,
													 ToDate:=ToDate,
													 StatusID:=StatusID,
													 VendorName:=VendorName,
													 TransTypeID:=mTransTypeID,
													 VendorNo:=VendorNo)

			Session("EnquiryList") = EnquiryList
			dgEnqList.DataSource = EnquiryList
			lblResult.Text = "List of Enquiries as per criteria :" & EnquiryList.Count & " Record(s) found."

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CallFindNow(Index As Integer)

		Try

			FindNow(ItemName:=Trim(Name),
					Text:=Trim(EnquiryText),
					No:=CInt(Val(No)),
					FromDate:=txtFromDate.Text.Trim,
					ToDate:=txtToDate.Text.Trim,
					StatusID:=CInt(StatusId),
					VendorName:=Trim(VendorName))


			dgEnqList.PageIndex = 0                 'Added Code on May,2007

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetPeriod(Index As Int32)

		Try

			Select Case Index
				Case 0 ' All   
					txtFromDate.Text = CDate("1-1-1900").ToString(AppSettings("DateFormat"))
					txtToDate.Text = CDate("1-1-2200").ToString(AppSettings("DateFormat"))
				Case 1 'Last 1 Week
					txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
					txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
				Case 2 'Last 1 Month
					txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
					txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
				Case 3 'Last 1 Quarter

					Select Case Today.Month
						Case 1, 2, 3
							txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
							txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
						Case 4, 5, 6
							txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
							txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
						Case 7, 8, 9
							txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
							txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
						Case 10, 11, 12
							txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
							txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
					End Select

				Case 4 'Last 1 Year
					txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat"))
					txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
				Case 5 'Current Financial Year

					If Today.Month <= 3 Then  'Jan|Feb|Mar
						txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
					Else
						txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))   '31-Mar-2006
					End If

					txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))

				Case 6 'Between Dates
					FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date) 'Changes by Prashant on 09-01-2008
					ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date) 'Changes by Prashant on 09-01-2008
					txtFromDate.Text = CDate(FromDate).ToString(AppSettings("DateFormat"))
					txtToDate.Text = CDate(ToDate).ToString(AppSettings("DateFormat"))
			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibility(SearchIndex As Int32, Optional DateIndex As Int32 = 0)

		Try

			lblFrom.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
			lblTo.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)

			If DateIndex = 6 Then
				txtFromDate.Visible = True
				txtToDate.Visible = True
				txtFromDate.Enabled = True
				txtToDate.Enabled = True
			ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
				txtFromDate.Visible = True
				txtToDate.Visible = True
				txtFromDate.Enabled = False
				txtToDate.Enabled = False
			Else
				txtFromDate.Visible = False
				txtToDate.Visible = False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ClearControls()

		Try

			txtEnquiryNo.Text = ""
			txtPartNoSearch.Text = ""

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetVariables()

		Try

			DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
			FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
			ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
			StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
			EnquiryText = IIf(cmbEnquiryText.SelectedIndex <= 0, "", cmbEnquiryText.SelectedValue)
			Name = txtPartNoSearch.Text.Trim
			No = txtEnquiryNo.Text.Trim
			VendorName = txtVendorName.Text.Trim
			Session("FromDate") = FromDate
			Session("ToDate") = ToDate
			Session("SearchIndexEnq") = SearchIndex
			Session("DateIndex") = DateIndex
			Session("StatusId") = StatusId
			Session("EnquiryText") = EnquiryText
			Session("No") = No
			Session("Name") = Name
			Session("VendorNo") = VendorNo
			Session("VendorName") = VendorName

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddAttributes()
		txtEnquiryNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtEnquiryNo').value,event)")
	End Sub

	Private Sub SetTitle()

		Try

			Dim mTransTypeList As TransactionList
			mTransTypeList = TransactionList.GetTransactionList()

			btnAddNew.ToolTip = " Add New " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
			btnClose.ToolTip = " Close list of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString + " screen"
			btnPrint.ToolTip = " Print list of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString

			mModuleName = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
			Session("mModuleName") = mModuleName
			lblEnquiryList.Text = "List of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString   'shweta

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Function IsInRole(CheckFor As Rights) As Boolean

		Dim IsInRoleString As String = ""
		Try

			'Deciding IsInRole String to check Rights
			Select Case mTransTypeID
				Case Trans.Enquiry
					IsInRoleString = "Enquiry"
				Case Trans.RequestingForQuotation
					IsInRoleString = "RequestingForQuotation"
				Case Trans.OverHaulRepairEnquiry
					IsInRoleString = "PurchaseEnquiryRepairOverHaul"
				Case Trans.RentialLeaseEnquiry
					IsInRoleString = "PurchaseEnquiryRentalLease"
			End Select

			'Depending upon decided IsInRole String; checking Rights of the User
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
			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Sub FillCombo() ' Rajnish On 18-12-2007

		Try

			If mTransTypeID = 1 Then
				dgEnqList.Columns(3).HeaderText = "Customer"
			ElseIf (mTransTypeID = 32) Or (mTransTypeID = 34) Or (mTransTypeID = 35) Then
				dgEnqList.Columns(3).HeaderText = "Supplier"
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
			ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
			SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
			DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
			StatusId = Session("StatusId")
			EnquiryText = Session("EnquiryText")
			Name = Session("Name")
			VendorNo = Session("VendorNo")
			DistinctTextListForEnquiry = DistinctTextListForEnquiry.GetDistinctTextList("7", , True, "(ALL)")
			cmbEnquiryText.DataSource = DistinctTextListForEnquiry
			TransactionListCount = TransactionListCount.GetTransactionListCountt(mTransTypeID)

			Session("TransactionListCount") = TransactionListCount

			DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		ClearAll()
		GetSession()
		AddAttributes()

		EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 20-Jul-2011 For All19072011
		Try

			If Not IsPostBack And Session("sender") = "" Then

				If cmbDate.Enabled = True Then
					SetFocus(cmbDate)
				End If

				mTransTypeID = Request.QueryString("TransTypeId")
				Session("mTransTypeId") = mTransTypeID
				Session("MiddleFrame") = "wfEnquiryList_Ajax.aspx?TransTypeId=" & mTransTypeID

				FillCombo()  ' Rajnish On 18-12-2007
				DataFieldBind()
				SetControl()
				SetTitle()
				SetSession()

				BtnPrint.Enabled = IIf(EnquiryList.Count = 0, False, True)

				If Session("RFQCreatedFromNewApplication") Is Nothing Then

					If CBool(AppSettings("NewUi")) Then
						CreateRFQFromNewApplication(sender:=sender, e:=e)
						Session("RFQCreatedFromNewApplication") = True
					End If

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_EnquiryList_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgEnqList.RowCommand

		Dim Index As Integer
		Dim mID As Guid

		Try

			Select Case e.CommandName

				Case "EditRec"

					dgEnqList.DataSource = EnquiryList
					dgEnqList.DataBind()

					Index = CInt(e.CommandArgument)
					mID = EnquiryList(Index).ID

					If (Not IsInRole(Rights.Edit) And Not IsInRole(Rights.View)) Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
										MSGBox.Message_Text.Authorization,
										"",
										MsgBoxStyle.OkOnly,
										"")

						Exit Sub

					End If

					EditRecord(mID)
					'Changed By Utkarsh On 20-Jul-2011 For All19072011
					EnquiryDetail = Enquiry.EnquiryNo + " Dated : " + Enquiry.DateFormatted + " from " + EnquiryList(Enquiry.ID).VendorName

					MarkLog(Action.Edit,
							mModuleName,
							EnquiryDetail,
							ErrorType.NoError,
							Enquiry.ID,
							EventLogID)
					'End
					Dim str As String
					str = "openledgersame('wfEnquiry_Ajax.aspx?BackPage=index.aspx');"
					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"OpenScript",
														str,
														True)

				Case "DeleteRec"

					dgEnqList.DataSource = EnquiryList
					dgEnqList.DataBind()

					Index = CInt(e.CommandArgument)
					mID = EnquiryList(Index).ID

					If Not IsInRole(Rights.Delete) Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
										MSGBox.Message_Text.Authorization,
										"",
										MsgBoxStyle.OkOnly,
										"")
						Exit Sub

					End If

					DeleteRecord(mID)

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_EnquiryList_PageIndexChanging(source As Object, e As GridViewPageEventArgs) Handles dgEnqList.PageIndexChanging

		Try

			dgEnqList.PageIndex = e.NewPageIndex
			dgEnqList.DataSource = EnquiryList
			Session("EnquiryList") = EnquiryList
			dgEnqList.DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_EnquiryList_Sorting(source As Object, e As GridViewSortEventArgs) Handles dgEnqList.Sorting

		Try

			EnquiryList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
			Session("EnquiryList") = EnquiryList
			dgEnqList.DataSource = EnquiryList
			dgEnqList.DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DateChanged(sender As Object, e As EventArgs) Handles cmbDate.SelectedIndexChanged, cmbEnquiryText.SelectedIndexChanged

		Try

			If sender.ID = "cmbDate" Then

				Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)

				ControlVisibility(1, DateIndex)
				SetPeriod(DateIndex)

				If cmbDate.Enabled = True Then
					SetFocus(cmbDate)
				End If

			ElseIf sender.ID = "cmbEnquiryText" Then

				txtEnquiryNo.Text = "0"

				If cmbEnquiryText.Enabled = True Then
					SetFocus(cmbEnquiryText)
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SearchRecords(sender As Object, e As EventArgs) Handles btnFindNow.Click

		Try

			SetVariables()
			CallFindNow(SearchIndex)

			dgEnqList.DataBind()

			BtnPrint.Enabled = IIf(EnquiryList.Count = 0, False, True)
			lblResult.Text = "List of Enquiry as per criteria :" & EnquiryList.Count & " Record(s) found."

			upnlGridView.Update()
			upnlActionBtnTop.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddRecord(sender As Object, e As EventArgs) Handles btnAddNew.Click

		Try

			NewRecord()

			If Not IsInRole(Rights.[New]) Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			End If

			'Changed By Utkarsh On 20-Jul-2011 For All19072011
			MarkLog(Action.[New],
					mModuleName,
					"",
					ErrorType.NoError,
					Enquiry.ID,
					EventLogID)

			Dim str As String
			str = "openledgersame('wfEnquiry_Ajax.aspx?BackPage=wfEnquiryList_Ajax.aspx');"
			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Open Script",
												str,
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

		Try

			RemoveSession()
			Response.Redirect("Dashboard.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

	Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnPrint.Click

		Dim CurrentPageIndex As Integer = Me.dgEnqList.PageIndex
		Dim ColumnsCount As Integer = dgEnqList.Columns.Count - 1
		Dim ColumnHeaders(ColumnsCount) As String

		Try

			If Not IsInRole(Rights.Print) Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			End If

			If EnquiryList.Count = 0 Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.NoRecordFound,
								MSGBox.Message_Text.NoRecordFound,
								"There are no records for this Criteria",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			End If

			For i As Integer = 0 To ColumnsCount
				ColumnHeaders(i) = dgEnqList.Columns.Item(i).HeaderText
			Next

			Dim Result = ReportHelper.ListReport(List:=EnquiryList,
												 ColumnHeaders:=ColumnHeaders,
												 IsForAPI:=False,
												 ReportOf:="EnquiryList")

			Session("CrystalReport") = CType(Result.Item1, Engine.ReportClass)

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"openTranDetail",
												"openTranDetail();",
												True)

			Me.dgEnqList.PageIndex = CurrentPageIndex
			Me.dgEnqList.DataSource = EnquiryList

			Session("EnquiryList") = EnquiryList
			dgEnqList.DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CreateRFQFromNewApplication(sender As Object, e As EventArgs) Handles btnCheckoutNewApplication.Click

		Try

			Dim NewUrl As String = RedirectToNewUIHelper.NavigationLinkForNewUI(Request:=Request,
																				 NavigationLink:="Procurement?tab=rfqs")

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Open in New Tab",
												$"window.open('{NewUrl}', '_blank');",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class