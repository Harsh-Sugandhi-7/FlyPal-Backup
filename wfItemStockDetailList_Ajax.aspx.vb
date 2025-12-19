Imports System.Linq
Public Class wfItemStockDetailList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mItemStockDetailList As ItemStockDetailList
    Dim mFileAttach As FileAttach 'Ajay 27-Des-2022
    Dim mFileAttachments As FileAttachments 'Ajay 27-Des-2022
    Dim ItemName As String
    Dim ItemDescription As String
    Dim StockQty As String
    Dim SerialNo As String
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            ItemName = Session("ItemName")
            ItemDescription = Session("ItemDescription")
            StockQty = Session("StockQty")
            SerialNo = Session("SerialNo")
            lblPartNoInfo.Text = ItemName
            lblDescriptionInfo.Text = ItemDescription
            lblStockQty.Text = StockQty

            mItemStockDetailList = ItemStockDetailList.GetItemStockDetailList(ItemName, "", "", False, Guid.Empty, Guid.Empty, False, , 0, False, "01-01-3050", True, "", Guid.Empty.ToString, "Landing Value")
            dgItemStockDetailList.DataSource = mItemStockDetailList
            dgItemStockDetailList.DataBind()

            Dim TotalBalQty = From c In mItemStockDetailList
                                    Group c By PartName = c.PartName Into Group
                                   Select New With {Key .TotalBalAmount = Group.Sum(Function(x) x.BalAmountForGrid)}

            Dim BalQty
            For Each BalQty In TotalBalQty
                lblTotalBalAmt.Text = BalQty.TotalBalAmount.ToString()
            Next

            'Dim BlQty=TotalBalQty.
            'lblTotalBalAmt.Text = 


            '(New System.Linq.SystemCore_EnumerableDebugView(Of <anonymous type>)(TotalBalQty)).Items(0)
            lblItemStockDetailList.Text = "List Of " + mItemStockDetailList.Count.ToString + " Record(s)"
            'If mItemStockDetailList.Count > 0 Then
            '    lblTotalBaAmount.Text = dgItemStockDetailList.Items(dgItemStockDetailList.Items.Count - 1).Cells(13).Text
            'End If
        End If
    End Sub
    'Ajay 27-Des-2022
    Private Sub dgItemStockDetailList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgItemStockDetailList.RowCommand
        Select Case e.CommandName
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument)
                Dim mID As Guid
                mID = New Guid(dgItemStockDetailList.DataKeys(CInt(e.CommandArgument)).Values("ReciptItemID").ToString)
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString

                mFileAttachments = FileAttachments.GetChildFileAttachments(mID)
                Dim AttachmentCount As Integer = mFileAttachments.Count
                If AttachmentCount > 1 Then

                    Session("mFileAttachments") = mFileAttachments
                    Session("TransactionNameMarkLog") = "Receipt Cum Invoice Item"
                    Session("TransactionName") = "Receipt Cum Invoice No.and Date"
                    Session("TransactionDetails") = dgItemStockDetailList.DataKeys(CInt(e.CommandArgument)).Values("ReceiptNumber").ToString + " & " + dgItemStockDetailList.DataKeys(CInt(e.CommandArgument)).Values("ReceiptDate").ToString
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAttachWindow", "OpenAttachWindow();", True)

                Else
                    '------
                    mFileAttach = FileAttach.GetAttachmentChild(mID)
                    If mFileAttach.Size > 0 Then
                        Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
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
                            Dim Str1 As String
                            Str1 = "openFile();"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", Str1, True)
                        End If

                    Else
                        MSGBoxCtrl.show("Attachment!", "No Attach File Present", "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                End If
        End Select
    End Sub
    '------------------------------
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("ItemName")
        Session.Remove("ItemDescription")
        Session.Remove("StockQty")
        Session.Remove("SerialNo")
        Response.Redirect("DashboardForInventory.aspx")
    End Sub
#End Region

End Class