Imports System.Linq
Imports System.Linq.Enumerable

Public Class wfRemoveSelectedOrderFromRequisitionItem_Ajax
	Inherits Page

#Region " Variable Declaration "
	Public mDistinctOverhaulRepairOrderText As DistinctOverhaulRepairOrderText
	Dim EventLogID As Guid
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mDistinctOverhaulRepairOrderText = CType(Session("mDistinctOverhaulRepairOrderText"), DistinctOverhaulRepairOrderText)
	End Sub

	Private Sub SetSession()
		Session("mDistinctOverhaulRepairOrderText") = mDistinctOverhaulRepairOrderText
	End Sub

	Private Sub RemoveSessions()
		Session.Remove("mOrderList")
	End Sub

	Private Sub DataAccess(RequisitionItemID As Guid, Optional ForWhat As String = "", Optional RequisitionNo As String = "")
		Dim conString As String = ConfigurationManager.AppSettings("DB:FlyPal")
		Dim con = New SqlConnection(conString)
		Try
			con.Open()
			Dim cmd As New SqlCommand()
			cmd.Connection = con
			cmd.CommandType = CommandType.StoredProcedure
			If ForWhat = "Remove" Then
				cmd.CommandText = "RequisitionItemListFetch"
				cmd.Parameters.AddWithValue("@OrderId", New Guid(cmbOrder.SelectedValue.ToString))
				cmd.Parameters.AddWithValue("@RequisitionItemID", RequisitionItemID)
				cmd.Parameters.AddWithValue("@ForWhat", ForWhat)
				Dim Dr As New SafeDataReader(cmd.ExecuteReader)
			End If
		Catch ex As Exception
			Throw ex.GetBaseException
		Finally
			con.Close()
			MarkLog(Action.Remove, "RemoveSelectedOrderFromRequisitionItem", "Selected order removed from requisition " + RequisitionNo + " .By User " + User.Identity.Name, Util.ErrorType.NoError, RequisitionItemID, EventLogID)
		End Try
	End Sub
	Private Function CreateDataTable(Optional ByVal ForWhat As String = "") As DataTable
		Dim dataTable As New DataTable("TMainReport")
		Dim conString As String = System.Configuration.ConfigurationManager.AppSettings("DB:FlyPal")

		Dim con = New SqlConnection(conString)

		con.Open()

		Dim cmd As New SqlCommand()
		cmd.Connection = con
		cmd.CommandText = "RequisitionItemListFetch"
		cmd.CommandType = CommandType.StoredProcedure
		cmd.Parameters.AddWithValue("@OrderId", New Guid(cmbOrder.SelectedValue.ToString))
		cmd.Parameters.AddWithValue("@RequisitionItemID", Guid.Empty)
		cmd.Parameters.AddWithValue("@ForWhat", ForWhat)
		Dim adaptor = New SqlDataAdapter

		adaptor.SelectCommand = cmd
		adaptor.Fill(dataTable)
		con.Close()
		Return dataTable
	End Function
	Private Sub GenerateList(ByVal tbl As DataTable)

		dgRequisitionItemList.DataSource = tbl
		dgRequisitionItemList.DataBind()
		Session("tbl") = tbl
		upnlGridView.Update()

	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
		cntrl.Focus()
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Delete" Then
					End If
				Case MsgBoxResult.No
			End Select
		End If
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBindList()
		cmbOrder.DataSource = DistinctOverhaulRepairOrderText.GetOrderList(IsSelectTagRequired:=True, Tag:="(SELECT)", FromDate:="01-Jan-1900", ToDate:="01-Jan-4400", IsRequisionNew:=False)
		cmbOrder.DataBind()
	End Sub
#End Region

#Region " Events "

	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not IsPostBack And Session("Sender") = "" Then
			DataFieldBindList()
		End If
		SetSession()
	End Sub

	Private Sub cmbOrder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOrder.SelectedIndexChanged
		GenerateList(CreateDataTable(ForWhat:="FetchList"))
	End Sub

	Private Sub dgRequisitionItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRequisitionItemList.RowCommand
		Select Case e.CommandName
			Case "Remove"
				Dim mID As Guid = New Guid(dgRequisitionItemList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)
				Dim RequisitionNo As String = dgRequisitionItemList.DataKeys(CInt(e.CommandArgument)).Values("RequisitionNo").ToString
				DataAccess(RequisitionItemID:=mID, ForWhat:="Remove", RequisitionNo:=RequisitionNo)
				GenerateList(CreateDataTable(ForWhat:="FetchList"))
		End Select
	End Sub
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		Session("MiddleFrame") = ""
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub

#End Region

#Region "Service Methods"
	<System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
	Public Shared Function GetOrderList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim tmpOrderlist As OrderList
		Dim OrderText As String()
		OrderText = prefixText.Split("-")

		tmpOrderlist = OrderList.GetOrderList(, "", , , "", "1-1-1850", "1-1-2200", , , "")

		If count = 0 Then
			Return (From c As OrderList.OrderInfo In tmpOrderlist Where c.OrderNo.Contains(prefixText.ToString.ToUpper)
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.OrderNo, c.ID.ToString())).ToArray
		Else
			Return (From c As OrderList.OrderInfo In tmpOrderlist Where c.OrderNo.Contains(prefixText.ToString.ToUpper)
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.OrderNo, c.ID.ToString())).Take(count).ToArray

		End If
	End Function
#End Region

End Class