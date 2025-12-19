Imports System.Linq
Public Class wfCWPInspection_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Description "
    Public mCWP As CWP
    Protected mEmployeeListForCombo As EmployeeListForCombo
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mCWP = Session("mCWP")
        mEmployeeListForCombo = Session("mEmployeeListForComboOnCWPInspection")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEmployeeListForComboOnCWPInspection")
        Session.Remove("Edit")
    End Sub
    Private Sub setSession()
        Session("mCWP") = mCWP
    End Sub
    Private Function setObject() As Boolean
        mCWP.CWPInspections.CurrentItem.SrNo = mCWP.CWPInspections.CurrentIndex + 1
        mCWP.CWPInspections.CurrentItem.Defect = Trim(txtDefect.Text)
        mCWP.CWPInspections.CurrentItem.WorkDone = Trim(txtWorkDone.Text)

        mCWP.CWPInspections.CurrentItem.TechEmployeeID = New Guid(cmbTechEmployeeList.SelectedValue)
        mCWP.CWPInspections.CurrentItem.EngEmployeeID = New Guid(cmbEngEmployeeList.SelectedValue)


        mCWP.CWPInspections.CurrentItem.TechEmpName = IIf(cmbTechEmployeeList.SelectedIndex > 0, mEmployeeListForCombo(New Guid(cmbTechEmployeeList.SelectedValue.ToString)).Name, "") 'cmbTechEmployeeList.SelectedItem.ToString
        mCWP.CWPInspections.CurrentItem.EngEmpName = IIf(cmbEngEmployeeList.SelectedIndex > 0, mEmployeeListForCombo(New Guid(cmbEngEmployeeList.SelectedValue.ToString)).Name, "")

        mCWP.CWPInspections.CurrentItem.InspSheetDefectEngEmployeeID = New Guid(cmbInspSheetDefectEngEmployeeList.SelectedValue)
        mCWP.CWPInspections.CurrentItem.InspSheetDefectEngEmpName = IIf(cmbInspSheetDefectEngEmployeeList.SelectedIndex > 0, mEmployeeListForCombo(New Guid(cmbInspSheetDefectEngEmployeeList.SelectedValue.ToString)).Name, "")
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

        If cmbInspSheetDefectEngEmployeeList.SelectedIndex > 0 Then
            cmbInspSheetDefectEngLicenseNoList.Enabled = True
        Else
            cmbInspSheetDefectEngLicenseNoList.Enabled = False
        End If
    End Sub
    Private Sub SetLicenseNo()
        If cmbTechEmployeeList.SelectedIndex > 0 Then
            cmbTechLicenseNoList.SelectedValue = mCWP.CWPInspections.CurrentItem.TechLicenseNo
        End If

        If cmbEngEmployeeList.SelectedIndex > 0 Then
            cmbEngLicenseNoList.SelectedValue = mCWP.CWPInspections.CurrentItem.EngLicenseNo
        End If

        If cmbInspSheetDefectEngEmployeeList.SelectedIndex > 0 Then
            cmbInspSheetDefectEngLicenseNoList.SelectedValue = mCWP.CWPInspections.CurrentItem.InspSheetDefectEngLicenseNo
        End If
    End Sub
    Private Sub DataFieldBind()
        mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo("(SELECT)")
        cmbTechEmployeeList.DataSource = mEmployeeListForCombo
        cmbEngEmployeeList.DataSource = mEmployeeListForCombo
        cmbInspSheetDefectEngEmployeeList.DataSource = mEmployeeListForCombo
        Session("mEmployeeListForComboOnCWPInspection") = mEmployeeListForCombo

        If Not mCWP.CWPInspections.CurrentItem.TechEmployeeID.Equals(Guid.Empty) Then
            Dim mTechLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mCWP.CWPInspections.CurrentItem.TechEmpName.ToString, User.Identity.Name, True, "(SELECT)", False)
            cmbTechLicenseNoList.DataSource = mTechLicenseNoList
        End If

        If Not mCWP.CWPInspections.CurrentItem.EngEmployeeID.Equals(Guid.Empty) Then
            Dim mEngLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mCWP.CWPInspections.CurrentItem.EngEmpName.ToString, User.Identity.Name, True, "(SELECT)", False)
            cmbEngLicenseNoList.DataSource = mEngLicenseNoList
        End If

        If Not mCWP.CWPInspections.CurrentItem.InspSheetDefectEngEmployeeID.Equals(Guid.Empty) Then
            Dim mInspSheetDefectEngLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mCWP.CWPInspections.CurrentItem.InspSheetDefectEngEmpName.ToString, User.Identity.Name, True, "(SELECT)", False)
            cmbInspSheetDefectEngLicenseNoList.DataSource = mInspSheetDefectEngLicenseNoList
        End If
        DataBind()

        If Not mCWP.CWPInspections.CurrentItem.TechEmployeeID.Equals(Guid.Empty) And mCWP.CWPInspections.CurrentItem.TechLicenseNo <> "" Then cmbTechLicenseNoList.SelectedValue = mCWP.CWPInspections.CurrentItem.TechLicenseNo
        If Not mCWP.CWPInspections.CurrentItem.EngEmployeeID.Equals(Guid.Empty) And mCWP.CWPInspections.CurrentItem.EngLicenseNo <> "" Then cmbEngLicenseNoList.SelectedValue = mCWP.CWPInspections.CurrentItem.EngLicenseNo
        If Not mCWP.CWPInspections.CurrentItem.InspSheetDefectEngEmployeeID.Equals(Guid.Empty) And mCWP.CWPInspections.CurrentItem.InspSheetDefectEngLicenseNo <> "" Then cmbInspSheetDefectEngLicenseNoList.SelectedValue = mCWP.CWPInspections.CurrentItem.InspSheetDefectEngLicenseNo

    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        getSession()
        If Not IsPostBack Then
            txtDefect.Focus()
            DataFieldBind()
            SetLicenseNo()
            '  SetLicenseNo()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If IsValid Then
            If setObject() Then
                Session("mCWP") = mCWP
                RemoveSession()
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
            End If
        Else
            'upnlValidationSummary.Update()
            'Exit Sub
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mCWP.CWPInspections.CurrentItem.IsNew And Not Session("Edit") = True Then mCWP.CWPInspections.Remove(mCWP.CWPInspections.CurrentItem)
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

           

            Dim mTechLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mEmployeeListForCombo(New Guid(cmbTechEmployeeList.SelectedValue.ToString)).Name, User.Identity.Name, True, "(SELECT)", False)

            cmbTechLicenseNoList.DataSource = mTechLicenseNoList
            cmbTechLicenseNoList.DataBind()

            Dim mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(cmbTechEmployeeList.SelectedValue.ToString, mCWP.CWPDateFormatted.ToString)
            If (mEmployeeStatus(0).Information <> "") Then
                cmbTechEmployeeList.SelectedIndex = 0
                cmbTechLicenseNoList.ClearSelection()
                MSGBoxCtrl.show("Save Alert!", mEmployeeStatus(0).Information, "", MsgBoxStyle.OkOnly, "")
                mCWP.CWPInspections.CurrentItem.TechLicenseNo = ""
                Exit Sub
            End If
        Else
            cmbTechLicenseNoList.ClearSelection()
        End If
        mCWP.CWPInspections.CurrentItem.TechLicenseNo = ""
    End Sub
    Private Sub cmbEngEmployeeList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbEngEmployeeList.SelectedIndexChanged
        ControlVisibility()
      

        'If cmbEngEmployeeList.SelectedIndex > 0 Then
        Dim mEngLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mEmployeeListForCombo(New Guid(cmbEngEmployeeList.SelectedValue.ToString)).Name, User.Identity.Name, True, "(SELECT)", False)

        cmbEngLicenseNoList.DataSource = mEngLicenseNoList
        cmbEngLicenseNoList.DataBind()
        'Else
        cmbEngLicenseNoList.ClearSelection()
        '  End If
        mCWP.CWPInspections.CurrentItem.EngLicenseNo = ""

        Dim mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(cmbEngEmployeeList.SelectedValue.ToString, mCWP.CWPDateFormatted.ToString)
        If (mEmployeeStatus(0).Information <> "") Then
            cmbEngEmployeeList.SelectedIndex = 0

            MSGBoxCtrl.show("Save Alert!", mEmployeeStatus(0).Information, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Private Sub cmbInspSheetDefectEngEmployeeList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbInspSheetDefectEngEmployeeList.SelectedIndexChanged
        ControlVisibility()
        'If cmbEngEmployeeList.SelectedIndex > 0 Then

      

        Dim mInspSheetDefectEngLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mCWP.CWPInspections.CurrentItem.InspSheetDefectEngEmpName.ToString, User.Identity.Name, True, "(SELECT)", False)

        cmbInspSheetDefectEngLicenseNoList.DataSource = mInspSheetDefectEngLicenseNoList
        cmbInspSheetDefectEngLicenseNoList.DataBind()
        'Else
        cmbInspSheetDefectEngLicenseNoList.ClearSelection()
        '  End If
        mCWP.CWPInspections.CurrentItem.InspSheetDefectEngLicenseNo = ""

        Dim mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(cmbInspSheetDefectEngEmployeeList.SelectedValue.ToString, mCWP.CWPDateFormatted.ToString)
        If (mEmployeeStatus(0).Information <> "") Then
            cmbInspSheetDefectEngEmployeeList.SelectedIndex = 0
            MSGBoxCtrl.show("Save Alert!", mEmployeeStatus(0).Information, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Private Sub cmbEngLicenseNoList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbEngLicenseNoList.SelectedIndexChanged
        mCWP.CWPInspections.CurrentItem.EngLicenseNo = IIf(cmbEngLicenseNoList.SelectedIndex > 0, cmbEngLicenseNoList.SelectedItem.ToString, "")
        Session("mCWP") = mCWP
    End Sub
    Private Sub cmbInspSheetDefectEngLicenseNoList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbInspSheetDefectEngLicenseNoList.SelectedIndexChanged
        mCWP.CWPInspections.CurrentItem.InspSheetDefectEngLicenseNo = IIf(cmbInspSheetDefectEngLicenseNoList.SelectedIndex > 0, cmbInspSheetDefectEngLicenseNoList.SelectedItem.ToString, "")
        Session("mCWP") = mCWP
    End Sub
    Private Sub cmbTechLicenseNoList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbTechLicenseNoList.SelectedIndexChanged
        mCWP.CWPInspections.CurrentItem.TechLicenseNo = IIf(cmbTechLicenseNoList.SelectedIndex > 0, cmbTechLicenseNoList.SelectedItem.ToString, "")
        Session("mCWP") = mCWP
    End Sub
#End Region

End Class