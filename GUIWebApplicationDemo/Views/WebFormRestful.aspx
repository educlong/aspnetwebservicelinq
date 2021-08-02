<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormRestful.aspx.cs" Inherits="GUIWebApplicationDemo.Views.WebFormRestful" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>WebForm Restful Webservice</title>
    <!--ANGULARJS  -->
    <script src="angular.min.js"></script>
    <script>
        var myApp=angular.module("DemoPeopleApp",[])
        myApp.controller("PeopleController",function($scope,$http){
            $http({
                method:"GET",
                url:"https://www.w3schools.com/angular/customers.php"
            }).then(function mySuccess(response){
                $scope.data=response.data
            }, function myError(response){
                $scope.dataError=response.statusText
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Query DanhMục và list SảnPhẩm theo mã DM:
            <asp:TextBox ID="txtCodeDm" runat="server" Width="46px"></asp:TextBox>
            &nbsp;với khoảng zá từ:&nbsp;
            <asp:TextBox ID="txtFromPrice" runat="server" Width="44px"></asp:TextBox>
&nbsp;đến:
            <asp:TextBox ID="txtToPrice" runat="server" Width="56px"></asp:TextBox>
            &nbsp;hoặc query sản phẩm có mã SP:
            <asp:TextBox ID="txtCodeSp" runat="server" Width="67px"></asp:TextBox>
&nbsp;<asp:Button ID="btnQuery" runat="server" Text="Query" Width="81px" OnClick="btnQuery_Click" />
            
            <br />
            Add/Delete:
            tênSP:
            <asp:TextBox ID="txtNameSP" runat="server" Width="110px"></asp:TextBox>
            , záSP:
            <asp:TextBox ID="txtPriceSP" runat="server" Width="46px"></asp:TextBox>
            <asp:Button ID="btnAddSP" runat="server" Text="Add SP" Width="93px" OnClick="btnAddSP_Click"/>
            <asp:Button ID="btnDeleteSP" runat="server" Text="Delete SP" Width="127px" OnClick="btnDeleteSP_Click"/>
            
            , tênDM:
            <asp:TextBox ID="txtNameDM" runat="server" Width="121px"></asp:TextBox>
            
            <asp:Button ID="btnAddDM" runat="server" Text="Add DM" Width="102px" OnClick="btnAddDM_Click"/>
            <asp:Button ID="btnDeleteDM" runat="server" Text="Delete DM" Width="131px" OnClick="btnDeleteDM_Click"/>
            <asp:Label ID="lblQueryDM" runat="server" Text="Infor danh mục."></asp:Label>
            <asp:Label ID="lblQuerySP" runat="server" Text="Infor sản phẩm."></asp:Label>
            
            <br />
            <asp:ListBox ID="lsbDanhMuc" runat="server" Height="194px"></asp:ListBox>
            <asp:ListBox ID="lsbSanPham" runat="server" Height="199px"></asp:ListBox>

            <!-- PHẦN XỬ LÝ ANGULAR JS -->
            <div ng-app="DemoPeopleApp" ng-controller="PeopleController">
                Lọc data: <input type="text" ng-model="keyWordSearch"/><br/>
                Danh sách khách hàng:
                <table>
                    <tr>
                        <th>Tên</th>
                        <th>Thành phố</th>
                        <th>Quốc za</th>
                    </tr>
                    <tr ng-repeat="people in data.records|filter:keyWordSearch">
                        <td>{{people.Name}}</td>
                        <td>{{people.City}}</td>
                        <td>{{people.Country}}</td>
                    </tr>
                </table>
            </div>
            
        </div>
    </form>
</body>
</html>
