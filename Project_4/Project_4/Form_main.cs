using Project_4;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
        DateTime nextMonths = DateTime.Now.AddMonths(1);
        public static Form_inv_add form_inv_add;
        private Form_login form_login;

        public Form_main(Form_login form_login, bool auth, string Name)
        {

            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle; // 화면 사이즈 변경 불가능
            MaximizeBox = false; // 최대화 불가능
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
            foreach (var patientData in lines)
            {
                string reservationDate = patientData.Last(); // 예약 날짜를 가져옴
                if (reservationDate == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    string patientInfo = string.Join(" ", patientData.Take(patientData.Count - 1)); // 마지막 예약 날짜를 제외하고 환자 정보를 가져
                    textBox1.Text += patientInfo.ToString() + Environment.NewLine;
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



            //Grid 없애기
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            chart1.ChartAreas[1].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[1].AxisY.MajorGrid.Enabled = false;


            // y축 설정
            this.chart1.ChartAreas[0].AxisY.Minimum = 0;
            this.chart1.ChartAreas[0].AxisY.Maximum = 30;

            this.chart1.ChartAreas[1].AxisY.Minimum = 0;
            this.chart1.ChartAreas[1].AxisY.Maximum = 30;

            //차트 추가
            chart1.Series.Add("연령별");
            chart1.Series[1].ChartArea = "ChartArea2";

            // x축 설정
            chart1.ChartAreas[0].AxisX.Interval = 1; // 각 요일 간격
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Auto; // 요일 간격 설정


            // x축 설정 ChartArea2
            chart1.ChartAreas[1].AxisX.Interval = 1; // 각 연령 간격
            chart1.ChartAreas[1].AxisX.IntervalType = DateTimeIntervalType.Auto; // 연령 간격 설정



            // 연령별 x축
            chart1.Series[1].Points.AddXY("~19", GetAgeGroupCount(lines, "1"));
            chart1.Series[1].Points.AddXY("~29", GetAgeGroupCount(lines, "10-20"));
            chart1.Series[1].Points.AddXY("~39", GetAgeGroupCount(lines, "21-30"));
            chart1.Series[1].Points.AddXY("~49", GetAgeGroupCount(lines, "31-40"));
            chart1.Series[1].Points.AddXY("~59", GetAgeGroupCount(lines, "50-60"));
            chart1.Series[1].Points.AddXY("60~", GetAgeGroupCount(lines, "61"));


            // 월요일부터 금요일까지의 예약자 수를 차트에 추가
            for (int i = 0; i < 5; i++)
            {
                DateTime currentDay = thisMonday.AddDays(i);
                int reservationCount = GetReservationCount(lines, currentDay);

                chart1.Series[0].Points.AddXY(currentDay.ToString("ddd"), reservationCount); // 요일과 예약자 수 차트에 추가

            }



        }

        // 해당 날짜에 대한 예약자 수를 반환하는 메서드
        private int GetReservationCount(List<List<string>> lines, DateTime date)
        {
            int count = 0;
            foreach (var line in lines)
            {
                DateTime reservationDate = DateTime.Parse(line.Last()); // 예약일자를 DateTime으로 변환
                if (reservationDate.Date == date.Date) // 예약일자와 입력받은 날짜가 같은 경우
                {
                    count++; // 예약자 수 추가
                }
            }
            return count;
        }


        //연령별 인원 수 
        private int GetAgeGroupCount(List<List<string>> lines, string ageGroup)
        {
            int count = 0;
            foreach (var line in lines)
            {
                string patientAge = line[3]; // Assuming the age information is in the 4th column (adjust as needed)
                if (patientAge == ageGroup)
                {
                    count++; // Increment count for the specified age group
                }
            }
            return count;
        }



        // 현재 주의 월요일을 가져오는 메서드
        private DateTime GetThisWeekMonday()
        {
            DateTime today = DateTime.Today;
            int diff = today.DayOfWeek - DayOfWeek.Monday;
            return today.AddDays(-diff);
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
            Form_login.form_main.dataGridView1.Rows.Clear();
            //Inventory.Line_inv();
            textBox3_1.Focus();


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
            Patient.Psearch();
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel8.Visible = true;
        }



        private void button7_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = monthCalendar1.SelectionStart; // 달력 날짜

            // 오늘의 날짜를 가져옵니다.
            DateTime today = DateTime.Today;

            monthCalendar1.AddBoldedDate(selectedDate);// BoldedDates에 오늘의 날짜를 추가합니다.
            monthCalendar1.UpdateBoldedDates();// BoldedDates를 업데이트하여 변경 사항을 적용합니다.
            monthCalendar1.ForeColor = Color.Pink;
            // 선택된 날짜와 오늘의 날짜를 비교합니다.
            if (selectedDate.Date == today)
            {
                MessageBox.Show("출근 하셨습니다.");
            }
            else
            {
                MessageBox.Show("오늘은 " + DateTime.Now.ToString("yyyy년-MM월-dd일") + " 이아닙니다");
                //BoldedDates를 업데이트한것을 제거
                monthCalendar1.RemoveBoldedDate(selectedDate);
                // BoldedDates를 업데이트하여 변경 사항을 적용합니다.
                monthCalendar1.UpdateBoldedDates();
            }
        }

        private void textBox3_1_Click(object sender, EventArgs e)
        {
            if (textBox3_1.Text == "코드")
            {
                textBox3_1.Text = null;
            }
        }

        private void textBox3_4_Click(object sender, EventArgs e)
        {
            if (textBox3_4.Text == "수량")
            {
                textBox3_4.Text = null;
            }
        }
        private void button3_1_Click(object sender, EventArgs e)          // 제품코드 검색 버튼
        {
            dataGridView1.Rows.Clear();
            Inventory.line_inv();
            textBox3_1.Focus();
        }
        private void button3_2_Click(object sender, EventArgs e)        // 제품 등록 버튼
        {
            form_inv_add = new Form_inv_add();
            form_inv_add.ShowDialog();
        }
        private void button3_3_Click(object sender, EventArgs e)
        {
            Inventory.use_inv();
            textBox3_4.Text = null;
        }
        private void button3_4_Click(object sender, EventArgs e)
        {
            // 주문버튼을 클릭하면 현재 선택되어있는 제품을
            // textBox3_4.Text만큼을 장바구니 페이지에 입력하고 textBox3_4는 초기화
            // 장바구니에는 삭제 버튼 재고 앞뒤로 +-버튼
        }

        private void button3_5_Click(object sender, EventArgs e)
        {
            // 장바구니의 결재버튼을 누르면
            // 현재 장바구니에 들어있는 내용을 관리자에게 전송
            // 장바구니 초기화

        }
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 포커스가 datagridview.columns[0]에 있는지 확인
            if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.ColumnIndex == 0)
            {
                textBox3_4.Visible = true;
            }
            else
            {
                textBox3_4.Visible = false;
            }
        }

        

        private void main_textBox1_Click(object sender, EventArgs e)
        {
            panel8.Visible = true;
        }

        private void button5_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Patient.Psearch();

            if (panel1.Visible == true)
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;
            }
            else if (panel2.Visible == true)
            {
                panel1.Visible = false;
                panel2.Visible = true;
                panel3.Visible = false;
                panel4.Visible = false;
            }
            else if (panel3.Visible == true)
            {
                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = true;
                panel4.Visible = false;
            }
            else
            {
                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = true;
            }
            panel8.Visible = true;
        }

        private void main_textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Patient.Psearch();
            }
        }

        private void button8_Click_2(object sender, EventArgs e)
        {
            Patient.outpatient();
        }

       

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 포커스가 datagridview.columns[0]에 있는지 확인
            if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.ColumnIndex == 0)    // 제품코드열을 포커스해도 textBox3_4가 뜸  
            {
                textBox3_4.Visible = true;
            }
            else
            {
                textBox3_4.Visible = false;
            }
        }

        private void monthCalendar1_DateChanged_1(object sender, DateRangeEventArgs e)
        {
            textBox11.Clear();

            string selectedDate = monthCalendar1.SelectionStart.ToShortDateString(); //달력에서 오늘 날짜
            {
                List<List<string>> linez = Patient.Att();
                foreach (var AttData in linez)  //배열 반복
                {
                    string scheduleDate = AttData.Last();
                    if (scheduleDate == selectedDate)
                    {
                        string AttInfo = string.Join(" ", AttData.Take(AttData.Count - 1)); //마지막 날짜를 제외하고 다 가져옴
                        textBox11.Text += AttInfo + Environment.NewLine;
                    }
                }
            }
            List<List<string>> lines = Patient.Line();
            textBox10.Clear();
            string reserDate = monthCalendar1.SelectionStart.ToShortDateString(); //달력에서 오늘 날짜
            foreach (var patientData in lines)
            {
                try
                {

                    string reservationDate = patientData.Last(); // 예약 날짜를 가져옴
                    if (reserDate == reservationDate)
                    {
                        string patientInfo = string.Join(" ", patientData.Take(patientData.Count)); // 마지막 예약 날짜를 제외하고 환자 정보를 가져옴
                        textBox10.Text += patientInfo + Environment.NewLine;
                        textBox10.Text += Environment.NewLine;
                    }
                }
                catch (Exception ex)
                {
                    Text = $"예약 날짜 비교 중 오류 발생: {ex.Message}";
                }
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            try
            {
                Patient.Pnew();
                textBox4.Text = null;
                textBox5.Text = null;
                textBox6.Text = null;
                textBox7.Text = null;
            }
            catch
            {
                label10.Visible = true;
            }

        }

        //글자수 제한도 만들어야함
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            char[] inputchars = textBox4.Text.ToCharArray();
            var sb = new StringBuilder();
            
            foreach (var item in inputchars)
            {
                if (char.GetUnicodeCategory(item) == UnicodeCategory.OtherLetter)
                {
                    sb.Append(item);
                    label10.Visible = false;
                }
                else
                {
                    label10.Visible = true;
                }
            }
            textBox4.Text = sb.ToString().Trim();

        }


        private void EventHandler()
        {
            throw new NotImplementedException();
        }

        //주민번호 19991111이런식으로 입력하면 1999-11-11로 변환해서 저장되게 수정할것
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자와 백스페이스와 '-'만 입력 형식에 맞지않게써도 뒤에 사용할 데이트타임변환에서 막혀서 메모장으로 안들어감
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == '-'))
            {
                e.Handled = true;
                label10.Visible = true;
            }
            else
            {
                label10.Visible = false;
            }
        }

        //한글입력시에도 경고문 뜨게 수정
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == '-'))
            {
                e.Handled = true;
                label10.Visible = true;
            }
            else
            {
                label10.Visible = false;
            }
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            label10.Visible = false;
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            label10.Visible = false;
        }

        private void textBox6_Click(object sender, EventArgs e)
        {
            label10.Visible = false;
        }

        private void textBox7_Click(object sender, EventArgs e)
        {
            label10.Visible = false;
        }
    }

}
class Patient
{

    public static List<string> text_ss()
    {
        string P_path = "Reservation.txt";      // 환자 정보 텍스트 파일
        string[] lines = File.ReadAllLines(P_path);       // 파일의 모든 줄을 읽어옴
        List<string> linesList = lines.ToList();
        return linesList;
    }

    public static List<string> text_aa()
    {
        string PP_path = "Patient.txt";      // 환자 정보 텍스트 파일
        string[] Patients = File.ReadAllLines(PP_path);       // 파일의 모든 줄을 읽어옴
        List<string> linesLists = Patients.ToList();
        return linesLists;
    }


    public static List<string> Att_cal()
    {
        string schedule = "schedule.txt";
        string[] sch = File.ReadAllLines(schedule);
        List<string> schcel = sch.ToList();
        return schcel;
    }
    public static List<List<string>> Att()
    {
        List<string> AttData = new List<string>();
        List<List<string>> Att_list = new List<List<string>>();
        List<string> linez = Patient.Att_cal();
        for (int i = 0; i < linez.Count; i++)          // 파일 내용을 한 줄씩 읽어가며 처리
        {
            AttData = linez[i].Split('\t').ToList();
            Att_list.Add(AttData); // 각 환자 데이터 리스트를 전체 리스트에 추가
        }
        return Att_list;
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
    public static List<List<string>> patients()
    {
        List<string> lines = Patient.text_aa();
        List<List<string>> patients_list = new List<List<string>>(); // 각 줄의 데이터를 저장한 리스트를 저장할 리스트

        for (int i = 0; i < lines.Count; i++)          // 파일 내용을 한 줄씩 읽어가며 처리
        {
            List<string> patientDatas = lines[i].Split('\t').ToList();
            patients_list.Add(patientDatas); // 각 환자 데이터 리스트를 전체 리스트에 추가
        }
        return patients_list;
    }


    public static void Pnew()
    {
        string text_name = Form_login.form_main.textBox4.Text;
        string text_ssnum = Form_login.form_main.textBox5.Text;
        string text_phone = Form_login.form_main.textBox6.Text;
        string text_address = Form_login.form_main.textBox7.Text;
        DateTime Pnew_age = DateTime.Parse(text_ssnum);
        DateTime today = DateTime.Now;   //여기까지가 텍스트박스에 입력한 텍스트 불러오기--이걸 텍스트파일안에 탭기준으로 나눠서 한줄로 넣기

        int age = today.Year - Pnew_age.Year + 1;
        string text_age = age < 10 ? "0" + age.ToString() : age.ToString();
        //string text_age = ((today.Year - Pnew_age.Year)+1).ToString();

        string Pnew_info = $"{text_name}\t{text_age}\t{text_ssnum}\t{text_phone}\t{text_address}";

        string patient = "C:/Users/301-10/Desktop/Patient.txt";

        File.AppendAllText(patient, Pnew_info + Environment.NewLine);
    }

    public static void Psearch()
    {
        try
        {
            string PatientFilePath = "Patient.txt"; // 환자 정보 파일 경로
            string P_search = Form_login.form_main.main_textBox1.Text; // 환자 검색창에서 입력된 이름
            string[] lines = File.ReadAllLines(PatientFilePath); // 파일의 모든 줄 읽기

            // 데이터그리드뷰 초기화
            Form_login.form_main.dataGridView2.Rows.Clear();
            Form_login.form_main.dataGridView2.Columns.Clear();

            string[] headers = lines[0].Split('\t');
            foreach (string header in headers)
            {
                Form_login.form_main.dataGridView2.Columns.Add(header, header);
            }

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(P_search) || lines[i].Contains(P_search))
                {
                    string[] columns = lines[i].Split('\t'); // 탭으로 구분된 열을 분리

                    DataGridViewRow row = new DataGridViewRow();
                    foreach (string column in columns)
                    {
                        DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                        cell.Value = column;
                        row.Cells.Add(cell);
                    }
                    Form_login.form_main.dataGridView2.Rows.Add(row);
                }
            }
        }
        catch
        {

        }
    }

    public static void outpatient()
    {
        Form outpatient = new outpatient();
        outpatient.ShowDialog();
    }



}



class Inventory
{
    public static List<string> text_inv()               // 재고
    {
        string P_path = "Inventory Manager.txt";      // 재고 정보 텍스트 파일
        List<string> lines = File.ReadAllLines(P_path).ToList();       // 파일의 모든 줄을 읽고 리스트화
        return lines;
    }
    public static void line_inv()         // 제품 검색
    {
        // string x, y;
        List<string> inventory_data = new List<string>(); // 각 줄의 데이터를 저장할 리스트 생성
        List<List<string>> inventory_list = new List<List<string>>(); // 각 줄의 데이터를 저장한 리스트를 저장할 리스트
        List<string> lines = Inventory.text_inv();
        for (int i = 0; i < lines.Count; i++)          // 파일 내용을 한 줄씩 읽어가며 처리
        {
            inventory_data = lines[i].Split('\t').ToList();      // Inventory Manager의 각 줄
            if (i > 0)     // 카테고리 라인 제외
            {
                // textBox3_1에 글자 입력하고 버튼을 누르면 입력된 글자 수 까지 비교
                // lines의 첫번째 열의 값과 textBox3_1의 값을 비교해서 같으면 이름 출력

                // Inventory Manager의 i번째 재고의 글자와 textBox3_1의 글자가 같다면
                if (inventory_data[0].Substring(0, Form_login.form_main.textBox3_1.Text.Length) == Form_login.form_main.textBox3_1.Text)
                {
                    // dataGridView1에 행 추가
                    Form_login.form_main.dataGridView1.Rows.Add(inventory_data[0], inventory_data[1], inventory_data[2], false); // dataGridView1에 행 추가
                }
            }
        }
    }
    public static void use_inv()         //  사용 
    {
        string P_path = "Inventory Manager.txt";
        string[] inventory_list = File.ReadAllLines(P_path);
        int rowIndex = Form_login.form_main.dataGridView1.CurrentRow.Index;          // 선택된 행
                                                                                     // 사용버튼을 클릭하면 datagridview에 현재 선택한 제품의 재고의 값에서 
                                                                                     // textBox3_4.Text의 값만큼 감소하고 textBox3_4는 초기화
                                                                                     // 
        if (Form_login.form_main.textBox3_4 != null)
        {
            int x = Int32.Parse(Form_login.form_main.textBox3_4.Text);                                               // x = 수량 입력
            int y = Int32.Parse(Form_login.form_main.dataGridView1.Rows[rowIndex].Cells[2].Value.ToString());       // y = 재고 수량
            if (y >= x)
            {
                Form_login.form_main.dataGridView1.Rows[rowIndex].Cells[2].Value = y - x;                                // 재고 - 입력한 값
                                                                                                                         // Inventory Manager파일에 변화 적용
                List<string> inventory_data = new List<string>(); // 각 줄의 데이터를 저장할 리스트 생성
                List<string> lines = Inventory.text_inv();
                int code = 0, name = 0, value = 0;
                // int d_code = dataGridView1.Columns[].Cells[IndexOf("제품코드")];    // 첫번째 열에서 제품코드의 인덱스 위치
                for (int i = 0; i < lines.Count; i++)          // 파일 내용을 한 줄씩 읽어가며 처리
                {
                    inventory_data = lines[i].Split('\t').ToList();      // Inventory Manager의 각 줄
                    if (i == 0)                                              // 카테고리 행 일때
                    {
                        code = inventory_data.IndexOf("코드");               // 코드의 열번호
                        name = inventory_data.IndexOf("제품명");               // 제품명의 열번호
                        value = inventory_data.IndexOf("재고");               // 재고의 열번호
                    }
                    else
                    {
                        if (Form_login.form_main.dataGridView1.Rows[rowIndex].Cells[0].Value.ToString() == inventory_data[code])    // 선택한 행의 제품코드와 Inventory Manager의 제품코드와 같다면
                        {
                            int result = Int32.Parse(inventory_data[value]);        // 현재 텍스트파일에 있는 재고값
                            result -= x;                                            // 재고값 - 사용값
                            inventory_data[value] = result.ToString();              // 텍스트파일에 있는 재고값을 변경 - 텍스트파일에 적용안됨
                        }
                    }
                }
            }
        }
    }
}