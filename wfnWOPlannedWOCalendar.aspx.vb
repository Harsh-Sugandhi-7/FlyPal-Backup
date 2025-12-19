'CReated By: Saylee
'Dated:     3-Jul-2019

Imports System.Web.Script.Serialization

Public Class wfnWOPlannedWOCalendar
	Inherits System.Web.UI.Page


#Region " Variable Declaration "
	Shared Count As Integer = 0
	Shared PlannedList As String = ""
	Public mWOStatusList As nWOStatusList
	Shared tmpWOstatusID As Integer = 0
	Shared tmpCustomerID As Guid = Guid.Empty
	Shared tmpMonth As Integer = 0
	Shared tmpYear As Integer = 0
	Dim mCustomerList As VendorList
#End Region

#Region " Methods "
	Private Sub GetSession()
		Count = Session("Count")
		mWOStatusList = Session("mWOStatusList")
		mCustomerList = Session("mCustomerList")
	End Sub
	Private Sub ClearAll()
		If InStr(Session("MiddleFrame"), "wfnWOPlannedWOCalendar.aspx?") <= 0 Then
			PlannedList = ""
			Session.Remove("mWOStatusList")
			Session.Remove("mCustomerList")
		End If


	End Sub
	Private Sub DatafieldBind()
		mWOStatusList = nWOStatusList.GetWOStatusListList(, "(ALL)")
		cmbStatus.DataSource = mWOStatusList
		Session("mWOStatusList") = mWOStatusList
		cmbStatus.SelectedIndex = 1
		cmbStatus.DataBind()
		hdnStatus.Value = 1

		mCustomerList = VendorList.GetVendorstList(0, , , , , , "(ALL)", True)
		cmbCustomerList.DataSource = mCustomerList
		cmbCustomerList.DataBind()
		Session("mCustomerList") = mCustomerList
		hdnCustomer.Value = Guid.Empty.ToString
	End Sub
#End Region

#Region " Events "
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		ClearAll()
		GetSession()
		If Not IsPostBack Then
			Session("MiddleFrame") = "wfnWOPlannedWOCalendar.aspx?"
			DatafieldBind()
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FullCalendarDueFunc", "FullCalendarDueFunc();", True)

		End If
		'''Dim mDueLimits As DueLimits
		'''Dim mPerDayLimits As PerDayLimits

		'''Dim mrptModelMonitorDueStatusList As MaintenanceActiivtyStatusList

		'''Dim mServiceTypeList As PartMonitorServiceTypeList
		'''Dim mInspectionTypeList As ModelMonitorInspTypeList
		'''Dim mModificationTypeList As ModelMonitorModTypeList

		'''Dim MonitorServiceTypeIDs As String = ""
		'''Dim MonitorInspTypeIDs As String = ""
		'''Dim MonitorModTypeIDs As String = ""

		'''Dim IsSerSelect As Boolean = False
		'''Dim IsModSelect As Boolean = False
		'''Dim IsInsSelect As Boolean = True

		'''mDueLimits = DueLimits.GetDueLimits(New Guid("2b070bc9-09c9-4b47-8405-d6098c0aeb90"))
		'''mPerDayLimits = PerDayLimits.GetPerDayLimits(New Guid("2b070bc9-09c9-4b47-8405-d6098c0aeb90"))

		'''mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()

		'''For i As Integer = 0 To mInspectionTypeList.Count - 1
		'''    If MonitorInspTypeIDs = "" Then
		'''        MonitorInspTypeIDs = mInspectionTypeList(i).ID.ToString
		'''    Else
		'''        MonitorInspTypeIDs = MonitorInspTypeIDs + "," + mInspectionTypeList(i).ID.ToString
		'''    End If
		'''Next

		'''mrptModelMonitorDueStatusList = MaintenanceActiivtyStatusList.GetDueStatusList(Today.Date.ToString, New Guid("2b070bc9-09c9-4b47-8405-d6098c0aeb90"), New Guid(Guid.Empty.ToString), mDueLimits, 30, MonitorInspTypeIDs, , MonitorServiceTypeIDs, , MonitorModTypeIDs, , IsInsSelect, IsSerSelect, IsModSelect, True, 0, True, mPerDayLimits)


		''''Dim tmp As MaintenanceActiivtyStatusList
		'''Dim tmprptModelMonitorDueStatusList As List(Of MaintenanceActiivtyStatusList.MaintenanceActiivtyStatusListInfo) = New List(Of MaintenanceActiivtyStatusList.MaintenanceActiivtyStatusListInfo)
		'''tmprptModelMonitorDueStatusList = (From c As MaintenanceActiivtyStatusList.MaintenanceActiivtyStatusListInfo In mrptModelMonitorDueStatusList.AsParallel
		'''                Where c.IsDue = "true" And c.IsApplicable = True And (c.MonitorTypeID <> "3" Or (c.MonitorTypeID = "1" And c.IsDone = "False"))
		'''                Select c).ToList

		'''Dim DueValues As String = New JavaScriptSerializer().Serialize(tmprptModelMonitorDueStatusList)
		'''DueValues = DueValues.Replace("Description", "title").Replace("EstimatedDateFormatted", "start")
		''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FullCalendarDueFunc", "FullCalendarDueFunc('" + DueValues.ToString + "');", True)
		'''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FullCalendarDueFunc", "FullCalendarDueFunc();", True)


	End Sub

	Protected Sub Timer1_Tick(ByVal sender As Object, ByVal e As EventArgs)
		Try
			If CInt(Session("Count")) > 0 Then
				'Count = CInt(Session("Count")) + 1
				'hdncount.Value = Count
				'Session("Count") = Count
				If CInt(Session("Count")) = 3 Then
					Count = 0
					Session("Count") = Count
					Session("ChangeForm") = "ChangeForm"
					'Timer1.Enabled = False
				End If
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FullCalendarDueFunc", "FullCalendarDueFunc();", True)
				Exit Sub
			End If
		Catch ex As Exception

		End Try
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

		StartDateM = New SmartDate(DateAdd(DateInterval.Month, 0, DateSerial(Val(year), Val(month) + 1, 1)), False)
		EndDateM = New SmartDate(CStr(DateSerial(StartDateM.Date.Year, StartDateM.Date.Month, DateTime.DaysInMonth(StartDateM.Date.Year, StartDateM.Date.Month))), False)
		If Not tmpWOstatusID = Val(WOStatusID) Or Not (tmpCustomerID.Equals(New Guid(CustomerID))) Or Not (tmpMonth = Val(month) Or Not (tmpYear = Val(year))) Then
			'mnWOPlannedList = nWOList.GetWOList(WOStatusID:=4)
			tmpWOstatusID = WOStatusID
			tmpCustomerID = New Guid(CustomerID)
			tmpMonth = Val(month)
			tmpYear = Val(year)
			mnWOPlannedList = nWOListForPlanCalendar.GetWOListForPlanCalendar(WOStatusID:=tmpWOstatusID, CustomerID:=CustomerID, FromDate:=StartDateM.ToString, ToDate:=EndDateM.ToString)
			PlannedList = New JavaScriptSerializer().Serialize(mnWOPlannedList)
		End If




		Dim jss = New JavaScriptSerializer()

		Dim data = jss.Deserialize(Of Object)(PlannedList) 'JsonConvert.DeserializeObject(Of MaintenanceActiivtyStatusList.MaintenanceActiivtyStatusListInfo)(DueValues)


		PlannedList = PlannedList.Replace("HeaderCalender", "title").Replace("WOPlanedAndWODateCalender", "start").Replace("ID", "id")
		'  PlannedList = PlannedList.Replace("DescriptionCalender", "title").Replace("WOPlanedAndWODateCalender", "start").Replace("ID", "id")
		Return PlannedList
	End Function
#End Region

End Class