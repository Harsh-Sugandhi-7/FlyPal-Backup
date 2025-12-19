
Imports System.Linq
Imports System.Collections.Generic
Imports System.Text

Public Class wfUpdateDueLimtForFAS_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mDueLimits As DueLimits
    Dim mPerDayLimits As PerDayLimits
#End Region

#Region " Helper Methods "
    Private Sub addAttributes()
    End Sub
    Private Sub SetGridObject()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.gdvDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            mDueLimits.Item(i).DueLimitsForFAS = Trim(txtLimit.Text)
        Next i
        Session("mDueLimits") = mDueLimits
    End Sub
    Private Sub GetSession()
        mDueLimits = CType(Session("mDueLimits"), DueLimits)
        mPerDayLimits = CType(Session("mPerDayLimits"), PerDayLimits)
    End Sub
    Private Sub SetSession()
        Session("mDueLimits") = mDueLimits
        Session("mPerDayLimits") = mPerDayLimits
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfUpdateDueLimtForFAS_Ajax.aspx?" Then
            Session.Remove("mDueLimits")
            Session.Remove("mPerDayLimits")
        End If
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
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
    Public Sub TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtValue As TextBox
        For i As Integer = 0 To gdvDuePeriodLimits.Rows.Count - 1
            Try
                txtValue = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
                txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")
            Catch ex As Exception
            End Try
        Next
    End Sub
#End Region

#Region " Data Binding "
    Public Sub DataFieldBind()
        mDueLimits = DueLimits.GetDueLimits(Guid.Empty, True)
        gdvDuePeriodLimits.DataSource = mDueLimits
        Session("mDueLimits") = mDueLimits
        upnlDueLimits.Update()
        DataBind()
    End Sub
#End Region

#Region "Eventes"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            Session("MiddleFrame") = "wfUpdateDueLimtForFAS_Ajax.aspx?"
            DataFieldBind()
        End If
        TextChanged(sender, e)
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
        If IsValid Then
            'Try
            '    MSGBoxCtrl.show("Update Alert", "This will update Min/Max/One time purchase of " + mOptimizationOfInventoryList.Count.ToString + " Part(s). Do you want to continue? ", "", MsgBoxStyle.YesNo, "Save")
            '    Exit Sub
            'Catch ex As Exception
            'Finally

            'End Try
            'Save()
            SetGridObject()
            mDueLimits.Save()
            MSGBoxCtrl.show("Updated Successfully", "Updated Successfully", "", MsgBoxStyle.OkOnly, "")
        Else
            'upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mDueLimits = Nothing
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class