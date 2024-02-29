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
        public static async void Inew()
        {
            string Icode = Form_main.form_inv_add.textBox1.Text;          // 신규제품 코드
            string Iname = Form_main.form_inv_add.textBox2.Text;         // 신규제품 이름
            string Icount = Form_main.form_inv_add.textBox3.Text;         // 신규제품 재고
            string inventory = "Inventory Manager.txt";
            List<string> lines = File.ReadAllLines(inventory).ToList();
            string Inew_info = $"{Icode}\t{Iname}\t{Icount}";

            int count = 0;                          // 중복 값 확인
            for (int i = 0; i < lines.Count; i++)                         // 제품 텍스트파일 행만큼 반복
            {
                List<string> columns = lines[i].Split('\t').ToList();
                if (Icode == columns[0])                            // 제품 코드가 같다면
                {
                    count++;
                    break;
                }
            }
            if (count == 0)             // 중복이 없다면
            {
                File.AppendAllText(inventory, Inew_info + Environment.NewLine);         // 제품 추가
                List<string> sort = File.ReadAllLines(inventory).ToList();
                string firstLine = sort.FirstOrDefault();
                List<string> linesToSort = sort.Skip(1).ToList();
                linesToSort.Sort();
                List<string> sortedLines = new List<string>();
                sortedLines.Add(firstLine);
                sortedLines.AddRange(linesToSort);
                File.WriteAllLines(inventory, sortedLines);
                // 각 라인들의 이름을 비교해서 사전순으로 정렬
                // 카테고리 줄은 고정
            }
            else
            {
                Form_main.form_inv_add.label4.Text = "이미 포함된 제품입니다.";
                await Task.Delay(1000);                     // 일정시간후 에러메시지 삭제
                Form_main.form_inv_add.label4.Text = null;
            }
        }
    }
}