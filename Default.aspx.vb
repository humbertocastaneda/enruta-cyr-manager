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
            getResumen()
        End If
    End Sub

    Sub getResumen()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        'Dim sqlSelect As String = "Select concat_ws(' ', nombre, appat, apmat) nombre, emp.Remociones, emp.Desconexiones, emp.Reconexiones, emp.recremos" & _
        '                        " from empleados emp left outer join tareas tar (tar.idEmpleado= emp.idEmpleado)"

        Dim sqlSelect As String = "Select if(isnull(alias) or LENGTH(alias)=0, CONCAT_WS(' ',nombre,  appat, apmat), alias) nombre, emp.Remociones, (select count(*) from lecturas ord where ord.tipoLectura='' and tar.idTarea=ord.idTarea and emp.idEmpleado=tar.idEmpleado) desconexiones, emp.Reconexiones, emp.recremos, " & _
            " ifnull(tar.Remociones, 0)remocionesAsign, ifnull(tar.Reconexiones, 0)reconexionesAsign, (select count(*) from lecturas ord where tar.idTarea=ord.idTarea and emp.idEmpleado=tar.idEmpleado) desconexionesAsign, ifnull(tar.Recremos, 0) RecremoAsign " & _
            " from empleadoslect emp left outer join tareaslect tar on (tar.idEmpleado= emp.idEmpleado and tar.fechaDeAsignacion = STR_TO_DATE('" & getFecha("yyyyMMdd") & "', '%Y%m%d%')) " & _
            " where estado='EG001' order by if(isnull(alias) or LENGTH(alias)=0, CONCAT_WS(' ',nombre,  appat, apmat), alias)"

        Try
            Dim ds As DataSet = conectarMySql(conn, sqlSelect, "", True)

            Dim row As DataRow = ds.Tables(0).Rows.Add()
            row.Item("nombre") = "Total"

            'sqlSelect = "select sum(remociones) remociones, sum(recremos) recremos, sum(desconexiones) desconexiones, sum(reconexiones) reconexiones from empleados  "

            sqlSelect = "Select sum(emp.Remociones) remociones,sum( (select count(*) from lecturas ord where ord.tipoLectura='' and tar.idTarea=ord.idTarea and emp.idEmpleado=tar.idEmpleado)) desconexiones, sum(emp.Reconexiones) reconexiones, sum(emp.recremos) recremos, " & _
                " sum(ifnull(tar.Remociones, 0)) remocionesAsign, sum(ifnull(tar.Reconexiones, 0)) reconexionesAsign, sum( (select count(*) from lecturas ord where tar.idTarea=ord.idTarea and emp.idEmpleado=tar.idEmpleado)) desconexionesAsign, sum( ifnull(tar.Recremos, 0)) RecremoAsign " & _
                " from empleadoslect emp left outer join tareaslect tar on (tar.idEmpleado= emp.idEmpleado and tar.fechaDeAsignacion = STR_TO_DATE('" & getFecha("yyyyMMdd") & "', '%Y%m%d%'))"

            Dim dsTotales As DataSet = conectarMySql(conn, sqlSelect, "", False)
            row.Item("Remociones") = dsTotales.Tables(0).Rows(0).Item("remociones")
            row.Item("recremos") = dsTotales.Tables(0).Rows(0).Item("recremos")
            row.Item("desconexiones") = dsTotales.Tables(0).Rows(0).Item("desconexiones")
            row.Item("reconexiones") = dsTotales.Tables(0).Rows(0).Item("reconexiones")
            row.Item("RemocionesAsign") = dsTotales.Tables(0).Rows(0).Item("remocionesAsign")
            row.Item("RecremoAsign") = dsTotales.Tables(0).Rows(0).Item("RecremoAsign")
            row.Item("desconexionesAsign") = dsTotales.Tables(0).Rows(0).Item("desconexionesAsign")
            row.Item("reconexionesAsign") = dsTotales.Tables(0).Rows(0).Item("reconexionesAsign")


            dgOrdenes.DataSource = ds.Tables(0)
            'dgOrdenes.DataBind()

            sqlSelect = "select count(*) canti from tareaslect  " & _
                    "where date_Format(fechaDeAsignacion, '%Y%m%d')= date_Format(now(), '%Y%m%d')"
            Dim dsTareas As DataSet = conectarMySql(conn, sqlSelect, "", False)
            ii_tareas = dsTareas.Tables(0).Rows(0).Item("canti")

            ii_ordenes += changeDBNull(dsTotales.Tables(0).Rows(0).Item("remociones"), 0)
            ii_ordenes += changeDBNull(dsTotales.Tables(0).Rows(0).Item("recremos"), 0)
            ii_ordenes += changeDBNull(dsTotales.Tables(0).Rows(0).Item("desconexiones"), 0)
            ii_ordenes += changeDBNull(dsTotales.Tables(0).Rows(0).Item("reconexiones"), 0)


            'sqlSelect = "select count(*) canti from lecturas  " & _
            '        "where tipoLectura='EO000' and isnull(idtarea)"
            'Dim dsFaltantes As DataSet = conectarMySql(conn, sqlSelect, "", False)
            'ii_faltantes = dsFaltantes.Tables(0).Rows(0).Item("canti")

            Me.DataBind()

        Catch ex As Exception
        Finally
            If Not conn Is Nothing Then
                conn.Close()



            End If

        End Try
    End Sub
End Class
