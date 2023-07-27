using QLCF.DAO;
using QLCF.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLCF
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource tableList = new BindingSource();
        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            LoadData();
        }
        
        #region methods
        void LoadData()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            dtgvFoodCategory.DataSource = categoryList;
            dtgvTable.DataSource = tableList;
            LoadDateTimePickerBill();
            LoadListFood();
            LoadListFoodCategory();
            LoadCategoryIntoCombobox(cbbFoodCategory);
            LoadCategoryIntoCombobox(cbbNameCategory);
            AddFoodBinding();
            AddCategoryBinding();
            AddTableBinding();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListTable();
            LoadStatusTable();
            LoadAccount();
            AddAccountBinding();
        }
        void LoadCategoryIntoCombobox(System.Windows.Forms.ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        //Bill
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(checkIn, checkOut, Convert.ToInt32(txtPageBill.Text));
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1, 0,0,0);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
        }
        //Food
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetFood();
            LoadCategoryIntoCombobox(cbbFoodCategory);

        }       
        void AddFoodBinding()
        {
            txtFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtIDFood.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmPicerFood.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        //Category
        void AddCategoryBinding()
        {
            txtIDCategory.DataBindings.Add(new Binding("Text", dtgvFoodCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            cbbNameCategory.DataBindings.Add(new Binding("Text", dtgvFoodCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));           
        }
        void LoadListFoodCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }
        //Table
        void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.LoadTableList();
        }
        void LoadStatusTable()
        {
            txtStatusTable.Text = "Trống";
        }
        void AddTableBinding()
        {
            txtIDTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtNameTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtStatusTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Status", true, DataSourceUpdateMode.Never));
        }
        //Account
        void AddAccountBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
            dtgvAccount.Columns[0].HeaderText = "Tên tài khoản";
            dtgvAccount.Columns[1].HeaderText = "Tên hiển thị";
            dtgvAccount.Columns[2].HeaderText = "Loại tài khoản";
        }
        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công!\nMật khẩu mặc định là: @123.", "Thông báo");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại!", "Thông báo");
            }

            LoadAccount();
        }
        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount2(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công!", "Thông báo");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại!", "Thông báo");
            }

            LoadAccount();
        }
        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Bạn không thể xóa tài khoản bạn đang sử dụng!", "Thông báo");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công!", "Thông báo");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại!", "Thông báo");
            }

            LoadAccount();
        }
        void ResetPass(string userName)
        {
            
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công! Mật khẩu mặc định là: @123\nBạn có thể vào quản lý tài khoản để đổi lại mật khẩu!", "Thông báo");
                }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại", "Thông báo");
            }
        }
        #endregion
        #region events
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Bill
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }
        //Food
        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }
        private void bntView_Click(object sender, EventArgs e)
        {

            LoadListFood();
        }
        private void txtIDFood_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["Column3"].Value;
                    Category category = CategoryDAO.Instance.GetCategoryByID(id);
                    cbbFoodCategory.SelectedItem = category;
                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }
        }
        private void bntNewFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int categoryID = (cbbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmPicerFood.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công!", "Thông báo");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn!", "Thông báo");
            }
        }
        private void bntEditFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int categoryID = (cbbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmPicerFood.Value;
            int id = Convert.ToInt32(txtIDFood.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công!", "Thông báo");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn!", "Thông báo");
            }
        }
        private void BntDeleteFood_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa món này ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.No)
            {
                int id = Convert.ToInt32(txtIDFood.Text);
                if (FoodDAO.Instance.DeleteFood(id))
                {
                    MessageBox.Show("Xóa món thành công!", "Thông báo");
                    LoadListFood();
                    if (deleteFood != null)
                        deleteFood(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa thức ăn!", "Thông báo");
                }
            }
            
        }
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }
        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }
        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        private void bntSearch_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txtsearch.Text);
        }
        //Category
        private void bntViewCategory_Click(object sender, EventArgs e)
        {
            LoadListFoodCategory();
        }
        private void txtIDCategory_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFoodCategory.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFoodCategory.SelectedCells[0].OwningRow.Cells["Column6"].Value;
                    Category category = CategoryDAO.Instance.GetCategoryByID(id);
                    cbbFoodCategory.SelectedItem = category;
                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }
        }
        private void bntAddCategory_Click(object sender, EventArgs e)
        {
            string name = cbbNameCategory.Text;
            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm loại món thành công!", "Thông báo");
                LoadListFoodCategory();
                if (insertCategory != null)
                    insertCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm loại món!", "Thông báo");
            }
        }
        private void bntEditCatelory_Click(object sender, EventArgs e)
        {
            string name = cbbNameCategory.Text;
            int id = Convert.ToInt32(txtIDCategory.Text);
            if (CategoryDAO.Instance.UpdateCategory(id, name))
            {
                MessageBox.Show("Sửa tên loại món thành công!", "Thông báo");
                LoadListFoodCategory();
                if (updateCategory != null)
                    updateCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa tên loại món!", "Thông báo");
            }
        }
        private void bntDeleteCategory_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa loại món này ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.No)
            {
                int id = Convert.ToInt32(txtIDCategory.Text);

                if (CategoryDAO.Instance.DeleteCategory(id))
                {
                    MessageBox.Show("Xóa loại món thành công!", "Thông báo");
                    LoadListFoodCategory();
                    if (deleteCategory != null)
                        deleteCategory(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa loại món!", "Thông báo");
                }
            }
           
        }
        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }
        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }
        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }
        }
        //Table
        private void bntViewTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }
        private void txtIDTable_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvTable.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvTable.SelectedCells[0].OwningRow.Cells["Column7"].Value;
                    Table table = TableDAO.Instance.GetListTableByID(id);
                    cbbFoodCategory.SelectedItem = table;
                    int index = -1;
                    int i = 0;                   
                }
            }
            catch { }
        }
        private void bntAddTable_Click(object sender, EventArgs e)
        {
            string name = txtNameTable.Text;
            string status = "Trống";
            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn thành công!", "Thông báo");
                LoadListTable();
                if (insertTable != null)
                    insertTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn món!", "Thông báo");
            }
        }
        private void bntDeleteTable_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa bàn này ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.No)
            {
                int id = Convert.ToInt32(txtIDTable.Text);

                if (TableDAO.Instance.DeleteTable(id))
                {
                    MessageBox.Show("Xóa bàn thành công!", "Thông báo");
                    LoadListTable();
                    if (deleteTable != null)
                        deleteTable(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa bàn!", "Thông báo");
                }
            }
            
        }
        private void bntEditTable_Click(object sender, EventArgs e)
        {
            string name = txtNameTable.Text;
            int id = Convert.ToInt32(txtIDTable.Text);
            if (TableDAO.Instance.UpdateTable(id, name))
            {
                MessageBox.Show("Sửa bàn món thành công!", "Thông báo");
                LoadListTable();
                if (updateTable != null)
                    updateTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa bàn món!", "Thông báo");
            }
        }
        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }
        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }
        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }
        //Account
        private void bntViewUser_Click(object sender, EventArgs e)
        {
            AddAccountBinding();
        }
        private void bntAddUser_Click(object sender, EventArgs e)
        {
                string userName = txtUserName.Text;
                string displayName = txtDisplayName.Text;
                int type = (int)nmType.Value;
                AddAccount(userName, displayName, type);                 
        }
        private void bntDeleteUser_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.No)
            {
                string userName = txtUserName.Text;
                DeleteAccount(userName);
            }
            
        }
        private void BntEditUser_Click(object sender, EventArgs e)
        {
                string userName = txtUserName.Text;
                string displayName = txtDisplayName.Text;
                int type = (int)nmType.Value;
                EditAccount(userName, displayName, type);                     
        }
        private void txtResetPass_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đặt lại mật khẩu mặc định?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.No)
            {
                string userName = txtUserName.Text;
                ResetPass(userName);
            }     
        }
        private void fAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn quay về giao diện chính ?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
        private void btnFirst_Click(object sender, EventArgs e)
        {
            txtPageBill.Text = "1";
        }
        private void bntPreviours_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPageBill.Text);
            if (page > 1)
                page--;
            txtPageBill.Text = page.ToString();
        }
        private void bntNext_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            if (page < sumRecord)
                page++;
            txtPageBill.Text = page.ToString();
        }
        private void bntLast_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            int lastPage = sumRecord / 10;
            if (sumRecord % 10 != 0)
                lastPage++;
            txtPageBill.Text = lastPage.ToString();
        }
        private void txtPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txtPageBill.Text));
        }
        private void btnViewReportDT_Click(object sender, EventArgs e)
        {
            DateTime datefrom = dtpkFromDate.Value;
            DateTime dateto = dtpkToDate.Value;
            this.uSP_GetListBillByDateForReportTableAdapter.Fill(this.qLCFDataSetDoanhThu.USP_GetListBillByDateForReport, dtpkFromDate.Value, dtpkToDate.Value);
            this.reportDoanhThu.RefreshReport();
        }
        #endregion


    }
}
