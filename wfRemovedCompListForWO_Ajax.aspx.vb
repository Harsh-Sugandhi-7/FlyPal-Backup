'AJAX Conversion By Vikrant On 30-Mar-2015
Imports System.Linq
Public Class wfRemovedCompListForWO_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	'New Added
	Public mCompStatusList As CompStatusList
	'End
	Public mMachineNameValueList As MachineNameValueList
	Public mCompStatus As CompStatus
	Public RemoveDate As String
	Public InstallOnId As String
	Public AircraftId As String
	Public AssemblyId As String
	'  Public mInstallCompStatus As CompStatus   'Code Added 25,Jan,2007
	' Public mCurrentDate As String             'Added Code  25,Jan,2007
	'Public mCompInstallInfo As String         'Added Code  25,Jan,2007
	Public PartNo As String 'added by Rahul 29-apr-09

	'28-Apr-2009
	Public mInstallInAssemblylist As AssemblyList
	Public mInstallOnAssemblyID As String

	Public mMachineMaintenance As MachineMaintenance      'Added by Saylee on 8th-Oct-2009

	Dim EventLogID As Guid 'Added By Utkarsh On 26-Jul-2011 For All19072011
	Dim MaintDetail As String 'Added By Utkarsh On 26-Jul-2011 For All19072011

	'Added By Saylee On 27-Nov-2014 
	Dim mFileAttach As FileAttach
	'End
	Dim RecordsToShowForRemCompList As Integer
	''Dim RecordsToShowForInstCompList As Integer

	Dim IsReadOnly As Boolean 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
	Dim IsReadOnlyInstalledOn As Boolean 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
	'Public mSpareAssemblyComponent As Integer 'Added by Shital on 23-Dec-2020 
	Dim mInstCompStatus As CompStatus
	Dim SerialNo As String
	Dim Part As String
	Dim mOpenForWOJobCompRemInstNewPage As String = ""
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		'New Added
		mCompStatusList = CType(Session("mCompStatusList"), CompStatusList)
		'End
		'mtmpInstalledCompList = CType(Session("mtmpInstalledCompList"), tmpInstalledCompList)
		'mRemovedCompList = CType(Session("mRemovedCompList"), tmpRemovedCompList)
		mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
		mCompStatus = CType(Session("mCompStatus"), CompStatus)
		RemoveDate = CType(Session("RemoveDate"), String)
		AircraftId = CType(Session("AircraftId"), String)
		AssemblyId = CType(Session("AssemblyId"), String)
		InstallOnId = CType(Session("InstallOnId"), String)
		'Added by Rahul on 29-Apr-2009
		PartNo = CType(IIf(Session("PartNo") Is Nothing, "", Session("PartNo")), String)
		SerialNo = CType(IIf(Session("SerialNo") Is Nothing, "", Session("SerialNo")), String)

		'28-Apr-2009
		mInstallInAssemblylist = CType(Session("mInstallInAssemblylist"), AssemblyList)
		mInstallOnAssemblyID = CType(Session("mInstallOnAssemblyID"), String)

		mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 8th-Oct-2009
		'Added By Saylee On 27-Nov-2014 
		mFileAttach = Session("mFileAttach")
		'End    
		RecordsToShowForRemCompList = CType(Session("RecordsToShowForRemCompList"), Integer)
		''RecordsToShowForInstCompList = CType(Session("RecordsToShowForInstCompList"), Integer)
		IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
		IsReadOnlyInstalledOn = Session("IsReadOnlyInstalledOn") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
		mInstCompStatus = CType(Session("mInstCompStatus"), CompStatus)
		mOpenForWOJobCompRemInstNewPage = IIf(Session("OpenForWOJobCompRemInstNewPage") Is Nothing, "", Session("OpenForWOJobCompRemInstNewPage"))

	End Sub
	Private Sub RemoveSession()
		'mtmpInstalledCompList = Nothing
		'mRemovedCompList = Nothing
		mMachineNameValueList = Nothing
		mCompStatus = Nothing
		Session.Remove("mtmpInstalledCompList")
		Session.Remove("mRemovedCompList")
		Session.Remove("mMachineNameValueList")
		Session.Remove("mCompStatus")
		Session.Remove("InstallOnId")
		'28-Apr-2009
		Session.Remove("mInstallInAssemblylist")
		Session.Remove("mInstallOnAssemblyID")
		Session.Remove("mMachineMaintenance") 'Added by Saylee on 8th-Oct-2009
		'Added By Saylee On 27-Nov-2014 
		Session.Remove("mFileAttach")
		'End
		Session.Remove("RecordsToShowForRemCompList")
		''Session.Remove("RecordsToShowForInstCompList")
		'New Added
		Session.Remove("mCompStatusList")
		Session.Remove("mInstList")
		'End
		Session.Remove("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
		Session.Remove("IsReadOnlyInstalledOn")
		Session.Remove("mOpenForWOJobCompRemInstNewPage")
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Dim ErrorsCount As Integer = 0
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes

				Case MsgBoxResult.No
					Session("sender") = ""
				Case MsgBoxResult.Cancel
					Session("sender") = ""
				Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
					Session("sender") = ""
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
					Session("sender") = ""
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
		ElseIf Result1 = 0 Then   'Code Added
			Session("sender") = ""
			'   DataFieldBind()
		End If
	End Sub
	'Added by Saylee on 19-Mar-2013 for ALL14032013-1
	'Added (RemovedCompStatus) parameter By Utkarsh ON 04-Apr-2013 FOR ALL04042013
	Public Function CheckPeriodsForRemovedCompStatus(ByVal RemovedCompStatus As CompStatus) As Boolean
		Dim i As Integer = 0
		Dim tmpIsPeriodExists As Boolean = False
		'Commented By Utkarsh ON 04-Apr-2013 FOR ALL04042013
		'Dim RemovedCompStatus As CompStatus = CompStatus.GetCompStatus(mRemovedCompList(Index).CompStatusID, mRemovedCompList(Index).AssemblyStatusID, txtInstallationDate.Text )
		'End
		' Dim mtmpAssemblyStatusList As AssemblyStatusList = AssemblyStatusList.GetAssemblyStatusList(New Guid(cmbInstalledOnAssembly.SelectedValue.ToString))
		Dim mAssemblyStatusList As AssemblyStatusList
		mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(mInstCompStatus.InstalledOnFormatted.ToString, Session("InstalledOnMachineID").ToString, , , , , , , , , , True, , , mInstCompStatus.AssemblyID.ToString, , , , , , , , , , , , , , , , , MonitoringInspRequired:=False, MonitoringModRequired:=False, MonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList()

		'  For j As Integer = 0 To mAssemblyStatusList.Count - 1
		If mAssemblyStatusList(0).AssemblyID.Equals(mInstCompStatus.AssemblyID) Then
			Dim tmpAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyStatusList(0).ID)
			While i <= RemovedCompStatus.CompStatusPeriods.Count - 1
				If tmpAssemblyStatus.AssemblyStatusPeriods.Contains(RemovedCompStatus.CompStatusPeriods(i).PeriodID) Then
					tmpIsPeriodExists = True
				Else
					tmpIsPeriodExists = False
					Exit While
				End If
				i = i + 1
			End While
		End If
		'   Next

		Return tmpIsPeriodExists
	End Function
	Private Sub SetPage()
		If RecordsToShowForRemCompList < mCompStatusList.Count Then
			lblRemovedComponents.Text = "List of Removed components as per selected criteria : " & RecordsToShowForRemCompList.ToString & " of " & mCompStatusList.Count & " Record(s) shown."
		Else
			lblRemovedComponents.Text = "List of Removed components as per selected criteria : " & mCompStatusList.Count & " Record(s) found."
		End If
	End Sub
	Private Sub ControlVisibility()
		If RecordsToShowForRemCompList < mCompStatusList.Count Then
			lnkRemCompLoadMoreTop.Visible = True
		Else
			lnkRemCompLoadMoreTop.Visible = False
		End If
		''Added By Prashant 2-Dec-2020
		'If (User.IsInRole("BuildSpareCompNew") = True And User.IsInRole("BuildSpareCompEdit") = True) Then
		'    lnkSpareComponent.Visible = True
		'End If
		''End of Added By Prashant 2-Dec-2020
		If Session("OpenForWOJobCompRemInstNewPage") IsNot Nothing Then
			If Session("OpenForWOJobCompRemInstNewPage") = "True" Then
				txtPart.Enabled = False
				txtSerialNo.Enabled = False
			End If
		Else
			txtPart.Enabled = True
			txtSerialNo.Enabled = True

		End If

	End Sub
	Private Sub FindNow()

		dgRemovedList.PageIndex = 0


		'Added By Rahul on 29-Apr-2009
		Session("PartNO") = Trim(txtPart.Text)
		Session("SerialNo") = Trim(txtSerialNo.Text)
		'============================================

		mCompStatusList = CompStatusList.GetCompStatusList(CurrentDate:=mInstCompStatus.InstalledOnFormatted.ToString, AssemblyID:=Guid.Empty,
						  PartName:=Trim(txtPart.Text), CompSerialNo:=Trim(txtSerialNo.Text), MachineID:=Guid.Empty.ToString,
						  IsCompRemoved:=True, IsCompPeriodsRequired:=False, ShowForNotInUseAircrafts:=CType(AppSettings("ShowForNotInUseAircrafts"), Boolean)) 'New Added

		Session("mCompStatusList") = mCompStatusList

		dgRemovedList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
									Order By StatusInfo.PartName
									Select StatusInfo).ToList.Take(RecordsToShowForRemCompList)
		dgRemovedList.DataBind()
	End Sub
	Private Sub InstallRecord(ByVal mCompStatusInfo As CompStatusInfo)
		'Added By Utkarsh ON 04-Apr-2013 FOR ALL04042013
		Dim mRemovedCompStatus As CompStatus = CompStatus.GetCompStatus(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, mInstCompStatus.InstalledOnFormatted.ToString)
		'End
		Session("InstalledOnMachineID") = mCompStatusInfo.MachineID.ToString
		''mInstCompStatus = CompStatus.NewInstallCompStatus(Guid.Empty, mRemovedCompStatus.AssemblyID, mCompStatusInfo.AssemblyStatusID, mInstCompStatus.InstalledOnFormatted.ToString, True, mRemovedCompStatus.ID.ToString, Guid.Empty.ToString)
		mInstCompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mRemovedCompStatus.AssemblyID, mCompStatusInfo.AssemblyStatusID, mInstCompStatus.InstalledOnFormatted.ToString, True, mRemovedCompStatus.ID.ToString, Guid.Empty.ToString)

		Session("mInstCompStatus") = mInstCompStatus
		'Added by Saylee on 19-Mar-2013 for ALL14032013-1
		'Changed By Utkarsh ON 04-Apr-2013 FOR ALL04042013 (if condition & Message text)
		If CheckPeriodsForRemovedCompStatus(mRemovedCompStatus) = False Then
			MSGBoxCtrl.Show("Component Status Installation Alert!", "Periods for " & mRemovedCompStatus.PartNameSerialNo & " are mismatching with selected " & mInstCompStatus.AssemblyID.ToString & " Assembly on " & Session("InstalledOnMachineID").ToString & " .Can not be installed.", "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		Else
			mInstCompStatus.ATAID = mCompStatusInfo.ATAID
			mInstCompStatus.Comp.PartID = mCompStatusInfo.PartID
			mInstCompStatus.Position = mCompStatusInfo.Position
			mInstCompStatus.CompID = mCompStatusInfo.CompID
			Session("mInstCompStatus") = mInstCompStatus
			Session("mRemCompStatus") = mRemovedCompStatus
			Dim mRemAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompStatusInfo.AssemblyStatusID)
			Session("mRemAssemblyStatus") = mRemAssemblyStatus

			''28-Apr-2009 Commented
			''Dim mCompStatus As CompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, Guid.Empty, mRemovedCompList(Index).AssemblyStatusID, txtInstallationDate.Text , True, mRemovedCompList(Index).CompStatusID.ToString, Guid.Empty.ToString)
			''28-Apr-2009 Replaced
			'Dim mCompStatus As CompStatus = CompStatus.NewInstallCompStatus(Guid.NewGuid, mInstCompStatus.AssemblyID, mCompStatusInfo.AssemblyStatusID, _
			'                                mInstCompStatus.InstalledOnFormatted.ToString, True, mCompStatusInfo.ID.ToString,  Guid.Empty.ToString)
			''---

			''Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompStatusInfo.AssemblyStatusID)
			''Dim mMachine As Machine = Machine.GetMachine(mCompStatusInfo.MachineID)
			'Dim mAssemblyStatus As AssemblyStatus
			'Dim mMachine As Machine

			'mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompStatusInfo.AssemblyStatusID)
			'mMachine = Machine.GetMachine(mCompStatusInfo.MachineID)

			'Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mCompStatus.ID, Sort:=1) 'Sort = 1 : Installation
			'Session("mFileAttach") = mFileAttach
			''---28-Apr-2009
			'Session("IsAdded") = "False"

			''---28-Apr-2009

			'Session("From") = 1 'NewInstall
			'Session("InstallSelected") = 1
			'Session("mCompStatus") = mCompStatus
			'Session("mRemovedCompStatus") = mRemovedCompStatus
			'Session("mAssemblyStatus") = mAssemblyStatus
			'Session("mMachine") = mMachine

			'''NewMachineMaintenance() 'Added by Saylee on 8th-Oct-2009

			''Changed By Utkarsh On 26-Jul-2011 For All19072011
			'MaintDetail = "Reg No. : " + mCompStatusList(mRemovedCompStatus.ID).MachineInfo & " Assembly Info : " & mCompStatusList(mRemovedCompStatus.ID).AssemblyInfo.Replace(Environment.NewLine, " ") & " Part Info : " & mCompStatusList(mRemovedCompStatus.ID).CompInfo.Replace(Environment.NewLine, " ")
			'MarkLog(Util.Action.Install, "Component Installation", MaintDetail, Util.ErrorType.NoError, mRemovedCompStatus.ID, EventLogID)
			''End


			''''Changed By Utkarsh ON 24-Apr-2012 For ALL23042012 (For Buddha Air)
			'''If (AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
			'''    str = "openledgersame('wfInstallCompBA.aspx?GChildPage2=Index.aspx');"
			'''Else
			'''    str = "openledgersame('wfInstallComp.aspx?GChildPage2=Index.aspx');"
			'''End If
			''''End
			'''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", str, True)

			'''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openledgersame('wfInstallComp_AJAX.aspx?GChildPage2=Index.aspx');", True)
		End If
	End Sub
#End Region

#Region " Data Bindng "
	Private Sub DataFieldBind()
		If Not IsDate(RemoveDate) Then
			RemoveDate = Today.Date.ToString(AppSettings("DateFormat")) 'Added By Rahul on 29-Apr-2009
		Else
		End If


		'Commented and added by Rahul 29-Apr-09
		'mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(txtInstallationDate.Text , AircraftId, Trim(txtPart.Text), Trim(txtSerialNo.Text), New Guid(AssemblyId))

		'Commented and added by Rahul 29-Apr-09
		'VmRemovedCompList = tmpRemovedCompList.GetRemovedCompList(RemoveDate, AircraftId, PartNo, SerialNo, New Guid(AssemblyId), CType(AppSettings("ShowForNotInUseAircrafts"), Boolean))
		'New Added

		If PartNo Is Nothing Or PartNo = "" Then
			mCompStatusList = CompStatusList.GetCompStatusList(CurrentDate:=RemoveDate, AssemblyID:=Guid.Empty, PartName:="",
						 CompSerialNo:="", MachineID:=Guid.Empty.ToString, IsCompRemoved:=True, IsCompPeriodsRequired:=False,
						 ShowForNotInUseAircrafts:=CType(AppSettings("ShowForNotInUseAircrafts"), Boolean)) 'New Added
		Else
			mCompStatusList = CompStatusList.GetCompStatusList(CurrentDate:=RemoveDate, AssemblyID:=Guid.Empty, PartName:=PartNo,
						 CompSerialNo:=SerialNo, MachineID:=Guid.Empty.ToString, IsCompRemoved:=True, IsCompPeriodsRequired:=False,
						 ShowForNotInUseAircrafts:=CType(AppSettings("ShowForNotInUseAircrafts"), Boolean)) 'New Added
		End If


		Session("mCompStatusList") = mCompStatusList

		dgRemovedList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
									Order By StatusInfo.PartName
									Select StatusInfo).ToList.Take(RecordsToShowForRemCompList)

		DataBind()


		RemoveDate = mInstCompStatus.InstalledOnFormatted.ToString


		'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft

		'***********************************

		'Added By Rahul on 29-Apr-2009
		txtPart.Text = PartNo
		txtSerialNo.Text = SerialNo
		'===========================

		Session("RemoveDate") = RemoveDate
		Session("MachineId") = AircraftId
		Session("AssemblyId") = AssemblyId
		Session("InstallOnId") = InstallOnId
		'28-Apr-2009
		Session("mInstallOnAssemblyID") = mInstallOnAssemblyID
	End Sub

#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 26-Jul-2011 For All19072011
		If Not IsPostBack Then
			RecordsToShowForRemCompList = 10
			Session("RecordsToShowForRemCompList") = RecordsToShowForRemCompList
			DataFieldBind()
			SetPage()
			ControlVisibility()

		End If
	End Sub
	Private Sub dgRemovedList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRemovedList.RowCommand
		Dim mCompStatusInfo As CompStatusInfo
		Select Case e.CommandName
			Case "InstallSelected"
				mCompStatusInfo = mCompStatusList(New Guid(dgRemovedList.DataKeys(CInt(e.CommandArgument)).Value.ToString))
				If Not User.IsInRole("ComponentInstallationNew") Then
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
				End If
				'GridBind(False, True)
				InstallRecord(mCompStatusInfo)
				RemoveSession()
				Dim mopenas As String = Request.QueryString("Type")
				If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
					Exit Sub
				End If
			Case "ShowVal"
				Dim mtmpRemovedCompList As tmpRemovedCompList
				mCompStatusInfo = mCompStatusList(New Guid(e.CommandArgument.ToString))
				mtmpRemovedCompList = tmpRemovedCompList.GetRemovedCompList(mCompStatusInfo.RemovedOnFormatted.ToString, mCompStatusInfo.MachineID.ToString, mCompStatusInfo.PartName, mCompStatusInfo.CompSerialNo, mCompStatusInfo.AssemblyID, CType(AppSettings("ShowForNotInUseAircrafts"), Boolean))

				dgRemovedList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
											Order By StatusInfo.PartName
											Select StatusInfo).ToList.Take(RecordsToShowForRemCompList)

				Dim RemLabel, TSOLabel As Label
				Dim Remlnkbtn, TSOlnkbtn As LinkButton
				Dim currentRow As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)

				RemLabel = CType(currentRow.FindControl("lblRemValues"), Label)
				TSOLabel = CType(currentRow.FindControl("lblRemTSOValues"), Label)


				Remlnkbtn = CType(currentRow.FindControl("lnkRemValue"), LinkButton)
				TSOlnkbtn = CType(currentRow.FindControl("lnkRemTSOValue"), LinkButton)

				Remlnkbtn.Visible = False
				TSOlnkbtn.Visible = False

				If mtmpRemovedCompList.Count > 0 Then
					If mtmpRemovedCompList.Contains(mCompStatusInfo.ID) Then
						TSOLabel.Text = mtmpRemovedCompList(mCompStatusInfo.ID).TSOFormatted
						RemLabel.Text = mtmpRemovedCompList(mCompStatusInfo.ID).TextFormatted.ToString
					End If
				Else
					TSOLabel.Text = ""
					RemLabel.Text = ""
				End If
		End Select
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		RemoveSession()
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
	End Sub
	Private Sub txtPart_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPart.TextChanged
		Part = txtPart.Text
		FindNow()
		SetPage()
		ControlVisibility()
		upnlRemovalGrid.Update()
		upnlActionBtn.Update()
		upnlSearchCriteria.Update()
	End Sub
	Private Sub txtSerialNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSerialNo.TextChanged
		SerialNo = txtSerialNo.Text
		FindNow()
		SetPage()
		ControlVisibility()
		upnlRemovalGrid.Update()
		upnlActionBtn.Update()
		upnlSearchCriteria.Update()
	End Sub
	'New addition by Rupali on 22-Jun-09 for Sorting Order
	Private Sub dgRemovedList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRemovedList.Sorting
		mCompStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mCompStatusList") = mCompStatusList
		dgRemovedList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
									Order By StatusInfo.PartName
									Select StatusInfo).ToList.Take(RecordsToShowForRemCompList)
		dgRemovedList.DataBind()
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	Protected Sub dgRemovedList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
		If e.Row.RowType = DataControlRowType.DataRow Then
			For i As Integer = 0 To e.Row.Cells.Count - 1
				e.Row.Cells(i).ToolTip = dgRemovedList.Columns(i).HeaderText
			Next
		End If
	End Sub
	Private Sub lnkRemCompLoadMoreTop_Click(sender As Object, e As System.EventArgs) Handles lnkRemCompLoadMoreTop.Click
		RecordsToShowForRemCompList = mCompStatusList.Count
		Session("RecordsToShowForRemCompList") = RecordsToShowForRemCompList
		dgRemovedList.DataSource = (From StatusInfo As CompStatusInfo In mCompStatusList
									Order By StatusInfo.PartName
									Select StatusInfo).ToList
		dgRemovedList.DataBind()
		'VlnkRemCompLoadMore.Enabled = False
		'VlnkRemCompLoadMoreTop.Enabled = False
		SetPage()
		ControlVisibility()
	End Sub
#End Region


End Class