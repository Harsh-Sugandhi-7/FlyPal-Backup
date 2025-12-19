'AJAX Conversion By Vikrant

Partial Class wfEmployeeDetails_Ajax
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents pnlAdvancedSearch As System.Web.UI.WebControls.Panel


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Public mEmployee As Employee
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

    Dim EventLogID As Guid 'Added by Saylee on 19-July-2011

    'COMPANY EQUIPMENTT INFO  'Added By Prashant 16-July-2012
    Public mCompanyEquipment As CompanyEquipment
    Public mCompanyEquipmentList As CompanyEquipmentList
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mEmployee = CType(Session("mEmployee"), Employee)
    End Sub
    Private Sub SetSession()
        Session("mEmployee") = mEmployee
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEmployeeList")
        Session.Remove("Type")
        Session.Remove("Text")
        Session.Remove("Index")
    End Sub
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
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0

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
                                MSGBoxCtrl.show("Delete Alert!", ex.Message, "", MsgBoxStyle.OkOnly, "")
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
                                MSGBoxCtrl.show("Delete Alert!", ex.Message, "", MsgBoxStyle.OkOnly, "")
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
                                MSGBoxCtrl.show("Delete Alert!", ex.Message, "", MsgBoxStyle.OkOnly, "")
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
    End Sub
    Private Sub SetGrid()  'Added By Utkarsh On 4-May-2011
        Dim lnkDepartmentView As LinkButton 'ButtonColumn 
        For D1 As Integer = 0 To dgEmployeeDepartmentInfoList.Rows.Count - 1

            Dim result As Integer
            Dim IsAttachment As Boolean = Int32.TryParse(dgEmployeeDepartmentInfoList.Rows(D1).Cells(7).Text, result)
            'If Not IsAttachment Then
            If result <= 0 Then
                lnkDepartmentView = CType(dgEmployeeDepartmentInfoList.Rows.Item(D1).Cells(6).FindControl("lnkDepartmentView"), LinkButton)
                lnkDepartmentView.Enabled = False
            End If
            'End If
        Next
        Dim P As Integer  'ContactInfo
        Dim lnkContactInfoView As LinkButton 'ButtonColumn 
        For j As Integer = 0 To dgContactInfoList.Rows.Count - 1
            P = CType(Me.dgContactInfoList.Rows.Item(j).Cells(14).Text, Integer)
            If P <= 0 Then
                lnkContactInfoView = CType(dgContactInfoList.Rows.Item(j).Cells(13).FindControl("lnkContactInfoView"), LinkButton) 'CHK and do changes to EmpDept if working
                lnkContactInfoView.Enabled = False
            End If
        Next

        Dim q As Integer   'Designation
        Dim lnkDesignationView As LinkButton 'ButtonColumn 
        For k As Integer = 0 To dgDesignationList.Rows.Count - 1
            q = CType(Me.dgDesignationList.Rows.Item(k).Cells(8).Text, Integer)
            If q <= 0 Then
                lnkDesignationView = CType(dgDesignationList.Rows.Item(k).Cells(7).FindControl("lnkDesignationView"), LinkButton)
                lnkDesignationView.Enabled = False
            End If
        Next
        Dim r As Integer   'Service
        Dim lnkServiceView As LinkButton 'ButtonColumn 
        For l As Integer = 0 To dgServiceList.Rows.Count - 1
            r = CType(Me.dgServiceList.Rows.Item(l).Cells(6).Text, Integer)
            If r <= 0 Then
                lnkServiceView = CType(dgServiceList.Rows.Item(l).Cells(5).FindControl("lnkServiceView"), LinkButton)
                lnkServiceView.Enabled = False
            End If
        Next

        Dim s As Integer   'Document
        Dim lnkDocumentView As LinkButton 'ButtonColumn 
        Dim lnkDocumentHistory As LinkButton
        Dim DocumentHistoryCount As Boolean
        Dim IsDocumentApplicable As Boolean
        Dim OneTimeDocument As Boolean = False
        For m As Integer = 0 To dgDocumentList.Rows.Count - 1
            s = CType(Me.dgDocumentList.Rows.Item(m).Cells(16).Text, Integer)
            DocumentHistoryCount = CType(Me.dgDocumentList.Rows.Item(m).Cells(18).Text, Boolean)
            IsDocumentApplicable = CType(Me.dgDocumentList.Rows.Item(m).Cells(19).Text, Boolean)
            OneTimeDocument = CType(Me.dgDocumentList.Rows.Item(m).Cells(20).Text, Boolean) 'Added by Prashant 0n 24-Nov-2020 ALL24112020
            If s <= 0 Then
                lnkDocumentView = CType(dgDocumentList.Rows.Item(m).Cells(15).FindControl("lnkDocumentView"), LinkButton)
                lnkDocumentView.Enabled = False
            End If
            If DocumentHistoryCount = False Then
                lnkDocumentHistory = CType(dgDocumentList.Rows.Item(m).Cells(17).FindControl("lnkDocumentHistory"), LinkButton)
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
            t = CType(Me.dgTrainingList.Rows(n).Cells(16).Text, Boolean)
            TrainingHistoryCount = CType(Me.dgTrainingList.Rows(n).Cells(18).Text, Boolean)
            IsNotApplicable = CType(Me.dgTrainingList.Rows(n).Cells(19).Text, Boolean)
            If t = False Then
                'lnkTrainingView = CType(dgTrainingList.Rows(n).Cells(13).FindControl("lnkTrainingView"), LinkButton)
                'lnkTrainingView.Enabled = False
                dgTrainingList.Rows(n).Cells(15).Enabled = False
            End If
            If TrainingHistoryCount = False Then
                'lnkTrainingHistory = CType(dgTrainingList.Rows(n).Cells(15).FindControl("lnkTrainingHistory"), LinkButton)
                'lnkTrainingHistory.Enabled = False
                dgTrainingList.Rows(n).Cells(17).Enabled = False
            End If

            If IsNotApplicable = True Then
                dgTrainingList.Rows(n).Cells(12).Enabled = False 'Renew
            End If
        Next

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

        Dim v As Integer    'Disciplinary
        Dim lnkDisciplinaryView As LinkButton 'ButtonColumn 
        For b As Integer = 0 To dgDisciplinaryList.Rows.Count - 1
            v = CType(Me.dgDisciplinaryList.Rows(b).Cells(10).Text, Integer)
            If v <= 0 Then
                lnkDisciplinaryView = CType(dgDisciplinaryList.Rows(b).Cells(9).FindControl("lnkDisciplinaryView"), LinkButton)
                lnkDisciplinaryView.Enabled = False
            End If
        Next
        Dim w As Integer  'Leave
        Dim lnkLeaveView As LinkButton 'ButtonColumn 
        For c As Integer = 0 To dgLeaveRecordList.Rows.Count - 1
            w = CType(Me.dgLeaveRecordList.Rows(c).Cells(10).Text, Integer)
            If w <= 0 Then
                lnkLeaveView = CType(dgLeaveRecordList.Rows(c).Cells(9).FindControl("lnkLeaveView"), LinkButton)
                lnkLeaveView.Enabled = False
            End If
        Next
    End Sub
    Private Sub ControlEnability()
        'Employee Department
        If User.IsInRole("EmployeeDepartmentView") = False And User.IsInRole("EmployeeDepartmentPrint") = False And User.IsInRole("EmployeeDepartmentNew") = False And User.IsInRole("EmployeeDepartmentEdit") = False And User.IsInRole("EmployeeDepartmentDelete") = False Then
            pnlEmployeeDepartmentInfoList.Visible = False
        End If
        If User.IsInRole("EmployeeDepartmentNew") = False Then
            btnEmployeeDepartmentInfoList.Enabled = False
            btnEmployeeDepartmentInfoList.ToolTip = "You are not authorized user"
        End If
        'End Employee Department

        'Employee Next To Kin Info.
        If User.IsInRole("EmployeeNextToKinInfoView") = False And User.IsInRole("EmployeeNextToKinInfoPrint") = False And User.IsInRole("EmployeeNextToKinInfoNew") = False And User.IsInRole("EmployeeNextToKinInfoEdit") = False And User.IsInRole("EmployeeNextToKinInfoDelete") = False Then
            pnlContactInfoResult.Visible = False
        End If
        If User.IsInRole("EmployeeNextToKinInfoNew") = False Then
            btnContactInfoAdd.Enabled = False
            btnContactInfoAdd.ToolTip = "You are not authorized user"
        End If
        'End Employee Next To Kin Info.

        'Employee Designation 
        If User.IsInRole("EmployeeDesignationView") = False And User.IsInRole("EmployeeDesignationPrint") = False And User.IsInRole("EmployeeDesignationNew") = False And User.IsInRole("EmployeeDesignationEdit") = False And User.IsInRole("EmployeeDesignationDelete") = False Then
            pnlDesignationResult.Visible = False
        End If
        If User.IsInRole("EmployeeDesignationNew") = False Then
            btnDesignationAdd.Enabled = False
            btnDesignationAdd.ToolTip = "You are not authorized user"
        End If
        'End Employee Designation

        'Employee Services   
        If User.IsInRole("EmployeeServicesView") = False And User.IsInRole("EmployeeServicesPrint") = False And User.IsInRole("EmployeeServicesNew") = False And User.IsInRole("EmployeeServicesEdit") = False And User.IsInRole("EmployeeServicesDelete") = False Then
            pnlServiceResult.Visible = False
        End If
        If User.IsInRole("EmployeeServicesNew") = False Then
            btnServiceAdd.Enabled = False
            btnServiceAdd.ToolTip = "You are not authorized user"
        End If
        'End Employee Services

        'Employee Documents 
        If User.IsInRole("EmployeeDocumentsView") = False And User.IsInRole("EmployeeDocumentsPrint") = False And User.IsInRole("EmployeeDocumentsNew") = False And User.IsInRole("EmployeeDocumentsEdit") = False And User.IsInRole("EmployeeDocumentsDelete") = False Then
            pnlDocumentResult.Visible = False
        End If
        If User.IsInRole("EmployeeDocumentsNew") = False Then
            btnDocumentAdd.Enabled = False
            btnDocumentAdd.ToolTip = "You are not authorized user"
        End If
        'End  Employee Documents 

        'Employee Training  
        If User.IsInRole("EmployeeTrainingView") = False And User.IsInRole("EmployeeTrainingPrint") = False And User.IsInRole("EmployeeTrainingNew") = False And User.IsInRole("EmployeeTrainingEdit") = False And User.IsInRole("EmployeeTrainingDelete") = False Then
            pnlTrainingResult.Visible = False
        End If
        If User.IsInRole("EmployeeTrainingNew") = False Then
            btnTrainingAdd.Enabled = False
            btnTrainingAdd.ToolTip = "You are not authorized user"
        End If
        'End  Employee Training  

        'Employee Skill 
        If User.IsInRole("EmployeeSkillView") = False And User.IsInRole("EmployeeSkillPrint") = False And User.IsInRole("EmployeeSkillNew") = False And User.IsInRole("EmployeeSkillEdit") = False And User.IsInRole("EmployeeSkillDelete") = False Then
            pnlSkillResult.Visible = False
        End If
        If User.IsInRole("EmployeeSkillNew") = False Then
            btnSkillAdd.Enabled = False
            btnSkillAdd.ToolTip = "You are not authorized user"
        End If
        'End  Employee Skill 

        'Employee Disciplinary  
        If User.IsInRole("EmployeeDisciplinaryView") = False And User.IsInRole("EmployeeDisciplinaryPrint") = False And User.IsInRole("EmployeeDisciplinaryNew") = False And User.IsInRole("EmployeeDisciplinaryEdit") = False And User.IsInRole("EmployeeDisciplinaryDelete") = False Then
            pnlDisciplinaryResult.Visible = False
        End If
        If User.IsInRole("EmployeeDisciplinaryNew") = False Then
            btnDisciplinaryAdd.Enabled = False
            btnDisciplinaryAdd.ToolTip = "You are not authorized user"
        End If
        'End  Employee Disciplinary 

        'Employee Leave 
        If User.IsInRole("EmployeeLeaveView") = False And User.IsInRole("EmployeeLeavePrint") = False And User.IsInRole("EmployeeLeaveNew") = False And User.IsInRole("EmployeeLeaveEdit") = False And User.IsInRole("EmployeeLeaveDelete") = False Then
            pnlLeaveResult.Visible = False
        End If
        If User.IsInRole("EmployeeLeaveNew") = False Then
            btnLeaveAdd.Enabled = False
            btnLeaveAdd.ToolTip = "You are not authorized user"
        End If
        'End  Employee Leave 

        'Company Equipment 
        If User.IsInRole("CompanyEquipmentView") = False And User.IsInRole("CompanyEquipmentPrint") = False And User.IsInRole("CompanyEquipmentNew") = False And User.IsInRole("CompanyEquipmentEdit") = False And User.IsInRole("CompanyEquipmentDelete") = False Then
            pnlCompanyEquipment.Visible = False
        End If
        If User.IsInRole("CompanyEquipmentNew") = False Then
            btnCompanyEquipment.Enabled = False
            btnCompanyEquipment.ToolTip = "You are not authorized user"
        End If
        'End  Company Equipment 
    End Sub
#End Region

#Region " Data Binding "
    Public Sub BindEmpDepartment()
        mEmployeeDepartmentInfoList = EmployeeDepartmentInfoList.GetEmployeeDepartmentInfoList(mEmployee.ID)
        dgEmployeeDepartmentInfoList.DataSource = mEmployeeDepartmentInfoList
        dgEmployeeDepartmentInfoList.DataBind()
        lblDepartmentRecCount.Text = "Department (" + mEmployeeDepartmentInfoList.Count.ToString + ")"

        Dim D As Integer   'Departmnet
        Dim lnkDepartmentView As LinkButton 'ButtonColumn 
        For D1 As Integer = 0 To dgEmployeeDepartmentInfoList.Rows.Count - 1
            D = CType(Me.dgEmployeeDepartmentInfoList.Rows(D1).Cells(7).Text, Integer)
            If D <= 0 Then
                lnkDepartmentView = CType(dgEmployeeDepartmentInfoList.Rows(D1).Cells(6).FindControl("lnkDepartmentView"), LinkButton)
                lnkDepartmentView.Enabled = False
            End If
        Next
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
            s = CType(Me.dgDocumentList.Rows.Item(m).Cells(16).Text, Integer)
            DocumentHistoryCount = CType(Me.dgDocumentList.Rows.Item(m).Cells(18).Text, Boolean)
            IsDocumentApplicable = CType(Me.dgDocumentList.Rows.Item(m).Cells(19).Text, Boolean)
            If s <= 0 Then
                lnkDocumentView = CType(dgDocumentList.Rows.Item(m).Cells(15).FindControl("lnkDocumentView"), LinkButton)
                lnkDocumentView.Enabled = False
            End If
            If DocumentHistoryCount = False Then
                lnkDocumentHistory = CType(dgDocumentList.Rows.Item(m).Cells(17).FindControl("lnkDocumentHistory"), LinkButton)
                lnkDocumentHistory.Enabled = False
            End If
            If IsDocumentApplicable = False Then
                dgDocumentList.Rows(m).Cells(12).Enabled = False
            End If
        Next
    End Sub
    Public Sub BindEmpContactInfo()
        mEmployeeContactInfoList = EmployeeContactInfoList.GetEmployeeContactInfoList(mEmployee.ID)
        dgContactInfoList.DataSource = mEmployeeContactInfoList
        dgContactInfoList.DataBind()
        lblContactRecCount.Text = "Next To Kin Info (" + mEmployeeContactInfoList.Count.ToString + ")"

        Dim P As Integer  'ContactInfo
        Dim lnkContactInfoView As LinkButton 'ButtonColumn 
        For j As Integer = 0 To dgContactInfoList.Rows.Count - 1
            P = CType(Me.dgContactInfoList.Rows.Item(j).Cells(14).Text, Integer)
            If P <= 0 Then
                lnkContactInfoView = CType(dgContactInfoList.Rows.Item(j).Cells(13).FindControl("lnkContactInfoView"), LinkButton)
                lnkContactInfoView.Enabled = False
            End If
        Next
    End Sub
    Public Sub BindDesignation()
        mEmployeeDesignationList = EmployeeDesignationList.GetEmployeeDesignationList(mEmployee.ID)
        dgDesignationList.DataSource = mEmployeeDesignationList
        dgDesignationList.DataBind()
        lblDesignationRecCount.Text = "Designation (" + mEmployeeDesignationList.Count.ToString + ")"

        Dim q As Integer   'Designation
        Dim lnkDesignationView As LinkButton 'ButtonColumn 
        For k As Integer = 0 To dgDesignationList.Rows.Count - 1
            q = CType(Me.dgDesignationList.Rows.Item(k).Cells(8).Text, Integer)
            If q <= 0 Then
                lnkDesignationView = CType(dgDesignationList.Rows.Item(k).Cells(7).FindControl("lnkDesignationView"), LinkButton)
                lnkDesignationView.Enabled = False
            End If
        Next
    End Sub
    Public Sub BindEmpService()
        mEmployeeServiceList = EmployeeServiceList.GetEmployeeServiceList(mEmployee.ID)
        dgServiceList.DataSource = mEmployeeServiceList
        dgServiceList.DataBind()
        lblServiceRecCount.Text = "Service (" + mEmployeeServiceList.Count.ToString + ")"

        Dim r As Integer   'Service
        Dim lnkServiceView As LinkButton 'ButtonColumn 
        For l As Integer = 0 To dgServiceList.Rows.Count - 1
            r = CType(Me.dgServiceList.Rows.Item(l).Cells(6).Text, Integer)
            If r <= 0 Then
                lnkServiceView = CType(dgServiceList.Rows.Item(l).Cells(5).FindControl("lnkServiceView"), LinkButton)
                lnkServiceView.Enabled = False
            End If
        Next
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
            t = CType(Me.dgTrainingList.Rows(n).Cells(16).Text, Boolean)
            TrainingHistoryCount = CType(Me.dgTrainingList.Rows(n).Cells(18).Text, Boolean)
            IsNotApplicable = CType(Me.dgTrainingList.Rows(n).Cells(19).Text, Boolean)
            If t = False Then
                'lnkTrainingView = CType(dgTrainingList.Rows(n).Cells(13).FindControl("lnkTrainingView"), LinkButton)
                'lnkTrainingView.Enabled = False
                dgTrainingList.Rows(n).Cells(15).Enabled = False
            End If
            If TrainingHistoryCount = False Then
                'lnkTrainingHistory = CType(dgTrainingList.Rows(n).Cells(15).FindControl("lnkTrainingHistory"), LinkButton)
                'lnkTrainingHistory.Enabled = False
                dgTrainingList.Rows(n).Cells(17).Enabled = False
            End If

            If IsNotApplicable = True Then
                dgTrainingList.Rows(n).Cells(12).Enabled = False 'Renew
            End If
        Next
    End Sub
    Public Sub BindEmpDisciplinary()
        mEmployeeDisciplinaryList = EmployeeDisciplinaryList.GetEmployeeDisciplinaryList(mEmployee.ID)
        dgDisciplinaryList.DataSource = mEmployeeDisciplinaryList
        dgDisciplinaryList.DataBind()
        lblDisciplinaryRecCount.Text = "Disciplinary (" + mEmployeeDisciplinaryList.Count.ToString + ")"

        Dim v As Integer    'Disciplinary
        Dim lnkDisciplinaryView As LinkButton 'ButtonColumn 
        For b As Integer = 0 To dgDisciplinaryList.Rows.Count - 1
            v = CType(Me.dgDisciplinaryList.Rows(b).Cells(10).Text, Integer)
            If v <= 0 Then
                lnkDisciplinaryView = CType(dgDisciplinaryList.Rows(b).Cells(9).FindControl("lnkDisciplinaryView"), LinkButton)
                lnkDisciplinaryView.Enabled = False
            End If
        Next
    End Sub
    Public Sub BindEmpLeaves()
        mEmployeeLeaveList = EmployeeLeaveList.GetEmployeeLeaveList(mEmployee.ID)
        dgLeaveRecordList.DataSource = mEmployeeLeaveList
        dgLeaveRecordList.DataBind()
        lblLeaveRecCount.Text = "Leave Record (" + mEmployeeLeaveList.Count.ToString + ")"

        Dim w As Integer  'Leave
        Dim lnkLeaveView As LinkButton 'ButtonColumn 
        For c As Integer = 0 To dgLeaveRecordList.Rows.Count - 1
            w = CType(Me.dgLeaveRecordList.Rows(c).Cells(10).Text, Integer)
            If w <= 0 Then
                lnkLeaveView = CType(dgLeaveRecordList.Rows(c).Cells(9).FindControl("lnkLeaveView"), LinkButton)
                lnkLeaveView.Enabled = False
            End If
        Next

    End Sub
    Public Sub BindEmpEquipment()
        mCompanyEquipmentList = CompanyEquipmentList.GetCompanyEquipmentList(mEmployee.ID)
        dgCompanyEquipmentList.DataSource = mCompanyEquipmentList
        dgCompanyEquipmentList.DataBind()
        lblEquipmentRecCount.Text = "Equipment (" & mCompanyEquipmentList.Count & ")"


    End Sub
    Public Sub DataFieldBind()
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

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        '   ClearAll()
        GetSession()

        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 19-July-2011

        If Not IsPostBack And Session("sender") = "" Then
            If Type <> 1 Then
                Session("MiddleFrame") = "wfEmployeeList_Ajax.aspx"
            End If
            DataFieldBind()
            SetGrid()  'Added By Utkarsh On 4-May-2011
            ControlEnability()
        End If
        'MessageBoxResult()
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
                End If
        End Select
    End Sub
    '-----END OF EMPLOYEE SERVICE

    'EMPLOYEE TRAINING
    Private Sub btnTrainingAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrainingAdd.Click

        If (Not User.IsInRole("EmployeeTrainingNew")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '*******************************
                DeleteTrainingRecord(mID)
            Case "View"
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------

                'Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                'Dim rowIndex As Integer = gvr.RowIndex
                'Idx = rowIndex + dgTrainingList.PageIndex * dgTrainingList.PageSize
                'mID = New Guid(dgTrainingList.DataKeys(rowIndex).Values("ID").ToString)

                Idx = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
                mID = New Guid(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)


                'mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)

                Dim mFileAttach As FileAttach
                mFileAttach = FileAttach.GetAttachment(mID)
                Session("mFileAttach") = mFileAttach

                If mFileAttach.Size > 0 Then
                    'Dim path As String = AppSettings("FilePath") & "\" & StrName & mCalibrationItemChild.FileExtension
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                    End If
                Else
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
                End If
            Case "Renew"

                'Added By Prashant On 17-July-2012
                If User.IsInRole("EmployeeTrainingEdit") = False Then
                    SetSession()
                    MarkLog(Action.Edit, "Employee Training", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
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
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
                End If
                'New addition by Amrita for Document Renewal
            Case "Renew"

                'Added By Prashant On 17-July-2012
                If User.IsInRole("EmployeeDocumentsEdit") = False Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "Employee Document", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
                End If
        End Select
    End Sub
    '-----END OF EMPLOYEE DISCIPLINARY

    'EMPLOYEE LEAVE
    Private Sub btnLeaveAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeaveAdd.Click
        '
        If (Not User.IsInRole("EmployeeLeaveNew")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "") 'CHK
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "") 'CHK
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
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
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
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                '*******************************
                DeleteCompanyEquipmentRecord(mID)
        End Select
    End Sub
    '------END OF Company Equipment
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        SetReport()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Response.Redirect("wfEmployee.aspx")
        If Not mEmployee.IsNew Then
            MarkLog(Flypal.Util.Action.Close, "Employee", "Emp : " + mEmployee.EmpNoName, Flypal.Util.ErrorType.NoError, mEmployee.ID, EventLogID)
        End If

        Response.Redirect(Request.QueryString("ChildPage") & "?Backpage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
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
