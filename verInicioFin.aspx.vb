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
            'fecha1.Text = getFecha("yyyyMMdd")
            'fecha2.Text = getFecha("yyyyMMdd")
            Dim cookie As HttpCookie = Request.Cookies("verOrdenesPorEmpleado")

            'If cookie Is Nothing Then
            'cookie = New HttpCookie("verOrdenesPorEmpleado")
            fecha1.Text = getFecha("yyyyMMdd")
            'fecha2.Text = getFecha("yyyyMMdd")
            'Else
            '    fecha1.Text = cookie("dia")
            '    'fecha2.Text = cookie("dia2")
            'End If
            getCentro()
            getResumen()
        End If
    End Sub

    Public Sub insertaRegistroTabla(conn As MySqlConnection, ByRef row As DataRow, estadoOrdenas As String, idEmpleado As Integer, tableField As String)

        Dim sqlSelect As String = "select  data_format(max(ord.fechaDeEjecucion), '%H:%i:%s' ) maximo , data_format(min(ord.fechaDeEjecucion), '%H:%i:%s' ) minimo  from empleados emp  " & _
                          "left outer join tareas tar on (tar.idEmpleado=emp.idEmpleado)    " & _
                          "left outer join ordenes ord  on (ord.idTarea = tar.idTarea)    " & _
                          "where tar.fechadeasignacion =STR_TO_DATE('" & fecha1.Text & "', '%Y%m%d%')    " & _
                          "group by emp.idEmpleado"

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
       

        Dim sqlSelect As String = "select emp.idEmpleado id, concat_ws(' ', nombre, appat, apmat) nombre ,  if (max(ord.fechaDeEjecucion)=min(ord.fechaDeEjecucion), '', date_format(max(ord.fechaDeEjecucion), '%H:%i:%s' )) maximo , date_format(min(ord.fechaDeEjecucion), '%H:%i:%s' ) minimo  from empleados emp  " & _
                          "left outer join tareas tar on (tar.idEmpleado=emp.idEmpleado)    " & _
                          "left outer join ordenes ord  on (ord.idTarea = tar.idTarea)    and  estadoDeLaOrden<>'EO012'" & _
                          "where tar.fechadeasignacion =STR_TO_DATE('" & fecha1.Text & "', '%Y%m%d%')    and " & getCentroLectura() & _
                          " group by emp.idEmpleado order by concat_ws(' ', nombre, appat, apmat)"

        Try
            Dim ds As DataSet = conectarMySql(conn, sqlSelect, "ordenes", True)
           
            dgOrdenes.DataSource = ds.Tables(0)
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

    Public Sub LinkButton_Click(sender As Object, e As EventArgs)
        'Dim ls_idTarea As DataGridItem = CType(sender, DataGridItem)
        'ls_idTarea.
        'Response.Redirect("verOrdenesPorEmpleado.aspx?option=" & verTarea & "&tarea=" & )
    End Sub

    Public Function linkClickedParams(mensaje As String) As String
        'messagebox(mensaje)
        Return "default.aspx?result=" & mensaje
    End Function

    Public Function getLinkVerResumen(id As String, estadoDeLaOrden As String, fecha1 As String) As String
        'messagebox(mensaje)
        Return "verOrdenesPorEmpleado.aspx?id=" & id & "&option=" & verSeguimientoDesglose & "&motivo=" & estadoDeLaOrden & "&fecha1=" & fecha1

    End Function

    Sub guardarFechasEnCookie()
        Dim cookie As HttpCookie = Request.Cookies("verOrdenesPorEmpleado")
        If cookie Is Nothing Then
            cookie = New HttpCookie("verOrdenesPorEmpleado")

        End If

        cookie("dia") = fecha1.Text
        'cookie("dia2") = fecha2.Text

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
