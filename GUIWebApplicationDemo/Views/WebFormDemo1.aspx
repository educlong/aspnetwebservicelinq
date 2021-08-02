<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormDemo1.aspx.cs" Inherits="GUIWebApplicationDemo.Views.WebFormDemo1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    </head>
<body>
    <form id="form1" runat="server">
        <div>
            Username:&nbsp;
            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            <br />
            Password:
            <asp:TextBox ID="txtPassword" runat="server" height="38px" width="248px"></asp:TextBox>
            <br />
            <asp:CheckBox ID="CheckBox1" runat="server" Text="Luu thong tin dang nhap" />
            <br />
            <asp:Button ID="Button1" runat="server" Text="Login" BackColor="#FF33CC" />
            <asp:Button ID="Button2" runat="server" Text="Logout" />
            <asp:Button ID="Button3" runat="server" Text="Click me" OnClick="Button3_Click" />
            <br />
            <asp:Label ID="lblInforUser" runat="server" Text="Thong tin user dang nhap"></asp:Label>
            <br />
            <asp:Button ID="btnLuuSinhVien" runat="server" Text="SaveSV" OnClick="btnLuuSinhVien_Click" Width="103px" />

            <br />
            <asp:ListBox ID="lstSinhVien" runat="server" Height="125px" Width="90px"></asp:ListBox>

            <br />
            <br />
            <asp:Button ID="btnQueryDanhMuc" runat="server" Text="QueryDanhMuc" OnClick="btnQueryDanhMuc_Click" />
            <asp:TextBox ID="txtSearchDanhMucTheoMa" runat="server" Width="115px"></asp:TextBox>
            <asp:Button ID="btnSearchDanhMucTheoMa" runat="server" Text="SearchDanhMuc" OnClick="btnSearchDanhMucTheoMa_Click" Width="213px" />
            <asp:TextBox ID="txtMaDanhMuc" runat="server" Width="92px" Text="Nhập mã danh mục"></asp:TextBox>
            <asp:TextBox ID="txtTenDanhMuc" runat="server" Width="107px" Text="Nhập tên danh mục muốn thêm"></asp:TextBox>
            <asp:Button ID="btnUpdateAndInsert" runat="server" Text="Update & Insert DanhMuc" Width="321px" OnClick="btnUpdateAndInsert_Click" />
            <asp:Button ID="btnDeleteDanhMuc" runat="server" Text="Delete Danh Muc" OnClick="btnDeleteDanhMuc_Click" Width="219px" />
            <br />
            <asp:ListBox ID="lstDanhMuc" runat="server" Width="244px" Height="132px"></asp:ListBox>
            <asp:Label ID="lblDanhMuc" runat="server" Text=""></asp:Label>

            <br />
            <asp:Button ID="btnQueryAllSanPham" runat="server" Text="QuerySanPham" Width="197px" OnClick="btnQueryAllSanPham_Click" />
            <asp:Label ID="Label1" runat="server" Text="Nhập Mã danh mục:"></asp:Label>
            <asp:TextBox ID="txtQuerySpTheoMaDm" runat="server" Width="144px"></asp:TextBox>
            <asp:Label ID="Label2" runat="server" Text="Nhập Mã sản phẩm:"></asp:Label>
            <asp:TextBox ID="txtQuerySpTheoMaSp" runat="server"  Width="144px"></asp:TextBox>
            <asp:Button ID="btnSearchSanPham" runat="server" Text="SearchSanPham" Width="207px" OnClick="btnSearchSanPham_Click" />
            <br />
            <asp:TextBox ID="txtTenSanPham" runat="server" Width="155px" Text="Nhập tên SP"></asp:TextBox>
            <asp:TextBox ID="txtDonZa" runat="server" Width="155px" Text="Nhập đơn zá"></asp:TextBox>
            <asp:Button ID="btnUpdateAndInsertSP" runat="server" Text="Update & Insert SanPham" Width="321px" OnClick="btnUpdateAndInsertSP_Click" />
            <asp:Button ID="btnDeleteSanPham" runat="server" Text="Delete San Pham" OnClick="btnDeleteSanPham_Click" Width="219px" />
            <br />
            <asp:ListBox ID="lstSanPham" runat="server" Width="244px" Height="132px"></asp:ListBox>
            <asp:Label ID="lblSanPham" runat="server" Text=""></asp:Label>
            <br />
            <asp:Button ID="btnQuerySanPhamDataAdapter" runat="server" Text="QuerySanPhamAdapter" Width="308px" OnClick="btnQueryAllSanPhamDataAdapter_Click" />
            <asp:Button ID="btnInsertSPDataAdapter" runat="server" Text="Insert và Update SanPham DataAdapter" Width="373px" OnClick="btnUpdateAndInsertSPDataAdapter_Click" />
            <asp:Label ID="lblSanPhamDataAdapter" runat="server" Text=""></asp:Label>
            <br />
            <asp:GridView ID="gvSanPham" runat="server" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
                <AlternatingRowStyle BackColor="White" />
                <FooterStyle BackColor="#CCCC99" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <RowStyle BackColor="#F7F7DE" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                <SortedAscendingHeaderStyle BackColor="#848384" />
                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                <SortedDescendingHeaderStyle BackColor="#575357" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Db_ProductManagementConnections %>" SelectCommand="SELECT [MaSp], [TenSp], [DonGia], [MaDm] FROM [SanPham]"></asp:SqlDataSource>
            
        </div>
    </form>
</body>
</html>
