using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
// panel1: 초기화면-환자
// panel1_1: 당일 예약 환자
// panel1_2: 환자 신규 등록
// panel1_3: 그래프

// panel2: 초기화면-근태
// panel3: 초기화면-재고
// panel4: 초기화면-관리자
// panel5: 대기자 화면
// panel6: 진료실1
// panel7: 진료실2

namespace Project_4
{

   
    public partial class Form_main : Form
    {
        private Form_login form_login;

        public Form_main(Form_login form_login, bool auth, string Name)
        {
            InitializeComponent();

           
            button4.Visible = auth;     // 관리자계정일때 보여주기
            if (auth == false)       // 간호사 계정일때
            {
                label1.Text = Name + " 간호사님 환영합니다.";
            }
            else
            {
                label1.Text = Name + " 관리자님 환영합니다.";
            }
            this.form_login = form_login;



            List<List<string>> lines = Patient.Line();
            for (int i = 0; i < lines.Count; i++)
            {

                if (lines[i][lines.Count] == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    for (int j = 0; j < lines.Count - 1; j++)
                    {
                        textBox1.Text += lines[i][j] + " ";
                    }
                    textBox1.Text += Environment.NewLine;
                }
            }

        }



        private void Form_main_Load(object sender, EventArgs e)
        {
            timer1.Interval = 100; // 타이머 간격 100ms
            timer1.Start();  // 타이머 시작  

            chart1.Series.Clear();
            chart1.Series.Add("이번주 방문객 수");

            List<List<string>> lines = Patient.Line();

            // 이번 주 월요일부터 금요일까지의 요일 설정
            DateTime thisMonday = GetThisWeekMonday();
            textBox3.Text = thisMonday.ToString("yyyy-MM-dd");
            for (int i = 0; i < lines.Count; i++)
            {

                if (thisMonday.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    //textBox2.Text = "HI";
                }
            }
            DayOfWeek[] weekdays = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };

            // 각 요일에 대한 데이터 포인트 추가
            foreach (DayOfWeek weekday in weekdays)
            {
                DateTime nextWeekday = thisMonday.AddDays((int)weekday - 1); // 해당 요일의 날짜 가져오기
                textBox2.Text = nextWeekday.ToString("yyyy-MM-dd");
                //int reservationCount = GetReservationCount(lines, nextWeekday); // 해당 날짜의 예약자 수 가져오기
                chart1.Series[0].Points.AddXY(nextWeekday.ToString("ddd"), lines.Count); // 요일과 예약자 수 차트에 추가
            }

            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            // y축 설정
            this.chart1.ChartAreas[0].AxisY.Minimum = 0;
            this.chart1.ChartAreas[0].AxisY.Maximum = 30;

            // x축 설정
            chart1.ChartAreas[0].AxisX.Interval = 1; // 각 요일 간격
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Auto; // 요일 간격 설정

        }

        // 현재 주의 월요일을 가져오는 메서드
        private DateTime GetThisWeekMonday()
        {
            DateTime today = DateTime.Today;
            int diff = today.DayOfWeek - DayOfWeek.Monday;
            return today.AddDays(-diff);
        }
/*
        private List<List<string>> ReadVisitorData(string filePath)
        {
            List<List<string>> lines = Patient.Line();
            List<string> strList = lines.ConvertAll(new Converter<int, string>(intToString));


            try
            {
                foreach (string line in lines)
                {
                    string[] parts = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> entry = new List<string>();

                    // 예약 정보가 예상대로 포함되어 있는지 확인
                    if (parts.Length >= 2)
                    {
                        entry.Add(parts[0]); // 날짜
                        entry.Add(parts[1]); // 방문자 수
                    }

                    visitorData.Add(entry);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("파일을 읽는 동안 오류가 발생했습니다: " + ex.Message);
            }

            return visitorData;
        }*/

        // 해당 날짜에 대한 예약자 수를 반환하는 메서드
        private int GetReservationCount(List<List<string>> lines, DateTime date)
        {
            int count = 0;
            foreach (var line in lines)
            {
                DateTime reservationDate = DateTime.Parse(line[0]); // 예약일자를 DateTime으로 변환
                textBox3.Text = reservationDate.ToString();
                if (reservationDate.Date == date.Date) // 예약일자와 입력받은 날짜가 같은 경우
                {
                    count += int.Parse(line[1]); // 예약자 수 추가
                }
            }
            return count;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel8.Visible = false;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
            panel8.Visible = false;


        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            panel4.Visible = false;
            panel8.Visible = false;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = true;
            panel8.Visible = false;

        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToString("F"); // label1에 현재날짜시간 표시, F:자세한 전체 날짜/시간
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel8.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Patient.Pnew();
        }


    }
    class Patient
    {

        public static List<string> text_ss()
        {
            string P_path = "C:/Users/301-24/Desktop/Reservation.txt";      // 환자 정보 텍스트 파일
            string[] lines = File.ReadAllLines(P_path);       // 파일의 모든 줄을 읽어옴
            List<string> linesList = lines.ToList();
            return linesList;
        }

        public static List<List<string>> Line()
        {
            List<List<string>> patient_list = new List<List<string>>(); // 각 줄의 데이터를 저장한 리스트를 저장할 리스트
            List<string> lines = Patient.text_ss();
            for (int i = 0; i < lines.Count; i++)          // 파일 내용을 한 줄씩 읽어가며 처리
            {
                List<string> patientData = lines[i].Split('\t').ToList();
                patient_list.Add(patientData); // 각 환자 데이터 리스트를 전체 리스트에 추가
            }
            return patient_list;
        }

        public static void Pnew()
        {
            string text_name = Form_login.form_main.textBox4.Text;
            string text_ssnum = Form_login.form_main.textBox5.Text;
            string text_phone = Form_login.form_main.textBox6.Text;
            string text_address = Form_login.form_main.textBox7.Text;
            DateTime Pnew_age = DateTime.Parse(text_ssnum);
            DateTime today = DateTime.Now;   //여기까지가 텍스트박스에 입력한 텍스트 불러오기--이걸 텍스트파일안에 탭기준으로 나눠서 한줄로 넣기
            string text_age = ((today.Year - Pnew_age.Year)+1).ToString();
            string patient = "C:/Users/301-24/Desktop/Patient.txt";
            string Pnew_info = $"{text_name}\t{text_age}\t{text_ssnum}\t{text_phone}\t{text_address}";
            File.AppendAllText(patient, Pnew_info + Environment.NewLine);
        }
    }
}
