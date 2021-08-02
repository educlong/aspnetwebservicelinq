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
    public class DanhMucAccess: DatabaseAccess
    {
        private SqlCommand sqlCommand;
        public DanhMucAccess()
        {
            this.sqlCommand = new SqlCommand();
        }

        /** Cách 1: Cách tương tác với CSDL thông qua biến zá trị*/
        public List<DanhMucNoLinq> QueryDanhMuc()     /*trả về tất cả các danh mục*/
        {
            List<DanhMucNoLinq> danhMucs = new List<DanhMucNoLinq>();
            OpenConnection();
            this.sqlCommand.CommandType = CommandType.Text;
            this.sqlCommand.CommandText = "Select *from DanhMuc";
            this.sqlCommand.Connection = sqlConnection;
            SqlDataReader sqlDataReader = this.sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
                danhMucs.Add(new DanhMucNoLinq(){/*Đây là cách sử dụng Object Initializers để khởi tạo object trong LinQ*/
                    MaDm= sqlDataReader.GetInt32(0), TenDm= sqlDataReader.GetString(1) });
            sqlDataReader.Close();
            CloseConnection();
            return danhMucs;
        }
        public int CountDanhMuc()   /*đếm xem có tất cả bao nhiêu danh mục*/
        {
            OpenConnection();
            this.sqlCommand.CommandType = CommandType.Text;
            this.sqlCommand.CommandText = "select count(*) from DanhMuc";
            this.sqlCommand.Connection = sqlConnection;
            object data = this.sqlCommand.ExecuteScalar();
            CloseConnection();
            return (int)data;
        }
        public DanhMucNoLinq QueryDanhMucTheoMaDm(string maDm)  /*trả về danh mục theo mã danh mục*/
        {
            DanhMucNoLinq danhMuc = null;
            OpenConnection();
            this.sqlCommand.CommandType = CommandType.Text;
            this.sqlCommand.CommandText = "Select *from DanhMuc where MaDm=" + maDm;
            this.sqlCommand.Connection = sqlConnection;
            SqlDataReader sqlDataReader = this.sqlCommand.ExecuteReader();
            if (sqlDataReader.Read())   //có data
                danhMuc = new DanhMucNoLinq(){    /*Đây là cách sử dụng Object Initializers để khởi tạo object trong LinQ*/
                    MaDm= sqlDataReader.GetInt32(0), TenDm= sqlDataReader.GetString(1) };
            sqlDataReader.Close();
            CloseConnection();
            return danhMuc;
        }
        public int InsertDanhMuc(string maDm, string tenDm)   // insert danh mục
        {
            OpenConnection();
            this.sqlCommand.CommandType = CommandType.Text;
            this.sqlCommand.CommandText = "Insert into DanhMuc(MaDm, TenDm) values (" + maDm + ",N'" + tenDm + "')";
            this.sqlCommand.Connection = sqlConnection;
            int resultSave = this.sqlCommand.ExecuteNonQuery();
            CloseConnection();
            return resultSave;
        }

        public int UpdateDanhMuc(string maDm, string tenDm)   // update danh mục
        {
            OpenConnection();
            this.sqlCommand.CommandType = CommandType.Text;
            this.sqlCommand.CommandText = "Update DanhMuc set TenDm=N'" + tenDm + "' where MaDm=" + maDm;
            this.sqlCommand.Connection = sqlConnection;
            int resultSave = this.sqlCommand.ExecuteNonQuery();
            CloseConnection();
            return resultSave;
        }

        public int DeleteDanhMuc(string maDm)   // delete danh mục item
        {
            OpenConnection();
            this.sqlCommand.CommandType = CommandType.Text;
            this.sqlCommand.CommandText = "Delete from DanhMuc where MaDm=" + maDm;
            this.sqlCommand.Connection = sqlConnection;
            int result = this.sqlCommand.ExecuteNonQuery();
            CloseConnection();
            return result;
        }

        /** Cách 2: Cách tương tác CSDL thông qua Parameter => cách này sẽ tối ưu hơn cách trên, tránh bị hack*/
        public DanhMucNoLinq QueryDanhMucTheoMaDmParameter(string maDm)  /*trả về danh mục theo mã danh mục*/
        {
            DanhMucNoLinq danhMuc = null;
            OpenConnection();
            this.sqlCommand.CommandType = CommandType.Text;
            this.sqlCommand.CommandText = "Select *from DanhMuc where MaDm=@maquery";
            this.sqlCommand.Connection = sqlConnection;
            this.sqlCommand.Parameters.Add("@maquery", SqlDbType.Int).Value = maDm;
            SqlDataReader sqlDataReader = this.sqlCommand.ExecuteReader();
            if (sqlDataReader.Read())   //có data
                danhMuc = new DanhMucNoLinq(){    /*Đây là cách sử dụng Object Initializers để khởi tạo object trong LinQ*/
                    MaDm= sqlDataReader.GetInt32(0),TenDm= sqlDataReader.GetString(1) };
            sqlDataReader.Close();
            CloseConnection();
            return danhMuc;
        }
        public int InsertDanhMucParameter(string maDm, string tenDm)   // insert danh mục
        {
            OpenConnection();
            this.sqlCommand.CommandType = CommandType.Text;
            this.sqlCommand.CommandText = "Insert into DanhMuc(MaDm, TenDm) values (@mainsert, @teninsert)";
            this.sqlCommand.Connection = sqlConnection;
            this.sqlCommand.Parameters.Add("@mainsert", SqlDbType.Int).Value = maDm;
            this.sqlCommand.Parameters.Add("@teninsert", SqlDbType.NVarChar).Value = tenDm;
            int resultSave = this.sqlCommand.ExecuteNonQuery();
            CloseConnection();
            return resultSave;
        }
        public int UpdateDanhMucParameter(string maDm, string tenDm)   // update danh mục
        {
            OpenConnection();
            this.sqlCommand.CommandType = CommandType.Text;
            this.sqlCommand.CommandText = "Update DanhMuc set TenDm=@tenupdate where MaDm=@maupdate";
            this.sqlCommand.Connection = sqlConnection;
            this.sqlCommand.Parameters.Add("@tenupdate", SqlDbType.NVarChar).Value = tenDm;
            this.sqlCommand.Parameters.Add("@maupdate", SqlDbType.Int).Value = maDm;
            int resultSave = this.sqlCommand.ExecuteNonQuery();
            CloseConnection();
            return resultSave;
        }
        public int DeleteDanhMucParameter(string maDm)   // delete danh mục item
        {
            OpenConnection();
            this.sqlCommand.CommandType = CommandType.Text;
            this.sqlCommand.CommandText = "Delete from DanhMuc where MaDm=@madelete";
            this.sqlCommand.Connection = sqlConnection;
            this.sqlCommand.Parameters.Add("@madelete", SqlDbType.Int).Value = maDm;
            int result = this.sqlCommand.ExecuteNonQuery();
            CloseConnection();
            return result;
        }


        /** Cách 3: Cách tương tác CSDL thông qua Store Procedures
         * Tương tự cách 2, nhưng thay thế câu commandType thành CommandType.StoredProcedure
         * và thay thế câu truy vấn bằng store procedures trong sql server*/
        public List<DanhMucNoLinq> QueryAllDanhMucStoreProcedures()
        {
            List<DanhMucNoLinq> danhMucs= new List<DanhMucNoLinq>();
            OpenConnection();
            this.sqlCommand.CommandType = CommandType.StoredProcedure;
            this.sqlCommand.CommandText = "QueryDanhMuc";
            this.sqlCommand.Connection = sqlConnection;
            SqlDataReader sqlDataReader = this.sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
                danhMucs.Add(new DanhMucNoLinq(){     /*Đây là cách sử dụng Object Initializers để khởi tạo object trong LinQ*/
                    MaDm= sqlDataReader.GetInt32(0),TenDm= sqlDataReader.GetString(1) });
            sqlDataReader.Close();
            CloseConnection();
            return danhMucs;
        }


        public delegate string DanhMucService(string danhMucService); /*DELEGSTE là gì? tạo delegate ở đây có tác dụng zì?*/


        /*___________________________________ TƯƠNG TÁC VỚI CSDL THÔNG QUA LINQ __________________________*/
        public List<DanhMuc> QueryDanhMucLinQ(string madm)
        {
            Db_ProductManagementDataContext context = new Db_ProductManagementDataContext();
            return (madm == "") /*nếu madm = "" thì lấy hết danhmuc, ngược lại nếu có madm thì chỉ lấy madm thôi*/
                ? (from dms in context.DanhMucs select dms).ToList()                        /*Cách 1: Dùng Query Syntax*/
                : (context.DanhMucs.ToList().FindAll(dm => dm.MaDm == Int32.Parse(madm)));  /*Cách 2: Dùng Method Syntax*/
        }
        private bool CheckDanhMuc(string madm) /*nếu = null, tức là k có madm này, thì trả về false, nếu có trả về true*/
        {
            if (madm == "") return false; /*FirstOrDefault hoặc dùng Find để trả về 1 đối tượng cụ thể*/
            return (QueryDanhMucLinQ(madm).FirstOrDefault(dm => dm.MaDm == Int32.Parse(madm)) == null) ? false : true;
        }
        public bool InsertUpdateDanhMucLinQ(string madm, string tendm)
        {
            Db_ProductManagementDataContext context = new Db_ProductManagementDataContext();
            if (CheckDanhMuc(madm)) /*nếu đã tồn tại madm này rồi thì chỉ update lại danh mục ==> Trường hợp UPDATE*/
            {
                try
                {
                    DanhMuc danhMuc = context.DanhMucs.FirstOrDefault(dm => dm.MaDm == Int32.Parse(madm));
                    if (danhMuc != null)    /*FirstOrDefault hoặc dùng Find để trả về 1 đối tượng cụ thể*/
                    {
                        danhMuc.TenDm = tendm;
                        context.SubmitChanges();
                        return true;
                    }
                }
                catch { }
                return false;
            }
            else /*nếu madm này chưa tồn tại thì add mới ==> Trường hợp INSERT*/
            {
                try
                {
                    context.DanhMucs.InsertOnSubmit(new DanhMuc { MaDm = Int32.Parse(madm), TenDm = tendm });
                    context.SubmitChanges();
                    return true;
                }
                catch { }
                return false;
            }
        }
        public bool DeleteDanhMucLinQ(string madm)
        {
            Db_ProductManagementDataContext context = new Db_ProductManagementDataContext();
            DanhMuc danhMuc = context.DanhMucs.FirstOrDefault(dm => dm.MaDm == Int32.Parse(madm));
            if (danhMuc != null)    /*FirstOrDefault hoặc dùng Find để trả về 1 đối tượng cụ thể*/
            {
                if (context.SanPhams.FirstOrDefault(sp => sp.MaDm == Int32.Parse(madm)) != null)
                    return false;   //lấy sản phẩm có mã sản phẩm bằng madm trên, nếu trong danh mục vẫn còn sản phẩm thì k đc xóa
                context.DanhMucs.DeleteOnSubmit(danhMuc);
                context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}
