using System;
using System.Configuration;
using System.Data;
using System.Windows;
using PETFactoryERP.DAL;

namespace PETFactoryERP.UI.Views
{
    public partial class EditDailyWageWindow : Window
    {
        private readonly int _wageId;
        private readonly string _connectionString;
        private readonly DatabaseConnection _dbConnection;

        public EditDailyWageWindow(int wageId)
        {
            InitializeComponent();
            _wageId = wageId;
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _dbConnection = new DatabaseConnection(_connectionString);
            LoadWageData();
            LoadProductionOrders();

            // ربط حدث تغيير القيم بحساب الإجمالي
            HoursWorkedTextBox.TextChanged += (s, e) => CalculateTotal();
            WagePerHourTextBox.TextChanged += (s, e) => CalculateTotal();
        }

        private void LoadWageData()
        {
            try
            {
                DataTable wageData = _dbConnection.ExecuteQuery($@"
                    SELECT * FROM DailyWages WHERE WageID = {_wageId}
                ");

                if (wageData.Rows.Count > 0)
                {
                    DataRow row = wageData.Rows[0];
                    EmployeeNameTextBox.Text = row["EmployeeName"].ToString();
                    WageDatePicker.SelectedDate = (DateTime)row["WageDate"];
                    HoursWorkedTextBox.Text = row["HoursWorked"].ToString();
                    WagePerHourTextBox.Text = row["WagePerHour"].ToString();
                    NotesTextBox.Text = row["Notes"]?.ToString() ?? "";
                    CalculateTotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تحميل البيانات: {ex.Message}");
                this.Close();
            }
        }

        private void LoadProductionOrders()
        {
            try
            {
                DataTable orders = _dbConnection.ExecuteQuery(@"
                    SELECT OrderID, OrderNumber 
                    FROM ProductionOrders 
                    WHERE Status IN ('Pending', 'In Progress')
                    ORDER BY OrderNumber DESC
                ");

                OrderCombo.Items.Clear();
                foreach (DataRow row in orders.Rows)
                {
                    OrderCombo.Items.Add($"{row["OrderNumber"]} (#{row["OrderID"]})");
                }
            }
            catch { }
        }

        private void CalculateTotal()
        {
            try
            {
                if (decimal.TryParse(HoursWorkedTextBox.Text, out decimal hours) &&
                    decimal.TryParse(WagePerHourTextBox.Text, out decimal wagePerHour))
                {
                    decimal total = hours * wagePerHour;
                    TotalWageBlock.Text = total.ToString("N2") + " ج.م";
                }
            }
            catch { }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmployeeNameTextBox.Text) ||
                !decimal.TryParse(HoursWorkedTextBox.Text, out decimal hours) ||
                !decimal.TryParse(WagePerHourTextBox.Text, out decimal wagePerHour))
            {
                MessageBox.Show("الرجاء ملء جميع الحقول المطلوبة بشكل صحيح", "تحذير", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DateTime wageDate = WageDatePicker.SelectedDate ?? DateTime.Today;
                decimal totalWage = hours * wagePerHour;

                string query = $@"
                    UPDATE DailyWages SET
                        EmployeeName = '{EmployeeNameTextBox.Text.Replace("'", "''")}',
                        WageDate = '{wageDate:yyyy-MM-dd}',
                        HoursWorked = {hours},
                        WagePerHour = {wagePerHour},
                        TotalWage = {totalWage},
                        Notes = '{NotesTextBox.Text.Replace("'", "''")}',
                        UpdatedDate = GETDATE()
                    WHERE WageID = {_wageId}
                ";

                _dbConnection.ExecuteCommand(query);
                MessageBox.Show("تم تحديث السجل بنجاح", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("هل أنت متأكد من حذف هذا السجل؟", "تأكيد الحذف", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    string query = $"DELETE FROM DailyWages WHERE WageID = {_wageId}";
                    _dbConnection.ExecuteCommand(query);
                    MessageBox.Show("تم حذف السجل بنجاح", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"خطأ: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
