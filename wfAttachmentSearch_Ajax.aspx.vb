Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports Newtonsoft.Json
Public Class wfAttachmentSearch_Ajax
    Inherits Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As Object

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Enumaration "
    Private Enum Rights

        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6

    End Enum

#End Region

#Region " Variable Declaration "
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Overloads Sub SetFocus(control As WebControl)
        If control.Enabled = False Or control.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Delete" Then
					ElseIf MSGBoxCtrl.Sender = "IsScheduledJobExists" Then
						Session("sender") = ""
					End If
				Case MsgBoxResult.No
					Session("sender") = ""
				Case MsgBoxResult.Ok
					Session("sender") = ""
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
			Session("sender") = ""
        End If
    End Sub

#End Region

#Region "DataFieldBind"
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "GetAttachmentsWithSearch"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue(parameterName:="@Month", value:=0)
        cmd.Parameters.AddWithValue(parameterName:="@Year", value:=CType(cmbYear.SelectedItem.Text, Integer))
        cmd.Parameters.AddWithValue(parameterName:="@SearchText", value:=txtSearch.Text.Trim)

        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()
		Return dataTable
	End Function
	Private Sub DataFieldBind(tbl As DataTable)
		DataBind()
	End Sub
	Private Sub SetCombo()
        If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
            For i As Integer = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If

		For k As Integer = 1 To 12
			Dim mon As String = MonthName(k, False)
		Next
	End Sub
	Private Sub BindTree()
		Dim json = GetHierarchicalJson()
		Dim data As List(Of YearData) = JsonConvert.DeserializeObject(Of List(Of YearData))(json)

		TreeView1.Nodes.Clear()
		Dim yearItem, monthItem, grp, f
		For Each yearItem In data
			Dim yearNode As New TreeNode(yearItem.Year.ToString())

			For Each monthItem In yearItem.Months
				Dim monthNode As New TreeNode(monthItem.Month)

				For Each grp In monthItem.Groups
					Dim textNode As New TreeNode(grp.TextNo, grp.TransID.ToString())

					For Each f In grp.Files
						Dim fileNode As New TreeNode(f.FileName, f.AttachmentID.ToString())
						textNode.ChildNodes.Add(fileNode)
					Next

					monthNode.ChildNodes.Add(textNode)
				Next

				yearNode.ChildNodes.Add(monthNode)
			Next

			TreeView1.Nodes.Add(yearNode)
		Next

		TreeView1.ExpandDepth = 1
		upnlTreeView.Update()
	End Sub
	Private Function GetHierarchicalJson() As String
		Dim dt As DataTable = CreateDataTable()

		Dim query = From row In dt.AsEnumerable()
					Group row By yr = row.Field(Of Integer)("Year") Into gYear = Group
					Select New With {
					.Year = yr,
					.Months = (From m In gYear
							   Group m By mn = m.Field(Of String)("MonthName") Into gMonth = Group
							   Select New With {
								   .Month = mn,
								   .Groups = (From a In gMonth
											  Group a By trans = a.Field(Of Guid)("TransID") Into gTrans = Group
											  Select New With {
												  .TransID = trans,
												  .TextNo = gTrans.First().Field(Of String)("Text") &
															" - " &
															gTrans.First().Field(Of Integer)("No").ToString() & " (" &
															gTrans.First().Field(Of String)("SourceType") & ")",
												  .Files = (From f In gTrans
															Select New With {
																.AttachmentID = f.Field(Of Guid)("AttachmentID"),
																.FileName = If(String.IsNullOrEmpty(f.Field(Of String)("FileName")),
																			   "<img src='icons/Attachment.png' alt='Attachment' class='clip-icon' />",
																			   f.Field(Of String)("FileName") + " " + "<img src='icons/Attachment.png' alt='Attachment' class='clip-icon' />")
															}).ToList()
											  }).ToList()
							   }).ToList()
				}

		Return JsonConvert.SerializeObject(query, Formatting.Indented)
	End Function


#End Region

#Region "Events"

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
			SetCombo()
			DataFieldBind(CreateDataTable())
            BindTree()
        End If
    End Sub
	Private Sub btnCloseTop_Click(sender As Object, e As EventArgs) Handles btnCloseTop.Click
		Session("MiddleFrame") = ""
		Session.Remove("IsReadOnly")
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub


    Private Sub cmbYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbYear.SelectedIndexChanged, txtSearch.TextChanged
        DataFieldBind(CreateDataTable())
        BindTree()
    End Sub
	Protected Sub TreeView1_SelectedNodeChanged(sender As Object, e As EventArgs)
		Dim selectedNode As TreeNode = TreeView1.SelectedNode

		' Only handle attachment level (leaf nodes that have a TransID)
		If selectedNode.ChildNodes.Count = 0 Then
			Dim ID As Guid = Guid.Parse(selectedNode.Value)

			Dim mFileAttach As FileAttach
			Dim No As New Random
			Dim StrName As String = "abc" & No.Next.ToString

			mFileAttach = FileAttach.GetAttachment(ID:=ID, ReferenceID:=Guid.Empty, IsByID:=True)
			If mFileAttach.Size > 0 Then
				Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
				Dim fs As FileStream
				If File.Exists(AppSettings("DOCPath")) = False Then
					'Delete File if exist
					IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
					' Create the file.
					fs = File.Create(path)
					'' Add some information to the file.
					fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
					fs.Close()
					Session("DOCPath") = path
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
					Dim Detail As String = "Attachment( " + mFileAttach.FileName + ") viewed by  " + User.Identity.Name
					MarkLog(Action.View, "Attachment", Detail, ErrorType.HandledError, TransID:=ID, EventLogID)
				End If
			End If

		End If
	End Sub
#End Region

End Class

Public Class YearData
    Public Property Year As Integer
    Public Property Months As List(Of MonthData)
End Class
Public Class MonthData
    Public Property Month As String
    Public Property Groups As List(Of GroupData)
End Class
Public Class GroupData
    Public Property TransID As Guid
    Public Property TextNo As String
    Public Property Files As List(Of FileData)
End Class
Public Class FileData
    Public Property AttachmentID As Guid
    Public Property FileName As String
End Class
