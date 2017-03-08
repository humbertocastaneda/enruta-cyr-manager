Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports MySql.Data.MySqlClient
Imports Ionic.Zip
Imports paginaGenerica
Imports System.Security.AccessControl
Imports System.Security.Principal

Partial Class _Default
    Inherits paginaGenerica

    Public cantidad_total As Integer = 0
    Public espacio_total As Decimal = 0
    Public desgloce As String = ""
    Public tipoDeBusquedaText As String = ""

    Dim dias As Hashtable
    Dim cantidades As Hashtable
    Dim maximos As Hashtable
    Dim minimos As Hashtable



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        
        If Not IsPostBack Then

            tb_ruta.Text = Server.MapPath("~/")
        End If

    End Sub

    Public Sub actualizaResumen()
        dias = New Hashtable()
        cantidades = New Hashtable()
        maximos = New Hashtable()
        minimos = New Hashtable()

        tipoDeBusquedaText = "Busqueda por Fotos"
        'Dim servidor As String = subirDirectorios(Server.MapPath("~/"), 2) & "AppsTampico\"
        Dim servidor As String = Server.MapPath("~/") & "\"
        Dim ls_directorio_fotos As String = servidor & "fotos\"
        Dim fotos As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(ls_directorio_fotos, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*.jpg")
        For Each archivo As String In fotos

            Dim info As New FileInfo(archivo)

            agregarPesoDia(archivo.Substring(archivo.Length - 18, 8), info.Length)

        Next
        cantidad_total = 0
        espacio_total = 0
        desgloce = ""


        Dim dsFinal As DataSet = New DataSet
        dsFinal.Tables.Add()
        dsFinal.Tables(0).Columns.Add("dia")
        dsFinal.Tables(0).Columns.Add("espacio")
        dsFinal.Tables(0).Columns.Add("cantidad")
        dsFinal.Tables(0).Columns.Add("maximos")
        dsFinal.Tables(0).Columns.Add("minimos")



        For Each entry As DictionaryEntry In dias
            Dim row As DataRow = dsFinal.Tables(0).Rows.Add()

            Dim espacio = Math.Round(Convert.ToDecimal(entry.Value), 2)
            'Dim formato = " Mb"
            ''If (Math.Round(Convert.ToDecimal(entry.Value), 2) = 0.0) Then
            'espacio = entry.Value
            ''End If
            'desgloce &= "Dia " & entry.Key & " " & Math.Round(espacio, 2) & formato & " en " & cantidades(entry.Key) & " archivos <br />"

            row.Item("dia") = entry.Key
            row.Item("espacio") = espacio
            row.Item("cantidad") = cantidades(entry.Key)
            row.Item("maximos") = Math.Round(Convert.ToDecimal(maximos(entry.Key)), 2)
            row.Item("minimos") = Math.Round(Convert.ToDecimal(minimos(entry.Key)), 5)
            cantidad_total += cantidades(entry.Key)
            espacio_total += entry.Value

        Next
        dsFinal.Tables(0).DefaultView.Sort = "dia asc"
        GridView1.DataSource = dsFinal
        espacio_total = Math.Round(espacio_total, 2)
        Me.DataBind()

    End Sub


    Public Sub agregarPesoDia(dia As String, peso As Decimal)
        peso /= 1048576
        If Not dias.ContainsKey(dia) Then
            'Si no existe agregamos el valor
            dias.Add(dia, peso)
            cantidades.Add(dia, 1)
            minimos.Add(dia, peso)
            maximos.Add(dia, peso)
        Else
            'Si existe lo sumamos
            dias(dia) = dias(dia) + peso
            cantidades(dia) += 1
            If minimos(dia) > peso Then
                minimos(dia) = peso
            End If

            If maximos(dia) < peso Then
                maximos(dia) = peso
            End If
        End If



    End Sub
  

    Protected Sub b_reporte_Click(sender As Object, e As EventArgs) Handles b_reporte.Click
        actualizaResumen()
    End Sub

    Public Sub actualizaDirectorio()
        tipoDeBusquedaText = "Busqueda por Directorios"
        dias = New Hashtable()
        cantidades = New Hashtable()
        maximos = New Hashtable()
        minimos = New Hashtable()
        Dim tipoDeBusqueda = Microsoft.VisualBasic.FileIO.SearchOption.SearchTopLevelOnly
        If cb_subdirectorios.Checked Then
            tipoDeBusqueda = FileIO.SearchOption.SearchAllSubDirectories
        End If
        'Dim servidor As String = subirDirectorios(Server.MapPath("~/"), 2) & "AppsTampico\"
        Dim fotos As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(tb_ruta.Text, tipoDeBusqueda, "*.*")
        'Dim fotos = DirSearch(tb_ruta.Text)
        For Each archivo As String In fotos

            Dim info As New FileInfo(archivo)

            agregarPesoDia(info.Name, info.Length)

        Next
        cantidad_total = 0
        espacio_total = 0
        desgloce = ""


        Dim dsFinal As DataSet = New DataSet
        dsFinal.Tables.Add()
        dsFinal.Tables(0).Columns.Add("archivo")
        dsFinal.Tables(0).Columns.Add("espacio")
        dsFinal.Tables(0).Columns.Add("cantidad")

        For Each entry As DictionaryEntry In dias


            Dim espacio = Math.Round(Convert.ToDecimal(entry.Value), 2)
            'Dim formato = " Mb"
            ''If (Math.Round(Convert.ToDecimal(entry.Value), 2) = 0.0) Then
            'espacio = entry.Value
            ''End If
            'desgloce &= "Dia " & entry.Key & " " & Math.Round(espacio, 2) & formato & " en " & cantidades(entry.Key) & " archivos <br />"
            If ck_mostrararchivos.Checked Then
                Dim row As DataRow = dsFinal.Tables(0).Rows.Add()
                row.Item("archivo") = entry.Key
                row.Item("espacio") = espacio
                row.Item("cantidad") = cantidades(entry.Key)
            End If


            cantidad_total += cantidades(entry.Key)
            espacio_total += entry.Value

        Next

        dsFinal.Tables(0).DefaultView.Sort = "archivo"
        GridView1.DataSource = dsFinal
        espacio_total = Math.Round(espacio_total, 2)
        Me.DataBind()

    End Sub

    Protected Sub b_genera_directorio_Click(sender As Object, e As EventArgs) Handles b_genera_directorio.Click
        actualizaDirectorio()
    End Sub

    Private Function DirSearch(sDir As String) As List(Of String)
        Dim files As List(Of String) = New List(Of String)
        Try
            For Each f As String In Directory.GetFiles(sDir)
                files.Add(f)
            Next
            For Each d As String In Directory.GetDirectories(sDir)

                files.AddRange(DirSearch(d))
            Next
        Catch ex As Exception
        End Try
        Return files
    End Function
End Class
