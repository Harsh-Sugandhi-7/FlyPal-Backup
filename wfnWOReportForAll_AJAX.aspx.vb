'CREATED By : Saylee
'Dated      : 8-July-2013


Public Class wfnWOReportForAll_AJAX
	Inherits System.Web.UI.Page


#Region " Variable Declaration "
	Dim mnWO As nWO
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mnWO = Session("mnWO")
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		If Not IsPostBack Then

		End If
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lbltitle.Text = "E. O. Report For All"
			rbWOSummary.Text = "E. O. Summary"
			rbWOJobSummary.Text = "E. O. Job Summary"
		Else
			lbltitle.Text = "W. O. Report For All"
			rbWOSummary.Text = "W. O. Summary"
			rbWOJobSummary.Text = "W. O. Job Summary"
		End If
	End Sub
	Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
		GetSession()
		Dim da As New CSLA.Data.ObjectAdapter
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

		Dim myReport = New crnWORegisterWithJobsAndTasksDetailLandScapeForAll

		Dim SearchStr1, SearchStr2, SearchStr3 As String
		Dim SearchStr4, SearchStr5, SearchStr6 As String

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "DOL" Then
			myReport = New crnWORegisterWithJobsAndTasksDetailLandScapeForAllDolphin
		Else
			myReport = New crnWORegisterWithJobsAndTasksDetailLandScapeForAll
		End If

		mnWO = Session("mnWO")
		mnWOJobs = mnWO.WOJobs
		mnWOTools = mnWO.WOTools
		mnWOJobComps = nWOJobComps.GetWOJobComps(mnWO.ID, "")
		mnWOJobSpares = nWOJobSpares.GetWOSpares(mnWO.ID, "")
		mnWOJobTasks = nWOJobTasks.GetWOTasks(mnWO.ID, "")
		mnWOJobDesignationAllocations = nWOJobDesignationAllocations.GetWOJobDesignationAllocations(mnWO.ID, "")
		objTaskSteps = TaskSteps.GetTaskCardSteps(mnWO.ID)

		mnWORegisterList = nWORegisterList.GetnWORegisterList(mnWO.WOText, mnWO.WONo, , , mnWO.RegNo, , mnWO.SerialNo)

		myReport.SetDataSource(ds)

		With myReport
			If (rbWOSummary.Checked = True) Then
				.Section7.SectionFormat.EnableSuppress = True
				.Section26.SectionFormat.EnableSuppress = True
				.Section18.SectionFormat.EnableSuppress = True
				.Section9.SectionFormat.EnableSuppress = True
				.Section20.SectionFormat.EnableSuppress = True
				.Section19.SectionFormat.EnableSuppress = True
				.Section17.SectionFormat.EnableSuppress = True
				.Section15.SectionFormat.EnableSuppress = True
				SearchStr4 = "True"
				SearchStr5 = "False"
				SearchStr6 = "False"

			ElseIf (rbWOJobSummary.Checked = True) Then
				.Section7.SectionFormat.EnableSuppress = False
				.Section26.SectionFormat.EnableSuppress = False
				.Section18.SectionFormat.EnableSuppress = False
				.Section9.SectionFormat.EnableSuppress = False
				.Section20.SectionFormat.EnableSuppress = False
				.Section19.SectionFormat.EnableSuppress = False
				.Section17.SectionFormat.EnableSuppress = False
				.Section15.SectionFormat.EnableSuppress = False

				SearchStr4 = "False"
				SearchStr5 = "True"
				SearchStr6 = "False"
			ElseIf (rbTaskCard.Checked = True) Then
				.Section7.SectionFormat.EnableSuppress = True
				.Section26.SectionFormat.EnableSuppress = True
				.Section18.SectionFormat.EnableSuppress = True
				.Section9.SectionFormat.EnableSuppress = True
				.Section20.SectionFormat.EnableSuppress = True
				.Section19.SectionFormat.EnableSuppress = True
				.Section17.SectionFormat.EnableSuppress = True
				.Section15.SectionFormat.EnableSuppress = False

				SearchStr4 = "False"
				SearchStr5 = "False"
				SearchStr6 = "True"
			ElseIf (rbAll.Checked = True) Then
				'        'Do nothing

				SearchStr4 = "False"
				SearchStr5 = "False"
				SearchStr6 = "False"
			End If
		End With
		If chkMPDNo.Checked = True And chkAMMNo.Checked = False Then
			SearchStr1 = "True"
			SearchStr2 = "False"
		ElseIf chkAMMNo.Checked = True And chkMPDNo.Checked = False Then
			SearchStr1 = "False"
			SearchStr2 = "True"
		ElseIf chkAMMNo.Checked = True And chkMPDNo.Checked = True Then
			SearchStr1 = "True"
			SearchStr2 = "True"
		End If
		SearchStr3 = txtNo.Text

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
					  mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
					  mCompanyDetail.WebSite, "Part No. Status", SearchStr1, SearchStr2, SearchStr3, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6, AppSettings("ClientCode"), AppSettings("CRS"))

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

		myReport.SetDataSource(ds)

		Session("CrystalReport") = myReport
		'Dim str As String
		'str = "<script language=Javascript>openTranDetail();</script>"
		'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", str)
		Dim Str As String
		Str = "openTranDetail();"
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "openTranDetail", Str, True)
	End Sub
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click

		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If

		Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
	End Sub
#End Region



End Class