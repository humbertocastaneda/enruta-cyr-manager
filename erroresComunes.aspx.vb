Imports MySql.Data.MySqlClient
Imports System.Data

Partial Class _Default
    Inherits paginaGenerica

    Public ii_tareas As Integer = 0
    Public ii_ordenes As Integer = 0
    Public ii_faltantes As Integer = 0



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Not isLoged() Then
        '    Return
        'End If
        Dim ga As getArchivos = New getArchivos(Server.MapPath("~/") & "in\ordenes\")
        If Not IsPostBack() Then
            setMenu(contenedor)

            'Select Case getRol()
            '    Case "RU000"
            '        menu1.Visible = True
            '    Case "RU001"
            '        menu1.Visible = True
            '    Case "RU002"
            '        menu2.Visible = True
            '    Case "RU003"
            '        menu3.Visible = True
            'End Select
        End If
    End Sub

   

End Class
