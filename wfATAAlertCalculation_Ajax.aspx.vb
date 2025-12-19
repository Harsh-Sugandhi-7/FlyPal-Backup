'Created By: Saylee
'Date       : 26-Sep-2017

Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq
Imports System.Linq.Enumerable
Imports System
Imports System.IO
Imports System.Text


Public Class wfATAAlertCalculation_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mModelList As ModelList
    Public mATAList As ATAList
    Public mReliabilityATAAlertList As ReliabilityATAAlertList

    Public IsAllATASelected As Boolean = False
    Public ATAIDs As StringBuilder
    Dim EventLogID As Guid
    Dim totcnt As Integer
#End Region


#Region "Business Methods"
    Private Sub SetSession()
        Session("mModelList") = mModelList
        Session("mReliabilityATAAlertList") = mReliabilityATAAlertList
        Session("mATAList") = mATAList
    End Sub
    Private Sub GetSession()
        mModelList = Session("mModelList")
        mATAList = Session("mATAList")
        mReliabilityATAAlertList = Session("mReliabilityATAAlertList")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "OKOnly" Then
                        mReliabilityATAAlertList = ReliabilityATAAlertList.GetReliabilityATAAlertList(cmbModelSearchList.SelectedValue.ToString)
                        dgATAList.DataSource = mReliabilityATAAlertList
                        lblResult.Text = "ATA Alert List : " & mReliabilityATAAlertList.Count & " Record(s) Found."
                        Session("mReliabilityATAAlertList") = mReliabilityATAAlertList
                        dgATAList.DataBind()
                        upnlGridView.Update()
                    End If
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mModelList = ModelList.GetAirframeModelList("(SELECT)")
        'end
        cmbModel.DataSource = mModelList
        Session("mModelList") = mModelList
        cmbModel.DataBind()

        mATAList = ATAList.GetATAList()
        cmbATAList.DataSource = mATAList
        Session("mATAList") = mATAList
        cmbATAList.DataBind()

        cmbATAList.DataSource = mATAList
        cmbATAList.DataBind()

        cmbModelSearchList.DataSource = mModelList
        Session("mModelList") = mModelList
        cmbModelSearchList.DataBind()

        mReliabilityATAAlertList = ReliabilityATAAlertList.GetReliabilityATAAlertList(mModelList(1).ID.ToString)
        dgATAList.DataSource = mReliabilityATAAlertList
        lblResult.Text = "ATA Alert List : " & mReliabilityATAAlertList.Count & " Record(s) Found."
        Session("mReliabilityATAAlertList") = mReliabilityATAAlertList

        DataBind()
        upnlGridView.Update()

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            ' SetTitle()
            cmbModelSearchList.SelectedIndex = 1
        End If
    End Sub
    Private Function ReCalculate(ByVal mATAID As Guid, mModelID As Guid, IsPireps As Boolean) As Decimal

        Dim SumX As Decimal = 0
        Dim SumXAvg As Decimal = 0
        Dim SD As Decimal = 0
        Dim AlertLevel As Decimal = 0
        Dim RoundedAlertLevel As Decimal
        Dim SumSquareDiffFlightHoursPer1000AndSumXAvg As Decimal = 0

        Dim mrptATAWiseMonthlyAlertLevel As New rptATAWiseMonthlyAlertLevel

        SumX = mrptATAWiseMonthlyAlertLevel.GetSumFlightHoursPer1000(mATAID, mModelID.ToString, IsPireps, 3, Today.Year)
        SumXAvg = SumX / 24

        SumSquareDiffFlightHoursPer1000AndSumXAvg = mrptATAWiseMonthlyAlertLevel.GetSumSquareDiffFlightHoursPer1000AndSumXAvg(mATAID, mModelID.ToString, IsPireps, 3, Today.Year, SumXAvg)
        SD = SumSquareDiffFlightHoursPer1000AndSumXAvg / 24
        AlertLevel = SumXAvg + (3 * SD)

        RoundedAlertLevel = Decimal.Round(AlertLevel)
        Return RoundedAlertLevel


    End Function
    Private Sub dgATAList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgATAList.RowCommand
        Dim Idx As Int32
        Dim mId As Guid
        Select Case e.CommandName
            Case "ReCalculate"
                'Idx = CInt(e.CommandArgument) + dgATAList.PageIndex * dgATAList.PageSize
                mId = New Guid(e.CommandArgument.ToString)

                If (Not User.IsInRole("ReliabilityAlertATAView") And Not User.IsInRole("ReliabilityAlertATAEdit")) Then
                    SetSession()
                    'Changed By Utkarsh On 19-Jul-2011 For All19072011
                    MarkLog(Util.Action.Edit, "Reliability", User.Identity.Name & " is not Authorized User to Re-Calculate " & mATAList(mId).ATAChapter, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Dim mReliabilityATAAlert As ReliabilityATAAlert = ReliabilityATAAlert.GetReliabilityATAAlert(mId)

                mReliabilityATAAlert.ReliabilityAlertLevelPireps = ReCalculate(mReliabilityATAAlert.ATAID, mReliabilityATAAlert.ModelID, True)
                mReliabilityATAAlert.ReliabilityAlertLevelMaintDefect = ReCalculate(mReliabilityATAAlert.ATAID, mReliabilityATAAlert.ModelID, False)

                If mReliabilityATAAlert.ReliabilityAlertLevelPireps > 0 Or mReliabilityATAAlert.ReliabilityAlertLevelMaintDefect > 0 Then
                    mReliabilityATAAlert.Save()
                End If
                mReliabilityATAAlertList = ReliabilityATAAlertList.GetReliabilityATAAlertList(cmbModelSearchList.SelectedValue.ToString)
                dgATAList.DataSource = mReliabilityATAAlertList
                lblResult.Text = "ATA Alert List : " & mReliabilityATAAlertList.Count & " Record(s) Found."
                Session("mReliabilityATAAlertList") = mReliabilityATAAlertList
                dgATAList.DataBind()
                upnlGridView.Update()
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If cmbModelSearchList.SelectedIndex > 0 Then
            mReliabilityATAAlertList = ReliabilityATAAlertList.GetReliabilityATAAlertList(cmbModelSearchList.SelectedValue.ToString)
            dgATAList.DataSource = mReliabilityATAAlertList
            lblResult.Text = "ATA Alert List : " & mReliabilityATAAlertList.Count & " Record(s) Found."
            Session("mReliabilityATAAlertList") = mReliabilityATAAlertList
            dgATAList.DataBind()
            upnlGridView.Update()
            'ENd
        End If

    End Sub
   
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Changed By Utkarsh On 19-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, "ATA", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        ' RemoveSession()
     Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub dgATAList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgATAList.PageIndexChanging
        dgATAList.PageIndex = e.NewPageIndex
        dgATAList.DataSource = mReliabilityATAAlertList
        Session("mReliabilityATAAlertList") = mReliabilityATAAlertList
        dgATAList.DataBind()
    End Sub
    Private Sub dgATAList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgATAList.Sorting
        mATAList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mReliabilityATAAlertList") = mReliabilityATAAlertList
        dgATAList.DataSource = mReliabilityATAAlertList
        dgATAList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub btnReCalculate_Click(sender As Object, e As EventArgs) Handles btnReCalculate.Click
        If cmbModel.SelectedIndex = 0 Then
            MSGBoxCtrl.show("Alert!", "Please select Model", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf cmbATAList.SelectedIndex = -1 Then
            MSGBoxCtrl.show("Alert!", "Please select atleast one ATA", "", MsgBoxStyle.OkOnly, "")
            Exit Sub

        End If

        Dim j As Integer

        For i As Integer = 0 To cmbATAList.Items.Count - 1
            If cmbATAList.Items(i).Selected Then

                Dim mReliabilityATAAlertList As ReliabilityATAAlertList = ReliabilityATAAlertList.GetReliabilityATAAlertList(cmbModel.SelectedValue.ToString, cmbATAList.Items(i).Value.ToString)
                Dim mReliabilityATAAlert As ReliabilityATAAlert
                If Not mReliabilityATAAlertList.Contains(New Guid(cmbModel.SelectedValue), New Guid(cmbATAList.Items(i).Value)) Then
                    mReliabilityATAAlert = ReliabilityATAAlert.NewReliabilityATAAlert()
                    mReliabilityATAAlert.ATAID = New Guid(cmbATAList.Items(i).Value)
                    mReliabilityATAAlert.ModelID = New Guid(cmbModel.SelectedValue)
                Else
                    mReliabilityATAAlert = ReliabilityATAAlert.GetReliabilityATAAlert(New Guid(cmbModel.SelectedValue), New Guid(cmbATAList.Items(i).Value))

                End If



                mReliabilityATAAlert.ReliabilityAlertLevelPireps = ReCalculate(mReliabilityATAAlert.ATAID, mReliabilityATAAlert.ModelID, True) 'Pireps Alert
                mReliabilityATAAlert.ReliabilityAlertLevelMaintDefect = ReCalculate(mReliabilityATAAlert.ATAID, mReliabilityATAAlert.ModelID, False) 'Maint Defect Alert

                If mReliabilityATAAlert.ReliabilityAlertLevelPireps > 0 Or mReliabilityATAAlert.ReliabilityAlertLevelMaintDefect > 0 Then
                    mReliabilityATAAlert.Save()
                End If
                j = j + 1
            End If

        Next


        MSGBoxCtrl.show("Alert Calculation!", "Alert Calculated Succesfully!!", "", MsgBoxStyle.OkOnly, "OKOnly")


    End Sub
#End Region
  
End Class