Public Class wfReqItemsViewForWO_Ajax
	Inherits System.Web.UI.Page

#Region " Variables and Declarations "
	Protected mRequisitionItemsNew As RequisitionItemsNew
	Protected mnWO As nWO
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mRequisitionItemsNew = Session("mRequisitionItemsNew")
		mnWO = Session("mnWO")
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		If mRequisitionItemsNew Is Nothing Then
			mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForWO(WOID:=mnWO.ID, IsForWO:=True, TransactionDate:=mnWO.WODateFormatted.ToString)
			Session("mRequisitionItemsNew") = mRequisitionItemsNew
		End If
		dgIndents.DataSource = mRequisitionItemsNew
		lblResult.Text = "List of Requisition Item(s) : " + mRequisitionItemsNew.Count.ToString + " Record(s) found."
		DataBind()
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not IsPostBack Then
			DataFieldBind()
		End If
	End Sub
	Private Sub btnCloseTop_Click(sender As Object, e As System.EventArgs) Handles btnCloseTop.Click
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
	End Sub
	'Added by vikrant on 19-Sep-2019
	Private Sub dgIndents_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgIndents.RowCommand
		Select Case e.CommandName
			Case "ReqNo"
				Dim mRequisitionNew As RequisitionNew
				mRequisitionNew = RequisitionNew.GetRequisition(New Guid(e.CommandArgument.ToString))
				Session("mRequisitionNew") = mRequisitionNew
				Dim ReqURLFromWO As New Stack
				ReqURLFromWO.Push(Request.Url)
				Session("ReqURLFromWO") = ReqURLFromWO
				Session("MiddleFrameForWO") = Session("MiddleFrame")
				Session("TransTypeID") = CInt(Util.Trans.EngineeringRequisition)
				Response.Redirect("wfRequisition_Ajax.aspx?BackPage=wfnWODetail_AJAX.aspx")
		End Select
	End Sub
	'End
#End Region

End Class