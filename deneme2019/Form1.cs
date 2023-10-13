using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace deneme2019 //
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private DataTable masterTable;

        public DataTable CreateDataTable()
        {
            masterTable = new DataTable();
            masterTable.Columns.Add("Id", typeof(int));
            masterTable.Columns.Add("Name");
            masterTable.Columns.Add("IsActive", typeof(bool));
            masterTable.Columns.Add("OrderCount", typeof(int));
            masterTable.Columns.Add("RegistrationDate", typeof(DateTime));

            for (int i = 0; i < 10; i++)
            {
                masterTable.Rows.Add(i, "Name" + i, i % 2 == 0, i * 10, DateTime.Now.AddDays(i));
            }

            return masterTable;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = CreateDataTable();
            gridControl1.AllowDrop = true;
            gridView1.OptionsBehavior.Editable = false;

            gridControl1.DragOver += gridControl1_DragOver;
            gridControl1.DragDrop += gridControl1_DragDrop;
            gridView1.MouseDown += gridView1_MouseDown;
            gridView1.MouseMove += gridView1_MouseMove;
        }
        private void gridControl1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DataRow)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;

            e.Effect = DragDropEffects.Move;
        }

        private void gridControl1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Data.DataRow"))
            {
                GridView gridView = gridControl1.MainView as GridView;
                GridControl grid = sender as GridControl;

                Point point = grid.PointToClient(new Point(e.X, e.Y));
                int targetRowHandle = gridView.CalcHitInfo(point).RowHandle;
                if (targetRowHandle >= 0)
                {
                    DataRow draggedDataRow = e.Data.GetData("System.Data.DataRow") as DataRow;
                    DataRow targetDataRow = gridView.GetDataRow(targetRowHandle);

                    string nereden = draggedDataRow["Name"].ToString();
                    string nereye =  targetDataRow["Name"].ToString();

                    MessageBox.Show("BURADAN -> "+nereden+ "\n\nBURAYA -> " + nereye+"\n\nTut Sürükle Bırak Yapıldı");
                    // Burada draggedDataRow'dan targetDataRow'a veri taşıma işlemini gerçekleştirin.
                    // Verilerinizi güncelleyin ve görüntüyü yeniden çizdirin.
                }
            }
        }
        GridHitInfo downHitInfo = null;
        private void gridView1_MouseMove(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Button == MouseButtons.Left && downHitInfo != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(downHitInfo.HitPoint.X - dragSize.Width / 2,
                    downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    string cellTextValue = view.GetDataRow(downHitInfo.RowHandle)["Id"].ToString();

                    DataRow dataRow = view.GetDataRow(downHitInfo.RowHandle);

                    view.GridControl.DoDragDrop(dataRow, DragDropEffects.Move);
                    downHitInfo = null;
                    DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                }
            }
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            downHitInfo = null;
            GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None) return;
            if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
                downHitInfo = hitInfo;

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
    }
}
