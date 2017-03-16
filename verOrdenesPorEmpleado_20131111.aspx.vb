Imports MySql.Data.MySqlClient
Imports System.Data

Partial Class verOrdenesPorEmpleado
    Inherits paginaGenerica


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Not isLoged() Then
        '    Return
        'End If
        Dim ga As getArchivos = New getArchivos(Server.MapPath("~/") & "in\ordenes\")
        If Not Me.IsPostBack Then
            Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

            Dim conn As New MySqlConnection(cs)
            Try
                Dim selectSQL As String = "Select concat_ws(' ', nombre, appat, apmat) nombreComp, emp.idEmpleado id, Remociones, Desconexiones, Reconexiones, " & _
                    "recremos " & _
                    "from empleados emp "

                Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "Empleados", True)

                'lbEmpleados.DataSource = dsEmp
                'lbEmpleados.DataTextField = "nombreComp"
                'lbEmpleados.DataValueField = "id"

                gv_tecnicos.DataSource = dsEmp

            Catch ex As Exception

            Finally
                If Not conn Is Nothing Then
                    conn.Close()
                End If

            End Try
            dia.Text = getFecha("yyyyMMdd")

            lbInfo.Text = "Seleccione un usuario y escriba una fecha para obtener datos"

            lbAlerta.Visible = True
            lbStatus.Visible = False
            gv_tecnicos.SelectedIndex = 0
            Me.DataBind()
        End If

    End Sub
    Public Sub LinkButton_Click(sender As Object, e As EventArgs)
        Response.Redirect("verOrdenes.aspx?busqueda=" & CType(sender, LinkButton).Text & "&tipo=" & "ord.numOrden")
    End Sub


    Protected Sub bObtenerOrdenes_Click(sender As Object, e As EventArgs) Handles bObtenerOrdenes.Click
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)
        Try
            getOrdenes()
            lbInfo.Text = "Técnico: " & gv_tecnicos.SelectedRow.Cells(0).Text


            If (rbTipoDeBusqueda.SelectedValue = 0) Then
                lbInfo.Text &= "<br/>" & "Fecha: " & dia.Text


            End If

            If dgOrdenes.Items.Count = 0 Then
                lbAlerta.Visible = True
                lbStatus.Visible = False


            Else
                lbAlerta.Visible = False

                If (rbTipoDeBusqueda.SelectedValue = 0) Then
                    lbStatus.Visible = False
                Else
                    lbStatus.Visible = True
                End If
            End If
        Catch ex As Exception

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try
    End Sub


    Public Sub getOrdenes()

        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)
        Try
            Dim selectSQL As String

            If (rbTipoDeBusqueda.SelectedValue = 0) Then 'Todas las que no se han empezado
                selectSQL = "Select ord.idOrden, ord.numOrden numorden, ord.descOrden tipodeOrden, est.nombre estadodelaorden ,ord.numMedidor, ord.lectura, mot.nombre motivo , ord.fechadeejecucion fechadeejecucion, ord.comentarios, ord.numSello " & _
                                      " from ordenes ord left outer join  tareas tar on ( tar.idtarea=ord.idtarea) left outer join codigos mot on (ord.motivo=mot.codigo), codigos est " & _
                                      "where  tar.idEmpleado=" & gv_tecnicos.SelectedRow.Cells(1).Text & " and est.codigo=ord.estadodelaorden" & " and ord.estadoDeLaorden='EO001'" & _
                                      " order By ord.fechadeejecucion asc"
            Else
                selectSQL = "Select ord.idOrden, ord.numOrden numorden, ord.descOrden tipodeOrden, est.nombre estadodelaorden ,ord.numMedidor, ord.lectura, mot.nombre motivo , ord.fechadeejecucion fechadeejecucion, ord.comentarios, ord.numSello " & _
                                      " from ordenes ord left outer join  tareas tar on ( tar.idtarea=ord.idtarea) left outer join codigos mot on (ord.motivo=mot.codigo), codigos est " & _
                                      "where  tar.idEmpleado=" & gv_tecnicos.SelectedRow.Cells(1).Text & " and est.codigo=ord.estadodelaorden" & " and tar.fechadeasignacion=STR_TO_DATE('" & dia.Text & "', '%Y%m%d%')"



                If ck_filtrarRealizadas.Checked Then
                    selectSQL &= " and ord.estadodelaorden<>'EO001'  "
                End If

                selectSQL &= " order By ord.fechadeejecucion asc"
            End If

            'selectSQL = "Select ord.idOrden, ord.numOrden numorden, ord.descOrden tipodeOrden, est.nombre estadodelaorden ,ord.numMedidor, ord.lectura, mot.nombre motivo , ord.fechadeejecucion fechadeejecucion, ord.comentarios, ord.numSello " & _
            '                          " from ordenes ord left outer join  tareas tar on ( tar.idtarea=ord.idtarea) left outer join codigos mot on (ord.motivo=mot.codigo), codigos est " & _
            '                          "where  tar.idEmpleado=" & lbEmpleados.SelectedValue & " and est.codigo=ord.estadodelaorden" & " and tar.fechadeasignacion=STR_TO_DATE('" & dia.Text & "', '%Y%m%d%')" & _
            '                          " order By ord.fechadeejecucion asc"


            '"Select idOrden, numOrden numorden, descOrden tipoDeOrden , municipio, colonia,  calle, numExterior, numInterior,  if (isnull(idEmpleado), 0, 1) asignada " & _
            '                "from ordenes where isnull(idEmpleado) or idEmpleado= " & lbEmpleados.SelectedValue & " order By municipio asc , colonia asc, calle asc"

            Dim dsOrdenes As DataSet = conectarMySql(conn, selectSQL, "Ordenes", True)
            dgOrdenes.DataSource = dsOrdenes.Tables("ordenes")

            dgOrdenes.DataBind()

            'Hora de inicio, hora fin

            If (rbTipoDeBusqueda.SelectedValue = 1) Then
                selectSQL = "Select" & _
                                         " max(ord.fechadeejecucion) fin, min(ord.fechadeejecucion) inicio from ordenes ord left outer join  tareas tar on ( tar.idtarea=ord.idtarea) left outer join codigos mot on (ord.motivo=mot.codigo), codigos est " & _
                                         "where  tar.idEmpleado=" & gv_tecnicos.SelectedRow.Cells(1).Text & " and est.codigo=ord.estadodelaorden" & " and tar.fechadeasignacion=STR_TO_DATE('" & dia.Text & "', '%Y%m%d%')" & _
                                         " order By ord.fechadeejecucion"
                Dim dsInfo As DataSet = conectarMySql(conn, selectSQL, "Ordenes", False)

                If IsDBNull(dsInfo.Tables(0).Rows(0).Item("inicio")) And IsDBNull(dsInfo.Tables(0).Rows(0).Item("fin")) Then
                    lbStatus.Text = "Sin Empezar"
                Else
                    lbStatus.Text = "Inicio " & dsInfo.Tables(0).Rows(0).Item("inicio") & " Fin: " & dsInfo.Tables(0).Rows(0).Item("fin")
                End If
            End If



        Catch ex As Exception
            Throw ex
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
    End Sub

    Protected Sub gv_tecnicos_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.CssClass = e.Row.RowState.ToString()

            e.Row.Attributes.Add("onclick", String.Format("javascript:__doPostBack('gv_tecnicos','Select${0}')", e.Row.RowIndex))

        End If
    End Sub

    Protected Sub lbEmpleados_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_tecnicos.SelectedIndexChanged
        
    End Sub

End Class
