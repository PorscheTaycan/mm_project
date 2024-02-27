using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project_4
{
    public partial class outpatient : Form
    {
        private Form_login form_Login;
        public outpatient()
        {
            InitializeComponent();
        }

        private void outpatient_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = comboBox2.Items.Count - 1;
            //outpatientGrid = new DataGridView();


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

 

        private void button2_Click(object sender, EventArgs e)
        {
            //저장 누를시 해당 환자에 인적사항과 증상이 메인화면 좌측 진료대기자로 넘어가게
        }

        public static void gridview()
        {
        /*    Form_login form_Login = new Form_login();
            Form_main form_Main = new Form_main(form_Login: form_Login);
            //outpatient outpatientGrid = form_Main.dataGridView1.SelectedRows[0];
*/

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = null;
        }
    }
}
