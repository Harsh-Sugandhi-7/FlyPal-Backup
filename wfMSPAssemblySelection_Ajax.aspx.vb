Public Class wfMSPAssemblySelection_Ajax
    Inherits Page

#Region " Variable Declaration"

    Public mOrder As Order
    Public mnWO As nWO
    Public mLineMaintenanceOrder As LineMaintenanceOrder
    Dim PartNo As String
    Dim mFileAttach As FileAttach
    Dim mItemId As Guid = Guid.Empty
    Dim mMSPAssemblyListForSelection As MSPAssemblyListForSelection
    Dim mMSPAssemblySelectionOpenFrom As String = ""

#End Region

#Region " Business Methods "

    Private Sub GetSession()

        mOrder = Session("mOrder")
        mnWO = Session("mnWO")
        mLineMaintenanceOrder = Session("mLineMaintenanceOrder")
        mMSPAssemblyListForSelection = Session("mMSPAssemblyListForSelection")

    End Sub
    Private Sub RemoveSession()

    End Sub

    Private Overloads Sub SetFocus(Control As WebControl)
        If Control.Enabled = False Or Control.Visible = False Then Exit Sub
        Control.Focus()
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
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

                            MSGBoxCtrl.show(MSGBox.Message_title.Alert,
                                            MSGBox.Message_text.Alert,
                                            ex.Message,
                                            MsgBoxStyle.OkOnly,
                                            "")
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

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        If mMSPAssemblySelectionOpenFrom = "FromPurchaseOrder" Then
            mMSPAssemblyListForSelection = MSPAssemblyListForSelection.GetMSPAssemblyListForSelection(AsOnDate:=mOrder.OrderDate.ToString)
        ElseIf mMSPAssemblySelectionOpenFrom = "FromWO" Then
            mMSPAssemblyListForSelection = MSPAssemblyListForSelection.GetMSPAssemblyListForSelection(AsOnDate:=mnWO.WODate.ToString)
        ElseIf mMSPAssemblySelectionOpenFrom = "FromLineMaintenanceOrder" Then
            mMSPAssemblyListForSelection = MSPAssemblyListForSelection.GetMSPAssemblyListForSelection(AsOnDate:=mLineMaintenanceOrder.OrderDate.ToString)
        End If

        Session("mMSPAssemblyListForSelection") = mMSPAssemblyListForSelection
        dgMSPAssembly.DataSource = mMSPAssemblyListForSelection
        dgMSPAssembly.DataBind()

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        getSession()
        mMSPAssemblySelectionOpenFrom = Request.QueryString("Type")

        If Not IsPostBack Then
            DataFieldBind()
        End If

    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click

        Dim openAs As String = Request.QueryString("Type")

        If openAs IsNot Nothing AndAlso (openAs = "FromPurchaseOrder" Or openAs = "FromWO" Or openAs = "FromLineMaintenanceOrder") Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "onclose",
                                                "CallParentCallback();",
                                                True)
            Exit Sub

        End If

    End Sub

    Private Sub dgMSPAssembly_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgMSPAssembly.RowCommand

        Select Case e.CommandName
            Case "Select"

                Dim index As Integer = CInt(e.CommandArgument) + dgMSPAssembly.PageIndex * dgMSPAssembly.PageSize

                If mMSPAssemblySelectionOpenFrom = "FromPurchaseOrder" Then

                    mOrder.MSPID = mMSPAssemblyListForSelection(index).MSPID
                    mOrder.MSPAssemblyID = mMSPAssemblyListForSelection(index).ID
                    mOrder.AssemblyName = mMSPAssemblyListForSelection(index).AssemblyName
                    mOrder.PlanName = mMSPAssemblyListForSelection(index).PlanName
                    mOrder.ContractNo = mMSPAssemblyListForSelection(index).ContractNo
                    mOrder.MSPPORemark = ""
                    Session("mOrder") = mOrder

                ElseIf mMSPAssemblySelectionOpenFrom = "FromWO" Then

                    mnWO.MSPID = mMSPAssemblyListForSelection(index).MSPID
                    mnWO.MSPAssemblyID = mMSPAssemblyListForSelection(index).ID
                    mnWO.AssemblyName = mMSPAssemblyListForSelection(index).AssemblyName
                    mnWO.PlanName = mMSPAssemblyListForSelection(index).PlanName
                    mnWO.ContractNo = mMSPAssemblyListForSelection(index).ContractNo
                    mnWO.MSPWORemark = ""
                    Session("mnWO") = mnWO

                ElseIf mMSPAssemblySelectionOpenFrom = "FromLineMaintenanceOrder" Then

                    mLineMaintenanceOrder.MSPID = mMSPAssemblyListForSelection(index).MSPID
                    mLineMaintenanceOrder.ContractNO = mMSPAssemblyListForSelection(index).ContractNo
                    Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

                End If
                RemoveSession()
                Dim openAs As String = Request.QueryString("Type")

                If openAs IsNot Nothing AndAlso (openAs = "FromPurchaseOrder" Or openAs = "FromWO" Or openAs = "FromLineMaintenanceOrder") Then

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "onclose",
                                                        "CallParentCallback();",
                                                        True)
                    Exit Sub

                End If

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
                        File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path

                        ScriptManager.RegisterStartupScript(Me,
                                                            [GetType],
                                                            "openFilel",
                                                            "openFilel();",
                                                            True)
                    End If

                End If

        End Select

    End Sub

    Private Sub dgMSPAssembly_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgMSPAssembly.PageIndexChanging

        dgMSPAssembly.PageIndex = e.NewPageIndex
        dgMSPAssembly.DataSource = mMSPAssemblyListForSelection
        Session("mMSPAssemblyListForSelection") = mMSPAssemblyListForSelection
        dgMSPAssembly.DataBind()
        upnlMSPAssembly.Update()

    End Sub

    Private Sub dgMSPAssembly_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgMSPAssembly.Sorting

        mMSPAssemblyListForSelection.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMSPAssemblyListForSelection") = mMSPAssemblyListForSelection
        dgMSPAssembly.DataSource = mMSPAssemblyListForSelection
        dgMSPAssembly.DataBind()
        upnlMSPAssembly.Update()

    End Sub

    Protected Sub OnDataBound(sender As Object, e As EventArgs)

        If dgMSPAssembly.Rows.Count = 0 Then Exit Sub

        Dim j As Integer = dgMSPAssembly.Rows.Count - 1

        For i As Integer = dgMSPAssembly.Rows.Count - 1 To 1 Step -1

            Dim row As GridViewRow = dgMSPAssembly.Rows(i)
            Dim previousRow As GridViewRow = dgMSPAssembly.Rows(i - 1)

            If row.Cells(8).Text = previousRow.Cells(8).Text Then

                If previousRow.Cells(0).RowSpan = 0 Then

                    If row.Cells(0).RowSpan = 0 Then

                        previousRow.Cells(0).RowSpan += 2
                        previousRow.Cells(1).RowSpan += 2
                        previousRow.Cells(2).RowSpan += 2
                        previousRow.Cells(3).RowSpan += 2
                        previousRow.Cells(4).RowSpan += 2

                    Else

                        previousRow.Cells(0).RowSpan = row.Cells(0).RowSpan + 1
                        previousRow.Cells(1).RowSpan = row.Cells(1).RowSpan + 1
                        previousRow.Cells(2).RowSpan = row.Cells(2).RowSpan + 1
                        previousRow.Cells(3).RowSpan = row.Cells(3).RowSpan + 1
                        previousRow.Cells(4).RowSpan = row.Cells(4).RowSpan + 1

                    End If

                    row.Cells(0).Visible = False
                    row.Cells(1).Visible = False
                    row.Cells(2).Visible = False
                    row.Cells(3).Visible = False
                    row.Cells(4).Visible = False

                End If

            End If

        Next

    End Sub

#End Region

End Class