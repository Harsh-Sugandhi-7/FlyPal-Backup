
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

Imports Microsoft.VisualBasic
Imports CSLA
Imports CSLA.Data
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports CSLA.Security

<WebService(Namespace:="https://localhost/FlyPalAjax/GDService")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class GDService
    Inherits System.Web.Services.WebService


#Region " Security "
    Public Class CSLACredentials
        Inherits SoapHeader

        Public UserName As String
        Public Password As String
    End Class

    Public Credentials As CSLACredentials = New CSLACredentials

    'Public Sub Login()

    '    If Len(Credentials.UserName) = 0 Then
    '        Throw New System.Security.SecurityException("Valid credentials not provided.")
    '    End If

    '    With Credentials
    '        BusinessPrincipal.login(.UserName, .Password)
    '    End With

    '    Dim principal As System.Security.Principal.IPrincipal = Threading.Thread.CurrentPrincipal

    '    If Not principal.Identity.IsAuthenticated Then
    '        'The user is not valid, raise an exception.
    '        ''Throw New Exception("Invalid user or password.")
    '    End If

    'End Sub
#End Region


    Private Function Connect(ByVal strQuery As String, ByVal DB As String) As String
        Dim cn As New SqlConnection(ConfigurationManager.AppSettings(DB))
        Dim cm As New SqlCommand
        Dim tr As SqlTransaction = Nothing

        Dim sXML As String = ""

        cn.Open()
        Try
            With cm
                .Connection = cn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "Tool_RunRequestQuery"
                .CommandTimeout = 1000
                .Parameters.AddWithValue("@str", strQuery)

                Dim dr As SqlDataReader
                dr = cm.ExecuteReader

                sXML = ReaderToXML(dr, "Data")
            End With
        Catch ex As Exception
            Return ex.Message
        Finally
            cn.Close()
        End Try

        Return sXML
    End Function


    Public Function ReaderToXML(ByVal objReader As SqlDataReader, Optional ByVal ParentNodeName As String = "") As String
        'You Can use OLEDBDataReader as well
        'If ParentNodeName is not blank, it will be used as
        'Start End node of the XML

        Dim sXML As String = ""
        Dim intColumnCount As Integer
        Dim intCtr As Integer
        Dim sValue As String

        ParentNodeName = Trim(ParentNodeName)
        Try
            intColumnCount = objReader.FieldCount
            If ParentNodeName <> "" Then sXML += "<" & ParentNodeName & ">"

            Do While objReader.Read


                sXML = sXML + "<Record>"

                'Loop through each row
                For intCtr = 0 To intColumnCount - 1
                    'Get the Value of each column
                    'Does not include nodes for null/blank values

                    If Not objReader.IsDBNull(intCtr) Then
                        sValue = objReader.Item(intCtr).ToString
                        If Trim(sValue) <> "" Then
                            sXML += "<" & objReader.GetName(intCtr) & ">" & XMLizeString(sValue) & "</" & objReader.GetName(intCtr) & ">"
                        End If
                    End If
                Next

                sXML = sXML + "</Record>"

            Loop
            If ParentNodeName <> "" Then sXML += "</" & ParentNodeName & ">"


        Catch Ex As Exception
            sXML = ""

        End Try
        Return sXML
    End Function

    Private Function XMLizeString(ByVal sInput As String) As String
        Dim s As String = ""

        'SHORTENED VERSION TO REDUCE EXECUTION TIME
        'Return " <![CDATA[" & sInput & "]]>"
        'THIS WILL INCREASE THE SIZE OF YOUR XML String
        If Not (IsAlphaNumeric(sInput)) Then
            Return " <![CDATA[" & sInput & "]]>"
        Else
            Return sInput
        End If
    End Function

    Private Function IsAlphaNumeric(ByVal TestString As String) As Boolean

        Dim sTemp As String
        Dim iLen As Integer
        Dim iCtr As Integer
        Dim sChar As String

        'returns true if all characters in a string are alphabetical
        '   or numeric
        'returns false otherwise or for empty string

        sTemp = TestString
        iLen = Len(sTemp)
        If iLen > 0 Then
            For iCtr = 1 To iLen
                sChar = Mid(sTemp, iCtr, 1)
                If Not sChar Like "[0-9A-Za-z.:, ]" Then _
                     Exit Function
            Next

            IsAlphaNumeric = True
        End If
    End Function

#Region "Individual"
    <WebMethod(Description:="Get Data"), SoapHeader("Credentials")> _
    Public Function GetData(ByVal strQuery As String) As String
        Try
            Return Connect(strQuery, "DB:FlyPal")
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
#End Region

End Class



