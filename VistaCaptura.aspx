<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VistaCaptura.aspx.vb" Inherits="Default3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

    <div>
    <asp:DataGrid ID="dgOrdenes" runat="server" AutoGenerateColumns="False" Font-Bold="True" Font-Italic="False" Font-Size="X-Large" Font-Names="Segoe UI" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="Black" AllowPaging="True" PageSize="1" ShowHeader="False" >
                    <AlternatingItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Size="X-Large" Font-Strikeout="False" Font-Underline="False" ForeColor="Black"  />
                    <Columns>
                        
                        <asp:BoundColumn  DataField="numOrden" HeaderText="Número de Orden" />
                        <asp:BoundColumn DataField="numMedidor" HeaderText="Código de Barras" />
                        <asp:BoundColumn DataField="tipodeorden" HeaderText="Tipo de Orden" />
                        <asp:TemplateColumn HeaderText="Estado de la Orden">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%#Container.DataItem("estadodelaorden")%>'></asp:Label>
                                <asp:Label ID="Label2" runat="server" Text='<%#estadoAnterior(Container.DataItem("estadoAnterior"))%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="numMedidor" HeaderText="Código de Barras" />
                        <asp:BoundColumn DataField="lectura" HeaderText="Lectura" />
                        <asp:BoundColumn DataField="numSello" HeaderText="Sello" />
                        <asp:BoundColumn DataField="motivo" HeaderText="Motivo" />
                        <asp:BoundColumn DataField="fechaderecepcion" HeaderText="Fecha de Carga" />
                        <asp:BoundColumn DataField="fechadeejecucion" HeaderText="Fecha de Ejecucion" />
                        <asp:BoundColumn DataField="comentarios" HeaderText="Comentarios" />
                    </Columns>
                    <HeaderStyle BackColor="#004FC5" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="White" />
                    <ItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Size="X-Large" Font-Strikeout="False" Font-Underline="False" ForeColor="Black" />
                    <PagerStyle Position="TopAndBottom" />
                    <SelectedItemStyle BackColor="#004FC5" Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Size="X-Large" Font-Strikeout="False" Font-Underline="False" ForeColor="White" />
                </asp:DataGrid>
    </div>
    
    </form>
</body>
</html>
