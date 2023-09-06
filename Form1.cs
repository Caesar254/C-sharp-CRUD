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
using MySql.Data.MySqlClient;

namespace StudentReg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Load();
        }
        //Open connection
        //string conn_string = "server =localhost; uid=root; password=123456; database=mydb";
        MySqlConnection conn = new MySqlConnection("server =localhost; uid=root; password=123456; database=mydb");

        // conn.ConnectionString = conn_string;
        // conn.Open();

        //mysql command and query
        //   string sql = "select * from  user";
        //MySqlCommand cmd = new MySqlCommand(sql, conn);

        // MySqlDataReader reader = cmd.ExecuteReader();
        MySqlCommand cmd;
        MySqlDataReader read;
       // MySqlDataAdapter drr;
        string id;
        bool Mode = true;
        string sql;



        public void Load()
        {
            try
            {
                sql = "select * from student_reg";
                cmd = new MySqlCommand(sql, conn);
                conn.Open();
                read = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();

                while (read.Read())
                {
                    dataGridView1.Rows.Add(read[0], read[1], read[2], read[3]);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }



        public void getID(String id)
        {
            sql = "select * from student_reg where id = '" + id + "'  ";
            cmd = new MySqlCommand(sql, conn);
            conn.Open();
            read = cmd.ExecuteReader();

            while (read.Read())
            {
                txtName.Text = read[1].ToString();
                txtCourse.Text = read[2].ToString();
                txtFee.Text = read[3].ToString();
            }
            conn.Close();
        }



        //if the Mode is true means allow to add the new records. Mode false means update the existing records.
        private void button1_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string course = txtCourse.Text;
            string fee = txtFee.Text;


            if (Mode == true)
            {
                sql = "insert into student_reg(stname,course,fee) values(@stname,@course,@fee)";
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@stname", name);
                cmd.Parameters.AddWithValue("@course", course);
                cmd.Parameters.AddWithValue("@fee", fee);
                MessageBox.Show("Record Added!");
                cmd.ExecuteNonQuery();

                //clearing the textboxes
                txtName.Clear();
                txtCourse.Clear();
                txtFee.Clear();
                txtName.Focus(); // cursor focus on textbox

            }
            else
            {
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "update student_reg set stname = @stname, course= @course,fee = @fee where id = @id";
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@stname", name);
                cmd.Parameters.AddWithValue("@course", course);
                cmd.Parameters.AddWithValue("@fee", fee);
                cmd.Parameters.AddWithValue("@id", id);
                MessageBox.Show("Record Updated!");
                cmd.ExecuteNonQuery();

                txtName.Clear();
                txtCourse.Clear();
                txtFee.Clear();
                txtName.Focus();
                save.Text = "Save";
                Mode = true;

            }
            conn.Close();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                getID(id);
                save.Text = "Edit";

            }
            else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "delete from student_reg where id  = @id";
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id ", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Deleted!");
               
                conn.Close();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Load();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtCourse.Clear();
            txtFee.Clear();
            txtName.Focus();
            button1.Text = "Save";
            Mode = true;

        }
    }
    }

