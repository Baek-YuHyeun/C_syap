using AccountBook.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountBook
{
    public partial class fMain : Form
    {
        //DB 관련 함수 모아놓은 클래스 연결 (객체 생성해서)
        DBMsSql msDB = new DBMsSql(); 

        public fMain()
        {
            InitializeComponent();
        }
        
        public int bookRowCnt; //제로 베이스
        public void initAccountBook() 
        {
            string sDgBook = "num as 'NO.', use_date as '날짜', use_type as '구분', use_cost as '금액', use_details as '상세내역'";
            DataSet books = msDB.SelectDetail("account_history",sDgBook);
            dgAccountList.DataSource = books.Tables[0];

            bookRowCnt = books.Tables[0].Rows.Count;

        }

        private void fMain_Load(object sender, EventArgs e)
        {
            msDB.Connection();
            initAccountBook();
            
        }

        public string dgcol0, dgcol1, dgcol2, dgcol3;

        private void dgAccountList_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            /*string str = dgEnterAccountList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            //string str1 = dgCbUsetypeCol2.Items[dgCbUsetypeCol2.Index].ToString();

            if (e.ColumnIndex == 1)
            {
                dgEnterAccountList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = str;
                dgcol1 = dgEnterAccountList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }*/
            /*if (e.ColumnIndex == 2)
            {
                dgEnterAccountList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = str;
                dgcol2 = (string)dgEnterAccountList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }

            if (e.ColumnIndex == 3)
            {
                dgEnterAccountList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = str;
                dgcol3 = (string)dgEnterAccountList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }*/
        }

        private void dgAccountList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string str = dgEnterAccountList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            if (e.ColumnIndex == 2)
            {
                dgEnterAccountList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = str;
                dgcol2 = (string)dgEnterAccountList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }

            if (e.ColumnIndex == 3)
            {
                dgEnterAccountList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = str;
                dgcol3 = (string)dgEnterAccountList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }
        }

        private void fMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            msDB.EndConn();
        }

        private void dgEnterAccountList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                string value = $"{bookRowCnt + 1},'{dgcol0}','{dgcol1}','{dgcol2}','{dgcol3}'";
                msDB.Insert("account_history", value);
            }
        }

        private void dgEnterAccountList_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
           /* string str; 
            //string str1 = dgCbUsetypeCol2.Items[dgCbUsetypeCol2.Index].ToString();

            *//*if (e.Cell.ColumnIndex == 1)
            {
                dgEnterAccountList.Rows[e.Cell.RowIndex].Cells[e.Cell.ColumnIndex].Value = str; 
                dgcol1 = dgEnterAccountList.Rows[e.Cell.RowIndex].Cells[e.Cell.ColumnIndex].Value.ToString(); 
            }*//*
            if (e.Cell.ColumnIndex == 2)
            {
                str = dgEnterAccountList.Rows[e.Cell.RowIndex].Cells[e.Cell.ColumnIndex].Value.ToString();
                dgEnterAccountList.Rows[e.Cell.RowIndex].Cells[e.Cell.ColumnIndex].Value = str;
                dgcol2 = (string)dgEnterAccountList.Rows[e.Cell.RowIndex].Cells[e.Cell.ColumnIndex].Value;
            }

            if (e.Cell.ColumnIndex == 3)
            {
                str = dgEnterAccountList.Rows[e.Cell.RowIndex].Cells[e.Cell.ColumnIndex].Value.ToString();
                dgEnterAccountList.Rows[e.Cell.RowIndex].Cells[e.Cell.ColumnIndex].Value = str;
                dgcol3 = (string)dgEnterAccountList.Rows[e.Cell.RowIndex].Cells[e.Cell.ColumnIndex].Value;
            }*/

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
//            dgAccountList.Rows[0].Cells[0].Value = "";
            DateTime dt = dateTimePicker1.Value;
            dt.Date.ToLongDateString();
//            dgcol0 = $"{dgAccountList.Rows[0].Cells[0].Value}";
            dgcol0 = $"{dt:YYYY-MM-DD}";
        }

      

        private void dgBtnCol5_Click(object sender, DataGridViewCellEventArgs e) 
        {
            string value = $"{bookRowCnt + 1},'{dgcol0}','{dgcol1}','{dgcol2}','{dgcol3}'";
            msDB.Insert("account_history", value);
        }
    }
}
