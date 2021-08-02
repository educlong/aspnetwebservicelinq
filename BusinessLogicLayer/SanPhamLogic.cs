using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataTransferObject;
namespace BusinessLogicLayer
{
    public class SanPhamLogic
    {
        SanPhamAccess sanPhamAccess = new SanPhamAccess();
        DanhMucAccess danhMucAccess = new DanhMucAccess();
        public List<SanPhamNoLinq> QuerySanPham(string madm)
        {
            return (madm == "") 
                ? sanPhamAccess.QueryAllSanPham(3)  //cách 1 or 3
                //: sanPhamAccess.QueryAllSanPhamTheoMaDm(madm,3);    //cách 1 or 2 or 3

                    /* LINQ:
                     * FindAll trong LINQ*/
                    /* Cách 2: (LINQ) Dùng FindAll để tìm tất cả sp có mã dm = madm này có trong ds hay k*/
                : sanPhamAccess.QueryAllSanPham(3).FindAll(sp => sp.MaDm == Int32.Parse(madm));
        }
        public DataTable QueryAllSanPhamSqlDataAdapter()
        {
            return sanPhamAccess.QuerySanPham_SqlDataAdapter();     //cách 4
        }

        public string CountSanPham(string maDm)
        {
            return "Số lượng tất cả sản phẩm" + ((maDm == "") ? "" : (" trong danh mục có mã " + maDm)) + " = " 
               + (maDm == "" ? sanPhamAccess.CountAllSanPham() : sanPhamAccess.CountAllSanPhamTheoMaDm(maDm,1)); //cách 1or2
        }
        public string QuerySanPhamTheoMaSp(string maSp)
        {
            SanPhamNoLinq sanPham = sanPhamAccess.QuerySanPhamTheoMaSp(maSp,3);   //cách 1 or 2 or 3
            //return (sanPham == null) ? "Không có sản phẩm này trong danh sách. " : sanPham.ToString();

            /* LINQ:
             * Exist trong LINQ*/
            /* Cách 2: (LINQ) Dùng Exist để kiểm tra xem sp này có trong ds hay k*/
            bool resultExist = QuerySanPham("").Exists(sp => sp.MaSp == Int32.Parse(maSp));
            //return resultExist ? sanPham.ToString() : "Không có sản phẩm này trong danh sách. ";

            /* LINQ:
             * Find và FindIndex trong LINQ*/
            /* Cách 3: (LINQ) Dùng Find để tìm xem sp này có trong ds hay k               ==> Find     : trả về zá trị đầu tiên tìm thấy
             *                Dùng FindIndex để lấy ra vị trí của sản phẩm trong danh mục ==> FindIndex: trả về vị trí đầu tiên tìm thấy
             *                
             *  Ngoài ra có thể dùng hàm FindLast (trả về zá trị cuối cùng tìm thấy) và FindLastIndex(trả về vị trí cuối cùng tìm thấy)
             */
            SanPhamNoLinq resultFind = QuerySanPham("").Find(sp => sp.MaSp == Int32.Parse(maSp));
            return resultFind!=null 
                ? (resultFind.ToString() +", tìm thấy tại vị trí thứ "+                     // Find
                        (QuerySanPham("").FindIndex(sp=> sp.MaSp==Int32.Parse(maSp))+1))    // FindIndex
                : "Không có sản phẩm này trong danh sách. ";
        }
        public string InsertUpdateSanPham(string maSP, string tenSp, string donZa, string maDm)
        {
            DanhMucNoLinq danhMuc = danhMucAccess.QueryDanhMucTheoMaDm(maDm);
            if (danhMuc == null) return CountSanPham(maDm);
            else
            {
                SanPhamNoLinq sanPham = sanPhamAccess.QuerySanPhamTheoMaSp(maSP,3);                      //cách 1 or 2 or 3
                int result = (sanPham == null) ? sanPhamAccess.InsertSanPham(maSP, tenSp, donZa, maDm,3) //cách 1 or 2 or 3
                    : sanPhamAccess.UpdateSanPham(maSP, tenSp, donZa, maDm,3);                           //cách 1 or 2 or 3
                return ((result <= 0) ? "Save thất bại. " : "Save thành công. ") + CountSanPham(maDm);
            }
        }
        public string InsertSanPhamSqlDataAdapter(string maSP, string tenSp, string donZa, string maDm)
        {
            DanhMucNoLinq danhMuc = danhMucAccess.QueryDanhMucTheoMaDm(maDm);
            if (danhMuc == null) return CountSanPham(maDm);
            else
            {
                SanPhamNoLinq sanPham = sanPhamAccess.QuerySanPhamTheoMaSp(maSP, 3);                          //cách 1 or 2 or 3
                int result=(sanPham==null)?sanPhamAccess.InsertSanPham_SqlDataAdapter(maSP,tenSp,donZa,maDm)  //cách 4
                    : sanPhamAccess.UpdateSanPham(maSP, tenSp, donZa, maDm, 2);                           //cách 1 or 2 or 3
                return ((result  <= 0) ? "Save thất bại. " : "Save thành công. ") + CountSanPham(maDm);
            }
        }

        public string DeleteSanPham(string masp, string madm)
        {
            return (sanPhamAccess.DeleteSanPham(masp,1) <= 0 ? "Delete Fail. " : "Delete success. ") + CountSanPham(madm);
        }

        /*LINQ => OfType*/
        /*Nếu 1 đối tượng đc KẾ THỪA => Muốn lấy kiểu data của đối tượng => dùng OfType
         * VD: NhanVienChinhThuc và NhanVienThoiVu đc kế thừa từ NhanVien
         * => chỉ lọc ra nhân viên chính thức: 
         * var ds = dsGốc.OfType<NhanVienChinhThuc>();  //sau đó đưa ds này lên zao diện*/




        /*___________________________________ TƯƠNG TÁC VỚI CSDL THÔNG QUA LINQ __________________________*/

        /*Convert qua, nếu k convert thì phải set references cho GUI, add thêm DataTransferObject vào GUI*/
        private List<SanPhamNoLinq> ConvertSanPhamLinQ(List<SanPham> sanPhams)
        {
            List<SanPhamNoLinq> sanPhamNoLinqs = new List<SanPhamNoLinq>();
            foreach (SanPham sanPham in sanPhams)
            {
                SanPhamNoLinq sanPhamNoLinq=new SanPhamNoLinq
                    { MaSp = sanPham.MaSp, TenSp = sanPham.TenSp, DonGia = (int)sanPham.DonGia, MaDm = (int)sanPham.MaDm };
                sanPhamNoLinqs.Add(sanPhamNoLinq);
            }
            return sanPhamNoLinqs;
        }

        //Query tất cả sản phẩm có mã danh mục = madm và khoảng zá từ priceFrom đến priceTo
        public List<SanPhamNoLinq> QuerySanPhamLinQ(string madm, string priceFrom, string priceTo) 
        {
            List<SanPham> sanPhams = (priceFrom == "" && priceTo == "")
                ? sanPhamAccess.QuerySanPhamLinQ(madm)

                /*Cách 1: Dùng method Syntax*/
                //: sanPhamAccess.QuerySanPhamLinQ(madm)
                //    .Where(sp => (sp.DonGia >= Int32.Parse(priceFrom) && sp.DonGia <= Int32.Parse(priceTo))).ToList();

                /*Cách 2: Dùng Query Syntax*/
                : (from sps in sanPhamAccess.QuerySanPhamLinQ(madm)
                        where sps.DonGia >= Int32.Parse(priceFrom) && sps.DonGia <= Int32.Parse(priceTo) select sps).ToList();

            return ConvertSanPhamLinQ(sanPhams);
        }
        public bool InsertUpdateSanPhamLinQ(string codesp, string namesp, string price, string codedm)    //insert/update 1 sản phẩm
        {
            return sanPhamAccess.InsertUpdateSanPhamLinQ(codesp, namesp, price, codedm);
        }
        public bool DeleteSanPhamLinQ(string masp)      //delete 1 sản phẩm
        {
            return sanPhamAccess.DeleteSanPhamLinQ(masp);
        }
    }
}
