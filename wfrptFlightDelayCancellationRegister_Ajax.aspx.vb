'Added By Vikrant On 21-Nov-2014 For 

Public Class wfrptFlightDelayCancellationRegister_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mModelList As ModelList
    Public mATAList As ATAList
    Public mMachineNameValueList As MachineNameValueList
    Public mFligthDelayCancellationRegister As FligthDelayCancellationRegister
    Dim ModelIDs, ModelNames, FromDate, ToDate, mSearchingCriteria As String
    Dim EventLogID As Guid
#End Region

#Region "Business Methods"
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub GetSession()
        mMachineNameValueList = Session("mMachineNameValueList")
        mATAList = Session("mATAList")
        mModelList = Session("mModelList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mATAList")
        Session.Remove("mModelList")
    End Sub
#End Region

#Region "Data Binding"
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, IsTagRequired:=True, TagText:="(All)", SkipIsForInventoryAircarft:=True)
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataSource = mMachineNameValueList

        mModelList = ModelList.GetAirframeModelList()
        ChkModelList.DataSource = mModelList

        mATAList = ATAList.GetATAList(AddTopItem:="(All)")
        cmbATAList.DataSource = mATAList

        DataBind()
    End Sub

    Private Sub Display()
        lblAircraftCriteria.Visible = True
        lblModelNoCriteria.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblATACriteria.Visible = True
        lblTypeCriteria.Visible = True
    End Sub
    Private Sub SetValues()
        FromDate = txtFromDate.Text
        ToDate = txtToDate.Text
        ModelNames = String.Empty
        ModelIDs = String.Empty
        For i As Integer = 0 To ChkModelList.Items.Count - 1
            If ChkModelList.Items(i).Selected Then
                If Trim(ModelNames) = "" Then
                    ModelNames = ChkModelList.Items(i).Text
                    ModelIDs = ChkModelList.Items(i).Value
                Else
                    ModelNames = ModelNames + "," + ChkModelList.Items(i).Text
                    ModelIDs = ModelIDs + "," + ChkModelList.Items(i).Value
                End If
            End If
        Next
        lblDateRangeFrom.Text = "From Date : " + txtFromDate.Text
        lblDateRangeTo.Text = "To Date : " + txtToDate.Text
        lblModelNoCriteria.Text = "Model : " & IIf(IsNothing(ModelNames), "", ModelNames)
        lblTypeCriteria.Text = "Type : " & IIf(cmbType.SelectedIndex <= 0, "All", cmbType.SelectedItem.Text)
        lblATACriteria.Text = "ATA : " & IIf(cmbATAList.SelectedIndex <= 0, "All", cmbATAList.SelectedItem.Text)
        lblAircraftCriteria.Text = "Aircraft : " & IIf(cmbAircraft.SelectedIndex <= 0, "All", cmbAircraft.SelectedItem.Text)

        mSearchingCriteria = lblDateRangeFrom.Text + ", " + lblDateRangeTo.Text + ", " + lblModelNoCriteria.Text + ", " + lblAircraftCriteria.Text + ", " + lblATACriteria.Text + ", " + lblTypeCriteria.Text
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As New CompanyDetail
        Dim ReportName As String = String.Empty
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsFlightDelayCancellationRegister
        myReport = New crFligthDelayCancellationRegister

        ReportName = "Flight Delay/Cancellation Register"
        SetValues()

        mFligthDelayCancellationRegister = FligthDelayCancellationRegister.GetFlightDCList(txtFromDate.Text, txtToDate.Text, cmbAircraft.SelectedValue.ToString, ModelIDs.ToString, cmbATAList.SelectedValue.ToString, CInt(cmbType.SelectedValue))
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                 mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                 mCompanyDetail.WebSite, ReportName, ToDate, ModelIDs, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", AppSettings("Logo"), SearchStr8:=IIf(cmbType.SelectedIndex <= 0, "", cmbType.SelectedItem.ToString), SearchStr9:=IIf(cmbATAList.SelectedIndex <= 0, "", cmbATAList.SelectedItem.ToString), SearchStr10:=IIf(chkModel.Checked, "Model :", "Aircraft :"), SearchStr11:=IIf(chkModel.Checked, ModelNames, IIf(cmbAircraft.SelectedIndex <= 0, "All", cmbAircraft.SelectedItem.ToString)), SearchStr12:=txtFromDate.Text, SearchStr13:=txtToDate.Text)


        If mFligthDelayCancellationRegister.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1300)
        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mFligthDelayCancellationRegister)
        da.Fill(ds, Report)

        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "FlightDelayCancellationRegister", mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    '
            End Select
        End If
    End Sub
    Private Sub ControlEnability()
        If chkModel.Checked Then
            cmbAircraft.ClearSelection()
            cmbAircraft.Enabled = False
            chkAircraft.Checked = False
            chkAircraft.Enabled = False
        ElseIf chkAircraft.Checked Then
            ChkModelList.ClearSelection()
            ChkModelList.Enabled = False
            chkModel.Checked = False
            chkModel.Enabled = False
        Else
            chkAircraft.Enabled = True
            cmbAircraft.ClearSelection()
            cmbAircraft.Enabled = True
            ChkModelList.ClearSelection()
            ChkModelList.Enabled = True
            chkModel.Enabled = True
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
            ControlEnability()
        End If
    End Sub

    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlCriteria.Update() 'Added By Sachin on 12-10-23
    End Sub

    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
            End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub chkAircraft_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAircraft.CheckedChanged
        ControlEnability()
    End Sub

    Private Sub chkModel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkModel.CheckedChanged
        ControlEnability()
    End Sub
#End Region

   
End Class