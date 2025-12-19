Imports System.Collections.Generic
Public Class wfConsumablesAndExpendablePendingList_Ajax
    Inherits System.Web.UI.Page

#Region "Variables and Declarations"
    Dim mRequisitionItemsNew As RequisitionItemsNew
    Public mConsumableAndExpendable As ConsumableAndExpendable
    Private checkedIds As New List(Of String)()
#End Region

#Region " Business Method "
    Private Sub GetSession()
        mRequisitionItemsNew = Session("mRequisitionItemsNew")
        mConsumableAndExpendable = Session("mConsumableAndExpendable")
    End Sub
    Private Sub DataFieldBind()
        mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForConsumablesAndExpendables(mConsumableAndExpendable.ReqID, "", mConsumableAndExpendable.TransDateFormatted.ToString)
        If Not mRequisitionItemsNew Is Nothing Then
            For Each Child As RequisitionItemNew In mRequisitionItemsNew
                Child.IsSelect = mConsumableAndExpendable.ConsumableAndExpendableItems.Contains(Child.ID)
                If mConsumableAndExpendable.ConsumableAndExpendableItems.Contains(Child.ID) Then
                    checkedIds.Add(Child.ID.ToString)
                End If
            Next
        End If
        dgPartList.DataSource = mRequisitionItemsNew
        Session("mRequisitionItemsNew") = mRequisitionItemsNew
        dgPartList.DataBind()
        lblResult.Text = "List of Items : " & mRequisitionItemsNew.Count & " Record(s) found."
    End Sub
    Private Sub SetObject()
        Dim checkString = Request.Form("chkSelect")
        ' Set Selectedvalue  
        If Not checkString Is Nothing Then
            Dim values = checkString.Split(","c)
            For Each value As String In values
                mRequisitionItemsNew(New Guid(value)).IsSelect = True
            Next

            For i As Integer = 0 To mRequisitionItemsNew.Count - 1
                If mRequisitionItemsNew(i).IsSelect = True And Array.IndexOf(values, mRequisitionItemsNew(i).ID.ToString) = -1 Then
                    mRequisitionItemsNew(i).IsSelect = False
                End If
            Next
        End If
        For i As Integer = 0 To mRequisitionItemsNew.Count - 1
            If mRequisitionItemsNew(i).IsSelect = False Then
                If mConsumableAndExpendable.ConsumableAndExpendableItems.Contains(mRequisitionItemsNew(i).ID) Then
                    mConsumableAndExpendable.ConsumableAndExpendableItems.Remove(mRequisitionItemsNew(i).ID, "")
                End If
            End If
        Next
        Session("mConsumableAndExpendable") = mConsumableAndExpendable
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("AddCEParts") = "False"
        Session.Remove("mRequisitionItemsNew")
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub dgPartList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartList.PageIndexChanging
        dgPartList.PageIndex = e.NewPageIndex
        dgPartList.DataSource = mRequisitionItemsNew
        Session("mRequisitionItemsNew") = mRequisitionItemsNew
        dgPartList.DataBind()
    End Sub
    Private Sub dgPartList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartList.Sorting
        mRequisitionItemsNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRequisitionItemsNew") = mRequisitionItemsNew
        dgPartList.DataSource = mRequisitionItemsNew
        dgPartList.DataBind()
    End Sub
    Private Sub btnOk_Click(sender As Object, e As System.EventArgs) Handles btnOk.Click, btnOkTop.Click
        SetObject()
        Dim checkString = Request.Form("chkSelect")
        If checkString Is Nothing Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select at least one item", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            Session("AddCEParts") = "True"
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "Checked Selection"
    Public Function NumeroChequeInclus(ByVal numero As String) As String
        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
    End Function
#End Region

End Class