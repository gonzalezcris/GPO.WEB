<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GPORelAcompMensalObjetivosImp.aspx.cs" Inherits="GPORelAcompMensalObjetivosImp" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>GPO - Sistema Gestão de Programa de Objetivo</title>
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-control" content="no-cache">
    <meta http-equiv="Expires" content="0">
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width: 98%">
                <tr>
                    <td align="center" colspan="2">
                        <asp:Image ID="Image1" runat="server" Width="23px" Visible="false" />
                        <CR:CrystalReportViewer ID="viewReport" runat="server" AutoDataBind="True" Height="1036px"
                            HasCrystalLogo="False" HasToggleParameterPanelButton="False" HasSearchButton="False"
                            HasToggleGroupTreeButton="False" PrintMode="ActiveX" ToolPanelView="None" HasPrintButton="False" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>