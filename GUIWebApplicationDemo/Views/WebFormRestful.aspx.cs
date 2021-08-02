using DataTransferObject;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.UI.WebControls;

/** 
 * WebFormRestful -> Xây dựng Web page có tương tác với Restful Webservice để truy vấn thay đổi data thông qua kỹ thuật LINQ 
 * ____________Tương tác Webservice Restful (tương tác trên Web page và mobile), ở đây là tương tác trên Web page___________
 * 
 * Muốn tương tác JSON -> cần reference library System.Runtime.Serialization
 */

namespace GUIWebApplicationDemo.Views
{
    public partial class WebFormRestful : System.Web.UI.Page
    {
        string url = "http://localhost/DanhmucSanPhamWebServiceDemo/api/";
        string urlSanPham = "SanPham/";
        string urlDanhMuc = "DanhMuc/";
        string urlMadm = "?madm=";
        string urlFromPrice = "fromPrice=";
        string urlToPrice = "&toPrice=";

        private object ArrayObjects(string url, string method, Type type)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);    /*request gửi lên server lấy url của webservice về*/
            request.Method = method;/*method ở đây là GET, POST, PUT hay DELETE?*/
            if (method != "GET")    /*nếu k phải là phương thức get => tức là insert, update, delete (xem trong Controller)*/
            {
                request.ContentType = "application/json;charset=UTF-8";
                string postString = url.Substring(url.LastIndexOf("/") + 1);    /*Lấy postString từ url ra: VD: ?masp=...*/
                byte[] byteArray = Encoding.UTF8.GetBytes(postString);  /*đưa postString băm về mảng byte*/
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream(); /*xử lý xong thì đưa data lên server, write lên server từ dataStream*/
                dataStream.Write(byteArray, 0, byteArray.Length);/*đưa mảng byte vừa xử lý xong lên server từ 0 đến cuối mảng .length*/
                dataStream.Close(); /*đưa lên server xong thì đóng stream. Sau khi đưa data lên thì làm sao biết nó trả về là T hay F*/
            }   /*ở đây KQ trả về chỉ có True hoặc False, do đó we dùng 1 DataContractJsonSerializer như dưới để xử lý*/
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;    /*server trả về 1 response*/
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(type);   /*đưa response của server về json*/
            object responseData = jsonSerializer.ReadObject(response.GetResponseStream());  /*đưa json về 1 object để xử lý*/
            return responseData;    /*nếu object đc đưa về là array thì trả về 1 array*/
        }

        protected void Page_Load(object sender, EventArgs e)
        {   /*vì chắc chắn trả về 1 mảng các danh mục nên truyền vào là typeof(DanhMucNoLinq[]) và trả về DanhMucNoLinq[]*/
            DanhMucNoLinq[] arrayDanhMuc = ArrayObjects(url + urlDanhMuc, "GET", typeof(DanhMucNoLinq[])) as DanhMucNoLinq[];
            lsbDanhMuc.Items.Clear();
            lsbDanhMuc.DataSource = arrayDanhMuc.ToList();                                  /*cách 1: đưa vào listbox = datasource*/
            lsbDanhMuc.DataBind();
            SanPhamNoLinq[] arraySanPham = ArrayObjects(url + urlSanPham, "GET", typeof(SanPhamNoLinq[])) as SanPhamNoLinq[];
            lsbSanPham.Items.Clear();
            arraySanPham.ToList().ForEach(sp => { lsbSanPham.Items.Add(sp.ToString()); });  /*cách 2: đưa vào listbox = LINQ*/
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {   /** _________________________ ĐẦU TIÊN XÁC ĐỊNH CHUỖI URL TRUYỀN VÀO ___________________________*/
            string urlRequest = "";
            if (txtToPrice.Text == "")  /*xử lý các Controller: AllSanPhams(), SanPhams(int madm),*/
            {                           /*SanPhams(int fromPrice, int toPrice) và SanPhams(int madm, int fromPrice, int toPrice)*/
                if (txtFromPrice.Text == "")     /*Nếu các khoảng zá đều rỗng => lấy hết sản phẩm (url+urlSanpham) theo mã dm*/
                    urlRequest = url + urlSanPham + (txtCodeDm.Text == "" ? "" : (urlMadm + txtCodeDm.Text));
                else    /*Nếu toPrice rỗng => lấy hết sản phẩm có khoảng zá từ fromPrice đến Maximum theo mã dm*/
                    urlRequest = url + urlSanPham + ((txtCodeDm.Text == "") ? "?" : (urlMadm + txtCodeDm.Text + "&")) +
                        urlFromPrice + txtFromPrice.Text + urlToPrice +                                        /*url đến toPrice=*/
                        (ArrayObjects(url + urlSanPham, "GET", typeof(SanPhamNoLinq[])) as SanPhamNoLinq[])    /*lấy Max trong listSP*/
                            .ToList().Max(sp => sp.DonGia);
            }
            else /*xử lý các Controller: SanPhams(int fromPrice, int toPrice) và SanPhams(int madm, int fromPrice, int toPrice)*/
            {
                if (txtFromPrice.Text == "")    /*Nếu fromPrice rỗng => lấy hết sản phẩm từ 0 đến toPrice theo mã dm*/
                    urlRequest = url + urlSanPham + ((txtCodeDm.Text == "") ? "?" : (urlMadm + txtCodeDm.Text + "&")) +
                        urlFromPrice + "0" + urlToPrice + txtToPrice.Text;
                else    /*Nếu có dữ liệu khoảng zá muốn lấy => lấy hết sản phẩm từ fromPrice đến toPrice theo mã dm*/
                    urlRequest = url + urlSanPham + ((txtCodeDm.Text == "") ? "?" : (urlMadm + txtCodeDm.Text + "&")) +
                        urlFromPrice + txtFromPrice.Text + urlToPrice + txtToPrice.Text;
            }
            lsbSanPham.Items.Clear();   /**____________________________ SAU ĐÓ ĐƯA LÊN ZAO DIỆN __________________*/
            try
            {
                lsbSanPham.DataSource = (ArrayObjects(urlRequest, "GET", typeof(SanPhamNoLinq[])) as SanPhamNoLinq[]).ToList();
                if (txtCodeDm.Text != "")   /*xử lý Controller DanhMucDetail(int id)*/
                    lblQueryDM.Text =
                        (ArrayObjects(url + urlDanhMuc + txtCodeDm.Text, "GET", typeof(DanhMucNoLinq)) as DanhMucNoLinq).ToString() + ". ";
                if (txtCodeSp.Text != "")   /*xử lý Controller SanPhamDetail(int id)*/
                    lblQuerySP.Text =
                        (ArrayObjects(url + urlSanPham + txtCodeSp.Text, "GET", typeof(SanPhamNoLinq)) as SanPhamNoLinq).ToString();
            }
            catch (Exception ex)
            {
                lsbSanPham.DataSource = (ArrayObjects(url + urlSanPham, "GET", typeof(SanPhamNoLinq[])) as SanPhamNoLinq[]).ToList();
            }
            lsbSanPham.DataBind();
        }
        private bool checkSanPhamExist()    /*___________________CHECK SẢN PHẨM CÓ TỒN TẠI TRONG LIST HAY K?__________________*/
        {
            try
            {
                SanPhamNoLinq sanPham = (ArrayObjects(url + urlSanPham + txtCodeSp.Text, "GET", typeof(SanPhamNoLinq)) as SanPhamNoLinq);
                return sanPham != null;
            }
            catch (Exception ex) { }
            return false;
        }
        protected void btnAddSP_Click(object sender, EventArgs e)   /*______________________INSERT VÀ UPDATE SẢN PHẨM_________________*/
        {
            string postString = string.Format("?masp={0}&tensp={1}&price={2}&madm={3}", /*masp, tensp, price, madm là các đối số */
                txtCodeSp.Text, txtNameSP.Text, txtPriceSP.Text, txtCodeDm.Text);       /*trong SanPhamInsert(...) trong Controller*/
            try
            {   /*nếu sản phẩm này đã tồn tại thì update => dùng PUT. Nếu sản phẩm này chưa tồn tại thì insert => dùng POST */
                lblQuerySP.Text = ((bool)ArrayObjects(url + urlSanPham + postString, checkSanPhamExist() ? "PUT" : "POST", typeof(bool)))
                                    ? "Success" : "Fail";
            }
            catch (Exception ex)
            {
                lblQuerySP.Text = "Fail";
            }
        }
        protected void btnDeleteSP_Click(object sender, EventArgs e)/*______________________DELETE 1 SẢN PHẨM_________________*/
        {
            try
            {
                lblQuerySP.Text = ((bool)ArrayObjects(url + urlSanPham + string.Format("?masp={0}", txtCodeSp.Text),
                                    checkSanPhamExist() ? "DELETE" : "", typeof(bool))) ? "Success" : "Fail";
            }
            catch (Exception ex)
            {
                lblQuerySP.Text = "Fail";
            }
        }

        private bool checkDanMucExist()    /*_____________________CHECK DANH MỤC CÓ TỒN TẠI TRONG LIST HAY K?____________________*/
        {
            try
            {
                DanhMucNoLinq danhMuc = (ArrayObjects(url + urlDanhMuc + txtCodeDm.Text, "GET", typeof(DanhMucNoLinq)) as DanhMucNoLinq);
                return danhMuc != null;
            }
            catch (Exception ex) { }
            return false;
        }
        protected void btnAddDM_Click(object sender, EventArgs e)   /*_______________INSERT VÀ UPDATE DANH  MỤC__________________*/
        {
            string poststring = string.Format("?madm={0}&namedm={1}", txtCodeDm.Text, txtNameDM.Text);
            try
            {
                lblQueryDM.Text = ((bool)ArrayObjects(url + urlDanhMuc + poststring, checkDanMucExist() ? "PUT" : "POST", typeof(bool)))
                                    ? "Success" : "Fail";
            }
            catch (Exception ex)
            {
                lblQueryDM.Text = "Fail";
            }
        }
        protected void btnDeleteDM_Click(object sender, EventArgs e)/*______________________DELETE 1 DANH MỤC_________________*/
        {
            try
            {
                lblQueryDM.Text = ((bool)ArrayObjects(url + urlDanhMuc + string.Format("?madm={0}", txtCodeDm.Text),
                                    checkDanMucExist() ? "DELETE" : "", typeof(bool))) ? "Success" : "Fail";
            }
            catch (Exception ex)
            {
                lblQueryDM.Text = "Fail";
            }
        }
    }
}