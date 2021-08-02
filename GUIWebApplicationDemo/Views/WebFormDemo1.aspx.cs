using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GUIWebApplicationDemo.Models;
using DataTransferObject;
using BusinessLogicLayer;

/** 
 * WebFormDemo1 -> Xây dựng Web page có tương tác data thông qua kỹ thuật LINQ (tập trung hướng dẫn và sử dụng LINQ)
 * 
 *  TRONG THỰC TẾ SẼ KO LÀM THẾ NÀY (VÌ Ở ĐÂY ĐANG TƯƠNG TÁC TRỰC TIẾP VS DATABASE)
 *  
 *  TRONG THỰC TẾ WEB FORM CHỈ ĐC TƯƠNG TÁC VS ZAO DIỆN NGƯỜI DÙNG => XỬ LÝ TRONG WebFormRestful.aspx
 *  MUỐN TƯƠNG TÁC VS DATABASE PHẢI THÔNG QUA CONTROLLER 
 */

namespace GUIWebApplicationDemo.Views
{
    public partial class WebFormDemo1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
          
            lblInforUser.Text = txtUsername.Text + txtPassword.Text;
        }

        protected void btnLuuSinhVien_Click(object sender, EventArgs e)
        {
            SinhVien sv = new SinhVien();
            sv.Account = txtUsername.Text;
            sv.Password = txtPassword.Text;
            lstSinhVien.Items.Add(sv.ToString());
        }

        /*QUẢN LÝ DANH MỤC*/

        DanhMucLogic danhMucLogic = new DanhMucLogic();
        protected void btnQueryDanhMuc_Click(object sender, EventArgs e)  //query toàn bộ danh mục
        {
            /*Cách 1:*/
            lstDanhMuc.DataSource = danhMucLogic.QueryDanhMuc();
            lstDanhMuc.DataBind();

            lstDanhMuc.Items.Clear();

            /*Cách 2: (LINQ) Dùng ForEach đẩy data vào */
            List<DanhMucNoLinq> danhMucs = danhMucLogic.QueryDanhMuc();
            danhMucs.ForEach(danhMuc => { lstDanhMuc.Items.Add(danhMuc.ToString()); }); // kết hợp vs lambda expression
                          /*(........ trong này là 1 anonymous method ...............)*/         

            lblDanhMuc.Text = danhMucLogic.CountDanhMuc();
        }

        protected void btnSearchDanhMucTheoMa_Click(object sender, EventArgs e) //search danh mục (query danh mục theo mã danh mục)
        {
            if (txtSearchDanhMucTheoMa.Text == "") return;
            DanhMucNoLinq danhMuc = (from dm in danhMucLogic.QueryDanhMuc()         //?
                         where dm.MaDm == Int32.Parse(txtSearchDanhMucTheoMa.Text)  //?
                         select dm).ToList().FirstOrDefault();                      //?
            lblDanhMuc.Text = danhMucLogic.QueryDanhMucTheoMaDm(txtSearchDanhMucTheoMa.Text.ToString());
        }

        protected void btnUpdateAndInsert_Click(object sender, EventArgs e) //update or insert danh mục
        {
            if (txtMaDanhMuc.Text == "") return;
            lblDanhMuc.Text = danhMucLogic.InsertUpdateDanhMuc(txtMaDanhMuc.Text.ToString(), txtTenDanhMuc.Text.ToString());
        }

        protected void btnDeleteDanhMuc_Click(object sender, EventArgs e)   //delete danh mục
        {
            if (txtMaDanhMuc.Text == "") return;
            lblDanhMuc.Text = danhMucLogic.DeleteDanhMuc(txtMaDanhMuc.Text.ToString());
        }

        /*QUẢN LÝ SẢN PHẨM*/

        SanPhamLogic sanPhamLogic = new SanPhamLogic();
        protected void btnQueryAllSanPham_Click(object sender, EventArgs e) //query all sản phẩm or query sản phẩm theo mãDm
        {
            lstSanPham.DataSource = sanPhamLogic.QuerySanPham(txtQuerySpTheoMaDm.Text);
            lstSanPham.DataBind();

            lstSanPham.Items.Clear();

            /*LINQ:
             * ForEach trong LINQ*/
            /*Cách 2: (LINQ) Dùng ForEach đẩy data vào */
            List<SanPhamNoLinq> sanPhams= sanPhamLogic.QuerySanPham(txtQuerySpTheoMaDm.Text);
            sanPhams.ForEach(sanPham => { lstSanPham.Items.Add(sanPham.ToString()); }); // kết hợp vs lambda expression
                          /*(........ trong này là 1 anonymous method ...............)*/


            lblSanPham.Text = sanPhamLogic.CountSanPham(txtQuerySpTheoMaDm.Text);
        }

        protected void btnSearchSanPham_Click(object sender, EventArgs e)   //query sản phẩm theo mã sản phẩm
        {
            if (txtQuerySpTheoMaSp.Text == "") return;
            lblSanPham.Text = "Cách 1: Query bình thường: "+sanPhamLogic.QuerySanPhamTheoMaSp(txtQuerySpTheoMaSp.Text)

                /*LINQ:
                 * FirstOrDefault trong LINQ*/
                /*Cách 2: (LINQ) Dùng FirstOrDefault để tìm ra sản phẩm theo mã sản phẩm đc đẩy vào */
                + ", Cách 2: Dùng FirstOrDefault trong LINQ: " +
                (sanPhamLogic.QuerySanPham(txtQuerySpTheoMaDm.Text).FirstOrDefault(sp => sp.MaSp==Int32.Parse(txtQuerySpTheoMaSp.Text)));
        }

        protected void btnUpdateAndInsertSP_Click(object sender, EventArgs e)   //update và insert sản phẩm
        {
            if (txtQuerySpTheoMaDm.Text == "" || txtQuerySpTheoMaSp.Text == ""
                || txtTenSanPham.Text == "" || txtDonZa.Text == "") return;
            lblSanPham.Text = sanPhamLogic.InsertUpdateSanPham(txtQuerySpTheoMaSp.Text, txtTenSanPham.Text,
                txtDonZa.Text, txtQuerySpTheoMaDm.Text);
        }

        protected void btnDeleteSanPham_Click(object sender, EventArgs e)   //delete sản phẩm
        {
            if (txtQuerySpTheoMaSp.Text == null) return;
            lblSanPham.Text = sanPhamLogic.DeleteSanPham(txtQuerySpTheoMaSp.Text, txtQuerySpTheoMaDm.Text);
        }

        protected void btnQueryAllSanPhamDataAdapter_Click(object sender, EventArgs e)  //query tất cả sản phẩm dùng dataadapter
        {
            gvSanPham.DataSource = sanPhamLogic.QueryAllSanPhamSqlDataAdapter();
            gvSanPham.DataBind();
        }

        protected void btnUpdateAndInsertSPDataAdapter_Click(object sender, EventArgs e)
        {
            if (txtQuerySpTheoMaDm.Text == "" || txtQuerySpTheoMaSp.Text == ""
                || txtTenSanPham.Text == "" || txtDonZa.Text == "") return;
            lblSanPhamDataAdapter.Text = sanPhamLogic.InsertSanPhamSqlDataAdapter(txtQuerySpTheoMaSp.Text,
                txtTenSanPham.Text, txtDonZa.Text, txtQuerySpTheoMaDm.Text);
        }


    }
}