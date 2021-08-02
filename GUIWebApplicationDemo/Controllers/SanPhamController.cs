using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataTransferObject;
using BusinessLogicLayer;

namespace GUIWebApplicationDemo.Controllers
{
    public class SanPhamController : ApiController
    {
        SanPhamLogic sanPhamLogic = new SanPhamLogic();
        [HttpGet]       //lấy toàn bộ list sản phẩm
        public List<SanPhamNoLinq> AllSanPhams()
        {
            return sanPhamLogic.QuerySanPhamLinQ("", "", "");
        }
        [HttpGet]       //id: mã dm, lấy list sp theo madm
        public List<SanPhamNoLinq> SanPhams(int madm) 
        {
            return sanPhamLogic.QuerySanPhamLinQ(madm + "", "", "");
        }
        [HttpGet]       //lấy list sp có zá từ fromPrice đến toPrice
        public List<SanPhamNoLinq> SanPhams(int fromPrice, int toPrice)   
        {
            return sanPhamLogic.QuerySanPhamLinQ("", fromPrice + "", toPrice + "");
        }
        [HttpGet]       //lấy list sp theo madm có zá từ fromPrice đến toPrice
        public List<SanPhamNoLinq> SanPhams(int madm, int fromPrice, int toPrice)
        {
            return sanPhamLogic.QuerySanPhamLinQ(madm + "", fromPrice + "", toPrice + "");
        }
        [HttpGet]       //id: mã SP, lấy 1 sp cụ thể theo masp (id)
        public SanPhamNoLinq SanPhamDetail(int id)
        {
            return sanPhamLogic.QuerySanPhamLinQ("", "", "").FirstOrDefault(sp => sp.MaSp == id);
        }
        [HttpPost]      //insert 1 san pham
        public bool SanPhamInsert(int masp, string tensp, int price, int madm)
        {
            return sanPhamLogic.InsertUpdateSanPhamLinQ(masp + "", tensp, price + "", madm + "");
        }
        [HttpPut]       //update 1 sản phẩm
        public bool SanPhamUpdate(int masp, string tensp, int price, int madm)
        {
            return sanPhamLogic.InsertUpdateSanPhamLinQ(masp + "", tensp, price + "", madm + "");
        }
        [HttpDelete]    //delete 1 sản phẩm
        public bool SanPhamDelete(int masp)
        {
            return sanPhamLogic.DeleteSanPhamLinQ(masp + "");
        }
    }
}
