Imports System.Linq
Imports System.Collections.Generic
Imports System.Text

Public Class wfUnscheduleComponentRemovals
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList
    Dim mMachineNameValueList As MachineNameValueList

    Dim StartDate As String
    Dim EndDate As String
    Dim MachineName As String
    Dim MachineID As String
    Dim Aircraft As String
    Dim AssemblyType As String
    Dim AssemblyText, ComponentText As String
    Dim Model As String
    Dim SerialNo As String

    Dim RegNo, SerialNoPosition As String
    Dim da As New CSLA.Data.ObjectAdapter
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mCompanyDetail As New CompanyDetail
    Dim LogType As Integer
    Dim AssemblyTypeID As Integer

    Dim EventLogID As Guid
    Dim AOnDate, AOdate As String

    Dim mUnscheduleComponentRemovals As UnscheduleComponentRemovals
    Dim mUnscheduleComponentRemovalsDetails As UnscheduleComponentRemovalsDetails
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        AOnDate = Session("AOnDate")
        mUnscheduleComponentRemovals = Session("mUnscheduleComponentRemovals")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfUnscheduleComponentRemovals.aspx?" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mUnscheduleComponentRemovals")
        End If
    End Sub
    Private Sub SetSession()

    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Public Sub SetComboOfMachine(ByVal AsonDate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
    End Sub
    Private Sub ControlVisibility()

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
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        If cmbAircraft.SelectedIndex > 0 Then
            MachineID = cmbAircraft.SelectedValue.ToString

        Else
        End If
        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")

    End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate.Text.ToString
        EndDate = txtToDate.Text.ToString
        MachineID = "{00000000-0000-0000-0000-000000000000}"

        AssemblyType = ""
        Aircraft = ""
        AssemblyText = ""
        AssemblyTypeID = 1
        ComponentText = ""
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
                    Session("LogType") = LogType
                    Response.Redirect("wfUnscheduleComponentRemovals.aspx?LogType=" + CStr(LogType))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            Response.Redirect("wfUnscheduleComponentRemovals.aspx?LogType=" + CStr(LogType))
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbAircraft" Then
            If cmbAircraft.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select the Aircraft"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If

    End Sub
    Private Sub DataFieldBind()

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))

            Session("MiddleFrame") = "wfUnscheduleComponentRemovals.aspx?"
            ResetValues()
            AOnDate = Now.Date.ToString(AppSettings("DateFormat"))
            Session("AOnDate") = AOnDate
            SetComboOfMachine(AOnDate)
            DataFieldBind()
        End If
        ControlVisibility()

    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlCriteria.Update()
    End Sub
    Private Sub dgUnscheduleComponentRemovalsList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgUnscheduleComponentRemovalsList.RowDataBound
        If e.Row.RowType <> DataControlRowType.DataRow Then
            Return
        End If

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim PartID As Guid = (DataBinder.Eval(e.Row.DataItem, "PartID"))

            Dim grdUnscheduleComponentRemovalsDetails As GridView = DirectCast(e.Row.FindControl("grdUnscheduleComponentRemovalsDetails"), GridView)
            Dim grdInstComponentDetails As GridView = DirectCast(e.Row.FindControl("grdInstComponentDetails"), GridView)

            Dim lblDetails As Label = DirectCast(e.Row.FindControl("lblDetails"), Label)
            Dim lblCompDetails As Label = DirectCast(e.Row.FindControl("lblCompDetails"), Label)

            'Dim mUnscheduleComponentRemovalsDetails As UnscheduleComponentRemovalsDetails = UnscheduleComponentRemovalsDetails.GetUnscheduleComponentRemovalsDetails(StartDate, EndDate, New Guid(MachineID), PartID)

            If mUnscheduleComponentRemovals(PartID).UnscheduleComponentRemovalsDetails.Count > 0 Then
                e.Row.Cells(0).BackColor = Color.Yellow 'System.Drawing.ColorTranslator.FromHtml("#0000FF")
                lblDetails.Text = "Details of Unschedule Component Removals  : " & mUnscheduleComponentRemovals(PartID).UnscheduleComponentRemovalsDetails.Count & " Record(s)."
            Else
                lblDetails.Text = "Details of Unschedule Component Removals : 0 Record(s)."
            End If

            If mUnscheduleComponentRemovals(PartID).tmpInstalledCompList.Count > 0 Then
                e.Row.Cells(0).BackColor = Color.Yellow 'System.Drawing.ColorTranslator.FromHtml("#0000FF")
                lblCompDetails.Text = "Details of Component Installations  : " & mUnscheduleComponentRemovals(PartID).tmpInstalledCompList.Count & " Record(s)."
            Else
                lblCompDetails.Text = "Details of Component Installations : 0 Record(s)."
            End If

            grdUnscheduleComponentRemovalsDetails.DataSource = mUnscheduleComponentRemovals(PartID).UnscheduleComponentRemovalsDetails
            grdUnscheduleComponentRemovalsDetails.DataBind()

            Dim tmpInstCompList
            tmpInstCompList = (From c As tmpInstalledCompList.tmpInstalledCompInfo In mUnscheduleComponentRemovals(PartID).tmpInstalledCompList
                        Order By CDate(c.InstalledOnFormatted.ToString) Descending
                        Select c).ToList

            grdInstComponentDetails.DataSource = tmpInstCompList
            grdInstComponentDetails.DataBind()

        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

        If IsValid = True Then
            SetValues()
            mUnscheduleComponentRemovals = UnscheduleComponentRemovals.GetUnscheduleComponentRemovalList(StartDate, EndDate, New Guid(MachineID))
            dgUnscheduleComponentRemovalsList.DataSource = mUnscheduleComponentRemovals
            dgUnscheduleComponentRemovalsList.DataBind()
            upnlGrid.Update()


        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mMachineNameValueList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub txtFromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
        AOdate = txtFromDate.Text.Trim


        If AOnDate = AOdate Then
        Else
            SetComboOfMachine(AOdate)
            DataFieldBind()
            'End If
        End If
        upnlDate.Update()
    End Sub

#End Region
End Class