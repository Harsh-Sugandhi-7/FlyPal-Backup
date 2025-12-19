'Created By Utkarsh ON 22-Aug-2013 FOR ALL20082013-1

Public Class wfUserMachineList
    Inherits Page

#Region " Variable Declaration "

    Public mUserMachinelist As UserMachineList
    Dim mMachineName As String
    Dim mMachineID As Guid

#End Region


#Region " Business Methods "

    Private Sub GetSession()

        mMachineID = CType(Session("MachineID"), Guid)
        mMachineName = Session("MachineName")
        mUserMachinelist = Session("mUserMachinelist")

    End Sub

#End Region

#Region "DataBinding"

    Private Sub DataFieldBind()

        mUserMachinelist = UserMachineList.GetUserMachineList(mMachineID)
        GVMachine.DataSource = mUserMachinelist
        GVMachine.DataBind()
        Session("mUserMachinelist") = mUserMachinelist

    End Sub

#End Region

#Region "Business Method"

    Private Sub SetGrid()

        For i As Integer = 0 To GVMachine.Rows.Count - 1

            Dim chkselect As CheckBox
            chkselect = CType(GVMachine.Rows(i).FindControl("chkSelect"), CheckBox)
            mUserMachinelist.Item(i).IsSelected = chkselect.Checked

        Next

        Session("mUserMachinelist") = mUserMachinelist

    End Sub

    Private Sub SetPage()

        lbltitle.Text = "Current Aircraft rights [ " & mMachineName & " ]"
        lblAircraftList.Text = "List of User as per criteria : " & mUserMachinelist.Count & " Record(s) found."

    End Sub

#End Region

#Region "Events"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
        End If
        SetPage()

    End Sub

    Protected Sub SaveRecord(sender As Object, e As EventArgs) Handles btnSave.Click

        SetGrid()
        Try

            mUserMachinelist.Save()
            Session.Remove("MachineID")
            Session.Remove("MachineName")
            Session.Remove("mUserMachinelist")
            Dim url As String = String.Empty
            url = Session("MachineURL").ToString
            Session.Remove("MachineURL")
            Response.Redirect(url)

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

#End Region

End Class