

' Created By :   Saylee
' Date       :   17-Oct-2018

Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Public Class wfTaskCardHistory_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mTaskCardHistoryList As TaskCardHistoryList
    Public mDetail As String
    Private mMachineNameValueList As MachineNameValueList
    Dim mAssemblylist As AssemblyList
    Dim IsReadOnly As Boolean
    Private AircraftId As String
    Private AssemblyId As String
    Dim Flag As Int16
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mTaskCardHistoryList = CType(Session("mTaskCardHistoryList"), TaskCardHistoryList)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        IsReadOnly = Session("IsReadOnly")
        AircraftId = Session("AircraftId")
    End Sub

    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mAssemblylist") = mAssemblylist
        Session("mTaskCardHistoryList") = mTaskCardHistoryList
        Session("IsReadOnly") = IsReadOnly
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mTaskCardHistoryList")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mAssemblylist")
        Session.Remove("IsReadOnly")
    End Sub
    Private Sub ControlVisibility()
        btnPrintTop.Visible = (mTaskCardHistoryList.Count > 10)
        btnPrintTop.Enabled = (mTaskCardHistoryList.Count > 0)
        btnPrint.Enabled = (mTaskCardHistoryList.Count > 0)
        btnBackTop.Visible = (mTaskCardHistoryList.Count > 10)
        upnlActionBtnTop.Update()
        upnlActionBtn.Update()
        'End
    End Sub
    Private Sub FindNow()
        mTaskCardHistoryList = TaskCardHistoryList.GetTaskCardHistory(txtTaskNo.Text, cmbAircraftList.SelectedValue.ToString, cmbAircraftAssembly.SelectedValue.ToString, txtPart.Text.Trim, txtSerialNo.Text.Trim)
        dgMonitorInspStatusList.DataSource = mTaskCardHistoryList
        dgMonitorInspStatusList.DataBind()
        upnlGrid.Update()


        If (String.IsNullOrEmpty(txtTaskNo.Text.Trim)) Then
            txtDescription.Text = ""
            txtReference.Text = ""
            txtFreq.Text = ""
            txtATA.Text = ""
        Else
            Dim mTaskCard As TaskCard = TaskCard.GetTaskCard(txtTaskNo.Text.Trim)
            txtDescription.Text = mTaskCard.TaskDesc
            txtReference.Text = mTaskCard.Reference
            txtFreq.Text = mTaskCard.INSPTypeInterval
            txtATA.Text = mTaskCard.ATAChapter
        End If

        upnlDetails.Update()

        If mTaskCardHistoryList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        lblResult.Text = "History of Task Card as per criteria :" & mTaskCardHistoryList.Count & " Record(s) found."
        upnlResult.Update()

       
    End Sub
#End Region

#Region " DataBind "
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, SkipIsForInventoryAircarft:=True, IsTagRequired:=True, TagText:="ALL")
        cmbAircraftList.DataSource = mMachineNameValueList
        If IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString Then
            'do nothing
        Else
            cmbAircraftList.SelectedValue = AircraftId
        End If
        cmbAircraftList.DataBind()   'Added Code
        Session("AircraftId") = cmbAircraftList.SelectedValue
        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraftList.SelectedValue)).IsReadOnly
        Session("IsReadOnly") = IsReadOnly
        Session("mMachineNameValueList") = mMachineNameValueList

        'Added By Prashant 15-Jun-2015 
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraftList.SelectedValue, Today.Date.ToString, "(All)", True)
        cmbAircraftAssembly.DataSource = mAssemblylist
        If (Session("AssemblyId") = Guid.Empty.ToString Or IsNothing(Session("AssemblyId"))) Then
            'Do nothing
        Else
            cmbAircraftAssembly.SelectedValue = CType(Session("AssemblyId"), String)
        End If
        cmbAircraftAssembly.DataBind()
        Session("AssemblyId") = cmbAircraftAssembly.SelectedValue
        Session("mAssemblyList") = mAssemblylist

        'mTaskCardHistoryList = TaskCardHistoryList
        'dgMonitorInspStatusList.DataSource = mTaskCardHistoryList
        'txtDescription.Text = mTaskCardHistoryList(0).Description
        'txtReference.Text = mTaskCardHistoryList(0).Reference

        DataBind()
    End Sub


#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 2-Aug-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            DataFieldBind()
            '  ControlVisibility()
        End If
    End Sub
   
    Private Sub btnBackTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackTop.Click, btnBack.Click
        MarkLog(Util.Action.Close, "Task Card History", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect("Index.aspx")
        'Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    Private Sub dgMonitorInspStatusList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgMonitorInspStatusList.PageIndexChanging
        dgMonitorInspStatusList.PageIndex = e.NewPageIndex
        dgMonitorInspStatusList.DataSource = mTaskCardHistoryList
        dgMonitorInspStatusList.DataBind()
        Session("mTaskCardHistoryList") = mTaskCardHistoryList

    End Sub
    'Added By Vikrant On 14-Jan-2015 For All14012015
    Private Sub btnPrintTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click, btnPrint.Click
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsComplyHistory
        Dim mCompanyDetail As New CompanyDetail
        Dim ReportName As String = String.Empty

        mTaskCardHistoryList = TaskCardHistoryList.GetTaskCardHistory(txtTaskNo.Text, cmbAircraftList.SelectedValue.ToString, cmbAircraftAssembly.SelectedValue.ToString, txtPart.Text.Trim, txtSerialNo.Text.Trim)

        myReport = New crptTaskCardHistoryList

        If mTaskCardHistoryList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Aircraft As String = ""
        Aircraft = IIf(cmbAircraftList.SelectedIndex > 0, cmbAircraftList.SelectedItem.ToString, "")

        Dim Assembly As String = ""
        Assembly = IIf(cmbAircraftAssembly.SelectedIndex > 0, cmbAircraftAssembly.SelectedItem.ToString, "")

     

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Task Card History", txtDescription.Text.Trim, txtReference.Text.Trim, txtFreq.Text.Trim, txtTaskNo.Text.Trim, txtATA.Text.Trim, AppSettings("Product Version"), AppSettings("SINote"), Aircraft, Assembly, "", "", AppSettings("Logo"), txtPart.Text.Trim, txtSerialNo.Text.Trim)

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mTaskCardHistoryList)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)

        Session("CrystalReport") = myReport
        MarkLog(Util.Action.Print, "Task Card History", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    'End

    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click  ''btnFindNow.Click
        If txtTaskNo.Text = "" Then
            MSGBoxCtrl.Show("Alert..!!", "Task Card No. Required", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        FindNow()
        ControlVisibility()
    End Sub
    Private Sub cmbAircraftList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraftList.SelectedIndexChanged
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraftList.SelectedValue, Today.Date.ToString, "(All)", True)
        cmbAircraftAssembly.DataSource = mAssemblylist
        cmbAircraftAssembly.DataBind()
        Session("mAssemblyList") = mAssemblylist

        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraftList.SelectedValue)).IsReadOnly
        Session("IsReadOnly") = IsReadOnly

        upnlSearchCriteria.Update()
        btnFindNow_Click(sender, e)
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetTaskCardList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mTaskCardlist As TaskCardList
        mTaskCardlist = TaskCardList.GetTaskCardList(, , , prefixText)
        If count = 0 Then
            Return (From c As TaskCard In mTaskCardlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.TaskCardNo, c.ID.ToString())).ToArray
        Else
            Return (From c As TaskCard In mTaskCardlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.TaskCardNo, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region

End Class