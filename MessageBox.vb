Imports System.Text

Public Class MessageBox
    Enum MessageBoxButton
        OK
        YesNo
    End Enum
    Public Shared Function Show(ByVal str As String, Optional ByVal IsTagRequired As Boolean = True) As String
        Dim strScript As StringBuilder = New StringBuilder
        If IsTagRequired Then
            strScript.Append(" <script language=javascript > ")
            strScript.Append(" alert('" & str & "');")
            strScript.Append(" </script>")
        Else
            strScript.Append(" alert('" & str & "');")
        End If

        Return strScript.ToString
    End Function
    Public Shared Function Show(ByVal str As String, ByVal button As MessageBoxButton) As String
        Dim strScript As StringBuilder = New StringBuilder
        If button = MessageBoxButton.OK Then
            Return MessageBox.Show(str)
        Else
            strScript.Append("return confirm('" & str & "');")
            Return strScript.ToString
        End If
    End Function
    Public Shared Function Show(ByVal str As String, ByVal button As MessageBoxButton, ByVal obj As Object) As String
        Dim strScript As StringBuilder = New StringBuilder
        If button = MessageBoxButton.OK Then
            Return MessageBox.Show(str)
        Else
            strScript.Append("return confirm('" & str & "');")
            If button = MessageBoxButton.YesNo Then
                SaveObject(obj)
            End If
            Return strScript.ToString
        End If
    End Function
    Public Shared Function SaveObject(ByVal obj As Object) As Object
        obj = obj.save
        Return obj
    End Function
    'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
    Public Shared Function Show(ByVal title As String, ByVal message As String, Optional ByVal width As String = "", Optional ByVal IsTagRequired As Boolean = True) As String
        Dim strScript As StringBuilder = New StringBuilder
        If IsTagRequired Then
            strScript.Append(" <script language=javascript > ")
            If width.Trim.Length > 0 Then
                strScript.Append("OpenAlert1_1('" & title & "','" & message & "'," & width & ");")
            Else
                strScript.Append("OpenAlert1_1('" & title & "','" & message & "');")
            End If

            strScript.Append(" </script>")
        Else
            If width.Trim.Length > 0 Then
                strScript.Append("OpenAlert1_1('" & title & "','" & message & "'," & width & ");")
            Else
                strScript.Append("OpenAlert1_1('" & title & "','" & message & "');")
            End If
        End If
       
        Return strScript.ToString
    End Function
    'End
End Class
