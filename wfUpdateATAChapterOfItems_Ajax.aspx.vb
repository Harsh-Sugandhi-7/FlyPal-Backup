'Ajax Conversion By Vikrant On 12-Feb-2014

'Added By Vikrant on 15-Oct-2012 For ALL10102012

Public Class wfUpdateATAChapterOfItems_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItemListForATA As ItemListForATA
    Public mATAList As ATAList
    Dim SearchIndex, Text, ATAID As String
    Dim EventLogID As Guid
    Dim cnt As Integer = 0
    'Added By Vikrant On 08-Aug-2013 For All08082013
    Dim TotalRecords As Integer = 0
    Dim CurrentPageIndex As Integer = 1
    Dim NoOfPages As Integer = 0
    'End
    Public mModelList As ModelList 'Added By Prashantt On 21-AUg-2013 For ALL21082013-1
    Public mCategoryLists As CategoryList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mItemListForATA = Session("mItemListForATA")
        SearchIndex = Session("SearchIndex")
        mATAList = Session("mATAList")
        Text = Session("Text")
        ATAID = Session("ATAID")
        'Added By Vikrant On 08-Aug-2013 For All08082013
        TotalRecords = Session("TotalRecords")
        CurrentPageIndex = Session("CurrentPageIndex")
        NoOfPages = Session("NoOfPages")
        mModelList = Session("mModelList")
        'End
        mCategoryLists = CType(Session("mCategoryLists"), CategoryList)
    End Sub
    Private Sub SetSession()
        Session("mItemListForATA") = mItemListForATA
        Session("mATAList") = mATAList
        Session("mModelList") = mModelList
        Session("mCategoryLists") = mCategoryLists
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemListForATA")
        Session.Remove("SearchIndex")
        Session.Remove("mATAList")
        Session.Remove("Text")
        Session.Remove("ATAID")
        'Added By Vikrant On 08-Aug-2013 For All08082013
        Session.Remove("TotalRecords")
        Session.Remove("CurrentPageIndex")
        Session.Remove("NoOfPages")
        Session.Remove("mModelList")
        'End
        Session.Remove("mCategoryLists")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfUpdateATAChapterOfItems_Ajax.aspx?") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Continue1" Then
                        Try
                            Session("sender") = ""
                            Dim msg1 As New SIMsgBox(Page, "Alert!", "You are going to update ATA Chapter(s) .<BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
                            msg1.ReplacePage = "wfUpdateATAChapterOfItems_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
                            Session("sender") = "Continue2"
                            msg1.Show()
                        Catch ex As SqlException

                        Finally

                        End Try
                    ElseIf CType(Session("sender"), String) = "Continue2" Then
                        Try
                            Session("sender") = ""
                            Response.Redirect("wfUpdateATAChapterOfItems_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException

                        Finally

                        End Try
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
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
#End Region

#Region " DataFieldBind "
    Public Sub DataFieldBind(Optional ByVal PartName As String = "", Optional ByVal CategoryName As String = "")

        'Added By Vikrant On 08-Aug-2013 For All08082013
        mItemListForATA = ItemListForATA.GetItemListForATA(PartName, "", CategoryName, "", Guid.Empty.ToString, "", 0, chkBlankLocation.Checked) 'Get Total Records
        TotalRecords = mItemListForATA.Count
        Session("TotalRecords") = TotalRecords
        mItemListForATA = Nothing
        CurrentPageIndex = 1
        Session("CurrentPageIndex") = CurrentPageIndex
        'End

        mItemListForATA = ItemListForATA.GetItemListForATA(PartName, "", CategoryName, "", Guid.Empty.ToString, "", 1, AppSettings("GridPageSize"), chkBlankLocation.Checked)
        dgItemsList.DataSource = mItemListForATA
        Session("mItemListForATA") = mItemListForATA

        mATAList = ATAList.GetATAList("", "(SELECT)")
        Session("mATAList") = mATAList

        mModelList = ModelList.GetModelList(1, ModelList.IsSelectTagRequired.True) 'Added By Prashantt On 21-AUg-2013 For ALL21082013-1
        Session("mModelList") = mModelList

        DataBind()

        lblResult.Text = "List of Parts as per criteria :" & mItemListForATA.Count & " Record(s) found."
    End Sub
    Public Sub DindControlWithData() 'Added By Prashant 16-Jun-2014 ALL16062014
        mCategoryLists = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryLists
        Session("mCategoryLists") = mCategoryLists
        cmbCategory.DataBind()
    End Sub
    'Added By Vikrant On 08-Aug-2013 For All08082013
    Private Sub ControlVisibilityForPrevNextButtons(ByVal TotRec As Integer)
        NoOfPages = Math.Ceiling(TotRec / AppSettings("GridPageSize"))
        Session("NoOfPages") = NoOfPages
        'btnPrevious.Enabled = IIf(CurrentPageIndex = 1, False, True)
        'btnPreviousBottom.Enabled = IIf(CurrentPageIndex = 1, False, True)
        'btnSavenNext.Enabled = IIf(CurrentPageIndex = NoOfPages, False, True)
        'btnSavenUpdateBottom.Enabled = IIf(CurrentPageIndex = NoOfPages, False, True)
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
    End Sub
    Private Sub GridBind()
        dgItemsList.DataSource = mItemListForATA
        dgItemsList.DataBind()
    End Sub
    Private Function save() As Boolean

        Dim ATAIDs() As String = hdnATAIDValueList.Value.Split(",")
        Dim ATAName() As String = hdnATANameValueList.Value.Split(",")
        Dim ModelIDs() As String = hdnModelIDValueList.Value.Split(",")

        For i As Integer = 0 To dgItemsList.Rows.Count - 1
            Dim OldATA As String = Trim(mItemListForATA.Item(i).ATAChapter)
            Dim OldLocationName As String = Trim(mItemListForATA.Item(i).Location)
            Dim txtValue As TextBox
            txtValue = CType(Me.dgItemsList.Rows(i).FindControl("txtLocation"), TextBox)
            mItemListForATA(i).Location = txtValue.Text
            mItemListForATA.Item(i).ATAID = New Guid(ATAIDs(i))
            mItemListForATA.Item(i).ModelID = New Guid(ModelIDs(i))  'Added By Prashantt On 21-AUg-2013 For ALL21082013-1
            If mItemListForATA.Item(i).IsDirty Then
                Try
                    Dim mNewApplicability As String = IIf(mItemListForATA.Item(i).ItemApplicable.Contains(mModelList(New Guid(ModelIDs(i))).ModelName) Or (New Guid(ModelIDs(i)).Equals(Guid.Empty)), mItemListForATA.Item(i).ItemApplicable, mItemListForATA.Item(i).ItemApplicable + "," + mModelList(New Guid(ModelIDs(i))).ModelName)
                    Dim mDetailOld As String = "Old ATA,Location & Applicability : " + OldATA + "; " + OldLocationName + "; " + mItemListForATA.Item(i).ItemApplicable
                    ItemListForATA.UpdateATAChapter(mItemListForATA(i).ItemID, New Guid(ATAIDs(i)), txtValue.Text, New Guid(ModelIDs(i)))
                    Dim mDetailNew As String = "New ATA,Location & Applicability : " + IIf(New Guid(ATAIDs(i)).Equals(Guid.Empty), "", ATAName(i)) + "; " + txtValue.Text.Trim + "; " + mNewApplicability
                    MarkLog(Util.Action.Save, "ChangePartATA", "Item Name : " + mItemListForATA(i).ItemName + Environment.NewLine + mDetailOld + Environment.NewLine + mDetailNew, Util.ErrorType.NoError, mItemListForATA(i).ItemID, EventLogID)
                Catch ex As Exception
                    Return False
                    MSGBoxCtrl.Show("Alert", "Error In Updating ATA Chapter/Location/Applicability.", "", MsgBoxStyle.OkOnly, "")
                End Try
            End If
        Next
        GridBind()
        Return True
    End Function
    'End
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfUpdateATAChapterOfItems_Ajax.aspx?"
            DindControlWithData()  'Added By Prashant 16-Jun-2014 ALL16062014
            DataFieldBind()
            ControlVisibilityForPrevNextButtons(TotalRecords) 'Added By Vikrant On 08-Aug-2013 For All08082013
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Added By Vikrant On 08-Aug-2013 For All08082013
    Protected Sub btnSavenNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSavenNext.Click, btnSavenUpdateBottom.Click
        If save() Then
            CurrentPageIndex += 1
            Session("CurrentPageIndex") = CurrentPageIndex
            mItemListForATA = ItemListForATA.GetItemListForATA(txtPartName.Text.Trim, "", IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, ""), "", Guid.Empty.ToString, "", CurrentPageIndex, AppSettings("GridPageSize"), chkBlankLocation.Checked)
            dgItemsList.DataSource = mItemListForATA
            Session("mItemListForATA") = mItemListForATA
            dgItemsList.DataBind()
            btnSavenNext.Enabled = mItemListForATA.Count > 0
            btnSavenUpdateBottom.Enabled = mItemListForATA.Count > 0
            If (btnSavenNext.Enabled = False Or btnSavenUpdateBottom.Enabled = False) Then
                btnPrevious.Enabled = True
                btnPreviousBottom.Enabled = True
            End If
            upnlActionBtnTop.Update()
            upnlActionBtnBottom.Update()
            'ControlVisibilityForPrevNextButtons(TotalRecords)
            upnlgrid.Update()
            lblResult.Text = "List of Parts as per criteria :" & mItemListForATA.Count & " Record(s) found."
            upnlResult.Update()
        End If
    End Sub
    Protected Sub btnPrevious_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrevious.Click, btnPreviousBottom.Click
        If save() Then
            CurrentPageIndex -= 1
            Session("CurrentPageIndex") = CurrentPageIndex
            mItemListForATA = ItemListForATA.GetItemListForATA(txtPartName.Text.Trim, "", IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, ""), "", Guid.Empty.ToString, "", CurrentPageIndex, AppSettings("GridPageSize"), chkBlankLocation.Checked)
            dgItemsList.DataSource = mItemListForATA
            Session("mItemListForATA") = mItemListForATA
            dgItemsList.DataBind()
            upnlgrid.Update()
            btnPrevious.Enabled = mItemListForATA.Count > 0
            btnPreviousBottom.Enabled = mItemListForATA.Count > 0
            If (btnPrevious.Enabled = False Or btnPreviousBottom.Enabled = False) Then
                btnSavenNext.Enabled = True
                btnSavenUpdateBottom.Enabled = True
            End If
            upnlActionBtnTop.Update()
            upnlActionBtnBottom.Update()
            'ControlVisibilityForPrevNextButtons(TotalRecords)
            lblResult.Text = "List of Parts as per criteria :" & mItemListForATA.Count & " Record(s) found."
            upnlResult.Update()
        End If
    End Sub
    'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFindNow.Click
        DataFieldBind(txtPartName.Text.Trim, IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, ""))
        btnPrevious.Enabled = True
        btnPreviousBottom.Enabled = True
        btnSavenNext.Enabled = True
        btnSavenUpdateBottom.Enabled = True
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
        upnlgrid.Update()
        lblResult.Text = "List of Parts as per criteria :" & mItemListForATA.Count & " Record(s) found."
        upnlResult.Update()
    End Sub
#End Region
End Class