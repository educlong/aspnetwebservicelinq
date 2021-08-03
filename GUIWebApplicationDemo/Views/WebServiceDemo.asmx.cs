using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DataTransferObject;
using BusinessLogicLayer;

/** 
 * WebServiceDemo -> Xây dựng Web service SOAP API để truy vấn thay đổi data thông qua kỹ thuật LINQ 
 * ____________________Tương tác Webservice SOAP API (tương tác trên mobile)_________________________
 * 
 * FILE NÀY K LIÊN QUAN ĐẾN THIẾT KẾ BACKEND CỦA THIẾT KẾ WEB (ko tương tác vs Controllers và models) 
 */

namespace GUIWebApplicationDemo.Views
{
    /// <summary>
    /// Summary description for WebServiceDemo
    /// </summary>
    [WebService(Namespace = "http://www.webservicecoursedemo.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]   //để có thể triệu gọi cái service này từ aspnet hay từ bất cứ trình duyệt nào đó

    public class WebServiceDemo : System.Web.Services.WebService
    {
        DanhMucLogic danhMucLogic = new DanhMucLogic(); 
        [WebMethod]
        public int DanhMucCount()
        {
            return danhMucLogic.QueryDanhMucLinQ("").Count();
        }
        [WebMethod]
        public List<DanhMucNoLinq> DanhMucs()
        {
            return danhMucLogic.QueryDanhMucLinQ("");
        }
        [WebMethod]
        public DanhMucNoLinq DanhMucDetail(string madm)
        {
            return madm == "" ? null : danhMucLogic.QueryDanhMucLinQ("").FirstOrDefault(dm => dm.MaDm == Int32.Parse(madm));
        }
        [WebMethod]
        public bool DanhMucInsertUpdate(string madm, string tendm)
        {
            return danhMucLogic.InsertUpdateDanhMucLinQ(madm, tendm);
        }
        [WebMethod]
        public bool DanhMucDelete(string madm)
        {
            return danhMucLogic.DeleteDanhMucLinQ(madm);
        }


        SanPhamLogic sanPhamLogic = new SanPhamLogic();
        [WebMethod]                 
        public int SanPhamCount(string madm, string fromPrice, string toPrice)
        {       //trả về tổng số lượng sản phẩm theo 1 mã danh mục nào đó có zá từ fromPrice đến toPrice
            if (toPrice == "") toPrice = sanPhamLogic.QuerySanPhamLinQ(madm, "", "").Max(sp => sp.DonGia) + "";
            return sanPhamLogic.QuerySanPhamLinQ(madm, (fromPrice == "") ? "0" : fromPrice, toPrice).Count();
        }
        [WebMethod]
        public List<SanPhamNoLinq> SanPhams(string madm, string fromPrice, string toPrice)
        {
            if (toPrice == "") toPrice = sanPhamLogic.QuerySanPhamLinQ(madm, "", "").Max(sp => sp.DonGia) + "";
            return sanPhamLogic.QuerySanPhamLinQ(madm, (fromPrice == "") ? "0" : fromPrice, toPrice);
        }
        [WebMethod]
        public SanPhamNoLinq SanPhamDetail(string masp)
        {
            return masp == "" ? null : sanPhamLogic.QuerySanPhamLinQ("", "", "").FirstOrDefault(sp => sp.MaSp == Int32.Parse(masp));
        }
        [WebMethod]
        public bool SanPhamInsertUpdate(string codesp,string namesp, string price, string codedm)
        {
            return sanPhamLogic.InsertUpdateSanPhamLinQ(codesp, namesp, price, codedm);
        }
        [WebMethod]
        public bool SanPhamDelete(string masp)
        {
            return sanPhamLogic.DeleteSanPhamLinQ(masp);
        }
    }
}
