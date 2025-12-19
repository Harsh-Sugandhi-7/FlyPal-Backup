Imports System.Text
Imports System.Linq
Public Class wfPartMTBURList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mPartMTBURList As PartMTBURList
    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 100
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCount As Integer = 0
    Public mModelList As ModelList
    Public mATAList As ATAList
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mPartMTBURList = CType(Session("mPartMTBURList"), PartMTBURList)
        mModelList = Session("mModelList")
        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCount = Session("totalCount")
        mATAList = CType(Session("mATAList"), ATAList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPartMTBURList")
        Session.Remove("mModelList")
        Session.Remove("MiddleFrame")
        Session.Remove("mCurrentpage")
        Session.Remove("mpageSize")
        Session.Remove("mpageindex")
        Session.Remove("pagecount")
        Session.Remove("totalCount")
        Session.Remove("mATAList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfPartMTBURList_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mPartMTBURList = PartMTBURList.GetList()
        dgPartList.DataSource = mPartMTBURList
        Session("mPartMTBURList") = mPartMTBURList

        mModelList = ModelList.GetModelList(0)
        Session("mModelList") = mModelList
        cmbModelList.DataSource = mModelList

        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        dgPartList.Columns(4).HeaderText = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS", "BHA MTBUR", AppSettings("ClientCode").ToString + " MTBUR")
        DataBind()
        lblResult.Text = "List of Parts :" & mPartMTBURList.Count & " Record(s) found "

    End Sub
    Private Sub SetControl()
        mModelList = ModelList.GetModelList(0, AddTopItem:="(SELECT)")
        Session("mModelList") = mModelList
        cmbModelList.DataSource = mModelList
        cmbModelList.DataBind()

        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        cmbATAChapter.DataBind()

        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgPartList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = CInt(Session("mpageindex"))
        pagecount = CInt(Session("pagecount"))

        mpageindex = dgPartList.PageIndex
        mCurrentpage = mpageindex + 1

        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        Session("mpageSize") = mpageSize

        FindNow(0)
    End Sub
    Private Sub Save()
        For i As Integer = 0 To dgPartList.Rows.Count - 1
            Dim txtSelfMTBUR, txtWorldMTBUR, txtLastUpdateDate As TextBox
            Dim ChkIsOneTimePurchase As CheckBox 'Added By Vikrant On 21-Nov-2016 For BA21112016
            txtSelfMTBUR = CType(Me.dgPartList.Rows(i).FindControl("txtSelfMTBUR"), TextBox)
            txtWorldMTBUR = CType(Me.dgPartList.Rows(i).FindControl("txtWorldMTBUR"), TextBox)
            txtLastUpdateDate = CType(Me.dgPartList.Rows(i).FindControl("txtLastUpdateDate"), TextBox)
            ChkIsOneTimePurchase = CType(Me.dgPartList.Rows(i).FindControl("ChkIsOneTimePurchase"), CheckBox)

            mPartMTBURList(i).SelfMTBUR = Val(txtSelfMTBUR.Text)
            mPartMTBURList(i).WorldMTBUR = Val(txtWorldMTBUR.Text)
            mPartMTBURList(i).UpdateDate = txtLastUpdateDate.Text


            If mPartMTBURList.Item(i).IsDirty Then
                Try
                    PartMTBURList.UpdateValues(mPartMTBURList(i).PartID, mPartMTBURList(i).SrNo + 1, Val(txtSelfMTBUR.Text), Val(txtWorldMTBUR.Text), txtLastUpdateDate.Text)
                    MarkLog(Util.Action.Save, "UpdateMTBUR", "User Name : " + HttpContext.Current.User.Identity.Name + " Date Time : " + Environment.NewLine + Now.ToString, ErrorType.NoError, mPartMTBURList(i).ID, EventLogID)
                Catch ex As Exception
                    MSGBoxCtrl.show("Alert", "Error In Updating MTBUR values.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End Try
            End If
        Next
        MSGBoxCtrl.show("Success!", "MTBUR Values updates successfully.", "", MsgBoxStyle.OkOnly, "Success")

    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Success" Then
                        For i As Integer = 0 To mPartMTBURList.Count - 1
                            mPartMTBURList.Item(i).IsSelected = False
                        Next
                        Session("mPartMTBURList") = mPartMTBURList
                        dgPartList.Columns(4).HeaderText = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS", "BHA MTBUR", AppSettings("ClientCode").ToString + " MTBUR")
                        dgPartList.DataSource = mPartMTBURList
                        dgPartList.DataBind()

                        upnlgrid.Update()
                    End If
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
    Private Sub FindNow(ByVal Index As Int32)
        mPartMTBURList = PartMTBURList.GetList(txtSearch.Text.Trim, cmbModelList.SelectedValue.ToString, IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize, ATAID:=cmbATAChapter.SelectedValue.ToString)

        totalCount = mPartMTBURList.TotalCount
        pagecount = Math.Ceiling(totalCount / mpageSize)

        Session("totalCount") = totalCount
        Session("pagecount") = pagecount

        Session("mPartMTBURList") = mPartMTBURList
        dgPartList.DataSource = mPartMTBURList
        dgPartList.DataBind()
        UpdateItemGridView()
    End Sub
    Private Sub UpdateItemGridView()
        Dim currentrow As Integer = mpageSize * (mpageindex)
        If totalCount = 0 Then
            lblResult.Text = "List of Part as per criteria : " & totalCount & " Record(s) found."
        Else
            lblResult.Text = "List of Part as per criteria : " & currentrow + 1 & " to " & currentrow + mPartMTBURList.Count & " of " & totalCount & " Record(s) found."
        End If

        SliderExtender1.Minimum = 1
        SliderExtender1.Maximum = pagecount
        Slidercontrol.Text = mCurrentpage
        txtPageDisplay.Text = mCurrentpage
        lblpagecount.Text = pagecount
        If pagecount > 1 Then
            PnlPaging.Visible = True
        Else
            PnlPaging.Visible = False
        End If

        dgPartList.Columns(4).HeaderText = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS", "BHA MTBUR", AppSettings("ClientCode").ToString + " MTBUR")
        dgPartList.DataBind()
        upnlgrid.Update()
    End Sub
    Private Function CustomValidate() As Boolean
        Dim strError As String = String.Empty
        Dim builder = New StringBuilder()
        Dim txtLastUpdateDate As TextBox
        Dim cvUpdateDate As CustomValidator
        Dim chkBox As CheckBox
        Dim upnlUpdateDateValidate As UpdatePanel


        For i As Integer = 0 To dgPartList.Rows.Count - 1
            cvUpdateDate = CType(Me.dgPartList.Rows(i).FindControl("cvUpdateDate"), CustomValidator)
            upnlUpdateDateValidate = CType(Me.dgPartList.Rows(i).FindControl("upnlUpdateDateValidate"), UpdatePanel)
            txtLastUpdateDate = CType(Me.dgPartList.Rows(i).FindControl("txtLastUpdateDate"), TextBox)

            chkBox = CType(dgPartList.Rows.Item(i).Cells(1).FindControl("chkSelect"), CheckBox)
            Dim mID As New Guid(dgPartList.DataKeys(i).Values(0).ToString)
            mPartMTBURList(mID, "").IsSelected = chkBox.Checked

            If chkBox.Checked Then
                If txtLastUpdateDate.Text = "" Then
                    cvUpdateDate.IsValid = False
                    cvUpdateDate.Text = "* Last Update Date Required"
                    strError = "* Last Update Date Required"
                    upnlUpdateDateValidate.Update()
                ElseIf mPartMTBURList(i).UpdateDateFormatted.ToString <> "" Then
                    If CDate(txtLastUpdateDate.Text) < CDate(mPartMTBURList(i).UpdateDateFormatted.ToString) Then
                        cvUpdateDate.IsValid = False
                        cvUpdateDate.Text = "Update Date should be greater than Last Update date"
                        strError = "Update Date should be greater than Last Update date"
                        upnlUpdateDateValidate.Update()
                    End If
                End If
            End If
        Next

        If strError <> "" Then
            Return True
        End If

        Return False
    End Function
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfPartMTBURList_Ajax.aspx?"
            txtSearch.Focus()
            SetControl()
        End If
    End Sub
    Private Sub dgPartList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartList.PageIndexChanging
        dgPartList.PageIndex = e.NewPageIndex
        mCurrentpage = e.NewPageIndex
        Session("mCurrentpage") = mCurrentpage
        FindNow(0)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartList.PageIndex = 0
        mpageindex = 0
        mCurrentpage = mpageindex + 1
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        FindNow(0)
    End Sub
    Private Sub tabUpdateTop_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click, btnUpdateTop.Click
        If IsValid Then
            If CustomValidate() Then
                Exit Sub
            End If
            Dim count As Integer = 0
            count = (From c As PartMTBUR In mPartMTBURList Where c.IsSelected
                   Select c).Count
            If count = 0 Then
                MSGBoxCtrl.show("Alert", "Please Select At least One Record", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            Save()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnGridPaging_Click(sender As Object, e As System.EventArgs) Handles btnGridPaging.Click
        mCurrentpage = CInt(Slidercontrol.Text.Trim)
        mpageindex = mCurrentpage - 1
        dgPartList.PageIndex = mpageindex
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        FindNow(0)
    End Sub
    Private Sub btnExportToExcel_Click(sender As Object, e As System.EventArgs) Handles btnExportToExcel.Click, btnExportToExcelTop.Click
        If mPartMTBURList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter


        Dim dsNew As New dsPartMTBUR
        dsNew.Clear()

        da.Fill(dsNew, "PartMTBUR", mPartMTBURList)

        Dim columnToRemove As String() = {"ID", "PartID", "SrNo", "UpdateDate", "IsValid", "IsDirty", "IsDeleted", "IsNew"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If dsNew.Tables("PartMTBUR").Columns.Contains(columnToRemove(i)) Then
                dsNew.Tables("PartMTBUR").Columns.Remove(columnToRemove(i))
            End If
        Next

        dsNew.Tables("PartMTBUR").Columns("PartName").ColumnName = "Part Name"
        dsNew.Tables("PartMTBUR").Columns("PartDescription").ColumnName = "Part Description"
        dsNew.Tables("PartMTBUR").Columns("SelfMTBUR").ColumnName = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS", "BHA MTBUR", AppSettings("ClientCode").ToString + " MTBUR")
        dsNew.Tables("PartMTBUR").Columns("WorldMTBUR").ColumnName = "World MTBUR"
        dsNew.Tables("PartMTBUR").Columns("UpdateDateFormatted").ColumnName = "Last Update Date"

        dsNew.Tables("PartMTBUR").TableName = "Part MTBUR"

        Session("dsNew") = dsNew
		Session("ExcelFileName") = "Part MTBUR"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
		MarkLog(Util.Action.Print, "PartMTBUR", "Export To excel " + "User Name : " + HttpContext.Current.User.Identity.Name + " Date Time : " + Environment.NewLine + Now.ToString, ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
#End Region







End Class