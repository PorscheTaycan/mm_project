using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project_4
{
    public partial class Form_inv_add : Form
    {
        public Form_inv_add()
        {
            InitializeComponent();
        }

        private void Form_inv_add_Load(object sender, EventArgs e)
        {
            // 코드 제품명 재고 사진(?)
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != null && textBox2.Text != null && textBox3.Text != null)        // textBox들이 null이어도 작동
            {
                Inv_add.Inew();
                textBox1.Text = null;
                textBox2.Text = null;
                textBox3.Text = null;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox1.Text != null && textBox2.Text != null && textBox3.Text != null)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    button1_Click(sender, e);
                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox1.Text != null && textBox2.Text != null && textBox3.Text != null)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    button1_Click(sender, e);
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox1.Text != null && textBox2.Text != null && textBox3.Text != null)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    button1_Click(sender, e);
                }
            }
        }


    }
    class Inv_add
    {
        public static List<string> text_ss()
        {
            string I_path = "C:/Users/301-24/Desktop/Inventory Manager.txt";       // 제품 정보 텍스트 파일
            List<string> lines = File.ReadAllLines(I_path).ToList();                // 파일의 모든 줄을 읽고 리스트화
            return lines;
        }
        public static List<List<string>> Line()
        {
            List<string> inventory_data = new List<string>();                   // 각 줄의 데이터를 저장할 리스트 생성
            List<List<string>> inventory_list = new List<List<string>>();       // 각 줄의 데이터를 저장한 리스트를 저장할 리스트
            List<string> lines = Patient.text_ss();                             // lines는 text_ss()의 lines를 리턴받음
            for (int i = 0; i < lines.Count; i++)                               // 파일 내용을 한 줄씩 읽어가며 처리
            {
                inventory_data = lines[i].Split('\t').ToList();
                inventory_list.Add(inventory_data);                             // 각 재품 데이터 리스트를 전체 리스트에 추가
            }
            return inventory_list;
        }
        public static void Inew()
        {
            string Icode = Form_main.form_inv_add.textBox1.Text;          // 신규제품 코드
            string Iname = Form_main.form_inv_add.textBox2.Text;         // 신규제품 이름
            string Icount = Form_main.form_inv_add.textBox3.Text;         // 신규제품 재고
            string inventory = "C:/Users/301-24/Desktop/Inventory Manager.txt";
            string Inew_info = $"{Icode}\t{Iname}\t{Icount}";
            File.AppendAllText(inventory, Inew_info + Environment.NewLine);
            // 각 라인들의 이름을 비교해서 사전순으로 정렬
            List<string> lines = File.ReadAllLines(inventory).ToList();
            for (int i = 0; i < lines.Count; i++)
            {
                List<string> columns = lines[i].Split('\t').ToList();    // lines[i]를 탭으로 나눠서 리스트에 배치, {"코드","제품명","재고"}
            }
        }
    }
}