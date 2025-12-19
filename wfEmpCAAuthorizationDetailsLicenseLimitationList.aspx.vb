'Added by    Saylee
'Created By : 24-Aug-2023

Imports System.Collections.Generic
Imports System.Text

Public Class wfEmpCAAuthorizationDetailsLicenseLimitationList
    Inherits System.Web.UI.Page


#Region " Variable Declaration "
    Public mEmpCAAuthorizationDetailsLicenseLimitation As EmpCAAuthorizationDetailsLicenseLimitation
    Public mEmpCAAuthorizationDetail As EmpCAAuthorizationDetail
    Public mEmpCAAuthorizationDetailsLicenseLimitationList As EmpCAAuthorizationDetailsLicenseLimitationList
    Public mEmpCAAuthorization As EmpCAAuthorization
    Dim mCALimitationList As CALimitationList
    Dim EventLogID As Guid
    Private checkedIds As New List(Of String)()
#End Region

#Region " Helper Methods"
    Private Sub GetSession()
        mCALimitationList = CType(Session("mCALimitationList"), CALimitationList)
        mEmpCAAuthorizationDetailsLicenseLimitationList = Session("mEmpCAAuthorizationDetailsLicenseLimitationList")
        mEmpCAAuthorizationDetail = Session("mEmpCAAuthorizationDetail")
        mEmpCAAuthorization = Session("mEmpCAAuthorization")
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCALimitationList = CALimitationList.GetCALimitation()
        dgLimitationList.DataSource = mCALimitationList
        Session("mCALimitationList") = mCALimitationList

        mEmpCAAuthorizationDetailsLicenseLimitationList = EmpCAAuthorizationDetailsLicenseLimitationList.GetLicenseLimitationList(mEmpCAAuthorizationDetail.ID)
        Session("mEmpCAAuthorizationDetailsLicenseLimitationList") = mEmpCAAuthorizationDetailsLicenseLimitationList

        If Not mCALimitationList Is Nothing Then
            For Each Child As CALimitationList.CALimitationInfo In mCALimitationList
                If mEmpCAAuthorizationDetailsLicenseLimitationList.Contains(Child.ID) Then
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

                For i As Integer = 0 To checkedIds.Count - 1
                    If Not mEmpCAAuthorizationDetailsLicenseLimitationList.Contains(CType(checkedIds(i), Integer)) Then

                        mEmpCAAuthorizationDetailsLicenseLimitation = EmpCAAuthorizationDetailsLicenseLimitation.NewEmpCAAuthorizationDetailsLicenseLimitation(mEmpCAAuthorizationDetail.ID)
                        mEmpCAAuthorizationDetailsLicenseLimitation.EmpCAAuthorizationDetailsID = mEmpCAAuthorizationDetail.ID
                        mEmpCAAuthorizationDetailsLicenseLimitation.CALimitationsID = Val(checkedIds(i))
                        Session("mEmpCAAuthorizationDetailsLicenseLimitation") = mEmpCAAuthorizationDetailsLicenseLimitation

                        If mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsNew Then mEmpCAAuthorization.Save()
                        Session("mEmpCAAuthorization") = mEmpCAAuthorization

                        mEmpCAAuthorizationDetailsLicenseLimitation.Save()

                        'Else

                        '    If chkLimitationList.Items(i).Selected = False Then
                        '        mEmpCAAuthorizationDetailsLicenseLimitation = EmpCAAuthorizationDetailsLicenseLimitation.GetEmpCAAuthorizationDetailsLicenseLimitation(mEmpCAAuthorizationDetail.ID, Val(checkedIds(i)))
                        '        EmpCAAuthorizationDetailsLicenseLimitation.DeleteAuthorizationDetailsLicenseLimitation(mEmpCAAuthorizationDetailsLicenseLimitation.ID) 'New Guid(chkSkillList.Items(i).Value))
                        '    End If
                    End If
                Next

                For i As Integer = 0 To mEmpCAAuthorizationDetailsLicenseLimitationList.Count - 1
                    If Not checkedIds.Contains(mEmpCAAuthorizationDetailsLicenseLimitationList(i).CALimitationsID.ToString) Then
                        mEmpCAAuthorizationDetailsLicenseLimitation = EmpCAAuthorizationDetailsLicenseLimitation.GetEmpCAAuthorizationDetailsLicenseLimitation(mEmpCAAuthorizationDetail.ID, mEmpCAAuthorizationDetailsLicenseLimitationList(i).CALimitationsID)
                        EmpCAAuthorizationDetailsLicenseLimitation.DeleteAuthorizationDetailsLicenseLimitation(mEmpCAAuthorizationDetailsLicenseLimitation.ID) 'New Guid(chkSkillList.Items(i).Value))

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
        Session("mEmpCAAuthorizationDetailsLicenseLimitation") = mEmpCAAuthorizationDetailsLicenseLimitation
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