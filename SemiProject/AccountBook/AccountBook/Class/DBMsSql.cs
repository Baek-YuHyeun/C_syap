using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountBook.Class
{
    public class DBMsSql
    {
        SqlConnection sConn = new SqlConnection();
        SqlCommand sCmd = new SqlCommand();
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\C#\SemiProject\Account_db\Account_db.mdf;Integrated Security=True;Connect Timeout=30";

        SqlDataAdapter adpt;

        public void Connection() 
        {
            try 
            {
                sConn.ConnectionString = connString;
                sConn.Open();
                sCmd.Connection = sConn;
            } 
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
                throw; //이거 왜 하는거지?
            }
            //sConn.Close();
        }

        public void EndConn() 
        {
            sConn.Close();
        }

        //return 값이 있는 sql 문
        public DataSet SelectAll(string table_name) 
        {
            try 
            {
                DataSet ds = new DataSet();

                string sSql = $"select * from {table_name}";
                adpt = new SqlDataAdapter(sSql, sConn); //excuteReader 랑 비슷한 역할
                adpt.Fill(ds, table_name); //읽어온 데이터를 ds(dataSet)에 저장

                if (ds.Tables.Count > 0)
                    return ds;
                else
                    return null;

            } 
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        public DataSet SelectDetail(string table_name, string col_name, string where="") 
        {
            try
            {
                DataSet ds = new DataSet();

                string sSql = $"select {col_name} from {table_name} {where}";
                adpt = new SqlDataAdapter(sSql, sConn); 
                adpt.Fill(ds, table_name); //읽어온 데이터를 ds(dataSet)에 저장

                if (ds.Tables.Count > 0) //ds에 저장된 레코드가 1개 이상일때
                    return ds;
                else
                    return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        //return 값이 없는 sql 문 (ExecuteNonQuery() 함수 사용)
        public void Insert(string table_name, string value) 
        {
            try 
            {
                string sSql = $"insert into {table_name} values ({value})";
                sCmd.CommandText = sSql;
                sCmd.ExecuteNonQuery(); 
                //sConn.Close();
                
            } 
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
               
            }
        }

        public void Update(string table_name, string setValue, string where) 
        {
            try
            {
                string sSql = $"update {table_name} set {setValue} where {where}";
                sCmd.CommandText = sSql;
                sCmd.ExecuteNonQuery();
                //sConn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        public void DeleteAll(string table_name) 
        {
            try
            {
                string sSql = $"delete from {table_name}";
                sCmd.CommandText = sSql;
                sCmd.ExecuteNonQuery();
                //sConn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        public void DeleteDetail(string table_name, string where)
        {
            try
            {
                string sSql = $"delete from {table_name} where {where}";
                sCmd.CommandText = sSql;
                sCmd.ExecuteNonQuery();
                //sConn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
              
            }
        }



    }
}
