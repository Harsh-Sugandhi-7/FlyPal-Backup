'******************************************
'CREATED By : Saylee
'Dated      : 1-Oct-2013
'******************************************


Imports System.Collections.Generic
Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Text

Imports Authenticate.Authentication

Imports Flypal.ModelListAutoComplete


Public Class wfnWODetail_AJAX
	Inherits Page

#Region " Enumeration "

	Private Enum Rights
		[New] = 1
		Edit = 2
		Delete = 3
		Save = 4
		View = 5
		Print = 6
		Authorized = 7
		Completed = 8 'Added By Vikrant on 30-Jun-2021 For ALL30062021
	End Enum

#End Region

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As Object

	Private Sub Page_Init(sender As Object, e As EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

#Region " Variable Declaration "

	Protected mnWO As nWO
	Protected mnWOList As nWOList
	Dim mReportLogRegister As New ReportLogRegister
	Dim mWorkShopList As WorkShopList
	'Dim mMachineList As MachineList
	Dim mMachineList As MachineNameValueList
	Dim mnWOJobStatusList As nWOJobStatusList
	Dim mPeriodUnitList As PeriodUnitList
	Dim mSelectPeriods As SelectPeriods = SelectPeriods.NewSelectPeriods
	Dim mCustomerList As VendorList
	Dim mTempAssemblyList As AssemblyList
	Dim mnWOClone As nWO  'Added By Prashant 20-Jan-2011
	Dim tmpText As String = ""
	Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
	Dim LogId As Guid
	Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
	Dim mWODetail As String
	Dim Flag As Int16
	Dim mRequisitionNew As RequisitionNew
	Dim NRCJobText As String = String.Empty
	Dim mFileAttach As FileAttach
	Dim mFileAttachnWO As FileAttach
	'12-Jun-2019
	Dim mRequisitionItemsNew As RequisitionItemsNew
	Dim ReqItemIds As New StringBuilder
	'End
	Dim PrevStatusID As Integer
	Dim mTransactionList As TransactionList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
	Dim AirframeHrsAsOnCompletionDate As String = String.Empty
	Dim AFAllPeriodsAsOnCompletionDate As String = String.Empty
	Dim CompletedByUserLicenceNos As String = String.Empty  'IND

	Dim mnWOApproveReject As nWOApproveReject
	Dim mnWOApproveRejectList As nWOApproveRejectList
	Dim mtmpTransTypeID As Integer
	Public mnIssuedWOSpares As nIssuedWOSpares  'Added By Prashant on 7-Jul-2020 All07072020
	Protected mIssuedWOTools As nIssuedWOTools  'Added By Prashant on 7-Jul-2020 All07072020
	Dim NumberOfMultipleRequisitionOfTaskSparesDetails As StringBuilder = New StringBuilder 'Added By Prashant on 31-Aug-2020 STR28082020
	Dim mUser As User
	Dim mAutoCreatedReqCount As Integer = 0
	Dim mFromRequisitionNo As String = String.Empty
	Dim mToRequisitionNo As String = String.Empty  'End of Added By Prashant on 31-Aug-2020 STR28082020
	'Added By Vikrant On 27-Jul-2020 For ALL27072020
	Public mRemovedAssemblyListForCombo As RemovedAssemblyListForCombo
	Public mRemovedCompListForCombo As RemovedCompListForCombo
	'End
	Shared UserNameForLicenceList As String
	Dim mServiceProviderList As VendorList

	'Added by Saylee on 22-Jun-2023
	Dim Extension As String = String.Empty
	Dim MyConnection As OleDb.OleDbConnection
	Dim MyCommand As OleDb.OleDbDataAdapter
	Dim DS As New DataSet
	''*********************************************************

#End Region

#Region " Helper Methods "

	Private Sub GetSession()
		mnWO = Session("mnWO")
		mnWOList = Session("mnWOList")
		mReportLogRegister = Session("mReportLogRegister")
		mnWOJobStatusList = Session("mnWOJobStatusList")
		mPeriodUnitList = Session("mPeriodUnitList")
		mMachineList = Session("mMachineList")
		mSelectPeriods = Session("mSelectPeriods")
		mWorkShopList = Session("mWorkShopList")
		mTempAssemblyList = Session("mTempAssemblyList")

		tmpText = Session("tmpText")
		NRCJobText = Session("NRCJobText")
		mRequisitionItemsNew = Session("mRequisitionItemsNew") '12-Jun-2019
		PrevStatusID = Session("PrevStatusID")     ' 11-oct-2019
		mTransactionList = Session("mTransactionList")  'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 

		mnWOApproveReject = Session("mnWOApproveReject")
		mnWOApproveRejectList = Session("mnWOApproveRejectList")
		mtmpTransTypeID = Session("mtmpTransTypeID")
		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		mRemovedAssemblyListForCombo = Session("mRemovedAssemblyListForCombo")
		mRemovedCompListForCombo = Session("mRemovedCompListForCombo")
		'End
	End Sub

	Private Sub SendMailForToolsRequest(Optional onButtonClick As Boolean = False) ''Ádded by Sáylee on 24-Sep-2019
		Dim str As String
		Dim mSendMailFile As New SendMailFile

		'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
		'Dim ToMailIDs As String = AppSettings("StoreMailID")
		Dim ToMailIDs As String = ""
		If mnWO.TransTypeID = 88 Then
			ToMailIDs = mTransactionList.Item(Trans.WO145).SendToMailID
		ElseIf mnWO.TransTypeID = 89 Then
			ToMailIDs = mTransactionList.Item(Trans.WOCAMO).SendToMailID
			'Added By Vikrant On 27-Jul-2020 For ALL27072020
		ElseIf mnWO.TransTypeID = 92 Then
			ToMailIDs = mTransactionList.Item(Trans.SpareAssemblyWO).SendToMailID
		ElseIf mnWO.TransTypeID = 93 Then
			ToMailIDs = mTransactionList.Item(Trans.SpareComponentWO).SendToMailID
		End If


		str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Following Tool(s) has been requested by <b>" + User.Identity.Name + "</b>" + " in Work Order " + mnWO.WONumber + " ,Created/Planned on " + New SmartDate(mnWO.WOPlanedAndWODateCalender.ToString).FormattedText + " in FlyPal System." + "</font></P></br> ")
		str = str + ("<TABLE BORDER=1 CELLSPACING=0 CELLPADING=0 ID=""Table2"">")
		str = str + ("<tr>" & "<td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>Sr. No.</b>" & "</font>" & "</td><td align=""center"" width=""200"" style=""background-color: #829e82; color: black;"" >" & "<font face=""Calibri""><b>Part No</b>" & "</font>" & "</td><td align=""center"" width=""200"" style=""background-color: #829e82; color: black;"" >" & "<font face=""Calibri""><b>Description</b>" & "</font>" & "</td><td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>Qty</b>" & "</font>" & "</td></tr>")

		For i As Integer = 0 To mnWO.WOTools.Count - 1
			str = str + ("<TR>")
			str = str + ("<TD WIDTH=20px >")
			str = str + ("<font face=""Calibri"">")
			str = str + (mnWO.WOTools(i).SrNo.ToString) + "."
			str = str + ("</font>")
			str = str + ("</TD>")

			str = str + ("<TD WIDTH=200px >")
			str = str + ("<font face=""Calibri"">")
			str = str + (mnWO.WOTools(i).PartNo)
			str = str + ("</font>")
			str = str + ("</TD>")

			str = str + ("<TD WIDTH=200px >")
			str = str + ("<font face=""Calibri"">")
			str = str + (mnWO.WOTools(i).Description)
			str = str + ("</font>")
			str = str + ("</TD>")

			str = str + ("<TD WIDTH=50px >")
			str = str + ("<font face=""Calibri"">")
			str = str + (mnWO.WOTools(i).RequiredQty.ToString)
			str = str + ("</font>")
			str = str + ("</TD>")

			str = str + ("</TR>")
		Next

		str = str + ("</TABLE>")

		str = str + ("<p><font face=""Calibri"">")
		str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")
		str = str + ("</body></html>")
		Dim mToolsDetail As String
		If onButtonClick = False Then
			SendMailFile.SendMailFile(, User.Identity.Name, "Tools Requested Notification", Info:=str, ToMailID:=ToMailIDs.ToString, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
				 SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
			mToolsDetail = "Tools Requested Notification sent successfully to " + ToMailIDs.ToString.TrimEnd(",") + " by " + User.Identity.Name + " Work Order " + mnWO.WONumber
		Else
			SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "Tools Requested Notification", Info:=str, ToMailID:=Session("ToSendMailIDs"),
			 CCMailID:=Session("CcSendMailIDs"), Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
			  SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
			mToolsDetail = "Tools Requested Notification sent successfully to " + Session("ToSendMailIDs").ToString.TrimEnd(",") + " by " + User.Identity.Name + " Work Order " + mnWO.WONumber
		End If


		MarkLog(Action.SendMail, "Work Order", mToolsDetail, ErrorType.HandledError, mnWO.ID, EventLogID)
		' ScriptManager.RegisterStartupScript(Me, [GetType], "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)
		MSGBoxCtrl.Show("Mail!", "Mail Sent Successfully to store for requesting Tools", "", MsgBoxStyle.OkOnly, "")
	End Sub

	Private Sub FillJobTypeCombo(MachineID As Guid)

		cmbJobType.Items.Clear()

		If mnWO.TransTypeID = Trans.SpareAssemblyWO Or
		   mnWO.TransTypeID = Trans.SpareComponentWO Then 'Added By Vikrant On 27-Jul-2020 For ALL27072020

			cmbJobType.Items.Add(New ListItem("UnScheduled/Customer WO", "1"))
			cmbJobType.Items.Add(New ListItem("Scheduled", "2"))

		ElseIf mnWO.TransTypeID = 102 And AppSettings("IsEngineeringWORequired").ToLower = "true" Then

			cmbJobType.Items.Add(New ListItem("Scheduled", "2"))

		Else 'Existing Condition

			If Not MachineID.Equals(Guid.Empty) Then

				'Added By Prashant On 30-Jun-2023
				If (AppSettings("ShowMaintenanceForNewClients") = "True" And
					AppSettings("ShowCAMOOnlyForNewClients") = "True" And
					(mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 102)) Then '89 Camo WO

					cmbJobType.Items.Add(New ListItem("UnScheduled", "1"))

				ElseIf (AppSettings("ShowMaintenanceForNewClients") = "True" And
						AppSettings("ShowAMOOnlyForNewClients") = "True" And
						mnWO.TransTypeID = 88) Or
						Session("wfProject_Ajax") = "wfProject_Ajax" Then '88 Third Party/AMO WO

					If mnWO.TransTypeID = 117 Then
						cmbJobType.Items.Add(New ListItem("UnScheduled", "1"))
					Else
						cmbJobType.Items.Add(New ListItem("Customer WO", "1"))
					End If


				ElseIf AppSettings("ClientCode") = "TAAL" And mnWO.IsScheduledJobPresent Then
					'do nothing --As TAAL is restricted with Schedule and Un-Schedule Jobs in 1 WO , as two different Prints are designed accordingly
				Else

					cmbJobType.Items.Add(New ListItem("UnScheduled / Customer WO", "1"))
				End If

				If AppSettings("ClientCode") = "TAAL" And mnWO.WOJobs.IsUnScheduledJobExists Then
					'do nothing -------As TAAL is restricted with Schedule and Un-Schedule Jobs in 1 WO , as two different Prints are designed accordingly
				Else
					cmbJobType.Items.Add(New ListItem("Scheduled", "2"))
				End If

				If AppSettings("ShowNewDiscrepancyFlow") = "True" Then

					cmbJobType.Items.Add(New ListItem("Discrepancies", "3"))

				Else

					cmbJobType.Items.Add(New ListItem(IIf(AppSettings("MELSnagNomenclature") = "True",
														   "Defect / ADD",
														   "Snag / MEL"), "3"))

				End If

				If ((AppSettings("ShowCAMOOnlyForNewClients") = "False") And
				   (AppSettings("ShowAMOOnlyForNewClients") = "False")) Or
				   AppSettings("ClientCode") = "CVA" Then '88 Third Party/AMO WO

					cmbJobType.Items.Add(New ListItem("Deferred", "4"))

				End If

				If Not mnWO.IsNew And
				   (AppSettings("ShowCAMOOnlyForNewClients") = "False" Or
					AppSettings("ShowAMOOnlyForNewClients") = "True" Or
					Session("wfProject_Ajax") = "wfProject_Ajax") Then 'if condition added by Vikrant On 23-Aug-2020

					cmbJobType.Items.Add(New ListItem(NRCJobText, "5"))

				End If

				If AppSettings("ClientCode") = "IND" Then 'Added By Prashant 16-Sep-2019

					cmbJobType.Items.Add(New ListItem("Shop Work Order", "7"))

				End If 'End of Added By Prashant 16-Sep-2019

				cmbJobType.Items.Remove("From Model Maint. Activity") 'Added By Vikrant on 24-Apr-2018 For All24042018

			Else

				'Added By Prashant On 30-Jun-2023
				If (AppSettings("ShowMaintenanceForNewClients") = "True" And
					AppSettings("ShowCAMOOnlyForNewClients") = "True" And
					(mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 102)) Then '89 Camo WO

					cmbJobType.Items.Add(New ListItem("UnScheduled", "1"))

					If (Not mnWO.ModelName = "") Then cmbJobType.Items.Add(New ListItem("From Model Maint. Activity", "6")) 'Added By Vikrant on 24-Apr-2018 For All24042018

				ElseIf (AppSettings("ShowMaintenanceForNewClients") = "True" And
						AppSettings("ShowAMOOnlyForNewClients") = "True" And
						mnWO.TransTypeID = 88) Or
					   Session("wfProject_Ajax") = "wfProject_Ajax" Then '88 Third Party WO
					If mnWO.TransTypeID = 117 Then
						cmbJobType.Items.Add(New ListItem("UnScheduled", "1"))
					Else
						cmbJobType.Items.Add(New ListItem("Customer WO", "1"))
					End If
				Else

					cmbJobType.Items.Add(New ListItem("UnScheduled / Customer WO", "1"))

					If (Not mnWO.ModelName = "") Then cmbJobType.Items.Add(New ListItem("From Model Maint. Activity", "6")) 'Added By Vikrant on 24-Apr-2018 For All24042018

				End If

				If Not mnWO.IsNew And
				   (AppSettings("ShowCAMOOnlyForNewClients") = "False" Or
					AppSettings("ShowAMOOnlyForNewClients") = "True" Or
					Session("wfProject_Ajax") = "wfProject_Ajax") Then 'if condition added by Vikrant On 23-Aug-2020

					cmbJobType.Items.Add(New ListItem(NRCJobText, "5"))

				End If

				If AppSettings("ClientCode") = "IND" Then 'Added By Prashant 16-Sep-2019

					cmbJobType.Items.Add(New ListItem("Shop Work Order", "7"))

				End If 'End of Added By Prashant 16-Sep-2019

				If AppSettings("ShowNewDiscrepancyFlow") = "True" Then

					cmbJobType.Items.Remove("Discrepancies")

				Else

					cmbJobType.Items.Remove(IIf(AppSettings("MELSnagNomenclature") = "True",
												"Defect / ADD",
												"Snag / MEL"))

				End If

				cmbJobType.Items.Remove("Scheduled")
				cmbJobType.Items.Remove("Deferred")

			End If

		End If

	End Sub

	Private Sub SetSession()
		Session("mnWO") = mnWO
		Session("mnWOList") = mnWOList
		Session("mReportLogRegister") = mReportLogRegister
		Session("mnWOJobStatusList") = mnWOJobStatusList
		Session("mPeriodUnitList") = mPeriodUnitList
		Session("mMachineList") = mMachineList
		Session("mSelectPeriods") = mSelectPeriods
		Session("mWorkShopList") = mWorkShopList
		Session("mnWOApproveReject") = mnWOApproveReject
		Session("mnWOApproveRejectList") = mnWOApproveRejectList
		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		Session("mRemovedAssemblyListForCombo") = mRemovedAssemblyListForCombo
		Session("mRemovedCompListForCombo") = mRemovedCompListForCombo
		'End
	End Sub

	Private Sub RemoveSession()
		Session.Remove("mnWO")
		Session.Remove("mReportLogRegister")
		Session.Remove("mnWOJobStatusList")
		Session.Remove("mPeriodUnitList")
		Session.Remove("mMachineList")
		Session.Remove("mSelectPeriods")
		Session.Remove("Edit")
		Session.Remove("mWorkShopList")
		Session.Remove("mMachineNameValueList")
		Session.Remove("mnWOApproveReject")
		Session.Remove("mnWOApproveRejectList")
		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		Session.Remove("mRemovedAssemblyListForCombo")
		Session.Remove("mRemovedCompListForCombo")
		'End
	End Sub

	Private Sub AddAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
		txtNoOfSupplementalSheets.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNoOfSupplementalSheets').value,event)")
	End Sub

	Private Function IsInRole(CheckFor As Rights) As Boolean

		Dim IsInRoleString As String = ""

		If AppSettings("ShowNewWOFlow") = "True" Then

			If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mtmpTransTypeID Then

				If mnWO.TransTypeID = Trans.WO145 Then

					IsInRoleString = "WOCreate"

				ElseIf mnWO.TransTypeID = Trans.OJS145 Then

					IsInRoleString = "OJSWorkOrder"

				ElseIf mnWO.TransTypeID = Trans.OJSCAMO Then

					IsInRoleString = "OJSCAMOWorkOrder"

				Else

					IsInRoleString = "CAMOWOCreate"

				End If

			ElseIf Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Then

				IsInRoleString = "WOPlanning"

			ElseIf Session("MiddleFrame") = "wfnWOExecutionList.aspx" Then

				IsInRoleString = "WOExecution"

			ElseIf Session("MiddleFrame") = "wfnWOCompletionList.aspx?" Then

				IsInRoleString = "WOCompletion"

			ElseIf Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" Then

				IsInRoleString = "WOQCApproval"

			ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1" Then

				IsInRoleString = "WOCAMOUpdate"

			ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0" Then

				IsInRoleString = "WOBilling"

			End If

		Else

			If mnWO.TransTypeID = Trans.WO145 Or
				mnWO.TransTypeID = Trans.DiscrepancyWO Or
				mnWO.TransTypeID = Trans.AMODiscrepancyWO Or
				mnWO.TransTypeID = Trans.AMOAMPTask Or
				mnWO.TransTypeID = Trans.AMOCustomerWO Or
				mnWO.TransTypeID = Trans.AMOADSBWO Then

				IsInRoleString = "WorkOrder"

			ElseIf mnWO.TransTypeID = Trans.SpareAssemblyWO Then

				IsInRoleString = "SpareAssemblyWO"

			ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then

				IsInRoleString = "SpareComponentWO"

			ElseIf mnWO.TransTypeID = Trans.EngineeringWO Then

				IsInRoleString = "EngineeringOrder"

			Else

				IsInRoleString = "CAMOWO"

			End If

		End If

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

			Case Rights.Authorized

				Return User.IsInRole(IsInRoleString + "Authorized")

				'Added By Vikrant on 30-Jun-2021 For ALL30062021 
			Case Rights.Completed

				Return User.IsInRole(IsInRoleString + "Completed")
				'End

		End Select

	End Function

	Private Overloads Sub SetFocus(cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "try{document.getElementById('" + cntrl.ClientID + "').focus();}catch (Error) {}"
		ScriptManager.RegisterStartupScript(Me, [GetType], "focusscript", str, True)
	End Sub

	Private Sub SetPage()
		If mnWO.IsNew Then
			lblStatus.Text = "OPEN"
		Else
			SetControlStatus(mnWO.WOStatusID)
		End If
	End Sub

	Private Sub SetPeroids()
		Dim mPeriodlist As PeriodList
		mSelectPeriods = SelectPeriods.NewSelectPeriods
		mPeriodlist = PeriodList.GetPeriodList
		For i As Integer = 0 To mPeriodlist.Count - 1
			If Not mnWO.WOPeriods.Contains(mPeriodlist(i).ID) Then
				mSelectPeriods.Add(mPeriodlist(i).ID, mPeriodlist(i).PeriodName)
			End If
		Next
		Session("mSelectPeriods") = mSelectPeriods
	End Sub

	Private Sub AddSelectedPeroids()
		mSelectPeriods = Session("mSelectPeriods")

		Dim mSelectPeriod As SelectPeriod

		If IsNothing(mSelectPeriods) Then
			mSelectPeriods = SelectPeriods.NewSelectPeriods
		End If
		For Each mSelectPeriod In mSelectPeriods
			If mSelectPeriod.IsSelected Then
				mnWO.WOPeriods.Add(nWOPeriod.NewWOPeriod(mnWO.ID, 1, mSelectPeriod.PeriodID, mSelectPeriod.PeriodName, mnWO.HourType, "", ""))
			End If
		Next

		Session("wfWODetail.WO") = mnWO
		Session.Remove("mSelectPeriods")
		mSelectPeriods = Nothing
	End Sub

	Private Sub SetObject()

		Try

			If txtWODate.Text.ToString <> "" Then

				If AppSettings("ClientCode") = "IND" Or
				   AppSettings("ClientCode") = "STR" Or
				   AppSettings("ClientCode") = "YA" Or
				   AppSettings("ClientCode") = "AFC" Or
				   AppSettings("ClientCode") = "BAP" Or
				   AppSettings("ClientCode") = "GLD" Then 'Added By Saylee on 26-Sep-2018 for STR26092018,  Star Air needs Time with Date

					mnWO.WODate = CType(txtWODate.Text.ToString.Trim + " " + txtWOTime.Text.ToString.Trim, DateTime)

				Else
					mnWO.WODate = CDate(txtWODate.Text)
				End If

			Else
				mnWO.WODate = DBNull.Value
			End If

			If txtStartDate.Text.ToString <> "" Then

				If (AppSettings("ClientCode") = "IND" Or
					AppSettings("ClientCode") = "YA" Or
					AppSettings("ClientCode") = "AFC" Or
					AppSettings("ClientCode") = "BAP" Or
					AppSettings("ClientCode") = "GLD") Then

					mnWO.WOStartDate = CType(txtStartDate.Text.ToString.Trim + " " + txtStartDateTime.Text.ToString.Trim, DateTime)
				Else
					mnWO.WOStartDate = CDate(txtStartDate.Text)
				End If

			Else
				mnWO.WOStartDate = DBNull.Value
			End If

			mnWO.WOText = txtText.Text
			mnWO.WONo = Val(txtNo.Text)
			mnWO.RegNo = Trim(txtRegNo.Text)
			mnWO.CustomerID = New Guid(cmbCustomerList.SelectedValue.ToString)
			mnWO.WorkShopID = New Guid(cmbWorkShopList.SelectedValue.ToString)
			mnWO.WorkShopName = IIf(cmbWorkShopList.SelectedIndex > 0, cmbWorkShopList.SelectedItem.Text, "")
			mnWO.ModelName = Trim(txtModelNo.Text)
			mnWO.SerialNo = Trim(txtSerialNo.Text)
			mnWO.WOBy = Trim(txtCreatedBy.Text)

			If txtCloseDate.Text.ToString <> "" Then

				If (AppSettings("ClientCode") = "IND" Or
					AppSettings("ClientCode") = "STR" Or
					AppSettings("ClientCode") = "YA" Or
					AppSettings("ClientCode") = "AFC" Or
					AppSettings("ClientCode") = "BAP" Or
					AppSettings("ClientCode") = "GLD") Then
					mnWO.WOCloseDate = CType(txtCloseDate.Text.ToString.Trim + " " + txtClosedDateTime.Text.ToString.Trim, DateTime)
				Else
					mnWO.WOCloseDate = CDate(txtCloseDate.Text)
				End If

			Else
				mnWO.WOCloseDate = DBNull.Value
			End If

			If txtPlanDate.Text.ToString <> "" Then

				If (AppSettings("ClientCode") = "IND") Then
					mnWO.WOPlanedDate = CType(txtPlanDate.Text.ToString.Trim + " " + txtPlanDateTime.Text.ToString.Trim, DateTime)
				Else
					mnWO.WOPlanedDate = CDate(txtPlanDate.Text)
				End If

			Else
				mnWO.WOPlanedDate = DBNull.Value
			End If

			mnWO.IsInHouse = rdbIsInHouse.Checked
			mnWO.IsThirdParty = rdbIsThirdParty.Checked
			mnWO.WOActualTime = Trim(txtActualTime.Text)
			mnWO.HourType = Val(cmbHourTypeList.SelectedValue)
			mnWO.ClosedBy = Trim(txtClosedBy.Text)
			mnWO.WORemark = Trim(txtRemark.Text)

			If mnWO.WOStartDate.ToString <> "" And Not mnWO.MachineID.Equals(Guid.Empty) Then

				If cmbLogList.SelectedValue.ToString = "" Then
					mnWO.LogID = Guid.Empty
				Else
					mnWO.LogID = New Guid(cmbLogList.SelectedValue.ToString)
				End If

			ElseIf mnWO.WODate.ToString <> "" And Not mnWO.MachineID.Equals(Guid.Empty) Then  'Added by Saylee 16-Sep-2019

				If cmbLogList.SelectedValue.ToString = "" Then
					mnWO.LogID = Guid.Empty
				Else
					mnWO.LogID = New Guid(cmbLogList.SelectedValue.ToString)
				End If

			End If

			mnWO.LogNo = txtLogNo.Text.Trim 'Added by Prashant on 15-Apr-2019 LAMA15042019

			'Changes by Saylee on 18-Feb-2013 for ALL18022013
			If mnWO.WOStartDate.ToString <> "" And Not mnWO.MachineID.Equals(Guid.Empty) Then

				If mReportLogRegister.Contains(mnWO.LogID, "") Then cmbLogList.SelectedValue = mnWO.LogID.ToString

			ElseIf mnWO.WODate.ToString <> "" And Not mnWO.MachineID.Equals(Guid.Empty) Then 'Added by Saylee 16-Sep-2019

				If mReportLogRegister.Contains(mnWO.LogID, "") Then cmbLogList.SelectedValue = mnWO.LogID.ToString

			End If

			If mReportLogRegister Is Nothing Then
				'Do nothing 
			Else

				If mReportLogRegister IsNot Nothing And mReportLogRegister.Count > 0 Then

					mnWO.FromPlace = IIf(mReportLogRegister Is Nothing, "", mReportLogRegister(mnWO.LogID).DepartureFrom)
					mnWO.ArrivalPlace = IIf(mReportLogRegister Is Nothing, "", mReportLogRegister(mnWO.LogID).ArrivalTo)

				End If

			End If
			'End of 'Added By Prashant on 20-May-2021 DANA20052021

			'Added By Utkarsh ON 09-May-2013 FOR BA09052013-1
			mnWO.FormNo = txtFormNo.Text.Trim
			mnWO.IssueNo = txtIssueNo.Text.Trim
			mnWO.RevisionNo = txtRevisionNo.Text.Trim    'End
			mnWO.IsFMC = chkFMC.Checked

			If AppSettings("ClientCode") = "IND" Then  'This Change is for Aircraft MRO

				If mnWO.WOJobs.Is_Job_IsRII = True Then
					chkIsCritical.Checked = True
				Else
					chkIsCritical.Checked = False
				End If

			End If

			mnWO.IsCriticalWO = chkIsCritical.Checked 'Added by Saylee for STR23112018 on 23-11-2018

			'Added By Saylee on 18-Oct-2016
			mnWO.CustomerWONo = txtCustWO.Text.Trim
			mnWO.IsCustApprovedObtained = rdpYes.Checked
			If rdpYes.Checked Then
				mnWO.CustApprovedByEmailWO = cmbCustApprovedByEmailWO.SelectedValue.ToString
			Else
				mnWO.CustApprovedByEmailWO = String.Empty
			End If
			mnWO.IssueTo = txtIssueTo.Text.Trim

			'******************************
			'''''''AttachMyFile()
			For i As Integer = 0 To mnWO.FileAttachments.Count - 1

				Dim txtValue As TextBox
				txtValue = CType(Me.dgWOAttachment.Rows(i).FindControl("txtFileName"), TextBox)
				mnWO.FileAttachments(i).FileName = txtValue.Text.Trim

			Next

			'Added by Saylee on 12-Oct-2018, ALL11102018
			If mnWO.IsDigitalSignatureAdded = True Then

				If mnWO.IsNew Then

					mFileAttach = FileAttach.GetAttachment(mnWO.EmployeeID, , "DigitalSignature")
					mFileAttachnWO = FileAttach.NewAttachment(mnWO.ID, "DigitalSignatureWO")
					mFileAttachnWO.Extension = mFileAttach.Extension
					mFileAttachnWO.Size = mFileAttach.Size
					mFileAttachnWO.ImageFile = mFileAttach.ImageFile
					mFileAttachnWO.FileName = "DigitalSignatureWO"
					Session("mFileAttachnWO") = mFileAttachnWO

				Else

					mFileAttachnWO = Session("mFileAttachnWO")

					If mFileAttachnWO Is Nothing Then

						mFileAttachnWO = FileAttach.GetAttachment(mnWO.ID, , "DigitalSignatureWO")
						Session("mFileAttachnWO") = mFileAttachnWO

					End If

				End If

			End If
			'******************************
			mnWO.IsAttachmentAdded = IIf(mnWO.FileAttachments.Count > 0, True, False)
			'--Added by Saylee on 20-Sep-2019 for HSC20092019
			If AppSettings("ClientCode") = "HSC" Then

				If Not mnWO.IsNew Then
					mnWO.WOAction = txtWOAction.Text
					mnWO.WOWorkDone = txtWOWorkDone.Text
				End If

			End If
			'*****************************

			'Added by Shital on 09-Oct-2019
			mnWO.PlanningRemark = txtPlanningRemark.Text
			mnWO.PPCRemark = txtPPCRemark.Text
			mnWO.CAMOUpdateRemark = txtCAMOUpdateRemark.Text  '----

			'Added By Vikrant On 27-Jul-2020 For ALL27072020
			If mnWO.TransTypeID = Trans.SpareComponentWO Then
				mnWO.AssemblyStatusID = New Guid(cmbCompList.SelectedValue)
				mnWO.IsSpareAssemblyWO = True
			ElseIf mnWO.TransTypeID = Trans.SpareAssemblyWO Then
				mnWO.AssemblyStatusID = New Guid(cmbAssembly.SelectedValue)
				mnWO.IsSpareAssemblyWO = True
			End If    'End

			'Added by Saylee on 18-May-2021 for STR18052021
			mnWO.IsSupplementalSheetAttached = chkSupplementalSheetAttached.Checked
			mnWO.NoOfSupplementalSheets = Val(txtNoOfSupplementalSheets.Text)
			mnWO.IsNRCRaised = chkNRCRaised.Checked
			mnWO.NoOfNRCs = txtNoOfNRCs.Text
			mnWO.IsReInspection = chkIsReInspection.Checked
			mnWO.IsIndependentInspection = chkIsIndependentInspection.Checked
			mnWO.CRSNo = txtCRSNo.Text

			Dim LicenseNo As String = String.Empty
			Dim EmpName As String = String.Empty
			If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
				LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
				EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
			Else
				LicenseNo = Trim(txtLicenceNo.Text)
			End If
			mnWO.CertifylingLicenseNo = LicenseNo
			mnWO.CertifyingEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID

			LicenseNo = String.Empty
			EmpName = String.Empty

			If (txtLicenceNo2.Text.Trim.IndexOf("[") > 0 And txtLicenceNo2.Text.Trim.IndexOf("]") > 0) Then
				LicenseNo = txtLicenceNo2.Text.Substring(0, txtLicenceNo2.Text.Trim.IndexOf("[")).Trim
				EmpName = Mid(txtLicenceNo2.Text.Trim, txtLicenceNo2.Text.Trim.IndexOf("[") + 2, txtLicenceNo2.Text.Trim.IndexOf("]") - txtLicenceNo2.Text.Trim.IndexOf("[") - 1).Trim
			Else
				LicenseNo = Trim(txtLicenceNo2.Text)
			End If

			mnWO.CertifyingEmployeeID2 = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
			mnWO.CertifylingLicenseNo2 = LicenseNo
			'*****************************

			'Added by shital on 03-Dec-2021
			If Not mnWO.IsNew Then
				mnWO = Session("mnWO")
				mnWO.IsMailSend = mnWO.IsMailSend
			End If
			'--
			mnWO.IsMSP = chkIsMSP.Checked
			mnWO.ServiceProviderID = New Guid(cmbServiceProvider.SelectedValue.ToString)
			'Sankalp 20-11-25
			If User.Identity.Name.ToUpper() <> "BTPLADMIN" Then
				If mnWO.IsNew = True Then
					mnWO.CreatedBy = User.Identity.Name
				Else
					mnWO.LastUpdatedBy = User.Identity.Name
				End If
			End If
			Session("mnWO") = mnWO
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetLog()

		If Not cmbLogList.SelectedValue = "(SELECT)" And Not cmbLogList.SelectedValue = "" Then
			LogId = New Guid(cmbLogList.SelectedValue.ToString)
			Session("LogId") = CType(Session("LogId"), String)
			If Not LogId.Equals(Guid.Empty) Then
				If AppSettings("ClientCode") = "IND" Then  'Added by Saylee on 30-Nov-2020 for INDAMAR for showing Log Current values as per selected Log
					Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(mReportLogRegister(LogId).LogDateFormatted.ToString, cmbAircraftList.SelectedValue.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , LogId.ToString, True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
					AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
					Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
					tmpAssemblyStatusList = Nothing
				Else
					If txtStartDate.Text.ToString <> "" Then
						Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtStartDate.Text, cmbAircraftList.SelectedValue.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , LogId.ToString, True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
						AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
						Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
						tmpAssemblyStatusList = Nothing
					Else
						Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtWODate.Text, cmbAircraftList.SelectedValue.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , LogId.ToString, True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
						AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
						Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
						tmpAssemblyStatusList = Nothing
					End If
				End If
				If mnWO.WOPeriods.Count <> 0 Then
					For i As Integer = mnWO.WOPeriods.Count - 1 To 0 Step -1
						mnWO.WOPeriods.RemoveAt(i)
					Next
				End If
				mnWO.WOPeriods.SetWOPeriods(mnWO.ID, AssemblyStatusPeriodList, mnWO.HourType)
				dgCurrentPeriodValue.DataSource = mnWO.WOPeriods
				dgCurrentPeriodValue.DataBind()
			End If
		End If
	End Sub

	Private Sub SetGridEnability(IsEnabled As Boolean)
		Dim i As Integer
		Dim txtValue As TextBox
		For i = 0 To dgCurrentPeriodValue.Rows.Count - 1
			txtValue = CType(Me.dgCurrentPeriodValue.Rows(i).FindControl("txtValue"), TextBox)
			txtValue.ReadOnly = IsEnabled
		Next i
		Session("mnWO") = mnWO
	End Sub

	Private Sub SetGridObject(Optional setWOCompletionAirframeValues As Boolean = False)
		Dim i As Integer
		Dim txtValue As TextBox
		'Added By Vikrant On 30-Jan-2020 For ALL30012020
		Dim mAssemblyStatusList As AssemblyStatusList
		If setWOCompletionAirframeValues Then
			If mnWO.StatusID = 2 And mnWO.WOStatusID = 3 Then 'Only Completed WO
				mAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(mnWO.MachineID, AssemblyType:="Airframe", CurrentDate:=mnWO.WOCloseDateFormatted.ToString, IsAssemblyInstalled:=True)
			End If
			'End
		End If
		'End
		For i = 0 To dgCurrentPeriodValue.Rows.Count - 1
			txtValue = CType(Me.dgCurrentPeriodValue.Rows(i).FindControl("txtValue"), TextBox)
			If mnWO.WOPeriods(i).PeriodID = 2 Then
				If Not Period.IsDate(txtValue.Text) Then
					mnWO.WOPeriods(i).CurrentValue = ""
				Else
					mnWO.WOPeriods(i).CurrentValue = Trim(txtValue.Text)
				End If
			Else
				mnWO.WOPeriods(i).CurrentValue = Trim(txtValue.Text)
			End If
			'Added By Vikrant On 30-Jan-2020 For ALL30012020
			If setWOCompletionAirframeValues Then
				If mAssemblyStatusList.Count > 0 Then
					If mAssemblyStatusList(0).AssemblyStatusPeriodList(mnWO.WOPeriods(i).PeriodID, "") IsNot Nothing Then
						mnWO.WOPeriods(i).CurrentValueAsOnCompletionDate = mAssemblyStatusList(0).AssemblyStatusPeriodList(mnWO.WOPeriods(i).PeriodID, "").AssemblyCurrentValue
					End If
				End If

			End If
			'End
		Next i
		Session("mnWO") = mnWO
	End Sub

	Private Sub DeleteJobRecord(Index As Int32)
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveJob")
		mnWO.WOJobs.CurrentIndex = Index
		Session("mnWO") = mnWO
	End Sub

	'Added By Vikrant For WO NRC	
	Private Sub DeleteWONRC(Index As Int32)
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveWONRC")
		mnWO.WONRCJobs.CurrentIndex = Index
		Session("mnWO") = mnWO
	End Sub
	'End

	Private Sub DeleteToolRecord(Index As Int32)

		mnWO.WOTools.CurrentIndex = Index
		Session("mnWO") = mnWO
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveTool")
	End Sub

	Private Sub DeleteAttachment(Index As Int32)
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
		mnWO.FileAttachments.CurrentIndex = Index
		Session("mnWO") = mnWO
	End Sub

	'For BRD
	Private Function MaxWOExceeded() As Boolean
		Dim NoOfUnScheduledWOAllowed As Integer = CInt(AppSettings("USWOAllowed"))
		If NoOfUnScheduledWOAllowed > 0 And mnWO.MachineID.Equals(Guid.Empty) Then 'Allow only no of WO to be entered for typed Aircrafts
			Dim CharsToRemove() As Char = {" "c, "*"c, "~"c, "!"c, "@"c, "#"c, "$"c, "%"c, "^"c, "&"c, "_"c, "-"c, "="c, ","c, "."c, ":"c, ";"c, "<"c, ">"c, "/"c, "{"c, "}"c, "["c, "]"c, "|"c, "\"c, "?"c, "("c, ")"c, "+"c, "="c, "`"c, "'"c}

			mnWOList = nWOList.GetWOList(RegNo:=txtRegNo.Text.Trim(CharsToRemove), SerialNo:=txtSerialNo.Text.Trim(CharsToRemove))
			If mnWOList.Count >= NoOfUnScheduledWOAllowed And Not mnWOList.Contains(mnWO.ID) Then
				MSGBoxCtrl.Show("Max. WO. Limit Reached!", "Selected Aircraft reached Max. WO. No. limit.", "Please add this Aircraft in Master to continue.", MsgBoxStyle.OkOnly, "")
				Return True
			Else
				Return False
			End If
		Else
			Return False
		End If
	End Function
	'End

	Private Function Save(Optional IsFromAuthorize As Boolean = False,
						  Optional IsFromWOCompletion As Boolean = False) As Boolean
		Try

			Dim WOType As String = ""

			'Check Whether min. one item is present while saving except for Religare (Only NRC can be saved without job)
			If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or
			   (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then

				SetObject()
				SetGridObject()
				SetSession()
				mWODetail = mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy

				MarkLog(Action.Save,
						"Work Order",
						User.Identity.Name & " is not Authorized User to save " & mWODetail,
						ErrorType.HandledError,
						Guid.Empty,
						EventLogID)

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"Authorization")

				Exit Function

			End If

			If (AppSettings("ClientCode") = "RAL" And
				Not (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)) Or
			   (AppSettings("ClientCode") <> "RAL" And Not (mnWO.WOJobs.Count = 0)) Then

				SetObject()
				SetGridObject(IsFromWOCompletion)

				If Not mnWO.IsValid Then

					Dim strMSG As String = ""

					If Not mnWO.IsValid Then

						For i As Integer = 0 To mnWO.GetBrokenRulesCollection.Count - 1
							strMSG = strMSG + mnWO.GetBrokenRulesCollection(i).Description + "<Br>"
						Next

						For j As Integer = 0 To mnWO.WOJobs.Count - 1

							For i As Integer = 0 To mnWO.WOJobs(j).GetBrokenRulesCollection.Count - 1
								strMSG = strMSG + mnWO.WOJobs(j).GetBrokenRulesCollection(i).Description + "<Br>"
							Next

						Next

					End If

					If strMSG.Trim <> "" Then
						cvControlValidator.ErrorMessage = strMSG
						cvControlValidator.IsValid = False
					End If

					upnlValidationsummary.Update()

					Exit Function

				End If

				If Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" Then

					'When comes for QC Rejection then will reverse to AME Completion Status, 
					If mnWO.IsQCStatusApproved = 2 Then
						mnWO.WOStatusID = 7
					End If '*************************

				ElseIf Session("MiddleFrame") = "wfnWOCompletionList.aspx?" Then 'When comes for PPC Completion after QC Rejection then will reverse,

					If mnWO.IsQCStatusApproved = 2 Then
						mnWO.IsQCStatusApproved = 0
						mnWO.QCApprovedNotApprovedBy = "" 'Added By Vikrant On 14-Oct-2019 For New WO
					End If

				End If

				'Added By Vikrant On 14-Oct-2019 For New WO
				If AppSettings("ShowNewWOFlow") = "True" Then 'If AppSettings("ClientCode") = "IND" Then

					Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
					Dim mEmployee As Employee

					If Not mUser.EmployeeID.Equals(Guid.Empty) Then

						mEmployee = Employee.GetEmployee(mUser.EmployeeID)

						If mnWO.WOStatusID = 4 Then
							mnWO.PlannedBy = mEmployee.Name

						ElseIf mnWO.WOStatusID = 3 And mnWO.IsQCStatusApproved = 0 Then
							mnWO.PPCCompletedBy = mEmployee.Name
						ElseIf mnWO.IsQCStatusApproved = 1 Or mnWO.IsQCStatusApproved = 2 Then 'ElseIf mnWO.WOStatusID = 5 Then
							mnWO.QCApprovedNotApprovedBy = mEmployee.Name

						End If

					End If

					SetRevertedWOStage()

				End If
				'End

				mnWO.Save()

				Session("mnWO") = mnWO
				mWODetail = $"{mnWO.WOStatus}: {mnWO.WONumber}" &
							$"( Dated : {mnWO.WODateFormatted}" &
							$" Created By : {mnWO.WOBy}" &
							$"{If(Not mnWO.MachineID.Equals(Guid.Empty), $" Aircraft : {mnWO.RegNo}", "")}" &
							$"{If(mnWO.ModelName <> "", $" Model : {mnWO.ModelName}", "")}" &
							$"{If(mnWO.SerialNo <> "", $" Serial No. : {mnWO.SerialNo}", "")}"

				mnWOApproveReject = Session("mnWOApproveReject")
				'Added By Saylee On 4-Mar-2020 For Approval Reject history

				If mnWOApproveReject IsNot Nothing Then

					Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
					Dim mEmployee As Employee

					If Not mUser.EmployeeID.Equals(Guid.Empty) Then
						mEmployee = Employee.GetEmployee(mUser.EmployeeID)
						mnWOApproveReject.DoneBy = mEmployee.Name
					End If

					mnWOApproveReject.Save()
					mWODetail = Session("WODetailForMarkLog") + mnWOApproveReject.DoneBy

					MarkLog(Action.Save,
							"Work Order",
							mWODetail,
							ErrorType.NoError,
							mnWO.ID,
							EventLogID)

					Session.Remove("mnWOApproveReject")
					mnWOApproveReject = Nothing

				End If
				'End

				If mnWO.StatusID = 2 And mnWO.WOStatusID < 3 Then

					MarkLog(Action.Authorize,
							"Work Order",
							mWODetail,
							ErrorType.NoError,
							mnWO.ID,
							EventLogID)

				ElseIf mnWO.StatusID = 3 Then

					MarkLog(Action.Amend,
							"Work Order",
							mWODetail,
							ErrorType.NoError,
							mnWO.ID,
							EventLogID)

				ElseIf mnWO.StatusID = 4 Then

					MarkLog(Action.Cancel,
							"Work Order",
							mWODetail,
							ErrorType.NoError,
							mnWO.ID,
							EventLogID)

				Else

					MarkLog(Action.Save,
							"Work Order",
							mWODetail,
							ErrorType.NoError,
							mnWO.ID,
							EventLogID)

				End If

				mnWO.MarkClean()
				Session("mnWO") = mnWO

				If (AppSettings("ClientCode") IsNot Nothing) AndAlso
				   (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
					WOType = "Engineering Order"
				Else
					WOType = "Work Order"
				End If

				If mnWO.WOStatusID = 3 And mnWO.WOJobs(0).WOJobStatusID <> 4 And
				   mnWO.WOJobs.IsScheduledJobExists = True And
				   (Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=88" Or
					Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=89" Or
					Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=92" Or
					Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=93" Or
					Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=102") Then 'Added by Saylee on 22-Aug-2012

					MSGBoxCtrl.Show("Alert!",
									"<BR>There are some Scheduled Jobs in this " & WOType &
									".<BR><BR>Do you want to comply these jobs?",
									"",
									MsgBoxStyle.YesNo,
									"ComplyJobs")

					Session("IsValid") = True

					Exit Function

				End If

				If Session("mDueJobPlanning") IsNot Nothing Then

					Dim mDueJobPlanning As DueJobPlanning = CType(Session("mDueJobPlanning"), DueJobPlanning)
					mDueJobPlanning.IsWOCreated = True
					mDueJobPlanning.WOID = mnWO.ID

					If mDueJobPlanning.IsValid Then
						mDueJobPlanning.Save()
					End If

				End If

				'Re-Fetch WO to get the Period Values
				mnWO = WOHelper.FetchWO(ID:=mnWO.ID)
				Session("mnWO") = mnWO

				DataFieldBind()
				SetPage()
				SetGrid()
				SetNRCGrid() 'Added By Vikrant For WO NRC	
				ControlVisibility()
				FillJobTypeCombo(mnWO.MachineID)
				UpdatePanels()

				Return True

			Else

				MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
								MSGBox.Message_Text.saveAlert,
								"Work Order can not be Saved without a Job.",
								MsgBoxStyle.OkOnly,
								"")

				Session("mnWO") = mnWO
				SetObject()
				SetGridObject()
				DataFieldBind()
				SetGrid()

				Return False

			End If

		Catch ex As SqlException

			If ex.Number = 8114 Or ex.Number = 8115 Then

				MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
								MSGBox.Message_text.saveAlert,
								"Work Order cannot be Saved without a Job.",
								MsgBoxStyle.OkOnly,
								"")

			ElseIf ex.Number = 8145 Then

				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
								MSGBox.Message_text.ProcedureError,
								ex.Procedure,
								MsgBoxStyle.OkOnly,
								"")

			ElseIf ex.Number = 2627 Then

				MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
								MSGBox.Message_text.Duplicate,
								ex.Procedure,
								MsgBoxStyle.OkOnly,
								"")


			ElseIf ex.Number = 8144 Then

				MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
									MSGBox.Message_text.ReferenceDelete,
									ex.Procedure + "," + ex.Message,
									MsgBoxStyle.OkOnly,
									"")

			End If

			Return False

		End Try

	End Function

	Private Sub SaveIssuedSpares()
		Dim mIssue As Issue
		Dim i As Integer = 0
		Dim mnIssuedWOSpares As nIssuedWOSpares
		mnIssuedWOSpares = nIssuedWOSpares.GetIssuedWOSpares(mnWO.ID)

		While i < mnIssuedWOSpares.Count
			If mnIssuedWOSpares.Item(i).IsValid Then

				mIssue = Issue.GetIssue(mnIssuedWOSpares.Item(i).ID)
				mIssue.IssueItems(mnIssuedWOSpares.Item(i).IssueItemID).WOUsedQty = mnIssuedWOSpares.Item(i).IssuedQty
				mIssue.IssueItems(mnIssuedWOSpares.Item(i).IssueItemID).WOReturnQty = 0

				If mIssue.IsDirty And mIssue.IsValid Then
					mIssue.Save()
				Else
					Dim strMSG As String = ""
					If Not mIssue.IsValid Then
						For j As Integer = 0 To mIssue.GetBrokenRulesCollection.Count - 1
							strMSG = strMSG + mIssue.GetBrokenRulesCollection(j).Description + "<Br>"
						Next
					End If
					If strMSG.Trim <> "" Then
						cvControlValidator.ErrorMessage = strMSG
						cvControlValidator.IsValid = False
					End If
				End If
			End If
			i = i + 1
		End While
	End Sub

	Private Sub MessageBoxResult()

		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes

					If MSGBoxCtrl.Sender = "RemoveJob" Then

						If mnWO.WOJobs.CurrentItem.WOJobSpares.IsIssuedSpares Then

							MSGBoxCtrl.Show("Alert!",
											"You cannot delete this record, as one of the spare has been already Issued!",
											"",
											MsgBoxStyle.OkOnly,
											"")
							Exit Sub

						End If

						Try

							Session("Sender") = ""

							Dim _WOJob As nWOJob = mnWO.WOJobs.CurrentItem

							mnWO = WOHelper.FetchWO(ID:=mnWO.ID)

							If (mnWO.WOJobs.Contains(_WOJob.ID)) Then
								mnWO.WOJobs.Remove(_WOJob.ID)
							End If

							Session("mnWO") = mnWO

							DataFieldBind()
							SetPage()
							SetGrid()
							SetNRCGrid() 'Added By Vikrant For WO NRC
							ControlVisibility()
							UpdatePanels()

						Catch ex As SqlException

							If ex.Number = 8145 Then

								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
												MSGBox.Message_text.ProcedureError,
												ex.Procedure,
												MsgBoxStyle.OkOnly,
												"")

							ElseIf ex.Number = 2627 Then

								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
												MSGBox.Message_text.Duplicate,
												ex.Procedure,
												MsgBoxStyle.OkOnly,
												"")


							ElseIf ex.Number = 8144 Then

								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
												MSGBox.Message_text.ReferenceDelete,
												ex.Procedure + "," + ex.Message,
												MsgBoxStyle.OkOnly,
												"")

							End If

						End Try

						'Added By Vikrant For WO NRC
					ElseIf MSGBoxCtrl.Sender = "RemoveWONRC" Then

						If mnWO.WONRCJobs.CurrentItem.WOJobSpares.IsIssuedSpares Then

							MSGBoxCtrl.Show("Alert!",
											"You cannot delete this record, as one of the spare has been already Issued!",
											"",
											MsgBoxStyle.OkOnly,
											"")

							Exit Sub

						End If

						Try

							Session("Sender") = ""
							Dim mnWO As nWO
							mnWO = CType(Session("mnWO"), nWO)
							mnWO.WONRCJobs.Remove(mnWO.WONRCJobs.CurrentItem)
							Session("mnWO") = mnWO
							DataFieldBind()
							SetGrid()
							SetPage()
							SetNRCGrid()
							ControlVisibility()
							UpdatePanels()

						Catch ex As SqlException

							If ex.Number = 8145 Then

								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
												MSGBox.Message_text.ProcedureError,
												ex.Procedure,
												MsgBoxStyle.OkOnly,
												"")

							ElseIf ex.Number = 2627 Then

								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
												MSGBox.Message_text.Duplicate,
												ex.Procedure,
												MsgBoxStyle.OkOnly,
												"")


							ElseIf ex.Number = 8144 Then

								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
												MSGBox.Message_text.ReferenceDelete,
												ex.Procedure + "," + ex.Message,
												MsgBoxStyle.OkOnly,
												"")

							End If

						End Try
						'End

					ElseIf MSGBoxCtrl.Sender = "RemoveTool" Then

						If mnWO.WOTools.CurrentItem.WOIssuedToolsCount > 0 Then

							MSGBoxCtrl.Show("Alert!",
											"You cannot delete this record, as Issue against this tool has been already done!",
											"",
											MsgBoxStyle.OkOnly,
											"")

							Exit Sub

						End If

						Try

							Session("Sender") = ""
							Dim mnWO As nWO
							mnWO = CType(Session("mnWO"), nWO)
							mnWO.WOTools.Remove(mnWO.WOTools.CurrentItem)
							Session("mnWO") = mnWO
							DataFieldBind()
							SetPage()
							SetGrid()
							SetNRCGrid() 'Added By Vikrant For WO NRC
							ControlVisibility()
							UpdatePanels()

						Catch ex As SqlException

							If ex.Number = 8145 Then

								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
												MSGBox.Message_text.ProcedureError,
												ex.Procedure,
												MsgBoxStyle.OkOnly,
												"")

							ElseIf ex.Number = 2627 Then

								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
												MSGBox.Message_text.Duplicate,
												ex.Procedure,
												MsgBoxStyle.OkOnly,
												"")


							ElseIf ex.Number = 8144 Then

								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
												MSGBox.Message_text.ReferenceDelete,
												ex.Procedure + "," + ex.Message,
												MsgBoxStyle.OkOnly,
												"")

							End If

						End Try

					ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation

						Session("sender") = ""

						If Session("IsValid") Then

							Session.Remove("IsValid")
							'For BRD
							If MaxWOExceeded() Then
								Exit Sub
							End If
							'End

							If Save() Then

								SetPage()
								SetGrid()
								ControlVisibility()
								UpdatePanels()

							End If

						Else
							Session.Remove("IsValid")
						End If

					ElseIf MSGBoxCtrl.Sender = "Status" Then

						Session("sender") = ""

						Try

							If Session("IsValid") Then

								Session.Remove("IsValid")

								'For BRD
								If MaxWOExceeded() Then
									Exit Sub
								End If
								'End

								If Save(IsFromAuthorize:=(mnWO.StatusID = 2)) Then

									SaveAttachment()
									SetPage()
									SetGrid()
									SetNRCGrid() 'Added By Vikrant For WO NRC
									ControlVisibility()
									UpdatePanels()

									If Not AppSettings("ClientCode") = "APFT" Or
									   AppSettings("ClientCode") = "AAP" Then 'Client code Added by Saylee on 5-Nov-2019, as APFT has Send Mail button to send mail,so auto mail needed
										SendMail() 'Added By Prashant 1-Nov-2018 StarAir1112018
									End If

									If AppSettings("ShowNewWOFlow") = "True" Then

										If mnWO.StatusID = 2 Then
											Response.Redirect("index.aspx")
										End If

									End If

								End If

							Else
								Session.Remove("IsValid")
							End If

						Catch ex As Exception
							Throw ex.GetBaseException
						End Try

					ElseIf MSGBoxCtrl.Sender = "WOStatus" Then

						Session("sender") = ""

						If Session("IsValid") Then

							Session.Remove("IsValid")
							'For BRD
							If MaxWOExceeded() Then
								Exit Sub
							End If
							'End

							If mnWO.WOStatusID = 3 And IsIssuedSparesReturned() = 1 Then

								MSGBoxCtrl.Show("Alert!", "You have not mentioned Used Qty. <Br> We consider Issued Qty is wholly used? <Br><Br> Do you want to continue?", "", MsgBoxStyle.YesNo, "IsIssuedSparesReturned")
								Session("IsValid") = True
								Exit Sub

							End If

							If Save(IsFromWOCompletion:=IIf(mnWO.WOStatusID = 3, True, False)) Then

								SetPage()
								SetGrid()
								SetNRCGrid() 'Added By Vikrant For WO NRC
								ControlVisibility()
								UpdatePanels()
								upnlJobType.Update()

								If mnWO.WOStatusID = 3 And mnWO.WOJobs.IsScheduledJobExists = True And (Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=88" Or Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=89" Or Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=92" Or Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=93") Then
									'Do Nothing
								ElseIf mnWO.WOStatusID = 4 Then 'Plan

									If mnWO.WOTools.Count > 0 Then 'Added by Saylee on 24-Sep-2019
										SendMailForToolsRequest()
									End If
									If mnWO.WOResourceCount > 0 Then 'Added by Shital on 11-Oct-2019
										NotifyMail()
									End If

								Else
									MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
								End If

								mWODetail = mnWO.WOStatus + ": " + mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy + IIf(Not mnWO.MachineID.Equals(Guid.Empty), " Aircraft : " + mnWO.RegNo, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
								MarkLog(IIf(mnWO.WOStatusID = 3, Action.PPCComplete, IIf(mnWO.WOStatusID = 4, Action.Planned, Action.Save)), "Work Order", mWODetail, ErrorType.NoError, mnWO.ID, EventLogID)

								'Added by Saylee on 26-Jun-2020, ALL25062020-1 
								If mnWO.WOStatusID = 3 And mnWO.WOJobs.IsMELJobExists Then

									If mnWO.WOJobs.IsSingleMELJobExists = True Then

										If AppSettings("ShowNewDiscrepancyFlow") = "True" Then
											MSGBoxCtrl.Show("Alert!", "You have completed Work Order of Discrepancy. <Br><Br> Do you want to enter Rectification details in Discrepancy and Close it?", "", MsgBoxStyle.YesNo, "IsMELJob")
										Else
											MSGBoxCtrl.Show("Alert!", "You have completed Work Order of " + IIf(AppSettings("MELSnagNomenclature") = "True", "Defect/ADD", "Snag/MEL") + ". <Br><Br> Do you want to enter Rectification details in " + IIf(AppSettings("MELSnagNomenclature") = "True", "Defect/ADD", "MEL/Snag") + " and Close it?", "", MsgBoxStyle.YesNo, "IsMELJob")
										End If

										Exit Sub

									Else

										If AppSettings("ShowNewDiscrepancyFlow") = "True" Then
											MSGBoxCtrl.Show("Alert!", "You have a multiple Discrepancy Jobs. <Br><Br> You need close them individually through Discrepancy?", "", MsgBoxStyle.OkOnly, "IsMELMulJob")
										Else
											MSGBoxCtrl.Show("Alert!", "You have a multiple " + IIf(AppSettings("MELSnagNomenclature") = "True", "ADD", "MEL") + " Jobs. <Br><Br> You need close them individually through " + IIf(AppSettings("MELSnagNomenclature") = "True", "ADD/Defect", "MEL/Snag") + "?", "", MsgBoxStyle.OkOnly, "IsMELMulJob")
										End If

										Exit Sub

									End If

								End If
								'*********************

								If Session("wfProject_Ajax") = "wfProject_Ajax" Then 'Added By Prashant on 3-May-2024

									Session("MiddleFrame") = "wfProjectList_Ajax.aspx?TransTypeID=" & Session("TransTypeID").ToString
									Dim mopenas As String = Request.QueryString("Type")
									ScriptManager.RegisterStartupScript(Me, [GetType], "on close", "CallParentCallback();", True)
									Exit Sub

								Else
									Response.Redirect("index.aspx")
								End If

							End If

						Else
							Session.Remove("IsValid")
						End If

						'Added by Saylee on 26-Jun-2020, ALL25062020-1 
					ElseIf MSGBoxCtrl.Sender = "IsMELMulJob" Then '
						Response.Redirect("index.aspx")
					ElseIf MSGBoxCtrl.Sender = "IsMELJob" Then

						Dim mMELSnagCorrectiveAction As MELSnagCorrectiveAction
						mMELSnagCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(mnWO.WOJobs.GetMELJobID)
						mMELSnagCorrectiveAction.Action = mnWO.WOJobs(0).WOJobAction  ''mnWO.WORemark

						mMELSnagCorrectiveAction.Save()
						Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
						Session("MachineID") = mMELSnagCorrectiveAction.MachineID.ToString
						Dim mtmpLog As Log = Log.GetLog(mMELSnagCorrectiveAction.LogID)
						Session("tmpLogDate") = mtmpLog.Date


						If mMELSnagCorrectiveAction.IsAttachmentAdded Then
							Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mMELSnagCorrectiveAction.ID) 'Sort = 1 - Installation
							Session("mFileAttach") = mFileAttach
						Else
							Dim mFileAttach As FileAttach
							mFileAttach = FileAttach.NewAttachment(Guid.Empty, mMELSnagCorrectiveAction.ID)
							Session("mFileAttach") = mFileAttach
						End If
						Dim URLFromDueReportPreview As Stack = CType(Session("URLFromDueReportPreview"), Stack)

						If URLFromDueReportPreview IsNot Nothing Then

							If URLFromDueReportPreview.Count > 0 Then

								If Session("wfMELSnagCorrectiveActionNew_AJAX") = "wfMELSnagCorrectiveActionNew_AJAX" Then

									mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")
									mMELSnagCorrectiveAction.IsWOCreated = True
									mMELSnagCorrectiveAction.WONumber = mnWO.WONumber & vbCrLf & mnWO.WODateFormatted
									mMELSnagCorrectiveAction.WOID = mnWO.ID
									Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction

									Session("MiddleFrame") = "wfMELSnagCorrectiveActionListNew_AJAX.aspx?"

								ElseIf Session("wfLogDefectActionList_Ajax") = "wfLogDefectActionList_Ajax" Then

									Session("MiddleFrame") = "wfLogList.aspx"
									If Session("LogFromMEL") IsNot Nothing Then
										Session("LogFromMEL") = Log.GetLog(CType(Session("LogFromMEL"), Log).ID)
									End If
									Session.Remove("mMELSnagCorrectiveAction")
									Dim mopenas As String = Request.QueryString("Type")
									If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
										ScriptManager.RegisterStartupScript(Me, [GetType], "on close", "CallParentCallback();", True)
										Exit Sub
									End If

								ElseIf Session("wfProject_Ajax") = "wfProject_Ajax" Then 'Added By Prashant on 3-May-2024

									Session("MiddleFrame") = "wfProjectList_Ajax.aspx?TransTypeID=" & Session("TransTypeID").ToString
									Dim mopenas As String = Request.QueryString("Type")
									If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
										ScriptManager.RegisterStartupScript(Me, [GetType], "on close", "CallParentCallback();", True)
										Exit Sub
									End If

								End If

								Session.Remove("URLFromDueReportPreview")
								Response.Redirect(URLFromDueReportPreview.Peek.ToString)
								Exit Sub

							End If

						End If

						'******************
					ElseIf MSGBoxCtrl.Sender = "WOBillingStatus" Then

						Session("sender") = ""

						If Session("IsValid") Then

							Session.Remove("IsValid")
							'For BRD
							If MaxWOExceeded() Then
								Exit Sub
							End If
							'End

							Dim BillingRequired As String = ""
							If rdbBillingDone.Checked Then
								mnWO.BillingRequired = 1
								BillingRequired = "Billing"
							ElseIf rdbBillingNotRequired.Checked Then
								mnWO.BillingRequired = 2
								BillingRequired = "Billing Not Required"
							Else
								mnWO.BillingRequired = 0
								BillingRequired = ""
							End If

							If rdbBillingDone.Checked Then
								mnWO.BillingDate = Trim(txtBillingDate.Text)
								mnWO.BillingInvoiceNumber = Trim(txtInvoiceNumber.Text)
								mnWO.BillingRemark = Trim(txtBillingRemark.Text)
								mnWO.BillingBy = Trim(txtBillingBy.Text)
								BillingRequired = BillingRequired + " on " + Trim(txtBillingDate.Text) + " ,Billing Invoice No : " + Trim(txtInvoiceNumber.Text) + " Billing by : " + Trim(txtBillingBy.Text)
							End If

							mnWO.Save()
							mWODetail = BillingRequired + " " + mnWO.WOStatus + ": " + mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy + IIf(Not mnWO.MachineID.Equals(Guid.Empty), " Aircraft : " + mnWO.RegNo, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
							MarkLog(IIf(mnWO.BillingRequired = 1, Action.Billed, IIf(mnWO.BillingRequired = 2, Action.BillingNotRequired, Action.Save)), "Work Order", mWODetail, ErrorType.NoError, mnWO.ID, EventLogID)
							SetPage()
							SetGrid()
							SetNRCGrid() 'Added By Vikrant For WO NRC
							ControlVisibility()
							UpdatePanels()
							upnlJobType.Update()
							btnBilling.Enabled = False
							MSGBoxCtrl.Show("Billing Alert!", "Billing Details updated Successfully!", "", MsgBoxStyle.OkOnly, "")
						Else
							Session.Remove("IsValid")
						End If

					ElseIf MSGBoxCtrl.Sender = "IsIssuedSparesReturned" Then

						Session("sender") = ""

						If Session("IsValid") Then

							Session.Remove("IsValid")
							'For BRD
							If MaxWOExceeded() Then
								Exit Sub
							End If
							'End
							Save(IsFromWOCompletion:=IIf(mnWO.WOStatusID = 3, True, False))
							SaveIssuedSpares() 'this saves the Issued Qty as WO used Qty
							SetPage()
							SetGrid()
							ControlVisibility()
							UpdatePanels()

							If Session("MiddleFrame") = "wfnWOExecutionList.aspx" Then
								Response.Redirect("index.aspx")
							End If

						Else
							Session.Remove("IsValid")
						End If

					ElseIf MSGBoxCtrl.Sender = "WOQCStatus" Then

						Session("sender") = ""

						If Session("IsValid") Then

							Session.Remove("IsValid")
							'For BRD
							If MaxWOExceeded() Then
								Exit Sub
							End If
							'End

							Save()
							mWODetail = mnWO.IsQCStatusApprovedStatus + ": " + mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy + IIf(Not mnWO.MachineID.Equals(Guid.Empty), " Aircraft : " + mnWO.RegNo, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
							MarkLog(IIf(mnWO.IsQCStatusApproved = 1, Action.QCApproved, Action.QCRejected), "Work Order", mWODetail, ErrorType.NoError, mnWO.ID, EventLogID)
							SetPage()
							SetGrid()
							SetNRCGrid() 'Added By Vikrant For WO NRC
							ControlVisibility()
							UpdatePanels()
							upnlJobType.Update()
							MSGBoxCtrl.Show("QC Alert!", "QC Details updated Successfully!", "", MsgBoxStyle.OkOnly, "")

						Else
							Session.Remove("IsValid")
						End If

					ElseIf MSGBoxCtrl.Sender = "ComplyJobs" Then 'Added by Saylee on 22-Aug-2012

						Session("sender") = ""
						Session.Remove("IsValid")
						Session("IsWOForRemovedOrSpareAssembly") = False
						Session("IsWOForRemovedOrSpareComp") = False

						If mnWO.TransTypeID = Trans.SpareAssemblyWO Then
							Session("IsWOForRemovedOrSpareAssembly") = mRemovedAssemblyListForCombo(New Guid(cmbAssembly.SelectedValue.ToString)).IsSpareAssembly
						ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then
							Session("IsWOForRemovedOrSpareComp") = mRemovedCompListForCombo(New Guid(cmbCompList.SelectedValue.ToString)).IsSpareComp
						End If

						Response.Redirect("wfnWOForMulticompliance_Ajax.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))

					ElseIf MSGBoxCtrl.Sender = "RemoveAttachment" Then

						Try

							Session("Sender") = ""
							Dim mnWO As nWO
							mnWO = CType(Session("mnWO"), nWO)
							mnWO.FileAttachments.Remove(mnWO.FileAttachments.CurrentItem)
							dgWOAttachment.DataSource = mnWO.FileAttachments
							dgWOAttachment.DataBind()
							mnWO.IsAttachmentAdded = IIf(mnWO.FileAttachments.Count > 0, True, False) 'Added By Sachin 30-Jul-2024
							upnldgWOAttachment.Update()
							upnlWOAttachment.Update()
							Session("mnWO") = mnWO

						Catch ex As SqlException

							If ex.Number = 8145 Then

								MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
												MSGBox.Message_Text.ProcedureError,
												ex.Procedure,
												MsgBoxStyle.OkOnly,
												"")

							ElseIf ex.Number = 2627 Then

								MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
												MSGBox.Message_Text.Duplicate,
												ex.Procedure,
												MsgBoxStyle.OkOnly,
												"")


							ElseIf ex.Number = 8144 Then

								MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
												MSGBox.Message_Text.ReferenceDelete,
												ex.Procedure + "," + ex.Message,
												MsgBoxStyle.OkOnly,
												"")

							End If

						End Try

					ElseIf MSGBoxCtrl.Sender = "SignatureRequired" Then

						Print(SignatureRequired:=True, ByMail:=IIf(Session("btnSendMail") = "btnSendMail", True, False))

						Dim Text As String = ""
						If AppSettings("ClientCode") = "APFT" Or
						   AppSettings("ClientCode") = "AAP" Then
							Text = " CALL-OUT/Work Order - " + mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
						Else
							Text = mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
						End If

						If Session("btnSendMail") = "btnSendMail" Then

							Session.Remove("btnSendMail")
							SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Work Order Details", Text.ToString, "",
							 "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
							  SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))

						End If

					ElseIf MSGBoxCtrl.Sender = "SignatureRequiredForPrintWithJobAttachment" Then

						PrintWithJobAttachment(SignatureRequired:=True, ByMail:=IIf(Session("btnSendMail") = "btnSendMail", True, False))

						Dim Text As String = ""
						If AppSettings("ClientCode") = "APFT" Or
						   AppSettings("ClientCode") = "AAP" Then
							Text = " CALL-OUT/Work Order - " + mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
						Else
							Text = mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
						End If

						If Session("btnSendMail") = "btnSendMail" Then

							Session.Remove("btnSendMail")
							SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Work Order Details", Text.ToString, "",
							 "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
							  SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))

						End If

					ElseIf MSGBoxCtrl.Sender = "CompleteAllJobs" Then

						'JOBS

						For Each tmpJob As nWOJob In mnWO.WOJobs  'JOBS

							If txtJobStartDate.Text.ToString <> "" Then
								If txtJobStartDateTime.Text <> "" Then
									tmpJob.WOJobStartDate = CType(txtJobStartDate.Text.ToString.Trim + " " + txtJobStartDateTime.Text.ToString.Trim, DateTime)
								Else
									tmpJob.WOJobStartDate = txtJobStartDate.Text
								End If
							Else
								tmpJob.WOJobStartDate = DBNull.Value
							End If

							If txtJobEndDate.Text.ToString <> "" Then

								If txtJobEndDateTime.Text <> "" Then
									tmpJob.WOJobCloseDate = CType(txtJobEndDate.Text.ToString.Trim + " " + txtJobEndDateTime.Text.ToString.Trim, DateTime)
								Else
									tmpJob.WOJobCloseDate = txtJobEndDate.Text
								End If

							Else
								tmpJob.WOJobCloseDate = DBNull.Value
							End If

							'Job NRC(s)
							Dim mWOJobNRCList As WOJobNRCList
							mWOJobNRCList = WOJobNRCList.GetWOJobNRCList(mnWO.ID, tmpJob.ID)
							Dim tmpNRCJob As nWOJob


							If mWOJobNRCList.Count > 0 Then

								For Each tmpNRCJobInfo As WOJobNRCList.WOJobNRCListInfo In mWOJobNRCList

									tmpNRCJob = nWOJob.GetWOJob(tmpNRCJobInfo.ID)
									If txtJobStartDate.Text.ToString <> "" Then

										If txtJobStartDateTime.Text <> "" Then
											tmpNRCJob.WOJobStartDate = CType(txtJobStartDate.Text.ToString.Trim + " " + txtJobStartDateTime.Text.ToString.Trim, DateTime)
										Else
											tmpNRCJob.WOJobStartDate = txtJobStartDate.Text
										End If

									Else
										tmpNRCJob.WOJobStartDate = DBNull.Value
									End If

									If txtJobEndDate.Text.ToString <> "" Then

										If txtJobEndDateTime.Text <> "" Then
											tmpNRCJob.WOJobCloseDate = CType(txtJobEndDate.Text.ToString.Trim + " " + txtJobEndDateTime.Text.ToString.Trim, DateTime)
										Else
											tmpNRCJob.WOJobCloseDate = txtJobEndDate.Text
										End If

									Else
										tmpNRCJob.WOJobCloseDate = DBNull.Value
									End If
									tmpNRCJob.WOJobStatusID = 2
									tmpNRCJob.Save()

								Next

							End If

							tmpJob.WOJobStatusID = 2

						Next

						'WO NRC(S)
						For Each tmpNRCJob As nWOJob In mnWO.WONRCJobs

							If txtJobStartDate.Text.ToString <> "" Then

								If txtJobStartDateTime.Text <> "" Then
									tmpNRCJob.WOJobStartDate = CType(txtJobStartDate.Text.ToString.Trim + " " + txtJobStartDateTime.Text.ToString.Trim, DateTime)
								Else
									tmpNRCJob.WOJobStartDate = txtJobStartDate.Text
								End If

							Else
								tmpNRCJob.WOJobStartDate = DBNull.Value
							End If

							If txtJobEndDate.Text.ToString <> "" Then

								If txtJobEndDateTime.Text <> "" Then
									tmpNRCJob.WOJobCloseDate = CType(txtJobEndDate.Text.ToString.Trim + " " + txtJobEndDateTime.Text.ToString.Trim, DateTime)
								Else
									tmpNRCJob.WOJobCloseDate = txtJobEndDate.Text
								End If

							Else
								tmpNRCJob.WOJobCloseDate = DBNull.Value
							End If

							tmpNRCJob.WOJobStatusID = 2

						Next

						Session("mnWO") = mnWO
						If Save() Then

							mWODetail = mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy + IIf(Not mnWO.MachineID.Equals(Guid.Empty), " Aircraft : " + mnWO.RegNo, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")

							SetPage()
							SetGrid()
							SetNRCGrid()
							ControlVisibility()
							UpdatePanels()
							upnlJobType.Update()
							MSGBoxCtrl.Show("Job Alert!", "Job(s) completed Successfully!", "", MsgBoxStyle.OkOnly, "")
							mdlPopUpChangeCloseAll.Hide()

						End If

					End If

				Case MsgBoxResult.No

					If MSGBoxCtrl.Sender = "Close" Then
						Session.Remove("IsValid")
						Session.Remove("mTypeList")
						Session.Remove("ReportLogRegister")
						If mnWO.IsNew Then
							Session.Remove("mnWO")
						End If
						Session("Sender") = ""
						'Added By Vikrant on 14-Jun-2018 For ALL14062018
						Session("IsBackFromCompliance") = "True"
						Dim URLFromDueReportPreview As Stack = CType(Session("URLFromDueReportPreview"), Stack)
						If URLFromDueReportPreview IsNot Nothing Then
							If URLFromDueReportPreview.Count > 0 Then
								If Session("wfSearchCriteriaForMaintenanceAdviceFromQC") = "wfSearchCriteriaForMaintenanceAdviceFromQC" Then
									Session("MiddleFrame") = "wfSearchCriteriaForMaintenanceAdviceFromQC_Ajax.aspx?DueType=" & Session("DueType").ToString
								ElseIf Session("wfSearchCriteriaForDueWithAircraftSelection") = "wfSearchCriteriaForDueWithAircraftSelection" Then
									Session("MiddleFrame") = "wfSearchCriteriaForDueWithAircraftSelection.aspx?DueType=" & Session("DueType").ToString
								ElseIf Session("wfMELSnagCorrectiveActionNew_AJAX") = "wfMELSnagCorrectiveActionNew_AJAX" Then
									Session("MiddleFrame") = "wfMELSnagCorrectiveActionListNew_AJAX.aspx?"
								ElseIf Session("wfLogDefectActionList_Ajax") = "wfLogDefectActionList_Ajax" Then
									Session("MiddleFrame") = "wfLogList.aspx"
								ElseIf Session("wfDueJobPlanning_Ajax") = "wfDueJobPlanning_Ajax" Then
									Session("MiddleFrame") = "wfDueJobPlanningList_Ajax.aspx?"
								ElseIf Session("wfProject_Ajax") = "wfProject_Ajax" Then 'Added By Prashant on 3-May-2024
									Session("MiddleFrame") = "wfProjectList_Ajax.aspx?TransTypeID=" & Session("TransTypeID").ToString
								Else
									Session("MiddleFrame") = "wfSearchCriteriaForDue_Ajax.aspx?DueType=" & Session("DueType").ToString
								End If
								Response.Redirect(URLFromDueReportPreview.Peek.ToString)
								Exit Sub
							End If
						End If
						'End
						Response.Redirect("Index.aspx")
					ElseIf MSGBoxCtrl.Sender = "WOStatus" Then
						Session.Remove("IsValid")
						If (mnWO.WOStatusID = 3 Or mnWO.WOStatusID = 4) And Session("sender") <> "Close" Then
							mnWO.WOStatusID = 1
						ElseIf mnWO.WOStatusID = 5 And Session("sender") <> "Close" Then
							mnWO.WOStatusID = 3
						ElseIf mnWO.WOStatusID = 9 And Session("sender") <> "Close" Then
							mnWO.WOStatusID = 5
						End If
						Session("sender") = ""
						mnWOApproveReject = Nothing
						Session("mnWOApproveReject") = mnWOApproveReject
						Session("mnWO") = mnWO
						'Response.Redirect("wfnWODetail_AJAX.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					ElseIf MSGBoxCtrl.Sender = "WOQCStatus" Then
						Session("sender") = ""
						Session.Remove("IsValid")
						If mnWO.IsQCStatusApproved = 1 And Session("sender") <> "Close" Then
							mnWO.IsQCStatusApproved = 0
							rdbApproved.Checked = False
						ElseIf mnWO.IsQCStatusApproved = 2 And Session("sender") <> "Close" Then
							mnWO.IsQCStatusApproved = 0
							rdbNotApproved.Checked = False
						End If
						rdbNone.Checked = True
						txtQcDate.Text = ""
						txtQcRemark.Text = ""
						UpnlApproval.Update()
						UpnlPrint.Update()
						Session("mnWO") = mnWO
					ElseIf MSGBoxCtrl.Sender = "IsIssuedSparesReturned" Then
						Session("sender") = ""
						Session.Remove("IsValid")
						If mnWO.WOStatusID = 3 And Session("sender") <> "Close" Then
							mnWO.WOStatusID = 1
						End If
						Session("mnWO") = mnWO
						'Response.Redirect("wfnWODetail_AJAX.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

					ElseIf MSGBoxCtrl.Sender = "Status" Then
						Session("sender") = ""
						Session.Remove("IsValid")
						If mnWO.StatusID = 2 And Session("sender") <> "Close" Then
							mnWO.StatusID = 1
						ElseIf mnWO.StatusID = 4 Then
							'mnWO.StatusID = 2            
							'Added by Shital on 11-Oct-2019
							mnWO.StatusID = PrevStatusID
							Session.Remove("PrevStatusID")
							PrevStatusID = Nothing
						ElseIf mnWO.StatusID = 1 Then
							mnWO.StatusID = 2
						End If
						Session("mnWO") = mnWO
						DataFieldBind()
						SetPage()
						SetGrid()
						ControlVisibility()
						UpdatePanels()
						'Response.Redirect("wfnWODetail_AJAX.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					ElseIf MSGBoxCtrl.Sender = "ComplyJobs" Then 'Added by Saylee on 22-Aug-2012
						Session("Sender") = ""
						'Response.Redirect("wfnWODetail_AJAX.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					ElseIf MSGBoxCtrl.Sender = "SignatureRequired" Then
						Print(SignatureRequired:=False, ByMail:=IIf(Session("btnSendMail") = "btnSendMail", True, False))

						Dim Text As String = ""
						If AppSettings("ClientCode") = "APFT" Or
						   AppSettings("ClientCode") = "AAP" Then
							Text = " CALL-OUT/Work Order - " + mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
						Else
							Text = mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
						End If
						If Session("btnSendMail") = "btnSendMail" Then
							Session.Remove("btnSendMail")
							SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Work Order Details", Text.ToString, "",
							 "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
							  SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
						End If
					ElseIf MSGBoxCtrl.Sender = "IsMELJob" Then
						Session("Sender") = ""
					Else
						Session("Sender") = ""
						'Response.Redirect("wfnWODetail_AJAX.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					End If

					DataFieldBind()
					SetPage()
					SetGrid()
					SetNRCGrid() 'Added By Vikrant For WO NRC
					ControlVisibility()
					UpdatePanels()

				Case MsgBoxResult.Ok 'And Session("sender") = ""

					If MSGBoxCtrl.Sender = "Status" Then
						If mnWO.StatusID = 2 And Session("sender") <> "Close" Then
							mnWO.StatusID = 1
						ElseIf mnWO.StatusID = 4 Then
							mnWO.StatusID = 2
						ElseIf mnWO.StatusID = 1 Then
							mnWO.StatusID = 2
						End If
						Session("mnWO") = mnWO
					End If

					If MSGBoxCtrl.Sender = "NotInUseSelectInWODate" Then
						txtWODate.Text = mnWO.WODateFormatted.ToString
						UpnlWODet.Update()
						Exit Sub
					End If

					If MSGBoxCtrl.Sender = "NotInUseSelectInWOStartDate" Then
						txtStartDate.Text = mnWO.WOStartDateFormatted.ToString
						upnlStartDetails.Update()
						Exit Sub
					End If

					Session("sender") = ""
					DataFieldBind()
					SetPage()
					SetGrid()
					SetNRCGrid() 'Added By Vikrant For WO NRC
					ControlVisibility()
					UpdatePanels()

			End Select

		ElseIf Result1 = -1 Then

			If MSGBoxCtrl.Sender = "Status" Then

				If mnWO.StatusID = 2 And Session("sender") <> "Close" Then
					mnWO.StatusID = 1
				ElseIf mnWO.StatusID = 4 Then
					mnWO.StatusID = 2
				ElseIf mnWO.StatusID = 1 Then
					mnWO.StatusID = 2
				End If

			End If

			If MSGBoxCtrl.Sender = "WOStatus" Then
				If mnWO.WOStatusID = 3 And Session("sender") <> "Close" Then
					mnWO.WOStatusID = 1
				End If
			End If

			Session("mnWO") = mnWO
			Session("sender") = ""

			DataFieldBind()
			SetPage()
			SetGrid()
			SetNRCGrid() 'Added By Vikrant For WO NRC
			ControlVisibility()
			UpdatePanels()

		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added

			Session("sender") = ""
			DataFieldBind()
			SetGrid()

		End If

	End Sub

	Private Sub ControlVisibility(Optional IsRemovedAssembly As Boolean = False) 'IsRemovedAssembly Added By Vikrant On 27-Jul-2020 For ALL27072020

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso
			(AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then

			lblTitle.Text = "E.O. Detail"
			ldwodetail.InnerText = "E.O. Detail"
			lblStatus.Text = mnWO.WOStatus
			lblWOJobs.Text = "E.O. Jobs"
			lblRequiredToolList.Text = "E.O. Tools"
			btnPrintAll.Text = "Print FOD"

			'TransTypeID Code added by Saylee on 5-Sep-2018
			If mnWO.TransTypeID = Trans.WO145 Then

				phPrintNC.Visible = True
				phPrintBlankEO.Visible = True

			ElseIf mnWO.TransTypeID = Trans.WOCAMO Then

				phPrintNC.Visible = False
				phPrintBlankEO.Visible = False
				chkMaintenance.Checked = True

			End If

			btnPrintAll.ToolTip = "Click to Print Blank FOD"
			dgWOTools.ToolTip = "List of E.O Tools"
			dgWOJobs.ToolTip = "List of E.O Jobs"
			dgCurrentPeriodValue.ToolTip = "List of E.O Periods"
			tmpText = "Engineering Order"
			Session("tmpText") = tmpText

		Else

			lblTitle.Text = "W.O. Detail"
			ldwodetail.InnerText = "W.O. Detail"
			lblStatus.Text = mnWO.WOStatus
			lblWOJobs.Text = "W.O. Jobs"
			lblRequiredToolList.Text = "W.O. Tools"
			btnPrintAll.Text = "Print All"
			phPrintNC.Visible = False
			phPrintBlankEO.Visible = False
			dgWOTools.ToolTip = "List of W.O Tools"
			dgWOJobs.ToolTip = "List of W.O Jobs"
			dgCurrentPeriodValue.ToolTip = "List of W.O Periods"
			tmpText = "Work Order"
			Session("tmpText") = tmpText

		End If

		If AppSettings("ClientCode") = "IND" Then

			txtWODate.Enabled = False

			If mnWO.StatusID = 1 Then
				txtWOTime.Enabled = IIf(mnWO.IsScheduledJobPresent, False, True)
			Else
				txtWOTime.Enabled = False
			End If

			lblAddWONRC.Text = "W.O. OJS"
			btnAddNRC.ToolTip = "Click to Add WO OJS"
			btnPrintNRC.Text = "Print OJS"
			lblIssueTo.Text = "AMO Ref"
			lblPlanDate.Text = "PPC Work Order Date" 'IND

		Else

			'Added By Vikrant On 18-Feb-2021
			If AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "SPZ" Then ' SPZ Code added by Saylee on 13-Jun-2022 
				lblIssueTo.Text = "MRO Name"
			End If
			'End

			If mnWO.StatusID = 1 Then
				txtWODate.Enabled = IIf(mnWO.IsScheduledJobPresent, False, True)
				txtWOTime.Enabled = IIf(mnWO.IsScheduledJobPresent, False, True) 'Added By Saylee on 26-Sep-2018 for STR26092018,  Star Air needs Time with Date    
			Else
				txtWODate.Enabled = False
				txtWOTime.Enabled = False 'Added By Saylee on 26-Sep-2018 for STR26092018,  Star Air needs Time with Date
			End If
			'End If

			If Session("OpenFromProject") = "OpenFromProject" Then
				txtWODate.Enabled = False
				txtWOTime.Enabled = False
			End If

			If AppSettings("ClientCode") = "TSL" Then 'Added By Saylee On 23-Feb-2022
				lblRevisionNo.Text = "Manual Reference/Revision"
			End If

			lblAddWONRC.Text = "W.O. NRC"
			btnAddNRC.ToolTip = "Click to Add WO NRC"
			btnPrintNRC.Text = "Print NRC"
			lblPlanDate.Text = "Plan Date" 'IND

		End If

		btnSave.Visible = IIf(mnWO.WOStatusID = 3 Or mnWO.IsQCStatusApproved = 1 Or mnWO.IsCAMOUpdated = 1 Or mnWO.WOStatusID = 9, False, True) And IIf(mnWO.StatusID = 4, False, True)
		btnCancel.Visible = (Not mnWO.IsNew) And IIf(mnWO.StatusID = 4, False, True) And (mnWO.WOStatusID <> 3 And mnWO.IsQCStatusApproved <> 1 And mnWO.WOStatusID <> 8 And mnWO.WOStatusID <> 9)     'One Condition Added by Saylee on 2-June-2010

		If AppSettings("ClientCode") = "RAL" Then

			btnAuthorize.Visible = (Not (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)) And (Not mnWO.IsNew) And (mnWO.StatusID = 1)
			btnComplete.Visible = (Not (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.WOJobs.IsCompleted = True And mnWO.WONRCJobs.IsCompleted) And (mnWO.WOStatusID <> 3 And mnWO.WOStatusID <> 5 And mnWO.WOStatusID <> 8 And mnWO.WOStatusID <> 9)  'And (IsInRole(Rights.[New]) And IsInRole(Rights.Edit) And IsInRole(Rights.Delete) And IsInRole(Rights.View) And IsInRole(Rights.Print))
			'Added by Shital on 09-Oct-2019
			txtPPCRemark.Visible = (Not (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.WOJobs.IsCompleted = True And mnWO.WONRCJobs.IsCompleted) And (mnWO.WOStatusID <> 3 And mnWO.WOStatusID <> 8 And mnWO.WOStatusID <> 9)
			lblPPCRemark.Visible = (Not (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.WOJobs.IsCompleted = True And mnWO.WONRCJobs.IsCompleted) And (mnWO.WOStatusID <> 3 And mnWO.WOStatusID <> 8 And mnWO.WOStatusID <> 9)

			cmbAircraftList.Enabled = (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0) And chkMaintenance.Checked   'Added by Utkarsh 13-Dec-2010
			txtRegNo.Enabled = (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)
			cmbHourTypeList.Enabled = (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)
			txtModelNo.Enabled = (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)
			txtSerialNo.Enabled = (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)
			txtCloseDate.Enabled = ((Not (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And IIf(mnWO.WOStatusID = 3, False, True)) And (IIf(mnWO.StatusID = 4, False, True))
			txtClosedDateTime.Enabled = ((Not (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And IIf(mnWO.WOStatusID = 3, False, True)) And (IIf(mnWO.StatusID = 4, False, True))
			txtClosedBy.Enabled = ((Not (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And IIf(mnWO.WOStatusID = 3, False, True)) And (IIf(mnWO.StatusID = 4, False, True))
			fldClosingDet.Visible = (Not (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.WOJobs.IsCompleted = True And mnWO.WONRCJobs.IsCompleted) And (mnWO.WOStatusID <> 3 And mnWO.WOStatusID <> 5 And mnWO.WOStatusID <> 8 And mnWO.WOStatusID <> 9)  'And (IsInRole(Rights.[New]) And IsInRole(Rights.Edit) And IsInRole(Rights.Delete) And IsInRole(Rights.View) And IsInRole(Rights.Print))

		Else

			btnAuthorize.Visible = (Not mnWO.WOJobs.Count = 0) And (Not mnWO.IsNew) And (mnWO.StatusID = 1)
			btnAuthorize.Text = IIf(AppSettings("ClientCode") = "IND", "Authorize", "Submit")
			btnComplete.Visible = (Not mnWO.WOJobs.Count = 0) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.WOJobs.IsCompleted = True And mnWO.WONRCJobs.IsCompleted) And (mnWO.WOStatusID <> 3 And mnWO.WOStatusID <> 5 And mnWO.WOStatusID <> 8 And mnWO.WOStatusID <> 9)  'And (IsInRole(Rights.[New]) And IsInRole(Rights.Edit) And IsInRole(Rights.Delete) And IsInRole(Rights.View) And IsInRole(Rights.Print))
			cmbAircraftList.Enabled = (mnWO.WOJobs.Count = 0) And chkMaintenance.Checked   'Added by Utkarsh 13-Dec-2010
			cmbHourTypeList.Enabled = (mnWO.WOJobs.Count = 0)

			If Session("wfProject_Ajax") = "wfProject_Ajax" And mnWO.ModelName <> "" Then 'Added By Prashant on 14-May-2024
				txtModelNo.Enabled = False
				txtSerialNo.Enabled = False
				cmbCustomerList.Enabled = False
				txtRegNo.Enabled = False
				cmbAircraftList.Enabled = False
				cmbJobType.SelectedValue = mnWO.WOJobTypeID.ToString
				cmbJobType.Enabled = False

			Else
				txtModelNo.Enabled = (mnWO.WOJobs.Count = 0)
				txtSerialNo.Enabled = (mnWO.WOJobs.Count = 0)
				txtRegNo.Enabled = (mnWO.WOJobs.Count = 0)
			End If

			txtCloseDate.Enabled = ((Not mnWO.WOJobs.Count = 0) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And IIf(mnWO.WOStatusID = 3, False, True)) And (IIf(mnWO.StatusID = 4, False, True))
			txtClosedDateTime.Enabled = ((Not mnWO.WOJobs.Count = 0) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And IIf(mnWO.WOStatusID = 3, False, True)) And (IIf(mnWO.StatusID = 4, False, True))
			txtClosedBy.Enabled = ((Not mnWO.WOJobs.Count = 0) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And IIf(mnWO.WOStatusID = 3, False, True)) And (IIf(mnWO.StatusID = 4, False, True))
			fldClosingDet.Visible = (Not mnWO.WOJobs.Count = 0) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.WOJobs.IsCompleted = True And mnWO.WONRCJobs.IsCompleted) And (mnWO.WOStatusID <> 3 And mnWO.WOStatusID <> 5 And mnWO.WOStatusID <> 8 And mnWO.WOStatusID <> 9)  'And (IsInRole(Rights.[New]) And IsInRole(Rights.Edit) And IsInRole(Rights.Delete) And IsInRole(Rights.View) And IsInRole(Rights.Print))
			'Added by Shital on 09-Oct-2019
			txtPPCRemark.Visible = True '(Not mnWO.WOJobs.Count = 0) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.WOJobs.IsCompleted = True And mnWO.WONRCJobs.IsCompleted) And (mnWO.WOStatusID <> 3)
			lblPPCRemark.Visible = True '(Not mnWO.WOJobs.Count = 0) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.WOJobs.IsCompleted = True And mnWO.WONRCJobs.IsCompleted) And (mnWO.WOStatusID <> 3)
			txtPPCRemark.Enabled = ((Not mnWO.WOJobs.Count = 0) And (Not mnWO.IsNew) And (mnWO.StatusID = 2) And IIf(mnWO.WOStatusID = 3, False, True)) And (IIf(mnWO.StatusID = 4, False, True))
			lblstarPPCRemark.Visible = True
			'-----
		End If

		If mnWO.StatusID >= 4 Or (mnWO.WOStatusID = 3 Or mnWO.IsQCStatusApproved = 1 Or mnWO.IsCAMOUpdated = 1 Or mnWO.WOStatusID = 9) Then

			If mnWO.IsQCStatusApproved = 2 Then
				If mnWO.WOStatusID = 7 Then dgWOJobs.Columns(17).Visible = True
			Else
				If mnWO.IsQCStatusApproved = 1 Or mnWO.IsCAMOUpdated = 1 Or mnWO.WOStatusID = 9 Then dgWOJobs.Columns(18).Visible = False Else dgWOJobs.Columns(18).Visible = True 'Edit link
			End If

			chkFMC.Enabled = False
			dgWONRC.Columns(14).Visible = False 'Edit link  'Ajay 20-Sep-2022 Columns no.  
			dgWOTools.Columns(7).Visible = False 'Tools Edit link

		Else

			If Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Or Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID Then
				dgWOJobs.Columns(16).Visible = False 'Installation/Removal link
				dgWOJobs.Columns(17).Visible = False 'NRC link
			Else
				dgWOJobs.Columns(16).Visible = IIf((AppSettings("ShowMaintenanceForNewClients") = "True" And (AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True")) Or Session("wfProject_Ajax") = "wfProject_Ajax", False, True) 'True   'Installation/Removal link
				dgWOJobs.Columns(17).Visible = True 'NRC link
			End If

			If Session("wfProject_Ajax") = "wfProject_Ajax" And mnWO.ModelName <> "" Then 'Added By Prashant on 14-May-2024
				chkFMC.Enabled = False
			Else
				chkFMC.Enabled = True
			End If
			dgWONRC.Columns(14).Visible = True 'WONRC Edit link 'Ajay 20-Sep-2022 Columns no.
			dgWOTools.Columns(7).Visible = True 'Tools Edit link

		End If

		If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID Then

			btnComplete.Visible = False
			'Added by Shital on 09-Oct-2019
			txtPPCRemark.Visible = False
			lblPPCRemark.Visible = False
			'-----------
			btnSave.Visible = IIf(mnWO.StatusID = 2, False, True)
			dgWOJobs.Columns(18).Visible = IIf(mnWO.StatusID = 2, False, True)   'Edit link
			dgWOJobs.Columns(19).Visible = IIf(mnWO.StatusID = 2, False, True)  'Remove link
			dgWONRC.Columns(14).Visible = IIf(mnWO.StatusID = 2, False, True) 'WONRC Edit link 'Ajay 20-Sep-2022 Columns no.
			dgWOTools.Columns(7).Visible = IIf(mnWO.StatusID = 2, False, True) 'Tools Edit link
			btnReject.Visible = (mnWO.StatusID = 2) And Not mnWO.IsNew
			fldRemark.Visible = (mnWO.StatusID = 2) And Not mnWO.IsNew

		End If

		If Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Then

			If mnWO.WOStatusID = 4 Then

				btnPlan.Visible = False
				'Added by Shital on 09-Oct-2019                
				btnComplete.Visible = False
				'Added by Shital on 09-Oct-2019
				txtPPCRemark.Visible = False
				lblPPCRemark.Visible = False

			Else

				btnPlan.Visible = True
				'Added by Shital on 09-Oct-2019
				txtPlanningRemark.Visible = True
				lblPlanningRemark.Visible = True

			End If

			txtPlanDate.Enabled = IIf(mnWO.WOJobs.IsAleastOneJobCompleted = True, False, True)
			txtPlanDateTime.Enabled = IIf(mnWO.WOJobs.IsAleastOneJobCompleted = True, False, True)
			lblstarPlanDate.Visible = True

			'Added by Shital on 09-Oct-2019
			lblstarPlanningRemark.Visible = True
			lblstarCAMOUpdateRemark.Visible = False
			lblstarPPCRemark.Visible = False
			lblStatusRemark.Text = "Planning Remark"
			btnReject.Visible = (Not mnWO.WOStatusID = 4)
			fldRemark.Visible = (Not mnWO.WOStatusID = 4)

		Else

			btnPlan.Visible = False
			txtPlanDate.Enabled = False
			txtPlanDateTime.Enabled = False
			lblstarPlanDate.Visible = False
			btnQCApproval.Visible = False
			QcApproval.Visible = False
			'Added by Shital on 09-Oct-2019
			lblPlanningRemark.Visible = True
			txtPlanningRemark.Visible = True
			lblCAMOUpdateRemark.Visible = True
			txtCAMOUpdateRemark.Visible = True
			txtPPCRemark.Enabled = False
			txtPlanningRemark.Enabled = False
			lblstarPlanningRemark.Visible = False
			lblstarCAMOUpdateRemark.Visible = False
			lblstarPPCRemark.Visible = False

		End If

		If (Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=" & mnWO.TransTypeID) Or (Session("MiddleFrame") = "wfnWOJobList_AJAX.aspx") Then

			btnComplyJobs.Visible = (Not mnWO.IsNew) And (mnWO.WOJobs.IsCompleted = True) And (mnWO.WOJobs.IsScheduledJobExists) And (mnWO.WOJobs.IsJobsComplied = False) And (mnWO.WOStatusID = 3)
			'Added by Shital on 09-Oct-2019
			lblCAMOUpdateRemark.Visible = (Not mnWO.IsNew) And (mnWO.WOJobs.IsCompleted = True) And (mnWO.WOJobs.IsScheduledJobExists) And (mnWO.WOJobs.IsJobsComplied = False) And (mnWO.WOStatusID = 3)
			txtCAMOUpdateRemark.Visible = (Not mnWO.IsNew) And (mnWO.WOJobs.IsCompleted = True) And (mnWO.WOJobs.IsScheduledJobExists) And (mnWO.WOJobs.IsJobsComplied = False) And (mnWO.WOStatusID = 3)
			'----------
			btnBilling.Visible = False
			pnlBilling.Visible = False
			'Added by Shital on 09-Oct-2019
			txtPPCRemark.Visible = False
			lblPPCRemark.Visible = False
			txtPlanningRemark.Enabled = IIf(mnWO.WOJobs.IsAleastOneJobCompleted = True, False, True)
			'-----
			btnQCApproval.Visible = False
			QcApproval.Visible = False

			CustApproval.Visible = True
			txtPlanDate.Enabled = IIf(mnWO.WOJobs.IsAleastOneJobCompleted = True, False, True)

			fldClosingDet.Visible = True

			If mnWO.WOStatusID = 3 Then
				fldClosingDet.Disabled = True
				txtCloseDate.Enabled = False
			ElseIf mnWO.WOStatusID = 1 Then  'when click on No button of msg box  mnWO.WOStatusID = 3 change to  mnWO.WOStatusID = 1 in this case it 
				fldClosingDet.Disabled = False 'should be open to add closing details
				txtCloseDate.Enabled = True
			End If

		ElseIf Session("MiddleFrame") = "wfnWOCompletionList.aspx?" And Session("IsShowAllWOs") = False Then

			fldClosingDet.Visible = True
			fldClosingDet.Disabled = True
			txtCloseDate.Enabled = False
			btnQCApproval.Visible = False
			QcApproval.Visible = False
			btnBilling.Visible = False
			pnlBilling.Visible = False
			btnComplyJobs.Visible = False

			'Added by Shital on 09-Oct-2019
			lblCAMOUpdateRemark.Visible = False
			txtCAMOUpdateRemark.Visible = False
			'-----
			CustApproval.Visible = True
			txtPPCRemark.Enabled = True
			lblstarPPCRemark.Visible = True
			lblStatusRemark.Text = "PPC Remark"
			btnReject.Visible = Not (mnWO.WOStatusID = 3)

		ElseIf Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" And Session("IsShowAllWOs") = False Then

			btnQCApproval.Visible = True

			'Added by Prashant on 11-Oct-2019
			txtQcDate.Text = Today.Date.ToString("dd-MMM-yyyy")
			txtQcDate.Enabled = False
			'----------

			If mnWO.IsQCStatusApproved = 1 Then

				btnQCApproval.Enabled = False
				QcApproval.Enabled = False
				btnReject.Visible = False

			ElseIf mnWO.IsQCStatusApproved = 2 Then

				QcApproval.Enabled = False
				btnReject.Visible = False
				btnQCApproval.Enabled = False

			Else

				QcApproval.Enabled = True
				btnReject.Visible = True
				btnQCApproval.Enabled = True

			End If

			fldClosingDet.Visible = True
			fldClosingDet.Disabled = True
			txtCloseDate.Enabled = False
			btnBilling.Visible = False
			pnlBilling.Visible = False
			btnComplyJobs.Visible = False
			'Added by Shital on 09-Oct-2019
			txtPPCRemark.Visible = False
			lblPPCRemark.Visible = False
			txtCAMOUpdateRemark.Visible = True
			lblCAMOUpdateRemark.Visible = True
			lblstarCAMOUpdateRemark.Visible = True
			txtCAMOUpdateRemark.Enabled = False
			'-----
			btnComplete.Visible = False
			btnSave.Visible = False
			btnCancel.Visible = False
			CustApproval.Visible = False
			lblStatusRemark.Text = "QC Remark"

		ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1" And Session("IsShowAllWOs") = False Then

			btnComplyJobs.Visible = (Not mnWO.IsNew) And (mnWO.WOJobs.IsCompleted = True) And (mnWO.WOJobs.IsScheduledJobExists) And (mnWO.WOJobs.IsJobsComplied = False) And (mnWO.WOStatusID = 3)
			'Added by Shital on 09-Oct-2019
			lblCAMOUpdateRemark.Visible = True ' (Not mnWO.IsNew) And (mnWO.WOJobs.IsCompleted = True) And (mnWO.WOJobs.IsScheduledJobExists) And (mnWO.WOJobs.IsJobsComplied = False) And (mnWO.WOStatusID = 3)
			txtCAMOUpdateRemark.Visible = True '(Not mnWO.IsNew) And (mnWO.WOJobs.IsCompleted = True) And (mnWO.WOJobs.IsScheduledJobExists) And (mnWO.WOJobs.IsJobsComplied = False) And (mnWO.WOStatusID = 3)
			txtCAMOUpdateRemark.Enabled = (Not mnWO.IsNew) And (mnWO.WOJobs.IsCompleted = True) And (mnWO.WOJobs.IsScheduledJobExists) And (mnWO.WOJobs.IsJobsComplied = False) And (mnWO.WOStatusID = 3)
			lblstarCAMOUpdateRemark.Visible = (Not mnWO.IsNew) And (mnWO.WOJobs.IsCompleted = True) And (mnWO.WOJobs.IsScheduledJobExists) And (mnWO.WOJobs.IsJobsComplied = False) And (mnWO.WOStatusID = 3)
			'-----------
			btnBilling.Visible = False
			pnlBilling.Visible = False
			'Added by Shital on 09-Oct-2019
			txtPPCRemark.Visible = False
			lblPPCRemark.Visible = False
			'-----
			btnQCApproval.Visible = False
			QcApproval.Visible = False
			CustApproval.Visible = False
			btnReject.Visible = False
			fldRemark.Visible = True
			lblStatusRemark.Text = "CAMO Update Remark"

		ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0" And Session("IsShowAllWOs") = False Then

			btnBilling.Visible = (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.WOStatusID = 3 Or mnWO.IsCAMOUpdated = 1)
			pnlBilling.Visible = (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.WOStatusID = 3 Or mnWO.IsCAMOUpdated = 1)
			'Added by Shital on 09-Oct-2019
			txtPPCRemark.Visible = (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.WOStatusID = 3 Or mnWO.IsCAMOUpdated = 1)
			lblPPCRemark.Visible = (Not mnWO.IsNew) And (mnWO.StatusID = 2) And (mnWO.WOStatusID = 3 Or mnWO.IsCAMOUpdated = 1)
			txtPPCRemark.Enabled = False
			lblCAMOUpdateRemark.Visible = False
			txtCAMOUpdateRemark.Visible = False
			lblstarCAMOUpdateRemark.Visible = False
			'-----
			btnComplyJobs.Visible = False
			btnQCApproval.Visible = False
			QcApproval.Visible = False
			CustApproval.Visible = False
			btnReject.Visible = False
			fldRemark.Visible = False

			If rdbBillingDone.Checked Or rdbBillingNotRequired.Checked Then

				btnBilling.Enabled = True

				If rdbBillingDone.Checked Then

					lblBillingByStar.Visible = True
					lblBillingInvoiceNumberStar.Visible = True
					lblBillingStar.Visible = True

				Else

					lblBillingByStar.Visible = False
					lblBillingInvoiceNumberStar.Visible = False
					lblBillingStar.Visible = False

				End If

			ElseIf rdbBillingNone.Checked Then

				btnBilling.Enabled = False
				lblBillingByStar.Visible = False
				lblBillingInvoiceNumberStar.Visible = False
				lblBillingStar.Visible = False

			End If

			UpnlPrint.Update()

		ElseIf Session("MiddleFrame") = "wfnWOExecutionList.aspx" And Session("IsShowAllWOs") = False Then 'Added By Prashant 16-Aug-2019

			btnCancel.Visible = False
			btnComplete.Visible = False
			'Added by Shital on 09-Oct-2019
			txtPPCRemark.Visible = False
			lblPPCRemark.Visible = False
			lblCAMOUpdateRemark.Visible = False
			txtCAMOUpdateRemark.Visible = False
			lblstarCAMOUpdateRemark.Visible = False
			'----------
			btnAMECompletion.Visible = True
			btnSave.Visible = False
			btnBilling.Visible = False
			btnComplyJobs.Visible = False
			btnReject.Visible = False
			lblStatusRemark.Text = "AME Remark"

		Else

			btnBilling.Visible = False
			pnlBilling.Visible = False
			'Added by Shital on 09-Oct-2019
			txtPPCRemark.Visible = False
			lblPPCRemark.Visible = False
			lblCAMOUpdateRemark.Visible = False
			txtCAMOUpdateRemark.Visible = False
			lblstarCAMOUpdateRemark.Visible = False
			'-----
			btnComplyJobs.Visible = False
			btnQCApproval.Visible = False
			QcApproval.Visible = False
			CustApproval.Visible = False

		End If

		If Session("MiddleFrame") = "wfnWOExecutionList.aspx" And AppSettings("ClientCode") = "IND" Then 'Added By Prashant 16-Aug-2019
			btnLogBookEntry.Visible = True
		Else
			btnLogBookEntry.Visible = IIf(AppSettings("ClientCode") = "IND" And (mnWO.WOStatusID = 3 Or mnWO.WOStatusID = 4 Or mnWO.IsCAMOUpdated = 1), True, False)
		End If

		cmbLogList.Enabled = (mnWO.WOStartDate.ToString <> "" And Not mnWO.MachineID.Equals(Guid.Empty)) And IIf(mnWO.WOStatusID = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)
		'--------------Added By Utkarsh 14-Dec-2010
		btnAddTool.Enabled = IIf(rdbIsThirdParty.Checked, False, True) And (IIf(mnWO.WOStatusID = 3 Or mnWO.IsQCStatusApproved = 1 Or mnWO.IsCAMOUpdated = 1 Or mnWO.WOStatusID = 9, False, True)) And IIf(mnWO.StatusID = 4, False, True)
		'--------------------------------

		dgWOTools.Columns(7).Visible = IIf(rdbIsThirdParty.Checked, False, True) And IIf(mnWO.StatusID = 4, False, True) And IIf(mnWO.WOStatusID = 3 Or mnWO.WOStatusID = 7 Or mnWO.IsQCStatusApproved = 1 Or mnWO.IsCAMOUpdated = 1 Or mnWO.WOStatusID = 9, False, True)

		If mnWO IsNot Nothing And Not mnWO.MachineID.Equals(Guid.Empty) Then

			txtRegNo.ReadOnly = True
			txtModelNo.ReadOnly = True
			txtSerialNo.ReadOnly = True
			cmbHourTypeList.Enabled = False
			dgCurrentPeriodValue.Columns(3).Visible = False
			btnSelectPeriod.Enabled = False
			cmbAircraftList.Enabled = IIf((mnWO.WOJobs.Count > 0) Or (Session("wfProject_Ajax") = "wfProject_Ajax" And mnWO.ModelName <> ""), False, True)
			Session("mnWO") = mnWO
			dgWOJobs.Columns(4).Visible = True

		Else

			txtRegNo.ReadOnly = False
			txtModelNo.ReadOnly = False
			txtSerialNo.ReadOnly = False
			cmbHourTypeList.Enabled = True
			cmbLogList.ClearSelection()
			cmbLogList.DataSource = Nothing
			cmbLogList.DataBind()
			cmbLogList.Enabled = False
			dgCurrentPeriodValue.Columns(3).Visible = True

			If mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO Then 'Added By Vikrant On 27-Jul-2020 For ALL27072020

				btnSelectPeriod.Enabled = False
				'End

			Else

				btnSelectPeriod.Enabled = True

			End If

			dgWOJobs.Columns(4).Visible = False

		End If

		'Added By Prashant 17-Aug-2011
		If Not IsInRole(Rights.Authorized) Then
			btnAuthorize.Enabled = False
			btnAuthorize.ToolTip = "You are not authorized user "
			'Added by Vikrant On 03-Apr-2019 For ALL03042019
			btnSaveAttachment.Enabled = False
			btnSaveAttachment.ToolTip = "You are not authorized user "
			'End
		End If

		If ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA")) Then

			If (mnWO.WOJobs.Count = 1) Then

				mnuPrintReportBA.Visible = True
				btnPrint.Visible = False
				btnPrintWithPDF.Visible = False

			Else

				mnuPrintReportBA.Visible = False
				btnPrintWithPDF.Visible = True
				btnPrint.Visible = True

			End If

			btnPrintCommon.Visible = True
			btnPrintAll.Visible = False
			phPrintNC.Visible = False

			If (mnWO.WOJobs.Count = 1) Then btnPrintCommon.Text = "More Prints " & Server.HtmlDecode("&#9650;")

		Else

			mnuPrintReportBA.Visible = False
			btnPrintCommon.Visible = True

			If AppSettings("ClientCode") = "RAL" Then

				btnPrint.Visible = (mnWO.WOJobs.Count >= 1 Or mnWO.WONRCJobs.Count >= 1)
				btnPrintWithPDF.Visible = (mnWO.WOJobs.Count >= 1 Or mnWO.WONRCJobs.Count >= 1)

			Else

				btnPrint.Visible = (mnWO.WOJobs.Count >= 1)
				btnPrintWithPDF.Visible = (mnWO.WOJobs.Count >= 1 And Not AppSettings("ClientCode") = "PAS") Or
										  (mnWO.WOJobs.Count >= 1 And mnWO.WOJobs.IsSelectedTaskExists And AppSettings("ClientCode") = "PAS")

			End If

			'Added By Vikrant On 06-Jun-2016 For TP06062016
			If AppSettings("ClientCode") = "TP" Or AppSettings("ClientCode") = "IND" Then
				btnPrint.Enabled = (mnWO.WOJobs.Count = 1)
				btnPrint.ToolTip = "Click to print Single Job"
			End If
			'End

		End If

		If AppSettings("ClientCode") = "YA" Then
			BtnPrintProductionPlanningForm.Visible = True
		Else
			BtnPrintProductionPlanningForm.Visible = False
		End If

		'12-Jun-2019
		lnkCreateRequisition.Enabled = (mnWO.WOStatusID <> 3) '12-Jun-2019
		lnkViewIndent.Enabled = mRequisitionItemsNew.Count > 0

		If (mnWO.StatusID = 2 And
			mnWO.WOStatusID = 1 And
			mnWO.WOJobTaskSparesCount > 0 And
			AppSettings("ClientCode") = "STR" And
			mRequisitionItemsNew.Count = 0) Then 'Added By Prashant on 31-Aug-2020 STR28082020

			'Added By Prashant On 30-Jun-2023
			If (AppSettings("ShowMaintenanceForNewClients") = "True" And
			   (AppSettings("ShowCAMOOnlyForNewClients") = "True") And
			   (mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 102)) Then '89 Camo WO

				lnkCreateMultipleRequisitionOfTaskSpares.Visible = False

			Else

				lnkCreateMultipleRequisitionOfTaskSpares.Visible = True

			End If

		Else

			lnkCreateMultipleRequisitionOfTaskSpares.Visible = False

		End If

		If ((mnWO.StatusID = 2 And mnWO.WOStatusID = 1) Or
			(mnWO.WOStatusID = 4) And mnWO.WOJobSparesCount > 0) Then 'Added By Prashant on 31-Aug-2020 STR28082020

			If (AppSettings("ClientCode") = "STR" And mRequisitionItemsNew.Count > 0) Then

				lnkCreateRequisition.Visible = False

			Else

				'Added By Prashant On 30-Jun-2023
				If ((mnWO.TransTypeID = 88) Or
					((mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 102) And
					AppSettings("ShowCAMOOnlyForNewClients") = "False")) Then '89 Camo WO

					lnkCreateRequisition.Visible = True

				Else

					lnkCreateRequisition.Visible = False

					If AppSettings("ShowAMOOnlyForNewClients") = "True" Or Session("wfProject_Ajax") = "wfProject_Ajax" Then  'if both keys are true (used when both CAMO and AMO)

						lnkCreateRequisition.Visible = True

					End If

				End If

			End If

		Else

			lnkCreateRequisition.Visible = False 'opens only after WO submit

		End If

		If (((mnWO.StatusID = 2 And mnWO.WOStatusID = 1) Or mnWO.WOStatusID = 4) And
			mnWO.WOTools.Count > 0) Then

			If (AppSettings("ClientCode") = "STR" And mRequisitionItemsNew.Count > 0) Then

				lnkCreateToolsRequisition.Visible = False

			Else

				If ((mnWO.TransTypeID = 88 Or
					 mnWO.TransTypeID = 89 Or
					 mnWO.TransTypeID = 102) And
					 AppSettings("ShowCAMOOnlyForNewClients") = "False") Then '89 Camo WO

					lnkCreateToolsRequisition.Visible = True

				Else

					lnkCreateToolsRequisition.Visible = False

					If AppSettings("ShowAMOOnlyForNewClients") = "True" Or Session("wfProject_Ajax") = "wfProject_Ajax" Then  'if both keys are true (used when both CAMO and AMO)

						lnkCreateToolsRequisition.Visible = True

					End If

				End If

			End If

		Else

			lnkCreateToolsRequisition.Visible = False

		End If

		If mRequisitionItemsNew.Count > 0 Then

			lnkViewIndent.Text = "Requisition Item (" + mRequisitionItemsNew.Count.ToString + ")"

		End If
		'End

		If lnkCreateRequisition.Enabled Then

			lnkCreateRequisition.ToolTip = "Click to create Requisition of Job Spares Items(s)"

		Else

			lnkCreateRequisition.ToolTip = "Requisition already created against this WO."

		End If

		If mnWO.IsCustApprovedObtained Then

			lblCustBy.Visible = True
			cmbCustApprovedByEmailWO.Visible = True

		Else

			lblCustBy.Visible = False
			cmbCustApprovedByEmailWO.Visible = False

		End If

		phPrintNRC.Visible = ((mnWO.WONRCJobs.Count > 0) Or (mnWO.IsNRCJobExists = True))
		'Added By Vikrant On 05-Sep-2018 For ALL05092018
		rdbIsInHouse.Enabled = IIf(mnWO.WOStatusID = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)
		rdbIsThirdParty.Enabled = IIf(mnWO.WOStatusID = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)
		'End

		If AppSettings("ClientCode") = "STR" Then

			txtCreatedBy.Enabled = False

		ElseIf AppSettings("ClientCode") = "IND" Then

			fldDocument.Visible = False
			txtCreatedBy.Enabled = False
			lnkWOParameters.Enabled = IIf(btnSave.Enabled = True, True, False)
		ElseIf AppSettings("WOParametersRequired") = "True" Then

			lnkWOParameters.Enabled = IIf(btnSave.Enabled = True, True, False)

		End If

		'SendMailTool 
		If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID Then

			btnSendMailTool.Visible = IIf(mnWO.StatusID = 2, False, True) And mnWO.WOTools.Count > 0

		ElseIf Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Or Session("MiddleFrame") = "wfnWOExecutionList.aspx" Or Session("MiddleFrame") = "wfnWOCompletionList.aspx?" Then

			btnSendMailTool.Visible = True And mnWO.WOTools.Count > 0

		ElseIf Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" Or Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1" Or Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0" Then

			btnSendMailTool.Visible = False

		Else

			btnSendMailTool.Visible = IIf(mnWO.WOStatusID = 3, False, True) And IIf(mnWO.StatusID = 4, False, True) And mnWO.WOTools.Count > 0

		End If

		If AppSettings("ClientCode") = "IND" Then

			btnAddJob.Enabled = IIf(mnWO.StatusID = 2 Or
											 mnWO.StatusID = 4 Or
											 mnWO.WOJobs.Count >= 1,
									False,
									True) 'Lock ADD JOb button when WO Submission

			cmbJobType.Enabled = IIf(mnWO.StatusID = 2 Or
											  mnWO.StatusID = 4 Or
											  mnWO.WOJobs.Count >= 1,
									 False,
									 True) 'Lock ADD JOb button when WO Submission

			'ElseIf (AppSettings("ClientCode") = "STR" Or
			'		AppSettings("ClientCode") = "Deccan" Or
			'		AppSettings("ClientCode") = "IPA" Or
			'		AppSettings("ClientCode") = "FBW" Or
			'		AppSettings("ClientCode") = "IRM" Or
			'		AppSettings("ClientCode") = "SPZ" Or
			'		AppSettings("ClientCode") = "SAP" Or ' SPZ Code added by Saylee on 13-Jun-2022  'Deccan Code added by Vikrant On 16-Feb-2021
			'		AppSettings("ClientCode") = "AFC" Or ' Added By sachin on 06-Jun-2024 for AFCOM
			'		AppSettings("ClientCode") = "PTW" Or
			'		AppSettings("ClientCode") = "RAJ" Or
			'		AppSettings("ClientCode") = "ASH" Or
			'		AppSettings("ClientCode") = "SIT") And
			'		Session("wfProject_Ajax") <> "wfProject_Ajax" Then  'FIT clientCode removed as multiple jobs needed 'Or  AppSettings("ClientCode") = "FIT"
			'Sankalp 20-11-25
		ElseIf AppSettings("SelectOnlySingleJob") = True And
				Session("wfProject_Ajax") <> "wfProject_Ajax" Then

			btnAddJob.Enabled = IIf(mnWO.WOJobs.Count >= 1, False, True) 'Lock ADD JOb button if One Job is added
			cmbJobType.Enabled = IIf(mnWO.WOJobs.Count >= 1, False, True) 'Lock ADD JOb button if One Job is added
		ElseIf Session("wfProject_Ajax") = "wfProject_Ajax" And mnWO.ModelName <> "" Then
			'do nothing
			btnAddJob.Enabled = IIf(mnWO.WOStatusID = 3 Or mnWO.IsQCStatusApproved = 1 Or mnWO.IsCAMOUpdated = 1 Or mnWO.WOStatusID = 9, False, True) And IIf(mnWO.StatusID = 4, False, True) And (Not (Session("wfMELSnagCorrectiveActionNew_AJAX") = "wfMELSnagCorrectiveActionNew_AJAX")) And (Not (Session("wfLogDefectActionList_Ajax") = "wfLogDefectActionList_Ajax"))
		Else

			btnAddJob.Enabled = IIf(mnWO.WOStatusID = 3 Or mnWO.IsQCStatusApproved = 1 Or mnWO.IsCAMOUpdated = 1 Or mnWO.WOStatusID = 9, False, True) And IIf(mnWO.StatusID = 4, False, True) And (Not (Session("wfMELSnagCorrectiveActionNew_AJAX") = "wfMELSnagCorrectiveActionNew_AJAX")) And (Not (Session("wfLogDefectActionList_Ajax") = "wfLogDefectActionList_Ajax"))
			cmbJobType.Enabled = IIf(mnWO.WOStatusID = 3 Or mnWO.IsQCStatusApproved = 1 Or mnWO.IsCAMOUpdated = 1 Or mnWO.WOStatusID = 9, False, True) And IIf(mnWO.StatusID = 4, False, True)

		End If
		lblRemark.Text = IIf(AppSettings("ClientCode") = "PTW", "Title", "Remark")
		txtRemark.ToolTip = IIf(AppSettings("ClientCode") = "PTW", "Enter W.O. Title", "Enter Remark")


		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		If mnWO.TransTypeID = Trans.SpareAssemblyWO Then

			cmbAssembly.Visible = True
			cmbAssembly.Enabled = (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0) And chkMaintenance.Checked
			cmbCompList.Visible = False
			lblAircraft.Text = "Assembly"
			cmbAircraftList.Visible = False
			lblAircraftDetailsInfo.Text = "Assembly Details"
			lblCurrentValue.Text = "Assembly Current Value"
			lblModel.Text = "Model"

		ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then

			cmbAssembly.Visible = False
			cmbCompList.Visible = True
			cmbCompList.Enabled = (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0) And chkMaintenance.Checked
			lblAircraft.Text = "Component"
			cmbAircraftList.Visible = False
			lblAircraftDetailsInfo.Text = "Component Details"
			lblCurrentValue.Text = "Component Current Value"
			lblModel.Text = "Part"

		Else

			lblAircraft.Text = "Aircraft"
			cmbAssembly.Visible = False
			cmbCompList.Visible = False
			cmbAircraftList.Visible = True
			lblAircraftDetailsInfo.Text = "Aircraft Details"
			lblCurrentValue.Text = "Airframe Current Value"
			lblModel.Text = "Model"

		End If
		'End

		If chkSupplementalSheetAttached.Checked Then
			txtNoOfSupplementalSheets.ReadOnly = False
			txtNoOfSupplementalSheets.BackColor = Color.White
		Else
			txtNoOfSupplementalSheets.ReadOnly = True
			txtNoOfSupplementalSheets.Text = "0"
			txtNoOfSupplementalSheets.BackColor = Color.LightGray
		End If

		If chkNRCRaised.Checked Then
			txtNoOfNRCs.ReadOnly = False
			txtNoOfNRCs.BackColor = Color.White
		Else
			txtNoOfNRCs.ReadOnly = True
			txtNoOfNRCs.Text = "0"
			txtNoOfNRCs.BackColor = Color.LightGray
		End If

		'Added by Saylee on 6-Jun-2023, for MPD
		If AppSettings("ShowCAMOOnlyForNewClients") = "True" Or
		   AppSettings("ShowAMOOnlyForNewClients") = "True" Or
		   Session("wfProject_Ajax") = "wfProject_Ajax" Then

			dgWOJobs.Columns(25).Visible = False 'Print With Task(s) 27 =25
			dgWOJobs.HeaderRow.Cells(4).Text = "Task Type"

		Else
			dgWOJobs.Columns(2).Visible = False 'Task No.
		End If

		'Added By Prashant On 30-Jun-2023
		If (AppSettings("ShowMaintenanceForNewClients") = "True" And
			AppSettings("ShowCAMOOnlyForNewClients") = "True" And
			(mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 102 Or Session("wfProject_Ajax") = "wfProject_Ajax")) Then '89 Camo WO

			phlinks.Visible = False
			PlaceHolder9.Visible = False   'W.O. Tools Buttons
			PlaceHolder10.Visible = False 'dgWOTools W.O. Tools
			If AppSettings("ShowMaintenanceForNewClientsWithTaskCard").ToUpper = "True".ToUpper Then 'Added By Prashant on 24-Sep-2024
				dgWOJobs.Columns(13).Visible = True 'Task Card link
			Else
				dgWOJobs.Columns(13).Visible = False 'Task Card link
			End If
			dgWOJobs.Columns(14).Visible = False 'Designation Allocation link
			dgWOJobs.Columns(15).Visible = False 'Required Spare link

			plhCustomer.Visible = False 'Customer
			plhCustApproval.Visible = False 'CustomerApproval

			If AppSettings("ShowAMOOnlyForNewClients") = "True" Or Session("wfProject_Ajax") = "wfProject_Ajax" Then 'if both keys are true (used when both CAMO and AMO)

				phlinks.Visible = True
				PlaceHolder9.Visible = True   'W.O. Tools Buttons
				PlaceHolder10.Visible = True 'dgWOTools W.O. Tools
				dgWOJobs.Columns(15).Visible = True 'Required Spare link

				If mnWO.MachineID.Equals(Guid.Empty) Then
					plhCustomer.Visible = True 'Customer
					plhCustApproval.Visible = True 'CustomerApproval
				End If


			End If

			dgWOJobs.Columns(16).Visible = False 'Inst./Rem. link
			dgWOJobs.Columns(25).Visible = False 'Print With Task(s) 27=25

			'NRC Grid
			dgWONRC.Columns(10).Visible = False 'Task Card link
			dgWONRC.Columns(11).Visible = False 'Designation Allocation link
			dgWONRC.Columns(13).Visible = False 'Inst./Rem. link
			dgWOJobs.HeaderRow.Cells(4).Text = "Task Type"

		ElseIf (AppSettings("ShowMaintenanceForNewClients") = "True" And
				AppSettings("ShowAMOOnlyForNewClients") = "True" And
				(mnWO.TransTypeID = 88 Or Session("wfProject_Ajax") = "wfProject_Ajax")) Then '88 Third Party WO

			phlinks.Visible = True
			dgWOJobs.Columns(11).Visible = False  '11 Job Type Column
			PlaceHolder9.Visible = True   'W.O. Tools Buttons
			PlaceHolder10.Visible = True  'dgWOTools W.O. Tools
			If AppSettings("ShowMaintenanceForNewClientsWithTaskCard").ToUpper = "True".ToUpper Then 'Added By Prashant on 24-Sep-2024
				dgWOJobs.Columns(13).Visible = True 'Task Card link
			Else
				dgWOJobs.Columns(13).Visible = False 'Task Card link
			End If
			dgWOJobs.Columns(14).Visible = False 'Designation Allocation link
			dgWOJobs.Columns(15).Visible = True 'Required Spare link
			dgWOJobs.Columns(16).Visible = False 'Inst./Rem. link
			dgWOJobs.HeaderRow.Cells(4).Text = "Task Type"
			dgWOJobs.Columns(25).Visible = False '27=25

			'NRC Grid
			dgWONRC.Columns(10).Visible = False 'Task Card link
			dgWONRC.Columns(11).Visible = False 'Designation Allocation link
			dgWONRC.Columns(13).Visible = False 'Inst./Rem. link
			txtCreatedBy.Enabled = False

			plhCustomer.Visible = True 'Customer
			plhCustApproval.Visible = True 'CustomerApproval
		End If
		If Session("wfProject_Ajax") <> "wfProject_Ajax" Then
			If AppSettings("ClientCode") = "Deccan" Or
			(AppSettings("ShowMaintenanceForNewClients") = "True" And AppSettings("ShowAMOOnlyForNewClients") = "True" And mnWO.TransTypeID = 88) Then '88 Third Party WO

				plhCustomer.Visible = True

				If AppSettings("ShowAMOOnlyForNewClients") = "True" Then plhCustApproval.Visible = False

			Else

				plhCustomer.Visible = False

			End If
		End If
		'End of Added By Prashant On 30-Jun-2023

		pnlMaintComplainceDetails.Enabled = IIf(mnWO.WOStatusID = 3 Or
														 mnWO.IsQCStatusApproved = 1 Or
														 mnWO.IsCAMOUpdated = 1 Or
														 mnWO.WOStatusID = 9,
												False,
												True) And
											IIf(mnWO.StatusID = 4,
												False,
												True)

		'Added by Harsh Sugandhi on 20th June 2024 for FLYPAL-1703 Engineering Work Order
		If mnWO.TransTypeID = 102 Then

			dgWOJobs.HeaderRow.Cells(2).Text = "Directive No."

		Else

			dgWOJobs.HeaderRow.Cells(2).Text = "Task No."

		End If

		'Modified by Harsh Sugandhi on 14th Jan 2025 for FLYPAL-2077 =>
		'Shifted here to keep it disabled while coming from Project Add Button, as on Project user is selecting it.
		cmbServiceProvider.Enabled = IIf(mnWO.WOStatusID = 3, False, True) AndAlso
									 IIf(mnWO.StatusID = 4, False, True) AndAlso
									 (Not Session("wfProject_Ajax") = "wfProject_Ajax")

		upnlMaintComplainceDetails.Update()
		'Sankalp  20-11-25
		createdByAndUpdatedBy.Visible = IIf(mnWO.LastUpdatedBy <> "" Or mnWO.CreatedBy <> "", True, False)
		'Sankalp 27-11-25
		txtWODate.Enabled = Not CBool(AppSettings("RestrictDateSelection"))
	End Sub

	Private Sub SetControlStatus(WOStatusId As Integer)

		btnSelectPeriod.Enabled = (IIf(WOStatusId = 3, False, True) Or IIf(mnWO.StatusID = 4, False, True))
		txtText.Enabled = IIf(mnWO.IsNew = False, False, True)
		txtNo.Enabled = IIf(mnWO.IsNew = False, False, True)

		txtWODate.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)

		txtPlanDate.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)
		txtPlanDateTime.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)
		'Added by Shital on 09-Oct-2019
		txtPlanningRemark.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)

		'cmbCustomerList.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)
		If Session("wfProject_Ajax") = "wfProject_Ajax" And mnWO.ModelName <> "" Then 'Added By Prashant on 14-May-2024
			cmbCustomerList.Enabled = False
		Else
			cmbCustomerList.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)
		End If

		txtStartDate.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4 Or mnWO.StatusID = 1, False, True)
		txtStartDateTime.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4 Or mnWO.StatusID = 1, False, True)

		txtCloseDate.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)
		txtClosedDateTime.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)

		txtActualTime.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)
		'cmbJobType.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)

		If AppSettings("ClientCode") = "RAL" Then
			cmbAircraftList.Enabled = IIf(WOStatusId = 3, False, True) Or (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)
			txtRegNo.Enabled = IIf(WOStatusId = 3, False, True) Or (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)
			cmbHourTypeList.Enabled = IIf(WOStatusId = 3, False, True) Or (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)
			txtModelNo.Enabled = IIf(WOStatusId = 3, False, True) Or (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)
			txtSerialNo.Enabled = IIf(WOStatusId = 3, False, True) Or (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0)
		Else
			cmbAircraftList.Enabled = IIf(WOStatusId = 3, False, True) Or (mnWO.WOJobs.Count = 0)
			cmbHourTypeList.Enabled = IIf(WOStatusId = 3, False, True) Or (mnWO.WOJobs.Count = 0)
			If Session("wfProject_Ajax") = "wfProject_Ajax" And mnWO.ModelName <> "" Then 'Added By Prashant on 14-May-2024
				txtModelNo.Enabled = False
				txtSerialNo.Enabled = False
				cmbCustomerList.Enabled = False
				txtRegNo.Enabled = False
			Else
				txtRegNo.Enabled = IIf(WOStatusId = 3, False, True) Or (mnWO.WOJobs.Count = 0)
				txtModelNo.Enabled = IIf(WOStatusId = 3, False, True) Or (mnWO.WOJobs.Count = 0)
				txtSerialNo.Enabled = IIf(WOStatusId = 3, False, True) Or (mnWO.WOJobs.Count = 0)
			End If
		End If

		If AppSettings("ClientCode") = "STR" Then
			txtCreatedBy.Enabled = False
		Else
			txtCreatedBy.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)
		End If
		btnSelectFiles.Enabled = IIf(mnWO.StatusID = 4, False, True)
		'End
		btnSave.Visible = IIf(mnWO.WOStatusID = 3 Or mnWO.IsQCStatusApproved = 1 Or mnWO.IsCAMOUpdated = 1 Or mnWO.WOStatusID = 9, False, True) And IIf(mnWO.StatusID = 4, False, True)
		dgCurrentPeriodValue.Columns(3).Visible = (IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4, False, True))
		cmbWorkShopList.Enabled = IIf(WOStatusId = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)

		If (WOStatusId = 3) Or (mnWO.StatusID = 4) Then
			SetGridEnability(True)
		Else
			SetGridEnability(False)
		End If

	End Sub

	Private Sub AttachMyFile()

		Dim BackupPath As String = ""
		BackupPath = AppSettings("DOCPath") & "New.PDF"
		mnWO = Session("mnWO")
		Try
			If Not mnWO.FileAttachments.Contains(mnWO.ID, CType(Session("FileUpload.FileName"), String)) Then

				mnWO.FileAttachments.Add(mnWO.ID, CType(Session("FileUpload.FileName"), String))
				' mnWO.FileAttachments.CurrentItem.FileName = mFileAttach.FileName
				mnWO.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
				mnWO.FileAttachments.CurrentItem.Size = Session("Size")
				mnWO.FileAttachments.CurrentItem.Extension = Session("Extension")
				'   mnWO.FileAttachments.CurrentItem.SrNo = (mnWO.FileAttachments.Count - 1) + 1

				Session("mnWO") = mnWO
				dgWOAttachment.DataSource = mnWO.FileAttachments
				dgWOAttachment.DataBind()

				For i As Integer = 0 To mnWO.FileAttachments.Count - 1
					Dim txtValue As TextBox
					txtValue = CType(Me.dgWOAttachment.Rows(i).FindControl("txtFileName"), TextBox)
					txtValue.Text = mnWO.FileAttachments(i).FileName
				Next
				mnWO.IsAttachmentAdded = IIf(mnWO.FileAttachments.Count > 0, True, False) 'Added By Sachin 30-Jul-2024
				Session.Remove("Size")
				Session.Remove("ImageFile")
				Session.Remove("Extension")
				Session.Remove("FileUpload.FileName")
				upnlWOAttachment.Update()
				upnldgWOAttachment.Update()
			Else
				Session("mnWO") = mnWO
				MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate, MSGBox.Message_Text.Duplicate, "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
		Catch ex As Exception
		End Try
	End Sub

	Private Sub SetGrid()
		Dim deletebtn As ImageButton
		Dim Editbtn As ImageButton
		'Dim ArrowBtn As Image
		Dim P As Boolean = False
		For j As Integer = 0 To dgWOJobs.Rows.Count - 1

			Dim lnkbtnTaskCard As LinkButton = CType(dgWOJobs.Rows.Item(j).Cells(13).FindControl("lnkbtnTaskCard"), LinkButton)
			lnkbtnTaskCard.Text = dgWOJobs.Rows.Item(j).Cells(20).Text '22 =20

			Dim lnkbtnDesignationAllocation As LinkButton = CType(dgWOJobs.Rows.Item(j).Cells(14).FindControl("lnkbtnDesignationAllocation"), LinkButton)
			lnkbtnDesignationAllocation.Text = dgWOJobs.Rows.Item(j).Cells(21).Text '23 = 21

			Dim lnkbtnSparesAddRemove As LinkButton = CType(dgWOJobs.Rows.Item(j).Cells(15).FindControl("lnkbtnSparesAddRemove"), LinkButton)
			lnkbtnSparesAddRemove.Text = dgWOJobs.Rows.Item(j).Cells(23).Text '25 = 23

			Dim lnkbtnInstallationRemovalRec As LinkButton = CType(dgWOJobs.Rows.Item(j).Cells(16).FindControl("lnkbtnInstallationRemovalRec"), LinkButton)
			lnkbtnInstallationRemovalRec.Text = dgWOJobs.Rows.Item(j).Cells(22).Text '24=22

			Dim lnkbtnAddNRC As LinkButton = CType(dgWOJobs.Rows.Item(j).Cells(17).FindControl("lnkbtnAddNRC"), LinkButton)
			lnkbtnAddNRC.Text = dgWOJobs.Rows.Item(j).Cells(24).Text '26 = 24 Ajay

			If lnkbtnTaskCard.Text = "(0)" Then
				dgWOJobs.Rows.Item(j).Cells(25).Enabled = False '27 = 25
			Else
				dgWOJobs.Rows.Item(j).Cells(25).Enabled = True '27 = 25
			End If

			deletebtn = CType(Me.dgWOJobs.Rows.Item(j).Cells(18).FindControl("DeleteRecord"), ImageButton)
			Editbtn = CType(Me.dgWOJobs.Rows.Item(j).Cells(18).FindControl("EditView"), ImageButton)



			If mnWO.StatusID >= 4 Or (mnWO.WOStatusID = 3 Or mnWO.IsQCStatusApproved = 1 Or mnWO.IsCAMOUpdated = 1 Or mnWO.WOStatusID = 9) _
				Or (Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID And mnWO.StatusID = 2) _
				Or (mnWO.WOJobs(j).WOJobStatusID = 2 And mnWO.StatusID <> 3 And AppSettings("ShowNewWOFlow") <> "True" And Not IsInRole(Rights.Completed)) Then 'Completion rights chk condition added by vikrant on 15-Jul-2021 for ALL30062021

				lnkbtnTaskCard.Enabled = False
				lnkbtnDesignationAllocation.Enabled = False
				lnkbtnInstallationRemovalRec.Enabled = False
				lnkbtnSparesAddRemove.Enabled = False
				'lnkbtnAddNRC.Enabled = False
				Editbtn.Visible = False 'Edit link   Ajay h
				deletebtn.Visible = False 'Remove link Ajay h

			Else
				Editbtn.Visible = True  'Edit link
				If mnWO.StatusID >= 2 Then
					deletebtn.Visible = False  'Remove link
				Else
					deletebtn.Visible = True  'Remove link
				End If

			End If

			If (Me.dgWOJobs.Rows.Item(j).Cells(12).Text = "Cancel" Or Me.dgWOJobs.Rows.Item(j).Cells(12).Text = "Deferred") Then
				lnkbtnTaskCard.Enabled = False
				lnkbtnDesignationAllocation.Enabled = False
				lnkbtnInstallationRemovalRec.Enabled = False
				lnkbtnSparesAddRemove.Enabled = False
				'lnkbtnAddNRC.Enabled = False

			End If


		Next
		If AppSettings("ClientCode") = "IND" Then
			dgWOJobs.HeaderRow.Cells(17).Text = "OJS"
		Else
			dgWOJobs.HeaderRow.Cells(17).Text = "NRC"
		End If


		'******************************************
		For j As Integer = 0 To dgWOStages.Rows.Count - 1
			If dgWOStages.Rows.Item(j).Cells(3).Text.Contains("Rejected") Then
				Me.dgWOStages.Rows.Item(j).Cells(3).ForeColor = Color.Red
				Me.dgWOStages.Rows.Item(j).Cells(3).Font.Bold = True
			Else
				Me.dgWOStages.Rows.Item(j).Cells(3).ForeColor = Color.Green
				Me.dgWOStages.Rows.Item(j).Cells(3).Font.Bold = True
			End If
		Next

		'******************************************
	End Sub

	'Added By Vikrant For WO NRC
	Private Sub SetNRCGrid()
		Dim P As Boolean = False
		For j As Integer = 0 To dgWONRC.Rows.Count - 1
			P = CType(Me.dgWONRC.Rows.Item(j).Cells(15).Text, Boolean) '17=15
			'============================ Ajay 20-Sep-2022 Start ===============================================
			Dim CountTaskCards As LinkButton = CType(dgWONRC.Rows.Item(j).Cells(10).FindControl("CountTaskCards"), LinkButton)
			CountTaskCards.Text = dgWONRC.Rows.Item(j).Cells(16).Text '18=16

			Dim CountDesignationAllocation As LinkButton = CType(dgWONRC.Rows.Item(j).Cells(11).FindControl("CountDesignationAllocation"), LinkButton)
			CountDesignationAllocation.Text = dgWONRC.Rows.Item(j).Cells(17).Text '19=17

			Dim CountRequiredSpares As LinkButton = CType(dgWONRC.Rows.Item(j).Cells(12).FindControl("CountRequiredSpares"), LinkButton)
			CountRequiredSpares.Text = dgWONRC.Rows.Item(j).Cells(19).Text '21=19

			Dim CountInstRem As LinkButton = CType(dgWONRC.Rows.Item(j).Cells(13).FindControl("CountInstRem"), LinkButton)
			CountInstRem.Text = dgWONRC.Rows.Item(j).Cells(18).Text '20 = 18

			'============================= Ajay 20-Sep-2022 End ========================================
		Next
	End Sub

	Private Sub AddWONRC()

		mnWO.WONRCJobs.Add(mnWO.ID, 5)
		Session("mnWO") = mnWO
		Session("ActiveNRCDetailsTabIndex") = "0"
		Response.Redirect("wfnWONRC.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))

	End Sub
	'End

	Public Sub UpdatePanels()

		Try

			upnlAirframePeriods.Update()
			upnlGrids.Update()
			upnlMachineDet.Update()
			upnlJobType.Update()
			UpnlWODet.Update()
			upnlStatusHeader.Update()
			upnlTitle.Update()
			UpnlPrint.Update()
			upnlReq.Update()
			upnlBilling.Update()
			upnlLinks.Update()
			upnlClosing.Update()
			UpnlApproval.Update()
			upnlCloseAll.Update()
			upnlDocumentStatusDetail.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub NotifyMail()
		Dim str As String
		Dim mSendMailFile As New SendMailFile
		Dim ToMailIDs As String = ""
		Dim Resources As String = ""
		' we'll need a split to get the individual ids
		'Dim values = checkString.Split(","c)

		mnWO = nWO.GetWO(mnWO.ID)
		If mnWO.WOResourceCount > 0 Then
			ToMailIDs = mnWO.WOResourceMailDs
			Resources = mnWO.WOResources
		End If


		str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Following Resource(s) has been assigned Work Order " + mnWO.WONumber + " created On " + mnWO.WODateFormatted + " In FlyPal System." + "</font></P></br> ")

		str = str + ("<p><font face=""Calibri"">")
		str = str + ("<b>Resource Name " + "</b>" + Resources + "</p><p><b>Job Description: " + "</b>" + mnWO.WOJobs(0).WOJobDescription)
		str = str + ("</font></p>")

		str = str + ("<p><font face=""Calibri"">")
		str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")
		str = str + ("</body></html>")
		SendMailFile.SendMailFile(, User.Identity.Name, "Job Assigned Notification", Info:=str, ToMailID:=ToMailIDs.ToString, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
			 SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
		Dim mResourcesDetail As String = "Job Assigned Notification sent successfully to " + Resources.ToString.TrimEnd(",") + " by " + User.Identity.Name
		MarkLog(Action.SendMail, "Work Order", mResourcesDetail, ErrorType.HandledError, mnWO.ID, EventLogID)
		' ScriptManager.RegisterStartupScript(Me, [GetType], "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)
		MSGBoxCtrl.Show("Mail!", "Mail Sent Successfully", "", MsgBoxStyle.OkOnly, "")
	End Sub

	Public Sub SendMail()  'Added By Prashant 1-Nov-2018 StarAir1112018
		If AppSettings("WorkOrderSubmitMail") = "True" Then
			Print(ByMail:=True)
			Dim str As String
			str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Work Order No.: <b> " & mnWO.WOText + "-" + mnWO.WONo.ToString & "</b> Dated: <b> " + mnWO.WODateFormatted + " Is attached for your information And planning. </b></font></P> ")
			Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
			If mUser.Name.ToUpper = "BTPLADMIN" Or mUser.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019 
				'Do nothing 
			Else
				SendMailFile.SendMailFile(Session("CrystalReport"), User.Identity.Name, "Work Order Details", mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString, Info:=str, ToMailID:=IIf(AppSettings("SendToMailID").Trim <> "", AppSettings("SendToMailID").Trim + "," + mUser.UserEmail.Trim, mUser.UserEmail.Trim), Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
										   SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
			End If
		End If
	End Sub

	Private Sub AddMultipleTaskCards()  'Added by Saylee on 29-May-2019
		Dim tmpTaskCard As TaskCard
		Dim mTaskCardList As TaskCardList = Session("mSelectTaskCardList")
		Dim mnWOJob As nWOJob
		mnWOJob = Session("mnWOJob")
		For Each tmpTaskCard In mTaskCardList
			If tmpTaskCard.IsSelect Then
				If Not mnWOJob.WOJobTasks.Contains(tmpTaskCard.ID, "") Then
					Dim mTaskCard As TaskCard
					mTaskCard = TaskCard.GetTaskCard(tmpTaskCard.ID)
					mnWOJob.WOJobTasks.Add(mnWOJob.ID, mTaskCard.ID.ToString)
					With mnWOJob.WOJobTasks.CurrentItem
						mnWOJob.WOJobTasks.CurrentItem.SrNo = mnWOJob.WOJobTasks.CurrentIndex + 1
						mnWOJob.WOJobTasks.CurrentItem.TaskCardNo = mTaskCard.TaskCardNo
						mnWOJob.WOJobTasks.CurrentItem.EstimatedHours = mTaskCard.EstimatedHours
						mnWOJob.WOJobTasks.CurrentItem.Reference = mTaskCard.Reference
						mnWOJob.WOJobTasks.CurrentItem.Equipment = mTaskCard.Equipment
						mnWOJob.WOJobTasks.CurrentItem.Material = mTaskCard.Material
						mnWOJob.WOJobTasks.CurrentItem.TaskDescription = mTaskCard.TaskDesc
						mnWOJob.WOJobTasks.CurrentItem.RevNo = mTaskCard.RevNo
						mnWOJob.WOJobTasks.CurrentItem.RevDate = mTaskCard.RevDate
						mnWOJob.WOJobTasks.CurrentItem.IssueDate = mTaskCard.IssueDate
						mnWOJob.WOJobTasks.CurrentItem.checks = mTaskCard.Check
						mnWOJob.WOJobTasks.CurrentItem.RelatedTaskCardsNo = mTaskCard.RelatedTaskCardsNo

						Dim mTaskCardSpare As TaskCardSpare
						Dim mTaskCardStepsSpare As TaskCardSpare

						For Each mTaskCardSpare In mTaskCard.TaskCardSpares
							mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
							With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.CurrentItem
								.ItemID = mTaskCardSpare.ItemID
								.RequiredQty = mTaskCardSpare.RequiredQty
								.PartNo = mTaskCardSpare.PartNo
								.Description = mTaskCardSpare.Description
								.Remark = mTaskCardSpare.Remark
								.OnSerialNo = mTaskCardSpare.OnSerialNo
								.OffSerialNo = mTaskCardSpare.OffSerialNo
								.IsForSteps = False
							End With

						Next

						For Each mTaskCardStepsSpare In mTaskCard.TaskCardStepsSpares
							mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
							With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.CurrentItem
								.ItemID = mTaskCardStepsSpare.ItemID
								.RequiredQty = mTaskCardStepsSpare.RequiredQty
								.PartNo = mTaskCardStepsSpare.PartNo
								.Description = mTaskCardStepsSpare.Description
								.Remark = mTaskCardStepsSpare.Remark
								.OnSerialNo = mTaskCardStepsSpare.OnSerialNo
								.OffSerialNo = mTaskCardStepsSpare.OffSerialNo
								.IsForSteps = True
							End With
						Next

						'Added By Vikrant on 03-Mar-2020 For ALL03032020
						For Each mTaskCardSpare In mTaskCard.TaskCardPartRemovals
							mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
							With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.CurrentItem
								.ItemID = mTaskCardSpare.ItemID
								.RequiredQty = mTaskCardSpare.RequiredQty
								.PartNo = mTaskCardSpare.PartNo
								.Description = mTaskCardSpare.Description
								.Remark = mTaskCardSpare.Remark
								.OnSerialNo = mTaskCardSpare.OnSerialNo
								.OffSerialNo = mTaskCardSpare.OffSerialNo
								.IsForSteps = False
								.IsPartRemoval = True
								.Position = mTaskCardSpare.Position
							End With

						Next
						'End
					End With
					'Else
				End If
			Else
				If mnWO.WOJobs.CurrentItem.WOJobTasks.Contains(tmpTaskCard.ID, "") Then
					mnWO.WOJobs.CurrentItem.WOJobTasks.Remove(tmpTaskCard.ID, "")
				End If
			End If
		Next
		Session("TaskCards") = "False"
		Session.Remove("mTaskCard")
		Session.Remove("mTaskCardList")
	End Sub

	'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
	Public Sub SetUserMailIDs()
		'Added on 11-Feb-2020 By Shital For Star Air
		If AppSettings("CLientCode") = "STR" Then
			Dim mEmployee As Employee
			If Not mnWO.EmployeeID.Equals(Guid.Empty) Then
				mEmployee = Employee.GetEmployee(mnWO.EmployeeID)
			End If
			Session("UserEmailID") = mnWO.WOJobs.CurrentItem.WOJobDesignationAllocations.WOResourceMailDs
		Else
			Session("UserEmailID") = mTransactionList.Item(mnWO.TransTypeID).SendToMailID
		End If
		' Session("UserEmailID") = mTransactionList.Item(mnWO.TransTypeID).SendToMailID
		Session("UserCcEmailID") = mTransactionList.Item(mnWO.TransTypeID).SendCCMailID
		Session("MailsRequire") = mTransactionList.Item(mnWO.TransTypeID).MailsRequire
		Session("SmtpHost") = mTransactionList.Item(mnWO.TransTypeID).SmtpHost
		Session("SmtpPort") = mTransactionList.Item(mnWO.TransTypeID).SmtpPort
		Session("SmtpUser") = mTransactionList.Item(mnWO.TransTypeID).SmtpUser
		Session("SmtpPassword") = mTransactionList.Item(mnWO.TransTypeID).SmtpPassword
		Session("FormRevisionNo") = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
		Session("FormRevisionDate") = mTransactionList.Item(mnWO.TransTypeID).FormRevisionDate
	End Sub

	Private Sub SetRevertedWOStage()
		If mnWOApproveReject IsNot Nothing Then

			If mnWOApproveReject.ApprovedRejectStatus = 2 Then

				If mnWOApproveReject.WOStatusID = 2 Then
					mnWO.WOStatusID = 1
					mnWO.StatusID = 1
				ElseIf mnWOApproveReject.WOStatusID = 3 Then
					mnWO.WOStatusID = 4 'reverted to Planned state, so need to remove all closing details and to remove job closing details
					mnWO.IsClosed = False
					mnWO.ClosedBy = ""
					mnWO.WOCloseDate = DBNull.Value
					mnWO.WOJobs(0).WOJobStatusID = 1
					mnWO.WOJobs(0).WOJobStartDate = DBNull.Value
					mnWO.WOJobs(0).WOJobCloseDate = DBNull.Value
				ElseIf mnWOApproveReject.WOStatusID = 4 Then
					mnWO.WOStatusID = 1
					mnWO.StatusID = 1 'reverted to open state
				ElseIf mnWOApproveReject.WOStatusID = 5 Then
					mnWO.IsQCStatusApproved = 2
					mnWO.WOStatusID = 7 'reverted to AME state
				ElseIf mnWOApproveReject.WOStatusID = 7 Then
					mnWO.WOStatusID = 2 'reverted to submitted state
					mnWO.WOPlanedDate = DBNull.Value
				ElseIf mnWOApproveReject.WOStatusID = 8 Then
					mnWO.WOStatusID = 7 'reverted to AME state
				End If
			End If

		End If
		Session("mnWO") = mnWO
	End Sub

	Public Function IsIssuedSparesReturned() As Integer
		Dim mnIssuedWOSpares As nIssuedWOSpares
		mnIssuedWOSpares = nIssuedWOSpares.GetIssuedWOSpares(mnWO.ID)
		Dim ReturnQty As Integer = 0
		Dim i As Integer = 0

		If mnIssuedWOSpares.Count = 0 Then
			ReturnQty = 0
		End If

		While i < mnIssuedWOSpares.Count
			If mnIssuedWOSpares.Item(i).UsedQty = 0 Then 'mnIssuedWOSpares.Item(i).ReturnQty 
				ReturnQty = 1
			Else
				ReturnQty = 2
				Exit While
			End If
			i = i + 1
		End While
		Return ReturnQty
		''If ReturnQty = 0 Then
		''    Return True
		''Else
		''    Return False
		''End If
	End Function

	'Added by Saylee on 11-Oct-2018 for ALL11102018
	Public Sub SaveAttachment()
		If mnWO.IsDigitalSignatureAdded = True Then
			If mFileAttachnWO IsNot Nothing Then
				If mFileAttachnWO.Size > 0 Then
					Try
						mFileAttachnWO.Save()
					Catch ex As Exception
						ScriptManager.RegisterClientScriptBlock(Me, [GetType], "", MessageBox.Show(ex.InnerException.ToString, False), True)
					End Try
				End If
			End If
		End If
	End Sub

	Public Function CustValidate1() As Boolean
		Dim strMsg As String = ""
		If rdbBillingDone.Checked Then
			If txtBillingDate.Text.ToString = "" Then
				strMsg = "Billing Date required."
			Else
				If IsDate(txtBillingDate.Text) AndAlso IsDate(txtCloseDate.Text) Then
					If CDate(txtBillingDate.Text) < CDate(txtCloseDate.Text) Then
						strMsg = "Billing Date should be Greater than Or Equal to WO Completion Date."
					End If
				End If
			End If
			If txtInvoiceNumber.Text.ToString.Trim = "" Then
				strMsg = strMsg + "Invoice No. Required." + vbCrLf
			End If
			If txtBillingBy.Text.ToString.Trim = "" Then
				strMsg = strMsg + "Billing By Required." + vbCrLf
			End If
		End If


		If strMsg.Trim <> "" Then
			cvCommon.ErrorMessage = strMsg
			cvCommon.IsValid = False
			Return False
		End If
		Return True
	End Function

	Private Sub ShowMessage(Optional FromRequisitionNo As String = "", Optional ToRequisitionNo As String = "",
						  Optional RequisitionDate As String = "", Optional AutoCreatedReqCount As Integer = 0) 'Added By Prashant on 31-Aug-2020 STR28082020
		Dim str1 As String = ""
		If AutoCreatedReqCount = 1 Then
			str1 = str1 + ("<span class=""clsLabelAuto""><b>" + AutoCreatedReqCount.ToString + "</b> Requisition(s) Created Successfully! Dated : " + RequisitionDate + "<BR>" + FromRequisitionNo + "</BR></span>")
		Else
			str1 = str1 + ("<span class=""clsLabelAuto""><b>" + AutoCreatedReqCount.ToString + "</b> Requisition(s) Created Successfully! Dated  " + RequisitionDate + "<BR>" + FromRequisitionNo + " To " + ToRequisitionNo + "</BR></span>")
		End If
		MSGBoxCtrl.Show("Alert!", str1, "", MsgBoxStyle.OkOnly, "RequisitionCreated")
		Exit Sub
	End Sub

	'Added by Saylee on 22-Jun-2023 , for Third Party job transferring
	Private Function GetATAID(Name As String) As Guid
		Try

			Dim obj As ATA, objList As ATAList = ATAList.GetATAList()
			If objList.Contains(CType(Trim(Name), Integer)) = True Then
				obj = ATA.GetATA(objList.Item(CType(Trim(Name), Integer), "").ID)
			Else
				obj = ATA.NewATA(Guid.NewGuid)
				obj.ATACode = CType(Trim(Name), Integer)
				obj.ATANomenclature = "to be Updated"
				obj.Save()
				obj = ATA.GetATA(obj.ID)
			End If
			Return obj.ID

		Catch ex As Exception
			Throw ex
		End Try
	End Function

	Private Function GetSkillID(Name As String) As Integer
		Try

			Dim obj As MPDSkillList.MPDSkillInfo, objList As MPDSkillList = MPDSkillList.GetSkillList()
			If objList.Contains(Name) = True Then
				obj = objList.Item(Name)
			Else
				Return 0
			End If
			Return obj.ID

		Catch ex As Exception
			Throw ex
		End Try
	End Function

	Private Function ThirdPartyJobTransfer() As Boolean
		Dim intCounter As Integer
		Dim mIsSaved As Boolean = False
		Session("Err") = ""
		DS = CType(Session("DS"), DataSet)

		If DS.Tables.Count > 0 Then
			For intCounter = 0 To DS.Tables(0).Rows.Count - 1
				Try
					Dim mATACode As Integer = 0

					Dim TaskNo As String
					Dim WOJobDescription As String

					Dim mErrStr As String

					''0 TaskNo
					If DS.Tables(0).Rows(intCounter).Item(0) IsNot DBNull.Value Then TaskNo = DS.Tables(0).Rows(intCounter).Item(0)

					''1 TaskDescription
					If DS.Tables(0).Rows(intCounter).Item(1) IsNot DBNull.Value Then WOJobDescription = DS.Tables(0).Rows(intCounter).Item(1)


					If TaskNo = "" Or WOJobDescription = "" Then
						If mnWO.WOJobs.CurrentItem.TaskCardNo = "" Then
							mErrStr = mErrStr + "Row No  " + DS.Tables(0).Rows(intCounter).ToString + " Task No. Required" + vbCrLf
						End If
						If mnWO.WOJobs.CurrentItem.WOJobDescription = "" Then
							mErrStr = mErrStr + "Row No  " + DS.Tables(0).Rows(intCounter).ToString + " Task Description Required" + vbCrLf
						End If
						Session("mErrStr") = mErrStr
						FileOpen(1, AppSettings("DOCPath") + "TaskCards Errors " + Format(Now.Date, "dd-MMM-yy"), OpenMode.Append, OpenAccess.Write)
						WriteLine(1, mErrStr + vbCrLf)
						FileClose(1)
						mErrStr = ""
					Else

						If Not mnWO.WOJobs.Contains(TaskNo) Then
							mnWO.WOJobs.Add(mnWO.ID, 1)

							''0 TaskNo
							mnWO.WOJobs.CurrentItem.TaskCardNo = TaskNo

							''1 TaskDescription
							mnWO.WOJobs.CurrentItem.WOJobDescription = WOJobDescription

							''2 ATACode
							If DS.Tables(0).Rows(intCounter).Item(2) IsNot DBNull.Value Then mATACode = CType(Trim(DS.Tables(0).Rows(intCounter).Item(2)), Integer)
							If mATACode <> 0 Then
								mnWO.WOJobs.CurrentItem.ATAChapterID = GetATAID(Trim(DS.Tables(0).Rows(intCounter).Item(2)))
								mnWO.WOJobs.CurrentItem.ATAChapterCode = mATACode
							End If

							''3 Reference doc
							If DS.Tables(0).Rows(intCounter).Item(3) IsNot DBNull.Value Then mnWO.WOJobs.CurrentItem.Publication = Trim(DS.Tables(0).Rows(intCounter).Item(3))

							''4 Estimated Man Hr
							If DS.Tables(0).Rows(intCounter).Item(4) IsNot DBNull.Value Then mnWO.WOJobs.CurrentItem.WOJobEstimatedTime = Trim(DS.Tables(0).Rows(intCounter).Item(4))

							''5 Source doc
							If DS.Tables(0).Rows(intCounter).Item(5) IsNot DBNull.Value Then mnWO.WOJobs.CurrentItem.TaskSourceRef = Trim(DS.Tables(0).Rows(intCounter).Item(5))

							''6 DueAsOF
							If DS.Tables(0).Rows(intCounter).Item(6) IsNot DBNull.Value Then mnWO.WOJobs.CurrentItem.DueAsOf = Trim(DS.Tables(0).Rows(intCounter).Item(6))

							''7 Zone
							If DS.Tables(0).Rows(intCounter).Item(7) IsNot DBNull.Value Then mnWO.WOJobs.CurrentItem.Zone = Trim(DS.Tables(0).Rows(intCounter).Item(7))

							''8 Area
							If DS.Tables(0).Rows(intCounter).Item(8) IsNot DBNull.Value Then mnWO.WOJobs.CurrentItem.AREA = Trim(DS.Tables(0).Rows(intCounter).Item(8))

							''9 Skill
							If DS.Tables(0).Rows(intCounter).Item(9) IsNot DBNull.Value Then mnWO.WOJobs.CurrentItem.Skill = Trim(DS.Tables(0).Rows(intCounter).Item(9))

							If mnWO.WOJobs.CurrentItem.Skill <> "" Then
								mnWO.WOJobs.CurrentItem.SkillID = GetSkillID(Trim(mnWO.WOJobs.CurrentItem.Skill))
							End If

							'10 Access
							If DS.Tables(0).Rows(intCounter).Item(10) IsNot DBNull.Value Then mnWO.WOJobs.CurrentItem.Panels = Trim(DS.Tables(0).Rows(intCounter).Item(10))

							''11 Work Pack Ref
							If DS.Tables(0).Rows(intCounter).Item(11) IsNot DBNull.Value Then mnWO.WOJobs.CurrentItem.WorkPACKREF = Trim(DS.Tables(0).Rows(intCounter).Item(11))

							''12 Remark
							If DS.Tables(0).Rows(intCounter).Item(12) IsNot DBNull.Value Then mnWO.WOJobs.CurrentItem.WOJobRemark = Trim(DS.Tables(0).Rows(intCounter).Item(12))
						End If
					End If
				Catch ex As Exception
					mIsSaved = False
					Return mIsSaved
				End Try
			Next
		End If
		mIsSaved = True
		Return mIsSaved
		Session("mnWO") = mnWO
	End Function

#End Region

#Region " DataFieldBind "

	'Added By Vikrant On 27-Jul-2020 For ALL27072020
	Public Sub HighlightSpareAssembly()
		Dim da As New ObjectAdapter
		Dim ds As New DataSet()

		If mnWO.TransTypeID = Trans.SpareAssemblyWO Then
			da.Fill(ds, mRemovedAssemblyListForCombo)
			Dim dv As DataView = ds.Tables(0).DefaultView
			dv.RowFilter = "IsSpareAssembly='True'"
			For Each dr As DataRowView In dv
				For Each item1 As ListItem In cmbAssembly.Items
					If dr("AssemblyStatusID").ToString() = item1.Value.ToString() Then
						item1.Attributes.Add("style", "background-color:#ffbf00;color:white;font-weight:bold;")
						item1.Attributes.Add("title", "Spare Assembly")
					End If
				Next
			Next
		ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then
			da.Fill(ds, mRemovedCompListForCombo)
			Dim dv As DataView = ds.Tables(0).DefaultView
			dv.RowFilter = "IsSpareComp='True'"
			For Each dr As DataRowView In dv
				For Each item1 As ListItem In cmbCompList.Items
					If dr("CompStatusID").ToString() = item1.Value.ToString() Then
						item1.Attributes.Add("style", "background-color:#ffbf00;color:white;font-weight:bold;")
						item1.Attributes.Add("title", "Spare Component")
					End If
				Next
			Next
		End If

	End Sub
	'End

	Private Sub DataFieldBind()

		mnWO = Session("mnWO")

		If mnWO Is Nothing Or mnWO.IsNew Then
			mMachineList = MachineNameValueList.GetMachineList(Today.Date.ToString, SkipIsForInventoryAircarft:=True, IsTagRequired:=True, TagText:="(SELECT)", SkipReadOnlyAircrafts:=True)
			Session("mMachineList") = mMachineList
			cmbAircraftList.DataSource = mMachineList
		End If

		Dim tmpAssemblyStatusList As AssemblyStatusList
		If mnWO IsNot Nothing And Not mnWO.IsNew Then

			mMachineList = MachineNameValueList.GetMachineList(mnWO.WODateFormatted.ToString, SkipIsForInventoryAircarft:=True, IsTagRequired:=True, TagText:="(SELECT)", ForInventory:=True)
			Session("mMachineList") = mMachineList
			cmbAircraftList.DataSource = mMachineList

			If mnWO.WOStartDate.ToString <> "" And Not mnWO.MachineID.Equals(Guid.Empty) Then

				tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(mnWO.WOStartDate.ToString, mnWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
				AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
				mReportLogRegister = ReportLogRegister.GetRectifiedLog(mnWO.WOStartDate.ToString, mnWO.WOStartDate.ToString, tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyID.ToString, mnWO.MachineID.ToString, False, , 1, , , , "(SELECT)", , , True)

				cmbLogList.DataSource = mReportLogRegister

				If AppSettings("ClientCode") = "IND" Then
					Dim mtempReportLogRegister = (From mReportLogRegisterInfo As ReportLogRegister.ReportLogRegisterInfo In mReportLogRegister
												  Order By (mReportLogRegisterInfo.LogDate) Descending, (mReportLogRegisterInfo.DepartureUTCTime) Descending, (mReportLogRegisterInfo.IntLogNo) Descending
												  Select mReportLogRegisterInfo).ToList

					cmbLogList.DataSource = mtempReportLogRegister
					Session("mtempReportLogRegister") = mtempReportLogRegister
				End If

				Session("mReportLogRegister") = mReportLogRegister
				cmbLogList.DataBind()
				cmbLogList.Enabled = True

			ElseIf mnWO.WODate.ToString <> "" And Not mnWO.MachineID.Equals(Guid.Empty) Then  'Added by Saylee 16-Sep-2019

				tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(mnWO.WODate.ToString, mnWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList

				mReportLogRegister = ReportLogRegister.GetRectifiedLog(mnWO.WODate.ToString, mnWO.WODate.ToString, tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyID.ToString, mnWO.MachineID.ToString, False, , 1, , , , "(SELECT)", , , True)

				cmbLogList.DataSource = mReportLogRegister

				If AppSettings("ClientCode") = "IND" Then
					Dim mtempReportLogRegister = (From mReportLogRegisterInfo As ReportLogRegister.ReportLogRegisterInfo In mReportLogRegister
												  Order By (mReportLogRegisterInfo.LogDate) Descending, (mReportLogRegisterInfo.DepartureUTCTime) Descending, (mReportLogRegisterInfo.IntLogNo) Descending
												  Select mReportLogRegisterInfo).ToList

					cmbLogList.DataSource = mtempReportLogRegister
					Session("mtempReportLogRegister") = mtempReportLogRegister
				End If
				Session("mReportLogRegister") = mReportLogRegister
				cmbLogList.DataBind()

				If txtLogNo.Text = "" AndAlso mReportLogRegister IsNot Nothing AndAlso mReportLogRegister.Count > 0 Then
					txtLogNo.Text = IIf(mReportLogRegister Is Nothing, "", mReportLogRegister(mReportLogRegister.Count - 1).LogNo)
				End If

			End If

		ElseIf txtStartDate.Text.ToString <> "" And cmbAircraftList.SelectedIndex > 0 Then

			tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtStartDate.Text.ToString, cmbAircraftList.SelectedValue.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
			AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
			mReportLogRegister = ReportLogRegister.GetRectifiedLog(txtStartDate.Text, txtStartDate.Text, tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyID.ToString, New Guid(cmbAircraftList.SelectedValue.ToString).ToString, False, , 1, , , , "(SELECT)", , , True)
			cmbLogList.Enabled = True

			cmbLogList.DataSource = mReportLogRegister

			If AppSettings("ClientCode") = "IND" Then
				Dim mtempReportLogRegister = (From mReportLogRegisterInfo As ReportLogRegister.ReportLogRegisterInfo In mReportLogRegister
											  Order By (mReportLogRegisterInfo.LogDate) Descending, (mReportLogRegisterInfo.DepartureUTCTime) Descending, (mReportLogRegisterInfo.IntLogNo) Descending
											  Select mReportLogRegisterInfo).ToList

				cmbLogList.DataSource = mtempReportLogRegister
				Session("mtempReportLogRegister") = mtempReportLogRegister
			End If
			Session("mReportLogRegister") = mReportLogRegister
			cmbLogList.DataBind()
		Else
			If mnWO.WOStartDate.ToString <> "" And Not mnWO.MachineID.Equals(Guid.Empty) Then
				tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(mnWO.WOStartDate.ToString, mnWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
				AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
				mReportLogRegister = ReportLogRegister.GetRectifiedLog(mnWO.WOStartDate.ToString, mnWO.WOStartDate.ToString, tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyID.ToString, mnWO.MachineID.ToString, False, , 1, , , , "(SELECT)", , , True)

				cmbLogList.DataSource = mReportLogRegister

				If AppSettings("ClientCode") = "IND" Then
					Dim mtempReportLogRegister = (From mReportLogRegisterInfo As ReportLogRegister.ReportLogRegisterInfo In mReportLogRegister
												  Order By (mReportLogRegisterInfo.LogDate) Descending, (mReportLogRegisterInfo.DepartureUTCTime) Descending, (mReportLogRegisterInfo.IntLogNo) Descending
												  Select mReportLogRegisterInfo).ToList

					cmbLogList.DataSource = mtempReportLogRegister
					Session("mtempReportLogRegister") = mtempReportLogRegister
				End If
				Session("mReportLogRegister") = mReportLogRegister
				cmbLogList.DataBind()
			ElseIf mnWO.WODate.ToString <> "" And Not mnWO.MachineID.Equals(Guid.Empty) Then  'Added by Saylee 16-Sep-2019
				tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(mnWO.WODate.ToString, mnWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList

				mReportLogRegister = ReportLogRegister.GetRectifiedLog(mnWO.WODate.ToString, mnWO.WODate.ToString, tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyID.ToString, mnWO.MachineID.ToString, False, , 1, , , , "(SELECT)", , , True)

				cmbLogList.DataSource = mReportLogRegister

				If AppSettings("ClientCode") = "IND" Then
					Dim mtempReportLogRegister = (From mReportLogRegisterInfo As ReportLogRegister.ReportLogRegisterInfo In mReportLogRegister
												  Order By (mReportLogRegisterInfo.LogDate) Descending, (mReportLogRegisterInfo.DepartureUTCTime) Descending, (mReportLogRegisterInfo.IntLogNo) Descending
												  Select mReportLogRegisterInfo).ToList

					cmbLogList.DataSource = mtempReportLogRegister
					Session("mtempReportLogRegister") = mtempReportLogRegister
				End If
				Session("mReportLogRegister") = mReportLogRegister

				cmbLogList.DataBind()

				If txtLogNo.Text = "" AndAlso mReportLogRegister IsNot Nothing AndAlso mReportLogRegister.Count > 0 Then
					txtLogNo.Text = IIf(mReportLogRegister Is Nothing, "", mReportLogRegister(mReportLogRegister.Count - 1).LogNo)
				End If
			End If
			cmbLogList.Enabled = False
		End If

		If mReportLogRegister IsNot Nothing Then
			cmbLogList.DataSource = mReportLogRegister

			If AppSettings("ClientCode") = "IND" Then
				Dim mtempReportLogRegister = (From mReportLogRegisterInfo As ReportLogRegister.ReportLogRegisterInfo In mReportLogRegister
											  Order By (mReportLogRegisterInfo.LogDate) Descending, (mReportLogRegisterInfo.DepartureUTCTime) Descending, (mReportLogRegisterInfo.IntLogNo) Descending
											  Select mReportLogRegisterInfo).ToList

				cmbLogList.DataSource = mtempReportLogRegister
				Session("mtempReportLogRegister") = mtempReportLogRegister
			End If

			Session("mReportLogRegister") = mReportLogRegister
		End If


		mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(SELECT)")
		cmbWorkShopList.DataSource = mWorkShopList
		Session("mWorkShopList") = mWorkShopList

		mPeriodUnitList = PeriodUnitList.GetPeriodUnitList(1)
		cmbHourTypeList.DataSource = mPeriodUnitList

		mCustomerList = VendorList.GetVendorstList(0, , , , , , "(SELECT)", True)
		cmbCustomerList.DataSource = mCustomerList

		dgWOJobs.DataSource = mnWO.WOJobs

		dgWONRC.DataSource = mnWO.WONRCJobs 'Added By Vikrant For WO NRC
		dgWOTools.DataSource = mnWO.WOTools
		dgCurrentPeriodValue.DataSource = mnWO.WOPeriods
		dgWOAttachment.DataSource = mnWO.FileAttachments
		txtWODate.Text = mnWO.WODateFormatted
		If mnWO IsNot Nothing Then cmbAircraftList.SelectedValue = mnWO.MachineID.ToString
		mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForWO(WOID:=mnWO.ID, IsForWO:=True, TransactionDate:=mnWO.WODateFormatted.ToString)
		Session("mRequisitionItemsNew") = mRequisitionItemsNew
		mnWOApproveRejectList = nWOApproveRejectList.GetApprovalRejectList(mnWO.ID)
		dgWOStages.DataSource = mnWOApproveRejectList

		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		If mnWO.TransTypeID = Trans.SpareAssemblyWO Then
			mRemovedAssemblyListForCombo = RemovedAssemblyListForCombo.GetAssemblyList(mnWO.WODateFormatted.ToString, "(SELECT)")
			Session("mRemovedAssemblyListForCombo") = mRemovedAssemblyListForCombo
			cmbAssembly.DataSource = mRemovedAssemblyListForCombo
			cmbAssembly.SelectedValue = mnWO.AssemblyStatusID.ToString
		ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then
			mRemovedCompListForCombo = RemovedCompListForCombo.GetCompList(mnWO.WODateFormatted.ToString, AddTopItem:="(SELECT)")
			Session("mRemovedCompListForCombo") = mRemovedCompListForCombo
			cmbCompList.DataSource = mRemovedCompListForCombo
			cmbCompList.SelectedValue = mnWO.AssemblyStatusID.ToString
		End If
		'End

		'Added by saylee on 18-May-2021 for STR18052021
		mServiceProviderList = VendorList.GetVendorstList(0, , , , , , "(SELECT)", IsServiceProvider:=True)
		cmbServiceProvider.DataSource = mServiceProviderList

		DataBind()

		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		If mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO Then
			HighlightSpareAssembly()
		End If
		'End
		If mnWO.IsCustApprovedObtained Then
			cmbCustApprovedByEmailWO.SelectedValue = mnWO.CustApprovedByEmailWO
		End If

		If mnWO IsNot Nothing Then
			'Added By Saylee on 26-Sep-2018 for STR26092018,  Star Air needs Time with Date
			If mnWO.WODate IsNot DBNull.Value Then
				txtWODate.Text = Format(CDate(mnWO.WODateFormatted), AppSettings("DateFormat"))
				txtWOTime.Text = Format(CDate(mnWO.WODateFormatted), AppSettings("TimeFormat"))
			Else
				txtWODate.Text = ""
				txtWOTime.Text = ""
			End If
			'************************************************
			If mnWO.WOStartDate IsNot DBNull.Value Then
				txtStartDate.Text = Format(CDate(mnWO.WOStartDateFormatted), AppSettings("DateFormat"))
				txtStartDateTime.Text = Format(CDate(mnWO.WOStartDateFormatted), AppSettings("TimeFormat"))
			Else
				txtStartDate.Text = ""
				txtStartDateTime.Text = ""
			End If

			If mnWO.WOCloseDate IsNot DBNull.Value Then
				txtCloseDate.Text = Format(CDate(mnWO.WOCloseDateFormatted), AppSettings("DateFormat"))
				txtClosedDateTime.Text = Format(CDate(mnWO.WOCloseDateFormatted), AppSettings("TimeFormat"))
			Else
				txtCloseDate.Text = ""
				txtClosedDateTime.Text = ""
			End If

			If mnWO.WOPlanedDate IsNot DBNull.Value Then
				txtPlanDate.Text = Format(CDate(mnWO.WOPlanedDateFormatted), AppSettings("DateFormat"))
				txtPlanDateTime.Text = Format(CDate(mnWO.WOPlanedDateFormatted), AppSettings("TimeFormat"))
			Else
				txtPlanDate.Text = ""
				txtPlanDateTime.Text = ""
			End If

			'Changes by Saylee on 18-Feb-2013 for ALL18022013
			If mnWO.WOStartDate.ToString <> "" And Not mnWO.MachineID.Equals(Guid.Empty) Then
				If mReportLogRegister.Contains(mnWO.LogID, "") Then cmbLogList.SelectedValue = mnWO.LogID.ToString
			ElseIf mnWO.WODate.ToString <> "" And Not mnWO.MachineID.Equals(Guid.Empty) Then  'Saylee on 16-Sep-2019
				If mReportLogRegister IsNot Nothing Then
					If mReportLogRegister.Contains(mnWO.LogID, "") Then cmbLogList.SelectedValue = mnWO.LogID.ToString
				End If
			End If
			txtBillingDate.Text = IIf(mnWO.BillingDate Is DBNull.Value, "", mnWO.BillingDateFormatted)

		End If

		Select Case mnWO.BillingRequired
			Case 0
				rdbBillingNone.Checked = True  '"None"
				rdbBillingDone.Checked = False
				rdbBillingNotRequired.Checked = False
			Case 1
				rdbBillingDone.Checked = True  '"Billing Done"
				rdbBillingNone.Checked = False
				rdbBillingNotRequired.Checked = False
			Case 2
				rdbBillingNotRequired.Checked = True  '"Not Required"
				rdbBillingNone.Checked = False
				rdbBillingDone.Checked = False

		End Select
		If mnWO.IsQCStatusApproved = 1 Then
			rdbApproved.Checked = True
			rdbNotApproved.Checked = False
			rdbNone.Checked = False
		ElseIf mnWO.IsQCStatusApproved = 2 Then
			rdbNotApproved.Checked = True
			rdbApproved.Checked = False
			rdbNone.Checked = False
		End If

		If mnWO.IsQCStatusApproved = 1 Or mnWO.IsQCStatusApproved = 2 Then
			Dim mnWOApproveReject As nWOApproveReject
			mnWOApproveReject = nWOApproveReject.GetApproval(mnWO.ID)
			If mnWOApproveReject IsNot Nothing Then
				txtQcRemark.Text = mnWOApproveReject.Remark
				If mnWOApproveReject.Date IsNot DBNull.Value Then
					txtQcDate.Text = mnWOApproveReject.DateFormatted
				Else
					txtQcDate.Text = ""
				End If
			End If
		End If

		If Session("MiddleFrame") = "wfnWOExecutionList.aspx" Then
			Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
			txtClosedBy.Text = mUser.EmployeeName
		End If
		mnIssuedWOSpares = nIssuedWOSpares.GetIssuedWOSpares(mnWO.ID)
		mIssuedWOTools = nIssuedWOTools.GetnIssuedWOTools(mnWO.ID)
		Session("mnIssuedWOSpareswfnWODetail") = mnIssuedWOSpares 'Added By Prashant 13-Oct-2020 STR12102020 Again change on 26-Nov-2020
		Session("mIssuedWOToolswfnWODetail") = mIssuedWOTools 'Added By Prashant 13-Oct-2020 STR12102020 Again change on 26-Nov-2020
		If mnIssuedWOSpares.Count > 0 Then
			lnkIssuedSpares.Text = "Issued Spares (" + mnIssuedWOSpares.Count.ToString + ")"
		End If
		If mIssuedWOTools.Count > 0 Then
			lnkIssuedTools.Text = "Issued Tools (" + mIssuedWOTools.Count.ToString + ")"
		End If

		If Not mnWO.CertifyingEmployeeID.Equals(Guid.Empty) Then
			txtLicenceNo.Text = mnWO.CertifylingLicenseNo + " [" + mnWO.CertifyingEmployeeName + "]"
		End If

		If Not mnWO.CertifyingEmployeeID2.Equals(Guid.Empty) Then
			txtLicenceNo2.Text = mnWO.CertifylingLicenseNo2 + " [" + mnWO.CertifyingEmployeeName2 + "]"
		End If

		'End of Added By Prashant on 7-Jul-2020 All07072020
		UpdatePanels()

	End Sub

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)

		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		Try

			If custValidator.ControlToValidate = "txtWODate" Then

				If txtWODate.Text.ToString = "" Then

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso
					   (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
						custValidator.ErrorMessage = "Select E.O. Date."
					Else
						custValidator.ErrorMessage = "Select W.O. Date."
					End If

					e.IsValid = False

				End If

			ElseIf custValidator.ControlToValidate = "txtPlanDate" Then

				If txtPlanDate.Text.ToString = "" And mnWO.WOStatusID = 4 Then

					custValidator.ErrorMessage = "Plan Date required."
					e.IsValid = False

				ElseIf CDate(txtPlanDate.Text.ToString) < CDate(txtWODate.Text.ToString) Then

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso
					   (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
						custValidator.ErrorMessage = "Plan Date should be Greater than or Equal to E.O. Date."
					Else
						custValidator.ErrorMessage = "Plan Date should be Greater than or Equal to Work Order Date."
					End If

					e.IsValid = False

				End If

			ElseIf custValidator.ControlToValidate = "txtCloseDate" Then

				Dim IsWOJobCompletionDateLargerthanWOClosingDate As Boolean = False
				Dim JobSrNo As String = String.Empty

				If txtStartDate.Text <> "" And txtCloseDate.Text <> "" Then

					For Each mnWOJob As nWOJob In mnWO.WOJobs

						If IsDate(mnWOJob.WOJobCloseDateFormatted.ToString) Then

							If AppSettings("ClientCode") IsNot Nothing AndAlso
							   AppSettings("ClientCode") = "IND" Or
							   AppSettings("ClientCode") = "YA" Or
							   AppSettings("ClientCode") = "AFC" Or
							   AppSettings("ClientCode") = "ARA" Or
							   AppSettings("ClientCode") = "BAP" Or
							   AppSettings("ClientCode") = "RPS" Or
							   AppSettings("ClientCode") = "GLD" Then

								If CDate(txtCloseDate.Text.ToString + " " + txtClosedDateTime.Text.ToString.Trim) < CDate(mnWOJob.WOJobCloseDateFormatted.ToString) Then

									IsWOJobCompletionDateLargerthanWOClosingDate = True
									JobSrNo = mnWOJob.SrNo

									Exit For

								End If

							Else

								''Added this Condition to avoid the confilt of Closing Date of W.O and Closing DateTime of Job on same Day                                
								If CDate(txtCloseDate.Text.ToString + " " + "23:59") < CDate(mnWOJob.WOJobCloseDateFormatted.ToString) Then

									IsWOJobCompletionDateLargerthanWOClosingDate = True
									JobSrNo = mnWOJob.SrNo

									Exit For

								End If

							End If

						End If

					Next

					If CDate(txtCloseDate.Text.ToString + " " + txtClosedDateTime.Text.ToString.Trim) < CDate(txtStartDate.Text.ToString + " " + txtStartDateTime.Text.ToString.Trim) Then

						If IsWOJobCompletionDateLargerthanWOClosingDate Then

							If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
								custValidator.ErrorMessage = $"Closing Date of E.O. should be Greater than or Equal to E.O's Start Date &  E.O. Job SrNo {JobSrNo}'s Closing Date." &
															 "<BR>" & "( To Update Job's Closing Date firstly clear E.O's Closing Date. ) "
							Else
								custValidator.ErrorMessage = $"Closing Date of W.O. should be Greater than or Equal to W.O's Start Date & W.O. Job SrNo {JobSrNo}'s Closing Date." &
															 "<BR>" & "( To Update Job's Closing Date firstly clear W.O's Closing Date. ) "
							End If

						Else

							If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
								custValidator.ErrorMessage = "Closing Date of E.O. should be Greater than or Equal to E.O's Start Date."
							Else
								custValidator.ErrorMessage = "Closing Date of W.O. should be Greater than or Equal to W.O's Start Date."
							End If

						End If

						e.IsValid = False

					Else

						If IsWOJobCompletionDateLargerthanWOClosingDate Then

							If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
								custValidator.ErrorMessage = $"Closing Date of E.O. should be Greater than or Equal to E.O. Job SrNo {JobSrNo}'s Closing Date." &
															 "<BR>" & "( To Update Job's Closing Date firstly clear E.O's Closing Date. ) "

							Else
								custValidator.ErrorMessage = $"Closing Date of W.O. should be Greater than or Equal to W.O. Job SrNo {JobSrNo}'s Closing Date." &
															 "<BR>" & "( To Update Job's Closing Date firstly clear W.O's Closing Date. ) "
							End If

							e.IsValid = False

						End If

					End If

				End If

			ElseIf custValidator.ControlToValidate = "txtRemark" Then

				If Len(txtRemark.Text) > 500 Then

					custValidator.ErrorMessage = "Remark must not be Greater than 500 Char."
					e.IsValid = False

				Else
					e.IsValid = True
				End If

			ElseIf custValidator.ControlToValidate = "cmbJobType" Then

				If (cmbJobType.SelectedValue = "2" Or cmbJobType.SelectedValue = "3") And
					cmbAircraftList.SelectedIndex <= 0 And
					(mnWO.TransTypeID <> Trans.SpareAssemblyWO And mnWO.TransTypeID <> Trans.SpareComponentWO) Then

					custValidator.ErrorMessage = "Please select Aircraft from the list."
					e.IsValid = False

				ElseIf (cmbJobType.SelectedValue = "6" And Trim(txtModelNo.Text) = "") Then 'Added By Vikrant on 24-Apr-2018 For All24042018

					custValidator.ErrorMessage = "Please enter Model No."
					e.IsValid = False

				ElseIf txtPlanDate.Text.ToString = "" And mnWO.WOStatusID = 4 Then 'Added by Saylee, as controltovalidate need to be fired for blank plan date

					custValidator.ErrorMessage = "Plan Date required."
					e.IsValid = False

				Else
					e.IsValid = True
				End If

			ElseIf custValidator.ControlToValidate = "txtStartDate" Then

				If txtStartDate.Text.ToString <> "" Then

					If CDate(txtStartDate.Text.ToString) < CDate(txtWODate.Text.ToString) Then

						If (AppSettings("ClientCode") IsNot Nothing) AndAlso
						   (AppSettings("ClientCode") = "TAAL" Or
							AppSettings("ClientCode") = "GlobalJet") Then
							custValidator.ErrorMessage = "Start Date should be equal or later to EO. Date"
						Else
							custValidator.ErrorMessage = "Start Date should be equal or later to WO. Date"
						End If

						e.IsValid = False

					ElseIf Not IsDate(txtStartDate.Text) Then

						custValidator.ErrorMessage = "Start date should be in valid date format."
						e.IsValid = False

					Else

						Dim Date1, Time1 As String
						Date1 = txtStartDate.Text.ToString
						Time1 = txtStartDate.Text.ToString

						If Date1 = "1/1/0001" Then

							custValidator.ErrorMessage = "Start date should be in valid date format."
							e.IsValid = False

							Exit Sub

						End If

						txtStartDate.Text = Date1
						e.IsValid = True

					End If

				End If

			ElseIf custValidator.ControlToValidate = "txtQcDate" Then

				If txtQcDate.Text.ToString <> "" Then

					If AppSettings("ShowNewWOFlow") = "True" Then 'If AppSettings("ClientCode") = "IND" Then

						If CDate(txtQcDate.Text.ToString) < CDate(Format(CDate(mnWO.WOCloseDate), AppSettings("DateFormat"))) Then

							custValidator.ErrorMessage = "QC Date should be Greater than Work Order Close Date"
							e.IsValid = False

						End If

					End If

				End If

			ElseIf custValidator.ControlToValidate = "txtIssueTo" Then

				If Len(Trim(txtIssueTo.Text)) > 150 Then

					custValidator.ErrorMessage = IIf(AppSettings("ClientCode") = "IND",
													 "AMO Ref must not be Greater than 150 Char.",
													 "Issue To information must not be Greater than 150 Char.")
					e.IsValid = False

				Else
					e.IsValid = True
				End If

			ElseIf custValidator.ControlToValidate = "cmbAircraftList" Then

				If cmbAircraftList.SelectedIndex <= 0 And mnWO.TransTypeID = Trans.WOCAMO Then

					custValidator.ErrorMessage = "Please select Aircraft from the list."
					e.IsValid = False

				Else
					e.IsValid = True
				End If

			ElseIf custValidator.ControlToValidate = "cmbAssembly" Then 'Added By Vikrant On 27-Jul-2020 For ALL27072020

				If mnWO.TransTypeID = Trans.SpareComponentWO And cmbCompList.SelectedIndex <= 0 Then

					custValidator.ErrorMessage = "Please select Component from the list."
					e.IsValid = False

				ElseIf mnWO.TransTypeID = Trans.SpareAssemblyWO And cmbAssembly.SelectedIndex <= 0 Then

					custValidator.ErrorMessage = "Please select Assembly from the list."
					e.IsValid = False

				Else
					e.IsValid = True
				End If

			ElseIf custValidator.ControlToValidate = "cmbWorkShopList" And
				   (AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "STR") Then 'Added By Prashant 16-Sep-2019

				If cmbWorkShopList.SelectedIndex <= 0 Then

					custValidator.ErrorMessage = "Please select Location from the list."
					e.IsValid = False

				Else
					e.IsValid = True
				End If

			ElseIf custValidator.ControlToValidate = "txtSerialNo" Then

				If txtSerialNo.Text = "" Then

					custValidator.ErrorMessage = "Enter the Serial No."
					e.IsValid = False

				Else
					e.IsValid = True
				End If

			ElseIf custValidator.ControlToValidate = "txtLicenceNo" Then

				If txtLicenceNo.Text <> "" And txtLicenceNo2.Text <> "" Then

					If txtLicenceNo.Text = txtLicenceNo2.Text Then
						custValidator.ErrorMessage = "Select different Certifying Employees"
						e.IsValid = False
					Else
						e.IsValid = True
					End If

				End If

			ElseIf custValidator.ControlToValidate = "txtNoOfSupplementalSheets" Then

				If chkSupplementalSheetAttached.Checked Then

					If txtNoOfSupplementalSheets.Text = "0" Then

						custValidator.ErrorMessage = "Enter No of Supplements to be attached"
						e.IsValid = False

					Else
						e.IsValid = True
					End If

				End If

			ElseIf custValidator.ControlToValidate = "txtNoOfNRCs" Then

				If chkNRCRaised.Checked Then

					If txtNoOfNRCs.Text = "0" Then

						custValidator.ErrorMessage = "Enter No of NRC(s)"
						e.IsValid = False

					Else
						e.IsValid = True
					End If

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Function CustomValidateObject() As Boolean

		If Flag = 1 Then Exit Function

		SetObject()
		SetGridObject()
		Dim str As String = ""
		Dim txtValue As TextBox

		Try

			If Not mnWO.IsValid Then

				For i As Integer = 0 To mnWO.GetBrokenRulesCollection.Count - 1
					str = str + mnWO.GetBrokenRulesCollection(i).Description + "<Br>"
				Next

				For j As Integer = 0 To mnWO.WOJobs.Count - 1

					For i As Integer = 0 To mnWO.WOJobs(j).GetBrokenRulesCollection.Count - 1
						str = str + mnWO.WOJobs(j).GetBrokenRulesCollection(i).Description + "<Br>"
					Next

				Next

			End If

			For i As Integer = 0 To CShort(dgCurrentPeriodValue.Rows.Count - 1)

				If Not mnWO.WOPeriods.Item(i).IsValid Then

					Dim x As Integer

					For x = 0 To mnWO.WOPeriods.Item(i).GetBrokenRulesCollection.Count - 1
						str = str + mnWO.WOPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
					Next

				End If

				txtValue = CType(Me.dgCurrentPeriodValue.Rows(i).FindControl("txtValue"), TextBox)

				If mnWO.WOPeriods(i).PeriodID = 2 Then

					If Not Period.IsDate(txtValue.Text) Then
						str = str + "Valid Date Required."
					End If

				End If

			Next

			If str <> "" Then

				cvCurrentValue.ErrorMessage = str
				cvCurrentValue.IsValid = False

				Return False

			End If

			Flag = 1

			Return True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	'Added By Saylee On 26-Sep-2018 For STR26092018
	Private Function IsValidTime(TimeValue As String) As Boolean
		Dim TimeRegulerExpression As String = ""
		If (AppSettings("TimeFormat").IndexOf("tt") <> -1 Or AppSettings("TimeFormat").IndexOf("TT") <> -1) Then
			'TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm)$"    '12 Hour Format
			TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm|aM|pM)$"    '12 Hour Format
		Else
			TimeRegulerExpression = "^(([01][0-9])|(2[0-3])|([0-9])):[0-5][0-9]$"   '24 Hour Format
		End If

		If (Text.RegularExpressions.Regex.IsMatch(TimeValue, TimeRegulerExpression)) Then
			Return True
		Else
			Return False
		End If
	End Function
	'End

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			'Put user code to initialize the page here
			AddAttributes()
			GetSession()

			EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011

			If Not Page.IsPostBack Then

				SetFocus(txtNo)
				AddSelectedPeroids()

				If AppSettings("ClientCode") = "RAL" Then

					NRCJobText = "NRC / OJS & PDR"
					lblAddWONRC.Text = "W.O. NRC / OJS & PDR"
					btnAddNRC.ToolTip = "Click to Add WO NRC / OJS & PDR"

				Else

					NRCJobText = IIf(AppSettings("ClientCode") = "IND", "OJS", "NRC")
					lblAddWONRC.Text = IIf(AppSettings("ClientCode") = "IND", "W.O. OJS", "W.O. NRC")
					btnAddNRC.ToolTip = IIf(AppSettings("ClientCode") = "IND", "Click to Add WO OJS", "Click to Add WO NRC")

				End If

				Session("NRCJobText") = NRCJobText
				FillJobTypeCombo(mnWO.MachineID)
				DataFieldBind()
				UserNameForLicenceList = User.Identity.Name
				Session("UserNameForLicenceList") = UserNameForLicenceList

				''Project
				If (Session("wfProject_Ajax") = "wfProject_Ajax") And
				   mnWO.IsNew And mnWO.WOJobs.Count = 0 And
				   Not mnWO.MachineID.Equals(Guid.Empty) And
				   Session("OpenFromProject") IsNot Nothing Then

					Page.Validate()
					cmbJobType.SelectedValue = mnWO.WOJobTypeID.ToString
					Session("OpenFromProject") = "OpenFromProject"
					AddJobs(btnAddJob, New ImageClickEventArgs(0, 0))

				End If

			End If

			SetPage()
			SetGrid()
			SetNRCGrid() 'Added By Vikrant For WO NRC
			ControlVisibility()

			If Session("IsShowAllWOs") = True And Not mnWO.IsNew Then
				UsedForAllWO.Enabled = False
				UsedForAllWO1.Enabled = False
				UsedForAllWO2.Enabled = False
			End If

			cmbWorkShopList.Attributes.Add("title", "Selected Workshop is : " + cmbWorkShopList.SelectedItem.Text)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	'Added By Vikrant On 27-Jul-2020 For ALL27072020
	Private Sub cmbAssembly_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAssembly.SelectedIndexChanged
		If mnWO.WOPeriods.Count <> 0 Then
			For i As Integer = mnWO.WOPeriods.Count - 1 To 0 Step -1
				mnWO.WOPeriods.RemoveAt(i)
			Next
		End If
		If cmbAssembly.SelectedIndex > 0 Then
			Dim mAssemblyStatus As AssemblyStatus

			mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(New Guid(cmbAssembly.SelectedValue), True)

			mnWO.WOPeriods.SetWOPeriods(mnWO.ID, mAssemblyStatus.AssemblyStatusPeriods, mnWO.HourType, mAssemblyStatus.IsRemoved)
			mnWO.ModelName = mAssemblyStatus.ModelName
			mnWO.SerialNo = mAssemblyStatus.Assembly.SerialNo
			dgCurrentPeriodValue.DataSource = mnWO.WOPeriods
			dgCurrentPeriodValue.DataBind()
			txtModelNo.ReadOnly = True
			txtSerialNo.ReadOnly = True
			txtModelNo.DataBind()
			txtSerialNo.DataBind()
			dgCurrentPeriodValue.Columns(3).Visible = False
			btnSelectPeriod.Enabled = False
		Else
			txtModelNo.Text = ""
			txtSerialNo.Text = ""
			txtModelNo.ReadOnly = False
			txtSerialNo.ReadOnly = False
			dgCurrentPeriodValue.Columns(3).Visible = True
			btnSelectPeriod.Enabled = True
		End If
		HighlightSpareAssembly() 'Added By Vikrant On 27-Jul-2020 For ALL27072020
		upnlStartDetails.Update()
		upnlAirframePeriods.Update()
		upnlJobType.Update()
	End Sub

	Private Sub cmbCompList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCompList.SelectedIndexChanged
		If mnWO.WOPeriods.Count <> 0 Then
			For i As Integer = mnWO.WOPeriods.Count - 1 To 0 Step -1
				mnWO.WOPeriods.RemoveAt(i)
			Next
		End If
		If cmbCompList.SelectedIndex > 0 Then
			Dim mCompStatus As CompStatus
			If mRemovedCompListForCombo(New Guid(cmbCompList.SelectedValue)).IsSpareComp Then
				mCompStatus = CompStatus.GetSpareCompStatus(New Guid(cmbCompList.SelectedValue), True)
			Else
				mCompStatus = CompStatus.GetCompStatus(New Guid(cmbCompList.SelectedValue), Guid.Empty, mnWO.WODateFormatted.ToString)
			End If
			mnWO.WOPeriods.SetWOPeriods(mnWO.ID, mCompStatus.CompStatusPeriods, mnWO.HourType, mCompStatus.IsRemoved)
			mnWO.ModelName = mCompStatus.PartName
			mnWO.SerialNo = mCompStatus.SerialNo
			dgCurrentPeriodValue.DataSource = mnWO.WOPeriods
			dgCurrentPeriodValue.DataBind()
			txtModelNo.ReadOnly = True
			txtSerialNo.ReadOnly = True
			txtModelNo.DataBind()
			txtSerialNo.DataBind()
			dgCurrentPeriodValue.Columns(3).Visible = False
			btnSelectPeriod.Enabled = False
		Else
			txtModelNo.Text = ""
			txtSerialNo.Text = ""
			txtModelNo.ReadOnly = False
			txtSerialNo.ReadOnly = False
			dgCurrentPeriodValue.Columns(3).Visible = True
			btnSelectPeriod.Enabled = True
		End If
		HighlightSpareAssembly() 'Added By Vikrant On 27-Jul-2020 For ALL27072020
		upnlStartDetails.Update()
		upnlAirframePeriods.Update()
		upnlJobType.Update()
	End Sub
	'End

	Private Sub cmbAircraftList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAircraftList.SelectedIndexChanged
		'SetObject()
		AjaxLoader.Visible = False
		txtLogNo.Text = ""
		If cmbAircraftList.SelectedIndex > 0 Then
			Dim tmpAssemblyStatusList As AssemblyStatusList
			mnWO.MachineID = New Guid(cmbAircraftList.SelectedValue.ToString)
			If (AppSettings("ClientCode") = "RAL" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "FBW") Then
				Dim TempRegNo As String = ""
				TempRegNo = cmbAircraftList.SelectedItem.Text
				mnWO.WOText = Replace(TempRegNo, "VT-", "")
				If AppSettings("ClientCode") = "ADeccan" Then 'ADeccan Code Added by Saylee on 11-May-2018 for ADeccan11052018
					mnWO.WOText = mnWO.WOText + "/" + Today.Date.ToString("yy")
				ElseIf AppSettings("ClientCode") = "FBW" Then 'FBW Code Added by Saylee on 10-Jan-2022 for FBW10012022
					mnWO.WOText = mnWO.WOText + "/" + Today.Date.ToString("yyyy") + Today.Date.ToString("MM") + Today.Date.ToString("dd")
				End If

				txtText.Text = mnWO.WOText
				txtText.DataBind()
				UpnlWODet.Update()

				'Added By Saylee on 29-Jul-2016 for YATA29072016 as per their requirement
			ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
				mnWO.WOText = "MJO# " & CStr(CDate(txtWODate.Text).Date.Year) & " - " & mnWO.ModelName
				txtText.Text = mnWO.WOText
				txtText.DataBind()
				UpnlWODet.Update()
			ElseIf AppSettings("ClientCode") = "TP" Or AppSettings("ClientCode") = "IND" Then  'AppSettings("ClientCode") = "IND" Added By Prashant 16-Sep-2019
				If cmbAircraftList.SelectedIndex > 0 Then
					txtText.Text = Replace(cmbAircraftList.SelectedItem.Text, "VT-", "") & "/" & CStr(CDate(txtWODate.Text).Date.Year)
					mnWO.WOText = txtText.Text
					txtText.DataBind()
					UpnlWODet.Update()
				End If
			ElseIf AppSettings("ClientCode") = "7AR" Then
				If cmbAircraftList.SelectedIndex > 0 Then
					txtText.Text = Replace(cmbAircraftList.SelectedItem.Text, "VT-", "") & "-" & CStr(CDate(txtWODate.Text).Date.Year) & "-" & CDate(txtWODate.Text).ToString("MM")
					mnWO.WOText = txtText.Text
					txtText.DataBind()
					UpnlWODet.Update()
				End If
				'Sankalp 27-11-25
			ElseIf AppSettings("ClientCode") = "CVA" AndAlso cmbAircraftList.SelectedIndex > 0 Then
				txtText.Text = Replace(cmbAircraftList.SelectedItem.Text, "D4-", "")
				mnWO.WOText = txtText.Text
				txtText.DataBind()
				UpnlWODet.Update()
			End If
			If txtStartDate.Text.ToString <> "" Then
				cmbLogList.Enabled = True
				'mTempAssemblyList = AssemblyList.GetAssemblyList(1, cmbAircraftList.SelectedValue.ToString, txtStartDate.Text.ToString)
				tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtStartDate.Text.ToString, cmbAircraftList.SelectedValue.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
				AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
			Else
				'cmbLogList.Enabled = False Commented by Saylee on 21-Jul-2022, as its need to be opened
				'mTempAssemblyList = AssemblyList.GetAssemblyList(1, cmbAircraftList.SelectedValue.ToString, txtWODate.Text.ToString)
				tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtWODate.Text.ToString, cmbAircraftList.SelectedValue.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
				AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
			End If
			'Session("mTempAssemblyList") = mTempAssemblyList
			If mnWO.WOPeriods.Count <> 0 Then
				For i As Integer = mnWO.WOPeriods.Count - 1 To 0 Step -1
					mnWO.WOPeriods.RemoveAt(i)
				Next
			End If
			mnWO.WOPeriods.SetWOPeriods(mnWO.ID, AssemblyStatusPeriodList, mnWO.HourType)
			dgCurrentPeriodValue.DataSource = mnWO.WOPeriods
			dgCurrentPeriodValue.DataBind()

			If txtWODate.Text.ToString <> "" Then  'Added by Saylee 16-Sep-2019
				If mnWO IsNot Nothing And Not mnWO.IsNew Then
					mReportLogRegister = ReportLogRegister.GetRectifiedLog(txtWODate.Text, txtWODate.Text, tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyID.ToString, mnWO.MachineID.ToString, False, , 1, , , , "(SELECT)", , , True)
				Else
					mReportLogRegister = ReportLogRegister.GetRectifiedLog(txtWODate.Text, txtWODate.Text, tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyID.ToString, New Guid(cmbAircraftList.SelectedValue.ToString).ToString, False, , 1, , , , "(SELECT)", , , True)
				End If
				''Commented by Saylee on 21-Jul-2022, 
				''If txtLogNo.Text = "" AndAlso Not mReportLogRegister Is Nothing AndAlso mReportLogRegister.Count > 0 Then
				''    txtLogNo.Text = IIf(mReportLogRegister Is Nothing Or mReportLogRegister.Count = 0, "", mReportLogRegister(0).LogNo)

				''End If
			ElseIf txtStartDate.Text.ToString <> "" Then
				If mnWO IsNot Nothing And Not mnWO.IsNew Then
					mReportLogRegister = ReportLogRegister.GetRectifiedLog(mnWO.WOStartDate.ToString, txtStartDate.Text, tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyID.ToString, mnWO.MachineID.ToString, False, , 1, , , , "(SELECT)", , , True)
				Else
					mReportLogRegister = ReportLogRegister.GetRectifiedLog(txtStartDate.Text, txtStartDate.Text, tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyID.ToString, New Guid(cmbAircraftList.SelectedValue.ToString).ToString, False, , 1, , , , "(SELECT)", , , True)
				End If
			End If
			cmbLogList.DataSource = mReportLogRegister  ''Opened by Saylee on 21-Jul-2022      
			Session("mReportLogRegister") = mReportLogRegister
			cmbLogList.DataBind()

			SetLog()

			txtRegNo.ReadOnly = True
			txtModelNo.ReadOnly = True
			txtSerialNo.ReadOnly = True

			txtRegNo.DataBind()
			txtModelNo.DataBind()
			txtSerialNo.DataBind()
			cmbCustomerList.DataBind()
			cmbHourTypeList.DataBind()

			dgCurrentPeriodValue.Columns(3).Visible = False
			btnSelectPeriod.Enabled = False

			FillJobTypeCombo(mnWO.MachineID)
			cmbJobType.DataBind()

		Else
			mnWO.MachineID = Guid.Empty
			txtRegNo.ReadOnly = False
			txtModelNo.ReadOnly = False
			txtSerialNo.ReadOnly = False

			cmbLogList.ClearSelection()
			cmbLogList.DataSource = Nothing
			cmbLogList.DataBind()
			'cmbLogList.SelectedIndex = 0
			cmbLogList.Enabled = False

			If mnWO.WOPeriods.Count <> 0 Then
				For i As Integer = mnWO.WOPeriods.Count - 1 To 0 Step -1
					mnWO.WOPeriods.RemoveAt(i)
				Next
			End If
			dgCurrentPeriodValue.DataSource = mnWO.WOPeriods
			dgCurrentPeriodValue.DataBind()

			txtRegNo.Text = ""
			txtModelNo.Text = ""
			txtSerialNo.Text = ""
			cmbCustomerList.SelectedIndex = 0
			dgCurrentPeriodValue.Columns(3).Visible = True
			btnSelectPeriod.Enabled = True
			FillJobTypeCombo(Guid.Empty)
			cmbJobType.DataBind()
		End If
		Session("mnWO") = mnWO
		UpnlWODet.Update()
		If AppSettings("ClientCode") = "RAL" Then
			cmbAircraftList.Enabled = (mnWO.WOJobs.Count = 0 And mnWO.WONRCJobs.Count = 0) And chkMaintenance.Checked
		Else
			cmbAircraftList.Enabled = (mnWO.WOJobs.Count = 0) And chkMaintenance.Checked
		End If


		upnlStartDetails.Update()
		upnlAirframePeriods.Update()
		upnlJobType.Update()
	End Sub

	Private Sub txtWODate_TextChanged(sender As Object, e As EventArgs) Handles txtWODate.TextChanged
		If cmbAircraftList.SelectedIndex > 0 Then
			Dim mMachine As Machine
			mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue), False)
			If mMachine.NIUContext And mMachine.NIUDContextFormatted.ToString <> "" Then
				If IsDate(mMachine.NIUDContextFormatted.ToString) Then
					If CDate(mMachine.NIUDContextFormatted.ToString) <= CDate(txtWODate.Text) Then
						MSGBoxCtrl.Show("Alert", "Selected Aircraft " + cmbAircraftList.SelectedItem.ToString + " is marked as Not In Use on " + mMachine.NIUDContextFormatted.ToString + ".Work Order Date should be less than Aircraft Not In Use Date.", "", MsgBoxStyle.OkOnly, "NotInUseSelectInWODate")
						Exit Sub
					End If
				End If
			End If
		End If


		If AppSettings("ClientCode") = "TP" Then
			If cmbAircraftList.SelectedIndex > 0 And txtWODate.Text.ToString <> "" Then
				txtText.Text = Replace(cmbAircraftList.SelectedItem.Text, "VT-", "") & "/" & CStr(CDate(txtWODate.Text).Date.Year)
				mnWO.WOText = txtText.Text
				txtText.DataBind()
			End If
		End If
		If txtWODate.Text.ToString <> "" And cmbAircraftList.SelectedIndex > 0 Then
			If txtStartDate.Text.ToString = "" And cmbAircraftList.SelectedIndex > 0 Then
				Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtWODate.Text.ToString, cmbAircraftList.SelectedValue.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
				AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
				If mnWO.WOPeriods.Count <> 0 Then
					For i As Integer = mnWO.WOPeriods.Count - 1 To 0 Step -1
						mnWO.WOPeriods.RemoveAt(i)
					Next
				End If
				mnWO.WOPeriods.SetWOPeriods(mnWO.ID, AssemblyStatusPeriodList, mnWO.HourType)
				dgCurrentPeriodValue.DataSource = mnWO.WOPeriods
				dgCurrentPeriodValue.DataBind()

				If mnWO IsNot Nothing And Not mnWO.IsNew Then
					mReportLogRegister = ReportLogRegister.GetRectifiedLog(txtWODate.Text, txtWODate.Text, tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyID.ToString, mnWO.MachineID.ToString, False, , 1, , , , , , , True)
				Else
					mReportLogRegister = ReportLogRegister.GetRectifiedLog(txtWODate.Text, txtWODate.Text, tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyID.ToString, New Guid(cmbAircraftList.SelectedValue.ToString).ToString, False, , 1, , , , , , , True)
				End If

				''  If AppSettings("ClientCode") = "IND" Then
				Dim mtempReportLogRegister = (From mReportLogRegisterInfo As ReportLogRegister.ReportLogRegisterInfo In mReportLogRegister
											  Order By (mReportLogRegisterInfo.LogDate) Descending, (mReportLogRegisterInfo.DepartureUTCTime) Descending, (mReportLogRegisterInfo.IntLogNo) Descending
											  Select mReportLogRegisterInfo).ToList

				cmbLogList.DataSource = mtempReportLogRegister
				Session("mtempReportLogRegister") = mtempReportLogRegister
				If mReportLogRegister Is Nothing OrElse mReportLogRegister.Count = 0 Then
					txtLogNo.Text = ""
				Else
					txtLogNo.Text = mReportLogRegister(mReportLogRegister.Count - 1).LogNo
				End If
				''End If
				''cmbLogList.ClearSelection()
				''cmbLogList.DataSource = Nothing
				cmbLogList.DataBind()
				'cmbLogList.SelectedIndex = 0
				cmbLogList.Enabled = False
			End If
		Else
			If mnWO.WOPeriods.Count <> 0 Then
				For i As Integer = mnWO.WOPeriods.Count - 1 To 0 Step -1
					mnWO.WOPeriods.RemoveAt(i)
				Next
			End If
			dgCurrentPeriodValue.DataSource = mnWO.WOPeriods
			dgCurrentPeriodValue.DataBind()
			cmbLogList.ClearSelection()
			cmbLogList.DataSource = Nothing
			cmbLogList.DataBind()
			'cmbLogList.SelectedIndex = 0
			cmbLogList.Enabled = False
		End If
		upnlMachineDet.Update()
		upnlAirframePeriods.Update()
		upnlStartDetails.Update()
	End Sub

	Private Sub txtStartDate_TextChanged(sender As Object, e As EventArgs) Handles txtStartDate.TextChanged

		If txtStartDate.Text.ToString <> "" And cmbAircraftList.SelectedIndex > 0 Then
			txtLogNo.Text = ""
			Dim mMachine As Machine
			mMachine = Machine.GetMachine(New Guid(cmbAircraftList.SelectedValue), False)
			If mMachine.NIUContext And mMachine.NIUDContextFormatted.ToString <> "" Then
				If IsDate(mMachine.NIUDContextFormatted.ToString) Then
					If CDate(mMachine.NIUDContextFormatted.ToString) <= CDate(txtStartDate.Text) Then
						MSGBoxCtrl.Show("Alert", "Selected Aircraft " + cmbAircraftList.SelectedItem.ToString + " is marked as Not In Use on " + mMachine.NIUDContextFormatted.ToString + ".Start Date should be less than Aircraft Not In Use Date.", "", MsgBoxStyle.OkOnly, "NotInUseSelectInWOStartDate")
						Exit Sub
					End If
				End If
			End If

			cmbLogList.Enabled = True
			dgCurrentPeriodValue.DataSource = mnWO.WOPeriods
			dgCurrentPeriodValue.DataBind()

			Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtStartDate.Text.ToString, cmbAircraftList.SelectedValue.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
			AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
			If mnWO.WOPeriods.Count <> 0 Then
				For i As Integer = mnWO.WOPeriods.Count - 1 To 0 Step -1
					mnWO.WOPeriods.RemoveAt(i)
				Next
			End If
			mnWO.WOPeriods.SetWOPeriods(mnWO.ID, AssemblyStatusPeriodList, mnWO.HourType)
			dgCurrentPeriodValue.DataSource = mnWO.WOPeriods
			dgCurrentPeriodValue.DataBind()


			If mnWO IsNot Nothing And Not mnWO.IsNew Then
				mReportLogRegister = ReportLogRegister.GetRectifiedLog(txtStartDate.Text, txtStartDate.Text, tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyID.ToString, mnWO.MachineID.ToString, False, , 1, , , , "(SELECT)", , , True)
			Else
				mReportLogRegister = ReportLogRegister.GetRectifiedLog(txtStartDate.Text, txtStartDate.Text, tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyID.ToString, New Guid(cmbAircraftList.SelectedValue.ToString).ToString, False, , 1, , , , "(SELECT)", , , True)
			End If

			cmbLogList.DataSource = mReportLogRegister
			Session("mReportLogRegister") = mReportLogRegister
			cmbLogList.DataBind()
			SetObject()
			SetGridObject()
		Else
			If txtStartDate.Text.ToString = "" And cmbAircraftList.SelectedIndex > 0 Then
				Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtWODate.Text.ToString, cmbAircraftList.SelectedValue.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
				AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
				If mnWO.WOPeriods.Count <> 0 Then
					For i As Integer = mnWO.WOPeriods.Count - 1 To 0 Step -1
						mnWO.WOPeriods.RemoveAt(i)
					Next
				End If
				mnWO.WOPeriods.SetWOPeriods(mnWO.ID, AssemblyStatusPeriodList, mnWO.HourType)
				dgCurrentPeriodValue.DataSource = mnWO.WOPeriods
				dgCurrentPeriodValue.DataBind()
				cmbLogList.ClearSelection()
				cmbLogList.DataSource = Nothing
				cmbLogList.DataBind()
				'cmbLogList.SelectedIndex = 0
				''cmbLogList.Enabled = False Commented by Saylee on 21-Jul-2022, as its need to be opened
			End If
		End If

		upnlAirframePeriods.Update()
		upnlValidationsummary.Update()
		'  If Not IsValid Then upnlValidationsummary.Update() : Exit Sub
	End Sub

	Private Sub AddJobs(sender As Object, e As ImageClickEventArgs) Handles btnAddJob.Click

		Try

			'Added by Saylee on 7-Mar-2014 for ALL07032014
			If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or
			   (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then

				SetSession()
				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"Authorization")

				Exit Sub

			End If

			'If Not CustomValidateObject() Then upnlValidationsummary.Update() : Exit Sub

			If IsValid Then

				'For BRD
				If MaxWOExceeded() Then
					Exit Sub
				End If
				'End

				SetObject()
				SetGridObject()
				mnWO.WOJobTypeID = CInt(cmbJobType.SelectedValue)
				Session("WOJobTypeID") = CInt(cmbJobType.SelectedValue)
				Session("Edit") = False
				Session("IsWOForRemovedOrSpareComp") = False
				Session("IsWOForRemovedOrSpareAssembly") = False

				Select Case CInt(cmbJobType.SelectedValue)
					Case 1, 7 '1 UnScheduled  '7 Shop Work Order Added By Prashant 16-Sep-2019

						If Not mnWO.IsNew Then
							mnWO = WOHelper.FetchWO(ID:=mnWO.ID)
						End If

						mnWO.WOJobs.Add(mnWO.ID, Val(Session("WOJobTypeID")))
						Session("mnWO") = mnWO
						Response.Redirect("wfnWOJobDetail.aspx?BackPage1=wfnWODetail_AJAX.aspx" &
										  "&BackPage=" & Request.QueryString("BackPage"))

					Case 2 'Scheduled Jobs

						Session("wfWODetail.SelectList") = "DueJobs"
						Session("mnWO") = mnWO

						If mnWO.TransTypeID = Trans.SpareAssemblyWO Then 'Added By Vikrant On 27-Jul-2020 For ALL27072020

							Session("IsWOForRemovedOrSpareAssembly") = mRemovedAssemblyListForCombo(New Guid(cmbAssembly.SelectedValue.ToString)).IsSpareAssembly
							Session("AssemblyID") = mRemovedAssemblyListForCombo(New Guid(cmbAssembly.SelectedValue)).AssemblyID.ToString
							Session("IsRemovedAssembly") = IIf(mRemovedAssemblyListForCombo(New Guid(cmbAssembly.SelectedValue)).IsSpareAssembly, "False", "True")
							Response.Redirect("wfnWOSelectDueJobListForSparedAssemblies_AJAX.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))

						ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then

							Session("CompStatusID") = cmbCompList.SelectedValue.ToString
							Session("IsSpareOrRemovedComp") = IIf(mRemovedCompListForCombo(New Guid(cmbCompList.SelectedValue)).IsSpareComp, "1", "2")
							Session("IsWOForRemovedOrSpareComp") = mRemovedCompListForCombo(New Guid(cmbCompList.SelectedValue.ToString)).IsSpareComp
							Response.Redirect("wfnWOSelectDueJobListForSparedAssemblies_AJAX.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))

						Else
							Session("mIsNewDueReportObjectBindingRequired") = IIf(CDate(mnWO.WODateFormatted).ToString(AppSettings("DateFormat")) >= CDate(Today.Date.ToString(AppSettings("DateFormat"))), "True", "") 'Added by vikrant on 19-May-2021
							Response.Redirect("wfnWOSelectDueJobList_AJAX.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
						End If

					Case 3 'Snag/MEL

						Session("wfWODetail.SelectList") = "SnagMELJobs"
						Session("mnWO") = mnWO
						Response.Redirect("wfnWOSelectMELJobList_Ajax.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))

					Case 4 'Deferred

						Session("wfWODetail.SelectList") = "DeferredJobs"
						Session("mnWO") = mnWO
						Response.Redirect("wfnWOSelectDeferredJobList_AJAX.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))

					Case 5 'NRC

						If AppSettings("ClientCode") <> "RAL" And mnWO.WOJobs.Count = 0 Then

							MSGBoxCtrl.Show("Add Alert!",
											"Record can not be added",
											NRCJobText & " cannot be added without Job.",
											MsgBoxStyle.OkOnly, "")

							Exit Sub

						End If

						AddWONRC()

					Case 6 'From Model Maint. Activity 'Added By Vikrant on 24-Apr-2018 For All24042018

						Session("wfWODetail.SelectList") = "ModelMaintActivityJobs"
						Session("mnWO") = mnWO
						Response.Redirect("wfnWOModelMaintActivityJobList_Ajax.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))

				End Select

			Else
				upnlValidationsummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub btnAddTool_Click(sender As Object, e As ImageClickEventArgs) Handles btnAddTool.Click
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		If Not CustomValidateObject() Then upnlValidationsummary.Update() : Exit Sub
		If IsValid Then
			SetObject()
			SetGridObject()
			mnWO.WOTools.Add(mnWO.ID)
			Session("mnWO") = mnWO
			Session("Edit") = False
			' Response.Redirect("wfnWOTool_AJAX.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
			ScriptManager.RegisterStartupScript(Me, [GetType], "OpenWOTool", "OpenWOTool();", True)
		Else
			upnlValidationsummary.Update()
		End If
	End Sub

	Private Sub btnSelectPeriod_Click(sender As Object, e As ImageClickEventArgs) Handles btnSelectPeriod.Click
		If IsValid Then
			SetPeroids()
			SetObject()
			SetGridObject()
			Session("mnWO") = mnWO
			Session("Sender") = "wfnWODetail_AJAX.aspx"
			Response.Redirect("wfSelectPeriod_Ajax.aspx?BackPage1=wfnWODetail_AJAX.aspx&BackPage=" & Request.QueryString("BackPage"))
			Session.Remove("Sender")
		Else
			upnlValidationsummary.Update()
		End If

	End Sub

	Protected Sub txtModelNo_TextChanged(sender As Object, e As EventArgs)
		mnWO.ModelName = Trim(txtModelNo.Text)
		FillJobTypeCombo(mnWO.MachineID)
		upnlJobType.Update()
	End Sub

	Private Sub btnPlan_Click(sender As Object, e As EventArgs) Handles btnPlan.Click
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If IsValid Then

			If txtPlanDate.Text.ToString = "" Then
				MSGBoxCtrl.Show("Alert!!", "Plan Date required.", "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
			If txtStatusRemark.Text.ToString = "" Then
				MSGBoxCtrl.Show("Alert!!", "Plan Remark required.", "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If

			'Added By Saylee On 4-Mar-2020 For Approval Reject history
			mnWOApproveReject = nWOApproveReject.NewApproval(mnWO.ID)

			If (AppSettings("ClientCode") = "IND") Then
				mnWOApproveReject.Date = CType(txtPlanDate.Text.ToString.Trim + " " + txtPlanDateTime.Text.ToString.Trim, DateTime)
			Else
				mnWOApproveReject.Date = CDate(txtPlanDate.Text.ToString.Trim)
			End If


			mnWOApproveReject.ApprovedRejectStatus = 1
			mnWOApproveReject.Remark = txtStatusRemark.Text
			mnWOApproveReject.WOStatusID = 4

			mWODetail = "Planned Stage " & mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Planned By : "
			Session("WODetailForMarkLog") = mWODetail

			Session("mnWOApproveReject") = mnWOApproveReject
			'**************************************************************

			If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
				tmpText = "Engineering Order"
				''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.StatusCompleted, SIMsgBox.Message_text.StatusCompleted, "<strong>Engineering Order</strong>", MsgBoxStyle.YesNo)
				''msg1.ReplacePage = "wfnWODetail_AJAX.aspx?BackPage=" & Request.QueryString("BackPage")
				''Session("sender") = "WOStatus"
				''msg1.Show()
				Session("IsValid") = IsValid
				MSGBoxCtrl.Show(MSGBox.Message_Title.StatusPlanned, MSGBox.Message_Text.StatusPlanned, "<strong>Engineering Order</strong>", MsgBoxStyle.YesNo, "WOStatus")
				SetObject()
				SetGridObject()
				mnWO.WOStatusID = 4
				Session("mnWO") = mnWO
			Else
				tmpText = "Work Order"
				''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.StatusCompleted, SIMsgBox.Message_text.StatusCompleted, "<strong>Work Order</strong>", MsgBoxStyle.YesNo)
				''msg1.ReplacePage = "wfnWODetail_AJAX.aspx?BackPage=" & Request.QueryString("BackPage")
				''Session("sender") = "WOStatus"
				''msg1.Show()
				MSGBoxCtrl.Show(MSGBox.Message_Title.StatusPlanned, MSGBox.Message_Text.StatusPlanned, "<strong>Work Order</strong>", MsgBoxStyle.YesNo, "WOStatus")
				Session("IsValid") = IsValid

				SetObject()
				SetGridObject()
				mnWO.WOStatusID = 4
				Session("mnWO") = mnWO
			End If


		Else
			upnlValidationsummary.Update()
		End If
	End Sub

	'Modified by Harsh Sugandhi on 29th Jan 2025 FLYPAL-2155
	Private Sub CompleteWorkOrder(sender As Object, e As EventArgs) Handles btnComplete.Click

		If IsValid Then

			If ((txtCloseDate.Text.ToString = "" Or txtClosedBy.Text = "" Or txtStartDate.Text.ToString = "") And
				(Not AppSettings("ClientCode") = "STR") And
				(Not AppSettings("ClientCode") = "SUH")) Or
			   (txtCloseDate.Text.ToString = "" Or txtStartDate.Text.ToString = "") And (AppSettings("ClientCode") = "STR") Then

				MSGBoxCtrl.Show("Alert!",
								"Please enter Starting / Closing Details before completing the Work Order.",
								"",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			ElseIf (txtCloseDate.Text.ToString = "" Or txtClosedBy.Text = "") And (AppSettings("ClientCode") = "SUH") Then 'Suhan added by Saylee on 17-Mar-2022

				MSGBoxCtrl.Show("Alert!",
								"Please enter Closing Details before completing the Work Order.",
								"",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			End If

			If AppSettings("ShowNewWOFlow") = "True" Then

				If txtStatusRemark.Text = "" Then

					MSGBoxCtrl.Show("Alert!",
									"Please enter the PPC Remark before completing a Work Order",
									"",
									MsgBoxStyle.OkOnly,
									"")

					Exit Sub

				End If

			End If

			'Added By Vikrant On 24-May-2019 For New WO
			Dim mIssuedWOTools As nIssuedWOTools
			Dim ToolsPartNos As New StringBuilder
			mIssuedWOTools = nIssuedWOTools.GetnIssuedWOTools(mnWO.ID)

			For i As Integer = 0 To mIssuedWOTools.Count - 1

				If mIssuedWOTools(i).LoanQty > 0 Then
					ToolsPartNos.Append(mIssuedWOTools(i).PartNo + " (" + mIssuedWOTools(i).SerialNo + ")" + ",")
				End If

			Next

			If ToolsPartNos.ToString.TrimEnd(",") <> "" Then

				MSGBoxCtrl.Show("Alert!",
								"Tool(s) " + ToolsPartNos.ToString.TrimEnd(",") + " are issued against Work Order which are not returned yet.",
								"Please return back Tool(s) before completing a Work Order",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			End If

			'Added By Saylee On 4-Mar-2020 For Approval Reject history
			mnWOApproveReject = nWOApproveReject.NewApproval(mnWO.ID)

			If (AppSettings("ClientCode") = "IND") Then
				mnWOApproveReject.Date = CType(DateTime.Now.ToString.Trim, DateTime)
			Else
				mnWOApproveReject.Date = CDate(DateTime.Now.ToString.Trim)
			End If

			mnWOApproveReject.ApprovedRejectStatus = 1
			mnWOApproveReject.Remark = txtStatusRemark.Text
			mnWOApproveReject.WOStatusID = 3
			mWODetail = "PPC Completion Stage " & mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + "PPC Completion By : "
			Session("WODetailForMarkLog") = mWODetail
			Session("mnWOApproveReject") = mnWOApproveReject
			'**************************************************************

			If (AppSettings("ClientCode") IsNot Nothing) AndAlso
			   (AppSettings("ClientCode") = "TAAL" Or
				AppSettings("ClientCode") = "GlobalJet") Then

				tmpText = "Engineering Order"
				Session("IsValid") = IsValid
				MSGBoxCtrl.Show(MSGBox.Message_Title.StatusCompleted,
								MSGBox.Message_Text.StatusCompleted,
								"<strong>Engineering Order</strong>",
								MsgBoxStyle.YesNo,
								"WOStatus")

				SetObject()
				SetGridObject()
				mnWO.WOStatusID = 3
				mnWO.IsClosed = True
				mnWO.ClosedBy = Trim(txtClosedBy.Text)
				Session("mnWO") = mnWO

			Else

				tmpText = "Work Order"
				MSGBoxCtrl.Show(MSGBox.Message_Title.StatusCompleted,
								MSGBox.Message_Text.StatusCompleted,
								"<strong>Work Order</strong>",
								MsgBoxStyle.YesNo,
								"WOStatus")

				Session("IsValid") = IsValid

				SetObject()
				SetGridObject()
				mnWO.WOStatusID = 3
				mnWO.IsClosed = True
				mnWO.ClosedBy = Trim(txtClosedBy.Text)

				If mnWO.WOCompletedDateTimeFormatted.ToString = "" Then
					mnWO.WOCompletedDateTime = DateTime.Now.ToString
				End If

				Session("mnWO") = mnWO
			End If

		Else
			upnlValidationsummary.Update()
		End If

	End Sub

	Private Sub AuthorizeWO(sender As Object, e As EventArgs) Handles btnAuthorize.Click

		Try

			If (Not IsInRole(Rights.Authorized) And Not mnWO.IsNew) Then

				SetObject()
				SetGridObject()
				SetSession()

				mWODetail = mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy

				MarkLog(Action.Authorize,
						"Work Order",
						User.Identity.Name & " is not Authorized User to Submit " & mWODetail,
						ErrorType.HandledError,
						Guid.Empty,
						EventLogID)

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"Authorization")

				Exit Sub

			End If

			If IsValid Then

				Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
				Dim mEmployee As Employee

				If Not mUser.EmployeeID.Equals(Guid.Empty) Then
					mEmployee = Employee.GetEmployee(mUser.EmployeeID)
				End If

				If (AppSettings("ClientCode") IsNot Nothing) AndAlso
				   (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then

					tmpText = "Engineering Order"
					Session("IsValid") = IsValid
					mnWO.StatusID = 2
					mnWO.AuthorizedBy = User.Identity.Name

					If Not mUser.EmployeeID.Equals(Guid.Empty) Then

						mnWO.AuthorizedBy = mEmployee.Name
						mnWO.EmployeeID = mEmployee.ID
						mnWO.EmployeeDesignationName = mEmployee.DesignationName 'Added by Saylee on 7-Jan-2019
						mnWO.IsDigitalSignatureAdded = mEmployee.IsDigitalSignatureAdded 'Added by Saylee on 7-Jan-2019

						If mnWO.IsDigitalSignatureAdded = True Then

							If mFileAttachnWO Is Nothing Then

								mFileAttachnWO = FileAttach.NewAttachment(mnWO.ID, "DigitalSignatureWO")

							Else

								mFileAttachnWO = FileAttach.GetAttachment(mnWO.ID, "DigitalSignatureWO")

							End If

							mFileAttach = FileAttach.GetAttachment(mnWO.EmployeeID, , "DigitalSignature")
							mFileAttachnWO.Extension = mFileAttach.Extension
							mFileAttachnWO.Size = mFileAttach.Size
							mFileAttachnWO.ImageFile = mFileAttach.ImageFile
							mFileAttachnWO.FileName = "DigitalSignatureWO"
							Session("mFileAttachnWO") = mFileAttachnWO

						End If

					End If

					Session("mnWO") = mnWO
					UpdatePanels()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Submission,
									MSGBox.Message_Text.Submission,
									"<strong>Engineering Order</strong>",
									MsgBoxStyle.YesNo,
									"Status")

				Else

					tmpText = "Engineering Order"
					Session("IsValid") = IsValid
					mnWO.StatusID = 2
					mnWO.AuthorizedBy = User.Identity.Name

					If Not mUser.EmployeeID.Equals(Guid.Empty) Then

						mnWO.AuthorizedBy = mEmployee.Name
						mnWO.EmployeeID = mEmployee.ID
						mnWO.EmployeeDesignationName = mEmployee.DesignationName 'Added by Saylee on 7-Jan-2019
						mnWO.IsDigitalSignatureAdded = mEmployee.IsDigitalSignatureAdded 'Added by Saylee on 7-Jan-2019

						If mnWO.IsDigitalSignatureAdded = True Then

							If mFileAttachnWO Is Nothing Then

								mFileAttachnWO = FileAttach.NewAttachment(mnWO.ID, "DigitalSignatureWO")

							Else

								mFileAttachnWO = FileAttach.GetAttachment(mnWO.ID, "DigitalSignatureWO")

							End If

							mFileAttach = FileAttach.GetAttachment(mnWO.EmployeeID, , "DigitalSignature")
							mFileAttachnWO.Extension = mFileAttach.Extension
							mFileAttachnWO.Size = mFileAttach.Size
							mFileAttachnWO.ImageFile = mFileAttach.ImageFile
							mFileAttachnWO.FileName = "DigitalSignatureWO"
							Session("mFileAttachnWO") = mFileAttachnWO

						End If

					End If

					'Added By Saylee On 4-Mar-2020 For Approval Reject history
					mnWOApproveReject = nWOApproveReject.NewApproval(mnWO.ID)

					If (AppSettings("ClientCode") = "IND") Then
						mnWOApproveReject.Date = CType(DateTime.Now.ToString.Trim, DateTime)
					Else
						mnWOApproveReject.Date = CDate(DateTime.Now.ToString.Trim)
					End If

					mnWOApproveReject.ApprovedRejectStatus = 1
					mnWOApproveReject.Remark = txtStatusRemark.Text
					mnWOApproveReject.WOStatusID = 2
					mWODetail = "Authorization Stage " & mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + "Authorized By : "
					Session("WODetailForMarkLog") = mWODetail
					Session("mnWOApproveReject") = mnWOApproveReject
					'**************************************************************

					Session("mnWO") = mnWO
					lnkCreateRequisition.DataBind()
					UpdatePanels()

					If cmbServiceProvider.SelectedIndex = 0 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.Submission,
										MSGBox.Message_Text.Submission,
										"<strong>Work Order To Self</strong>",
										MsgBoxStyle.YesNo,
										"Status")

					Else

						MSGBoxCtrl.Show(MSGBox.Message_Title.Submission,
										MSGBox.Message_Text.Submission,
										"<strong>Work Order To " + cmbServiceProvider.SelectedItem.Text + "</strong>",
										MsgBoxStyle.YesNo,
										"Status")

					End If

				End If

			Else
				upnlValidationsummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
		If (Not IsInRole(Rights.Authorized) And Not mnWO.IsNew) Then
			SetObject()
			SetGridObject()
			SetSession()
			'MarkLog(Action.Save, "Work Order", "Not Authorized User", ErrorType.HandledError, Guid.Empty)
			mWODetail = mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy
			MarkLog(Action.Authorize, "Work Order", User.Identity.Name & " is not Authorized User to Cancel " & mWODetail, ErrorType.HandledError, Guid.Empty, EventLogID)
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		If IsValid Then
			Dim IsInUse As IsInUse = (IsInUse.GetIsInUsenWOINIssue(mnWO.ID))
			If IsInUse.IsInUse Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.Cancel, MSGBox.Message_Text.Cancel, "<Strong>Work Order,It is used in Issue</Strong>", MsgBoxStyle.OkOnly, "Status")

				mnWO.StatusID = 4
				Session("mnWO") = mnWO
				Exit Sub
			End If

			Session("IsValid") = IsValid
			MSGBoxCtrl.Show(MSGBox.Message_Title.StatusCanceled, MSGBox.Message_Text.StatusCanceled, "<strong>Work Order</strong>", MsgBoxStyle.YesNo, "Status")

			Session("PrevStatusID") = mnWO.StatusID 'Added by Shital on 09-Oct-2019
			PrevStatusID = Session("PrevStatusID")

			mnWO.StatusID = 4
			Session("mnWO") = mnWO
		Else
			upnlValidationsummary.Update()
		End If
	End Sub

	Private Sub SaveWODetails(sender As Object, e As EventArgs) Handles btnSave.Click

		Try

			If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then

				SetObject()
				SetGridObject()
				SetSession()
				mWODetail = mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy

				MarkLog(Action.Save,
						"Work Order",
						User.Identity.Name & " is not Authorized User to save " & mWODetail,
						ErrorType.HandledError,
						Guid.Empty,
						EventLogID)

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"Authorization")

				Exit Sub

			End If

			If Not IsValid Then upnlValidationsummary.Update() : Exit Sub

			If Page.IsValid Then

				'For BRD
				If MaxWOExceeded() Then
					Exit Sub
				End If
				'End

				If Save() Then

					mnWO = WOHelper.FetchWO(ID:=mnWO.ID)
					Session("mnWO") = mnWO

					DataFieldBind()
					SaveAttachment() 'Added by Saylee on 12-Oct-2018, ALL11102018
					SetPage()
					SetGrid()
					SetNRCGrid() 'Added By Vikrant For WO NRC
					ControlVisibility()
					UpdatePanels()

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

	Private Sub dgWOJobs_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgWOJobs.RowCommand
		'Page.Validate()
		'If Not IsValid Then upnlValidationsummary.Update() : Exit Sub
		'If Not CustomValidateObject() Then upnlValidationsummary.Update() : Exit Sub
		Select Case e.CommandName
			Case "EditRec"
				Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobs.PageSize * dgWOJobs.PageIndex
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetGridObject()
					SetSession()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Session("Edit") = True
				SetObject()
				SetGridObject()
				mnWO.WOJobs.CurrentIndex = Index
				Session("WOJobTypeID") = mnWO.WOJobs.CurrentItem.WOJobTypeID
				Session("mnWO") = mnWO
				'Added By Prashant 20-Jan-2011
				mnWOClone = mnWO.Clone
				Session("mnWOClone") = mnWOClone
				'-----------------------------
				'Added By Vikrant on 30-Jun-2021 For ALL30062021 
				If mnWO.WOJobs.CurrentItem.WOJobStatusID = 2 And mnWO.StatusID <> 3 And AppSettings("ShowNewWOFlow") <> "True" And Not IsInRole(Rights.Completed) Then
					Session("ToDisbleJobControlsAsCompletedRightNotGiven") = "True"
				End If
				'End
				'Response.Redirect("wfnWOJobDetail.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
				' Response.Redirect("wfnWOJobDetail_AJAX.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
				Response.Redirect("wfnWOJobDetail.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
			Case "DeleteRec"
				Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobs.PageSize * dgWOJobs.PageIndex
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
					SetSession()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If

				DeleteJobRecord(Index)
			Case "ViewRec"
				'Added by Saylee on 7-Mar-2014 for ALL07032014

				If (Not IsInRole(Rights.View)) Then

					SetGridObject()
					SetSession()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

					Exit Sub

				End If

				Dim mFileAttachments As New FileAttachments

				Dim mID As Guid
				Dim Idx As Integer = CInt(e.CommandArgument) + dgWOJobs.PageSize * dgWOJobs.PageIndex
				mnWO.WOJobs.CurrentIndex = Idx
				mID = mnWO.WOJobs.CurrentItem.ID
				mFileAttachments = FileAttachments.GetChildFileAttachments(mID)

				Dim AttachmentCount As Integer = mFileAttachments.Count
				DataFieldBind()
				SetGrid()

				Session("mnWO") = mnWO

				If AttachmentCount > 1 Then

					Session("mFileAttachments") = mFileAttachments
					Session("TransactionNameMarkLog") = "Work Order" 'used for marklog

					If mnWO.WOJobs.CurrentItem.TaskCardNo <> "" Then
						Session("TransactionName") = "Task Card No. : "
						Session("TransactionDetails") = mnWO.WOJobs.CurrentItem.TaskCardNo
					End If

					ControlVisibility()
					ScriptManager.RegisterStartupScript(Me, [GetType], "OpenAttachWindow", "OpenAttachWindow();", True)

				Else

					Dim mFileAttach As FileAttach
					Dim No As New Random
					Dim StrName As String = "abc" & No.Next.ToString

					mFileAttach = FileAttach.GetAttachment(mID, , mnWO.WOJobs.CurrentItem.FileAttachments(0).FileName)

					If mFileAttach.Size > 0 Then

						Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
						Dim fs As FileStream

						If File.Exists(AppSettings("DOCPath")) = False Then

							'Delete File if exist
							File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
							' Create the file.
							fs = File.Create(path)
							'' Add some information to the file.
							fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
							fs.Close()
							Session("DOCPath") = path
							ScriptManager.RegisterStartupScript(Me, [GetType], "openFilel", "openFilel();", True)
							Dim Detail As String = "Work Order Attachment( " + mFileAttach.FileName + ") viewed by  " + User.Identity.Name
							MarkLog(Action.View, "Work Order", Detail, ErrorType.HandledError, mID, EventLogID)

						End If

					End If

				End If

			Case "TaskCardsRec" 'Added by Saylee on 29-May-2019

				Dim Index As Integer '= CInt(e.CommandArgument) + dgWOJobs.PageSize * dgWOJobs.PageIndex
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Index = gvr.RowIndex
				'Added by Saylee on 7-Mar-2014 for ALL07032014

				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then

					SetGridObject()
					SetSession()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

					Exit Sub

				End If

				Session("Edit") = True
				SetObject()
				SetGridObject()
				mnWO.WOJobs.CurrentIndex = Index
				Session("WOJobTypeID") = mnWO.WOJobs.CurrentItem.WOJobTypeID


				Session("mnWO") = mnWO
				mnWOClone = mnWO.Clone
				Session("mnWOClone") = mnWOClone
				Session("mnWOJob") = mnWO.WOJobs.CurrentItem
				If mnWO.WOJobs.CurrentItem.WOJobTasks.Count > 0 Then
					ScriptManager.RegisterStartupScript(Me, [GetType], "OpenJobTaskListWindow", "OpenJobTaskListWindow();", True)
				Else

					If mnWO.WOJobs.CurrentItem.WOJobTypeID = 1 Then 'For UnScheduled Jobs

						Session("IsOpenFrom") = "WorkOrder"
						Session("AddTaskCards") = "False"
						Session.Remove("mSelectTaskCardList")
						Session.Remove("mTaskCardNo")
						Session.Remove("mInspInterval")
						Session.Remove("mModelID")
						ScriptManager.RegisterStartupScript(Me, [GetType], "OpenToAddSelectTasks", "OpenToAddSelectTasks();", True)

					Else
						Session("mIndex") = "-1"
						ScriptManager.RegisterStartupScript(Me, [GetType], "OpenToAddJobTaskDetail", "OpenToAddJobTaskDetail();", True)
					End If

				End If

			Case "InstallationRemovalRec" 'Added by Saylee on 29-May-2019
				'Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobs.PageSize * dgWOJobs.PageIndex
				Dim Index As Integer
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Index = gvr.RowIndex
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetGridObject()
					SetSession()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Session("Edit") = False
				SetObject()
				SetGridObject()
				mnWO.WOJobs.CurrentIndex = Index
				Session("WOJobTypeID") = mnWO.WOJobs.CurrentItem.WOJobTypeID


				Session("mnWO") = mnWO
				mnWOClone = mnWO.Clone
				Session("mnWOClone") = mnWOClone
				Session("mnWOJob") = mnWO.WOJobs.CurrentItem
				Session("mIndex") = "-1"
				ScriptManager.RegisterStartupScript(Me, [GetType], "OpenToAddJobCompDetail", "OpenToAddJobCompDetail();", True)
			Case "DesignationAllocationRec"
				'Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobs.PageSize * dgWOJobs.PageIndex
				Dim Index As Integer
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Index = gvr.RowIndex
				If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
					SetSession()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If

				SetObject()
				Session("mDesignationAllocationEdit") = False
				'Session("WOJobTypeID") = mWOJobTypeID
				mnWO.WOJobs.CurrentIndex = Index
				Session("mnWOJob") = mnWO.WOJobs.CurrentItem 'Added By Vikrant For WO NRC

				ScriptManager.RegisterStartupScript(Me, [GetType], "OpenToAddDesignaionAllocation", "OpenToAddDesignaionAllocation();", True)
				'Added By Vikrant On 24-May-2019 For New WO
			Case "SparesAddRemove"
				'Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobs.PageSize * dgWOJobs.PageIndex
				Dim Index As Integer
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Index = gvr.RowIndex
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetGridObject()
					SetSession()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Session("Edit") = True
				SetObject()
				SetGridObject()
				mnWO.WOJobs.CurrentIndex = Index
				Session("WOJobTypeID") = mnWO.WOJobs.CurrentItem.WOJobTypeID

				Session("mnWO") = mnWO
				mnWOClone = mnWO.Clone
				Session("mnWOClone") = mnWOClone
				Session("mnWOJob") = mnWO.WOJobs.CurrentItem
				ScriptManager.RegisterStartupScript(Me, [GetType], "OpenToAddJobSpareDetail", "OpenToAddJobSpareDetail();", True)
			Case "NRCRec"
				'Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobs.PageSize * dgWOJobs.PageIndex
				Dim Index As Integer
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Index = gvr.RowIndex
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetGridObject()
					SetSession()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Session("Edit") = True
				SetObject()
				SetGridObject()
				mnWO.WOJobs.CurrentIndex = Index
				Session("WOJobTypeID") = mnWO.WOJobs.CurrentItem.WOJobTypeID
				Session("mnWO") = mnWO
				mnWOClone = mnWO.Clone
				Session("mnWOClone") = mnWOClone
				Session("mnWOJob") = mnWO.WOJobs.CurrentItem
				ScriptManager.RegisterStartupScript(Me, [GetType], "OpenToAddSelectNRC", "OpenToAddSelectNRC();", True)
			Case "PrintWithTaskCardsRec"
				Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobs.PageSize * dgWOJobs.PageIndex
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.Print)) Then
					SetGridObject()
					SetSession()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Session("Edit") = True
				SetObject()
				SetGridObject()
				mnWO.WOJobs.CurrentIndex = Index

				PrintWithPDF(True, Index)
		End Select
	End Sub

	Private Sub dgWOTools_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgWOTools.RowCommand
		If Not CustomValidateObject() Then upnlValidationsummary.Update() : Exit Sub
		Select Case e.CommandName
			Case "EditRec"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetGridObject()
					SetSession()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Dim Index As Integer = CInt(e.CommandArgument) + dgWOTools.PageSize * dgWOTools.PageIndex
				Session("Edit") = True
				SetObject()
				SetGridObject()
				mnWO.WOTools.CurrentIndex = Index
				If mnWO.WOTools.CurrentItem.WOIssuedToolsCount > 0 Then
					MSGBoxCtrl.Show("Alert!", "You cannot edit this record, as Issue against this part has been already done!", "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				Session("mnWO") = mnWO
				Session("ComeForEdit") = "ComeForEdit"
				'Response.Redirect("wfnWOTool_AJAX.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
				ScriptManager.RegisterStartupScript(Me, [GetType], "OpenWOTool", "OpenWOTool();", True)
			Case "DeleteRec"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
					SetSession()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If

				Dim Index As Integer = CInt(e.CommandArgument) + dgWOTools.PageSize * dgWOTools.PageIndex

				DeleteToolRecord(Index)
			Case "View"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View)) Then
					'SetObject()
					SetGridObject()
					SetSession()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'----------------------------------------------------------------------
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Dim rowIndex As Integer = gvr.RowIndex
				Dim Index As Integer
				Index = rowIndex
				'----------------------------------------------------------------------
				mnWO.WOJobs.CurrentIndex = Index
				If mnWO.WOJobs.CurrentItem.Size = 0 Then
					'Dim msg1 As New SIMsgBox(Page, "Attachment!", "No Attach File Present.", "", MsgBoxStyle.OKOnly)
					'msg1.ReplacePage = "wfnWODetail_AJAX.aspx?BackPage=" & Request.QueryString("BackPage")
					'msg1.Show()
					'Exit Sub
				Else
					Dim path As String = AppSettings("DOCPath") & StrName & mnWO.WOJobs.CurrentItem.FileExtension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						File.Delete(AppSettings("DOCPath") & StrName & mnWO.WOJobs.CurrentItem.FileExtension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mnWO.WOJobs.CurrentItem.AttachFileName, 0, mnWO.WOJobs.CurrentItem.AttachFileName.Length)
						fs.Close()
						Session("DOCPath") = path
						Dim Str As String
						'Str = "<script language=Javascript>openFile();</script>"
						Str = "openFilel"
						'ClientScript.RegisterStartupScript([GetType], "openFilel", Str)
						ScriptManager.RegisterStartupScript(Me, [GetType], "openFilel", Str, True)
					End If
				End If
		End Select
	End Sub

	Private Sub lnkIssuedTools_Click(sender As Object, e As EventArgs) Handles lnkIssuedTools.Click

		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		SetObject()
		SetGridObject()
		Session("mnWO") = mnWO
		Session("WOID") = mnWO.ID.ToString
		ScriptManager.RegisterStartupScript(Me, [GetType], "OpenIssuedWOTools", "OpenIssuedWOTools();", True)
		' Response.Redirect("wfnIssuedWOTools_AJAX.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&WOID=" & mnWO.ID.ToString)
	End Sub

	Private Sub lnkIssuedSpares_Click(sender As Object, e As EventArgs) Handles lnkIssuedSpares.Click
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		SetObject()
		SetGridObject()
		Session("mnWO") = mnWO
		Session("mWOID") = mnWO.ID
		' Response.Redirect("wfnIssuedWOSpares_AJAX.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&WOID=" & mnWO.ID.ToString)
		ScriptManager.RegisterStartupScript(Me, [GetType], "OpenIssuedWOSpares", "OpenIssuedWOSpares();", True)
	End Sub

	'12-Jun-2019
	Private Sub lnkViewIndent_Click(sender As Object, e As EventArgs) Handles lnkViewIndent.Click
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		SetObject()
		SetGridObject()
		Session("mnWO") = mnWO
		Session("mWOID") = mnWO.ID
		ScriptManager.RegisterClientScriptBlock(Me, [GetType], "RequisitionView", "RequisitionView();", True)
	End Sub
	'End

	Private Sub lnkFuelDetail_Click(sender As Object, e As EventArgs) Handles lnkFuelDetail.Click
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If


		SetObject()
		SetGridObject()
		Session("mnWO") = mnWO
		If Not IsValid Then upnlValidationsummary.Update() : Exit Sub

		If cmbLogList.SelectedIndex <= 0 Then
			''Dim msg1 As New SIMsgBox(Page, "Alert!", "Please select the Log from the List", "", MsgBoxStyle.OKOnly)
			''msg1.ReplacePage = "wfnWODetail_AJAX.aspx?BackPage=" & Request.QueryString("BackPage")
			''msg1.Show()
			MSGBoxCtrl.Show("Alert!", "Please select the Log from the List", "", MsgBoxStyle.OkOnly, "")

			Exit Sub
		End If
		Dim mLog As Log
		Dim mMachine As Machine
		mLog = Log.GetLog(mnWO.LogID)
		mMachine = Machine.GetMachine(mLog.MachineID)
		Session("mLog") = mLog
		Session("WOStatusID") = mnWO.WOStatusID
		Session("StatusIDForWO") = mnWO.StatusID
		Session("mMachine") = mMachine
		Session("OpenFromWO") = True
		Session("mOpenFromLogFuelNew") = False

		' Response.Redirect("wfLogFuelOil_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfnWODetail_AJAX.aspx" & "&ChildPage=wfnWODetail_AJAX.aspx")
		ScriptManager.RegisterStartupScript(Me, [GetType], "OpenLogFuelOilWindow", "OpenLogFuelOilWindow();", True)
	End Sub

	Private Sub chkMaintenance_CheckedChanged(sender As Object, e As EventArgs) Handles chkMaintenance.CheckedChanged
		If chkMaintenance.Checked = True Then
			cmbAircraftList.Enabled = (mnWO.WOJobs.Count = 0) And chkMaintenance.Checked
			btnSelectPeriod.Enabled = False
		Else
			cmbAircraftList.Enabled = False
			cmbAircraftList.SelectedIndex = 0
			txtRegNo.Text = ""
			txtModelNo.Text = ""
			txtSerialNo.Text = ""

			If (AppSettings("ClientCode") = "RAL" Or AppSettings("ClientCode") = "ADeccan") Then
				txtText.Text = ""
				UpnlWODet.Update()
			End If
			cmbCustomerList.SelectedIndex = 0
			cmbHourTypeList.SelectedIndex = 0
			btnSelectPeriod.Enabled = True
			txtRegNo.ReadOnly = False
			txtModelNo.ReadOnly = False
			txtSerialNo.ReadOnly = False
			mnWO.MachineID = Guid.Empty
			cmbLogList.ClearSelection()
			If mnWO.WOPeriods.Count <> 0 Then
				For i As Integer = mnWO.WOPeriods.Count - 1 To 0 Step -1
					mnWO.WOPeriods.RemoveAt(i)
				Next
			End If
			'cmbLogList.SelectedIndex = 0
			cmbLogList.Enabled = False

			dgCurrentPeriodValue.DataSource = mnWO.WOPeriods
			dgCurrentPeriodValue.DataBind()

			FillJobTypeCombo(Guid.Empty)
		End If

		If cmbAircraftList.SelectedIndex > 0 And txtStartDate.Text.ToString <> "" Then
			cmbLogList.Enabled = True
		Else
			cmbLogList.Enabled = False
		End If


		upnlStartDetails.Update()
		upnlAirframePeriods.Update()
		upnlJobType.Update()
	End Sub

	Private Sub dgCurrentPeriodValue_ItemCommand(source As Object, e As GridViewCommandEventArgs) Handles dgCurrentPeriodValue.RowCommand

		Dim Index As Integer = CType(e.CommandArgument, Integer) 'e.Row.RowIndex + dgCurrentPeriodValue.PageIndex * dgCurrentPeriodValue.PageSize
		Select Case e.CommandName
			Case "DeleteRecord"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
					SetSession()
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If

				mnWO.WOPeriods.RemoveAt(Index)
				dgCurrentPeriodValue.DataSource = mnWO.WOPeriods
				dgCurrentPeriodValue.DataBind()
				Session("mnWO") = mnWO
		End Select
	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

		Session.Remove("ReqURLFromWO")
		'Added By Prashant 16-Aug-2019
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"on close",
												"CallParentCallback();",
												True)
			Exit Sub

		End If
		Session("mtmpTransTypeID") = mtmpTransTypeID
		'End of Added By Prashant 16-Aug-2019
		If mnWO.IsDirty Then

			Session("IsValid") = "True"

			MSGBoxCtrl.Show(MSGBox.Message_Title.CloseConfirm,
							MSGBox.Message_Text.Save,
							"",
							MsgBoxStyle.YesNo,
							"Close")

		Else

			mnWO = Session("mnWO")
			SetObject()
			Session("mnWO") = mnWO
			Session.Remove("mnWO")
			Session.Remove("mFileAttach")
			Session.Remove("mReportLogRegister")
			Session.Remove("mnWOApproveReject")
			Session.Remove("IsWOForRemovedOrSpareComp")
			Session.Remove("IsWOForRemovedOrSpareAssembly")

			mWODetail = mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy + IIf(Not mnWO.MachineID.Equals(Guid.Empty), " Aircraft : " + mnWO.RegNo, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
			MarkLog(Action.Close, "Work Order", mWODetail, ErrorType.NoError, mnWO.ID, EventLogID)
			'Added By Vikrant on 14-Jun-2018 For ALL14062018
			Dim URLFromDueReportPreview As Stack = CType(Session("URLFromDueReportPreview"), Stack)
			If URLFromDueReportPreview IsNot Nothing Then
				If URLFromDueReportPreview.Count > 0 Then
					If Session("wfSearchCriteriaForMaintenanceAdviceFromQC") = "wfSearchCriteriaForMaintenanceAdviceFromQC" Then
						Session("MiddleFrame") = "wfSearchCriteriaForMaintenanceAdviceFromQC_Ajax.aspx?DueType=" & Session("DueType").ToString
					ElseIf Session("wfSearchCriteriaForDueWithAircraftSelection") = "wfSearchCriteriaForDueWithAircraftSelection" Then
						Session("MiddleFrame") = "wfSearchCriteriaForDueWithAircraftSelection.aspx?DueType=" & Session("DueType").ToString
					ElseIf Session("wfMELSnagCorrectiveActionNew_AJAX") = "wfMELSnagCorrectiveActionNew_AJAX" Then

						Dim mMELSnagCorrectiveAction As MELSnagCorrectiveAction
						mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")
						mMELSnagCorrectiveAction.IsWOCreated = True
						mMELSnagCorrectiveAction.WONumber = mnWO.WONumber & vbCrLf & mnWO.WODateFormatted
						mMELSnagCorrectiveAction.WOID = mnWO.ID
						Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction

						Session("MiddleFrame") = "wfMELSnagCorrectiveActionListNew_AJAX.aspx?"
					ElseIf Session("wfLogDefectActionList_Ajax") = "wfLogDefectActionList_Ajax" Then
						Session("MiddleFrame") = "wfLogList.aspx"
						If Session("LogFromMEL") IsNot Nothing Then
							Session("LogFromMEL") = Log.GetLog(CType(Session("LogFromMEL"), Log).ID)
						End If
						Session.Remove("mMELSnagCorrectiveAction")
						mopenas = Request.QueryString("Type")
						If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
							ScriptManager.RegisterStartupScript(Me, [GetType], "on close", "CallParentCallback();", True)
							Exit Sub
						End If
					ElseIf Session("wfDueJobPlanning_Ajax") = "wfDueJobPlanning_Ajax" Then
						Session("MiddleFrame") = "wfDueJobPlanningList_Ajax.aspx?"
					ElseIf Session("wfProject_Ajax") = "wfProject_Ajax" Then 'Added By Prashant on 3-May-2024
						Session("MiddleFrame") = "wfProjectList_Ajax.aspx?TransTypeID=" & Session("TransTypeID").ToString
						mopenas = Request.QueryString("Type")
						'If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
						'    ScriptManager.RegisterStartupScript(Me, [GetType], "on close", "CallParentCallback();", True)
						'    Exit Sub
						'End If
						ScriptManager.RegisterStartupScript(Me, [GetType], "on close", "CallParentCallback();", True)
						Exit Sub
					Else
						'Session("MiddleFrame") = "wfSearchCriteriaForDue_Ajax.aspx?DueType=" & Session("DueType").ToString
						If Session("DueType") Is Nothing Then
							'Do nothing
						Else
							Session("MiddleFrame") = "wfSearchCriteriaForDue_Ajax.aspx?DueType=" & Session("DueType").ToString
						End If
					End If
					Session.Remove("URLFromDueReportPreview")
					Response.Redirect(URLFromDueReportPreview.Peek.ToString)
					Exit Sub
				End If
			End If
			'End
			Response.Redirect("index.aspx")

		End If

	End Sub

	Private Sub rdbIsThirdParty_CheckedChanged(sender As Object, e As EventArgs) Handles rdbIsThirdParty.CheckedChanged
		If rdbIsThirdParty.Checked Then
			cmbAircraftList.SelectedIndex = 0
			txtRegNo.Text = ""
			txtModelNo.Text = ""
			txtSerialNo.Text = ""
		End If
	End Sub

	Private Sub cmbLogList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLogList.SelectedIndexChanged
		If cmbAircraftList.SelectedIndex > 0 Then ''Commented by Saylee on 21-Jul-2022,  txtStartDate.Text.ToString <> "" And  ''
			SetLog()
			'txtLogNo.Text = IIf(cmbLogList.SelectedIndex = 0, "", cmbLogList.SelectedItem.Text) 'Added by Prashant on 15-Apr-2019 LAMA15042019
			txtLogNo.Text = IIf(cmbLogList.SelectedValue = "(SELECT)", "", cmbLogList.SelectedItem.Text)
		End If
		cmbLogList.Enabled = True
		upnlAirframePeriods.Update()
	End Sub

	Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		' AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
		MessageBoxResult()
	End Sub

	Private Sub hdnBtnFileUpload_Click(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click
		AttachMyFile()
		upnlWOAttachment.Update()
	End Sub

	Private Sub btnComplyJobs_Click(sender As Object, e As EventArgs) Handles btnComplyJobs.Click

		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If AppSettings("ShowNewWOFlow") = "True" Then ' If AppSettings("ClientCode") = "IND" Then
			If txtStatusRemark.Text = "" Then
				MSGBoxCtrl.Show("Alert!", "Please enter the CAMO Update Remark", "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
		End If


		Session("IsWOForRemovedOrSpareAssembly") = False
		Session("IsWOForRemovedOrSpareComp") = False
		If mnWO.TransTypeID = Trans.SpareAssemblyWO Then
			Session("IsWOForRemovedOrSpareAssembly") = mRemovedAssemblyListForCombo(New Guid(cmbAssembly.SelectedValue.ToString)).IsSpareAssembly
		ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then
			Session("IsWOForRemovedOrSpareComp") = mRemovedCompListForCombo(New Guid(cmbCompList.SelectedValue.ToString)).IsSpareComp
		End If



		'Added By Saylee On 4-Mar-2020 For Approval Reject history
		mnWOApproveReject = nWOApproveReject.NewApproval(mnWO.ID)

		If (AppSettings("ClientCode") = "IND") Then
			mnWOApproveReject.Date = CType(DateTime.Now.ToString.Trim, DateTime)
		Else
			mnWOApproveReject.Date = CDate(DateTime.Now.ToString.Trim)
		End If
		mnWOApproveReject.ApprovedRejectStatus = 1
		mnWOApproveReject.Remark = txtStatusRemark.Text
		mnWOApproveReject.WOStatusID = 8

		mWODetail = "Compliance Stage " & mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Complied By : "
		Session("WODetailForMarkLog") = mWODetail

		Session("mnWOApproveReject") = mnWOApproveReject
		'**************************************************************


		mnWO.CAMOUpdateRemark = txtStatusRemark.Text
		Session("mnWO") = mnWO

		Session.Remove("IsValid")
		Response.Redirect("wfnWOForMulticompliance_Ajax.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
	End Sub

	'Added By Vikrant For WO NRC
	Private Sub dgWONRC_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgWONRC.RowCommand
		If Not CustomValidateObject() Then upnlValidationsummary.Update() : Exit Sub
		Select Case e.CommandName
			Case "EditRec"
				Dim Index As Integer = CInt(e.CommandArgument) + dgWONRC.PageSize * dgWONRC.PageIndex
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Session("Edit") = True
				SetObject()
				SetGridObject()
				mnWO.WONRCJobs.CurrentIndex = Index
				Session("WOJobTypeID") = mnWO.WONRCJobs.CurrentItem.WOJobTypeID
				Session.Remove("ActiveNRCDetailsTabIndex")
				Session("mnWO") = mnWO
				'Added By Prashant 20-Jan-2011
				mnWOClone = mnWO.Clone
				Session("mnWOClone") = mnWOClone
				Response.Redirect("wfnWONRC.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))

			Case "DeleteRec"
				Dim Index As Integer = CInt(e.CommandArgument) + dgWONRC.PageSize * dgWONRC.PageIndex
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				DeleteWONRC(Index)
			Case "View"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View)) Then
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'----------------------------------------------------------------------
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Dim rowIndex As Integer = gvr.RowIndex
				Dim Index As Integer
				Index = rowIndex
				'----------------------------------------------------------------------
				mnWO.WONRCJobs.CurrentIndex = Index

				Dim mFileJobAttach As FileAttach
				If mnWO.WONRCJobs.CurrentItem.IsAttachmentAdded Then
					mFileJobAttach = FileAttach.GetAttachment(mnWO.WONRCJobs.CurrentItem.ID) 'Sort = 2 : Removal
					Session("mFileAttach") = mFileJobAttach
				End If

				If mFileJobAttach.Size > 0 Then

					Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileJobAttach.Extension
					Dim fs As FileStream

					If File.Exists(AppSettings("DOCPath")) = False Then

						'Delete File if exist
						File.Delete(AppSettings("DOCPath") & StrName & mFileJobAttach.Extension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mFileJobAttach.ImageFile, 0, mFileJobAttach.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, [GetType], "openFilel", "openFilel();", True)

					End If

				End If
				'======================== Ajay 20-sep-2022 Start====================
			Case "TaskCards"
				Dim Index As Integer '= CInt(e.CommandArgument) + dgWOJobs.PageSize * dgWOJobs.PageIndex
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Index = gvr.RowIndex
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetGridObject()
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Session("Edit") = True
				SetObject()
				SetGridObject()
				mnWO.WONRCJobs.CurrentIndex = Index
				Session("WOJobTypeID") = mnWO.WONRCJobs.CurrentItem.WOJobTypeID

				Session("mnWO") = mnWO
				mnWOClone = mnWO.Clone
				Session("mnWOClone") = mnWOClone
				Session("mnWOJob") = mnWO.WONRCJobs.CurrentItem
				If mnWO.WONRCJobs.CurrentItem.WOJobTasks.Count > 0 Then
					ScriptManager.RegisterStartupScript(Me, [GetType], "OpenJobTaskListWindow", "OpenJobTaskListWindow();", True)
				Else
					If mnWO.WONRCJobs.CurrentItem.WOJobTypeID = 1 Then 'For UnScheduled Jobs
						Session("IsOpenFrom") = "WorkOrder"
						Session("AddTaskCards") = "False"
						Session.Remove("mSelectTaskCardList")
						Session.Remove("mTaskCardNo")
						Session.Remove("mInspInterval")
						Session.Remove("mModelID")
						ScriptManager.RegisterStartupScript(Me, [GetType], "OpenToAddSelectTasks", "OpenToAddSelectTasks();", True)
					Else
						'Response.Redirect("wfnWOJobTask_AJAX.aspx?BackPage2=wfnWOJobDetail_AJAX.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage") & "&Index=-1")
						Session("mIndex") = "-1"
						ScriptManager.RegisterStartupScript(Me, [GetType], "OpenToAddJobTaskDetail", "OpenToAddJobTaskDetail();", True)
					End If
				End If
				'======================== 
			Case "InstRem"
				'Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobs.PageSize * dgWOJobs.PageIndex
				Dim Index As Integer
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Index = gvr.RowIndex
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetGridObject()
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Session("Edit") = False
				SetObject()
				SetGridObject()
				mnWO.WONRCJobs.CurrentIndex = Index
				Session("WOJobTypeID") = mnWO.WONRCJobs.CurrentItem.WOJobTypeID
				Session("mnWO") = mnWO
				mnWOClone = mnWO.Clone
				Session("mnWOClone") = mnWOClone
				Session("mnWOJob") = mnWO.WONRCJobs.CurrentItem

				Session("mIndex") = "-1"
				ScriptManager.RegisterStartupScript(Me, [GetType], "OpenToAddJobCompDetail", "OpenToAddJobCompDetail();", True)
				'===============================
			Case "DesignationAllocation"
				'Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobs.PageSize * dgWOJobs.PageIndex
				Dim Index As Integer
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Index = gvr.RowIndex
				If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If

				SetObject()
				Session("mDesignationAllocationEdit") = False
				'Session("WOJobTypeID") = mWOJobTypeID
				mnWO.WONRCJobs.CurrentIndex = Index
				Session("mnWOJob") = mnWO.WONRCJobs.CurrentItem
				ScriptManager.RegisterStartupScript(Me, [GetType], "OpenToAddDesignaionAllocation", "OpenToAddDesignaionAllocation();", True)

			Case "RequiredSpares"
				'Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobs.PageSize * dgWOJobs.PageIndex
				Dim Index As Integer
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Index = gvr.RowIndex

				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetGridObject()
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Session("Edit") = True
				SetObject()
				SetGridObject()
				mnWO.WONRCJobs.CurrentIndex = Index
				Session("WOJobTypeID") = mnWO.WONRCJobs.CurrentItem.WOJobTypeID

				Session("mnWO") = mnWO
				mnWOClone = mnWO.Clone
				Session("mnWOClone") = mnWOClone
				Session("mnWOJob") = mnWO.WONRCJobs.CurrentItem
				ScriptManager.RegisterStartupScript(Me, [GetType], "OpenToAddJobSpareDetail", "OpenToAddJobSpareDetail();", True)
				'======================== Ajay 20-sep-2022 End ====================
		End Select
	End Sub

	Private Sub btnAddNRC_Click(sender As Object, e As ImageClickEventArgs) Handles btnAddNRC.Click
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If Not CustomValidateObject() Then upnlValidationsummary.Update() : Exit Sub
		AddWONRC()
	End Sub
	'End

	Private Sub dgwoAttachment_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgWOAttachment.RowCommand
		Dim mFileAttachments As FileAttachments
		Select Case e.CommandName
			Case "View"
				Dim Index As Integer = CInt(e.CommandArgument)

				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				mFileAttachments = mnWO.FileAttachments
				'mFileAttachments.CurrentIndex = Index - 1

				If mFileAttachments.Count = 1 Then
					mFileAttachments.CurrentIndex = 0
				Else
					mFileAttachments.CurrentIndex = Index - 1
				End If

				If mFileAttachments.CurrentItem.Size > 0 Then
					Dim path As String = AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						File.Delete(AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mFileAttachments.CurrentItem.ImageFile, 0, mFileAttachments.CurrentItem.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, [GetType], "openFilel", "openFilel();", True)
					End If
				End If
				dgWOAttachment.DataSource = mnWO.FileAttachments
				dgWOAttachment.DataBind()
				ControlVisibility()
				upnlWOAttachment.Update()
				upnldgWOAttachment.Update()
			Case "Remove"
				'Dim Index As Integer = CInt(e.CommandArgument) '+ dgWOAttachment.PageSize * dgWOAttachment.PageIndex
				Dim Index As Integer = CInt(e.CommandArgument) + dgWOAttachment.PageSize * dgWOAttachment.PageIndex
				' DeleteAttachment(Index)
				mFileAttachments = mnWO.FileAttachments
				If mFileAttachments.Count = 1 Then
					DeleteAttachment(0)
				Else
					DeleteAttachment(Index - 1)
				End If
		End Select

	End Sub

	'Added by Saylee on 23-Jun-2016 for NRC
	Private Sub btnPrintManHrs_Click(sender As Object, e As EventArgs) Handles btnPrintManHrs.Click
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		GetSession()
		Dim da As New ObjectAdapter
		Dim mCompanyDetail As New CompanyDetail

		Dim mnWOJobs As nWOJobs
		Dim mWONRCJobList As WONRCJobList
		Dim mnWOJobDesignationAllocations As nWOJobDesignationAllocations
		Dim WOIssueNo As String = ""
		Dim WORevisionNo As String = ""

		Dim ds As New dsnWORegister

		Dim myReport = New crnWOManHoursUtilization

		Dim SearchStr1, SearchStr3 As String
		Dim SearchStr4, SearchStr5, SearchStr6 As String

		WOIssueNo = AppSettings("WOIssueNo")

		'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
		' WORevisionNo = AppSettings("WORevisionNo")
		WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
		'-----

		mnWO = nWO.GetWO(mnWO.ID, AllWOJobType:=False)
		mnWOJobs = mnWO.WOJobs
		mWONRCJobList = WONRCJobList.GetWONRCJobList(mnWO.ID, 5)
		mnWOJobDesignationAllocations = nWOJobDesignationAllocations.GetWOJobDesignationAllocations(mnWO.ID, "", IsNRCAllocationsRequired:=True)
		myReport.SetDataSource(ds)

		SearchStr3 = txtNo.Text

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
					  mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
					  mCompanyDetail.WebSite, "", SearchStr1, WOIssueNo, WORevisionNo, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6, AppSettings("ClientCode"), AppSettings("Government Authority"), , AppSettings("Logo")) 'Dont Use SearchStr20 

		Dim mrptImage As rptImage = rptImage.GetImage(ds)

		'WO Detail
		da.Fill(ds, mnWO)
		da.Fill(ds, mnWOJobs)
		da.Fill(ds, mWONRCJobList)
		da.Fill(ds, mnWOJobDesignationAllocations)
		da.Fill(ds, Report)
		da.Fill(ds, mrptImage)
		myReport.SetDataSource(ds)

		Session("CrystalReport") = myReport
		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print Man Hrs. : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)

	End Sub

	Protected Sub btnSparePartConsumption_Click(sender As Object, e As EventArgs) Handles btnSparePartConsumption.Click
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		GetSession()
		Dim da As New ObjectAdapter
		Dim mCompanyDetail As New CompanyDetail

		Dim mnWOJobs As nWOJobs
		Dim mWONRCJobList As WONRCJobList
		Dim mnWOJobSpares As nWOJobSpares
		Dim mnWOTools As nWOTools
		Dim WOIssueNo As String = ""
		Dim WORevisionNo As String = ""

		Dim ds As New dsnWORegister

		Dim myReport = New crnSparePartConsumption

		Dim SearchStr1, SearchStr3 As String
		Dim SearchStr4, SearchStr5, SearchStr6 As String

		WOIssueNo = AppSettings("WOIssueNo")
		'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
		' WORevisionNo = AppSettings("WORevisionNo")
		WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
		'-----

		myReport = New crnSparePartConsumption

		mnWO = nWO.GetWO(mnWO.ID, AllWOJobType:=False)
		mnWOJobs = mnWO.WOJobs
		mWONRCJobList = WONRCJobList.GetWONRCJobList(mnWO.ID, 5)
		mnWOJobSpares = nWOJobSpares.GetWONRCJobSpares(mnWO.ID, "")
		mnWOTools = mnWO.WOTools
		myReport.SetDataSource(ds)

		SearchStr3 = txtNo.Text

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
					  mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
					  mCompanyDetail.WebSite, "", SearchStr1, WOIssueNo, WORevisionNo, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6, AppSettings("ClientCode"), AppSettings("Government Authority"), , AppSettings("Logo")) 'Dont Use SearchStr20 

		Dim mrptImage As rptImage = rptImage.GetImage(ds)

		'WO Detail
		da.Fill(ds, mnWO)
		da.Fill(ds, mnWOJobs)
		da.Fill(ds, mWONRCJobList)
		da.Fill(ds, mnWOJobSpares)
		da.Fill(ds, mnWOTools)
		da.Fill(ds, Report)
		da.Fill(ds, mrptImage)
		myReport.SetDataSource(ds)

		myReport.Section6.SectionFormat.EnableSuppress = True

		Session("CrystalReport") = myReport

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print for Spare Part Consumption : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)
	End Sub

	Private Sub btnSelectFiles_Click(sender As Object, e As ImageClickEventArgs) Handles btnSelectFiles.Click
		SetObject()
		Session("mnWO") = mnWO
		ScriptManager.RegisterStartupScript(Me, [GetType], "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
	End Sub

	'Added by Vikrant On 03-Apr-2019 For ALL03042019
	Private Sub btnSaveAttachment_Click(sender As Object, e As EventArgs) Handles btnSaveAttachment.Click
		If mnWO.IsDirty Then
			mnWO.UpdateAttachments(mnWO.FileAttachments)
			MarkLog(Action.Save, "Work Order", "Attachment Saved By " + User.Identity.Name, ErrorType.NoError, mnWO.ID, EventLogID)
			MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
		End If
	End Sub
	'End

	'Added by Saylee on 29-May-2019
	Private Sub hdnBtnAddSelectTasks_Click(sender As Object, e As EventArgs) Handles hdnBtnAddSelectTasks.Click
		If CType(Session("AddTaskCards"), String) = "True" Then
			'Add selected part(s) to Task's Items
			AddMultipleTaskCards()
			Session("AddTaskCards") = "False"
		Else
			Session("AddTaskCards") = "False"
		End If

		dgWOJobs.DataSource = mnWO.WOJobs
		dgWOJobs.DataBind()
		SetGrid()
		ControlVisibility() 'Added By Prashant on 24-Sep-2024
		upnlGrids.Update()

		SetGrid()

	End Sub

	Private Sub btnQCApproval_Click(sender As Object, e As EventArgs) Handles btnQCApproval.Click
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		If IsValid Then

			If txtStatusRemark.Text.ToString = "" Then
				MSGBoxCtrl.Show("Alert!!", "QC Remark required.", "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If

			'Commented by Saylee on 4-Mar-2020, now new table maintained

			''''If rdbNone.Checked Then
			''''    MSGBoxCtrl.show("Alert!", "Please select QC Approved/Rejected  details", "", MsgBoxStyle.OkOnly, "")
			''''    Exit Sub
			''''End If
			'**********************************
			Dim str As String = ""

			'Commented by Saylee on 21-Aug-2019, as now QCApproved Field shifted into WO table directly
			''''If rdbApproved.Checked Then
			''''    str = "Do want to Approve "
			''''    mnWO.WOStatusID = 5
			''''ElseIf rdbNotApproved.Checked Then
			''''    str = "Do want to Reject "
			''''    mnWO.WOStatusID = 6
			''''End If
			'Commented by Saylee on 4-Mar-2020, now new table maintained
			'''''''If rdbApproved.Checked Then
			'''''''    str = "Do you want to Approve "
			'''''''    mnWO.IsQCStatusApproved = 1
			'''''''ElseIf rdbNotApproved.Checked Then
			'''''''    str = "Do you want to Reject "
			'''''''    mnWO.IsQCStatusApproved = 2
			'''''''End If
			mnWO.IsQCStatusApproved = 1
			'********************************************************************************

			'Added By Saylee On 4-Mar-2020 For Approval Reject history
			mnWOApproveReject = nWOApproveReject.NewApproval(mnWO.ID)

			If (AppSettings("ClientCode") = "IND") Then
				mnWOApproveReject.Date = CType(DateTime.Now.ToString.Trim, DateTime)
			Else
				mnWOApproveReject.Date = CDate(DateTime.Now.ToString.Trim)
			End If


			mnWOApproveReject.ApprovedRejectStatus = 1
			mnWOApproveReject.Remark = txtStatusRemark.Text
			mnWOApproveReject.WOStatusID = 5
			'mnWO.WOStatusID = 5
			mWODetail = "QC Stage " & mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + "QC Approved By : "
			Session("WODetailForMarkLog") = mWODetail

			Session("mnWOApproveReject") = mnWOApproveReject
			'**************************************************************

			If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
				tmpText = "Engineering Order"
				''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.StatusCompleted, SIMsgBox.Message_text.StatusCompleted, "<strong>Engineering Order</strong>", MsgBoxStyle.YesNo)
				''msg1.ReplacePage = "wfnWODetail_AJAX.aspx?BackPage=" & Request.QueryString("BackPage")
				''Session("sender") = "WOStatus"
				''msg1.Show()
				Session("IsValid") = IsValid
				MSGBoxCtrl.Show("Approval Confirmation", "Do you want to Approve " + "<strong>Engineering Order?</strong>", "", MsgBoxStyle.YesNo, "WOQCStatus")
				SetObject()
				SetGridObject()

				'mnWO.IsClosed = True
				'mnWO.ClosedBy = Trim(txtClosedBy.Text)
				Session("mnWO") = mnWO
			Else
				tmpText = "Work Order"
				''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.StatusCompleted, SIMsgBox.Message_text.StatusCompleted, "<strong>Work Order</strong>", MsgBoxStyle.YesNo)
				''msg1.ReplacePage = "wfnWODetail_AJAX.aspx?BackPage=" & Request.QueryString("BackPage")
				''Session("sender") = "WOStatus"
				''msg1.Show()  Dim str As String = ""

				MSGBoxCtrl.Show("Approval Confirmation", "Do you want to Approve " + "<strong>Work Order?</strong>", "", MsgBoxStyle.YesNo, "WOQCStatus")
				Session("IsValid") = IsValid

				SetObject()
				SetGridObject()

				'mnWO.IsClosed = True
				'mnWO.ClosedBy = Trim(txtClosedBy.Text)

				Session("mnWO") = mnWO
			End If
		Else
			upnlValidationsummary.Update()
		End If
	End Sub

	Private Sub lnkCreateToolsRequisition_Click(sender As Object, e As EventArgs) Handles lnkCreateToolsRequisition.Click
		If (AppSettings("ClientCode") <> "STR" And Not User.IsInRole("EngineeringRequisitionNew")) Or (AppSettings("ClientCode") = "STR" And ((mnWO.WOJobs(0).WOJobTypeID = 1 And Not User.IsInRole("PlanningRequisitionNew")) Or (mnWO.WOJobs(0).WOJobTypeID <> 1 And Not User.IsInRole("EngineeringRequisitionNew")))) Then 'For Star Air For Unscheduled Job create Planning Req and for other jobs create Engg. Req.
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If

		mRequisitionNew = RequisitionNew.NewRequisition(Trans.EngineeringRequisition)
		mRequisitionNew.ReqDate = mnWO.WODate

		For i As Integer = 0 To mRequisitionItemsNew.Count - 1
			ReqItemIds.Append(mRequisitionItemsNew(i).ItemID.ToString + ",")
		Next

		For i As Integer = 0 To mnWO.WOTools.Count - 1
			If Not ReqItemIds.ToString.TrimEnd(",").Contains(mnWO.WOTools(i).ItemID.ToString) Then '12-Jun-2019
				Dim mItemList As ItemList
				mItemList = ItemList.GetItemList(1, ItemName:=mnWO.WOTools(i).PartNo)
				If mItemList.Count > 0 Then
					If Not mRequisitionNew.RequisitionItemsNew.Contains(mItemList(0).ID) Then
						mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, Guid.Empty)
						mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = mItemList(0).ID
						mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo = mItemList(0).Name
						mRequisitionNew.RequisitionItemsNew.CurrentItem.Description = mItemList(0).Description
						mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference = mItemList(0).IPCReference
						mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty = mnWO.WOTools(i).RequiredQty
						mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID = mItemList(0).UnitID
						mRequisitionNew.RequisitionItemsNew.CurrentItem.Unit = mItemList(0).UnitName
						mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase = mItemList(0).IsOneTimePurchase
						mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = mnWO.MachineID
						mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = mnWO.RegNo
						mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = mnWO.ID
						mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = mnWO.WONumber

						If Not mItemList(0).IsOneTimePurchase Then
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = mItemList(0).MinStockLevel
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = mItemList(0).MaxStockLevel
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = mItemList(0).MinReOrderLevel
						Else
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = 0
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = 0
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = 0
						End If

					Else
						mRequisitionNew.RequisitionItemsNew(mItemList(0).ID, "").RequestedQty += mnWO.WOTools(i).RequiredQty
					End If
				End If
			End If
		Next
		Session("mRequisitionNew") = mRequisitionNew
		Session("TransTypeID") = Trans.EngineeringRequisition
		MarkLog(Action.[New], "Engineering Requisition", "", ErrorType.NoError, mRequisitionNew.ID, EventLogID)
		Dim ReqURLFromWO As New Stack
		ReqURLFromWO.Push(Request.Url)
		Session("ReqURLFromWO") = ReqURLFromWO
		Session("MiddleFrameForWO") = Session("MiddleFrame")
		Session.Remove("ActiveJobDetailsTabIndex")
		Response.Redirect("wfRequisition_Ajax.aspx?BackPage=wfnWODetail_AJAX.aspx")
	End Sub

	Private Sub lnkCreateRequisition_Click(sender As Object, e As EventArgs) Handles lnkCreateRequisition.Click
		If (AppSettings("ClientCode") <> "STR" And Not User.IsInRole("EngineeringRequisitionNew")) Or (AppSettings("ClientCode") = "STR" And ((mnWO.WOJobs(0).WOJobTypeID = 1 And Not User.IsInRole("PlanningRequisitionNew")) Or (mnWO.WOJobs(0).WOJobTypeID <> 1 And Not User.IsInRole("EngineeringRequisitionNew")))) Then 'For Star Air For Unscheduled Job create Planning Req and for other jobs create Engg. Req.
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If

		'Commneted & Added by vikrant on 19-Sep-2019
		'mRequisitionNew = RequisitionNew.NewRequisition(Trans.EngineeringRequisition)
		'mRequisitionNew.ReqDate = mnWO.WODate
		Dim mRequisitionListNew As RequisitionListNew
		If AppSettings("ShowNewWOFlow") = "True" Then 'If AppSettings("ClientCode") = "IND" Then
			mRequisitionListNew = RequisitionListNew.GetRequisitionList(WOID:=mnWO.ID.ToString)
			If mRequisitionListNew.Count > 0 Then
				For i As Integer = 0 To mRequisitionListNew.Count - 1
					If mRequisitionListNew(i).StatusID = 1 Then
						mRequisitionNew = RequisitionNew.GetRequisition(mRequisitionListNew(i).ID)
						GoTo NextStatement
					End If
				Next
				mRequisitionNew = RequisitionNew.NewRequisition(Trans.EngineeringRequisition)
				mRequisitionNew.ReqDate = mnWO.WODate
			Else
				mRequisitionNew = RequisitionNew.NewRequisition(Trans.EngineeringRequisition)
				mRequisitionNew.ReqDate = mnWO.WODate
			End If
		Else
			If AppSettings("ClientCode") = "STR" Then
				If mnWO.WOJobs(0).WOJobTypeID = 1 Then
					mRequisitionNew = RequisitionNew.NewRequisition(Trans.EngineeringRequisition)
				Else
					mRequisitionNew = RequisitionNew.NewRequisition(Trans.PlanningRequisition)
				End If
			Else
				mRequisitionNew = RequisitionNew.NewRequisition(Trans.EngineeringRequisition)
			End If
			mRequisitionNew.ReqDate = mnWO.WODate
		End If
		'End
NextStatement:
		'Added by Shital on 18-Sep-2019
		If AppSettings("ClientCode") = "IND" And mRequisitionNew.IsNew Then 'mRequisitionNew.IsNew Added by vikrant on 19-Sep-2019
			mRequisitionNew.LocationID = mWorkShopList(mnWO.WorkShopID).locationID
		End If

		'12-Jun-2019
		For i As Integer = 0 To mRequisitionItemsNew.Count - 1
			ReqItemIds.Append(mRequisitionItemsNew(i).ItemID.ToString + ",")
		Next
		'End
		For i As Integer = 0 To mnWO.WOJobs.Count - 1
			For j As Integer = 0 To mnWO.WOJobs(i).WOJobSpares.Count - 1
				If Not ReqItemIds.ToString.TrimEnd(",").Contains(mnWO.WOJobs(i).WOJobSpares(j).ItemID.ToString) Then '12-Jun-2019
					Dim mItemList As ItemList
					mItemList = ItemList.GetItemList(1, ItemName:=mnWO.WOJobs(i).WOJobSpares(j).PartNo)
					If mItemList.Count > 0 Then
						If Not mRequisitionNew.RequisitionItemsNew.Contains(mItemList(0).ID) Then
							mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, Guid.Empty)
							mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = mItemList(0).ID
							mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo = mItemList(0).Name
							mRequisitionNew.RequisitionItemsNew.CurrentItem.Description = mItemList(0).Description
							mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference = mItemList(0).IPCReference
							mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty = mnWO.WOJobs(i).WOJobSpares(j).RequiredQty
							mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID = mItemList(0).UnitID        'Added By Prashant On 07-May-2019 BA07052019
							mRequisitionNew.RequisitionItemsNew.CurrentItem.Unit = mItemList(0).UnitName        'Added By Prashant On 07-May-2019 BA07052019
							mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase = mItemList(0).IsOneTimePurchase
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = mnWO.MachineID
							mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = mnWO.RegNo
							mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = mnWO.ID
							mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = mnWO.WONumber

							If Not mItemList(0).IsOneTimePurchase Then
								mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = mItemList(0).MinStockLevel
								mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = mItemList(0).MaxStockLevel
								mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = mItemList(0).MinReOrderLevel
							Else
								mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = 0
								mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = 0
								mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = 0
							End If

						Else
							mRequisitionNew.RequisitionItemsNew(mItemList(0).ID, "").RequestedQty += mnWO.WOJobs(i).WOJobSpares(j).RequiredQty
						End If
					End If
				Else 'Added By Prashant On 18-Nov-2022 if job is added later with spares having same part no. to create its req. this code is
					If AppSettings("ClientCode") = "KAS" Then
						Dim TempItemWiseRequisitionItemQtySum, TempItemWiseRequiredQtySum, Diffrence As Decimal
						Dim mTempItemID As Guid = mnWO.WOJobs(i).WOJobSpares(j).ItemID
						Dim ItemWiseRequisitionItemQtySum = From c In mRequisitionItemsNew
															Where c.ItemID = mTempItemID
															Group c By ItemID = c.ItemID, PartNo = c.PartNo Into Group
															Select New With {Key .ItemID = ItemID, Key .PartNo = PartNo, Key .RequestedQty = Group.Sum(Function(x) x.RequestedQty)}

						Dim mnWOJobSpares As nWOJobSpares
						mnWOJobSpares = nWOJobSpares.GetWOSpares(mnWO.ID, "")
						Dim ItemWiseRequiredQtySum = From c In mnWOJobSpares
													 Where c.ItemID = mTempItemID
													 Group c By ItemID = c.ItemID, PartNo = c.PartNo Into Group
													 Select New With {Key .ItemID = ItemID, Key .PartNo = PartNo, Key .RequiredQty = Group.Sum(Function(x) x.RequiredQty)}

						For Each variable1 As Object In ItemWiseRequisitionItemQtySum
							TempItemWiseRequisitionItemQtySum = variable1.RequestedQty
						Next
						For Each variable2 As Object In ItemWiseRequiredQtySum
							TempItemWiseRequiredQtySum = variable2.RequiredQty
						Next

						Diffrence = (TempItemWiseRequiredQtySum - TempItemWiseRequisitionItemQtySum)
						If TempItemWiseRequiredQtySum > TempItemWiseRequisitionItemQtySum And Not Diffrence < 0 Then
							Dim mItemList As ItemList
							mItemList = ItemList.GetItemList(1, ItemName:=mnWO.WOJobs(i).WOJobSpares(j).PartNo)
							If mItemList.Count > 0 Then
								If Not mRequisitionNew.RequisitionItemsNew.Contains(mItemList(0).ID) Then
									mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, Guid.Empty)
									mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = mItemList(0).ID
									mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo = mItemList(0).Name
									mRequisitionNew.RequisitionItemsNew.CurrentItem.Description = mItemList(0).Description
									mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference = mItemList(0).IPCReference
									mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty = Diffrence
									mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID = mItemList(0).UnitID        'Added By Prashant On 07-May-2019 BA07052019
									mRequisitionNew.RequisitionItemsNew.CurrentItem.Unit = mItemList(0).UnitName        'Added By Prashant On 07-May-2019 BA07052019
									mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase = mItemList(0).IsOneTimePurchase
									mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = mnWO.MachineID
									mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = mnWO.RegNo
									mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = mnWO.ID
									mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = mnWO.WONumber

									If Not mItemList(0).IsOneTimePurchase Then
										mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = mItemList(0).MinStockLevel
										mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = mItemList(0).MaxStockLevel
										mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = mItemList(0).MinReOrderLevel
									Else
										mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = 0
										mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = 0
										mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = 0
									End If
								End If
							End If
						End If
						'-----------
					End If
				End If
			Next
		Next

		For i As Integer = 0 To mnWO.WONRCJobs.Count - 1
			For j As Integer = 0 To mnWO.WONRCJobs(i).WOJobSpares.Count - 1
				If Not ReqItemIds.ToString.TrimEnd(",").Contains(mnWO.WONRCJobs(i).WOJobSpares(j).ItemID.ToString) Then '12-Jun-2019
					Dim mItemList As ItemList
					mItemList = ItemList.GetItemList(1, ItemName:=mnWO.WONRCJobs(i).WOJobSpares(j).PartNo)
					If mItemList.Count > 0 Then
						If Not mRequisitionNew.RequisitionItemsNew.Contains(mItemList(0).ID) Then
							mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, Guid.Empty)
							mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = mItemList(0).ID
							mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo = mItemList(0).Name
							mRequisitionNew.RequisitionItemsNew.CurrentItem.Description = mItemList(0).Description
							mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference = mItemList(0).IPCReference
							mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty = mnWO.WONRCJobs(i).WOJobSpares(j).RequiredQty
							mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID = mItemList(0).UnitID        'Added By Prashant On 07-May-2019 BA07052019
							mRequisitionNew.RequisitionItemsNew.CurrentItem.Unit = mItemList(0).UnitName        'Added By Prashant On 07-May-2019 BA07052019
							mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase = mItemList(0).IsOneTimePurchase
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = mnWO.MachineID
							mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = mnWO.RegNo
							mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = mnWO.ID
							mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = mnWO.WONumber

							If Not mItemList(0).IsOneTimePurchase Then
								mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = mItemList(0).MinStockLevel
								mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = mItemList(0).MaxStockLevel
								mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = mItemList(0).MinReOrderLevel
							Else
								mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = 0
								mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = 0
								mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = 0
							End If

						Else
							mRequisitionNew.RequisitionItemsNew(mItemList(0).ID, "").RequestedQty += mnWO.WONRCJobs(i).WOJobSpares(j).RequiredQty
						End If

					End If
				End If
			Next
		Next
		Session("mRequisitionNew") = mRequisitionNew
		If AppSettings("ClientCode") = "STR" Then
			If mnWO.WOJobs(0).WOJobTypeID = 1 Then
				Session("TransTypeID") = Trans.EngineeringRequisition
				MarkLog(Action.[New], "Engineering Requisition", "", ErrorType.NoError, mRequisitionNew.ID, EventLogID)
			Else
				Session("TransTypeID") = Trans.PlanningRequisition
				MarkLog(Action.[New], "Planning Requisition", "", ErrorType.NoError, mRequisitionNew.ID, EventLogID)
			End If
		Else
			Session("TransTypeID") = Trans.EngineeringRequisition
			MarkLog(Action.[New], "Engineering Requisition", "", ErrorType.NoError, mRequisitionNew.ID, EventLogID)
		End If


		Dim ReqURLFromWO As New Stack
		ReqURLFromWO.Push(Request.Url)
		Session("ReqURLFromWO") = ReqURLFromWO
		Session("MiddleFrameForWO") = Session("MiddleFrame") '12-Jun-2019
		Session.Remove("ActiveJobDetailsTabIndex")
		Response.Redirect("wfRequisition_Ajax.aspx?BackPage=wfnWODetail_AJAX.aspx")
	End Sub

	Private Sub lnkWOParameters_Click(sender As Object, e As EventArgs) Handles lnkWOParameters.Click 'Saylee on 16-Sep-2019
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		SetObject()
		SetGridObject()
		Session("mnWO") = mnWO
		Session("WOID") = mnWO.ID.ToString
		ScriptManager.RegisterStartupScript(Me, [GetType], "OpenWOParameters", "OpenWOParameters();", True)
	End Sub

	Private Sub rdpYes_CheckedChanged(sender As Object, e As EventArgs) Handles rdpYes.CheckedChanged, rdpNo.CheckedChanged
		If rdpYes.Checked Then
			lblCustBy.Visible = True
			cmbCustApprovedByEmailWO.Visible = True
			cmbCustApprovedByEmailWO.SelectedIndex = 0
		Else
			lblCustBy.Visible = False
			cmbCustApprovedByEmailWO.Visible = False
		End If
	End Sub

	'Added By Saylee On 26-Sep-2018 For STR26092018
	Private Sub txtWOTime_TextChanged(sender As Object, e As EventArgs) Handles txtWOTime.TextChanged
		If IsValidTime(txtWOTime.Text.ToString.Trim) = False Then
			txtWOTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			Dim DateTime As String = txtWODate.Text.ToString + " " + txtWOTime.Text.ToString.Trim
			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mnWO.WODateFormatted.ToString), New SmartDate(DateTime).Date) <> 0 Then
				mnWO.WODate = DateTime
				' DataFieldBind()
				Session("mnWO") = mnWO
			End If
		End If
	End Sub

	Private Sub txtStartDateTime_TextChanged(sender As Object, e As EventArgs) Handles txtStartDateTime.TextChanged
		If IsValidTime(txtStartDateTime.Text.ToString.Trim) = False Then
			txtStartDateTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			Dim DateTime As String = txtStartDate.Text.ToString + " " + txtStartDateTime.Text.ToString.Trim
			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mnWO.WOStartDateFormatted.ToString), New SmartDate(DateTime).Date) <> 0 Then
				mnWO.WOStartDate = DateTime
				' DataFieldBind()
				Session("mnWO") = mnWO
			End If
		End If
	End Sub

	Private Sub txtPlanDateTime_TextChanged(sender As Object, e As EventArgs) Handles txtPlanDateTime.TextChanged
		If IsValidTime(txtPlanDateTime.Text.ToString.Trim) = False Then
			txtPlanDateTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			Dim DateTime As String = txtPlanDate.Text.ToString + " " + txtPlanDateTime.Text.ToString.Trim
			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mnWO.WOPlanedDateFormatted.ToString), New SmartDate(DateTime).Date) <> 0 Then
				mnWO.WOPlanedDate = DateTime
				' DataFieldBind()
				Session("mnWO") = mnWO
			End If
		End If
	End Sub

	Private Sub txtClosedDateTime_TextChanged(sender As Object, e As EventArgs) Handles txtClosedDateTime.TextChanged
		If IsValidTime(txtClosedDateTime.Text.ToString.Trim) = False Then
			txtClosedDateTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			Dim DateTime As String = txtCloseDate.Text.ToString + " " + txtClosedDateTime.Text.ToString.Trim
			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mnWO.WOCloseDateFormatted.ToString), New SmartDate(DateTime).Date) <> 0 Then
				mnWO.WOCloseDate = DateTime
				' DataFieldBind()
				Session("mnWO") = mnWO
			End If
		End If
	End Sub

	Private Sub txtQCDateTime_TextChanged(sender As Object, e As EventArgs) Handles txtQCDateTime.TextChanged
		If IsValidTime(txtQCDateTime.Text.ToString.Trim) = False Then
			txtQCDateTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		End If
	End Sub

	Private Sub hdnBtnJobList_Click(sender As Object, e As EventArgs) Handles hdnBtnJobList.Click, hdnBtnAddJobCompDetail.Click, hdnBtnAddDesignaionAllocation.Click, hdnBtnAddJobSpareDetail.Click, hdnBtnAddSelectNRC.Click, hdnBtnAddJobTaskDetail.Click

		dgWOJobs.DataSource = mnWO.WOJobs
		dgWOJobs.DataBind()
		SetGrid()

		' Added by Ajay 20-Sep-2022
		dgWONRC.DataSource = mnWO.WONRCJobs
		dgWONRC.DataBind()
		SetNRCGrid()
		'****************************************

		ControlVisibility()
		upnlGrids.Update()

	End Sub

	Private Sub hdnBtnAddWOTool_Click(sender As Object, e As EventArgs) Handles hdnBtnAddWOTool.Click
		dgWOTools.DataSource = mnWO.WOTools
		dgWOTools.DataBind()
		SetGrid()
		upnlToolGrid.Update()
	End Sub

	Private Sub rdbBillingDone_CheckedChanged(sender As Object, e As EventArgs) Handles rdbBillingDone.CheckedChanged, rdbBillingNotRequired.CheckedChanged, rdbBillingNone.CheckedChanged
		If rdbBillingDone.Checked Or rdbBillingNotRequired.Checked Then
			btnBilling.Enabled = True

			If rdbBillingDone.Checked Then
				lblBillingByStar.Visible = True
				lblBillingInvoiceNumberStar.Visible = True
				lblBillingStar.Visible = True
			Else
				lblBillingByStar.Visible = False
				lblBillingInvoiceNumberStar.Visible = False
				lblBillingStar.Visible = False
			End If
		ElseIf rdbBillingNone.Checked Then
			btnBilling.Enabled = False

			lblBillingByStar.Visible = False
			lblBillingInvoiceNumberStar.Visible = False
			lblBillingStar.Visible = False
		End If
		UpnlPrint.Update()
	End Sub

	Private Sub btnBilling_Click(sender As Object, e As EventArgs) Handles btnBilling.Click

		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If CustValidate1() = False Then upnlValidationSummary3.Update() : Exit Sub
		Page.Validate("c")
		If Not Page.IsValid Then
			upnlValidationSummary3.Update()
			Exit Sub
		End If


		MSGBoxCtrl.Show("Billing Confirmation", "Do you want to save this Billing Details?", "", MsgBoxStyle.YesNo, "WOBillingStatus")
		Session("IsValid") = IsValid

		Session("mnWO") = mnWO
	End Sub

	Private Sub btnSendMailTool_Click(sender As Object, e As EventArgs) Handles btnSendMailTool.Click

		'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
		SetUserMailIDs()
		If Session("UserEmailID") = "" Then
			Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
		End If

		Session("btnSendMail") = "btnSendMailTool"
		Dim Str As String
		Str = "OpenByMaiWindow();"
		ScriptManager.RegisterStartupScript(Me, [GetType], "OpenByMaiWindow", Str, True)

	End Sub

	Private Sub btnSendMail_Click(sender As Object, e As EventArgs) Handles btnSendMail.Click

		Dim Str As String
		SetUserMailIDs()         'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
		Session("btnSendMail") = "btnSendMail"
		Str = "OpenByMaiWindow();"
		ScriptManager.RegisterStartupScript(Me, [GetType], "OpenByMaiWindow", Str, True)

	End Sub

	Private Sub hdnimgBtnSendMail_Click(sender As Object, e As EventArgs) Handles hdnimgBtnSendMail.Click
		Dim email As Thread
		Try
			Dim ReportName As String = "Work Order Details"
			If Session("btnSendMail") = "btnSendMailTool" Then
				SendMailForToolsRequest(True)
			ElseIf Session("btnSendMail") = "btnSendMail" Then
				If (AppSettings("ClientCode") = "APFT" Or
					AppSettings("ClientCode") = "AAP" Or
					AppSettings("ClientCode") = "SHR") And
				   mnWO.IsDigitalSignatureAdded = True And
				   mnWO.StatusID <> 1 Then
					MSGBoxCtrl.Show("Digital Signature Confirmation!", "Do you want to print with Digital Signature?", "", MsgBoxStyle.YesNo, "SignatureRequired")
					Exit Sub
				ElseIf AppSettings("ClientCode") = "Heligo" Then
					Print(ByMail:=True, HeligoCallOutPrint:=True)
					'' ReportName = "CAMO Call Out No : " + mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
					If mnWO.TransTypeID = Trans.WOCAMO Then
						ReportName = "CAMO Call Out No : " + mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
					ElseIf mnWO.TransTypeID = Trans.WO145 Then
						ReportName = "QC Work Order : " + mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
					End If
				Else
					Print(ByMail:=True)
				End If

				Dim Text As String = ""
				Dim RegNo As String = ""
				Dim Info As String = ""
				If AppSettings("ClientCode") = "APFT" Or
				   AppSettings("ClientCode") = "AAP" Then
					Text = " CALL-OUT/Work Order - " + mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
				ElseIf AppSettings("ClientCode") = "STR" Then
					ReportName = mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
					RegNo = mnWO.RegNo
					Text = mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
					Info = " <b>" + mnWO.WOJobs(0).WOJobDescription + " </b>"
				Else
					Text = mnWO.WOText.Replace("/", " ").ToString + "-" + mnWO.WONo.ToString
				End If

				SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, Text.ToString, Info,
									 "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
									  SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"), ClientCode:=AppSettings("ClientCode"), TransTypeID:=mnWO.TransTypeID)
				mnWO.IsMailSend = True
				Session("mnWO") = mnWO
				mnWO.Save()
			End If
			email.IsBackground = True
			email.Start()
		Catch ex As Exception
			Dim Day, Month, Year As String
			Day = Format(Today.Date.Day, "0#")
			Month = Format(Today.Date.Month, "0#")
			Year = Format(Today.Date.Year, "0#")
			Dim todaydate As String = Day & Month & Year
			Dim Path As String = AppSettings("DOCPath") & todaydate
			FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
			WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
			FileClose(1)
		End Try

	End Sub

	Private Sub btnAMECompletion_Click(sender As Object, e As EventArgs) Handles btnAMECompletion.Click 'Added By Prashant 16-Aug-2019

		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If IsValid Then
			If txtCloseDate.Text.ToString = "" Or txtClosedBy.Text = "" Or txtStartDate.Text.ToString = "" Then
				MSGBoxCtrl.Show("Alert!", "Please enter the Starting/Closing details before completing a Work Order", "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If

			If AppSettings("ShowNewWOFlow") = "True" Then ' If AppSettings("ClientCode") = "IND" Then
				If txtStatusRemark.Text = "" Then
					MSGBoxCtrl.Show("Alert!", "Please enter the AME Remark before completing a Work Order", "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
			End If

			'Added By Vikrant On 24-May-2019 For New WO  
			Dim mIssuedWOTools As nIssuedWOTools
			Dim ToolsPartNos As New StringBuilder
			mIssuedWOTools = nIssuedWOTools.GetnIssuedWOTools(mnWO.ID)
			For i As Integer = 0 To mIssuedWOTools.Count - 1
				If mIssuedWOTools(i).LoanQty > 0 Then
					ToolsPartNos.Append(mIssuedWOTools(i).PartNo + " (" + mIssuedWOTools(i).SerialNo + ")" + ",")
				End If
			Next
			If ToolsPartNos.ToString.TrimEnd(",") <> "" Then
				MSGBoxCtrl.Show("Alert!", "Tool(s) " + ToolsPartNos.ToString.TrimEnd(",") + " are issued against Work Order which are not returned yet.", "Please return back Tool(s) before completing a Work Order", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
			'End
			If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
				tmpText = "Engineering Order"
				'Session("IsValid") = IsValid
				'MSGBoxCtrl.show(MSGBox.Message_title.StatusCompleted, MSGBox.Message_text.StatusCompleted, "<strong>Engineering Order</strong>", MsgBoxStyle.YesNo, "WOStatus")
				SetObject()
				SetGridObject()
				mnWO.WOStatusID = 7  'AME Completion
				mnWO.IsClosed = True
				mnWO.ClosedBy = Trim(txtClosedBy.Text)
			Else
				tmpText = "Work Order"
				'MSGBoxCtrl.show(MSGBox.Message_title.StatusCompleted, MSGBox.Message_text.StatusCompleted, "<strong>Work Order</strong>", MsgBoxStyle.YesNo, "WOStatus")
				'Session("IsValid") = IsValid

				SetObject()
				SetGridObject()
				mnWO.WOStatusID = 7   'AME Completion
				mnWO.IsClosed = True
				mnWO.ClosedBy = Trim(txtClosedBy.Text)
				If mnWO.WOCompletedDateTimeFormatted.ToString = "" Then
					mnWO.WOCompletedDateTime = DateTime.Now.ToString
				End If
			End If
			Session("mnWO") = mnWO
			If IsIssuedSparesReturned() = 2 Then
				MSGBoxCtrl.Show("Alert!", "You have not mentioned Used Qty. <Br> We consider Issued Qty is wholly used? <Br><Br> Do you want to continue?", "", MsgBoxStyle.YesNo, "IsIssuedSparesReturned")
				Session("IsValid") = True
				'Session("sender") = "IsIssuedSparesReturned"
				Exit Sub
			End If

			'Added By Saylee On 4-Mar-2020 For Approval Reject history
			mnWOApproveReject = nWOApproveReject.NewApproval(mnWO.ID)

			If (AppSettings("ClientCode") = "IND") Then
				mnWOApproveReject.Date = CType(txtCloseDate.Text.ToString.Trim + " " + txtClosedDateTime.Text.ToString.Trim, DateTime)
			Else
				mnWOApproveReject.Date = CDate(txtCloseDate.Text.ToString.Trim)
			End If



			mnWOApproveReject.ApprovedRejectStatus = 1
			mnWOApproveReject.Remark = txtStatusRemark.Text
			mnWOApproveReject.WOStatusID = 7

			mWODetail = "AME Completion Stage " & mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + "AME Completion By : "
			Session("WODetailForMarkLog") = mWODetail

			Session("mnWOApproveReject") = mnWOApproveReject
			'**************************************************************
			If Save() Then
				SetPage()
				SetGrid()
				SetNRCGrid()
				ControlVisibility()
				UpdatePanels()
				upnlJobType.Update()
				mWODetail = mnWO.WOStatus + ": " + mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy + IIf(Not mnWO.MachineID.Equals(Guid.Empty), " Aircraft : " + mnWO.RegNo, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
				MarkLog(Action.AMEComplete, "Work Order", mWODetail, ErrorType.NoError, mnWO.ID, EventLogID)
				Response.Redirect("index.aspx")
			End If
		Else
			upnlValidationsummary.Update()
		End If
	End Sub

	Private Sub btnReject_Click(sender As Object, e As EventArgs) Handles btnReject.Click
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If


		If txtStatusRemark.Text = "" Then
			MSGBoxCtrl.Show("Alert!", "Please enter the Remark before rejecting a Work Order", "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If

		'Added By Saylee On 4-Mar-2020 For Approval Reject history
		mnWOApproveReject = nWOApproveReject.NewApproval(mnWO.ID)

		If (AppSettings("ClientCode") = "IND") Then
			mnWOApproveReject.Date = CType(DateTime.Now.ToString.Trim, DateTime)
		Else
			mnWOApproveReject.Date = CDate(DateTime.Now.ToString.Trim)
		End If

		mnWOApproveReject.ApprovedRejectStatus = 2
		mnWOApproveReject.Remark = txtStatusRemark.Text




		'**************************************************************

		If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID And mnWO.StatusID = 1 Then
			mnWOApproveReject.WOStatusID = 2
			mWODetail = "Rejected at Creation Stage " & mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Rejected By : "
		ElseIf Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Then
			mnWOApproveReject.WOStatusID = 4

			mWODetail = "Rejected at Planning Stage " & mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Rejected By : "
		ElseIf Session("MiddleFrame") = "wfnWOExecutionList.aspx" Then
			mnWOApproveReject.WOStatusID = 7

		ElseIf Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=" & mnWO.TransTypeID Then
			'do nothing
		ElseIf Session("MiddleFrame") = "wfnWOCompletionList.aspx?" Then
			mnWOApproveReject.WOStatusID = 3
			mWODetail = "Rejected at PPC Completion Stage " & mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Rejected By : "

		ElseIf Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" Then
			mnWOApproveReject.WOStatusID = 5

			mWODetail = "Rejected at QC Stage " & mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Rejected By : "

		ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1" Then
			mnWOApproveReject.WOStatusID = 8

			mWODetail = "Rejected at CAMO Update Stage " & mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Rejected By : "
		ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0" Then 'billing
			'do nothing
			mnWO.WOStatusID = 3 'reverted to AME state
			mWODetail = "Rejected at Billing Stage " & mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Rejected By : "
		End If

		Session("WODetailForMarkLog") = mWODetail
		Session("IsValid") = IsValid
		Session("mnWO") = mnWO
		Session("mnWOApproveReject") = mnWOApproveReject
		MSGBoxCtrl.show(MSGBox.Message_title.RejectWO, MSGBox.Message_text.RejectWO, "<strong>Work Order</strong>", MsgBoxStyle.YesNo, "WOStatus")

	End Sub

	Private Sub lnkCreateMultipleRequisitionOfTaskSpares_Click(sender As Object, e As EventArgs) Handles lnkCreateMultipleRequisitionOfTaskSpares.Click 'Added By Prashant on 31-Aug-2020 STR28082020
		If (AppSettings("ClientCode") <> "STR" And Not User.IsInRole("EngineeringRequisitionNew")) Or (AppSettings("ClientCode") = "STR" And ((mnWO.WOJobs(0).WOJobTypeID = 1 And Not User.IsInRole("PlanningRequisitionNew")) Or (mnWO.WOJobs(0).WOJobTypeID <> 1 And Not User.IsInRole("EngineeringRequisitionNew")))) Then 'For Star Air For Unscheduled Job create Planning Req and for other jobs create Engg. Req.
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		For i As Integer = 0 To mnWO.WOJobs.Count - 1                   'Work Order Job Count
			For j As Integer = 0 To mnWO.WOJobs(i).WOJobTasks.Count - 1 'Work Order Job Tasks Count
				If mnWO.WOJobs(i).WOJobTypeID = 2 Then                  'Work Order Job Scheduled
					If mnWO.WOJobs(i).WOJobTasks(j).WOJobTaskSpares.Count = 0 Then
						'Do nothing
					Else
						mRequisitionNew = RequisitionNew.NewRequisition(Trans.PlanningRequisition)
						'mRequisitionNew.ReqDate = mnWO.WODate
						mRequisitionNew.ReqDate = Today.Date 'Changed by Prashant on 10-Jan-2022 STR10012022
						mRequisitionNew.LocationID = mWorkShopList(mnWO.WorkShopID).locationID
						mUser = SI.UTILITY.User.GetUser(User.Identity.Name)
						mRequisitionNew.EmployeeID = mUser.EmployeeID
						mRequisitionNew.ReqTypeID = 1 'Added by Prashant 20-Oct-2020 STR20102020.Add Requisition Type as “Part Purchase or Part Request” in Planning Requisition module
					End If
				Else
					Exit Sub
				End If
				For k As Integer = 0 To mnWO.WOJobs(i).WOJobTasks(j).WOJobTaskSpares.Count - 1
					Dim mItem As Item
					mItem = Item.GetItem(ID:=mnWO.WOJobs(i).WOJobTasks(j).WOJobTaskSpares(k).ItemID)
					If mItem.Name <> "" Then
						mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, Guid.Empty)
						mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = mnWO.WOJobs(i).WOJobTasks(j).WOJobTaskSpares(k).ItemID
						mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo = mnWO.WOJobs(i).WOJobTasks(j).WOJobTaskSpares(k).PartNo
						mRequisitionNew.RequisitionItemsNew.CurrentItem.Description = mnWO.WOJobs(i).WOJobTasks(j).WOJobTaskSpares(k).Description
						mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference = mItem.IPCReference
						If mnWO.WOJobs(i).WOJobTasks(j).WOJobTaskSpares(k).RequiredQty <= 0 Then
							mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty = 1
						Else
							mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty = mnWO.WOJobs(i).WOJobTasks(j).WOJobTaskSpares(k).RequiredQty
						End If
						mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID = mItem.UnitID
						mRequisitionNew.RequisitionItemsNew.CurrentItem.Unit = mItem.UnitName
						mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase = mItem.IsOneTimePurchase
						mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = mnWO.MachineID
						mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = mnWO.RegNo
						mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = mnWO.ID
						mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = mnWO.WONumber
						If Not mItem.IsOneTimePurchase Then
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = mItem.MinStockLevel
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = mItem.MaxStockLevel
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = mItem.MinReOrderLevel
						Else
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = 0
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = 0
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = 0
						End If
					End If
				Next
				If (mnWO.WOJobs(i).WOJobTasks(j).WOJobTaskSpares.Count = 0 Or mRequisitionNew.RequisitionItemsNew.Count = 0) Then
					'Do nothing
				Else
					mRequisitionNew.Save()
					mAutoCreatedReqCount = mAutoCreatedReqCount + 1
					If mAutoCreatedReqCount = 1 Then
						NumberOfMultipleRequisitionOfTaskSparesDetails.Append("Work Order No: " + mnWO.WOText + "-" + mnWO.WONo.ToString + " ")
						mFromRequisitionNo = mRequisitionNew.Text.ToString & "-" & mRequisitionNew.No.ToString
					Else
						mToRequisitionNo = mRequisitionNew.Text.ToString & "-" & mRequisitionNew.No.ToString
					End If
					NumberOfMultipleRequisitionOfTaskSparesDetails.Append(mRequisitionNew.Text.ToString & "-" & mRequisitionNew.No.ToString + " Dated : " + mRequisitionNew.ReqDateFormatted.ToString + Chr(13))
					'MarkLog(Action.[New], "Planning Requisition", NumberOfMultipleRequisitionOfTaskSparesDetails.ToString, ErrorType.NoError, mRequisitionNew.ID, EventLogID)
				End If
			Next
		Next
		MarkLog(Action.[New], "Planning Requisition", NumberOfMultipleRequisitionOfTaskSparesDetails.ToString, ErrorType.NoError, mRequisitionNew.ID, EventLogID)
		ShowMessage(mFromRequisitionNo, mToRequisitionNo, mRequisitionNew.ReqDateFormatted.ToString, mAutoCreatedReqCount)
	End Sub

	Private Sub chkIsReInspection_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsReInspection.CheckedChanged
		chkIsIndependentInspection.Checked = False
		upnlMaintComplainceDetails.Update()
	End Sub

	Private Sub chkIsIndependentInspection_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsIndependentInspection.CheckedChanged
		chkIsReInspection.Checked = False
		upnlMaintComplainceDetails.Update()
	End Sub

	Private Sub chkSupplementalSheetAttached_CheckedChanged(sender As Object, e As EventArgs) Handles chkSupplementalSheetAttached.CheckedChanged
		If chkSupplementalSheetAttached.Checked Then
			txtNoOfSupplementalSheets.ReadOnly = False
			txtNoOfSupplementalSheets.BackColor = Color.White
		Else
			txtNoOfSupplementalSheets.ReadOnly = True
			txtNoOfSupplementalSheets.Text = "0"
			txtNoOfSupplementalSheets.BackColor = Color.LightGray
		End If
		upnlMaintComplainceDetails.Update()
	End Sub

	Private Sub chkNRCRaised_CheckedChanged(sender As Object, e As EventArgs) Handles chkNRCRaised.CheckedChanged
		If chkNRCRaised.Checked Then
			txtNoOfNRCs.ReadOnly = False
			txtNoOfNRCs.BackColor = Color.White
		Else
			txtNoOfNRCs.ReadOnly = True
			txtNoOfNRCs.Text = "0"
			txtNoOfNRCs.BackColor = Color.LightGray
		End If
		upnlMaintComplainceDetails.Update()
	End Sub

	Private Sub lnkCloseALLJobs_Click(sender As Object, e As EventArgs) Handles lnkCloseALLJobs.Click
		''   ScriptManager.RegisterStartupScript(Me, [GetType], "ShowJobClosing", "ShowJobClosing();", True)
		mdlPopUpChangeCloseAll.Show()
	End Sub

	Private Sub txtJobStartDate_TextChanged(sender As Object, e As EventArgs) Handles txtJobStartDate.TextChanged, txtJobEndDate.TextChanged
		mdlPopUpChangeCloseAll.Show()
	End Sub

	Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
		mdlPopUpChangeCloseAll.Hide()
	End Sub

	Private Sub txtJobStartDateTime_TextChanged(sender As Object, e As EventArgs) Handles txtJobStartDateTime.TextChanged
		If IsValidTime(txtJobStartDateTime.Text.ToString.Trim) = False Then
			txtJobStartDateTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		End If
	End Sub

	Private Sub txtEndDateTime_TextChanged(sender As Object, e As EventArgs) Handles txtJobEndDateTime.TextChanged
		If IsValidTime(txtJobEndDateTime.Text.ToString.Trim) = False Then
			txtJobEndDateTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		End If
	End Sub

	Private Function CustomValidateJob() As Boolean
		Dim strMSG As String = ""
		If txtJobStartDate.ToString = "" And txtJobEndDate.ToString = "" Then
			strMSG = strMSG + "Actual Start Date required" & "<BR>" & "Actual End Date required" + "<br>" 'mWO.GetBrokenRulesString
		ElseIf txtJobStartDate.Text = "" Then
			strMSG = strMSG + "Actual Start Date required" + "<br>"  'mWO.GetBrokenRulesString

		ElseIf txtJobEndDate.Text = "" Then
			strMSG = strMSG + "Actual End Date required" + "<br>" 'mWO.GetBrokenRulesString
		End If

		If IsDate(CType(mnWO.WODate.ToString, String)) Then
			If txtJobStartDate.Text <> "" Then
				If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "BAP" Then
					If CDate(CDate(txtJobStartDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) Then
						strMSG = strMSG + "Actual Start Date should be Greater than Work Order Date." + "<br>"
					ElseIf txtJobStartDate.Text <> "" And txtJobEndDate.Text <> "" Then
						If CDate(txtJobStartDate.Text) > CDate(txtJobEndDate.Text) Then
							strMSG = strMSG + "Actual Start Date cannot be Greater than Actual End Date." + "<br>"
						End If
					ElseIf txtJobStartDate.Text <> "" And IsDate(CType(mnWO.WOStartDate.ToString, String)) Then 'Added by Saylee
						If CDate(txtJobStartDate.Text) < CDate(CType(mnWO.WOStartDate.ToString, String)) Then
							strMSG = strMSG + "Actual Start Date should be equal to or Greater than Work Order Start Date." + "<br>"
						End If
					End If
				Else
					If CDate(txtJobStartDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then
						If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
							strMSG = strMSG + "Actual Start Date should be Greater than E.O. Date." + "<br>"
						Else
							strMSG = strMSG + "Actual Start Date should be Greater than Work Order Date." + "<br>"
						End If
					ElseIf txtJobStartDate.Text <> "" And txtJobEndDate.Text <> "" Then
						If CDate(txtJobStartDate.Text) > CDate(txtJobEndDate.Text) Then
							strMSG = strMSG + "Actual Start Date cannot be Greater than Actual End Date." + "<br>"
						End If
					ElseIf txtJobStartDate.Text <> "" And IsDate(CType(mnWO.WOStartDate.ToString, String)) Then 'Added by Saylee
						If CDate(txtJobStartDate.Text) < CDate(CType(mnWO.WOStartDate.ToString, String)) Then
							If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
								strMSG = strMSG + "Actual Start Date should be equal to or Greater than E.O. Start Date." + "<br>"
							Else
								strMSG = strMSG + "Actual Start Date should be equal to or Greater than Work Order Start Date." + "<br>"
							End If
						End If
					End If
				End If
			End If
		ElseIf txtJobStartDate.Text <> "" And txtJobEndDate.Text <> "" Then
			If CDate(txtJobStartDate.Text) > CDate(txtJobEndDate.Text) Then
				strMSG = strMSG + "Actual Start Date cannot be Greater than Actual End Date." + "<br>" 'mWO.GetBrokenRulesString
			Else
			End If
		ElseIf txtJobEndDate.Text <> "" And IsDate(CType(mnWO.WODate.ToString, String)) Then
			If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "BAP" Then
				If CDate(CDate(txtJobEndDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) Then
					strMSG = strMSG + "Actual End Date should be Greater than Work Order Date." + "<br>"
				End If
			Else

				If CDate(txtJobEndDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then
					If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
						strMSG = strMSG + "Actual End Date should be Greater than E.O. Date." + "<br>"
					Else
						strMSG = strMSG + "Actual End Date should be Greater than Work Order Date." + "<br>"
					End If

				End If
			End If
		End If
		If txtJobEndDate.Text <> "" Then
			If IsDate(CType(mnWO.WODate.ToString, String)) Then
				If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "BAP" Then
					If CDate(CDate(txtJobEndDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) Then
						strMSG = strMSG + "Actual End Date should be Greater than Work Order Date." + "<br>"
					ElseIf txtJobStartDate.Text <> "" And txtJobEndDate.Text <> "" Then
						If CDate(txtJobStartDate.Text) > CDate(txtJobEndDate.Text) Then
							strMSG = strMSG + "Actual End Date cannot be less than Actual Start Date." + "<br>"
						End If
					End If
				Else
					If CDate(txtJobEndDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then
						If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
							strMSG = strMSG + "Actual End Date should be Greater than E.O. Date."
						Else
							strMSG = strMSG + "Actual End Date should be Greater than Work Order Date." + "<br>"
						End If
					ElseIf txtJobStartDate.Text <> "" And txtJobEndDate.Text <> "" Then
						If CDate(txtJobStartDate.Text) > CDate(txtJobEndDate.Text) Then
							strMSG = strMSG + "Actual End Date cannot be less than Actual Start Date." + "<br>"

						End If
					End If
				End If
			ElseIf txtJobStartDate.Text <> "" And txtJobEndDate.Text <> "" Then
				If CDate(txtJobStartDate.Text) > CDate(txtJobEndDate.Text) Then
					strMSG = strMSG + "Actual End Date cannot be less than Actual Start Date." + "<br>"
				End If
			End If
		End If

		If strMSG.Trim <> "" Then
			CustomValidatorJob.ErrorMessage = strMSG
			CustomValidatorJob.IsValid = False
			Return False
		End If
		Return True

	End Function

	Private Sub bntCompleteAllJobs_Click(sender As Object, e As EventArgs) Handles bntCompleteAllJobs.Click
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If


		If Not CustomValidateJob() Then upnlValidationsummary1.Update() : Exit Sub

		MSGBoxCtrl.Show("Confirmation!", "Do you want to complete all job(s)?", "", MsgBoxStyle.YesNo, "CompleteAllJobs")



	End Sub

	Private Sub chkIsMSP_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsMSP.CheckedChanged
		If chkIsMSP.Checked = True Then
			SetObject()
			ScriptManager.RegisterStartupScript(Me, [GetType], "OpenMSPAssemblySelectionWindow", "OpenMSPAssemblySelectionWindow();", True)
		ElseIf chkIsMSP.Checked = False Then
			mnWO.MSPID = Guid.Empty
			mnWO.MSPAssemblyID = Guid.Empty
			mnWO.AssemblyName = ""
			mnWO.PlanName = ""
			mnWO.ContractNo = ""
			mnWO.MSPWORemark = ""
			Session("mnWO") = mnWO
			lblContractNo.DataBind()
			upnlMachineDet.Update()
		End If
	End Sub

	Private Sub hdnBtnMSPAssemblySelection_Click(sender As Object, e As EventArgs) Handles hdnBtnMSPAssemblySelection.Click
		If mnWO.MSPID.Equals(Guid.Empty) And chkIsMSP.Checked = True Then
			chkIsMSP.Checked = False
		End If
		lblContractNo.DataBind()
		upnlMachineDet.Update()
	End Sub

#End Region

#Region " Reports "

	Public Sub Print(Optional ByMail As Boolean = False,
					 Optional SignatureRequired As Boolean = False,
					 Optional HeligoCallOutPrint As Boolean = False,
					 Optional IsForDS As Boolean = False,
					 Optional IsForPrintWithJobAttachment As Boolean = False,
					 Optional IsFromSpecialWOButton As Boolean = False)

		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If Not IsInRole(Rights.Print) Then

			MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.Authorization,
							MessageText:=MSGBox.Message_text.Authorization,
							ExtraMessage:="",
							ButtonToShow:=MsgBoxStyle.OkOnly,
							Sender:="Authorization")

			Exit Sub

		End If

		Dim FormRevisionNo As String = ""
		Dim FormRevisionDate As String = ""
		Dim da As New ObjectAdapter
		Dim myReport As Engine.ReportDocument
		Dim mCompanyDetail As New CompanyDetail
		Dim ds As New dsnWODetail
		Dim mnWOJobs As nWOJobs
		Dim mnWOJobComps As nWOJobComps
		Dim mnWOJobSpares As nWOJobSpares 'Added By Saylee on 20-Sep-2019 HSC20092019
		Dim mnWOJobDesignationAllocations As nWOJobDesignationAllocations 'Added By Vikrant On 24-June-2013 For Indamer21062013
		Dim mnWONRCJobs As nWOJobs
		Dim WOJobActions As nWOJobActions
		Dim WODocumentNo As String = ""
		Dim WORevisionNo As String = ""
		Dim FormNo As String = ""
		Dim IssueNo As String = ""
		Dim IssueDate As String = ""
		Dim Searchstr7 As String = ""
		Dim LastLogDate As String = ""
		Dim LastLogDateHavingAPUValues As String = ""
		Dim ReportTitle As String = "AIRCRAFT WORK ORDER"
		Dim _ModuleInfo As ModuleList
		Dim MaintainanceOrganization As String
		Dim mAirCraftManufacturerName As String = ""   'Sankalp
		Dim mEnginManufacturerName As String = ""      'Sankalp
		Dim mTaskReference As String = ""       'Sankalp
		Dim mAMPRev As String = ""       'Sankalp 
		Dim mAMPDate As String = ""       'Sankalp 
		If AppSettings("ShowCAMOOnlyForNewClients") = "False" And
		   AppSettings("ShowAMOOnlyForNewClients") = "False" Then
			ReportTitle = "AIRCRAFT WORK ORDER"
		Else
			ReportTitle = "WORK ORDER"
		End If

		Dim EOFooterLine As String = ""
		Dim mnWORegisterList As nWORegisterList

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso
		   (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then EOFooterLine = CType(AppSettings("EOFooterLine"), String)

		If AppSettings("ClientCode") = "Indamer" Then
			myReport = New crnWODetailForIndamar 'added By Saylee ON 05-April-2013 FOR Indamar04104013
		ElseIf (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then

			If mnWO.WOJobs.IsScheduledJobExists Then
				myReport = New crnWODetailForTAALLandscapeSch
			ElseIf mnWO.WOJobs.IsUnScheduledJobExists Then
				myReport = New crnWODetailForTAALLandscapeUnSch
			Else
				myReport = New crnWODetailForTAALLandscape
			End If

		ElseIf AppSettings("ClientCode") = "DOL" Then
			myReport = New crnWODetailForDolphin
		ElseIf AppSettings("ClientCode") = "HL" Then

			If mnWO.EngCount > 2 Then
				myReport = New crnWODetailForHeavyLiftFormat2
			Else
				myReport = New crnWODetailForHeavyLiftFormat1
			End If

		ElseIf AppSettings("ClientCode") = "RAL" Then

			LastLogDate = MaxLogOfAircraft.GetMaxLogOfAircraft(mnWO.MachineID,
															   IsLastLogWithValuesRequired:=True,
															   AssemblyTypeID:=1,
															   WODate:=mnWO.WODateFormatted.ToString).LogDateFormatted.ToString

			LastLogDateHavingAPUValues = MaxLogOfAircraft.GetMaxLogOfAircraft(mnWO.MachineID,
																			  IsLastLogWithValuesRequired:=True,
																			  AssemblyTypeID:=4,
																			  WODate:=mnWO.WODateFormatted.ToString).LogDateFormatted.ToString

			myReport = New crnWOIssueDetail 'New addition by Saylee on 25-July-2011

		ElseIf (AppSettings("ClientCode") = "BA") Then
			myReport = New crnWODetailForBA
		ElseIf (AppSettings("ClientCode") = "Novo") Then 'Added by Saylee on 23-Jan-2018 for NOVO23012018
			myReport = New crnWODetailForNOVO 'Added by Saylee on 23-Jan-2018 for NOVO23012018
		ElseIf AppSettings("ClientCode") = "TA" Then
			myReport = New crnWODetailReportYATA
			'Added By Utkarsh ON 30-Nov-2012 FOR ALL30112012-1
		ElseIf AppSettings("ClientCode") = "IIC" Then
			myReport = New crnWODetailForDeccan
		ElseIf AppSettings("ClientCode") = "Deccan" Then ' SPZ Code added by Saylee on 13-Jun-2022 
			myReport = New crnWOIssueDetailForDeccan 'Added by Vikrant For Deccan03022021
		ElseIf AppSettings("ClientCode") = "ADeccan" Then
			myReport = New crnWODetailForAirDeccan
		ElseIf AppSettings("ClientCode") = "FG" Or AppSettings("ClientCode") = "JA" Then
			myReport = New crnWODetailForFG  'Added By Vikrant On 15-May-2013 FOR FGA15052013
			'End
			'Added By Shweta on 11-Sep-2013 For UHPL11092013-1
		ElseIf AppSettings("ClientCode") = "UHPL" Then
			myReport = New crnWODetailForUHPL
			'End
		ElseIf AppSettings("ClientCode") = "Heligo" Then  'Added By Prashant 9-Jun-2014 HELIGO09062014

			'Added by Saylee on 20-Nov-2020 for Heligo20112020
			If HeligoCallOutPrint = True And mnWO.TransTypeID = 89 Then ' CAMO-Work Order
				myReport = New crnWODetailForHeligoCallOut
			Else
				myReport = New crnWODetailForHeligo
			End If

		ElseIf AppSettings("ClientCode") = "TP" Then 'Added By Vikrant On 06-Jun-2016 For TP06062016
			myReport = New crnWODetailForTP
		ElseIf (AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA") Then 'Added by saylee on 13-Jun-2016

			myReport = New crnWODetailForBIRD
			'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
			da.Fill(ds, mnWO.WOTools)
			'End

		ElseIf AppSettings("ClientCode") = "GEP" Then
			myReport = New crnWODetailGEP
		ElseIf AppSettings("ClientCode") = "RBH" Then 'Added by Saylee on 17-Nov-2017 for RBH

			mnWORegisterList = nWORegisterList.GetnWORegisterList(mnWO.WOText,
																  mnWO.WONo, , ,
																  mnWO.RegNo, ,
																  mnWO.SerialNo)

			da.Fill(ds, mnWORegisterList)
			da.Fill(ds, mnWO.WOTools)
			da.Fill(ds, nWOJobSpares.GetWOSpares(mnWO.ID, ""))
			myReport = New crnWODetailForRBH

		ElseIf AppSettings("ClientCode") = "STR" Then 'Added by Vikrant On 09-May-2018 For STR09052018

			myReport = New crnWOIssueDetailForStarAir
			da.Fill(ds, "nIssuedWOSpares", Session("mnIssuedWOSpareswfnWODetail")) 'Added By Prashant 13-Oct-2020 STR12102020 Again change on 26-Nov-2020
			da.Fill(ds, "nIssuedWOTools", Session("mIssuedWOToolswfnWODetail")) 'Added By Prashant 13-Oct-2020 STR12102020 Again change on 26-Nov-2020

		ElseIf AppSettings("ClientCode") = "DHL" Then 'Added By Prashant 27-Sep-2018 DHILLON27092018
			myReport = New crnWODetailForDhillon
		ElseIf AppSettings("ClientCode") = "APFT" Then 'Added by Saylee on 29-Nov-2018 for APFT
			myReport = New crnWODetailForAPFT
		ElseIf AppSettings("ClientCode") = "ASH" Then 'Added by Saylee on 18-Feb-2019 for ASHLEY for ASH18022019
			myReport = New crnWODetailForASHLEY
			mnWORegisterList = nWORegisterList.GetnWORegisterList(mnWO.WOText, mnWO.WONo, , , mnWO.RegNo, , mnWO.SerialNo)
			da.Fill(ds, mnWORegisterList)
		ElseIf AppSettings("ClientCode") = "KLP" Then 'Added by Saylee on 4-APR-2019 for Kelachandra Logistics Private Limited for KLP04042019
			myReport = New crnWODetailForKLP
		ElseIf AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "UYA" Then 'Added by Saylee on 17-JUN-2019 for Indamar for IND14062019 'UYA Added By Vikrant On 14-Jul-2020 For ALL14072020 UYA needs same values like IND so used sama patch

			If AppSettings("ClientCode") = "IND" Then
				myReport = New crnWODetailForIND
			ElseIf AppSettings("ClientCode") = "UYA" Then 'Added By Vikrant On 14-Jul-2020 For ALL14072020
				myReport = New crnWODetailForUYA
			End If
			'IND

			If mnWO.StatusID = 2 And mnWO.WOStatusID = 3 Then 'Only Completed WO

				Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
				Dim mAssemblyStatusList As AssemblyStatusList

				CompletedByUserLicenceNos = mUser.LicenseNo
				mnWO = nWO.GetWO(mnWO.ID, AllWOJobType:=False, getAircraftValuesAsOnCompletionDate:=True)

				mAssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(mnWO.MachineID,
																			   AssemblyType:="Airframe",
																			   CurrentDate:=mnWO.WOCloseDateFormatted.ToString,
																			   IsAssemblyInstalled:=True)

				If mAssemblyStatusList.Count > 0 Then

					If mAssemblyStatusList(0).AssemblyStatusPeriodList(1, "") IsNot Nothing Then

						AirframeHrsAsOnCompletionDate = mAssemblyStatusList(0).AssemblyStatusPeriodList(1, "").AssemblyCurrentValue

					End If

					If mnWO.Cycles <> "" Then 'Same Formula used here as that of IND crystal rpeort

						If mAssemblyStatusList(0).AssemblyStatusPeriodList(3, "") IsNot Nothing Then
							AFAllPeriodsAsOnCompletionDate = mAssemblyStatusList(0).AssemblyStatusPeriodList(3, "").AssemblyCurrentValue + " C" + IIf(mnWO.AFAllPeriodsAsOnWOCompletionDate = "", "", Chr(13) + mnWO.AFAllPeriodsAsOnWOCompletionDate)
						End If

					Else

						If mAssemblyStatusList(0).AssemblyStatusPeriodList(7, "") IsNot Nothing Then
							AFAllPeriodsAsOnCompletionDate = mAssemblyStatusList(0).AssemblyStatusPeriodList(7, "").AssemblyCurrentValue + " L" + IIf(mnWO.AFAllPeriodsAsOnWOCompletionDate = "", "", Chr(13) + mnWO.AFAllPeriodsAsOnWOCompletionDate)
						End If

					End If

				End If

				Session("mnWO") = mnWO

			End If
			'End

		ElseIf AppSettings("ClientCode") = "LNT" Then 'Added by Saylee on 17-JUN-2019 for LNT for LNT17062019
			myReport = New crnWODetailForLNT
		ElseIf AppSettings("ClientCode") = "HSC" Then
			myReport = New crnWOIssueDetailForHeliStar
		ElseIf AppSettings("ClientCode") = "IIC" Then 'Added by Saylee on 16-JUL-2019 for LNT for IIC16072019
			myReport = New crnWODetailForIIC
		ElseIf AppSettings("ClientCode") = "PAS" Then 'Added by Prashant on 22-Aug-2019 for Passion for PAS22082019
			myReport = New crnWODetailPassion
		ElseIf AppSettings("ClientCode") = "SUH" Then
			myReport = New crnWODetailForSUH
		ElseIf AppSettings("ClientCode") = "FAP" Then 'Added by Prashant on 10-Sep-2019 for Fiducia Aviation Pvt. Ltd. for Fiducia10092019
			myReport = New crnWODetailFAP
		ElseIf AppSettings("ClientCode") = "PNW" Then 'Added by Prashant on 2-Dec-2019 for Poonawalla aviation
			myReport = New crnWODetailForPNW
		ElseIf AppSettings("ClientCode") = "HNS" Then
			myReport = New crnWODetailForSAFAL
		ElseIf AppSettings("ClientCode") = "Dana" Then 'Added By Prashant on 20-May-2021 DANA20052021
			myReport = New crnWODetailForDana
		ElseIf AppSettings("ClientCode") = "GMP" Then 'Added By Saylee on 26-Aug-2021 GMP26082021
			myReport = New crnWODetailForGMP
		ElseIf AppSettings("ClientCode") = "BLUE" Then 'Added By Saylee on 24-Sep-2021 BLUE24092021
			myReport = New crnWODetailForBlueRay
		ElseIf AppSettings("ClientCode") = "IRM" Then 'Added By Saylee on 22-Oct-2021 IRM22102021
			IssueNo = AppSettings("WOIssueNo")
			myReport = New crnWODetailForIRM
		ElseIf AppSettings("ClientCode") = "FBW" Then
			myReport = New crnWODetailForFBW
		ElseIf AppSettings("ClientCode") = "IPA" Then '''Added By Saylee - Indo Pacific
			myReport = New crnWODetailForIPA
		ElseIf AppSettings("ClientCode") = "TSL" Then
			myReport = New crnWODetailForTSL
			IssueNo = AppSettings("WOIssueNo")
		ElseIf AppSettings("ClientCode") = "SAA" Then '''Added By Prashant 28-Mar-2022
			myReport = New crnWODetailForSaurya
		ElseIf AppSettings("ClientCode") = "SPZ" Then '''Added By Prashant 7-Jun-2022
			myReport = New crnWODetailForSparzana
		ElseIf AppSettings("ClientCode") = "SHN" Then '''Added By Ajay 6-Sep-2022
			'myReport = New crnWODetailForShivan
			myReport = New crnWODetailForSHN
		ElseIf AppSettings("ClientCode") = "GUN" Then '''Added By Prashant 19-Jan-2023
			myReport = New crnWODetailForGuna
		ElseIf AppSettings("ClientCode") = "MEL" Then '''Added By Ajay 3-May-2023
			myReport = New crnWODetailForMEL
		ElseIf AppSettings("ClientCode") = "ACI" Then '''Added By Prashant 25-Jan-2023
			myReport = New crnWOWorkPackForACI
		ElseIf AppSettings("ClientCode") = "RED" Then 'Added by Sachin on 09-FEb-2024 for RED Bird
			myReport = New crnWODetailRED
		ElseIf AppSettings("ClientCode") = "KZN" Then 'Added by Sachin on 09-FEb-2024 for RED Bird
			myReport = New crnWOJobDetailKZN
		ElseIf AppSettings("ClientCode") = "SHR" Then 'Added by Prashant on 3-Apr-2024
			myReport = New crnWODetailForShraddha
		ElseIf AppSettings("ClientCode") = "GLD" Then '''Added By Harsh on 3rd April 2024 -- New Report for GOLDEN CRANE AVIATION 

			myReport = New crnWODetailForGolden
			If mnWO.TransTypeID = 88 Then
				ReportTitle = "CAMO WORK ORDER (One-time)"  'Third party
			Else
				ReportTitle = "CAMO WORK ORDER"
			End If

		ElseIf AppSettings("ClientCode") = "MSPL" Then 'Added by Harsh on 17th April 2024 for FLYPAL-1572 ( New W.O. Report for MSPL ) 
			myReport = New crnWODetailMSPL
			LastLogDate = MaxLogOfAircraft.GetMaxLogOfAircraft(mnWO.MachineID, IsLastLogWithValuesRequired:=True, AssemblyTypeID:=1, WODate:=mnWO.WODateFormatted.ToString).LogDateFormatted.ToString
		ElseIf AppSettings("ClientCode") = "SAP" Then 'Added by Harsh on 25th May 2024 for FLYPAL-1648 New W.O. Report for Sapphire Airline 
			myReport = New crnWODetailForSapphire
		ElseIf AppSettings("ClientCode") = "AFC" Then 'Added by Sachin on 24th May 2024 for FLYPAL-1572 ( New W.O. Report for Afcom ) 
			myReport = New crnWODetailForAfcom
		ElseIf AppSettings("ClientCode") = "MYT" Then 'Added by Sachin on 24th May 2024 for FLYPAL-1572 ( New W.O. Report for Mytri ) 
			myReport = New crnWODetailForMytri
		ElseIf AppSettings("ClientCode") = "PTW" Then

			If mnWO.TransTypeID = 102 Then
				myReport = New crnEngWODetailForPattaya 'Added by Harsh Sugandhi on 26th June 2024 for FLYPAL-1714 Engineering Order Report for Pattaya Airways
			Else
				myReport = New crnWODetailForPattaya 'Added by Prashant on 26th June 2024 for FLYPAL-1714 Work Order Report for Pattaya Airways
				da.Fill(ds, "nIssuedWOSpares", Session("mnIssuedWOSpareswfnWODetail"))
				da.Fill(ds, "nIssuedWOTools", Session("mIssuedWOToolswfnWODetail"))
			End If

			'Added Specifically for Pattaya as the fields from TransType Table were already occupied
			_ModuleInfo = ModuleList.GetModuleList(ModuleName:="CAMO Work Order")
			MaintainanceOrganization = _ModuleInfo.Item(0).FormRevisionNo

		ElseIf AppSettings("ClientCode") = "ARA" Then
			myReport = New crnWODetailForARAirways
		ElseIf AppSettings("ClientCode") = "FIT" Then

			If mnWO.TransTypeID = 102 Then
				ReportTitle = "ENGINEERING ORDER"
			ElseIf IsFromSpecialWOButton = True Then 'added by Saylee on 13-Nov-2024, for FIT
				ReportTitle = "SPECIAL WORK CARD (SWC)"
			Else
				ReportTitle = "WORK ORDER"
			End If

			myReport = New crnWODetailFitsAir

		ElseIf AppSettings("ClientCode") = "CAI" Then   'Added by Harsh on 27th August 2024 for FLYPAL-1846
			myReport = New crnWODetailForCarwell
		ElseIf AppSettings("ClientCode") = "BAP" Then   'Added  by Harsh Sugandhi on 5th September 2024 for FLYPAL-1874 
			myReport = New crnWODetailForBharat
		ElseIf AppSettings("ClientCode") = "AAP" Then   'Added  by Harsh Sugandhi on 21th October 2024 for FLYPAL-1973 WO report for AVYANNA Aviation
			myReport = New crnWODetailForAvyanna
		ElseIf AppSettings("ClientCode") = "DAD" Then   'Added  by Prashant on 2-Jan-2025
			myReport = New crnWODetailForDadachandji
		ElseIf AppSettings("ClientCode") = "YA" Then
			myReport = New crnWODetailReportYeti
		ElseIf AppSettings("ClientCode") = "RAJ" Then      'Added Sankalp
			myReport = New crnWODetailForRAJ                    'Added by Sankalp
		ElseIf AppSettings("ClientCode") = "SIT" Then      'Added Sankalp
			myReport = New crnWODetailForSIT                    'Added by Sankalp
		ElseIf AppSettings("ClientCode") = "SKY" Then      'Added Sankalp

			Dim mtmpMachineList As tmpMachineList
			Dim ReportStatusList As New rptStatusList

			mtmpMachineList = tmpMachineList.GetMachineList(, mnWO.RegNo, , , , , True, mnWO.WODate.ToString)
			mAirCraftManufacturerName = mtmpMachineList(0).ManufacturerName
			mEnginManufacturerName = mnWO.Eng1Manufacture
			mTaskReference = mnWO.WOJobs(0).TaskSourceRef

			myReport = New crnWODetailForSKY
			'Added by Sankalp
		ElseIf AppSettings("ClientCode") = "CVA" Then      'Added Sankalp

			Dim mLastAMPRef As LastMPDAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(mnWO.MachineID)
			If (mLastAMPRef.AMPNo = "") Then
			Else
				mAMPRev = "AMP No.: " + mLastAMPRef.AMPNo + " ,Rev No.: " + mLastAMPRef.RevNo
				mAMPDate = "Dated: " + mLastAMPRef.FromDateFormatted
			End If

			myReport = New crnWODetailForCVA                    'Added by Sankalp
		ElseIf AppSettings("ClientCode") = "RGP" Then      'Added by Harsh
			myReport = New crnWODetailForRithwik
		Else
			myReport = New crnWODetail
		End If

		mnWO = Session("mnWO")

		If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "SHN" Then
			mnWO = nWO.GetWO(mnWO.ID, getAircraftValuesAsOnCompletionDate:=True)
		End If

		mnWOJobs = mnWO.WOJobs
		mnWOJobComps = nWOJobComps.GetWOJobComps(mnWO.ID, "")
		mnWOJobSpares = nWOJobSpares.GetWOSpares(mnWO.ID, "") 'Added By Saylee on 20-Sep-2019 HSC20092019
		mnWOJobDesignationAllocations = nWOJobDesignationAllocations.GetWOJobDesignationAllocations(mnWO.ID, "")  'Added By Vikrant On 24-June-2013 For Indamer21062013
		WOJobActions = nWOJobActions.GetWOJobActionsByWOID(mnWO.ID)

		If (AppSettings("ClientCode") = "BA" Or
			AppSettings("ClientCode") = "Novo" Or
			AppSettings("ClientCode") = "YA" Or
			AppSettings("ClientCode") = "TA" Or
			AppSettings("ClientCode") = "BRD" Or
			AppSettings("ClientCode") = "LAMA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

			' Added By Vikrant On 13-May-2013 For BA13052013
			mnWORegisterList = nWORegisterList.GetnWORegisterList(mnWO.WOText, mnWO.WONo, , , mnWO.RegNo, , mnWO.SerialNo)
			da.Fill(ds, mnWORegisterList)
			'End

		ElseIf AppSettings("ClientCode") = "Indamer" Then  'Added By Vikrant On 14-May-2013 For IND14052013

			Dim mtmpMachineList As tmpMachineList
			Dim ReportStatusList As New rptStatusList

			mtmpMachineList = tmpMachineList.GetMachineList(, mnWO.RegNo, , , , , True, mnWO.WODate.ToString)

			For i As Integer = 0 To mtmpMachineList.Count - 1

				ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString,
													1, , , , , , , , , , , , , , , ,
													mtmpMachineList(i).Cycles, , ,
													Year(New SmartDate(mnWO.WODate.ToString).FormattedText).ToString, ,
													mtmpMachineList(i).RegNo,
													mtmpMachineList(i).ModelName,
													mtmpMachineList(i).Type,
													mtmpMachineList(i).SerialNo,
													mtmpMachineList(i).ManufacturerName, ,
													mtmpMachineList(i).ManufacturingDate,
													mtmpMachineList(i).Hours,
													mtmpMachineList(i).Landings))

			Next

			da.Fill(ds, ReportStatusList)

		End If

		'Added by Saylee on 11-Oct-2018 for ALL11102018
		If mnWO.IsDigitalSignatureAdded Then

			mFileAttachnWO = FileAttach.GetAttachment(mnWO.ID, ,
													  "DigitalSignatureWO",
													  ds,
													  AppSettings("DOCPath"))

			da.Fill(ds, "FileAttach", mFileAttachnWO)

		End If
		'***************************
		Dim EmpName As String = ""

		Dim mEmployee As Employee

		If Not mnWO.EmployeeID.Equals(Guid.Empty) Then
			mEmployee = Employee.GetEmployee(mnWO.EmployeeID)
			EmpName = mEmployee.Name       'SearchStr15
		End If

		Dim EmployeeName As String = ""

		If mnWO.AuthorizedBy = "" Then
			EmployeeName = ""
		Else
			EmployeeName = mnWO.AuthorizedBy
		End If

		If AppSettings("ClientCode") = "IND" Or
		   AppSettings("ClientCode") = "SUH" Or
		   AppSettings("ClientCode") = "LNT" Or
		   AppSettings("ClientCode") = "UYA" Or
		   AppSettings("ClientCode") = "FBW" Or
		   AppSettings("ClientCode") = "IPA" Or
		   AppSettings("ClientCode") = "IRM" Or
		   AppSettings("ClientCode") = "SPZ" Or
		   AppSettings("ClientCode") = "MEL" Or
		   AppSettings("ClientCode") = "SAP" Or
		   AppSettings("ClientCode") = "RED" Then 'UYA Added By Vikrant On 14-Jul-2020 For ALL14072020 

			Dim tmpLog As Log 'Added by Saylee 16-Sep-2019

			If Not mnWO.LogID.Equals(Guid.Empty) Then
				tmpLog = Log.GetLog(mnWO.LogID)
				LastLogDate = tmpLog.DateFormatted
			Else
				LastLogDate = ""
			End If

			tmpLog = Nothing

		End If

		If AppSettings("WOParametersRequired") = "True" Then

			Dim mnWOTaskParameterList As nWOParameterList
			Dim mnWORequestsParameterList As nWOParameterList
			Dim mnWOStatisticsParameterList As nWOParameterList

			mnWOTaskParameterList = nWOParameterList.GetWOParameterList(WOID:=mnWO.ID,
																		SectionName:="Tasks",
																		IsForReport:=True)

			mnWORequestsParameterList = nWOParameterList.GetWOParameterList(WOID:=mnWO.ID,
																			SectionName:="Requests",
																			IsForReport:=True)

			mnWOStatisticsParameterList = nWOParameterList.GetWOParameterList(WOID:=mnWO.ID,
																			  SectionName:="Statistics",
																			  IsForReport:=True)

			da.Fill(ds, "mnWOTaskParameterList", mnWOTaskParameterList)
			da.Fill(ds, "mnWORequestsParameterList", mnWORequestsParameterList)
			da.Fill(ds, "mnWOStatisticsParameterList", mnWOStatisticsParameterList)

		End If

		If AppSettings("ClientCode") = "Deccan" Or
		   AppSettings("ClientCode") = "ACI" Or
		   AppSettings("ClientCode") = "PTW" Or
		   AppSettings("ClientCode") = "ARA" Or
		   AppSettings("ClientCode") = "Indamer" Then

			Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(mnWO.MachineID)
			If mMachineOperatorName.OperatorName <> "" Then Searchstr7 = mMachineOperatorName.OperatorName

		End If

		WODocumentNo = AppSettings("WODocumentNo") 'SearchStr2
		WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo 'SearchStr3
		FormRevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo '21 SearchStr21
		FormRevisionDate = mTransactionList.Item(mnWO.TransTypeID).FormRevisionDate ''22 SearchStr22
		IssueNo = AppSettings("WOIssueNo") 'SearchStr6
		IssueDate = AppSettings("IssueDate") 'SearchStr30
		FormNo = AppSettings("WoNo") 'SearchStr5

		If (mnWO.TransTypeID = 88 Or
			mnWO.TransTypeID = 92 Or
			mnWO.TransTypeID = 93) And
			AppSettings("ClientCode") = "Heligo" And
			HeligoCallOutPrint = False Then 'added by Prashant on 11-Jan-2023 as Per mail According to work order type

			FormNo = "HCPL/QC/21"

		End If

		Dim Report As New ReportData(mCompanyDetail.CompanyName,
									 mCompanyDetail.Address,
									 mCompanyDetail.Tel1,
									 mCompanyDetail.Tel2,
									 mCompanyDetail.Fax,
									 mCompanyDetail.Email,
									 mCompanyDetail.WebSite,
									 ReportName:=ReportTitle,
									 SearchStr1:=EOFooterLine,
									 SearchStr2:=WODocumentNo,
									 SearchStr3:=WORevisionNo,
									 SearchStr4:=AppSettings("ClientCode"),
									 SearchStr5:=FormNo,
									 ProductVersion:=AppSettings("Product Version"),
									 SINote:=AppSettings("SINote"),
									 SearchStr6:=IssueNo,
									 SearchStr7:=Searchstr7,
									 SearchStr8:=EmployeeName,
									 SearchStr9:=AppSettings("Government Authority"),
									 SearchStr10:=AppSettings("Logo"),
									 SearchStr11:=LastLogDate,
									 SearchStr12:=LastLogDateHavingAPUValues,
									 SearchStr13:=AppSettings("CRS"),
									 SearchStr14:=mnWO.WOJobs(0).WOJobTypeID.ToString,
									 SearchStr15:=EmpName,
									 SearchStr16:=IIf(SignatureRequired = True, "True", "False"),
									 SearchStr17:=AirframeHrsAsOnCompletionDate,
									 SearchStr18:=AFAllPeriodsAsOnCompletionDate,
									 SearchStr19:=CompletedByUserLicenceNos,
									 SearchStr21:=FormRevisionNo,
									 SearchStr22:=FormRevisionDate,
									 SearchStr23:=mnWO.WOJobs(0).OtherJob.ToString,
									 SearchStr24:=mnWO.WOJobs(0).OtherJobSpecification.ToString,
									 SearchStr25:=mnWO.LogNo,
									 SearchStr26:=AppSettings("ShowMaintenanceForNewClients"),
									 SearchStr27:=AppSettings("ShowCAMOOnlyForNewClients"),
									 SearchStr28:=AppSettings("ShowAMOOnlyForNewClients"),
									 SearchStr29:=mnWO.TransTypeID.ToString,
									 SearchStr30:=IssueDate,
									 SearchStr31:=txtStartDate.Text,
									 SearchStr32:=txtCloseDate.Text,
									 SearchStr33:=txtStartDateTime.Text,
									 SearchStr34:=txtClosedDateTime.Text,
									 SearchStr35:=mnWO.WOJobs(0).ATACode.ToString,
									 SearchStr36:=mnWO.WOJobs(0).Zone.ToString,
									 SearchStr37:=mnWO.WOJobs(0).SkillCode.ToString,
									 SearchStr38:=mnWO.WOJobs(0).InspCode.ToString,
									 SearchStr39:=mnWO.WOJobs(0).TaskCardNo.ToString,
									 SearchStr40:=mnWO.WOJobs(0).TaskSourceRef.ToString,
									 SearchStr41:=mnWO.WOJobs(0).Publication.ToString,
									 SearchStr42:=mnWO.WOJobs(0).WorkPACKREF.ToString,
									 SearchStr43:=IIf(AppSettings("CAMOAPPROVALREFERENCENO") IsNot Nothing,
													  AppSettings("CAMOAPPROVALREFERENCENO"),
													  ""), 'Added by Harsh on 28th March 2025 for FLYPAL-2276
									 SearchStr44:=mnWO.WOJobs(0).DueAsOf.ToString,
									 SearchStr45:=MaintainanceOrganization, 'Added by Harsh on 4th July 2025 for Pattaya
									 SearchStr46:=mAirCraftManufacturerName, 'Sankalp 07-11-25
									SearchStr47:=mEnginManufacturerName,  'Sankalp 07-11-25
									SearchStr48:=mTaskReference,      'Sankalp 07-11-25
									SearchStr49:=mAMPRev,         'Sankalp 07-11-25
									SearchStr50:=mAMPDate) 'Sankalp 07-11-25

		'SearchStr44 used on Sapphire report

		Dim mrptImage As rptImage = rptImage.GetImage(ds, , "rptImage")


		da.Fill(ds, mnWO)
		da.Fill(ds, mnWOJobs)
		da.Fill(ds, mnWOJobComps)
		da.Fill(ds, mnWOJobDesignationAllocations) 'Added By Vikrant On 24-June-2013 For Indamer21062013
		da.Fill(ds, Report)
		da.Fill(ds, mnWOJobSpares)
		da.Fill(ds, mnWO.WOTools) 'Added By Prashant 13-Oct-2020 STR12102020
		da.Fill(ds, "WOJobActions", WOJobActions)
		da.Fill(ds, mrptImage)


		If AppSettings("ClientCode") = "RAL" Then

			mnWONRCJobs = mnWO.WONRCJobs
			da.Fill(ds, mnWONRCJobs)

		End If

		myReport.SetDataSource(ds)

		Session("CrystalReport") = myReport

		If ByMail = True Then 'Added By Prashant 1-Nov-2018  StarAir1112018

			'Do nothing

		ElseIf IsForDS = True Or IsForPrintWithJobAttachment = True Then 'Added By Prashant on 4-Jun-2024

			Dim fs As FileStream
			Dim br As BinaryReader
			Dim myExportOption As ExportOptions
			Dim myDiskOption As DiskFileDestinationOptions
			Dim myFile As String
			Dim n As New Random
			Dim imgbyte As Byte()

			myFile = "C:\Temp\Rep" + "Work Order Report " + Now.ToString.Replace(":", " ") + ".PDF"

			Session("myFile") = myFile

			myDiskOption = New DiskFileDestinationOptions
			myDiskOption.DiskFileName = myFile
			myExportOption = myReport.ExportOptions

			With myExportOption
				.DestinationOptions = myDiskOption
				.ExportDestinationType = .ExportDestinationType.DiskFile
				.ExportFormatType = .ExportFormatType.PortableDocFormat
			End With

			myReport.Export()
			myReport.Close()
			myReport.Dispose()

			br = Nothing
			imgbyte = Nothing

			If fs IsNot Nothing Then
				fs.Dispose()
			End If

		Else

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"openTranDetail",
												"openTranDetail();",
												True)

		End If

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " +
					mnWO.WODateFormatted.ToString +
					IIf(mnWO.RegNo.ToString <> "",
						" Aircraft : " + mnWO.RegNo.ToString,
						"") +
					IIf(mnWO.ModelName <> "",
						" Model : " + mnWO.ModelName,
						"") +
					IIf(mnWO.SerialNo <> "",
						" Serial No. : " + mnWO.SerialNo,
						"")

		MarkLog(Action.Print,
				"Work Order",
				"Work Order Detail Print : " + mWODetail,
				ErrorType.NoError,
				Guid.Empty,
				EventLogID)

	End Sub

	Public Sub PrintBlankEO()

		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		Dim da As New ObjectAdapter
		Dim myReport As Engine.ReportClass
		Dim mCompanyDetail As New CompanyDetail
		Dim ds As New dsnWODetail
		Dim EOFooterLine As String = ""
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			myReport = New crBlankWODetailForTAAL

			EOFooterLine = CType(AppSettings("EOFooterLine"), String)

		End If

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
					   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
					   mCompanyDetail.WebSite, "", EOFooterLine, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote")) 'Dont Use SearchStr20 

		da.Fill(ds, Report)
		myReport.SetDataSource(ds)

		Session("CrystalReport") = myReport

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print Blank EO : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------

		''Dim str As String
		''str = "<script language=Javascript>openTranDetail();</script>"
		''ClientScript.RegisterStartupScript([GetType], "openTranDetail", str)
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)
	End Sub

	Private Sub btnPrintAll_Click(sender As Object, e As EventArgs) Handles btnPrintAll.Click
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			Dim da As New ObjectAdapter
			Dim myReport As Engine.ReportClass
			Dim mCompanyDetail As New CompanyDetail

			Dim objnWO As nWO

			Dim ds As New dsnWODetail
			Dim EOFooterLine As String = ""

			If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then       'Added by Archana on 24-Nov-2009 for TAAL
				myReport = New crWOForeignObjectExclusionProgrammeReportForTAAL
				EOFooterLine = CType(AppSettings("EOFooterLine"), String)
			End If
			objnWO = Session("mnWO")

			Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
							mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
							mCompanyDetail.WebSite, "", EOFooterLine, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote")) 'Dont Use SearchStr20 


			da.Fill(ds, objnWO)
			da.Fill(ds, Report)
			myReport.SetDataSource(ds)

			Session("CrystalReport") = myReport

			'Added on 15-Mar-2019
			mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
			MarkLog(Action.Print, "Work Order", "Work Order Print All : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)

			Dim Str As String
			Str = "openTranDetail();"
			ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", Str, True)

		Else
			SetObject()
			SetGridObject()
			Session("mnWO") = mnWO
			'Response.Redirect("wfnWOReportForAll_AJAX.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
			ScriptManager.RegisterStartupScript(Me, [GetType], "OpenReportAllWindow", "OpenReportAllWindow();", True)
		End If
	End Sub

	Public Sub PrintNC()

		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		Dim da As New ObjectAdapter
		Dim myReport As Engine.ReportClass
		Dim mCompanyDetail As New CompanyDetail
		Dim ds As New dsnWODetail
		Dim EOFooterLine As String = ""

		myReport = New crnNonConfirmatoryDiscrepancies
		EOFooterLine = CType(AppSettings("EOFooterLine"), String)
		mnWO = Session("mnWO")

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
				mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
				mCompanyDetail.WebSite, "ADDITIONAL WORK SHEET / OFF JOB SHEET", EOFooterLine, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote")) 'Dont Use SearchStr20 


		da.Fill(ds, mnWO)
		da.Fill(ds, Report)

		myReport.SetDataSource(ds)

		Session("CrystalReport") = myReport

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print NC : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)
	End Sub

	''added By Prashant ON 08-Nov-2012 FOR ALL06112012-2
	''Changed By Saylee on 11-Jan-2013
	Public Sub PrintAdditionalWO()

		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		GetSession()
		Dim da As New ObjectAdapter
		Dim mCompanyDetail As New CompanyDetail

		'Dim mnWO As nWO
		Dim mnWOJobs As nWOJobs
		Dim mnWOJobTasks As nWOJobTasks
		Dim mnWOJobDesignationAllocations As nWOJobDesignationAllocations
		Dim mnWOJobSpares As nWOJobSpares
		Dim mnWOJobComps As nWOJobComps
		Dim mnWORegisterList As nWORegisterList
		Dim objTaskSteps As TaskSteps
		Dim mnWOTools As nWOTools

		Dim WOIssueNo As String = ""
		Dim WORevisionNo As String = ""

		Dim ds As New dsnWORegister

		Dim myReport = New crnWORegisterAdditionalWorkSheetBA

		Dim SearchStr1, SearchStr3 As String
		Dim SearchStr4, SearchStr5, SearchStr6 As String

		WOIssueNo = AppSettings("WOIssueNo")
		'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
		' WORevisionNo = AppSettings("WORevisionNo")
		WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
		'-----

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Novo") Then
			myReport = New crnWORegisterAdditionalWorkSheetNOVO  'Added by Saylee on 23-Jan-2018 for NOVO23012018
		Else
			myReport = New crnWORegisterAdditionalWorkSheetBA
		End If

		mnWO = Session("mnWO")
		mnWO = nWO.GetWO(mnWO.ID)
		mnWOJobs = mnWO.WOJobs
		mnWOTools = mnWO.WOTools
		mnWOJobComps = nWOJobComps.GetWOJobComps(mnWO.ID, "")
		mnWOJobSpares = nWOJobSpares.GetWOSpares(mnWO.ID, "")
		mnWOJobTasks = nWOJobTasks.GetWOTasks(mnWO.ID, "")
		mnWOJobDesignationAllocations = nWOJobDesignationAllocations.GetWOJobDesignationAllocations(mnWO.ID, "")
		objTaskSteps = TaskSteps.GetTaskCardSteps(mnWO.ID)

		mnWORegisterList = nWORegisterList.GetnWORegisterList(mnWO.WOText, mnWO.WONo, , , mnWO.RegNo, , mnWO.SerialNo)

		myReport.SetDataSource(ds)

		SearchStr3 = txtNo.Text

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
					  mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
					  mCompanyDetail.WebSite, "", SearchStr1, WOIssueNo, WORevisionNo, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6, AppSettings("ClientCode"), AppSettings("Government Authority"), , AppSettings("Logo")) 'Dont Use SearchStr20 

		Dim mrptImage As rptImage = rptImage.GetImage(ds)

		'WO Detail
		da.Fill(ds, mnWO)
		da.Fill(ds, mnWOJobs)
		da.Fill(ds, mnWOJobTasks)
		da.Fill(ds, mnWOJobDesignationAllocations)
		da.Fill(ds, mnWOJobSpares)
		da.Fill(ds, mnWOJobComps)
		da.Fill(ds, mnWORegisterList)
		da.Fill(ds, objTaskSteps)
		da.Fill(ds, mnWOTools)
		da.Fill(ds, Report)
		da.Fill(ds, mrptImage)
		myReport.SetDataSource(ds)

		myReport.Section6.SectionFormat.EnableSuppress = True

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print Only Additional WO : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------

		Session("CrystalReport") = myReport
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)
	End Sub

	'---------------------------------------------------
	Public Sub PrintWOPackage()

		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		GetSession()
		Dim da As New ObjectAdapter
		Dim mCompanyDetail As New CompanyDetail

		'Dim mnWO As nWO
		Dim mnWOJobs As nWOJobs
		Dim mnWOJobTasks As nWOJobTasks
		Dim mnWOJobDesignationAllocations As nWOJobDesignationAllocations
		Dim mnWOJobSpares As nWOJobSpares
		Dim mnWOJobComps As nWOJobComps
		Dim mnWORegisterList As nWORegisterList
		Dim objTaskSteps As TaskSteps
		Dim mnWOTools As nWOTools

		Dim objTaskCardChildsForWO As TaskCardChildsForWO  'Added By Saylee on 23-Jan-2013 for ALL21012013

		Dim ds As New dsnWORegister

		Dim WOIssueNo As String = ""
		Dim WORevisionNo As String = ""


		Dim myReport = New crnWORegisterWithJobsAndTasksDetailLandScapeForAll

		Dim SearchStr1, SearchStr3 As String
		Dim SearchStr4, SearchStr5, SearchStr6 As String



		myReport = New crnWORegisterWithJobsAndTasksDetailLandScapeForAllBA


		mnWO = Session("mnWO")
		mnWO = nWO.GetWO(mnWO.ID)
		mnWOJobs = mnWO.WOJobs
		mnWOTools = mnWO.WOTools
		mnWOJobComps = nWOJobComps.GetWOJobComps(mnWO.ID, "")
		mnWOJobSpares = nWOJobSpares.GetWOSpares(mnWO.ID, "")
		mnWOJobTasks = nWOJobTasks.GetWOTasks(mnWO.ID, "")
		mnWOJobDesignationAllocations = nWOJobDesignationAllocations.GetWOJobDesignationAllocations(mnWO.ID, "")
		objTaskSteps = TaskSteps.GetTaskCardSteps(mnWO.ID)

		mnWORegisterList = nWORegisterList.GetnWORegisterList(mnWO.WOText, mnWO.WONo, , , mnWO.RegNo, , mnWO.SerialNo)

		objTaskCardChildsForWO = TaskCardChildsForWO.GetTaskCardChildsForWO(mnWOJobTasks)  'Added By Saylee on 23-Jan-2013 for ALL21012013

		Dim mTaskCardStepsForJobTasks As TaskCardStepsForJobTasks = TaskCardStepsForJobTasks.GetTaskCardSteps(mnWOJobTasks) 'Added By Saylee on 5-Sep-2013 for BA04092013
		Dim mnWOJobTaskStepsSparesList As nWOJobTaskStepsSparesList = nWOJobTaskStepsSparesList.GetWOJobTaskStepsSpares(mnWOJobTasks) 'Added By Saylee on 5-Sep-2013 for BA04092013

		myReport.SetDataSource(ds)

		WOIssueNo = AppSettings("WOIssueNo")
		'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
		' WORevisionNo = AppSettings("WORevisionNo")
		WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
		'-----
		SearchStr3 = txtNo.Text

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
					  mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
					  mCompanyDetail.WebSite, "WO Package", SearchStr1, WOIssueNo, WORevisionNo, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6, AppSettings("ClientCode"), AppSettings("Government Authority"), , AppSettings("Logo")) 'Dont Use SearchStr20 

		Dim mrptImage As rptImage = rptImage.GetImage(ds)

		'WO Detail
		da.Fill(ds, mnWO)
		da.Fill(ds, mnWOJobs)
		da.Fill(ds, mnWOJobTasks)
		da.Fill(ds, mnWOJobDesignationAllocations)
		da.Fill(ds, mnWOJobSpares)
		da.Fill(ds, mnWOJobComps)
		da.Fill(ds, mnWORegisterList)
		da.Fill(ds, objTaskSteps)
		da.Fill(ds, mnWOTools)
		da.Fill(ds, objTaskCardChildsForWO) 'Added By Saylee on 23-Jan-2013 for ALL21012013


		da.Fill(ds, Report)
		da.Fill(ds, mrptImage)
		da.Fill(ds, mTaskCardStepsForJobTasks) 'Added By Saylee on 5-Sep-2013 for BA04092013
		da.Fill(ds, mnWOJobTaskStepsSparesList) 'Added By Saylee on 5-Sep-2013 for BA04092013

		myReport.SetDataSource(ds)

		myReport.Section6.SectionFormat.EnableSuppress = True


		Session("CrystalReport") = myReport

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print for Package : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------

		''Dim str As String
		''str = "<script language=Javascript>openTranDetail();</script>"
		''ClientScript.RegisterStartupScript([GetType], "openTranDetail", str)
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)
	End Sub

	Public Sub PrintAdditionalWOAndSheet()

		GetSession()
		Dim da As New ObjectAdapter
		Dim mCompanyDetail As New CompanyDetail

		'Dim mnWO As nWO
		Dim mnWOJobs As nWOJobs
		Dim mnWOJobTasks As nWOJobTasks
		Dim mnWOJobDesignationAllocations As nWOJobDesignationAllocations
		Dim mnWOJobSpares As nWOJobSpares
		Dim mnWOJobComps As nWOJobComps
		Dim mnWORegisterList As nWORegisterList
		Dim objTaskSteps As TaskSteps
		Dim mnWOTools As nWOTools

		Dim ds As New dsnWORegister

		Dim myReport = New crnWORegisterAdditionalWorkSheetBA

		Dim SearchStr1, SearchStr2, SearchStr3 As String
		Dim SearchStr4, SearchStr5, SearchStr6 As String


		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Novo") Then
			myReport = New crnWORegisterAdditionalWorkSheetNOVO  'Added by Saylee on 23-Jan-2018 for NOVO23012018
		Else
			myReport = New crnWORegisterAdditionalWorkSheetBA
		End If




		mnWO = Session("mnWO")
		mnWO = nWO.GetWO(mnWO.ID)
		mnWOJobs = mnWO.WOJobs
		mnWOTools = mnWO.WOTools
		mnWOJobComps = nWOJobComps.GetWOJobComps(mnWO.ID, "")
		mnWOJobSpares = nWOJobSpares.GetWOSpares(mnWO.ID, "")
		mnWOJobTasks = nWOJobTasks.GetWOTasks(mnWO.ID, "")
		mnWOJobDesignationAllocations = nWOJobDesignationAllocations.GetWOJobDesignationAllocations(mnWO.ID, "")
		objTaskSteps = TaskSteps.GetTaskCardSteps(mnWO.ID)

		mnWORegisterList = nWORegisterList.GetnWORegisterList(mnWO.WOText, mnWO.WONo, , , mnWO.RegNo, , mnWO.SerialNo)

		myReport.SetDataSource(ds)

		SearchStr3 = txtNo.Text

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
					  mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
					  mCompanyDetail.WebSite, "Part No. Status", SearchStr1, SearchStr2, SearchStr3, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6, AppSettings("ClientCode"), AppSettings("Government Authority"), , AppSettings("Logo")) 'Dont Use SearchStr20 

		Dim mrptImage As rptImage = rptImage.GetImage(ds)

		'WO Detail
		da.Fill(ds, mnWO)
		da.Fill(ds, mnWOJobs)
		da.Fill(ds, mnWOJobTasks)
		da.Fill(ds, mnWOJobDesignationAllocations)
		da.Fill(ds, mnWOJobSpares)
		da.Fill(ds, mnWOJobComps)
		da.Fill(ds, mnWORegisterList)
		da.Fill(ds, objTaskSteps)
		da.Fill(ds, mnWOTools)
		da.Fill(ds, Report)
		da.Fill(ds, mrptImage)
		myReport.SetDataSource(ds)

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print Additional WO With Sheet : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------

		Session("CrystalReport") = myReport
		''Dim str As String
		''str = "<script language=Javascript>openTranDetail();</script>"
		''ClientScript.RegisterStartupScript([GetType], "openTranDetail", str)
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)
	End Sub

	' Added by Saylee on 10-Dec-2019 ,here if one job and one task then format changes
	Public Sub PrintWithPDFNOVOSingleTask()


		'Protected Sub btnPrintWithPDF_Click( sender As Object,  e As EventArgs) Handles btnPrintWithPDF.Click
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		Dim da As New ObjectAdapter
		Dim myReport As Engine.ReportClass
		Dim myReportChild = New crptTaskCard
		Dim mCompanyDetail As New CompanyDetail
		Dim ds As New dsnWODetail
		Dim mnWOJobs As nWOJobs
		Dim mnWONRCJobs As nWOJobs
		Dim mnWOJobComps As nWOJobComps
		Dim mnWOJobDesignationAllocations As nWOJobDesignationAllocations 'Added By Vikrant On 24-June-2013 For Indamer21062013
		Dim mnWOJobTasks As nWOJobTasks


		Dim WODocumentNo As String = ""
		Dim WORevisionNo As String = ""
		Dim Searchstr7 As String = ""
		Dim WOIssueNo As String = ""

		Dim SearchStr1 As String
		''Dim SearchStr4, SearchStr5, SearchStr6 As String

		Dim WONo As String = "WO-" & mnWO.WONo.ToString + "-"

		Dim EOFooterLine As String = ""

		myReport = New crnWODetailForNOVO

		Dim mrptImage As rptImage
		Dim mnWORegisterList As nWORegisterList
		Dim PDFNo As Integer = 1
		Dim PDFNoChild As Integer = 1
		Dim tmp As Integer
		Dim a As New Random
		Dim pageCount As Integer = 0

		Dim pdfList As New Collections.ArrayList

		Dim MyFile1 = ""
		Dim myExportOption As CrystalDecisions.Shared.ExportOptions
		Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions
		Dim ReportName As String = ""

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "BSA") Then
			ReportName = "WORK ORDER INDEX"
		Else
			ReportName = "WORK ORDER"
		End If
		mnWO = Session("mnWO")
		mnWO = nWO.GetWO(mnWO.ID, False)
		mnWOJobs = mnWO.WOJobs
		mnWONRCJobs = mnWO.WONRCJobs
		mnWOJobComps = nWOJobComps.GetWOJobComps(mnWO.ID, "")
		mnWOJobDesignationAllocations = nWOJobDesignationAllocations.GetWOJobDesignationAllocations(mnWO.ID, "")
		mnWOJobTasks = nWOJobTasks.GetWOTasks(mnWO.ID, "")

		mnWORegisterList = nWORegisterList.GetnWORegisterList(mnWO.WOText, mnWO.WONo, , , mnWO.RegNo, , mnWO.SerialNo)
		da.Fill(ds, mnWORegisterList)


		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
			mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
			mCompanyDetail.WebSite, ReportName, mnWO.RegNo, mnWO.WODateFormatted, "", AppSettings("ClientCode"), "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", AppSettings("Government Authority"), AppSettings("Logo")) 'Dont Use SearchStr20 

		mrptImage = rptImage.GetImage(ds, True)

		da.Fill(ds, mnWO)
		da.Fill(ds, mnWOJobs)
		da.Fill(ds, mnWOJobComps)
		da.Fill(ds, mnWOJobDesignationAllocations)
		da.Fill(ds, Report)
		da.Fill(ds, mrptImage)
		If AppSettings("ClientCode") = "RAL" Then
			da.Fill(ds, mnWONRCJobs)
		End If



		myReport.SetDataSource(ds)

		Session("CrystalReport") = myReport



		tmp = a.Next

		'Dim MyFile1 = "C:\Temp\" & tmp & PDFNo.ToString & ".pdf"
		MyFile1 = "C:\Temp\" & WONo & tmp & PDFNo.ToString & ".pdf"
		myReport = CType(Session("CrystalReport"), Engine.ReportClass)




		myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
		myDiskOption.DiskFileName = MyFile1
		myExportOption = myReport.ExportOptions
		With myExportOption
			.DestinationOptions = myDiskOption
			.ExportDestinationType = ExportDestinationType.DiskFile
			.ExportFormatType = ExportFormatType.PortableDocFormat
		End With
		myReport.Export()
		myReport.Close()
		myReport.Dispose()
		GC.Collect()

		If mnWO.WOJobs.Count > 1 Then
			pdfList.Add(MyFile1)
			PDFNo = PDFNo + 1
		End If


		Dim IsOneTaskExists As Boolean = mnWOJobs.IsOneTaskExists

		For k As Integer = 0 To mnWOJobs.Count - 1  'Added by Saylee on 7-Mar-2014 for ALL07032014-1 : Only For loop for mnWOJobs added
			Dim mnWOJob As nWOJob = mnWOJobs(k)

			Dim aChild As New Random
			Dim tmpChild As Integer
			PDFNoChild = 1
			If IsOneTaskExists = True Then
				myReportChild = New crnWODetailSingleTaskForNOVO
				Dim mnWOTaskCardListForExcel As nWOTaskCardListForExcel
				mnWOTaskCardListForExcel = nWOTaskCardListForExcel.GetnWOTaskCardListForExcel(mnWO.ID, WOJobID:=mnWOJob.ID.ToString)



				Dim dsWOTaskCardList As New dsnWOTaskCardListForExcel
				dsWOTaskCardList.Clear()
				da.Fill(dsWOTaskCardList, mnWOTaskCardListForExcel)
				da.Fill(dsWOTaskCardList, Report)
				mrptImage = rptImage.GetImage(dsWOTaskCardList, True)
				da.Fill(dsWOTaskCardList, mrptImage)

				da = New ObjectAdapter
				mCompanyDetail = New CompanyDetail

				SearchStr1 = New SmartDate(Today.Date).FormattedText

				Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
					   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
					   mCompanyDetail.WebSite, "", SearchStr1, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", SearchStr10:=AppSettings("Logo")) 'Dont Use SearchStr20 

				myReportChild.SetDataSource(dsWOTaskCardList)
				Session("myReportChild") = myReportChild

				'PDFNo = 1
				If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "BSA") Then
					myReportChild.Section8.SectionFormat.EnableSuppress = True
					myReportChild.Section9.SectionFormat.EnableSuppress = True
				End If
				tmpChild = aChild.Next

				'Dim MyFile1Child = "C:\Temp\" & tmpChild & PDFNo.ToString & ".pdf"

				'Dim MyFile1Child = "C:\Temp\" & tmpChild & PDFNoChild.ToString & ".pdf"
				Dim MyFile1Child = "C:\Temp\" & tmpChild & PDFNoChild.ToString & ".pdf"

				myReportChild = CType(Session("myReportChild"), Engine.ReportClass)

				Dim myDiskOptionChild As CrystalDecisions.Shared.DiskFileDestinationOptions


				myDiskOptionChild = New CrystalDecisions.Shared.DiskFileDestinationOptions
				myDiskOptionChild.DiskFileName = MyFile1Child
				myExportOption = myReportChild.ExportOptions
				With myExportOption
					.DestinationOptions = myDiskOptionChild
					.ExportDestinationType = ExportDestinationType.DiskFile
					.ExportFormatType = ExportFormatType.PortableDocFormat
				End With

				Try
					myReportChild.Export()
					myReportChild.Close()
					myReportChild.Dispose()
					GC.Collect()
				Catch ex As Exception
					Throw ex
				End Try


				'/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/
				pageCount = 0

				'Dim MyFile1Child_Ext As String = "C:\Temp\" & tmp & PDFNo.ToString & "_Ext" & ".pdf"
				Dim MyFile1Child_Ext As String = "C:\Temp\" & WONo & tmp & PDFNo.ToString & "_Ext" & ".pdf"

				pdfList.Add(MyFile1Child)

				PDFNo = PDFNo + 1
				PDFNoChild = PDFNoChild + 1

				MyFile1Child = Nothing
				myReportChild = Nothing
				Session.Remove("myReportChild")

				For Each mnWOTaskCardListForExcelInfo As nWOTaskCardListForExcel.nWOTaskCardListForExcelInfo In mnWOTaskCardListForExcel
					Dim mTaskCard As TaskCard = TaskCard.GetTaskCard(mnWOTaskCardListForExcelInfo.TaskCardID)
					Dim mTaskCardAttachment As TaskCardAttachment

					For j As Integer = 0 To mTaskCard.TaskCardAttachments.Count - 1
						mTaskCardAttachment = mTaskCard.TaskCardAttachments(j)

						If mTaskCardAttachment.ImageSize > 0 And LCase(mTaskCardAttachment.FileExtension) = ".pdf" Then
							'Dim ChildAttachment_path As String = "C:\Temp\" & tmp & PDFNo.ToString & mTaskCardAttachment.FileExtension
							''Dim ChildAttachment_path As String = "C:\Temp\" & tmpChild & PDFNoChild.ToString & mTaskCardAttachment.FileExtension
							Dim ChildAttachment_path As String = "C:\Temp\" & WONo & PDFNoChild.ToString & mTaskCardAttachment.FileExtension

							Dim fs As FileStream
							If File.Exists("C:\Temp\") = False Then
								File.Delete(ChildAttachment_path)
								fs = File.Create(ChildAttachment_path)
								fs.Write(mTaskCardAttachment.ImageFile, 0, mTaskCardAttachment.ImageFile.Length)
								fs.Close()

								pdfList.Add(ChildAttachment_path)                               '2. TaskCardAttachment attachment
								PDFNo = PDFNo + 1
								PDFNoChild = PDFNoChild + 1
							End If
						End If
						mTaskCardAttachment = Nothing

					Next
				Next
			Else
				For i As Integer = 0 To mnWOJobTasks.Count - 1
					Dim mnWOJobTask As nWOJobTask = mnWOJobTasks(i)
					Dim mTaskCard As TaskCard

					Dim dsTaskCard As New dsTaskCard
					If Not mnWOJobTask.TaskCardID.Equals(Guid.Empty) Then
						mTaskCard = TaskCard.GetTaskCard(mnWOJobTask.TaskCardID)

						da = New ObjectAdapter
						mCompanyDetail = New CompanyDetail

						SearchStr1 = New SmartDate(Today.Date).FormattedText

						Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
							   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
							   mCompanyDetail.WebSite, "", SearchStr1, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", SearchStr10:=AppSettings("Logo")) 'Dont Use SearchStr20 

						mrptImage = rptImage.GetImage(dsTaskCard, True)

						If mnWOJob.WOJobTasks.Count > 1 Then
							myReportChild = New crptTaskCardNOVO    'Added by Saylee on 25-Jan-2018 for NOVO23012018
						Else



						End If
						da.Fill(dsTaskCard, mrptImage)
						da.Fill(dsTaskCard, mTaskCard)
						da.Fill(dsTaskCard, mTaskCard.TaskCardSpares)
						da.Fill(dsTaskCard, mTaskCard.TaskCardTools)
						da.Fill(dsTaskCard, mTaskCard.TaskSteps)
						da.Fill(dsTaskCard, mTaskCard.TaskCardStepsSpares)
						da.Fill(dsTaskCard, Report)
						da.Fill(dsTaskCard, mnWOJobTasks) 'Added by Saylee on 7-Oct-2014
						da.Fill(dsTaskCard, mnWOJobs) 'Added by Saylee on 7-Oct-2014


						myReportChild.SetDataSource(dsTaskCard)

						Session("myReportChild") = myReportChild

						'PDFNo = 1
						If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "BSA") Then
							myReportChild.Section8.SectionFormat.EnableSuppress = True
							myReportChild.Section9.SectionFormat.EnableSuppress = True
						End If
						tmpChild = aChild.Next

						'Dim MyFile1Child = "C:\Temp\" & tmpChild & PDFNo.ToString & ".pdf"

						'Dim MyFile1Child = "C:\Temp\" & tmpChild & PDFNoChild.ToString & ".pdf"
						Dim MyFile1Child = "C:\Temp\" & tmpChild & PDFNoChild.ToString & ".pdf"

						myReportChild = CType(Session("myReportChild"), Engine.ReportClass)

						Dim myDiskOptionChild As CrystalDecisions.Shared.DiskFileDestinationOptions


						myDiskOptionChild = New CrystalDecisions.Shared.DiskFileDestinationOptions
						myDiskOptionChild.DiskFileName = MyFile1Child
						myExportOption = myReportChild.ExportOptions
						With myExportOption
							.DestinationOptions = myDiskOptionChild
							.ExportDestinationType = ExportDestinationType.DiskFile
							.ExportFormatType = ExportFormatType.PortableDocFormat
						End With

						Try
							myReportChild.Export()
							myReportChild.Close()
							myReportChild.Dispose()
							GC.Collect()
						Catch ex As Exception
							Throw ex
						End Try


						'/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/
						pageCount = 0

						'Dim MyFile1Child_Ext As String = "C:\Temp\" & tmp & PDFNo.ToString & "_Ext" & ".pdf"
						Dim MyFile1Child_Ext As String = "C:\Temp\" & WONo & tmp & PDFNo.ToString & "_Ext" & ".pdf"
						Dim PageNumbersToExt As Integer()

						'Find "Cat." word in WO Print Out
						Dim TextToSearch As String = ""

						If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "BSA") Then
							If mTaskCard.TaskCardTools.Count = 0 And mTaskCard.TaskCardSpares.Count = 0 Then
								TextToSearch = "CARD COMPLETE"
							ElseIf mTaskCard.TaskCardTools.Count > 0 Then
								TextToSearch = "Qty."
							ElseIf mTaskCard.TaskCardSpares.Count > 0 Then
								TextToSearch = "BATCH NUMBER"
							End If

						Else
							If mTaskCard.TaskCardStepsCount = 0 And mTaskCard.TaskCardStepsSpares.Count = 0 Then
								TextToSearch = "AME"
							ElseIf mTaskCard.TaskCardStepsSpares.Count > 0 Then
								TextToSearch = "PLACE"
							ElseIf mTaskCard.TaskCardStepsCount > 0 Then
								TextToSearch = "Signature"
							End If
						End If
						Dim Task_PageStartedFrom As Integer = getPageNoBySpecificText(1, MyFile1Child, TextToSearch) '"Autho. Rel.")
						ReDim PageNumbersToExt(Task_PageStartedFrom - 1)

						For PageNo As Integer = 1 To Task_PageStartedFrom
							PageNumbersToExt(PageNo - 1) = PageNo
						Next

						If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Novo") Then
							pdfList.Add(MyFile1Child)
						Else
							ExtractPdfPage(MyFile1Child, PageNumbersToExt, MyFile1Child_Ext)
							pdfList.Add(MyFile1Child_Ext)
						End If
						'1. TaskCardAttachment with its extension
						PDFNo = PDFNo + 1
						PDFNoChild = PDFNoChild + 1

						MyFile1Child = Nothing
						myReportChild = Nothing
						Session.Remove("myReportChild")
						dsTaskCard.Clear()

						Dim mTaskCardAttachment As TaskCardAttachment
						For j As Integer = 0 To mTaskCard.TaskCardAttachments.Count - 1
							mTaskCardAttachment = mTaskCard.TaskCardAttachments(j)

							If mTaskCardAttachment.ImageSize > 0 And LCase(mTaskCardAttachment.FileExtension) = ".pdf" Then
								'Dim ChildAttachment_path As String = "C:\Temp\" & tmp & PDFNo.ToString & mTaskCardAttachment.FileExtension
								''Dim ChildAttachment_path As String = "C:\Temp\" & tmpChild & PDFNoChild.ToString & mTaskCardAttachment.FileExtension
								Dim ChildAttachment_path As String = "C:\Temp\" & WONo & PDFNoChild.ToString & mTaskCardAttachment.FileExtension

								Dim fs As FileStream
								If File.Exists("C:\Temp\") = False Then
									File.Delete(ChildAttachment_path)
									fs = File.Create(ChildAttachment_path)
									fs.Write(mTaskCardAttachment.ImageFile, 0, mTaskCardAttachment.ImageFile.Length)
									fs.Close()

									pdfList.Add(ChildAttachment_path)                               '2. TaskCardAttachment attachment
									PDFNo = PDFNo + 1
									PDFNoChild = PDFNoChild + 1
								End If
							End If
							mTaskCardAttachment = Nothing

						Next
					End If
					mTaskCard = Nothing
				Next
			End If

		Next



		' //********************************************Send Files for Merging****************************************************//
		Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
		Dim MergedPath_WM As String = "C:\Temp\" & "temp_myMergedPdf_WM.pdf"

		Dim filesByte As New List(Of Byte())()
		For Each file__1 As String In pdfList 'files
			filesByte.Add(File.ReadAllBytes(file__1))
		Next

		File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))

		'AddWatermarkText(MergedPath, MergedPath_WM, mnWO.WONumber, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount)
		AddWatermarkText(MergedPath, MergedPath_WM, mnWO.WOText.ToString & "-" & mnWO.WONo.ToString, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount) 'Added on 24-Jun-2019
		''//********************************************Set Sessions*********************************************************//
		Session("CrystalReport") = MergedPath_WM
		Session("PrintReportWithAttachment") = "True"

		'//*******************************************Delete created file*********************************************************//

		'Commented and Added by Saylee on 2-Dec-2014
		' Dim MyFile, MyFile_Ext As String
		'For j As Integer = 1 To PDFNo - 1
		'    MyFile = "C:\Temp\" & WONo & j.ToString & ".pdf"
		'    MyFile_Ext = "C:\Temp\" & WONo & j.ToString & "_Ext" & ".pdf"

		'    File.Delete(MyFile)
		'    File.Delete(MyFile_Ext)
		'Next

		Dim DeleteThis As String = WONo
		Dim Files As String() = Directory.GetFiles("C:\Temp\")

		For Each file__1 As String In Files
			If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
				File.Delete(file__1)
			End If
		Next
		'End

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print With Task Attachments : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------

		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)
	End Sub

	'********************************************************************************
	'Added By Saylee
	Public Sub PrintWithPDF(Optional EachJobPrint As Boolean = False, Optional JobIndexToPrint As Integer = -1)


		'Protected Sub btnPrintWithPDF_Click( sender As Object,  e As EventArgs) Handles btnPrintWithPDF.Click
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		Dim da As New ObjectAdapter
		Dim myReport As Engine.ReportClass
		Dim myReportChild = New crptTaskCard
		Dim myReportJob As Engine.ReportClass
		Dim mCompanyDetail As New CompanyDetail
		Dim ds As New dsnWODetail
		Dim mnWOJobs As nWOJobs
		Dim mnWONRCJobs As nWOJobs
		Dim mnWOJobComps As nWOJobComps
		Dim mnWOJobDesignationAllocations As nWOJobDesignationAllocations 'Added By Vikrant On 24-June-2013 For Indamer21062013
		Dim mnWOJobTasks As nWOJobTasks


		Dim WODocumentNo As String = ""
		Dim WORevisionNo As String = ""
		Dim Searchstr7 As String = ""
		Dim WOIssueNo As String = ""

		Dim SearchStr1 As String
		''Dim SearchStr4, SearchStr5, SearchStr6 As String

		Dim WONo As String = "WO-" & mnWO.WONo.ToString + "-"

		Dim EOFooterLine As String = ""
		Dim mAMPRev As String = ""
		Dim mAMPDate As String = ""


		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Novo") Then
			myReport = New crnWODetailForNOVO 'Added by Saylee on 23-Jan-2018 for NOVO23012018
		ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "STR") Then  'Added by Saylee on 13-Feb-2020
			myReport = New crnWOIssueDetailForStarAir 'Same is printed when clicked on "Print"
			da.Fill(ds, "nIssuedWOSpares", Session("mnIssuedWOSpareswfnWODetail")) 'Added By Prashant 13-Oct-2020 STR12102020 Again change on 26-Nov-2020
			da.Fill(ds, "nIssuedWOTools", Session("mIssuedWOToolswfnWODetail")) 'Added By Prashant 13-Oct-2020 STR12102020 Again change on 26-Nov-2020
		ElseIf (AppSettings("ClientCode") = "CVA") Then    'Sankalp 24-11-25
			Dim mLastAMPRef As LastMPDAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(mnWO.MachineID)
			If (mLastAMPRef.AMPNo = "") Then
			Else
				mAMPRev = mLastAMPRef.AMPNo + " ,Rev No.: " + mLastAMPRef.RevNo
				mAMPDate = mLastAMPRef.FromDateFormatted
			End If
			myReport = New crnWODetailForAttachForCVA
		Else
			myReport = New crnWODetailForAttach
		End If


		mnWO = Session("mnWO")
		mnWO = nWO.GetWO(mnWO.ID, False)
		mnWOJobs = mnWO.WOJobs
		mnWONRCJobs = mnWO.WONRCJobs
		mnWOJobComps = nWOJobComps.GetWOJobComps(mnWO.ID, "")
		mnWOJobDesignationAllocations = nWOJobDesignationAllocations.GetWOJobDesignationAllocations(mnWO.ID, "")
		mnWOJobTasks = nWOJobTasks.GetWOTasks(mnWO.ID, "")


		Dim mnWORegisterList As nWORegisterList
		mnWORegisterList = nWORegisterList.GetnWORegisterList(mnWO.WOText, mnWO.WONo, , , mnWO.RegNo, , mnWO.SerialNo)
		da.Fill(ds, mnWORegisterList)

		Dim ReportName As String = ""
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "BSA") Then
			ReportName = "WORK ORDER INDEX"
		Else
			ReportName = "WORK ORDER"
		End If

		'Added By Vikrant on 07-Dec-2020 For Passion Air
		Dim CRSStatement As String = ""
		If AppSettings("ClientCode") = "PAS" Then
			CRSStatement = "Certifies that the work specified, except as otherwise specified, was carried out in accordance with the Ghana CAA Directives GCAD Part 6 and in respect to that work, the aircraft/ aircraft component is considered ready for release to service."
		Else
			CRSStatement = AppSettings("Government Authority") 'Existing Condition
		End If
		'End

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
			mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
			mCompanyDetail.WebSite, ReportName, mnWO.RegNo, mnWO.WODateFormatted, "", AppSettings("ClientCode"), "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", CRSStatement, AppSettings("Logo"), SearchStr20:=mnWO.BarcodeNo, SearchStr12:=mAMPRev, SearchStr13:=mAMPDate) 'DoSearchStr13:=nt Use SearchStr20 

		Dim mrptImage As rptImage = rptImage.GetImage(ds, True)

		da.Fill(ds, mnWO)
		da.Fill(ds, mnWOJobs)
		da.Fill(ds, mnWOJobComps)
		da.Fill(ds, mnWOJobDesignationAllocations)
		da.Fill(ds, Report)
		da.Fill(ds, mnWO.WOTools) 'Added By Prashant 13-Oct-2020 STR12102020
		da.Fill(ds, mrptImage)
		If AppSettings("ClientCode") = "RAL" Then
			da.Fill(ds, mnWONRCJobs)
		End If
		myReport.SetDataSource(ds)

		Session("CrystalReport") = myReport

		Dim PDFNo As Integer = 1
		Dim PDFNoChild As Integer = 1
		Dim tmp As Integer
		Dim a As New Random

		tmp = a.Next

		'Dim MyFile1 = "C:\Temp\" & tmp & PDFNo.ToString & ".pdf"
		Dim MyFile1 = "C:\Temp\" & WONo & tmp & PDFNo.ToString & ".pdf"

		myReport = CType(Session("CrystalReport"), Engine.ReportClass)

		Dim myExportOption As CrystalDecisions.Shared.ExportOptions
		Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions


		myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
		myDiskOption.DiskFileName = MyFile1
		myExportOption = myReport.ExportOptions
		With myExportOption
			.DestinationOptions = myDiskOption
			.ExportDestinationType = ExportDestinationType.DiskFile
			.ExportFormatType = ExportFormatType.PortableDocFormat
		End With
		myReport.Export()
		myReport.Close()
		myReport.Dispose()
		GC.Collect()

		Dim pageCount As Integer = 0

		Dim pdfList As New ArrayList

		pdfList.Add(MyFile1)
		PDFNo = PDFNo + 1

		For k As Integer = 0 To mnWOJobs.Count - 1  'Added by Saylee on 7-Mar-2014 for ALL07032014-1 : Only For loop for mnWOJobs added

			If EachJobPrint = True And JobIndexToPrint <> -1 Then k = JobIndexToPrint



			Dim mnWOJob As nWOJob = mnWOJobs(k)
			mnWOJobTasks = mnWOJob.WOJobTasks
			'Added By Vikrant on 03-Mar-2020 For ALL03032020
			If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "PAS" Then 'PAS code added by Saylee as Passion needs to skip jobPage
				GoTo NextStatement
			End If
			'End
			If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "BSA") Then
				myReportJob = New crnWOJobDetailForAttachBSA  'Added by Saylee on 7-Oct-2014
			ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Novo") Then
				myReportJob = New crnWOJobDetailForAttachNOVO 'Added by Saylee on 23-Jan-2018 for NOVO23012018
			ElseIf (AppSettings("ClientCode") = "KAS") Then
				myReportJob = New crnWOJobDetailForAttachForKAS
			ElseIf (AppSettings("ClientCode") = "CVA") Then    'Sankalp 24-11-25
				myReportJob = New crnWOJobDetailForAttachForCVA
			Else
				myReportJob = New crnWOJobDetailForAttach
			End If

			'Added by Saylee on 27-Oct-2022 for KASAS27102022
			Dim mnrptWOJobResourceDetails As nrptWOJobResourceDetails
			mnrptWOJobResourceDetails = nrptWOJobResourceDetails.GetrptWOJobResourceDetails(mnWOJob.ID.ToString)
			'****************************************************************


			Dim mnWOTools As nWOTools
			Dim mnWOJobSpares As nWOJobSpares
			Dim dsWOReg As New dsnWORegister
			mnWO = Session("mnWO")
			mnWOJobs = mnWO.WOJobs
			mnWOTools = mnWO.WOTools
			mnWOJobSpares = mnWOJob.WOJobSpares 'nWOJobSpares.GetWOSpares(mnWO.ID, "")
			mnWOJobDesignationAllocations = mnWOJobs(k).WOJobDesignationAllocations 'nWOJobDesignationAllocations.GetWOJobDesignationAllocations(mnWO.ID, "")

			mrptImage = rptImage.GetImage(dsWOReg, True)

			da.Fill(dsWOReg, mnWO)
			da.Fill(dsWOReg, mnWOJob)
			da.Fill(dsWOReg, mnWOJobDesignationAllocations)
			da.Fill(dsWOReg, mnWOJobSpares)
			da.Fill(dsWOReg, mnWOTools)
			da.Fill(dsWOReg, mnWOJobTasks)
			da.Fill(dsWOReg, mnWOJob.WOJobComps)     'Sankalp 02-12-25
			da.Fill(dsWOReg, mnWORegisterList)
			da.Fill(dsWOReg, Report)
			da.Fill(ds, mrptImage)
			da.Fill(dsWOReg, mnrptWOJobResourceDetails)   'Added by Saylee on 27-Oct-2022 for KASAS27102022

			myReportJob.SetDataSource(dsWOReg)
			Session("myReportJob") = myReportJob

			tmp = a.Next

			'MyFile1 = "C:\Temp\" & tmp & PDFNo.ToString & ".pdf"
			MyFile1 = "C:\Temp\" & WONo & tmp & PDFNo.ToString & ".pdf"

			myReportJob = CType(Session("myReportJob"), Engine.ReportClass)

			Dim myDiskOptionJob As CrystalDecisions.Shared.DiskFileDestinationOptions
			Dim CrFormatTypeOptions As New CrystalDecisions.Shared.PdfFormatOptions
			myDiskOptionJob = New CrystalDecisions.Shared.DiskFileDestinationOptions
			myDiskOptionJob.DiskFileName = MyFile1
			myExportOption = myReportJob.ExportOptions
			With myExportOption
				.DestinationOptions = myDiskOptionJob
				.ExportDestinationType = ExportDestinationType.DiskFile
				.ExportFormatType = ExportFormatType.PortableDocFormat
				.FormatOptions = CrFormatTypeOptions
			End With

			'Dim param As String = ""
			'If (Not parameters = DBNull.Value) Then
			'    For Each param In parameters.Keys
			'        myReportJob.SetParameterValue(param, parameters(param))
			'    Next
			'End If
			Try
				myReportJob.Export()
				myReportJob.Close()
				myReportJob.Dispose()
				GC.Collect()
			Catch ex As Exception
				Throw ex
			End Try


			pageCount = 0

			pdfList.Add(MyFile1)
			PDFNo = PDFNo + 1

NextStatement:
			Dim aChild As New Random
			Dim tmpChild As Integer
			PDFNoChild = 1



			For i As Integer = 0 To mnWOJobTasks.Count - 1
				Dim mnWOJobTask As nWOJobTask = mnWOJobTasks(i)
				Dim mTaskCard As TaskCard

				Dim dsTaskCard As New dsTaskCard

				If Not mnWOJobTask.TaskCardID.Equals(Guid.Empty) Then

					mTaskCard = TaskCard.GetTaskCard(mnWOJobTask.TaskCardID)
					da.Fill(dsTaskCard, mTaskCard)
					da.Fill(dsTaskCard, mTaskCard.TaskCardTools)
					da.Fill(dsTaskCard, mTaskCard.TaskSteps)

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "BSA") Then
						myReportChild = New crptTaskCardForBSA   'Added by Saylee on 7-Oct-2014
					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Novo") Then
						myReportChild = New crptTaskCardNOVO    'Added by Saylee on 25-Jan-2018 for NOVO23012018
					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "STR") Then 'Added By Vikrant on 03-Mar-2020 For ALL03032020
						myReportChild = New crptTaskCardSTR
						'End
					Else
						'Commented and added by Saylee on 19-Mar-2021 , as we needed report for selected Task Cards
						' myReportChild = New crptTaskCard
						myReportChild = New crptTaskCardWithSelectedTaskCardMaster  'New crptTaskCard
					End If
				Else
					If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "STR") Then
						myReportChild = New crptTaskCardUnSCHSTR 'Added by Saylee on 30-Apr-2020, LockDown Period
					Else
						myReportChild = New crptTaskCardWithManualTaskCard  'added by Saylee on 19-Mar-2021 , as we needed report for Manually added Task Cards

					End If


				End If

				da = New ObjectAdapter
				mCompanyDetail = New CompanyDetail

				SearchStr1 = New SmartDate(Today.Date).FormattedText




				Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
						mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
						mCompanyDetail.WebSite, "", SearchStr1, "", "", AppSettings("ClientCode"), "", AppSettings("Product Version"), AppSettings("SINote"), mnWO.WONumber, mnWO.RegNo, "", mnWOJobTask.BarcodeNo, AppSettings("Logo"), AppSettings("ClientCode"), "Job #" + mnWOJob.SrNo.ToString) 'Dont Use SearchStr20 


				mrptImage = rptImage.GetImage(dsTaskCard, True)
				da.Fill(dsTaskCard, mrptImage)



				da.Fill(dsTaskCard, Report)
				da.Fill(dsTaskCard, mnWOJobTasks) 'Added by Saylee on 7-Oct-2014
				da.Fill(dsTaskCard, mnWOJobs) 'Added by Saylee on 7-Oct-2014
				''''If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "STR") Then da.Fill(dsTaskCard, "nWOJobTask", mnWOJobTask)
				da.Fill(dsTaskCard, "nWOJobTask", mnWOJobTask)
				'Added By Vikrant on 03-Mar-2020 For ALL03032020
				'da.Fill(dsTaskCard, "TaskCardSpares", mTaskCard.TaskCardSpares)
				'da.Fill(dsTaskCard, "TaskCardStepsSpares", mTaskCard.TaskCardStepsSpares)
				'da.Fill(dsTaskCard, "TaskCardPartRemovals", mnWOJobTask.TaskCardPartRemovals)
				da.Fill(dsTaskCard, "TaskCardSpares", mnWOJobTask.WOJobTaskSpares)
				da.Fill(dsTaskCard, "TaskCardStepsSpares", mnWOJobTask.WOJobTaskStepsSpares)
				da.Fill(dsTaskCard, "TaskCardPartRemovals", mnWOJobTask.WOJobTaskPartRemovals)
				'End

				If myReportChild IsNot Nothing Then
					myReportChild.SetDataSource(dsTaskCard)
					Session("myReportChild") = myReportChild
				End If


				'PDFNo = 1
				If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "BSA") Then
					myReportChild.Section8.SectionFormat.EnableSuppress = True
					myReportChild.Section9.SectionFormat.EnableSuppress = True
				End If
				tmpChild = aChild.Next

				'Dim MyFile1Child = "C:\Temp\" & tmpChild & PDFNo.ToString & ".pdf"

				'Dim MyFile1Child = "C:\Temp\" & tmpChild & PDFNoChild.ToString & ".pdf"
				Dim MyFile1Child = "C:\Temp\" & WONo & tmpChild & PDFNoChild.ToString & ".pdf"

				myReportChild = CType(Session("myReportChild"), Engine.ReportClass)

				Dim myDiskOptionChild As CrystalDecisions.Shared.DiskFileDestinationOptions


				myDiskOptionChild = New CrystalDecisions.Shared.DiskFileDestinationOptions
				myDiskOptionChild.DiskFileName = MyFile1Child
				myExportOption = myReportChild.ExportOptions
				With myExportOption
					.DestinationOptions = myDiskOptionChild
					.ExportDestinationType = ExportDestinationType.DiskFile
					.ExportFormatType = ExportFormatType.PortableDocFormat
				End With

				Try
					myReportChild.Export()
					myReportChild.Close()
					myReportChild.Dispose()
					GC.Collect()
				Catch ex As Exception
					Throw ex
				End Try


				'/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/
				pageCount = 0

				'Dim MyFile1Child_Ext As String = "C:\Temp\" & tmp & PDFNo.ToString & "_Ext" & ".pdf"
				Dim MyFile1Child_Ext As String = "C:\Temp\" & WONo & tmp & PDFNo.ToString & "_Ext" & ".pdf"
				Dim PageNumbersToExt As Integer()

				'Find "Cat." word in WO Print Out
				Dim TextToSearch As String = ""
				If Not mnWOJobTask.TaskCardID.Equals(Guid.Empty) Then
					If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "BSA") Then
						If mTaskCard.TaskCardTools.Count = 0 And mTaskCard.TaskCardSpares.Count = 0 Then
							TextToSearch = "CARD COMPLETE"
						ElseIf mTaskCard.TaskCardTools.Count > 0 Then
							TextToSearch = "Qty."
						ElseIf mTaskCard.TaskCardSpares.Count > 0 Then
							TextToSearch = "BATCH NUMBER"
						End If

					Else
						If mTaskCard.TaskCardStepsCount = 0 And mTaskCard.TaskCardStepsSpares.Count = 0 Then
							TextToSearch = "AME"
						ElseIf mTaskCard.TaskCardStepsSpares.Count > 0 Then
							TextToSearch = "PLACE"
						ElseIf mTaskCard.TaskCardStepsCount > 0 Then
							TextToSearch = "Signature"
						End If
					End If
					Dim Task_PageStartedFrom As Integer = getPageNoBySpecificText(1, MyFile1Child, TextToSearch) '"Autho. Rel.")
					ReDim PageNumbersToExt(Task_PageStartedFrom - 1)

					For PageNo As Integer = 1 To Task_PageStartedFrom
						PageNumbersToExt(PageNo - 1) = PageNo
					Next
				End If


				If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Novo") Then
					pdfList.Add(MyFile1Child)
				ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "STR") Then  'Added by Saylee on 13-Feb-2020 
					'do nothing :  'Added by Saylee on 13-Feb-2020 for STR13022020
					'As STR does not need TaskCard Print
					''Added by Saylee on 19-Mar-2020
					pdfList.Add(MyFile1Child)

				Else
					'If Not PageNumbersToExt Is Nothing Then
					'    ExtractPdfPage(MyFile1Child, PageNumbersToExt, MyFile1Child_Ext)
					'    pdfList.Add(MyFile1Child_Ext)
					'Else
					pdfList.Add(MyFile1Child)
					' End If

				End If
				'1. TaskCardAttachment with its extension
				PDFNo = PDFNo + 1
				PDFNoChild = PDFNoChild + 1

				MyFile1Child = Nothing
				myReportChild = Nothing
				Session.Remove("myReportChild")
				dsTaskCard.Clear()

				If Not mnWOJobTask.TaskCardID.Equals(Guid.Empty) Then
					Dim mTaskCardAttachment As TaskCardAttachment
					For j As Integer = 0 To mTaskCard.TaskCardAttachments.Count - 1
						mTaskCardAttachment = mTaskCard.TaskCardAttachments(j)

						If mTaskCardAttachment.ImageSize > 0 And LCase(mTaskCardAttachment.FileExtension) = ".pdf" Then
							'Dim ChildAttachment_path As String = "C:\Temp\" & tmp & PDFNo.ToString & mTaskCardAttachment.FileExtension
							''Dim ChildAttachment_path As String = "C:\Temp\" & tmpChild & PDFNoChild.ToString & mTaskCardAttachment.FileExtension
							Dim ChildAttachment_path As String = "C:\Temp\" & WONo & mTaskCard.TaskCardNo.Replace("/", "-").Replace("\", "-") & PDFNoChild.ToString & mTaskCardAttachment.FileExtension

							Dim fs As FileStream
							If File.Exists("C:\Temp\") = False Then
								File.Delete(ChildAttachment_path)
								fs = File.Create(ChildAttachment_path)
								fs.Write(mTaskCardAttachment.ImageFile, 0, mTaskCardAttachment.ImageFile.Length)
								fs.Close()

								pdfList.Add(ChildAttachment_path)                               '2. TaskCardAttachment attachment
								PDFNo = PDFNo + 1
								PDFNoChild = PDFNoChild + 1
							End If
						End If
						mTaskCardAttachment = Nothing

					Next
				Else
					If mnWOJobTask.ImageSize > 0 And LCase(mnWOJobTask.FileExtension) = ".pdf" Then
						'Dim ChildAttachment_path As String = "C:\Temp\" & tmp & PDFNo.ToString & mTaskCardAttachment.FileExtension
						''Dim ChildAttachment_path As String = "C:\Temp\" & tmpChild & PDFNoChild.ToString & mTaskCardAttachment.FileExtension
						Dim ChildAttachment_path As String = "C:\Temp\" & WONo & mTaskCard.TaskCardNo.Replace("/", "-").Replace("\", "-") & PDFNoChild.ToString & mnWOJobTask.FileExtension

						Dim fs As FileStream
						If File.Exists("C:\Temp\") = False Then
							File.Delete(ChildAttachment_path)
							fs = File.Create(ChildAttachment_path)
							fs.Write(mnWOJobTask.ImageFile, 0, mnWOJobTask.ImageFile.Length)
							fs.Close()

							pdfList.Add(ChildAttachment_path)                               '2. TaskCardAttachment attachment
							PDFNo = PDFNo + 1
							PDFNoChild = PDFNoChild + 1
						End If
					End If
				End If
				mTaskCard = Nothing
			Next
			If EachJobPrint = True And JobIndexToPrint <> -1 Then
				Exit For
			End If
		Next

		If AppSettings("ClientCode") = "RAL" Then
			For k As Integer = 0 To mnWONRCJobs.Count - 1  'Added by Saylee on 7-Mar-2014 for ALL07032014-1 : Only For loop for mnWOJobs added
				Dim mnWOJob As nWOJob = mnWONRCJobs(k)

				If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "BSA") Then
					myReportJob = New crnWOJobDetailForAttachBSA  'Added by Saylee on 7-Oct-2014
				Else
					myReportJob = New crnWOJobDetailForAttach
				End If


				Dim mnWOTools As nWOTools
				Dim mnWOJobSpares As nWOJobSpares
				Dim dsWOReg As New dsnWORegister
				mnWO = Session("mnWO")
				mnWOJobs = mnWO.WOJobs
				mnWOTools = mnWO.WOTools
				mnWOJobSpares = mnWONRCJobs(k).WOJobSpares 'nWOJobSpares.GetWOSpares(mnWO.ID, "")
				mnWOJobDesignationAllocations = mnWONRCJobs(k).WOJobDesignationAllocations 'nWOJobDesignationAllocations.GetWOJobDesignationAllocations(mnWO.ID, "")
				mnWOJobTasks = mnWONRCJobs(k).WOJobTasks
				mrptImage = rptImage.GetImage(dsWOReg, True)

				da.Fill(dsWOReg, mnWO)
				da.Fill(dsWOReg, mnWOJob)
				da.Fill(dsWOReg, mnWOJobDesignationAllocations)
				da.Fill(dsWOReg, mnWOJobSpares)
				da.Fill(dsWOReg, mnWOJobTasks)
				da.Fill(dsWOReg, mnWORegisterList)
				da.Fill(dsWOReg, Report)
				da.Fill(ds, mrptImage)

				myReportJob.SetDataSource(dsWOReg)
				Session("myReportJob") = myReportJob

				tmp = a.Next

				'MyFile1 = "C:\Temp\" & tmp & PDFNo.ToString & ".pdf"
				MyFile1 = "C:\Temp\" & WONo & tmp & PDFNo.ToString & ".pdf"

				myReportJob = CType(Session("myReportJob"), Engine.ReportClass)

				Dim myDiskOptionJob As CrystalDecisions.Shared.DiskFileDestinationOptions
				Dim CrFormatTypeOptions As New CrystalDecisions.Shared.PdfFormatOptions
				myDiskOptionJob = New CrystalDecisions.Shared.DiskFileDestinationOptions
				myDiskOptionJob.DiskFileName = MyFile1
				myExportOption = myReportJob.ExportOptions
				With myExportOption
					.DestinationOptions = myDiskOptionJob
					.ExportDestinationType = ExportDestinationType.DiskFile
					.ExportFormatType = ExportFormatType.PortableDocFormat
					.FormatOptions = CrFormatTypeOptions
				End With

				'Dim param As String = ""
				'If (Not parameters = DBNull.Value) Then
				'    For Each param In parameters.Keys
				'        myReportJob.SetParameterValue(param, parameters(param))
				'    Next
				'End If
				Try
					myReportJob.Export()
					myReportJob.Close()
					myReportJob.Dispose()
					GC.Collect()
				Catch ex As Exception
					Throw ex
				End Try


				pageCount = 0

				pdfList.Add(MyFile1)
				PDFNo = PDFNo + 1


				Dim aChild As New Random
				PDFNoChild = 1
			Next
		End If

		' //********************************************Send Files for Merging****************************************************//
		Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
		Dim MergedPath_WM As String = "C:\Temp\" & "temp_myMergedPdf_WM.pdf"

		Dim filesByte As New List(Of Byte())()
		For Each file__1 As String In pdfList 'files
			filesByte.Add(File.ReadAllBytes(file__1))
		Next

		File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))

		'AddWatermarkText(MergedPath, MergedPath_WM, mnWO.WONumber, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount)
		AddWatermarkText(MergedPath, MergedPath_WM, mnWO.WOText.ToString & "-" & mnWO.WONo.ToString, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount) 'Added on 24-Jun-2019
		''//********************************************Set Sessions*********************************************************//
		Session("CrystalReport") = MergedPath_WM
		Session("PrintReportWithAttachment") = "True"

		'//*******************************************Delete created file*********************************************************//

		'Commented and Added by Saylee on 2-Dec-2014
		' Dim MyFile, MyFile_Ext As String
		'For j As Integer = 1 To PDFNo - 1
		'    MyFile = "C:\Temp\" & WONo & j.ToString & ".pdf"
		'    MyFile_Ext = "C:\Temp\" & WONo & j.ToString & "_Ext" & ".pdf"

		'    File.Delete(MyFile)
		'    File.Delete(MyFile_Ext)
		'Next

		Dim DeleteThis As String = WONo
		Dim Files As String() = Directory.GetFiles("C:\Temp\")

		For Each file__1 As String In Files
			If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
				File.Delete(file__1)
			End If
		Next
		'End

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print With Task Attachments : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------


		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)
	End Sub

	'Added By Vikrant On 30-Apr-2014 For ALL30042014
	'Private Sub btnCRS_Click( sender As Object,  e As EventArgs) Handles btnCRS.Click
	'    Response.Redirect("wfAircraftCRS_Ajax.aspx?BackPage1=wfnWODetail.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
	'End Sub
	'End
	Public Sub PrintToolsSpares()
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		GetSession()
		Dim da As New ObjectAdapter
		Dim mCompanyDetail As New CompanyDetail

		'Dim mnWO As nWO
		Dim mnWOJobs As nWOJobs
		Dim mTaskCardTools As rptWOTaskCardTools
		Dim mTaskCardSpares As rptWOTaskCardSpares

		Dim mnWOJobTasks As nWOJobTasks


		Dim ds As New dsWOToolsSparesList

		Dim WOIssueNo As String = ""
		Dim WORevisionNo As String = ""


		Dim myReport = New crnWOToolsAndSpares

		Dim SearchStr1, SearchStr3 As String
		Dim SearchStr4, SearchStr5, SearchStr6 As String


		mnWO = Session("mnWO")
		mnWO = nWO.GetWO(mnWO.ID)
		mnWOJobs = mnWO.WOJobs
		mnWOJobTasks = nWOJobTasks.GetWOTasks(mnWO.ID, "")

		mTaskCardTools = rptWOTaskCardTools.GetTaskCardTools(mnWOJobTasks)
		mTaskCardSpares = rptWOTaskCardSpares.GetTaskCardSpares(mnWOJobTasks)


		If mTaskCardTools.Count = 0 And mTaskCardSpares.Count = 0 Then
			MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There are no Tools and Spares for this Work Order", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		myReport.SetDataSource(ds)

		WOIssueNo = AppSettings("WOIssueNo")
		'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
		' WORevisionNo = AppSettings("WORevisionNo")
		WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
		'-----
		SearchStr3 = txtNo.Text

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
					  mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
					  mCompanyDetail.WebSite, "", SearchStr1, WOIssueNo, WORevisionNo, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6, AppSettings("ClientCode"), AppSettings("Government Authority"), , AppSettings("Logo")) 'Dont Use SearchStr20 

		Dim mrptImage As rptImage = rptImage.GetImage(ds)

		'WO Detail
		da.Fill(ds, mnWO)
		da.Fill(ds, mnWOJobs)
		da.Fill(ds, "rptWOTaskCardTools", mTaskCardTools)
		da.Fill(ds, "rptWOTaskCardSpares", mTaskCardSpares)
		da.Fill(ds, Report)
		da.Fill(ds, mrptImage)

		myReport.SetDataSource(ds)

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print Tools and Spares  : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------

		Session("CrystalReport") = myReport
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)
	End Sub

	Public Sub PrintProductionPlanningForm()
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		GetSession()
		Dim da As New ObjectAdapter
		Dim mCompanyDetail As New CompanyDetail

		Dim ds As New dsnWODetail

		Dim WOIssueNo As String = ""
		Dim WORevisionNo As String = ""


		Dim myReport = New crnProductionPlanningForm

		Dim SearchStr1, SearchStr3 As String
		Dim SearchStr4, SearchStr5, SearchStr6 As String


		'mnWO = Session("mnWO")
		'mnWO = nWO.GetWO(mnWO.ID)

		'myReport.SetDataSource(ds)

		WOIssueNo = AppSettings("WOIssueNo")
		WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
		'-----
		SearchStr3 = txtNo.Text

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
					  mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
					  mCompanyDetail.WebSite, "", SearchStr1, WOIssueNo, WORevisionNo, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6, AppSettings("ClientCode"), AppSettings("Government Authority"), , AppSettings("Logo")) 'Dont Use SearchStr20 

		Dim mrptImage As rptImage = rptImage.GetImage(ds)

		'WO Detail
		da.Fill(ds, mnWO)
		da.Fill(ds, mnWO.WOJobs)
		da.Fill(ds, mnWO.WOJobs(0).WOJobSpares)
		da.Fill(ds, mnWO.WOTools)
		da.Fill(ds, Report)
		da.Fill(ds, mrptImage)

		myReport.SetDataSource(ds)


		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print Tools and Spares  : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------

		Session("CrystalReport") = myReport
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)
	End Sub

	Public Sub PrintCAMO()
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		Dim da As New ObjectAdapter
		Dim myReport As Engine.ReportDocument 'Engine.ReportClass
		Dim mCompanyDetail As New CompanyDetail
		Dim ds As New dsnWODetail
		Dim mnWOJobs As nWOJobs
		Dim mnWOJobComps As nWOJobComps
		Dim mnWOJobDesignationAllocations As nWOJobDesignationAllocations 'Added By Vikrant On 24-June-2013 For Indamer21062013
		Dim WODocumentNo As String = ""
		Dim WORevisionNo As String = ""
		Dim FormNo As String = ""
		Dim IssueNo As String = ""

		Dim Searchstr7 As String = ""
		Dim LastLogDate As String = ""
		Dim LastLogDateHavingAPUValues As String = ""

		Dim ReportTitle As String = "CAMO-Work Order"

		Dim EOFooterLine As String = ""

		EOFooterLine = CType(AppSettings("EOFooterLine"), String)


		myReport = New crnWOCAMOWO
		mnWO = Session("mnWO")
		mnWOJobs = mnWO.WOJobs
		mnWOJobComps = nWOJobComps.GetWOJobComps(mnWO.ID, "")
		mnWOJobDesignationAllocations = nWOJobDesignationAllocations.GetWOJobDesignationAllocations(mnWO.ID, "")  'Added By Vikrant On 24-June-2013 For Indamer21062013
		FormNo = AppSettings("WoNo")

		Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(mnWO.MachineID)
		If mMachineOperatorName.OperatorName <> "" Then Searchstr7 = mMachineOperatorName.OperatorName

		WODocumentNo = AppSettings("WODocumentNo")
		'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
		' WORevisionNo = AppSettings("WORevisionNo")
		WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
		'-----
		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
			mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
			mCompanyDetail.WebSite, ReportTitle, EOFooterLine, WODocumentNo, WORevisionNo, AppSettings("ClientCode"), FormNo, AppSettings("Product Version"),
			AppSettings("SINote"), IssueNo, Searchstr7, "", AppSettings("Government Authority"), AppSettings("Logo"), SearchStr11:=LastLogDate,
			SearchStr12:=LastLogDateHavingAPUValues, SearchStr13:=AppSettings("CRS"), SearchStr14:=mnWO.WOJobs(0).WOJobTypeID.ToString) 'Dont Use SearchStr20 

		Dim mrptImage As rptImage = rptImage.GetImage(ds)

		da.Fill(ds, mnWO)
		da.Fill(ds, mnWOJobs)
		da.Fill(ds, mnWOJobComps)
		da.Fill(ds, mnWOJobDesignationAllocations) 'Added By Vikrant On 24-June-2013 For Indamer21062013
		da.Fill(ds, Report)

		da.Fill(ds, mrptImage)

		myReport.SetDataSource(ds)

		Session("CrystalReport") = myReport

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print CAMO : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------

		''Dim str As String
		''str = "<script language=Javascript>openTranDetail();</script>"
		''ClientScript.RegisterStartupScript([GetType], "openTranDetail", str)
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)
	End Sub

	Public Sub PrintNRC()

		If Not IsInRole(Rights.Print) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If


		Dim pdfList As New ArrayList

		Dim PDFNo As Integer = 1
		Dim PDFNoChild As Integer = 1
		PDFNo = PDFNo + 1

		Dim tmp As Integer
		Dim a As New Random

		tmp = a.Next

		Dim myExportOption As ExportOptions
		Dim myDiskOption As DiskFileDestinationOptions
		Dim pageCount As Integer = 0
		myDiskOption = New DiskFileDestinationOptions

		For Each WONRCJob As nWOJob In mnWO.WONRCJobs
			Dim da As New ObjectAdapter
			Dim mCompanyDetail As New CompanyDetail
			Dim mnWOTools As nWOTools
			Dim mnWOPeriods As nWOPeriods
			Dim mnWOJobTasks As nWOJobTasks
			Dim mnrptWOJobResourceDetails As nrptWOJobResourceDetails
			Dim mnWOJobSpares As nWOJobSpares
			Dim mnWOJobComps As nWOJobComps
			Dim SearchStr1 As String = New SmartDate(Today.Date).FormattedText
			' Dim rpt As New crnWOJobDetail
			Dim ds As New dsnWODetail

			Dim myReport As Engine.ReportClass
			If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
				myReport = New crnWOJobDetailTAAL
			ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "STR") Then  'Added by Saylee on 13-Aug-2018  for StarAir13082018-1
				myReport = New crnWOJobDetailSTR
			ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "IND") Then
				myReport = New crnOffJobSheet
			Else
				myReport = New crnWOJobDetail
			End If

			Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
				   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
				   mCompanyDetail.WebSite, IIf(AppSettings("ClientCode") = "KAS", "Defect Task Card", "NRC Details"), SearchStr1, AppSettings("WO-NRCIssueRev"),
				   mnWO.WONumber + "-" + WONRCJob.SrNo.ToString, AppSettings("ClientCode"), "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "",
				   SearchStr9:=AppSettings("ClientCode"), SearchStr10:=AppSettings("Logo")) 'Dont Use SearchStr20 

			mnWO = Session("mnWO")

			mnWOTools = mnWO.WOTools
			mnWOPeriods = mnWO.WOPeriods
			mnWOJobTasks = WONRCJob.WOJobTasks
			mnrptWOJobResourceDetails = nrptWOJobResourceDetails.GetrptWOJobResourceDetails(WONRCJob.ID.ToString)
			mnWOJobSpares = WONRCJob.WOJobSpares
			mnWOJobComps = WONRCJob.WOJobComps

			da.Fill(ds, mnWO)
			da.Fill(ds, WONRCJob)
			da.Fill(ds, mnWOTools)
			da.Fill(ds, mnWOPeriods)
			da.Fill(ds, mnWOJobTasks)
			da.Fill(ds, mnrptWOJobResourceDetails)
			da.Fill(ds, mnWOJobSpares)
			da.Fill(ds, mnWOJobComps)
			da.Fill(ds, Report)
			Dim mrptImage As rptImage = rptImage.GetImage(ds)
			da.Fill(ds, mrptImage)
			myReport.SetDataSource(ds)
			Session("CrystalReport") = myReport

			Dim MyFile1 = "C:\Temp\" & mnWO.WOText.ToString.Replace("/", "-").Replace("\", "-") & "-" & mnWO.WONo.ToString + "-" + WONRCJob.SrNo.ToString & ".pdf" 'Added on 24-Jun-2019
			myDiskOption.DiskFileName = MyFile1
			myExportOption = myReport.ExportOptions
			With myExportOption
				.DestinationOptions = myDiskOption
				.ExportDestinationType = ExportDestinationType.DiskFile
				.ExportFormatType = ExportFormatType.PortableDocFormat
			End With
			myReport.Export()
			myReport.Close()
			myReport.Dispose()
			GC.Collect()

			pdfList.Add(MyFile1)
		Next

		'here WOjob NRC's

		Dim mnWOJob As nWOJob
		For Each mnTempWOJob As nWOJob In mnWO.WOJobs
			Dim mWOJobNRCList As WOJobNRCList

			mWOJobNRCList = WOJobNRCList.GetWOJobNRCList(mnWO.ID, mnTempWOJob.ID)
			For Each mnWOJobNRC As WOJobNRCList.WOJobNRCListInfo In mWOJobNRCList
				mnWOJob = nWOJob.GetWOJob(mnWOJobNRC.ID)

				Dim da As New ObjectAdapter
				Dim mCompanyDetail As New CompanyDetail
				Dim mnWOTools As nWOTools
				Dim mnWOPeriods As nWOPeriods
				Dim mnWOJobTasks As nWOJobTasks
				Dim mnrptWOJobResourceDetails As nrptWOJobResourceDetails
				Dim mnWOJobSpares As nWOJobSpares
				Dim mnWOJobComps As nWOJobComps
				Dim SearchStr1 As String = New SmartDate(Today.Date).FormattedText
				Dim rpt As New crnWOJobDetail
				Dim ds As New dsnWODetail

				Dim myReport As Engine.ReportClass
				If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
					myReport = New crnWOJobDetailTAAL
				ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Novo") Then
					myReport = New crnWOJobDetailNOVO
				ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "STR") Then 'Added by Saylee on 13-Aug-2018  for StarAir13082018-1
					myReport = New crnWOJobDetailSTR
				Else
					myReport = New crnWOJobDetail
				End If

				Dim mnWOJobParent As nWOJob = mnTempWOJob
				Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
						  mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
						  mCompanyDetail.WebSite, IIf(AppSettings("ClientCode") = "KAS", "Defect Task Card", "NRC Details"), SearchStr1, AppSettings("WO-NRCIssueRev"), mnWO.WONumber + "-" + mnWOJobParent.SrNo.ToString + "-" + mnWOJob.SrNo.ToString, mnWOJobParent.WorkPACKREF, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", AppSettings("ClientCode"), AppSettings("Logo")) 'Dont Use SearchStr20 

				mnWO = Session("mnWO")


				mnWOTools = mnWO.WOTools
				mnWOPeriods = mnWO.WOPeriods

				mnWOJobTasks = mnWOJob.WOJobTasks
				mnrptWOJobResourceDetails = nrptWOJobResourceDetails.GetrptWOJobResourceDetails(mnWOJob.ID.ToString)
				mnWOJobSpares = mnWOJob.WOJobSpares
				mnWOJobComps = mnWOJob.WOJobComps

				da.Fill(ds, mnWO)
				da.Fill(ds, mnWOJob)
				da.Fill(ds, mnWOTools)
				da.Fill(ds, mnWOPeriods)
				da.Fill(ds, mnWOJobTasks)
				da.Fill(ds, mnrptWOJobResourceDetails)
				da.Fill(ds, mnWOJobSpares)
				da.Fill(ds, mnWOJobComps)
				da.Fill(ds, Report)
				Dim mrptImage As rptImage = rptImage.GetImage(ds)
				da.Fill(ds, mrptImage)
				myReport.SetDataSource(ds)
				Session("CrystalReport") = myReport

				'Added on 24-Jun-2019
				Dim MyFile1 = "C:\Temp\" & mnWO.WOText.ToString.Replace("/", "-").Replace("\", "-") & "-" & mnWO.WONo.ToString + "-" + mnWOJobParent.SrNo.ToString + "-" + mnWOJob.SrNo.ToString & ".pdf"
				myDiskOption.DiskFileName = MyFile1
				myExportOption = myReport.ExportOptions
				With myExportOption
					.DestinationOptions = myDiskOption
					.ExportDestinationType = ExportDestinationType.DiskFile
					.ExportFormatType = ExportFormatType.PortableDocFormat
				End With
				myReport.Export()
				myReport.Close()
				myReport.Dispose()
				GC.Collect()

				pdfList.Add(MyFile1)
			Next

		Next


		Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
		Dim MergedPath_WM As String = IIf(AppSettings("ClientCode") = "IND", "C:\Temp\" & "OJS.pdf", "C:\Temp\" & "NRC.pdf")

		Dim filesByte As New List(Of Byte())()
		For Each file__1 As String In pdfList 'files
			filesByte.Add(File.ReadAllBytes(file__1))
		Next

		File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))

		'AddWatermarkText(MergedPath, MergedPath_WM, mnWO.WOText, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount)
		AddWatermarkText(MergedPath, MergedPath_WM, mnWO.WOText.ToString & "-" & mnWO.WONo.ToString, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount) 'Added on 24-Jun-2019
		''//********************************************Set Sessions*********************************************************//


		Dim DeleteThis As String = mnWO.WOText
		Dim Files As String() = Directory.GetFiles("C:\Temp\")

		For Each file__1 As String In Files
			If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
				File.Delete(file__1)
			End If
		Next

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", IIf(AppSettings("ClientCode") = "IND", "Work Order Print OJS : ", "Work Order Print NRC : ") + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------

		Session("PrintReportWithAttachment") = "True"
		Session("CrystalReport") = MergedPath_WM
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)


	End Sub

	Public Sub PrintCRS()
		Dim myReport As Engine.ReportClass
		Dim da As New ObjectAdapter
		Dim ds As New dsnWORegister
		Dim mCompanyDetail As New CompanyDetail
		Dim mMaintenanceActivityValuesList As MaintenanceActivityValuesList
		Dim NextDueValues As New StringBuilder

		If AppSettings("ClientCode") = "IND" Then
			For Each mnWOJob As nWOJob In mnWO.WOJobs
				If mnWOJob.WOJobTypeID = 2 Then 'Scheduled Job
					If mnWOJob.OnTypeID = 1 Then 'Assembly
						If mnWOJob.MonitorTypeID = 1 Then 'Service
							mMaintenanceActivityValuesList = MaintenanceActivityValuesList.GetList(5, mnWOJob.PreviousTransID, mnWOJob.ID)
						ElseIf mnWOJob.MonitorTypeID = 2 Then 'Inspection
							mMaintenanceActivityValuesList = MaintenanceActivityValuesList.GetList(6, mnWOJob.PreviousTransID, mnWOJob.ID)
						ElseIf mnWOJob.MonitorTypeID = 3 Then 'Directive
							mMaintenanceActivityValuesList = MaintenanceActivityValuesList.GetList(7, mnWOJob.PreviousTransID, mnWOJob.ID)
						End If
					ElseIf mnWOJob.OnTypeID = 2 Then 'Component
						If mnWOJob.MonitorTypeID = 1 Then 'Service
							mMaintenanceActivityValuesList = MaintenanceActivityValuesList.GetList(8, mnWOJob.PreviousTransID, mnWOJob.ID)
						ElseIf mnWOJob.MonitorTypeID = 2 Then 'Inspection
							mMaintenanceActivityValuesList = MaintenanceActivityValuesList.GetList(9, mnWOJob.PreviousTransID, mnWOJob.ID)
						ElseIf mnWOJob.MonitorTypeID = 3 Then 'Directive
							mMaintenanceActivityValuesList = MaintenanceActivityValuesList.GetList(10, mnWOJob.PreviousTransID, mnWOJob.ID)
						End If
					End If
					Exit For

				End If
			Next

			If mMaintenanceActivityValuesList IsNot Nothing Then
				For i As Integer = 0 To mMaintenanceActivityValuesList.Count - 1
					If mMaintenanceActivityValuesList(i).PeriodID = 1 Then
						If NextDueValues.ToString <> "" Then
							NextDueValues.Append(" ")
						End If
						NextDueValues.Append(New Period(mMaintenanceActivityValuesList(i).PeriodID, (mMaintenanceActivityValuesList(i).FrequencyValue + (New Period(mMaintenanceActivityValuesList(i).PeriodID, mnWO.AirFrameHrs, mMaintenanceActivityValuesList(i).PeriodUnitID, False, False, 1).DbValueDec)), mMaintenanceActivityValuesList(i).PeriodUnitID, False, False, 1).ValueFormatted)
						NextDueValues.Append(" H")
					ElseIf mMaintenanceActivityValuesList(i).PeriodID = 2 Then
						If NextDueValues.ToString <> "" Then
							NextDueValues.Append(" ")
						End If
						If mMaintenanceActivityValuesList(i).PeriodUnitID = 3 Then
							NextDueValues.Append(DateAdd(DateInterval.Day, mMaintenanceActivityValuesList(i).FrequencyValue, Today.Date).ToString(AppSettings("DateFormat")))
						ElseIf mMaintenanceActivityValuesList(i).PeriodUnitID = 4 Then
							NextDueValues.Append(DateAdd(DateInterval.Month, mMaintenanceActivityValuesList(i).FrequencyValue, Today.Date).ToString(AppSettings("DateFormat")))
						ElseIf mMaintenanceActivityValuesList(i).PeriodUnitID = 5 Then
							NextDueValues.Append(DateAdd(DateInterval.Year, mMaintenanceActivityValuesList(i).FrequencyValue, Today.Date).ToString(AppSettings("DateFormat")))
						End If
					ElseIf mMaintenanceActivityValuesList(i).PeriodID = 3 Then
						If NextDueValues.ToString <> "" Then
							NextDueValues.Append(" ")
						End If
						NextDueValues.Append(mMaintenanceActivityValuesList(i).FrequencyValue + CDec(mnWO.Cycles))
						NextDueValues.Append(" C")
					ElseIf mMaintenanceActivityValuesList(i).PeriodID = 7 Then
						If NextDueValues.ToString <> "" Then
							NextDueValues.Append(" ")
						End If
						NextDueValues.Append(mMaintenanceActivityValuesList(i).FrequencyValue + CDec(mnWO.Landings))
						NextDueValues.Append(" L")
					End If
				Next
			End If
		End If

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "STR" Then
			myReport = New crptAircraftCertificateofReleaseToServiceSTR
		ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "IND" Then
			myReport = New crptAircraftCertificateofReleaseToServiceIndamer
		ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "KAS" Then
			myReport = New crnCRS
		End If


		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
			  mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
			  mCompanyDetail.WebSite, "", "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"), SearchStr11:=AppSettings("ClientCode"), SearchStr12:=AppSettings("WO-CRSIssueRev"), SearchStr13:=NextDueValues.ToString) 'Dont Use SearchStr20 
		ds.Clear()
		Dim mrptImage As rptImage = rptImage.GetImage(ds)
		da.Fill(ds, mrptImage)
		da.Fill(ds, Report)
		da.Fill(ds, mnWO)
		da.Fill(ds, mnWO.WOJobs)
		myReport.SetDataSource(ds)
		Session("CrystalReport") = myReport

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print CRS : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------

		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)
	End Sub

	Public Sub PrintLogBookEntry() 'Fn same as Print

		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		Dim da As New ObjectAdapter
		Dim myReport As Engine.ReportDocument 'Engine.ReportClass
		Dim mCompanyDetail As New CompanyDetail
		Dim ds As New dsnWODetail
		Dim mnWOJobs As nWOJobs
		Dim mnWOJobComps As nWOJobComps
		Dim mnWOJobDesignationAllocations As nWOJobDesignationAllocations 'Added By Vikrant On 24-June-2013 For Indamer21062013
		Dim mnWONRCJobs As nWOJobs

		Dim WODocumentNo As String = ""
		Dim WORevisionNo As String = ""
		Dim FormNo As String = ""
		Dim IssueNo As String = ""

		Dim Searchstr7 As String = ""
		Dim LastLogDate As String = ""
		Dim LastLogDateHavingAPUValues As String = ""

		Dim ReportTitle As String
		If AppSettings("ClientCode") = "IND" Then
			ReportTitle = mnWO.WOJobs(0).AssemblyTypeWithPosition.Split("(")(0).ToUpper + " LOG BOOK ENTRY"
		Else
			ReportTitle = "LOG BOOK ENTRY"
		End If
		Dim EOFooterLine As String = ""

		Dim mnWORegisterList As nWORegisterList

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then EOFooterLine = CType(AppSettings("EOFooterLine"), String)


		If AppSettings("ClientCode") = "IND" Then
			myReport = New crptLogEntryFormatIND
		End If
		mnWO = Session("mnWO")
		mnWOJobs = mnWO.WOJobs
		mnWOJobComps = nWOJobComps.GetWOJobComps(mnWO.ID, "")
		mnWOJobDesignationAllocations = nWOJobDesignationAllocations.GetWOJobDesignationAllocations(mnWO.ID, "")  'Added By Vikrant On 24-June-2013 For Indamer21062013
		FormNo = AppSettings("WoNo")

		If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

			' Added By Vikrant On 13-May-2013 For BA13052013

			mnWORegisterList = nWORegisterList.GetnWORegisterList(mnWO.WOText, mnWO.WONo, , , mnWO.RegNo, , mnWO.SerialNo)
			da.Fill(ds, mnWORegisterList)
			'End
		ElseIf AppSettings("ClientCode") = "Indamer" Then  'Added By Vikrant On 14-May-2013 For IND14052013
			Dim mtmpMachineList As tmpMachineList
			Dim ReportStatusList As New rptStatusList
			mtmpMachineList = tmpMachineList.GetMachineList(, mnWO.RegNo, , , , , True, mnWO.WODate.ToString)
			For i As Integer = 0 To mtmpMachineList.Count - 1
				ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , , , , , , , , , , , , mtmpMachineList(i).Cycles, , , Year(New SmartDate(mnWO.WODate.ToString).FormattedText).ToString, , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
			Next
			da.Fill(ds, ReportStatusList)

			Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(mnWO.MachineID)
			If mMachineOperatorName.OperatorName <> "" Then Searchstr7 = mMachineOperatorName.OperatorName
		End If

		'Added by Saylee on 11-Oct-2018 for ALL11102018
		If mnWO.IsDigitalSignatureAdded Then
			mFileAttachnWO = FileAttach.GetAttachment(mnWO.ID, , "DigitalSignatureWO", ds, AppSettings("DOCPath"))
			da.Fill(ds, "FileAttach", mFileAttachnWO)
		End If
		'***************************
		Dim EmpName As String = ""

		Dim mEmployee As Employee
		If Not mnWO.EmployeeID.Equals(Guid.Empty) Then
			mEmployee = Employee.GetEmployee(mnWO.EmployeeID)
			EmpName = mEmployee.Name
		End If

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
			mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
			mCompanyDetail.WebSite, ReportTitle, EOFooterLine, WODocumentNo, WORevisionNo, AppSettings("ClientCode"), FormNo, AppSettings("Product Version"),
			AppSettings("SINote"), IssueNo, Searchstr7, "", AppSettings("Government Authority"), AppSettings("Logo"), SearchStr11:=LastLogDate,
			SearchStr12:=LastLogDateHavingAPUValues, SearchStr13:=AppSettings("CRS"), SearchStr14:=mnWO.WOJobs(0).WOJobTypeID.ToString, SearchStr15:=EmpName) 'Dont Use SearchStr20 

		Dim mrptImage As rptImage = rptImage.GetImage(ds)


		da.Fill(ds, mnWO)
		da.Fill(ds, mnWOJobs)
		da.Fill(ds, mnWOJobComps)
		da.Fill(ds, mnWOJobDesignationAllocations) 'Added By Vikrant On 24-June-2013 For Indamer21062013
		da.Fill(ds, Report)

		da.Fill(ds, mrptImage)


		If AppSettings("ClientCode") = "RAL" Then
			mnWONRCJobs = mnWO.WONRCJobs
			da.Fill(ds, mnWONRCJobs)
		End If

		myReport.SetDataSource(ds)

		Session("CrystalReport") = myReport
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Log Book Entry Print : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
	End Sub

	Public Sub PrintTaskCardExcel() ''Added by Saylee on 26-Feb-2019 for ALL27022019

		Dim mnWOTaskCardListForExcel As nWOTaskCardListForExcel
		mnWOTaskCardListForExcel = nWOTaskCardListForExcel.GetnWOTaskCardListForExcel(mnWO.ID)

		Dim da As New ObjectAdapter
		Dim ds As New dsnWOTaskCardListForExcel
		Dim mCompanyDetail As New CompanyDetail

		If mnWOTaskCardListForExcel.Count > 0 Then
			da.Fill(ds, "nWOTaskCardListForExcel", mnWOTaskCardListForExcel)
			Dim columnToRemove1 As String()
			columnToRemove1 = {"WOID", "WOText", "WONo", "WOText", "WODate", "CallOutID", "StatusID", "MachineID", "CustomerID", "CustomerAddress", "WOStartDate",
							   "WOCloseDate", "WOPlanedDate", "WOBy", "WORemark", "WOStatusID", "IsAuthorized", "AuthorizedBy", "IsClosed", "ClosedBy",
							   "HourType", "IsSync", "WorkShopID", "WorkShopName", "LogID", "IsInHouse", "IsThirdParty", "WOJobTypeID", "WOTotalActualTime", "UserName",
							   "Eng1SrNo", "Eng2SrNo", "Eng1CurHr", "Eng2CurHr", "Eng1", "Eng2", "Eng1Model", "Eng2Model", "WODay", "WOMonth", "WOYear", "WOIssuedSparesCount",
							   "WOJobID", "WOJobDescription", "WOJobAction", "WOJobStartDate", "WOJobCloseDate", "WOJobRemark", "WOJobStatusID", "WOJobStatusName", "WOJobTaskID",
							   "WOJobTaskIsDone", "Eng3", "Eng4", "Eng3SrNo", "Eng4SrNo", "Eng3CurHr", "Eng4CurHr", "Eng3Model", "Eng4Model", "EngCount", "IsRII",
							   "DueAsOfHours", "DueAsOfLanding", "DueAsOfDate", "DueAsOfCycle", "GroupBy", "Heading", "AMPRevDate", "FormNo", "IssueNo", "TaskCardStepsCount",
							   "ToolsCount", "Eng1CurCycles", "Eng2CurCycles", "Eng3CurCycles", "Eng4CurCycles", "APUModel", "APUSrNo", "APUCurHr", "DoneOnHours", "DoneOnCycles",
							   "DoneOnDate", "FreqHours", "FreqCycles", "FreqForDate", "CustomerWONo", "IsCustApprovedObtained", "AssemblyTypeWithPosition",
							   "DueAsOf", "StatusName", "WOStatusName", "WOActualTime", "RevisionNo", "APUCurCycles", "APU", "CustApprovedByEmailWO", "WOJobCloseDateFormatted",
							   "WOJobStartDateFormatted", "WOJobTaskActualStartDate", "WOJobTaskActualEndDate", "WOCloseDateFormatted", "WOStartDateFormatted", "WOJobTaskTaskAction",
							   "TaskCardDescription", "TaskSourceReference", "TaskSubject", "TaskCardID"}

			For i As Integer = 0 To columnToRemove1.Length - 1
				If ds.Tables("nWOTaskCardListForExcel").Columns.Contains(columnToRemove1(i)) Then
					ds.Tables("nWOTaskCardListForExcel").Columns.Remove(columnToRemove1(i))
				End If
			Next

			ds.Tables("nWOTaskCardListForExcel").Columns("WONumber").SetOrdinal(0)
			ds.Tables("nWOTaskCardListForExcel").Columns("WODateFormatted").SetOrdinal(1)
			ds.Tables("nWOTaskCardListForExcel").Columns("RegNo").SetOrdinal(2)
			ds.Tables("nWOTaskCardListForExcel").Columns("ModelName").SetOrdinal(3)
			ds.Tables("nWOTaskCardListForExcel").Columns("SerialNo").SetOrdinal(4)

			ds.Tables("nWOTaskCardListForExcel").Columns("WOJobSrNo").SetOrdinal(5)
			ds.Tables("nWOTaskCardListForExcel").Columns("TaskSourceReferenceExcel").SetOrdinal(6)
			ds.Tables("nWOTaskCardListForExcel").Columns("WOJobDescriptionExcel").SetOrdinal(7)
			ds.Tables("nWOTaskCardListForExcel").Columns("ATAChapter").SetOrdinal(8)
			ds.Tables("nWOTaskCardListForExcel").Columns("CustomerName").SetOrdinal(9)


			ds.Tables("nWOTaskCardListForExcel").Columns("WOJobTypeName").SetOrdinal(10)

			ds.Tables("nWOTaskCardListForExcel").Columns("WOJobActionExcel").SetOrdinal(11)
			ds.Tables("nWOTaskCardListForExcel").Columns("WorkPACKREF").SetOrdinal(12)
			ds.Tables("nWOTaskCardListForExcel").Columns("DueAsOfExcel").SetOrdinal(13)

			ds.Tables("nWOTaskCardListForExcel").Columns("WOJobTaskSrNo").SetOrdinal(14)

			ds.Tables("nWOTaskCardListForExcel").Columns("TaskCardNo").SetOrdinal(15)


			ds.Tables("nWOTaskCardListForExcel").Columns("TaskHeading").SetOrdinal(16)
			ds.Tables("nWOTaskCardListForExcel").Columns("TaskSubjectExcel").SetOrdinal(17)
			ds.Tables("nWOTaskCardListForExcel").Columns("WOJobTaskTaskActionExcel").SetOrdinal(18)

			ds.Tables("nWOTaskCardListForExcel").Columns("INSPTypeInterval").SetOrdinal(19)
			ds.Tables("nWOTaskCardListForExcel").Columns("TaskCardDescriptionExcel").SetOrdinal(20)

			ds.Tables("nWOTaskCardListForExcel").Columns("TaskCardReference").SetOrdinal(21)

			ds.Tables("nWOTaskCardListForExcel").Columns("Zone").SetOrdinal(22)
			ds.Tables("nWOTaskCardListForExcel").Columns("AREA").SetOrdinal(23)
			ds.Tables("nWOTaskCardListForExcel").Columns("Publication").SetOrdinal(24)
			ds.Tables("nWOTaskCardListForExcel").Columns("Skill").SetOrdinal(25)
			ds.Tables("nWOTaskCardListForExcel").Columns("Panels").SetOrdinal(26)
			ds.Tables("nWOTaskCardListForExcel").Columns("InspCode").SetOrdinal(27)

			ds.Tables("nWOTaskCardListForExcel").Columns("AMPRevNo").SetOrdinal(28)
			ds.Tables("nWOTaskCardListForExcel").Columns("AMPRevDateFormatted").SetOrdinal(29)

			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("WONumber") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("WONumber").ColumnName = "W.O.No."
			End If

			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("ATAChapter") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("ATAChapter").ColumnName = "ATA"
			End If


			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("WODateFormatted") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("WODateFormatted").ColumnName = "Date"
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("RegNo") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("RegNo").ColumnName = "Reg.No."
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("ModelName") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("ModelName").ColumnName = "Model No."
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("SerialNo") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("SerialNo").ColumnName = "Serial No."
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("CustomerName") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("CustomerName").ColumnName = "Customer"
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("WOJobTypeName") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("WOJobTypeName").ColumnName = "Job Type"
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("WOJobSrNo") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("WOJobSrNo").ColumnName = "Job SrNo."
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("WOJobDescriptionExcel") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("WOJobDescriptionExcel").ColumnName = "Job Description."
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("WOJobActionExcel") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("WOJobActionExcel").ColumnName = "Action"
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("WOJobTaskSrNo") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("WOJobTaskSrNo").ColumnName = "Task SrNo."
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("WOJobTaskTaskActionExcel") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("WOJobTaskTaskActionExcel").ColumnName = "Task Card Action"
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("TaskCardNo") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("TaskCardNo").ColumnName = "Task Card No."
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("TaskCardReference") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("TaskCardReference").ColumnName = "Task Card Reference"
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("TaskCardDescriptionExcel") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("TaskCardDescriptionExcel").ColumnName = "Task Card Description"
			End If

			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("DueAsOfExcel") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("DueAsOfExcel").ColumnName = "Due As Of"
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("TaskSourceReferenceExcel") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("TaskSourceReferenceExcel").ColumnName = "Task Source Ref."
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("AMPRevNo") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("AMPRevNo").ColumnName = "AMP RevNo."
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("AMPRevDateFormatted") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("AMPRevDateFormatted").ColumnName = "AMP Rev Date"
			End If

			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("TaskHeading") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("TaskHeading").ColumnName = "Task Card Heading"
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("TaskSubjectExcel") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("TaskSubjectExcel").ColumnName = "Task Card Subject"
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("INSPTypeInterval") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("INSPTypeInterval").ColumnName = "Task Card Interval"
			End If
			If ds.Tables("nWOTaskCardListForExcel").Columns.Contains("WorkPACKREF") Then
				ds.Tables("nWOTaskCardListForExcel").Columns("WorkPACKREF").ColumnName = "Work Pack Ref."
			End If

			Dim dsNew As New DataSet
			dsNew.Clear()

			dsNew.Merge(ds.Tables("nWOTaskCardListForExcel"))
			dsNew.Tables("nWOTaskCardListForExcel").TableName = "Task Card Report"
			Session("dsNew") = dsNew
			Session("DataTableToBeFormattedForExportToExcel") = "Task Card Report"
			Session("ExcelFileName") = "Task Card Report"

			'Added on 15-Mar-2019
			mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
			MarkLog(Action.Print, "Work Order", "Work Order Print for Export Task Card to Excel : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
			'-------

			ScriptManager.RegisterStartupScript(Me, [GetType], "openFilel", "openFile();", True)
		End If
	End Sub


	Public Sub PrintTallySheet()  ''Added by Saylee on 15-Nov-2021 for STR15092021
		Dim mnWOTaskCardListForExcel As nWOTaskCardListForExcel
		mnWOTaskCardListForExcel = nWOTaskCardListForExcel.GetnWOTaskCardListForExcel(mnWO.ID)

		Dim WORevisionNo As String = ""
		Dim FormNo As String = ""
		Dim WOIssueNo As String = ""

		WORevisionNo = mTransactionList.Item(mnWO.TransTypeID).FormRevisionNo
		WOIssueNo = AppSettings("WOIssueNo")
		FormNo = AppSettings("FormNo")
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		Dim da As New ObjectAdapter
		Dim myReport As Engine.ReportClass
		Dim mCompanyDetail As New CompanyDetail
		Dim ds As New dsnWOTaskCardListForExcel


		myReport = New crnTallySheetForSTR

		mnWO = Session("mnWO")

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
				mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
				mCompanyDetail.WebSite, "", FormNo, WOIssueNo, WORevisionNo, "", "", AppSettings("Product Version"), AppSettings("SINote"), SearchStr10:=AppSettings("Logo")) 'Dont Use SearchStr20 

		Dim mrptImage As rptImage = rptImage.GetImage(ds, True)
		da.Fill(ds, Report)
		da.Fill(ds, mnWOTaskCardListForExcel)
		da.Fill(ds, mrptImage)

		myReport.SetDataSource(ds)

		Session("CrystalReport") = myReport

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print NC : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)

		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)


	End Sub

	Public Sub PrintJobsExcel() ''Added by Saylee on 31-Jul-20223 
		Dim da As New ObjectAdapter
		Dim mCompanyDetail As New CompanyDetail

		'Dim mnWO As nWO
		Dim mnWOJobs As nWOJobs
		mnWOJobs = mnWO.WOJobs
		Dim ds As New dsnWOJobsExcel
		da.Fill(ds, "WOJobsExcel", mnWOJobs)

		If ds.Tables("WOJobsExcel").Columns.Contains("TaskCardNo") Then
			ds.Tables("WOJobsExcel").Columns("TaskCardNo").ColumnName = "Task No."

		End If

		If ds.Tables("WOJobsExcel").Columns.Contains("WOJobDescription") Then
			ds.Tables("WOJobsExcel").Columns("WOJobDescription").ColumnName = "Task Description"
		End If

		If ds.Tables("WOJobsExcel").Columns.Contains("ATACode") Then
			ds.Tables("WOJobsExcel").Columns("ATACode").ColumnName = "ATA"
		End If
		If ds.Tables("WOJobsExcel").Columns.Contains("Publication") Then
			ds.Tables("WOJobsExcel").Columns("Publication").ColumnName = "Reference Doc."
		End If
		If ds.Tables("WOJobsExcel").Columns.Contains("TaskSourceRef") Then
			ds.Tables("WOJobsExcel").Columns("TaskSourceRef").ColumnName = "Source Doc."
		End If

		If ds.Tables("WOJobsExcel").Columns.Contains("WOJobEstimatedTime") Then
			ds.Tables("WOJobsExcel").Columns("WOJobEstimatedTime").ColumnName = "Estimated Man Hr."

		End If
		If ds.Tables("WOJobsExcel").Columns.Contains("DueAsOF") Then
			ds.Tables("WOJobsExcel").Columns("DueAsOF").ColumnName = "DueAs OF"

		End If
		If ds.Tables("WOJobsExcel").Columns.Contains("Zone") Then
			ds.Tables("WOJobsExcel").Columns("Zone").ColumnName = "Zone"

		End If
		If ds.Tables("WOJobsExcel").Columns.Contains("AREA") Then
			ds.Tables("WOJobsExcel").Columns("AREA").ColumnName = "Area"

		End If
		If ds.Tables("WOJobsExcel").Columns.Contains("Skill") Then
			ds.Tables("WOJobsExcel").Columns("Skill").ColumnName = "Skill"

		End If
		If ds.Tables("WOJobsExcel").Columns.Contains("Panels") Then
			ds.Tables("WOJobsExcel").Columns("Panels").ColumnName = "Access"
		End If
		If ds.Tables("WOJobsExcel").Columns.Contains("WorkPACKREF") Then
			ds.Tables("WOJobsExcel").Columns("WorkPACKREF").ColumnName = "Work Pack Ref"

		End If

		If ds.Tables("WOJobsExcel").Columns.Contains("WOJobRemark") Then
			ds.Tables("WOJobsExcel").Columns("WOJobRemark").ColumnName = "Remark"
		End If

		Dim columnToRemove1 As String()
		ReDim columnToRemove1(109)

		For i As Integer = 13 To 122

			columnToRemove1(i - 13) = ds.Tables("WOJobsExcel").Columns(i).ColumnName

		Next

		For i As Integer = 0 To columnToRemove1.Length - 1
			If ds.Tables("WOJobsExcel").Columns.Contains(columnToRemove1(i)) Then
				ds.Tables("WOJobsExcel").Columns.Remove(columnToRemove1(i))
			End If
		Next

		Dim dsNew As New DataSet
		dsNew.Clear()

		dsNew.Merge(ds.Tables("WOJobsExcel"))
		dsNew.Tables("WOJobsExcel").TableName = "Sheet1"
		Session("dsNew") = dsNew
		Session("DataTableToBeFormattedForExportToExcel") = "Jobs Report"
		Session("ExcelFileName") = "Jobs Report"
		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WOText.ToString + " - " + mnWO.WONo.ToString + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print for Export Jobs to Excel : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------

		ScriptManager.RegisterStartupScript(Me, [GetType], "openFilel", "openFile();", True)

	End Sub

	Public Sub PrintWithJobAttachment(Optional SignatureRequired As Boolean = False, Optional ByMail As Boolean = False)
		Dim mnWOJobs As nWOJobs

		Print(SignatureRequired:=SignatureRequired, ByMail:=ByMail, IsForPrintWithJobAttachment:=True)

		Dim myFile As String = ""
		myFile = Session("myFile")

		Dim bytes As Byte() = File.ReadAllBytes(myFile)
		Dim fileName As String = myFile
		Dim fi As FileInfo = New FileInfo(fileName)


		Dim pageCount As Integer = 0

		Dim pdfList As New Collections.ArrayList

		pdfList.Add(myFile)


		Dim PDFNo As Integer = 1
		Dim PDFNoChild As Integer = 1

		PDFNo = PDFNo + 1



		''Job attachements
		mnWOJobs = mnWO.WOJobs

		For k As Integer = 0 To mnWOJobs.Count - 1
			If mnWOJobs(k).IsAttachmentAdded Then
				Dim mJobAttachment As FileAttach
				For j As Integer = 0 To mnWOJobs(k).FileAttachments.Count - 1
					mJobAttachment = mnWOJobs(k).FileAttachments(j)
					If mJobAttachment.Size > 0 And LCase(mJobAttachment.Extension) = ".pdf" Then
						Dim ChildAttachment_path As String = "C:\Temp\" & mnWO.WONumber.Replace("/", "-") & mnWOJobs(k).TaskCardNo.Replace("/", "-").Replace("\", "-") & PDFNoChild.ToString & mJobAttachment.Extension
						Dim fs As FileStream
						If File.Exists("C:\Temp\") = False Then
							File.Delete(ChildAttachment_path)
							fs = File.Create(ChildAttachment_path)
							fs.Write(mJobAttachment.ImageFile, 0, mJobAttachment.ImageFile.Length)
							fs.Close()

							pdfList.Add(ChildAttachment_path)                               '2. mJobAttachment attachment
							PDFNo = PDFNo + 1
							PDFNoChild = PDFNoChild + 1
						End If
					End If
					mJobAttachment = Nothing
				Next
			End If

		Next

		Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
		Dim MergedPath_WM As String = "C:\Temp\" & "temp_myMergedPdf_WM.pdf"

		Dim filesByte As New List(Of Byte())()
		For Each file__1 As String In pdfList 'files
			filesByte.Add(File.ReadAllBytes(file__1))
		Next

		File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))

		'AddWatermarkText(MergedPath, MergedPath_WM, mnWO.WONumber, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount)
		AddWatermarkText(MergedPath, MergedPath_WM, mnWO.WOText.ToString & "-" & mnWO.WONo.ToString, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount) 'Added on 24-Jun-2019
		''//********************************************Set Sessions*********************************************************//
		Session("CrystalReport") = MergedPath_WM
		Session("PrintReportWithAttachment") = "True"

		Dim DeleteThis As String = mnWO.WONumber.Replace("/", "-")
		Dim Files As String() = Directory.GetFiles("C:\Temp\")

		For Each file__1 As String In Files
			If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
				File.Delete(file__1)
			End If
		Next
		'End

		'Added on 15-Mar-2019
		mWODetail = "WO NO : " + mnWO.WONumber.Replace("/", "-") + " Dated : " + mnWO.WODateFormatted.ToString + IIf(mnWO.RegNo.ToString <> "", " Aircraft : " + mnWO.RegNo.ToString, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
		MarkLog(Action.Print, "Work Order", "Work Order Print With Job(s) Attachments : " + mWODetail, ErrorType.NoError, Guid.Empty, EventLogID)
		'-------


		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)
	End Sub

#Region " Report Events "

	Private Sub btnPrintBlankEO_Click(sender As Object, e As EventArgs) Handles btnPrintBlankEO.Click
		PrintBlankEO()
	End Sub

	Private Sub btnPrintNC_Click(sender As Object, e As EventArgs) Handles btnPrintNC.Click
		PrintNC()
	End Sub

	Private Sub btnPrintWithPDF_Click(sender As Object, e As EventArgs) Handles btnPrintWithPDF.Click, btnPrintWithPDFBA.Click
		''' PrintWithPDF()
		If AppSettings("ClientCode") = "Novo" Then
			If mnWO.WOJobs.Count = 1 Then     ' Added by Saylee on 10-Dec-2019 ,here if one job and one task then format changes
				If mnWO.WOJobs(0).WOJobTasks.Count = 1 Then
					PrintWithPDFNOVOSingleTask()
				ElseIf mnWO.WOJobs(0).WOJobTasks.Count > 1 Or mnWO.WOJobs(0).WOJobTasks.Count = 0 Then
					PrintWithPDF()
				End If
			Else
				If mnWO.WOJobs.IsOneTaskExists = True Then 'Added by Saylee on 10-Dec-2019 ,multiple jobs and check each has one Task then also format changes
					PrintWithPDFNOVOSingleTask()
				Else
					PrintWithPDF()
				End If

			End If
		Else
			PrintWithPDF()
		End If
	End Sub

	Private Sub btnLogBookEntry_Click(sender As Object, e As EventArgs) Handles btnLogBookEntry.Click
		PrintLogBookEntry()
	End Sub

	Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click, btnPrintBA.Click
		'Print()
		If (AppSettings("ClientCode") = "APFT" Or
			AppSettings("ClientCode") = "GEP" Or
			AppSettings("ClientCode") = "ASH" Or
			AppSettings("ClientCode") = "MEL" Or
			AppSettings("ClientCode") = "SHR" Or
			AppSettings("ClientCode") = "AAP") And
		   mnWO.IsDigitalSignatureAdded = True And
		   mnWO.StatusID <> 1 Then 'Added By Saylee on 8-Nov-2019 for APFT08112019
			MSGBoxCtrl.Show("Digital Signature Confirmation!", "Do you want to print with Digital Signature?", "", MsgBoxStyle.YesNo, "SignatureRequired")
			Exit Sub
		Else
			Print()
		End If
	End Sub
	Private Sub btnPrintSWC_Click(sender As Object, e As EventArgs) Handles btnPrintSWC.Click
		Print(IsFromSpecialWOButton:=True)
	End Sub


	'Added by Saylee on 20-Nov-2020
	Private Sub btnPrintCallOut_Click(sender As Object, e As EventArgs) Handles btnPrintCallOut.Click
		Print(HeligoCallOutPrint:=True)
	End Sub

	Private Sub btnPrintAdditionalWO_Click(sender As Object, e As EventArgs) Handles btnPrintAdditionalWO.Click
		PrintAdditionalWO()
	End Sub

	Private Sub btnPrintWOPackage_Click(sender As Object, e As EventArgs) Handles btnPrintWOPackage.Click
		PrintWOPackage()
	End Sub

	Private Sub btnPrintAdditionalWOAndSheet_Click(sender As Object, e As EventArgs) Handles btnPrintAdditionalWOAndSheet.Click
		PrintAdditionalWOAndSheet()
	End Sub

	Private Sub btnCRS_Click(sender As Object, e As EventArgs) Handles btnCRS.Click
		If Not IsInRole(Rights.Print) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "KAS") Then
			PrintCRS()
		Else
			Response.Redirect("wfAircraftCRS_Ajax.aspx?BackPage1=wfnWODetail_AJAX.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
		End If

	End Sub

	Private Sub btnPrintNRC_Click(sender As Object, e As EventArgs) Handles btnPrintNRC.Click
		PrintNRC()
	End Sub

	Private Sub btnPrintCAMO_Click(sender As Object, e As EventArgs) Handles btnPrintCAMO.Click
		PrintCAMO()
	End Sub


	Private Sub btnPrintToolsSpares_Click(sender As Object, e As EventArgs) Handles btnPrintToolsSpares.Click
		PrintToolsSpares()
	End Sub


	Private Sub BtnPrintProductionPlanningForm_Click(sender As Object, e As EventArgs) Handles BtnPrintProductionPlanningForm.Click
		PrintProductionPlanningForm()
	End Sub

	''Added by Saylee on 26-Feb-2019 for ALL27022019
	Private Sub btnTaskCardExcel_Click(sender As Object, e As EventArgs) Handles btnTaskCardExcel.Click
		PrintTaskCardExcel()
	End Sub
	'**********************************************

	''Added by Saylee on 15-Nov-2021 for STR15092021

	Private Sub btnPrintTallySheet_Click(sender As Object, e As EventArgs) Handles btnPrintTallySheet.Click
		PrintTallySheet()
	End Sub
	'**********************************************
	''Added by Saylee on 31-Jul-20223 

	Private Sub btnJobsExcel_Click(sender As Object, e As EventArgs) Handles btnJobsExcel.Click
		PrintJobsExcel()
	End Sub

	Private Sub btnPrintWithJobAttachment_Click(sender As Object, e As EventArgs) Handles btnPrintWithJobAttachment.Click
		'Print()
		If (AppSettings("ClientCode") = "APFT" Or
			AppSettings("ClientCode") = "GEP" Or
			AppSettings("ClientCode") = "ASH" Or
			AppSettings("ClientCode") = "MEL" Or
			AppSettings("ClientCode") = "SHR" Or
			AppSettings("ClientCode") = "AAP") And
		   mnWO.IsDigitalSignatureAdded = True And
		   mnWO.StatusID <> 1 Then 'Added By Saylee on 8-Nov-2019 for APFT08112019
			MSGBoxCtrl.Show("Digital Signature Confirmation!", "Do you want to print with Digital Signature?", "", MsgBoxStyle.YesNo, "SignatureRequiredForPrintWithJobAttachment")
			Exit Sub
		Else
			PrintWithJobAttachment()
		End If
	End Sub

#End Region

#End Region

#Region " Service Methods "

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetTextList(prefixText As String, count As Integer, contextKey As String) As String()
		Dim DistinctTextList As DistinctTextListAutoComplete
		DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, 16)
		If count = 0 Then
			Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
		Else
			Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
		End If
	End Function

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetCompletionList(prefixText As String, count As Integer, contextKey As String) As List(Of String)
		Dim mModelList As ModelListAutoComplete
		Dim str As String = contextKey 'Holds the parameters to filter criteria..
		Dim AssemblyTypID As Integer = CInt(str)
		mModelList = ModelListAutoComplete.GetModelList(prefixText)

		If count = 0 Then
			Return (From c As ModelListAutoCompleteInfo In mModelList
					Select c.Name).ToList
		Else
			Return (From c As ModelListAutoCompleteInfo In mModelList
					Select c.Name).Take(count).ToList
		End If
	End Function

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetRegTextList(prefixText As String, count As Integer, contextKey As String) As String()
		Dim DistinctTextList As DistinctTextListAutoComplete
		DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, 26)
		If count = 0 Then
			Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
		Else
			Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
		End If
	End Function

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetModelNameList(prefixText As String, count As Integer, contextKey As String) As String()
		Dim DistinctTextList As DistinctTextListAutoComplete
		DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, 27)
		If count = 0 Then
			Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
		Else
			Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
		End If
	End Function
	'MLNo

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetLicenseNoList(prefixText As String, count As Integer, contextKey As String) As String()
		Dim mLicenses As LicenseNoListWithEmployee
		mLicenses = LicenseNoListWithEmployee.GetLicenseNoList(prefixText, UserNameForLicenceList, , , False)

		If count = 0 Then
			Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).ToArray
		Else
			Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).Take(count).ToArray
		End If
	End Function

	Private Sub cmbServiceProvider_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbServiceProvider.SelectedIndexChanged
		If cmbServiceProvider.SelectedIndex > 0 Then
			txtIssueTo.Text = cmbServiceProvider.SelectedItem.ToString
		Else
			txtIssueTo.Text = ""
		End If
	End Sub

	'Added by Saylee on 22-Jun-2023 , for Third Party job transferring
	Private Sub lnkImportJobs_Click(sender As Object, e As EventArgs) Handles lnkImportJobs.Click
		SetObject()
		mdlPopUpImportJobs.Show()

		pnlImportJobs.Visible = True
		pnlJobs.Visible = True
		upnlImportJobs.Update()
	End Sub

	Private Sub btnImportClose_Click(sender As Object, e As EventArgs) Handles btnImportClose.Click
		mdlPopUpImportJobs.Hide()
		upnlImportJobs.Update()
		pnlImportJobs.Visible = False
	End Sub

	Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
		SetObject()
		If Not IsValid Then upnlValidationsummary.Update() : Exit Sub

		If (BrowseYourFile.HasFile) Then
			Dim FilePath As String
			Dim FileName As String = Guid.NewGuid.ToString


			Extension = Path.GetExtension(BrowseYourFile.FileName)

			If Not Extension.Contains("xls") Then
				MSGBoxCtrl.Show("Alert..!!", "File Selection Error..!!!", "Please select only Excel file", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If

			FilePath = Server.MapPath(BrowseYourFile.FileName)
			Try
				BrowseYourFile.PostedFile.SaveAs(FilePath)
				If Extension = ".xls" Then
					MyConnection = New OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; " &
							   "data source=" & FilePath & "; " & "Extended Properties=Excel 8.0;")
				Else
					MyConnection = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; data source=" & FilePath & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1""")
				End If

				MyCommand = New OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection)
				DS = New DataSet

				Try
					MyCommand.Fill(DS)
				Catch ex As Exception
					Throw ex
				Finally

				End Try

				Session("DS") = DS
				MyConnection.Close()

				If DS.Tables.Count = 0 Then
					MSGBoxCtrl.Show("Alert..!!", "File Slection", "Please select Excel.", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If

				Dim IsTranfered As Boolean = ThirdPartyJobTransfer()

				dgWOJobs.DataSource = mnWO.WOJobs
				dgWOJobs.DataBind()

				upnlGrids.Update()
				If IsTranfered = True Then
					MSGBoxCtrl.Show("Import Alert!", "Data Imported Successfully!", "", MsgBoxStyle.OkOnly, "")
				Else
					MSGBoxCtrl.Show("Import Alert!", "Some Records were not Imported. Refer the Log File!", "", MsgBoxStyle.OkOnly, "")
				End If
				File.Delete(FilePath)

			Catch ex As Exception

			End Try
		End If
		'''End of Added by Saylee on 22-Jun-2023 , for Third Party job transferring
	End Sub

	Private Sub chkFMC_CheckedChanged(sender As Object, e As EventArgs) Handles chkFMC.CheckedChanged
		If chkFMC.Checked = True Then
			SetObject()
			ScriptManager.RegisterStartupScript(Me, [GetType], "OpenCustomerContractSelectionWindow", "OpenCustomerContractSelectionWindow();", True)
		ElseIf chkFMC.Checked = False Then
			mnWO.CustomerContractID = Guid.Empty
			mnWO.CustomerContractNo = ""
			Session("mnWO") = mnWO
			lblCustomerContractNo.DataBind()
			upnlMachineDet.Update()
		End If
	End Sub

	Private Sub hdnBtnCustomerContractSelection_Click(sender As Object, e As EventArgs) Handles hdnBtnCustomerContractSelection.Click
		If mnWO.CustomerContractID.Equals(Guid.Empty) And chkFMC.Checked = True Then
			chkFMC.Checked = False
		End If
		lblCustomerContractNo.DataBind()
		upnlMachineDet.Update()
	End Sub

#End Region

#Region " Digital Signature "

	Private Sub btnlRequestForDigitalSignature_Click(sender As Object, e As EventArgs) Handles btnlRequestForDigitalSignature.Click
		Try
			Dim mDS_Queue As DS_Queue = DS_Queue.NewDS_Queue()
			Dim mDS_ModuleList As DS_ModuleList = DS_ModuleList.GetDS_ModuleList()

			With mDS_Queue

				.ModuleID = 2
				.ModuleName = mDS_ModuleList.Item(.ModuleID, "").Name
				.TransactionID = mnWO.ID

				Print(IsForDS:=True)

				Dim myFile As String = ""
				myFile = Session("myFile")

				Dim bytes As Byte() = File.ReadAllBytes(myFile)
				Dim fileName As String = myFile
				Dim fi As FileInfo = New FileInfo(fileName)

				.ImageSize = fi.Length
				.Extension = fi.Extension
				.FileName = fi.Name

				Dim b1(fi.Length - 1) As Byte
				Dim txt As Byte() = New UTF8Encoding(True).GetBytes(myFile)
				.Imagefile = bytes 'txt
				Dim mUser As SI.UTILITY.User = SI.UTILITY.User.GetUser(User.Identity.Name)
				.RequestedUserID = mUser.UserID
			End With

			Session("mDS_Queue") = mDS_Queue
			Session("myFile") = Nothing

			ScriptManager.RegisterStartupScript(Me, [GetType], "OpenDigitalSignatureRequestWindow", "OpenDigitalSignatureRequestWindow();", True)

		Catch ex As Exception
		End Try

	End Sub

	Private Sub btnViewDSFile_Click(sender As Object, e As EventArgs) Handles btnViewDSFile.Click


		Dim DS_Queue As DS_Queue = DS_Queue.GetDS_QueueAfterSigned(mnWO.ID, True)


		If DS_Queue.DS_ImageSize > 0 Then

			Dim NO As New Random
			Dim mFile As String = "PurchaseOrderDS" & NO.Next.ToString
			Dim fileName As String = mFile & DS_Queue.DS_Extension
			Dim path As String = Server.MapPath("~/Temp") & "\" & fileName
			Dim fs As FileStream

			If File.Exists(path) = False Then
				File.Delete(path)
				fs = File.Create(path)
				fs.Write(DS_Queue.DS_ImageFile, 0, DS_Queue.DS_ImageFile.Length)
				fs.Close()
				Dim str As String
				str = "openFile()"
				ScriptManager.RegisterStartupScript(Me, [GetType], "OpenScript", str, True)
				Session("DocPath") = path

			End If
		Else
			ScriptManager.RegisterStartupScript(Me, [GetType], "openTransDetail", MessageBox.Show("Digital Signature Is Pending", False), True)
		End If

	End Sub

#End Region

End Class