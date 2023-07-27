using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLCF.DAO;
using QLCF.DTO;
using static QLCF.fAccount;

namespace QLCF
{
    public partial class fMain : Form
    {
        private Account loginAccount;
        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }
        public fMain(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
            LoadTable();
            LoadCategory();
            LoadComboboxTable(cbSwitchTable);
        }
        #region Method
        void ChangeAccount(int type)
        {           
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";           
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }
        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tablelist = TableDAO.Instance.LoadTableList();
            foreach (Table item in tablelist)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;              
                btn.Click += Btn_Click;
                btn.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }
                flpTable.Controls.Add(btn);
                LoadTableNow();
            }
        }
        void LoadTableNow()
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                txbTableNow.Text = "Chưa chọn bàn!";
            }
            else
                txbTableNow.Text = table.Name;
        }
        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }
        void ShowBill(int id, bool sale)
        {
            lsvBill.Items.Clear();
            List<QLCF.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            double totalPrice = 0;
            foreach (QLCF.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");           
            txbTamTinh.Text = totalPrice.ToString("c", culture);           
            int dt = (int)nmDisCount.Value;
            double finalTotalPrice = totalPrice - (totalPrice/100) * dt;
            double dicount = (totalPrice/100) * dt;

            if (sale == true)
            {
                txbGiamGia.Text = dicount.ToString("c", culture);
                txbTotalPrice.Text = finalTotalPrice.ToString("c", culture);
            }
            else
            {
                double zero = 0;
                txbGiamGia.Text = zero.ToString("c", culture);
                txbTotalPrice.Text = totalPrice.ToString("c", culture);
            }
        }
        #endregion

        #region Events
        private void Btn_Click(object sender, EventArgs e)
        {
            int tablleID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tablleID, false);
            LoadTableNow();

        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát ra trình đăng nhập ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.No)
            {
                this.Close();
            }
        }
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccount f = new fAccount(LoginAccount);
            this.Hide();
            f.UpdateAccount += F_UpdateAccount1; ;
            f.ShowDialog();
            this.Show();
        }
        private void F_UpdateAccount1(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }
        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            this.Hide();
            f.loginAccount = LoginAccount;
            f.InsertFood += F_InsertFood;
            f.DeleteFood += F_DeleteFood;
            f.UpdateFood += F_UpdateFood;
            f.InsertCategory += F_InsertCategory;
            f.DeleteCategory += F_DeleteCategory;
            f.UpdateCategory += F_UpdateCategory;
            f.UpdateTable += F_UpdateTable;
            f.InsertTable += F_InsertTable;
            f.DeleteTable += F_DeleteTable;
            f.ShowDialog();
            this.Show();
        }
        private void F_DeleteTable(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                if (cbxGiamGia.Checked)

                    ShowBill((lsvBill.Tag as Table).ID, true);
                if (!cbxGiamGia.Checked)
                    ShowBill((lsvBill.Tag as Table).ID, false);
            }
            LoadTable();
        }
        private void F_InsertTable(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                if (cbxGiamGia.Checked)

                    ShowBill((lsvBill.Tag as Table).ID, true);
                if (!cbxGiamGia.Checked)
                    ShowBill((lsvBill.Tag as Table).ID, false);
            }
            LoadTable();
        }
        private void F_UpdateTable(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                if (cbxGiamGia.Checked)

                    ShowBill((lsvBill.Tag as Table).ID, true);
                if (!cbxGiamGia.Checked)
                    ShowBill((lsvBill.Tag as Table).ID, false);
            }
            LoadTable();
        }
        private void F_UpdateCategory(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                if (cbxGiamGia.Checked)

                    ShowBill((lsvBill.Tag as Table).ID, true);
                if (!cbxGiamGia.Checked)
                    ShowBill((lsvBill.Tag as Table).ID, false);
            }
            LoadCategory();
        }
        private void F_DeleteCategory(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                if (cbxGiamGia.Checked)

                    ShowBill((lsvBill.Tag as Table).ID, true);
                if (!cbxGiamGia.Checked)
                    ShowBill((lsvBill.Tag as Table).ID, false);
            }
            LoadCategory();
            LoadTable();
        }
        private void F_InsertCategory(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                if (cbxGiamGia.Checked)

                    ShowBill((lsvBill.Tag as Table).ID, true);
                if (!cbxGiamGia.Checked)
                    ShowBill((lsvBill.Tag as Table).ID, false);
            }
            LoadCategory();
        }
        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                if (cbxGiamGia.Checked)

                    ShowBill((lsvBill.Tag as Table).ID, true);
                if (!cbxGiamGia.Checked)
                    ShowBill((lsvBill.Tag as Table).ID, false);
            }
        }
        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if(lsvBill.Tag != null)
            {
                if (cbxGiamGia.Checked)

                    ShowBill((lsvBill.Tag as Table).ID, true);
                if (!cbxGiamGia.Checked)
                    ShowBill((lsvBill.Tag as Table).ID, false);
            }          
            LoadTable();
        }
        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                if (cbxGiamGia.Checked)

                    ShowBill((lsvBill.Tag as Table).ID, true);
                if (!cbxGiamGia.Checked)
                    ShowBill((lsvBill.Tag as Table).ID, false);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null)
                return;
            Category selected = cb.SelectedItem as Category;
            id = selected.ID;
            LoadFoodListByCategoryID(id);
        }
        private void bntAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn!");
                return;
            }
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;
            if (idBill == -1)
            {
                if(count <= 0 )
                {
                    MessageBox.Show("Số lượng món bạn thêm vào phải lớn hơn 0!", "Cảnh báo");
                }
                else
                {
                    BillDAO.Instance.InsertBill(table.ID);
                    BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
                }
                
            }
            else
            {       
                    BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);              
            }
            if (cbxGiamGia.Checked)
            {
                ShowBill(table.ID, true);
            }
            if (!cbxGiamGia.Checked)
                ShowBill(table.ID, false);
            LoadTable();
        }
        private void bntTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).ID;
            int id2 = (cbSwitchTable.SelectedItem as Table).ID;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển bàn {0} qua bàn {1}", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwichTable(id1, id2);
                BillDAO.Instance.ClearBill(id1);
                lsvBill.Items.Clear();
                if (cbxGiamGia.Checked)
                {
                    ShowBill(id1, true);
                }
                if (!cbxGiamGia.Checked)
                    ShowBill(id1, false);
                LoadTable();
                LoadTable();
            }
        }
        private void bntCheck_Click_1(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Xin vui lòng chọn bàn để thanh toán!", "Thông báo");
            }
            else
            {
                int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
                int discount = (int)nmDisCount.Value;
                double totalPrice = Convert.ToDouble(txbTamTinh.Text.Split(',')[0].Replace(".", ""));
                double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;
                CultureInfo culture = new CultureInfo("vi-VN");
                if (idBill != -1)
                {
                    if (totalPrice < 0)
                    {
                        MessageBox.Show("Bạn không thể thanh toán bàn này!\nKhi món ăn trong bill có số lượng là âm!", "Cảnh báo");
                    }
                    else
                    {
                        if (cbxGiamGia.Checked)
                        {

                            if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0}\nvà giảm " + discount + "%\nTổng tiền: {1}.000đ x {2}% = {3}.000đ", table.Name, totalPrice.ToString(), discount, finalTotalPrice.ToString()), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                            {
                                BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                                ShowBill(table.ID, true);
                                LoadTable();
                            }
                        }
                        if (!cbxGiamGia.Checked)
                        {
                            if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0}\nTổng tiền: {1}.000đ", table.Name, totalPrice.ToString()), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                            {

                                BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                                ShowBill(table.ID, false);
                                LoadTable();
                            }
                        }
                    }       
                }
            }

        }
        private void cbGiamGia_CheckedChanged(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if (cbxGiamGia.Checked)
            {
                ShowBill(table.ID, true);
            }
            if (!cbxGiamGia.Checked)
                ShowBill(table.ID, false);
        }
        private void bntUp_Click(object sender, EventArgs e)
        {
            nmFoodCount.UpButton();
        }
        private void bntDown_Click(object sender, EventArgs e)
        {
            nmFoodCount.DownButton();
        }
        private void thôngTinTàiKhoảnToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                Color.White, 1, ButtonBorderStyle.Solid,
                Color.White, 1, ButtonBorderStyle.Solid,
                Color.White, 1, ButtonBorderStyle.Solid,
                Color.White, 1, ButtonBorderStyle.Solid);
        }
        private void bntExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát ra trình đăng nhập ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.No)
            {
                this.Close();
            }
        }
        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bntCheck_Click_1(this, new EventArgs());
        }
        private void thêmMónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bntAddFood_Click(this, new EventArgs());
        }
        private void resetBillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn!");
                return;
            }
            if (MessageBox.Show(string.Format("Bạn có chắn muốn reset lại hóa đơn {0} không?", table.Name), "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.No)
            {
                if (table == null)
                {
                    MessageBox.Show("Hãy chọn bàn!");
                    return;
                }
                BillDAO.Instance.ClearBill(table.ID);
                lsvBill.Items.Clear();
                if (cbxGiamGia.Checked)
                {
                    ShowBill(table.ID, true);
                }
                if (!cbxGiamGia.Checked)
                    ShowBill(table.ID, false);
                LoadTable();
            }       
        }
        private void nmDisCount_ValueChanged(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if (cbxGiamGia.Checked)
            {
                ShowBill(table.ID, true);
            }
            if (!cbxGiamGia.Checked)
                ShowBill(table.ID, false);
        }

        #endregion

        
    }
}
