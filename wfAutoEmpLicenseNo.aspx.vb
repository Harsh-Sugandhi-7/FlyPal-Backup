'************************************
'Created By Utkarsh On 11-Jun-2012 FOR ALL08062012
'Modified by Harsh Sugandhi on 26th May 2025 for FlyPaL-2439.
'************************************


Imports System.Text

Partial Class wfAutoEmpLicenseNo
    Inherits Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(sender As System.Object, e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim prefixText As String = Request.QueryString("q")
        Dim ExludeUseInLogRequried As Boolean = False
        Dim WithoutLicenseNoAlso As Integer = 0
        'Added by Utkarsh on 20-Jan-2014 FOR ALL20012014
        Try

            If Not String.IsNullOrEmpty(Request.QueryString("ExludeUseInLogRequried")) Then
                ExludeUseInLogRequried = CBool(Request.QueryString("ExludeUseInLogRequried"))
            End If
            'End
            If Not String.IsNullOrEmpty(Request.QueryString("WithoutLicenseNoAlso")) Then
                WithoutLicenseNoAlso = CInt(Request.QueryString("WithoutLicenseNoAlso"))
            End If

            Dim sb As New StringBuilder
            Dim i As Integer = 0
			Dim Licenses As LicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList(SearchText:=prefixText,
																								   User:=User.Identity.Name, , ,
																								   ExludeUseInLogRequried:=ExludeUseInLogRequried,
																								   WithoutLicenseNoAlso:=WithoutLicenseNoAlso)
			For i = 0 To Licenses.Count - 1

				sb.Append(Licenses(i).LicenseNoEmpName).Append(Environment.NewLine)

			Next

            Response.Write(sb.ToString)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

End Class
