Partial Class wfnWOReturnIssueList
	Inherits Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	''Protected WithEvents cmbWoText As DropDownList

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
	Public mIssueList As IssueList
	Public mIssue As Issue
	Public mDistinctTextListForIssue As DistinctTextListForIssue
	Public mDistinctTextListForReceipt As DistinctTextListForReceipt
	Dim objSearch As rptSearchingCriteriaForReceipt
	Dim objReg As rptIssueReg
	Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, IssueText, ReceiptText, WOText, IssueTypeId, Name, No, IssueTo, IssueAs As String
	Dim mTransTypeID As Trans
	Dim mTransTypeList As TransactionList
	Public ModuleName As String
	Public Tital As String
	Public mIssueTypeList As IssueTypeList
	'Rajnish 19-08-2008
	' Public mWOList As FlyPal22.Maintain.WOList
	Dim mDistinctWOText As nDistinctWOText

	Dim EventLogID As Guid
	Dim mIssueDetail As String
	Dim totcnt As Integer 'Added by shweta on 23-12-11

	Dim DateFormat As String = AppSettings("DateFormat").ToString()
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mIssueTypeList = Session("mIssueTypeList")
		mIssue = Session("mIssue")
		mIssueList = Session("mIssueList")
		mTransTypeID = Session("mTransTypeID")
		mDistinctTextListForIssue = Session("mDistinctTextListForIssue")
		mDistinctTextListForReceipt = Session("mDistinctTextListForReceipt")
		'Rajnish 19-08-2008
		'' mWOList = Session("mWOList")
		'========
		SearchIndex = Session("SearchIndex")
		'SearchIndex1 = Session("SearchIndex1")
		DateIndex = Session("DateIndex")
		FromDate = Session("FromDate")
		ToDate = Session("ToDate")
		StatusId = Session("StatusId")
		IssueTypeId = Session("IssueTypeId")
		IssueText = Session("IssueText")
		ReceiptText = Session("ReceiptText")
		'Rajnish 09-08-2008
		WOText = Session("WOText")
		Name = Session("Name")
		No = IIf(IsNothing(Session("No")), 0, Session("No"))
		ModuleName = Session("ModuleName")
		IssueTo = Session("IssueTo")
		IssueAs = Session("IssueAs")
		mDistinctWOText = Session("mDistinctWOText")
	End Sub
	Private Sub SetSession()
		Session("mIssueTypeList") = mIssueTypeList
		Session("mIssue") = mIssue
		Session("mIssueList") = mIssueList
		'Rajnish 19-08-2008
		''Session("mWOList") = mWOList
		Session("mTransTypeID") = mTransTypeID
		Session("mDistinctTextListForIssue") = mDistinctTextListForIssue
		Session("mDistinctTextListForReceipt") = mDistinctTextListForReceipt
		Session("ModuleName") = ModuleName
		Session("IssueTo") = IssueTo
		Session("IssueAs") = IssueAs
		Session("mDistinctWOText") = mDistinctWOText
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mIssue")
		Session.Remove("mIssueList")
		'Rajnish 19-08-2008
		''Session.Remove("mWOList")
		'============
		Session.Remove("mDistinctTextListForIssue")
		Session.Remove("mDistinctTextListForReceipt")
		Session.Remove("SearchIndex")
		'Session.Remove("SearchIndex1")
		Session.Remove("DateIndex")
		Session.Remove("FromDate")
		Session.Remove("ToDate")
		Session.Remove("StatusId")
		Session.Remove("IssueTypeId")
		Session.Remove("IssueText")
		Session.Remove("ReceiptText")
		'Rajnish 09-08-2008
		Session.Remove("WOText")

		Session.Remove("Name")
		Session.Remove("No")
		'Added on 22-01-2007 ''  value of machine is not refresing (SearcCriteriaforFlyingHours.aspx)
		Session.Remove("mMachineList")
		'Session.Remove("SelectedIndex")
		Session.Remove("mTransTypeId")
		Session.Remove("mIssueTypeList")
		Session.Remove("IssueTo")
		Session.Remove("IssueAs")
		Session.Remove("mDistinctWOText")
		Session.Remove("totcnt")
	End Sub
	Private Sub ClearAll()
		If InStr(Session("MiddleFrame"), "wfnWOReturnIssueList.aspx?") <= 0 Then
			RemoveSession()
			Session.Remove("mOrder")
		End If
	End Sub
	Private Sub NewRecord()
		mIssue = Issue.NewIssue(mTransTypeID)
		mIssue.IDate = Today.Date
		If mTransTypeID = 16 Or mTransTypeID = 18 Or mTransTypeID = 49 Or mTransTypeID = 51 Or mTransTypeID = 55 Or mTransTypeID = 58 Or mTransTypeID = 59 Or mTransTypeID = 60 Then  '55, 58 Added By Prashant 6-Jan-2010 
			mIssue.IssueItems.Add(mIssue.ID, mTransTypeID)
			mIssue.IssueItems.CurrentIndex = mIssue.IssueItems.Count - 1
		End If
		Session("mIssue") = mIssue
	End Sub
	Private Sub EditRecord(mId As Guid)
		mIssue = Issue.GetIssue(mId)
		mIssue.MarkClean()
		Session("mIssue") = mIssue

		Dim mTransTypeList As TransactionList
		mTransTypeList = TransactionList.GetTransactionList()
		ModuleName = mTransTypeList.GetTransactionTypeName(mIssue.TransTypeID).ToString
		Session("ModuleName") = ModuleName
		Session("mIssue") = mIssue

	End Sub
	Private Sub DeleteRecord(mId As Guid)
		Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
		msg1.ReplacePage = "wfnWOReturnIssueList.aspx?BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID
		Session("sender") = "Delete"
		msg1.Show()
		mIssue = Issue.GetIssue(mId)
		Session("mIssue") = mIssue
	End Sub
	Private Sub DataFieldBind()
		Session("totcnt") = totcnt 'Added by shweta on 22-12-11
		FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
		ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
		SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
		DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
		StatusId = Session("StatusId")
		IssueText = Session("IssueText")
		ReceiptText = Session("ReceiptText")
		'Rajnish 19-08-2008
		WOText = Session("WOText")

		IssueTypeId = Session("IssueTypeId")
		Name = Session("Name")
		mDistinctTextListForIssue = DistinctTextListForIssue.GetDistinctText("3", , True, "(All)")
		mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("2", , True, "(All)")
		cmbIssueText.DataSource = mDistinctTextListForIssue
		cmbReceiptText.DataSource = mDistinctTextListForReceipt
		'Rajnish 19-08-2008
		'Commented by Saylee 31-Jan-2011
		''mWOList = FlyPal22.Maintain.WOList.GetWOList(, , , New SmartDate("01-01-1800").FormattedText, New SmartDate("01-01-2200").FormattedText, , , , , , , , , "(All)")
		''cmbWoText.DataSource = mWOList
		''Session("mWOList") = mWOList
		mDistinctWOText = nDistinctWOText.GetDistinctWOText("(All)")
		cmbWoText.DataSource = mDistinctWOText
		Session("mDistinctWOText") = mDistinctWOText
		'===============
		mIssueList = IssueList.GetIssueList(, 0, "1/1/1900", "1/1/2200", , , , 0, 0, , 0, , , , mTransTypeID, , , , , True)
		gvIssueList.DataSource = mIssueList
		Session("mIssueList") = mIssueList

		totcnt = mIssueList.Count 'Added by shweta on 22-12-11
		Session("totcnt") = totcnt 'Added by shweta on 22-12-11

		mIssueTypeList = IssueTypeList.GetIssueTypeList(0)
		Session("mIssueTypeList") = mIssueTypeList
		DataBind()
		lblResult.Text = "List of Issue as per criteria : " & mIssueList.Count & " Record(s) found."
	End Sub
	Private Sub DataFieldBindForSymco() 'Added by Saylee on 28-July-2010
		FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
		ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
		SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
		DateIndex = IIf(IsNothing(DateIndex), 2, DateIndex)
		StatusId = Session("StatusId")
		IssueText = Session("IssueText")
		ReceiptText = Session("ReceiptText")
		'Rajnish 19-08-2008
		WOText = Session("WOText")

		IssueTypeId = Session("IssueTypeId")
		Name = Session("Name")
		mDistinctTextListForIssue = DistinctTextListForIssue.GetDistinctText("3", , True, "(All)")
		mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("2", , True, "(All)")
		cmbIssueText.DataSource = mDistinctTextListForIssue
		cmbReceiptText.DataSource = mDistinctTextListForReceipt
		'Rajnish 19-08-2008
		'Commented by Saylee 31-Jan-2011
		''mWOList = FlyPal22.Maintain.WOList.GetWOList(, , , New SmartDate("01-01-1800").FormattedText, New SmartDate("01-01-2200").FormattedText, , , , , , , , , "(All)")
		''cmbWoText.DataSource = mWOList
		''Session("mWOList") = mWOList
		mDistinctWOText = nDistinctWOText.GetDistinctWOText("(All)")
		cmbWoText.DataSource = mDistinctWOText
		Session("mDistinctWOText") = mDistinctWOText
		'===============
		mIssueList = IssueList.GetIssueList(, 0, "1/1/1900", "1/1/2200", , , , 0, 0, , 0, , , , mTransTypeID, , , , , True)
		gvIssueList.DataSource = mIssueList
		Session("mIssueList") = mIssueList

		mIssueTypeList = IssueTypeList.GetIssueTypeList(0)
		Session("mIssueTypeList") = mIssueTypeList
		DataBind()
		lblResult.Text = "List of Issue as per criteria : " & mIssueList.Count & " Record(s) found."
	End Sub
	Private Overloads Sub setFocus(cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Dim msgCount As Integer = 0
		If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
			Result1 = -1
		Else
			Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
		End If
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If CType(Session("sender"), String) = "Delete" Then
						Try
							Dim mIssue As Issue
							Session("sender") = ""
							mIssue = CType(Session("mIssue"), Issue)
							'mIssue.DeleteIssue(mIssue.ID)
							''If ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") <> "Indamer") And (mIssue.IsSync <> 1 Or mIssue.IsSync <> 2) Then
							''    mIssue.Delete()
							''    mIssue.Save()
							''    Response.Redirect("wfnWOReturnIssueList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID)
							''Else
							''    Dim msg1 As New SIMsgBox(Page, "Alert!", "This Transaction cannot be deleted. Already sent for billing.", "", MsgBoxStyle.OKOnly)
							''    msg1.ReplacePage = "wfnWOReturnIssueList.aspx?BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID
							''    DataFieldBindForSymco()
							''    SetControl()
							''    msg1.Show()
							''    Exit Sub
							''End If
							If ((AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer") Then
								If (mIssue.IsSync = 1 Or mIssue.IsSync = 2) Then
									Dim msg1 As New SIMsgBox(Page, "Alert!", "This Transaction cannot be deleted. Already sent for billing.", "", MsgBoxStyle.OkOnly)
									msg1.ReplacePage = "wfnWOReturnIssueList.aspx?BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID
									DataFieldBindForSymco()
									SetControl()
									msg1.Show()
									Exit Sub
								Else
									mIssue.Delete()
									mIssue.Save()
									Response.Redirect("wfnWOReturnIssueList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID)
								End If
							Else
								mIssue.Delete()
								mIssue.Save()
								Response.Redirect("wfnWOReturnIssueList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID)
							End If
						Catch ex As SqlException
							If ex.Number = 8145 Then
								Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
								msg1.ReplacePage = "wfnWOReturnIssueList.aspx?BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID
								msg1.Show()
							ElseIf ex.Number = 2627 Then
								Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
								msg1.ReplacePage = "wfnWOReturnIssueList.aspx?BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID
								msg1.Show()
							ElseIf ex.Number = 547 Then
								Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
								msg1.ReplacePage = "wfnWOReturnIssueList.aspx?BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID
								'MarkLog(Util.Action.Delete, ModuleName, "Can't delete : This is Currently in use", Util.ErrorType.NoError, mIssue.ID)
								msg1.Show()
							End If
							DataFieldBind()
							SetControl()
							msgCount = ex.Errors.Count
						Finally
							If msgCount = 0 Then
								'MarkLog(Util.Action.Delete, ModuleName, mIssue.IssueNo, Util.ErrorType.NoError, mIssue.ID)
							End If
						End Try
					End If
				Case MsgBoxResult.No
					Session("sender") = ""
					Response.Redirect("wfnWOReturnIssueList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID)
				Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
					Session("sender") = ""
					DataFieldBind()
					Response.Redirect("wfnWOReturnIssueList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID)
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
					DataFieldBind()
					Response.Redirect("wfnWOReturnIssueList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID)
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			Response.Redirect("wfnWOReturnIssueList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID)

		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
			Session("sender") = ""
			DataFieldBind()
		End If
	End Sub
	Private Sub FindNow(Optional Text As String = "", Optional No As Integer = 0, Optional FromDate As String = "1-Jan-1900", Optional ToDate As String = "1-Jan-2099", Optional StoreName As String = "", Optional VendorName As String = "", Optional AircraftName As String = "", Optional ToTypeId As Int32 = 0, Optional StatusID As Int32 = 0, Optional ReceiptText As String = "", Optional ReceiptNo As Int32 = 0, Optional RealeaseNoteNo As String = "", Optional SerialNo As String = "", Optional ItemName As String = "", Optional WorkShop As String = "", Optional WOText As String = "", Optional WONo As Int32 = 0)
		mIssueList = Nothing
		gvIssueList.DataSource = Nothing

		Dim IsVendor As Integer
		If ToTypeId = 1 Then
			IsVendor = 1
		ElseIf ToTypeId = 15 Then
			IsVendor = 2
		Else
			IsVendor = 0
		End If
		'Get List From the Database as per Criteria             
		mIssueList = IssueList.GetIssueList(Text, No, FromDate, ToDate, StoreName, VendorName, AircraftName, ToTypeId, StatusID, ReceiptText, ReceiptNo, RealeaseNoteNo, SerialNo, ItemName, , IsVendor, WorkShop, WOText, WONo, True)
		'Set DataSource of the Grid
		Session("mIssueList") = mIssueList
		gvIssueList.DataSource = mIssueList
		'Set Mapping Name 
	End Sub
	Private Sub CallFindNow(Index As Integer)
		Select Case Index
			Case -1 'all
				FindNow()
			Case 0 'all
				FindNow()
			Case 1 'issue date
				'FindNow(, , FromDate, ToDate)
				FindNow(, , FromDate_Txt.Text.ToString, ToDate_Txt.Text.ToString)
			Case 2  'issue no
				FindNow(IssueText, CInt(Val(No)))
			Case 3  'Receipt no
				FindNow(, , , , , , , , , ReceiptText, CInt(Val(No)))
			Case 4 'Item name
				FindNow(, , , , , , , , , , , , , Trim(Name))
			Case 5  'Store Name
				FindNow(, , , , Trim(Name))
			Case 6  'Vendor name
				FindNow(, , , , , Trim(Name), , 1)
			Case 7  'Aircraft name
				FindNow(, , , , , , Trim(Name), 2)
				'Case 8  'Totype 1 -vendor 2 aircraft 3 discard
				'    FindNow(, , , , , , , IssueTypeId)
			Case 8  'Release note no
				FindNow(, , , , , , , , , , , Trim(Name))
			Case 9 'serial no
				FindNow(, , , , , , , , , , , , Trim(Name))
			Case 10  'Status 1-incomplete 2-complete 3 authorize 4 cancel
				FindNow(, , , , , , , , StatusId)
			Case 11 'WorkShop
				FindNow(, , , , , , , 16, , , , , , , Trim(Name))
			Case 12 'Work Order
				FindNow(, , , , , , , 17, , , , , , , , WOText, CInt(Val(No)))
		End Select
		gvIssueList.PageIndex = 0   'Added Code on May,25,2007
	End Sub
	Private Sub setPeriod(Index As Int32)
		Select Case Index
			Case 0 ' All   
				FromDate_Txt.Text = CDate("1-Jan-1900").ToString(DateFormat)
				ToDate_Txt.Text = CDate("1-Jan-2200").ToString(DateFormat)
			Case 1 'Last 1 Week
				FromDate_Txt.Text = Today.AddDays(-6).ToString(DateFormat)
				ToDate_Txt.Text = Today.ToString(DateFormat)
			Case 2 'Last 1 Month
				FromDate_Txt.Text = Today.AddDays(1).AddMonths(-1).ToString(DateFormat)
				ToDate_Txt.Text = Today.ToString(DateFormat)
			Case 3 'Last 1 Quater
				Select Case Today.Month
					Case 1, 2, 3
						FromDate_Txt.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(DateFormat)
						ToDate_Txt.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(DateFormat)
					Case 4, 5, 6
						FromDate_Txt.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(DateFormat)
						ToDate_Txt.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(DateFormat)
					Case 7, 8, 9
						FromDate_Txt.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(DateFormat)
						ToDate_Txt.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(DateFormat)
					Case 10, 11, 12
						FromDate_Txt.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(DateFormat)
						ToDate_Txt.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(DateFormat)
				End Select
			Case 4 'Last 1 Year
				FromDate_Txt.Text = Today.AddDays(1).AddYears(-1).ToString(DateFormat)
				ToDate_Txt.Text = Today.ToShortDateString
			Case 5 'Current Financial Year
				If Today.Month <= 3 Then  'Jan|Feb|Mar
					FromDate_Txt.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(DateFormat)
				Else
					FromDate_Txt.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(DateFormat) '31-Mar-2006
				End If
				ToDate_Txt.Text = Today.ToString(DateFormat)
			Case 6 'Between Dates
				FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date) 'Changes by Prashant on 09-01-2008
				ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date) 'Changes by Prashant on 09-01-2008
				FromDate_Txt.Text = CDate(FromDate).ToString(DateFormat)
				ToDate_Txt.Text = CDate(ToDate).ToString(DateFormat)
		End Select
	End Sub
	Private Sub ControlVisibility(SearchIndex As Int32, Optional DateIndex As Int32 = 0)
		cmbDate.Visible = IIf(SearchIndex = 1, True, False)
		lblFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		lblToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		FromDate_Txt.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		ToDate_Txt.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		''calFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0 And DateIndex = 6, True, False)
		''calToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0 And DateIndex = 6, True, False)
		cmbIssueText.Visible = IIf(SearchIndex = 2, True, False)
		cmbReceiptText.Visible = IIf(SearchIndex = 3, True, False)
		'Rajnish 19-08-2008
		cmbWoText.Visible = IIf(SearchIndex = 12, True, False)
		lblNo.Visible = IIf(SearchIndex = 2 And cmbIssueText.SelectedIndex <> 0 Or SearchIndex = 3 And cmbReceiptText.SelectedIndex <> 0 Or SearchIndex = 12 And cmbWoText.SelectedIndex <> 0, True, False)
		txtNo.Visible = IIf(SearchIndex = 2 And cmbIssueText.SelectedIndex <> 0 Or SearchIndex = 3 And cmbReceiptText.SelectedIndex <> 0 Or SearchIndex = 12 And cmbWoText.SelectedIndex <> 0, True, False)
		'txtAmend.Visible = IIf(SearchIndex = 2 And cmbIssueText.SelectedIndex <> 0 Or SearchIndex = 3 And cmbReceiptText.SelectedIndex <> 0, True, False)
		txtName.Visible = IIf(SearchIndex >= 4 And SearchIndex <= 7 Or SearchIndex = 8 Or SearchIndex = 9 Or SearchIndex = 11, True, False)
		cmbToType.Visible = IIf(SearchIndex = 8, True, False)
		cmbStatus.Visible = IIf(SearchIndex = 10, True, False)

		'lblNo.Visible = IIf(txtNo.Visible And SearchIndex = 3 And cmbReceiptText.SelectedIndex <= 0, False, True)
		'txtNo.Visible = IIf((txtNo.Visible And ((cmbReceiptText.Visible And cmbReceiptText.SelectedIndex <= 0) Or (cmbIssueText.Visible And cmbIssueText.SelectedIndex <= 0))), False, True)
		'txtAmend.Visible = IIf((txtAmend.Visible And ((cmbReceiptText.Visible And cmbReceiptText.SelectedIndex <= 0) Or (cmbIssueText.Visible And cmbIssueText.SelectedIndex <= 0))), False, True)

		If SearchIndex = 1 And DateIndex = 6 Then
			FromDate_Txt.Visible = True
			ToDate_Txt.Visible = True
			FromDate_Txt.Enabled = True
			ToDate_Txt.Enabled = True
		ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
			FromDate_Txt.Visible = True
			ToDate_Txt.Visible = True
			FromDate_Txt.Enabled = False
			ToDate_Txt.Enabled = False
		Else
			FromDate_Txt.Visible = False
			ToDate_Txt.Visible = False
		End If
	End Sub
	Private Sub ClearControls()
		txtNo.Text = ""
		txtName.Text = ""
	End Sub
	Private Sub CallFindNowReport(Index As Integer)
		'If txtNo.Text = "" Or IsNumeric(txtNo.Text) = False Then txtNo.Text = "0"
		Tital = GetTitle()
		Dim IssueText As String = ""
		Dim ReceiptText As String = ""
		Dim IssueTypeId As Int16
		Dim StatusId As Int16
		IssueTypeId = Val(cmbToType.SelectedValue)
		StatusId = Val(cmbStatus.SelectedValue)
		IssueText = IIf(cmbIssueText.SelectedIndex <= 0, "", cmbIssueText.SelectedItem.Text)
		ReceiptText = IIf(cmbReceiptText.SelectedIndex <= 0, "", cmbReceiptText.SelectedItem.Text)
		Select Case Index
			Case -1 'all
				objReg = rptIssueReg.GetrptIssueList(, , "1/1/1900", "1/1/2200", , , , , , , , , , , , , , mTransTypeID)
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
			Case 0 'all
				objReg = rptIssueReg.GetrptIssueList(, , "1/1/1900", "1/1/2200", , , , , , , , , , , , , , mTransTypeID)
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
			Case 1  'issue date
				objReg = rptIssueReg.GetrptIssueList(, , FromDate_Txt.Text.ToString, ToDate_Txt.Text.ToString, , , , , , , , , , , , , , mTransTypeID)
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate_Txt.Text.ToString, ToDate_Txt.Text.ToString, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
			Case 2  'issue no
				objReg = rptIssueReg.GetrptIssueList(IssueText, Trim(txtNo.Text), , , , , , , , , , , , , , , , mTransTypeID)
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", IssueText, "", "", Trim(txtNo.Text), "", "", "", "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
			Case 3  'Receipt no
				objReg = rptIssueReg.GetrptIssueList(, , , , , , , , , ReceiptText, Trim(txtNo.Text), , , , , , , mTransTypeID)
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", ReceiptText, "", "", Trim(txtNo.Text), "", "", "", "", "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
			Case 4 'Item name
				objReg = rptIssueReg.GetrptIssueList(, , , , , , , , , , , , , Trim(txtName.Text), , , , mTransTypeID)
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", Trim(txtName.Text), "", "", "", "", Tital, "", "", "", "", "", "", "")
			Case 5  'Store Name
				objReg = rptIssueReg.GetrptIssueList(, , , , Trim(txtName.Text), , , , , , , , , , , , , mTransTypeID)
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", Trim(txtName.Text), "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
			Case 6  'Vendor name
				objReg = rptIssueReg.GetrptIssueList(, , , , , Trim(txtName.Text), , 1, , , , , , , , , , mTransTypeID)
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", Trim(txtName.Text), "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
			Case 7  'Aircraft name
				objReg = rptIssueReg.GetrptIssueList(, , , , , , Trim(txtName.Text), 2, , , , , , , , , , mTransTypeID)
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", Trim(txtName.Text), "", "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
			Case 8  'Totype 1 -vendor 2 aircraft 8 Store
				objReg = rptIssueReg.GetrptIssueList(, , , , , , , IssueTypeId, , , , , , , , , , mTransTypeID) ', IIf(IssueTypeId = 1, 1, 2))
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
			Case 9  'Release note no
				objReg = rptIssueReg.GetrptIssueList(, , , , , , , , , , , Trim(txtNo.Text), , , , , , mTransTypeID)
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", Trim(txtNo.Text), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
			Case 10 'serial no
				objReg = rptIssueReg.GetrptIssueList(, , , , , , , , , , , , Trim(txtNo.Text), , , , , mTransTypeID)
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Tital, "", "", Trim(txtNo.Text), "", "", "", "")
			Case 11  'Status 1-incomplete 2-complete 3 authorize 4 cancel
				objReg = rptIssueReg.GetrptIssueList(, , , , , , , , StatusId, , , , , , , , , mTransTypeID)
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", cmbStatus.SelectedItem.Text, "", "", "", "", "", "", Tital, "", "", "", "", "", "", "")
		End Select
	End Sub
	Private Sub setVariables()
		SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
		DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
		FromDate = IIf(FromDate_Txt.Text.ToString <> "", FromDate_Txt.Text.ToString, "1/1/1900")
		ToDate = IIf(ToDate_Txt.Text.ToString <> "", ToDate_Txt.Text.ToString, "1/1/2200")
		StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
		IssueText = IIf(cmbIssueText.SelectedIndex <= 0, "", cmbIssueText.SelectedValue)
		ReceiptText = IIf(cmbReceiptText.SelectedIndex <= 0, "", cmbReceiptText.SelectedValue)
		'Rajnish 19-08-2008
		WOText = IIf(cmbWoText.SelectedIndex <= 0, "", cmbWoText.SelectedValue)
		IssueTypeId = cmbToType.SelectedValue
		Name = txtName.Text.Trim
		No = txtNo.Text.Trim
		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("SearchIndex") = SearchIndex
		Session("DateIndex") = DateIndex
		Session("StatusId") = StatusId
		Session("IssueText") = IssueText
		Session("ReceiptText") = ReceiptText
		'Rajnish 19-08-2008
		Session("WOText") = WOText
		Session("IssueTypeId") = IssueTypeId
		Session("No") = No
		Session("Name") = Name
	End Sub
	Private Sub SetControl()
		setPeriod(DateIndex)
		CallFindNow(SearchIndex)
		gvIssueList.DataBind()
		cmbSearch.SelectedIndex = SearchIndex
		cmbDate.SelectedIndex = DateIndex
		cmbStatus.SelectedValue = StatusId
		cmbIssueText.SelectedValue = IIf(IssueText = "", "(All)", IssueText)
		cmbReceiptText.SelectedValue = IIf(ReceiptText = "", "(All)", ReceiptText)
		'Rajnish 19-08-2008
		cmbWoText.SelectedValue = IIf(WOText = "", "(All)", WOText)

		cmbToType.SelectedValue = IIf(IssueTypeId = "", 0, IssueTypeId)
		txtName.Text = Name
		txtNo.Text = No
		ControlVisibility(SearchIndex, DateIndex)
		lblResult.Text = "List of Issue as per criteria : " & mIssueList.Count & " Record(s) found."
	End Sub
	Private Sub SetTitle()
		Dim mTransTypeList As TransactionList
		mTransTypeList = TransactionList.GetTransactionList()
		'lblTitle.Text = "List of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
		lblTitle.Text = "List of Issue " '+ mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
		ModuleName = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
		Session("ModuleName") = ModuleName
		totcnt = Session("totcnt") 'Added by shweta on 23-12-11
		lblTitle.Text = " List of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
	End Sub
	Private Sub ControlEnability()
		BtnPrint.Enabled = IIf(gvIssueList.Rows.Count = 0, False, True)
	End Sub
	Private Sub addAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub
	Private Function IsInRole(CheckFor As Rights, Optional Str As String = "") As Boolean
		Dim IsInRoleString As String = ""
		Select Case mTransTypeID
			Case Util.Trans.IssueToWorkOrderAsSpares
				If Str = "Issue To WorkOrder As Spare Req." Then
					IsInRoleString = "IssuetoworkorderasSparerequisition"
				Else
					IsInRoleString = "IssueToWorkOrderAsSpares"
				End If
		End Select
		Select Case CheckFor
			Case Rights.View
				Return User.IsInRole(IsInRoleString + "View")
			Case Rights.[New]
				Return User.IsInRole(IsInRoleString + "New")
			Case Rights.Edit
				Return User.IsInRole(IsInRoleString + "Edit")
			Case Rights.Save
				Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
			Case Rights.Delete
				Return User.IsInRole(IsInRoleString + "Delete")
			Case Rights.Print
				Return User.IsInRole(IsInRoleString + "Print")
		End Select
	End Function

#End Region

#Region " Events "
	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		ClearAll()
		addAttributes()
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not IsPostBack And Session("sender") = "" Then
			If cmbSearch.Enabled = True Then
				setFocus(cmbSearch)
			End If
			Session.Remove("mPendingItemList")
			mTransTypeID = Request.QueryString("TransTypeId")
			Session("mTransTypeId") = mTransTypeID
			Session("MiddleFrame") = "wfnWOReturnIssueList.aspx?TransTypeId=" & mTransTypeID
			DataFieldBind()
			SetControl()
		End If
		MessageBoxResult()
		SetTitle()
		ControlEnability()
	End Sub
	Private Sub GridViewRowCommand(source As Object, e As GridViewCommandEventArgs) Handles gvIssueList.RowCommand
		Select Case e.CommandName
			Case "EditRecord"
				Dim index As Integer = CInt(e.CommandArgument) + gvIssueList.PageSize * gvIssueList.PageIndex
				Dim mId As Guid = mIssueList(index).ID

				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					ClientScript.RegisterStartupScript(type:=[GetType],
													   key:="OpenScript",
													   script:=MessageBox.Show("You are not authorized user"))
					Exit Sub
				End If

				EditRecord(mId)
				Session("IsForWOReturn") = True
				Session("Edit") = True
				mIssueDetail = mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted + " to " + mIssueList(mIssue.ID).Destination
				MarkLog(Util.Action.Edit, ModuleName, mIssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
				Dim str As String
				str = "<script language='javascript'>  openledgersame('wfIssue_Ajax.aspx?BackPage=wfnWOReturnIssueList.aspx'); </script>"
				ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
		End Select
	End Sub
	Private Sub GridViewPageChanged(source As Object, e As GridViewPageEventArgs) Handles gvIssueList.PageIndexChanged
		gvIssueList.PageIndex = e.NewPageIndex
		gvIssueList.DataSource = mIssueList
		Session("mIssueList") = mIssueList
		gvIssueList.DataBind()
	End Sub
	Private Sub cmbSearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSearch.SelectedIndexChanged
		ClearControls()
		cmbDate.SelectedIndex = 0
		cmbIssueText.SelectedIndex = 0
		cmbReceiptText.SelectedIndex = 0
		cmbWoText.SelectedIndex = 0
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		setPeriod(DateIndex)
		If cmbSearch.Enabled = True Then
			setFocus(cmbSearch)
		End If
	End Sub
	Private Sub cmbIssueText_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbIssueText.SelectedIndexChanged
		ClearControls()
		Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		setPeriod(DateIndex)
		If cmbIssueText.Enabled = True Then
			setFocus(cmbIssueText)
		End If
	End Sub
	Private Sub cmbReceiptText_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbReceiptText.SelectedIndexChanged
		ClearControls()
		Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		setPeriod(DateIndex)
		If cmbReceiptText.Enabled = True Then
			setFocus(cmbReceiptText)
		End If
	End Sub
	Private Sub cmbWoText_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbWoText.SelectedIndexChanged
		ClearControls()
		Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		setPeriod(DateIndex)
		If cmbWoText.Enabled = True Then
			setFocus(cmbReceiptText)
		End If
	End Sub
	Private Sub cmbDate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDate.SelectedIndexChanged
		Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		setPeriod(DateIndex)
		If cmbDate.Enabled = True Then
			setFocus(cmbDate)
		End If
	End Sub
	Private Sub btnFindNow_Click(sender As Object, e As ImageClickEventArgs) Handles btnFindNow.Click
		setVariables()
		CallFindNow(SearchIndex)
		gvIssueList.DataBind()
		BtnPrint.Enabled = IIf(mIssueList.Count = 0, False, True)
		lblResult.Text = "List of Issue as per criteria : " & mIssueList.Count & " Record(s) found."
	End Sub
	Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
		RemoveSession()
		Session("MiddleFrame") = ""
		ModuleName = Nothing
		Response.Redirect("Dashboard.aspx")
	End Sub
	'Added By Prashant 18-June-2009
	Private Sub GridViewSorting(source As Object, e As GridViewSortEventArgs) Handles gvIssueList.Sorting
		mIssueList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		gvIssueList.DataSource = mIssueList
		Session("mIssueList") = mIssueList
		gvIssueList.DataBind()
	End Sub
	'------------------------------
#End Region

#Region " Report "
	'Created By :- Jyoti
	'Dated On 11/5/2007

#Region " Report Variable Declaration "
	Dim mCompanyDetail As New CompanyDetail
	Dim objStatus As rptStatus
	Private SearchStr1 As String
	Private SearchStr2 As String
#End Region

#Region " Event "

	Private Function GetTitle() As String           'New Addition
		'By - Jyoti
		'Dated by - 11/5/2007
		Dim mTransTypeList As TransactionList
		mTransTypeList = TransactionList.GetTransactionList()
		Dim mTitle As String = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString + " List Report"

		If mTitle = "" Then
			Return "Goods Outward Note List Report"
		Else
			Return mTitle
		End If
	End Function
	Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
		If Not IsInRole(Rights.Print) Then
			ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
			Exit Sub
		End If
		'For Issue List
		Dim Rpt As New crIssueList
		Dim da As New CSLA.Data.ObjectAdapter
		Dim ds As New dsCommon
		Dim ReportDetails As New rptStatusList
		Dim Title As String = GetTitle()

		If cmbSearch.SelectedIndex = 0 Then
			'All
			SearchStr1 = "The report shows all records till date."
			SearchStr2 = ""
		ElseIf cmbSearch.SelectedIndex = 1 Then
			'Date
			SearchStr1 = "The report shows records filtered by the following criteria"
			If cmbDate.SelectedIndex = 0 Then
				SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text
			ElseIf cmbDate.SelectedIndex = 6 Then
				'SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbSearch.SelectedItem.Text + " " + lblFromDate.Text + " " + FromDate_Txt.Text.ToString + " " + lblToDate.Text + " " + ToDate_Txt.Text.ToString
				SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(FromDate_Txt.Text.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(ToDate_Txt.Text.ToString).FormattedText
			Else
				'SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbSearch.SelectedItem.Text + " " + lblFromDate.Text + " " + FromDate_Txt.Text.ToString + " " + lblToDate.Text + " " + ToDate_Txt.Text.ToString
				SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(FromDate_Txt.Text.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(ToDate_Txt.Text.ToString).FormattedText
			End If
		ElseIf cmbSearch.SelectedIndex = 2 Then
			'Issue No.
			SearchStr1 = "The report shows records filtered by the following criteria"
			SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbIssueText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
		ElseIf cmbSearch.SelectedIndex = 3 Then
			'Receipt No.
			SearchStr1 = "The report shows records filtered by the following criteria"
			SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbReceiptText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
		ElseIf cmbSearch.SelectedIndex = 4 Then
			'Part Number
			SearchStr1 = "The report shows records filtered by the following criteria"
			SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
		ElseIf cmbSearch.SelectedIndex = 5 Then
			'From Store
			SearchStr1 = "The report shows records filtered by the following criteria"
			SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
		ElseIf cmbSearch.SelectedIndex = 6 Then
			'Vendor
			SearchStr1 = "The report shows records filtered by the following criteria"
			SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
		ElseIf cmbSearch.SelectedIndex = 7 Then
			'Aircraft
			SearchStr1 = "The report shows records filtered by the following criteria"
			SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
		ElseIf cmbSearch.SelectedIndex = 8 Then
			'Issue To
			SearchStr1 = "The report shows records filtered by the following criteria"
			SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbToType.SelectedItem.Text
		ElseIf cmbSearch.SelectedIndex = 9 Then
			'Release Note No.
			SearchStr1 = "The report shows records filtered by the following criteria"
			SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtNo.Text
		ElseIf cmbSearch.SelectedIndex = 10 Then
			'Serial No.
			SearchStr1 = "The report shows records filtered by the following criteria"
			SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtNo.Text
		ElseIf cmbSearch.SelectedIndex = 11 Then
			'Status
			SearchStr1 = "The report shows records filtered by the following criteria"
			SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
		End If

		ReportDetails.Add(New rptStatus(, 0, ,
			  gvIssueList.Columns.Item(1).HeaderText, gvIssueList.Columns.Item(2).HeaderText, gvIssueList.Columns.Item(3).HeaderText,
			  gvIssueList.Columns.Item(4).HeaderText, gvIssueList.Columns.Item(5).HeaderText, gvIssueList.Columns.Item(6).HeaderText,
			  gvIssueList.Columns.Item(7).HeaderText))

		Dim TotalCount As Integer
		Dim mCurrentPageindex As Integer = Me.gvIssueList.PageIndex 'Code Added
		TotalCount = Me.gvIssueList.PageCount
		Dim j As Integer
		Dim I As Integer
		Dim str(6) As String

		For j = 0 To TotalCount - 1

			Me.gvIssueList.PageIndex = j
			Me.gvIssueList.DataSource = mIssueList
			Session("mIssueList") = mIssueList
			gvIssueList.DataBind()
			For I = 0 To Me.gvIssueList.PageSize - 1
				If I <= Me.gvIssueList.Rows.Count - 1 Then

					str(0) = ""
					str(1) = ""
					str(2) = ""
					str(3) = ""
					str(4) = ""
					str(5) = ""
					str(6) = ""

					If Me.gvIssueList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.gvIssueList.Rows(I).Cells.Item(1).Text
					If Me.gvIssueList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.gvIssueList.Rows(I).Cells.Item(2).Text
					If Me.gvIssueList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.gvIssueList.Rows(I).Cells.Item(3).Text
					If Me.gvIssueList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.gvIssueList.Rows(I).Cells.Item(4).Text
					If Me.gvIssueList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.gvIssueList.Rows(I).Cells.Item(5).Text
					If Me.gvIssueList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.gvIssueList.Rows(I).Cells.Item(6).Text
					If Me.gvIssueList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.gvIssueList.Rows(I).Cells.Item(7).Text


					ReportDetails.Add(New rptStatus(, 1, , str(0), str(1), str(2), str(3), str(4), str(5), str(6)))
				End If
			Next
		Next

		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
		mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
		mCompanyDetail.WebSite, Title, SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

		If mIssueList.Count = 0 Then
			Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
			msg1.ReplacePage = "wfnWOReturnIssueList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&TransTypeId=" & mTransTypeID
			' msg1.ReplacePage = "wfnWOReturnIssueList.aspx?Backpage="
			msg1.Show()
			Exit Sub
		End If
		Dim mrptImage As rptImage = rptImage.GetImage(ds)

		da.Fill(ds, ReportDetails)
		da.Fill(ds, Report)
		da.Fill(ds, mrptImage)
		Rpt.SetDataSource(ds)
		Session("CrystalReport") = Rpt
		Dim Str1 As String
		Str1 = "<script language=Javascript>openTranDetail();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)

		Me.gvIssueList.PageIndex = mCurrentPageindex
		Me.gvIssueList.DataSource = mIssueList
		Session("mIssueList") = mIssueList
		gvIssueList.DataBind()

	End Sub
#End Region

#End Region

End Class
