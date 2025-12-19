'CREATED By : Saylee
'Dated      : 8-Jan-2013


Public Class wfnIssuedWOTools_AJAX
	Inherits System.Web.UI.Page


#Region "Variable Declarations"
	Protected mIssuedWOTools As nIssuedWOTools
	Dim WOID As String
	Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mIssuedWOTools = Session("mIssuedWOTools")
	End Sub
	Private Sub SetSession()
		Session("mIssuedWOTools") = mIssuedWOTools
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)

	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		'Get List From the Database as per Criteria             
		mIssuedWOTools = nIssuedWOTools.GetnIssuedWOTools(New Guid(WOID.ToString), ClientCode:=AppSettings("ClientCode"))
		dgIssuedTools.DataSource = mIssuedWOTools
		Session("mIssuedWOTools") = mIssuedWOTools
		dgIssuedTools.DataBind()
		upnlGrid.Update()
	End Sub

#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		GetSession()
		If Not IsPostBack Then
			WOID = Session("WOID") 'Request.QueryString("WOID")
			DataFieldBind()
		End If
	End Sub
	Private Sub dgIssuedTools_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgIssuedTools.Sorting
		mIssuedWOTools.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgIssuedTools.DataSource = mIssuedWOTools
		Session("mIssuedWOTools") = mIssuedWOTools
		dgIssuedTools.DataBind()
		upnlGrid.Update()
	End Sub
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
		'End

		Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
	End Sub
	Private Sub dgIssuedTools_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgIssuedTools.RowCommand
		Dim Index As Integer
		Dim ReceiptItemID As Guid
		Select Case e.CommandName
			Case "ViewRec"
				Index = CInt(e.CommandArgument)
				ReceiptItemID = mIssuedWOTools(Index).ReceiptItemID
				' If condition Added by Shital on 29-Jun-2020
				Dim mFileAttachments As FileAttachments
				mFileAttachments = FileAttachments.GetChildFileAttachments(ReceiptItemID)
				Dim AttachmentCount As Integer = mFileAttachments.Count
				If AttachmentCount > 1 Then

					Session("mFileAttachments") = mFileAttachments
					Session("TransactionNameMarkLog") = "Issued WO Tools"
					Session("TransactionName") = "Issue No.and Date"
					Session("TransactionDetails") = mIssuedWOTools(Index).IssueNo + " & " + mIssuedWOTools(Index).IDateFormatted
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAttachWindow", "OpenAttachWindow();", True)

				Else

					Dim No As New Random
					Dim StrName As String = "abc" & No.Next.ToString
					mFileAttach = FileAttach.GetAttachment(ReceiptItemID)
					Session("mFileAttach") = mFileAttach
					If mFileAttach.Size > 0 Then
						Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
						Dim fs As FileStream
						If File.Exists(AppSettings("DOCPath")) = False Then
							'Delete File if exist
							System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
							' Create the file.
							fs = File.Create(path)
							'' Add some information to the file.
							fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
							fs.Close()
							Session("DOCPath") = path
							ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
						End If
					End If
				End If
		End Select
	End Sub
#End Region
End Class