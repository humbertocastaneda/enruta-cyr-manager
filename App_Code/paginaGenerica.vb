Imports Microsoft.VisualBasic

Public Class paginaGenerica
    Inherits System.Web.UI.Page
    Public verTarea As Integer = 0
    Public verResumenDesglose As Integer = 1
    Public verSeguimientoDesglose As Integer = 2
    Public ver_tabla As Integer = 3

    Public Shared ReadOnly ASIGNADAS As Integer = 0
    Public Shared ReadOnly PENDIENTES As Integer = 1
    Public Shared ReadOnly EJECUTADAS As Integer = 2
    Public Shared ReadOnly AUTORECONECTADO As Integer = 3
    Public Shared ReadOnly REJA As Integer = 4
    Public Shared ReadOnly CTE_NO_PERMITE As Integer = 5
    Public Shared ReadOnly NO_VISITADAS As Integer = 6
    Public Shared ReadOnly INCORRECTAS As Integer = 7
    Public Shared ReadOnly OTROS As Integer = 8
    Public Shared ReadOnly SIN_REGISTRO As Integer = 9

    Public Shared Function conectarMySql(ByRef conn As MySql.Data.MySqlClient.MySqlConnection, selectSQL As String, tabla As String, Conectar As Boolean) As Data.DataSet
        If Conectar Then
            conn.Open()
        End If

        Dim cmd As New MySql.Data.MySqlClient.MySqlCommand(selectSQL, conn)
        'Dim reader As MySqlDataReader
        Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
        Dim pubs As New Data.DataSet()

        If (Not tabla = "") Then
            adapter.Fill(pubs, tabla)
        Else
            adapter.Fill(pubs)
        End If

        Return pubs
    End Function
    Public Shared Function getBD() As String
        Return ConfigurationManager.AppSettings("bd")
    End Function

    Public Sub messagebox(message As String)

        Dim sb As New System.Text.StringBuilder()
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
    End Sub

    'Regresa a la pagina de login en caso de que no se haya logeado
    Public Function isLoged() As Boolean
        If (HttpContext.Current.User.Identity.IsAuthenticated) Then

            Return True
        Else
            Response.Redirect("default.aspx")
            Return False

        End If

    End Function

    Protected Sub cerrarSession()
        FormsAuthentication.SignOut()
        'Response.Redirect("default.aspx")
    End Sub

    Public Function obtieneLongitud(conn As MySql.Data.MySqlClient.MySqlConnection, campo As String) As Integer
        Dim sqlSelect As String = "select longitudEntrada from estructuras where upper(nombrecampobd)='" & campo.ToUpper & "'"
        Dim dsEstructura As Data.DataSet = conectarMySql(conn, sqlSelect, "estructura", False)

        Dim row As Data.DataRow = dsEstructura.Tables("estructura").Rows.Item(0)

        Return row("longitudEntrada")

    End Function

    Public Shared Function obtieneColumna(conn As MySql.Data.MySqlClient.MySqlConnection, campo As String) As Integer
        Dim sqlSelect As String = "select campo from estructuras where upper(nombrecampobd)='" & campo.ToUpper & "'"
        Dim dsEstructura As Data.DataSet = conectarMySql(conn, sqlSelect, "estructura", False)

        Dim row As Data.DataRow = dsEstructura.Tables("estructura").Rows.Item(0)

        Return row("campo")

    End Function

    Public Shared Function getLastInsertedId(conn As MySql.Data.MySqlClient.MySqlConnection) As Integer

        Return getLastInsertedId(conn, True)
    End Function


    Public Shared Function getLastInsertedId(conn As MySql.Data.MySqlClient.MySqlConnection, conectar As Boolean) As Integer
        Dim selectSQL As String = "select last_insert_id() id from dual"
        Dim ds As Data.DataSet = conectarMySql(conn, selectSQL, "dual", conectar)

        Dim row As Data.DataRow = ds.Tables("dual").Rows.Item(0)

        Return row("id")

    End Function

    Shared Function getFecha(format As String) As String
        'Return DateTime.Now.ToString(format)
        Dim ahora As DateTime = DateTime.Now


        Dim tiempo As String = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(ahora, TimeZoneInfo.Local.Id, "Central Standard Time (Mexico)").ToString(format)

        Return tiempo
    End Function


    Function changeDBNull(item As Object) As Object
        Return changeDBNull(item, "")
    End Function

    Function changeDBNull(item As Object, format As String) As Object
        'changeDBNull = If(IsDBNull(item), format, item)
        If IsDBNull(item) Then
            changeDBNull = format
        Else
            changeDBNull = item
        End If
        If TypeName(item) = "String" Then
            'changeDBNull = If(changeDBNull = "" And Not format = "", format, changeDBNull)
            If changeDBNull = "" And Not format = "" Then
                changeDBNull = format
            End If
        ElseIf TypeName(item) = "Date" Then
            'changeDBNull = If(CType(item, Date) = "#12:00:00 AM#" And Not format = "", format, changeDBNull)
            If CType(item, Date) = "#12:00:00 AM#" And Not format = "" Then
                changeDBNull = format
            End If
        End If


    End Function

    Function getCadenaAConcatenar(item As Object) As Object
        getCadenaAConcatenar = changeDBNull(item, "")
        getCadenaAConcatenar = CType(getCadenaAConcatenar, String).Trim
    End Function

    Shared Function moverABackup(ruta As String, archivo As String) As String
        If System.IO.File.Exists(ruta & archivo) = True Then
            Dim nuevoArchivo As String
            Dim li_pos As Integer = archivo.IndexOf(".")
            Dim nuevaRuta As String = subirDirectorios(ruta, 3) & "backup" & obtenerUltimoDirectorio(ruta, 2)
            nuevoArchivo = archivo.Substring(0, li_pos) & "_" & getFecha("yyyyMMddHHmmss") & archivo.Substring(li_pos)



            System.IO.File.Move(ruta & archivo, nuevaRuta & nuevoArchivo)
            moverABackup = nuevaRuta & nuevoArchivo
        Else
            Throw New Exception("mover a bkp:No existe el archivo")

        End If
    End Function

    Shared Function subirDirectorios(ruta As String, cuantos As String) As String

        Dim li_pos As String
        subirDirectorios = ruta
        Dim i As Integer
        li_pos = subirDirectorios.LastIndexOf("\")
        While li_pos > 0
            i += 1
            subirDirectorios = subirDirectorios.Substring(0, li_pos)
            li_pos = subirDirectorios.LastIndexOf("\")
            If i = cuantos Then
                Exit While
            End If
        End While
        subirDirectorios &= "\"
    End Function

    Shared Function obtenerUltimoDirectorio(ruta As String, veces As Integer) As String

        Dim li_pos As Integer
        obtenerUltimoDirectorio = ruta
        Dim i As Integer
        li_pos = ruta.Length
        While li_pos > 0
            i += 1
            li_pos = obtenerUltimoDirectorio.LastIndexOf("\", li_pos)
            obtenerUltimoDirectorio = obtenerUltimoDirectorio.Substring(0, li_pos)
            If i = veces + 1 Then
                Exit While
            End If
        End While
        obtenerUltimoDirectorio = ruta.Substring(li_pos)
    End Function

    Shared Function getConnection() As MySql.Data.MySqlClient.MySqlConnection
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySql.Data.MySqlClient.MySqlConnection(cs)
        conn.Open()
        Return conn
    End Function

    Public Sub escribeArchivoSalida(fileName As String, Linea As String, subcarpeta As String)
        If Not subcarpeta.EndsWith("/") And Trim(subcarpeta).Length > 0 Then
            subcarpeta &= "/"
        End If
        Dim directorio As String = Server.MapPath("~/") & "out/" & subcarpeta & fileName & ".txt"
        Dim w As System.IO.StreamWriter = New System.IO.StreamWriter(directorio, True, Encoding.ASCII)



        Try
            'w = File.AppendText(directorio)
            w.WriteLine(Linea)

        Catch ex As Exception
            '           Throw New Exception("Ocurrio un error:" & ex.Message)
        Finally
            If Not w Is Nothing Then
                w.Close()
            End If
        End Try
    End Sub

    'Escribe un archivo generico
    Public Sub escribeArchivoGen(fileName As String, Linea As String, subcarpeta As String, append As Boolean)
        escribeArchivoGen(fileName, Linea, subcarpeta, append, "txt")
    End Sub

    'Escribe un archivo generico
    Public Sub escribeArchivoGen(fileName As String, Linea As String, subcarpeta As String, append As Boolean, ext As String)
        If Not subcarpeta.EndsWith("/") And Trim(subcarpeta).Length > 0 Then
            subcarpeta &= "/"
        End If
        Dim directorio As String = Server.MapPath("~/") & subcarpeta & fileName & "." & ext
        Dim w As System.IO.StreamWriter = New System.IO.StreamWriter(directorio, append, Encoding.ASCII)



        Try
            'w = File.AppendText(directorio)
            w.WriteLine(Linea)

        Catch ex As Exception
            Throw New Exception("Ocurrio un error:" & ex.Message)
        Finally
            If Not w Is Nothing Then
                w.Close()
            End If
        End Try
    End Sub

    Sub getCadenaUsuarios(conn As MySql.Data.MySqlClient.MySqlConnection)
        'Dim ls_cadena As String
        Dim ls_archivo As String = "usuarios"
        Dim ls_subcarpeta As String = "CFG"


        'Encabezado
        escribeArchivoGen(ls_archivo, "usuarios                                          ", ls_subcarpeta, False)
        escribeArchivoGen(ls_archivo, "id                                                Usuario                                                                                                                                               10  0    0", ls_subcarpeta, True)
        escribeArchivoGen(ls_archivo, "password                                          Password                                                                                                                                              8   10   0", ls_subcarpeta, True)
        escribeArchivoGen(ls_archivo, "Nombre                                            Nombre del Usuario                                                                                                                                    50  18   0", ls_subcarpeta, True)
        escribeArchivoGen(ls_archivo, "imei                                              imei                                                                                                                                                  15  68   0", ls_subcarpeta, True)
        escribeArchivoGen(ls_archivo, "-                                                                                                                                                                                                                 ", ls_subcarpeta, True)



        'Contenido
        Dim selectSQL As String = "Select nomina idEmpleado from empleados where estado='EG001'"

        Dim dsOrdenes As Data.DataSet = conectarMySql(conn, selectSQL, "empleados", False)

        For Each row As Data.DataRow In dsOrdenes.Tables(0).Rows
            escribeArchivoGen(ls_archivo, row("idEmpleado").ToString.PadRight(10) & "        Usuario de prueba                                 353399049464034", ls_subcarpeta, True)
        Next

    End Sub

    Public Sub getCadenaAEscribir2(lsOrden As String, asignada As Boolean, conn As MySql.Data.MySqlClient.MySqlConnection)
        Dim resultado As String
        If asignada Then
            'If IsDBNull(row("idEmpleado")) Then
            resultado = "A"
            'Else
            '    Return

            'End If
        Else
            'If IsDBNull(row("idEmpleado")) Then
            '    Return
            'Else
            resultado = "C"

            'End If

        End If

        getCadenaAEscribir(lsOrden, resultado, conn)
    End Sub

    Public Sub getCadenaAEscribir(lsOrden As String, tipo As String, conn As MySql.Data.MySqlClient.MySqlConnection)
        'Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        'Dim conn As New MySqlConnection(cs)
        Dim resultado As String = ""
        Dim selectSQL As String = "Select ord.idOrden, ord.diametro_toma, ord.vencido, ord.balance, ord.giro, ord.fecha_ultimo_pago, ord.ultimo_Pago, ord.numSerie numSerie, ord.ano ano, emp.nomina nomina, ord.numOrden numOrden, ord.poliza poliza, cli.nombre cliente, ord.numMedidor numMedidor, ca.nombre calle, cli.NumInterior numInterior, " & _
            "cli.numExterior numExterior, co.nombre colonia, mu.nombre municipio, cli.entrecalles entrecalles, cli.comoLlegar comoLlegar, ord.descOrden descOrden, ord.tipodeorden,  " & _
            " date_Format(tar.fechadeAsignacion, '%Y%m%d' ) fechadeAsignacion, tar.idEmpleado idEmpleado from empleados emp, ordenes ord, tareas tar , calles ca, colonias co, municipios mu, clientes cli  " & _
            "where tar.idTarea=ord.idtarea and ord.idOrden=" & lsOrden & _
            " and tar.idEmpleado=emp.idEmpleado and ca.idcalle=cli.idCalle and cli.idcliente=ord.idCliente and ca.idColonia = co.idColonia and co.idMunicipio = mu.idMunicipio" & _
            " and ord.estadodelaorden not in ('EO002','EO003','EO004','EO006','EO007', 'CD000','CD001','CD002' )"

        'Select ord.numOrden numOrden, ord.poliza poliza, cli.nombre cliente, ord.numMedidor numMedidor, ca.nombre calle, cli.NumInterior numInterior,
        '    cli.numExterior numExterior, co.nombre colonia, mu.nombre municipio, cli.entrecalles entrecalles, cli.comoLlegar comoLlegar, ord.descOrden descOrden, 
        '    tar.fechadeAsignacion fechadeAsignacion, tar.idEmpleado idEmpleado from ordenes ord, tareas tar , calles ca, colonias co, municipios mu, clientes cli 
        '    where tar.idTarea=ord.idtarea and ord.idOrden=23 and ca.idcalle=cli.idCalle and cli.idcliente=ord.idCliente and ca.idColonia = co.idColonia and co.idMunicipio = mu.idMunicipio
        '     and ord.estadodelaorden not in ('EO002','EO003','EO004','EO005','EO006','EO007', 'CD000','CD001','CD002' );

        Dim dsOrdenes As Data.DataSet = conectarMySql(conn, selectSQL, "ordenes", False)


        'verificamos que tenga resultado, si no, habra una excepcion
        If dsOrdenes.Tables("Ordenes").Rows.Count Then
            'Primero hay que verificar si esa orden si ya estuvo asignada o no
            Dim row As Data.DataRow = dsOrdenes.Tables("Ordenes").Rows.Item(0)

            'If asignada Then
            '    'If IsDBNull(row("idEmpleado")) Then
            '    resultado &= "A"
            '    'Else
            '    '    Return

            '    'End If
            'Else
            '    'If IsDBNull(row("idEmpleado")) Then
            '    '    Return
            '    'Else
            '    resultado &= "C"

            '    'End If

            'End If
            resultado &= tipo

            Dim directorio As String = "ordenes/"

            'Cuando sea un pago, su directorio debe cambiar al igual que su estructura
            'If (tipo = "B") Then
            '    directorio = "pagos/"
            '    resultado &= row("numOrden").ToString.PadRight(obtieneLongitud(conn, "numOrden"), " ")
            '    resultado &= row("poliza").ToString.PadRight(obtieneLongitud(conn, "poliza"), " ")
            'Else
            resultado &= row("idOrden").ToString.PadRight(obtieneLongitud(conn, "numOrden"), " ") ' Voy a cambiar el numero de orden, por el id interno.
            resultado &= row("poliza").ToString.PadRight(obtieneLongitud(conn, "poliza"), " ")
            resultado &= row("cliente").ToString.PadRight(obtieneLongitud(conn, "cliente"), " ")
            resultado &= row("nummedidor").ToString.PadRight(obtieneLongitud(conn, "numMedidor"), " ")
            resultado &= row("calle").ToString.PadRight(obtieneLongitud(conn, "calle"), " ").Substring(0, obtieneLongitud(conn, "calle"))
            resultado &= row("numInterior").ToString.PadRight(obtieneLongitud(conn, "numInterior"), " ")
            resultado &= row("numExterior").ToString.PadRight(obtieneLongitud(conn, "numExterior"), " ")
            resultado &= "".ToString.PadRight(obtieneLongitud(conn, "piso_departamento"), " ")
            resultado &= row("colonia").ToString.PadRight(obtieneLongitud(conn, "colonia"), " ")
            resultado &= row("municipio").ToString.PadRight(obtieneLongitud(conn, "municipio"), " ")
            resultado &= row("entreCalles").ToString.PadRight(obtieneLongitud(conn, "entreCalles"), " ")
            resultado &= row("comoLlegar").ToString.PadRight(obtieneLongitud(conn, "comollegar"), " ")
            resultado &= "".ToString.PadRight(obtieneLongitud(conn, "avisoallector"), " ")
            resultado &= "".ToString.PadRight(obtieneLongitud(conn, "marcaMedidor"), " ")
            resultado &= "".ToString.PadRight(obtieneLongitud(conn, "tipoMedidor"), " ")
            resultado &= "".ToString.PadRight(obtieneLongitud(conn, "estadoDelServicio"), " ")
            resultado &= "".ToString.PadRight(obtieneLongitud(conn, "tarifa"), " ")
            'resultado &= parseCodigo(conn, row("descOrden"))
            resultado &= row("tipoDeOrden")
            resultado &= "001".ToString.PadRight(obtieneLongitud(conn, "timeoflife"), " ")
            resultado &= "d".ToString.PadRight(obtieneLongitud(conn, "medtimeoflife"), " ")
            resultado &= row("fechadeAsignacion") & getFecha("HHmmss")
            resultado &= "".ToString.PadRight(obtieneLongitud(conn, "numSello"), " ")
            resultado &= changeDBNull(row("ano")).ToString.PadRight(obtieneLongitud(conn, "anioMedidor"), " ")
            resultado &= changeDBNull(row("numSerie")).ToString.PadRight(obtieneLongitud(conn, "serieMedidor"), " ")
            resultado &= changeDBNull(row("vencido")).ToString.PadRight(obtieneLongitud(conn, "vencido"), " ")
            resultado &= changeDBNull(row("balance")).ToString.PadRight(obtieneLongitud(conn, "balance"), " ")
            resultado &= changeDBNull(row("ultimo_pago")).ToString.PadRight(obtieneLongitud(conn, "ultimo_pago"), " ")
            resultado &= changeDBNull(row("fecha_ultimo_pago")).ToString.PadRight(obtieneLongitud(conn, "fecha_ultimo_pago"), " ")
            resultado &= changeDBNull(row("giro")).ToString.PadRight(obtieneLongitud(conn, "giro"), " ").Substring(0, obtieneLongitud(conn, "giro"))
            resultado &= changeDBNull(row("diametro_toma")).ToString.PadRight(obtieneLongitud(conn, "diametro_toma"), " ").Substring(0, obtieneLongitud(conn, "diametro_toma"))
            'End If
            If My.Computer.FileSystem.FileExists(Server.MapPath("~/") & "out/" & directorio & "/" & row("nomina") & ".txt") Then
                System.IO.File.Move(Server.MapPath("~/") & "out/" & directorio & row("nomina") & ".txt", Server.MapPath("~/") & "out/" & directorio & "p" & row("nomina") & ".txt")
            End If
            escribeArchivoSalida("p" & row("nomina"), resultado, directorio)
        End If


    End Sub

    'Function parseCodigo(conn As MySql.Data.MySqlClient.MySqlConnection, campo As String) As String
    '    Dim sqlSelect As String = "select codigo from codigos where upper(nombre)='" & campo.ToUpper & "'"
    '    Dim dsEstructura As Data.DataSet = conectarMySql(conn, sqlSelect, "codigos", False)

    '    Dim row As Data.DataRow = dsEstructura.Tables("codigos").Rows.Item(0)

    '    Return row("codigo")

    'End Function

    Shared Sub simuloTrigger(idOrden As Integer, oldEstadoDeLaorden As String, newEstadoDeLaOrden As String, idTarea As String, conn As MySql.Data.MySqlClient.MySqlConnection)
        'ATENCION, esta rutina debe ser ejecutada despues de haberse realizado el cambio

        If IsDBNull(idTarea) Then
            Return
        End If

        If idTarea.Equals("") Or idTarea.Equals("null") Then
            Return
        End If

        'Dim newEstadoDeLaOrden, tipoDeOrden As String
        Dim tipoDeOrden As String
        'Dim idTarea As Integer
        Dim sqlSelect As String
        Dim update As String
        Dim cmd As New MySql.Data.MySqlClient.MySqlCommand()
        'hay que obtener el nuevo estado de la orden de la bd

        sqlSelect = "Select estadoDeLaorden, idTarea, tipoDeOrden from ordenes where idOrden=" & idOrden

        Dim dsOrdenes As Data.DataSet = conectarMySql(conn, sqlSelect, "ordenes", False)

        'newEstadoDeLaOrden = dsOrdenes.Tables(0).Rows(0).Item("estadoDeLaOrden")
        'idTarea = dsOrdenes.Tables(0).Rows(0).Item("idTarea")
        tipoDeOrden = dsOrdenes.Tables(0).Rows(0).Item("tipoDeOrden")

        cmd.Connection = conn




        If newEstadoDeLaOrden <> oldEstadoDeLaorden Then
            If newEstadoDeLaOrden = "EO001" Then
                If tipoDeOrden = "TO001" Then
                    update = "update tareas set remociones= remociones + 1 where idTarea=" & idTarea

                    cmd.CommandText = update
                    cmd.ExecuteNonQuery()

                    update = "update empleados emp, tareas tar " & _
                             "set emp.remociones = emp.remociones + 1 " & _
                             "where " + idTarea + " = tar.idtarea " & _
                             "and tar.idEmpleado=emp.idEmpleado"

                    cmd.CommandText = update
                    cmd.ExecuteNonQuery()

                ElseIf tipoDeOrden = "TO002" Then

                    update = "update tareas set desconexiones= desconexiones + 1 where idTarea=" & idTarea

                    cmd.CommandText = update
                    cmd.ExecuteNonQuery()

                    update = "update empleados emp, tareas tar " & _
                             "set emp.desconexiones = emp.desconexiones + 1 " & _
                             "where " + idTarea + " = tar.idtarea " & _
                             "and tar.idEmpleado=emp.idEmpleado"
                    cmd.CommandText = update
                    cmd.ExecuteNonQuery()


                ElseIf tipoDeOrden = "TO003" Then

                    update = "update tareas set reconexiones= reconexiones + 1 where idTarea=" & idTarea

                    cmd.CommandText = update
                    cmd.ExecuteNonQuery()


                    update = "update empleados emp, tareas tar " & _
                             "set emp.reconexiones = emp.reconexiones + 1 " & _
                             "where " + idTarea + " = tar.idtarea " & _
                             "and tar.idEmpleado=emp.idEmpleado"

                    cmd.CommandText = update
                    cmd.ExecuteNonQuery()


                ElseIf tipoDeOrden = "TO004" Then

                    update = "update tareas set recremos= recremos + 1 where idTarea=" & idTarea

                    cmd.CommandText = update
                    cmd.ExecuteNonQuery()

                    update = "update empleados emp, tareas tar " & _
                             "set emp.recremos = emp.recremos + 1 " & _
                             "where " + idTarea + " = tar.idtarea " & _
                             "and tar.idEmpleado=emp.idEmpleado"

                    cmd.CommandText = update
                    cmd.ExecuteNonQuery()

                End If
            Else
                '/*En las restas, el estado anterior siempre debe ser EO001*/
                If (oldEstadoDeLaorden = "EO001" Or oldEstadoDeLaorden = "EO004") And newEstadoDeLaOrden <> "EO004" Then
                    'If newEstadoDeLaOrden = "EO000" And oldEstadoDeLaorden <> "EO004" Then
                    If newEstadoDeLaOrden = "EO000" Then
                        If tipoDeOrden = "TO001" Then

                            update = "update tareas set  remociones= remociones - 1 where idTarea=" & idTarea

                            cmd.CommandText = update
                            cmd.ExecuteNonQuery()

                            '/*update empleados emp, tareas tar 
                            '              emp.remociones = emp.remociones - 1
                            '              where old.idTarea = tar.idtarea
                            'and tar.idEmpleado=emp.idEmpleado;*/

                        ElseIf tipoDeOrden = "TO002" Then

                            update = "update tareas set  desconexiones= desconexiones - 1 where idTarea=" & idTarea

                            '/*update empleados emp, tareas tar 
                            '              emp.desconexiones = emp.desconexiones - 1
                            '              where old.idTarea = tar.idtarea
                            'and tar.idEmpleado=emp.idEmpleado;*/
                            cmd.CommandText = update
                            cmd.ExecuteNonQuery()

                        ElseIf tipoDeOrden = "TO003" Then
                            update = "update tareas set reconexiones= reconexiones - 1 where idTarea=" & idTarea


                            '/*update empleados emp, tareas tar 
                            '              emp.reconexiones = emp.reconexiones - 1
                            '              where old.idTarea = tar.idtarea
                            'and tar.idEmpleado=emp.idEmpleado;*/

                            cmd.CommandText = update
                            cmd.ExecuteNonQuery()

                        ElseIf tipoDeOrden = "TO004" Then

                            update = "update tareas set recremos= recremos - 1 where idTarea=" & idTarea


                            '/*update empleados emp, tareas tar 
                            '              emp.recremos = emp.recremos - 1
                            '              where old.idTarea = tar.idtarea
                            'and tar.idEmpleado=emp.idEmpleado;*/

                            cmd.CommandText = update
                            cmd.ExecuteNonQuery()
                        End If

                        '/*else*/

                    End If

                    '/*Para las demas ordenes*/
                    If tipoDeOrden = "TO001" Then

                        update = "update empleados emp, tareas tar " & _
                             "set emp.remociones = emp.remociones - 1 " & _
                             "where " + idTarea + " = tar.idtarea " & _
                             "and tar.idEmpleado=emp.idEmpleado"

                        cmd.CommandText = update
                        cmd.ExecuteNonQuery()

                    ElseIf tipoDeOrden = "TO002" Then


                        update = "update empleados emp, tareas tar " & _
                             "set emp.desconexiones = emp.desconexiones - 1 " & _
                             "where " + idTarea + " = tar.idtarea " & _
                             "and tar.idEmpleado=emp.idEmpleado"

                        cmd.CommandText = update
                        cmd.ExecuteNonQuery()

                    ElseIf tipoDeOrden = "TO003" Then

                        update = "update empleados emp, tareas tar " & _
                             "set emp.reconexiones = emp.reconexiones - 1 " & _
                             "where " + idTarea + " = tar.idtarea " & _
                             "and tar.idEmpleado=emp.idEmpleado"

                        cmd.CommandText = update
                        cmd.ExecuteNonQuery()


                    ElseIf tipoDeOrden = "TO004" Then

                        update = "update empleados emp, tareas tar " & _
                             "set  emp.recremos = emp.recremos - 1 " & _
                             "where " + idTarea + " = tar.idtarea " & _
                             "and tar.idEmpleado=emp.idEmpleado"

                        cmd.CommandText = update
                        cmd.ExecuteNonQuery()

                    End If
                End If
            End If
        End If

    End Sub


    Public Shared Sub asignaDesasigna(idEmpleado As String, idorden As String, estadoDeLaOrden As String, fechaDeAsignacion As String)
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySql.Data.MySqlClient.MySqlConnection(cs)

        Try
            conn.Open()
            asignaDesasigna(idEmpleado, idorden, estadoDeLaOrden, conn, fechaDeAsignacion)
        Catch ex As Exception
            Throw ex
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try



    End Sub

    Public Shared Sub asignaDesasigna(idEmpleado As String, idorden As String, estadoDeLaOrden As String, conn As MySql.Data.MySqlClient.MySqlConnection, fechaDeAsignacion As String)



        Dim ls_update As String
        Dim selectSQL As String
        Dim estadoDelEnvio As String = "EE002"
        Dim idTarea As String
        Dim cmd As New MySql.Data.MySqlClient.MySqlCommand()


        Try
            'conn.Open()
            cmd.Connection = conn

            'getCadenaAEscribir(idorden, If(idEmpleado = "null", False, True), conn)

            'Buscamos si no hay una tarea del empleado seleccionado
            selectSQL = "Select idTarea from tareas where idEmpleado=" & idEmpleado & " and " & "fechadeAsignacion=str_to_date('" & fechaDeAsignacion & "', '%Y%m%d%')"

            Dim ds As Data.DataSet = conectarMySql(conn, selectSQL, "tareas", False)

            If ds.Tables(0).Rows.Count > 0 Then
                ' hay una tarea  para el dia de hoy de este empleado
                idTarea = ds.Tables(0).Rows(0).Item("idtarea")
                'De cualquier manera hay que establecer la bandera a no enviada, ya que se cambiaron los datos
                Dim ls_insert As String = "update tareas set enviada='EE002' where idTarea=" & idTarea
                cmd.CommandText = ls_insert
                cmd.ExecuteNonQuery()
            Else
                'no hay tareas hay que generar una
                Dim ls_insert As String = "Insert into tareas(idEmpleado, idOrden, fechadeAsignacion, enviada) values( " & idEmpleado & ", " & idorden & ", str_to_date('" & fechaDeAsignacion & "', '%Y%m%d%'), 'EE002')"
                cmd.CommandText = ls_insert
                cmd.ExecuteNonQuery()
                idTarea = getLastInsertedId(conn, False)

            End If

            ' si vamos a desasignar y la orden se encuentra desAsignada y no enviada... no hay que enviarla, solo la desasignamos
            selectSQL = "select enviada from ordenes where idOrden=" & idorden

            Dim dsEnviada As Data.DataSet = conectarMySql(conn, selectSQL, "tareas", False)
            If estadoDeLaOrden = "EO000" And dsEnviada.Tables(0).Rows(0).Item("enviada") = "EE002" Then
                idTarea = "null"
                estadoDelEnvio = "EE001"
            End If

            'vamos a sacar el estado anterior de esta orden...

            selectSQL = "select estadoDeLaOrden, idTarea from ordenes where idOrden=" & idorden

            ds = conectarMySql(conn, selectSQL, "ordenes", False)

            ls_update = "update ordenes set idTarea=" & idTarea & ", estadoDeLaOrden='" & estadoDeLaOrden & "', enviada='" & estadoDelEnvio & "' where idOrden=" & idorden

            cmd.CommandText = ls_update
            ' cmd.Connection = conn
            cmd.ExecuteNonQuery()

            If idTarea.Equals("null") Then
                idTarea = ds.Tables(0).Rows(0).Item("idTarea")
            End If


            'simuloTrigger(idorden, ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), conn)
            paginaGenerica.simuloTrigger(idorden, ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), estadoDeLaOrden, idTarea, conn)


        Catch ex As Exception
            Throw ex
            'Finally
            '    If Not conn Is Nothing Then
            '        conn.Close()
            '    End If
        End Try
    End Sub

    Function parseMotivos(motivo As Integer) As String
        parseMotivos = ""
        Select Case motivo
            Case PENDIENTES
                parseMotivos = " ord.estadoDeLaOrden = 'EO001' "
            Case EJECUTADAS
                parseMotivos = " ord.estadoDeLaOrden = 'EO004' "
            Case AUTORECONECTADO
                parseMotivos = "  ord.motivo in ('AC062' , 'AC063') "
            Case REJA
                parseMotivos = " ord.estadoDeLaOrden = 'EO002' "
            Case CTE_NO_PERMITE
                parseMotivos = " ord.motivo='AV020' "
            Case NO_VISITADAS
                parseMotivos = " ord.estadoDeLaOrden in ('EO012', 'EO011')  "

            Case INCORRECTAS
                parseMotivos = " ord.estadoDeLaRevision = 'ER002'  "

            Case OTROS
                parseMotivos = " ord.motivo='AV106' "

            Case SIN_REGISTRO
                parseMotivos = " ord.sinregistro='1' "
        End Select

        If parseMotivos.Length > 0 Then
            parseMotivos = " and " + parseMotivos

        End If

    End Function

    Sub gridAExcel(nombre As String, grilla As DataGrid)
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As IO.StringWriter = New IO.StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        grilla.EnableViewState = False
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        pagina.Controls.Add(form)
        form.Controls.Add(grilla)
        pagina.RenderControl(htw)
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=" & nombre & ".xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
    End Sub


    Public Function returnPrintParams(datos As String) As String
        'returnPrintParams = "var mywindow = window.open('', 'my div', 'height=400,width=600');" & _
        '"mywindow.document.write('<html><head><title>my div</title>');" & _
        '"/*optional stylesheet*/ //mywindow.document.write('<link rel='stylesheet' href='main.css' type='text/css' />');" & _
        '"mywindow.document.write('</head><body >');" & _
        '"mywindow.document.write($(#DatosOrden).html());" & _
        '"mywindow.document.write('</body></html>');" & _
        '"mywindow.print();" & _
        '"mywindow.close();" & _
        '"return true;"

        returnPrintParams = "var headstr = '<html><head><title></title></head><body>';" & _
        "var footstr = '</body>';" & _
        "var newstr = document.all.item('" & datos & "').innerHTML;" & _
        "var oldstr = document.body.innerHTML;" & _
        "document.body.innerHTML = headstr+newstr+footstr;" & _
        "window.print();" & _
        "document.body.innerHTML = oldstr;"
    End Function

    Public Function getRol() As String
        Return getRol(User.Identity.Name)
    End Function



    Public Function getRol(user As String) As String


        Dim sql As String = "SELECT rol " & _
                            "FROM Usuarios " & _
                            "WHERE userName = '" & user & "'"

        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySql.Data.MySqlClient.MySqlConnection(cs)


        Dim ds As Data.DataSet = paginaGenerica.conectarMySql(conn, sql, "usuarios", True)

        If Not IsNothing(conn) Then
            conn.Close()
        End If




        If Not IsNothing(ds.Tables(0).Rows(0).Item("rol")) Then
            Return ds.Tables(0).Rows(0).Item("rol")
        Else
            Return ""
        End If


    End Function

    Public Sub setMenu(ByRef div As HtmlGenericControl)
        Dim inner As String = "<ul id=""menu1"" runat=""server"" visible=""false"">"


       


        Select Case getRol()
            Case "RU000"
                inner &= " <li><a href=""envio.aspx"">Importar</a></li>"
                inner &= " <li><a href=""verSeguimiento.aspx"">Seguimiento</a></li>"
                inner &= " <li><a href=""verInicioFin.aspx"">Inicio y Fin de Jornada</a></li>"

                inner &= "<li>"
                inner &= "  <a href=""#"">Ver Ordenes</a>"
                inner &= " <ul class=""children"" > "
                inner &= " <li><a href=""verOrdenesPorEmpleado.aspx"">Por Técnico</a></li>"
                inner &= " <li><a href=""verOrdenesPorEmpleadoFotos.aspx"">Por Técnico con Fotos</a></li>"
                inner &= "  <li><a href=""verOrdenes.aspx"">Por Número de Orden/Cte.</a></li>"
                inner &= "<li><a href=""verResumenEstados.aspx"">Por Estados de la Orden</a></li>"
                inner &= " <li><a href=""verOrdenesporCalle.aspx"">Por Calle</a></li>"
                inner &= " </ul>"
                inner &= "</li>"

                inner &= "<li>"
                inner &= "<a href=""#"">Catálogos</a>"
                inner &= "<ul class=""children"" > "
                inner &= "<li><a href=""empleadosABC.aspx"">Técnicos</a></li>"
                inner &= " </ul>"
                inner &= " </li>"
                inner &= " <li><a href=""erroresComunes.aspx"">Errores Comunes</a></li>"
               
            Case "RU001"
                inner &= " <li><a href=""envio.aspx"">Importar</a></li>"
                inner &= " <li><a href=""verSeguimiento.aspx"">Seguimiento</a></li>"
                inner &= " <li><a href=""verInicioFin.aspx"">Inicio y Fin de Jornada</a></li>"

                inner &= "<li>"
                inner &= "  <a href=""#"">Ver Ordenes</a>"
                inner &= " <ul class=""children"" > "
                inner &= " <li><a href=""verOrdenesPorEmpleado.aspx"">Por Técnico</a></li>"
                inner &= " <li><a href=""verOrdenesPorEmpleadoFotos.aspx"">Por Técnico con Fotos</a></li>"
                inner &= "  <li><a href=""verOrdenes.aspx"">Por Número de Orden/Cte.</a></li>"
                inner &= "<li><a href=""verResumenEstados.aspx"">Por Estados de la Orden</a></li>"
                inner &= " <li><a href=""verOrdenesporCalle.aspx"">Por Calle</a></li>"
                inner &= " </ul>"
                inner &= "</li>"

                inner &= "<li>"
                inner &= "<a href=""#"">Catálogos</a>"
                inner &= "<ul class=""children"" > "
                inner &= "<li><a href=""empleadosABC.aspx"">Técnicos</a></li>"
                inner &= " </ul>"
                inner &= " </li>"
                inner &= " <li><a href=""erroresComunes.aspx"">Errores Comunes</a></li>"
            Case "RU002"
                inner &= " <li><a href=""verSeguimiento.aspx"">Seguimiento</a></li>"
                inner &= " <li><a href=""verInicioFin.aspx"">Inicio y Fin de Jornada</a></li>"

                inner &= "<li>"
                inner &= "  <a href=""#"">Ver Ordenes</a>"
                inner &= " <ul class=""children"" > "
                inner &= " <li><a href=""verOrdenesPorEmpleado.aspx"">Por Técnico</a></li>"
                inner &= " <li><a href=""verOrdenesPorEmpleadoFotos.aspx"">Por Técnico con Fotos</a></li>"
                inner &= "  <li><a href=""verOrdenes.aspx"">Por Número de Orden/Cte.</a></li>"
                inner &= "<li><a href=""verResumenEstados.aspx"">Por Estados de la Orden</a></li>"
                inner &= " <li><a href=""verOrdenesporCalle.aspx"">Por Calle</a></li>"
                inner &= " </ul>"
                inner &= "</li>"

                inner &= "<li>"
                inner &= "<a href=""#"">Catálogos</a>"
                inner &= "<ul class=""children"" > "
                inner &= "<li><a href=""empleadosABC.aspx"">Técnicos</a></li>"
                inner &= " </ul>"
                inner &= " </li>"
                inner &= " <li><a href=""erroresComunes.aspx"">Errores Comunes</a></li>"
            Case "RU003"

                inner &= " <li><a href=""envio.aspx"">Importar</a></li>"
        End Select

        inner &= "<li><a href=""logIn.aspx/?log=1"">Salir</a></li>"
        inner &= " </ul>"

        div.InnerHtml = inner
    End Sub


End Class
