Imports System.Linq
Imports System.Linq.Enumerable


Public Class PendingToolsToReceiveFromEmployeeDetailPage
	Inherits Page

#Region " Variable Declaration "

	Public mEmployeeStatus As EmployeeStatus
	Public mnWOListForCombo As nWOListForCombo
	Public mReceiptCumInvoice As ReceiptCumInvoice
	Public mLastWarrantyInformation As LastWarrantyInformation
	Public mPendingToolsToReceiveFromEmployee As PendingToolsToReceiveFromEmployee

#End Region

#Region " Helper Method(s) "

	Private Sub GetSession()
		mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
		mPendingToolsToReceiveFromEmployee = CType(Session("mPendingToolsToReceiveFromEmployee"), PendingToolsToReceiveFromEmployee)
		mnWOListForCombo = Session("mnWOListForCombo")
	End Sub

	Private Sub RemoveSession()
		Session.Remove("mPendingToolsToReceiveFromEmployee")
		Session.Remove("mnWOListForCombo")
	End Sub

	Private Sub SetObject(Index As Int32)

		Try

			If mReceiptCumInvoice IsNot Nothing Then

				If txtDate.Text.ToString = "" Then
					mReceiptCumInvoice.RecCumInvDate = Today.Date
				Else
					mReceiptCumInvoice.RecCumInvDate = txtDate.Text
				End If

				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 19 'From Employee
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID = mPendingToolsToReceiveFromEmployee(Index).ItemID
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Part = mPendingToolsToReceiveFromEmployee(Index).ItemName
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartDescription = mPendingToolsToReceiveFromEmployee(Index).Description
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mPendingToolsToReceiveFromEmployee(Index).FromStoreID
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreName = mPendingToolsToReceiveFromEmployee(Index).FromStoreName
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToolsToReceiveFromEmployee(Index).UnitID
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPendingToolsToReceiveFromEmployee(Index).IssueItemID
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = 1
				'Added By Prashant 1-Apr-2019 ALL01042019 
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mPendingToolsToReceiveFromEmployee(Index).EffRate                 'Assigned EffRate here because ReceiptItem may be in diffrent currency and other charges
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CEffRate = mPendingToolsToReceiveFromEmployee(Index).EffRate              'Assigned EffRate here because ReceiptItem may be in diffrent currency and other charges
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CCommercialRate = mPendingToolsToReceiveFromEmployee(Index).EffRate       'Assigned EffRate here because ReceiptItem may be in diffrent currency and other charges
				'End Of Added By Prashant 1-Apr-2019 ALL01042019 
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeID = mPendingToolsToReceiveFromEmployee(Index).ItemTypeID
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo = mPendingToolsToReceiveFromEmployee(Index).SerialNo
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo = mPendingToolsToReceiveFromEmployee(Index).SerialNo
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteNo = mPendingToolsToReceiveFromEmployee(Index).ReleaseNoteNo
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CodeNo = mPendingToolsToReceiveFromEmployee(Index).CodeNo

				If mPendingToolsToReceiveFromEmployee(Index).CalibrationDoneOnDateFormatted.ToString <> "" Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CalibrationDoneOnDate = mPendingToolsToReceiveFromEmployee(Index).CalibrationDoneOnDateFormatted.ToString
				Else
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CalibrationDoneOnDate = DBNull.Value
				End If

				mLastWarrantyInformation = LastWarrantyInformation.GetLastWarrantyInformation(mPendingToolsToReceiveFromEmployee(Index).ItemID.ToString, mPendingToolsToReceiveFromEmployee(Index).SerialNo)

				If mLastWarrantyInformation.Count > 0 Then

					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CodeNo = mLastWarrantyInformation(0).CodeNo
					Dim CalDate As String = mLastWarrantyInformation(0).LastCalibrationDoneOnDateFormatted.ToString

					If CalDate <> "" Then
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CalibrationDoneOnDate = mLastWarrantyInformation(0).LastCalibrationDoneOnDate
					Else
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CalibrationDoneOnDate = DBNull.Value
					End If

					mLastWarrantyInformation = Nothing

				End If

				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate = mPendingToolsToReceiveFromEmployee(Index).ExpiryDate
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsExpiryNA = mPendingToolsToReceiveFromEmployee(Index).IsExpiryNA
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsExpiryUnlimited = mPendingToolsToReceiveFromEmployee(Index).IsExpiryUnlimited
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate = mPendingToolsToReceiveFromEmployee(Index).WarrantyExpiryDate
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = mPendingToolsToReceiveFromEmployee(Index).StartDate
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpQtrs = mPendingToolsToReceiveFromEmployee(Index).ExpiryQuarter
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpYear = mPendingToolsToReceiveFromEmployee(Index).ExpiryYear
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureQtrs = mPendingToolsToReceiveFromEmployee(Index).CurrentQuarter
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureYear = mPendingToolsToReceiveFromEmployee(Index).CurrentYear

				Session("mReceiptCumInvoice") = mReceiptCumInvoice

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MessageBoxResult()

		Dim MsgBoxResult As MsgBoxResult
		MsgBoxResult = MSGBoxCtrl.Result
		Try

			If MsgBoxResult > 0 Then

				Select Case MsgBoxResult
					Case MsgBoxResult.Ok

						If MSGBoxCtrl.Sender = "ResetIssuedToEmployee" Then

							txtIssuedToEmployee.Text = ""
							txtIssuedToEmployee.DataBind()
							hdnIssuedToEmployeeId.Value = ""
							upnlDetails.Update()

						End If

				End Select

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibility()

		Try

			txtDate.Enabled = IIf(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count > 1, False, True)
			lblCodeNo.Text = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo", "Code No.", "GSE No.")
			txtCodeNo.ToolTip = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo", "Enter Code No.", "Enter GSE No.")
			dgPartList.Columns(11).Visible = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", True, False)
			lblCodeNo.Visible = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", True, False)
			txtCodeNo.Visible = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", True, False)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Data Binding "

	Private Sub SetCombo()
		mnWOListForCombo = nWOListForCombo.GetnWOListForCombo("(SELECT)", , , New SmartDate("01-01-1900").FormattedText, New SmartDate(mReceiptCumInvoice.RecCumInvDateFormatted.ToString).FormattedText, , , 2, 1)
		cmbWorkOrder.DataSource = mnWOListForCombo
		cmbWorkOrder.DataBind()
	End Sub

	Private Sub DataFieldBinding(Optional PartNo As String = "", Optional ToDate As String = "", Optional IssueToEmpName As String = "", Optional WOID As String = "{00000000-0000-0000-0000-000000000000}", Optional CodeNo As String = "")
		If mReceiptCumInvoice.ToolsCheckInAgainstID = 1 Then
			mPendingToolsToReceiveFromEmployee = PendingToolsToReceiveFromEmployee.GetPendingTools(txtName.Text, txtDate.Text, txtIssuedToEmployee.Text,
																							  WOID:=cmbWorkOrder.SelectedValue.ToString,
																							  CodeNo:=Trim(txtCodeNo.Text), UserName:=User.Identity.Name,
																							  ToolsCheckInAgainstID:=mReceiptCumInvoice.ToolsCheckInAgainstID)
		ElseIf mReceiptCumInvoice.ToolsCheckInAgainstID = 2 Then
			mPendingToolsToReceiveFromEmployee = PendingToolsToReceiveFromEmployee.GetPendingTools(txtName.Text, txtDate.Text, txtIssuedToEmployee.Text,
																								  WOID:=mReceiptCumInvoice.WOID.ToString,
																								  CodeNo:=Trim(txtCodeNo.Text), UserName:=User.Identity.Name,
																								  ToolsCheckInAgainstID:=mReceiptCumInvoice.ToolsCheckInAgainstID)
		End If

		lblResult.Text = "List of Tools Issued to Employee: " & mPendingToolsToReceiveFromEmployee.Count & " Record(s) Found."
		dgPartList.DataSource = mPendingToolsToReceiveFromEmployee
		Session("mPendingToolsToReceiveFromEmployee") = mPendingToolsToReceiveFromEmployee
		dgPartList.Columns(11).HeaderText = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo", "Code No.", "GSE No.")
		dgPartList.DataBind()
	End Sub

#End Region

#Region " Event(s) "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			GetSession()

			If Not IsPostBack Then

				txtName.Focus()
				txtDate.Text = mReceiptCumInvoice.RecCumInvDateFormatted.ToString

				SetCombo()
				DataFieldBinding(PartNo:=txtName.Text.Trim, ToDate:=txtDate.Text.ToString)
				ControlVisibility()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SearchRecords(sender As Object, e As EventArgs) Handles btnFindNow.Click

		Try

			dgPartList.PageIndex = 0
			DataFieldBinding(txtName.Text.Trim, txtDate.Text.ToString)
			upnlPartList.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_PartList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgPartList.PageIndexChanging

		Try

			dgPartList.PageIndex = e.NewPageIndex
			mPendingToolsToReceiveFromEmployee = Session("mPendingToolsToReceiveFromEmployee")

			DataFieldBinding(PartNo:=txtName.Text.Trim, ToDate:=txtDate.Text.ToString)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_PartList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgPartList.Sorting

		Try

			mPendingToolsToReceiveFromEmployee.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
			Session("mPendingToolsToReceiveFromEmployee") = mPendingToolsToReceiveFromEmployee
			dgPartList.DataSource = mPendingToolsToReceiveFromEmployee
			dgPartList.DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

		Try

			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.Equals(Guid.Empty) Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.Remove(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem)
			End If

			Session("mReceiptCumInvoice") = mReceiptCumInvoice
			RemoveSession()

			If Session("OpenFromCheckInDetailPage") = True Then
				Session("OpenFromCheckInDetailPage") = False
				Response.Redirect(Request.QueryString("ChildPage") & "?&BackPage=" & Request.QueryString("BackPage"))
			Else
				Response.Redirect(Request.QueryString("BackPage"))
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Protected Sub DateChanged(sender As Object, e As EventArgs)

		Try

			dgPartList.PageIndex = 0

			DataFieldBinding(txtName.Text.Trim, txtDate.Text.ToString)
			upnlPartList.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddRecord(sender As Object, e As EventArgs) Handles btnAdd.Click

		Try

			If IsValid Then

				Dim chk As CheckBox
				Dim IsFirstItem As Boolean = True

				For i As Integer = 0 To mPendingToolsToReceiveFromEmployee.Count - 1

					chk = CType(dgPartList.Rows(i).FindControl("chkSelect"), CheckBox)

					If chk.Checked Then

						If mReceiptCumInvoice.ReceiptCumInvoiceItems.Contains(mPendingToolsToReceiveFromEmployee(i).IssueItemID, "") Then '4
							MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate, MSGBox.Message_Text.Duplicate, "Issue Item", MsgBoxStyle.OkOnly, "")
							Exit Sub
						End If

						If IsFirstItem Then
							SetObject(i)
							IsFirstItem = False
						Else
							mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(Guid.NewGuid)
							mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentIndex = mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 1
							SetObject(i)
						End If

					End If

				Next

				If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.Equals(Guid.Empty) Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.Remove(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem)
				End If

				RemoveSession()
				Response.Redirect("wfToolsCheckIn_Ajax.aspx?&BackPage=" & Request.QueryString("BackPage"))

			Else
				upnlValidationSummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddBarcodeItem(sender As Object, e As EventArgs) Handles btnAddBarcodeItem.Click

		Try

			If IsValid Then

				If txtBarcodeItem.Text <> "" Then '--

					Dim mPendingItemList As PendingToolsToReceiveFromEmployee
					mPendingItemList = PendingToolsToReceiveFromEmployee.GetPendingTools(ReceiptDate:=txtDate.Text, BarcodeNo:=txtBarcodeItem.Text.Trim, UserName:=User.Identity.Name)

					If mPendingItemList.Count > 0 Then '3

						If mReceiptCumInvoice.ReceiptCumInvoiceItems.Contains(mPendingItemList(0).IssueItemID, "") Then '4
							MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate, MSGBox.Message_Text.Duplicate, "Issue Item", MsgBoxStyle.OkOnly, "")
							txtBarcodeItem.Text = ""
							Exit Sub
						Else '4
							AddItemByBarcode(mPendingItemList)
						End If '5

					Else

						MSGBoxCtrl.Show("Add alert !", "Tool can not be added <br> Tool not present in Stock or Wrong Employee Name selected", "", MsgBoxStyle.OkOnly, "")
						txtBarcodeItem.Text = ""
						Exit Sub

					End If '4

				Else

					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Invalid Barcode Number.", False), True)
					txtBarcodeItem.Text = ""
					Exit Sub

				End If

			Else
				upnlValidationSummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub AddItemByBarcode(mPendingItemList As PendingToolsToReceiveFromEmployee)

		Try

			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 19 'From Employee
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPendingItemList(0).IssueItemID
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID = mPendingItemList(0).ItemID
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Part = mPendingItemList(0).ItemName
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartDescription = mPendingItemList(0).Description
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mPendingItemList(0).FromStoreID
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreName = mPendingItemList(0).FromStoreName
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingItemList(0).UnitID
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = 1
			'Added By Prashant 1-Apr-2019 ALL01042019 
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mPendingItemList(0).EffRate                 'Assigned EffRate here because ReceiptItem may be in diffrent currency and other charges
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CEffRate = mPendingItemList(0).EffRate              'Assigned EffRate here because ReceiptItem may be in diffrent currency and other charges
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CCommercialRate = mPendingItemList(0).EffRate       'Assigned EffRate here because ReceiptItem may be in diffrent currency and other charges
			'End Of Added By Prashant 1-Apr-2019 ALL01042019 
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeID = mPendingItemList(0).ItemTypeID

			Session("mReceiptCumInvoice") = mReceiptCumInvoice
			RemoveSession()
			Response.Redirect("wfToolsCheckIn_Ajax.aspx?&BackPage=" & Request.QueryString("BackPage"))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Protected Sub IssuedToEmployeeChanged(sender As Object, e As EventArgs)

		Dim message As String = ""
		Try

			If IsNumeric(txtIssuedToEmployee.Text) Then

				Dim mEmployeeListForCombo As EmployeeListForCombo
				mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo(BarcodeNo:=txtIssuedToEmployee.Text)

				If mEmployeeListForCombo.Count > 0 Then

					mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeListForCombo(0).ID.ToString, txtDate.Text)

					If mEmployeeStatus.Count > 0 Then

						If (mEmployeeStatus(0).Information <> "") Then

							message = mEmployeeStatus(0).Information
							MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.Custom, "", MsgBoxStyle.OkOnly, "ResetIssuedToEmployee")
							Exit Sub

						End If

						txtIssuedToEmployee.Text = mEmployeeListForCombo(0).LicenceNoName
						txtIssuedToEmployee.DataBind()

					End If

					Exit Sub

				End If

			End If

			If txtIssuedToEmployee.Text <> "" Then

				Dim Emplist As EmpNoNameAutoComplete = EmpNoNameAutoComplete.GeEmpNoNameList(txtIssuedToEmployee.Text.Split("-")(1).Trim)

				If Emplist.Count <> 0 Then
					mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(Emplist(0).ID.ToString, txtDate.Text)
				End If

				If mEmployeeStatus.Count > 0 Then

					If (mEmployeeStatus(0).Information <> "") Then
						message = mEmployeeStatus(0).Information
						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.Custom, "", MsgBoxStyle.OkOnly, "ResetIssuedToEmployee")
						Exit Sub
					Else
						DataFieldBinding()
						upnlPartList.Update()
					End If

				Else
					txtIssuedToEmployee.Text = ""
				End If

			Else

				txtIssuedToEmployee.Text = ""
				txtIssuedToEmployee.DataBind()
				DataFieldBinding()
				upnlPartList.Update()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub NameChanged(sender As Object, e As EventArgs) Handles txtName.TextChanged

		Try

			DataFieldBinding()
			upnlPartList.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CodeNoChanged(sender As Object, e As EventArgs) Handles txtCodeNo.TextChanged

		Try

			DataFieldBinding()
			upnlPartList.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub WorkOrderChanged(sender As Object, e As EventArgs) Handles cmbWorkOrder.SelectedIndexChanged

		Try

			DataFieldBinding()
			upnlPartList.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Service Method(s) "

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetEmployeeList(prefixText As String, count As Integer, contextKey As String) As String()

		Try

			Dim ItemList As EmpNoNameAutoComplete
			ItemList = EmpNoNameAutoComplete.GeEmpNoNameList(Name:=prefixText)

			If count = 0 Then
				Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In ItemList
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).ToArray
			Else
				Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In ItemList
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).Take(count).ToArray
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class