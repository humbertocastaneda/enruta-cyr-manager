Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf



Partial Class _Default
    Inherits paginaGenerica



    Dim idCliente, idOrden As Integer
    Dim crearListaOrdenes As Boolean = True
    Dim busqueda As String
    Dim tipo As String
    Dim dsLista As DataSet
    Dim lugarEnLista As Integer
    Dim rol As String

    Public motivoVisible As Boolean = True

    Public ls_lectura_real, ls_tipoDeOrden, ls_numOrden, ls_estadoDeLaOrden, ls_motivo, ls_fechaDeEjecucion, _
       ls_lectura, ls_medidor, ls_anio, ls_serie, ls_comentarios, ls_tecnico, ls_direccion, ls_nombre, ls_poliza, ls_estadoDeLaOrdenCod, ls_idTarea, ls_sello, ls_estadoDeLaRevision _
       , ls_descEstadoRev, ls_latitud, ls_longitud, ls_accion, ls_numOrdenReal, ls_fechaDeInicio, ls_habitada, ls_sinRegistro, ls_vencido, ls_ultimoPago, ls_balance, ls_diametro, ls_tipoUsuario, ls_giro, ls_fechaUltimoPago As String

    Protected Sub message_generico(mensaje As String)
        lbldialogo.Text = mensaje
        mp1.Show()
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Not isLoged() Then
        '    Return
        'End If

        Dim ga As getArchivos = New getArchivos(Server.MapPath("~/") & "in\ordenes\")

        rol = getRol()


        If Not IsPostBack() Then
            setMenu(contenedor)
            If Not Request.QueryString("busqueda") = "" And Not Request.QueryString("tipo") = "" Then
                setDatos(Request.QueryString("tipo"), Request.QueryString("busqueda"))

                If Not Request.QueryString("q") = "" Then
                    'generamos un dataset
                    obtieneDSListado()

                End If

            Else
                DatosOrden.Visible = False
                datosCliente.Visible = False
            End If
            manejaBotonesDireccion()
        End If

        If Not Me.ViewState("idCliente") = Nothing Then
            idCliente = CType(Me.ViewState("idCliente"), Integer)
            idOrden = CType(Me.ViewState("idOrden"), Integer)
            busqueda = CType(Me.ViewState("busqueda"), String)
            tipo = CType(Me.ViewState("tipo"), String)
            ls_idTarea = CType(Me.ViewState("idTarea"), String)
            motivoVisible = CType(Me.ViewState("motivoVisible"), Boolean)
            ls_estadoDeLaRevision = CType(Me.ViewState("ls_estadoDeLaRevision"), String)
        Else
            idCliente = Nothing
            idOrden = Nothing


        End If

        Try
            dsLista = CType(Me.ViewState("dsLista"), DataSet)
            lugarEnLista = CType(Me.ViewState("lugarEnLista"), Integer)
        Catch ex As Exception

        End Try

        b_imprimir.Attributes.Clear()
        b_imprimir.Attributes.Add("onclick", returnPrintParams("contenido"))

    End Sub



    'Utiliza los ultimos parametros de set datos
    Public Sub setDatos()
        If tipo = "" Or busqueda = "" Then
            Return

        End If

        setDatos(tipo, busqueda)

        b_imprimir.Attributes.Clear()
        b_imprimir.Attributes.Add("onclick", returnPrintParams("DatosOrden"))
    End Sub


    Public Sub setDatos(tipoBusqueda As String, busqueda As Integer)
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Me.tipo = tipoBusqueda
        Me.busqueda = busqueda
        Me.ViewState("tipo") = tipo
        Me.ViewState("busqueda") = busqueda
        Dim sqlSelect As String = "select ord.lecturareal, ord.vencido, ord.diametro_toma, ord.tipo_usuario, ord.giro, ord.balance, ord.fecha_ultimo_pago, ord.ultimo_pago, ord.sinRegistro,ord.habitado, ord.fechaDeInicio, ord.latitud, ord.longitud, ord.numSello, ord.estadoDeLaRevision, cast(if (isnull(tar.idtarea),'' ,tar.idtarea ) as char(10)) idTarea,  cli.poliza poliza, ord.estadoAnterior, ord.idOrden idOrden, cli.idcliente idCliente, ord.idOrden numOrden, ord.numOrden numOrdenReal, ord.tipoDeOrden,  ord.fechaDeEjecucion fechaDeEjecucion, " & _
                        "ord.lectura lectura, ord.numMedidor numMedidor, ord.ano anio, ord.numSerie numSerie, ord.estadodelaorden estadodelaordencod, " & _
                        "ord.Comentarios comentarios, eo.nombre estadoDeLaOrden, concat_ws(' ', emp.Nombre, emp.ApPat, emp.ApMat) tecnico, ca.nombre calle, " & _
                        "col.nombre colonia, mun.nombre municipio, cli.Nombre cliente, cli.poliza , cli.entreCalles, mo.nombre motivo, cli.numExterior, cli.numInterior, cli.entreCalles, cli.comoLlegar " & _
                        "from ordenes ord left outer join tareas tar on (tar.idtarea=ord.idTarea) " & _
                        "left outer join empleados emp on (tar.idEmpleado=emp.idEmpleado) " & _
                        "left outer join codigos mo on (mo.codigo=ord.motivo), " & _
                        "codigos eo, " & _
                        "clientes cli, " & _
                        "municipios mun, " & _
                        "colonias col, " & _
                        "calles ca " & _
                        "where ord.idCliente = cli.idCliente " & _
                        "and ord.estadodelaorden= eo.codigo " & _
                        "and ca.idColonia = col.idColonia " & _
                        "and col.idMunicipio = mun.idMunicipio " & _
                        "and ca.idCalle = cli.idCalle " & _
                        "and " & tipoBusqueda & "=" & busqueda & _
                        " order by ord.fechaDeEjecucion desc limit 1"

        Try
            Dim ds As DataSet = conectarMySql(conn, sqlSelect, "", True)
            If ds.Tables(0).Rows.Count = 0 Then
                DatosOrden.Visible = False

                datosCliente.Visible = False
                lbOrdenes.Items.Clear()
                Return
            End If
            DatosOrden.Visible = True
            datosCliente.Visible = True


            Dim row As DataRow = ds.Tables(0).Rows(0)

            ls_lectura_real = row("lecturareal")
            ls_tipoDeOrden = parseCodigo(conn, row("tipodeorden"))
            ls_accion = parseCodigo(conn, "AC" + row("lectura").ToString.PadLeft(3, "0"))
            ls_estadoDeLaOrden = row("estadoDeLaOrden")
            ls_motivo = changeDBNull(row("motivo"), "Sin Motivo")
            ls_fechaDeEjecucion = changeDBNull(row("fechaDeEjecucion"), "S/D")
            ls_lectura = changeDBNull(row("lectura"), "0")
            ls_medidor = changeDBNull(row("nummedidor"), "S/D")
            ls_anio = changeDBNull(row("anio"), "S/D")
            ls_serie = changeDBNull(row("numserie"), "S/D")
            ls_comentarios = changeDBNull(row("comentarios"))
            ls_tecnico = changeDBNull(row("tecnico"), "Sin Asignar")
            ls_numOrden = row("numOrden")
            idCliente = row("idCliente")
            idOrden = row("idOrden")
            ls_poliza = row("poliza")
            ls_estadoDeLaOrdenCod = row("estadodelaordencod")
            ls_idTarea = row("idTarea")
            ls_estadoDeLaRevision = row("estadoDeLaRevision")
            ls_sello = changeDBNull(row("numSello"), "S/D")
            ls_latitud = changeDBNull(row("latitud"), "0.0")
            ls_longitud = changeDBNull(row("longitud"), "0.0")
            ls_numOrdenReal = changeDBNull(row("numOrdenReal"), "N")
            ls_habitada = changeDBNull(row("habitado"), "S/D")
            ls_fechaDeInicio = changeDBNull(row("fechaDeInicio"), "S/D")
            ls_sinRegistro = changeDBNull(row("sinRegistro"), "0")
            ls_vencido = changeDBNull(row("vencido"), "S/D")
            ls_ultimoPago = changeDBNull(row("ultimo_Pago"), "S/D")
            ls_balance = changeDBNull(row("balance"), "S/D")
            ls_diametro = changeDBNull(row("diametro_toma"), "S/D")
            ls_tipoUsuario = changeDBNull(row("tipo_usuario"), "S/D")
            ls_giro = changeDBNull(row("giro"), "S/D")
            ls_fechaUltimoPago = changeDBNull(row("fecha_ultimo_pago"), "S/D")

            If ls_fechaUltimoPago.Trim.Length = 8 Then
                ls_fechaUltimoPago = ls_fechaUltimoPago.Substring(6, 2) & "/" & ls_fechaUltimoPago.Substring(4, 2) & "/" & ls_fechaUltimoPago.Substring(0, 4)
            End If


            If ls_habitada.Equals("0") Then
                ls_habitada = "Esta habitada"
            Else
                ls_habitada = "No esta habitada"
            End If

            If (ls_lectura.Equals("0")) Then
                ls_lectura = "Sin Acción"
            End If

            If Not (ls_longitud.Equals("0.0") And ls_latitud.Equals("0.0")) Then
                ruta.Value = "http://maps.google.com/maps?q=" & ls_latitud & "+" & ls_longitud
                ver_mapa.Attributes.Clear()

                ver_mapa.Attributes.Add("onclick", "window.open('http://maps.google.com/maps?q=" & ls_latitud & "+" & ls_longitud & "'" & "); return false;")
            Else
                ver_mapa.Visible = False
            End If

            If Not IsDBNull(row("estadoAnterior")) Then
                If row("estadoAnterior").Equals("EO005") Then
                    ls_estadoDeLaOrden &= "(Se resolvio estando PAGADA)"
                ElseIf row("estadoAnterior").Equals("EO008") Then
                    ls_estadoDeLaOrden &= "(Se resolvio estando CANCELADA)"
                End If

            End If

            'La direccion será de la siguiente manera
            'Calle Num Ext Num Int, Nombre del Edificio
            'Entre Calles (o manzana)
            'Colonia
            'Municipio

            ls_descEstadoRev = getNombreCodigo(conn, ls_estadoDeLaRevision)
            Me.ViewState("ls_estadoDeLaRevision") = ls_estadoDeLaRevision
            Me.ViewState("idCliente") = idCliente
            Me.ViewState("idOrden") = idOrden
            Me.ViewState("idTarea") = ls_idTarea

            ls_nombre = row("cliente")
            ls_direccion = getCadenaAConcatenar(row("calle")) & " " & getCadenaAConcatenar(row("numExterior")) & " " & getCadenaAConcatenar(row("numInterior")) & _
                "<BR>" & _
                IIf(Not getCadenaAConcatenar(row("entreCalles")) = "", getCadenaAConcatenar(row("entreCalles")) & "<br>", "") & _
                getCadenaAConcatenar(row("colonia")) & _
                "<BR>" & getCadenaAConcatenar(row("municipio")) & _
             "<BR>" & " " & getCadenaAConcatenar(row("comoLlegar"))
            If crearListaOrdenes Then
                'ls_nombre = row("cliente")
                'ls_direccion = getCadenaAConcatenar(row("calle")) & " " & getCadenaAConcatenar(row("numExterior")) & " " & getCadenaAConcatenar(row("numInterior")) & _
                '    " " & getCadenaAConcatenar(row("comoLlegar")) & "<BR>" & _
                '    IIf(Not getCadenaAConcatenar(row("entreCalles")) = "", getCadenaAConcatenar(row("entreCalles")) & "<br>", "") & _
                '    getCadenaAConcatenar(row("colonia")) & _
                '    "<BR>" & getCadenaAConcatenar(row("municipio"))
                obtenerOrdenesDeLaPoliza()
            End If

            setImagenes(conn)

            manejaEstadodeBotones()

        Catch ex As Exception
            Throw ex
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try

        Me.DataBind()


    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not txtBuscar.Text.Trim = "" Then
            setDatos(rbTipoBusqueda.SelectedValue, txtBuscar.Text.Trim)
        End If

    End Sub

    Public Sub obtenerOrdenesDeLaPoliza()
        If idCliente = Nothing Then
            Return
        End If

        'llenamos la lista
        lbOrdenes.Items.Clear()

        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Dim sqlSelect As String = "select ord.idOrden, cast(if ( ord.fechaDeEjecucion='',ord.idOrden,  concat_ws('-', ord.idOrden, DATE_FORMAT(ord.fechaDeEjecucion, '%d/%m%/%Y')  ))as char(50)) descripcion " & _
                                "FROM ordenes ord, clientes cli " & _
                                "where ord.idCliente = cli.idCliente " & _
                                "and ord.idCliente=" & idCliente & " " & _
                                "order by ord.fechaDeEjecucion desc"

        Try
            Dim ds As DataSet = conectarMySql(conn, sqlSelect, "", True)
            If ds.Tables(0).Rows.Count = 0 Then

                Return
            End If
            DatosOrden.Visible = True

            lbOrdenes.DataSource = ds
            lbOrdenes.DataTextField = "descripcion"
            lbOrdenes.DataValueField = "idOrden"

            lbOrdenes.DataBind()

            'elegimos la poliza que se esta visualizando...
            For Each item As System.Web.UI.WebControls.ListItem In lbOrdenes.Items
                If item.Value = idOrden Then
                    lbOrdenes.SelectedIndex = lbOrdenes.Items.IndexOf(item)
                    Exit For
                End If
            Next

        Catch ex As Exception

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try


    End Sub

    Public Sub obtenerLaUltimaPoliza()

    End Sub

    Protected Sub lbOrdenes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbOrdenes.SelectedIndexChanged
        crearListaOrdenes = False
        setDatos("ord.idOrden", lbOrdenes.SelectedValue)
    End Sub


    Public Sub setImagenes(conn As MySqlConnection)

        Dim fechaFinal As String = ls_fechaDeInicio
        Dim selectSQL As String

        If fechaFinal.Length >= 10 Then
            fechaFinal = ls_fechaDeInicio.Substring(6, 4) & ls_fechaDeInicio.Substring(3, 2) & ls_fechaDeInicio.Substring(0, 2)
        Else
            fechaFinal = 0
        End If

        If ls_sinRegistro.Equals("1") Then
            selectSQL = "select * from fotos where idorden=0 and idpoliza=" & Me.ls_poliza & " and fecha =" & fechaFinal

        Else
            selectSQL = "select * from fotos where idorden=" & Me.ls_numOrden

        End If



        Dim ds As DataSet = conectarMySql(conn, selectSQL, "fotos", False)
        Dim servidorFotos As String = Server.MapPath("~/")
        Dim images As New List(Of String)

        For Each row As DataRow In ds.Tables(0).Rows
            images.Add(String.Format("~/{0}/{1}", row.Item("path"), row.Item("name")))
        Next


        RepeaterImages.DataSource = images
        RepeaterImages.DataBind()

        If images.Count = 0 Then
            setImagenesOld(conn)
        End If


    End Sub

    Public Sub setImagenesOld(conn As MySqlConnection)
        Dim ls_numOrden As String
        Dim ls_poliza As String
        Dim nombreGenerico As String
        Dim extension As String = ".jpg"
        If ls_sinRegistro.Equals("1") Then
            ls_numOrden = "0".PadLeft(obtieneLongitud(conn, "numOrden"), "0")
        Else
            ls_numOrden = Me.ls_numOrden.PadLeft(obtieneLongitud(conn, "numOrden"), "0")
        End If

        ls_poliza = Me.ls_poliza.PadLeft(obtieneLongitud(conn, "poliza"), "0")
        ls_numOrdenReal = Me.ls_numOrdenReal.PadLeft(obtieneLongitud(conn, "numOrden"), "0")

        nombreGenerico = ls_numOrden & "_" & ls_poliza

        'If System.IO.File.Exists(Server.MapPath("~/") & "fotos\" & nombreGenerico & "_" & "1" & extension) Then
        '    iAntes.ImageUrl = "fotos/" & nombreGenerico & "_" & "1" & extension

        'Else
        '    iAntes.ImageUrl = "botones/medFace.png"

        'End If



        'If System.IO.File.Exists(Server.MapPath("~/") & "fotos\" & nombreGenerico & "_" & "2" & extension) Then
        '    iDespues.ImageUrl = "fotos/" & nombreGenerico & "_" & "2" & extension
        'Else
        '    iDespues.ImageUrl = "botones/medFace.png"

        'End If
        Dim fechaFinal As String = ls_fechaDeEjecucion

        If fechaFinal.Length >= 10 Then
            fechaFinal = ls_fechaDeEjecucion.Substring(6, 4) & ls_fechaDeEjecucion.Substring(3, 2) & ls_fechaDeEjecucion.Substring(0, 2)
        Else
            fechaFinal = "S"
        End If

        If ls_sinRegistro.Equals("1") Then
            nombreGenerico += fechaFinal
        End If

        Dim selectSQL As String = "select valorINT from parametros where nombre='fechaDelHistorico'"


        Dim ds As DataSet = conectarMySql(conn, selectSQL, "parametros", False)
        Dim servidorFotos As String = Server.MapPath("~/") & "fotos\"
        'If fechaFinal.Length = 8 Then
        '    If Convert.ToInt64(fechaFinal) <= ds.Tables(0).Rows(0).Item("valorINT") Then
        '        selectSQL = "select valorSTRING from parametros where nombre='sitioFotosHistoricas'"
        '        ds = conectarMySql(conn, selectSQL, "parametros", False)

        '        servidorFotos = ds.Tables(0).Rows(0).Item("valorSTRING")
        '    End If
        'End If



        mostrarImagenes(servidorFotos, nombreGenerico, ls_numOrdenReal & "_" & ls_poliza & fechaFinal)



    End Sub

    Protected Sub bCancelada_Click(sender As Object, e As EventArgs) Handles bCancelada.Click




        Dim conn As MySqlConnection
        Dim cmd As New MySqlCommand()

        Try
            conn = getConnection()
            If (estaEnviada(conn)) Then
                getCadenaAEscribir2(idOrden.ToString, False, conn)
            ElseIf ls_estadoDeLaOrdenCod = "EO001" Then
                Dim ls_update As String = "update ordenes set  enviada='EE001' where idOrden=" & idOrden

                cmd.CommandText = ls_update
                cmd.Connection = conn
                cmd.ExecuteNonQuery()


            End If
            cambiaEstadoDeLaOrden("EO008")
            setDatos()

        Catch ex As Exception
            message_generico("Ocurrio un error:" & ex.Message)
            'Throw New Exception("Ocurrio un error:" & ex.Message)
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try

    End Sub

    Protected Sub cambiaEstadoDeLaOrden(estado As String)
        Dim conn As MySqlConnection
        Dim cmd As New MySqlCommand()

        Try
            conn = getConnection()
            cmd.Connection = conn

            Dim selectSQL As String = "select estadoDeLaOrden , idTarea from ordenes where idOrden=" & idOrden

            Dim ds As DataSet = conectarMySql(conn, selectSQL, "ordenes", False)

            cmd.CommandText = "update ordenes set estadodeLaorden='" & estado & "' ,  fechadeejecucion=date_format('" & getFecha("yyyyMMddHHmmss") & "', '%Y%m%d%H%i%s') where idOrden=" & idOrden
            'ls_estadoDeLaOrdenCod = estado

            cmd.ExecuteNonQuery()

            'simuloTrigger(idOrden, ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), conn)
            paginaGenerica.simuloTrigger(idOrden, ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), estado, ds.Tables(0).Rows(0).Item("idTarea"), conn)


        Catch ex As Exception
        Finally

            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
    End Sub

    Protected Sub bPagada_Click(sender As Object, e As EventArgs) Handles bPagada.Click



        Dim conn As MySqlConnection

        Dim cmd As New MySqlCommand()

        Try
            conn = getConnection()
            If (estaEnviada(conn)) Then
                getCadenaAEscribir(idOrden.ToString, "B", conn)
            ElseIf ls_estadoDeLaOrdenCod = "EO001" Then
                Dim ls_update As String = "update ordenes set  enviada='EE001' where idOrden=" & idOrden

                cmd.CommandText = ls_update
                cmd.Connection = conn
                cmd.ExecuteNonQuery()


            End If
            cambiaEstadoDeLaOrden("EO005")
            setDatos()

        Catch ex As Exception
            'Throw New Exception("Ocurrio un error:" & ex.Message)
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try

    End Sub


    Private Sub manejaEstadodeBotones()
        'El boton de cancelar y pagado debe estar habilitado solo para las ordenes "Sin comenzar" y "Sin Asignar"
        If ls_estadoDeLaOrdenCod = "EO000" Or ls_estadoDeLaOrdenCod = "EO001" Then
            bCancelada.Enabled = True
            bPagada.Enabled = True
        Else
            bCancelada.Enabled = False
            bPagada.Enabled = False
        End If

        If ls_idTarea = "" Then
            bIrATarea.Enabled = False
        Else
            bIrATarea.Enabled = True
        End If

        'If ls_estadoDeLaOrdenCod = "EO000" Or ls_estadoDeLaOrdenCod = "EO001" Or ls_estadoDeLaOrdenCod = "EO010" Then
        '    ddl_rev.Enabled = False
        'Else
        '    ddl_rev.Enabled = True
        'End If

        'If ls_estadoDeLaRevision = "ER001" Then
        '    bIncorrecta.Enabled = False
        '    'bCorrecta.Enabled = True
        'End If

        'If ls_estadoDeLaRevision = "ER002" Then
        '    bCorrecta.Enabled = False
        '    bIncorrecta.Enabled = True
        'End If

        If ls_estadoDeLaOrdenCod = "EO004" Then
            motivoVisible = False
        Else
            motivoVisible = True
        End If

        If rol.Equals("RU002") Or rol.Equals("RU005") Then
            bPagada.Enabled = False
            bCorrecta.Enabled = False
            bIncorrecta.Enabled = False
            b_guardaLectura.Visible = False
            tb_lectura.Enabled = False

        End If

        Me.ViewState("motivoVisible") = motivoVisible
    End Sub


    'verifica si esta enviada
    Private Function estaEnviada(conn As MySqlConnection) As Boolean

        Dim sqlSelect As String



        sqlSelect = "Select enviada, estadodelaorden from ordenes where idOrden=" & idOrden

        Dim ds As DataSet = conectarMySql(conn, sqlSelect, "ordenes", False)

        If (ds.Tables(0).Rows.Count = 0) Then
            Throw New Exception("No existe la orden")
        End If

        estaEnviada = ds.Tables(0).Rows(0).Item("enviada") = "EE001" And ds.Tables(0).Rows(0).Item("estadodelaorden") = "EO001"


    End Function


    Protected Sub bIrATarea_Click(sender As Object, e As EventArgs) Handles bIrATarea.Click
        Response.Redirect("verOrdenesPorEmpleado.aspx?option=" & verTarea & "&tarea=" & ls_idTarea)
    End Sub

    Protected Sub cambiaEstadoRevision(estado As String)

        Dim conn As MySqlConnection
        Dim cmd As New MySqlCommand()

        Try
            conn = getConnection()
            cmd.Connection = conn

            'Dim selectSQL As String = "select estadoDeLaOrden , idTarea from ordenes where idOrden=" & idOrden

            'Dim ds As DataSet = conectarMySql(conn, selectSQL, "ordenes", False)

            cmd.CommandText = "update ordenes set estadodeLaRevision='" & estado & "' where idOrden=" & idOrden
            'ls_estadoDeLaOrdenCod = estado

            cmd.ExecuteNonQuery()

            'simuloTrigger(idOrden, ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), conn)
            'paginaGenerica.simuloTrigger(idOrden, ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), estado, ds.Tables(0).Rows(0).Item("idTarea"), conn)


        Catch ex As Exception

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
        ls_estadoDeLaRevision = estado
    End Sub


    Protected Sub bCorrecta_Click(sender As Object, e As EventArgs) Handles bCorrecta.Click
        cambiaEstadoRevision("ER001")
        manejaEstadodeBotones()
        If Not IsNothing(dsLista) Then
            siguiente()
            manejaBotonesDireccion()
        End If
    End Sub

    Protected Sub bIncorrecta_Click(sender As Object, e As EventArgs) Handles bIncorrecta.Click
        cambiaEstadoRevision("ER002")
        If Not IsNothing(dsLista) Then
            siguiente()
            manejaBotonesDireccion()
        End If
    End Sub

    Sub obtieneDSListado()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Dim lugar As Integer = 0
        Try
            dsLista = conectarMySql(conn, Request.QueryString("q"), "ordenes", True)

            'Buscamos en un apuntador
            Me.ViewState("q") = Request.QueryString("q")
            For Each row As DataRow In dsLista.Tables(0).Rows
                If Convert.ToInt32(row("numorden")) = Convert.ToInt32(Request.QueryString("busqueda")) Then
                    lugarEnLista = lugar
                    Exit For
                End If
                lugar += 1
            Next


            Me.ViewState("lugarEnLista") = lugarEnLista
            Me.ViewState("dsLista") = dsLista
        Catch ex As Exception

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
    End Sub

    Sub anterior()
        Dim lugar As Integer

        lugar = lugarEnLista

        lugar -= 1

        If lugar >= 0 Then

            setDatos("ord.idOrden", dsLista.Tables(0).Rows(lugar).Item("idOrden"))
            lugarEnLista = lugar
            Me.ViewState("lugarEnLista") = lugarEnLista
        End If

        'If (lugar - 1) < 0 Then
        '    bAtras.Enabled = False
        '    bSiguiente.Enabled = True
        'Else
        '    bAtras.Enabled = True
        '    bSiguiente.Enabled = False
        'End If

    End Sub

    Sub siguiente()
        Dim lugar As Integer

        lugar = lugarEnLista

        lugar += 1

        If lugar < dsLista.Tables(0).Rows.Count Then

            setDatos("ord.idOrden", dsLista.Tables(0).Rows(lugar).Item("idOrden"))
            lugarEnLista = lugar
            Me.ViewState("lugarEnLista") = lugarEnLista
        End If

        'If (lugar + 1) > dsLista.Tables(0).Rows.Count Then
        '    bAtras.Enabled = True
        '    bSiguiente.Enabled = False

        'Else
        '    bAtras.Enabled = False
        '    bSiguiente.Enabled = True
        'End If
    End Sub

    Protected Sub bSiguiente_Click(sender As Object, e As EventArgs) Handles bSiguiente.Click
        siguiente()
        manejaBotonesDireccion()
    End Sub


    Protected Sub bAtras_Click(sender As Object, e As EventArgs) Handles bAtras.Click
        anterior()
        manejaBotonesDireccion()
    End Sub

    Sub manejaBotonesDireccion()
        If Not IsNothing(dsLista) Then
            bAtras.Enabled = True
            bSiguiente.Enabled = True
            bIrTabla.Visible = True
            If (lugarEnLista + 1) > dsLista.Tables(0).Rows.Count Then
                'bAtras.Enabled = True
                bSiguiente.Enabled = False

            End If
            If (lugarEnLista - 1) < 0 Then
                bAtras.Enabled = False
                'bSiguiente.Enabled = True
            End If
        Else
            bAtras.Visible = False
            bSiguiente.Visible = False
            bIrTabla.Visible = False
        End If

    End Sub

    Protected Sub bIrTabla_Click(sender As Object, e As EventArgs) Handles bIrTabla.Click
        Dim queryString As String = "verOrdenesPorEmpleado.aspx?option=" & ver_tabla & _
        "&rbTipoDeBusqueda=" & Request.QueryString("tipo") & _
        "&dia=" & Request.QueryString("dia") & _
        "&dia2=" & Request.QueryString("dia2") & _
        "&ck_filtrarRealizadas=" & Request.QueryString("ck_filtrarRealizadas") & _
        "&ck_PorUsuario=" & Request.QueryString("ck_PorUsuario") & _
        "&ck_filtrarSinVerificar=" & Request.QueryString("ck_filtrarSinVerificar") & _
        "&ck_filtrarCorrectas=" & Request.QueryString("ck_filtrarCorrectas") & _
        "&ck_filtrarIncorrectas=" & Request.QueryString("ck_filtrarIncorrectas") & _
        "&cb_desconexiones=" & Request.QueryString("cb_desconexiones") & _
        "&cb_reconexiones=" & Request.QueryString("cb_reconexiones") & _
        "&cb_remociones=" & Request.QueryString("cb_remociones") & _
        "&cb_recremos=" & Request.QueryString("cb_recremos") & _
        "&ddl_estadosOrden=" & Request.QueryString("ddl_estadosOrden") & _
        "&id=" & Request.QueryString("id") & _
        "&rbTipoDeBusqueda=" & Request.QueryString("rbTipoDeBusqueda") & _
        "&ddl_motivos=" & Request.QueryString("ddl_motivos")

        Response.Redirect(queryString)
    End Sub

    Function getNombreCodigo(conn As MySqlConnection, codigo As String) As String
        Dim sqlSelect As String = "Select nombre from codigos where codigo='" & codigo & "'"
        Dim ds As DataSet = conectarMySql(conn, sqlSelect, "codigos", False)

        If ds.Tables(0).Rows.Count > 0 Then
            getNombreCodigo = ds.Tables(0).Rows(0).Item("nombre")
        Else
            getNombreCodigo = codigo
        End If
    End Function

    Function parseCodigo(conn As MySqlConnection, desc As String) As String



        Dim selectSQL As String = "Select nombre from codigos where codigo='" & desc.ToUpper & "'"
        Dim ds As DataSet
        parseCodigo = "     "



        Try
            ds = conectarMySql(conn, selectSQL, "codigos", False)
            If ds.Tables(0).Rows.Count > 0 Then
                parseCodigo = ds.Tables(0).Rows(0).Item("nombre")
            End If

        Catch ex As Exception

        End Try



    End Function

    Public Sub mostrarImagenes(path As String, nombreGenerico As String, nombreGenerico2 As String)


        Dim images As New List(Of String)
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(
    path,
    Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, nombreGenerico & "*.jpg")
            Dim directorio = System.IO.Path.GetDirectoryName(foundFile)
            directorio = directorio.Substring(directorio.LastIndexOf("\") + 1)
            Dim nombrefinal = System.IO.Path.GetFileName(foundFile)
            If Not directorio.Equals("fotos") Then
                nombrefinal = directorio & "/" & nombrefinal
            End If
            images.Add(String.Format("~/fotos/{0}", nombrefinal))
        Next

        If images.Count = 0 Then
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(
              path,
              Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, nombreGenerico2 & "*.jpg")

                images.Add(String.Format("~/fotos/{0}", System.IO.Path.GetFileName(foundFile)))
            Next
        End If

        RepeaterImages.DataSource = images
        RepeaterImages.DataBind()

        If images.Count = 0 Then
            iAntes.ImageUrl = "botones/medFace.png"
            iAntes.Visible = True

        Else
            iAntes.Visible = False
        End If
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(Control As Control)

        '/* Confirms that an HtmlForm control is rendered for the specified ASP.NET
        ' server control at run time. */
    End Sub


    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Response.ContentType = "application/pdf"
        'Response.AddHeader("content-disposition", "attachment;filename=Panel.pdf")
        Response.AddHeader("Content-Disposition", "inline; filename=orden.pdf")
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        DatosOrden.RenderControl(hw)
        Dim sr As New StringReader(sw.ToString())
        Dim pdfDoc As New Document(PageSize.A4, 10.0F, 10.0F, 100.0F, 0.0F)
        Dim htmlparser As New HTMLWorker(pdfDoc)
        PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
        pdfDoc.Open()
        htmlparser.Parse(sr)
        For Each items As RepeaterItem In RepeaterImages.Items
            Dim imagen As System.Web.UI.WebControls.Image = CType(items.FindControl("image"), System.Web.UI.WebControls.Image)
            Dim foto As Image = Image.GetInstance(Server.MapPath("~/") & imagen.ImageUrl.Substring(2))
            foto.ScaleToFit(150.0F, 200.0F)
            foto.Border = Rectangle.BOX

            pdfDoc.Add(foto)
        Next
        pdfDoc.Close()
        Response.Write(pdfDoc)
        Response.End()



    End Sub

    Protected Sub b_guardaLectura_Click(sender As Object, e As EventArgs) Handles b_guardaLectura.Click
        If tb_lectura.Text.ToString.Length > 8 Then
            message_generico("La lectura no puede ser mayor a 8 caracteres")

            Return
        End If

        Dim conn As MySqlConnection
        Dim cmd As New MySqlCommand()

        Try
            conn = getConnection()
                Dim ls_update As String = "update ordenes set  lecturareal='" & tb_lectura.Text.ToString & "' where idOrden=" & idOrden

                cmd.CommandText = ls_update
                cmd.Connection = conn
                cmd.ExecuteNonQuery()
            message_generico("Se ha guardado la lectura")

        Catch ex As Exception
            message_generico("Ocurrio un error:" & ex.Message)
            'Throw New Exception("Ocurrio un error:" & ex.Message)
        Finally
            If Not IsNothing(conn) Then
                conn.Close()
            End If


        End Try


    End Sub
End Class
