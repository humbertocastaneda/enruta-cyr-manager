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

    Dim debug As String = " Debug:"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        setMenu(contenedor)
        If Not IsPostBack Then
            actualizaArchivos()
            fechaProgram.Text = getFecha("yyyyMMdd")
            ' b_cargar.Attributes.Add("onclick", "this.disabled='true';")
            b_cargar.Attributes.Add("onclick", " this.disabled = true; __doPostBack('b_cargar', ''); return false;")
            b_cargar.Attributes.Add("onload", "this.disabled='false';")
        End If

    End Sub

    Protected Sub b_cargar_Click(sender As Object, e As EventArgs) Handles b_cargar.Click

        Dim uploaded As Boolean = False
        'If Not isLoged() Then
        '    messagebox("No estas conectado")
        '    lbl_msj.Text = "No estas conectado."
        '    Return
        'End If


        If (FileUploadControl.HasFile) Then
            Dim filename As String = Path.GetFileName(FileUploadControl.FileName)
            If Not (filename.EndsWith("xlsx") Or filename.EndsWith("xls")) Then
                mensajeGenerico("El documento seleccionado no es una archivo de excel")
                Return
            End If

            Try


                FileUploadControl.SaveAs(Server.MapPath("~/") & "uploads\" & filename)
                uploaded = True
                'Ingresamos en la bd el archivo

                index = guardarArchivoBD(filename)

                readFile(Server.MapPath("~/") & "uploads\" & filename)

                estableceArchivoVisible(index)

                'messagebox("Archivo ha sido cargado.")

                mensajeGenerico("Archivo ha sido cargado. " & idRow & " Registros Procesados") '& debug)
            Catch ex As Exception
                'Interaction.MsgBox("Ocurrio un error: " & ex.Message)
                'Throw ex
                'Borramos el archivo, ya no nos interesa, y su referencia
                eliminaArchivo(index, False)
                lbl_msj.Text = "Ocurrio un error:" & ex.Message
                'messagebox("Ocurrio un error: " & ex.Message)

                mensajeGenerico("Ocurrio un error:" & ex.Message)


            Finally
                If uploaded Then

                    'Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(New System.Uri("http://espinosacarlos.com/uploads/eliminaExcel.php?ruta=" & filename))

                    'request.Method = System.Net.WebRequestMethods.Http.Get

                    'Dim response As System.Net.HttpWebResponse = request.GetResponse()

                    '' process response here

                    'response.Close()

                    Try
                        'Dim dir As New FileInfo(Server.MapPath("~/") & "uploads\" & filename)

                        'If (dir.Attributes And FileAttributes.ReadOnly) > 0 Then
                        '    dir.Attributes = dir.Attributes Xor FileAttributes.ReadOnly

                        'End If

                        'Dim Level1DirSec As DirectorySecurity = Directory.GetAccessControl(Server.MapPath("~/") & "uploads")

                        'Level1DirSec.AddAccessRule(New FileSystemAccessRule(New System.Security.Principal.SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, Nothing),
                        '     FileSystemRights.ReadAndExecute,
                        '     InheritanceFlags.ContainerInherit + InheritanceFlags.ObjectInherit,
                        '     PropagationFlags.None,
                        '     AccessControlType.Allow))

                        'Directory.SetAccessControl(Server.MapPath("~/") & "uploads\", Level1DirSec)
                        'Level1DirSec.SetAccessRuleProtection(True, True)

                        'dir.Delete()

                        System.IO.File.Delete(Server.MapPath("~/") & "uploads\" & filename)
                    Catch ex As Exception
                        'Throw ex
                    End Try

                End If

            End Try

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


            Dim sheetName As String = "" 'dtExcelSchema.Rows(0)("TABLE_NAME")

            For Each row As DataRow In dtExcelSchema.Rows
                sheetName = row("TABLE_NAME").ToString
                If sheetName.Contains("_xlnm#_FilterDatabase") Then
                    Continue For
                Else
                    Exit For
                End If
            Next

            ' Create OleDbCommand object and select data from worksheet Sheet1
            'Dim cmd As OleDbCommand = New OleDbCommand("SELECT * FROM [Hoja1$]", oledbConn)
            cmd = New OleDbCommand("SELECT * FROM [" & sheetName & "]", oledbConn)

            debug &= " Sheet name: " & sheetName

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

            debug &= ds.Tables.Count & " Tablas "
            debug &= " Mi nombre es: " & ds.Tables(0).TableName
            debug &= ds.Tables(0).Rows.Count & " Registros en el archivo"
            For Each row In ds.Tables(0).Rows
                idRow += 1
                'Dim valores As String = generaElementosAInsertar(row, ds.Tables(0).Columns.Count)
                'ls_insert = "Insert into ordenes(" & ls_columnas & _
                '") values(" & valores & ")"



                Dim numOrden As String = getValueFile(conn, row, "numOrden")

                'como puede haber registros vacios necesitamos que los sepa controlar... asi que si el numero de orden esta vacio quiere decir que no es una orden.
                If numOrden = "" Or numOrden = "0" Then
                    debug &= "Campo numero de orden vacio"
                    Continue For
                End If

                'If idRow = 59 Then
                '    Dim a As Integer = 0
                'End If

                debug &= "Cargamos datos"
                Dim poliza As String = getValueFile(conn, row, "poliza")
                Dim cliente As String = getValueFile(conn, row, "cliente")
                Dim numMedidor As String = getValueFile(conn, row, "numMedidor")
                Dim calle As String = getValueFile(conn, row, "calle")
                Dim numInterior As String = getValueFile(conn, row, "numInterior")
                'Dim numExterior As String = getValueFile(conn, row, "numExterior")
                Dim colonia As String = getValueFile(conn, row, "colonia")
                Dim ls_municipio As String = getValueFile(conn, row, "municipio")
                Dim entreCalles As String = getValueFile(conn, row, "entreCalles")
                Dim comoLlegar As String = getValueFile(conn, row, "comoLlegar")
                Dim status As String = getValueFile(conn, row, "status")


                'entreCalles = remplazarManzana(entreCalles)

                Dim tipoDeOrden As String = "'TO002'" 'getValueFile(conn, row, "tipoDeOrden")
                Dim descOrden As String = "'Corte'" 'parseCodigo(conn, tipoDeOrden)

                Dim idMunicipio As Integer = getID(conn, PR_Municipio, ls_municipio, Nothing)
                Dim idColonia As Integer = getID(conn, PR_Colonia, colonia, idMunicipio)
                Dim idCalle As Integer = getID(conn, PR_calle, calle, idColonia)
                'Dim numeroSerie As String = "''" 'por ahora es el mismo
                Dim idCliente As Integer
                'Dim edificio As String = getValueFile(conn, row, "comoLlegar")
                Dim vencido As String = getValueFile(conn, row, "vencido")
                Dim diametro_toma As String = getValueFile(conn, row, "diametro_toma")
                Dim tipo_usuario As String = getValueFile(conn, row, "tipo_usuario")
                Dim giro As String = getValueFile(conn, row, "giro")
                Dim balance As String = getValueFile(conn, row, "balance")
                Dim fecha_ultimo_pago As String = getValueFile(conn, row, "fecha_ultimo_pago")

                Dim ultimo_pago As String = getValueFile(conn, row, "ultimo_pago")



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
                SelectSQL = "Select idCliente from clientes where poliza=" & poliza

                Dim dsCliente As DataSet = conectarMySql(conn, SelectSQL, cliente, False)

                If dsCliente.Tables(0).Rows.Count = 0 Then
                    'si no existe el cliente, lo agregamos
                    ls_insert = "insert into clientes (idCalle, nombre, poliza, numInterior, entreCalles, comoLlegar ) " & _
                        " values(" & idCalle & ", " & cliente & ", " & poliza & ", " & numInterior & ", " & _
                        entreCalles & ", " & comoLlegar & ")"
                    cmd.CommandText = ls_insert
                    cmd.ExecuteNonQuery()

                    idCliente = getLastInsertedId(conn, False)
                Else

                    idCliente = dsCliente.Tables(0).Rows(0).Item("idCliente")
                    'If tipoDeOrden = "TO001" Or tipoDeOrden = "TO002" Then
                    Dim ls_update As String = "update clientes set  entreCalles=" & entreCalles & ", nombre=" & cliente & ", numInterior=" & numInterior & ", comoLlegar=" & comoLlegar & " where idCliente=" & dsCliente.Tables(0).Rows(0).Item("idCliente")
                    Try
                        cmd.CommandText = ls_update
                        cmd.ExecuteNonQuery()
                    Catch ex As Exception

                    End Try
                    'End If


                End If


                ls_insert = "insert into ordenes(numOrden, poliza, numMedidor, fechadeRecepcion, idCliente, tipodeorden,  idArchivo, vencido,diametro_toma,tipo_usuario, giro, balance, fecha_ultimo_pago, descOrden, ultimo_pago, status) " & _
                    " values (" & numOrden & ", " & poliza & ", " & numMedidor & ", str_to_date('" & getFecha("yyyyMMddHHmmss") & "', '%Y%m%d%H%i%s'), " & idCliente & ", " & tipoDeOrden & ", " & index & ", " & vencido & ", " & diametro_toma & ", " & tipo_usuario & _
                        ", " & giro & ", " & balance & ", " & fecha_ultimo_pago & ", " & descOrden & ", " & ultimo_pago & ", " & status & ")"

                cmd.CommandText = ls_insert
                cmd.ExecuteNonQuery()

                Try
                    'Asignamos si acaso la orden tiene un empleado valido
                    'Obtenemos el ultimo numero de orden asig
                    SelectSQL = "Select idOrden from ordenes where numOrden=" & numOrden & " order by idOrden desc limit 1"
                    ds = conectarMySql(conn, SelectSQL, "ordenes", False)

                    Dim idOrden As Integer
                    idOrden = ds.Tables(0).Rows(0).Item("idOrden")
                    'Buscamos el tecnico


                    Dim idEmpleado As Integer
                    Dim ptn As String = getValueFile(conn, row, "ptn")

                    SelectSQL = "Select idEmpleado from empleados where nomina=" & ptn
                    ds = conectarMySql(conn, SelectSQL, "empleados", False)

                    If ds.Tables(0).Rows.Count > 0 Then
                        idEmpleado = ds.Tables(0).Rows(0).Item("idEmpleado")

                        asignaDesasigna(idEmpleado, idOrden, "EO001", conn, fechaProgram.Text)
                    Else
                        ls_insert = "Insert into empleados (Nombre, alias, nomina, apmat, appat) values (" & ptn & ", " & ptn & ", " & ptn & ", '', '')"
                        cmd.CommandText = ls_insert
                        cmd.ExecuteNonQuery()

                        SelectSQL = "Select idEmpleado from empleados where nomina=" & ptn
                        ds = conectarMySql(conn, SelectSQL, "empleados", False)

                        If ds.Tables(0).Rows.Count > 0 Then
                            idEmpleado = ds.Tables(0).Rows(0).Item("idEmpleado")

                            asignaDesasigna(idEmpleado, idOrden, "EO001", conn, fechaProgram.Text)
                        End If


                    End If

                Catch ex As Exception

                End Try




            Next


            ' ds.Tables("[Hoja1$]").Rows.

        Catch ex As Exception
            Throw New Exception(ex.Message & " Verifique linea " & idRow & " del archivo.")

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
    End Sub

    Public Function generaColumnas(conn As MySqlConnection) As String
        Dim selectSQL As String = "Select nombreCampoBd, tipoDeDato  from estructuras where esDeEntrada=1 and not isnull(campo) order by campo"
        Dim cmd As New MySqlCommand(selectSQL, conn)
        'Dim reader As MySqlDataReader
        Dim adapter As New MySqlDataAdapter(cmd)
        Dim pubs As New DataSet()
        Dim resultado As String = ""
        Dim i As Integer = 0

        Try
            'reader = cmd.ExecuteReader


            adapter.Fill(pubs, "estructuras")

            Dim row As DataRow
            ReDim tiposDeDato(pubs.Tables("estructuras").Rows.Count)

            For Each row In pubs.Tables("estructuras").Rows
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

        Dim sqlSelect As String = "Select campo, tipodedato from estructuras where upper(nombrecampoBD)='" & campo.ToUpper & "'"

        Dim ds As DataSet = paginaGenerica.conectarMySql(conn, sqlSelect, "estructuras", False)

        If ds.Tables(0).Rows.Count = 0 Then
            Throw New Exception("No se encuentra el campo indicado")
        End If

        Dim rowDB As DataRow = ds.Tables(0).Rows.Item(0)

        getValueFile = IIf(IsDBNull(row(rowDB("campo"))), "", row(rowDB("campo")))

        'Hay que eliminar caracteres que no deban existir, como comillas simples
        getValueFile = eliminaCaracteresRaros(getValueFile, "'")

        If rowDB("tipodedato") = 1 Then
            'es un string, hay que agregarle las comillas
            getValueFile = "'" & getValueFile & "'"
        Else

            If getValueFile = "" Or Not IsNumeric(getValueFile) Then
                getValueFile = "0"
            End If
        End If


    End Function

    Function eliminaCaracteresRaros(cadena As String, caracterAEliminar As String) As String
        Dim index As Integer = cadena.IndexOf(caracterAEliminar)

        While Not index = -1
            cadena = cadena.Substring(0, index) & cadena.Substring(index + 1)

            index = cadena.IndexOf(caracterAEliminar)
        End While

        Return cadena

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
                ", idArchivo id, fecha from archivos  where visible= 1" & _
                " order by fecha desc limit 5 "

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
            Dim selectSQL As String = "Insert into archivos(nombre, fecha) values ('" & filename & "', str_to_date('" & getFecha("yyyyMMddHHmmss") & "', '%Y%m%d%H%i%s'))"

            conn.Open()

            'Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "archivos", True)

            ''lbEmpleados.DataSource = dsEmp
            ''lbEmpleados.DataTextField = "nombreComp"
            ''lbEmpleados.DataValueField = "id"

            'gv_tecnicos.DataSource = dsEmp
            cmd.CommandText = selectSQL

            cmd.ExecuteNonQuery()


            selectSQL = "Select idArchivo canti from archivos order by idArchivo desc"
            Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "archivos", False)

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

            Dim selectSQL As String = "Select idOrden from ordenes where idArchivo=" & index & " and (estadoDeLaOrden='EO000' or estadoDeLaOrden='EO001')  limit 1"

            Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "ordenes", True)

            cmd.Connection = conn

            If dsEmp.Tables(0).Rows.Count = 0 Then
                If mostrarMensaje Then
                    'messagebox("El archivo no puede ser removido debido a que tiene ordenes asignadas o trabajadas")
                    mensajeGenerico("El archivo no puede ser removido debido a que tiene ordenes trabajadas")
                End If
                Return
            End If

            'Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "archivos", True)

            ''lbEmpleados.DataSource = dsEmp
            ''lbEmpleados.DataTextField = "nombreComp"
            ''lbEmpleados.DataValueField = "id"

            'gv_tecnicos.DataSource = dsEmp

            'Hay que borrar las tareas

            selectSQL = "Select idTarea fecha from ordenes where idArchivo=" & index & " group by idTarea"

            dsEmp = conectarMySql(conn, selectSQL, "ordenes", False)

            Dim tareas As String = ""

            ' Obtenemos las tareas que quedaran vacias...
            For Each row As DataRow In dsEmp.Tables("ordenes").Rows
                If Not tareas = "" Then
                    tareas &= ","
                End If
                tareas &= row(0)

            Next

            selectSQL = "delete from ordenes where idArchivo=" & index

            cmd.CommandText = selectSQL

            cmd.ExecuteNonQuery()

            selectSQL = "delete from archivos where idArchivo=" & index

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

            Dim selectSQL As String = "Select idOrden from ordenes where idArchivo=" & index & " and estadoDeLaOrden='EO000' limit 1"

            Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "ordenes", True)

            cmd.Connection = conn

            If dsEmp.Tables(0).Rows.Count = 0 Then
                If mostrarMensaje Then
                    'messagebox("El archivo no puede ser removido debido a que tiene ordenes asignadas o trabajadas")
                    mensajeGenerico("Ordenes enviadas al celular.")
                End If
                Return
            End If

            'Dim dsEmp As DataSet = conectarMySql(conn, selectSQL, "archivos", True)

            ''lbEmpleados.DataSource = dsEmp
            ''lbEmpleados.DataTextField = "nombreComp"
            ''lbEmpleados.DataValueField = "id"

            'gv_tecnicos.DataSource = dsEmp

            selectSQL = "delete from archivos where idArchivo=" & index

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

            Dim selectSQL As String = "update archivos set visible=1 where idArchivo=" & index

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
            selectSQL = "select emp.nomina idEmpleado, ord.idTarea,  ord.idOrden idOrden, if(ord.estadoDeLaOrden='EO001', true, false) asignar , cli.entreCalles " & _
                        " from empleados emp, tareas tar, ordenes ord, clientes cli , calles ca, colonias co, municipios mu " & _
                        " where emp.idEmpleado=tar.idEmpleado and tar.idTarea = ord.idTarea and  cli.idCliente=ord.idCliente and ca.idCalle = cli.idCalle and ca.idColonia=co.idColonia and mu.idMunicipio = co.idMunicipio " & _
                        " and ord.enviada='EE002' and tar.enviada ='EE002'  and ord.estadodelaorden not in ('EO002','EO003','EO004','EO005','EO006','EO007', 'CD000','CD001','CD002' ) " & _
                        " order by tar.idEmpleado, mu.nombre, co.Nombre, cli.entreCalles ,ca.idCalle "
            ds = conectarMySql(conn, selectSQL, "ordenes", True)
            cmd.Connection = conn
            Dim idTarea As Integer = 0
            Dim idEmpleado As String = ""

            Dim ruta As String = Server.MapPath("~/") & "out\ordenes\"
            For Each row In ds.Tables(0).Rows
                If idTarea <> row("idTarea") Then

                    ls_update = "update tareas set enviada='EE001' where idTarea=" & row("idTarea")
                    cmd.CommandText = ls_update
                    cmd.ExecuteNonQuery()
                    idTarea = row("idTarea")
                End If

                getCadenaAEscribir2(row("idOrden"), row("asignar"), conn)

                ls_update = "update ordenes set enviada='EE001' where idOrden=" & row("idOrden")
                cmd.CommandText = ls_update
                cmd.ExecuteNonQuery()
                If (idEmpleado <> row("idEmpleado") And Not idEmpleado.Equals("")) Then
                    System.IO.File.Move(ruta & "p" & idEmpleado & ".txt", ruta & idEmpleado & ".txt")
                    idEmpleado = row("idEmpleado")
                ElseIf idEmpleado.Equals("") Then
                    idEmpleado = row("idEmpleado")
                End If
            Next

            If My.Computer.FileSystem.FileExists(ruta & "p" & idEmpleado & ".txt") Then
                System.IO.File.Move(ruta & "p" & idEmpleado & ".txt", ruta & idEmpleado & ".txt")
            End If



            'ls_update = "update tareas set enviada='EE001' where enviada='EE002'"
            'cmd.CommandText = ls_update
            'cmd.ExecuteNonQuery()

            'messagebox("Ordenes enviadas al celular.")
            mensajeGenerico("Ordenes enviadas al celular.")

        Catch ex As Exception
            'messagebox("Ocurrio un error: " + ex.Message)
            mensajeGenerico("Ocurrio un error: " & ex.Message)
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try

    End Sub

    Public Sub mensajeGenerico(ls_mensaje As String)
        lbldialogo.Text = ls_mensaje
        mp1.Show()
    End Sub
End Class
