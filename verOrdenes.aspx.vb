Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.IO


Partial Class _Default
    Inherits paginaGenerica



    Dim idlectura As Integer
    Dim crearListaOrdenes As Boolean = True
    Dim busqueda As String
    Dim dsLista As DataSet
    Dim lugarEnLista As Integer

    Dim tipoDeBusqueda As String = " cli.clave_usuario "


    Public motivoVisible As Boolean = True

    Public ls_poliza, ls_motivo, ls_fechaDeEjecucion, _
       ls_lectura, ls_medidor, ls_serie, ls_comentarios, ls_tecnico, ls_direccion, ls_nombre, ls_estadoDeLaLectura, ls_idLectura As String
    Public ls_longitud, ls_latitud, ls_estadoDeLaOrden, ls_descEstadoRev, ls_estadoDeLaRevision, ls_economico, ls_idTarea, ls_claveUsuario, ls_idCliente As String

    Protected Sub message_generico(mensaje As String)
        lbldialogo.Text = mensaje
        mp1.Show()
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Not isLoged() Then
        '    Return
        'End If

        Dim ga As getArchivos = New getArchivos(Server.MapPath("~/") & "lecturaEntrada\activos\")



        If Not IsPostBack() Then
            If Not Request.QueryString("busqueda") = "" Then
                tipoDeBusqueda = " lec.idlectura "
                setDatos(Request.QueryString("busqueda"))

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

        If Not Me.ViewState("idlecturas") = Nothing Then
            idlectura = CType(Me.ViewState("idlecturas"), Integer)
            busqueda = CType(Me.ViewState("busqueda"), String)
            motivoVisible = CType(Me.ViewState("motivoVisible"), Boolean)
            ls_idTarea = CType(Me.ViewState("idTarea"), String)

        Else
            idlectura = Nothing


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
        If busqueda = "" Then
            Return

        End If

        setDatos(busqueda)

        b_imprimir.Attributes.Clear()
        b_imprimir.Attributes.Add("onclick", returnPrintParams("contenido"))
    End Sub


    Public Sub setDatos(busqueda As Integer)
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Me.busqueda = busqueda
        Me.ViewState("busqueda") = busqueda
        Dim sqlSelect As String = "Select cli.idcliente, cli.clave_usuario, tar.idTarea idtarea, lec.poliza eco,  lec.idLectura, lec.tipoLectura, lec.fecha, lec.latitud, lec.longitud, lec.idlectura idlectura,  cli.poliza poliza,cli.direccion,  lec.fechaderecepcion fechaderecepcion, " & _
                        "lec.lectura lectura, lec.serieMedidor serieMedidor, lec.comentarios comentarios, concat_ws(' ', emp.Nombre, emp.ApPat, emp.ApMat) lecturista,cli.nombre, mo.nombre anomalia from " & _
                        "lecturas lec left outer join tareaslect tar on (lec.idTarea=tar.idTarea) left outer join empleadoslect emp on (tar.idEmpleado=emp.idEmpleado) " & _
                        "left outer join codigos mo on (mo.codigo=lec.anomalia), clienteslect cli where cli.idCliente= lec.idCliente and " & tipoDeBusqueda & " =" & busqueda & " order by tar.fechaDeAsignacion desc"
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

            ls_motivo = changeDBNull(row("anomalia"), "Sin Anomalia")
            ls_fechaDeEjecucion = changeDBNull(row("fecha"), "S/D")
            ls_lectura = changeDBNull(row("lectura"), "0")
            ls_medidor = changeDBNull(row("serieMedidor"), "S/D")
            ls_comentarios = changeDBNull(row("comentarios"), "Sin Comentarios")
            ls_tecnico = changeDBNull(row("lecturista"), "Sin Asignar")
            ls_poliza = row("poliza")
            idlectura = row("idlectura")
            ls_nombre = row("nombre")
            ls_direccion = row("direccion")
            ls_idLectura = row("idLectura")
            ls_economico = row("eco")
            ls_idTarea = row("idtarea")
            ls_claveUsuario = row("clave_usuario")
            ls_idCliente = row("idcliente")

            Select Case row("tipoLectura").ToString
                Case "0"
                    ls_estadoDeLaLectura = "Con Lectura"
                Case "4"
                    ls_estadoDeLaLectura = "Sin Lectura"
                Case Else
                    ls_estadoDeLaLectura = "Pendiente"
            End Select


            ls_latitud = changeDBNull(row("latitud"), "0.0")
            ls_longitud = changeDBNull(row("longitud"), "0.0")

            If Not (ls_longitud.Equals("0.0") And ls_latitud.Equals("0.0")) Then
                ruta.Value = "http://maps.google.com/maps?q=" & ls_latitud & "+" & ls_longitud
                ver_mapa.Attributes.Clear()

                ver_mapa.Attributes.Add("onclick", "window.open('http://maps.google.com/maps?q=" & ls_latitud & "+" & ls_longitud & "'" & "); return false;")
            Else
                ver_mapa.Visible = False
            End If

            'If Not IsDBNull(row("estadoAnterior")) Then
            '    If row("estadoAnterior").Equals("EO005") Then
            '        ls_estadoDeLaOrden &= "(Se resolvio estando PAGADA)"
            '    ElseIf row("estadoAnterior").Equals("EO008") Then
            '        ls_estadoDeLaOrden &= "(Se resolvio estando CANCELADA)"
            '    End If

            'End If

            'La direccion será de la siguiente manera
            'Calle Num Ext Num Int, Nombre del Edificio
            'Entre Calles (o manzana)
            'Colonia
            'Municipio

            ' ls_descEstadoRev = getNombreCodigo(conn, ls_estadoDeLaRevision)
            'Me.ViewState("ls_estadoDeLaRevision") = ls_estadoDeLaRevision
            Me.ViewState("idlecturas") = idlectura

            Me.ViewState("idTarea") = ls_idTarea

            If crearListaOrdenes Then
                ls_nombre = row("nombre")

                obtenerOrdenesDeLaPoliza()
            End If

            setImagenes(conn)

            'manejaEstadodeBotones()

        Catch ex As Exception
            Throw ex
        Finally
            conn.Close()

        End Try

        Me.DataBind()


    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not txtBuscar.Text.Trim = "" Then
            setDatos(txtBuscar.Text.Trim)
        End If

    End Sub

    Public Sub obtenerOrdenesDeLaPoliza()

        'llenamos la lista
        lbOrdenes.Items.Clear()

        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Dim sqlSelect As String = "select lec.idlectura, cast(if ( lec.fecha='',lec.idlectura,  concat_ws('-', lec.idlectura, DATE_FORMAT(lec.fecha, '%d/%m%/%Y')  ))as char(50)) fecha " & _
                                "FROM lecturas lec where lec.idcliente =  " & ls_idCliente & " order by lec.fecha desc"

        Try
            Dim ds As DataSet = conectarMySql(conn, sqlSelect, "", True)
            If ds.Tables(0).Rows.Count = 0 Then

                Return
            End If
            DatosOrden.Visible = True

            lbOrdenes.DataSource = ds
            lbOrdenes.DataTextField = "fecha"
            lbOrdenes.DataValueField = "idlectura"

            lbOrdenes.DataBind()

            'elegimos la poliza que se esta visualizando...
            For Each item As ListItem In lbOrdenes.Items
                If item.Value = idlectura Then
                    lbOrdenes.SelectedIndex = lbOrdenes.Items.IndexOf(item)
                    Exit For
                End If
            Next

        Catch ex As Exception

        Finally
            conn.Close()

        End Try


    End Sub

    Public Sub obtenerLaUltimaPoliza()

    End Sub

    Protected Sub lbOrdenes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbOrdenes.SelectedIndexChanged
        crearListaOrdenes = False
        tipoDeBusqueda = " lec.idlectura "
        setDatos(lbOrdenes.SelectedValue)
    End Sub

    Public Sub setImagenes(conn As MySqlConnection)
        Dim ls_poliza As String

        Dim nombreGenerico As String
        Dim extension As String = ".jpg"
        ls_poliza = Me.ls_poliza.PadLeft(obtieneLongitud(conn, "poliza"), "0")
        Dim serieMedidor = Me.ls_medidor.PadLeft(obtieneLongitud(conn, "serieMedidor"), "0")


        nombreGenerico = ls_poliza

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

        mostrarImagenes(Server.MapPath("~/") & "fotos\", nombreGenerico, ls_medidor & "-" & fechaFinal, ls_poliza & "-*" & ls_medidor)



    End Sub

    'Protected Sub bCancelada_Click(sender As Object, e As EventArgs) Handles bCancelada.Click




    '    Dim conn As MySqlConnection
    '    Dim cmd As New MySqlCommand()

    '    Try
    '        conn = getConnection()
    '        If (estaEnviada(conn)) Then
    '            getCadenaAEscribir2(idlectura.ToString, False, conn)
    '        ElseIf ls_estadoDeLaOrdenCod = "EO001" Then
    '            Dim ls_update As String = "update ordenes set  enviada='EE001' where idOrden=" & idOrden

    '            cmd.CommandText = ls_update
    '            cmd.Connection = conn
    '            cmd.ExecuteNonQuery()


    '        End If
    '        cambiaEstadoDeLaOrden("EO008")
    '        setDatos()

    '    Catch ex As Exception
    '        message_generico("Ocurrio un error:" & ex.Message)
    '        'Throw New Exception("Ocurrio un error:" & ex.Message)
    '    Finally
    '        conn.Close()

    '    End Try

    'End Sub

    'Protected Sub cambiaEstadoDeLaOrden(estado As String)
    '    Dim conn As MySqlConnection
    '    Dim cmd As New MySqlCommand()

    '    Try
    '        conn = getConnection()
    '        cmd.Connection = conn

    '        Dim selectSQL As String = "select estadoDeLaOrden , idTarea from ordenes where idOrden=" & idOrden

    '        Dim ds As DataSet = conectarMySql(conn, selectSQL, "ordenes", False)

    '        cmd.CommandText = "update ordenes set estadodeLaorden='" & estado & "' ,  fechadeejecucion=date_format('" & getFecha("yyyyMMddHHmmss") & "', '%Y%m%d%H%i%s') where idOrden=" & idOrden
    '        'ls_estadoDeLaOrdenCod = estado

    '        cmd.ExecuteNonQuery()

    '        'simuloTrigger(idOrden, ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), conn)
    '        paginaGenerica.simuloTrigger(idOrden, ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), estado, ds.Tables(0).Rows(0).Item("idTarea"), conn)


    '    Catch ex As Exception
    '        If Not conn Is Nothing Then
    '            conn.Close()
    '        End If
    '    End Try
    'End Sub

    'Protected Sub bPagada_Click(sender As Object, e As EventArgs) Handles bPagada.Click



    '    Dim conn As MySqlConnection

    '    Dim cmd As New MySqlCommand()

    '    Try
    '        conn = getConnection()
    '        If (estaEnviada(conn)) Then
    '            getCadenaAEscribir(idOrden.ToString, "B", conn)
    '        ElseIf ls_estadoDeLaOrdenCod = "EO001" Then
    '            Dim ls_update As String = "update ordenes set  enviada='EE001' where idOrden=" & idOrden

    '            cmd.CommandText = ls_update
    '            cmd.Connection = conn
    '            cmd.ExecuteNonQuery()


    '        End If
    '        cambiaEstadoDeLaOrden("EO005")
    '        setDatos()

    '    Catch ex As Exception
    '        'Throw New Exception("Ocurrio un error:" & ex.Message)
    '    Finally
    '        conn.Close()

    '    End Try

    'End Sub


    'Private Sub manejaEstadodeBotones()
    '    'El boton de cancelar y pagado debe estar habilitado solo para las ordenes "Sin comenzar" y "Sin Asignar"
    '    If ls_estadoDeLaOrdenCod = "EO000" Or ls_estadoDeLaOrdenCod = "EO001" Then
    '        bCancelada.Enabled = True
    '        bPagada.Enabled = True
    '    Else
    '        bCancelada.Enabled = False
    '        bPagada.Enabled = False
    '    End If

    '    If ls_idTarea = "" Then
    '        bIrATarea.Enabled = False
    '    Else
    '        bIrATarea.Enabled = True
    '    End If

    '    'If ls_estadoDeLaOrdenCod = "EO000" Or ls_estadoDeLaOrdenCod = "EO001" Or ls_estadoDeLaOrdenCod = "EO010" Then
    '    '    ddl_rev.Enabled = False
    '    'Else
    '    '    ddl_rev.Enabled = True
    '    'End If

    '    'If ls_estadoDeLaRevision = "ER001" Then
    '    '    bIncorrecta.Enabled = False
    '    '    'bCorrecta.Enabled = True
    '    'End If

    '    'If ls_estadoDeLaRevision = "ER002" Then
    '    '    bCorrecta.Enabled = False
    '    '    bIncorrecta.Enabled = True
    '    'End If

    '    If ls_estadoDeLaOrdenCod = "EO004" Then
    '        motivoVisible = False
    '    Else
    '        motivoVisible = True
    '    End If
    '    Me.ViewState("motivoVisible") = motivoVisible
    'End Sub


    'verifica si esta enviada
    'Private Function estaEnviada(conn As MySqlConnection) As Boolean

    '    Dim sqlSelect As String



    '    sqlSelect = "Select enviada, estadodelaorden from ordenes where idOrden=" & idOrden

    '    Dim ds As DataSet = conectarMySql(conn, sqlSelect, "ordenes", False)

    '    If (ds.Tables(0).Rows.Count = 0) Then
    '        Throw New Exception("No existe la orden")
    '    End If

    '    estaEnviada = ds.Tables(0).Rows(0).Item("enviada") = "EE001" And ds.Tables(0).Rows(0).Item("estadodelaorden") = "EO001"


    'End Function


    'Protected Sub bIrATarea_Click(sender As Object, e As EventArgs) Handles bIrATarea.Click
    '    Response.Redirect("verOrdenesPorEmpleado.aspx?option=" & verTarea & "&tarea=" & ls_idTarea)
    'End Sub

    'Protected Sub cambiaEstadoRevision(estado As String)

    '    Dim conn As MySqlConnection
    '    Dim cmd As New MySqlCommand()

    '    Try
    '        conn = getConnection()
    '        cmd.Connection = conn

    '        'Dim selectSQL As String = "select estadoDeLaOrden , idTarea from ordenes where idOrden=" & idOrden

    '        'Dim ds As DataSet = conectarMySql(conn, selectSQL, "ordenes", False)

    '        cmd.CommandText = "update ordenes set estadodeLaRevision='" & estado & "' where idOrden=" & idOrden
    '        'ls_estadoDeLaOrdenCod = estado

    '        cmd.ExecuteNonQuery()

    '        'simuloTrigger(idOrden, ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), conn)
    '        'paginaGenerica.simuloTrigger(idOrden, ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), estado, ds.Tables(0).Rows(0).Item("idTarea"), conn)


    '    Catch ex As Exception
    '        If Not conn Is Nothing Then
    '            conn.Close()
    '        End If
    '    End Try
    '    ls_estadoDeLaRevision = estado
    'End Sub


    'Protected Sub bCorrecta_Click(sender As Object, e As EventArgs) Handles bCorrecta.Click
    '    cambiaEstadoRevision("ER001")
    '    manejaEstadodeBotones()
    '    If Not IsNothing(dsLista) Then
    '        siguiente()
    '        manejaBotonesDireccion()
    '    End If
    'End Sub

    'Protected Sub bIncorrecta_Click(sender As Object, e As EventArgs) Handles bIncorrecta.Click
    '    cambiaEstadoRevision("ER002")
    '    If Not IsNothing(dsLista) Then
    '        siguiente()
    '        manejaBotonesDireccion()
    '    End If
    'End Sub

    Sub obtieneDSListado()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Dim lugar As Integer = 0
        Try
            dsLista = conectarMySql(conn, Request.QueryString("q"), "lecturas", True)

            'Buscamos en un apuntador
            Me.ViewState("q") = Request.QueryString("q")
            For Each row As DataRow In dsLista.Tables(0).Rows
                If Convert.ToInt32(row("poliza")) = Convert.ToInt32(Request.QueryString("busqueda")) Then
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
            tipoDeBusqueda = " lec.idlectura "

            setDatos(dsLista.Tables(0).Rows(lugar).Item("idlectura"))
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
            tipoDeBusqueda = " lec.idlectura "

            setDatos(dsLista.Tables(0).Rows(lugar).Item("idlectura"))
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

    Public Sub mostrarImagenes(path As String, nombreGenerico As String, nombreGenerico2 As String, nombreGenerico3 As String)


        Dim images As New List(Of String)
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(
    path,
    Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, nombreGenerico & "*.jpg")

            images.Add(String.Format("~/fotos/{0}", System.IO.Path.GetFileName(foundFile)))
        Next

        If images.Count = 0 Then
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(
        path,
        Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*" & nombreGenerico2 & "*.jpg")

                images.Add(String.Format("~/fotos/{0}", System.IO.Path.GetFileName(foundFile)))
            Next
        End If

        If images.Count = 0 Then
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(
        path,
        Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, nombreGenerico3 & "*.jpg")

                images.Add(String.Format("~/fotos/{0}", System.IO.Path.GetFileName(foundFile)))
            Next
        End If


        RepeaterImages.DataSource = images
        RepeaterImages.DataBind()

        If images.Count = 0 Then
            iAntes.ImageUrl = "botones/medFace.png"
            iAntes.Visible = True

        End If


    End Sub

    Protected Sub bIrATarea_Click(sender As Object, e As EventArgs) Handles bIrATarea.Click
        Response.Redirect("verOrdenesPorEmpleado.aspx?option=" & verTarea & "&tarea=" & ls_idTarea)
    End Sub
End Class
