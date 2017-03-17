Imports Microsoft.VisualBasic

Public Class lectura
    Dim anomalia As String
    Dim lectura As String
    Dim comentarios As String
    Dim fecha As String
    Dim poliza As String
    Dim ptn As String

    Public Sub New(ls_linea As String)
        Try
            poliza = ls_linea.Substring(2, 8).Trim()
            lectura = ls_linea.Substring(10, 10).Trim()
            fecha = ""
            anomalia = ls_linea.Substring(ls_linea.Length - 1).Trim()
            ptn = "833359" + ls_linea.Substring(27, 4).Trim()
        Catch ex As Exception
            Throw New Exception("Error al compilar la siguiente linea " & ls_linea & " " & poliza & " " & lectura & " " & anomalia & " " & ptn)
        End Try
       
    End Sub

    Public Function getPoliza() As String
        Return poliza
    End Function
    Public Function getLectura() As String
        Return lectura
    End Function
    Public Function getPtn() As String
        Return ptn
    End Function
    Public Function getComentarios() As String
        Return comentarios
    End Function

    Public Function getAnomalia() As String
        Return anomalia
    End Function

    Public Function getfecha() As String
        Return fecha
    End Function

    

End Class
