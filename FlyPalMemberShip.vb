
'To restrict concurrent user login (same user cannot login from multiple PCs)


REM Created By Kalpesh
REM Date: 02-11-17

<Serializable()> _
Public Class FlyPalMemberShip
    Inherits BusinessBase

#Region "Variable Decleration"
    Private mUserName As String = ""
    Private mUserKey As String = ""
#End Region

#Region "Business Properties and Methods"

    Public Property UserName() As String
        Get
            Return mUserName
        End Get
        Set(ByVal Value As String)
            If mUserName <> Value Then
                mUserName = Trim(Value)

                MarkDirty()
            End If
        End Set
    End Property

    Public Property UserKey() As String
        Get
            Return mUserKey
        End Get
        Set(ByVal Value As String)
            If mUserKey <> Value Then
                mUserKey = Trim(Value)

                MarkDirty()
            End If
        End Set
    End Property
    Public Overrides ReadOnly Property IsValid() As Boolean
        Get
            Return MyBase.IsValid
        End Get
    End Property
    Public Overrides ReadOnly Property IsDirty() As Boolean
        Get
            Return MyBase.IsDirty
        End Get
    End Property
#End Region

#Region " Shared Methods "

    Public Shared Function CreateUser(ByVal UserName As String, ByVal UserKey As String) As FlyPalMemberShip
        Return CType(CSLA.DataPortal.Create(New Criteria(UserName, UserKey)), FlyPalMemberShip)
    End Function

    Public Shared Function GetUser(ByVal UserName As String) As FlyPalMemberShip

        'Return CType(CSLA.DataPortal.Fetch(New Criteria(UserName)), FlyPalMemberShip)

        Return Fetch(New Criteria(UserName))


    End Function

#End Region

#Region " Constructor "
    Public Sub New()
        'prevent direct instantnation
    End Sub
#End Region

#Region " Criteria "
    'Criteria for identifying existing object
    <Serializable()> _
    Private Class Criteria

        Public UserName As String
        Public UserKey As String

        Public Sub New(ByVal UserName As String, ByVal UserKey As String)
            Me.UserName = UserName
            Me.UserKey = UserKey
        End Sub

        Public Sub New(ByVal UserName As String)
            Me.UserName = UserName
        End Sub

    End Class
#End Region


#Region " Data Access"

    Protected Overrides Sub DataPortal_Create(ByVal Criteria As Object)

        Dim Crit As Criteria = CType(Criteria, Criteria)

        mUserName = Crit.UserName
        mUserKey = Crit.UserKey

        Me.MarkDirty()

    End Sub

    Public Shared Function Fetch(ByVal Criteria As Object) As FlyPalMemberShip

        Dim crit As Criteria = CType(Criteria, Criteria)

        Dim mMemberUser As FlyPalMemberShip = New FlyPalMemberShip

        Dim cn As New SqlConnection(AppSettings("DB:Flypal"))
        Dim cm As New SqlCommand

        cn.Open()
        Try
            With cm
                .Connection = cn

                .CommandType = CommandType.StoredProcedure
                .CommandText = "fetchFlyPalhMemberShipUser"
                .Parameters.AddWithValue("@UserName", crit.UserName)

                Dim dr As New SafeDataReader(.ExecuteReader)
                Try
                    If dr.Read() Then

                        With dr
                            mMemberUser.UserName = .GetString(0)
                            mMemberUser.UserKey = .GetString(1)
                        End With

                        mMemberUser.MarkOld()

                    End If
                Finally
                    dr.Close()
                End Try
            End With

        Catch ex As Exception
            Throw ex.GetBaseException
        Finally
            cn.Close()
        End Try


        Return mMemberUser

    End Function

    Protected Overrides Sub Dataportal_Fetch(ByVal Criteria As Object)

        Dim crit As Criteria = CType(Criteria, Criteria)

        Dim cn As New SqlConnection(DB("Flypal"))
        Dim cm As New SqlCommand

        cn.Open()
        Try
            With cm
                .Connection = cn

                .CommandType = CommandType.StoredProcedure
                .CommandText = "fetchFlyPalhMemberShipUser"
                .Parameters.AddWithValue("@UserName", crit.UserName)

                Dim dr As New SafeDataReader(.ExecuteReader)
                Try
                    If dr.Read() Then

                        With dr
                            mUserName = .GetString(0)
                            mUserKey = .GetString(1)
                        End With

                    End If
                Finally
                    dr.Close()
                End Try
            End With
            MarkOld()
        Catch ex As Exception
            Throw ex.GetBaseException
        Finally
            cn.Close()
        End Try
    End Sub

    Protected Overrides Sub DataPortal_Update()

        Dim Cn As New SqlConnection(DB("Flypal"))

        Dim Cm As New SqlCommand
        Dim tr As SqlTransaction
        Try
            Cn.Open()
            tr = Cn.BeginTransaction(IsolationLevel.Serializable)
            With Cm
                .Connection = Cn
                .Transaction = tr
                .CommandType = CommandType.StoredProcedure
                If Me.IsDeleted Then

                    If Not Me.IsNew Then
                        .CommandText = "deleteFlyPalhMemberShipUser"
                        .Parameters.AddWithValue("@UserName", mUserName)
                        .ExecuteNonQuery()
                    End If

                    MarkNew()
                Else
                    If Me.IsNew = True Then
                        .CommandText = "addFlyPalhMemberShipUser"
                    Else
                        .CommandText = "updateFlyPalhMemberShipUser"
                    End If

                    .Parameters.AddWithValue("@UserName", mUserName)
                    .Parameters.AddWithValue("@UserKey", mUserKey)

                    .ExecuteNonQuery()
                    MarkOld()

                End If
                tr.Commit()
            End With
        Catch ex As Exception
            tr.Rollback()
            Throw ex.GetBaseException
        Finally
            Cn.Close()
        End Try
    End Sub

    Protected Overrides Sub DataPortal_Delete(ByVal Criteria As Object)
        Dim Crit As Criteria = CType(Criteria, Criteria)
        Dim Cn As New SqlConnection(DB("Flypal"))
        Dim Cm As New SqlCommand
        Dim tr As SqlTransaction
        Try
            Cn.Open()
            tr = Cn.BeginTransaction(IsolationLevel.Serializable)
            With Cm
                .Connection = Cn
                .Transaction = tr
                .CommandType = CommandType.StoredProcedure

                .CommandText = "deleteFlyPalMemberShip"
                .Parameters.AddWithValue("@UserName", Crit.UserName)
                .ExecuteNonQuery()
            End With
            tr.Commit()
        Catch ex As Exception
            tr.Rollback()
            Throw ex.GetBaseException
        Finally
            Cn.Close()
        End Try
    End Sub

#End Region

End Class

