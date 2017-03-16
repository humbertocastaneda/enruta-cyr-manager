Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports MySql.Data.MySqlClient
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

    Dim idRow As Integer = 0

    Dim index As Integer = -1

    Dim oledbConn As OleDbConnection

    Dim filename, savedFile As String
    Dim usuario As String = ""


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            setMenu(contenedor)
            actualizaArchivos()
            fechaProgram.Text = getFecha("yyyyMMdd")
            b_cargar.Attributes.Add("onclick", " this.disabled = true; __doPostBack('b_cargar', ''); return false;")
            b_cargar.Attributes.Add("onload", "this.disabled='false';")

            bEnviar.Attributes.Add("onclick", " this.disabled = true; __doPostBack('bEnviar', ''); return false;")
            bEnviar.Attributes.Add("onload", "this.disabled='false';")
        End If

    End Sub

    Protected Sub b_cargar_Click(sender As Object, e As EventArgs) Handles b_cargar.Click

        Dim uploaded As Boolean = False
        'If Not isLoged() Then
        '    messagebox("No estas conectado")
        '    lbl_msj.Text = "No estas conectado."
        '    Return
        'End If


        usuario = User.Identity.Name

        If (FileUploadControl.HasFile) Then
            filename = Path.GetFileName(FileUploadControl.FileName)
            Dim sinError As Boolean = True
            Dim flImages As HttpFileCollection = Request.Files
            For j As Integer = 0 To flImages.Count - 1
                Dim file As HttpPostedFile = flImages(j)
                filename = Path.GetFileName(file.FileName)



                Try

                    If Not (filename.ToUpper.EndsWith("XLSX") Or filename.ToUpper.EndsWith("XLS")) Then
                        mensajeGenerico("El archivo cargado no es un archivo de excel.")
                        Return
                    End If

                    index = guardarArchivoBD(filename)
                    file.SaveAs(Server.MapPath("~/") & "uploads\" & filename)
                    uploaded = True
                    savedFile = filename.Substring(0, filename.IndexOf("."))
                    'Ingresamos en la bd el archivo

                    'index = guardarArchivoBD(filename)

                    readFile(Server.MapPath("~/") & "uploads\" & filename)
                    lbl_msj.Text = "Archivo ha sido cargado."
                    estableceArchivoVisible(index)
                    mensajeGenerico("Los archivos han sido cargados")


                Catch ex As Exception
                    mensajeGenerico("Ocurrio un error: " & ex.Message)
                    sinError = False
                    Exit For

                Finally
                    If uploaded Then


                        Try

                            'Omitimos que se borre el archivo de momento.
                            'System.IO.File.Delete(Server.MapPath("~/") & "uploads\" & filename)
                        Catch ex As Exception
                            ' Throw ex
                        End Try

                    End If
                    filename = ""
                End Try

            Next
            'Try


            '    FileUploadControl.SaveAs(Server.MapPath("~/") & "uploads\" & filename)
            '    uploaded = True
            '    'Ingresamos en la bd el archivo

            '    index = guardarArchivoBD(filename)

            '    readFile(Server.MapPath("~/") & "uploads\" & filename)
            '    lbl_msj.Text = "Archivo ha sido cargado."
            '    estableceArchivoVisible(index)

            '    'messagebox("Archivo ha sido cargado.")

            '    mensajeGenerico("Archivo ha sido cargado.")
            'Catch ex As Exception
            '    'Interaction.MsgBox("Ocurrio un error: " & ex.Message)
            '    'Throw ex
            '    'Borramos el archivo, ya no nos interesa, y su referencia
            '    eliminaArchivo(index, False)
            '    lbl_msj.Text = "Ocurrio un error:" & ex.Message
            '    'messagebox("Ocurrio un error: " & ex.Message)

            '    mensajeGenerico("Ocurrio un error:" & ex.Message)


            'Finally
            '    If uploaded Then

            '        'Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(New System.Uri("http://espinosacarlos.com/uploads/eliminaExcel.php?ruta=" & filename))

            '        'request.Method = System.Net.WebRequestMethods.Http.Get

            '        'Dim response As System.Net.HttpWebResponse = request.GetResponse()

            '        '' process response here

            '        'response.Close()

            '        Try
            '            'Dim dir As New FileInfo(Server.MapPath("~/") & "uploads\" & filename)

            '            'If (dir.Attributes And FileAttributes.ReadOnly) > 0 Then
            '            '    dir.Attributes = dir.Attributes Xor FileAttributes.ReadOnly

            '            'End If

            '            'Dim Level1DirSec As DirectorySecurity = Directory.GetAccessControl(Server.MapPath("~/") & "uploads")

            '            'Level1DirSec.AddAccessRule(New FileSystemAccessRule(New System.Security.Principal.SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, Nothing),
            '            '     FileSystemRights.ReadAndExecute,
            '            '     InheritanceFlags.ContainerInherit + InheritanceFlags.ObjectInherit,
            '            '     PropagationFlags.None,
            '            '     AccessControlType.Allow))

            '            'Directory.SetAccessControl(Server.MapPath("~/") & "uploads\", Level1DirSec)
            '            'Level1DirSec.SetAccessRuleProtection(True, True)

            '            'dir.Delete()

            '            System.IO.File.Delete(Server.MapPath("~/") & "uploads\" & filename)
            '        Catch ex As Exception
            '            'Throw ex
            '        End Try

            '    End If

            'End Try

        Else
            mensajeGenerico("No se ha cargado algun archivo")
        End If

        actualizaArchivos()

    End Sub


    Protected Sub readFile(ls_file As String)
        Dim connString As String

        If ls_file.EndsWith("xls") Then
            connString = ConfigurationManager.ConnectionStrings("xls").ConnectionString
        Else
            connString = ConfigurationManager.ConnectionStrings("xlsx").ConnectionString
        End If

        ' Create the connection object
        oledbConn = New OleDbConnection(connString & "; data source=" & ls_file)

        'obtieneMunicipios()

        'For i As Integer = 0 To municipios.Length - 1
        Try
            ' Open connection
            oledbConn.Open()
            Dim cmd As OleDbCommand
            Dim dtExcelSchema As DataTable = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)


            'Dim sheetName As String = dtExcelSchema.Rows(0)("TABLE_NAME")
            '' Create OleDbCommand object and select data from worksheet Sheet1
            ''Dim cmd As OleDbCommand = New OleDbCommand("SELECT * FROM [Hoja1$]", oledbConn)


            Dim sheetName As String = "" 'dtExcelSchema.Rows(0)("TABLE_NAME")

            For Each row As DataRow In dtExcelSchema.Rows
                sheetName = row("TABLE_NAME").ToString
                If sheetName.Contains("_xlnm#_FilterDatabase") Then
                    Continue For
                Else
                    Exit For
                End If
            Next
            cmd = New OleDbCommand("SELECT * FROM [" & sheetName & "]", oledbConn)

            ' Create new OleDbDataAdapter
            Dim oleda As OleDbDataAdapter = New OleDbDataAdapter()

            oleda.SelectCommand = cmd

            ' Create a DataSet which will hold the data extracted from the worksheet.
            Dim ds As DataSet = New DataSet()

            ' Fill the DataSet from the data extracted from the worksheet.
            oleda.Fill(ds)
            agregaArchivoDeEntrada(ds)

        Catch err As Exception
            Throw err
        Finally
            ' Close connection
            oledbConn.Close()
        End Try
        'Next


    End Sub

    Public Sub agregaArchivoDeEntrada(ds As DataSet)
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)
        Dim ls_insert As String
        'Dim ls_columnas As String
        Dim SelectSQL As String
        Dim row As DataRow
        Dim cmd As New MySqlCommand()

        idRow = 0
        Dim gCliente As String = ""
        Try
            conn.Open()
            cmd.Connection = conn
            'ls_columnas = generaColumnas(conn)

            'If ls_columnas = "" Then
            '    Throw New Exception("No hay columnas")
            'End If


            For Each row In ds.Tables(0).Rows
                idRow += 1
                'Dim valores As String = generaElementosAInsertar(row, ds.Tables(0).Columns.Count)
                'ls_insert = "Insert into ordenes(" & ls_columnas & _
                '") values(" & valores & ")"



                Dim clave_usuario As String = getValueFile(conn, row, "clave_usuario")

                'como puede haber registros vacios necesitamos que los sepa controlar... asi que si el numero de orden esta vacio quiere decir que no es una orden.
                If clave_usuario = "" Or clave_usuario = "0" Then
                    Continue For
                End If

                If idRow = 59 Then
                    Dim a As Integer = 0
                End If

                'Dim poliza As String = getValueFile(conn, row, "poliza")
                Dim cliente As String = getValueFile(conn, row, "cliente")
                Dim tarifa As String = getValueFile(conn, row, "tarifa")
                Dim serieMedidor As String = getValueFile(conn, row, "serieMedidor")
                Dim colonia As String = getValueFile(conn, row, "colonia")
                Dim diametro_toma As String = "'1/2""'" 'getValueFile(conn, row, "diametro_toma")
                Dim giro As String = 3 'getValueFile(conn, row, "giro")
                Dim tu As String = 1 'getValueFile(conn, row, "tu")
                Dim direccion As String = getValueFile(conn, row, "direccion")
                Dim marcaMedidor As String = 14 'getValueFile(conn, row, "marcaMedidor")
                Dim sectorCorto As String = getValueFile(conn, row, "sectorcorto")
                Dim numEsferas As String = 5 'getValueFile(conn, row, "numEsferas")
                Dim economico As String = "''" 'getValueFile(conn, row, "economico")
                Dim poliza As String = getValueFile(conn, row, "poliza")
                Dim sector As String = "0" 'getValueFile(conn, row, "sector")
                Dim lecturaAnterior = getValueFile(conn, row, "lecturaanterior2")
                Dim promedio = getValueFile(conn, row, "promedio")
                Dim cveLectura = "''" 'getValueFile(conn, row, "cveLectura")

                sector = sectorCorto.Substring(1, 4)

                'Dim dsDatosAdicionales As DataSet = getDBDatosRestantes(poliza)

                'Dim porcion As String = ""
                'Dim unidad As String = ""
                'Dim secuencia As String = ""

                ''Por ahora no los tomaremos de ahi... los tomaremos del nuevo
                'If dsDatosAdicionales.Tables(0).Rows.Count > 0 Then
                '    porcion = dsDatosAdicionales.Tables(0).Rows(0).Item(2)
                '    unidad = dsDatosAdicionales.Tables(0).Rows(0).Item(11)
                'End If

                'Try
                '    unidad = row.Item("UNIDAD")
                'Catch ex As Exception

                '    If dsDatosAdicionales.Tables(0).Rows.Count > 0 Then
                '        unidad = dsDatosAdicionales.Tables(0).Rows(0).Item(11)

                '    End If
                'End Try

                'Try
                '    porcion = "GCMG" & row.Item("PORCION").ToString("0000")
                'Catch ex As Exception
                '    'Verificamos como antes
                '    If dsDatosAdicionales.Tables(0).Rows.Count > 0 Then
                '        porcion = dsDatosAdicionales.Tables(0).Rows(0).Item(2)

                '    End If
                'End Try

                'Try
                '    secuencia = row.Item("SECUENCIA")
                'Catch ex As Exception
                '    secuencia = "0"
                'End Try




                gCliente = cliente

                'buscamos por la poliza si existe un cliente
                SelectSQL = "Select idCliente from clienteslect where clave_usuario=" & clave_usuario

                Dim dsCliente As DataSet = conectarMySql(conn, SelectSQL, cliente, False)
                Dim idCliente As Integer

                If dsCliente.Tables(0).Rows.Count = 0 Then
                    'si no existe el cliente, lo agregamos
                    ls_insert = "insert into clienteslect (direccion, nombre,colonia,  poliza, sectorCorto, tarifa, clave_usuario, tu, sector ) " & _
                        " values(" & direccion & ", " & cliente & ", " & colonia & ", " & poliza & ", " & sectorCorto & ", " & _
                        tarifa & ", " & clave_usuario & ", " & tu & ", " & sector & ")"
                    cmd.CommandText = ls_insert
                    cmd.ExecuteNonQuery()

                    idCliente = getLastInsertedId(conn, False)
                Else

                    idCliente = dsCliente.Tables(0).Rows(0).Item("idCliente")

                    Dim ls_update As String = "update clienteslect set  clave_usuario=" & clave_usuario & ", nombre=" & cliente & ", tarifa=" & tarifa & ", tu=" & tu & ", poliza=" & poliza & ", sector=" & sector & " where idCliente=" & dsCliente.Tables(0).Rows(0).Item("idCliente")
                    Try
                        cmd.CommandText = ls_update
                        cmd.ExecuteNonQuery()
                    Catch ex As Exception

                    End Try
                End If






                ls_insert = "insert into lecturas( poliza, serieMedidor, fechadeRecepcion, idCliente,   idArchivo, diametro_toma,marcaMedidor, giro, numEsferas, tipoLectura, cvelectura, promedio, lecturaanterior) " & _
                    " values (" & economico & ", " & serieMedidor & ", str_to_date('" & getFecha("yyyyMMddHHmmss") & "', '%Y%m%d%H%i%s'), " & idCliente & ", " & index & ", " & diametro_toma & ", " & marcaMedidor & _
                        ", " & giro & ", " & numEsferas & ", '', " & cveLectura & ", " & promedio & ", " & lecturaAnterior & " )"

                cmd.CommandText = ls_insert
                cmd.ExecuteNonQuery()

                Try
                    'Asignamos si acaso la orden tiene un empleado valido
                    'Obtenemos el ultimo numero de orden asig
                    SelectSQL = "Select idLectura from lecturas where poliza=" & CStr(economico) & " order by idLectura desc limit 1"
                    ds = conectarMySql(conn, SelectSQL, "lecturas", False)

                    Dim idOrden As Integer
                    idOrden = ds.Tables(0).Rows(0).Item("idLectura")
                    'Buscamos el tecnico


                    Dim idEmpleado As Integer
                    Dim ptn As String = getValueFile(conn, row, "lecturista")

                    SelectSQL = "Select idEmpleado from empleadoslect where nomina=" & ptn
                    ds = conectarMySql(conn, SelectSQL, "empleados", False)

                    If ptn.Trim.Equals("''") Then
                        Continue For
                    End If

                    If ds.Tables(0).Rows.Count > 0 Then
                        idEmpleado = ds.Tables(0).Rows(0).Item("idEmpleado")

                        asignaDesasigna(idEmpleado, idOrden, "EO001", conn, fechaProgram.Text)
                    Else
                        ls_insert = "Insert into empleadoslect (Nombre, alias, nomina, apmat, appat) values (" & ptn & ", " & ptn & ", " & ptn & ", '', '')"
                        cmd.CommandText = ls_insert
                        cmd.ExecuteNonQuery()

                        SelectSQL = "Select idEmpleado from empleadoslect where nomina=" & ptn
                        ds = conectarMySql(conn, SelectSQL, "empleadoslect", False)

                        If ds.Tables(0).Rows.Count > 0 Then
                            idEmpleado = ds.Tables(0).Rows(0).Item("idEmpleado")

                            asignaDesasigna(idEmpleado, idOrden, "EO001", conn, fechaProgram.Text)
                        End If


                    End If

                Catch ex As Exception
                    Throw ex
                End Try




            Next


            ' ds.Tables("[Hoja1$]").Rows.

        Catch ex As Exception
            Throw ex

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
    End Sub

    Public Function generaColumnas(conn As MySqlConnection) As String
        Dim selectSQL As String = "Select nombreCampoBd, tipoDeDato  from estructuraslect where esDeEntrada=1 and not isnull(campo) order by campo"
        Dim cmd As New MySqlCommand(selectSQL, conn)
        'Dim reader As MySqlDataReader
        Dim adapter As New MySqlDataAdapter(cmd)
        Dim pubs As New DataSet()
        Dim resultado As String = ""
        Dim i As Integer = 0

        Try
            'reader = cmd.ExecuteReader


            adapter.Fill(pubs, "estructuraslect")

            Dim row As DataRow
            ReDim tiposDeDato(pubs.Tables("estructuraslect").Rows.Count)

            For Each row In pubs.Tables("estructuraslect").Rows
                If Not resultado = "" Then
                    resultado &= ","
                End If
                resultado &= row("nombreCampoBd")

                tiposDeDato(i) = row("tipodedato")
                i += 1


            Next

            resultado &= ",estadoDeLaOrden"

        Catch ex As Exception
            Throw ex

        End Try

        Return resultado
    End Function

    Public Function generaElementosAInsertar(row As DataRow, columns As Integer) As String
        Dim resultado As String = ""



        columns = columns - 3
        For i As Integer = 0 To columns
            If Not resultado = "" Then
                resultado &= ","
            End If

            If tiposDeDato(i) = 1 Then
                resultado &= "'"
            End If

            resultado &= row(i + 1)

            If tiposDeDato(i) = 1 Then
                resultado &= "'"
            End If
        Next

        resultado &= ", 'TO000'"


        Return resultado

    End Function


    Public Sub obtieneMunicipios()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)
        Dim selectSQL As String = "Select nombre from codigos where codigo like 'MU%'"
        Try
            Dim pubs As DataSet = conectarMySql(conn, selectSQL, "codigos", True)

            ReDim municipios(pubs.Tables("codigos").Rows.Count - 1)

            For i As Integer = 0 To municipios.Length - 1
                Dim row As DataRow
                row = pubs.Tables("codigos").Rows.Item(i)
                municipios(i) = row("nombre")
            Next
        Catch ex As Exception

            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try

    End Sub

     Function parseCodigo(conn As MySqlConnection, desc As String) As String



        Dim selectSQL As String = "Select nombre from codigos where codigo=" & desc.ToUpper
        Dim ds As DataSet
        parseCodigo = "''"



        Try
            ds = conectarMySql(conn, selectSQL, "codigos", False)
            If ds.Tables(0).Rows.Count > 0 Then
                parseCodigo = "'" & ds.Tables(0).Rows(0).Item("nombre") & "'"
            End If

        Catch ex As Exception

        End Try



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
                ds = conectarMySql(conn, selectSQL, "municipios", False)

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

                    Return getLastInsertedId(conn, False)
                End If


            Case PR_calle

                selectSQL = "Select idcalle from calles where nombre=" & buscar & " and idcolonia=" & padre
                ds = conectarMySql(conn, selectSQL, "calles", False)

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

                    Return getLastInsertedId(conn, False)
                End If


            Case PR_Colonia
                selectSQL = "Select idcolonia from colonias where nombre=" & buscar & " and idmunicipio=" & padre
                ds = conectarMySql(conn, selectSQL, "colonias", False)

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

                    Return getLastInsertedId(conn, False)
                End If
            Case Else
                Throw New Exception("Funcion GetId(): No existe el dato")

        End Select
    End Function

    Function getValueFile(conn As MySqlConnection, row As DataRow, campo As String) As String

        Dim sqlSelect As String = "Select campo, tipodedato from estructuraslect where upper(nombrecampoBD)='" & campo.ToUpper & "'"

        Dim ds As DataSet = paginaGenerica.conectarMySql(conn, sqlSelect, "estructuraslect", False)

        If ds.Tables(0).Rows.Count = 0 Then
            Throw New Exception("No se encuentra el campo indicado")
        End If

        Dim rowDB As DataRow = ds.Tables(0).Rows.Item(0)

        getValueFile = IIf(IsDBNull(row(rowDB("campo"))), "", row(rowDB("campo")))

        If rowDB("tipodedato") = 1 Then
            'es un string, hay que agregarle las comillas
            getValueFile = "'" & getValueFile & "'"
        Else
            If getValueFile = "" Then
                getValueFile = "0"
            End If
        End If


    End Function

    Public Function getDBDatosRestantes(poliza As String) As DataSet
        Dim cmd As OleDbCommand = New OleDbCommand("SELECT * FROM [GENERADOS$] where cliente=" & Trim(poliza), oledbConn)


        ' Create new OleDbDataAdapter
        Dim oleda As OleDbDataAdapter = New OleDbDataAdapter()

        oleda.SelectCommand = cmd

        ' Create a DataSet which will hold the data extracted from the worksheet.
        Dim ds As DataSet = New DataSet()

        ' Fill the DataSet from the data extracted from the worksheet.
        oleda.Fill(ds)
        Return ds
    End Function

    Protected Sub gv_tecnicos_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.CssClass = e.Row.RowState.ToString()

            e.Row.Attributes.Add("onclick", String.Format("javascript:__doPostBack('gv_tecnicos','Select${0}')", e.Row.RowIndex))

        End If
    End Sub

    Public Sub actualizaArchivos()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)

        Try
            Dim selectSQL As String = "Select nombre " & _
                ", idArchivo id, fecha from archivoslect  where visible= 1" & _
                " order by fecha desc limit 30 "

            Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "archivos", True)

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

        gv_tecnicos.DataBind()

        If gv_tecnicos.Rows.Count = 0 Then
            bEliminar.Visible = False
        Else
            bEliminar.Visible = True

        End If
    End Sub

    Private Function guardarArchivoBD(filename As String) As Integer

        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)
        Dim cmd As New MySqlCommand()

        guardarArchivoBD = -1
        Try
            cmd.Connection = conn
            Dim selectSQL As String = "Insert into archivoslect(nombre, fecha, usuario) values ('" & filename & "', str_to_date('" & getFecha("yyyyMMddHHmmss") & "', '%Y%m%d%H%i%s'), '" & usuario & "')"

            conn.Open()

            'Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "archivos", True)

            ''lbEmpleados.DataSource = dsEmp
            ''lbEmpleados.DataTextField = "nombreComp"
            ''lbEmpleados.DataValueField = "id"

            'gv_tecnicos.DataSource = dsEmp
            cmd.CommandText = selectSQL

            cmd.ExecuteNonQuery()


            selectSQL = "Select idArchivo canti from archivoslect order by idArchivo desc"
            Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "archivoslect", False)

            guardarArchivoBD = dsEmp.Tables(0).Rows(0).Item("canti")

        Catch ex As Exception

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try
    End Function

    Public Sub eliminaArchivo(index As Integer, mostrarMensaje As Boolean)

        If index < 0 Then
            Return
        End If
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)
        Dim cmd As New MySqlCommand()


        Try

            Dim selectSQL As String = "Select idLectura from lecturas where idArchivo=" & index & " and tipoLectura='' limit 1"

            Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "lecturas", True)

            cmd.Connection = conn

            If dsEmp.Tables(0).Rows.Count = 0 Then
                If mostrarMensaje Then
                    'messagebox("El archivo no puede ser removido debido a que tiene ordenes asignadas o trabajadas")
                    mensajeGenerico("El archivo no puede ser removido debido a que tiene lecturas trabajadas")
                End If
                Return
            End If

            'Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "archivos", True)

            ''lbEmpleados.DataSource = dsEmp
            ''lbEmpleados.DataTextField = "nombreComp"
            ''lbEmpleados.DataValueField = "id"

            'gv_tecnicos.DataSource = dsEmp

            'Hay que borrar las tareas

            selectSQL = "Select idTarea fecha from lecturas where idArchivo=" & index & " group by idTarea"

            dsEmp = conectarMySql(conn, selectSQL, "lecturas", False)

            Dim tareas As String = ""

            ' Obtenemos las tareas que quedaran vacias...
            For Each row As DataRow In dsEmp.Tables("lecturas").Rows
                If Not tareas = "" Then
                    tareas &= ","
                End If
                tareas &= row(0)

            Next




            selectSQL = "delete from lecturas where idArchivo=" & index

            cmd.CommandText = selectSQL

            cmd.ExecuteNonQuery()

            selectSQL = "delete from archivoslect where idArchivo=" & index

            cmd.CommandText = selectSQL

            cmd.ExecuteNonQuery()

            'Actualizamos la cantidad de tareas
            If Not tareas = "" Then
                selectSQL = "update tareas as tar " & _
            " inner join (select tar2.idtarea, count(*) canti from ordenes ord, tareas tar2 where tar2.idTarea=ord.idTarea group by ord.idTarea) as ord " & _
            "on (tar.idTarea= ord.idTarea) set tar.desconexiones=ord.canti where tar.idTarea in (" & tareas & ")"

                cmd.CommandText = selectSQL

                cmd.ExecuteNonQuery()
            End If

            

            'selectSQL = "Delete  tar from tareaslect tar, (select ord.idTarea, count(*) canti from tareaslect tar, lecturas ord where ord.idTarea = tar.idTarea group by ord.idTarea) tar2 where tar.idTarea=tar2.idTarea and tar2.canti=0"
            'cmd.CommandText = selectSQL

            'cmd.ExecuteNonQuery()


            'actualizaArchivos()
            If mostrarMensaje Then
                'messagebox("Se ha borrado el archivo")
                mensajeGenerico("Se ha borrado el archivo")
            End If


        Catch ex As Exception
            If mostrarMensaje Then
                'messagebox("Ocurrio un error: " & ex.Message)
                mensajeGenerico("Ocurrio un error: " & ex.Message)
            End If

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try
    End Sub

    Public Sub eliminaArchivoReferencia(index As Integer, mostrarMensaje As Boolean)

        If index < 0 Then
            Return
        End If
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)
        Dim cmd As New MySqlCommand()


        Try

            Dim selectSQL As String = "Select idLectura from lecturas where idArchivo=" & index & " and tipoLectura='' limit 1"

            Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "lecturas", True)

            cmd.Connection = conn

            If dsEmp.Tables(0).Rows.Count = 0 Then
                If mostrarMensaje Then
                    'messagebox("El archivo no puede ser removido debido a que tiene ordenes asignadas o trabajadas")
                    mensajeGenerico("Lecturas enviadas al celular.")
                End If
                Return
            End If

            'Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "archivos", True)

            ''lbEmpleados.DataSource = dsEmp
            ''lbEmpleados.DataTextField = "nombreComp"
            ''lbEmpleados.DataValueField = "id"

            'gv_tecnicos.DataSource = dsEmp

            selectSQL = "delete from archivoslect where idArchivo=" & index

            cmd.CommandText = selectSQL

            cmd.ExecuteNonQuery()


            'actualizaArchivos()

        Catch ex As Exception

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try
    End Sub

    Public Sub estableceArchivoVisible(index As Integer)
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)
        Dim cmd As New MySqlCommand()


        Try

            conn.Open()
            cmd.Connection = conn

            Dim selectSQL As String = "update archivoslect set visible=1 where idArchivo=" & index

            cmd.CommandText = selectSQL

            cmd.ExecuteNonQuery()


        Catch ex As Exception

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try
    End Sub

    Protected Sub bEliminar_Click(sender As Object, e As EventArgs) Handles bEliminar.Click
        Try
            Dim index As Integer = gv_tecnicos.SelectedRow.Cells(1).Text
            eliminaArchivo(index, True)
            actualizaArchivos()
            'gv_tecnicos.SelectedIndex = 0
        Catch ex As Exception
            mensajeGenerico("Seleccione un registro")

        End Try


    End Sub

    Public Sub New()

    End Sub

    Function remplazarManzana(texto As String) As String
        Dim retorno As String = ""
        Dim pos As Integer = 0
        If texto.ToUpper.StartsWith("'M") Then
            pos = texto.IndexOf(" ")
            If pos > 0 Then
                retorno = "'MZA. " & texto.Substring(pos + 1)
            Else
                retorno = texto 'no encontramos un espacio y puede que regrese vacio asi que..
            End If
        Else
            retorno = texto

        End If
        Return retorno
    End Function


    Protected Sub bEnviar_Click(sender As Object, e As EventArgs) Handles bEnviar.Click
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)

        Dim cmd As New MySqlCommand()
        cmd.Connection = conn
        Dim ls_update As String

        Dim selectSQL As String = "select count(*) canti from lecturas where tipoLectura=''"
        Dim ds As DataSet
        Dim row As DataRow

        'Try
        'ds = conectarMySql(conn, selectSQL, "ordenes", True)

        'If ds.Tables(0).Rows(0).Item("canti") > 0 Then
        '    'Si hay aun ordenes por asignar, indicamos continuar o quiere seguir asignando las ordenes

        '    Select Case MsgBox("Aún hay " & ds.Tables(0).Rows(0).Item("canti") & " ordene(s) por ser asignadas, ¿Desea continuar?", MsgBoxStyle.YesNo, "Asignar Ordenes")
        '        Case 7
        '            Return
        '    End Select


        'End If

        'Las ordenes tienen que ir ordenadas por entrecalles
        selectSQL = "select ord.idLectura idLectura, if(ord.tipoLectura='', true, false) asignar, tar.idEmpleado, emp.nomina, substr(emp.nomina, -4) nombreArchivo" & _
                    " from tareaslect tar, lecturas ord, empleadoslect emp" & _
                    " where tar.idTarea = ord.idTarea " & _
                    " and ord.enviada='EE002' and tar.enviada ='EE002'  and ord.tipoLectura = '' and emp.idEmpleado=tar.idEmpleado" & _
                    " order by ord.idLectura, ord.idTarea "
        ds = conectarMySql(conn, selectSQL, "lecturas", True)
        cmd.Connection = conn
        Dim idTareaAnt As String = ""
        For Each row In ds.Tables(0).Rows

            If Not idTareaAnt.Equals("") And Not row("nombreArchivo").Equals(idTareaAnt) Then
                agregaAnomalias(idTareaAnt)
            End If

            If Not row("nombreArchivo").Equals(idTareaAnt) Then
                Dim encabezado As String = "".PadLeft(340, " ")
                idTareaAnt = row("nombreArchivo")
                escribeArchivoGen(idTareaAnt, encabezado, "lecturaSalida\activos", False, "TPL")
            End If

            idTareaAnt = row("nombreArchivo")

            getCadenaAEscribir2(row("idLectura"), row("asignar"), conn)

            ls_update = "update lecturas set enviada='EE001' where idLectura=" & row("idLectura")
            cmd.CommandText = ls_update
            cmd.ExecuteNonQuery()




        Next

        If (ds.Tables(0).Rows.Count > 0) Then
            'La ultima nunca se va a insertar, asi que ...
            agregaAnomalias(idTareaAnt)
        End If

        ls_update = "update tareaslect set enviada='EE001' where enviada='EE002'"
        cmd.CommandText = ls_update
        cmd.ExecuteNonQuery()

        'messagebox("Ordenes enviadas al celular.")
        mensajeGenerico("Lecturas enviadas al celular.")

        'Catch ex As Exception
        'messagebox("Ocurrio un error: " + ex.Message)
        ' mensajeGenerico("Ocurrio un error: " & ex.Message)
        'Finally
        If Not conn Is Nothing Then
            conn.Close()
        End If
        'End Try

    End Sub

    Public Sub mensajeGenerico(ls_mensaje As String)
        lbldialogo.Text = ls_mensaje
        mp1.Show()
    End Sub

    Public Sub agregaAnomalias(idTareaAnt As String)
        Dim ptn = idTareaAnt
        Dim cadenaAEnviar As String
        Dim carpeta As String = "lecturasalida/activos/"
        Dim ext = "TPL"
        cadenaAEnviar = "#NMA0011014.0AA-DEFECTUOSO".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0021014.0BB-DESTRUIDO".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0031010.0CC-SIN SELLO".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0041010.0DD-VOLTEADO POR EL USUARIO".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0051014.0EE-RETIRADO".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0061010.0FF-DESCALIBRADO".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0071014.0GG-NO MARCA".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        'cadenaAEnviar = "#NMA0081014.0HH-SIN MEDIDOR".PadRight(340)
        'escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0091010.0II-USO ILICITO".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0101014.0JJ-OBSTRUIDO".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0111010.0KK-INSTALADO AL REVES".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0121010.0LL-VIOLO LOS SELLOS".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0131014T0MM-COMENTARIO".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0141010.0OO-CASA SOLA".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0150014.0PP-CASA CERRADA".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0161014T0QQ-NO ABRIERON".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)
        cadenaAEnviar = "#NMA0171014.0SS-EMPANADO".PadRight(340)
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)

        Dim ptn2 As String = ptn
        If ptn2.Length > 4 Then
            ptn2 = ptn2.Substring(ptn2.Length - 4, 4)
        End If

        'cadenaAEnviar = "!                    123456789           1234                Lecturista                                                                                                                                                                                                                                                                                                                                                                                                                                       101075"
        'escribeArchivoGen(savedFile, cadenaAEnviar, carpeta, ext, True)
        cadenaAEnviar = "!                    " & ptn2.PadRight(20, " ") & ptn.PadRight(20, " ") & "Lecturista".PadRight(273)
        cadenaAEnviar &= "101075"
        escribeArchivoGen(idTareaAnt, cadenaAEnviar, carpeta, True, ext)


    End Sub

    Protected Sub b_decargar0_Click(sender As Object, e As EventArgs) Handles b_decargar0.Click


        Dim ls_extension As String = "CSV"
        Dim reader As StreamReader = Nothing
        Dim ls_linea As String

        Dim ls_query As String = ""

        Dim hayArchivos As Boolean = False

        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)
        Try


            'Primero hay que realizar un query para traer todas las lecturas donde el tipo de lectura sea 0 o (4 con anomalia) del dia que se especifica
            '      ls_query = "Select cli.poliza CONTRATO, cli.clave_usuario  USU_CENS, cli.sectorcorto 'CLAVE UBICACION', lec.poliza ECONOMICO,  " & _
            '      "substring(cli.sectorcorto, 2, 2) DISTRITO, substring(cli.sectorcorto, 4, 2) PLANTA, substring(cli.sectorcorto, 6, 2) 'SECTOR F', substring(cli.sectorcorto, 8, 4) MANZANA, " & _
            '      "substring(cli.sectorcorto, 12, 3) LOTE, substring(cli.sectorcorto, 15, 2) FR, cli.sector SECTOR, emp.nomina PTN,  " & _
            '      "cli.nombre NOMBRE, cli.direccion DIRECCION, cli.colonia COLONIA, cli.tarifa TARIFA, cli.tu TU, lec.giro GIRO, lec.diametro_toma DTOMA, lec.marcaMedidor MARCA, " & _
            '      "lec.numEsferas DIGITOS, lec.serieMedidor 'N MEDIDOR', lec.promedio 'PROM M3', 'X' LECTURA_ANT2, if(trim(lecturaanterior)='', 'X', lecturaanterior) LECTURA_ANT, lec.lectura LECTURA_ACT,  " & _
            '      "if(isnull(co.nombre), '',co.nombre)  'OBSERVACION ADICIONAL', if(isnull(lec.anomalia),'' ,if (lec.anomalia='P' or lec.anomalia='Q', 'H', lec.anomalia ) ) 'CLAVE ANOMALIA', '' as 'CLAVE LECTURISTA', '' as 'CLAVE CAPTURISTA', lec.fecha 'FECHA LECTURA', '' as 'NOMBRE FOTO'  " & _
            '      "from clienteslect cli, empleadoslect emp, tareaslect tar, lecturas lec left outer join codigos co on (lec.anomalia =co.codigo) " & _
            '      "where lec.idCliente = cli.idCliente " & _
            '      "and lec.idtarea = tar.idTarea " & _
            '      "and tar.idEmpleado = emp.idEmpleado " & _
            '      "and lec.tipolectura<>'' " & _
            '      "and date_format(fechaDeAsignacion, '%Y%m%d')='" & fechaProgram.Text & "'"

            ls_query = "Select cli.poliza CONTRATO, if(isnull(lec.lectura),'0',if(lec.lectura='','0',lec.lectura)) LECTURA, '' DESCONOCIDO1, if(isnull(lec.anomalia),'',lec.anomalia) ANOMALIA, '' DESCONOCIDO2,  emp.idEmpleado as LECTURISTA, date_format(lec.fecha, '%d/%m/%Y') FECHA " & _
            "from clienteslect cli, empleadoslect emp, tareaslect tar, lecturas lec left outer join codigos co on (lec.anomalia =co.codigo) " & _
            "where lec.idCliente = cli.idCliente " & _
            "and lec.idtarea = tar.idTarea " & _
            "and tar.idEmpleado = emp.idEmpleado " & _
            "and lec.tipolectura<>'' " & _
            "and date_format(fechaDeAsignacion, '%Y%m%d')='" & fechaProgram.Text & "'"


            'ls_query = " Select lect.*, nomina, date_format(fecha, '%m%d%y15%h%i%s') fechaEjecucion from lecturas lect, tareaslect tar, empleadoslect emp " & _
            ' " where lect.idTarea = tar.idTarea And emp.idEmpleado = tar.idEmpleado " & _
            '" and (lect.tipoLectura='0'  or (lect.tipoLectura='4' and not( isNull(lect.anomalia) or lect.anomalia='' )))" & _
            ' "and date_format(fechaDeAsignacion, '%Y%m%d')='" & fechaProgram.Text & "'"


            Dim ds As DataSet = conectarMySql(conn, ls_query, "lecturas", True)
            Dim cadenaAEscribir As String = ""
            Dim gv As GridView = New GridView()
            gv.ID = "gvToExcel"

            For Each row2 As DataRow In ds.Tables("lecturas").Rows
                Dim nombreGenerico As String
                Dim extension As String = ".jpg"
                Dim ls_poliza As String = row2("CONTRATO")
                Dim serieMedidor = ""
                Dim ls_fechaDeEjecucion As String = row2("FECHA").ToString
                Dim serieMedidorRaw = ""

                nombreGenerico = ls_poliza
            Next

            gv.DataSource = ds
            gv.DataBind()


            Dim blanco As New Label()
            blanco.Text = ""
            Dim generacion As New Label()
            generacion.Text = getFecha("dd/MM/yyyy HH:mm")
            Dim rango As New Label()
            rango.Text = getFecha("De " & fechaProgram.Text.Substring(6, 2) & "/" & fechaProgram.Text.Substring(4, 2) & "/" & fechaProgram.Text.Substring(0, 4) & " al " _
                                  & fechaProgram.Text.Substring(6, 2) & "/" & fechaProgram.Text.Substring(4, 2) & "/" & fechaProgram.Text.Substring(0, 4))

            Dim dsFinal As DataSet = New DataSet
            dsFinal.Tables.Add()
            dsFinal.Tables(0).Columns.Add(" ")
            dsFinal.Tables(0).Columns.Add("  ")

            Dim row As DataRow = dsFinal.Tables(0).Rows.Add()
            row.Item(0) = "Fecha de Expedicion:"
            row.Item(1) = getFecha("dd/MM/yyyy HH:mm")
            row = dsFinal.Tables(0).Rows.Add()
            row.Item(0) = "Fecha:"
            row.Item(1) = getFecha("De " & fechaProgram.Text.Substring(6, 2) & "/" & fechaProgram.Text.Substring(4, 2) & "/" & fechaProgram.Text.Substring(0, 4) & " al " _
                                  & fechaProgram.Text.Substring(6, 2) & "/" & fechaProgram.Text.Substring(4, 2) & "/" & fechaProgram.Text.Substring(0, 4))

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
            'form.Controls.Add(enca)
            form.Controls.Add(gv)
            pagina.RenderControl(htw)
            Response.Clear()
            Response.Buffer = True
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment;filename=" & fechaProgram.Text & ".xls")
            Response.Charset = "UTF-8"
            Response.ContentEncoding = Encoding.Default
            Response.Write(sb.ToString())
            Response.End()

        Catch ex As Exception
            mensajeGenerico("Ocurrio un error:" & ex.Message)
        Finally
            If Not IsNothing(conn) Then
                conn.Close()
            End If
        End Try



    End Sub

    Public Function mostrarImagenes(path As String, nombreGenerico As String, nombreGenerico3 As String) As String



        For Each foundFile As String In My.Computer.FileSystem.GetFiles(
    path,
    Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, nombreGenerico & "*.jpg")

            Return System.IO.Path.GetFileName(foundFile)
        Next

        
        Return ""


    End Function
End Class
