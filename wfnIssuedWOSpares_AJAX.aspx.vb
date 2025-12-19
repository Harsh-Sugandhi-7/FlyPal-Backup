'CREATED By : Saylee
'Dated      : 26-Dec-2013

Public Class wfnIssuedWOSpares_AJAX
	Inherits System.Web.UI.Page


#Region "Variable Declarations"
	Public mnIssuedWOSpares As nIssuedWOSpares
	Public mWOID As Guid = Guid.Empty
	Protected mnWO As nWO
	Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mnIssuedWOSpares = Session("mnIssuedWOSpares")
		mWOID = Session("mWOID")
		mnWO = Session("mnWO")
	End Sub
	Private Sub SetSession()
		Session("mnIssuedWOSpares") = mnIssuedWOSpares
		Session("mWOID") = mWOID
		Session("mnWO") = mnWO
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Function CustomValidate1(ByVal index As Integer) As Boolean
		Dim strMSG As String = ""
		If Not mnIssuedWOSpares(index).IsValid Then
			For i As Integer = 0 To mnIssuedWOSpares(index).GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnIssuedWOSpares(index).GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If
		If strMSG.Trim <> "" Then
			cvControlValidator.ErrorMessage = strMSG
			cvControlValidator.IsValid = False
			upnlValidationSummary.Update()
			Return False
		End If
		Return True
	End Function
	Private Sub ControlVisibility()
		btnSave.Enabled = IIf(mnWO.WOStatusID = 3, False, True) And IIf(mnWO.StatusID = 4, False, True) And dgIssuedSpares.Rows.Count > 0

		Dim item As GridViewRow
		Dim txtBox As TextBox
		Dim Recordno, PageItems As Integer
		Dim i As Integer
		PageItems = dgIssuedSpares.Rows.Count - 1
		' Set Selected Notes value  
		For i = 0 To PageItems
			Recordno = i + dgIssuedSpares.PageSize * dgIssuedSpares.PageIndex
			item = dgIssuedSpares.Rows(i)
			txtBox = CType(item.FindControl("txtBox"), TextBox)
			txtBox.ReadOnly = IIf(mnWO.WOStatusID <> 3, False, True) And IIf(mnWO.StatusID <> 4, False, True)
		Next
		upnlGrid.Update()
	End Sub
	Private Function AddSpares() As Boolean
		Dim item As GridViewRow
		Dim txtBox As TextBox
		Dim Recordno, PageItems As Integer
		Dim i As Integer
		PageItems = dgIssuedSpares.Rows.Count - 1
		' Set Selected Notes value  
		For i = 0 To PageItems
			Recordno = i + dgIssuedSpares.PageSize * dgIssuedSpares.PageIndex
			item = dgIssuedSpares.Rows(i)
			txtBox = CType(item.FindControl("txtBox"), TextBox)
			If IsNumeric(txtBox.Text) And Val(txtBox.Text) >= 0 Then
				mnIssuedWOSpares(Recordno).UsedQty = CType(txtBox.Text, Decimal)
				Dim strMSG As String = ""
				If Not mnIssuedWOSpares(Recordno).IsValid Then
					For j As Integer = 0 To mnIssuedWOSpares(Recordno).GetBrokenRulesCollection.Count - 1
						strMSG = strMSG + mnIssuedWOSpares(Recordno).GetBrokenRulesCollection(j).Description + "<Br>"
					Next
				End If
				If strMSG.Trim <> "" Then
					cvControlValidator.ErrorMessage = strMSG
					cvControlValidator.IsValid = False
					upnlValidationSummary.Update()
					Return False
				End If
			Else
				If Not IsNumeric(txtBox.Text) Then
					cvControlValidator.ErrorMessage = mnIssuedWOSpares(Recordno).IssueNumber + "(" + mnIssuedWOSpares(Recordno).PartNo + ")" + " => Used quantity should be numeric value"
					cvControlValidator.IsValid = False
					upnlValidationSummary.Update()
					Return False
				ElseIf Val(txtBox.Text) < 0 Then
					cvControlValidator.ErrorMessage = mnIssuedWOSpares(Recordno).IssueNumber + "(" + mnIssuedWOSpares(Recordno).PartNo + ")" + " => Used quantity should not be -ve"
					cvControlValidator.IsValid = False
					upnlValidationSummary.Update()
					Return False
				End If

			End If
		Next

		dgIssuedSpares.DataSource = mnIssuedWOSpares
		dgIssuedSpares.DataBind()
		Session("mnIssuedWOSpares") = mnIssuedWOSpares
		upnlValidationSummary.Update()
		Return True

	End Function
	Private Sub Save()
		Dim mIssue As Issue
		Dim i As Integer = 0
		While i < mnIssuedWOSpares.Count
			If Not mnIssuedWOSpares.Item(i).IsValid Then
				If Not CustomValidate1(i) Then
					Exit Sub
				End If
			End If
			i = i + 1
		End While

		i = 0
		While i < mnIssuedWOSpares.Count
			If mnIssuedWOSpares.Item(i).IsValid Then
				If mnIssuedWOSpares.Item(i).IsDirty = True Then
					mIssue = Issue.GetIssue(mnIssuedWOSpares.Item(i).ID)
					mIssue.IssueItems(mnIssuedWOSpares.Item(i).IssueItemID).WOUsedQty = mnIssuedWOSpares.Item(i).UsedQty
					mIssue.IssueItems(mnIssuedWOSpares.Item(i).IssueItemID).WOReturnQty = mnIssuedWOSpares.Item(i).ReturnQty

					If mIssue.IsDirty And mIssue.IsValid Then
						mIssue.Save()
					Else
						Dim strMSG As String = ""
						If Not mIssue.IsValid Then
							For j As Integer = 0 To mIssue.GetBrokenRulesCollection.Count - 1
								strMSG = strMSG + mIssue.GetBrokenRulesCollection(j).Description + "<Br>"
							Next
						End If
						If strMSG.Trim <> "" Then
							cvControlValidator.ErrorMessage = strMSG
							cvControlValidator.IsValid = False
							upnlValidationSummary.Update()
						End If
					End If
				End If
			End If
			i = i + 1
		End While
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		mnIssuedWOSpares = nIssuedWOSpares.GetIssuedWOSpares(mWOID, ClientCode:=AppSettings("ClientCode"))
		dgIssuedSpares.DataSource = mnIssuedWOSpares
		Session("mnIssuedWOSpares") = mnIssuedWOSpares
		dgIssuedSpares.DataBind()

		upnlGrid.Update()
	End Sub
#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		If Not IsPostBack Then
			' mWOID = New Guid(CType(Request.QueryString("WOID"), String))
			Session("mWOID") = mnWO.ID
			setFocus(dgIssuedSpares)
			DataFieldBind()
		End If
		ControlVisibility()
	End Sub
	Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
		If AddSpares() = True Then
			Save()
		End If
		upnlValidationSummary.Update()
	End Sub
	Private Sub dgIssuedSpares_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgIssuedSpares.Sorting
		mnIssuedWOSpares.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgIssuedSpares.DataSource = mnIssuedWOSpares
		Session("mnIssuedWOSpares") = mnIssuedWOSpares
		dgIssuedSpares.DataBind()
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
	Private Sub dgIssuedSpares_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgIssuedSpares.RowCommand
		Dim Index As Integer
		Dim ReceiptItemID As Guid
		Select Case e.CommandName
			Case "ViewRec"
				Index = CInt(e.CommandArgument)
				ReceiptItemID = mnIssuedWOSpares(Index).ReceiptItemID
				' If condition Added by Shital on 29-Jun-2020
				Dim mFileAttachments As FileAttachments
				mFileAttachments = FileAttachments.GetChildFileAttachments(ReceiptItemID)
				Dim AttachmentCount As Integer = mFileAttachments.Count
				If AttachmentCount > 1 Then

					Session("mFileAttachments") = mFileAttachments
					Session("TransactionNameMarkLog") = "Issued WO Spares"
					Session("TransactionName") = "Issue No.and Date"
					Session("TransactionDetails") = mnIssuedWOSpares(Index).IssueNo + " & " + mnIssuedWOSpares(Index).IssueDateFormatted
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