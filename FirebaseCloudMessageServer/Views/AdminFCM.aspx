<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminFCM.aspx.cs" Inherits="FirebaseCloudMessageServer.Views.AdminFCM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Nhập tiêu đề muốn send thông báo<br />
        <asp:TextBox ID="TxtTieuDeFCM" runat="server" Height="16px" Width="305px"></asp:TextBox>
        <br />
        Nhập nội dung muốn thông báo<br />
        <asp:TextBox ID="TxtNoiDungFCM" runat="server" Height="69px" TextMode="MultiLine" Width="310px"></asp:TextBox>
        <br />
        <asp:Button ID="BtnSendThongBaoFCM" runat="server" OnClick="BtnSendThongBaoFCM_Click" Text="Send thông báo đến All Khách hàng" />
        <br />
        Kết quả sau khi send thông báo<br />
        <asp:TextBox ID="TxtKetQuaFCM" runat="server" Height="80px" TextMode="MultiLine" Width="311px"></asp:TextBox>
    
    </div>
    </form>
</body>
</html>
