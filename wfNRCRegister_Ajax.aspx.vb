Imports System.Collections.Generic
Imports System.Linq
Public Class wfNRCRegister_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Protected mNRCRegister As NRCRegister = Nothing
	Protected mMachineNameValueList As MachineNameValueList
	Protected mATAList As ATAList
	Protected mEmployeeStatus As EmployeeStatus
	Protected mEmployee As Employee
	Dim mNRCDetailForEventLog As String = String.Empty
	Dim message As String = ""
	Protected mMachine As Machine
	Dim mEmployeeList As EmployeeList 'Added By Vikrant On 01-Oct-2020 TO solve chrome browser issue.
#End Region

#Region " Helper Methods "
	Private Sub GetSession()
		mEmployeeList = Session("mEmployeeList")
	End Sub
	Public Sub SetValues()
		lblFrom.Text = "From Date : " & txtFromDate.Text
		lblTo.Text = "To Date : " & txtToDate.Text
		lblRegNo.Text = "Aircraft : " & IIf(cmbAircraftList.SelectedIndex > 0, cmbAircraftList.SelectedItem.Text, "")
		lblRaised.Text = IIf(AppSettings("ClientCode") = "APFT" Or
									  AppSettings("ClientCode") = "AAP", "Reported By : ", "Raised By : ") & txtRaisedBy.Text.Trim
		lblATAC.Text = "ATA : " & IIf(cmbATAChapter.SelectedIndex > 0, cmbATAChapter.SelectedItem.Text, "")
		lblAMEName.Text = "Done By AME : " & txtDoneByAME.Text.Trim
		lblTechName.Text = "Done By Tech : " & txtDoneByTech.Text.Trim
		lblPlaceName.Text = "Place : " & txtPlace.Text.Trim
		lblObser.Text = IIf(AppSettings("ClientCode") = "APFT" Or
									 AppSettings("ClientCode") = "AAP", "Defect Reported : ", "Observation : ") & txtObservation.Text.Trim
		lblRec.Text = IIf(AppSettings("ClientCode") = "APFT" Or
								   AppSettings("ClientCode") = "AAP", "Rectification Action Taken : ", "Rectification : ") & txtRectification.Text.Trim
	End Sub
	Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
		Dim da As New CSLA.Data.ObjectAdapter
		Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
		Dim rpt As NRCRegister
		Dim mCompanyDetail As New CompanyDetail
		SetValues()
		If AppSettings("ClientCode") = "APFT" Or
		   AppSettings("ClientCode") = "AAP" Then 'Added By Vikrant On 28-Sep-2020 For APFT28092020
			myReport = New crptNRCRegisterAPFT
		Else 'End
			myReport = New crptNRCRegister
		End If

		rpt = NRCRegister.GetNRCRegister(FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text, MachineID:=cmbAircraftList.SelectedValue.ToString _
										  , ATAID:=cmbATAChapter.SelectedValue.ToString, RaisedByEmpID:=mEmployeeList(txtRaisedBy.Text, "").ID.ToString, _
										  DoneByAMEID:=mEmployeeList(txtDoneByAME.Text, "").ID.ToString, _
										   DoneByTechID:=mEmployeeList(txtDoneByTech.Text, "").ID.ToString, _
										   Place:=txtPlace.Text.Trim, Observation:=txtObservation.Text.Trim, Rectification:=txtRectification.Text.Trim)
		If rpt.Count <= 0 Then
			MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
			Exit Sub
		Else
			RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1355)
		End If
		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
			   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
			   mCompanyDetail.WebSite, IIf(AppSettings("ClientCode") = "APFT" Or
																	 AppSettings("ClientCode") = "AAP", "Defect Register", "NRC Register"), txtFromDate.Text, txtToDate.Text, cmbAircraftList.SelectedItem.Text, txtRaisedBy.Text.Trim,
			   cmbATAChapter.SelectedItem.Text, AppSettings("Product Version"), AppSettings("SINote"), txtDoneByAME.Text.Trim, txtDoneByTech.Text.Trim,
			   txtPlace.Text.Trim, txtObservation.Text.Trim, AppSettings("Logo"), txtRectification.Text.Trim)

		Dim ds As New dsNRCRegister
		ds.Clear()
		Dim mrptImage As rptImage = rptImage.GetImage(ds)
		da.Fill(ds, rpt)
		da.Fill(ds, Report)
		da.Fill(ds, mrptImage)
		myReport.SetDataSource(ds)
		Session("CrystalReport") = myReport
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
		MarkLog(Util.Action.Print, "NRCRegister", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
	End Sub
	Private Sub ControlInVisible()
		lblFrom.Visible = False
		lblTo.Visible = False
		lblRegNo.Visible = False
		lblRaised.Visible = False
		lblATAC.Visible = False
		lblAMEName.Visible = False
		lblTechName.Visible = False
		lblPlaceName.Visible = False
		lblObser.Visible = False
		lblRec.Visible = False
	End Sub
	Private Sub ControlVisible()
		lblFrom.Visible = True
		lblTo.Visible = True
		lblRegNo.Visible = True
		lblRaised.Visible = True
		lblATAC.Visible = True
		lblAMEName.Visible = True
		lblTechName.Visible = True
		lblPlaceName.Visible = True
		lblObser.Visible = True
		lblRec.Visible = True
	End Sub
	Private Sub DataFieldBind()
		mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , True, "(All)", , True)
		cmbAircraftList.DataSource = mMachineNameValueList

		mATAList = ATAList.GetATAList("", "(All)")
		cmbATAChapter.DataSource = mATAList

		mEmployeeList = EmployeeList.GetEmployeeList()
		Session("mEmployeeList") = mEmployeeList
		DataBind()
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Ok
					If MSGBoxCtrl.Sender = "DoneByAME" Then

					End If '
			End Select
		End If
	End Sub
#End Region

#Region "Events"

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not Page.IsPostBack Then
			DataFieldBind()
			txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
			txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
			ControlInVisible()
		End If
	End Sub
	Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
		If Not IsValid() Then upnlValidationSummary.Update() : Exit Sub
		SetReport(False)
	End Sub
	Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
		SetValues()
		ControlVisible()
		upnlDisplaySearchCriteria.Update()
	End Sub
	Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
		Session("MiddleFrame") = ""
		Session.Remove("mEmployeeList")
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
#End Region

#Region " Service Methods "
	<System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
	Public Shared Function GetTextList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim DistinctTextList As DistinctTextListAutoComplete
		DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, 24)
		If count = 0 Then
			Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
		Else
			Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
		End If
	End Function
	<System.Web.Services.WebMethod(), System.Web.Script.Services.ScriptMethod()>
	Public Shared Function GetLicenseNoList(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
		Dim list As LicenseNoListWithEmployee
		list = LicenseNoListWithEmployee.GetLicenseNoList(prefixText)

		If count = 0 Then
			Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In list
					Select c.LicenseNoEmpName).ToList
		Else
			Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In list
					Select c.LicenseNoEmpName).Take(count).ToList
		End If
	End Function
	<System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
	Public Shared Function GetEmployeeList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim itemlist As EmpNoNameAutoComplete
		itemlist = EmpNoNameAutoComplete.GeEmpNoNameList(prefixText)
		If count = 0 Then
			Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).ToArray
		Else
			Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).Take(count).ToArray
		End If
	End Function
#End Region

End Class