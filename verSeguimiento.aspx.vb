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
            fecha1.Text = getFecha("yyyyMMdd")
            fecha2.Text = getFecha("yyyyMMdd")
            'Dim cookie As HttpCookie = Request.Cookies("verOrdenesPorEmpleado")

            'If cookie Is Nothing Then
            '    'cookie = New HttpCookie("verOrdenesPorEmpleado")
            '    fecha1.Text = getFecha("yyyyMMdd")
            '    fecha2.Text = getFecha("yyyyMMdd")
            'Else
            '    fecha1.Text = cookie("dia")
            '    fecha2.Text = cookie("dia2")
            'End If
            getCentro()

            getResumen()
        End If
    End Sub

    Public Function cuentaRegistroTabla(conn As MySqlConnection, estadoOrdenas As String, idEmpleado As Integer) As Integer
        Dim filtro As String = parseMotivos(estadoOrdenas)
        'If filtro.Length > 0 Then
        '    filtro = " and " & filtro
        'End If
        Dim sqlSelect As String = "select count(ord.idOrden)  canti from empleados emp " & _
                         " left outer join tareas tar on (tar.idEmpleado=emp.idEmpleado) " & _
                         " left outer join ordenes ord  on (ord.idTarea = tar.idTarea) " & _
                         " where 1=1 " & filtro & " And emp.idEmpleado = " & idEmpleado & " " & _
                         " and tar.fechadeasignacion between STR_TO_DATE('" & fecha1.Text & "', '%Y%m%d%') and " & " STR_TO_DATE('" & fecha2.Text & "', '%Y%m%d%')" & _
                         " group by emp.idEmpleado"

        Dim dsTarea As DataSet = conectarMySql(conn, sqlSelect, "", False)

        If dsTarea.Tables(0).Rows.Count > 0 Then
            Return dsTarea.Tables(0).Rows(0).Item("canti")
        Else
            Return 0
        End If
    End Function

    Public Sub insertaRegistroTabla(conn As MySqlConnection, ByRef row As DataRow, estadoOrdenas As String, idEmpleado As Integer, tableField As String)

        


        row.Item(tableField) = cuentaRegistroTabla(conn, estadoOrdenas, idEmpleado)
    End Sub

    Sub getResumen()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Dim sqlSelect As String = "Select idEmpleado id, if(isnull(alias) or LENGTH(alias)=0, CONCAT_WS(' ',nombre,  appat, apmat), alias) nombre, emp.Remociones, emp.Desconexiones, emp.Reconexiones, emp.recremos" & _
                                " from empleados emp where " & getCentroLectura() & " and  estado='EG001' order by if(isnull(alias) or LENGTH(alias)=0, CONCAT_WS(' ',nombre,  appat, apmat), alias)"

        If fecha2.Text.Length = 0 Then
            fecha2.Text = fecha1.Text ' Si esta vacio quiere decir que debe bscar en el mismo dia...
        End If

        Try
            Dim ds As DataSet = conectarMySql(conn, sqlSelect, "", True)
            Dim dsFinal As DataSet = New DataSet
            dsFinal.Tables.Add()
            dsFinal.Tables(0).Columns.Add("nombre")
            dsFinal.Tables(0).Columns.Add("asignadas")
            dsFinal.Tables(0).Columns.Add("pendientes")
            dsFinal.Tables(0).Columns.Add("ejecutadas")
            dsFinal.Tables(0).Columns.Add("autoreconectados")
            dsFinal.Tables(0).Columns.Add("reja")
            'dsFinal.Tables(0).Columns.Add("cte_no_permite")
            dsFinal.Tables(0).Columns.Add("no_visitadas")
            dsFinal.Tables(0).Columns.Add("incorrectas")
            dsFinal.Tables(0).Columns.Add("sinregistro")
            dsFinal.Tables(0).Columns.Add("idEmpleado")

            For Each rowEmp As DataRow In ds.Tables(0).Rows

                If ck_soloPendientes.Checked Then
                    If cuentaRegistroTabla(conn, PENDIENTES, rowEmp("id")) = 0 Then
                        Continue For
                    End If
                End If
                Dim row As DataRow = dsFinal.Tables(0).Rows.Add()
                row.Item("nombre") = rowEmp.Item("nombre")
                row.Item("idEmpleado") = rowEmp.Item("id")
                insertaRegistroTabla(conn, row, ASIGNADAS, rowEmp("id"), "asignadas")
                insertaRegistroTabla(conn, row, PENDIENTES, rowEmp("id"), "pendientes")
                insertaRegistroTabla(conn, row, EJECUTADAS, rowEmp("id"), "ejecutadas")
                insertaRegistroTabla(conn, row, AUTORECONECTADO, rowEmp("id"), "autoreconectados")
                insertaRegistroTabla(conn, row, REJA, rowEmp("id"), "reja")
                'insertaRegistroTabla(conn, row, CTE_NO_PERMITE, rowEmp("id"), "cte_no_permite")
                insertaRegistroTabla(conn, row, NO_VISITADAS, rowEmp("id"), "no_visitadas")
                insertaRegistroTabla(conn, row, INCORRECTAS, rowEmp("id"), "incorrectas")
                insertaRegistroTabla(conn, row, SIN_REGISTRO, rowEmp("id"), "sinregistro")
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
    Protected Sub lstcentral_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstcentral.SelectedIndexChanged
        Dim cookie As HttpCookie = Request.Cookies("globales")

        If cookie Is Nothing Then
            cookie = New HttpCookie("globales")
        End If

        cookie("centro") = lstcentral.SelectedValue

        cookie.Expires = DateTime.Now.AddDays(30)
        Response.Cookies.Add(cookie)

        getResumen()
    End Sub

    Public Sub getCentro()
        Dim cookie As HttpCookie = Request.Cookies("globales")

        If Not cookie Is Nothing Then
            'cookie = New HttpCookie("verOrdenesPorEmpleado")
            lstcentral.SelectedValue = cookie("centro")
        End If
    End Sub

    Protected Function getCentroLectura() As String
        Dim centro As String = lstcentral.SelectedValue

        If centro.EndsWith("0") Then
            centro = " 1=1 "
        Else
            centro = "1=1 and emp.centro='" & centro & "' "
        End If

        Return centro
    End Function



End Class
