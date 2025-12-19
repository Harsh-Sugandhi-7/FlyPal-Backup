Public Class wfCustomerContractSelection_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration"
    Public mOrder As Order
    Public mnWO As nWO
    Dim PartNo As String
    Dim mFileAttach As FileAttach
    Dim mItemId As Guid = Guid.Empty
    Dim mCustomerContractList As CustomerContractList
    Dim mMSPAssemblySelectionOpenFrom As String = ""
    Dim mProject As Project
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mOrder = Session("mOrder")
        mnWO = Session("mnWO")
        mCustomerContractList = Session("mCustomerContractList")
        mProject = Session("mProject")
    End Sub
    Private Sub RemoveSession()
        'Session.Remove("mItemId")
        'Session.Remove("mCustomerContractList")
        'Session.Remove("PartNo")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Confirmation" Then
                        Try
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Confirmation" Then
                        Session.Remove("mItemId1")
                    End If
            End Select
        End If
    End Sub
    '--------
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCustomerContractList = CustomerContractList.GetCustomerContractList()
        Session("mCustomerContractList") = mCustomerContractList
        dgCustomerContractList.DataSource = mCustomerContractList
        dgCustomerContractList.DataBind()
    End Sub
    Private Sub ControlVisibility()
    End Sub
    'End
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        mMSPAssemblySelectionOpenFrom = Request.QueryString("Type")
        If Not IsPostBack Then
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
    End Sub
    Private Sub dgCustomerContractList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCustomerContractList.RowCommand
        Select Case e.CommandName
            Case "Select"
                Dim index As Integer = CInt(e.CommandArgument) + dgCustomerContractList.PageIndex * dgCustomerContractList.PageSize
                If mMSPAssemblySelectionOpenFrom = "FromProejct" Then
                    mProject.CustomerContractID = mCustomerContractList(index).ID
                    mProject.CustomerContractNo = mCustomerContractList(index).ContractNumber
                    Session("mProject") = mProject
                Else
                    mnWO.CustomerContractID = mCustomerContractList(index).ID
                    mnWO.CustomerContractNo = mCustomerContractList(index).ContractNumber
                    Session("mnWO") = mnWO
                End If

                RemoveSession()
                Dim mopenas As String = Request.QueryString("Type")
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Case "ViewRec"
                Dim mID As Guid
                mID = New Guid(e.CommandArgument.ToString)
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                mFileAttach = FileAttach.GetAttachment(mID)
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgCustomerContractList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCustomerContractList.PageIndexChanging
        dgCustomerContractList.PageIndex = e.NewPageIndex
        dgCustomerContractList.DataSource = mCustomerContractList
        Session("mCustomerContractList") = mCustomerContractList
        dgCustomerContractList.DataBind()
        ControlVisibility()
        upnlMSPAssembly.Update()
    End Sub
    Private Sub dgCustomerContractList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCustomerContractList.Sorting
        mCustomerContractList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCustomerContractList") = mCustomerContractList
        dgCustomerContractList.DataSource = mCustomerContractList
        dgCustomerContractList.DataBind()
        ControlVisibility()
        upnlMSPAssembly.Update()
    End Sub
    Protected Sub OnDataBound(sender As Object, e As EventArgs)
        If dgCustomerContractList.Rows.Count = 0 Then Exit Sub
        Dim j As Integer = dgCustomerContractList.Rows.Count - 1
        For i As Integer = dgCustomerContractList.Rows.Count - 1 To 1 Step -1
            Dim row As GridViewRow = dgCustomerContractList.Rows(i)
            Dim previousRow As GridViewRow = dgCustomerContractList.Rows(i - 1)

            'If row.Cells(8).Text = previousRow.Cells(8).Text Then
            '    If previousRow.Cells(0).RowSpan = 0 Then
            '        If row.Cells(0).RowSpan = 0 Then
            '            previousRow.Cells(0).RowSpan += 2
            '            previousRow.Cells(1).RowSpan += 2
            '            previousRow.Cells(2).RowSpan += 2
            '            previousRow.Cells(3).RowSpan += 2
            '            previousRow.Cells(4).RowSpan += 2
            '            'If i = j Then 'i.e Last row bottom border
            '            '    'Do nothing 
            '            'Else
            '            '    dgCustomerContractList.Rows(i).Attributes("style") = "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-color:rgb(128,0,64);"
            '            '    previousRow.Cells(0).Attributes("style") = "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-bottom-color:rgb(128,0,64); border-bottom-width: 3px;"
            '            'End If
            '            'previousRow.Cells(0).BackColor = Color.FromArgb(211, 211, 211)
            '            'previousRow.Cells(1).BackColor = Color.FromArgb(211, 211, 211)
            '            'previousRow.Cells(2).BackColor = Color.FromArgb(211, 211, 211)
            '            'previousRow.Cells(3).BackColor = Color.FromArgb(211, 211, 211)
            '        Else
            '            previousRow.Cells(0).RowSpan = row.Cells(0).RowSpan + 1
            '            previousRow.Cells(1).RowSpan = row.Cells(1).RowSpan + 1
            '            previousRow.Cells(2).RowSpan = row.Cells(2).RowSpan + 1
            '            previousRow.Cells(3).RowSpan = row.Cells(3).RowSpan + 1
            '            previousRow.Cells(4).RowSpan = row.Cells(4).RowSpan + 1
            '            'previousRow.Cells(0).BackColor = Color.FromArgb(211, 211, 211)
            '            'previousRow.Cells(1).BackColor = Color.FromArgb(211, 211, 211)
            '            'previousRow.Cells(2).BackColor = Color.FromArgb(211, 211, 211)
            '            'previousRow.Cells(3).BackColor = Color.FromArgb(211, 211, 211)
            '        End If
            '        row.Cells(0).Visible = False
            '        row.Cells(1).Visible = False
            '        row.Cells(2).Visible = False
            '        row.Cells(3).Visible = False
            '        row.Cells(4).Visible = False
            '    End If
            'End If
        Next
    End Sub
#End Region

End Class