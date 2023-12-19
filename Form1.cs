using NguyenVanNguyen__2180606793.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NguyenVanNguyen__2180606793
{
    public partial class FrmQuanLyThuVien : Form
    {
        static ThuVienDBContext thuVienDBContext = new ThuVienDBContext();
        public FrmQuanLyThuVien()
        {
            InitializeComponent();
        }
        
        private void showMessage(string message)
        {
            MessageBox.Show(message,"Thông Báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void BindGridThuVien(List<SACH> listSach)
        {
            dgvThuVien.Rows.Clear();
            foreach(var item in listSach)
            {
                int indexColumn = dgvThuVien.Rows.Add();
                dgvThuVien.Rows[indexColumn].Cells[0].Value = item.MaSach;
                dgvThuVien.Rows[indexColumn].Cells[1].Value = item.TenSach;
                dgvThuVien.Rows[indexColumn].Cells[2].Value = item.TacGia;
                dgvThuVien.Rows[indexColumn].Cells[3].Value = item.NamXuatBan;
                dgvThuVien.Rows[indexColumn].Cells[4].Value = item.NhaXuatBan;
                dgvThuVien.Rows[indexColumn].Cells[5].Value = item.TriGia;
                dgvThuVien.Rows[indexColumn].Cells[6].Value = item.NgayNhap;
            }
        }

        public void setGridViewStyle(DataGridView dgView)
        {
            dgvThuVien.BorderStyle = BorderStyle.None;
            dgvThuVien.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgvThuVien.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvThuVien.BackgroundColor = Color.White;       
        }
        private void FrmQuanLyThuVien_Load(object sender, EventArgs e)
        {
            setGridViewStyle(dgvThuVien);
            var listSach = thuVienDBContext.SACHes.ToList();
            if(listSach != null)
            {
                BindGridThuVien(listSach);
            }
        }

        private bool CheckData()
        {
            if(string.IsNullOrEmpty(txtMaSach.Text))
            {
                showMessage("vui lòng nhập mã sách");
                return false;
            }
            if(string.IsNullOrEmpty(txtTenSach.Text))
            {
                showMessage("vui lòng nhập tên sách");
                return false;
            }
            if (string.IsNullOrEmpty(txtTacGia.Text))
            {
                showMessage("vui lòng nhập năm xuất bản");
                return false;
            }
            if (string.IsNullOrEmpty(txtNhaXB.Text))
            {
                showMessage("vui lòng nhập nhà xuất bản");
                return false;
            }
            if (string.IsNullOrEmpty(txtTriGia.Text))
            {
                showMessage("vui lòng nhập trị giá");
                return false;
            }
            return true;
        }
        private void btnThemSach_Click(object sender, EventArgs e)
        {

            if(CheckData())
            {
                int masachNew = int.Parse(txtMaSach.Text);
                string tenSachNew = txtTenSach.Text;
                string tacGia = txtTacGia.Text;
                int namXB = int.Parse(txtNamXB.Text);
                string nhaXB = txtNhaXB.Text;
                float triGia = float.Parse(txtTriGia.Text);
                DateTime dateNgayNhap = dtNgayNhap.Value;

                var existingSach = thuVienDBContext.SACHes.FirstOrDefault(
                            s => s.MaSach == masachNew
                        );
                if (existingSach == null)
                {
                    var sachNew = new SACH
                    {
                        TenSach = tenSachNew,
                        TacGia = tacGia,
                        NamXuatBan = namXB,
                        NhaXuatBan = nhaXB,
                        TriGia = triGia,
                        NgayNhap = dateNgayNhap
                    };
                    thuVienDBContext.SACHes.Add(sachNew);
                    thuVienDBContext.SaveChanges();
                    try
                    {
                        showMessage("Thêm sách vào database thành công");
                        var listSach = thuVienDBContext.SACHes.ToList();
                        BindGridThuVien(listSach);
                        tinhTongTien();
                    }
                    catch (Exception ex)
                    {
                        showMessage(ex.Message);
                    }
                    resetData();
                }
                else
                {
                    showMessage("Đã tồn tại mã sách trong database , vui lòng " +
                        "thêm mã sách khác");
                }    
            }    
        }

        private void txtMaSach_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
                showMessage("chỉ nhập số cho mã sách");
            }
        }

        private void txtNamXB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
                showMessage("chỉ nhập số cho năm xuất bản");
            }
        }

        private void txtTriGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
                showMessage("chỉ nhập số cho trị giá");
            }
        }

        private bool checkMaSach()
        {
            if(string.IsNullOrEmpty(txtMaSach.Text))
            {
                showMessage("Vui lòng nhập mã sách");
                return false;
            }
            return true;
        }
        private void btnXoaSach_Click(object sender, EventArgs e)
        {
            if(checkMaSach())
            {
                int masachNew = int.Parse(txtMaSach.Text);
                var existingSach = thuVienDBContext.SACHes.FirstOrDefault(
                            s => s.MaSach == masachNew
                        );
                if(existingSach != null)
                {
                    if(MessageBox.Show("Bạn chắc chắn muốn xóa không",
                        "Thông Báo",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        thuVienDBContext.SACHes.Remove(existingSach);
                        thuVienDBContext.SaveChanges();

                        try
                        {
                            showMessage("Xóa sách thành công");
                            var listSach = thuVienDBContext.SACHes.ToList();
                            BindGridThuVien(listSach);
                            tinhTongTien();
                        }
                        catch (Exception ex)
                        {
                            showMessage(ex.Message);
                        }
                    }
                    resetData();

                }
                else
                {
                    showMessage("không tồn tại mã sách cần xóa");
                }
            }    
        }
        private void resetData()
        {
            txtMaSach.Text = txtTenSach.Text = txtTacGia.Text = txtNhaXB.Text
                = txtTriGia.Text = txtNamXB.Text = "   ";
        }

        int indexCellClick = -1;
        private void dgvThuVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexCellClick = e.RowIndex;
            if(indexCellClick != -1 && dgvThuVien.Rows.Count > 0 )
            {
                txtMaSach.Text = dgvThuVien[0,indexCellClick].Value.ToString();
                txtTenSach.Text = dgvThuVien[1,indexCellClick].Value.ToString();
                txtTacGia.Text = dgvThuVien[2,indexCellClick].Value.ToString();
                txtNamXB.Text = dgvThuVien[3,indexCellClick].Value.ToString();
                txtNhaXB.Text = dgvThuVien[4,indexCellClick].Value.ToString();
                txtTriGia.Text = dgvThuVien[5,indexCellClick].Value.ToString();
                dtNgayNhap.Text = dgvThuVien[6,indexCellClick].Value.ToString();
            }
        }

        private void dgvThuVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            indexCellClick = e.RowIndex;
            if (indexCellClick != -1 && dgvThuVien.Rows.Count > 0)
            {
                txtMaSach.Text = dgvThuVien[0, indexCellClick].Value.ToString();
                txtTenSach.Text = dgvThuVien[1, indexCellClick].Value.ToString();
                txtTacGia.Text = dgvThuVien[2, indexCellClick].Value.ToString();
                txtNamXB.Text = dgvThuVien[3, indexCellClick].Value.ToString();
                txtNhaXB.Text = dgvThuVien[4, indexCellClick].Value.ToString();
                txtTriGia.Text = dgvThuVien[5, indexCellClick].Value.ToString();
                dtNgayNhap.Text = dgvThuVien[6, indexCellClick].Value.ToString();
            }
        }

        private void btnSuaSach_Click(object sender, EventArgs e)
        {
            if(CheckData())
            {
                int masachNew = int.Parse(txtMaSach.Text);
                var existingSach = thuVienDBContext.SACHes.FirstOrDefault(
                            s => s.MaSach == masachNew
                        );
                DateTime dateNgayNhap = dtNgayNhap.Value;
                if ( existingSach != null ) {
                    existingSach.TenSach = txtTenSach.Text;
                    existingSach.TacGia = txtTacGia.Text;
                    existingSach.NamXuatBan = int.Parse(txtNamXB.Text);
                    existingSach.NhaXuatBan = txtNhaXB.Text;
                    existingSach.TriGia = float.Parse(txtTriGia.Text);
                    existingSach.NgayNhap = dateNgayNhap;

                    thuVienDBContext.SaveChanges();
                    try
                    {
                        showMessage("cập nhật sách thành công");
                        var listSach = thuVienDBContext.SACHes.ToList();
                        resetData();
                        BindGridThuVien(listSach);
                        tinhTongTien();
                    }
                    catch (Exception ex)
                    {
                        showMessage(ex.Message);
                    }
                }
                else
                {
                    showMessage("không tồn tại mã sách cần sửa,vui lòng nhập mã sách khác");
                }
            }
        }

        private void tinhTongTien()
        {
            double tongTienSach = 0;
            foreach (DataGridViewRow row in dgvThuVien.Rows)
            {
                if (row.Cells[5].Value != null
                    && float.TryParse(row.Cells[5].Value.ToString(), out float TriGia))
                {
                    tongTienSach += TriGia;

                }
            }
            lbTotalPrice.Text = tongTienSach.ToString() + "VNĐ";
        }
        private void btnTongTien_Click(object sender, EventArgs e)
        {
            tinhTongTien();
        }

        private void searchTenSach(string tensach)
        {
            var listTenSach = thuVienDBContext.SACHes.Where(s => s.TenSach.ToLower().Trim().Contains(tensach.ToLower())).ToList();
            if(listTenSach != null)
            {
                var thuVienContextNew = new ThuVienDBContext();
                BindGridThuVien(listTenSach);
            }
            else
            {
                showMessage("Không tìm thấy dữ liệu cần tìm");
                dgvThuVien.Rows.Clear();
            }
        }
        private void searchTacGia(string tacgia)
        {
            var listTacGia = thuVienDBContext.SACHes.Where(s => s.TacGia.ToLower().Trim().Contains(tacgia.ToLower())).ToList();
            if (listTacGia != null)
            {
                BindGridThuVien(listTacGia);
            }
            else
            {
                showMessage("Không tìm thấy dữ liệu cần tìm");
                dgvThuVien.Rows.Clear();
            }
        }
        private void searchTenSachAndTacGia(string tensach,string tacgia)
        {
            var listTenSachAndTacGia = thuVienDBContext.SACHes
                .Where(s => s.TenSach.ToLower().Trim().Contains(tensach.ToLower())
                ||s.TacGia.ToLower().Trim().Contains(tacgia.ToLower())).ToList();
            if (listTenSachAndTacGia != null)
            {
                BindGridThuVien(listTenSachAndTacGia);
            }
            else
            {
                showMessage("Không tìm thấy dữ liệu cần tìm");
                dgvThuVien.Rows.Clear();
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTenSachKey = txtSearchTenSach.Text.ToString().Trim().ToLower();
            string searchTenTacGiaKey = txtSearchTacGia.Text.ToString().Trim().ToLower();

            if (string.IsNullOrEmpty(searchTenSachKey) && string.IsNullOrEmpty(searchTenTacGiaKey))
            {
                showMessage("Vui Long Nhập dữ liệu để tìm kiếm");
                BindGridThuVien(thuVienDBContext.SACHes.ToList());
            }
            else if (!string.IsNullOrEmpty(searchTenSachKey) && !string.IsNullOrEmpty(searchTenTacGiaKey))
            {
                searchTenSachAndTacGia(searchTenSachKey, searchTenTacGiaKey);
            }
            else
            {
                if(!string.IsNullOrEmpty(searchTenSachKey))
                {
                    searchTenSach(searchTenSachKey);
                }
                if(!string.IsNullOrEmpty(searchTenTacGiaKey))
                {
                    searchTacGia(searchTenTacGiaKey);
                }

            }

        }

        private void btnDuLieuBanDau_Click(object sender, EventArgs e)
        {
            var listSach = thuVienDBContext.SACHes.ToList();
            if (listSach != null)
            {
                BindGridThuVien(listSach);
                txtSearchTacGia.Text = txtSearchTenSach.Text = "";
            }
        }

        private void txtNamXB_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
