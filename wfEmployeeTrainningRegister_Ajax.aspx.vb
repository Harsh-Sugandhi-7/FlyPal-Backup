Imports System.Collections.Generic
Imports System.Linq
Imports Flypal.EmpNoNameAutoComplete
Public Class wfEmployeeTrainningRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mEmployeeTrainningRegister As EmployeeTrainningRegister
    Protected mTrainning As TrainingList
    Protected mTrainningOrg As TrainingOrgList
    Dim EmployeeID As String
    Private mEmployeeList As EmployeeList
#End Region

#Region " Business Method "
    Private Sub GetSession()
        mEmployeeTrainningRegister = Session("mEmployeeTrainningRegister")
    End Sub
    Public Sub RemoveSessions()
        Session.Remove("mEmployeeTrainningRegister")
    End Sub
    Public Sub SetReport(Optional EmployeeID As String = "{00000000-0000-0000-0000-000000000000}")
        GetSession()
        SetValues(EmployeeID)
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim obj As EmployeeTrainningRegister
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim ds As New dsEmployeeTrainningRegister

        myReport = New crEmployeeTrainningRegister

        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String
        Dim SearchStr4 As String


        If txtEmployee.Text <> "" Then
            SearchStr1 = txtEmployee.Text
        Else
            SearchStr1 = ""
        End If

        If cmbTrainningList.SelectedIndex > 0 Then
            SearchStr2 = cmbTrainningList.SelectedItem.Text
        Else
            SearchStr2 = ""
        End If

        If cmbTrainningOrgList.SelectedIndex > 0 Then
            SearchStr3 = cmbTrainningOrgList.SelectedItem.Text
        Else
            SearchStr3 = ""
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, "Employee Training Register", SearchStr1, SearchStr2, SearchStr3, SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        obj = EmployeeTrainningRegister.GetEmployeeTrainningRegister("", EmployeeID, cmbTrainningList.SelectedValue.ToString, cmbTrainningOrgList.SelectedValue.ToString)

        If obj.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, obj)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
    End Sub
    Private Sub SetValues(Optional EmployeeID As String = "{00000000-0000-0000-0000-000000000000}")
        Dim mEmployee As String = ""
        Dim mTraining As String = ""
        Dim mTrainingOrg As String = ""
        EmployeeID = EmployeeID 'IIf(SelectedCrewID.Value.Length > 0, SelectedCrewID.Value, Guid.Empty.ToString)
        If txtEmployee.Text <> "" Then
            mEmployee = txtEmployee.Text
            lblEmployee1.Text = "Employee : " & txtEmployee.Text
        Else
            mEmployee = ""
            lblEmployee1.Text = "Employee : All"
        End If

        If cmbTrainningList.SelectedIndex > 0 Then
            mTraining = cmbTrainningList.SelectedItem.Text
            lblTrainning1.Text = "Training : " & mTraining
        Else
            mTraining = ""
            lblTrainning1.Text = "Training : All"
        End If

        If cmbTrainningOrgList.SelectedIndex > 0 Then
            mTrainingOrg = cmbTrainningOrgList.SelectedItem.Text
            lblTrainningOrg1.Text = "Training Org. : " & mTrainingOrg
        Else
            mTrainingOrg = ""
            lblTrainningOrg1.Text = "Training Org. : All"

        End If
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        cmbTrainningList.DataSource = TrainingList.GetTrainingList(, , , "(All)")
        cmbTrainningList.DataBind()

        cmbTrainningOrgList.DataSource = TrainingOrgList.GetTrainingOrgList(, , , "(All)")
        cmbTrainningOrgList.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
              DataFieldBind()
         End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            mEmployeeList = EmployeeList.GetEmployeeList()
            If txtEmployee.Text.Trim = "" Then
                EmployeeID = Guid.Empty.ToString
            Else
                If mEmployeeList.Contains(txtEmployee.Text) Then
                    EmployeeID = mEmployeeList(txtEmployee.Text, "").ID.ToString
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Select correct employee", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
            'If (InccorectEmployee.Value.Length > 0) Then
            '    InccorectEmployee.Value = ""
            '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Select correct employee", MsgBoxStyle.OkOnly, "")
            '    Exit Sub
            'Else
            SetReport(EmployeeID)
        End If

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSessions()
        Session("mDefectList") = ""
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblEmployee1.Visible = True
        lblTrainning1.Visible = True
        lblTrainningOrg1.Visible = True
        SetValues()
        upnlCurrentCriteria.Update()
    End Sub
#End Region


#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCrewListAutoComplete(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim mEmpNoNameList As EmpNoNameAutoComplete = EmpNoNameAutoComplete.GeEmpNoNameList(prefixText)
        If count = 0 Then
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In mEmpNoNameList
             Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).ToArray
        Else
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In mEmpNoNameList
            Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).Take(count).ToArray
        End If

    End Function
#End Region

End Class