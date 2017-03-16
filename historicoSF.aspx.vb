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
            Dim cookie As HttpCookie = Request.Cookies("verHistorico")

            If cookie Is Nothing Then
                'cookie = New HttpCookie("verOrdenesPorEmpleado")
                txtfecha.Text = getFecha("yyyyMMdd")
            Else
                txtfecha.Text = cookie("dia")
            End If

            Dim cookie2 As HttpCookie = Request.Cookies("verOrdenesPorEmpleado")



            If cookie2 Is Nothing Then
                'cookie = New HttpCookie("verOrdenesPorEmpleado")
                dia.Text = getFecha("yyyyMMdd")
                dia2.Text = getFecha("yyyyMMdd")
            Else
                dia.Text = cookie2("dia")
                dia2.Text = cookie2("dia2")
            End If

        End If

    End Sub

    'Public Sub cargaArchivo()
    '    Using zip As New ZipFile()
    '        Dim extension As String = ".jpg"
    '        zip.AlternateEncodingUsage = ZipOption.AsNecessary
    '        'zip.AddDirectoryByName("fotos")
    '        Dim servidor As String = subirDirectorios(Server.MapPath("~/"), 2) & "AppsTampico\"
    '        'Empezamos abriendo el archivo y vamos a leer registro por registro
    '        'servidor = "D:\Proyectos de SISTOLE\SISTOLE COMAPA conurvada\SISTOLE COMAPA conurvada\"
    '        Dim ls_directorio As String = servidor & "lecturaEntrada\activos"
    '        Dim ls_directorio_fotos As String = servidor & "fotos\" & txtfecha.Text & "\"
    '        Dim reader As StreamReader
    '        'validamos que el csv exista
    '        Dim filePath As String = ls_directorio & "\" & txtfecha.Text & "\" & txtfecha.Text & ".CSV"
    '        'generamos el csv


    '        If Not System.IO.File.Exists(filePath) Then
    '            'no existe
    '            Try
    '                generaCSV()
    '            Catch ex As Exception
    '                mensaje_generico(ex.Message)
    '                Return
    '            End Try
    '        End If
    '        reader = File.OpenText(ls_directorio & "\" & txtfecha.Text & ".hist")
    '        Dim tablaLecturas As Hashtable = New Hashtable

    '        While (Not reader.EndOfStream)

    '            Dim ls_linea As String = reader.ReadLine()
    '            Dim poliza = ls_linea.Substring(0, 8).Trim()
    '            Dim medidor = ls_linea.Substring(193, 10).Trim()

    '            tablaLecturas(poliza) = medidor

    '        End While
    '        reader.Close()

    '        'ok ahora el archivo csv
    '        reader = File.OpenText(filePath)

    '        Dim almenosUnRegistro As Boolean = False


    '        While (Not reader.EndOfStream)
    '            Dim ll_fotosLect As Integer = 0
    '            Dim ll_fotosAnom As Integer = 0
    '            Dim lect As lectura
    '            Try
    '                lect = New lectura(reader.ReadLine())
    '            Catch ex As Exception
    '                Continue While
    '            End Try

    '            Dim medidor As String = tablaLecturas(Convert.ToInt32(lect.getPoliza).ToString)

    '            Dim esElPrimero As Boolean = True


    '            If Not (IsNothing(medidor)) Then
    '                If Not (medidor.PadLeft(9, "0") = "000000000") Then

    '                    Dim fotos As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(ls_directorio_fotos, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*" & medidor.PadLeft(9, "0") & "*.jpg")
    '                    For Each archivo As String In fotos

    '                        almenosUnRegistro = True
    '                        'If esElPrimero Then

    '                        Dim temp As String = ""
    '                        'If Not lect.getAnomalia.Equals("0") Then
    '                        '    row.Item("anomalia") = lect.getAnomalia
    '                        '    ll_fotosAnom += 1
    '                        '    FotosAnom += 1
    '                        'Else
    '                        '    row.Item("lectura") = lect.getLectura
    '                        '    ll_fotosLect += 1
    '                        '    FotosLectura += 1
    '                        'End If


    '                        Dim nombreGenerico = lect.getPoliza.PadLeft(10, "0") & "-" & medidor.PadLeft(20, "0") & "-" & archivo.Substring(archivo.Length - 18)

    '                        zip.AddFile(archivo, "fotos").FileName = nombreGenerico

    '                        'row.Item("lectura") = temp

    '                        'row.Item("anomalia") = lect.getAnomalia
    '                        'row.Item("poliza") = lect.getPoliza
    '                        'row.Item("idEmpleado") = lect.getPtn
    '                        'row.Item("comentario") = lect.getComentarios
    '                        'row.Item("medidor") = medidor

    '                        'End If





    '                    Next
    '                End If
    '            End If





    '        End While

    '        If Not almenosUnRegistro Then
    '            mensaje_generico("No hay fotos que descargar")
    '            Return
    '        End If

    '        Response.Clear()
    '        Response.BufferOutput = False
    '        'Dim zipName As String = [String].Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))
    '        Dim zipName As String = [String].Format("Fotos_{0}.zip", txtfecha.Text)
    '        Response.ContentType = "application/zip"
    '        Response.AddHeader("content-disposition", "attachment; filename=" + zipName)
    '        zip.Save(Response.OutputStream)
    '        Response.[End]()

    '    End Using
    'End Sub

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
        'If Convert.ToInt32(txtfecha.Text) < 20140905 Then
        '    cargaArchivo()
        'Else
        cargaArchivoNuevo()
        'End If

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

        Dim ls_directorio As String = servidor & "lecturaEntrada\activos"
        Dim ls_directorio_fotos As String = servidor & "fotos\"


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


    Protected Sub bSap_Click(sender As Object, e As EventArgs) Handles bSap.Click
        guardarFechasEnCookie2()
        descargarOrdenesConFormatoBlanca()
    End Sub


    Public Sub descargarOrdenesConFormatoBlanca()

        'Vamos a generar cadena por cadena de los rangos de fecha proporcionados
        Dim selectSQL As String
        Dim porUsuario As String = ""

        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)



        'If ck_PorUsuario.Checked Then
        '    porUsuario = " and  tar.idEmpleado=" & gv_tecnicos.SelectedRow.Cells(1).Text
        '    dgOrdenes.Columns(0).Visible = False
        'Else
        'End If

        'If ck_filtrarVerificadas.Checked Then
        '    porUsuario &= " and ord.estadoDeLaRevision<>'ER000' "
        'End If

        'If (rbTipoDeBusqueda.SelectedValue = 0) Then 'Todas las que no se han empezado
        '    selectSQL = "Select emp.nomina, ord.estadoDeLaRevision,  if(isnull(emp.alias) or LENGTH(emp.alias)=0, CONCAT_WS(' ',emp.nombre,  emp.appat, emp.apmat), emp.alias) tecnico,   cli.poliza poliza,  if (isnull(ord.estadoAnterior), '',ord.estadoAnterior) estadoAnterior , ord.idOrden, ord.numOrden numorden, ord.descOrden tipodeOrden, est.nombre estadodelaorden ,ord.numMedidor, ord.lectura, mot.codigo motivo , ord.fechadeejecucion fechadeejecucion, fechaderecepcion, ord.comentarios, ord.numSello " & _
        '                          " from ordenes ord left outer join  tareas tar on ( tar.idtarea=ord.idtarea) left outer join codigos mot on (ord.motivo=mot.codigo), empleados emp,  codigos est, clientes cli " & _
        '                          "where 1=1 " & porUsuario & " and est.codigo=ord.estadodelaorden" & " and ord.estadoDeLaorden='EO001' and cli.idCliente=ord.idCliente and tar.idEmpleado=emp.idEmpleado " & _
        '                            " and ord.estadodelaorden<>'EO001'  and ord.estadodelaorden<>'EO012'  " & _
        '                          " order By ord.tipodeorden, ord.estadodelaorden, ord.motivo, ord.fechadeejecucion "
        '    dgOrdenes.Columns(8).Visible = False 'Ocultamos la fecha de ejecucion
        'Else
        selectSQL = "Select ord.status statusOrden, emp.nomina,  ord.estadoDeLaRevision, if(isnull(emp.alias) or LENGTH(emp.alias)=0, CONCAT_WS(' ',emp.nombre,  emp.appat, emp.apmat), emp.alias) tecnico, cli.poliza poliza, if (isnull(ord.estadoAnterior), '',ord.estadoAnterior) estadoAnterior, ord.idOrden, ord.idOrden numorden, ord.descOrden tipodeOrden, est.nombre estadodelaorden ,ord.numMedidor, ord.lectura, mot.codigo motivo , ord.fechadeejecucion fechadeejecucion, fechaderecepcion, ord.comentarios, ord.numSello, ord.habitado " & _
                              " from ordenes ord left outer join  tareas tar on ( tar.idtarea=ord.idtarea) left outer join codigos mot on (ord.motivo=mot.codigo), codigos est,  empleados emp , clientes cli " & _
                              "where 1=1  and cli.idCliente=ord.idCliente and tar.idEmpleado=emp.idEmpleado " & _
                                " and est.codigo=ord.estadodelaorden" & " and tar.fechadeasignacion between STR_TO_DATE('" & dia.Text & "', '%Y%m%d%') and " & " STR_TO_DATE('" & dia2.Text & "', '%Y%m%d%')"


        selectSQL &= " and ord.estadodelaorden<>'EO001'  and ord.estadodelaorden<>'EO012' and ord.estadodelaorden<>'EO005'  "

        selectSQL &= " order By ord.fechadeejecucion"
        'End If

        Dim dsOrdenes As DataSet = conectarMySql(conn, selectSQL, "Ordenes", True)


        Dim nombreArchivo As String = getFecha("yyyyMMddHHmmss")

        Dim sobrescribir As Boolean = True

        If (dsOrdenes.Tables("ordenes").Rows.Count = 0) Then

            mensaje_generico("No hay datos")
            Return
        End If

        For Each row As DataRow In dsOrdenes.Tables("ordenes").Rows
            Dim cadenaAEnviar As String = ""

            cadenaAEnviar = row("numOrden").ToString.PadLeft(12, "0")
            cadenaAEnviar &= row("statusOrden").ToString.PadLeft(12, "0")
            cadenaAEnviar &= row("lectura").ToString.PadLeft(4, "0")
            cadenaAEnviar &= row("poliza").ToString.PadLeft(10, "0")
            Dim motivo As String = row("motivo").ToString
            If motivo.Length >= 5 Then
                motivo = row("motivo").ToString.Substring(2)
            End If
            cadenaAEnviar &= motivo.PadRight(4, "0")

            Dim habitado As String = row("habitado").ToString

            'If motivo.Equals("011") Or motivo.Equals("013") Or motivo.Equals("015") Or motivo.Equals("018") Then
            '    habitado = "0"
            'End If

            cadenaAEnviar &= habitado
            cadenaAEnviar &= row("nomina").ToString.Substring(row("nomina").ToString.Length - 4).PadLeft(4, "0")


            escribeArchivoGen(nombreArchivo, cadenaAEnviar, "uploads", Not sobrescribir)
            sobrescribir = False
        Next

        'Mandamos el archivo en zip
        Dim filePath As String = Server.MapPath("~/") & "uploads\" & nombreArchivo & "." & "txt"

        Dim file As System.IO.FileInfo = New System.IO.FileInfo(filePath)

        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
        Response.AddHeader("Content-Length", file.Length.ToString())
        Response.ContentType = "application/octet-stream"
        Response.WriteFile(file.FullName)
        Response.End() 'if file does not exist

        'Eliminamos el archivo
        System.IO.File.Delete(filePath)
    End Sub

    Protected Sub bInfoAExcel_Click(sender As Object, e As EventArgs) Handles bInfoAExcel.Click

        guardarFechasEnCookie2()
        Dim ls_extension As String = "CSV"
        Dim reader As StreamReader = Nothing
        Dim ls_linea As String

        Dim ls_query As String = ""

        Dim hayArchivos As Boolean = False

        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)
        Try
            ', call.nombre Calle, col.nombre Colonia
            'and call.idCalle=cli.idCalle and col.idColonia=call.idColonia 
            '" from ordenes ord left outer join  tareas tar on ( tar.idtarea=ord.idtarea) left outer join codigos mot on (ord.motivo=mot.codigo), codigos est,  empleados emp , clientes cli left outer join calles cax on (cli.idCalle=cax.idCalle) " & _

            'Primero hay que realizar un query para traer todas las lecturas donde el tipo de lectura sea 0 o (4 con anomalia) del dia que se especifica
            ls_query = "select ord.idOrden, ord.numorden 'Documento de Bloqueo',emp.nomina PTN,ord.status Status, if(isnull(emp.alias) or LENGTH(emp.alias)=0, CONCAT_WS(' ',emp.nombre,  emp.appat, emp.apmat), emp.alias) Tecnico, cli.poliza 'Interlocutor Comercial', ord.numMedidor 'Numero de Medidor', CONCAT('''',RIGHT(CONCAT('0000',ord.lectura),4)) 'Accion de Corte', if (isnull(mot.codigo), '''0000', CONCAT('''',RIGHT(CONCAT('0000',substring(mot.codigo, 3, 3)),4))) 'Anomalia', ord.fechadeejecucion 'Fecha de Ejecucion', ord.comentarios Comentarios, if (isnull(ord.habitado), '0', ord.habitado + 1) Habitado, if (isnull(ord.registro), '0', ord.registro + 1) 'Tiene Registro', ord.vencido Vencido, ord.balance Balance, ord.ultimo_pago 'Monto de Ultimo Pago', ord.fecha_ultimo_pago 'Fecha de Ultimo Pago', cli.nombre Nombre, cax.nombre Calle, cli.numInterior Numero, col.nombre Colonia, cli.entreCalles 'Entre Calles', mun.nombre Municipio, ord.giro Giro, ord.tipo_usuario 'Tipo de Usuario', cli.comoLlegar Sectorizacion " & _
            " from ordenes ord left outer join  tareas tar on ( tar.idtarea=ord.idtarea) left outer join codigos mot on (ord.motivo=mot.codigo), codigos est,  empleados emp , clientes cli , calles cax, colonias col, municipios mun " & _
            " where 1=1  and cli.idCliente=ord.idCliente and cax.idCalle=cli.idCalle and col.idColonia=cax.idColonia and mun.idMunicipio=col.idMunicipio and tar.idEmpleado=emp.idEmpleado  " & _
            " and tar.fechadeasignacion between STR_TO_DATE('" & dia.Text & "', '%Y%m%d%') and " & " STR_TO_DATE('" & dia2.Text & "', '%Y%m%d%') " & _
            " and est.codigo=ord.estadodelaorden   and tar.fechadeasignacion" & _
            " and ord.estadodelaorden<>'EO001'  and ord.estadodelaorden<>'EO012' and ord.estadodelaorden<>'EO005' " & _
            " order By ord.fechadeejecucion  "

            'ls_query = " Select lect.*, nomina, date_format(fecha, '%m%d%y15%h%i%s') fechaEjecucion from lecturas lect, tareaslect tar, empleadoslect emp " & _
            ' " where lect.idTarea = tar.idTarea And emp.idEmpleado = tar.idEmpleado " & _
            '" and (lect.tipoLectura='0'  or (lect.tipoLectura='4' and not( isNull(lect.anomalia) or lect.anomalia='' )))" & _
            ' "and date_format(fechaDeAsignacion, '%Y%m%d')='" & fechaProgram.Text & "'"


            Dim ds As DataSet = conectarMySql(conn, ls_query, "ordenes", True)

            If (ds.Tables("ordenes").Rows.Count = 0) Then

                mensaje_generico("No hay datos")
                Return
            End If

            Dim cadenaAEscribir As String = ""
            Dim gv As GridView = New GridView()
            gv.ID = "gvToExcel"

            gv.DataSource = ds
            gv.DataBind()




            Dim blanco As New Label()
            blanco.Text = ""
            Dim generacion As New Label()
            generacion.Text = getFecha("dd/MM/yyyy HH:mm")
            Dim rango As New Label()
            rango.Text = getFecha("De " & dia.Text.Substring(6, 2) & "/" & dia.Text.Substring(4, 2) & "/" & dia.Text.Substring(0, 4) & " al " _
                                  & dia2.Text.Substring(6, 2) & "/" & dia2.Text.Substring(4, 2) & "/" & dia2.Text.Substring(0, 4))

            Dim dsFinal As DataSet = New DataSet
            dsFinal.Tables.Add()
            dsFinal.Tables(0).Columns.Add(" ")
            dsFinal.Tables(0).Columns.Add("  ")

            Dim row As DataRow = dsFinal.Tables(0).Rows.Add()
            row.Item(0) = "Fecha de Expedicion:"
            row.Item(1) = getFecha("dd/MM/yyyy HH:mm")
            row = dsFinal.Tables(0).Rows.Add()
            row.Item(0) = "Fecha:"
            row.Item(1) = getFecha("De " & dia.Text.Substring(6, 2) & "/" & dia.Text.Substring(4, 2) & "/" & dia.Text.Substring(0, 4) & " al " _
                                  & dia2.Text.Substring(6, 2) & "/" & dia2.Text.Substring(4, 2) & "/" & dia2.Text.Substring(0, 4))

            Dim enca As GridView = New GridView()
            enca.ID = "gvToExcelEnca"

            enca.DataSource = dsFinal
            enca.DataBind()


            Dim sb As StringBuilder = New StringBuilder()
            Dim sw As IO.StringWriter = New IO.StringWriter(sb)
            Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
            Dim pagina As Page = New Page
            Dim form = New HtmlForm
            gv.EnableViewState = False
            pagina.EnableEventValidation = False
            pagina.DesignerInitialize()

            pagina.Controls.Add(form)
            'form.Controls.Add(blanco)
            'form.Controls.Add(generacion)
            'form.Controls.Add(rango)
            form.Controls.Add(enca)
            form.Controls.Add(gv)
            pagina.RenderControl(htw)
            Response.Clear()
            Response.Buffer = True
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment;filename=" & dia.Text & "_" & dia2.Text & ".xls")
            Response.Charset = "UTF-8"
            Response.ContentEncoding = Encoding.Default
            Response.Write(sb.ToString())
            Response.End()

        Catch ex As Exception
            mensaje_generico("Ocurrio un error:" & ex.Message)
        Finally
            If Not IsNothing(conn) Then
                conn.Close()
            End If
        End Try



    End Sub


    Sub guardarFechasEnCookie2()
        Dim cookie As HttpCookie = Request.Cookies("verOrdenesPorEmpleado")
        If cookie Is Nothing Then
            cookie = New HttpCookie("verOrdenesPorEmpleado")

        End If

        cookie("dia") = dia.Text
        cookie("dia2") = dia2.Text

        cookie.Expires = DateTime.Now.AddDays(1)
        Response.Cookies.Add(cookie)
    End Sub

End Class
