using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DataTransferObject;

namespace DataAccessLayer
{
    public class SanPhamAccess: DatabaseAccess
    {
        private SqlCommand sqlCommand;

        public SanPhamAccess() { }

        /** Cách 1: Cách tương tác với CSDL thông qua biến zá trị*/
        public List<SanPhamNoLinq> QueryAllSanPham(int phuongThucTuongTacVoiCsdl)  /*query tất cả sản phẩm*/
        {
            List<SanPhamNoLinq> sanPhams = new List<SanPhamNoLinq>();
            OpenConnection();
            this.sqlCommand = new SqlCommand();
            if (phuongThucTuongTacVoiCsdl == 1)
            {
                this.sqlCommand.CommandType = CommandType.Text;             /** Cách 1: Cách tương tác với CSDL */
                this.sqlCommand.CommandText = "Select *from SanPham";       /** thông qua biến zá trị*/
            }
            if (phuongThucTuongTacVoiCsdl == 3)
            {
                this.sqlCommand.CommandType = CommandType.StoredProcedure;  /** Cách 3: Cách tương tác CSDL */
                this.sqlCommand.CommandText = "QueryAllSanPham";            /** thông qua Store Procedures*/
            }
            this.sqlCommand.Connection = sqlConnection;
            SqlDataReader sqlDataReader = this.sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
                sanPhams.Add(new SanPhamNoLinq(){     /*Đây là cách sử dụng Object Initializers để khởi tạo object trong LinQ*/
                    MaSp = sqlDataReader.GetInt32(0), TenSp = sqlDataReader.GetString(1),
                    DonGia = sqlDataReader.GetInt32(2), MaDm = sqlDataReader.GetInt32(3)
                });
            sqlDataReader.Close();
            CloseConnection();
            return sanPhams;
        }       

        /*Có thể k cần method này, chỉ cần dùng FindAll trong LINQ là đủ*/
        public List<SanPhamNoLinq> QueryAllSanPhamTheoMaDm(string maDm, int phuongThucTuongTacVoiCsdl)  /*query tất cả sản phẩm theo mã danh mục*/
        {
            List<SanPhamNoLinq> sanPhams = new List<SanPhamNoLinq>();
            OpenConnection();
            this.sqlCommand = new SqlCommand();
            if (phuongThucTuongTacVoiCsdl == 1)
            {
                this.sqlCommand.CommandType = CommandType.Text; /** Cách 1: Cách tương tác với CSDL thông qua biến zá trị*/
                this.sqlCommand.CommandText = "Select *from SanPham where MaDm=" + maDm;
                this.sqlCommand.Connection = sqlConnection;
            }
            if (phuongThucTuongTacVoiCsdl == 2)
            {
                this.sqlCommand.CommandType = CommandType.Text; /** Cách 2: Cách tương tác CSDL thông qua Parameter*/
                this.sqlCommand.CommandText = "Select *from SanPham where MaDm=@maquery";
                this.sqlCommand.Connection = sqlConnection;
                this.sqlCommand.Parameters.Add("@maquery", SqlDbType.Int).Value = maDm;
            }
            if (phuongThucTuongTacVoiCsdl == 3)
            {
                this.sqlCommand.CommandType = CommandType.StoredProcedure; /** Cách 3: Tương tác CSDL Store Procedures*/
                this.sqlCommand.CommandText = "QuerySanPhamTheoMaDm";
                this.sqlCommand.Connection = sqlConnection;
                this.sqlCommand.Parameters.Add("@maqueryproc", SqlDbType.Int).Value = maDm;
            }
            SqlDataReader sqlDataReader = this.sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
                sanPhams.Add(new SanPhamNoLinq(){ /*Đây là cách sử dụng Object Initializers để khởi tạo object trong LinQ*/
                    MaSp=sqlDataReader.GetInt32(0),TenSp=sqlDataReader.GetString(1),
                    DonGia=sqlDataReader.GetInt32(2),MaDm=sqlDataReader.GetInt32(3)});
            sqlDataReader.Close();
            CloseConnection();
            return sanPhams;
        }

        public int CountAllSanPham()    /*đếm tất cả sản phẩm*/
        {
            OpenConnection();
            this.sqlCommand = new SqlCommand();
            this.sqlCommand.CommandType = CommandType.Text;
            this.sqlCommand.CommandText = "Select count(*) from SanPham";
            this.sqlCommand.Connection = sqlConnection;
            object data = this.sqlCommand.ExecuteScalar();
            CloseConnection();
            return (int)data;
        }
        public int CountAllSanPhamTheoMaDm(string maDm, int phuongThucTuongTacVoiCsdl) /*đếm tất cả sản phẩm của 1 danh mục nào đó*/
        {
            OpenConnection();
            this.sqlCommand = new SqlCommand();
            if (phuongThucTuongTacVoiCsdl == 1)
            {
                this.sqlCommand.CommandType = CommandType.Text; /** Cách 1: Cách tương tác với CSDL thông qua biến zá trị*/
                this.sqlCommand.CommandText = "Select count(*) from SanPham where MaDm=" + maDm;
                this.sqlCommand.Connection = sqlConnection;
            }
            if (phuongThucTuongTacVoiCsdl == 2)
            {
                this.sqlCommand.CommandType = CommandType.Text; /** Cách 2: Cách tương tác CSDL thông qua Parameter*/
                this.sqlCommand.CommandText = "Select count(*) from SanPham where MaDm=@macount";
                this.sqlCommand.Connection = sqlConnection;
                this.sqlCommand.Parameters.Add("@macount", SqlDbType.Int).Value = maDm;
            }
            object data = this.sqlCommand.ExecuteScalar();
            CloseConnection();
            return (int)data;
        }

        /*Có thể k cần method này, chỉ cần dùng Find hoặc Exits trong LINQ là đủ*/
        public SanPhamNoLinq QuerySanPhamTheoMaSp(string maSp, int phuongThucTuongTacVoiCsdl) /*query sản phầm nào đó theo mã san phẩm*/
        {
            SanPhamNoLinq sanPham = null;
            OpenConnection();
            this.sqlCommand = new SqlCommand();
            if (phuongThucTuongTacVoiCsdl == 1)
            {
                this.sqlCommand.CommandType = CommandType.Text; /** Cách 1: Cách tương tác với CSDL thông qua biến zá trị*/
                this.sqlCommand.CommandText = "Select *from SanPham where MaSp=" + maSp;
                this.sqlCommand.Connection = sqlConnection;
            }
            if (phuongThucTuongTacVoiCsdl == 2)
            {
                this.sqlCommand.CommandType = CommandType.Text; /** Cách 2: Cách tương tác CSDL thông qua Parameter*/
                this.sqlCommand.CommandText = "Select *from SanPham where MaSp=@maspquery";
                this.sqlCommand.Connection = sqlConnection;
                this.sqlCommand.Parameters.Add("@maspquery", SqlDbType.Int).Value = maSp;
            }
            if (phuongThucTuongTacVoiCsdl == 3)
            {
                this.sqlCommand.CommandType = CommandType.StoredProcedure; /** Cách 3: Tương tác CSDL Store Procedures*/
                this.sqlCommand.CommandText = "QuerySanPhamTheoMaSp";
                this.sqlCommand.Connection = sqlConnection;
                this.sqlCommand.Parameters.Add("@maspqueryproc", SqlDbType.Int).Value = maSp;
            }
            SqlDataReader sqlDataReader = this.sqlCommand.ExecuteReader();
            if (sqlDataReader.Read())
                sanPham = new SanPhamNoLinq(){        /*Đây là cách sử dụng Object Initializers để khởi tạo object trong LinQ*/
                    MaSp = sqlDataReader.GetInt32(0),TenSp= sqlDataReader.GetString(1),
                    DonGia= sqlDataReader.GetInt32(2),MaDm= sqlDataReader.GetInt32(3)};
            sqlDataReader.Close();
            CloseConnection();
            return sanPham;
        }
        
        public int InsertSanPham(string maSp, string tenSp, string donZa, string maDm, int phuongThucTuongTacVoiCsdl)  /*insert*/
        {
            OpenConnection();
            this.sqlCommand = new SqlCommand();
            if (phuongThucTuongTacVoiCsdl == 1)
            {
                this.sqlCommand.CommandType = CommandType.Text; /** Cách 1: Cách tương tác với CSDL thông qua biến zá trị*/
                this.sqlCommand.CommandText = "Insert into SanPham(MaSp, TenSp, DonGia, MaDm) values ("
                        + maSp + ",N'" + tenSp + "'," + donZa + "," + maDm + ")";
                this.sqlCommand.Connection = sqlConnection;
            }
            if (phuongThucTuongTacVoiCsdl == 2)
            {
                this.sqlCommand.CommandType = CommandType.Text; /** Cách 2: Cách tương tác CSDL thông qua Parameter*/
                this.sqlCommand.CommandText = "Insert into SanPham(MaSp, TenSp, DonGia, MaDm) values" +
                                            " (@maspinsert,@tenspinsert,@donzaspinsert,@madminsert)";
                this.sqlCommand.Connection = sqlConnection;
                this.sqlCommand.Parameters.Add("@maspinsert", SqlDbType.Int).Value = maSp;
                this.sqlCommand.Parameters.Add("@tenspinsert", SqlDbType.NVarChar).Value = tenSp;
                this.sqlCommand.Parameters.Add("@donzaspinsert", SqlDbType.Int).Value = donZa;
                this.sqlCommand.Parameters.Add("@madminsert", SqlDbType.Int).Value = maDm;
            }
            if (phuongThucTuongTacVoiCsdl == 3)
            {
                this.sqlCommand.CommandType = CommandType.StoredProcedure; /** Cách 3: Tương tác CSDL Store Procedures*/
                this.sqlCommand.CommandText = "InsertSanPham";
                this.sqlCommand.Connection = sqlConnection;
                this.sqlCommand.Parameters.Add("@mainsertproc", SqlDbType.Int).Value = maSp;
                this.sqlCommand.Parameters.Add("@nameinsertproc", SqlDbType.NVarChar).Value = tenSp;
                this.sqlCommand.Parameters.Add("@giainsertproc", SqlDbType.Int).Value = donZa;
                this.sqlCommand.Parameters.Add("@madminsertproc", SqlDbType.Int).Value = maDm;
            }
            int result = sqlCommand.ExecuteNonQuery();    /*return > 0 -> thành công, else -> thêm thất bại*/
            CloseConnection();
            return result;
        }

        public int UpdateSanPham(string maSp, string tenSp, string donZa, string maDm, int phuongThucTuongTacVoiCsdl)/*update*/
        {
            OpenConnection();
            this.sqlCommand = new SqlCommand();
            if (phuongThucTuongTacVoiCsdl == 1)
            {
                this.sqlCommand.CommandType = CommandType.Text; /** Cách 1: Cách tương tác với CSDL thông qua biến zá trị*/
                this.sqlCommand.CommandText = "Update SanPham set TenSp=N'" + tenSp + "', DonGia="
                                                + donZa + ", MaDm=" + maDm + " where MaSp=" + maSp;
                this.sqlCommand.Connection = sqlConnection;
            }
            if (phuongThucTuongTacVoiCsdl == 2)
            {
                this.sqlCommand.CommandType = CommandType.Text; /** Cách 2: Cách tương tác CSDL thông qua Parameter*/
                this.sqlCommand.CommandText = "Update SanPham set TenSp=@tenspupdate," +
                                             " DonGia=@donzaupdate, MaDm=@madmupdate where MaSp=@maspupdate";
                this.sqlCommand.Connection = sqlConnection;
                this.sqlCommand.Parameters.Add("@tenspupdate", SqlDbType.NVarChar).Value = tenSp;
                this.sqlCommand.Parameters.Add("@donzaupdate", SqlDbType.Int).Value = donZa;
                this.sqlCommand.Parameters.Add("@madmupdate", SqlDbType.Int).Value = maDm;
                this.sqlCommand.Parameters.Add("@maspupdate", SqlDbType.Int).Value = maSp;
            }
            if (phuongThucTuongTacVoiCsdl == 3)
            {
                this.sqlCommand.CommandType = CommandType.StoredProcedure; /** Cách 3: Tương tác CSDL Store Procedures*/
                this.sqlCommand.CommandText = "UpdatePriceSanPham";
                this.sqlCommand.Connection = sqlConnection;
                this.sqlCommand.Parameters.Add("@tenspupdateproc", SqlDbType.NVarChar).Value = tenSp;
                this.sqlCommand.Parameters.Add("@dongiaupdateproc", SqlDbType.Int).Value = donZa;
                this.sqlCommand.Parameters.Add("@madmupdateproc", SqlDbType.Int).Value = maDm;
                this.sqlCommand.Parameters.Add("@maspupdateproc", SqlDbType.Int).Value = maSp;
            }
            int result = sqlCommand.ExecuteNonQuery();    /*return > 0 -> thành công, else -> thêm thất bại*/
            CloseConnection();
            return result;
        }

        public int DeleteSanPham(string maSp, int phuongThucTuongTacVoiCsdl)   /*Delete 1 san pham*/
        {
            OpenConnection();
            this.sqlCommand = new SqlCommand();
            if (phuongThucTuongTacVoiCsdl == 1)
            {
                this.sqlCommand.CommandType = CommandType.Text; /** Cách 1: Cách tương tác với CSDL thông qua biến zá trị*/
                this.sqlCommand.CommandText = "Delete from SanPham where MaSp=" + maSp;
                this.sqlCommand.Connection = sqlConnection;
            }
            if (phuongThucTuongTacVoiCsdl == 2)
            {
                this.sqlCommand.CommandType = CommandType.Text; /** Cách 2: Cách tương tác CSDL thông qua Parameter*/
                this.sqlCommand.CommandText = "Delete from SanPham where MaSp=@maspdelete";
                this.sqlCommand.Connection = sqlConnection;
                this.sqlCommand.Parameters.Add("@maspdelete", SqlDbType.Int).Value = maSp;
            }
            int result = sqlCommand.ExecuteNonQuery();
            CloseConnection();
            return result;  /*return > 0 -> thành công, else -> thêm thất bại*/
        }


        /** Cách 4: Tương tác SQL sử dụng SqlDataAdapter (k cẩn mở đóng zì cả)*/
        private SqlDataAdapter dataAdapter = null;
        private DataSet dataSet = null;
        private DataSet QueryAllSanPham_SqlDataAdapter()      /** Cách 4: Sử dụng SqlDataAdapter (k cẩn mở đóng zì cả)*/
        {   /*dữ liệu ít thì nên dùng sqlDataAdapter, còn nhiều quá thì nên đọc từng dòng dùng dataReader.*/
            if (sqlConnection == null)
                sqlConnection = new SqlConnection(strConnection);
            dataAdapter = new SqlDataAdapter("Select * from SanPham", sqlConnection);
            SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(dataAdapter);   //dùng cho insert, update, delete
            this.dataSet = new DataSet();                //dataset là chứa tập các bảng, mà ở đây chỉ có 1 bảng
            dataAdapter.Fill(this.dataSet, "SanPham");           //đặt tên cho bảng này là "SanPham"
            return this.dataSet;
        }
        public DataTable QuerySanPham_SqlDataAdapter()
        {
            return QueryAllSanPham_SqlDataAdapter().Tables["SanPham"];       //lấy bảng SanPham ra
        }
        public int InsertSanPham_SqlDataAdapter(string maSp, string tenSp, string donZa, string maDm)
        {
            DataRow row = QuerySanPham_SqlDataAdapter().NewRow();   //di chuyển đến dòng cuối cùng để thêm dòng mới
            row["MaSp"] = maSp;
            row["TenSp"] = tenSp;
            row["DonGia"] = donZa;
            row["MaDm"] = maDm;
            this.dataSet.Tables["SanPham"].Rows.Add(row);    //add row và bảng
            return dataAdapter.Update(this.dataSet.Tables["SanPham"]);//update để cập nhật data, trả về số dòng bị ảnh hưởng
        }
        public int UpdateSanPham_SqlDataAdapter(string maSp, string tenSp, string donZa, string maDm, int position)
        {
            DataRow row = QuerySanPham_SqlDataAdapter().Rows[position];
            row.BeginEdit();
            row["MaSp"] = maSp;
            row["TenSp"] = tenSp;
            row["DonGia"] = donZa;
            row["MaDm"] = maDm;
            row.EndEdit();
            return dataAdapter.Update(this.dataSet.Tables["SanPham"]);//update để cập nhật data, trả về số dòng bị ảnh hưởng
        }
        public int DeleteSanPham_SqlDataAdapter(int position)
        {
            DataRow row = QuerySanPham_SqlDataAdapter().Rows[position];
            row.Delete();
            return dataAdapter.Update(this.dataSet.Tables["SanPham"]);//update để cập nhật data, trả về số dòng bị ảnh hưởng
        }



        /*___________________________________ TƯƠNG TÁC VỚI CSDL THÔNG QUA LINQ __________________________*/

        public List<SanPham> QuerySanPhamLinQ(string madm)              //Query tất cả sản phẩm theo mã danh mục
        {
            Db_ProductManagementDataContext context = new Db_ProductManagementDataContext();

            return (madm=="")/*nếu madm = "" thì lấy hết sanphams, ngược lại nếu có madm thì chỉ lấy sanpham thuộc madm đó thôi*/
                ? (from sps in context.SanPhams select sps).ToList()                            /*Cách 1: Dùng Query Syntax*/
                //: (context.SanPhams.ToList()    .FindAll(sp => sp.MaDm == Int32.Parse(madm)));  /*Cách 2: Dùng Method Syntax*/
                : (context.SanPhams.Where(sps => sps.MaDm == Int32.Parse(madm)).ToList());      /*Dùng FindAll hoặc dùng Where*/
        }

        private bool CheckMaSp(string masp) /*nếu = null, tức là k có masp này, thì trả về false, nếu có trả về true*/
        {
            if (masp == "") return false;   /*FirstOrDefault hoặc dùng Find để trả về 1 đối tượng cụ thể*/
            return (QuerySanPhamLinQ("").FirstOrDefault(sp => sp.MaSp == Int32.Parse(masp)) == null) ? false : true;  //existed->true
        }

        public bool InsertUpdateSanPhamLinQ(string masp, string namesp, string price, string madm)    //insert và update 1 sanPham
        {
            Db_ProductManagementDataContext context = new Db_ProductManagementDataContext();
            try
            {
                DanhMuc danhMuc = context.DanhMucs.Where(dm => dm.MaDm == Int32.Parse(madm)).ToList().FirstOrDefault();
                if (danhMuc.MaDm != Int32.Parse(madm)) return false;    //nếu mã danh mục chưa tồn tại thì k đc insert hay update zì cả
            }
            catch (Exception ex)
            {
                return false;   //nếu k truy vấn đc mã danh mục (mã danh mục này chưa tồn tại) thì trả về false
            }

            if (CheckMaSp(masp))    //nếu mã này đã tồn tại thì update
            {
                try
                {
                    SanPham sanPham = context.SanPhams.FirstOrDefault(sp => sp.MaSp == Int32.Parse(masp));
                    if (sanPham != null)
                    {
                        sanPham.TenSp = namesp;
                        sanPham.DonGia = Int32.Parse(price);
                        sanPham.MaDm = (madm == "") ? sanPham.MaDm : Int32.Parse(madm);
                        context.SubmitChanges();
                        return true;
                    }
                }
                catch { }
                return false;
            }
            else                    //nếu mã này đã tồn tại thì insert
            {
                try
                {
                    context.SanPhams.InsertOnSubmit(    /*Nếu muốn lưu 1list => thay vì gọi InsertOnSubmit thì gọi InsertAllOnSubmit*/
                        new SanPham { MaSp = int.Parse(masp), TenSp = namesp, DonGia = int.Parse(price), MaDm = int.Parse(madm) });
                    context.SubmitChanges();
                    return true;
                }
                catch { }
                return false;
            }
        }
        public bool DeleteSanPhamLinQ(string masp)
        {
            Db_ProductManagementDataContext context = new Db_ProductManagementDataContext();
            SanPham sanPham = context.SanPhams.FirstOrDefault(sp => sp.MaSp == Int32.Parse(masp));
            if (sanPham != null)
            {
                context.SanPhams.DeleteOnSubmit(sanPham);
                context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}
