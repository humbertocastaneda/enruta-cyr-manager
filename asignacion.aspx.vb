Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.IO

Partial Class _Default
    Inherits paginaGenerica



    Dim remocion As Integer = 0
    Dim desconexion As Integer = 0
    Dim elementosSeleccionados As New Hashtable()
    Dim cambiosGuardados As Boolean
    Dim titulo As String


    Dim idEmpleado As Integer = 0

    Protected Sub messagegenerico(mensaje As String)
        lbldialogo.Text = mensaje
        mp1.Show()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Not isLoged() Then
        '    Return
        'End If

        Dim ga As getArchivos = New getArchivos(Server.MapPath("~/") & "in\ordenes\")

        Try
            ' Traemos lo seleccionado
            Me.ViewState("idEmpleado") = ddl_tecnicos.SelectedValue
        Catch ex As Exception

        End Try



        If Not IsPostBack Then

            'actualizaListaTecnicos()
            actualizaFiltros()
            ddl_tecnicos.SelectedIndex = 0
            getOrdenes()

            setRestantes()

            titulo = Me.Title
            Me.ViewState("titulo") = titulo
            Me.ViewState("cambiosGuardados") = cambiosGuardados

            mensajeSeleccionarTecnico.Visible = True
            bAceptar.Enabled = False
            lb_Info.Text &= "Total de Ordenes Sin Asignar: <b>" & inputRestantes.Value & "</b>"
            'dgOrdenes.DataBind()
        Else
            titulo = CType(Me.ViewState("titulo"), String)
            cambiosGuardados = CType(Me.ViewState("cambiosGuardados"), Boolean)
            idEmpleado = CType(Me.ViewState("idEmpleado"), Integer)
            ' Si es un post back hay que seleccionar el usuario que se habia dado click
            'ddl_tecnicos.SelectedIndex = buscarIndexTecnico(idEmpleado)
        End If


        Me.MaintainScrollPositionOnPostBack = True
    End Sub

    Protected Sub gv_tecnicos_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.CssClass = e.Row.RowState.ToString()

            e.Row.Attributes.Add("onclick", String.Format("javascript:__doPostBack('gv_tecnicos','Select${0}')", e.Row.RowIndex))

        End If
    End Sub

    Public Sub orden_Click(sender As Object, e As EventArgs)
        Response.Redirect("verOrdenes.aspx?busqueda=" & CType(sender, LinkButton).Text & "&tipo=" & "ord.numOrden")
    End Sub

    Protected Sub obtieneSeleccionados()
        Dim fila As DataGridItem
        Dim check As CheckBox
        Dim j As Integer = 0
        remocion = 0
        desconexion = 0
        Try
            elementosSeleccionados.Clear()

            For Each fila In dgOrdenes.Items
                check = CType(fila.FindControl("cbAsignadas"), CheckBox)



                'vamos a agregar los elementos que tengamos checkeados en el arreglo
                If check.Checked Then

                    'ReDim Preserve elementosSeleccionados(i)
                    elementosSeleccionados(j) = "1"

                    Dim lb_tipo As LinkButton = CType(fila.FindControl("hltipoDeOrden"), LinkButton)
                    If lb_tipo.Text.Equals("REMOCION") Then
                        remocion += 1
                    ElseIf lb_tipo.Text.Equals("DESCONEXION") Then
                        desconexion += 1
                    End If

                End If

                j += 1
                'asignaDesasigna(If(check.Checked, lbEmpleados.SelectedValue.ToString, "null"), idOrden)

            Next

            lb_Info.Text = "<br/>Remociones:<b> " & remocion & "</b>"
            lb_Info.Text &= "&nbsp;&nbsp;&nbsp;&nbsp;Desconexiones: <b>" & desconexion & "</b>"
            lb_Info.Text &= "&nbsp;&nbsp;&nbsp;&nbsp;Total de Ordenes Asignadas: <b>" & (remocion + desconexion) & "</b>"
            lb_Info.Text &= "&nbsp;&nbsp;&nbsp;&nbsp;Total de Ordenes Sin Asignar: <b>" & inputRestantes.Value & "</b>"
            Me.ViewState("ElementosSeleccionados") = elementosSeleccionados
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub bAceptar_Click(sender As Object, e As EventArgs) Handles bAceptar.Click
        Dim fila As DataGridItem
        Dim check As CheckBox
        Dim lbOrden As Label
        Dim idOrden As String

        Dim i As Integer = 0

        If ddl_tecnicos.SelectedIndex = 0 Then

            messagegenerico("No se ha seleccionado un técnico")

            ' messagebox("No se ha seleccionado un técnico")
            Return
        End If

        elementosSeleccionados = CType(Me.ViewState("ElementosSeleccionados"), Hashtable)

        Try
            For Each fila In dgOrdenes.Items

                check = CType(fila.FindControl("cbAsignadas"), CheckBox)
                lbOrden = CType(fila.FindControl("lbIdOrden"), Label)
                idOrden = lbOrden.Text
                If (elementosSeleccionados.ContainsKey(i) And Not check.Checked) Or (Not elementosSeleccionados.ContainsKey(i) And check.Checked) Then
                    asignaDesasigna(ddl_tecnicos.SelectedValue, idOrden, IIf(check.Checked, "EO001", "EO000"), "")
                End If
                i += 1
            Next
            setRestantes()
            establecerCambiosGuardados(True)
            messagegenerico("Se han guardado los cambios.")
            ' messagebox("Se han guardado los cambios.")
        Catch ex As Exception
            messagegenerico("Ocurrio un error: " & ex.Message)
            ' messagebox("Ocurrio un error: " + ex.Message)
        End Try
        obtieneSeleccionados()
        'actualizaListaTecnicos()
    End Sub

    Sub establecerCambiosGuardados(cambiosGuardados As Boolean)
        Me.ViewState("cambiosGuardados") = cambiosGuardados
        If Not cambiosGuardados Then
            Me.Title = "* " & titulo
        Else
            Title = titulo
        End If
    End Sub

    'Public Sub asignaDesasigna(idEmpleado As String, idorden As String, estadoDeLaOrden As String)
    '    Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

    '    Dim conn As New MySqlConnection(cs)
    '    Dim ls_update As String
    '    Dim selectSQL As String
    '    Dim estadoDelEnvio As String = "EE002"
    '    Dim idTarea As String
    '    Dim cmd As New MySqlCommand()


    '    Try
    '        conn.Open()
    '        cmd.Connection = conn

    '        'getCadenaAEscribir(idorden, If(idEmpleado = "null", False, True), conn)

    '        'Buscamos si no hay una tarea del empleado seleccionado
    '        selectSQL = "Select idTarea from tareas where idEmpleado=" & idEmpleado & " and " & "fechadeAsignacion=str_to_date('" & getFecha("yyyyMMdd") & "', '%Y%m%d%')"

    '        Dim ds As DataSet = conectarMySql(conn, selectSQL, "tareas", False)

    '        If ds.Tables(0).Rows.Count > 0 Then
    '            ' hay una tarea  para el dia de hoy de este empleado
    '            idTarea = ds.Tables(0).Rows(0).Item("idtarea")
    '            'De cualquier manera hay que establecer la bandera a no enviada, ya que se cambiaron los datos
    '            Dim ls_insert As String = "update tareas set enviada='EE002' where idTarea=" & idTarea
    '            cmd.CommandText = ls_insert
    '            cmd.ExecuteNonQuery()
    '        Else
    '            'no hay tareas hay que generar una
    '            Dim ls_insert As String = "Insert into tareas(idEmpleado, idOrden, fechadeAsignacion, enviada) values( " & idEmpleado & ", " & idorden & ", str_to_date('" & getFecha("yyyyMMdd") & "', '%Y%m%d%'), 'EE002')"
    '            cmd.CommandText = ls_insert
    '            cmd.ExecuteNonQuery()
    '            idTarea = getLastInsertedId(conn, False)

    '        End If

    '        ' si vamos a desasignar y la orden se encuentra desAsignada y no enviada... no hay que enviarla, solo la desasignamos
    '        selectSQL = "select enviada from ordenes where idOrden=" & idorden

    '        Dim dsEnviada As DataSet = conectarMySql(conn, selectSQL, "tareas", False)
    '        If estadoDeLaOrden = "EO000" And dsEnviada.Tables(0).Rows(0).Item("enviada") = "EE002" Then
    '            idTarea = "null"
    '            estadoDelEnvio = "EE001"
    '        End If

    '        'vamos a sacar el estado anterior de esta orden...

    '        selectSQL = "select estadoDeLaOrden, idTarea from ordenes where idOrden=" & idorden

    '        ds = conectarMySql(conn, selectSQL, "ordenes", False)

    '        ls_update = "update ordenes set idTarea=" & idTarea & ", estadoDeLaOrden='" & estadoDeLaOrden & "', enviada='" & estadoDelEnvio & "' where idOrden=" & idorden

    '        cmd.CommandText = ls_update
    '        ' cmd.Connection = conn
    '        cmd.ExecuteNonQuery()

    '        If idTarea.Equals("null") Then
    '            idTarea = ds.Tables(0).Rows(0).Item("idTarea")
    '        End If


    '        'simuloTrigger(idorden, ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), conn)
    '        paginaGenerica.simuloTrigger(idorden, ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), estadoDeLaOrden, idTarea, conn)


    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If Not conn Is Nothing Then
    '            conn.Close()
    '        End If
    '    End Try
    'End Sub

    Public Sub getOrdenes()

        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)

        Try

            Dim selectSQL As String = "Select cli.entreCalles entreCalles,  ord.idOrden, ord.numOrden numorden, ord.descOrden, ord.descOrden tipoDeOrden , mu.nombre municipio, co.nombre colonia,  ca.nombre calle, cli.numExterior, cli.numInterior, " & _
                                      "if ((ord.EstadoDeLaOrden='EO000' ), 0, 1) asignada, cli.unidad unidad, cli.porcion porcion " & _
                                      "from ordenes ord left outer join  tareas tar on ( tar.idtarea=ord.idtarea), calles ca, colonias co, municipios mu, clientes cli " & _
                                      "where 1=1 " & getFiltro() & " and ((ord.EstadoDeLaOrden='EO000' and isnull(tar.idTarea)) or( tar.idEmpleado=" & ddl_tecnicos.SelectedValue & " and ord.estadodelaorden='EO001')) " & _
                                      "and ca.idcalle=cli.idCalle and cli.idcliente=ord.idCliente and ca.idColonia = co.idColonia and co.idMunicipio = mu.idMunicipio " & _
                                      "order By  cli.unidad, ord.tipodeorden, mu.nombre asc , co.nombre asc, ca.nombre asc,  cli.entreCalles"

            '            Select ord.idOrden, ord.numOrden numorden, ord.descOrden, ord.descOrden tipoDeOrden , mu.nombre municipio, co.nombre colonia,  ca.nombre calle, cli.numExterior, cli.numInterior, 
            'if ((ord.EstadoDeLaOrden='EO000' ), 0, 1) asignada 
            'from ordenes ord left outer join  tareas tar on ( tar.idtarea=ord.idtarea), calles ca, colonias co, municipios mu, clientes cli
            'where  ((ord.EstadoDeLaOrden='EO000' and isnull(tar.idTarea)) or( tar.idEmpleado=2 and ord.estadodelaorden='EO001'))
            'and ca.idcalle=cli.idCalle and cli.idcliente=ord.idCliente and ca.idColonia = co.idColonia and co.idMunicipio = mu.idMunicipio
            'order by mu.nombre, co.nombre, ca.nombre, ord.tipodeorden;




            '"Select idOrden, numOrden numorden, descOrden tipoDeOrden , municipio, colonia,  calle, numExterior, numInterior,  if (isnull(idEmpleado), 0, 1) asignada " & _
            '                "from ordenes where isnull(idEmpleado) or idEmpleado= " & lbEmpleados.SelectedValue & " order By municipio asc , colonia asc, calle asc"

            Dim dsOrdenes As DataSet = conectarMySql(conn, selectSQL, "Ordenes", True)
            dgOrdenes.DataSource = dsOrdenes.Tables("ordenes")

            dgOrdenes.DataBind()
            lb_Info.Visible = True

            If dsOrdenes.Tables(0).Rows.Count > 0 Then
                ls_tecnico.Visible = True
                lb_Info.Visible = True
                ls_tecnico.Text = "Técnico: <b>" & ddl_tecnicos.SelectedItem.Text & "</b>"
            Else
                ls_tecnico.Visible = False
                lb_Info.Visible = False
            End If
        Catch ex As Exception

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
    End Sub

    Protected Sub bObtenerOrdenes_Click(sender As Object, e As EventArgs) Handles bObtenerOrdenes.Click
        If ddl_tecnicos.SelectedIndex < 0 Then
            Return
        End If
        getOrdenes()
        obtieneSeleccionados()
        setRestantes()

        establecerCambiosGuardados(True)
    End Sub




    Public Sub LinkButton_Click(sender As Object, e As EventArgs)
        Dim check As CheckBox
        Dim colonia As String
        Dim clicked As LinkButton = CType(sender, LinkButton)
        Dim fila As DataGridItem
        ' ahora marcamos todos

        Try
            For Each fila In dgOrdenes.Items
                colonia = CType(fila.FindControl(clicked.ID), LinkButton).Text
                If colonia = clicked.Text Then
                    check = CType(fila.FindControl("cbAsignadas"), CheckBox)

                    If Not cbModoSeleccion.Checked Then
                        check.Checked = False
                    Else
                        check.Checked = True
                    End If

                End If


            Next
        Catch ex As Exception

        End Try

        cuentaOrdenes()
        establecerCambiosGuardados(False)
    End Sub

    Public Sub cuentaOrdenes()

        Dim check As CheckBox
        Dim fila As DataGridItem
        ' ahora marcamos todos
        remocion = 0
        desconexion = 0
        Try
            For Each fila In dgOrdenes.Items
                check = CType(fila.FindControl("cbAsignadas"), CheckBox)

                    If check.Checked Then
                        Dim lb_tipo As LinkButton = CType(fila.FindControl("hltipoDeOrden"), LinkButton)
                        If lb_tipo.Text.Equals("REMOCION") Then
                            remocion += 1
                        ElseIf lb_tipo.Text.Equals("DESCONEXION") Then
                            desconexion += 1
                        End If
                    End If



            Next
        Catch ex As Exception

        End Try



        

        lb_Info.Text = "<br/>Remociones:<b> " & remocion & "</b>"
        lb_Info.Text &= "&nbsp;&nbsp;&nbsp;&nbsp;Desconexiones: <b>" & desconexion & "</b>"
        lb_Info.Text &= "&nbsp;&nbsp;&nbsp;&nbsp;Total de Ordenes Asignadas: <b>" & (remocion + desconexion) & "</b>"
        lb_Info.Text &= "&nbsp;&nbsp;&nbsp;&nbsp;Total de Ordenes Sin Asignar: <b>" & inputRestantes.Value & "</b>"
    End Sub


    Protected Sub bEnviar_Click(sender As Object, e As EventArgs) Handles bEnviar.Click
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)

        Dim cmd As New MySqlCommand()
        cmd.Connection = conn
        Dim ls_update As String

        Dim selectSQL As String = "select count(*) canti from ordenes where estadoDeLaOrden='EO000'"
        Dim ds As DataSet
        Dim row As DataRow

        Try
            'ds = conectarMySql(conn, selectSQL, "ordenes", True)

            'If ds.Tables(0).Rows(0).Item("canti") > 0 Then
            '    'Si hay aun ordenes por asignar, indicamos continuar o quiere seguir asignando las ordenes

            '    Select Case MsgBox("Aún hay " & ds.Tables(0).Rows(0).Item("canti") & " ordene(s) por ser asignadas, ¿Desea continuar?", MsgBoxStyle.YesNo, "Asignar Ordenes")
            '        Case 7
            '            Return
            '    End Select


            'End If

            'Las ordenes tienen que ir ordenadas por entrecalles
            selectSQL = "select ord.idOrden idOrden, if(ord.estadoDeLaOrden='EO001', true, false) asignar , cli.entreCalles " & _
                        " from tareas tar, ordenes ord, clientes cli , calles ca, colonias co, municipios mu " & _
                        " where tar.idTarea = ord.idTarea and  cli.idCliente=ord.idCliente and ca.idCalle = cli.idCalle and ca.idColonia=co.idColonia and mu.idMunicipio = co.idMunicipio " & _
                        " and ord.enviada='EE002' and tar.enviada ='EE002'  and ord.estadodelaorden not in ('EO002','EO003','EO004','EO005','EO006','EO007', 'CD000','CD001','CD002' ) " & _
                        " order by mu.nombre, co.Nombre, cli.entreCalles ,ca.idCalle "
            ds = conectarMySql(conn, selectSQL, "ordenes", True)
            cmd.Connection = conn
            For Each row In ds.Tables(0).Rows
                getCadenaAEscribir2(row("idOrden"), row("asignar"), conn)

                ls_update = "update ordenes set enviada='EE001'"
                cmd.CommandText = ls_update
                cmd.ExecuteNonQuery()
            Next

            ls_update = "update tareas set enviada='EE001' where enviada='EE002'"
            cmd.CommandText = ls_update
            cmd.ExecuteNonQuery()
            messagegenerico("Ordenes enviadas al celular.")
            'messagebox("Ordenes enviadas al celular.")

        Catch ex As Exception
            messagegenerico("Ocurrio un error: " & ex.Message)
            ' messagebox("Ocurrio un error: " + ex.Message)
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try

    End Sub

    Public Sub actualizaFiltros()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim selectSQL As String = ""

        Dim conn As New MySqlConnection(cs)

        Try
            ' Actualizamos la lista de técnicos
            selectSQL = "select * from ((Select '-' nombreComp, -1 id from dual) union (Select  if(isnull(alias) or LENGTH(alias)=0, CONCAT_WS(' ',nombre,  appat, apmat), alias) nombreComp, emp.idEmpleado id " & _
                "from empleados emp " & _
                " where estado='EG001') ) emp2 order by nombreComp"

            Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "Empleados", True)

            ddl_tecnicos.DataSource = dsEmp
            ddl_tecnicos.DataTextField = "nombreComp"
            ddl_tecnicos.DataValueField = "id"

            ddl_tecnicos.DataBind()


            'Actualizamos los tipos de orden disponibles
            selectSQL = "select * from ((Select '(Tipo de Orden)' nombre, '-1' tipoDeOrden from dual) union (Select  concat(cod.nombre , '(', count(*), ')') nombre, ord.tipoDeOrden from  ordenes ord, codigos cod " & _
                        " where ord.tipoDeOrden = cod.codigo " & _
                        " and ord.estadoDeLaOrden in ('EO000', 'EO001')" & _
                        " group by ord.tipoDeOrden)) tmp order by nombre "

            Dim dsTipos As DataSet = conectarMySql(conn, selectSQL, "ordenes", False)
            ddl_tipoOrden.DataSource = dsTipos
            ddl_tipoOrden.DataTextField = "nombre"
            ddl_tipoOrden.DataValueField = "tipoDeOrden"

            ddl_tipoOrden.DataBind()

            'Actualizamos las porciones disponibles
            selectSQL = "Select * from ((Select '(Porciones)' porcion from dual) union (Select if (trim(cli.porcion)='', 'Vacias', cli.porcion) from  ordenes ord, clientes cli " & _
                        " where ord.idCliente = cli.idCliente " & _
                        " and ord.estadoDeLaOrden in ('EO000', 'EO001')" & _
                        " group by cli.porcion)) tmp order by porcion"

            Dim dsPorciones As DataSet = conectarMySql(conn, selectSQL, "ordenes", False)
            ddl_porcion.DataSource = dsPorciones
            ddl_porcion.DataTextField = "porcion"
            ddl_porcion.DataValueField = "porcion"

            ddl_porcion.DataBind()

            'Actualizamos las zonas disponibles
            selectSQL = "select * from ((Select '(Zonas)' zona from dual) union (Select if (trim(substr(cli.unidad, 1,3))='', 'Vacias',substr(cli.unidad, 1,3)) zona from  ordenes ord, clientes cli " & _
                        " where ord.idCliente = cli.idCliente " & _
                        " and ord.estadoDeLaOrden in ('EO000', 'EO001')" & _
                        " group by substr(cli.unidad, 1,3))) tmp order by zona "

            Dim dsZonas As DataSet = conectarMySql(conn, selectSQL, "ordenes", False)
            ddl_zona.DataSource = dsZonas
            ddl_zona.DataTextField = "zona"
            ddl_zona.DataValueField = "zona"

            ddl_zona.DataBind()

            'Unidades

            selectSQL = "Select * from ((Select '(Unidad)' unidad from dual) union (Select if (trim(cli.unidad)='', 'Vacias',cli.unidad)  from  ordenes ord, clientes cli " & _
                        " where ord.idCliente = cli.idCliente " & _
                        " and ord.estadoDeLaOrden in ('EO000', 'EO001')" & _
                        " group by cli.unidad)) tmp order by unidad"

            Dim dsUnidad As DataSet = conectarMySql(conn, selectSQL, "ordenes", False)
            ddl_unidad.DataSource = dsUnidad
            ddl_unidad.DataTextField = "unidad"
            ddl_unidad.DataValueField = "unidad"

            ddl_unidad.DataBind()

            selectSQL = "Select * from ((Select '(Colonias)' nombre, -1 idColonia from dual) union (Select co.nombre, co.idColonia from  ordenes ord, clientes cli, calles ca, colonias co " & _
                        " where ord.idCliente = cli.idCliente and ca.idCalle=cli.idCalle and co.idColonia=ca.idColonia " & _
                        " and ord.estadoDeLaOrden in ('EO000', 'EO001')" & _
                        " group by co.nombre)) tmp order by nombre "

            Dim dsColonias As DataSet = conectarMySql(conn, selectSQL, "ordenes", False)
            ddl_colonia.DataSource = dsColonias
            ddl_colonia.DataTextField = "nombre"
            ddl_colonia.DataValueField = "idColonia"

            ddl_colonia.DataBind()

            selectSQL = "select * from ((Select '(Municipios)' nombre, -1 idMunicipio from dual) union (Select mu.nombre, mu.idMunicipio from  ordenes ord, clientes cli, calles ca, colonias co, municipios mu " & _
                        " where ord.idCliente = cli.idCliente and ca.idCalle=cli.idCalle and co.idColonia=ca.idColonia and mu.idMunicipio= co.idMunicipio " & _
                        " and ord.estadoDeLaOrden in ('EO000', 'EO001')" & _
                        " group by mu.nombre )) tmp order by nombre"

            Dim dsMunicipios As DataSet = conectarMySql(conn, selectSQL, "ordenes", False)
            ddl_municipio.DataSource = dsMunicipios
            ddl_municipio.DataTextField = "nombre"
            ddl_municipio.DataValueField = "idMunicipio"

            ddl_municipio.DataBind()

        Catch ex As Exception

        End Try



    End Sub

    'Private Sub actualizaListaTecnicos()
    '    Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

    '    Dim conn As New MySqlConnection(cs)
    '    Try
    '        'Dim selectSQL As String = "Select concat(nombre,' ',  appat, ' ', apmat)nombreComp, idempleado id from empleados emp

    '        'Dim selectSQL As String = "Select concat_ws(' ', nombre, appat, apmat) nombreComp, idEmpleado id,  Remociones, Desconexiones, Reconexiones, " & _
    '        '    "recremos " & _
    '        '    "from empleados emp "

    '        Dim selectSQL As String = "Select  if(isnull(alias) or LENGTH(alias)=0, CONCAT_WS(' ',nombre,  appat, apmat), alias) nombreComp, emp.idEmpleado id, Remociones, Desconexiones, Reconexiones, " & _
    '            "recremos " & _
    '            "from empleados emp " & _
    '            " where estado='EG001' order by if(isnull(alias) or LENGTH(alias)=0, CONCAT_WS(' ',nombre,  appat, apmat), alias) "

    '        Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "Empleados", True)

    '        'lbEmpleados.DataSource = dsEmp
    '        'lbEmpleados.DataTextField = "nombreComp"
    '        'lbEmpleados.DataValueField = "id"


    '        gv_tecnicos.DataSource = dsEmp



    '    Catch ex As Exception

    '    Finally
    '        If Not conn Is Nothing Then
    '            conn.Close()
    '        End If

    '    End Try

    '    gv_tecnicos.DataBind()
    'End Sub

    Public Sub setRestantes()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)

        Dim cmd As New MySqlCommand()
        cmd.Connection = conn

        Dim selectSQL As String = "select count(*) canti from ordenes where estadoDeLaOrden='EO000' and isnull(idTarea)"
        Dim ds As DataSet

        ds = conectarMySql(conn, selectSQL, "ordenes", True)

        inputRestantes.Value = ds.Tables(0).Rows(0).Item("canti")

        'If ds.Tables(0).Rows(0).Item("canti") > 0 Then
        '    'Si hay aun ordenes por asignar, indicamos continuar o quiere seguir asignando las ordenes

        '    Select Case MsgBox("Aún hay " & ds.Tables(0).Rows(0).Item("canti") & " ordene(s) por ser asignadas, ¿Desea continuar?", MsgBoxStyle.YesNo, "Asignar Ordenes")
        '        Case 7
        '            Return
        '    End Select


        'End If
    End Sub


    'Protected Sub cbAsignadas_CheckedChanged(sender As Object, e As EventArgs)
    '    establecerCambiosGuardados(False)
    'End Sub


    '<System.Web.Services.WebMethod>
    'Protected Sub dgOrdenes_ItemCommand()


    'End Sub

    Protected Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        establecerCambiosGuardados(False)
        cuentaOrdenes()
    End Sub

    'Busca el tecnico en la lista de tecnicos. Si no lo encuentra regresa -1
    Sub buscarIndexTecnico(tecnico As Integer)
        'Dim fila As GridViewRow
        'Dim i As Integer = 0

        'For Each fila In gv_tecnicos.Rows
        '    'por cada fila buscamos el integer solicitado
        '    If fila.Cells(1).Text = tecnico Then
        '        Return i
        '    End If
        '    i += 1
        'Next
        'Return -1
        ddl_tecnicos.SelectedValue = tecnico
    End Sub

    'Protected Sub ddl_tecnicos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_tecnicos.SelectedIndexChanged
    '    getOrdenes()
    '    obtieneSeleccionados()
    '    setRestantes()
    'End Sub

    Function getFiltro() As String
        'Esta funcion obtiene el filtro de los drops
        'If ddl_tipoOrden.SelectedIndex>0
        Dim ls_filtro As String = ""

        If (Not ddl_tipoOrden.SelectedItem.Text.StartsWith("(")) Then
            ls_filtro = " and ord.tipoDeOrden='" & ddl_tipoOrden.SelectedValue & "' "
        End If

        If (Not ddl_porcion.SelectedItem.Text.StartsWith("(")) Then
            ls_filtro &= " and cli.porcion= '" & ddl_porcion.SelectedValue & "' "
        End If

        If (Not ddl_zona.SelectedItem.Text.StartsWith("(")) Then
            ls_filtro &= " and substr(cli.unidad, 1,3)= '" & ddl_zona.SelectedValue & "' "
        End If

        If (Not ddl_unidad.SelectedItem.Text.StartsWith("(")) Then
            ls_filtro &= " and cli.unidad= '" & ddl_unidad.SelectedValue & "' "
        End If

        If (Not ddl_municipio.SelectedItem.Text.StartsWith("(")) Then
            ls_filtro &= " and mu.idMunicipio= " & ddl_municipio.SelectedValue & " "
        End If

        If (Not ddl_colonia.SelectedItem.Text.StartsWith("(")) Then
            ls_filtro &= " and co.idColonia= " & ddl_colonia.SelectedValue & " "
        End If

        'If ls_filtro.Length > 0 Then
        '    ls_filtro &= " and "
        'End If
        Return ls_filtro
    End Function

    Protected Sub bFiltrar_Click(sender As Object, e As EventArgs) Handles bFiltrar.Click
        If ddl_tecnicos.SelectedIndex = 0 Then

            mensajeSeleccionarTecnico.Visible = True
            bAceptar.Enabled = False

        Else
            mensajeSeleccionarTecnico.Visible = False
            bAceptar.Enabled = True

        End If
        getOrdenes()
        obtieneSeleccionados()
        setRestantes()
    End Sub

    
    Protected Sub bSeleccionar_Click(sender As Object, e As EventArgs) Handles bSeleccionar.Click
        seleccionarDeseleccionar(True)
    End Sub

    Public Sub seleccionarDeseleccionar(estado As Boolean)
        Dim check As CheckBox
        Dim fila As DataGridItem
        ' ahora marcamos todos

        Try
            For Each fila In dgOrdenes.Items

                check = CType(fila.FindControl("cbAsignadas"), CheckBox)

                check.Checked = estado

            Next
        Catch ex As Exception

        End Try

        cuentaOrdenes()
        establecerCambiosGuardados(False)
    End Sub

    Protected Sub bDeseleccionar_Click(sender As Object, e As EventArgs) Handles bDeseleccionar.Click
        seleccionarDeseleccionar(False)
    End Sub

    
    
End Class
