Imports MySql.Data.MySqlClient
Imports System.Data
Imports Ionic.Zip

Partial Class verOrdenesPorCalle
    Inherits paginaGenerica

    Dim idEmpleado As Integer = 0
    Dim ls_filtro As String = ""
    Dim selectSQL As String = ""


    Protected Sub message_generico(mensaje As String)
        lbldialogo.Text = mensaje
        mp1.Show()
    End Sub




    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Not isLoged() Then
        '    Return
        'End If
        Dim ga As getArchivos = New getArchivos(Server.MapPath("~/") & "in\ordenes\")

        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)


       
        Try
            selectSQL = CType(Me.ViewState("selectSQL"), String)
        Catch ex As Exception

        End Try

        If Not IsPostBack Then

            setMenu(contenedor)
            'Hay que buscar si no hay una cookie primero

            'Dim cookie As HttpCookie = Request.Cookies("verOrdenesPorEmpleado")

            'If cookie Is Nothing Then
            '    'cookie = New HttpCookie("verOrdenesPorEmpleado")
            '    dia.Text = getFecha("yyyyMMdd")
            '    dia2.Text = getFecha("yyyyMMdd")
            'Else
            '    dia.Text = cookie("dia")
            '    dia2.Text = cookie("dia2")
            'End If
            dia.Text = getFecha("yyyyMMdd")
            dia2.Text = getFecha("yyyyMMdd")



            Try
                'Dim selectSQL As String = "Select  if(isnull(alias) or LENGTH(alias)=0, CONCAT_WS(' ',nombre,  appat, apmat), alias) nombreComp, emp.idEmpleado id, Remociones, Desconexiones, Reconexiones, " & _
                '    "recremos " & _
                '    "from empleados emp " & _
                '    " where estado='EG001' order by if(isnull(alias) or LENGTH(alias)=0, CONCAT_WS(' ',nombre,  appat, apmat), alias) "

                'Dim selectSQL As String = "Select if(isnull(alias) or LENGTH(alias)=0, CONCAT_WS(' ',nombre,  appat, apmat), alias) nombreComp, sum(ifnull( tar.Remociones, 0)), sum(ifnull(tar.Desconexiones, 0)), " & _
                '    " sum(ifnull(tar.Reconexiones, 0)), sum(ifnull(tar.recremos, 0)) " & _
                '    " from empleados emp left outer join tareas tar on (tar.idEmpleado= emp.idEmpleado and tar.fechaDeAsignacion " & _
                '    " between STR_TO_DATE('" & dia.Text & "', '%Y%m%d%') and STR_TO_DATE('" & dia2.Text & "', '%Y%m%d%') ) " & _
                '    " where  estado='EG001' group by emp.idEmpleado order by if(isnull(alias) or LENGTH(alias)=0, CONCAT_WS(' ',nombre,  appat, apmat), alias)"

                'Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "Empleados", True)

                ''lbEmpleados.DataSource = dsEmp
                ''lbEmpleados.DataTextField = "nombreComp"
                ''lbEmpleados.DataValueField = "id"

                'gv_tecnicos.DataSource = dsEmp

            Catch ex As Exception

            Finally
                If Not conn Is Nothing Then
                    conn.Close()
                End If

            End Try

        End If

        If Not Me.IsPostBack Then
            actualizaFiltros()
            'Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

            'Dim conn As New MySqlConnection(cs)
            'Try
            '    Dim selectSQL As String = "Select concat_ws(' ', nombre, appat, apmat) nombreComp, emp.idEmpleado id, Remociones, Desconexiones, Reconexiones, " & _
            '        "recremos " & _
            '        "from empleados emp "

            '    Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "Empleados", True)

            '    'lbEmpleados.DataSource = dsEmp
            '    'lbEmpleados.DataTextField = "nombreComp"
            '    'lbEmpleados.DataValueField = "id"

            '    gv_tecnicos.DataSource = dsEmp

            'Catch ex As Exception

            'Finally
            '    If Not conn Is Nothing Then
            '        conn.Close()
            '    End If

            'End Try

            Me.ViewState("idEmpleado") = idEmpleado
            'dia.Text = getFecha("yyyyMMdd")
            'dia2.Text = getFecha("yyyyMMdd")

            lbInfo.Text = "Escriba el nombre de una calle y la fecha para obtener datos"

            lbAlerta.Visible = True
            lbStatus.Visible = False


            Me.DataBind()

            If Not Request.QueryString("option") = "" Then
                Select Case Request.QueryString("option")
                    Case verTarea
                        getOrdenes(Request.QueryString("tarea"))
                    Case verResumenDesglose
                        setResumenDesglose()
                    Case verSeguimientoDesglose
                        setSeguimientoDesglose()
                    Case ver_tabla
                        setResumenTabla()
                End Select

            End If
            Request.Cookies.Remove("WTdivSampleScrollY")

            If rbTipoDeBusqueda.SelectedIndex = 1 Then
                ck_filtrarRealizadas.Visible = False
                dia.Visible = False
                dia2.Visible = False
                Label1.Visible = False
                'Label2.Visible = False
            Else
                ck_filtrarRealizadas.Visible = True
                dia.Visible = True
                dia2.Visible = True
                Label1.Visible = True
                'Label2.Visible = True
            End If

        Else
            
        End If


        b_imprimir.Attributes.Clear()
        If (dgOrdenes.Items.Count > 0) Then

            b_imprimir.Attributes.Add("onclick", returnPrintParams("d_imprimir"))
        Else
            b_imprimir.Attributes.Add("onclick", "alert('No hay datos que imprimir'); return true;")
        End If


    End Sub

    Sub setResumenTabla()
        verTabla()
        setDatos()
    End Sub
    'mostramos el desglose de ordenes segun los datos obtenidos de la pagina de ver resumen
    Public Sub setResumenDesglose()
        dia.Text = Request.QueryString("fecha1")
        dia2.Text = Request.QueryString("fecha2")

       

        If Not Request.QueryString("estadoDeLaOrden") = "" Then
            'ls_filtro = " and estadoDeLaOrden='" & Request.QueryString("estadoDeLaOrden") & "' "
            ddl_estadosOrden.SelectedValue = Request.QueryString("estadoDeLaOrden")
        End If
        'ck_filtrarVerificadas.Checked = False
        ck_filtrarSinVerificar.Checked = True

        setDatos()

    End Sub

    'mostramos el desglose de ordenes segun los datos obtenidos de la pagina de ver resumen
    Public Sub setSeguimientoDesglose()
        dia.Text = Request.QueryString("fecha1")
        dia2.Text = Request.QueryString("fecha2")




        If Not Request.QueryString("motivo") = "" Then
            'If Convert.ToInt32(Request.QueryString("motivo")) > 2 Then
            'ls_filtro = " and estadoDeLaOrden='" & Request.QueryString("estadoDeLaOrden") & "' "
            ddl_motivos.SelectedValue = Request.QueryString("motivo")
            'Else

            'End If


        End If
        'ck_filtrarVerificadas.Checked = False
        ck_filtrarSinVerificar.Checked = True

        setDatos()

    End Sub


    Public Sub LinkButton_Click(sender As Object, e As EventArgs)
        Response.Redirect("verOrdenes.aspx?busqueda=" & CType(sender, LinkButton).Text & "&tipo=" & "ord.idOrden")
        'Response.Redirect("verOrdenes.aspx?busqueda=" & CType(sender, LinkButton).Text & "&tipo=" & "ord.numOrden&q=" & selectSQL)

    End Sub

    Sub verTabla()
        rbTipoDeBusqueda.SelectedValue = Request.QueryString("rbTipoDeBusqueda")
        dia.Text = Request.QueryString("dia")
        dia2.Text = Request.QueryString("dia2")
        ck_filtrarRealizadas.Checked = Request.QueryString("ck_filtrarRealizadas")
        ck_filtrarSinVerificar.Checked = Request.QueryString("ck_filtrarSinVerificar")
        ck_filtrarCorrectas.Checked = Request.QueryString("ck_filtrarCorrectas")
        ck_filtrarIncorrectas.Checked = Request.QueryString("ck_filtrarIncorrectas")
        'cb_desconexiones.Checked = Request.QueryString("cb_desconexiones")
        'cb_reconexiones.Checked = Request.QueryString("cb_reconexiones")
        'cb_remociones.Checked = Request.QueryString("cb_remociones")
        'cb_recremos.Checked = Request.QueryString("cb_recremos")
        ddl_estadosOrden.SelectedValue = Request.QueryString("ddl_estadosOrden")
        ddl_motivos.SelectedValue = Request.QueryString("ddl_motivos")

    End Sub

    Protected Sub bObtenerOrdenes_Click(sender As Object, e As EventArgs) Handles bObtenerOrdenes.Click
        ls_filtro = ""
        setDatos()

        b_imprimir.Attributes.Clear()
        If (dgOrdenes.Items.Count > 0) Then

            b_imprimir.Attributes.Add("onclick", returnPrintParams("d_imprimir"))
        Else
            b_imprimir.Attributes.Add("onclick", "alert('No hay datos que imprimir'); return true;")
        End If
    End Sub

    Public Sub setDatos()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)
        Try

            getOrdenes()
            lbInfo.Text = "Calle a buscar '" & txtBuscar.Text & "'"


            lb_tecnico.Text = lbInfo.Text

            If (rbTipoDeBusqueda.SelectedValue = 0) Then
                lbInfo.Text &= "<br/>" & ""


            End If

            If dgOrdenes.Items.Count = 0 Then
                lbAlerta.Visible = True
                lbStatus.Visible = False


            Else
                lbAlerta.Visible = False

                If (rbTipoDeBusqueda.SelectedValue = 0) Then
                    lbStatus.Visible = False
                Else
                    lbStatus.Visible = False
                End If
            End If
        Catch ex As Exception

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try
    End Sub

    'Esta funcion sirve para emular lo que haria una persona, pero recibiendo la tarea
    Public Sub getOrdenes(tarea As Integer)


        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)

        'ck_filtrarVerificadas.Checked = False
        ck_filtrarSinVerificar.Checked = True

        Try



            Dim selectSQL As String = "Select idEmpleado,  DATE_FORMAT(fechaDeAsignacion, '%Y%m%d') dia from tareas where idTarea=" & tarea


            'Seleccionamos que queremos todas las ordenes.
            rbTipoDeBusqueda.SelectedValue = 1

            'Buscamos la tarea que queremos
            Dim dsTarea As DataSet = conectarMySql(conn, selectSQL, "tareas", True)

            If dsTarea.Tables(0).Rows.Count = 0 Then

                Return

            End If

            Dim row As DataRow = dsTarea.Tables(0).Rows(0)


            dia.Text = row("dia")
            dia2.Text = row("dia")

        Catch ex As Exception

        Finally
            If Not conn Is Nothing Then
                conn.Close()

            End If

        End Try


        setDatos()

    End Sub


    Public Sub getOrdenes()
        guardarFechasEnCookie()

        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)
        Try
            Dim selectSQL As String
            Dim porUsuario As String = " and  ca.idCalle = cli.idCalle and ca.nombre like '%" & txtBuscar.Text & "%' "


            If dia2.Text.Length = 0 Then
                dia2.Text = dia.Text
            End If


            'If ck_filtrarVerificadas.Checked Then
            '    porUsuario &= " and ord.estadoDeLaRevision<>'ER000' "
            'End If



            If (rbTipoDeBusqueda.SelectedValue = 0) Then 'Todas las que no se han empezado
                selectSQL = "Select ord.estadoDeLaRevision,  if(isnull(emp.alias) or LENGTH(emp.alias)=0, CONCAT_WS(' ',emp.nombre,  emp.appat, emp.apmat), emp.alias) tecnico,  cli.poliza poliza,  if (isnull(ord.estadoAnterior), '',ord.estadoAnterior) estadoAnterior , ord.idOrden, ord.idOrden numorden, ord.descOrden tipodeOrden, est.nombre estadodelaorden ,ord.numMedidor, ord.lectura, mot.nombre motivo , ord.fechadeejecucion fechadeejecucion, fechaderecepcion, ord.comentarios, ord.numSello, ord.latitud, ord.longitud " & _
                                      " , cli.comoLlegar, cli.numExterior, cli.numInterior,col.nombre colonia, mun.nombre municipio, cli.Nombre cliente, cli.poliza , cli.entreCalles,ca.nombre calle,ord.numMedidor numMedidor from ordenes ord left outer join  tareas tar on ( tar.idtarea=ord.idtarea) left outer join codigos mot on (ord.motivo=mot.codigo), empleados emp,  codigos est, clientes cli " & _
                                      ",municipios mun, " & _
                                        "colonias col, " & _
                                        "calles ca " & _
                                        "where 1=1 " & porUsuario & " and est.codigo=ord.estadodelaorden" & " and ord.estadoDeLaorden='EO001' and cli.idCliente=ord.idCliente and tar.idEmpleado=emp.idEmpleado " & _
                                      " and " & traerTiposDeOrdenSeleccionados() & _
                                      "and ca.idColonia = col.idColonia " & _
                                         "and col.idMunicipio = mun.idMunicipio " & _
                                        "and ca.idCalle = cli.idCalle " & _
                                        ls_filtro & _
                                      " order By ord.tipodeorden, ord.estadodelaorden, ord.motivo, ord.fechadeejecucion "
                dgOrdenes.Columns(8).Visible = False 'Ocultamos la fecha de ejecucion
            Else
                selectSQL = "Select  ord.estadoDeLaRevision, if(isnull(emp.alias) or LENGTH(emp.alias)=0, CONCAT_WS(' ',emp.nombre,  emp.appat, emp.apmat), emp.alias) tecnico, cli.poliza poliza, if (isnull(ord.estadoAnterior), '',ord.estadoAnterior) estadoAnterior, ord.idOrden, ord.idOrden numorden, ord.descOrden tipodeOrden, est.nombre estadodelaorden ,ord.numMedidor, ord.lectura, mot.nombre motivo , ord.fechadeejecucion fechadeejecucion, fechaderecepcion, ord.comentarios, ord.numSello, ord.latitud, ord.longitud " & _
                                      " ,  cli.comoLlegar, cli.numExterior, cli.numInterior,col.nombre colonia, mun.nombre municipio, cli.Nombre cliente, cli.poliza , cli.entreCalles,ca.nombre calle,ord.numMedidor numMedidor from ordenes ord left outer join  tareas tar on ( tar.idtarea=ord.idtarea) left outer join codigos mot on (ord.motivo=mot.codigo), codigos est,  empleados emp , clientes cli " & _
                                        ",municipios mun, " & _
                                        "colonias col, " & _
                                        "calles ca " & _
                                        "where 1=1  and cli.idCliente=ord.idCliente and tar.idEmpleado=emp.idEmpleado " & _
                                        "and ca.idColonia = col.idColonia " & _
                                         "and col.idMunicipio = mun.idMunicipio " & _
                                        "and ca.idCalle = cli.idCalle " & _
                                        ls_filtro & _
                                        porUsuario & " and est.codigo=ord.estadodelaorden" & " and tar.fechadeasignacion between STR_TO_DATE('" & dia.Text & "', '%Y%m%d%') and " & " STR_TO_DATE('" & dia2.Text & "', '%Y%m%d%')"



                If ck_filtrarRealizadas.Checked Then
                    selectSQL &= " and ord.estadodelaorden<>'EO001'  "
                End If

                selectSQL &= " and " & traerTiposDeOrdenSeleccionados()

                selectSQL &= " order By ord.tipodeorden, ord.estadodelaorden, ord.motivo, ord.fechadeejecucion"

                dgOrdenes.Columns(8).Visible = True 'Mostramos la fecha de ejecucion
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

            dgOrdenListado.DataSource = dsOrdenes.Tables("ordenes")
            dgOrdenListado.DataBind()

            Me.selectSQL = selectSQL
            Me.ViewState("selectSQL") = selectSQL
            Query.Value = selectSQL
            'establecerQuery()
            'Hora de inicio, hora fin

            If (rbTipoDeBusqueda.SelectedValue = 1 And Not Request.QueryString("option") = verResumenDesglose) Then
                selectSQL = "Select" & _
                                         " max(ord.fechadeejecucion) fin, min(ord.fechadeejecucion) inicio from ordenes ord left outer join  tareas tar on ( tar.idtarea=ord.idtarea) left outer join codigos mot on (ord.motivo=mot.codigo), codigos est , calles ca, clientes cli " & _
                                         "where ord.idCliente=cli.idCliente and ca.idCalle = cli.idCalle  and  ca.nombre like '%" & txtBuscar.Text & "%' and est.codigo=ord.estadodelaorden" & " and tar.fechadeasignacion between STR_TO_DATE('" & dia.Text & "', '%Y%m%d%') and " & " STR_TO_DATE('" & dia2.Text & "', '%Y%m%d%')" & _
                                         "and not( ord.estadodelaorden='EO005' or ord.estadodelaorden='EO008'  or ord.estadodelaorden='EO001') order By ord.fechadeejecucion, ord.tipodeorden"
                Dim dsInfo As DataSet = conectarMySql(conn, selectSQL, "Ordenes", False)



            Else
                lbStatus.Text = ""
            End If

            generaArchivoPuntos(dsOrdenes, User.Identity.Name & "_" & getFecha("HHmmss"))
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

    

    'Sub borrarCookie()

    '    Dim sb As New System.Text.StringBuilder()
    '    ' sb.Append("<script type = 'text/javascript'>")
    '    sb.Append("window.onload=function(){")
    '    sb.Append("eraseCookie('WTdivSampleScrollY');")
    '    sb.Append("'};")
    '    'sb.Append("</script>")
    '    'ClientScript.RegisterClientScriptBlock(Me.GetType(), "FocusTecnico", sb.ToString())
    '    ScriptManager.RegisterStartupScript(gv_tecnicos, Me.GetType(), "highlight", sb.ToString, True)
    'End Sub


    Protected Sub rbTipoDeBusqueda_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbTipoDeBusqueda.SelectedIndexChanged

        If rbTipoDeBusqueda.SelectedIndex = 1 Then
            ck_filtrarRealizadas.Visible = False
            dia.Visible = False
            dia2.Visible = False
            Label1.Visible = False
            'Label2.Visible = False
        Else
            ck_filtrarRealizadas.Visible = True
            dia.Visible = True
            dia2.Visible = True
            Label1.Visible = True
            'Label2.Visible = True
        End If
    End Sub

    Protected Function traerTiposDeOrdenSeleccionados() As String
        Dim cadena As String = " ord.tipoDeOrden in ("
        Dim tipos As String = ""
        'If cb_desconexiones.Checked Then
        '    tipos &= "'TO002'"
        'End If

        'If cb_reconexiones.Checked Then

        '    If tipos.Length > 0 Then
        '        tipos &= ","
        '    End If

        '    tipos &= "'TO003'"
        'End If

        'If cb_remociones.Checked Then

        '    If tipos.Length > 0 Then
        '        tipos &= ","
        '    End If

        '    tipos &= "'TO001'"
        'End If

        'If cb_recremos.Checked Then

        '    If tipos.Length > 0 Then
        '        tipos &= ","
        '    End If

        '    tipos &= "'TO004'"
        'End If

        'If tipos.Length = 0 Then
        '    tipos = "'TO000'" 'Ninguna esta seleccionada
        'End If

        If tipos.Length = 0 Then
            tipos = "'TO002'" 'Ninguna esta seleccionada
        End If

        cadena &= tipos & ") "

        'Ahora... vamos a traer los tipos de revision
        tipos = ""
        cadena &= " and ord.estadoDeLaRevision in ("

        If ck_filtrarCorrectas.Checked Then

            If tipos.Length > 0 Then
                tipos &= ","
            End If

            tipos &= "'ER001'"
        End If

        If ck_filtrarIncorrectas.Checked Then

            If tipos.Length > 0 Then
                tipos &= ","
            End If

            tipos &= "'ER002'"
        End If

        If ck_filtrarSinVerificar.Checked Then

            If tipos.Length > 0 Then
                tipos &= ","
            End If

            tipos &= "'ER000'"
        End If

        If tipos.Length = 0 Then
            tipos = "'TO000'" 'Ninguna esta seleccionada
        End If

        cadena &= tipos & ") "

        'ahora los estados

        If (Not ddl_estadosOrden.SelectedItem.Text.StartsWith("(")) Then
            cadena &= " and ord.estadoDeLaOrden='" & ddl_estadosOrden.SelectedValue & "' "
        End If

        cadena &= parseMotivos(ddl_motivos.SelectedValue)
        Return cadena
    End Function



    Function estadoAnterior(estado As String) As String
        Dim resultado As String = ""

        If Not IsNothing(estado) Then
            If estado.Equals("EO005") Then
                resultado &= " (PAGADA)"
            ElseIf estado.Equals("EO008") Then
                resultado &= " (CANCELADA)"
            End If

        End If

        Return resultado
    End Function

    Public Sub actualizaFiltros()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim selectSQL As String = ""

        Dim conn As New MySqlConnection(cs)

        Try
            'conn.Open()
            ' Actualizamos la lista de técnicos
            selectSQL = "select * from ((Select '(Estados de la Orden)' nombre, '-1' codigo) union (select nombre, codigo from codigos where codigo like 'EO%' and codigo not in ('EO000', 'EO006', 'EO010', 'EO008', 'EO003', 'EO011', 'EO009', 'EO007') order by nombre)) tmp order by nombre"

            Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "codigos", True)

            ddl_estadosOrden.DataSource = dsEmp
            ddl_estadosOrden.DataTextField = "nombre"
            ddl_estadosOrden.DataValueField = "codigo"

            ddl_estadosOrden.DataBind()



        Catch ex As Exception

        Finally
            'conn.Close()

        End Try



    End Sub

    Sub guardarFechasEnCookie()
        Dim cookie As HttpCookie = Request.Cookies("verOrdenesPorEmpleado")
        If cookie Is Nothing Then
            cookie = New HttpCookie("verOrdenesPorEmpleado")

        End If

        cookie("dia") = dia.Text
        cookie("dia2") = dia2.Text

        cookie.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(cookie)
    End Sub

    Function cambiaColorSegunRevision(estadoRevision As String) As System.Drawing.Color
        Select Case estadoRevision

            Case "ER001" ' 
                Return System.Drawing.ColorTranslator.FromHtml("#37e32e")
            Case "ER002" ' incorrecta
                Return System.Drawing.ColorTranslator.FromHtml("#e34c2e")
            Case Else 'sin revisar
                Return System.Drawing.Color.White
        End Select
    End Function

    'Protected Sub DownloadFiles(sender As Object, e As EventArgs)
    '    Using zip As New ZipFile()
    '        Dim extension As String = ".jpg"
    '        zip.AlternateEncodingUsage = ZipOption.AsNecessary
    '        zip.AddDirectoryByName("fotos")
    '        'Primero quiero el query que ya habiamos realizado

    '        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
    '        Dim conn As New MySqlConnection(cs)
    '        Try
    '            Dim ds As DataSet = conectarMySql(conn, selectSQL, "ordenes", True)

    '            For Each row As DataRow In ds.Tables(0).Rows
    '                'If TryCast(row.FindControl("chkSelect"), CheckBox).Checked Then
    '                '    Dim filePath As String = TryCast(row.FindControl("lblFilePath"), Label).Text
    '                '    
    '                'End If

    '                Dim ls_numOrden As String = row("numOrden")
    '                ls_numOrden = ls_numOrden.PadLeft(obtieneLongitud(conn, "numOrden"), "0")
    '                Dim ls_poliza As String = row("poliza")
    '                ls_poliza = ls_poliza.PadLeft(obtieneLongitud(conn, "poliza"), "0")
    '                Dim nombreGenerico As String = ls_numOrden & "_" & ls_poliza
    '                'Buscamos por la foto de antes

    '                'Dim images As New List(Of String)
    '                For Each foundFile As String In My.Computer.FileSystem.GetFiles(
    '                 Server.MapPath("~/") & "fotos\",
    '                Microsoft.VisualBasic.FileIO.SearchOption.SearchTopLevelOnly, nombreGenerico & "*.jpg")
    '                    zip.AddFile(foundFile, "fotos")
    '                    'images.Add(String.Format("~/fotos/{0}", System.IO.Path.GetFileName(foundFile)))
    '                Next

    '                'Dim filePath As String = Server.MapPath("~/") & "fotos\" & nombreGenerico & "_" & "1" & extension
    '                'If System.IO.File.Exists(filePath) Then
    '                '    zip.AddFile(filePath, "fotos")

    '                'End If

    '                ''buscamos por la foto del despues
    '                'filePath = Server.MapPath("~/") & "fotos\" & nombreGenerico & "_" & "2" & extension
    '                'If System.IO.File.Exists(filePath) Then
    '                '    zip.AddFile(filePath, "fotos")

    '                'End If
    '            Next
    '            Response.Clear()
    '            Response.BufferOutput = False
    '            'Dim zipName As String = [String].Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))
    '            Dim zipName As String = [String].Format("Fotos_" & gv_tecnicos.SelectedRow.Cells(0).Text & "_{0}.zip", dia.Text & "_" & dia2.Text)
    '            Response.ContentType = "application/zip"
    '            Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
    '            zip.Save(Response.OutputStream)
    '            Response.[End]()
    '        Catch ex As Exception

    '        Finally
    '            If Not conn Is Nothing Then
    '                conn.Close()
    '            End If
    '        End Try

    '    End Using
    'End Sub

    'Protected Sub bBajarFotos_Click(sender As Object, e As EventArgs) Handles bBajarFotos.Click
    '    If dgOrdenes.Items.Count = 0 Then
    '        message_generico("No hay ordenes seleccionadas")
    '        'messagebox("No hay ordenes seleccionadas")
    '        Return
    '    End If
    '    DownloadFiles(sender, e)
    'End Sub

    'Protected Sub bAExcel_Click(sender As Object, e As EventArgs) Handles bAExcel.Click
    '    Dim nombreArchivo As String = gv_tecnicos.SelectedRow.Cells(0).Text & "_" & dia.Text & "_" & dia2.Text

    '    gridAExcel(nombreArchivo, dgOrdenListado)
    'End Sub

    Public Sub generaArchivoPuntos(ds As DataSet, nombreArchivo As String)

        Dim esLaPrimera As Boolean = True
        Dim ext As String = "kml"


        For Each row As DataRow In ds.Tables("Ordenes").Rows
            If Not IsDBNull(row.Item("latitud")) Then

                If esLaPrimera Then
                    esLaPrimera = False
                    escribeArchivoGen(nombreArchivo, "<?xml version='1.0' encoding='UTF-8'?>", "uploads", False, ext)
                    escribeArchivoGen(nombreArchivo, "<kml xmlns='http://earth.google.com/kml/2.0'>", "uploads", True, ext)
                    escribeArchivoGen(nombreArchivo, "<Document>", "uploads", True, ext)
                    escribeArchivoGen(nombreArchivo, "<name>KML Example file</name>", "uploads", True, ext)
                    escribeArchivoGen(nombreArchivo, "<description>Simple markers</description>", "uploads", True, ext)

                End If
                escribeArchivoGen(nombreArchivo, "<Placemark>", "uploads", True, ext)
                escribeArchivoGen(nombreArchivo, "<description>" & row.Item("comoLlegar") & " " & row.Item("calle") & " " & row.Item("colonia") & " </description>", "uploads", True, ext)
                escribeArchivoGen(nombreArchivo, "<name>Numero de Orden " & row.Item("numOrden") & "</name>", "uploads", True, ext)
                escribeArchivoGen(nombreArchivo, "<Point>", "uploads", True, ext)
                escribeArchivoGen(nombreArchivo, "<coordinates>" & row.Item("longitud") & "," & row.Item("latitud") & ",0</coordinates>", "uploads", True, ext)
                escribeArchivoGen(nombreArchivo, "</Point>", "uploads", True, ext)
                escribeArchivoGen(nombreArchivo, "</Placemark>", "uploads", True, ext)

            End If

        Next

        If Not esLaPrimera Then
            escribeArchivoGen(nombreArchivo, "</Document>", "uploads", True, ext)
            escribeArchivoGen(nombreArchivo, "</kml>", "uploads", True, ext)
        End If

        b_verMapa.Attributes.Clear()
        b_verMapa.Attributes.Add("onclick", "window.open('http://maps.google.com/maps?q=http://www.espinosacarlos.com/cyrMaxigas/uploads/" & nombreArchivo & "." & ext & "'" & "); return false;")

    End Sub





End Class
