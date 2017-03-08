Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports MySql.Data.MySqlClient
Imports Ionic.Zip
Imports paginaGenerica
Imports System.Security.AccessControl
Imports System.Security.Principal

Partial Class _Default
    Inherits paginaGenerica

    Public ColumnaInicio As Integer = 0
    Public filaInicio As Integer = 0
    Private tiposDeDato As Integer()
    Private municipios As String()


    Private ReadOnly PR_Municipio As Integer = 0
    Private ReadOnly PR_Colonia As Integer = 1
    Private ReadOnly PR_calle As Integer = 2

    Dim filename, savedFile As String

    Dim index As Integer = -1

    Dim oledbConn As OleDbConnection


    Dim FotosAnom As Integer = 0
    Dim FotosLectura As Integer = 0


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            setMenu(contenedor)
            Dim cookie As HttpCookie = Request.Cookies("verHistorico")
            If getRol.Equals("RU000") Then
                Title() = "Acciones de SU"
            ElseIf getRol.Equals("RU004") Then
                Button2.Visible = False
            Else
                contenido.Visible = False
                mensaje_generico("No tiene permisos para ver esta pagina")
                Return
            End If
            If cookie Is Nothing Then
                'cookie = New HttpCookie("verOrdenesPorEmpleado")
                txtfecha.Text = getFecha("yyyyMMdd")
            Else
                txtfecha.Text = cookie("dia")
            End If

        End If

    End Sub

    Public Sub cargaArchivo()
        Using zip As New ZipFile()
            Dim extension As String = ".jpg"
            zip.AlternateEncodingUsage = ZipOption.AsNecessary
            'zip.AddDirectoryByName("fotos")
            Dim servidor As String = subirDirectorios(Server.MapPath("~/"), 2) & "AppsTampico\"
            'Empezamos abriendo el archivo y vamos a leer registro por registro
            'servidor = "D:\Proyectos de SISTOLE\SISTOLE COMAPA conurvada\SISTOLE COMAPA conurvada\"
            Dim ls_directorio As String = servidor & "lecturaEntrada\activos"
            Dim ls_directorio_fotos As String = servidor & "fotos\" & txtfecha.Text & "\"
            Dim reader As StreamReader
            'validamos que el csv exista
            Dim filePath As String = ls_directorio & "\" & txtfecha.Text & "\" & txtfecha.Text & ".CSV"
            'generamos el csv


            If Not System.IO.File.Exists(filePath) Then
                'no existe
                Try
                    generaCSV()
                Catch ex As Exception
                    mensaje_generico(ex.Message)
                    Return
                End Try
            End If
            reader = File.OpenText(ls_directorio & "\" & txtfecha.Text & ".hist")
            Dim tablaLecturas As Hashtable = New Hashtable

            While (Not reader.EndOfStream)

                Dim ls_linea As String = reader.ReadLine()
                Dim poliza = ls_linea.Substring(0, 8).Trim()
                Dim medidor = ls_linea.Substring(193, 10).Trim()

                tablaLecturas(poliza) = medidor

            End While
            reader.Close()

            'ok ahora el archivo csv
            reader = File.OpenText(filePath)

            Dim almenosUnRegistro As Boolean = False


            While (Not reader.EndOfStream)
                Dim ll_fotosLect As Integer = 0
                Dim ll_fotosAnom As Integer = 0
                Dim lect As lectura
                Try
                    lect = New lectura(reader.ReadLine())
                Catch ex As Exception
                    Continue While
                End Try

                Dim medidor As String = tablaLecturas(Convert.ToInt32(lect.getPoliza).ToString)

                Dim esElPrimero As Boolean = True


                If Not (IsNothing(medidor)) Then
                    If Not (medidor.PadLeft(9, "0") = "000000000") Then

                        Dim fotos As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(ls_directorio_fotos, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*" & medidor.PadLeft(9, "0") & "*.jpg")
                        For Each archivo As String In fotos

                            almenosUnRegistro = True
                            'If esElPrimero Then

                            Dim temp As String = ""
                            'If Not lect.getAnomalia.Equals("0") Then
                            '    row.Item("anomalia") = lect.getAnomalia
                            '    ll_fotosAnom += 1
                            '    FotosAnom += 1
                            'Else
                            '    row.Item("lectura") = lect.getLectura
                            '    ll_fotosLect += 1
                            '    FotosLectura += 1
                            'End If


                            Dim nombreGenerico = lect.getPoliza.PadLeft(10, "0") & "-" & medidor.PadLeft(20, "0") & "-" & archivo.Substring(archivo.Length - 18)

                            zip.AddFile(archivo, "fotos").FileName = nombreGenerico

                            'row.Item("lectura") = temp

                            'row.Item("anomalia") = lect.getAnomalia
                            'row.Item("poliza") = lect.getPoliza
                            'row.Item("idEmpleado") = lect.getPtn
                            'row.Item("comentario") = lect.getComentarios
                            'row.Item("medidor") = medidor

                            'End If





                        Next
                    End If
                End If



                

            End While

            If Not almenosUnRegistro Then
                mensaje_generico("No hay fotos que descargar")
                Return
            End If

            Response.Clear()
            Response.BufferOutput = False
            'Dim zipName As String = [String].Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))
            Dim zipName As String = [String].Format("Fotos_{0}.zip", txtfecha.Text)
            Response.ContentType = "application/zip"
            Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
            zip.Save(Response.OutputStream)
            Response.[End]()

        End Using
    End Sub

    Public Sub cargaArchivoNuevo()
        Using zip As New ZipFile()
            Dim extension As String = ".jpg"
            zip.AlternateEncodingUsage = ZipOption.AsNecessary
            'zip.AddDirectoryByName("fotos")
            Dim servidor As String = Server.MapPath("~/")
            'Empezamos abriendo el archivo y vamos a leer registro por registro
            Dim ls_directorio As String = servidor & "lecturaEntrada\activos"
            Dim ls_directorio_fotos As String = servidor & "fotos\"
            Dim reader As StreamReader
            'validamos que el csv exista
            'Dim filePath As String = ls_directorio & "\" & txtfecha.Text & "\" & txtfecha.Text & ".CSV"
            'generamos el csv


            Dim almenosUnRegistro As Boolean = False


            

                Dim esElPrimero As Boolean = True


             

            Dim fotos As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(ls_directorio_fotos, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*" & txtfecha.Text & "*.jpg")
                        For Each archivo As String In fotos

                            almenosUnRegistro = True
                            'If esElPrimero Then

                            Dim temp As String = ""
                            'If Not lect.getAnomalia.Equals("0") Then
                            '    row.Item("anomalia") = lect.getAnomalia
                            '    ll_fotosAnom += 1
                            '    FotosAnom += 1
                            'Else
                            '    row.Item("lectura") = lect.getLectura
                            '    ll_fotosLect += 1
                            '    FotosLectura += 1
                            'End If


                'Dim nombreGenerico = lect.getPoliza.PadLeft(10, "0") & "-" & medidor.PadLeft(20, "0") & "-" & archivo.Substring(archivo.Length - 18)

                zip.AddFile(archivo).FileName = archivo.Substring(archivo.LastIndexOf("\"))

                            'row.Item("lectura") = temp

                            'row.Item("anomalia") = lect.getAnomalia
                            'row.Item("poliza") = lect.getPoliza
                            'row.Item("idEmpleado") = lect.getPtn
                            'row.Item("comentario") = lect.getComentarios
                            'row.Item("medidor") = medidor

                            'End If





                        Next





            If Not almenosUnRegistro Then
                mensaje_generico("No hay fotos que descargar")
                Return
            End If

            Response.Clear()
            Response.BufferOutput = False
            'Dim zipName As String = [String].Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))
            Dim zipName As String = [String].Format("Fotos_{0}.zip", txtfecha.Text)
            Response.ContentType = "application/zip"
            Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
            zip.Save(Response.OutputStream)
            Response.[End]()

        End Using
    End Sub

   

    Public Sub mensaje_generico(ls_mensaje As String)
        lbldialogo.Text = ls_mensaje
        mp1.Show()

    End Sub



    Protected Sub generaCSV()

        Dim extension As String = ".TPL"
        Dim ls_extension As String = "CSV"
        Dim reader As StreamReader = Nothing
        Dim ls_linea As String
        Dim servidor As String = subirDirectorios(Server.MapPath("~/"), 2) & "AppsTampico\"
        'Empezamos abriendo el archivo y vamos a leer registro por registro
        'servidor = "D:\Proyectos de SISTOLE\SISTOLE COMAPA conurvada\SISTOLE COMAPA conurvada\"

        Dim ls_directorio As String = servidor & "lecturaEntrada\activos\" & txtfecha.Text & "\"
        Dim ls_directorio_carga As String = servidor & "lecturaSalida\activos\"
        Dim ls_directorio_fotos As String = servidor & "fotos\" & txtfecha.Text & "\"

        'zip.AddDirectoryByName("FOTOS")



        'Primero quiero el query que ya habiamos realizado

        'Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        'Dim conn As New MySqlConnection(cs)
        Dim hayArchivos As Boolean = False
        Try
            Dim dinf As New DirectoryInfo(ls_directorio)

            'dinf.GetFiles()
            'Dim ds As DataSet = conectarMySql(conn, selectSQL, "ordenes", True)

            Dim primerRegistro As Boolean = True

            For Each recivedFile As FileInfo In dinf.GetFiles()
                'por cada archivo hay que abrirlo
                Dim foundFile As String = recivedFile.Name
                If Not (recivedFile.Extension.EndsWith("TPL") Or recivedFile.Extension.EndsWith("tpl")) Then
                    
                    Continue For
                End If


                reader = File.OpenText(ls_directorio & foundFile)



                While (Not reader.EndOfStream)
                    'Hay que leerlo linea por linea
                    ls_linea = reader.ReadLine()

                    Dim tipoLectura As String = ls_linea.Substring(105, 1).Trim()
                    If tipoLectura.Equals("") Then
                        Continue While
                    End If

                    Dim poliza As String = ls_linea.Substring(0, 8).Trim()
                    Dim lectura As String = ls_linea.Substring(8, 8).Trim()
                    Dim fecha As String = ls_linea.Substring(16, 16).Trim()
                    Dim anomalia As String = ls_linea.Substring(32, 3).Trim()
                    Dim comentarios As String = ls_linea.Substring(35, 60).Trim()
                    Dim ptn As String = "833359" + ls_linea.Substring(95, 10).Trim()




                    'Ya tenemos los datos que nos interesan asi que hay que hacer una linea para excel separada por tabuladores

                    Dim cadenaAEscribir As String = ""
                    If Not lectura.Trim.Equals("") Then
                        cadenaAEscribir = "E:"
                        cadenaAEscribir &= poliza.PadLeft(8, "0") & lectura.PadLeft(10, "0") & "0" & ptn & fecha.Substring(3, 2) & fecha.Substring(0, 2) & _
                            fecha.Substring(8, 2) & "15" & fecha.Substring(10, 6) & "10"

                        escribeArchivoGen(txtFecha.Text, cadenaAEscribir, "lecturaEntrada\activos\" & txtFecha.Text & "\", ls_extension, Not primerRegistro)
                        primerRegistro = False 'Esta variable me ayuda para borrar lo que se haya escrito antes

                    End If

                    If Not anomalia.Trim.Equals("") Then
                        cadenaAEscribir = "E:"
                        'Dim traduccion As String = ""

                        'Select Case Convert.ToInt32(anomalia.Trim)
                        '    Case 1
                        '        traduccion = "A"
                        '    Case 2
                        '        traduccion = "B"
                        '    Case 3
                        '        traduccion = "C"
                        '    Case 4
                        '        traduccion = "D"
                        '    Case 5
                        '        traduccion = "E"
                        '    Case 6
                        '        traduccion = "F"
                        '    Case 7
                        '        traduccion = "G"
                        '    Case 8
                        '        traduccion = "H"
                        '    Case 9
                        '        traduccion = "I"
                        '    Case 10
                        '        traduccion = "J"
                        '    Case 11
                        '        traduccion = "K"
                        '    Case Else
                        '        Continue While


                        'End Select



                        cadenaAEscribir &= poliza.PadLeft(8, "0") & anomalia.PadLeft(10, "0") & "1" & ptn & fecha.Substring(3, 2) & fecha.Substring(0, 2) & _
                            fecha.Substring(8, 2) & "15" & fecha.Substring(10, 6) & "0" & anomalia

                        escribeArchivoGen(txtFecha.Text, cadenaAEscribir, "lecturaEntrada\activos\" & txtFecha.Text & "\", ls_extension, Not primerRegistro)
                        primerRegistro = False 'Esta variable me ayuda para borrar lo que se haya escrito antes

                    End If

                    'Escribimos la nueva cadena en el nuevo archivo con terminacion .txt
                    'escribeArchivoGen(foundFile.Substring(0, foundFile.IndexOf(".")), cadenaAEscribir, "lecturaEntrada\activos\" & getFecha("yyyyMMdd") & "\", ls_extension, Not primerRegistro)


                End While

                reader.Close()


                'Dim ls_numOrden As String = row("numOrden")
                'ls_numOrden = ls_numOrden.PadLeft(obtieneLongitud(conn, "numOrden"), "0")
                'Dim ls_poliza As String = row("poliza")
                'ls_poliza = ls_poliza.PadLeft(obtieneLongitud(conn, "poliza"), "0")
                'Dim nombreGenerico As String = ls_numOrden & "_" & ls_poliza
                ''Buscamos por la foto de antes


            Next

            Dim filePath As String = ls_directorio & txtFecha.Text & ".CSV"
            If System.IO.File.Exists(filePath) Then
                'Agregamos el archivo terminado en un zip

                hayArchivos = True
            End If


            'si no se cargó algun archivo en el zip no tiene caso
            If Not hayArchivos Then
                'mensaje_generico("No hay archivos por descargar " & ls_directorio)
                Throw New Exception("No hay historicos en la fecha indicada")
                Return
            End If

        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(reader) Then
                reader.Close()
            End If
        End Try




    End Sub

    Protected Sub b_decargar0_Click(sender As Object, e As EventArgs) Handles b_decargar0.Click
        guardarFechasEnCookie()
        If Convert.ToInt32(txtfecha.Text) < 20140905 Then
            cargaArchivo()
        Else
            cargaArchivoNuevo()
        End If

    End Sub

    Sub guardarFechasEnCookie()
        Dim cookie As HttpCookie = Request.Cookies("verHistorico")
        If cookie Is Nothing Then
            cookie = New HttpCookie("verHistorico")

        End If

        cookie("dia") = txtfecha.Text

        cookie.Expires = DateTime.Now.AddDays(30)
        Response.Cookies.Add(cookie)
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        guardarFechasEnCookie()
        Try
            eliminaArchivo()
        Catch ex As Exception
            mensaje_generico("Ocurrio un error: " & ex.Message)
        End Try
    End Sub

    Public Sub eliminaArchivo()
            Dim extension As String = ".jpg"
            'zip.AddDirectoryByName("fotos")
            Dim servidor As String = Server.MapPath("~/")
        'Empezamos abriendo el archivo y vamos a leer registro por registro
        If Convert.ToInt32(txtfecha.Text) < 20140905 Then
            servidor = subirDirectorios(Server.MapPath("~/"), 2) & "AppsTampico\"
            'servidor = "C:\Users\Admin\Documents\My Web Sites\SISTOLE COMAPA conurvada\SISTOLE COMAPA conurvada\"
        End If

            Dim ls_directorio As String = servidor & "lecturaEntrada\activos"
            Dim ls_directorio_fotos As String = servidor & "fotos\"
            Dim reader As StreamReader
            'validamos que el csv exista
            'Dim filePath As String = ls_directorio & "\" & txtfecha.Text & "\" & txtfecha.Text & ".CSV"
            'generamos el csv


            Dim almenosUnRegistro As Boolean = False




            Dim esElPrimero As Boolean = True




        Dim fotos As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(ls_directorio_fotos, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*" & txtfecha.Text & "*.jpg")
            For Each archivo As String In fotos

                almenosUnRegistro = True
                'If esElPrimero Then

                Dim temp As String = ""


            System.IO.File.Delete(archivo)


            Next





            If Not almenosUnRegistro Then
            mensaje_generico("No hay fotos que borrar")
                Return
            End If

        mensaje_generico("Archivos Eliminados")

    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Queremos una funcion que elimine todo
        Dim conn As MySqlConnection
        Dim cmd As New MySqlCommand()

        Try
            conn = getConnection()
            cmd.Connection = conn

            Dim instrucciones As List(Of String) = New List(Of String)

            instrucciones.Add("delete from lecturas")
            instrucciones.Add("delete from clienteslect")
            instrucciones.Add("delete from tareaslect")
            instrucciones.Add("delete from archivoslect")
            instrucciones.Add("delete from empleadoslect")
            instrucciones.Add("delete from noregistrados")
            instrucciones.Add("delete from cambiomedidor")

            For Each instruccion As String In instrucciones
                cmd.CommandText = instruccion
                cmd.ExecuteNonQuery()
            Next

            mensaje_generico("Los datos fueron borrados.")


        Catch ex As Exception
            mensaje_generico("Ocurrio el siguiente error al borrar los datos:" & ex.Message)

            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try

    End Sub
End Class
