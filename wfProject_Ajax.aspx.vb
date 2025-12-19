'*************************************************
'Modified by Harsh Sugandhi on 14th Jan 2025 for FLYPAL-2077
'*************************************************


Imports System.Collections.Generic
Imports System.Linq
Imports System.Linq.Enumerable


Public Class ProjectDetails
	Inherits Page


#Region " Variable(s) "

	Public mUser As User
	Public WorkOrder As nWO
	Public Project As Project
	Public FileAttach As FileAttach
	Public ProjectWOList As nWOList
	Public CustomerList As VendorList
	Public ProjectList As ProjectList
	Public EmployeeList As EmployeeList
	Public AircraftList As MachineNameValueList
	Public AssemblyStatusList As AssemblyStatusList
	Public AttachmentHelper As New AttachmentHelper
	Public AuthorizationHelper As New AuthorizationHelper
	Public AssemblyStatusPeriodList As AssemblyStatusPeriodList

	Dim Prefix As String
	Dim EventLogID As Guid
	Dim ProjectDetails As String
	Dim ReportName As String = ""
	Dim mLocationName As String = ""
	Dim CompletedWOCount As Integer = 0
	Dim mAssemblyStatusPeriodInfo = Nothing
	Dim IsAttachmentDeleted As Boolean = False
	Dim Username As String = User.Identity.Name.ToUpper.Trim.ToString

#End Region

#Region " Helper Method(s) "

	Private Sub GetSession()

		Project = CType(Session("mProject"), Project)
		EmployeeList = CType(Session("EmployeeList"), EmployeeList)
		FileAttach = Session("mFileAttachProject")
		IsAttachmentDeleted = Session("IsAttachmentDeleted")
		ProjectList = Session("mProjectList")
		AssemblyStatusPeriodList = Session("AssemblyStatusPeriodListForProject")

	End Sub

	Private Sub SetSession()

		Session("mProject") = Project
		Session("EmployeeList") = EmployeeList
		Session("mFileAttachProject") = FileAttach
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
		Session("AssemblyStatusPeriodListForProject") = AssemblyStatusPeriodList

	End Sub

	Private Sub RemoveSessions()

		Session.Remove("mProject")
		Session.Remove("mEmployeeList")
		Session.Remove("mFileAttach")
		Session.Remove("IsAttachmentDeleted")
		Session.Remove("mFileAttachProject")

	End Sub

	Private Sub AddAttributes()

		txtProjectNo.Attributes.Add("onKeyPress",
									"validateText(('D'),document.getElementById('txtProjectNo').value,event)")

	End Sub

	Private Overloads Sub SetFocus(control As WebControl)

		If control.Enabled = False Or control.Visible = False Then Exit Sub
		control.Focus()

	End Sub

	Private Sub SetTitle()

		Try

			If Project.IsNew Then
				lblTitle.Text = $"{Prefix}  [New]"
			Else
				lblTitle.Text = $"{Prefix}  [ {Project.Text} - {CType(Project.No, String)} ]"
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetObject()

		Try

			If txtProjectDate.Text = "" Then
				Project.ProjectDate = Today.Date
			Else
				Project.ProjectDate = CDate(txtProjectDate.Text)
			End If

			Project.Text = txtProjectText.Text.Trim
			Project.No = Val(txtProjectNo.Text)
			Project.Description = Trim(txtDescription.Text)
			Project.ReceivingPersonID = New Guid(cmbEmployee.SelectedValue)
			Project.CustomerID = New Guid(cmbCustomer.SelectedValue)

			If txtReceivingDate.Text = "" Then
				Project.ReceivingDate = DBNull.Value
			Else
				Project.ReceivingDate = CDate(txtReceivingDate.Text)
			End If

			If txtInspectionDate.Text = "" Then
				Project.InspectionDate = DBNull.Value
			Else
				Project.InspectionDate = CDate(txtInspectionDate.Text)
			End If

			Project.Remark = Trim(txtRemark.Text)
			Project.CreatedBy = Username
			Project.IsCustomerContract = chkCustomerContract.Checked
			Project.ModelName = txtModelNo.Text.Trim
			Project.SerialNo = txtSerialNo.Text.Trim
			Project.RegNo = txtRegNo.Text.Trim

			If txtPlanStartDate.Text = "" Then
				Project.PlanStartDate = DBNull.Value
			Else
				Project.PlanStartDate = CDate(txtPlanStartDate.Text)
			End If

			If txtPlanEndDate.Text = "" Then
				Project.PlanEndDate = DBNull.Value
			Else
				Project.PlanEndDate = CDate(txtPlanEndDate.Text)
			End If

			Project.ServiceProviderID = New Guid(DD_ServiceProvider.SelectedValue.ToString)
			Session("mProject") = Project

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub SetWorkOrder(TransTypeID As Integer, WOTransType As Integer)

		Try

			If TransTypeID = 101 Then

				Select Case WOTransType
					Case 88
						WorkOrder = nWO.NewWO(1, 89) 'Un-Scheduled
						WorkOrder.WOJobTypeID = 1
					Case 89
						WorkOrder = nWO.NewWO(2, 89) 'Scheduled
						WorkOrder.WOJobTypeID = 2
					Case 102
						WorkOrder = nWO.NewWO(2, 102) 'EO or AD / SB
						WorkOrder.WOJobTypeID = 2
					Case 108
						WorkOrder = nWO.NewWO(3, 108) 'MEL / Snag
						WorkOrder.WOJobTypeID = 3
					Case 109
						WorkOrder = nWO.NewWO(3, 109) 'Discrepancy
						WorkOrder.WOJobTypeID = 3
					Case 117 'Added by Prashant 9-Oct-2025
						WorkOrder = nWO.NewWO(1, 117) 'Concession Task 
						WorkOrder.WOJobTypeID = 1
				End Select

			ElseIf TransTypeID = 104 Then

				Select Case WOTransType
					Case 111
						WorkOrder = nWO.NewWO(1, 111) 'AMO AMP Task
					Case 113
						WorkOrder = nWO.NewWO(1, 113) 'AMO AD / SB WO
					Case 112
						WorkOrder = nWO.NewWO(1, 112) 'AMO Customer WO
					Case 110
						WorkOrder = nWO.NewWO(1, 110) 'MEL / Snag
					Case 109
						WorkOrder = nWO.NewWO(1, 109) 'Discrepancy
				End Select

				WorkOrder.WOJobTypeID = 1

			End If

			Session("mnWO") = WorkOrder

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibility()

		Try

			If dgWOList.Rows.Count > 0 Then

				txtProjectDate.Enabled = False
				txtProjectText.Enabled = False
				txtProjectNo.Enabled = False
				cmbCustomer.Enabled = False
				chkCustomerContract.Enabled = False
				txtRegNo.Enabled = False
				txtModelNo.Enabled = False
				txtSerialNo.Enabled = False
				cmbAircraftList.Enabled = False

			Else

				txtProjectDate.Enabled = True
				txtProjectText.Enabled = True
				txtProjectNo.Enabled = True
				cmbCustomer.Enabled = True
				chkCustomerContract.Enabled = True
				txtRegNo.Enabled = True
				txtModelNo.Enabled = True
				txtSerialNo.Enabled = True
				cmbAircraftList.Enabled = True

			End If

			If Project.TransTypeID = 101 Then 'CAMO & EO

				cmbAircraftList.Visible = True
				txtRegNo.Visible = False
				txtModelNo.ReadOnly = True
				txtSerialNo.ReadOnly = True

			ElseIf Project.TransTypeID = 104 Then 'AMO

				cmbAircraftList.Visible = False
				txtRegNo.Visible = True
				txtModelNo.ReadOnly = False
				txtSerialNo.ReadOnly = False

			End If

			ProjectWOList = CType(Session("ProjectWOList"), nWOList)

			btnComplete.Visible = (Not Project.IsNew) And
								  (ProjectWOList.Count = CompletedWOCount) And
								  (Project.StatusID = 10)

			DD_ServiceProvider.Enabled = (Not ProjectWOList.Count >= 1)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibilityForFileAttachment()

		Try

			If Project.IsAttachmentAdded Then

				btnViewAttachment.Visible = True
				If Project.StatusID = 1 Then
					btnDelAttach.Enabled = True
				Else
					btnDelAttach.Enabled = False
				End If

			Else

				btnViewAttachment.Visible = False
				btnDelAttach.Enabled = False

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

					Case MsgBoxResult.Yes

						If MSGBoxCtrl.Sender = "Delete" Then

							Try

								Project = CType(Session("mProject"), Project)
								WorkOrder = Session("mnWO")

								WorkOrder.DeleteWO(ID:=WorkOrder.ID,
												   ProjectID:=Project.ID.ToString,
												   AddedWorkOrderFrom:=1)

								ProjectWOList = nWOList.GetWOList(ProjectID:=Project.ID)
								dgWOList.DataSource = ProjectWOList
								dgWOList.DataBind()
								ControlVisibility()
								upnlProjectDetail.Update()
								Session("mProject") = Project
								Session("ProjectWOList") = ProjectWOList

							Catch ex As SqlException

								MSGBoxCtrl.Show(MSGBox.Message_Title.Alert,
												MSGBox.Message_Text.Alert,
												ex.Message,
												MsgBoxStyle.OkOnly,
												"")

								Exit Sub

							End Try

						End If

						If MSGBoxCtrl.Sender = "ProjectCompletion" Or
						   MSGBoxCtrl.Sender = "ProjectSubmission" Then

							Try

								Session("sender") = ""

								If Project.IsValid = True Then

									Project.StatusID = IIf(MSGBoxCtrl.Sender = "ProjectSubmission",
															10,
															3)

									Project.AuthorizedBy = Username
									Save()
									Session.Remove("IsValid")
									upnlValidationsummary.Update()

								Else

									If ObjectValidation() = False Then

										upnlValidationsummary.Update()
										Exit Sub

									End If

								End If

							Catch Exception As Exception

								MSGBoxCtrl.Show(MSGBox.Message_Title.Alert,
												MSGBox.Message_Text.Alert,
												Exception.Message,
												MsgBoxStyle.OkOnly,
												"")

								Exit Sub

							End Try

						End If

						If MSGBoxCtrl.Sender = "Close" Then

							Try

								DataFieldBind()
								Save()

								Session.Remove("IsValid")
								Session.Remove("mModuleName")
								Session.Remove("mPendingItemList")
								Session("Sender") = ""
								Response.Redirect("Index.aspx")


							Catch Exception As Exception

								MSGBoxCtrl.Show(MSGBox.Message_Title.Alert,
												MSGBox.Message_Text.Alert,
												Exception.Message,
												MsgBoxStyle.OkOnly,
												"")

								Exit Sub

							End Try

						End If

					Case MsgBoxResult.No

						If MSGBoxCtrl.Sender = "Close" Then

							Session.Remove("IsValid")
							Session.Remove("mModuleName")
							Session.Remove("mPendingItemList")
							Session("Sender") = ""
							Response.Redirect("Index.aspx")

						End If

					Case MsgBoxResult.Ok

				End Select

			ElseIf MsgBoxResult = 0 And Session("sender") = "Authorization" Then

				Session("sender") = ""
				DataFieldBind()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub Save()

		Try

			'Authorization
			If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
															MSGBoxCtrl:=MSGBoxCtrl,
															ModuleName:=Prefix,
															TransTypeID:=Project.TransTypeID,
															Action:={Action.New, Action.Edit},
															MarkLogDetail:=ProjectDetails,
															IsForSave:=True) Then

				Exit Sub

			End If

			Dim ProjectClone As Project
			ProjectClone = Project.Clone

			Try

				'check whether min. one item & charge is present while saving
				If Not dgWOList.Rows.Count = 0 Then

					'save the object
					SetObject()

					If Project.IsValid Then

						Project.ApplyEdit()
						Project.Save()
						SaveAttachment()
						ProjectDetails = $"{Project.ProjectNumber} 
										  Dated : {Project.ProjectDateFormatted}
										  Customer {Project.CustomerName}"

						MarkLog(Action.Save,
								"Project",
								ProjectDetails,
								ErrorType.NoError,
								Project.ID,
								EventLogID)

						Project.MarkClean()
						Session("mProject") = Project
						DataFieldBind()
						ControlVisibility()
						ControlVisibilityForFileAttachment()
						SetTitle()
						upnlTitle.Update()
						upnlProjectDetails.Update()
						upnlProjectDetail.Update()
						upnlButtons.Update()
						upnlProjectStatus.Update()
						upnlAirframePeriods.Update()

						MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully,
										MSGBox.Message_Text.SavedSuccessFully,
										"",
										MsgBoxStyle.OkOnly,
										"")

					Else

						upnlValidationsummary.Update()

						Project = ProjectClone
						SetObject()
						Session("mProject") = Project
						DataFieldBind()

						Exit Sub

					End If

				Else

					MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
									MSGBox.Message_Text.saveAlert,
									IIf(Project.TransTypeID = 101,
												   "Work-Pack Detail can not be saved without Work Order.",
												   "Project Detail can not be saved without Work Order."),
									MsgBoxStyle.OkOnly,
									"")

					Project = ProjectClone
					SetObject()
					Session("mProject") = Project
					DataFieldBind()
					Exit Sub

				End If

			Catch SqlException As SqlException

				Session("ProjectClone") = ProjectClone

				If SqlException.Number = 8114 Or SqlException.Number = 8115 Then

					MSGBoxCtrl.Show(MSGBox.Message_Title.NumericOverFlow,
									MSGBox.Message_Text.NumericOverFlow,
									"",
									MsgBoxStyle.OkOnly,
									"")

					Exit Sub

				ElseIf SqlException.Number = 8145 Then

					MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
									MSGBox.Message_Text.ProcedureError,
									"",
									MsgBoxStyle.OkOnly,
									"")

					Exit Sub

				ElseIf SqlException.Number = 2627 Then

					MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate,
									MSGBox.Message_Text.Duplicate,
									"",
									MsgBoxStyle.OkOnly,
									"")

					Exit Sub

				End If

			Catch Exception As Exception

				Project = ProjectClone
				Session("mProject") = Project

			Finally
				ProjectClone = Nothing
			End Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			EmployeeList = EmployeeList.GetEmployeeList(Name:="", Designation:="", AddTopItem:="(SELECT)")
			cmbEmployee.DataSource = EmployeeList
			Session("EmployeeList") = EmployeeList

			CustomerList = VendorList.GetVendortList(LookInType:=0, , , , , , IsSelectTagRequired:=True, IsCustomer:=True, , )
			cmbCustomer.DataSource = CustomerList

			ProjectWOList = nWOList.GetWOList(ProjectID:=Project.ID)
			dgWOList.DataSource = ProjectWOList
			Session("ProjectWOList") = ProjectWOList

			txtProjectDate.Text = Project.ProjectDateFormatted.ToString
			txtReceivingDate.Text = Project.ReceivingDateFormatted.ToString
			txtInspectionDate.Text = Project.InspectionDateFormatted.ToString

			AircraftList = MachineNameValueList.GetMachineList(CurrentDate:=Today.Date.ToString,
															   SkipIsForInventoryAircarft:=True,
															   IsTagRequired:=True,
															   TagText:="(SELECT)",
															   SkipReadOnlyAircrafts:=True)
			Session("MachineList") = AircraftList
			cmbAircraftList.DataSource = AircraftList

			txtPlanStartDate.Text = Project.PlanStartDateFormatted.ToString
			txtPlanEndDate.Text = Project.PlanEndDateFormatted.ToString

			DD_ServiceProvider.DataSource = VendorList.GetVendorstList(LookInType:=0, , , , , , SelectTag:="(SELECT)", IsServiceProvider:=True)

			Session("mProject") = Project
			cmbEmployee.SelectedValue = Project.ReceivingPersonID.ToString
			cmbAircraftList.Visible = (Project.TransTypeID = 101)
			txtRegNo.Visible = (Project.TransTypeID = 104)

			CompletedWOCount = (From mWO In ProjectWOList
								Where mWO.WOStatusID = 3
								Select mWO).Count

			If Not Project.IsNew And Project.TransTypeID = 101 Then

				mAssemblyStatusPeriodInfo = AssemblyStatusPeriodInfo(ProjectDate:=txtProjectDate.Text.ToString,
																	 AircraftName:=Project.MachineID.ToString)

			Else
				mAssemblyStatusPeriodInfo = Session("AssemblyStatusPeriodInfo")
			End If

			GV_CurrentPeriodValue.DataSource = mAssemblyStatusPeriodInfo
			DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub FillWorkOrderTypeCombo(MachineID As Guid)

		Try

			cmbWorkOrderType.Items.Clear()

			If Project.TransTypeID = 101 Then ' 101 CAMO PROJECT

				cmbWorkOrderType.Items.Add(New ListItem("AMP Task", "89")) 'CAMO Scheduled

				If CBool(AppSettings("IsEngineeringWORequired")) Then
					cmbWorkOrderType.Items.Add(New ListItem("AD / SB", "102")) 'EO
				End If

				cmbWorkOrderType.Items.Add(New ListItem("Un-Scheduled Task", "88")) 'UnScheduled

				If CBool(AppSettings("ShowNewDiscrepancyFlow")) Then
					cmbWorkOrderType.Items.Add(New ListItem("Discrepancies", "109")) 'Discrepancies
				Else

					cmbWorkOrderType.Items.Add(New ListItem(IIf(CBool(AppSettings("MELSnagNomenclature")),
																		  "Defect / ADD",
 																		  "Snag / MEL"), "108")) 'MEL
				End If

				cmbWorkOrderType.Items.Add(New ListItem("Concession Task", "117")) 'Concession Task 'Added by Prashant 9-Oct-2025

			Else ' 104 AMO PROJECT

				cmbWorkOrderType.Items.Add(New ListItem("AMO Task", "111")) 'AMO AMP

				If CBool(AppSettings("IsEngineeringWORequired")) Then
					cmbWorkOrderType.Items.Add(New ListItem("AD / SB", "113")) 'EO
				End If

				cmbWorkOrderType.Items.Add(New ListItem("Customer WO", "112")) 'UnScheduled

				If CBool(AppSettings("ShowNewDiscrepancyFlow")) Then
					cmbWorkOrderType.Items.Add(New ListItem("Discrepancies", "109")) 'Discrepancies
				Else

					cmbWorkOrderType.Items.Add(New ListItem(IIf(CBool(AppSettings("MELSnagNomenclature")),
																		  "Defect / ADD",
																		  "Snag / MEL"), "110")) 'MEL
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Custom Validation(s) "

	Public Sub CustomValidation(sender As Object, e As ServerValidateEventArgs)

		Dim CustomValidator As CustomValidator = CType(sender, CustomValidator)

		Try

			If CustomValidator.ControlToValidate = "txtRemark" Then

				If Len(Trim(txtRemark.Text)) > 1000 Then
					CustomValidator.ErrorMessage = "Max. Length Of Remark should be 1000."
					e.IsValid = False
				Else
					e.IsValid = True
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Function ObjectValidation() As Boolean

		Dim errorMsg As String = ""

		Try

			SetObject()

			If Project.IsValid = False Then

				For i As Integer = 0 To Project.GetBrokenRulesCollection.Count - 1
					errorMsg += Project.GetBrokenRulesCollection(i).Description + "<Br>"
				Next

			End If

			If errorMsg.Trim <> "" Then

				CustValidator.ErrorMessage = errorMsg
				CustValidator.IsValid = False
				Return False

			End If

			Return True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function PlanCompletionValidation() As Boolean

		Dim errorMsg As String = ""
		Dim WorkOrderNumbers As New List(Of String)()
		Dim IsWOCompletionDateLargerThanPlanClosingDate As Boolean = False
		ProjectWOList = CType(Session("ProjectWOList"), nWOList)

		Try

			If txtPlanStartDate.Text.ToString = "" OrElse txtPlanEndDate.Text.ToString = "" Then
				errorMsg += $"Please enter the Plan Start / End Date before Completing a {Prefix}."
			Else

				If CDate(txtPlanStartDate.Text) < CDate(txtProjectDate.Text) Then
					errorMsg += $"Plan Start Date must be On Or after the {Prefix} Date."
				End If

				If (CDate(txtPlanStartDate.Text) > CDate(txtPlanEndDate.Text)) Then
					errorMsg += $"Plan's End Date must be on or after the Start Date"
				End If

				For Each WorkOrder As nWO In ProjectWOList

					If CDate(txtPlanEndDate.Text) < CDate(WorkOrder.WOCloseDate.ToString) Then
						IsWOCompletionDateLargerThanPlanClosingDate = True
						WorkOrderNumbers.Add(WorkOrder.WONumber)
					End If

				Next

				If IsWOCompletionDateLargerThanPlanClosingDate Then

					Dim WorkOrderNumber As String

					If WorkOrderNumbers.Count > 1 Then
						WorkOrderNumber = $"({String.Join(", ", WorkOrderNumbers)})"
					ElseIf WorkOrderNumbers.Count = 1 Then
						WorkOrderNumber = WorkOrderNumbers.First()
					Else
						WorkOrderNumber = ""
					End If

					errorMsg += $"Plan's End Date must be on or after the Work Order {WorkOrderNumber} Closing Date."

				End If

			End If

			If errorMsg.Trim <> "" Then

				CustValidator.ErrorMessage = errorMsg
				CustValidator.IsValid = False
				Return False

			End If

			Return True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function WODateValidation() As Boolean

		Dim ErrorMsg As String = ""

		Try

			If txtWODate.Text = "" Then
				ErrorMsg = $"Work Order Date is Required."
			ElseIf CDate(txtWODate.Text) < CDate(txtProjectDate.Text) Then
				ErrorMsg = $"Work Order Date should be Equal / Greater than
							{Prefix} Date."
			End If

			If ErrorMsg.Trim <> "" Then

				CustValidator.ErrorMessage = ErrorMsg
				CustValidator.IsValid = False
				Return False

			End If

			Return True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			GetSession()
			EventLogID = CType(Session("EventLogID"), Guid)
			AddAttributes()

			Prefix = $"{IIf(CInt(Session("TransTypeID")) = 101, "Work-Pack", "Project")}"

			If Not IsPostBack Then

				If CType(Session(name:="AddTransTextSeries"), String) = "True" AndAlso
				   (Session("TransText_ForTransSeries") IsNot Nothing) Then

					If Project.IsNew Then

						Project.Text = Session("TransText_ForTransSeries")
						Session("mProject") = Project
						Session("AddTransTextSeries") = "False"
						Session.Remove("TransName_ForTransSeries")
						Session.Remove("TransText_ForTransSeries")
						Session.Remove("TransNo_ForTransSeries")

					End If

				End If

				FillWorkOrderTypeCombo(Project.MachineID)
				DataFieldBind()
				SetTitle()
				ControlVisibility()
				ControlVisibilityForFileAttachment()
				upnlAirframePeriods.Update()
				upnlProjectStatus.Update()
				upnlValidationsummary.Update()

			End If

			ProjectDetails = $"{Project.ProjectNumber} Dated : {Project.ProjectDateFormatted} Customer {Project.CustomerName}"

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddWorkOrder(sender As Object, e As ImageClickEventArgs) Handles btnAddWO.Click

		Try

			If (Project.TransTypeID <> 104 And cmbAircraftList.SelectedIndex = 0) Then 'CAMO & EO

				MSGBoxCtrl.Show("Alert..!!",
								"Please select an Aircraft.",
								"",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			End If

			If IsValid AndAlso ObjectValidation() AndAlso WODateValidation() Then

				SetObject()

				SetWorkOrder(Project.TransTypeID, Val(cmbWorkOrderType.SelectedValue.ToString))

				WorkOrder = Session("mnWO")
				WorkOrder.WODate = CDate(txtWODate.Text)
				WorkOrder.ProjectID = Project.ID
				WorkOrder.CustomerID = New Guid(cmbCustomer.SelectedValue)
				WorkOrder.MachineID = Project.MachineID
				WorkOrder.RegNo = txtRegNo.Text.Trim
				WorkOrder.CustomerContractID = Project.CustomerContractID
				WorkOrder.IsFMC = chkCustomerContract.Checked
				WorkOrder.CustomerContractNo = lblCustomerContractNo.Text.Trim
				WorkOrder.ProjectDate = Project.ProjectDate
				WorkOrder.ModelName = Project.ModelName
				WorkOrder.SerialNo = txtSerialNo.Text.Trim
				WorkOrder.WorkOrderCountInProject = dgWOList.Rows.Count
				WorkOrder.ServiceProviderID = New Guid(DD_ServiceProvider.SelectedValue)

				If Not WorkOrder.MachineID.Equals(Guid.Empty) Then 'CAMO

					mAssemblyStatusPeriodInfo = AssemblyStatusPeriodInfo(ProjectDate:=txtProjectDate.Text.ToString,
																		 AircraftName:=cmbAircraftList.SelectedValue.ToString)

					If WorkOrder.WOPeriods.Count <> 0 Then

						For i As Integer = WorkOrder.WOPeriods.Count - 1 To 0 Step -1
							WorkOrder.WOPeriods.RemoveAt(i)
						Next

					End If

					WorkOrder.WOPeriods.SetWOPeriods(WorkOrder.ID, AssemblyStatusPeriodList, WorkOrder.HourType)

					AssemblyStatusList = Nothing

				End If

				ControlVisibility()
				upnlProjectDetail.Update()

				Dim URLFromDueReportPreview As New Stack
				URLFromDueReportPreview.Push(Request.Url)

				Session("mnWO") = WorkOrder
				Session("TransTypeID") = Project.TransTypeID.ToString
				Session("wfProject_Ajax") = "wfProject_Ajax"
				Session("OpenFromProject") = "OpenFromProject"
				Session("URLFromDueReportPreview") = URLFromDueReportPreview

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"OpenWODetail",
													"OpenWODetail();",
													True)
			Else

				upnlValidationsummary.Update()
				Exit Sub

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub GV_WOList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgWOList.RowCommand

		Try

			Dim ID
			Dim Index As Integer
			Dim GridViewRow As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
			Index = GridViewRow.RowIndex
			ID = New Guid(dgWOList.DataKeys(Index).Value.ToString)

			Select Case e.CommandName

				Case "EditRec"

					If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	ModuleName:=Prefix,
																	TransTypeID:=Project.TransTypeID,
																	Action:={Action.Edit},
																	MarkLogDetail:=ProjectDetails) Then

						Exit Sub

					End If

					WorkOrder = nWO.GetWO(ID, AllWOJobType:=False)
					WorkOrder.WorkOrderCountInProject = dgWOList.Rows.Count
					Session("mnWO") = WorkOrder

					Dim URLFromDueReportPreview As New Stack
					URLFromDueReportPreview.Push(Request.Url)

					Session("wfProject_Ajax") = "wfProject_Ajax"
					Session("URLFromDueReportPreview") = URLFromDueReportPreview

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"OpenWODetail",
														"OpenWODetail();",
														True)

				Case "DeleteRec"

					If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	ModuleName:=Prefix,
																	TransTypeID:=Project.TransTypeID,
																	Action:={Action.Delete},
																	MarkLogDetail:=ProjectDetails) Then

						Exit Sub

					End If

					If dgWOList.Rows.Count = 1 And Project.IsNew = False Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DeleteAlert,
										MSGBox.Message_Text.RecordCannotBeDelete,
										"As project cannot be saved without Work Order.",
										MsgBoxStyle.OkOnly,
										"")

						Exit Sub

					End If

					WorkOrder = nWO.GetWO(ID, AllWOJobType:=False)
					Session("mnWO") = WorkOrder

					DeleteRecord()

				Case "ViewRec"

					If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	ModuleName:=Prefix,
																	TransTypeID:=Project.TransTypeID,
																	Action:={Action.View},
																	MarkLogDetail:=ProjectDetails) Then

						Exit Sub

					End If

					WorkOrder = nWO.GetWO(ID)
					Dim mFileAttachments As New FileAttachments
					mFileAttachments = FileAttachments.GetChildFileAttachments(ReferenceID:=WorkOrder.ID)
					Dim AttachmentCount As Integer = mFileAttachments.Count

					DataFieldBind()
					SetTitle()

					If AttachmentCount > 1 Then

						Session("mFileAttachments") = mFileAttachments
						Session("TransactionNameMarkLog") = "Work Order" 'used for MarkLog
						Session("TransactionName") = "Work Order No. & Date"
						Session("TransactionDetails") = WorkOrder.WONumber + " & " + WorkOrder.WODateFormatted.ToString

						ScriptManager.RegisterStartupScript(Me,
															[GetType],
															"OpenAttachWindow",
															"OpenAttachWindow();",
															True)

					Else

						Dim Detail As String
						Dim FileAttach As FileAttach
						FileAttach = FileAttach.GetAttachment(ReferenceID:=ID, , FileName:=WorkOrder.FileAttachments(0).FileName)

						AttachmentHelper.DownloadAttachmentWithName(AttachmentObject:=FileAttach)

						ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)

						Detail = $"Work Order Attachment( {FileAttach.FileName} viewed by {User.Identity.Name}"

						MarkLog(Action.View,
								"Work Order",
								Detail,
								ErrorType.HandledError,
								ID,
								EventLogID)
					End If

				Case "JobDetails"

					If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	ModuleName:=Prefix,
																	TransTypeID:=Project.TransTypeID,
																	Action:={Action.View},
																	MarkLogDetail:=ProjectDetails) Then

						Exit Sub

					End If


					WorkOrder = nWO.GetWO(ID, False)

					GV_WOJobDetails.DataSource = WorkOrder.WOJobs
					GV_WOJobDetails.DataBind()

					If WorkOrder.TransTypeID = 102 Then
						GV_WOJobDetails.HeaderRow.Cells(2).Text = "Directive No."
					Else
						GV_WOJobDetails.HeaderRow.Cells(2).Text = "Task No."
					End If

					lblWOJobDetailsHeader.Text = "Job Details of [ " + WorkOrder.WONumber + " ] "

					upnlGV_WOJobDetails.Update()
					upnlWOJobDetails.Update()

					Session("WOJobs") = WorkOrder.WOJobs
					Session("mnWO") = WorkOrder
					mdlPopUpWOJobDetails.Show()

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_WOList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgWOList.RowDataBound

		Try

			If e.Row.RowType <> DataControlRowType.DataRow Then
				Return
			End If

			If (e.Row.RowType = DataControlRowType.DataRow) Then

				Dim TaskCompletionPercentage As Integer = (DataBinder.Eval(e.Row.DataItem, "TaskCompletionPercentage"))
				Dim tmpDiv As HtmlGenericControl = CType(e.Row.FindControl("prgbar"), HtmlGenericControl)
				Dim lblPercentage As HtmlGenericControl = CType(e.Row.FindControl("lblPercentage"), HtmlGenericControl)
				tmpDiv.Attributes.Add("style", "width:" + TaskCompletionPercentage.ToString + "%")
				tmpDiv.Attributes.Add("aria-valuenow", TaskCompletionPercentage.ToString)
				lblPercentage.InnerText = TaskCompletionPercentage.ToString + "%"

				If TaskCompletionPercentage = 0 Then
					lblPercentage.Attributes.Add("style", "color:black;")
				Else
					lblPercentage.Attributes.Add("style", "color:white;")
				End If

				'Back Color 
				Dim TransTypeID As Integer = (DataBinder.Eval(e.Row.DataItem, "TransTypeID"))
				Dim WOJobTypeID As Integer = (DataBinder.Eval(e.Row.DataItem, "WOJobTypeID"))


				For i As Integer = 1 To 11 ' Columns 1 to 11 (0-based index)

					Select Case TransTypeID
						Case 89

							If WOJobTypeID = 1 Then
								e.Row.Cells(i).BackColor = ColorTranslator.FromHtml("#ffff90") '"Un-Schedule"''Color.LightYellow 
							ElseIf WOJobTypeID = 2 Then
								e.Row.Cells(i).BackColor = Color.LightBlue  '' "AMP Task"
							End If

						Case 102
							e.Row.Cells(i).BackColor = Color.LightCoral    '' "AD / SB"
						Case 108, 110
							e.Row.Cells(i).BackColor = Color.LightCyan     '' "MEL / Snag"
						Case 109
							e.Row.Cells(i).BackColor = Color.LightGray     '' "Discrepancy"
						Case 111
							e.Row.Cells(i).BackColor = Color.LightBlue     '' "AMO Task"
						Case 113
							e.Row.Cells(i).BackColor = Color.LightCoral    '' "AMO AD / SB WO"
						Case 112
							e.Row.Cells(i).BackColor = Color.LightYellow   '' "Customer WO"
						Case 117
							e.Row.Cells(i).BackColor = Color.LightGreen    '' "Concession Task" 'Added by Prashant 9-Oct-2025
					End Select

				Next

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_WOJobDetails_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GV_WOJobDetails.PageIndexChanging

		Try

			GV_WOJobDetails.PageIndex = e.NewPageIndex
			GV_WOJobDetails.DataSource = CType(Session("WOJobs"), nWOJobs)
			GV_WOJobDetails.DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DeleteRecord()

		MSGBoxCtrl.Show(MSGBox.Message_Title.Delete,
						MSGBox.Message_Text.Delete,
						" ",
						MsgBoxStyle.YesNo,
						"Delete")

	End Sub

	Private Sub SaveProject(sender As Object, e As EventArgs) Handles btnSave.Click

		Try

			If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
															MSGBoxCtrl:=MSGBoxCtrl,
															ModuleName:=Prefix,
															TransTypeID:=Project.TransTypeID,
															Action:={Action.New, Action.Edit},
															MarkLogDetail:=ProjectDetails,
															IsForSave:=True) Then

				Exit Sub

			End If

			If IsValid And ObjectValidation() Then
				Save()
			Else
				upnlValidationsummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SelectFile(sender As Object, e As EventArgs) Handles btnSelectFile.ServerClick

		Try

			If (
					Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																 MSGBoxCtrl:=MSGBoxCtrl,
																 ModuleName:=Prefix,
																 TransTypeID:=Project.TransTypeID,
																 Action:={Action.Authorize},
																 MarkLogDetail:=ProjectDetails) AndAlso
					(
						AppSettings("ClientCode") = "Deccan" Or
						AppSettings("ClientCode") = "IIC" Or
						AppSettings("ClientCode") = "SPZ"
					)
			   ) Then ' SPZ Code added by Saylee on 13-Jun-2022 

				Exit Sub

			End If

			If Project.IsAttachmentAdded = True Then
				FileAttach = FileAttach.GetAttachment(Project.ID)
			Else
				FileAttach = FileAttach.NewAttachment(Guid.Empty, Project.ID)
			End If

			Session("mFileAttach") = FileAttach
			Session("DetailAttachment") = "False"
			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"OpenFileUploadWindow",
												"OpenFileUploadWindow()",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ViewAttachment(sender As Object, e As ImageClickEventArgs) Handles btnViewAttachment.Click
		ViewImage()
	End Sub

	Private Sub GetAttachment()

		Try

			If Project.IsAttachmentAdded And FileAttach Is Nothing Then

				FileAttach = FileAttach.GetAttachment(ReferenceID:=Project.ID)
				Session("mFileAttachProject") = FileAttach
				Session("mFileAttach") = FileAttach

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SaveAttachment()

		Try

			If FileAttach IsNot Nothing Then

				If FileAttach.Size > 0 Then

					Try

						FileAttach.Save()

					Catch ex As Exception
						ScriptManager.RegisterClientScriptBlock(Me,
																[GetType],
																"",
																MessageBox.Show(ex.InnerException.ToString, False),
																True)
					End Try

				Else

					If (Not Project.IsNew) And IsAttachmentDeleted Then

						FileAttach.DeleteAttachment(ID:=FileAttach.ID,
													ReferenceID:=Project.ID)

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

			GetAttachment()

			AttachmentHelper.DownloadAttachmentWithName(AttachmentObject:=FileAttach)

			ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub RemoveAttachment(sender As Object, e As EventArgs) Handles btnDelAttach.Click

		Try

			If (
					Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																 MSGBoxCtrl:=MSGBoxCtrl,
																 ModuleName:=Prefix,
																 TransTypeID:=Project.TransTypeID,
																 Action:={Action.Authorize},
																 MarkLogDetail:=ProjectDetails) AndAlso
					(
						AppSettings("ClientCode") = "Deccan" Or
						AppSettings("ClientCode") = "IIC" Or
						AppSettings("ClientCode") = "SPZ"
					)
			   ) Then ' SPZ Code added by Saylee on 13-Jun-2022 

				Exit Sub

			End If

			Dim fileSize As Integer = 0
			Dim file(fileSize) As Byte

			GetAttachment()

			FileAttach.ImageFile = file
			FileAttach.Size = 0
			btnViewAttachment.Visible = False
			btnDelAttach.Enabled = False
			IsAttachmentDeleted = True
			Project.IsAttachmentAdded = False
			Session("IsAttachmentDeleted") = IsAttachmentDeleted

			ControlVisibility()
			ControlVisibilityForFileAttachment()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

		MSGBoxCtrl.HideControl()
		MessageBoxResult()

	End Sub

	Private Sub HdnBtnFileUploaded(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click

		Try

			If Session("DetailAttachment") = "False" Then

				Project.IsAttachmentAdded = True
				Session("mFileAttachProject") = Session("mFileAttach")
				Session("mFileAttach") = Nothing
				Session.Remove("mFileAttach")
				ControlVisibilityForFileAttachment()
				upnlAttachFile.Update()

			ElseIf Session("DetailAttachment") = "True" Then

				dgWOList.DataSource = nWOList.GetWOList(Project.ID)
				dgWOList.DataBind()
				ControlVisibility()
				upnlProjectDetail.Update()

			End If

			Session.Remove("DetailAttachment")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub HdnBtnAddWODetails(sender As Object, e As EventArgs) Handles hdnBtnAddWODetail.Click

		Try

			Session("mProject") = Project

			ProjectWOList = nWOList.GetWOList(Project.ID)
			dgWOList.DataSource = ProjectWOList
			Session("ProjectWOList") = ProjectWOList

			If Project.IsNew And ProjectWOList.Count = 1 Then
				Project.Save()
			End If

			DataFieldBind()
			ControlVisibility()
			SetTitle()
			upnlTitle.Update()
			upnlButtons.Update()
			upnlProjectDetail.Update()
			upnlProjectDetails.Update()
			upnlAirframePeriods.Update()
			upnlValidationsummary.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub HdnBtnCustomerContractSelection_Click(sender As Object, e As EventArgs) Handles hdnBtnCustomerContractSelection.Click

		Try

			If Project.CustomerContractID.Equals(Guid.Empty) And chkCustomerContract.Checked = True Then
				chkCustomerContract.Checked = False
			End If

			lblCustomerContractNo.DataBind()
			upnlCustomerContract.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CustomerContractChanged(sender As Object, e As EventArgs) Handles chkCustomerContract.CheckedChanged

		Try

			If chkCustomerContract.Checked = True Then

				SetObject()
				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"OpenCustomerContractSelectionWindow",
													"OpenCustomerContractSelectionWindow();",
													True)

			ElseIf chkCustomerContract.Checked = False Then

				Project.CustomerContractID = Guid.Empty
				Project.CustomerContractNo = ""
				Session("mProject") = Project
				lblCustomerContractNo.DataBind()
				upnlCustomerContract.Update()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try
	End Sub

	Private Sub AircraftChanged(sender As Object, e As EventArgs) Handles cmbAircraftList.SelectedIndexChanged

		Try

			AjaxLoader.Visible = False

			If cmbAircraftList.SelectedIndex > 0 Then

				Project.MachineID = New Guid(cmbAircraftList.SelectedValue.ToString)
				txtRegNo.ReadOnly = True
				txtModelNo.ReadOnly = True
				txtSerialNo.ReadOnly = True
				FillWorkOrderTypeCombo(Project.MachineID)

			Else

				Project.MachineID = Guid.Empty
				txtRegNo.ReadOnly = False
				txtModelNo.ReadOnly = False
				txtSerialNo.ReadOnly = False

			End If

			If Project.TransTypeID = 101 Then

				mAssemblyStatusPeriodInfo = AssemblyStatusPeriodInfo(ProjectDate:=txtProjectDate.Text.ToString,
																	 AircraftName:=cmbAircraftList.SelectedValue.ToString)

				GV_CurrentPeriodValue.DataSource = mAssemblyStatusPeriodInfo
				Session("AssemblyStatusPeriodInfo") = mAssemblyStatusPeriodInfo
				GV_CurrentPeriodValue.DataBind()

			End If

			txtRegNo.DataBind()
			txtModelNo.DataBind()
			txtSerialNo.DataBind()
			upnlModelNo.Update()
			upnlSerialNo.Update()
			upnlAirframePeriods.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnPrint.Click

		Dim crystalReport As Engine.ReportClass = New crnProjectWODetails
		Dim companyDetail As New CompanyDetail
		Dim rptProjectWODetails As rptProjectWODetails
		Dim dataSet As New dsProjectWODetails
		Dim objectAdapter As New ObjectAdapter
		Dim AssemblyPeriodCurrentHours As String = String.Empty
		Dim AssemblyPeriodCurrentCycles As String = String.Empty

		Try

			If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
															MSGBoxCtrl:=MSGBoxCtrl,
															ModuleName:=Prefix,
															TransTypeID:=Project.TransTypeID,
															Action:={Action.Print},
															MSGBoxSender:="Authorization",
															MarkLogDetail:=ProjectDetails) Then

				Exit Sub

			End If

			rptProjectWODetails = rptProjectWODetails.GetProjectWODetails(ProjectID:=Project.ID)

			If rptProjectWODetails.Count > 0 Then

				RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name,
													1550)

			Else

				MSGBoxCtrl.Show(MSGBox.Message_Title.NoRecordFound,
								MSGBox.Message_Text.NoRecordFound,
								"No records found for this search criteria.",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			End If

			If Project.TransTypeID = 101 Then

				AssemblyPeriodCurrentHours = IIf(AssemblyStatusPeriodList.Contains(PeriodID:=1),
												 AssemblyStatusPeriodList.Item(PeriodID:=1, Str:="").AssemblyCurrentValueTextFormatted.ToString,
												 "")

				AssemblyPeriodCurrentCycles = IIf(AssemblyStatusPeriodList.Contains(PeriodID:=3),
												  AssemblyStatusPeriodList.Item(PeriodID:=3, Str:="").AssemblyCurrentValueTextFormatted.ToString,
												  "")

			End If

			If AppSettings("ClientCode") = "AFC" Then
				crystalReport = New crnProjectWODetailsForAfcom
				ReportName = "WORKPACKAGE - SUMMARY"
			Else
				ReportName = "WORK-PACK DETAILS"
			End If

			ProjectWOList = CType(Session("ProjectWOList"), nWOList)

			If ProjectWOList.Count > 0 Then
				mLocationName = ProjectWOList(0).WorkShopLocation 'SearchStr3
			Else
				mLocationName = "" 'SearchStr3
			End If

			Dim ReportData As New ReportData(CompanyName:=companyDetail.CompanyName,
											 Address:=companyDetail.Address,
											 Tel1:=companyDetail.Tel1,
											 Tel2:=companyDetail.Tel2,
											 Fax:=companyDetail.Fax,
											 Email:=companyDetail.Email,
											 WebSite:=companyDetail.WebSite,
											 ReportName:=ReportName,
											 ProductVersion:=AppSettings("Product Version"),
											 SINote:=AppSettings("SINote"),
											 SearchStr1:=AssemblyPeriodCurrentHours,
											 SearchStr2:=AssemblyPeriodCurrentCycles,
											 SearchStr3:=mLocationName,
											 SearchStr4:="",
											 SearchStr5:="",
											 SearchStr6:="",
											 SearchStr7:="",
											 SearchStr8:="",
											 SearchStr9:="",
											 SearchStr10:="",
											 SearchStr11:="",
											 SearchStr12:="",
											 SearchStr13:="",
											 SearchStr14:=AppSettings("Logo"),
											 SearchStr15:=AppSettings("ClientCode"))

			dataSet.Clear()

			Dim companyLogo As rptImage = rptImage.GetImage(dataSet)
			objectAdapter.Fill(dataSet, TableName:="rptImage", companyLogo)
			objectAdapter.Fill(dataSet, TableName:="ProjectWODetails", rptProjectWODetails)
			objectAdapter.Fill(dataSet, TableName:="ReportData", ReportData)
			objectAdapter.Fill(dataSet, TableName:="Project", Project)
			crystalReport.SetDataSource(dataSet)

			Session("CrystalReport") = crystalReport

			MarkLog(Action.Print,
					"Project Detail",
					ProjectDetails,
					ErrorType.NoError,
					Guid.Empty,
					EventLogID)

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Display Report",
												"displayReport()",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ReturnBack(sender As Object, e As EventArgs) Handles btnClose.Click

		Try

			MarkLog(Action.Close,
					"Project",
					"",
					ErrorType.NoError,
					Guid.Empty,
					EventLogID)

			Session("IsValid") = IsValid

			SetObject()

			If Project.IsDirty Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.CloseConfirm,
								MSGBox.Message_Text.Save,
								"",
								MsgBoxStyle.YesNo,
								"Close")

			Else

				RemoveSessions()
				EmployeeList = Nothing
				Project = Nothing
				Session.Remove("IsProjectForRenew")
				Response.Redirect("Index.aspx")

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CloseWOJobDetailsPopUp(sender As Object, e As EventArgs) Handles btnCloseWOJobDetails.Click

		Try

			mdlPopUpWOJobDetails.Hide()
			upnlProjectDetail.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Update Project Status "

	Private Sub SubmitProject(sender As Object, e As EventArgs) Handles btnSubmit.Click

		Try

			SetSession()
			SetObject()

			If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
															MSGBoxCtrl:=MSGBoxCtrl,
															ModuleName:=Prefix,
															TransTypeID:=Project.TransTypeID,
															Action:={Action.Authorize},
															MarkLogDetail:=ProjectDetails,
															MSGBoxSender:="Authorization") Then

				Exit Sub

			End If

			MSGBoxCtrl.Show(MSGBox.Message_Title.Submission,
							MSGBox.Message_Text.Submission,
							"<strong>Project.</strong>",
							MsgBoxStyle.YesNo,
							"ProjectSubmission")

		Catch ex As Exception
			Throw ex
		End Try

	End Sub

	Private Sub CompleteProject(sender As Object, e As EventArgs) Handles btnComplete.Click

		Try

			If IsValid AndAlso ObjectValidation() AndAlso PlanCompletionValidation() Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.StatusCompleted,
								MSGBox.Message_Text.StatusCompleted,
								"<strong>Project.</strong>",
								MsgBoxStyle.YesNo,
								"ProjectCompletion")

			Else
				upnlValidationsummary.Update()
			End If

		Catch ex As Exception
			Throw ex
		End Try

	End Sub

#End Region

#Region " Assembly Status Period Info "

	Public Function AssemblyStatusPeriodInfo(ProjectDate As String,
											 AircraftName As String) As AssemblyStatusPeriodList

		Try

			AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(CurrentDate:=ProjectDate,
																				  MachineID:=AircraftName, , , , , , , , , ,
																				  AssemblyRequired:=True, , , ,
																				  AssemblyType:="Airframe", , , , , , , , , , , , , , , , , ,
																				  ShowNotInUse:=True,
																				  SkipIsForInventoryAircarft:=True,
																				  MonitoringServiceRequired:=False,
																				  MonitoringModRequired:=False,
																				  MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList

			AssemblyStatusPeriodList = AssemblyStatusList(AssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
			Session("AssemblyStatusPeriodListForProject") = AssemblyStatusPeriodList

			Return AssemblyStatusPeriodList

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Service Method(s) "

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetRegTextList(prefixText As String,
										  count As Integer,
										  contextKey As String) As String()

		Try

			Dim DistinctTextList As DistinctTextListAutoComplete
			DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, 32)

			If count = 0 Then

				Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray

			Else

				Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetModelNameList(prefixText As String,
											count As Integer,
											contextKey As String) As String()

		Try

			Dim DistinctTextList As DistinctTextListAutoComplete
			DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, 27)

			If count = 0 Then

				Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray

			Else

				Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class