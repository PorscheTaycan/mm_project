using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_4
{
   
    public partial class Form_login : Form
    {
        public static Form_main form_main;

        public Form_login()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // 화면 사이즈 변경 불가능
        }

        private void Form_login_Load(object sender, EventArgs e)
        {
            label1.BackColor = Color.Transparent; // label1 배경화면 투명
            label2.BackColor = Color.Transparent; // label2 배경화면 투명

            MaximizeBox = false; // 최대화 불가능
        }

        private void label1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
        }


   private void button1_Click(object sender, EventArgs e)
        {
            string path = "C:/Users/301-24/Desktop/Info.txt";  // Info 파일 경로
            string[] lines = File.ReadAllLines(path);       // 파일의 모든 줄을 읽어옴
            for (int i = 0; i < lines.Length; i++)          // 파일 내용을 한 줄씩 읽어가며 처리
            {
                if (i > 0)                                  // 행 번호가 1 이상인 경우
                {
                    string[] columns = lines[i].Split('\t');    // 탭으로 구분된 열을 분리
                    if (columns[columns.Length-3] == textBox1.Text && columns[columns.Length-2] == textBox2.Text)            // 첫 번째 비교할 열의 값과 textBox1의 값이 같은 지 확인, 두 번째 비교할 열의 값과 textBox2의 값이 같은지 확인.
                    {
                        bool isAdmin = columns[columns.Length - 1] == "O";  // 권한이 O일때 관리자
                        string Name = columns[0];
                        this.Hide();  // Form_login 창을 숨김
                        form_main = new Form_main(this, isAdmin, Name);
                        form_main.ShowDialog();
                        this.Close();  //form_main 창이 없어지면 login창도 없어진다. 
                        return;
                    }
                    else
                    {
                        login_error login_Error_NotFound = new login_error();  // 아이디가 틀렸을 경우 에러창이 뜸
                        login_Error_NotFound.ShowDialog();
                        break;
                    }
                }
            }

            
        }



        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { 
                button1_Click(sender, e);
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }

        }
    }
}
