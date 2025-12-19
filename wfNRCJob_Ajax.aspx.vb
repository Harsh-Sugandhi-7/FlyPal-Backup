Imports System.Collections.Generic
Imports System.Linq
Public Class wfNRCJob_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Description "
	Protected mEmployeeStatus As EmployeeStatus
	Protected mEmployee As Employee
	Protected mNRC As NRC
	Dim message As String = ""
	Dim mEmployeeList As EmployeeList
#End Region

#Region " Business Methods "
	Private Sub getSession()
		mNRC = Session("mNRC")
	End Sub
	Private Sub RemoveSession()
		Session.Remove("Edit")
	End Sub
	Private Sub setSession()
		Session("mNRC") = mNRC
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Ok
					If MSGBoxCtrl.Sender = "DoneByAME" Then
						txtDoneByAME.Text = ""
						txtDoneByAME.DataBind()
						mNRC.NRCJobs.CurrentItem.DoneByAMEID = Guid.Empty
						mNRC.NRCJobs.CurrentItem.DoneByAMEName = ""
						upnlNRCJobDetail.Update()
					ElseIf MSGBoxCtrl.Sender = "ObserveByAME" Then
						txtObserveByAME.Text = ""
						mNRC.NRCJobs.CurrentItem.ObserveByAMEID = Guid.Empty
						mNRC.NRCJobs.CurrentItem.ObserveByAMEName = ""
						upnlNRCJobDetail.Update()
					ElseIf MSGBoxCtrl.Sender = "DoneByTech" Then
						txtDoneByTech.Text = ""
						txtDoneByTech.DataBind()
						mNRC.NRCJobs.CurrentItem.DoneByTechID = Guid.Empty
						mNRC.NRCJobs.CurrentItem.DoneByTechEName = ""
						upnlNRCJobDetail.Update()
					End If '
			End Select
		End If
	End Sub
	Private Function setObject() As Boolean
		mNRC.NRCJobs.CurrentItem.SrNo = mNRC.NRCJobs.CurrentIndex + 1
		mNRC.NRCJobs.CurrentItem.Observation = Trim(txtObservation.Text)
		If hdnObserveByAMEID.Value = "" Then
			'Do nothing
		Else
			mNRC.NRCJobs.CurrentItem.ObserveByAMEID = New Guid(hdnObserveByAMEID.Value)
		End If

		mNRC.NRCJobs.CurrentItem.Rectification = Trim(txtRectification.Text)

		If hdnDoneByAMEID.Value = "" Then
			'Do nothing
		Else
			mNRC.NRCJobs.CurrentItem.DoneByAMEID = New Guid(hdnDoneByAMEID.Value)

		End If
		If hdnDoneByTechID.Value = "" Then
			'Do nothing
		Else
			mNRC.NRCJobs.CurrentItem.DoneByTechID = New Guid(hdnDoneByTechID.Value)
		End If
		mNRC.ApplyEdit()
		Return True
	End Function
	Private Sub ControlVisibility()

	End Sub
	Private Sub SetLicenseNo()

	End Sub
	Private Sub DataFieldBind()
		txtObserveByAME.Text = mNRC.NRCJobs.CurrentItem.ObserveByAMEName
		txtDoneByAME.Text = mNRC.NRCJobs.CurrentItem.DoneByAMEName
		txtDoneByTech.Text = mNRC.NRCJobs.CurrentItem.DoneByTechEName
		DataBind()
	End Sub
#End Region

#Region " Events "
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		getSession()
		If Not IsPostBack Then
			txtObservation.Focus()
			DataFieldBind()
			SetLicenseNo()
			SetLicenseNo()
			ControlVisibility()
		End If
	End Sub
	Protected Sub txtObserveByAME_TextChanged(sender As Object, e As System.EventArgs)
		mEmployeeList = EmployeeList.GetEmployeeList()
		'If hdnObserveByAMEID.Value <> "" Then
		'    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(hdnObserveByAMEID.Value.ToString, mNRC.NRCDateFormatted.ToString)
		'    mEmployee = Employee.GetEmployee(New Guid(hdnObserveByAMEID.Value.ToString))
		'    If mEmployeeStatus.Count > 0 Then
		'        If (mEmployeeStatus(0).Information <> "") Then
		'            message = mEmployeeStatus(0).Information
		'            MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ObserveByAME")
		'            Exit Sub
		'        End If
		'        mNRC.NRCJobs.CurrentItem.ObserveByAMEID = New Guid(hdnObserveByAMEID.Value)
		'        mNRC.NRCJobs.CurrentItem.ObserveByAMEName = mEmployee.Name
		'        mNRC.NRCJobs.CurrentItem.ObserveByAMENo = mEmployee.EmpNo
		'    Else
		'        mNRC.NRCJobs.CurrentItem.ObserveByAMEID = New Guid(hdnObserveByAMEID.Value)
		'        mNRC.NRCJobs.CurrentItem.ObserveByAMEName = mEmployee.Name
		'        mNRC.NRCJobs.CurrentItem.ObserveByAMENo = mEmployee.EmpNo
		'    End If
		'Else
		'    txtObserveByAME.Text = ""
		'    mNRC.NRCJobs.CurrentItem.ObserveByAMEID = Guid.Empty
		'    mNRC.NRCJobs.CurrentItem.ObserveByAMEName = ""
		'    mNRC.NRCJobs.CurrentItem.ObserveByAMENo = ""
		'End If
		If mEmployeeList.Contains(txtObserveByAME.Text) Then
			mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeList(txtObserveByAME.Text, "").ID.ToString, mNRC.NRCDateFormatted.ToString)
			mEmployee = Employee.GetEmployee(mEmployeeList(txtObserveByAME.Text, "").ID)
			If mEmployeeStatus.Count > 0 Then
				If (mEmployeeStatus(0).Information <> "") Then
					message = mEmployeeStatus(0).Information
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ObserveByAME")
					Exit Sub
				End If
				mNRC.NRCJobs.CurrentItem.ObserveByAMEID = mEmployeeList(txtObserveByAME.Text, "").ID
				mNRC.NRCJobs.CurrentItem.ObserveByAMEName = mEmployee.Name
				mNRC.NRCJobs.CurrentItem.ObserveByAMENo = mEmployee.EmpNo
			Else
				mNRC.NRCJobs.CurrentItem.ObserveByAMEID = mEmployeeList(txtObserveByAME.Text, "").ID
				mNRC.NRCJobs.CurrentItem.ObserveByAMEName = mEmployee.Name
				mNRC.NRCJobs.CurrentItem.ObserveByAMENo = mEmployee.EmpNo
			End If
		Else
			txtObserveByAME.Text = ""
			mNRC.NRCJobs.CurrentItem.ObserveByAMEID = Guid.Empty
			mNRC.NRCJobs.CurrentItem.ObserveByAMEName = ""
			mNRC.NRCJobs.CurrentItem.ObserveByAMENo = ""
		End If
		upnlNRCJobDetail.Update()
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
		'        mNRC.NRCJobs.CurrentItem.DoneByAMEID = New Guid(hdnDoneByAMEID.Value)
		'        mNRC.NRCJobs.CurrentItem.DoneByAMEName = mEmployee.Name
		'        mNRC.NRCJobs.CurrentItem.DoneByAMENo = mEmployee.EmpNo
		'    Else
		'        mNRC.NRCJobs.CurrentItem.DoneByAMEID = New Guid(hdnDoneByAMEID.Value)
		'        mNRC.NRCJobs.CurrentItem.DoneByAMEName = mEmployee.Name
		'        mNRC.NRCJobs.CurrentItem.DoneByAMENo = mEmployee.EmpNo
		'    End If
		'Else
		'    txtDoneByAME.Text = ""
		'    mNRC.NRCJobs.CurrentItem.DoneByAMEID = Guid.Empty
		'    mNRC.NRCJobs.CurrentItem.DoneByAMEName = ""
		'    mNRC.NRCJobs.CurrentItem.DoneByAMENo = ""
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
				mNRC.NRCJobs.CurrentItem.DoneByAMEID = mEmployeeList(txtDoneByAME.Text, "").ID
				mNRC.NRCJobs.CurrentItem.DoneByAMEName = mEmployee.Name
				mNRC.NRCJobs.CurrentItem.DoneByAMENo = mEmployee.EmpNo
			Else
				mNRC.NRCJobs.CurrentItem.DoneByAMEID = mEmployeeList(txtDoneByAME.Text, "").ID
				mNRC.NRCJobs.CurrentItem.DoneByAMEName = mEmployee.Name
				mNRC.NRCJobs.CurrentItem.DoneByAMENo = mEmployee.EmpNo
			End If
		Else
			txtDoneByAME.Text = ""
			mNRC.NRCJobs.CurrentItem.DoneByAMEID = Guid.Empty
			mNRC.NRCJobs.CurrentItem.DoneByAMEName = ""
			mNRC.NRCJobs.CurrentItem.DoneByAMENo = ""
		End If
		upnlNRCJobDetail.Update()
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
		'        mNRC.NRCJobs.CurrentItem.DoneByTechID = New Guid(hdnDoneByTechID.Value)
		'        mNRC.NRCJobs.CurrentItem.DoneByTechEName = mEmployee.Name
		'        mNRC.NRCJobs.CurrentItem.DoneByTechNo = mEmployee.EmpNo
		'    Else
		'        mNRC.NRCJobs.CurrentItem.DoneByTechID = New Guid(hdnDoneByTechID.Value)
		'        mNRC.NRCJobs.CurrentItem.DoneByTechEName = mEmployee.Name
		'        mNRC.NRCJobs.CurrentItem.DoneByTechNo = mEmployee.EmpNo
		'    End If
		'Else
		'    txtDoneByTech.Text = ""
		'    mNRC.NRCJobs.CurrentItem.DoneByTechID = Guid.Empty
		'    mNRC.NRCJobs.CurrentItem.DoneByTechEName = ""
		'    mNRC.NRCJobs.CurrentItem.DoneByTechNo = ""
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
				mNRC.NRCJobs.CurrentItem.DoneByTechID = mEmployeeList(txtDoneByTech.Text, "").ID
				mNRC.NRCJobs.CurrentItem.DoneByTechEName = mEmployee.Name
				mNRC.NRCJobs.CurrentItem.DoneByTechNo = mEmployee.EmpNo
			Else
				mNRC.NRCJobs.CurrentItem.DoneByTechID = mEmployeeList(txtDoneByTech.Text, "").ID
				mNRC.NRCJobs.CurrentItem.DoneByTechEName = mEmployee.Name
				mNRC.NRCJobs.CurrentItem.DoneByTechNo = mEmployee.EmpNo
			End If
		Else
			txtDoneByTech.Text = ""
			mNRC.NRCJobs.CurrentItem.DoneByTechID = Guid.Empty
			mNRC.NRCJobs.CurrentItem.DoneByTechEName = ""
			mNRC.NRCJobs.CurrentItem.DoneByTechNo = ""
		End If
		upnlNRCJobDetail.Update()
		Session("mNRC") = mNRC
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
		If IsValid Then
			If setObject() Then
				Session("mNRC") = mNRC
				RemoveSession()
				Dim mopenas As String = Request.QueryString("Type")
				If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
					Exit Sub
				ElseIf mopenas IsNot Nothing AndAlso mopenas = "MELpup" Then
					'Response.Redirect("wfNRC_Ajax.aspx?BackPage=index.aspx")
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallbackDirect();", True)
					Exit Sub
				End If
			End If
		Else
			'upnlValidationSummary.Update()
			'Exit Sub
		End If
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		If mNRC.NRCJobs.CurrentItem.IsNew And Not Session("Edit") = True Then mNRC.NRCJobs.Remove(mNRC.NRCJobs.CurrentItem)
		Session("mNRC") = mNRC
		RemoveSession()
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		ElseIf mopenas IsNot Nothing AndAlso mopenas = "MELpup" Then
			'Response.Redirect("wfNRC_Ajax.aspx?BackPage=index.aspx")
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallbackDirect();", True)
			Exit Sub
		End If
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