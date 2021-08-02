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
    public class DanhMucController : ApiController
    {
        DanhMucLogic danhMucLogic = new DanhMucLogic();
        [HttpGet]       //lấy toàn bộ danh mục
        public List<DanhMucNoLinq> AllDanhMuc()
        {
            return danhMucLogic.QueryDanhMucLinQ("");
        }
        [HttpGet]       //lấy danh mục theo mã dm (id)
        public DanhMucNoLinq DanhMucDetail(int id)
        {
            return danhMucLogic.QueryDanhMucLinQ(id + "").FirstOrDefault();
        }
        [HttpPost]      //insert 1 danh mục
        public bool DanhMucInsert(int madm, string namedm)
        {
            return danhMucLogic.InsertUpdateDanhMucLinQ(madm + "", namedm);
        }
        [HttpPut]       //update 1 danh mục
        public bool DanhMucUpdate(int madm, string namedm)
        {
            return danhMucLogic.InsertUpdateDanhMucLinQ(madm + "", namedm);
        }
        [HttpDelete]    //delete 1 danh mục
        public bool DanhMucDelete(int madm)
        {
            return danhMucLogic.DeleteDanhMucLinQ(madm + "");
        }
    }
}
