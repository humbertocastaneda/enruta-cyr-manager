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
            'fecha1.Text = getFecha("yyyyMMdd")
            'fecha2.Text = getFecha("yyyyMMdd")
            Dim cookie As HttpCookie = Request.Cookies("verOrdenesPorEmpleado")

            If cookie Is Nothing Then
                'cookie = New HttpCookie("verOrdenesPorEmpleado")
                fecha1.Text = getFecha("yyyyMMdd")
                fecha2.Text = getFecha("yyyyMMdd")
            Else
                fecha1.Text = cookie("dia")
                fecha2.Text = cookie("dia2")
            End If

            'getResumen()
        End If
    End Sub

    Public Sub insertaRegistroTabla(conn As MySqlConnection, ByRef row As DataRow, estadoOrdenas As String, idEmpleado As Integer, tableField As String)

        Dim sqlSelect As String = "select count(ord.idOrden)  canti from empleados emp " & _
                         " left outer join tareas tar on (tar.idEmpleado=emp.idEmpleado) " & _
                         " left outer join ordenes ord  on (ord.idTarea = tar.idTarea) " & _
                         " where estadodelaorden = '" & estadoOrdenas & "' And emp.idEmpleado = " & idEmpleado & " " & _
                         " and tar.fechadeasignacion between STR_TO_DATE('" & fecha1.Text & "', '%Y%m%d%') and " & " STR_TO_DATE('" & fecha2.Text & "', '%Y%m%d%')" & _
                         " group by emp.idEmpleado"

        Dim dsTarea As DataSet = conectarMySql(conn, sqlSelect, "", False)

        If dsTarea.Tables(0).Rows.Count > 0 Then
            row.Item(tableField) = dsTarea.Tables(0).Rows(0).Item("canti")
        Else
            row.Item(tableField) = 0
        End If
    End Sub

    Sub getResumen()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Dim sqlSelect As String = "select ca.idlectura, ca.serieMedidor, ca.esferas, cli.direccion calle, cli.colonia, le.seriemedidor serieMedidor2 " & _
    "from cambiomedidor ca, lecturas le, clienteslect cli " & _
    "where fechaDeIngresoAlSistema between str_to_date('" & fecha1.Text & "', '%Y%m%d') and  str_to_date('" & fecha2.Text & "', '%Y%m%d') " & _
    "and le.idlectura=ca.idlectura and cli.idCliente=le.idCliente order by cli.sectorCorto"

        If fecha2.Text.Length = 0 Then
            fecha2.Text = fecha1.Text ' Si esta vacio quiere decir que debe bscar en el mismo dia...
        End If

        Try
            Dim ds As DataSet = conectarMySql(conn, sqlSelect, "cambiomedidor", True)

            dgOrdenes.DataSource = ds.Tables("cambiomedidor")
            dgOrdenes.DataBind()



        Catch ex As Exception
        Finally
            If Not conn Is Nothing Then
                conn.Close()



            End If

        End Try
        guardarFechasEnCookie()

    End Sub

    Protected Sub bRetrieve_Click(sender As Object, e As EventArgs) Handles bRetrieve.Click
        getResumen()
    End Sub



    Public Function linkClickedParams(mensaje As String) As String
        'messagebox(mensaje)
        Return "default.aspx?result=" & mensaje
    End Function

    Public Function getLinkVerResumen(value As Integer, id As String, estadoDeLaOrden As String, fecha1 As String, fecha2 As String) As String
        'messagebox(mensaje)
        If value > 0 Then
            Return "verOrdenesPorEmpleado.aspx?id=" & id & "&option=" & verResumenDesglose & "&estadoDeLaOrden=" & estadoDeLaOrden & "&fecha1=" & fecha1 & "&fecha2=" & fecha2
        Else
            Return "#"
        End If
    End Function

    Sub guardarFechasEnCookie()
        Dim cookie As HttpCookie = Request.Cookies("verOrdenesPorEmpleado")
        If cookie Is Nothing Then
            cookie = New HttpCookie("verOrdenesPorEmpleado")

        End If

        cookie("dia") = fecha1.Text
        cookie("dia2") = fecha2.Text

        cookie.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(cookie)
    End Sub

    Public Sub LinkButton_Click(sender As Object, e As EventArgs)
        Response.Redirect("verOrdenes.aspx?busqueda=" & CType(sender, LinkButton).Text & "&tipo=" & "ord.numOrden")
        'Response.Redirect("verOrdenes.aspx?busqueda=" & CType(sender, LinkButton).Text & "&tipo=" & "ord.numOrden&q=" & selectSQL)

    End Sub
    Protected Sub bAExcel_Click(sender As Object, e As EventArgs) Handles bAExcel.Click
        Dim nombreArchivo As String = "Cambio_de_Medidor_" & fecha1.Text & "_" & fecha2.Text

        gridAExcel(nombreArchivo, dgOrdenes)
    End Sub


End Class
