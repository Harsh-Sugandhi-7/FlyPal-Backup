Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Script.Serialization

Public Class DashBoardWO
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mnWOStatusCountDashboard As nWOStatusCountDashboard
	Public mnWOStatusCountForPieGraph As nWOStatusCountForPieGraph
	Public mrptRequisitionItemStatusList As rptRequisitionItemStatusList
	Public mWOListDash As nWOList
	Public mRequisitionListNewDash As RequisitionListNew
	Public mIssueListDash As IssueList
	Shared Count As Integer = 0
	Shared PlannedList As String = ""
	Public mWOStatusList As nWOStatusList
	Shared tmpWOstatusID As Integer = 0
	Shared tmpCustomerID As Guid = Guid.Empty
	Shared tmpMonth As Integer = 0
	Shared tmpYear As Integer = 0
	Dim mCustomerList As VendorList
	Public mEmployeeListForCombo As EmployeeListForCombo

	Dim mMissingMonthData As Object
	Dim mMonthList As MonthList
#End Region

#Region " Methods "
	Private Sub GetSession()
		Count = Session("Count")
		mWOStatusList = Session("mWOStatusList")
		mCustomerList = Session("mCustomerList")
		mrptRequisitionItemStatusList = Session("mrptRequisitionItemStatusListDash")
		mEmployeeListForCombo = CType(Session("mEmployeeListForCombo"), EmployeeListForCombo)
		mMissingMonthData = Session("mMissingMonthData")
		mMonthList = Session("mMonthList")
	End Sub
	Private Sub ClearAll()
		If InStr(Session("MiddleFrame"), "DashBoardWO.aspx?") <= 0 Then
			PlannedList = ""
			Session.Remove("mWOStatusList")
			Session.Remove("mCustomerList")
			Session.Remove("mEmployeeListForCombo")
			Session.Remove("mMissingMonthData")
			Session.Remove("mMonthList")
		End If
	End Sub
	Public Sub SetPieGraph()

		Dim PieGraphWOStatusCountForPieGraphValues As String = New JavaScriptSerializer().Serialize(mnWOStatusCountForPieGraph)
		PieGraphWOStatusCountForPieGraphValues = PieGraphWOStatusCountForPieGraphValues.Replace("StatusName", "label").Replace("StatusCnt", "value")
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FusionChartPieFunc", "FusionChartPieFunc('" + PieGraphWOStatusCountForPieGraphValues.ToString + "');", True)
	End Sub
	Public Sub GetRequisitionDetails()
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncRequisitionDet", "FuncRequisitionDet('" + chkNotIssuedItems.Checked.ToString + "', '" + chkNotReceivedItems.Checked.ToString + "');", True)
	End Sub
	Public Sub GetMonthlyEmployeeWiseWorkDone(ByVal EmployeeID As Guid)
		Dim mEmployeeWiseWorkDoneInWO As EmployeeWiseWorkDoneInWO
		Dim GraphMonthlyEmployeeWiseWorkDoneValues As String

		Dim StartDateM As SmartDate
		Dim EndDateM As SmartDate

		StartDateM = New SmartDate(CStr(DateAdd(DateInterval.Month, Today.Month, DateSerial(Today.Year - 1, 1, 1))))
		EndDateM = New SmartDate(CStr(DateAdd("d", -1, DateAdd("m", 1, DateSerial(Today.Year, Today.Month, 1)))))

		mEmployeeWiseWorkDoneInWO = EmployeeWiseWorkDoneInWO.GetEmployeeWiseWorkDone(EmployeeID:=EmployeeID, FromDate:="1/1/1900", ToDate:="1/1/2200")


		Dim templist As New System.Collections.ArrayList
		Dim tempinfo As EmployeeWiseWorkDoneInWO.EmployeeWiseWorkDoneInWOInfo
		Dim mFinalGraph As Object

		If mEmployeeWiseWorkDoneInWO.Count > 0 Then
			Dim mEmployeeWiseWorkDoneInWOGraph As Object
			mEmployeeWiseWorkDoneInWOGraph = (From c In mEmployeeWiseWorkDoneInWO
											  Where (c.year = Today.Year)
											  Order By c.month Ascending, c.year Ascending
											  Group By mMonth = c.Month, mYear = c.Year, mMonthYear = c.MonthYear Into Group
											  Select New With {.Month = mMonth, .Year = mYear, .MonthYear = mMonthYear, .TotalEstimatedTime = Group.Sum(Function(x) x.TotalEstimatedTime), .TotalActualTime = Group.Sum(Function(x) x.TotalActualTime)})
			', .TotalEstimatedTime = Group.Sum(Function(x) x.TotalEstimatedTime), .TotalActualTime = Group.Sum(Function(x) x.TotalActualTime)

			For Each variable As Object In mMissingMonthData
				tempinfo.TotalActualTime = 0
				tempinfo.TotalEstimatedTime = 0
				tempinfo.Year = variable.Year
				tempinfo.Month = variable.ID
				For Each variable1 As Object In mEmployeeWiseWorkDoneInWOGraph
					If variable.id = variable1.month Then
						tempinfo.TotalActualTime = variable1.TotalActualTime
						tempinfo.TotalEstimatedTime = variable1.TotalEstimatedTime
						Exit For
					End If
				Next

				templist.Add(tempinfo)
			Next


		Else

			'for showing 0 value graph if no data present
			For Each variable As Object In mMissingMonthData
				tempinfo.TotalActualTime = 0
				tempinfo.TotalEstimatedTime = 0
				tempinfo.Year = variable.Year
				tempinfo.Month = variable.ID
				templist.Add(tempinfo)
			Next

		End If

		mFinalGraph = (From c In templist
					   Select New With {c.Month, c.Year, .MonthYear = c.MonthYear, .TotalActualTime = c.TotalActualTime, .TotalEstimatedTime = c.TotalEstimatedTime})


		GraphMonthlyEmployeeWiseWorkDoneValues = New JavaScriptSerializer().Serialize(mFinalGraph)
		GraphMonthlyEmployeeWiseWorkDoneValues = GraphMonthlyEmployeeWiseWorkDoneValues.Replace("MonthYear", "label").Replace("TotalActualTime", "value")
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthlyEmployeeWiseWorkDoneValues", "MonthlyEmployeeWiseWorkDoneValues('" + GraphMonthlyEmployeeWiseWorkDoneValues.ToString + "');", True)
	End Sub
	Public Sub GetMonthlyWorkOrder()

		' Dim mnWOPlannedList As nWOList
		Dim mnWOPlannedList As nWOListForPlanCalendar

		mnWOPlannedList = nWOListForPlanCalendar.GetWOListForPlanCalendar(FromDate:="1/1/1900", ToDate:="1/1/2200")

		Dim mnWOPlannedListGraph As Object
		Dim mnWOPlannedListFinalGraph As Object 'As List(Of String) = New List(Of String) '


		mnWOPlannedListGraph = (From c In mnWOPlannedList
								Where (c.year = Today.Year)
								Order By c.month Ascending, c.year Ascending
								Group By mMonth = c.Month, mYear = c.Year, mMonthYear = c.MonthYear Into Group
								Select New With {.Month = mMonth, .Year = mYear, .MonthYear = mMonthYear, .Recordcount = Group.Count})

		Dim templist As New System.Collections.ArrayList
		Dim tempinfo As nWOListForPlanCalendar.nWOListForDueJobsInfo


		For Each variable As Object In mMissingMonthData
			tempinfo.SortOrder = variable.Recordcount
			tempinfo.Year = variable.Year
			tempinfo.Month = variable.ID
			For Each variable1 As Object In mnWOPlannedListGraph
				If variable.id = variable1.month Then
					tempinfo.SortOrder = variable1.Recordcount
					Exit For
				End If
			Next

			templist.Add(tempinfo)
		Next

		mnWOPlannedListFinalGraph = (From c In templist
									 Select New With {c.Month, c.Year, .MonthYear = c.MonthYear, .RecordCount = c.SortOrder})


		Dim GraphWOPlannedListValues As String = New JavaScriptSerializer().Serialize(mnWOPlannedListFinalGraph)
		GraphWOPlannedListValues = GraphWOPlannedListValues.Replace("MonthYear", "label").Replace("Recordcount", "value")

		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthlyWorkOrder", "MonthlyWorkOrder('" + GraphWOPlannedListValues.ToString + "');", True)

	End Sub
	Public Sub GetWOPercentage(ThisWeekStartDate As SmartDate, ThisWeekEndDate As SmartDate, LastWeekStartDate As SmartDate, LastWeekEndDate As SmartDate)
		mWOListDash = nWOList.GetWOList()
		lblWOCount.Text = mWOListDash.TotalWOCount.ToString

		Dim mWOListDashThisWEEK As List(Of nWO) = New List(Of nWO)
		Dim mWOListDashLastWEEK As List(Of nWO) = New List(Of nWO)




		mWOListDashThisWEEK = (From c As nWO In mWOListDash.AsParallel
							   Where (CDate(c.WODate) >= CDate(ThisWeekStartDate.ToString)) And CDate(c.WODate) <= CDate(ThisWeekEndDate.ToString)
							   Select c).ToList

		mWOListDashLastWEEK = (From c As nWO In mWOListDash.AsParallel
							   Where (CDate(c.WODate) >= CDate(LastWeekStartDate.ToString)) And CDate(c.WODate) <= CDate(LastWeekEndDate.ToString)
							   Select c).ToList

		Dim PercentIncreaseThisWeek As Integer
		If mWOListDashThisWEEK.Count > mWOListDashLastWEEK.Count Then 'increase percent
			PercentIncreaseThisWeek = IIf(mWOListDashLastWEEK.Count > 0, ((mWOListDashThisWEEK.Count - mWOListDashLastWEEK.Count) * 100) / mWOListDashLastWEEK.Count, 100)
			spnWOSubInfo.InnerText = PercentIncreaseThisWeek.ToString + "% higher than last week"
			icIconWOIncrease.Visible = True
			icIconWODecrease.Visible = False
		Else
			PercentIncreaseThisWeek = IIf(mWOListDashThisWEEK.Count > 0, ((mWOListDashLastWEEK.Count - mWOListDashThisWEEK.Count) * 100) / mWOListDashThisWEEK.Count, 100)
			spnWOSubInfo.InnerText = PercentIncreaseThisWeek.ToString + "% less than last week"
			icIconWOIncrease.Visible = False
			icIconWODecrease.Visible = True
		End If

	End Sub
	Public Sub GetRequisitions(ThisWeekStartDate As SmartDate, ThisWeekEndDate As SmartDate, LastWeekStartDate As SmartDate, LastWeekEndDate As SmartDate)
		' mRequisitionListNewDash = RequisitionListNew.GetRequisitionList(TransTypeID:=Util.Trans.EngineeringRequisition)
		'  Session("mRequisitionListNewDash") = mRequisitionListNewDash


		Dim mrptRequisitionItemStatusList As rptRequisitionItemStatusList
		mrptRequisitionItemStatusList = rptRequisitionItemStatusList.GetRequisitionItemStatusList(TransTypeID:=Util.Trans.EngineeringRequisition)
		'  Dim mRequisitionList As List(Of rptRequisitionItemStatusList.rptRequisitionItemStatusListInfo) = New List(Of rptRequisitionItemStatusList.rptRequisitionItemStatusListInfo)
		Dim mRequisitionList = From c In mrptRequisitionItemStatusList
							   Where Not c.WOID.Equals(Guid.Empty)
							   Select New With {Key c.ReqID, c.Date} Distinct.ToList

		'  lblRequisitionCount.Text = mRequisitionListNewDash.Count.ToString
		lblRequisitionCount.Text = mRequisitionList.Count.ToString

		'Dim mRequisitionListDashThisWEEK As List(Of RequisitionListNew.RequisitionNewInfo) = New List(Of RequisitionListNew.RequisitionNewInfo)
		'Dim mRequisitionListDashLastWEEK As List(Of RequisitionListNew.RequisitionNewInfo) = New List(Of RequisitionListNew.RequisitionNewInfo)

		'mRequisitionListDashThisWEEK = (From c As RequisitionListNew.RequisitionNewInfo In mRequisitionListNewDash.AsParallel
		'                       Where (CDate(c.Date) >= CDate(ThisWeekStartDate.ToString)) And CDate(c.Date) <= CDate(ThisWeekEndDate.ToString)
		'                       Select c).ToList

		'mRequisitionListDashLastWEEK = (From c As RequisitionListNew.RequisitionNewInfo In mRequisitionListNewDash.AsParallel
		'                     Where (CDate(c.Date) >= CDate(LastWeekStartDate.ToString)) And CDate(c.Date) <= CDate(LastWeekEndDate.ToString)
		'                     Select c).ToList
		Dim mRequisitionListDashThisWEEK = From c In mrptRequisitionItemStatusList
										   Where Not c.WOID.Equals(Guid.Empty) And (CDate(c.Date) >= CDate(ThisWeekStartDate.ToString)) And CDate(c.Date) <= CDate(ThisWeekEndDate.ToString)
										   Select New With {Key c.ReqID, c.Date} Distinct.ToList

		Dim mRequisitionListDashLastWEEK = From c In mrptRequisitionItemStatusList
										   Where Not c.WOID.Equals(Guid.Empty) And (CDate(c.Date) >= CDate(LastWeekStartDate.ToString)) And CDate(c.Date) <= CDate(LastWeekEndDate.ToString)
										   Select New With {Key c.ReqID, c.Date} Distinct.ToList


		Dim PercentIncreaseThisWeek As Integer
		If mRequisitionListDashThisWEEK.Count > mRequisitionListDashLastWEEK.Count Then 'increase percent
			PercentIncreaseThisWeek = IIf(mRequisitionListDashLastWEEK.Count > 0, ((mRequisitionListDashThisWEEK.Count - mRequisitionListDashLastWEEK.Count) * 100) / mRequisitionListDashLastWEEK.Count, 100)
			spnReqSubInfo.InnerText = PercentIncreaseThisWeek.ToString + "% higher than last week"
			icIconReqIncrease.Visible = True
			icIconReqDecrease.Visible = False
		Else
			PercentIncreaseThisWeek = IIf(mRequisitionListDashThisWEEK.Count > 0, ((mRequisitionListDashLastWEEK.Count - mRequisitionListDashThisWEEK.Count) * 100) / mRequisitionListDashThisWEEK.Count, 100)
			spnReqSubInfo.InnerText = PercentIncreaseThisWeek.ToString + "% less than last week"
			icIconReqIncrease.Visible = False
			icIconReqDecrease.Visible = True
		End If
	End Sub
	Public Sub GetIssues(ThisWeekStartDate As SmartDate, ThisWeekEndDate As SmartDate, LastWeekStartDate As SmartDate, LastWeekEndDate As SmartDate)
		mIssueListDash = IssueList.GetIssueList(FromDate:="1/1/1900", ToDate:="1/1/3300", TransTypeID:=Util.Trans.IssueToAircraft, IssueToType:=8, StatusID:=2, IsCustomPaging:=True)
		Session("mIssueListDash") = mIssueListDash
		lblIssueCount.Text = mIssueListDash.TotalRecords.ToString

		Dim mIssueListDashThisWEEK As List(Of IssueList.IssueInfo) = New List(Of IssueList.IssueInfo)
		Dim mIssueListDashLastWEEK As List(Of IssueList.IssueInfo) = New List(Of IssueList.IssueInfo)

		mIssueListDashThisWEEK = (From c As IssueList.IssueInfo In mIssueListDash.AsParallel
								  Where (CDate(c.ILDate) >= CDate(ThisWeekStartDate.ToString)) And CDate(c.ILDate) <= CDate(ThisWeekEndDate.ToString)
								  Select c).ToList

		mIssueListDashLastWEEK = (From c As IssueList.IssueInfo In mIssueListDash.AsParallel
								  Where (CDate(c.ILDate) >= CDate(LastWeekStartDate.ToString)) And CDate(c.ILDate) <= CDate(LastWeekEndDate.ToString)
								  Select c).ToList

		Dim PercentIncreaseThisWeek As Integer
		If mIssueListDashThisWEEK.Count > mIssueListDashLastWEEK.Count Then 'increase percent
			PercentIncreaseThisWeek = IIf(mIssueListDashLastWEEK.Count > 0, ((mIssueListDashThisWEEK.Count - mIssueListDashLastWEEK.Count) * 100) / mIssueListDashLastWEEK.Count, 100)
			spnIssueSubInfo.InnerText = PercentIncreaseThisWeek.ToString + "% higher than last week"
			icIconIssueIncrease.Visible = True
			icIconIssueDecrease.Visible = False
		Else
			PercentIncreaseThisWeek = IIf(mIssueListDashThisWEEK.Count > 0, ((mIssueListDashLastWEEK.Count - mIssueListDashThisWEEK.Count) * 100) / mIssueListDashThisWEEK.Count, 100)
			spnIssueSubInfo.InnerText = PercentIncreaseThisWeek.ToString + "% less than last week"
			icIconIssueIncrease.Visible = False
			icIconIssueDecrease.Visible = True
		End If
	End Sub
#End Region

#Region "Datafieldbind"
	Public Sub DatafieldBind()
		mnWOStatusCountDashboard = nWOStatusCountDashboard.GetnWOStatusCountDashboard()
		Session("mnWOStatusCountDashboard") = mnWOStatusCountDashboard



		mnWOStatusCountForPieGraph = nWOStatusCountForPieGraph.GetWOStatusCountForPieGraph(mnWOStatusCountDashboard)
		Session("mnWOStatusCountForPieGraph") = mnWOStatusCountForPieGraph


		Dim currentDay As DayOfWeek = DateTime.Now.DayOfWeek
		Dim daysTillCurrentDay As Integer = currentDay - DayOfWeek.Monday
		Dim currentWeekStartDate As Date = DateTime.Now.AddDays(-daysTillCurrentDay)
		Dim ThisWeekStartDate As SmartDate = New SmartDate(currentWeekStartDate)
		Dim ThisWeekEndDate As SmartDate = New SmartDate(currentWeekStartDate.AddDays(6).ToString)

		Dim LastWeekStartDate As SmartDate = New SmartDate(currentWeekStartDate.AddDays(-7).ToString)
		Dim LastWeekEndDate As SmartDate = New SmartDate(currentWeekStartDate.AddDays(-1).ToString)

		mMonthList = MonthList.GetMonthList()
		Session("mMonthList") = mMonthList

		mMissingMonthData = (From c In mMonthList
							 Select New With {c.Id, Today.Year, .MonthYear = c.MonthYear + " " + Today.Year.ToString, .RecordCount = 0})
		Session("mMissingMonthData") = mMissingMonthData

		'Weekly Work Order Higher/Lesser Percentage
		GetWOPercentage(ThisWeekStartDate, ThisWeekEndDate, LastWeekStartDate, LastWeekEndDate)

		'Weekly Requisitions Higher/Lesser Percentage
		GetRequisitions(ThisWeekStartDate, ThisWeekEndDate, LastWeekStartDate, LastWeekEndDate)

		'Weekly Issues Higher/Lesser Percentage
		GetIssues(ThisWeekStartDate, ThisWeekEndDate, LastWeekStartDate, LastWeekEndDate)


		'mWOStatusList = nWOStatusList.GetWOStatusListList(, "(ALL)")
		'cmbStatus.DataSource = mWOStatusList
		'Session("mWOStatusList") = mWOStatusList
		'cmbStatus.SelectedIndex = 0
		'' cmbStatus.DataBind()
		'hdnStatus.Value = 0

		'mCustomerList = VendorList.GetVendorstList(0, , , , , , "(ALL)", True)
		'cmbCustomerList.DataSource = mCustomerList
		'' cmbCustomerList.DataBind()
		'Session("mCustomerList") = mCustomerList
		'hdnCustomer.Value = Guid.Empty.ToString

		'Employee
		mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo(ExcludeNotWorkingEmployees:=True)
		cmbEmployee.DataSource = mEmployeeListForCombo
		Session("mEmployeeListForCombo") = mEmployeeListForCombo

		DataBind()

		SetPieGraph()
	End Sub
#End Region

#Region " Events "
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		GetSession()
		If Not IsPostBack Then
			Session.Remove("MiddleFrame")
			DatafieldBind()
			GetMonthlyWorkOrder()
			GetMonthlyEmployeeWiseWorkDone(New Guid(cmbEmployee.SelectedValue.ToString))
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FullCalendarDueFunc", "FullCalendarDueFunc();", True)
			' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncRequisitionDet", "FuncRequisitionDet('" + chkNotIssuedItems.Checked.ToString + "', '" + chkNotReceivedItems.Checked.ToString + "');", True)
			upnlRequisitionItemStatus.Update()
			'Added for Seasonal Greetings
			If Session("IsFromLogin") = "True" Then
				Session.Remove("IsFromLogin")
				Dim mCompanyDetailForGreetings As New CompanyDetailForGreetings
				mCompanyDetailForGreetings = CompanyDetailForGreetings.GetCompanyDetail("", "", "", "", "", "", "")
				Session("mCompanyDetailForGreetings") = mCompanyDetailForGreetings
				If mCompanyDetailForGreetings IsNot Nothing Then

					If mCompanyDetailForGreetings.ShowGreeting And IsDate(mCompanyDetailForGreetings.FromDateFormatted.ToString) And IsDate(mCompanyDetailForGreetings.FromDateFormatted.ToString) Then
						If CDate(Today.Date) >= CDate(mCompanyDetailForGreetings.FromDateFormatted.ToString) And CDate(Today.Date) <= CDate(mCompanyDetailForGreetings.ToDateFormatted.ToString) Then
							ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenGreetingsWindow", "OpenGreetingsWindow();", True)
							GoTo SkipLoop
						End If
					End If
					Session.Remove("mCompanyDetailForGreetings")
				End If
			End If
			'End
SkipLoop:
		End If
	End Sub
	Private Sub chkNotIssuedItems_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkNotIssuedItems.CheckedChanged, chkNotReceivedItems.CheckedChanged
		GetRequisitionDetails()
		upnlRequisitionItemStatus.Update()
	End Sub
	'Private Sub cmbStatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbStatus.SelectedIndexChanged
	'    hdnStatus.Value = cmbStatus.SelectedValue
	'    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FullCalendarDueFunc", "FullCalendarDueFunc();", True)

	'End Sub
	Private Sub cmbEmployee_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbEmployee.SelectedIndexChanged
		GetMonthlyEmployeeWiseWorkDone(New Guid(cmbEmployee.SelectedValue.ToString))

	End Sub
#End Region

#Region "Web Methods"

	'    <WebMethod(EnableSession:=True)> _
	<System.Web.Services.WebMethod()> _
	Public Shared Function TestOnWebService(WOStatusID As String, CustomerID As String, month As String, year As String) As String

		' Dim mnWOPlannedList As nWOList
		Dim mnWOPlannedList As nWOListForPlanCalendar
		Dim StartDateM As New SmartDate
		Dim EndDateM As New SmartDate

		'StartDateM = New SmartDate(DateAdd(DateInterval.Month, 0, DateSerial(Val(year), Val(month) + 1, 1)), False)
		'EndDateM = New SmartDate(CStr(DateSerial(StartDateM.Date.Year, StartDateM.Date.Month, DateTime.DaysInMonth(StartDateM.Date.Year, StartDateM.Date.Month))), False)
		' If Not tmpWOstatusID = Val(WOStatusID) Or Not (tmpCustomerID.Equals(New Guid(CustomerID))) Or Not (tmpMonth = Val(month) Or Not (tmpYear = Val(year))) Then
		'mnWOPlannedList = nWOList.GetWOList(WOStatusID:=4)
		'tmpWOstatusID = WOStatusID
		'tmpCustomerID = New Guid(CustomerID)
		'tmpMonth = Val(month)
		'tmpYear = Val(year)
		If PlannedList = "" Then

			Dim TodayDate As SmartDate = New SmartDate(CStr(DateSerial(Today.Date.Year, Today.Date.Month, DateTime.DaysInMonth(Today.Date.Year, Today.Date.Month))), False)
			Dim LastYearStartDate As SmartDate = New SmartDate(Today.AddYears(-1).ToString)
			mnWOPlannedList = nWOListForPlanCalendar.GetWOListForPlanCalendar(FromDate:=LastYearStartDate.ToString, ToDate:=TodayDate.ToString)
			PlannedList = New JavaScriptSerializer().Serialize(mnWOPlannedList)

		End If
		' End If




		PlannedList = PlannedList.Replace("HeaderCalender", "title").Replace("WOPlanedAndWODateCalender", "start").Replace("ID", "id")
		'  PlannedList = PlannedList.Replace("DescriptionCalender", "title").Replace("WOPlanedAndWODateCalender", "start").Replace("ID", "id")
		Return PlannedList
	End Function

#Region " Enumeration "
	Public Enum Rights
		[New] = 1
		Edit = 2
		Delete = 3
		Save = 4
		View = 5
		Print = 6
		Authorized = 7
		OpenDetailPage = 8
	End Enum
#End Region

	Public Shared Function IsInRole(ByVal CheckFor As Rights, ByVal mnWO As nWO) As Boolean
		Dim IsInRoleString As String = ""
		' Dim mUserList As UserList = UserList.GetUserList(HttpContext.Current.User.Identity.Name, , HttpContext.Current.User.Identity.Name)
		Dim mUserModuleFunctionList As UserModuleFunctionList = UserModuleFunctionList.GetUserModuleFunctionList(HttpContext.Current.User.Identity.Name)

		If mUserModuleFunctionList.Contains("WOCreateCAMO") And (mnWO.WOStatus = "Open" Or mnWO.WOStatus = "Authorized") Then
			'IsInRoleString = "CAMOWOCreate"
			If mnWO.TransTypeID = Trans.WO145 Then
				IsInRoleString = "WOCreate"
			Else
				IsInRoleString = "CAMOWOCreate"
			End If
			HttpContext.Current.Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID
		ElseIf mUserModuleFunctionList.Contains("WOPlanning") And mnWO.WOStatus = "Planned" Then
			IsInRoleString = "WOPlanning"
			HttpContext.Current.Session("MiddleFrame") = "wfnWOPlannedList.aspx?"
		ElseIf mUserModuleFunctionList.Contains("WOCompletion") And (mnWO.WOStatus = "PPC Completed") Then
			IsInRoleString = "WOCompletion"
			HttpContext.Current.Session("MiddleFrame") = "wfnWOCompletionList.aspx?"
		ElseIf mUserModuleFunctionList.Contains("WOQCApproval") And (mnWO.IsQCStatusApprovedStatus = "QC Approved" Or mnWO.IsQCStatusApprovedStatus = "QC Rejected") Then
			IsInRoleString = "WOQCApproval"
			HttpContext.Current.Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?"
		ElseIf mUserModuleFunctionList.Contains("WOCAMOUpdate") And (mnWO.CAMOStatus = "CAMO Updated") Then
			IsInRoleString = "WOCAMOUpdate"
			HttpContext.Current.Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1"
		ElseIf mUserModuleFunctionList.Contains("WOBilling") And (mnWO.BillingStatus = "Billing Done" Or mnWO.BillingStatus = "Not Required" Or mnWO.BillingStatus = "None") Then
			IsInRoleString = "WOBilling"
			HttpContext.Current.Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0"
		End If
		Select Case CheckFor
			Case Rights.View
				Return HttpContext.Current.User.IsInRole(IsInRoleString + "View")
			Case Rights.[New]
				Return HttpContext.Current.User.IsInRole(IsInRoleString + "New")
			Case Rights.Edit
				Return HttpContext.Current.User.IsInRole(IsInRoleString + "Edit")
			Case Rights.Save
				Return (HttpContext.Current.User.IsInRole(IsInRoleString + "New") Or HttpContext.Current.User.IsInRole(IsInRoleString + "Edit"))
			Case Rights.Delete
				Return HttpContext.Current.User.IsInRole(IsInRoleString + "Delete")
			Case Rights.Print
				Return HttpContext.Current.User.IsInRole(IsInRoleString + "Print")
			Case Rights.Authorized
				Return HttpContext.Current.User.IsInRole(IsInRoleString + "Authorized")
			Case Rights.OpenDetailPage
				Return (HttpContext.Current.User.IsInRole(IsInRoleString + "View") And HttpContext.Current.User.IsInRole(IsInRoleString + "Edit"))
		End Select

		'If AppSettings("ClientCode") = "IND" Then
		'    If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID Then
		'        If mnWO.TransTypeID = Trans.WO145 Then
		'            IsInRoleString = "WOCreate"
		'        Else
		'            IsInRoleString = "CAMOWOCreate"
		'        End If
		'    ElseIf Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Then
		'        IsInRoleString = "WOPlanning"
		'    ElseIf Session("MiddleFrame") = "wfnWOExecutionList.aspx" Then
		'        IsInRoleString = "WOExecution"
		'    ElseIf Session("MiddleFrame") = "wfnWOCompletionList.aspx?" Then
		'        IsInRoleString = "WOCompletion"
		'    ElseIf Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" Then
		'        IsInRoleString = "WOQCApproval"
		'    ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1" Then
		'        IsInRoleString = "WOCAMOUpdate"
		'    ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0" Then
		'        IsInRoleString = "WOBilling"
		'    End If

		'Else
		'    If mnWO.TransTypeID = Trans.WO145 Then
		'        IsInRoleString = "WorkOrder"
		'    Else
		'        IsInRoleString = "CAMOWO"
		'    End If
		'End If


	End Function
	<System.Web.Services.WebMethod()> _
	Public Shared Function GetWODet(WOID As String) As Boolean

		Dim mnWO As nWO = nWO.GetWO(New Guid(WOID))
		'If IsInRole(Rights.Save, mnWO) Then

		'End If

		HttpContext.Current.Session("mnWO") = mnWO
		If IsInRole(Rights.OpenDetailPage, mnWO) Then
			Return True
		Else
			Return False
		End If
		' Return IsInRole(Rights.Save, mnWO)
	End Function

	'  <WebMethod(EnableSession:=True)> _
	<System.Web.Services.WebMethod()> _
	Public Shared Function RequisitionItemStatusList(NotIssued As String, NotReceived As String) As Object
		Dim mrptRequisitionItemStatusList As rptRequisitionItemStatusList

		If UCase(NotIssued) = UCase("true") Then
			mrptRequisitionItemStatusList = rptRequisitionItemStatusList.GetRequisitionItemStatusList(TransTypeID:=Util.Trans.EngineeringRequisition, NotIssued:=1)
		Else
			mrptRequisitionItemStatusList = rptRequisitionItemStatusList.GetRequisitionItemStatusList(TransTypeID:=Util.Trans.EngineeringRequisition)
		End If

		Dim NotReceivedItems As Object

		If UCase(NotReceived) = UCase("true") Then
			NotReceivedItems = (From c In mrptRequisitionItemStatusList
								Where c.ReceiptDetails = ""
								Select c)
			Return NotReceivedItems
		End If

		Return mrptRequisitionItemStatusList
	End Function
#End Region




End Class