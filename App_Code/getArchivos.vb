Imports Microsoft.VisualBasic
Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class getArchivos

    Public Sub New(ls_directorio As String)
        Dim ls_empleado As String
        Dim cs As String = ConfigurationManager.ConnectionStrings(paginaGenerica.getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Dim reader As StreamReader
        Dim ls_linea As String
        Try
            conn.Open()
            Dim cmd As New MySqlCommand()
            cmd.Connection = conn

            Dim dinf As New DirectoryInfo(ls_directorio)

            dinf.GetFiles()

            For Each recivedFile As FileInfo In dinf.GetFiles()
                'por cada archivo, obtenemos el empleado
                Dim foundFile As String = recivedFile.Name

                If Not (foundFile.ToUpper.EndsWith("NEW") Or foundFile.ToUpper.EndsWith("TPL")) Then
                    paginaGenerica.moverABackup(ls_directorio, foundFile)
                    Continue For
                End If

                If foundFile.ToUpper.EndsWith("NEW") Then
                    reader = File.OpenText(paginaGenerica.moverABackup(ls_directorio, foundFile))
                    While (Not reader.EndOfStream)
                        Try
                            ls_linea = reader.ReadLine()
                            Dim direccion As String
                            Dim colonia As String
                            Dim medidor As String
                            Dim numEsferas As String
                            Dim poliza As String
                            Dim comentarios As String
                            Dim lectura As String

                            If ls_linea.Contains("CM Poliza=") Then
                                medidor = ls_linea.Substring(80, 20).Trim
                                numEsferas = ls_linea.Substring(100, 2).Trim
                                poliza = ls_linea.Substring(124, 15).Trim

                                Dim ls_update As String = "Insert into cambiomedidor (seriemedidor, esferas,fechaDeIngresoAlSistema, idlectura ) " & _
                                    " values ( '" & medidor & "', " & numEsferas & ", str_to_date('" & paginaGenerica.getFecha("ddMMyyyy") & "', '%d%m%Y') , " & poliza & ")"

                                cmd.CommandText = ls_update
                                cmd.ExecuteNonQuery()
                            Else
                                direccion = ls_linea.Substring(0, 30).Trim
                                colonia = ls_linea.Substring(30, 50).Trim
                                medidor = ls_linea.Substring(80, 20).Trim
                                numEsferas = ls_linea.Substring(100, 2).Trim
                                comentarios = ls_linea.Substring(110, 25).Trim
                                lectura = ls_linea.Substring(102, 8).Trim

                                Dim ls_update As String = "Insert into noregistrados  (calle, colonia, comentarios, lectura, seriemedidor, esferas,fechaDeIngresoAlSistema) " & _
                                    " values ( '" & direccion & "', '" & colonia & "', '" & comentarios & "','" & lectura & "', '" & medidor & "'," & numEsferas & ", str_to_date('" & paginaGenerica.getFecha("ddMMyyyy") & "', '%d%m%Y') )"

                                cmd.CommandText = ls_update
                                cmd.ExecuteNonQuery()

                            End If
                        Catch ex As Exception

                        End Try
                    End While
                    Continue For
                End If
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



                'Leemos hasta que lleguemos al final del archivo
                While (Not reader.EndOfStream)
                    Try


                        ls_linea = reader.ReadLine()
                        Dim actualizar As Boolean = False

                        'dessgrosamos la linea a leer
                        Dim poliza As String = getValueFile(conn, ls_linea, "poliza")
                        Dim lectura As String = getValueFile(conn, ls_linea, "lectura")
                        Dim fecha As String = getValueFile(conn, ls_linea, "fecha")
                        Dim anomalia As String = getValueFile(conn, ls_linea, "anomalia")
                        Dim comentarios As String = getValueFile(conn, ls_linea, "comentarios")
                        'Dim ptn As String = getValueFile(conn, ls_linea, "fechadeejecucion")
                        'Dim sospechosa As String = getValueFile(conn, ls_linea, "sospechosa")
                        Dim tipolectura As String = getValueFile(conn, ls_linea, "tipolectura")
                        Dim latitud As String = getValueFile(conn, ls_linea, "latitud")
                        Dim longitud As String = getValueFile(conn, ls_linea, "longitud")


                        Dim idTarea, idTareaTrigger As String
                        'buscamos la orden en base a su numero de orden
                        Dim sqlSelect As String = "Select * from lecturas where idlectura=" & poliza

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

                            If anomalia.Trim.Equals("'0'") Then
                                anomalia = "''"
                            End If
                            'If actualizar Then
                            'actualizamos las ordenes que lo requieran
                            Dim ls_update As String = "Update lecturas set lectura=" & lectura.Trim & _
                                ", anomalia=" & anomalia.Trim & ", fecha=str_to_date(" & fecha & ", '%d/%m/%Y%H%i%s') " & _
                                ", comentarios=" & comentarios.Trim & ", idTarea=" & idTarea & ", tipolectura=" & tipolectura & _
                                ", latitud=" & latitud & ", longitud=" & longitud & " where idlectura=" & ds.Tables(0).Rows(0).Item("idlectura")

                            cmd.CommandText = ls_update
                            cmd.ExecuteNonQuery()

                            'paginaGenerica.simuloTrigger(ds.Tables(0).Rows(0).Item("idOrden"), ds.Tables(0).Rows(0).Item("estadoDeLaOrden"), estadodelaorden, idTareaTrigger, conn)
                            'End If

                            'Hay que guardar el trayecto



                        End If

                        'Dim actualizada As Integer = 0

                        'If actualizar Then
                        '    actualizada = 1
                        'End If

                        'Dim ls_insert = "Insert into logEstados(numOrden, archivo, fecha, estadoActual, estadoNuevo, Actualizada) values (" & numOrden & _
                        '    ", '" & foundFile & "', '" & paginaGenerica.getFecha("yyyyMMddHHmmss") & "', '" & estadoActual & "', " & estadodelaorden & ", " & actualizada & ") "

                        'cmd.CommandText = ls_insert
                        'cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        'Hubo un error, tal vez se recibió algo que no se debia recibir
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

        Dim sqlSelect As String = "Select posicionEntrada, longitudEntrada, tipodedato from estructuraslect where upper(nombrecampoBD)='" & campo.ToUpper & "'"

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

End Class
