Imports System.Collections.Generic
Imports System.Linq
Public Class wfNRC_Ajax
	Inherits System.Web.UI.Page

#Region " Enumeration "
	Private Enum Rights
		[New] = 1
		Edit = 2
		Delete = 3
		Save = 4
		View = 5
		Print = 6
	End Enum
#End Region

#Region " Variable Declaration "
	Protected mNRC As NRC
	Protected mMachineNameValueList As MachineNameValueList
	Protected mATAList As ATAList
	Protected mEmployeeStatus As EmployeeStatus
	Protected mEmployee As Employee
	Dim mNRCDetailForEventLog As String = String.Empty
	Dim message As String = ""
	Protected mMachine As Machine
	Dim mEmployeeList As EmployeeList
	Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
	Private Sub GetSession()
		mNRC = Session("mNRC")
		mMachineNameValueList = Session("mMachineNameValueList")
		mEmployeeList = Session("mEmployeeList")
		mModuleList = Session("mModuleList")
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mNRC")
		Session.Remove("mMachineNameValueList")
		Session.Remove("mEmployeeList")
	End Sub
	Private Sub ControlVisibility()
	End Sub
	Private Sub SetReport()
		Dim da As New CSLA.Data.ObjectAdapter
		Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
		Dim mCompanyDetail As New CompanyDetail
		Dim ds As New dsNRC
		Dim mFileAttachDoneByAME As FileAttach
		Dim mFileAttachInspectedByAME As FileAttach
		mNRC = NRC.GetNRC(mNRC.ID)

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Novo") Then
			myReport = New crptNRCforNOVO   'Added by Saylee on 26-Feb-2018 for NOVO26022018
		ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "STR") Then 'Added by Saylee on 13-Aug-2018  for StarAir13082018-1
			myReport = New crptNRCSTR
		ElseIf (AppSettings("ClientCode") = "PAS") Then 'Added by Prashant on 16-Mar-2021 PAS16032021
			myReport = New crptNRCForPassionAir
		ElseIf (AppSettings("ClientCode") = "SAA") Then 'Added by Prashant on 16-Jun-2022 
			myReport = New crptNRCSaurya
		Else
			myReport = New crptNRC
		End If

		'Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
		' mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
		' mCompanyDetail.WebSite, "", "", AppSettings("WO-NRCIssueRev"), "", AppSettings("ClientCode"), "", AppSettings("Product Version"), AppSettings("SINote"), "", , "", "", AppSettings("Logo"))
		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, _
									  mCompanyDetail.Email, website:=mCompanyDetail.WebSite, ReportName:="", SearchStr1:=mModuleList.Item("NRC").FormRevisionNo, _
									  SearchStr2:=AppSettings("WO-NRCIssueRev"), SearchStr3:="", SearchStr4:=AppSettings("ClientCode"), SearchStr5:="", _
									  ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:="", SearchStr7:="", SearchStr8:="", _
									  SearchStr9:="", SearchStr10:=AppSettings("Logo"), SearchStr11:="", SearchStr12:="", SearchStr13:="", SearchStr14:="", SearchStr15:="", _
									  Searchstr16:="")

		'Added by Shital on 01-Jul-2021
		If Not mNRC.DoneByAMEID = Guid.Empty Then
			mFileAttachDoneByAME = FileAttach.GetAttachment(mNRC.DoneByAMEID, , "DigitalSignature", ds, AppSettings("DOCPath"))
			da.Fill(ds, "FileAttach", mFileAttachDoneByAME)
		End If
		If Not mNRC.InspectedByAMEID = Guid.Empty Then
			mFileAttachInspectedByAME = FileAttach.GetAttachment(mNRC.InspectedByAMEID, , "DigitalSignature", ds, AppSettings("DOCPath"), "InspectedAMESignature")
			da.Fill(ds, "InspectedAMESignature", mFileAttachInspectedByAME)
		End If
		'***************************

		Dim mrptImage As rptImage = rptImage.GetImage(ds)
		da.Fill(ds, "rptImage", mrptImage)
		da.Fill(ds, "ReportData", Report)
		da.Fill(ds, "NRC", mNRC)
		da.Fill(ds, "NRCJob", mNRC.NRCJobs)
		da.Fill(ds, "NRCPartOnOff", mNRC.NRCPartOnOffs)



		myReport.SetDataSource(ds)
		Session("CrystalReport") = myReport
	End Sub
	Private Sub DataFieldBind()
		mMachineNameValueList = MachineNameValueList.GetMachineList(mNRC.NRCDateFormatted.ToString, , , , , , , True, "(SELECT)", , True)
		cmbAircraftList.DataSource = mMachineNameValueList
		Session("mMachineNameValueList") = mMachineNameValueList

		txtRaisedBy.Text = mNRC.RaisedByEmpName
		txtDoneByAME.Text = mNRC.DoneByAMEName
		txtDoneByTech.Text = mNRC.DoneByTechName
		txtInspectedByAME.Text = mNRC.InspectedByAMEName

		'Commented & Added By Vikrant On 26-Sep-2018 For STR26092018
		' txtNRCDate.Text = mNRC.NRCDateFormatted.ToString
		If mNRC.NRCDate IsNot System.DBNull.Value Then
			'calDeparture.Text = Format(CDate(mLogDetail.SouLocalDateTime), AppSettings("DateTimeFormatLOG"))
			txtNRCDate.Text = Format(CDate(mNRC.NRCDate), AppSettings("DateFormat"))
			txtTime.Text = Format(CDate(mNRC.NRCDate), AppSettings("TimeFormat"))
		Else
			txtNRCDate.Text = ""
			txtTime.Text = ""
		End If
		'End
		txtDoneOnDate.Text = mNRC.DoneOnDateFormatted.ToString
		txtInspectionDate.Text = mNRC.InspectionDateFormatted.ToString

		mATAList = ATAList.GetATAList("", "(SELECT)")
		cmbATAChapter.DataSource = mATAList

		dgNRCJobs.DataSource = mNRC.NRCJobs
		dgNRCPartOnOff.DataSource = mNRC.NRCPartOnOffs
		dgNRCSpare.DataSource = mNRC.NRCSpares

		'mEmployeeList = EmployeeList.GetEmployeeList()
		'Session("mEmployeeList") = mEmployeeList

		DataBind()
	End Sub
	Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		'If custValidator.ControlToValidate = "txtVisitNo" Then
		'    If mNRC.NRCStatusChilds.Count > 1 Then
		'        For i As Integer = 1 To mNRC.NRCStatusChilds.Count - 1
		'            If mNRC.NRCStatusChilds(i).StatusDate < mNRC.NRCStatusChilds(i - 1).StatusDate Then
		'                custValidator.ErrorMessage = mNRC.NRCStatusChilds(i).StatusName + " date[" + mNRC.NRCStatusChilds(i).StatusDateFormatted.ToString + "] should be greater than or equal to " + mNRC.NRCStatusChilds(i - 1).StatusName + " date[" + mNRC.NRCStatusChilds(i - 1).StatusDateFormatted.ToString + "]"
		'                e.IsValid = False
		'                Exit Sub
		'            End If
		'        Next
		'    End If
		'    e.IsValid = True
		'End If
		If custValidator.ControlToValidate = "txtDoneOnDate" Then
			If txtNRCDate.Text <> "" And txtDoneOnDate.Text <> "" Then
				If CDate(txtDoneOnDate.Text.ToString) < CDate(txtNRCDate.Text.ToString) Then
					custValidator.ErrorMessage = "Done On Date should be greater than or Equal to NRC Date."
					e.IsValid = False
				End If
			End If
		End If
	End Sub
	Private Function Save(ByVal StatuID As Integer) As Boolean
		Try
			If Not mNRC.NRCJobs.Count = 0 Then
				SetObject()
				If Not mNRC.IsValid Then
					Dim strMSG As String = ""
					'If Not mNRC.IsValid Then
					For i As Integer = 0 To mNRC.GetBrokenRulesCollection.Count - 1
						strMSG = strMSG + mNRC.GetBrokenRulesCollection(i).Description + "<Br>"
					Next
					'End If
					If strMSG.Trim <> "" Then
						cvControlValidator.ErrorMessage = strMSG
						cvControlValidator.IsValid = False
					End If
					upnlValidationsummary.Update()
					Return False
				End If
				mNRC.Save()
				Session("mNRC") = mNRC

				mNRCDetailForEventLog = mNRC.NRCNumber + " Dated : " + mNRC.NRCDateFormatted.ToString + " WorkShop : " + cmbAircraftList.SelectedItem.ToString


				MarkLog(Util.Action.Save, "NRC", mNRCDetailForEventLog, Util.ErrorType.NoError, mNRC.ID, EventLogID)
				DataFieldBind()
				Return True
			Else
				MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "NRC can not be saved without job.", MsgBoxStyle.OkOnly, "")
				Return False
				Exit Function
			End If

		Catch ex As SqlException
			If ex.Number = 8145 Then
				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
			ElseIf ex.Number = 2627 Then
				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
			ElseIf ex.Number = 2601 Then
				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
				Return False
				Exit Function
			End If
		Catch ex As Exception
			Throw ex
		Finally

		End Try
	End Function
	Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
		Dim IsInRoleString As String = ""
		IsInRoleString = "NRC"
		Select Case CheckFor
			Case Rights.View
				Return User.IsInRole(IsInRoleString + "View")
			Case Rights.[New]
				Return User.IsInRole(IsInRoleString + "New")
			Case Rights.Edit
				Return User.IsInRole(IsInRoleString + "Edit")
			Case Rights.Save
				Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
			Case Rights.Delete
				Return User.IsInRole(IsInRoleString + "Delete")
			Case Rights.Print
				Return User.IsInRole(IsInRoleString + "Print")
		End Select
	End Function
	Private Sub SetTitle()
		If mNRC.IsNew Then
			lblTitle.Text = "NRC [ New ]"
		Else
			lblTitle.Text = "NRC [" + mNRC.NRCNumber + "]"
		End If
	End Sub
	Private Sub DeleteNRCJob(ByVal Index As Int32)
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "DeleteNRCJob")
		mNRC.NRCJobs.CurrentIndex = Index - 1
		Session("mNRC") = mNRC
	End Sub
	Private Sub DeleteNRCPartOnOff(ByVal Index As Int32)
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "DeleteNRCPartOnOff")
		mNRC.NRCPartOnOffs.CurrentIndex = Index - 1
		Session("mNRC") = mNRC
	End Sub
	Private Sub DeleteNRCSpare(ByVal Index As Int32)
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "DeleteNRCSpare")
		mNRC.NRCSpares.CurrentIndex = Index - 1
		Session("mNRC") = mNRC
	End Sub
	Private Sub addAttributes()
		txtManHourAME.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtManHourAME').value,event)")
		txtManHourTech.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtManHourTech').value,event)")
		txtManHourOther.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtManHourOther').value,event)")
	End Sub
	Private Sub SetObject()
		If txtNRCDate.Text <> "" Then
			'Commented & Added By Vikrant On 26-Sep-2018 For STR26092018
			'mNRC.NRCDate = txtNRCDate.Text
			mNRC.NRCDate = CType(txtNRCDate.Text.ToString.Trim + " " + txtTime.Text.ToString.Trim, DateTime)
			'End
		Else
			mNRC.NRCDate = System.DBNull.Value
		End If
		mNRC.TransTypeID = 82
		mNRC.Text = Trim(txtText.Text)
		mNRC.No = Val(txtNo.Text)
		mNRC.WorkOrderNo = Trim(txtWONo.Text)
		mNRC.MachineID = New Guid(cmbAircraftList.SelectedValue)
		mNRC.ATAID = New Guid(cmbATAChapter.SelectedValue)
		'If hdnRaisedByEmpID.Value = "" Then
		'    'Do nothing
		'Else
		'    mNRC.RaisedByEmpID = New Guid(hdnRaisedByEmpID.Value)
		'End If

		mNRC.AcceptedThrough = Trim(txtAcceptedThrough.Text)
		mNRC.ManHourAME = txtManHourAME.Text
		mNRC.ManHourTech = txtManHourTech.Text
		mNRC.ManHourOther = txtManHourOther.Text
		mNRC.PrevNRCNo = txtPrevNRCNo.Text.Trim
		mNRC.Place = txtPlace.Text.Trim

		If hdnDoneByAMEID.Value = "" Then
			'Do nothing
		Else
			mNRC.DoneByAMEID = New Guid(hdnDoneByAMEID.Value)
		End If
		If hdnDoneByTechID.Value = "" Then
			'Do nothing
		Else
			mNRC.DoneByTechID = New Guid(hdnDoneByTechID.Value)
		End If
		If hdnInspectedByAMEID.Value = "" Then
			'Do nothing
		Else
			mNRC.InspectedByAMEID = New Guid(hdnInspectedByAMEID.Value)
		End If

		If txtDoneOnDate.Text <> "" Then
			mNRC.DoneOnDate = txtDoneOnDate.Text
		Else
			mNRC.DoneOnDate = System.DBNull.Value
		End If
		If txtInspectionDate.Text <> "" Then
			mNRC.InspectionDate = txtInspectionDate.Text
		Else
			mNRC.InspectionDate = System.DBNull.Value
		End If
		Session("mNRC") = mNRC
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Close" Then
						'Added Code
						Session("sender") = ""
						Page.Validate()
						If Page.IsValid Then
							If Save(1) = True Then
								RemoveSession()
								Response.Redirect("index.aspx")
							End If
						Else
							Session.Remove("IsValid")
							upnlValidationsummary.Update()
						End If
					ElseIf MSGBoxCtrl.Sender = "RemoveAttachment" Then
						Try
							Session("Sender") = ""
							Dim mNRC As NRC
							mNRC = CType(Session("mNRC"), NRC)
							Session("mNRC") = mNRC
						Catch ex As SqlException
							If ex.Number = 8145 Then
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 2627 Then
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 547 Then
								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
							End If
						End Try
					ElseIf MSGBoxCtrl.Sender = "DeleteNRCJob" Then
						mNRC = Session("mNRC")
						mNRC.NRCJobs.Remove(mNRC.NRCJobs.CurrentItem)
						Session("mNRC") = mNRC
						dgNRCJobs.DataSource = mNRC.NRCJobs
						dgNRCJobs.DataBind()
						upnlNRCJob.Update()
					ElseIf MSGBoxCtrl.Sender = "DeleteNRCPartOnOff" Then
						mNRC = Session("mNRC")
						mNRC.NRCPartOnOffs.Remove(mNRC.NRCPartOnOffs.CurrentItem)
						Session("mNRC") = mNRC
						dgNRCPartOnOff.DataSource = mNRC.NRCPartOnOffs
						dgNRCPartOnOff.DataBind()
						upnlNRCPartOnOff.Update()
					ElseIf MSGBoxCtrl.Sender = "DeleteNRCSpare" Then
						mNRC = Session("mNRC")
						mNRC.NRCSpares.Remove(mNRC.NRCSpares.CurrentItem)
						Session("mNRC") = mNRC
						dgNRCSpare.DataSource = mNRC.NRCSpares
						dgNRCSpare.DataBind()
						upnlNRCSpare.Update()
					End If
				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "Close" Then
						Session.Remove("IsValid")
						Session("Sender") = ""
						Response.Redirect("Index.aspx")
					End If
				Case MsgBoxResult.Ok
					If MSGBoxCtrl.Sender = "DoneByAME" Then
						txtDoneByAME.Text = ""
						txtDoneByAME.DataBind()
						txtDoneByAMELicenseNo.Text = ""
						txtDoneByAMELicenseNo.DataBind()
						mNRC.DoneByAMEID = Guid.Empty
						mNRC.DoneByAMEName = ""
						upnlPrevNRCNo.Update()
					ElseIf MSGBoxCtrl.Sender = "RaisedByEmp" Then
						txtRaisedBy.Text = ""
						mNRC.RaisedByEmpID = Guid.Empty
						mNRC.RaisedByEmpName = ""
						upnlNRCDetail.Update()
					ElseIf MSGBoxCtrl.Sender = "DoneByTech" Then
						txtDoneByTech.Text = ""
						txtDoneByTech.DataBind()
						txtDoneByTechLicenseNo.Text = ""
						txtDoneByTechLicenseNo.DataBind()
						mNRC.DoneByTechID = Guid.Empty
						mNRC.DoneByTechName = ""
						upnlPrevNRCNo.Update()
					ElseIf MSGBoxCtrl.Sender = "InspectedByAME" Then
						txtInspectedByAME.Text = ""
						txtInspectedByAME.DataBind()
						txtInspectedBy.Text = ""
						txtInspectedBy.DataBind()
						mNRC.InspectedByAMEID = Guid.Empty
						mNRC.InspectedByAMEName = ""
						upnlDuplicateInsp.Update()
					End If '
			End Select
		End If
	End Sub
	'Private Sub cmbCRSEmployeeList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbCRSEmployeeList.SelectedIndexChanged
	'    '  If cmbCRSEmployeeList.SelectedIndex > 0 Then
	'    Dim mCRSLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mEmployeeListForCombo(New Guid(cmbCRSEmployeeList.SelectedValue.ToString)).Name, User.Identity.Name, True, "(SELECT)", False)

	'    cmbCRSLicenseNo.DataSource = mCRSLicenseNoList
	'    cmbCRSLicenseNo.DataBind()
	'    mNRC.CRSLicenseNo = ""
	'    Session("mNRC") = mNRC
	'    '   Else
	'    '   cmbCRSLicenseNo.ClearSelection()
	'    '   End If
	'End Sub
	'Added By Vikrant On 26-Sep-2018 For STR26092018
	Private Function IsValidTime(ByVal TimeValue As String) As Boolean
		Dim TimeRegulerExpression As String = ""
		If (AppSettings("TimeFormat").IndexOf("tt") <> -1 Or AppSettings("TimeFormat").IndexOf("TT") <> -1) Then
			'TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm)$"    '12 Hour Format
			TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm|aM|pM)$"    '12 Hour Format
		Else
			TimeRegulerExpression = "^(([01][0-9])|(2[0-3])|([0-9])):[0-5][0-9]$"   '24 Hour Format
		End If

		If (System.Text.RegularExpressions.Regex.IsMatch(TimeValue, TimeRegulerExpression)) Then
			Return True
		Else
			Return False
		End If
	End Function
	'End
#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		addAttributes()
		If Not Page.IsPostBack Then
			DataFieldBind()
			SetTitle()
			ControlVisibility()

		End If
		'AppSettings Added By Vikrant On 07-Sep-2020 For ALL07092020
		cmbAddJobType.Items(1).Text = IIf(AppSettings("MELSnagNomenclature") = "True", "Add Job from ADD/Defect", "Add Job from MEL/Snag")
		'End
	End Sub
	Private Sub cmbAircraftList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraftList.SelectedIndexChanged
		If cmbAircraftList.SelectedIndex > 0 Then
			mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue.ToString))
			Dim mAircraftCurrentStatusList As AircraftCurrentStatusList = AircraftCurrentStatusList.GetAircraftDailyStatusMachineList(, mMachine.RegNo, , , , mNRC.NRCDate.ToString)
			Dim mAircraftCurrentStatusInfo As AircraftCurrentStatusList.AircraftCurrentStatusInfo
			If mMachine IsNot Nothing Then
				mNRC.ModelName = mMachine.AssemblyStatus.ModelName
				txtModelNo.DataBind()
				mNRC.SerialNo = mMachine.AssemblyStatus.Assembly.SerialNo
				txtSerialNo.DataBind()
				Dim EngCntr As Integer = 0
				For Each mAircraftCurrentStatusInfo In mAircraftCurrentStatusList
					If mAircraftCurrentStatusInfo.TypeID = 2 Then   'Engine
						EngCntr = EngCntr + 1
						If EngCntr = 1 Then
							mNRC.EngineModelName = mAircraftCurrentStatusInfo.ModelName
							txtEng1.DataBind()
							Exit For
						End If
					End If
				Next
			End If
		Else
			mNRC.ModelName = ""
			txtModelNo.DataBind()
			mNRC.SerialNo = ""
			txtSerialNo.DataBind()
			mNRC.EngineModelName = ""
			txtEng1.DataBind()
		End If
	End Sub
	Protected Sub txtRaisedBy_TextChanged(sender As Object, e As System.EventArgs)
		mEmployeeList = EmployeeList.GetEmployeeList()
		'If hdnRaisedByEmpID.Value <> "" Then
		'    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(hdnRaisedByEmpID.Value.ToString, mNRC.NRCDateFormatted.ToString)
		'    mEmployee = Employee.GetEmployee(New Guid(hdnRaisedByEmpID.Value.ToString))
		'    If mEmployeeStatus.Count > 0 Then
		'        If (mEmployeeStatus(0).Information <> "") Then
		'            message = mEmployeeStatus(0).Information
		'            MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "RaisedByEmp")
		'            Exit Sub
		'        End If
		'        mNRC.RaisedByEmpID = New Guid(hdnRaisedByEmpID.Value)
		'        mNRC.RaisedByEmpName = mEmployee.Name
		'        mNRC.RaisedByEmpNo = mEmployee.EmpNo
		'    Else
		'        mNRC.RaisedByEmpID = New Guid(hdnRaisedByEmpID.Value)
		'        mNRC.RaisedByEmpName = mEmployee.Name
		'        mNRC.RaisedByEmpNo = mEmployee.EmpNo
		'    End If
		'Else
		'    txtRaisedBy.Text = ""
		'    mNRC.RaisedByEmpID = Guid.Empty
		'    mNRC.RaisedByEmpName = ""
		'    mNRC.RaisedByEmpNo = ""
		'End If
		If mEmployeeList.Contains(txtRaisedBy.Text) Then
			mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeList(txtRaisedBy.Text, "").ID.ToString, mNRC.NRCDateFormatted.ToString)
			mEmployee = Employee.GetEmployee(mEmployeeList(txtRaisedBy.Text, "").ID)
			If mEmployeeStatus.Count > 0 Then
				If (mEmployeeStatus(0).Information <> "") Then
					message = mEmployeeStatus(0).Information
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "RaisedByEmp")
					Exit Sub
				End If
				mNRC.RaisedByEmpID = mEmployeeList(txtRaisedBy.Text, "").ID
				mNRC.RaisedByEmpName = mEmployee.Name
				mNRC.RaisedByEmpNo = mEmployee.EmpNo
			Else
				mNRC.RaisedByEmpID = mEmployeeList(txtRaisedBy.Text, "").ID
				mNRC.RaisedByEmpName = mEmployee.Name
				mNRC.RaisedByEmpNo = mEmployee.EmpNo
			End If
		Else
			txtRaisedBy.Text = ""
			mNRC.RaisedByEmpID = Guid.Empty
			mNRC.RaisedByEmpName = ""
			mNRC.RaisedByEmpNo = ""
		End If
		upnlNRCDetail.Update()
		Session("mNRC") = mNRC
	End Sub
	Protected Sub txtDoneByAME_TextChanged(sender As Object, e As System.EventArgs)
		mEmployeeList = EmployeeList.GetEmployeeList()
		'If hdnDoneByAMEID.Value <> "" Then
		'    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(hdnDoneByAMEID.Value.ToString, mNRC.NRCDateFormatted.ToString)
		'    mEmployee = Employee.GetEmployee(New Guid(hdnDoneByAMEID.Value.ToString))
		'    If mEmployeeStatus.Count > 0 Then
		'        If (mEmployeeStatus(0).Information <> "") Then
		'            message = mEmployeeStatus(0).Information
		'            MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "DoneByAME")
		'            Exit Sub
		'        End If
		'        mNRC.DoneByAMEID = New Guid(hdnDoneByAMEID.Value)
		'        mNRC.DoneByAMEName = mEmployee.Name
		'        mNRC.DoneByAMELicenseNo = mEmployee.LicenseNo
		'        mNRC.DoneByAMENo = mEmployee.EmpNo
		'        txtDoneByAMELicenseNo.Text = mEmployee.LicenseNo
		'        txtDoneByAMELicenseNo.DataBind()
		'    Else
		'        mNRC.DoneByAMEID = New Guid(hdnDoneByAMEID.Value)
		'        mNRC.DoneByAMEName = mEmployee.Name
		'        mNRC.DoneByAMENo = mEmployee.EmpNo
		'        mNRC.DoneByAMELicenseNo = mEmployee.LicenseNo
		'        txtDoneByAMELicenseNo.Text = mEmployee.LicenseNo
		'        txtDoneByAMELicenseNo.DataBind()
		'    End If
		'Else
		'    txtDoneByAME.Text = ""
		'    txtDoneByAMELicenseNo.Text = ""
		'    mNRC.DoneByAMEID = Guid.Empty
		'    mNRC.DoneByAMEName = ""
		'    mNRC.DoneByAMENo = ""
		'End If
		If mEmployeeList.Contains(txtDoneByAME.Text) Then
			mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeList(txtDoneByAME.Text, "").ID.ToString, mNRC.NRCDateFormatted.ToString)
			mEmployee = Employee.GetEmployee(mEmployeeList(txtDoneByAME.Text, "").ID)
			If mEmployeeStatus.Count > 0 Then
				If (mEmployeeStatus(0).Information <> "") Then
					message = mEmployeeStatus(0).Information
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "DoneByAME")
					Exit Sub
				End If
				mNRC.DoneByAMEID = mEmployeeList(txtDoneByAME.Text, "").ID
				mNRC.DoneByAMEName = mEmployee.Name
				mNRC.DoneByAMELicenseNo = mEmployee.LicenseNo
				mNRC.DoneByAMENo = mEmployee.EmpNo
				txtDoneByAMELicenseNo.Text = mEmployee.LicenseNo
				txtDoneByAMELicenseNo.DataBind()
			Else
				mNRC.DoneByAMEID = mEmployeeList(txtDoneByAME.Text, "").ID
				mNRC.DoneByAMEName = mEmployee.Name
				mNRC.DoneByAMENo = mEmployee.EmpNo
				mNRC.DoneByAMELicenseNo = mEmployee.LicenseNo
				txtDoneByAMELicenseNo.Text = mEmployee.LicenseNo
				txtDoneByAMELicenseNo.DataBind()
			End If
		Else
			txtDoneByAME.Text = ""
			txtDoneByAMELicenseNo.Text = ""
			mNRC.DoneByAMEID = Guid.Empty
			mNRC.DoneByAMEName = ""
			mNRC.DoneByAMENo = ""
		End If
		upnlPrevNRCNo.Update()
		Session("mNRC") = mNRC
	End Sub
	Protected Sub txtDoneByTech_TextChanged(sender As Object, e As System.EventArgs)
		mEmployeeList = EmployeeList.GetEmployeeList()
		'If hdnDoneByTechID.Value <> "" Then
		'    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(hdnDoneByTechID.Value.ToString, mNRC.NRCDateFormatted.ToString)
		'    mEmployee = Employee.GetEmployee(New Guid(hdnDoneByTechID.Value.ToString))
		'    If mEmployeeStatus.Count > 0 Then
		'        If (mEmployeeStatus(0).Information <> "") Then
		'            message = mEmployeeStatus(0).Information
		'            MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "DoneByTech")
		'            Exit Sub
		'        End If
		'        mNRC.DoneByTechID = New Guid(hdnDoneByTechID.Value)
		'        mNRC.DoneByTechName = mEmployee.Name
		'        mNRC.DoneByTechNo = mEmployee.EmpNo
		'        mNRC.DoneByTechLicenseNo = mEmployee.LicenseNo
		'        txtDoneByTechLicenseNo.Text = mEmployee.LicenseNo
		'        txtDoneByTechLicenseNo.DataBind()
		'    Else
		'        mNRC.DoneByTechID = New Guid(hdnDoneByTechID.Value)
		'        mNRC.DoneByTechLicenseNo = mEmployee.LicenseNo
		'        mNRC.DoneByTechNo = mEmployee.EmpNo
		'        txtDoneByTechLicenseNo.Text = mEmployee.LicenseNo
		'        txtDoneByTechLicenseNo.DataBind()
		'    End If
		'Else
		'    txtDoneByTech.Text = ""
		'    txtDoneByTechLicenseNo.Text = ""
		'    mNRC.DoneByTechID = Guid.Empty
		'    mNRC.DoneByTechName = ""
		'    mNRC.DoneByTechNo = ""
		'End If
		If mEmployeeList.Contains(txtDoneByTech.Text) Then
			mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeList(txtDoneByTech.Text, "").ID.ToString, mNRC.NRCDateFormatted.ToString)
			mEmployee = Employee.GetEmployee(mEmployeeList(txtDoneByTech.Text, "").ID)
			If mEmployeeStatus.Count > 0 Then
				If (mEmployeeStatus(0).Information <> "") Then
					message = mEmployeeStatus(0).Information
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "DoneByTech")
					Exit Sub
				End If
				mNRC.DoneByTechID = mEmployeeList(txtDoneByTech.Text, "").ID
				mNRC.DoneByTechName = mEmployee.Name
				mNRC.DoneByTechNo = mEmployee.EmpNo
				mNRC.DoneByTechLicenseNo = mEmployee.LicenseNo
				txtDoneByTechLicenseNo.Text = mEmployee.LicenseNo
				txtDoneByTechLicenseNo.DataBind()
			Else
				mNRC.DoneByTechID = mEmployeeList(txtDoneByTech.Text, "").ID
				mNRC.DoneByTechLicenseNo = mEmployee.LicenseNo
				mNRC.DoneByTechNo = mEmployee.EmpNo
				txtDoneByTechLicenseNo.Text = mEmployee.LicenseNo
				txtDoneByTechLicenseNo.DataBind()
			End If
		Else
			txtDoneByTech.Text = ""
			txtDoneByTechLicenseNo.Text = ""
			mNRC.DoneByTechID = Guid.Empty
			mNRC.DoneByTechName = ""
			mNRC.DoneByTechNo = ""
		End If
		upnlPrevNRCNo.Update()
		Session("mNRC") = mNRC
	End Sub
	Protected Sub txtInspectedByAME_TextChanged(sender As Object, e As System.EventArgs)
		mEmployeeList = EmployeeList.GetEmployeeList()
		'If hdnInspectedByAMEID.Value <> "" Then
		'    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(hdnInspectedByAMEID.Value.ToString, mNRC.NRCDateFormatted.ToString)
		'    mEmployee = Employee.GetEmployee(New Guid(hdnInspectedByAMEID.Value.ToString))
		'    If mEmployeeStatus.Count > 0 Then
		'        If (mEmployeeStatus(0).Information <> "") Then
		'            message = mEmployeeStatus(0).Information
		'            MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "InspectedByAME")
		'            Exit Sub
		'        End If
		'        mNRC.InspectedByAMEID = New Guid(hdnInspectedByAMEID.Value)
		'        mNRC.InspectedByAMEName = mEmployee.Name
		'        mNRC.InspectedByAMENo = mEmployee.EmpNo
		'        mNRC.InspectedByAMELicenseNo = mEmployee.LicenseNo
		'        txtInspectedBy.Text = mEmployee.LicenseNo
		'        txtInspectedBy.DataBind()
		'    Else
		'        mNRC.InspectedByAMEID = New Guid(hdnInspectedByAMEID.Value)
		'        mNRC.InspectedByAMEName = mEmployee.Name
		'        mNRC.InspectedByAMENo = mEmployee.EmpNo
		'        mNRC.InspectedByAMELicenseNo = mEmployee.LicenseNo
		'        txtInspectedBy.Text = mEmployee.LicenseNo
		'        txtInspectedBy.DataBind()
		'    End If
		'Else
		'    txtInspectedByAME.Text = ""
		'    txtInspectedBy.Text = ""
		'    mNRC.InspectedByAMEID = Guid.Empty
		'    mNRC.InspectedByAMEName = ""
		'    mNRC.InspectedByAMENo = ""
		'End If
		If mEmployeeList.Contains(txtInspectedByAME.Text) Then
			mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeList(txtInspectedByAME.Text, "").ID.ToString, mNRC.NRCDateFormatted.ToString)
			mEmployee = Employee.GetEmployee(mEmployeeList(txtInspectedByAME.Text, "").ID)
			If mEmployeeStatus.Count > 0 Then
				If (mEmployeeStatus(0).Information <> "") Then
					message = mEmployeeStatus(0).Information
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "InspectedByAME")
					Exit Sub
				End If
				mNRC.InspectedByAMEID = mEmployeeList(txtInspectedByAME.Text, "").ID
				mNRC.InspectedByAMEName = mEmployee.Name
				mNRC.InspectedByAMENo = mEmployee.EmpNo
				mNRC.InspectedByAMELicenseNo = mEmployee.LicenseNo
				txtInspectedBy.Text = mEmployee.LicenseNo
				txtInspectedBy.DataBind()
			Else
				mNRC.InspectedByAMEID = mEmployeeList(txtInspectedByAME.Text, "").ID
				mNRC.InspectedByAMEName = mEmployee.Name
				mNRC.InspectedByAMENo = mEmployee.EmpNo
				mNRC.InspectedByAMELicenseNo = mEmployee.LicenseNo
				txtInspectedBy.Text = mEmployee.LicenseNo
				txtInspectedBy.DataBind()
			End If
		Else
			txtInspectedByAME.Text = ""
			txtInspectedBy.Text = ""
			mNRC.InspectedByAMEID = Guid.Empty
			mNRC.InspectedByAMEName = ""
			mNRC.InspectedByAMENo = ""
		End If
		upnlDuplicateInsp.Update()
		Session("mNRC") = mNRC
	End Sub
	Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		SetReport()
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
	End Sub
	Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
		If (Not IsInRole(Rights.[New]) And mNRC.IsNew) Or (Not IsInRole(Rights.Edit) And Not mNRC.IsNew) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		If IsValid Then
			If Save(1) = True Then
				SetTitle()
				upnlTitle.Update()
				upnlNRCDetail.Update()
				upblManHours.Update()
				upblManHours.DataBind()
				upnlActionBtn.Update()
			End If
		Else
			upnlValidationsummary.Update()
		End If
	End Sub
	Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
		SetObject()
		If mNRC.IsDirty Then
			MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
		Else
			RemoveSession()
			Response.Redirect("index.aspx")
		End If
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	Private Sub btnAddNRCPartOnOff_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnAddNRCPartOnOff.Click
		If IsValid Then
			SetObject()
			mNRC.NRCPartOnOffs.Add(mNRC.ID)
			Session("mNRC") = mNRC
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenNRCPartOnOffWindow", "OpenNRCPartOnOffWindow();", True)
		Else
			upnlValidationsummary.Update()
		End If
	End Sub
	Private Sub dgNRCPartOnOff_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgNRCPartOnOff.RowCommand
		Select Case e.CommandName
			Case "EditRec"
				Dim Index As Integer = CInt(e.CommandArgument)
				Session("Edit") = True
				SetObject()
				mNRC.NRCPartOnOffs.CurrentIndex = Index - 1
				Session("mNRC") = mNRC
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenNRCPartOnOffWindow", "OpenNRCPartOnOffWindow();", True)
			Case "DeleteRec"
				Dim Index As Integer = CInt(e.CommandArgument)
				DeleteNRCPartOnOff(Index)
		End Select
	End Sub
	Private Sub hdnimgBtnNRCPartOnOff_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnNRCPartOnOff.Click
		dgNRCPartOnOff.DataSource = mNRC.NRCPartOnOffs
		dgNRCPartOnOff.DataBind()
		SetTitle()
		ControlVisibility()
		upnlNRCPartOnOff.Update()
	End Sub
	Private Sub dgNRCPartOnOff_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgNRCPartOnOff.PageIndexChanging
		dgNRCPartOnOff.PageIndex = e.NewPageIndex
		dgNRCPartOnOff.DataSource = mNRC.NRCPartOnOffs
		Session("mNRC") = mNRC
		dgNRCPartOnOff.DataBind()
	End Sub
	Private Sub btnNRCJob_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnNRCJob.Click
		If IsValid Then
			SetObject()
			mNRC.NRCJobs.Add(mNRC.ID)

			If cmbAddJobType.SelectedIndex = 0 Then 'Add New Job
				mNRC.NRCJobs.CurrentItem.WOJobTypeID = 1
				Session("mNRC") = mNRC
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenNRCWindow", "OpenNRCWindow();", True)
			Else 'Add Job from MEL/Snag
				mNRC.NRCJobs.CurrentItem.WOJobTypeID = 3
				Session("mNRC") = mNRC
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPendingMELSnagListWindow", "OpenPendingMELSnagListWindow();", True)
			End If
		Else
			upnlValidationsummary.Update()
		End If
	End Sub
	Private Sub dgNRCJobs_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgNRCJobs.PageIndexChanging
		dgNRCJobs.PageIndex = e.NewPageIndex
		dgNRCJobs.DataSource = mNRC.NRCJobs
		Session("mNRC") = mNRC
		dgNRCJobs.DataBind()
	End Sub
	Private Sub dgNRCJob_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgNRCJobs.RowCommand
		Select Case e.CommandName
			Case "EditRec"
				Dim Index As Integer = CInt(e.CommandArgument)
				Session("Edit") = True
				SetObject()
				mNRC.NRCJobs.CurrentIndex = Index - 1
				Session("mNRC") = mNRC
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenNRCWindow", "OpenNRCWindow();", True)
			Case "DeleteRec"
				Dim Index As Integer = CInt(e.CommandArgument)
				DeleteNRCJob(Index)
		End Select
	End Sub
	Private Sub hdnimgBtnNRCJob_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnNRCJob.Click, hdnBtnPendingMELSnagList.Click
		dgNRCJobs.DataSource = mNRC.NRCJobs
		dgNRCJobs.DataBind()
		SetTitle()
		ControlVisibility()
		upnlNRCJob.Update()
	End Sub
	Private Sub btnAddNRCSpare_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnAddNRCSpare.Click
		If IsValid Then
			Session.Remove("EditSpare")
			SetObject()
			mNRC.NRCSpares.Add(mNRC.ID)
			Session("mNRC") = mNRC
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenNRCSpareWindow", "OpenNRCSpareWindow();", True)
		Else
			upnlValidationsummary.Update()
		End If
	End Sub
	Private Sub dgNRCSpare_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgNRCSpare.PageIndexChanging
		dgNRCSpare.PageIndex = e.NewPageIndex
		dgNRCSpare.DataSource = mNRC.NRCSpares
		Session("mNRC") = mNRC
		dgNRCSpare.DataBind()
	End Sub
	Private Sub dgNRCSpare_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgNRCSpare.RowCommand
		Select Case e.CommandName
			Case "EditRec"
				Dim Index As Integer = CInt(e.CommandArgument)
				Session("EditSpare") = "True"
				SetObject()
				mNRC.NRCSpares.CurrentIndex = Index - 1
				Session("mNRC") = mNRC
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenNRCSpareWindow", "OpenNRCSpareWindow();", True)
			Case "DeleteRec"
				Dim Index As Integer = CInt(e.CommandArgument)
				DeleteNRCSpare(Index)
		End Select
	End Sub
	Private Sub hdnimgBtnNRCSpare_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnNRCSpare.Click
		dgNRCSpare.DataSource = mNRC.NRCSpares
		dgNRCSpare.DataBind()
		SetTitle()
		ControlVisibility()
		upnlNRCSpare.Update()
	End Sub
	Private Sub txtNRCDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtNRCDate.TextChanged
		mNRC.NRCDate = txtNRCDate.Text
		txtText.Text = mNRC.Text
		txtText.DataBind()
		Session("mNRC") = mNRC
	End Sub
	'Added By Vikrant On 26-Sep-2018 For STR26092018
	Private Sub txtTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTime.TextChanged
		If IsValidTime(txtTime.Text.ToString.Trim) = False Then
			txtTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			Dim DateTime As String = txtNRCDate.Text.ToString + " " + txtTime.Text.ToString.Trim
			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mNRC.NRCDateFormatted.ToString), New SmartDate(DateTime).Date) <> 0 Then
				mNRC.NRCDate = DateTime
				DataFieldBind()
			End If
		End If
	End Sub
	'End
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