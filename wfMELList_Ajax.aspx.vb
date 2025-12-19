'********************************************************************************
'Modified By :  Harsh Sugandhi
'Modified Date : 11th June 2024 for FYLPAL-1692 Adding criteria PrimaryModelID 
'********************************************************************************
Imports System.Collections.Generic
Imports System.Linq
Imports System.Xml.Linq

Public Class wfMELList_Ajax
    Inherits Page

#Region " Enumaration "

    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
    End Enum

#End Region

#Region " Variable Declaration "

    Public mMELCategoryList As MELCategoryList
    Dim mSubATAList As SubATAList
    Public mMELList As MELList
    Public mATAList As ATAList
    Public mMEL As MEL
    Dim MELDetail As String
    Dim Code As String = String.Empty
    Dim PartName As String = String.Empty
    Dim ModelName As String = String.Empty
    Dim Reference As String = String.Empty
    Public ModelID As String = "{00000000-0000-0000-0000-000000000000}"
    Public PrimaryModelID As String = "{00000000-0000-0000-0000-000000000000}"
    Dim EventLogID As Guid
    Public mModelList As ModelList

#End Region

#Region " Business Methods "

    Private Sub GetSession()

        mMELList = Session("mMELList")

    End Sub

    Private Sub SetSession()

        Session("mMEL") = mMEL
        Session("mMELList") = mMELList

    End Sub

    Private Sub RemoveSession()

        Session.Remove("mMEL")
        Session.Remove("mMELList")

    End Sub

    Private Sub ClearAll()

        If Session("MiddleFrame") <> "wfMELList_Ajax.aspx" Then
            Session.Remove("mMELList")
        End If

    End Sub

    Private Sub NewRecord()

        mMEL = MEL.NewMEL()
        mMEL.MarkClean()
        Session("mMEL") = mMEL

    End Sub

    Private Sub EditRecord(mId As Guid)

        mMEL = MEL.GetMEL(mId)
        mMEL.MarkClean()
        Session("mMEL") = mMEL

    End Sub

    Private Sub DeleteRecord(mId As Guid)

        MSGBoxCtrl.show(MSGBox.Message_title.Delete,
                        MSGBox.Message_text.Delete,
                        "",
                        MsgBoxStyle.YesNo,
                        "Delete")

        mMEL = MEL.GetMEL(mId)
        Session("mMEL") = mMEL
        GridBind()

    End Sub

    Private Sub SetControl()

        FindNow()
        txtModel.Text = Code
        SetTitle()

    End Sub

    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult
        Dim ErrorsCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then

            Select Case Result1

                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "Delete" Then

                        Try

                            Session("sender") = ""
                            mMEL = CType(Session("mMEL"), MEL)
                            mMEL.Delete()
                            mMEL.Save()
                            DataFieldBind()
                            SetControl()
                            SetGrid()

                        Catch ex As SqlException

                            If ex.Number = 547 Then

                                MELDetail = mMEL.ModelName + "," + " ATA : " + mMEL.ATACode.ToString + "," + " SubATA : " + mMEL.SubATACode.ToString

                                MarkLog(Action.Delete,
                                        "MEL",
                                        "Can't delete : " & MELDetail & " is Currently in use",
                                        ErrorType.HandledError,
                                        mMEL.ID,
                                        EventLogID,
                                        "MEL")

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                                MSGBox.Message_text.ReferenceDelete,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")
                                Exit Sub

                            End If
                            ErrorsCount = ex.Errors.Count

                        Finally

                            If ErrorsCount = 0 Then

                                MELDetail = mMEL.ModelName + "," + " ATA : " + mMEL.ATACode.ToString + "," + " SubATA : " + mMEL.SubATACode.ToString

                                MarkLog(Action.Delete,
                                        "MEL",
                                        MELDetail,
                                        ErrorType.NoError,
                                        mMEL.ID,
                                        EventLogID,
                                        "MEL")

                            End If

                            Session("ForEventLog") = "For Event Log"

                        End Try

                    End If

                Case MsgBoxResult.No

                    If MSGBoxCtrl.Sender = "Delete" Then

                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()
                        SetGrid()

                    End If

                Case MsgBoxResult.Ok

                    DataFieldBind()
                    SetControl()
                    SetGrid()

            End Select

        End If

    End Sub

    Private Sub FindNow(Optional ModelID As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional PrimaryModelID As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional ATAID As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional SubATA As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional ItemSequenceNo As String = "",
                        Optional Description As String = "",
                        Optional MELCategoryID As Integer = -1,
                        Optional RevisionNo As String = "")

        mMELList = Nothing
        dgMELList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mMELList = MELList.GetListOfMELPart(ModelID:=ModelID,
                                            ATAID:=ATAID,
                                            SubATA:=SubATA,
                                            ItemSequenceNo:=ItemSequenceNo,
                                            Description:=Description,
                                            MELCategoryID:=MELCategoryID,
                                            RevisionNo:=RevisionNo,
                                            PrimaryModelID:=PrimaryModelID)
        'Set DataSource of the Grid
        Session("mMELList") = mMELList
        dgMELList.DataSource = mMELList
        dgMELList.DataBind()
        SetTitle() 'For lblResult
        upnlMELList.Update()

    End Sub

    Private Overloads Sub SetFocus(control As WebControl)

        If control.Enabled = False Or control.Visible = False Then Exit Sub
        control.Focus()

    End Sub

    Private Sub ClearControls()

        txtModel.Text = ""

    End Sub

    Private Sub SetVariables()

        Code = txtModel.Text.Trim
        Session("Code") = Code

    End Sub

    Private Sub SetTitle()

        lblResult.Text = "List of MMEL as per criteria : " & mMELList.Count.ToString & " Record(s) found."

    End Sub

    Private Function IsInRole(CheckFor As Rights) As Boolean

        Dim IsInRoleString As String = ""

        'Deciding IsInRole String to check Rights
        IsInRoleString = "MEL"
        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.FindNow
                Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
        End Select

    End Function

    Private Sub SetIDs()

        If hdnModelId.Value <> String.Empty Then

            ModelID = hdnModelId.Value.ToString

        End If

        If hdnModelId.Value = "" Then 'This is for Microsoft\Edge Browser

            mModelList = ModelList.GetModelList(0, "", , , "(All)")

            If txtModel.Text.Trim <> "" Then

                ModelID = mModelList.Item(txtModel.Text.Trim).ID.ToString
                PrimaryModelID = mModelList.Item(txtModel.Text.Trim, "").PrimaryModelID.ToString

            End If

        End If

    End Sub


	Public Sub SetReport(Optional ByVal IsExcel As Boolean = False)
		Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
		Dim obj As MELList
		Dim mCompanyDetail As New CompanyDetail
		Dim da As New CSLA10.Data.ObjectAdapter
		Dim ds As New dsMEL

		myReport = New crMMELlist

		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
									 mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
									 mCompanyDetail.WebSite, "", SearchStr1:=txtModel.Text.Trim, SearchStr2:=txtDescription.Text.Trim,
									 SearchStr3:=cmbATAChapter.SelectedItem.Text,
									 SearchStr4:=cmbSubATAList.SelectedItem.Text, SearchStr5:=cmbMELCategory.SelectedItem.Text,
									 ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"),
									 SearchStr6:=txtItemSequenceNo.Text.Trim, SearchStr7:=txtRevisionNo.Text.Trim,
									 "", "", AppSettings("Logo"))

		obj = mMELList

		If obj.Count <= 0 Then
			MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		Dim mrptImage As rptImage = rptImage.GetImage(ds)
		da.Fill(ds, obj)
		da.Fill(ds, mrptImage)
		da.Fill(ds, Report)
		myReport.SetDataSource(ds)
		Session("CrystalReport") = myReport
		If IsExcel = False Then
			Dim Str As String
			Str = "openTranDetail();"
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
		Else

			Dim columnToRemove As String() = {"ID", "MachineID", "PartID", "PartName", "MELCategoryID", "CurrentMELQty", "Description", "AsOnDate",
											  "ATAChapter", "FlyStatus", "IsHours", "SrNo", "Frequency", "ModelID", "ATACode", "ATANomenclature",
											  "SubATACode", "SubATANomenclature", "ATAID", "SubATAID", "NotApplicableNote", "RevisionDate",
											  "PrimaryModelName", "IsApplicable"}

			For i As Integer = 0 To columnToRemove.Length - 1
				If ds.Tables("MELList").Columns.Contains(columnToRemove(i)) Then
					ds.Tables("MELList").Columns.Remove(columnToRemove(i))
				End If
			Next

			Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "ApprovalNo",
										   "SINote", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11",
										   "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17",
										   "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24",
										   "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31",
										   "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38",
										   "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45",
										   "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50", "SearchStr51", "SearchStr52",
										   "SearchStr53", "SearchStr54", "SearchStr55", "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59",
										   "SearchStr60", "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65", "SearchStr66",
										   "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70", "SearchStr71", "SearchStr72", "SearchStr73",
										   "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80",
										   "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87",
										   "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94",
										   "SearchStr95", "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

			For i As Integer = 0 To columnToRemove2.Length - 1
				If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
					ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
				End If
			Next

			Dim dsNew As New DataSet
			dsNew.Clear()
			dsNew.Merge(table:=ds.Tables(name:="ReportData"))
			dsNew.Merge(table:=ds.Tables(name:="MELList"))

			dsNew.Tables("MELList").Columns("ModelName").ColumnName = "Model"
			dsNew.Tables("MELList").Columns("MELDescription").ColumnName = "Description"
			dsNew.Tables("MELList").Columns("ATACodeSubATACode").ColumnName = "ATA-SubATA"
			dsNew.Tables("MELList").Columns("ItemNo").ColumnName = "Item Sequence No."
			dsNew.Tables("MELList").Columns("PageNo").ColumnName = "Page No."
			dsNew.Tables("MELList").Columns("RevisionNo").ColumnName = "Issue No./ Rev. No."
			dsNew.Tables("MELList").Columns("RevisionDateFormatted").ColumnName = "Revision Date"
			dsNew.Tables("MELList").Columns("MELCategoryName").ColumnName = "Ref. Interval"
			dsNew.Tables("MELList").Columns("MakeMELQty").ColumnName = "Number Installed"
			dsNew.Tables("MELList").Columns("FlyMELQty").ColumnName = "No Req. to Dispatch"
			dsNew.Tables("MELList").Columns("FrequencyInDays").ColumnName = "Freq. In Days"
			dsNew.Tables("MELList").Columns("FrequencyInHours").ColumnName = "Freq. In Hours"
			dsNew.Tables("MELList").Columns("FrequencyInCycles").ColumnName = "Freq. In Cycles"
			dsNew.Tables("MELList").Columns("Applicable").ColumnName = "Applicable"

			dsNew.Tables("MELList").Columns("Model").SetOrdinal(0)
			dsNew.Tables("MELList").Columns("Description").SetOrdinal(1)
			dsNew.Tables("MELList").Columns("ATA-SubATA").SetOrdinal(2)
			dsNew.Tables("MELList").Columns("Item Sequence No.").SetOrdinal(3)
			dsNew.Tables("MELList").Columns("Page No.").SetOrdinal(4)
			dsNew.Tables("MELList").Columns("Issue No./ Rev. No.").SetOrdinal(5)
			dsNew.Tables("MELList").Columns("Revision Date").SetOrdinal(6)
			dsNew.Tables("MELList").Columns("Ref. Interval").SetOrdinal(7)
			dsNew.Tables("MELList").Columns("Number Installed").SetOrdinal(8)
			dsNew.Tables("MELList").Columns("No Req. to Dispatch").SetOrdinal(9)
			dsNew.Tables("MELList").Columns("Freq. In Days").SetOrdinal(10)
			dsNew.Tables("MELList").Columns("Freq. In Hours").SetOrdinal(11)
			dsNew.Tables("MELList").Columns("Freq. In Cycles").SetOrdinal(12)
			dsNew.Tables("MELList").Columns("Applicable").SetOrdinal(13)

			dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "Model"
			dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "Description"
			dsNew.Tables("ReportData").Columns("SearchStr3").ColumnName = "ATA"
			dsNew.Tables("ReportData").Columns("SearchStr4").ColumnName = "Sub ATA"
			dsNew.Tables("ReportData").Columns("SearchStr5").ColumnName = "Rectification Interval"
			dsNew.Tables("ReportData").Columns("SearchStr6").ColumnName = "Item Sequence No."
			dsNew.Tables("ReportData").Columns("SearchStr7").ColumnName = "Issue No./Rev. No."

			dsNew.Tables("ReportData").TableName = "Searching Criteria"
			dsNew.Tables("MELList").TableName = "Master Minimum Equipment List"


			Dim MELListInExcel As New List(Of String)
			MELListInExcel.AddRange(New String() {"Freq. In Hours", "Freq. In Cycles"})
			Session("MELListInExcel") = MELListInExcel

			Session("ExcelFileName") = "Master Minimum Equipment List"
			Session("dsNew") = dsNew

			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
		End If
	End Sub
#End Region

#Region " DataFieldBind "

	Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList("", "ALL")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList

        cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
        mSubATAList = SubATAList.GetSubATAList(Guid.Empty, "", "ALL")
        cmbSubATAList.DataSource = mSubATAList

        mMELCategoryList = MELCategoryList.GetMELCategoryList("ALL")
        cmbMELCategory.DataSource = mMELCategoryList

        DataBind()
    End Sub

    Public Sub GridBind()
        dgMELList.DataSource = mMELList
        dgMELList.DataBind()
        upnlMELList.Update()
    End Sub

    Private Sub SetGrid()
        'Dim P As Integer
        'For j As Integer = 0 To dgMELList.Rows.Count - 1
        '    P = CType(Me.dgMELList.Rows.Item(j).Cells(12).Text, Boolean)
        '    If P = False Then
        '        dgMELList.Rows.Item(j).Cells(11).Enabled = False
        '    End If
        'Next
    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ClearAll()
        GetSession()

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And Session("sender") = "" Then

            If txtModel.Enabled = True Then

                SetFocus(txtModel)

            End If

            Session("MiddleFrame") = "wfMELList_Ajax.aspx"
            DataFieldBind()
            SetControl()

            'Added by Harsh on 15th July 2024 for FLYPAL 1745
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "MEL") Then

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "Mark As Favourite",
                                                    "MarkAsFavourite();",
                                                    True)

            Else

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "Remove From Favourite",
                                                    "RemoveFromFavourite();",
                                                    True)

            End If

        End If

        SetGrid()
        SetTitle()

    End Sub

    Private Sub GridView_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgMELList.RowCommand

        Select Case e.CommandName

            Case "EditRec"

                Dim Index As Integer = CInt(e.CommandArgument) + dgMELList.PageSize * dgMELList.PageIndex
                Dim mID As Guid = mMELList(Index).ID
                EditRecord(mID)

                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "OpenScript",
                                                        MessageBox.Show("You are not authorized user", False),
                                                        True)
                    Exit Sub

                End If
                GridBind()
                SetTitle()
                MELDetail = mMEL.ModelName + "," + " ATA : " + mMEL.ATACode.ToString + "," + " SubATA : " + mMEL.SubATACode.ToString

                MarkLog(Action.Edit,
                        "MEL",
                        MELDetail,
                        ErrorType.NoError,
                        mMEL.ID,
                        EventLogID,
                        "MEL")

                Dim str As String
                str = "openledgersame('wfMELDetail_Ajax.aspx?BackPage=index.aspx');"

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "OpenScript",
                                                    str,
                                                    True)

            Case "DeleteRec"

                Dim Index As Integer = CInt(e.CommandArgument) + dgMELList.PageSize * dgMELList.PageIndex
                Dim mID As Guid = mMELList(Index).ID

                If (Not IsInRole(Rights.Delete)) Then

                    GridBind()
                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "OpenScript",
                                                        MessageBox.Show("You are not authorized user",
                                                                               False),
                                                        True)

                    Exit Sub

                End If

                DeleteRecord(mID)

        End Select

        SetGrid()

    End Sub

    Private Sub Model_TextChanged(sender As Object, e As EventArgs) Handles txtModel.TextChanged

        FindNow_Click(sender, e)

    End Sub

    Private Sub FindNow_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        SetIDs()
        FindNow(ModelID:=ModelID,
                ATAID:=cmbATAChapter.SelectedValue.ToString,
                SubATA:=cmbSubATAList.SelectedValue.ToString,
                ItemSequenceNo:=txtItemSequenceNo.Text.Trim,
                Description:=txtDescription.Text.Trim,
                MELCategoryID:=IIf(cmbMELCategory.SelectedIndex > 0,
                                   cmbMELCategory.SelectedValue,
                                   -1),
                RevisionNo:=txtRevisionNo.Text,
                PrimaryModelID:=PrimaryModelID)
        SetGrid()

    End Sub

    Private Sub Add_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        NewRecord()
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "OpenScript",
                                                MessageBox.Show("You are not authorized user",
                                                                False),
                                                True)

            Exit Sub

        End If
        MarkLog(Action.[New],
                "MEL",
                "",
                ErrorType.NoError,
                mMEL.ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfMELDetail_Ajax.aspx?BackPage=index.aspx');", True)
    End Sub

    Private Sub Close_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub GridView_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgMELList.PageIndexChanging

        dgMELList.PageIndex = e.NewPageIndex
        dgMELList.DataSource = mMELList
        Session("mMELList") = mMELList
        GridBind()
        SetGrid()

    End Sub

    Private Sub GridView_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgMELList.Sorting

        mMELList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMELList") = mMELList
        dgMELList.DataSource = mMELList
        GridBind()
        SetGrid()

    End Sub

    Private Sub ATAChapter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbATAChapter.SelectedIndexChanged
        cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
        mSubATAList = SubATAList.GetSubATAList(New Guid(cmbATAChapter.SelectedValue), "", "ALL")
        cmbSubATAList.DataSource = mSubATAList
        cmbSubATAList.DataBind()
        Session("mSubATAList") = mSubATAList
        upnlSubATA.Update()
        FindNow_Click(sender, e)
    End Sub

    Private Sub SubATAList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSubATAList.SelectedIndexChanged
        FindNow_Click(sender, e)
    End Sub

    Private Sub ItemSequenceNo_TextChanged(sender As Object, e As EventArgs) Handles txtItemSequenceNo.TextChanged
        FindNow_Click(sender, e)
    End Sub

    Private Sub Description_TextChanged(sender As Object, e As EventArgs) Handles txtDescription.TextChanged
        FindNow_Click(sender, e)
    End Sub

    Private Sub MELCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMELCategory.SelectedIndexChanged
        FindNow_Click(sender, e)
    End Sub

    Private Sub RevisionNo_TextChanged(sender As Object, e As EventArgs) Handles txtRevisionNo.TextChanged
        FindNow_Click(sender, e)
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

    'Added by Harsh on 15th July 2024 for FLYPAL 1745
    Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click

        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, "MEL")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click

        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, "MEL")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    'End

#End Region

#Region " Web Services "

    <Services.WebMethod(), Script.Services.ScriptMethod()>
    Public Shared Function GetModelList(prefixText As String,
                                        count As Integer,
                                        contextKey As String) As String()

        Dim list As ModelListAutoComplete
        list = ModelListAutoComplete.GetModelList(prefixText, 1)

        If count = 0 Then

            Return (From c As ModelListAutoComplete.ModelListAutoCompleteInfo In list
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.ToString())).ToArray

        Else

            Return (From c As ModelListAutoComplete.ModelListAutoCompleteInfo In list
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.ID.ToString())).Take(count).ToArray

        End If

    End Function

	Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
		SetReport(IsExcel:=False)
	End Sub

	Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
		SetReport(IsExcel:=True)
	End Sub
#End Region

End Class