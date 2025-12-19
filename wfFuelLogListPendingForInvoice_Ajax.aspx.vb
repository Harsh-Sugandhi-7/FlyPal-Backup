Imports System.Collections.Generic

Public Class wfFuelLogListPendingForInvoice_Ajax
    Inherits System.Web.UI.Page

#Region " Variables and Declarations "
    Dim mFuelLogListPendingForInvoices As FuelLogListPendingForInvoices
    Dim mFuelInvoice As FuelInvoice
    Private checkedIds As New List(Of String)()
    Dim mName As String
#End Region
#Region " Methods "
    Private Sub GetSession()
        mFuelLogListPendingForInvoices = Session("mFuelLogListPendingForInvoices")
        mFuelInvoice = Session("mFuelInvoice")
        mName = Session("mName")
    End Sub
    Private Sub SetObject()
        Dim checkString = Request.Form("chkSelect")
        ' Set Selectedvalue  
        If Not checkString Is Nothing Then
            Dim values = checkString.Split(","c)
            For Each value As String In values
                'If mFuelLogListPendingForInvoices.Contains(New Guid(value)) Then
                mFuelLogListPendingForInvoices(New Guid(value)).IsSelected = True
                'End If
            Next

            For i As Integer = 0 To mFuelLogListPendingForInvoices.Count - 1
                If mFuelLogListPendingForInvoices(i).IsSelected = True And Array.IndexOf(values, mFuelLogListPendingForInvoices(i).ID.ToString) = -1 Then
                    mFuelLogListPendingForInvoices(i).IsSelected = False
                End If
            Next
        End If
        For i As Integer = 0 To mFuelLogListPendingForInvoices.Count - 1
            If mFuelLogListPendingForInvoices(i).IsSelected = False Then
                If mFuelInvoice.FuelInvoiceLogs.Contains(LogFuelID:=mFuelLogListPendingForInvoices.Item(i).LogFuelID, str:="", str1:="") Then
                    mFuelInvoice.FuelInvoiceLogs.Remove(LogFuelID:=mFuelLogListPendingForInvoices.Item(i).LogFuelID, str:="")
                End If
            End If
        Next
        Session("mFuelInvoice") = mFuelInvoice
        Session("mFuelLogListPendingForInvoices") = mFuelLogListPendingForInvoices
    End Sub
    Private Sub FindNow()
        mFuelLogListPendingForInvoices = Flypal.FuelLogListPendingForInvoices.GetFuelLogListPendingForInvoices(FromDate:=txtFromDate.Text.Trim, ToDate:=txtToDate.Text.Trim, RegNo:=txtAircraft.Text.Trim, Departure:=txtDeparture.Text.Trim, Arrival:=txtArrival.Text.Trim)

        dgPartList.DataSource = mFuelLogListPendingForInvoices
        Session("mFuelLogListPendingForInvoices") = mFuelLogListPendingForInvoices

        If Not mFuelLogListPendingForInvoices Is Nothing Then
            For Each Child As FuelLogListPendingForInvoice In mFuelLogListPendingForInvoices
                Child.IsSelected = mFuelInvoice.FuelInvoiceLogs.Contains(LogFuelID:=Child.LogFuelID, str:="", str1:="")
                If mFuelInvoice.FuelInvoiceLogs.Contains(LogFuelID:=Child.LogFuelID, str:="", str1:="") Then
                    checkedIds.Add(Child.ID.ToString)
                End If
            Next
        End If
        dgPartList.DataBind()
        lblResult.Text = "List of Logs : " & mFuelLogListPendingForInvoices.Count & " Record(s) found."
        btnTopOk.Visible = (mFuelLogListPendingForInvoices.Count > 25)
        btnTopClose.Visible = (mFuelLogListPendingForInvoices.Count > 25)
        upnlTopActionBtn.Update()
        upnlPartDetails.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            mName = CType(Request.QueryString("Name"), String)
            txtFromDate.Text = Today.AddDays(1).AddMonths(-1).ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            FindNow()
        End If
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click, btnTopOk.Click
        SetObject()
        Session("AddParts") = "True"
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnTopClose.Click
        Session.Remove("mFuelLogListPendingForInvoices")
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    'Private Sub dgPartList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartList.PageIndexChanging
    '    SetObject()
    '    dgPartList.PageIndex = e.NewPageIndex
    '    dgPartList.DataSource = mFuelLogListPendingForInvoices
    '    dgPartList.DataBind()
    'End Sub
    Private Sub dgPartList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartList.Sorting
        mFuelLogListPendingForInvoices.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mFuelLogListPendingForInvoices") = mFuelLogListPendingForInvoices
        dgPartList.DataSource = mFuelLogListPendingForInvoices
        dgPartList.DataBind()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartList.PageIndex = 0
        FindNow()
    End Sub
#End Region

#Region "Checked Selection"
    Public Function NumeroChequeInclus(ByVal numero As String) As String
        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
    End Function
#End Region

End Class