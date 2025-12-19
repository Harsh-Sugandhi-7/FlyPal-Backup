'AJAX Conversion By Vikrant

Public Class wfEmployee_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mEmployeeCityList As CityInvList
    Public mGenderList As GenderList
    Public mContractorList As ContractorList
    Public mEmployee As Employee
    'Public BackPage As String
    Public strMsg As String = ""
    Dim EventLogID As Guid 'Added by Saylee on 22-July-2011
    Dim path As String = String.Empty
    Dim test As String
    Dim mFileAttach As FileAttach
	Dim IsDigitalSignatureDeleted As Boolean = False
	Public mLocationList As LocationList 'Aeed By Shital on 07-May-2020
    Public mIsRenew As Boolean = False
    '*************************************** Ajay 23-Nov-2022
    'EMPLOYEE DEPARTMENT INFO
    Public mEmployeeDepartmentInfo As EmployeeDepartmentInfo
    Public mEmployeeDepartmentInfoList As EmployeeDepartmentInfoList

    'EMPLOYEE SKILL
    Public mEmployeeSkill As EmployeeSkill
    Public mEmployeeSkillList As EmployeeSkillList

    'EMPLOYEE SERVICE
    Public mEmployeeService As EmployeeService
    Public mEmployeeServiceList As EmployeeServiceList

    'EMPLOYEE TRAINING
    Public mEmployeeTraining As EmployeeTraining
    Public mEmployeeTrainingList As EmployeeTrainingList
    'New addition by Amrita for Training Renewal
    Public mTraining As Training
    Public mFreqInMonths As Integer = 0

    'EMPLOYEE DOCUMENT
    Public mEmployeeDocument As EmployeeDocument
    Public mEmployeeDocumentList As EmployeeDocumentList

    'EMPLOYEE DESIGNATION
    Public mEmployeeDesignation As EmployeeDesignation
    Public mEmployeeDesignationList As EmployeeDesignationList

    'EMPLOYEE NEXT TO KIN INFO
    Public mEmployeeContactInfo As EmployeeContactInfo
    Public mEmployeeContactInfoList As EmployeeContactInfoList

    'EMPLOYEE DISCIPLINARY
    Public mEmployeeDisciplinary As EmployeeDisciplinary
    Public mEmployeeDisciplinaryList As EmployeeDisciplinaryList

    'EMPLOYEE LEAVE
    Public mEmployeeLeave As EmployeeLeave
    Public mEmployeeLeaveList As EmployeeLeaveList

    'To check History link visibility
    Public mEmployeeDocumentHistoryList As EmployeeDocumentHistoryList

    Dim Type As Int16

    'Dim EventLogID As Guid 'Added by Saylee on 19-July-2011

    'COMPANY EQUIPMENTT INFO  'Added By Prashant 16-July-2012
    Public mCompanyEquipment As CompanyEquipment
    Public mCompanyEquipmentList As CompanyEquipmentList

	'******************************* End
	Dim mCompanyDetail As New CompanyDetail
	Public AttachmentHelper As New AttachmentHelper

#End Region

#Region " Business Methods "

	Private Sub GetSession()
		mEmployeeCityList = CType(Session("mEmployeeCityList"), CityInvList)
		mGenderList = CType(Session("mGenderList"), GenderList)
		mContractorList = CType(Session("mContractorList"), ContractorList)
		mEmployee = CType(Session("mEmployee"), Employee)
		path = IIf(IsNothing(Session("path")), "", Session("path"))
		mFileAttach = Session("mFileAttach")
		IsDigitalSignatureDeleted = Session("IsDigitalSignatureDeleted")
		mLocationList = Session("mLocationList")
		mEmployee = CType(Session("mEmployee"), Employee)  'Ajay
		mCompanyDetail = Session("mCompanyDetail")
	End Sub

	Private Sub SetSession()
		Session("mEmployeeCityList") = mEmployeeCityList
		Session("mGenderList") = mGenderList
		Session("mContractorList") = mContractorList
		Session("mEmployee") = mEmployee
		Session("mCompanyDetail") = mCompanyDetail

	End Sub

	Private Sub RemoveSession()
		Session.Remove("mGenderList")
		Session.Remove("mEmployeeCityList")
		Session.Remove("mContractorList")
		Session.Remove("mFileAttach")
		Session.Remove("IsDigitalSignatureDeleted")
		Session.Remove("mCompanyDetail")
	End Sub

	Private Sub SetObject()
		mEmployee.EmpNo = Trim(txtEmpNo.Text)
		mEmployee.Name = Trim(txtName.Text)
		'mEmployee.DesignationID = New Guid(cmbDesignationList.SelectedValue.ToString)
		mEmployee.Address1 = Trim(txtAddress1.Text)
		mEmployee.Address2 = Trim(txtAddress2.Text)

		mEmployee.CityID = New Guid(cmbCityList.SelectedValue.ToString)
		mEmployee.StateName = Trim(txtState.Text)
		mEmployee.CountryName = Trim(txtCountry.Text)

		mEmployee.PointOfOrigin = Trim(txtPointOfOrigin.Text)
		mEmployee.PhoneNo = Trim(txtPhoneNo.Text)
		mEmployee.Mobile = Trim(txtMobile.Text)
		mEmployee.Email = Trim(txtEmail.Text)

		mEmployee.GenderID = cmbGenderList.SelectedValue

		mEmployee.Day = Val(txtDay.Text) 'Trim(txtDay.Text)
		mEmployee.Month = Val(txtMonth.Text) 'Trim(txtMonth.Text)
		mEmployee.Year = Val(txtYear.Text) 'Trim(txtYear.Text)

		mEmployee.ExpatStatus = chkExpatStatus.Checked
		mEmployee.Nationality = Trim(txtNationality.Text)

		mEmployee.ContractorID = New Guid(cmbContractorList.SelectedValue.ToString)

		mEmployee.IsWorking = chkWorkingStatus.Checked
		mEmployee.DateOfLeaving = CType(txtDateOfLeaving.Text, Object)
		mEmployee.LicenseNo = Trim(txtLicenceNo.Text) 'Added By Shweta On 04-Mar-2012

		'AttachMyFile()
		ShowPicture()

		mEmployee.IsUseInFlightLog = chkUseInFlightLog.Checked 'Added by Saylee on 3-Mar-2011

		mEmployee.CurrAddress1 = Trim(txtCurrAddress1.Text)
		mEmployee.CurrAddress2 = Trim(txtCurrAddress2.Text)
		mEmployee.CurrPointOfOrigin = Trim(txtCurrPointOfOrigin.Text)
		mEmployee.CurrCityID = New Guid(cmbCurrCityList.SelectedValue.ToString)
		mEmployee.CurrPhoneNo = Trim(txtCurrPhoneNo.Text)
		mEmployee.CurrMobile = Trim(txtCurrMobile.Text)
		mEmployee.CurrEmail = Trim(txtCurrEmail.Text)

		mEmployee.CurrStateName = Trim(txtCurrState.Text)
		mEmployee.CurrCountryName = Trim(txtCurrCountry.Text)

		mEmployee.CAT = Trim(txtCAT.Text) 'Added by Vikrant on 09-Dec-2013 For ALL09122013-1
		mEmployee.BankName = Trim(txtBankName.Text) 'Added by Abhishek
		mEmployee.AccountNo = Trim(txtAccountNo.Text) 'Added by Abhishek
		mEmployee.PanNo = Trim(txtPanNo.Text) 'Added by Abhishek
		mEmployee.IsContractedEmployee = chkContractedEmployee.Checked
		mEmployee.LocationID = New Guid(cmbLocation.SelectedValue.ToString) ' Added by Shital on 07-May-2020
		mEmployee.IsTechnicalCrew = chkIsTechnicalCrew.Checked

		'Ajay added 31-10-2023
		mEmployee.Zip = Trim(txtZip.Text)
		mEmployee.CurrZip = Trim(txtCurrZip.Text)
		mEmployee.IsOthers = chkIsOthers.Checked 'Added by Prashant on 24-Feb-2025 
		Session("mEmployee") = mEmployee
	End Sub

	Private Sub AttachMyFile()

		Try

			If Session("FileUpload.FileExtension") IsNot Nothing Then

				mEmployee.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
				mEmployee.ImageSize = Session("FileUpload.FileSize")
				mEmployee.FileExtension = Session("FileUpload.FileExtension")

				Session("mEmployee") = mEmployee

				Session.Remove("FileUpload.FileSize")
				Session.Remove("FileUpload.FileContent")
				Session.Remove("FileUpload.FileExtension")

			End If

			If mFileAttach IsNot Nothing Then

				If mFileAttach.FileName = "DigitalSignature" Then
					mEmployee.IsDigitalSignatureAdded = True
				End If

			End If

			ControlVisibilityForAttachment()

			upnlDigitalFileupload.Update()
			Session("mEmployee") = mEmployee

			MSGBoxCtrl.Show("Attachment Added!",
							"Digital Signature uploaded successfully!!!",
							"",
							MsgBoxStyle.OkOnly,
							"")

		Catch ex As Exception
			MSGBoxCtrl.Show("Attachment Alert!",
							ex.Message,
							"",
							MsgBoxStyle.Information,
							"")
		End Try

	End Sub

	Private Sub ControlVisibilityForAttachment()

		Try

			If mEmployee.ImageSize > 0 Then
				btnDelAttach.Enabled = True
			Else
				btnDelAttach.Enabled = False
			End If

			upnlAttachment.Update()

			If mEmployee.IsDigitalSignatureAdded Then
				btnDelDigitalAttach.Visible = True
				btnDelDigitalAttach.Enabled = True
			Else
				btnDelDigitalAttach.Enabled = False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If

        '************************* Ajay 
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    'EMPLOYEE SERVICE
                    If MSGBoxCtrl.Sender = "DeleteService" Then
                        Try
                            Session("sender") = ""
                            mEmployeeService = Session("mEmployeeService")
                            EmployeeService.DeleteEmployeeService(mEmployeeService.ID)
                            mEmployee = Employee.GetEmployee(mEmployeeService.EmployeeID)
                            BindEmpService()
                            upnlService.Update()
                            'Response.Redirect("wfEmployeeDetails_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Service", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Service : " + mEmployeeService.ServiceName + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeService.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Service", "Emp : " + mEmployee.EmpNoName + " Service : " + "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                        End Try
                    End If
                    '------END OF EMPLOYEE SERVICE

                    'EMPLOYEE SKILL
                    If MSGBoxCtrl.Sender = "DeleteSkill" Then
                        Dim SkillName As String
                        Try
                            Session("sender") = ""
                            mEmployeeSkill = Session("mEmployeeSkill")
                            SkillName = mEmployeeSkill.SkillName
                            EmployeeSkill.DeleteEmployeeSkill(mEmployeeSkill.ID)
                            BindEmpSkill()
                            upnlSkill.Update()
                            'Response.Redirect("wfEmployeeDetails_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Flypal.Util.Action.Delete, "Employee Skill", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Skill : " + SkillName + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeSkill.ID, EventLogID)
                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Skill", "Emp : " + mEmployee.EmpNoName + " Skill : " + SkillName, Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                        End Try
                    End If
                    '-----END OF EMPLOYEE SKILL

                    'EMPLOYEE TRAINING
                    If MSGBoxCtrl.Sender = "DeleteTraining" Then
                        Dim TrainingName As String
                        Try
                            Session("sender") = ""
                            mEmployeeTraining = Session("mEmployeeTraining")
                            TrainingName = mEmployeeTraining.TrainingName
                            EmployeeTraining.DeleteEmployeeTraining(mEmployeeTraining.ID)
                            BindEmpTraining()
                            upnlTraining.Update()
                            'Response.Redirect("wfEmployeeDetails_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))

                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Training", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Training : " + TrainingName + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeTraining.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Training", "Emp : " + mEmployee.EmpNoName + " Training : " + TrainingName, Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                        End Try
                    End If
                    '-----END OF EMPLOYEE TRAINING

                    'EMPLOYEE DOCUMENT
                    If MSGBoxCtrl.Sender = "DeleteDocument" Then
                        Try
                            Session("sender") = ""
                            mEmployeeDocument = Session("mEmployeeDocument")
                            EmployeeDocument.DeleteEmployeeDocument(mEmployeeDocument.ID)
                            BindEmpDocument()
                            upnlDocument.Update()
                            'Response.Redirect("wfEmployeeDetails_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Document", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Document ; " + mEmployeeDocument.DocumentName + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeDocument.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Document", "Emp : " + mEmployee.EmpNoName + " Document : " + mEmployeeDocument.DocumentName, Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                        End Try
                    End If
                    '-----END OF EMPLOYEE DOCUMENT

                    'EMPLOYEE DESIGNATION
                    If MSGBoxCtrl.Sender = "DeleteDesignation" Then
                        Dim DesignationName As String
                        Try
                            Session("sender") = ""
                            mEmployeeDesignation = Session("mEmployeeDesignation")
                            DesignationName = mEmployeeDesignation.DesignationName
                            EmployeeDesignation.DeleteEmployeeDesignation(mEmployeeDesignation)
                            ''MarkLog(Flypal.Util.Action.Delete, "Employee Designation", "Emp : " + mEmployee.EmpNoName + " Designation : " + DesignationName, Flypal.Util.ErrorType.NoError, mEmployee.ID, EventLogID)
                            BindDesignation()
                            upnlDesignation.Update()
                            'Added by Amrita on 17-Amrita to set topmost designation
                            mEmployee = Employee.GetEmployee(mEmployeeDesignation.EmployeeID)
                            Session("mEmployee") = mEmployee
                            '---------------

                            'Response.Redirect("wfEmployeeDetails_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Designation", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Designation : " + DesignationName + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeDesignation.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then 'Added by Saylee on 22-Apr-2009
                                MarkLog(Flypal.Util.Action.Delete, "Employee Designation", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Designation : " + DesignationName + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeDesignation.ID, EventLogID)
                                MSGBoxCtrl.Show("Delete Alert!", ex.Message, "", MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Designation", "Emp : " + mEmployee.EmpNoName + " Designation : " + DesignationName, Flypal.Util.ErrorType.NoError, mEmployeeDesignation.ID, EventLogID)
                            End If
                        End Try
                    End If
                    '------END OF EMPLOYEE DESIGNATION

                    'EMPLOYEE DEPARTMENT
                    If MSGBoxCtrl.Sender = "DeleteEmployeeDepartmentInfo" Then
                        Dim DepartmentName As String
                        Try
                            Session("sender") = ""
                            mEmployeeDepartmentInfo = Session("mEmployeeDepartmentInfo")
                            DepartmentName = mEmployeeDepartmentInfo.EmployeeDepartmentName
                            EmployeeDepartmentInfo.DeleteEmployeeDepartmentInfo(mEmployeeDepartmentInfo)

                            mEmployee = Employee.GetEmployee(mEmployeeDepartmentInfo.EmployeeID)
                            'AJAX Session("mEmployee") = mEmployee
                            BindEmpDepartment()
                            upnlDepartment.Update()
                            'Response.Redirect("wfEmployeeDetails_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Department", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Department : " + DepartmentName + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeDesignation.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Department", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Department : " + DepartmentName + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeDesignation.ID, EventLogID)
                                MSGBoxCtrl.Show("Delete Alert!", ex.Message, "", MsgBoxStyle.OkOnly, "")
                            End If
                            'CHK DataFieldBind()
                            BindEmpDepartment()
                            upnlDepartment.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Department", "Emp : " + mEmployee.EmpNoName + " Designation : " + DepartmentName, Flypal.Util.ErrorType.NoError, mEmployeeDepartmentInfo.ID, EventLogID)
                            End If
                        End Try
                    End If
                    '------END OF EMPLOYEE DEPARTMENT

                    'EMPLOYEE NEXT TO KIN INFO
                    If MSGBoxCtrl.Sender = "DeleteContactInfo" Then
                        Dim ContactInfo As String
                        Try
                            Session("sender") = ""
                            mEmployeeContactInfo = Session("mEmployeeContactInfo")
                            ContactInfo = mEmployeeContactInfo.Name
                            EmployeeContactInfo.DeleteEmployeeContactInfo(mEmployeeContactInfo.ID)

                            mEmployee = Employee.GetEmployee(mEmployeeContactInfo.EmployeeID)
                            'AJAX Session("mEmployee") = mEmployee
                            BindEmpContactInfo()
                            upnlContactInfo1.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Next to Kin Info", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Next to Kin Info : " + ContactInfo + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeContactInfo.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Next to Kin Info", "Emp : " + mEmployee.EmpNoName + " Next To Kin Info : " + ContactInfo, Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                        End Try
                    End If
                    '-----END OF EMPLOYEE NEXT TO KIN INFO

                    'EMPLOYEE DISCIPLINARY
                    If MSGBoxCtrl.Sender = "DeleteDisciplinary" Then
                        Dim Description As String
                        Try
                            Session("sender") = ""
                            mEmployeeDisciplinary = Session("mEmployeeDisciplinary")
                            Description = mEmployeeDisciplinary.Description
                            EmployeeDisciplinary.DeleteEmployeeDisciplinary(mEmployeeDisciplinary.ID)
                            BindEmpDisciplinary()
                            upnlDisciplinary.Update()
                            'Response.Redirect("wfEmployeeDetails_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Disciplinary", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Disciplinary : " + Description + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeDisciplinary.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Disciplinary", "Emp : " + mEmployee.EmpNoName + " Disciplinary : " + Description, Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                        End Try
                    End If
                    '-----END OF EMPLOYEE DISCIPLINARY

                    'EMPLOYEE LEAVE 
                    If MSGBoxCtrl.Sender = "DeleteLeave" Then
                        Dim ClassificationName As String
                        Try
                            Session("sender") = ""
                            mEmployeeLeave = Session("mEmployeeLeave")
                            ClassificationName = mEmployeeLeave.ClassificationName
                            EmployeeLeave.DeleteEmployeeLeave(mEmployeeLeave.ID)
                            BindEmpLeaves()
                            upnlLeaves.Update()
                            '' MarkLog(Flypal.Util.Action.Delete, "Employee Leave Records", "Emp : " + mEmployee.EmpNoName + " Leave Records : " + ClassificationName, Flypal.Util.ErrorType.NoError, mEmployee.ID, EventLogID)
                            'Response.Redirect("wfEmployeeDetails_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Leave Records", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Leave Records : " + ClassificationName + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeLeave.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Leave Records", "Emp : " + mEmployee.EmpNoName + " Leave Records : " + ClassificationName, Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                        End Try
                    End If
                    '-----END OF EMPLOYEE LEAVE

                    'Company Equipment Record '''
                    If MSGBoxCtrl.Sender = "DeleteCompanyEquipmentRecord" Then
                        Dim EquipmentName As String
                        Try
                            Session("sender") = ""
                            mCompanyEquipment = Session("mCompanyEquipment")
                            EquipmentName = mCompanyEquipment.EquipmentName
                            CompanyEquipment.DeleteCompanyEquipment(mCompanyEquipment)

                            mEmployee = Employee.GetEmployee(mCompanyEquipment.EmployeeID)
                            Session("mEmployee") = mEmployee
                            BindEmpEquipment()
                            upnlCompanyEquipment.Update()
                            'Response.Redirect("wfEmployeeDetails_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show("Delete Alert!", ex.Message, "", MsgBoxStyle.OkOnly, "")
                                MarkLog(Flypal.Util.Action.Delete, "Employee Department", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Department : " + EquipmentName + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeDesignation.ID, EventLogID)
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Equipment", "Emp : " + mEmployee.EmpNoName + " Designation : " + EquipmentName, Flypal.Util.ErrorType.NoError, mCompanyEquipment.ID, EventLogID)
                            End If
                        End Try
                    End If
                    '------END OF Company Equipment
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
        '************************* Ajay End
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfEmployee_Ajax.aspx?" Then
            Session.Remove("mEmployee")
            Session.Remove("mGenderList")
            Session.Remove("mContractorList")
            Session.Remove("mEmployeeGenders")
        End If
    End Sub
    Private Sub SetTitle()
        If mEmployee.IsNew Then
            lblTitle.Text = "Employee [New]"
        Else
            If Len(mEmployee.EmpNo) > 15 Then
                lblTitle.Text = "Employee [" & mEmployee.EmpNo.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Employee [" & mEmployee.EmpNo & "]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Sub SetDepartmentTextBox()
        If Not mEmployee.IsNew Then
            Dim mEmployeeDepartmentInfoList As EmployeeDepartmentInfoList
            mEmployeeDepartmentInfoList = EmployeeDepartmentInfoList.GetEmployeeDepartmentTop1Info(mEmployee.ID, "", "1/1/1900", "1/1/2200", "", "", False, True)
            If mEmployeeDepartmentInfoList.Count > 0 Then
                txtDepartment.Text = mEmployeeDepartmentInfoList(0).EmployeeDepartmentName
            End If

            txtDesignationName.DataBind()
        End If
    End Sub
    Private Sub NewRecord()
        mEmployee = Employee.NewEmployee
        Session("mEmployee") = mEmployee
    End Sub
    Private Sub Save()
        setObject()
        'Try
        If mEmployee.IsValid Then
            mEmployee.Save()
            SaveAttachment()
            If txtName.Enabled = True Then
                setFocus(txtEmpNo)
            End If
            MarkLog(Flypal.Util.Action.Save, "Employee", mEmployee.EmpNoName, Flypal.Util.ErrorType.HandledError, mEmployee.ID, EventLogID)
            SetSession()
            SetTitle()
        Else
            If Not mEmployee.IsValid Then
                For j As Integer = 0 To mEmployee.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mEmployee.GetBrokenRulesCollection(j).Description + "<BR>"
                Next
            End If

            If strMsg.Trim <> "" Then
                cvDate.ErrorMessage = strMsg
                cvDate.IsValid = mEmployee.IsValid
            End If
            upnlValidationSummary.Update()
        End If
        'cvDate.ErrorMessage = mEmployee.GetBrokenRulesString
        'cvDate.IsValid = mEmployee.IsValid
        'End If
        'Catch ex As Exception
        '    'Throw ex.GetBaseException
        '    cvDay.ErrorMessage = mEmployee.GetBrokenRulesString
        '    cvDay.IsValid = mEmployee.IsValid

        '    cvMonth.ErrorMessage = mEmployee.GetBrokenRulesString
        '    cvMonth.IsValid = mEmployee.IsValid
        'End Try
    End Sub

    Public Sub ControlVisibility()

        txtDateOfLeaving.Enabled = CType(IIf(chkWorkingStatus.Checked = True, False, True), Boolean)

        tabDepartment.Visible = (Not mEmployee.IsNew And mEmployee.IsSyncFromCRS = False)
        tabContactInfo.Visible = (Not mEmployee.IsNew And mEmployee.IsSyncFromCRS = False)
        tabDesignation.Visible = (Not mEmployee.IsNew And mEmployee.IsSyncFromCRS = False)
        tabService.Visible = (Not mEmployee.IsNew And mEmployee.IsSyncFromCRS = False)
        tabDocument.Visible = (Not mEmployee.IsNew And mEmployee.IsSyncFromCRS = False)
        tabTraining.Visible = (Not mEmployee.IsNew And mEmployee.IsSyncFromCRS = False)
        tabSkill.Visible = (Not mEmployee.IsNew And mEmployee.IsSyncFromCRS = False)

        'ShowPicture() to prevent duplicates
        If chkWorkingStatus.Checked = True Then
            txtDateOfLeaving.Text = ""
        End If
        DisableName(mEmployee.ID) 'Added by : Shital 19-Jun-2020, ALL16062020

        'Added by Saylee on 12-Jan-2023 , for CRS sync as bool
        If mCompanyDetail.IsSyncApplication Then

            chkUseInFlightLog.Enabled = Not mCompanyDetail.IsSyncApplication
            btnSave.Enabled = (Not chkUseInFlightLog.Checked)
            btnDesignationAdd.Visible = (Not chkUseInFlightLog.Checked)
            dgDesignationList.Columns(5).Visible = (Not chkUseInFlightLog.Checked)

        Else

            chkUseInFlightLog.Enabled = True
            btnSave.Enabled = True
            btnDesignationAdd.Visible = True
            dgDesignationList.Columns(5).Visible = True

        End If
        ''**********************

    End Sub

    Private Sub DisableName(ByVal mId As Guid) 'Added by : Shital 19-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerEmployee(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtName.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub

	Private Sub ViewImage()

		Try

			AttachmentHelper.DownloadAttachmentWithName(ModuleName:="Employee",
														AttachmentObject:=mEmployee)

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"openFile",
												"openFile();",
												True)

		Catch ex As Exception
		Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ViewDigitalSignature()

		Try

			If mEmployee.IsDigitalSignatureAdded And mFileAttach Is Nothing Then
				mFileAttach = FileAttach.GetAttachment(ReferenceID:=mEmployee.ID, FileName:="DigitalSignature")
			End If

			AttachmentHelper.DownloadAttachmentWithName(AttachmentObject:=mFileAttach)

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"openFile",
												"openFile();",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Sub ShowPicture()

		Try

			If (path <> "") Then

				File.Delete(path)
				path = String.Empty
				Session("path") = path

			End If

			Dim No As New Random
			Dim StrName As String = "abc" & No.Next.ToString

			If mEmployee.ImageSize > 0 Then

				path = $"{AppSettings("DOCPath")}\{StrName}{mEmployee.FileExtension}"

				Dim FileStream As FileStream
				If File.Exists(path:=AppSettings("DOCPath")) = False Then

					'Delete File if exist
					File.Delete(path:=path)

					' Create the file.
					FileStream = File.Create(path:=path)

					'' Add some information to the file.
					FileStream.Write(mEmployee.ImageFile, 0, mEmployee.ImageFile.Length)
					FileStream.Close()

					Session("DOCPath") = path

					'For Server
					MyImage.ImageUrl = $"{AppSettings("HTTPSecurity")}{Me.Request.Url.Host}/{Me.Request.Url.Segments(1)}Documents/{StrName}{mEmployee.FileExtension}"

					'for Local
					'MyImage.ImageUrl = "http://" & Me.Request.Url.Host & "/" & Me.Request.Url.Segments(1) & "Documents/" & StrName & mEmployee.FileExtension

					MyImage.Visible = True
					Session("path") = path

				End If

			End If

			If mEmployee.IsDigitalSignatureAdded Then

				If mEmployee.IsDigitalSignatureAdded And mFileAttach Is Nothing Then
					mFileAttach = FileAttach.GetAttachment(ReferenceID:=mEmployee.ID,
														   FileName:="DigitalSignature")
				End If

				If mFileAttach.Size > 0 Then

					Dim FileName As String = "Digital" & No.Next.ToString
					Dim path As String = AppSettings("DOCPath") & "\" & FileName & mFileAttach.Extension
					Dim FileStream As FileStream

					If File.Exists(path:=AppSettings("DOCPath")) = False Then

						'Delete File if exist
						File.Delete(path:=AppSettings("DOCPath") & FileName & mFileAttach.Extension)

						' Create the file.
						FileStream = File.Create(path:=path)

						'' Add some information to the file.
						FileStream.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
						FileStream.Close()

						Session("DOCPath") = path

						'For Server
						imgMyDigitalSignature.ImageUrl = $"{AppSettings("HTTPSecurity")}{Me.Request.Url.Host}/{Me.Request.Url.Segments(1)}Documents/{FileName}{mFileAttach.Extension}"

						'For Local
						'imgMyDigitalSignature.ImageUrl = "http://" & Me.Request.Url.Host & "/" & Me.Request.Url.Segments(1) & "Documents/" & StrName1 & mFileAttach.Extension

						imgMyDigitalSignature.Visible = True
						Session("path") = path

					End If

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SaveAttachment() '

		Try

			If mFileAttach IsNot Nothing Then

				If mFileAttach.Size > 0 Then

					Try

						mFileAttach.Save()

					Catch ex As Exception
						ScriptManager.RegisterClientScriptBlock(Me,
																[GetType],
																"", MessageBox.Show(ex.InnerException.ToString, False),
																True)
					End Try

				Else

					If (Not mEmployee.IsNew) And IsDigitalSignatureDeleted Then
						FileAttach.DeleteAttachment(mFileAttach.ID, mEmployee.ID)
					End If
					IsDigitalSignatureDeleted = False
					Session("IsDigitalSignatureDeleted") = IsDigitalSignatureDeleted

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

    ' **************************** Ajay
    'Employee Department Info
    Private Sub NewEmployeeDepartmentInfoRecord()
        mEmployeeDepartmentInfo = EmployeeDepartmentInfo.NewEmployeeDepartmentInfo
        Session("mEmployeeDepartmentInfo") = mEmployeeDepartmentInfo
    End Sub
    Private Sub EditEmployeeDepartmentInfoRecord(ByVal mID As Guid)
        mEmployeeDepartmentInfo = EmployeeDepartmentInfo.GetEmployeeDepartmentInfo(mID)
        Session("mEmployeeDepartmentInfo") = mEmployeeDepartmentInfo
    End Sub
    Private Sub DeleteEmployeeDepartmentInfoRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteEmployeeDepartmentInfo")
        mEmployeeDepartmentInfo = EmployeeDepartmentInfo.GetEmployeeDepartmentInfo(mID)
        Session("mEmployeeDepartmentInfo") = mEmployeeDepartmentInfo
    End Sub
    '-----END OF EMPLOYEE DEPARTMENT 
    'EMPLOYEE SERVICE
    Private Sub NewServiceRecord()
        mEmployeeService = EmployeeService.NewEmployeeService
        Session("mEmployeeService") = mEmployeeService
    End Sub
    Private Sub EditServiceRecord(ByVal mID As Guid)
        mEmployeeService = EmployeeService.GetEmployeeService(mID)
        Session("mEmployeeService") = mEmployeeService
    End Sub
    Private Sub DeleteServiceRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteService")
        mEmployeeService = EmployeeService.GetEmployeeService(mID)
        Session("mEmployeeService") = mEmployeeService
    End Sub
    '----END OF EMPLOYEE SERVICE

    'EMPLOYEE SKILL
    Private Sub NewSkillRecord()
        'commented by Shital on 18-Aug-2016
        mEmployeeSkill = EmployeeSkill.NewEmployeeSkill
        Session("mEmployeeSkill") = mEmployeeSkill
    End Sub
    Private Sub EditSkillRecord(ByVal mID As Guid)
        mEmployeeSkill = EmployeeSkill.GetEmployeeSkill(mID)
        Session("mEmployeeSkill") = mEmployeeSkill
    End Sub
    Private Sub DeleteSkillRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteSkill")
        mEmployeeSkill = EmployeeSkill.GetEmployeeSkill(mID)
        Session("mEmployeeSkill") = mEmployeeSkill
    End Sub
    '---END OF EMPLOYEE SKILL

    'EMPLOYEE TRAINING
    Private Sub NewTrainingRecord()
        mEmployeeTraining = EmployeeTraining.NewEmployeeTraining
        Session("mEmployeeTraining") = mEmployeeTraining
    End Sub
    Private Sub EditTrainingRecord(ByVal mID As Guid)
        mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)
        Session("mEmployeeTraining") = mEmployeeTraining
    End Sub
    Private Sub DeleteTrainingRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteTraining")
        mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)
        Session("mEmployeeTraining") = mEmployeeTraining
    End Sub
    '----END OF EMPLOYEE TRAINING

    'EMPLOYEE DOCUMENT
    Private Sub NewDocumentRecord()
        mEmployeeDocument = EmployeeDocument.NewEmployeeDocument
        Session("mEmployeeDocument") = mEmployeeDocument
        Session("IsRenew") = False
    End Sub
    Private Sub EditDocumentRecord(ByVal mID As Guid)
        mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
        Session("mEmployeeDocument") = mEmployeeDocument
    End Sub
    Private Sub DeleteDocumentRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteDocument")
        mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
        Session("mEmployeeDocument") = mEmployeeDocument
    End Sub
    '---END OF EMPLOYEE TRAINING

    'EMPLOYEE DESIGNATION
    Private Sub NewDesignationRecord()
        mEmployeeDesignation = EmployeeDesignation.NewEmployeeDesignation
        Session("mEmployeeDesignation") = mEmployeeDesignation
    End Sub
    Private Sub EditDesignationRecord(ByVal mID As Guid)
        mEmployeeDesignation = EmployeeDesignation.GetEmployeeDesignation(mID)
        Session("mEmployeeDesignation") = mEmployeeDesignation
    End Sub
    Private Sub DeleteDesignationRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteDesignation")
        mEmployeeDesignation = EmployeeDesignation.GetEmployeeDesignation(mID)
        Session("mEmployeeDesignation") = mEmployeeDesignation
    End Sub
    '-----END OF EMPLOYEE DESIGNATION 

    'EMPLOYEE NEXT TO KIN INFO
    Private Sub NewContactInfoRecord()
        mEmployeeContactInfo = EmployeeContactInfo.NewEmployeeContactInfo
        Session("mEmployeeContactInfo") = mEmployeeContactInfo
    End Sub
    Private Sub EditContactInfoRecord(ByVal mID As Guid)
        mEmployeeContactInfo = EmployeeContactInfo.GetEmployeeContactInfo(mID)
        Session("mEmployeeContactInfo") = mEmployeeContactInfo
    End Sub
    Private Sub DeleteContactInfoRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteContactInfo")
        mEmployeeContactInfo = EmployeeContactInfo.GetEmployeeContactInfo(mID)
        Session("mEmployeeContactInfo") = mEmployeeContactInfo
    End Sub
    '----END OF EMPLOYEE NEXT TO KIN INFO

    'EMPLOYEE DISCIPLINARY 
    Private Sub NewDisciplinaryRecord()
        mEmployeeDisciplinary = EmployeeDisciplinary.NewEmployeeDisciplinary
        Session("mEmployeeDisciplinary") = mEmployeeDisciplinary
    End Sub
    Private Sub EditDisciplinaryRecord(ByVal mID As Guid)
        mEmployeeDisciplinary = EmployeeDisciplinary.GetEmployeeDisciplinary(mID)
        Session("mEmployeeDisciplinary") = mEmployeeDisciplinary
    End Sub
    Private Sub DeleteDisciplinaryRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteDisciplinary")
        mEmployeeDisciplinary = EmployeeDisciplinary.GetEmployeeDisciplinary(mID)
        Session("mEmployeeDisciplinary") = mEmployeeDisciplinary
    End Sub
    '---END OF EMPLOYEE DISCIPLINARY

    'EMPLOYEE LEAVE 
    Private Sub NewLeaveRecord()
        mEmployeeLeave = EmployeeLeave.NewEmployeeLeave
        Session("mEmployeeLeave") = mEmployeeLeave
    End Sub
    Private Sub EditLeaveRecord(ByVal mID As Guid)
        mEmployeeLeave = EmployeeLeave.GetEmployeeLeave(mID)
        Session("mEmployeeLeave") = mEmployeeLeave
    End Sub
    Private Sub DeleteLeaveRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteLeave")
        mEmployeeLeave = EmployeeLeave.GetEmployeeLeave(mID)
        Session("mEmployeeLeave") = mEmployeeLeave
    End Sub
    '---END OF EMPLOYEE LEAVE


    'Company Equipment
    Private Sub NewCompanyEquipmentRecord()
        mCompanyEquipment = CompanyEquipment.NewCompanyEquipment
        Session("mCompanyEquipment") = mCompanyEquipment
    End Sub
    Private Sub EditCompanyEquipmentRecord(ByVal mID As Guid)
        mCompanyEquipment = CompanyEquipment.GetCompanyEquipment(mID)
        Session("mCompanyEquipment") = mCompanyEquipment
    End Sub
    Private Sub DeleteCompanyEquipmentRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteCompanyEquipmentRecord")
        mCompanyEquipment = CompanyEquipment.GetCompanyEquipment(mID)
        Session("mCompanyEquipment") = mCompanyEquipment
    End Sub
    '-----END OF COMPANY EQUIPMENT
    '************************************ Ajay End

    Private Sub SetGrid()

        Dim lnkDepartmentView As LinkButton 'ButtonColumn 

        Dim r As Integer   'Service

        Dim s As Integer   'Document
        Dim lnkDocumentView As LinkButton 'ButtonColumn 
        Dim lnkDocumentHistory As LinkButton
        Dim DocumentHistoryCount As Boolean
        Dim IsDocumentApplicable As Boolean
        Dim OneTimeDocument As Boolean = False
        For m As Integer = 0 To dgDocumentList.Rows.Count - 1
            s = CType(Me.dgDocumentList.Rows.Item(m).Cells(14).Text, Integer)
            DocumentHistoryCount = CType(Me.dgDocumentList.Rows.Item(m).Cells(16).Text, Boolean)
            IsDocumentApplicable = CType(Me.dgDocumentList.Rows.Item(m).Cells(17).Text, Boolean)
            OneTimeDocument = CType(Me.dgDocumentList.Rows.Item(m).Cells(18).Text, Boolean) 'Added by Prashant 0n 24-Nov-2020 ALL24112020

            If DocumentHistoryCount = False Then
                lnkDocumentHistory = CType(dgDocumentList.Rows.Item(m).Cells(15).FindControl("lnkDocumentHistory"), LinkButton)
                lnkDocumentHistory.Enabled = False
            End If
            If IsDocumentApplicable = False Then
                dgDocumentList.Rows(m).Cells(12).Enabled = False
            End If
            If OneTimeDocument = True Then 'Added by Prashant 0n 24-Nov-2020 ALL24112020
                dgDocumentList.Rows(m).Cells(12).Enabled = False 'Renew link
            End If
        Next

        Dim t As Boolean        'Training

        Dim TrainingHistoryCount, IsNotApplicable As Boolean
        For n As Integer = 0 To dgTrainingList.Rows.Count - 1

            TrainingHistoryCount = CType(Me.dgTrainingList.Rows(n).Cells(16).Text, Boolean)
            IsNotApplicable = CType(Me.dgTrainingList.Rows(n).Cells(17).Text, Boolean)

            If TrainingHistoryCount = False Then
                'lnkTrainingHistory = CType(dgTrainingList.Rows(n).Cells(15).FindControl("lnkTrainingHistory"), LinkButton)
                'lnkTrainingHistory.Enabled = False
                dgTrainingList.Rows(n).Cells(15).Enabled = False
            End If

            If IsNotApplicable = True Then
                dgTrainingList.Rows(n).Cells(12).Enabled = False 'Renew
            End If
        Next

        Dim v As Integer    'Disciplinary
        Dim lnkDisciplinaryView As LinkButton 'ButtonColumn 

        Dim w As Integer  'Leave
        Dim lnkLeaveView As LinkButton 'ButtonColumn 

    End Sub

    '********************************* Ajay
    Private Sub ControlEnability()

        'Employee Department
        If User.IsInRole("EmployeeDepartmentView") = False And
           User.IsInRole("EmployeeDepartmentPrint") = False And
           User.IsInRole("EmployeeDepartmentNew") = False And
           User.IsInRole("EmployeeDepartmentEdit") = False And
           User.IsInRole("EmployeeDepartmentDelete") = False Then

            pnlEmployeeDepartmentInfoList.Visible = False
            tabDepartment.Visible = False

        End If

        If User.IsInRole("EmployeeDepartmentNew") = False Then

            btnEmployeeDepartmentInfoList.Enabled = False
            btnEmployeeDepartmentInfoList.ToolTip = "You are not authorized user"

        End If
        'End Employee Department

        'Employee Next To Kin Info.
        If User.IsInRole("EmployeeNextToKinInfoView") = False And
           User.IsInRole("EmployeeNextToKinInfoPrint") = False And
           User.IsInRole("EmployeeNextToKinInfoNew") = False And
           User.IsInRole("EmployeeNextToKinInfoEdit") = False And
           User.IsInRole("EmployeeNextToKinInfoDelete") = False Then

            pnlContactInfoResult.Visible = False
            tabContactInfo.Visible = False

        End If

        If User.IsInRole("EmployeeNextToKinInfoNew") = False Then

            btnContactInfoAdd.Enabled = False
            btnContactInfoAdd.ToolTip = "You are not authorized user"

        End If
        'End Employee Next To Kin Info.

        'Employee Designation 
        If User.IsInRole("EmployeeDesignationView") = False And
           User.IsInRole("EmployeeDesignationPrint") = False And
           User.IsInRole("EmployeeDesignationNew") = False And
           User.IsInRole("EmployeeDesignationEdit") = False And
           User.IsInRole("EmployeeDesignationDelete") = False Then

            pnlDesignationResult.Visible = False
            tabDesignation.Visible = False

        End If

        If User.IsInRole("EmployeeDesignationNew") = False Then

            btnDesignationAdd.Enabled = False
            btnDesignationAdd.ToolTip = "You are not authorized user"

        End If
        'End Employee Designation

        'Employee Services   
        If User.IsInRole("EmployeeServicesView") = False And
           User.IsInRole("EmployeeServicesPrint") = False And
           User.IsInRole("EmployeeServicesNew") = False And
           User.IsInRole("EmployeeServicesEdit") = False And
           User.IsInRole("EmployeeServicesDelete") = False Then

            pnlServiceResult.Visible = False
            tabService.Visible = False

        End If

        If User.IsInRole("EmployeeServicesNew") = False Then

            btnServiceAdd.Enabled = False
            btnServiceAdd.ToolTip = "You are not authorized user"

        End If
        'End Employee Services

        'Employee Documents 
        If User.IsInRole("EmployeeDocumentsView") = False And
           User.IsInRole("EmployeeDocumentsPrint") = False And
           User.IsInRole("EmployeeDocumentsNew") = False And
           User.IsInRole("EmployeeDocumentsEdit") = False And
           User.IsInRole("EmployeeDocumentsDelete") = False Then

            pnlDocumentResult.Visible = False
            tabDocument.Visible = False

        End If

        If User.IsInRole("EmployeeDocumentsNew") = False Then

            btnDocumentAdd.Enabled = False
            btnDocumentAdd.ToolTip = "You are not authorized user"

        End If
        'End  Employee Documents 

        'Employee Training  
        If User.IsInRole("EmployeeTrainingView") = False And
           User.IsInRole("EmployeeTrainingPrint") = False And
           User.IsInRole("EmployeeTrainingNew") = False And
           User.IsInRole("EmployeeTrainingEdit") = False And
           User.IsInRole("EmployeeTrainingDelete") = False Then

            pnlTrainingResult.Visible = False
            tabTraining.Visible = False

        End If

        If User.IsInRole("EmployeeTrainingNew") = False Then

            btnTrainingAdd.Enabled = False
            btnTrainingAdd.ToolTip = "You are not authorized user"

        End If
        'End  Employee Training  

        'Employee Skill 
        If User.IsInRole("EmployeeSkillView") = False And
           User.IsInRole("EmployeeSkillPrint") = False And
           User.IsInRole("EmployeeSkillNew") = False And
           User.IsInRole("EmployeeSkillEdit") = False And
           User.IsInRole("EmployeeSkillDelete") = False Then

            pnlSkillResult.Visible = False
            tabSkill.Visible = False

        End If

        If User.IsInRole("EmployeeSkillNew") = False Then

            btnSkillAdd.Enabled = False
            btnSkillAdd.ToolTip = "You are not authorized user"

        End If
        'End  Employee Skill 

        'Employee Disciplinary  
        If User.IsInRole("EmployeeDisciplinaryView") = False And
           User.IsInRole("EmployeeDisciplinaryPrint") = False And
           User.IsInRole("EmployeeDisciplinaryNew") = False And
           User.IsInRole("EmployeeDisciplinaryEdit") = False And
           User.IsInRole("EmployeeDisciplinaryDelete") = False Then

            pnlDisciplinaryResult.Visible = False
            tabDisciplinary.Visible = False

        End If

        If User.IsInRole("EmployeeDisciplinaryNew") = False Then

            btnDisciplinaryAdd.Enabled = False
            btnDisciplinaryAdd.ToolTip = "You are not authorized user"

        End If
        'End  Employee Disciplinary 

        'Employee Leave 
        If User.IsInRole("EmployeeLeaveView") = False And
           User.IsInRole("EmployeeLeavePrint") = False And
           User.IsInRole("EmployeeLeaveNew") = False And
           User.IsInRole("EmployeeLeaveEdit") = False And
           User.IsInRole("EmployeeLeaveDelete") = False Then

            pnlLeaveResult.Visible = False
            tabLeaves.Visible = False

        End If

        If User.IsInRole("EmployeeLeaveNew") = False Then

            btnLeaveAdd.Enabled = False
            btnLeaveAdd.ToolTip = "You are not authorized user"

        End If
        'End  Employee Leave 

        'Company Equipment 
        If User.IsInRole("CompanyEquipmentView") = False And
           User.IsInRole("CompanyEquipmentPrint") = False And
           User.IsInRole("CompanyEquipmentNew") = False And
           User.IsInRole("CompanyEquipmentEdit") = False And
           User.IsInRole("CompanyEquipmentDelete") = False Then

            pnlCompanyEquipment.Visible = False
            tabCompanyEquipment.Visible = False

        End If

        If User.IsInRole("CompanyEquipmentNew") = False Then

            btnCompanyEquipment.Enabled = False
            btnCompanyEquipment.ToolTip = "You are not authorized user"

        End If
        'End  Company Equipment 

        'Added by Harsh Sugandhi on 15th July 2024 for FLYPAL-1728
        chkIsTechnicalCrew.Visible = IIf(AppSettings("ShowAMOOnlyForNewClients").ToLower = "true" AndAlso
                                          User.IsInRole("TechnicalCrewView"), True, False)

    End Sub
	'********************************* Ajay End

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()
		mEmployeeCityList = CityInvList.GetCityList(0, , , True)
		cmbCityList.DataSource = mEmployeeCityList
		cmbCurrCityList.DataSource = mEmployeeCityList
		Session("mEmployeeCityList") = mEmployeeCityList

		mGenderList = GenderList.GetGenderList(, "<SELECT>")
		cmbGenderList.DataSource = mGenderList
		Session("mGenderList") = mGenderList

		mContractorList = ContractorList.GetContractorList(, , , , "<SELECT>")
		cmbContractorList.DataSource = mContractorList
		Session("mContractorList") = mContractorList

		'mDesignationList = mDesignationList.GetDesignationList(, "<SELECT>")
		'cmbDesignationList.DataSource = mDesignationList
		'Session("mDesignationList") = mDesignationList

		txtDateOfLeaving.Text = mEmployee.DateOfLeavingFormatted.ToString
		ShowPicture()
		'ViewImage()
		'Added by Shital on 07-May-2020 for IND 
		mLocationList = LocationList.GetLocationList(0, , , , , , True)
		Session("mLocationList") = mLocationList
		cmbLocation.DataSource = mLocationList

		'************** Ajay 
		'Employee Department Info List
		mEmployeeDepartmentInfoList = EmployeeDepartmentInfoList.GetEmployeeDepartmentInfoList(mEmployee.ID)
		dgEmployeeDepartmentInfoList.DataSource = mEmployeeDepartmentInfoList

		dgEmployeeDepartmentInfoList.DataBind()
		lblDepartmentRecCount.Text = "Department (" + mEmployeeDepartmentInfoList.Count.ToString + ")"
		'----------

		'SERVICE LIST
		mEmployeeServiceList = EmployeeServiceList.GetEmployeeServiceList(mEmployee.ID)
		dgServiceList.DataSource = mEmployeeServiceList
		dgServiceList.DataBind()
		lblServiceRecCount.Text = "Service (" + mEmployeeServiceList.Count.ToString + ")"
		'----------

		'SKILL LIST
		mEmployeeSkillList = EmployeeSkillList.GetEmployeeSkillList(mEmployee.ID)
		dgSkillList.DataSource = mEmployeeSkillList
		dgSkillList.DataBind()
		lblSkillRecCount.Text = "Skill (" + mEmployeeSkillList.Count.ToString + ")"
		'----------

		'TRAINING LIST
		mEmployeeTrainingList = EmployeeTrainingList.GetEmployeeTrainingList(mEmployee.ID)
		dgTrainingList.DataSource = mEmployeeTrainingList
		Session("mEmployeeTrainingList") = mEmployeeTrainingList
		dgTrainingList.DataBind()
		lblTrainingRecCount.Text = "Training (" + mEmployeeTrainingList.Count.ToString + ")"
		'----------

		'DOCUMENT LIST
		mEmployeeDocumentList = EmployeeDocumentList.GetEmployeeDocumentList(mEmployee.ID)
		dgDocumentList.DataSource = mEmployeeDocumentList
		Session("mEmployeeDocumentList") = mEmployeeDocumentList
		dgDocumentList.DataBind()
		lblDocumentRecCount.Text = "Document (" + mEmployeeDocumentList.Count.ToString + ")"
		'----------

		'DESIGNATION LIST
		mEmployeeDesignationList = EmployeeDesignationList.GetEmployeeDesignationList(mEmployee.ID)
		dgDesignationList.DataSource = mEmployeeDesignationList
		dgDesignationList.DataBind()
		lblDesignationRecCount.Text = "Designation (" + mEmployeeDesignationList.Count.ToString + ")"
		'----------

		'NEXT tO kIN INFO LIST
		mEmployeeContactInfoList = EmployeeContactInfoList.GetEmployeeContactInfoList(mEmployee.ID)
		dgContactInfoList.DataSource = mEmployeeContactInfoList
		dgContactInfoList.DataBind()
		lblContactRecCount.Text = "Next To Kin Info (" + mEmployeeContactInfoList.Count.ToString + ")"
		'----------

		'DISCIPLINARY LIST
		mEmployeeDisciplinaryList = EmployeeDisciplinaryList.GetEmployeeDisciplinaryList(mEmployee.ID)
		dgDisciplinaryList.DataSource = mEmployeeDisciplinaryList
		dgDisciplinaryList.DataBind()
		lblDisciplinaryRecCount.Text = "Disciplinary (" + mEmployeeDisciplinaryList.Count.ToString + ")"
		'----------

		'Leave Record LIST
		mEmployeeLeaveList = EmployeeLeaveList.GetEmployeeLeaveList(mEmployee.ID)
		dgLeaveRecordList.DataSource = mEmployeeLeaveList
		dgLeaveRecordList.DataBind()
		lblLeaveRecCount.Text = "Leave Record (" + mEmployeeLeaveList.Count.ToString + ")"
		'----------

		'Company Equipment List
		mCompanyEquipmentList = CompanyEquipmentList.GetCompanyEquipmentList(mEmployee.ID)
		dgCompanyEquipmentList.DataSource = mCompanyEquipmentList
		dgCompanyEquipmentList.DataBind()
		lblEquipmentRecCount.Text = "Equipment (" & mCompanyEquipmentList.Count & ")"

		DataBind() 'CHK Bind TextBox Individually

		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
		Session("mCompanyDetail") = mCompanyDetail
	End Sub

	Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)

        If CustValid.ControlToValidate = "txtName" Then
            If Len(Trim(txtName.Text)) > 50 Then
                CustValid.ErrorMessage = "Employee Name too long "
                e.IsValid = False

            Else
                e.IsValid = True
            End If
        End If

        'If CustValid.ControlToValidate = "txtDay" Then
        '    If Val(txtDay.Text) <= 0 Then
        '        CustValid.ErrorMessage = "Day should not be Zero or less than zero."
        '        e.IsValid = False
        '    ElseIf Val(txtDay.Text) >= 32 Then
        '        CustValid.ErrorMessage = "Day should not be greater than 31."
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'End If

        'If CustValid.ControlToValidate = "txtMonth" Then
        '    If Val(txtMonth.Text) <= 0 Then
        '        CustValid.ErrorMessage = "Month should not be Zero or less than zero."
        '        e.IsValid = False
        '    ElseIf Val(txtMonth.Text) > 12 Then
        '        CustValid.ErrorMessage = "Month should not be greater than 12."
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'End If

        'If CustValid.ControlToValidate = "txtYear" Then
        '    If Len(txtYear.Text) < 4 And txtYear.Text <> "0" Then
        '        CustValid.ErrorMessage = "Year should not less than 4 digits."
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'End If


        If CustValid.ControlToValidate = "txtDay" Then
            If Val(txtDay.Text) < 0 Then
                CustValid.ErrorMessage = "Day should not be less than zero."
                e.IsValid = False
            ElseIf Val(txtDay.Text) >= 32 Then
                CustValid.ErrorMessage = "Day should not be greater than 31."
                e.IsValid = False
            ElseIf chkIsOthers.Checked = False And chkUseInFlightLog.Checked = False And chkIsTechnicalCrew.Checked = False Then
                If AppSettings("ClientCode") = "7AR" Then
                    If chkIsTechnicalCrew.Visible = True Then
                        CustValid.ErrorMessage = "Please select either technical staff or others."
                    Else
                        CustValid.ErrorMessage = "Please select others."
                    End If
                Else
                    If chkIsTechnicalCrew.Visible = True Then
                        CustValid.ErrorMessage = "Please select either flight crew, technical staff or others."
                    Else
                        CustValid.ErrorMessage = "Please select either flight crew or others."
                    End If
                End If
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If

        If CustValid.ControlToValidate = "txtMonth" Then
            If Val(txtMonth.Text) < 0 Then
                CustValid.ErrorMessage = "Month should not be Zero or less than zero."
                e.IsValid = False
            ElseIf Val(txtMonth.Text) > 12 Then
                CustValid.ErrorMessage = "Month should not be greater than 12."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If

        If CustValid.ControlToValidate = "txtYear" Then
            If Len(txtYear.Text) < 4 And txtYear.Text <> "0" Then
                CustValid.ErrorMessage = "Year should not less than 4 digits."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If

        If CustValid.ControlToValidate = "txtDateOfLeaving" Then
            If chkWorkingStatus.Checked = False And txtDateOfLeaving.Text = "" Then
                CustValid.ErrorMessage = "Date of Leaving should not be Blank."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub

    '*********************** Ajay 
#Region " Data Binding "
    Public Sub BindEmpDepartment()
        mEmployeeDepartmentInfoList = EmployeeDepartmentInfoList.GetEmployeeDepartmentInfoList(mEmployee.ID)
        dgEmployeeDepartmentInfoList.DataSource = mEmployeeDepartmentInfoList
        dgEmployeeDepartmentInfoList.DataBind()
        lblDepartmentRecCount.Text = "Department (" + mEmployeeDepartmentInfoList.Count.ToString + ")"

        Dim D As Integer   'Departmnet
        Dim lnkDepartmentView As LinkButton 'ButtonColumn 
        'For D1 As Integer = 0 To dgEmployeeDepartmentInfoList.Rows.Count - 1
        '    D = CType(Me.dgEmployeeDepartmentInfoList.Rows(D1).Cells(5).Text, Integer)
        '    If D <= 0 Then
        '        lnkDepartmentView = CType(dgEmployeeDepartmentInfoList.Rows(D1).Cells(6).FindControl("lnkDepartmentView"), LinkButton)
        '        lnkDepartmentView.Enabled = False
        '    End If
        'Next
        upnlEmployeeDetails.Update()
    End Sub
    Public Sub BindEmpDocument()
        mEmployeeDocumentList = EmployeeDocumentList.GetEmployeeDocumentList(mEmployee.ID)
        dgDocumentList.DataSource = mEmployeeDocumentList
        dgDocumentList.DataBind()
        Session("mEmployeeDocumentList") = mEmployeeDocumentList
        lblDocumentRecCount.Text = "Document (" + mEmployeeDocumentList.Count.ToString + ")"

        Dim s As Integer   'Document
        Dim lnkDocumentView As LinkButton 'ButtonColumn 
        Dim lnkDocumentHistory As LinkButton
        Dim DocumentHistoryCount As Boolean
        Dim IsDocumentApplicable As Boolean
        For m As Integer = 0 To dgDocumentList.Rows.Count - 1
            s = CType(Me.dgDocumentList.Rows.Item(m).Cells(14).Text, Integer)
            DocumentHistoryCount = CType(Me.dgDocumentList.Rows.Item(m).Cells(16).Text, Boolean)
            IsDocumentApplicable = CType(Me.dgDocumentList.Rows.Item(m).Cells(17).Text, Boolean)
            'If s <= 0 Then
            '    lnkDocumentView = CType(dgDocumentList.Rows.Item(m).Cells(15).FindControl("lnkDocumentView"), LinkButton)
            '    lnkDocumentView.Enabled = False
            'End If
            If DocumentHistoryCount = False Then
                lnkDocumentHistory = CType(dgDocumentList.Rows.Item(m).Cells(15).FindControl("lnkDocumentHistory"), LinkButton)
                lnkDocumentHistory.Enabled = False
            End If
            If IsDocumentApplicable = False Then
                dgDocumentList.Rows(m).Cells(12).Enabled = False
            End If
        Next
        upnlEmployeeDetails.Update()
    End Sub
    Public Sub BindEmpContactInfo()
        mEmployeeContactInfoList = EmployeeContactInfoList.GetEmployeeContactInfoList(mEmployee.ID)
        dgContactInfoList.DataSource = mEmployeeContactInfoList
        dgContactInfoList.DataBind()
        lblContactRecCount.Text = "Next To Kin Info (" + mEmployeeContactInfoList.Count.ToString + ")"

        'Dim P As Integer  'ContactInfo
        'Dim lnkContactInfoView As LinkButton 'ButtonColumn 
        'For j As Integer = 0 To dgContactInfoList.Rows.Count - 1
        '    P = CType(Me.dgContactInfoList.Rows.Item(j).Cells(14).Text, Integer)
        '    If P <= 0 Then
        '        lnkContactInfoView = CType(dgContactInfoList.Rows.Item(j).Cells(13).FindControl("lnkContactInfoView"), LinkButton)
        '        lnkContactInfoView.Enabled = False
        '    End If
        'Next
        upnlEmployeeDetails.Update()
    End Sub
    Public Sub BindDesignation()
        mEmployeeDesignationList = EmployeeDesignationList.GetEmployeeDesignationList(mEmployee.ID)
        dgDesignationList.DataSource = mEmployeeDesignationList
        dgDesignationList.DataBind()
        lblDesignationRecCount.Text = "Designation (" + mEmployeeDesignationList.Count.ToString + ")"

        'Dim q As Integer   'Designation
        'Dim lnkDesignationView As LinkButton 'ButtonColumn 
        'For k As Integer = 0 To dgDesignationList.Rows.Count - 1
        '    q = CType(Me.dgDesignationList.Rows.Item(k).Cells(8).Text, Integer)
        '    If q <= 0 Then
        '        lnkDesignationView = CType(dgDesignationList.Rows.Item(k).Cells(7).FindControl("lnkDesignationView"), LinkButton)
        '        lnkDesignationView.Enabled = False
        '    End If
        'Next
        upnlEmployeeDetails.Update()
    End Sub
    Public Sub BindEmpService()
        mEmployeeServiceList = EmployeeServiceList.GetEmployeeServiceList(mEmployee.ID)
        dgServiceList.DataSource = mEmployeeServiceList
        dgServiceList.DataBind()
        lblServiceRecCount.Text = "Service (" + mEmployeeServiceList.Count.ToString + ")"

        Dim r As Integer   'Service
        'Dim lnkServiceView As LinkButton 'ButtonColumn 
        'For l As Integer = 0 To dgServiceList.Rows.Count - 1
        '    r = CType(Me.dgServiceList.Rows.Item(l).Cells(6).Text, Integer)
        '    If r <= 0 Then
        '        lnkServiceView = CType(dgServiceList.Rows.Item(l).Cells(5).FindControl("lnkServiceView"), LinkButton)
        '        lnkServiceView.Enabled = False
        '    End If
        'Next
        upnlEmployeeDetails.Update()
    End Sub
    Public Sub BindEmpSkill()
        mEmployeeSkillList = EmployeeSkillList.GetEmployeeSkillList(mEmployee.ID)
        dgSkillList.DataSource = mEmployeeSkillList
        dgSkillList.DataBind()
        lblSkillRecCount.Text = "Skill (" + mEmployeeSkillList.Count.ToString + ")"

        'commented by Shital on 18-Aug-2016
        '
        'Dim u As Integer      'Skill
        'Dim lnkSkillView As LinkButton 'ButtonColumn 
        'For a As Integer = 0 To dgSkillList.Rows.Count - 1
        '    u = CType(Me.dgSkillList.Rows(a).Cells(8).Text, Integer)
        '    If u <= 0 Then
        '        lnkSkillView = CType(dgSkillList.Rows(a).Cells(7).FindControl("lnkSkillView"), LinkButton)
        '        lnkSkillView.Enabled = False
        '    End If
        'Next
        upnlEmployeeDetails.Update()
    End Sub
    Public Sub BindEmpTraining()
        mEmployeeTrainingList = EmployeeTrainingList.GetEmployeeTrainingList(mEmployee.ID)
        dgTrainingList.DataSource = mEmployeeTrainingList
        dgTrainingList.DataBind()
        Session("mEmployeeTrainingList") = mEmployeeTrainingList
        lblTrainingRecCount.Text = "Training (" + mEmployeeTrainingList.Count.ToString + ")" 'AJAX CHK

        Dim t As Boolean        'Training

        Dim TrainingHistoryCount, IsNotApplicable As Boolean
        For n As Integer = 0 To dgTrainingList.Rows.Count - 1

            TrainingHistoryCount = CType(Me.dgTrainingList.Rows(n).Cells(16).Text, Boolean)
            IsNotApplicable = CType(Me.dgTrainingList.Rows(n).Cells(17).Text, Boolean)

            If TrainingHistoryCount = False Then
                'lnkTrainingHistory = CType(dgTrainingList.Rows(n).Cells(15).FindControl("lnkTrainingHistory"), LinkButton)
                'lnkTrainingHistory.Enabled = False
                dgTrainingList.Rows(n).Cells(15).Enabled = False
            End If

            If IsNotApplicable = True Then
                dgTrainingList.Rows(n).Cells(12).Enabled = False 'Renew
            End If
        Next
        upnlEmployeeDetails.Update()
    End Sub
    Public Sub BindEmpDisciplinary()
        mEmployeeDisciplinaryList = EmployeeDisciplinaryList.GetEmployeeDisciplinaryList(mEmployee.ID)
        dgDisciplinaryList.DataSource = mEmployeeDisciplinaryList
        dgDisciplinaryList.DataBind()
        lblDisciplinaryRecCount.Text = "Disciplinary (" + mEmployeeDisciplinaryList.Count.ToString + ")"

        Dim v As Integer    'Disciplinary
        Dim lnkDisciplinaryView As LinkButton 'ButtonColumn 
        'For b As Integer = 0 To dgDisciplinaryList.Rows.Count - 1
        '    v = CType(Me.dgDisciplinaryList.Rows(b).Cells(10).Text, Integer)
        '    If v <= 0 Then
        '        lnkDisciplinaryView = CType(dgDisciplinaryList.Rows(b).Cells(9).FindControl("lnkDisciplinaryView"), LinkButton)
        '        lnkDisciplinaryView.Enabled = False
        '    End If
        'Next
        upnlEmployeeDetails.Update()
    End Sub
    Public Sub BindEmpLeaves()
        mEmployeeLeaveList = EmployeeLeaveList.GetEmployeeLeaveList(mEmployee.ID)
        dgLeaveRecordList.DataSource = mEmployeeLeaveList
        dgLeaveRecordList.DataBind()
        lblLeaveRecCount.Text = "Leave Record (" + mEmployeeLeaveList.Count.ToString + ")"

        Dim w As Integer  'Leave
        Dim lnkLeaveView As LinkButton 'ButtonColumn 
        'For c As Integer = 0 To dgLeaveRecordList.Rows.Count - 1
        '    w = CType(Me.dgLeaveRecordList.Rows(c).Cells(10).Text, Integer)
        '    If w <= 0 Then
        '        lnkLeaveView = CType(dgLeaveRecordList.Rows(c).Cells(9).FindControl("lnkLeaveView"), LinkButton)
        '        lnkLeaveView.Enabled = False
        '    End If
        'Next
        upnlEmployeeDetails.Update()
    End Sub
    Public Sub BindEmpEquipment()
        mCompanyEquipmentList = CompanyEquipmentList.GetCompanyEquipmentList(mEmployee.ID)
        dgCompanyEquipmentList.DataSource = mCompanyEquipmentList
        dgCompanyEquipmentList.DataBind()
        lblEquipmentRecCount.Text = "Equipment (" & mCompanyEquipmentList.Count & ")"

        upnlEmployeeDetails.Update()
    End Sub

#End Region

	'*********************** Ajay End
#End Region

#Region " Events "

	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'ClearAll()
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 22-July-2011
		If Not IsPostBack And Session("sender") = "" Then
			If txtEmpNo.Enabled = True Then
				setFocus(txtEmpNo)
			End If

			'If Session("MiddleFrame") <> "wfEmployee_Ajax.aspx" Then
			'    Session("MiddleFrame") = "wfEmployee_Ajax.aspx"
			'End If

			DataFieldBind()
			SetTitle()
			'SetDepartmentTextBox()
			ControlVisibility()
			ControlVisibilityForAttachment()
			'****** Ajay
			ControlEnability()
			SetGrid()
		End If
		SetDepartmentTextBox()
	End Sub

	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		If (Not User.IsInRole("EmployeeNew") And mEmployee.IsNew) Or (Not User.IsInRole("EmployeeEdit") And Not mEmployee.IsNew) Then
			SetObject()
			SetSession()
			MarkLog(Flypal.Util.Action.Save, "Employee", User.Identity.Name & " is not Authorized User to save " + mEmployee.EmpNoName, Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		If txtEmpNo.Text.Length > 50 Then

			MSGBoxCtrl.Show("Alert!", "Employee no. is too long ! ", "Employee no. is too long, Employee no. Should be less than 50 Characters", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If

		If txtName.Text.Length > 50 Then
			MSGBoxCtrl.Show("Alert!", "Employee name is too long ! ", "Employee name is too long, Employee name Should be less than 50 Characters", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		If Not IsValid Then
			upnlValidationSummary.Update()
			Exit Sub
		End If

		Try
			'Added By Prashant on 5-Aug-2013 ALL01082013
			Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeUsedStatus(mEmployee.ID.ToString, txtDateOfLeaving.Text)
			If mEmployeeStatus(0).Information <> "" Then
				Dim title As String = "Save Alert!"
				Dim message As String = "Working status of employee can not change on " + New SmartDate(txtDateOfLeaving.Text).FormattedText + ". As it is used in following transactions " + mEmployeeStatus(0).Information
				message = message.Replace(Environment.NewLine, "<br />")
				'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message), False)
				MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.Alert, message, MsgBoxStyle.OkOnly, "")
				Exit Sub
				'End
			Else
				Save()
				MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully, MSGBox.Message_Text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
			End If
			ControlVisibility()
			'Ajay 
			ControlEnability()
			upnlEmployeeDetails.Update()
		Catch ex As SqlException
			If ex.Number = 8145 Then
				'DataFieldBind()
				'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
				'msg1.ReplacePage = "wfEmployee_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
				'Session("sender") = "Delete"
				'msg1.Show()
				MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
			ElseIf ex.Number = 2627 Then
				'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
				'msg1.ReplacePage = "wfEmployee_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
				'Session("sender") = "Delete"
				'msg1.Show()
				MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
			ElseIf ex.Number = 547 Then
				'DataFieldBind()
				'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
				'msg1.ReplacePage = "wfEmployee_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
				'Session("sender") = "Delete"
				'msg1.Show()
				MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete, MSGBox.Message_Text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
			End If

		End Try
	End Sub

	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Response.Redirect("ImagetestPage.aspx")

        If Not mEmployee.IsNew Then
            MarkLog(Flypal.Util.Action.Close, "Employee", "Emp : " + mEmployee.EmpNoName, Flypal.Util.ErrorType.NoError, mEmployee.ID, EventLogID)
        End If
        Session("sender") = ""
        RemoveSession()

        '--------------CHANGED  BY VIKRANT-------------------
        'If Request.QueryString("BackPage1") = "wfLogDetail.aspx" Then
        '    Response.Redirect(Request.QueryString("BackPage1") & "?BackPage" & Request.QueryString("BackPage"))
        'Else
        '    Response.Redirect("Index.aspx")
        'End If


        If Request.QueryString("BackPage1") = "wfLogDetail.aspx" Or Request.QueryString("BackPage1") = "wfLogSOP.aspx" Then
            If path <> "" Then
                System.IO.File.Delete(path)
                path = String.Empty
                Session("path") = path
            End If
            Response.Redirect(Request.QueryString("BackPage1") & "?BackPage" & Request.QueryString("BackPage"))
        Else
            If path <> "" Then
                System.IO.File.Delete(path)
                path = String.Empty
                Session("path") = path
            End If
            Response.Redirect("Index.aspx")
        End If
        '---------------------------------------------------
    End Sub

	Private Sub cmbCityList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCityList.SelectedIndexChanged
		txtState.Text = IIf(cmbCityList.SelectedIndex > 0, mEmployeeCityList(cmbCityList.SelectedIndex).State, "")
		txtCountry.Text = IIf(cmbCityList.SelectedIndex > 0, mEmployeeCityList(cmbCityList.SelectedIndex).Country, "")
		If cmbCityList.Enabled = True Then
			setFocus(cmbCityList)
		End If
	End Sub

	Private Sub cmbCurrCityList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCurrCityList.SelectedIndexChanged
		txtCurrState.Text = IIf(cmbCurrCityList.SelectedIndex > 0, mEmployeeCityList(cmbCurrCityList.SelectedIndex).State, "")
		txtCurrCountry.Text = IIf(cmbCurrCityList.SelectedIndex > 0, mEmployeeCityList(cmbCurrCityList.SelectedIndex).Country, "")
		If cmbCurrCityList.Enabled = True Then
			setFocus(cmbCurrCityList)
		End If
	End Sub

	Private Sub btnEmpDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmpDetails.Click
		If IsValid Then
			If Not mEmployee Is Nothing Then
				'Session("mEmployee") = mEmployee
				'Response.Redirect("wfEmployeeDetails.aspx")
				SetObject()
				SetSession()
				Response.Redirect("wfEmployeeDetails_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Childpage=wfEmployee_Ajax.aspx")
			End If
		Else
			upnlValidationSummary.Update()
		End If

	End Sub

	Private Sub imgCity_Click(ByVal sender As System.Object, e As System.Web.UI.ImageClickEventArgs) Handles imgCity.Click
		SetObject() 'Added Code
		SetSession()
		Session("NewEmployee") = "True"
		'Response.Redirect("wfCityInv_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage3=wfEmployee_Ajax.aspx")
	End Sub

	Private Sub btnContractor_Click(ByVal sender As System.Object, e As System.Web.UI.ImageClickEventArgs) Handles btnContractor.Click
		SetObject() 'Added Code
		SetSession()
		Session("NewEmployee") = "True"
		'Response.Redirect("wfContractor.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfEmployee_Ajax.aspx")
	End Sub

	Private Sub chkWorkingStatus_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWorkingStatus.CheckedChanged
		If chkWorkingStatus.Checked = True Then
			txtDateOfLeaving.Text = ""
		End If
		ControlVisibility()
	End Sub

	Private Sub chkSameAddress_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSameAddress.CheckedChanged
        If chkSameAddress.Checked = True Then
            txtCurrAddress1.Text = Trim(txtAddress1.Text)
            txtCurrAddress2.Text = Trim(txtAddress2.Text)

            cmbCurrCityList.SelectedValue = cmbCityList.SelectedValue.ToString
            txtCurrPointOfOrigin.Text = Trim(txtPointOfOrigin.Text)
            txtCurrState.Text = Trim(txtState.Text)
            txtCurrCountry.Text = Trim(txtCountry.Text)
            txtCurrPhoneNo.Text = Trim(txtPhoneNo.Text)
            txtCurrMobile.Text = Trim(txtMobile.Text)
            txtCurrEmail.Text = Trim(txtEmail.Text)
            txtCurrZip.Text = Trim(txtZip.Text) 'Ajay Added 30-10-2023
        End If
    End Sub

	Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
		Dim fileSize1 As Integer = 0
		Dim file1(fileSize1) As Byte
		mEmployee.ImageFile = file1
		mEmployee.ImageSize = 0
		'ImageButton1.Visible = False
		btnDelAttach.Enabled = False
		'MyImage.Visible = False
		MyImage.ImageUrl = ""
		ShowPicture()
		upnlAttachment.Update()
	End Sub

	Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
		SetReport()
	End Sub

	Private Sub hdnimgBtnContractor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnContractor.Click
		mContractorList = ContractorList.GetContractorList(, , , , "<SELECT>")
		cmbContractorList.DataSource = mContractorList
		cmbContractorList.DataBind()
		Session("mContractorList") = mContractorList
		upnlOtherDetails.Update()

		'Added to update city list if it is updatet from Contractor Master Page
		mEmployeeCityList = CityInvList.GetCityList(0, , , True)
		cmbCityList.DataSource = mEmployeeCityList
		cmbCityList.DataBind()
		cmbCurrCityList.DataSource = mEmployeeCityList
		cmbCurrCityList.DataBind()
		Session("mEmployeeCityList") = mEmployeeCityList
		upnlCurrentContactDetails.Update()
		upnlPermContactDetails.Update()
	End Sub

	Private Sub HdnBtnFileUpload_Click(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click

		Try

			AttachMyFile()
			ShowPicture()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub hdnimgBtnCity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnCity.Click
		mEmployeeCityList = CityInvList.GetCityList(0, , , True)
		cmbCityList.DataSource = mEmployeeCityList
		cmbCityList.DataBind()
		cmbCurrCityList.DataSource = mEmployeeCityList
		cmbCurrCityList.DataBind()
		Session("mEmployeeCityList") = mEmployeeCityList
		upnlCurrentContactDetails.Update()
		upnlPermContactDetails.Update()
	End Sub

	Private Sub UploadDigitalSignature(sender As Object, e As EventArgs) Handles btnSelectDigitalSignature.Click

		Try

			If mEmployee.IsDigitalSignatureAdded Then
				mFileAttach = FileAttach.GetAttachment(ReferenceID:=mEmployee.ID, FileName:="DigitalSignature")
			Else

				If IsDigitalSignatureDeleted Then

					If Not mEmployee.IsNew Then

						mFileAttach = FileAttach.GetAttachment(ReferenceID:=mEmployee.ID, FileName:="DigitalSignature")

						If Not mFileAttach IsNot Nothing Then

							Dim FileSize As Integer = 0
							Dim ImageFile(FileSize) As Byte

							mFileAttach.ImageFile = ImageFile
							mFileAttach.Size = 0

							GoTo CodeBlock

						End If

					End If

				End If
				mFileAttach = FileAttach.NewAttachment(ReferenceID:=mEmployee.ID, FileName:="DigitalSignature")
			End If

CodeBlock:
			Session("mFileAttach") = mFileAttach
			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Open Digital-Signature",
												"OpenDigitalSignature();",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub RemoveDigitalSignature(sender As Object, e As EventArgs) Handles btnDelDigitalAttach.Click

		Dim FileSize As Integer = 0
		Dim ImageFile(FileSize) As Byte
		Try

			If mEmployee.IsDigitalSignatureAdded And mFileAttach Is Nothing Then
				mFileAttach = FileAttach.GetAttachment(ReferenceID:=mEmployee.ID, FileName:="DigitalSignature")
			End If

			mFileAttach.ImageFile = ImageFile
			mFileAttach.Size = 0

			btnDelDigitalAttach.Enabled = False
			imgMyDigitalSignature.ImageUrl = ""
			IsDigitalSignatureDeleted = True
			mEmployee.IsDigitalSignatureAdded = False
			Session("IsDigitalSignatureDeleted") = IsDigitalSignatureDeleted
			Session("mFileAttach") = mFileAttach
			Session("mEmployee") = mEmployee

			MSGBoxCtrl.Show("Alert",
							"Digital Signature Removed successfully.",
							"",
							MsgBoxStyle.OkOnly,
							"")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Grid and Events "

	Private Sub imgLocation_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgLocation.Click
        Dim str As String
        str = "OpenLocation('wfStoreLocation_Ajax.aspx?BackPage1=wfEmployee_Ajax.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type") & "');"

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenLocation", str, True)
    End Sub

	'************ Ajay
	Private Sub imgDepartment_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgDepartment.Click
        tabEmployeeDetailsContainer.ActiveTab = tabEmployeeDetailsContainer.Tabs(1)
    End Sub

	Private Sub imgDesignationName_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgDesignationName.Click
		tabEmployeeDetailsContainer.ActiveTab = tabEmployeeDetailsContainer.Tabs(3)
	End Sub

	'EMPLOYEE SKILL
	Private Sub btnSkillAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSkillAdd.Click
        If (Not User.IsInRole("EmployeeSkillNew")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        SetSession()
        NewSkillRecord()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpSkillWindow", "OpenEmpSkillWindow()", True)
        'Response.Redirect("wfEmployeeSkill.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
    End Sub

	Private Sub dgSkillList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSkillList.RowCommand
		Dim Idx As Int32
		Dim mID As Guid
		Select Case e.CommandName
			Case "EditRec"
				Idx = CInt(e.CommandArgument) + dgSkillList.PageIndex * dgSkillList.PageSize
				mID = CType(dgSkillList.DataKeys(CInt(e.CommandArgument)).Value, Guid)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeSkillEdit") = False Then
					SetSession()
					MarkLog(Util.Action.Edit, "Employee Skill", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'*******************************

				EditSkillRecord(mID)
				MarkLog(Flypal.Util.Action.Edit, "Employee Skill", "Emp : " + mEmployee.EmpNoName + " Skill : " + mEmployeeSkill.SkillName, Flypal.Util.ErrorType.NoError, mEmployeeSkill.ID, EventLogID)
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpSkillWindow", "OpenEmpSkillWindow()", True)
				'Response.Redirect("wfEmployeeSkill.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")

			Case "DeleteRec"
				Idx = CInt(e.CommandArgument) + dgSkillList.PageIndex * dgSkillList.PageSize
				mID = CType(dgSkillList.DataKeys(CInt(e.CommandArgument)).Value, Guid)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeSkillDelete") = False Then
					SetSession()
					MarkLog(Util.Action.Delete, "Employee Skill", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'*******************************
				DeleteSkillRecord(mID)
			Case "View"
				'----------------------------------------------------------------------
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				'----------------------------------------------------------------------
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Dim rowIndex As Integer = gvr.RowIndex
				Idx = rowIndex + dgSkillList.PageIndex * dgSkillList.PageSize

				mID = CType(dgSkillList.DataKeys(rowIndex).Value, Guid)
				mEmployeeSkill = EmployeeSkill.GetEmployeeSkill(mID)
				If mEmployeeSkill.ImageSize > 0 Then
					'Dim path As String = AppSettings("FilePath") & "\" & StrName & mCalibrationItemChild.FileExtension
					Dim path As String = AppSettings("DOCPath") & StrName & mEmployeeSkill.FileExtension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeSkill.FileExtension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mEmployeeSkill.ImageFile, 0, mEmployeeSkill.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
					End If
				Else
					MSGBoxCtrl.Show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
				End If
		End Select
	End Sub
	'----END OF EMPLOYEE SKILL

	'EMPLOYEE SERVICE
	Private Sub btnServiceAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnServiceAdd.Click

        If (Not User.IsInRole("EmployeeServicesNew")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        SetSession()
        NewServiceRecord()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpServiceWindow", "OpenEmpServiceWindow()", True)
        'Response.Redirect("wfEmployeeService.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
    End Sub

	Private Sub dgServiceList_EditCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgServiceList.RowCommand
		Dim Idx As Int32
		Dim mID As Guid

		Select Case e.CommandName
			Case "EditRec"
				Idx = CInt(e.CommandArgument) + dgServiceList.PageIndex * dgServiceList.PageSize
				mID = CType(dgServiceList.DataKeys(CInt(e.CommandArgument)).Value, Guid)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeServicesEdit") = False Then
					SetSession()
					MarkLog(Util.Action.Edit, "Employee Service", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'*******************************
				EditServiceRecord(mID)
				MarkLog(Flypal.Util.Action.Edit, "Employee Service", "Emp : " + mEmployee.EmpNoName + " Service : " + mEmployeeService.ServiceName, Flypal.Util.ErrorType.NoError, mEmployeeService.ID, EventLogID)
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpServiceWindow", "OpenEmpServiceWindow()", True)
				'Response.Redirect("wfEmployeeService.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
			Case "DeleteRec"
				Idx = CInt(e.CommandArgument) + dgServiceList.PageIndex * dgServiceList.PageSize
				mID = CType(dgServiceList.DataKeys(CInt(e.CommandArgument)).Value, Guid)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeServicesDelete") = False Then
					SetSession()
					MarkLog(Util.Action.Delete, "Employee Service", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'*******************************
				DeleteServiceRecord(mID)
			Case "View"
				'----------------------------------------------------------------------
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				'----------------------------------------------------------------------
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Dim rowIndex As Integer = gvr.RowIndex
				Idx = rowIndex + dgServiceList.PageIndex * dgServiceList.PageSize

				mID = CType(dgServiceList.DataKeys(rowIndex).Value, Guid)


				mEmployeeService = EmployeeService.GetEmployeeService(mID)
				If mEmployeeService.ImageSize > 0 Then
					'Dim path As String = AppSettings("FilePath") & "\" & StrName & mCalibrationItemChild.FileExtension
					Dim path As String = AppSettings("DOCPath") & StrName & mEmployeeService.FileExtension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeService.FileExtension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mEmployeeService.ImageFile, 0, mEmployeeService.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
					End If
				Else
					MSGBoxCtrl.Show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
				End If
		End Select
	End Sub
	'-----END OF EMPLOYEE SERVICE

	'EMPLOYEE TRAINING
	Private Sub btnTrainingAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrainingAdd.Click

		If (Not User.IsInRole("EmployeeTrainingNew")) Then
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		SetSession()
		'NewTrainingRecord()
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTrainingGroupWindow", "OpenTrainingGroupWindow()", True)
		'Response.Redirect("wfEmployeeTraining.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
	End Sub

	Private Sub dgTrainingList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTrainingList.RowCommand
		Dim Idx As Int32
		Dim mID As New Guid
		'Dim mName As String
		Select Case e.CommandName
			Case "EditRec"
				Idx = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
				mID = New Guid(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeTrainingEdit") = False Then
					SetSession()
					MarkLog(Util.Action.Edit, "Employee Training", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				'*******************************
				EditTrainingRecord(mID)
				Session("IsRenew") = False
				MarkLog(Flypal.Util.Action.Edit, "Employee Training", "Emp : " + mEmployee.EmpNoName + " Training : " + mEmployeeTraining.TrainingName, Flypal.Util.ErrorType.NoError, mEmployeeTraining.ID, EventLogID)
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpTrainingWindow", "OpenEmpTrainingWindow()", True)
				'Response.Redirect("wfEmployeeTraining.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
			Case "DeleteRec"
				Idx = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
				mID = New Guid(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeTrainingDelete") = False Then
					SetSession()
					MarkLog(Util.Action.Delete, "Employee Training", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				'*******************************
				DeleteTrainingRecord(mID)
			Case "View"

				Idx = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
				mID = New Guid(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)

				Dim mFileAttach As FileAttach
				mFileAttach = FileAttach.GetAttachment(ReferenceID:=mID)
				Session("mFileAttach") = mFileAttach

				If mFileAttach.Size > 0 Then
					AttachmentHelper.DownloadAttachmentWithName(AttachmentObject:=mFileAttach)
				Else
					MSGBoxCtrl.Show("Attachment!",
									"No Attachment present.",
									"",
									MsgBoxStyle.OkOnly,
									"")
				End If

			Case "Renew"

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeTrainingEdit") = False Then
					SetSession()
					MarkLog(Action.Edit, "Employee Training", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				'*******************************
				Idx = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
				mID = New Guid(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)

				mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)

				mTraining = Training.GetTraining(mEmployeeTraining.TrainingID)
				mFreqInMonths = mTraining.FreqInMonths

				SetSession()

				mEmployeeTraining = EmployeeTraining.NewRenew(mEmployeeTraining, mFreqInMonths, True)

				Session("mEmployeeTraining") = mEmployeeTraining
				Session("IsRenew") = True
				Session.Remove("mFileAttach")
				MarkLog(Flypal.Util.Action.Comply, "Employee Training", "Emp : " + mEmployee.EmpNoName + " Training : " + mEmployeeTraining.TrainingName, Flypal.Util.ErrorType.NoError, mEmployeeTraining.ID, EventLogID)
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpTrainingWindow", "OpenEmpTrainingWindow()", True)
				'Response.Redirect("wfEmployeeTraining.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
			Case "History"
				' Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				'Dim rowIndex As Integer = gvr.RowIndex
				Idx = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
				mID = New Guid(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)

				mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)
				Dim mEmployeeID As Guid = CType(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("EmployeeID"), Guid)
				Session("mEmployeeID") = mEmployeeID.ToString
				Session("mEmployeeTraining") = mEmployeeTraining
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpTrainingHistoryWindow", "OpenEmpTrainingHistoryWindow()", True)
				'Response.Redirect("wfEmployeeTrainingHistoryList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx" & "&EmployeeID=" & mEmployeeID.ToString & "&TrainingID=" & mEmployeeTraining.TrainingID.ToString & "&ReferenceID=" & mEmployeeTraining.ReferenceID.ToString)
		End Select
	End Sub
	'-----END OF EMPLOYEE TRAINING

	'EMPLOYEE DOCUMENT
	Private Sub btnDocumentAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDocumentAdd.Click
        If (Not User.IsInRole("EmployeeDocumentsNew")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        SetSession()
        NewDocumentRecord()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDocumentWindow", "OpenEmpDocumentWindow()", True)
        'Response.Redirect("wfEmployeeDocument.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
    End Sub

	Private Sub dgDocumentList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDocumentList.RowCommand
		Dim Idx As Int32
		Dim mID As New Guid
		Select Case e.CommandName
			Case "EditRec"
				Idx = CInt(e.CommandArgument) + dgDocumentList.PageIndex * dgDocumentList.PageSize
				mID = New Guid(dgDocumentList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeDocumentsEdit") = False Then
					SetSession()
					MarkLog(Util.Action.Edit, "Employee Document", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				'*******************************
				EditDocumentRecord(mID)
				Session("IsRenew") = False
				MarkLog(Flypal.Util.Action.Edit, "Employee Document", "Emp : " + mEmployee.EmpNoName + " Document : " + mEmployeeDocument.DocumentName, Flypal.Util.ErrorType.NoError, mEmployeeDocument.ID, EventLogID)
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDocumentWindow", "OpenEmpDocumentWindow()", True)
				'Response.Redirect("wfEmployeeDocument.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
			Case "DeleteRec"
				Idx = CInt(e.CommandArgument) + dgDocumentList.PageIndex * dgDocumentList.PageSize
				mID = New Guid(dgDocumentList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeDocumentsDelete") = False Then
					SetSession()
					MarkLog(Util.Action.Delete, "Employee Document", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				'*******************************
				DeleteDocumentRecord(mID)
			Case "View"
				'----------------------------------------------------------------------
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				'----------------------------------------------------------------------
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Dim rowIndex As Integer = gvr.RowIndex
				Idx = rowIndex + dgDocumentList.PageIndex * dgDocumentList.PageSize
				mID = New Guid(dgDocumentList.DataKeys(rowIndex).Values("ID").ToString)

				mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
				If mEmployeeDocument.ImageSize > 0 Then
					'Dim path As String = AppSettings("FilePath") & "\" & StrName & mCalibrationItemChild.FileExtension
					Dim path As String = AppSettings("DOCPath") & StrName & mEmployeeDocument.FileExtension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeDocument.FileExtension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mEmployeeDocument.ImageFile, 0, mEmployeeDocument.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
					End If
				Else
					MSGBoxCtrl.Show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
				End If
				'New addition by Amrita for Document Renewal
			Case "Renew"

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeDocumentsEdit") = False Then
					SetSession()
					MarkLog(Util.Action.Edit, "Employee Document", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				'*******************************
				Idx = CInt(e.CommandArgument) + dgDocumentList.PageIndex * dgDocumentList.PageSize
				mID = New Guid(dgDocumentList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)
				mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
				SetSession()
				'NewDocumentRecord()
				mEmployeeDocument = EmployeeDocument.NewRenew(mEmployeeDocument, True)
				Session("IsRenew") = True
				Session("mEmployeeDocument") = mEmployeeDocument
				Session.Remove("mFileAttach")
				MarkLog(Flypal.Util.Action.Comply, "Employee Document", "Emp : " + mEmployee.EmpNoName + " Document : " + mEmployeeDocument.DocumentName, Flypal.Util.ErrorType.NoError, mEmployeeDocument.ID, EventLogID)
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDocumentWindow", "OpenEmpDocumentWindow()", True)
				'Response.Redirect("wfEmployeeDocument.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
			Case "History"
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Dim rowIndex As Integer = gvr.RowIndex
				Idx = rowIndex + dgDocumentList.PageIndex * dgDocumentList.PageSize
				mID = New Guid(dgDocumentList.DataKeys(rowIndex).Values("ID").ToString)

				mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
				Session("mEmployeeDocument") = mEmployeeDocument
				Dim mEmployeeID As Guid = New Guid(dgDocumentList.DataKeys(rowIndex).Values("EmployeeID").ToString)
				Session("mEmployeeID") = mEmployeeID.ToString
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDocumentHistoryWindow", "OpenEmpDocumentHistoryWindow()", True)
				'Response.Redirect("wfEmployeeDocumentHistoryList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx" & "&EmployeeID=" & mEmployeeID.ToString & "&DocumentID=" & mEmployeeDocument.DocumentID.ToString & "&ReferenceID=" & mEmployeeDocument.ReferenceID.ToString)
		End Select
	End Sub
	'------END OF EMPLOYEE DOCUMENT

	'EMPLOYEE DESIGNATION
	Private Sub btnDesignationAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesignationAdd.Click
        SetSession()
        NewDesignationRecord()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDesgWindow", "OpenEmpDesgWindow()", True)
        'Response.Redirect("wfEmployeeDesignation.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
    End Sub

	Private Sub dgDesignationList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDesignationList.RowCommand
		Dim Idx As Int32
		Dim mID As New Guid
		Dim mName As String

		Select Case e.CommandName
			Case "EditRec"
				Idx = CInt(e.CommandArgument) + dgDesignationList.PageIndex * dgDesignationList.PageSize
				mID = CType(dgDesignationList.DataKeys(CInt(e.CommandArgument)).Values("ID"), Guid)
				mName = CType(dgDesignationList.DataKeys(CInt(e.CommandArgument)).Values("DesignationName"), String)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeDesignationEdit") = False Then
					SetSession()
					MarkLog(Util.Action.Edit, "Employee Designation", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'*******************************
				EditDesignationRecord(mID)
				'' MarkLog(Flypal.Util.Action.Edit, "Employee", mEmployee.EmpNo, Flypal.Util.ErrorType.NoError, mEmployee.ID,EventLogID.ToString)
				MarkLog(Flypal.Util.Action.Edit, "Employee Designation", "Emp : " + mEmployee.EmpNoName + " Designation : " + mEmployeeDesignation.DesignationName, Flypal.Util.ErrorType.NoError, mEmployeeDesignation.ID, EventLogID)
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDesgWindow", "OpenEmpDesgWindow()", True)
				'Response.Redirect("wfEmployeeDesignation.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
			Case "DeleteRec"
				Idx = CInt(e.CommandArgument) + dgDesignationList.PageIndex * dgDesignationList.PageSize
				mID = CType(dgDesignationList.DataKeys(CInt(e.CommandArgument)).Values("ID"), Guid)
				mName = CType(dgDesignationList.DataKeys(CInt(e.CommandArgument)).Values("DesignationName"), String)
				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeDesignationDelete") = False Then
					SetSession()
					MarkLog(Util.Action.Delete, "Employee Designation", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'*******************************
				DeleteDesignationRecord(mID)
			Case "View"
				'----------------------------------------------------------------------
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				'----------------------------------------------------------------------

				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Dim rowIndex As Integer = gvr.RowIndex
				Idx = rowIndex + dgDesignationList.PageIndex * dgDesignationList.PageSize
				mID = CType(dgDesignationList.DataKeys(rowIndex).Values("ID"), Guid)

				mEmployeeDesignation = EmployeeDesignation.GetEmployeeDesignation(mID)
				If mEmployeeDesignation.ImageSize > 0 Then
					'Dim path As String = AppSettings("FilePath") & "\" & StrName & mCalibrationItemChild.FileExtension
					Dim path As String = AppSettings("DOCPath") & StrName & mEmployeeDesignation.FileExtension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeDesignation.FileExtension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mEmployeeDesignation.ImageFile, 0, mEmployeeDesignation.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
					End If
				Else
					MSGBoxCtrl.Show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
				End If
		End Select
	End Sub
	'------END OF EMPLOYEE DESIGNATION

	'EMPLOYEE NEXT TO KIN INFO
	Private Sub btnContactInfoAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContactInfoAdd.Click

		If (Not User.IsInRole("EmployeeNextToKinInfoNew")) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If


		SetSession()
		NewContactInfoRecord()
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpContactInfoWindow", "OpenEmpContactInfoWindow()", True)
		'Response.Redirect("wfEmployeeContactInfo.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
	End Sub
	'-----END OF EMPLOYEE NEXT TO KIN INFO

	'EMPLOYEE DISCIPLINARY
	Private Sub btnDisciplinaryAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisciplinaryAdd.Click
        If (Not User.IsInRole("EmployeeDisciplinaryNew")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If


        SetSession()
        NewDisciplinaryRecord()
        'Response.Redirect("wfEmployeeSkill.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDisciplinaryWindow", "OpenEmpDisciplinaryWindow()", True)
        'Response.Redirect("wfEmployeeDisciplinary.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
    End Sub

	Private Sub dgDisciplinaryList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDisciplinaryList.RowCommand
		Dim Idx As Int32
		Dim mID As New Guid
		Select Case e.CommandName
			Case "EditRec"
				Idx = CInt(e.CommandArgument) + dgDisciplinaryList.PageIndex * dgDisciplinaryList.PageSize
				mID = New Guid(dgDisciplinaryList.DataKeys(CInt(e.CommandArgument)).Value.ToString)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeDisciplinaryEdit") = False Then
					SetSession()
					MarkLog(Util.Action.Edit, "Employee Disciplinary", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'*******************************
				EditDisciplinaryRecord(mID)
				MarkLog(Flypal.Util.Action.Edit, "Employee Disciplinary", "Emp : " + mEmployee.EmpNoName + " Disciplinary : " + mEmployeeDisciplinary.Description, Flypal.Util.ErrorType.NoError, mEmployeeDisciplinary.ID, EventLogID)
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDisciplinaryWindow", "OpenEmpDisciplinaryWindow()", True)
				'Response.Redirect("wfEmployeeDisciplinary.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
			Case "DeleteRec"
				Idx = CInt(e.CommandArgument) + dgDisciplinaryList.PageIndex * dgDisciplinaryList.PageSize
				mID = New Guid(dgDisciplinaryList.DataKeys(CInt(e.CommandArgument)).Value.ToString)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeDisciplinaryDelete") = False Then
					SetSession()
					MarkLog(Util.Action.Delete, "Employee Disciplinary", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'*******************************
				DeleteDisciplinaryRecord(mID)
			Case "View"
				'----------------------------------------------------------------------
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				'----------------------------------------------------------------------
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Dim rowIndex As Integer = gvr.RowIndex
				Idx = rowIndex + dgDisciplinaryList.PageIndex * dgDisciplinaryList.PageSize
				mID = New Guid(dgDisciplinaryList.DataKeys(rowIndex).Value.ToString)

				mEmployeeDisciplinary = EmployeeDisciplinary.GetEmployeeDisciplinary(mID)
				If mEmployeeDisciplinary.ImageSize > 0 Then
					'Dim path As String = AppSettings("FilePath") & "\" & StrName & mCalibrationItemChild.FileExtension
					Dim path As String = AppSettings("DOCPath") & StrName & mEmployeeDisciplinary.FileExtension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeDisciplinary.FileExtension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mEmployeeDisciplinary.ImageFile, 0, mEmployeeDisciplinary.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
					End If
				Else
					MSGBoxCtrl.Show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
				End If
		End Select
	End Sub
	'-----END OF EMPLOYEE DISCIPLINARY

	'EMPLOYEE LEAVE
	Private Sub btnLeaveAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeaveAdd.Click
		'
		If (Not User.IsInRole("EmployeeLeaveNew")) Then
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		SetSession()
		NewLeaveRecord()
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpLeaveWindow", "OpenEmpLeaveWindow()", True)
		'Response.Redirect("wfEmployeeLeaves.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
	End Sub

	Private Sub dgLeaveRecordList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLeaveRecordList.RowCommand
		Dim Idx As Int32
		Dim mID As New Guid
		Select Case e.CommandName
			Case "EditRec"
				Idx = CInt(e.CommandArgument) + dgLeaveRecordList.PageIndex * dgLeaveRecordList.PageSize
				mID = CType(dgLeaveRecordList.DataKeys(CInt(e.CommandArgument)).Value, Guid)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeLeaveEdit") = False Then
					SetSession()
					MarkLog(Util.Action.Edit, "Employee Leave", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'*******************************
				EditLeaveRecord(mID)
				MarkLog(Flypal.Util.Action.Edit, "Employee Leave Records", "Emp : " + mEmployee.EmpNoName + " Leave Records : " + mEmployeeLeave.ClassificationName, Flypal.Util.ErrorType.NoError, mEmployeeLeave.ID, EventLogID)
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpLeaveWindow", "OpenEmpLeaveWindow()", True)
				'Response.Redirect("wfEmployeeLeaves.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
			Case "DeleteRec"
				Idx = CInt(e.CommandArgument) + dgLeaveRecordList.PageIndex * dgLeaveRecordList.PageSize
				mID = CType(dgLeaveRecordList.DataKeys(CInt(e.CommandArgument)).Value, Guid)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeLeaveDelete") = False Then
					SetSession()
					MarkLog(Util.Action.Delete, "Employee Leave", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'*******************************
				DeleteLeaveRecord(mID)
			Case "View"
				'----------------------------------------------------------------------
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				'----------------------------------------------------------------------
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Dim rowIndex As Integer = gvr.RowIndex
				Idx = rowIndex + dgLeaveRecordList.PageIndex * dgLeaveRecordList.PageSize
				mID = CType(dgLeaveRecordList.DataKeys(rowIndex).Value, Guid)

				mEmployeeLeave = EmployeeLeave.GetEmployeeLeave(mID)
				If mEmployeeLeave.ImageSize > 0 Then
					'Dim path As String = AppSettings("FilePath") & "\" & StrName & mCalibrationItemChild.FileExtension
					Dim path As String = AppSettings("DOCPath") & StrName & mEmployeeLeave.FileExtension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeLeave.FileExtension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mEmployeeLeave.ImageFile, 0, mEmployeeLeave.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
					End If
				Else
					MSGBoxCtrl.Show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
				End If
		End Select
	End Sub

	'Employee Department Info List
	Private Sub btnEmployeeDepartmentInfoList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmployeeDepartmentInfoList.Click
        SetSession()
        NewEmployeeDepartmentInfoRecord()

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDeptWindow", "OpenEmpDeptWindow()", True)
        'Response.Redirect("wfEmployeeDepartmentInfo.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
    End Sub

	Private Sub dgContactInfoList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgContactInfoList.RowCommand
		Dim Idx As Int32
		Dim mID As New Guid
		'Dim mName As String
		Select Case e.CommandName
			Case "EditRec"
				Idx = CInt(e.CommandArgument) + dgContactInfoList.PageIndex * dgContactInfoList.PageSize
				mID = New Guid(dgContactInfoList.DataKeys(CInt(e.CommandArgument)).Value.ToString)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeNextToKinInfoEdit") = False Then
					SetSession()
					MarkLog(Util.Action.Edit, "Employee Next To Kin Info", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.Information, "") 'CHK
					Exit Sub
				End If
				'*******************************
				EditContactInfoRecord(mID)

				MarkLog(Flypal.Util.Action.Edit, "Employee Next To Kin Info", "Emp: " + mEmployee.EmpNoName + " Next To Kin Info : " + mEmployeeContactInfo.Name, Flypal.Util.ErrorType.NoError, mEmployeeContactInfo.ID, EventLogID)
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpContactInfoWindow", "OpenEmpContactInfoWindow()", True)
				'Response.Redirect("wfEmployeeContactInfo.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
			Case "DeleteRec"
				Idx = CInt(e.CommandArgument) + dgContactInfoList.PageIndex * dgContactInfoList.PageSize
				mID = New Guid(dgContactInfoList.DataKeys(CInt(e.CommandArgument)).Value.ToString)


				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeNextToKinInfoDelete") = False Then
					SetSession()
					MarkLog(Util.Action.Delete, "Employee Next To Kin Info", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.Information, "") 'CHK
					Exit Sub
				End If
				'*******************************
				DeleteContactInfoRecord(mID)
			Case "View"
				'----------------------------------------------------------------------
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString

				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Dim rowIndex As Integer = gvr.RowIndex
				Idx = rowIndex + dgContactInfoList.PageIndex * dgContactInfoList.PageSize
				mID = New Guid(dgContactInfoList.DataKeys(rowIndex).Value.ToString)

				mEmployeeContactInfo = EmployeeContactInfo.GetEmployeeContactInfo(mID)
				If mEmployeeContactInfo.ImageSize > 0 Then
					'Dim path As String = AppSettings("FilePath") & "\" & StrName & mCalibrationItemChild.FileExtension
					Dim path As String = AppSettings("DOCPath") & StrName & mEmployeeContactInfo.FileExtension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeContactInfo.FileExtension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mEmployeeContactInfo.ImageFile, 0, mEmployeeContactInfo.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
					End If
				Else
					MSGBoxCtrl.Show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
				End If
		End Select
	End Sub

	Private Sub dgEmployeeDepartmentInfoList_EditCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEmployeeDepartmentInfoList.RowCommand
		Dim Idx As Int32
		Dim mID As New Guid
		Select Case e.CommandName
			Case "EditRec"
				Idx = CInt(e.CommandArgument) + dgEmployeeDepartmentInfoList.PageIndex * dgEmployeeDepartmentInfoList.PageSize
				mID = New Guid(dgEmployeeDepartmentInfoList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeDepartmentEdit") = False Then
					SetSession()
					MarkLog(Util.Action.Edit, "Employee Department", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "You are not authorized user", True)
					Exit Sub
				End If
				'*******************************
				EditEmployeeDepartmentInfoRecord(mID)
				MarkLog(Flypal.Util.Action.Edit, "Employee Department", "Emp : " + mEmployee.EmpNoName + " Department : " + mEmployeeDepartmentInfo.EmployeeDepartmentName, Flypal.Util.ErrorType.NoError, mEmployeeDepartmentInfo.ID, EventLogID)
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDeptWindow", "OpenEmpDeptWindow()", True)
				'Response.Redirect("wfEmployeeDepartmentInfo.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
			Case "DeleteRec"
				Idx = CInt(e.CommandArgument) + dgEmployeeDepartmentInfoList.PageIndex * dgEmployeeDepartmentInfoList.PageSize
				mID = New Guid(dgEmployeeDepartmentInfoList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
				'If (Not User.IsInRole("EmployeeDelete")) Then
				'    SetSession()
				'    MarkLog(Flypal.Util.Action.Delete, "Employee Department", User.Identity.Name & " is not Authorized User to delete " & "Emp: " + mEmployee.EmpNoName + " Service : " & mName, Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
				'    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
				'    msg.ReplacePage = "wfEmployeeList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
				'    Session("sender") = "Authorization"
				'    msg.Show()
				'    Exit Sub
				'End If
				'Added By Prashant On 17-July-2012
				If User.IsInRole("EmployeeDepartmentDelete") = False Then
					SetSession()
					MarkLog(Util.Action.Delete, "Employee Department", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "You are not authorized user", True)
					Exit Sub
				End If
				'*******************************
				DeleteEmployeeDepartmentInfoRecord(mID)
			Case "View"
				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Dim rowIndex As Integer = gvr.RowIndex
				Idx = rowIndex + dgEmployeeDepartmentInfoList.PageIndex * dgEmployeeDepartmentInfoList.PageSize
				mID = New Guid(dgEmployeeDepartmentInfoList.DataKeys(rowIndex).Value.ToString)
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				mEmployeeDepartmentInfo = EmployeeDepartmentInfo.GetEmployeeDepartmentInfo(mID)
				If mEmployeeDepartmentInfo.ImageSize > 0 Then
					Dim path As String = AppSettings("DOCPath") & StrName & mEmployeeDepartmentInfo.FileExtension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeDepartmentInfo.FileExtension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mEmployeeDepartmentInfo.ImageFile, 0, mEmployeeDepartmentInfo.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
					End If
				Else
					MSGBoxCtrl.Show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
				End If
		End Select
	End Sub
	'------END OF EMPLOYEE DEPARTMENT

	'Company Equipment
	Private Sub btnCompanyEquipment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompanyEquipment.Click
        SetSession()
        NewCompanyEquipmentRecord()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpEquipmentWindow", "OpenEmpEquipmentWindow()", True)
        'Response.Redirect("wfCompanyEquipment.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
    End Sub

	Private Sub dgCompanyEquipmentList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCompanyEquipmentList.RowCommand
		Dim Idx As Int32
		Dim mID As New Guid

		Select Case e.CommandName
			Case "EditRec"
				Idx = CInt(e.CommandArgument) + dgCompanyEquipmentList.PageIndex * dgCompanyEquipmentList.PageSize
				mID = New Guid(dgCompanyEquipmentList.DataKeys(CInt(e.CommandArgument)).Value.ToString)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("CompanyEquipmentEdit") = False Then
					SetSession()
					MarkLog(Util.Action.Edit, "Company Equipment", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'*******************************
				EditCompanyEquipmentRecord(mID)
				MarkLog(Flypal.Util.Action.Edit, "Company Equipment", "Emp : " + mEmployee.EmpNoName + " Equipment : " + mCompanyEquipment.EquipmentName, Flypal.Util.ErrorType.NoError, mCompanyEquipment.ID, EventLogID)
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpEquipmentWindow", "OpenEmpEquipmentWindow()", True)
				'Response.Redirect("wfCompanyEquipment.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
			Case "DeleteRec"
				Idx = CInt(e.CommandArgument) + dgCompanyEquipmentList.PageIndex * dgCompanyEquipmentList.PageSize
				mID = New Guid(dgCompanyEquipmentList.DataKeys(CInt(e.CommandArgument)).Value.ToString)

				'Added By Prashant On 17-July-2012
				If User.IsInRole("CompanyEquipmentDelete") = False Then
					SetSession()
					MarkLog(Util.Action.Delete, "Company Equipment", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				'*******************************
				DeleteCompanyEquipmentRecord(mID)
		End Select
	End Sub
	'------END OF Company Equipment

	Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

#End Region

#Region " Hidden Button Events "

	Private Sub hdnBtnEmpDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpDept.Click
		BindEmpDepartment()
		upnlDepartment.Update()
	End Sub
	Private Sub hdnBtnEmpContactInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpContactInfo.Click
        BindEmpContactInfo()
        upnlContactInfo1.Update()
    End Sub
    Private Sub hdnBtnEmpDesg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpDesg.Click
        BindDesignation()
        upnlDesignation.Update()
    End Sub
    Private Sub hdnBtnEmpService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpService.Click
        BindEmpService()
        upnlService.Update()
    End Sub
    Private Sub hdnBtnEmpDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpDocument.Click
        BindEmpDocument()
        SetGrid()
        upnlDocument.Update()
    End Sub
    Private Sub hdnBtnEmpTraining_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpTraining.Click
        BindEmpTraining()
        Session("MiddleFrame") = "wfEmployeeList_Ajax.aspx"
        SetGrid()
        upnlTraining.Update()
    End Sub
    Private Sub hdnBtnEmpSkill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpSkill.Click
        BindEmpSkill()
        upnlSkill.Update()
    End Sub
    Private Sub hdnBtnEmpDisciplinary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpDisciplinary.Click
        BindEmpDisciplinary()
        upnlDisciplinary.Update()
    End Sub
    Private Sub hdnBtnEmpLeave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpLeave.Click
        BindEmpLeaves()
        upnlLeaves.Update()
    End Sub
    Private Sub hdnBtnEmpCompanyEquipment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpCompanyEquipment.Click
        BindEmpEquipment()
        upnlCompanyEquipment.Update()
    End Sub
	'****************** Ajay End

#End Region

#Region " Report "
	Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        'Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsEmployeeDetails
        Dim myReport = New crEmployeeDetails

        mEmployee = CType(Session("mEmployee"), Employee)
        mEmployeeSkillList = EmployeeSkillList.GetEmployeeSkillList(mEmployee.ID)
        mEmployeeServiceList = EmployeeServiceList.GetEmployeeServiceList(mEmployee.ID)
        mEmployeeTrainingList = EmployeeTrainingList.GetEmployeeTrainingList(mEmployee.ID)
        mEmployeeDocumentList = EmployeeDocumentList.GetEmployeeDocumentList(mEmployee.ID)
        mEmployeeDesignationList = EmployeeDesignationList.GetEmployeeDesignationList(mEmployee.ID)
        mEmployeeContactInfoList = EmployeeContactInfoList.GetEmployeeContactInfoList(mEmployee.ID)
        mEmployeeDepartmentInfoList = EmployeeDepartmentInfoList.GetEmployeeDepartmentInfoList(mEmployee.ID)
        mCompanyEquipmentList = CompanyEquipmentList.GetCompanyEquipmentList(mEmployee.ID)

        Dim Top1Department As String
        Dim mEmployeeDepartmentTop1Info As EmployeeDepartmentInfoList
        mEmployeeDepartmentTop1Info = EmployeeDepartmentInfoList.GetEmployeeDepartmentTop1Info(mEmployee.ID, "", "1/1/1900", "1/1/2200", "", "", False, True)
        If mEmployeeDepartmentTop1Info.Count > 0 Then
            Top1Department = mEmployeeDepartmentTop1Info(0).EmployeeDepartmentName
        Else
            Top1Department = ""
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim ReportData As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Employee Details Report", Top1Department, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mEmployeeSkillList.Count <= 0 And mEmployeeServiceList.Count <= 0 And mEmployeeTrainingList.Count <= 0 And mEmployeeDocumentList.Count <= 0 And mEmployeeDesignationList.Count <= 0 And mEmployeeContactInfoList.Count <= 0 And mEmployeeDepartmentInfoList.Count <= 0 And mCompanyEquipmentList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        Dim mEmployeePhoto As EmployeePhoto = EmployeePhoto.GetImage(ds, mEmployee.ID.ToString)
        da.Fill(ds, mEmployee)
        da.Fill(ds, mEmployeeSkillList)
        da.Fill(ds, mEmployeeServiceList)
        da.Fill(ds, mEmployeeTrainingList)
        da.Fill(ds, mEmployeeDocumentList)
        da.Fill(ds, mEmployeeDesignationList)
        da.Fill(ds, mEmployeeContactInfoList)
        da.Fill(ds, mEmployeeDepartmentInfoList)
        da.Fill(ds, mCompanyEquipmentList)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mEmployeePhoto)
        da.Fill(ds, ReportData)

        myReport.SetDataSource(ds)
        With myReport
            If mEmployeeDepartmentInfoList.Count = 0 Then
                .Section15.SectionFormat.EnableSuppress = True
            ElseIf User.IsInRole("EmployeeDepartmentView") = False And User.IsInRole("EmployeeDepartmentPrint") = False And User.IsInRole("EmployeeDepartmentNew") = False And User.IsInRole("EmployeeDepartmentEdit") = False And User.IsInRole("EmployeeDepartmentDelete") = False Then
                .Section15.SectionFormat.EnableSuppress = True
            End If
            If mEmployeeSkillList.Count = 0 Then
                .Section3.SectionFormat.EnableSuppress = True
            ElseIf User.IsInRole("EmployeeSkillView") = False And User.IsInRole("EmployeeSkillPrint") = False And User.IsInRole("EmployeeSkillNew") = False And User.IsInRole("EmployeeSkillEdit") = False And User.IsInRole("EmployeeSkillDelete") = False Then
                .Section3.SectionFormat.EnableSuppress = True
            End If
            If mEmployeeServiceList.Count = 0 Then
                .Section6.SectionFormat.EnableSuppress = True
            ElseIf User.IsInRole("EmployeeServicesView") = False And User.IsInRole("EmployeeServicesPrint") = False And User.IsInRole("EmployeeServicesNew") = False And User.IsInRole("EmployeeServicesEdit") = False And User.IsInRole("EmployeeServicesDelete") = False Then
                .Section6.SectionFormat.EnableSuppress = True
            End If
            If mEmployeeTrainingList.Count = 0 Then
                .Section10.SectionFormat.EnableSuppress = True
            ElseIf User.IsInRole("EmployeeTrainingView") = False And User.IsInRole("EmployeeTrainingPrint") = False And User.IsInRole("EmployeeTrainingNew") = False And User.IsInRole("EmployeeTrainingEdit") = False And User.IsInRole("EmployeeTrainingDelete") = False Then
                .Section10.SectionFormat.EnableSuppress = True
            End If
            If mEmployeeDocumentList.Count = 0 Then
                .Section11.SectionFormat.EnableSuppress = True
            ElseIf User.IsInRole("EmployeeDocumentsView") = False And User.IsInRole("EmployeeDocumentsPrint") = False And User.IsInRole("EmployeeDocumentsNew") = False And User.IsInRole("EmployeeDocumentsEdit") = False And User.IsInRole("EmployeeDocumentsDelete") = False Then
                .Section11.SectionFormat.EnableSuppress = True
            End If
            If mEmployeeDesignationList.Count = 0 Then
                .Section12.SectionFormat.EnableSuppress = True
            ElseIf User.IsInRole("EmployeeDesignationView") = False And User.IsInRole("EmployeeDesignationPrint") = False And User.IsInRole("EmployeeDesignationNew") = False And User.IsInRole("EmployeeDesignationEdit") = False And User.IsInRole("EmployeeDesignationDelete") = False Then
                .Section12.SectionFormat.EnableSuppress = True
            End If
            If mEmployeeContactInfoList.Count = 0 Then
                .Section14.SectionFormat.EnableSuppress = True
            ElseIf User.IsInRole("EmployeeNextToKinInfoView") = False And User.IsInRole("EmployeeNextToKinInfoPrint") = False And User.IsInRole("EmployeeNextToKinInfoNew") = False And User.IsInRole("EmployeeNextToKinInfoEdit") = False And User.IsInRole("EmployeeNextToKinInfoDelete") = False Then
                .Section14.SectionFormat.EnableSuppress = True
            End If
            If mCompanyEquipmentList.Count = 0 Then
                .Section16.SectionFormat.EnableSuppress = True
            ElseIf User.IsInRole("CompanyEquipmentView") = False And User.IsInRole("CompanyEquipmentPrint") = False And User.IsInRole("CompanyEquipmentNew") = False And User.IsInRole("CompanyEquipmentEdit") = False And User.IsInRole("CompanyEquipmentDelete") = False Then
                .Section16.SectionFormat.EnableSuppress = True
            End If
        End With
        Session("CrystalReport") = myReport

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

End Class