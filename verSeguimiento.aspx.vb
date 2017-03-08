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

        Dim ga As getArchivos = New getArchivos(Server.MapPath("~/") & getIn())

        If Not IsPostBack() Then
            'fecha1.Text = getFecha("yyyyMMdd")
            'fecha2.Text = getFecha("yyyyMMdd")
            setMenu(contenedor)
            Dim cookie As HttpCookie = Request.Cookies("verOrdenesPorEmpleado")

            If cookie Is Nothing Then
                'cookie = New HttpCookie("verOrdenesPorEmpleado")
                fecha1.Text = getFecha("yyyyMMdd")
                fecha2.Text = getFecha("yyyyMMdd")
            Else
                fecha1.Text = cookie("dia")
                fecha2.Text = cookie("dia2")
            End If

            getResumen()
        End If
    End Sub

    Public Sub insertaRegistroTabla(conn As MySqlConnection, ByRef row As DataRow, estadoOrdenas As String, idEmpleado As Integer, tableField As String)

        Dim filtro As String = parseMotivos(estadoOrdenas)
        'If filtro.Length > 0 Then
        '    filtro = " and " & filtro
        'End If
        Dim sqlSelect As String = "select count(ord.idTarea)  canti from empleadoslect emp " & _
                         " left outer join tareaslect tar on (tar.idEmpleado=emp.idEmpleado) " & _
                         " left outer join lecturas ord  on (ord.idTarea = tar.idTarea) " & _
                         " where 1=1 " & filtro & " And emp.idEmpleado = " & idEmpleado & " " & _
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
        Dim sqlSelect As String = "Select idEmpleado id,  concat_ws(' ', nombre, appat, apmat) nombre, emp.Remociones, emp.Desconexiones, emp.Reconexiones, emp.recremos" & _
                                " from empleadoslect emp where Estado<>'EG002' order by Nombre "

        If fecha2.Text.Length = 0 Then
            fecha2.Text = fecha1.Text ' Si esta vacio quiere decir que debe bscar en el mismo dia...
        End If

        Try
            Dim ds As DataSet = conectarMySql(conn, sqlSelect, "", True)
            Dim dsFinal As DataSet = New DataSet
            dsFinal.Tables.Add()
            dsFinal.Tables(0).Columns.Add("idEmpleado")
            dsFinal.Tables(0).Columns.Add("nombre")
            dsFinal.Tables(0).Columns.Add("asignadas")
            dsFinal.Tables(0).Columns.Add("pendientes")
            dsFinal.Tables(0).Columns.Add("ejecutadas")
            dsFinal.Tables(0).Columns.Add("reja")

            dsFinal.Tables(0).Columns.Add("consumoexcesivo")
            dsFinal.Tables(0).Columns.Add("consumobajo")
            dsFinal.Tables(0).Columns.Add("lecturainvalida")


            For Each rowEmp As DataRow In ds.Tables(0).Rows
                Dim row As DataRow = dsFinal.Tables(0).Rows.Add()
                row.Item("nombre") = rowEmp.Item("nombre")
                row.Item("idEmpleado") = rowEmp.Item("id")
                insertaRegistroTabla(conn, row, ASIGNADAS, rowEmp("id"), "asignadas")
                insertaRegistroTabla(conn, row, PENDIENTES, rowEmp("id"), "pendientes")
                insertaRegistroTabla(conn, row, EJECUTADAS, rowEmp("id"), "ejecutadas")
                insertaRegistroTabla(conn, row, REJA, rowEmp("id"), "reja")

                insertaRegistroTabla(conn, row, CONSUMO_EXCESIVO, rowEmp("id"), "consumoexcesivo")
                insertaRegistroTabla(conn, row, CONSUMO_BAJO, rowEmp("id"), "consumobajo")
                insertaRegistroTabla(conn, row, LECTURA_INVALIDA, rowEmp("id"), "lecturainvalida")

                'insertaRegistroTabla(conn, row, OTROS, rowEmp("id"), "otros")

            Next
            dgOrdenes.DataSource = dsFinal.Tables(0)
            dgOrdenes.DataBind()


            ''Dim row As DataRow = ds.Tables(0).Rows.Add()
            'row.Item("nombre") = "Total"

            'sqlSelect = "select sum(remociones) remociones, sum(recremos) recremos, sum(desconexiones) desconexiones, sum(reconexiones) reconexiones from empleados  "
            'Dim dsTotales As DataSet = conectarMySql(conn, sqlSelect, "", False)
            'row.Item("Remociones") = dsTotales.Tables(0).Rows(0).Item("remociones")
            'row.Item("recremos") = dsTotales.Tables(0).Rows(0).Item("recremos")
            'row.Item("desconexiones") = dsTotales.Tables(0).Rows(0).Item("desconexiones")
            'row.Item("reconexiones") = dsTotales.Tables(0).Rows(0).Item("reconexiones")

            'dgOrdenes.DataSource = ds.Tables(0)
            ''dgOrdenes.DataBind()

            'sqlSelect = "select count(*) canti from tareas  " & _
            '        "where date_Format(fechaDeAsignacion, '%Y%m%d')= date_Format(now(), '%Y%m%d')"
            'Dim dsTareas As DataSet = conectarMySql(conn, sqlSelect, "", False)
            'ii_tareas = dsTareas.Tables(0).Rows(0).Item("canti")

            'ii_ordenes += changeDBNull(dsTotales.Tables(0).Rows(0).Item("remociones"), 0)
            'ii_ordenes += changeDBNull(dsTotales.Tables(0).Rows(0).Item("recremos"), 0)
            'ii_ordenes += changeDBNull(dsTotales.Tables(0).Rows(0).Item("desconexiones"), 0)
            'ii_ordenes += changeDBNull(dsTotales.Tables(0).Rows(0).Item("reconexiones"), 0)


            'sqlSelect = "select count(*) canti from ordenes  " & _
            '        "where estadodelaorden='EO000' and isnull(idtarea)"
            'Dim dsFaltantes As DataSet = conectarMySql(conn, sqlSelect, "", False)
            'ii_faltantes = dsFaltantes.Tables(0).Rows(0).Item("canti")

            'Me.DataBind()

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

    Public Sub LinkButton_Click(sender As Object, e As EventArgs)
        'Dim ls_idTarea As DataGridItem = CType(sender, DataGridItem)
        'ls_idTarea.
        'Response.Redirect("verOrdenesPorEmpleado.aspx?option=" & verTarea & "&tarea=" & )
    End Sub

    Public Function linkClickedParams(mensaje As String) As String
        'messagebox(mensaje)
        Return "default.aspx?result=" & mensaje
    End Function

    Public Function getLinkVerResumen(value As Integer, id As String, estadoDeLaOrden As String, fecha1 As String, fecha2 As String) As String
        'messagebox(mensaje)
        If value > 0 Then
            Return "verOrdenesPorEmpleado.aspx?id=" & id & "&option=" & verSeguimientoDesglose & "&motivo=" & estadoDeLaOrden & "&fecha1=" & fecha1 & "&fecha2=" & fecha2
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

End Class
