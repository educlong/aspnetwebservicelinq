using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using FirebaseCloudMessageServer.Models;


namespace FirebaseCloudMessageServer.Controllers
{
    /*Đây chính là BƯỚC 3 trong lưu đồ hoạt động của firebasecloudmesssage (PHÍA SERVER - REMOTE SERVER)
    * (Lưu lại toàn bộ Token mà firebase gửi về cho từng device)
    * 
    *   _________________________      Bước 4: Admin gửi Token                         _____________________________
    *   |                       |      + thông điệp (Message) lên firebase             |                           |
    *   |   Remote Database     | ---------------------------------------------------->|   Firebase Cloud Message  |
    *   |_______________________|                                                      |   (1 database của google) |
    *       Bước 3: |                                                  Bước 1:  |----->|___________________________|
    *       Ta phải |                                                Các Device |     Bước 2:   |      Bước 5: |
    *       lưu lại |                                                đã cài đặt |     Firebase  |      Firebase|
    *       toàn bộ |                                                apps của   |     trả về    |      sẽ Push |
    *       Token   |                                                mình sẽ    |     Token     |      Message |
    *       mà      |                                                gửi yêu cầu|     (chính là |      (thông  |
    *       firebase|                                                lên server |     ID device |       điệp   |
    *       gửi về  |                                                firebase   |     của mỗi   |       từ     |
    *       cho từng|                                                của google |     thiết bị  |       bước 4)|
    *       device  |                                                để lấy     |     mà đã cài |              |
    *               |                                                Token      |     đặt app   |              |
    *               |    __________________________________________________     |     của mình) |              |
    *               |    |                                                |-----|               |              |
    *               |--> |   Devices mà customer đã cài đặt app của mình  |                     |              |
    *                    |   (smartphone, tablet, etc,...)                |<--------------------|              |
    *                    |                                                |                                    |
    *                    |________________________________________________|<-----------------------------------|
    *
    *
    * */

    public class FirebaseCMController : ApiController
    {
        [HttpPost] //lưu vào
        public bool saveToken(string token)
        {
            try
            {   //phải ktra: đối tượng #null -> đã tồn tại -> ko làm zì cả
                if (getFirebaseCloudMessage(token) != null) return false;
                //TA CẦN KẾT NỐI CSDL => vào file Web.config để cấu hình
                /*sau khi đã tạo chuỗi kết nối trong file web.config. 
                ta sẽ lấy đường dẫn để tạo chuỗi kết nối đến csdl, như sau:*/
                string strConnection =  /*truyền key đã đ/nghĩa trong file web.config vào*/
                    System.Configuration.ConfigurationManager.AppSettings["strConnection"];
                SqlConnection connection = new SqlConnection(strConnection);    //mở kết nối
                connection.Open();                                              //mở lên
                /*Truyền tên bảng và tên cột vào (cột token thôi, cột id đã tự động tăng)*/
                string sql = "insert into getTokenFCM(Token) values(@token)";
                SqlCommand command = new SqlCommand(sql, connection);
                /*gán đc zá trị cho biến token*/
                command.Parameters.Add("@token", SqlDbType.NVarChar).Value = token;
                /* Tiến hành insert. Khi insert thành công => sẽ trả về 1 kết quả 
                 * kết quả này trả về số dòng ảnh hưởng-> tức là int ketQua > 0*/
                int ketQua = command.ExecuteNonQuery();
                connection.Close();   //đóng kết nối
                return ketQua > 0;
            }   //đây là hàm để lưu token mà ta lấy đc của Firebase gửi cho từng thiết bị 
            catch (Exception ex)       //của khách hàng
            {
                throw ex;
            }
        }

        [HttpGet] /*Nếu token trùng nhau?, trường hợp máy khách hàng bị sự cố, lấy token ko auto đc
                    -> trường hợp này ta cũng phải lưu -> ta cần có 1 cái để kiểm tra nếu token đã
                    tồn tại thì ta ko lưu nữa -> do đó ta phải có 1 hàm trả về 1 
                    FirebaseCM theo 1 token nào đó ==> ta có hàm [HttpGet] này*/
        public FirebaseCM getFirebaseCloudMessage(String token)
        {
            try     //copy đoạn lệnh của hàm trên và sửa lại
            {
                string strConnection =
                    System.Configuration.ConfigurationManager.AppSettings["strConnection"];
                SqlConnection connection = new SqlConnection(strConnection);
                connection.Open();
                /*sửa lại câu lệnh truy vấn Select*/
                string sql = "select * from getTokenFCM where token=@token";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@token", SqlDbType.NVarChar).Value = token;
                /*nếu token tồn tại thì ko lưu nữa*/
                SqlDataReader reader = command.ExecuteReader();
                FirebaseCM fcm = null;                  //nếu ko có token thì luôn bằng null
                while (reader.Read())                   //có thì lấy kq ra (trong khi còn dữ liệu để đọc)
                {
                    fcm = new FirebaseCM();
                    fcm.Id = reader.GetInt32(0);        //lấy zá trị số 0 trong sqlserver
                    fcm.Token = reader.GetString(1);    //lấy zá trị số 1 trong sqlserver
                }   /*kết thúc hàm getFirebaseCloudMessage sẽ trả về 1fcm có id và token, chỉ cần*/
                connection.Close();                     //ktra là token đó có tồn tại hay ko thôi
                return fcm;                             //đối tượng #null -> tồn tại, =null-> chưa tồn tại
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]   //ta có thể trả về 1 token mà có liên quan đến id
        public FirebaseCM getFirebaseCloudMessage(int id)
        {
            try     //copy đoạn lệnh và sửa lại
            {
                string strConnection =
                    System.Configuration.ConfigurationManager.AppSettings["strConnection"];
                SqlConnection connection = new SqlConnection(strConnection);
                connection.Open();
                string sql = "select * from getTokenFCM where id=@id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
                SqlDataReader reader = command.ExecuteReader();
                FirebaseCM fcm = null;
                while (reader.Read())
                {
                    fcm = new FirebaseCM();
                    fcm.Id = reader.GetInt32(0);
                    fcm.Token = reader.GetString(1);
                }
                connection.Close();
                return fcm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]   //trả về danh sách các token
        public List<FirebaseCM> getFirebaseCloudMessageS()
        {
            try     //copy đoạn lệnh của hàm trên và sửa lại
            {
                List<FirebaseCM> listFCM = new List<FirebaseCM>();
                string strConnection =
                    System.Configuration.ConfigurationManager.AppSettings["strConnection"];
                SqlConnection connection = new SqlConnection(strConnection);
                connection.Open();
                string sql = "select * from getTokenFCM";  //lấy hết trong csdl
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                FirebaseCM fcm = null;
                while (reader.Read())
                {
                    fcm = new FirebaseCM();
                    fcm.Id = reader.GetInt32(0);
                    fcm.Token = reader.GetString(1);
                    listFCM.Add(fcm);
                }
                connection.Close();
                return listFCM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
