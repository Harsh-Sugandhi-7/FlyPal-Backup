Imports System.Collections.Generic

Public Class wfComponentReservationStockList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mComponentReservationStockList As ComponentReservationStockList
    Public mComponentReservation As ComponentReservation
    Dim mIndex2 As Int32
    Dim mFileAttach As FileAttach
    Public mUserHasNoStoreRights As UserHasNoStoreRights
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mComponentReservationStockList = CType(Session("mComponentReservationStockList"), ComponentReservationStockList)
        mComponentReservation = Session("mComponentReservation")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Expired" Then
                        Try
                            Session("Sender") = ""
                            mIndex2 = Session("Index2")

                            SetObject(mIndex2)

                            Session.Remove("mComponentReservationStockList")
                            Session("Edit") = False
                            Session.Remove("Index2")
                            Session.Remove("ItemName")
                            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Expired" Then
                        Session("sender") = ""
                        Response.Redirect("wfComponentReservationStockList_Ajax.aspx?ChildPage=" & Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
                    End If
            End Select
        End If
    End Sub
    Private Sub SetObject(ByVal Index As Int32)

        With mComponentReservation
            .ReceiptItemID = mComponentReservationStockList(Index).ReceiptItemID
            .ReserveForDate = CDate(txtReservationDate.Text)
            .PartNo = mComponentReservationStockList(Index).ItemName
            .Description = mComponentReservationStockList(Index).ItemDesc
            .SerialNo = mComponentReservationStockList(Index).SerialNo
            .ReceiptNo = mComponentReservationStockList(Index).ReceiptText + "-" + mComponentReservationStockList(Index).ReceiptNo.ToString
            .ReceiptDate = mComponentReservationStockList(Index).ReceiptDateFormatted
        End With

        Session("mComponentReservation") = mComponentReservation
        Response.Redirect("wfComponentReservation_Ajax.aspx?BackPage=Index.aspx")

    End Sub
    Private Sub ControlVisibility()

    End Sub
    Private Sub ReceiptItemAttachment(Optional ByVal ReceiptItemID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal Visibility As Integer = 0)
        mFileAttach = FileAttach.GetAttachment(New Guid(ReceiptItemID))
        If (mFileAttach.Size > 0) Then
            Dim No As New Random
            Dim StrName As String = "abc" & No.Next.ToString
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
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            End If
        End If
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataGridBind()
        mComponentReservationStockList = ComponentReservationStockList.GetComponentReservationStockList(Guid.Empty, txtSearch.Text.Trim, , , , , _
                                                                                                           IssueDate:=txtReservationDate.Text.Trim, _
                                                                                                           SerialNo:=txtSerialNo.Text.Trim)
        Session("mComponentReservationStockList") = mComponentReservationStockList
        dgComponentReservationStockList.DataSource = mComponentReservationStockList
        lblResult1.Text = "Component Stock Status List : " & mComponentReservationStockList.Count & " Record(s) found."
        DataBind()
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            txtReservationDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            If txtSearch.Enabled = True Then
                setFocus(txtSearch)
            End If
            If Session("ItemNameComponentReservationList") <> "" Then
                txtSearch.Text = Session("ItemNameComponentReservationList")
                Session.Remove("ItemNameComponentReservationList")
            End If
            If Session("ReallocateComponentReservation") = "Reallocate" Then
                'Session.Remove("ReallocateComponentReservation")
                txtSearch.Enabled = False
                txtSerialNo.Enabled = False
                btnFindNow.Enabled = False
                txtReservationDate.Enabled = False
                txtReservationDate.Text = mComponentReservation.ReserveForDateFormatted
            End If
            DataGridBind()
            lblResult1.Text = "Component Stock Status List : " & mComponentReservationStockList.Count & " Record(s) found."
        End If
        ControlVisibility()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click, txtReservationDate.TextChanged
        DataGridBind()
        upnlComponentReservationStockList.Update()
    End Sub
    Private Sub dgComponentReservationStockList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgComponentReservationStockList.RowCommand
        DueAtMessage.Visible = False
        Select Case e.CommandName
            Case "SelectRecord"
                Dim Index2 As Int32 = CInt(e.CommandArgument) + dgComponentReservationStockList.PageIndex * dgComponentReservationStockList.PageSize
                mUserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(User.Identity.Name, mComponentReservationStockList(Index2).StoreID.ToString) 'Added By Prashant 31-Oct-2018 ALL30102018
                If mUserHasNoStoreRights.Count > 0 Then
                    MSGBoxCtrl.show("Alert!", "Sorry you do not have rights to select this store. Please contact with admin.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                If Session("ReallocateComponentReservation") = "Reallocate" Then
                    mComponentReservation.ReallocateComponentReservation(OldReceiptItemID:=mComponentReservation.ID, NewReceiptItemID:=mComponentReservationStockList(Index2).ReceiptItemID)
                    Session.Remove("ReallocateComponentReservation")
                    If Request.QueryString("BackPage") = "Index.aspx" Then
                        Response.Redirect("Index.aspx")
                    End If
                Else
                    SetObject(Index2)
                End If
            Case "ViewRec"
                Dim Index2 As Int32 = CInt(e.CommandArgument) + dgComponentReservationStockList.PageIndex * dgComponentReservationStockList.PageSize
                ReceiptItemAttachment(ReceiptItemID:=mComponentReservationStockList(Index2).ReceiptItemID.ToString)
        End Select
    End Sub
    Private Sub dgComponentReservationStockList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgComponentReservationStockList.PageIndexChanging
        DueAtMessage.Visible = False
        dgComponentReservationStockList.PageIndex = e.NewPageIndex
        dgComponentReservationStockList.DataSource = mComponentReservationStockList
        dgComponentReservationStockList.DataBind()
        Session("mComponentReservationStockList") = mComponentReservationStockList
    End Sub
    Private Sub dgComponentReservationStockList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgComponentReservationStockList.Sorting
        DueAtMessage.Visible = False
        mComponentReservationStockList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mComponentReservationStockList") = mComponentReservationStockList
        dgComponentReservationStockList.DataSource = mComponentReservationStockList
        dgComponentReservationStockList.DataBind()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session.Remove("mComponentReservationStockList")
        Session.Remove("ReallocateComponentReservation")
        Session.Remove("Index2")
        Session.Remove("ItemName")
        Session("Edit") = False

        If Request.QueryString("BackPage") = "Index.aspx" Then
            Response.Redirect("Index.aspx")
        Else
            Session("Edit") = False
            Response.Redirect(Request.QueryString("BackPage"))
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

#End Region

End Class