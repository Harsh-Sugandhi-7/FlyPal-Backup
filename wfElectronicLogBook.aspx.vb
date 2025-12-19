Public Class wfElectronicLogBook
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim ReportStatusList As New rptStatusList
    Dim StartDate As String
    Dim EndDate As String
    Dim AssemblyID As String
    Dim Model, AssemblyText, AssemblyType, SerialNoPosition As String
    Dim SerialNo As String
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mCompanyDetail As New CompanyDetail
    Dim mrptElectronicLogBook As New rptElectronicLogBook
    Dim dsEleLogRegister As New dsHistoryCumLogRegister
    Dim mAssemblylist As AssemblyList
    Dim da As New CSLA.Data.ObjectAdapter
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
         mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
     
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfElectronicLogBook.aspx?" Then
            Session.Remove("mAssemblylist")
        End If
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtFromDate.Text) Then
            StartDate = ""
        Else
            StartDate = txtFromDate.Text.ToString
        End If
        If Not IsDate(txtToDate.Text) Then
            EndDate = ""
        Else
            EndDate = txtToDate.Text.ToString
        End If


        AssemblyText = IIf(cmbAircraftAssembly.SelectedIndex > 0, cmbAircraftAssembly.SelectedItem.Text, "")
        AssemblyID = cmbAircraftAssembly.SelectedValue.ToString
        AssemblyType = mAssemblylist(cmbAircraftAssembly.SelectedIndex).AssemblyType
        SerialNo = mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo
        Model = mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName
        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
        lblAssembly1.Text = "Assembly : " & IIf(AssemblyText <> "", AssemblyText, "")
     End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate.Text.ToString
        EndDate = txtToDate.Text.ToString
        AssemblyID = "{00000000-0000-0000-0000-000000000000}"
        AssemblyText = ""
    End Sub
    Private Sub Display()
        lblAssembly1.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
    Private Sub SetReport()
        SetValues()

        If mAssemblylist(cmbAircraftAssembly.SelectedIndex).Position <> "" Then
            SerialNoPosition = mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo + "(" + mAssemblylist(cmbAircraftAssembly.SelectedIndex).Position + ")"
        Else
            SerialNoPosition = mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo
        End If

        myReport = New crElectronicLogRegisterBA '
        ''If AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
        ''    myReport = New crElectronicLogRegisterBA 'Added by Saylee on 3-Dec-2013 for BA03122013
        ''Else
        ''    myReport = New crHistoryCumLogRegister
        ''End If

        mrptElectronicLogBook = rptElectronicLogBook.GetElectronicLogBook(StartDate, EndDate, "", _
       AssemblyType, Model, SerialNo, "", "", "", "", "", True, True, True, True, True, AssemblyID, , , , chkShowCompliance.Checked)
        ReportStatusList.Add(New rptStatus(, 0, New SmartDate(StartDate).FormattedText + " " + "To" + " " + New SmartDate(EndDate).FormattedText, AssemblyType + " " + "Details", , , _
             , , mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, SerialNoPosition, , , , , , , , , , , , "Period", "Before" + " " + New SmartDate(StartDate).FormattedText, , "Total Diff.", , "After" + " " + New SmartDate(EndDate).FormattedText))


        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
            mCompanyDetail.WebSite, "Electronic Log Register of" + " " + AssemblyType, "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mrptElectronicLogBook.Count = 0 Then
            ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            ''msg1.ReplacePage = "wfSearchCriteriaForLogBook.aspx?"
            ''msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
            'Added By Utkarsh On 7-Jun-2011 For All07062011
        Else

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1284)
            '*******************************
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(dsEleLogRegister)
        '----------------------------------------------------------

        da.Fill(dsEleLogRegister, mrptElectronicLogBook)
        da.Fill(dsEleLogRegister, Report)
        da.Fill(dsEleLogRegister, ReportStatusList)
        da.Fill(dsEleLogRegister, mrptImage) 'Added by Utkarsh for Report Logo)
        myReport.SetDataSource(dsEleLogRegister)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType, "openTranDetail", Str)
        ResetValues()
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbAircraftAssembly" Then
            If cmbAircraftAssembly.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select the Assembly"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub DataFieldBind()

        Dim mAssemblylist As AssemblyList
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, , txtFromDate.Text.ToString, "<SELECT>", True)
        Session("mAssemblyList") = mAssemblylist
        cmbAircraftAssembly.DataSource = mAssemblylist
        cmbAircraftAssembly.DataBind()
        upnlAssembly.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfElectronicLogBook.aspx?"
            ResetValues()
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
        End If
        upnlFromDate.Update()
        upnlToDate.Update()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click

        If IsValid = True Then
            SetReport()
        Else
            upnlValidations.Update()
            Exit Sub
        End If
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mAssemblylist = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
        If IsDate(txtFromDate.Text) Then
            If txtFromDate.Text = String.Empty Then
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Else
                txtFromDate.Text = CDate(txtFromDate.Text).ToString(AppSettings("DateFormat"))
            End If
        Else
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End If
    End Sub
    Private Sub txtToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.TextChanged
        If IsDate(txtToDate.Text) Then
            If txtToDate.Text = String.Empty Then
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Else
                txtToDate.Text = CDate(txtToDate.Text).ToString(AppSettings("DateFormat"))
            End If
        Else
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End If
    End Sub
#End Region

    
End Class