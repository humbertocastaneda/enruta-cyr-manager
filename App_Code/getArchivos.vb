Imports Microsoft.VisualBasic
Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class getArchivos

    Private ReadOnly PR_Municipio As Integer = 0
    Private ReadOnly PR_Colonia As Integer = 1
    Private ReadOnly PR_calle As Integer = 2

    Public Sub New(ls_directorio As String)
        Dim ls_empleado As String
        Dim cs As String = ConfigurationManager.ConnectionStrings(paginaGenerica.getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Dim reader As StreamReader
        Dim debug As String = ""

        Try
            conn.Open()
            Dim cmd As New MySqlCommand()
            cmd.Connection = conn

            Dim dinf As New DirectoryInfo(ls_directorio)

            dinf.GetFiles()

            For Each recivedFile As FileInfo In dinf.GetFiles()
                'por cada archivo, obtenemos el empleado
                Dim foundFile As String = recivedFile.Name
                ls_empleado = foundFile.Substring(0, foundFile.IndexOf("."))

                Try
                    'Abrimos el archivo de texto
                    'El try..catch sirve para especificar que si ocurre un error al abrir el archivo, continue con el siguiente, esto con el caso de que se abran dos navegadores al mismo tiempo
                    'reader = File.OpenText(ls_directorio & foundFile)
                    reader = File.OpenText(paginaGenerica.moverABackup(ls_directorio, foundFile))
                Catch ex As Exception
                    'Debemos continuar con el siguiente archivo en caso de un error en la lectura/movimiento del archivo
                    Continue For
                End Try

                Dim ls_linea As String

                'Leemos hasta que lleguemos al final del archivo
                While (Not reader.EndOfStream)
                    Try


                        ls_linea = reader.ReadLine()
                        Dim actualizar As Boolean = False
                        ls_linea &= "000000"
                        'dessgrosamos la linea a leer
                        Dim numOrden As String = getValueFile(conn, ls_linea, "numOrden")
                        Dim estadodelaorden As String = getValueFile(conn, ls_linea, "estadodelaorden")
                        Dim numMedidor As String = getValueFile(conn, ls_linea, "numMedidor")
                        Dim lectura As String = getValueFile(conn, ls_linea, "lectura")
                        Dim motivo As String = getValueFile(conn, ls_linea, "motivo")
                        Dim fechadeejecucion As String = getValueFile(conn, ls_linea, "fechadeejecucion")
                        Dim sospechosa As String = getValueFile(conn, ls_linea, "sospechosa")
                        Dim comentarios As String = getValueFile(conn, ls_linea, "comentarios")
                        Dim numSello As String = "0"
                        Dim estadoAnterior As String = "null"
                        Dim fechadeInicio As String = getValueFile(conn, ls_linea, "fechadeinicio")
                        Dim fechaderecepcion As String = getValueFile(conn, ls_linea, "fechaderecepcion")
                        Dim fechadetransmision As String = getValueFile(conn, ls_linea, "fechadetransmision")
                        Dim fechadeRecepcionServidor As String = paginaGenerica.getFecha("yyyyMMddHHmmss")
                        Dim latitud As String = getValueFile(conn, ls_linea, "latitud")
                        Dim longitud As String = getValueFile(conn, ls_linea, "longitud")
                        Dim poliza As String = getValueFile(conn, ls_linea, "poliza")
                        Dim habitado As String = getValueFile(conn, ls_linea, "habitado")
                        Dim registro As String = getValueFile(conn, ls_linea, "registro")
                        Dim escuadra_exterior As String = getValueFile(conn, ls_linea, "escuadra_exterior")
                        Dim se_puede_limitar As String = getValueFile(conn, ls_linea, "se_puede_limitar")
                        Dim serie_coincide As String = getValueFile(conn, ls_linea, "serie_coincide")
                        Dim tiene_medidor As String = getValueFile(conn, ls_linea, "tiene_medidor")
                        Dim domicilio_correcto As String = getValueFile(conn, ls_linea, "domicilio_correcto")

                        If (Not motivo.Equals("''")) Then
                            motivo = "'AV" & motivo.Substring(1).PadLeft(4, "0")
                        End If


                        If numOrden.Equals("0") Then
                            Dim ds2 As DataSet
                            Dim SelectSQL = "Select idEmpleado from empleados where nomina=" & ls_empleado
                            ds2 = paginaGenerica.conectarMySql(conn, SelectSQL, "empleados", False)

                            If ds2.Tables(0).Rows.Count = 0 Then
                                Continue While
                            End If

                            'Buscamos si el IC Existe

                            SelectSQL = "Select idCliente from clientes where poliza=" & poliza
                            Dim ls_insert As String

                            Dim idMunicipio As Integer = getID(conn, PR_Municipio, "''", Nothing)
                            Dim idColonia As Integer = getID(conn, PR_Colonia, "''", idMunicipio)
                            Dim idCalle As Integer = getID(conn, PR_calle, "''", idColonia)

                            Dim dsCliente As DataSet = paginaGenerica.conectarMySql(conn, SelectSQL, "clientes", False)

                            Dim idCliente As Integer


                            If dsCliente.Tables(0).Rows.Count = 0 Then
                                'si no existe el cliente, lo agregamos
                                ls_insert = "insert into clientes (idCalle, nombre, poliza, numInterior, entreCalles, comoLlegar ) " & _
                                    " values(" & idCalle & ", '" & "'" & ", " & poliza & ", '" & "'" & ", '" & _
                                    "'" & ", '" & "'" & ")"
                                cmd.CommandText = ls_insert
                                cmd.ExecuteNonQuery()

                                idCliente = paginaGenerica.getLastInsertedId(conn, False)
                            Else

                                idCliente = dsCliente.Tables(0).Rows(0).Item("idCliente")

                                'End If
                            End If

                            ls_insert = "insert into ordenes(numOrden, poliza, numMedidor, fechadeRecepcion, idCliente, tipodeorden,  idArchivo, status, sinRegistro) " & _
                   " values (" & numOrden & ", " & poliza & ", " & numMedidor & ", str_to_date('" & paginaGenerica.getFecha("yyyyMMddHHmmss") & "', '%Y%m%d%H%i%s'), " & idCliente & ", " & "'TO002'" & ", -1, 0, 1)"

                            debug &= ls_insert & ";    Primer insert"
                            cmd.CommandText = ls_insert
                            cmd.ExecuteNonQuery()

                            Try
                                'Asignamos si acaso la orden tiene un empleado valido
                                'Obtenemos el ultimo numero de orden asig


                                Dim idOrden As Integer
                                idOrden = paginaGenerica.getLastInsertedId(conn, False)
                                numOrden = idOrden
                                'Buscamos el tecnico


                                Dim idEmpleado As Integer
                                Dim ptn As String = ls_empleado


                                SelectSQL = "Select idEmpleado from empleados where nomina=" & ptn
                                ds2 = paginaGenerica.conectarMySql(conn, SelectSQL, "empleados", False)

                                If ds2.Tables(0).Rows.Count > 0 Then
                                    idEmpleado = ds2.Tables(0).Rows(0).Item("idEmpleado")

                                    paginaGenerica.asignaDesasigna(idEmpleado, idOrden, "EO001", conn, paginaGenerica.getFecha("yyyyMMdd"))
                                    'Else
                                    'ls_insert = "Insert into empleados (Nombre, alias, nomina, apmat, appat) values (" & ptn & ", " & ptn & ", " & ptn & ", '', '')"
                                    'cmd.CommandText = ls_insert
                                    'cmd.ExecuteNonQuery()

                                    'SelectSQL = "Select idEmpleado from empleados where nomina=" & ptn
                                    'ds2 = paginaGenerica.conectarMySql(conn, SelectSQL, "empleados", False)

                                    'If ds2.Tables(0).Rows.Count > 0 Then
                                    '    idEmpleado = ds2.Tables(0).Rows(0).Item("idEmpleado")

                                    '    paginaGenerica.asignaDesasigna(idEmpleado, idOrden, "EO001", conn)
                                    'End If


                                End If

                            Catch ex As Exception

                            End Try

                        End If


                        Dim idTarea, idTareaTrigger As String
                        'buscamos la orden en base a su numero de orden
                        Dim sqlSelect As String = "Select * from ordenes where idOrden=" & numOrden

                        Dim ds As DataSet = paginaGenerica.conectarMySql(conn, sqlSelect, "ordenes", False)

                        If IsDBNull(ds.Tables(0).Rows(0).Item("idTarea")) Then
                            Continue While
                        Else
                            idTarea = ds.Tables(0).Rows(0).Item("idTarea")
                            idTareaTrigger = idTarea
                        End If

                        Dim estadoActual As String = ""

                        'hay que verificar que la orden exista
                        If ds.Tables(0).Rows.Count > 0 Then
                            estadoActual = ds.Tables(0).Rows(0).Item("estadodelaorden")
                            Select Case estadoActual
                                Case "EO000"
                                    Select Case estadodelaorden.Substring(1, estadodelaorden.Length - 2)
                                        Case "CD002"
                                            estadodelaorden = "'EO000'"
                                            idTarea = "null"
                                            actualizar = True
                                    End Select
                                Case "EO001" ', "EO002", "EO003", "EO004", "EO005", "EO006", "EO007" No debo cambiar si ya realice!! correccion no existe.
                                    Select Case estadodelaorden.Substring(1, estadodelaorden.Length - 2)
                                        Case "CD000"
                                            estadodelaorden = "'EO010'" 'Caduca, y no puede regresar a otro estado...
                                            'idTarea = "null"
                                            actualizar = True
                                        Case "EO002", "EO003", "EO004", "EO005", "EO006", "EO007", "EO012"
                                            actualizar = True
                                    End Select

                                Case "EO007", "EO005", "EO008"
                                    Select Case estadodelaorden.Substring(1, estadodelaorden.Length - 2)
                                        Case "EO002", "EO004", "EO003"
                                            actualizar = True
                                    End Select
                                    If estadodelaorden.Substring(1, estadodelaorden.Length - 2).Equals("EO007") Then
                                        estadoAnterior = ds.Tables(0).Rows(0).Item("estadoAnterior")
                                    Else

                                        estadoAnterior = ds.Tables(0).Rows(0).Item("estadodelaorden")
                                    End If

                            End Select

                            If actualizar Then
                                'actualizamos las ordenes que lo requieran
                                Dim ls_update As String = "Update ordenes set estadodelaorden=" & estadodelaorden & _
                                    ", motivo=" & motivo & ", lectura=" & lectura & ", sospechosa=" & sospechosa & ", fechadeejecucion=str_to_date(" & fechadeejecucion & ", '%Y%m%d%H%i%s') " & _
                                ", fechaDeTransmisionCelular=str_to_date(" & fechadetransmision & ", '%Y%m%d%H%i%s') " & _
                                ", fechaRecepcionCelular=str_to_date(" & fechaderecepcion & ", '%Y%m%d%H%i%s') " & _
                                ", fechadeinicio=str_to_date(" & fechadeInicio & ", '%Y%m%d%H%i%s') " & _
                                ", fechaCargaOrdenServidor=str_to_date('" & fechadeRecepcionServidor & "', '%Y%m%d%H%i%s') " & _
                                    ", habitado=" & habitado & ", registro=" & registro & ", escuadra_exterior =" & escuadra_exterior & ", se_puede_limitar =" & se_puede_limitar & ", tiene_medidor =" & tiene_medidor & ", serie_coincide =" & serie_coincide & ", domicilio_correcto  =" & domicilio_correcto & _
                                    ", enviada='EE001', comentarios=" & comentarios & ", idTarea=" & idTarea & ", numSello=" & numSello & ", estadoAnterior='" & estadoAnterior & "', latitud=" & latitud & ", longitud=" & longitud & " where idOrden=" & ds.Tables(0).Rows(0).Item("idOrden")
                                debug &= ls_update & ";   segundo insert"
                                cmd.CommandText = ls_update
                                cmd.ExecuteNonQuery()

                                paginaGenerica.simuloTrigger(ds.Tables(0).Rows(0).Item("idOrden"), ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), estadodelaorden, idTareaTrigger, conn)
                            End If

                            'Hay que guardar el trayecto



                        End If

                        Dim actualizada As Integer = 0

                        If actualizar Then
                            actualizada = 1
                        End If

                        'Dim ls_insert = "Insert into logEstados(numOrden, archivo, fecha, estadoActual, estadoNuevo, Actualizada) values (" & numOrden & _
                        '    ", '" & foundFile & "', '" & paginaGenerica.getFecha("yyyyMMddHHmmss") & "', '" & estadoActual & "', " & estadodelaorden & ", " & actualizada & ") "

                        'cmd.CommandText = ls_insert
                        'cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        'Hubo un error, tal vez se recibió algo que no se debia recibir
                        'Throw New Exception(debug)
                    End Try


                End While





            Next
        Catch ex As Exception
            Throw New Exception("Ocurrio este error:" & ex.Message)
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

            If Not reader Is Nothing Then
                reader.Close()
            End If

        End Try


    End Sub


    Function getValueFile(conn As MySqlConnection, linea As String, campo As String) As String

        Dim sqlSelect As String = "Select posicionEntrada, longitudEntrada, tipodedato from estructuras where upper(nombrecampoBD)='" & campo.ToUpper & "'"

        Dim ds As DataSet = paginaGenerica.conectarMySql(conn, sqlSelect, "estructuras", False)

        If ds.Tables(0).Rows.Count = 0 Then
            Throw New Exception("No se encuentra el campo indicado")
        End If

        Dim row As DataRow = ds.Tables(0).Rows.Item(0)

        getValueFile = linea.Substring(row("posicionEntrada"), row("longitudEntrada")).Trim

        If row("tipodedato") = 1 Then
            'es un string, hay que agregarle las comillas
            getValueFile = "'" & getValueFile & "'"
        Else
            If getValueFile = "" Then
                getValueFile = "0"
            End If
        End If


    End Function

    Function getID(conn As MySqlConnection, tipo As Integer, buscar As String, padre As Integer) As Integer
        Dim selectSQL As String
        'Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        'Dim conn As New MySqlConnection(cs)
        Dim ds As DataSet
        Dim row As DataRow

        Select Case tipo
            Case PR_Municipio
                selectSQL = "Select idmunicipio from municipios where nombre=" & buscar
                ds = paginaGenerica.conectarMySql(conn, selectSQL, "municipios", False)

                Dim count As Integer = ds.Tables("municipios").Rows.Count

                If count > 0 Then
                    'Si hay...
                    row = ds.Tables("municipios").Rows.Item(0)

                    Return row("idMunicipio")
                Else
                    'insertamos
                    selectSQL = "insert into municipios(nombre) values(" & buscar & ")"

                    Dim cmd As New MySqlCommand(selectSQL, conn)
                    cmd.ExecuteNonQuery()

                    Return paginaGenerica.getLastInsertedId(conn, False)
                End If


            Case PR_calle

                selectSQL = "Select idcalle from calles where nombre=" & buscar & " and idcolonia=" & padre
                ds = paginaGenerica.conectarMySql(conn, selectSQL, "calles", False)

                Dim count As Integer
                If (IsNothing(ds.Tables("calles"))) Then
                    count = 0
                Else
                    count = ds.Tables("calles").Rows.Count
                End If

                If count > 0 Then
                    'Si hay...
                    row = ds.Tables("calles").Rows.Item(0)

                    Return row("idCalle")
                Else
                    'insertamos
                    selectSQL = "insert into calles(nombre, idcolonia) values(" & buscar & ", " & padre & ")"

                    Dim cmd As New MySqlCommand(selectSQL, conn)
                    cmd.ExecuteNonQuery()

                    Return paginaGenerica.getLastInsertedId(conn, False)
                End If


            Case PR_Colonia
                selectSQL = "Select idcolonia from colonias where nombre=" & buscar & " and idmunicipio=" & padre
                ds = paginaGenerica.conectarMySql(conn, selectSQL, "colonias", False)

                Dim count As Integer

                If (IsNothing(ds.Tables("colonias"))) Then
                    count = 0
                Else
                    count = ds.Tables("colonias").Rows.Count
                End If

                If count > 0 Then
                    'Si hay...
                    row = ds.Tables("colonias").Rows.Item(0)

                    Return row("idColonia")
                Else
                    'insertamos
                    selectSQL = "insert into colonias(nombre, idMunicipio) values(" & buscar & ", " & padre & ")"

                    Dim cmd As New MySqlCommand(selectSQL, conn)
                    cmd.ExecuteNonQuery()

                    Return paginaGenerica.getLastInsertedId(conn, False)
                End If
            Case Else
                Throw New Exception("Funcion GetId(): No existe el dato")

        End Select
    End Function

End Class
