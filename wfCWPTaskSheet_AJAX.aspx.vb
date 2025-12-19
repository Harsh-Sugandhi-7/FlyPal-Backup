Public Class wfCWPTaskSheet_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Description "
    Public mCWP As CWP
    Protected mEmployeeListForCombo As EmployeeListForCombo
    Protected mCWPFunctionList As FunctionNameList
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mCWP = Session("mCWP")
        mEmployeeListForCombo = Session("mEmployeeListForComboOnCWPTaskSheet")
        mCWPFunctionList = Session("mCWPFunctionList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEmployeeListForComboOnCWPTaskSheet")
        Session.Remove("Edit")
        Session.Remove("mCWPFunctionList")
    End Sub
    Private Sub setSession()
        Session("mCWP") = mCWP
    End Sub

    Private Function setObject() As Boolean
        mCWP.CWPTaskSheets.CurrentItem.SrNo = mCWP.CWPTaskSheets.CurrentIndex + 1
        mCWP.CWPTaskSheets.CurrentItem.FunctionID = IIf(cmbFunction.SelectedIndex = 0, Guid.Empty, New Guid(cmbFunction.SelectedValue))

        mCWP.CWPTaskSheets.CurrentItem.TechEmployeeID = New Guid(cmbTechEmployeeList.SelectedValue)
        mCWP.CWPTaskSheets.CurrentItem.EngEmployeeID = New Guid(cmbEngEmployeeList.SelectedValue)


        mCWP.CWPTaskSheets.CurrentItem.TechEmpName = IIf(cmbTechEmployeeList.SelectedIndex > 0, mEmployeeListForCombo(New Guid(cmbTechEmployeeList.SelectedValue.ToString)).Name, "") 'cmbTechEmployeeList.SelectedItem.ToString
        mCWP.CWPTaskSheets.CurrentItem.EngEmpName = IIf(cmbEngEmployeeList.SelectedIndex > 0, mEmployeeListForCombo(New Guid(cmbEngEmployeeList.SelectedValue.ToString)).Name, "")
        mCWP.ApplyEdit()
        Return True
    End Function
    Private Sub ControlVisibility()
        If cmbTechEmployeeList.SelectedIndex > 0 Then
            'lblTechLicenseNoStar.Visible = True
            cmbTechLicenseNoList.Enabled = True
        Else
            'lblTechLicenseNoStar.Visible = False
            cmbTechLicenseNoList.Enabled = False
        End If
        If cmbEngEmployeeList.SelectedIndex > 0 Then
            'lblEngLicenseNoStar.Visible = True
            cmbEngLicenseNoList.Enabled = True
        Else
            'lblEngLicenseNoStar.Visible = False
            cmbEngLicenseNoList.Enabled = False
        End If
    End Sub
    Private Sub SetLicenseNo()
        If cmbTechEmployeeList.SelectedIndex > 0 Then
            cmbTechLicenseNoList.SelectedValue = mCWP.CWPTaskSheets.CurrentItem.TechLicenseNo
        End If

        If cmbEngEmployeeList.SelectedIndex > 0 Then
            cmbEngLicenseNoList.SelectedValue = mCWP.CWPTaskSheets.CurrentItem.EngLicenseNo
        End If
    End Sub
    Private Sub DataFieldBind()
        mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo("(SELECT)")
        cmbTechEmployeeList.DataSource = mEmployeeListForCombo
        cmbEngEmployeeList.DataSource = mEmployeeListForCombo
        Session("mEmployeeListForComboOnCWPTaskSheet") = mEmployeeListForCombo

        If Not mCWP.CWPTaskSheets.CurrentItem.TechEmployeeID.Equals(Guid.Empty) Then
            Dim mTechLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mCWP.CWPTaskSheets.CurrentItem.TechEmpName.ToString, User.Identity.Name, True, "(SELECT)", False)
            cmbTechLicenseNoList.DataSource = mTechLicenseNoList
        End If

        If Not mCWP.CWPTaskSheets.CurrentItem.EngEmployeeID.Equals(Guid.Empty) Then
            Dim mEngLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mCWP.CWPTaskSheets.CurrentItem.EngEmpName.ToString, User.Identity.Name, True, "(SELECT)", False)
            cmbEngLicenseNoList.DataSource = mEngLicenseNoList
        End If

        'Added by Saylee on 29-Jun-2016
        mCWPFunctionList = FunctionNameList.GetFunctionNameList(, "(SELECT)")
        cmbFunction.DataSource = mCWPFunctionList
        Session("mCWPFunctionList") = mCWPFunctionList

        '-----------------------
        DataBind()

        If Not mCWP.CWPTaskSheets.CurrentItem.TechEmployeeID.Equals(Guid.Empty) And mCWP.CWPTaskSheets.CurrentItem.TechLicenseNo <> "" Then cmbTechLicenseNoList.SelectedValue = mCWP.CWPTaskSheets.CurrentItem.TechLicenseNo
        If Not mCWP.CWPTaskSheets.CurrentItem.EngEmployeeID.Equals(Guid.Empty) And mCWP.CWPTaskSheets.CurrentItem.EngLicenseNo <> "" Then cmbEngLicenseNoList.SelectedValue = mCWP.CWPTaskSheets.CurrentItem.EngLicenseNo
        If Not mCWP.CWPTaskSheets.CurrentItem.FunctionID.Equals(Guid.Empty) Then cmbFunction.SelectedValue = mCWP.CWPTaskSheets.CurrentItem.FunctionID.ToString
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        getSession()
        If Not IsPostBack Then
            cmbFunction.Focus()
            DataFieldBind()
            SetLicenseNo()
            SetLicenseNo()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Page.IsValid Then

            If mCWP.CWPTaskSheets.CurrentItem.IsNew And Not Session("Edit") = True And mCWP.CWPTaskSheets.Contains(New Guid(cmbFunction.SelectedValue), "") Then
                Session("Duplicate") = "Duplicate"
                Session("ToCleareList") = "ToCleareList"
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "CWPTaskSheet", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            If setObject() Then
                Session("mCWP") = mCWP
                If Not mCWP.CWPTaskSheets.CurrentItem.IsValid Then upnlValidation.Update() : Exit Sub
                RemoveSession()
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
            End If
        Else
            upnlValidation.Update()
            Exit Sub
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mCWP.CWPTaskSheets.CurrentItem.IsNew And Not Session("Edit") = True Then mCWP.CWPTaskSheets.Remove(mCWP.CWPTaskSheets.CurrentItem)
        Session("mCWP") = mCWP
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub cmbTechEmployeeList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbTechEmployeeList.SelectedIndexChanged
        ControlVisibility()
        If cmbTechEmployeeList.SelectedIndex > 0 Then
            Dim mEmployeeStatus As EmployeeStatus
            Dim message As String = ""
            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(cmbTechEmployeeList.SelectedValue.ToCharArray, mCWP.CWPDateFormatted.ToString)
            If (mEmployeeStatus(0).Information <> "") Then
                cmbTechEmployeeList.SelectedValue = mCWP.CWPTaskSheets.CurrentItem.TechEmployeeID.ToString
                message = mEmployeeStatus(0).Information
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            Dim mTechLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mEmployeeListForCombo(New Guid(cmbTechEmployeeList.SelectedValue.ToString)).Name, User.Identity.Name, True, "(SELECT)", False)

            cmbTechLicenseNoList.DataSource = mTechLicenseNoList
            cmbTechLicenseNoList.DataBind()
        Else
            cmbTechLicenseNoList.ClearSelection()
        End If
    End Sub
    Private Sub cmbEngEmployeeList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbEngEmployeeList.SelectedIndexChanged
        ControlVisibility()
        If cmbEngEmployeeList.SelectedIndex > 0 Then
            Dim mEmployeeStatus As EmployeeStatus
            Dim message As String = ""
            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(cmbEngEmployeeList.SelectedValue.ToCharArray, mCWP.CWPDateFormatted.ToString)
            If (mEmployeeStatus(0).Information <> "") Then
                cmbEngEmployeeList.SelectedValue = mCWP.CWPTaskSheets.CurrentItem.EngEmployeeID.ToString
                message = mEmployeeStatus(0).Information
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            Dim mEngLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mEmployeeListForCombo(New Guid(cmbEngEmployeeList.SelectedValue.ToString)).Name, User.Identity.Name, True, "(SELECT)", False)

            cmbEngLicenseNoList.DataSource = mEngLicenseNoList
            cmbEngLicenseNoList.DataBind()
        Else
            cmbEngLicenseNoList.ClearSelection()
        End If
    End Sub
    Private Sub cmbEngLicenseNoList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbEngLicenseNoList.SelectedIndexChanged
        mCWP.CWPTaskSheets.CurrentItem.EngLicenseNo = IIf(cmbEngLicenseNoList.SelectedIndex > 0, cmbEngLicenseNoList.SelectedItem.ToString, "")
        Session("mCWP") = mCWP
    End Sub
    Private Sub cmbTechLicenseNoList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbTechLicenseNoList.SelectedIndexChanged
        mCWP.CWPTaskSheets.CurrentItem.TechLicenseNo = IIf(cmbTechLicenseNoList.SelectedIndex > 0, cmbTechLicenseNoList.SelectedItem.ToString, "")
        Session("mCWP") = mCWP
    End Sub
    Private Sub hdnimgBtnFunction_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnFunction.Click
        mCWPFunctionList = FunctionNameList.GetFunctionNameList("", "(SELECT)")
        Session("mCWPFunctionList") = mCWPFunctionList
        cmbFunction.DataSource = mCWPFunctionList
        cmbFunction.DataBind()

        If Not mCWP.CWPTaskSheets.CurrentItem.FunctionID.Equals(Guid.Empty) Then
            cmbFunction.SelectedValue = mCWP.CWPTaskSheets.CurrentItem.FunctionID.ToString
            mCWP.CWPTaskSheets.CurrentItem.FunctionID = New Guid(cmbFunction.SelectedValue)
        End If

        upnlCWPTaskDetail.Update()
    End Sub
    Private Sub imgbtnFunction_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnFunction.Click
        If IsValid Then
            setObject()
            Session("mCWP") = mCWP
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFunctionMasterWindow", "OpenFunctionMasterWindow()", True)
        End If
    End Sub
#End Region

  
  
End Class