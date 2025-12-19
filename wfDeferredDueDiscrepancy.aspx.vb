Public Class DeferredDueDiscrepancyReport
	Inherits Page

#Region " Variable Declaration "

	Public MachineNameValueList As MachineNameValueList
	Public ATAList As ATAList
	Public DeferredDueDiscrepancy As DeferredDueDiscrepancy
	Public ModuleList As ModuleList

	Dim StartDate As String
	Dim EndDate As String
	Dim MachineID, ATAID As String
	Dim Aircraft, ATAChapter As String
	Dim MELDueReportSearchingCriteria As String = String.Empty
	Dim TransTypeID As Integer

#End Region

#Region " Business Methods "

	Private Sub GetSession()

		DeferredDueDiscrepancy = CType(Session("DeferredDueDiscrepancy"), DeferredDueDiscrepancy)
		MachineNameValueList = CType(Session("MachineNameValueList"), MachineNameValueList)
		ATAList = CType(Session("ATAList"), ATAList)
		ModuleList = Session("mModuleList")
		TransTypeID = Session("TransTypeID")

	End Sub

	Private Sub SetSession()

		Session("DeferredDueDiscrepancy") = DeferredDueDiscrepancy
		Session("MachineNameValueList") = MachineNameValueList
		Session("ATAList") = ATAList
		Session("TransTypeID") = TransTypeID

	End Sub

	Private Sub RemoveSession()

		Session.Remove("DeferredDueDiscrepancy")
		Session.Remove("MachineNameValueList")

	End Sub

	Private Overloads Sub SetFocus(control As WebControl)

		Try

			If control.Enabled = False Or control.Visible = False Then Exit Sub

			Dim str As String
			str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"
			ClientScript.RegisterStartupScript([GetType],
											   "Focus Script",
											   str)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Helper Methods "

	Private Sub Display()

		Try

			lblDateRangeSearchCriteria.Visible = True
			lblAircraftSearchCriteria.Visible = True
			lblATAChapterSearchCriteria.Visible = True

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetValues()

		Try

			If Not IsDate(txtAsOnDate.Text) Then
				StartDate = ""
			Else
				StartDate = txtAsOnDate.Text
			End If

			Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
			ATAChapter = IIf(cmbATAChapter.SelectedIndex > 0, cmbATAChapter.SelectedItem.Text, "")
			lblDateRangeSearchCriteria.Text = "As On Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
			lblAircraftSearchCriteria.Text = "Aircraft : " & Aircraft
			lblATAChapterSearchCriteria.Text = "ATA Chapter : " & ATAChapter

			MELDueReportSearchingCriteria = $"{lblDateRangeSearchCriteria.Text.Trim}, {lblAircraftSearchCriteria.Text.Trim}, {lblATAChapterSearchCriteria.Text.Trim}"

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetReport(Optional ByMail As Boolean = False) 'Parameter Added by Shital on 6-Sep-2016

		Try

			SetValues()

			Dim dataAdapter As New ObjectAdapter
			Dim dataSet As New dsDeferredDueDiscrepancy
			Dim CrystalReport As Engine.ReportClass
			Dim CompanyDetail As New CompanyDetail

			Dim IsMajorMinor As Integer
			Dim MajorMinor As String
			Dim IsPirepsDefectType As Integer 'Added By Sachin On 04-Mar-2024
			Dim PirepsDefectType As String

			If rbAll.Checked = True Then
				IsMajorMinor = 0  'ALL MAJOR AND MINOR
				MajorMinor = 0
			ElseIf rbMajor.Checked = True Then
				IsMajorMinor = 1  'MAJOR
				MajorMinor = 1    'To Show on report MAJOR/MINOR/ALL
			ElseIf rbIncident.Checked = True Then
				IsMajorMinor = 3  'MAJOR
				MajorMinor = 3    'To Show on report MAJOR/MINOR/ALL
			Else
				IsMajorMinor = 2  'MINOR
				MajorMinor = 2
			End If

			'Added By Shweta On 30-April-2013 For ALL29042013-3
			If rbAllDefectType.Checked = True Then
				IsPirepsDefectType = 0  'ALL Pireps And Defect Type
				PirepsDefectType = 0
			ElseIf rbIsPireps.Checked = True Then
				IsPirepsDefectType = 1  'Pireps
				PirepsDefectType = 1    'To Show on report Pireps/Defect Type/ALL
			Else
				IsPirepsDefectType = 2  'DEFECT type
				PirepsDefectType = 2
			End If

			CrystalReport = New crptDeferredDueDiscrepancy

			If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then

				DeferredDueDiscrepancy = DeferredDueDiscrepancy.GetDeferredDueDiscrepancy(AsonDate:=txtAsOnDate.Text,
																						  MachineID:=New Guid(cmbAircraft.SelectedValue.ToString),
																						  ATAID:=New Guid(cmbATAChapter.SelectedValue.ToString),
																						  MELCategoryID:=0,
																						  IsMajor:=IsMajorMinor,
																						  TimeFormat:="HH:mm",
																						  IsPireps:=IsPirepsDefectType,
																						  SkipIsForInventoryAircarft:=True)
			Else

				DeferredDueDiscrepancy = DeferredDueDiscrepancy.GetDeferredDueDiscrepancy(AsonDate:=txtAsOnDate.Text,
																						  MachineID:=New Guid(cmbAircraft.SelectedValue.ToString),
																						  ATAID:=New Guid(cmbATAChapter.SelectedValue.ToString),
																						  MELCategoryID:=0,
																						  IsMajor:=IsMajorMinor, ,
																						  IsPireps:=IsPirepsDefectType,
																						  SkipIsForInventoryAircarft:=True)
			End If

			Dim Report As New ReportData(CompanyDetail.CompanyName,
										 CompanyDetail.Address,
										 CompanyDetail.Tel1,
										 CompanyDetail.Tel2,
										 CompanyDetail.Fax,
										 CompanyDetail.Email,
										 CompanyDetail.WebSite,
										 ReportName:="Deferred Due Report",
										 SearchStr1:=New SmartDate(StartDate).FormattedText,
										 SearchStr2:=Aircraft,
										 SearchStr3:=ATAChapter,
										 SearchStr4:="",
										 SearchStr5:=MajorMinor,
										 ProductVersion:=AppSettings("Product Version"),
										 SINote:=AppSettings("SINote"),
										 SearchStr6:="",
										 SearchStr7:="",
										 SearchStr8:=AppSettings("MELSnagNomenclature").ToString,
										 SearchStr9:=PirepsDefectType,
										 SearchStr10:=AppSettings("Logo"))

			'If case Added By Shital On 6-Sep-2016

			If DeferredDueDiscrepancy.Count = 0 Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.NoRecordFound,
								MSGBox.Message_Text.NoRecordFound,
								"There are no records for this Criteria.",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			End If

			Dim CompanyLogo As rptImage = rptImage.GetImage(dataSet)

			dataSet.Clear()
			dataSet.Tables.Clear()

			dataAdapter.Fill(dataSet, "DeferredDueDiscrepancy", DeferredDueDiscrepancy)
			dataAdapter.Fill(dataSet, "rptImage", CompanyLogo)
			dataAdapter.Fill(dataSet, "ReportData", Report)

			CrystalReport.SetDataSource(dataSet)
			Session("CrystalReport") = CrystalReport

			If ByMail Then

				SendMailFile.SendMailFile(rpt:=Session("CrystalReport"),
										  UserName:=Thread.CurrentPrincipal.Identity.Name,
										  Subject:="Deferred Due Report",
										  Text:="Deferred Due Report",
										  Info:=" For " + lblDateRangeSearchCriteria.Text + ", " + lblAircraftSearchCriteria.Text, ,
										  ToMailID:=Session("ToSendMailIDs"),
										  CCMailID:=Session("CcSendMailIDs"),
										  ReportPath:="",
										  ReportByMail:=True,
										  Remark:=Session("SendMailRemark"),
										  ReportGeneratedBy:=Session("ReportGenratedBy"),
										  SmtpHost:=ModuleList.Item("MELDueReport").SmtpHost,
										  SmtpPort:=ModuleList.Item("MELDueReport").SmtpPort,
										  SmtpUser:=ModuleList.Item("MELDueReport").SmtpUser,
										  SmtpPassword:=ModuleList.Item("MELDueReport").SmtpPassword)
			Else

				Dim Str As String
				Str = "openTranDetail();"
				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"openTranDetail",
													Str,
													True)
			End If

			MarkLog(If(ByMail, Action.SendMail, Action.Print),
					"MELDueReport",
					MELDueReportSearchingCriteria,
					ErrorType.NoError,
					Guid.Empty,
					EventLogID)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub MessageBoxResult()

		Dim MsgBoxResult As MsgBoxResult
		MsgBoxResult = MSGBoxCtrl.Result
		Try

			If MsgBoxResult > 0 Then

				Select Case MsgBoxResult
					Case MsgBoxResult.Ok
						DataFieldBind()
				End Select

			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			MachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:="", , , , , , ,
																	   IsTagRequired:=True,
																	   TagText:="(ALL)", ,
																	   SkipIsForInventoryAircarft:=True)
			cmbAircraft.DataSource = MachineNameValueList
			Session("MachineNameValueList") = MachineNameValueList

			ATAList = ATAList.GetATAList(ATANomenclature:="", AddTopItem:="(ALL)")
			Session("ATAList") = ATAList
			cmbATAChapter.DataSource = ATAList

			DataBind()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Events "

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		GetSession()
		Try

			If Not IsPostBack Then

				txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat").ToString)
				rbAllDefectType.Checked = True
				rbAll.Checked = True

				DataFieldBind()

				If cmbAircraft.Enabled = True Then
					SetFocus(cmbAircraft)
				End If

				TransTypeID = IIf(Request.QueryString("TransTypeID") IsNot Nothing,
								  CInt(Request.QueryString("TransTypeID")),
								  115)

				Session("TransTypeID") = TransTypeID

			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub DisplayCurrentSearchCriteria(sender As Object, e As EventArgs) Handles btnCurrentSearchCriteria.Click

		Try

			Display()
			SetValues()
			upnlselection.Update()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnDisplay.Click

		Try

			If IsValid Then
				SetReport(False)  '6-Sep-2016
			Else
				upnlValidationsummary.Update()
			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

		Try

			Session("MiddleFrame") = ""
			RemoveSession()
			Response.Redirect("Dashboard.aspx")

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

		Try

			MSGBoxCtrl.HideControl()
			MessageBoxResult()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub SendReportByMail(sender As Object, e As EventArgs) Handles btnByMail.Click

		Try

			Session("UserEmailID") = ModuleList.Item("MELDueReport").SendToMailID
			Session("UserCcEmailID") = ModuleList.Item("MELDueReport").SendCCMailID

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Open By Mail Window",
												"OpenByMaiWindow();",
												True)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub HdnImgMELBtnSendMail_Click(sender As Object, e As EventArgs) Handles hdnimgMELBtnSendMail.Click

		Dim email As Thread
		Try

			email = New Thread(Sub() SetReport(True)) With {
				.IsBackground = True
			}
			email.Start()

		Catch ex As Exception

			Dim Day, Month, Year As String
			Day = Format(Today.Date.Day, "0#")
			Month = Format(Today.Date.Month, "0#")
			Year = Format(Today.Date.Year, "0#")
			Dim todaydate As String = Day & Month & Year
			Dim Path As String = AppSettings("DOCPath") & todaydate

			FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
			WriteLine(1, Date.Now.ToString + " Mail service (hdnimgMELBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
			FileClose(1)

		End Try

	End Sub

#End Region

End Class