using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FirebaseCloudMessageServer.Views
{
    public partial class AdminFCM : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /*Đây chính là BƯỚC 4 trong lưu đồ hoạt động của firebasecloudmesssage 
         *  (PHÍA SERVER - REMOTE SERVER - Trang thông báo Push message) --> (Admin gửi Token + Message lên firebase
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

        protected void BtnSendThongBaoFCM_Click(object sender, EventArgs e)
        {
            string applicationID = "AAAAUJEJ0WY:APA91bGef-AS0Bo8AajB9Yjwd3tJWTMNnUR8pL_RLdD1sP7Z6VE_cwPy1WoeKfpQRcbRTXLSHpvyl-ILEho9HSiuIJ5A_pJaNvT5h6RgqjGavY-DaiOb7Du_SOVrfM81Q468-KD4XOkB";   //API Key
            string SENDER_ID = "346030723430";                                  //đây chính là SenderID
            Controllers.FirebaseCMController fcmController = new Controllers.FirebaseCMController();
            List<Models.FirebaseCM> listFCM = fcmController.getFirebaseCloudMessageS(); //lấy danh sách FCM trong controller
            WebRequest tRequest;    //thiết lập FCM send
            /*link dưới là service của gg cho, lấy y xì link dưới, định dạng json 
             * Link này là firebase, gửi Token và message lên firebase theo link dưới
             * */
            tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "POST";
            tRequest.UseDefaultCredentials = true;
            tRequest.PreAuthenticate = true;
            tRequest.Credentials = CredentialCache.DefaultNetworkCredentials;
            tRequest.ContentType = "application/json";  //định dạng json
            tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
            //trả về mảng(ToArray) danh sách token. Select là phươngthức chỉ lấy token trên list
            string[] arrRegid = listFCM.Select(x => x.Token).ToArray();
            string RegArr = string.Empty;
            RegArr = string.Join("\",\"", arrRegid);
            //lệnh dưới dùng để nối tất cả các api_key lại vs nhau (viểt y xì), đây là cú pháp
            string postData = "{ \"registration_ids\": [ \"" + RegArr
                + "\" ],\"data\": {\"message\": \"" + TxtNoiDungFCM.Text
                + "\",\"body\": \"" + TxtNoiDungFCM.Text + "\",\"title\": \""
                + TxtTieuDeFCM.Text + "\",\"collapse_key\":\""
                + TxtNoiDungFCM.Text + "\"}}";
            //sử dụng encodingUTF8 đề phòng những đoạn text bằng tiếng việt
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;
            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();
            StreamReader tReader = new StreamReader(dataStream);
            String sResponseFromServer = tReader.ReadToEnd();
            TxtKetQuaFCM.Text = sResponseFromServer;    //lấy thông báo kquả từ FCM Server
            /*thông qua sResponseFromServer ta sẽ biết đc thiết bị nào nhận đc, tbị nào ko*/
            tReader.Close();
            dataStream.Close();
            tResponse.Close();
        }
    }
}