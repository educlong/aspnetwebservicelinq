using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataTransferObject;
namespace BusinessLogicLayer
{
    public class DanhMucLogic
    {
        DanhMucAccess danhMucAccess = new DanhMucAccess();
        public List<DanhMucNoLinq> QueryDanhMuc()
        {
            //return danhMucAccess.QueryDanhMuc();              /** Cách 1: Cách tương tác với CSDL thông qua biến zá trị*/
            return danhMucAccess.QueryAllDanhMucStoreProcedures();/*Cách 3: Cách tương tác CSDL thông qua Store Procedures*/
        }
        public string CountDanhMuc()
        {
            return "Số lượng danh mục  = "+danhMucAccess.CountDanhMuc();
        }
        public string QueryDanhMucTheoMaDm(string maDm)
        {
            //return danhMucAccess.DanhMucService(maDm);
            /** Cách 1: Cách tương tác với CSDL thông qua biến zá trị*/
            DanhMucNoLinq danhMuc = danhMucAccess.QueryDanhMucTheoMaDm(maDm);
            /** Cách 2: Cách tương tác CSDL thông qua Parameter => cách này sẽ tối ưu hơn cách trên, tránh bị hack*/
            DanhMucNoLinq danhMucPara = danhMucAccess.QueryDanhMucTheoMaDmParameter(maDm);

            //var x = from dm in danhMucAccess.QueryDanhMuc() where dm == danhMucAccess.QueryDanhMucTheoMaDm(maDm) select dm;
            //return (danhMucPara == null) ? "Không có trong danh mục" : (x as DanhMuc).ToString();
            
            return (danhMucPara == null) ? "Không có trong danh mục" : danhMucPara.ToString();
        }
        public string InsertUpdateDanhMuc(string maDm, string tenDm)
        {
            /** Cách 1: Cách tương tác với CSDL thông qua biến zá trị*/
            DanhMucNoLinq danhMuc1 = danhMucAccess.QueryDanhMucTheoMaDm(maDm);
            int resultSave1 = (danhMuc1 == null) ? 
                danhMucAccess.InsertDanhMuc(maDm, tenDm) : danhMucAccess.UpdateDanhMuc(maDm, tenDm);
            /** Cách 2: Cách tương tác CSDL thông qua Parameter => cách này sẽ tối ưu hơn cách trên, tránh bị hack*/
            DanhMucNoLinq danhMuc2 = danhMucAccess.QueryDanhMucTheoMaDmParameter(maDm);
            int resultSave2 = (danhMuc2 == null) ?
                danhMucAccess.InsertDanhMucParameter(maDm, tenDm) : danhMucAccess.UpdateDanhMucParameter(maDm, tenDm);
            
            return (resultSave2 <= 0 ? "Save thất bại. " : "Save thành công. ") + CountDanhMuc();
        }
        public string DeleteDanhMuc(string maDm)
        {
            return ((danhMucAccess.DeleteDanhMucParameter(maDm) <= 0    /** Cách 1: Tương tác với CSDL biến zá trị*/
                || danhMucAccess.DeleteDanhMuc(maDm) <= 0)              /** Cách 2: Tương tác CSDL Parameter*/
                ? "Delete fail. " : "Delete success.") + CountDanhMuc();
        }

        /*___________________________________ TƯƠNG TÁC VỚI CSDL THÔNG QUA LINQ __________________________*/

        /*Convert qua, nếu k convert thì phải set references cho GUI, add thêm DataTransferObject vào GUI*/
        private List<DanhMucNoLinq> ConvertDanhMucLinQ(List<DanhMuc> danhMucs)
        {
            List<DanhMucNoLinq> danhMucNoLinqs = new List<DanhMucNoLinq>();
            foreach(DanhMuc danhMuc in danhMucs)
                danhMucNoLinqs.Add(new DanhMucNoLinq { MaDm = danhMuc.MaDm, TenDm = danhMuc.TenDm });
            return danhMucNoLinqs;
        }

        public List<DanhMucNoLinq> QueryDanhMucLinQ(string madm)
        {
            List<DanhMuc> danhMucs= danhMucAccess.QueryDanhMucLinQ(madm);
            return ConvertDanhMucLinQ(danhMucs);
        }
        
        public bool InsertUpdateDanhMucLinQ(string madm, string namedm)
        {
             return danhMucAccess.InsertUpdateDanhMucLinQ(madm, namedm);
        }
        public bool DeleteDanhMucLinQ(string madm)
        {
            return danhMucAccess.DeleteDanhMucLinQ(madm);
        }
    }
}
