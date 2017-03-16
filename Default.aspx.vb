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
            getCentro()
            getResumen()

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

    Sub getResumen()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        'Dim sqlSelect As String = "Select concat_ws(' ', nombre, appat, apmat) nombre, emp.Remociones, emp.Desconexiones, emp.Reconexiones, emp.recremos" & _
        '                        " from empleados emp left outer join tareas tar (tar.idEmpleado= emp.idEmpleado)"

        Dim centro As String = lstcentral.SelectedValue

        If centro.EndsWith("0") Then
            centro = " 1=1 "
        Else
            centro = "1=1 and emp.centro='" & centro & "' "
        End If

        Dim sqlSelect As String = "Select concat_ws(' ', nombre, appat, apmat) nombre, emp.Remociones, (select count(*) from ordenes ord, tareas tar1 where ord.estadodelaorden='EO001' and tar1.idTarea=ord.idTarea and emp.idEmpleado=tar1.idEmpleado) Desconexiones, emp.Reconexiones, emp.recremos, " & _
            " ifnull(tar.Remociones, 0)remocionesAsign, ifnull(tar.Reconexiones, 0)reconexionesAsign, (select count(*) from ordenes ord where tar.idTarea=ord.idTarea) desconexionesAsign, ifnull(tar.Recremos, 0) RecremoAsign " & _
            " from empleados emp left outer join tareas tar on (tar.idEmpleado= emp.idEmpleado and tar.fechaDeAsignacion = STR_TO_DATE('" & getFecha("yyyyMMdd") & "', '%Y%m%d%'))  where " & centro & _
            " order by concat_ws(' ', nombre, appat, apmat)"

        Try
            Dim ds As DataSet = conectarMySql(conn, sqlSelect, "", True)

            Dim row As DataRow = ds.Tables(0).Rows.Add()
            row.Item("nombre") = "Total"

            'sqlSelect = "select sum(remociones) remociones, sum(recremos) recremos, sum(desconexiones) desconexiones, sum(reconexiones) reconexiones from empleados  "

            sqlSelect = "Select sum(emp.Remociones) remociones, sum( (select count(*) from ordenes ord, tareas tar1 where ord.estadodelaorden='EO001' and tar1.idTarea=ord.idTarea and emp.idEmpleado=tar1.idEmpleado)) desconexiones, sum(emp.Reconexiones) reconexiones, sum(emp.recremos) recremos, " & _
                " sum(ifnull(tar.Remociones, 0)) remocionesAsign, sum(ifnull(tar.Reconexiones, 0)) reconexionesAsign, sum(  (select count(*) from ordenes ord where tar.idTarea=ord.idTarea)) desconexionesAsign, sum( ifnull(tar.Recremos, 0)) RecremoAsign " & _
                " from empleados emp left outer join tareas tar on (tar.idEmpleado= emp.idEmpleado and tar.fechaDeAsignacion = STR_TO_DATE('" & getFecha("yyyyMMdd") & "', '%Y%m%d%')) where " & centro

            Dim dsTotales As DataSet = conectarMySql(conn, sqlSelect, "", False)

            row.Item("desconexiones") = dsTotales.Tables(0).Rows(0).Item("desconexiones")
           
            row.Item("desconexionesAsign") = dsTotales.Tables(0).Rows(0).Item("desconexionesAsign")


            dgOrdenes.DataSource = ds.Tables(0)
            'dgOrdenes.DataBind()

            sqlSelect = "select count(*) canti from tareas  " & _
                    "where date_Format(fechaDeAsignacion, '%Y%m%d')= date_Format(now(), '%Y%m%d')"
            Dim dsTareas As DataSet = conectarMySql(conn, sqlSelect, "", False)
            ii_tareas = dsTareas.Tables(0).Rows(0).Item("canti")

            ii_ordenes += changeDBNull(dsTotales.Tables(0).Rows(0).Item("remociones"), 0)
            ii_ordenes += changeDBNull(dsTotales.Tables(0).Rows(0).Item("recremos"), 0)
            ii_ordenes += changeDBNull(dsTotales.Tables(0).Rows(0).Item("desconexiones"), 0)
            ii_ordenes += changeDBNull(dsTotales.Tables(0).Rows(0).Item("reconexiones"), 0)


            sqlSelect = "select count(*) canti from ordenes  " & _
                    "where estadodelaorden='EO000' and isnull(idtarea)"
            Dim dsFaltantes As DataSet = conectarMySql(conn, sqlSelect, "", False)
            ii_faltantes = dsFaltantes.Tables(0).Rows(0).Item("canti")

            Me.DataBind()

        Catch ex As Exception
        Finally
            If Not conn Is Nothing Then
                conn.Close()



            End If

        End Try
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
End Class
