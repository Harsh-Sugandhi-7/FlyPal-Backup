'******************************************************
'Modified by Harsh Sugandhi on 18th September 2024 for FLYPAL-2619 Cabin Defect Module.
'******************************************************


Public Class DiscrepancyTroubleShootView
	Inherits Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	'Added on 29-05-2007 by Saylee

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As Object

	Private Sub Page_Init(sender As Object, e As EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

#Region " Variable Declaration "

	Public MELSnagCorrectiveAction As MELSnagCorrectiveAction
	Public DiscrepancyTroubleShootList As DiscrepancyTroubleShootList

	Dim EventLogID As Guid
	Dim TransTypeID As Integer

#End Region

#Region " Business Methods "

	Private Sub GetSession()

		MELSnagCorrectiveAction = Session("DiscrepancyCorrectiveAction")
		TransTypeID = Session("TransTypeID")

	End Sub

	Private Sub SetSession()

		Session("DiscrepancyTroubleShootList") = DiscrepancyTroubleShootList
		Session("TransTypeID") = TransTypeID

	End Sub

	Private Sub RemoveSession()

		Session.Remove("DiscrepancyTroubleShootList")
		Session.Remove("TransTypeID")

	End Sub

	Private Overloads Sub SetFocus(control As WebControl)


		Try

			If control.Enabled = False Or control.Visible = False Then Exit Sub

			Dim str As String
			str = "document.getElementById('" + control.ClientID + "').focus();"

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"focusscript",
												str,
												True)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataBindGrid()

		Try

			DiscrepancyTroubleShootList = DiscrepancyTroubleShootList.GetDiscrepancyTroubleShootList(MELSnagCorrectiveAction.ID)
			dgDiscrepancyTroubleShootList.DataSource = DiscrepancyTroubleShootList
			dgDiscrepancyTroubleShootList.Columns(3).HeaderText = IIf(AppSettings("ClientCode") = "7AR", "Log Date (UTC)", "Log Date")
			dgDiscrepancyTroubleShootList.DataBind()

			upnlGridView.Update()

			lblTitle.Text = $"{ IIf(TransTypeID = 116, "Cabin Defect", "Discrepancy")} Troubleshooting   [ {MELSnagCorrectiveAction.DefectNo} ]"

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			GetSession()
			EventLogID = CType(Session("EventLogID"), Guid)

			TransTypeID = IIf(Request.QueryString("TransTypeID") IsNot Nothing,
							  CInt(Request.QueryString("TransTypeID")),
 							  115)

			Session("TransTypeID") = TransTypeID

			If Not IsPostBack Then
				DataBindGrid()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub ReturnToWatchListPage(sender As Object, e As EventArgs) Handles btnBack.Click

		Try

			SetSession()
			RemoveSession()

			Dim OpenAs As String = Request.QueryString("Type")

			If OpenAs IsNot Nothing AndAlso OpenAs = "pup" Then

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"onclose",
													"CallParentCallback();",
													True)
				Exit Sub

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

End Class