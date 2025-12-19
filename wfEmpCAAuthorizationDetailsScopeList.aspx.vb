
'Added by    Saylee
'Created By : 24-Aug-2023

Imports System.Collections.Generic
Imports System.Text

Public Class wfEmpCAAuthorizationDetailsScopeList
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mEmpCAAuthorizationDetailsScope As EmpCAAuthorizationDetailsScope
    Public mEmpCAAuthorizationDetail As EmpCAAuthorizationDetail
    Public mEmpCAAuthorizationDetailsScopeList As EmpCAAuthorizationDetailsScopeList
    Public mEmpCAAuthorization As EmpCAAuthorization
    Dim mCAAuthorizationScopeList As CAAuthorizationScopeList
    Dim EventLogID As Guid
    Private checkedIds As New List(Of String)()
#End Region

#Region " Helper Methods"
    Private Sub GetSession()
        mCAAuthorizationScopeList = CType(Session("mCAAuthorizationScopeList"), CAAuthorizationScopeList)
        mEmpCAAuthorizationDetailsScopeList = Session("mEmpCAAuthorizationDetailsScopeList")
        mEmpCAAuthorizationDetail = Session("mEmpCAAuthorizationDetail")
        mEmpCAAuthorization = Session("mEmpCAAuthorization")
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCAAuthorizationScopeList = CAAuthorizationScopeList.GetCAAuthorizationScope()

        dgScopeList.DataSource = mCAAuthorizationScopeList
        Session("mCAAuthorizationScopeList") = mCAAuthorizationScopeList

        mEmpCAAuthorizationDetailsScopeList = EmpCAAuthorizationDetailsScopeList.GetEmpCAAuthorizationDetailsScopeList(mEmpCAAuthorizationDetail.ID)
        Session("mEmpCAAuthorizationDetailsScopeList") = mEmpCAAuthorizationDetailsScopeList

        If Not mCAAuthorizationScopeList Is Nothing Then
            For Each Child As CAAuthorizationScopeList.CAAuthorizationScopeInfo In mCAAuthorizationScopeList
                If mEmpCAAuthorizationDetailsScopeList.Contains(Child.ID) Then
                    checkedIds.Add(Child.ID.ToString)
                End If
            Next
        End If

        DataBind()
    End Sub

#End Region

#Region " Page Load "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not Page.IsPostBack Then
            DataFieldBind()

        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If IsValid Then
            Try

                Dim builder = New StringBuilder()
                builder.Append("You have selected the following checks :<br/>")

                Dim checkString = Request.Form("chkSelect")
                If checkString Is Nothing Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim values As String() = checkString.Split(","c)
                For Each value As String In values
                    builder.Append("<br/>")
                    builder.Append(value)
                    checkedIds.Add(value)
                Next
                Session("checkedIds") = checkedIds



                For i As Integer = 0 To checkedIds.Count - 1
                    If Not mEmpCAAuthorizationDetailsScopeList.Contains(CType(checkedIds(i), Integer)) Then
                        'If chkScopeList.Items(i).Selected Then
                        mEmpCAAuthorizationDetailsScope = EmpCAAuthorizationDetailsScope.NewEmpCAAuthorizationDetailsScope(mEmpCAAuthorizationDetail.ID)
                        mEmpCAAuthorizationDetailsScope.EmpCAAuthorizationDetailsID = mEmpCAAuthorizationDetail.ID
                        mEmpCAAuthorizationDetailsScope.CAAuthorizationScopeID = Val(checkedIds(i))
                        Session("mEmpCAAuthorizationDetailsScope") = mEmpCAAuthorizationDetailsScope

                        If mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsNew Then mEmpCAAuthorization.Save()
                        Session("mEmpCAAuthorization") = mEmpCAAuthorization

                        mEmpCAAuthorizationDetailsScope.Save()
                        'Else
                        '    mEmpCAAuthorizationDetailsScope = EmpCAAuthorizationDetailsScope.GetEmpCAAuthorizationDetailsScope(mEmpCAAuthorizationDetail.ID, Val(checkedIds(i)))
                        '    If chkScopeList.Items(i).Selected = False Then
                        '        EmpCAAuthorizationDetailsScope.DeleteAuthorizationDetailsScope(mEmpCAAuthorizationDetailsScope.ID) 'New Guid(chkSkillList.Items(i).Value))
                        '    End If
                        'End If
                        ' Else

                        ' mEmpCAAuthorizationDetailsScope = EmpCAAuthorizationDetailsScope.GetEmpCAAuthorizationDetailsScope(mEmpCAAuthorizationDetail.ID, Val(checkedIds(i)))
                        'EmpCAAuthorizationDetailsScope.DeleteAuthorizationDetailsScope(mEmpCAAuthorizationDetailsScope.ID) 'New Guid(chkSkillList.Items(i).Value))

                    End If
                Next

                For i As Integer = 0 To mEmpCAAuthorizationDetailsScopeList.Count - 1
                    If Not checkedIds.Contains(mEmpCAAuthorizationDetailsScopeList(i).CAAuthorizationScopeID.ToString) Then
                        mEmpCAAuthorizationDetailsScope = EmpCAAuthorizationDetailsScope.GetEmpCAAuthorizationDetailsScope(mEmpCAAuthorizationDetail.ID, mEmpCAAuthorizationDetailsScopeList(i).CAAuthorizationScopeID)
                        EmpCAAuthorizationDetailsScope.DeleteAuthorizationDetailsScope(mEmpCAAuthorizationDetailsScope.ID)

                    End If
                Next

                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
                '' MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            Catch ex As Exception

            End Try
        End If
        Session("mEmpCAAuthorizationDetailsScope") = mEmpCAAuthorizationDetailsScope
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
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