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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using static Project_4.Program;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
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
        int month, year;
        private DateTime clickedDatetime;
        private UserControldays clickedpanel;


        public Form_main(Form_login form_login, bool auth, string Name, string Birth, string Rent)
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
            //displaDays();

        }
/*        private void displaDays()
        {

            DateTime now = DateTime.Now;
            month = now.Month;
            year = now.Year;
            string monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);// 현재 월에 해당하는 달의 이름을 가져와서 라벨에 표시
            label18.Text = year + "년 " + monthname;

            DateTime startofthemonth = new DateTime(year, month, 1);// 현재 연도와 월에 해당하는 달의 시작 요일과 날짜 수를 계산
            int days = DateTime.DaysInMonth(year, month);
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 4;
            for (int i = 0; i < dayoftheweek; i++)  // 달력의 시작 부분에 공백을 추가
            {
                UserControl ucblank = new UserControlBlank();
                Daycontainer.Controls.Add(ucblank);
            }
            for (int i = 1; i <= days; i++)// 해당 월의 각 날짜를 패널에 추가
            {
                UserControldays ucdays = new UserControldays();
                ucdays.days(i);
                Daycontainer.Controls.Add(ucdays);


                ucdays.OnPanelClick += UserControlDays_OnPanelClick;// 클릭 이벤트 핸들러를 등록
            }

        }*/
        public void UserControlDays_OnPanelClick(object sender, EventArgs e)
        {
            UserControldays clickedPanel = sender as UserControldays;//클릭한 패널 가져오기
            string clickedDate = clickedPanel.DayLabelText; //클릭한 패널에서 날자 정보 추출


            DateTime clickedDateTime = new DateTime(year, month, int.Parse(clickedDate));// 클릭된 패널의 날짜를 메시지로 표시.
            clickedDatetime = clickedDateTime;
            List<List<string>> linez = Patient.Att();

            dataGridView4.Rows.Clear();
            dataGridView3.Rows.Clear();

            foreach (var patientData in linez)
            {
                string scheduleDate = patientData.Last();
                if (scheduleDate == clickedDateTime.ToString("yyyy-MM-dd"))
                {

                    dataGridView4.Rows.Add(patientData.ToArray()); // 클릭한 날짜와 스케줄 날짜가 일치하는 경우 DataGridView에 행 추가
                }
            }
            dataGridView2.Rows.Clear(); // 그리드뷰의 모든 행 제거

            List<List<string>> lines = Patient.Line(); // 예약된 환자 정보 가져오기
            foreach (var patientData in lines)
            {
                try
                {

                    string reservationDate = patientData.Last(); // 예약 날짜를 가져옴
                    if (clickedDateTime.ToString("yyyy-MM-dd") == reservationDate)
                    {

                        dataGridView3.Rows.Add(patientData.ToArray()); // DataGridView에 행 추가
                    }

                }
                catch (Exception ex)
                {
                    Text = $"예약 날짜 비교 중 오류 발생: {ex.Message}";
                }

            }

        }
        public void SetDayLabel(string day)
        {

            label18.Text = $"{DateTime.Now.Year}년 {DateTime.Now.Month}월 {day}일";// UserControlDays로부터 전달된 날짜 값을 받아서 라벨에 설정
        }
        public void label_Click(object sender, MouseEventArgs e)
        {

            UserControldays clickedPanel = sender as UserControldays; //클릭한 패널 가져오기
            clickedpanel = clickedPanel;
            string clickedDate = clickedPanel.lbdays.Text; //클릭한 패널에서 날자 정보 추출
            DateTime clickedDateTime = new DateTime(year, month, int.Parse(clickedDate));// 클릭된 패널의 날짜를 메시지로 표시합니다.

            if (DateTime.Now.ToString("yyyy-MM-dd") == clickedDateTime.ToString("yyyy-MM-dd"))
            {


                List<List<string>> linez = Patient.Att();

                dataGridView4.Rows.Clear();
                dataGridView3.Rows.Clear();

                foreach (var patientData in linez)
                {
                    string scheduleDate = patientData.Last();
                    if (scheduleDate == clickedDateTime.ToString("yyyy-MM-dd"))// 클릭한 날짜와 스케줄 날짜가 일치하는 경우 DataGridView에 행 추가
                    {

                        dataGridView4.Rows.Add(patientData.ToArray());
                    }
                }
                dataGridView2.Rows.Clear(); // 그리드뷰의 모든 행 제거

                // 예약된 환자 정보 가져오기
                List<List<string>> lines = Patient.Line();
                foreach (var patientData in lines)
                {
                    try
                    {

                        string reservationDate = patientData.Last(); // 예약 날짜를 가져옴
                        if (clickedDateTime.ToString("yyyy-MM-dd") == reservationDate)
                        {
                            // DataGridView에 행 추가
                            dataGridView3.Rows.Add(patientData.ToArray());
                        }

                    }
                    catch (Exception ex)
                    {
                        Text = $"예약 날짜 비교 중 오류 발생: {ex.Message}";
                    }

                }
            }


        }
        private void Form_main_Load(object sender, EventArgs e)
        {
            // main_textBox1.BorderColor = Color.Gray;


            this.button3_1.BackgroundImage = Properties.Resources.search1;
            this.button3_1.BackgroundImageLayout = ImageLayout.Stretch;

            this.button9.BackgroundImageLayout = ImageLayout.Stretch;           // button사이즈에 맞춰 이미지 추가
            this.button10.BackgroundImageLayout = ImageLayout.Stretch;
            this.button3_4.BackgroundImageLayout = ImageLayout.Stretch;
            this.button3_5.BackgroundImageLayout = ImageLayout.Stretch;


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

                int v = chart1.Series[0].Points.AddXY(currentDay.ToString("ddd"), reservationCount); // 요일과 예약자 수 차트에 추가

            }


        }

        // 해당 날짜에 대한 예약자 수를 반환하는 메서드`
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

            Font ft1 = new Font("G마켓 산스 TTF Light", 10);

            Font ft2 = new Font("G마켓 산스 TTF Light", 16, FontStyle.Underline);
            button1.Font = ft2;
            button2.Font = ft1;
            button3.Font = ft1;
            button4.Font = ft1;






        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
            panel8.Visible = false;
            Font ft1 = new Font("G마켓 산스 TTF Light", 10);

            Font ft2 = new Font("G마켓 산스 TTF Light", 16, FontStyle.Underline);
            button2.Font = ft2;
            button1.Font = ft1;
            button3.Font = ft1;
            button4.Font = ft1;



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
            Font ft1 = new Font("G마켓 산스 TTF Light", 10);

            Font ft2 = new Font("G마켓 산스 TTF Light", 16, FontStyle.Underline);
            button3.Font = ft2;
            button2.Font = ft1;
            button1.Font = ft1;
            button4.Font = ft1;



        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = true;
            panel8.Visible = false;
            Font ft1 = new Font("G마켓 산스 TTF Light", 10);

            Font ft2 = new Font("G마켓 산스 TTF Light", 16, FontStyle.Underline);
            button4.Font = ft2;
            button2.Font = ft1;
            button3.Font = ft1;
            button1.Font = ft1;
            //함수

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
            /*DateTime selectedDate = monthCalendar1.SelectionStart; // 달력 날짜

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
            }*/
        }

        // 재고----------------------------------------------------------------------------
        private static bool IsNumeric(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
        private void textBox3_1_Click(object sender, EventArgs e)
        {
            if (textBox3_1.Text == "코드")
            {
                textBox3_1.Text = null;
            }
        }

        private void textBox3_2_Click(object sender, EventArgs e)
        {
            textBox3_2.Text = null;
        }
        private void button3_1_Click(object sender, EventArgs e)          // 제품코드 검색 버튼
        {
            dataGridView1.Rows.Clear();
            Inventory.line_inv();
            textBox3_1.Focus();
            textBox3_2.Visible = false;                                 // 검색하면 수량입력 창 없애기
        }
        private void button3_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3_1_Click(sender, e);
            }
        }
        private void textBox3_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3_1_Click(sender, e);
            }
        }
        private void button3_2_Click(object sender, EventArgs e)        // 제품 등록 버튼
        {
            form_inv_add = new Form_inv_add();
            form_inv_add.ShowDialog();
        }
        private void button3_3_Click(object sender, EventArgs e)        // 사용버튼
        {
            Inventory.use_inv();
            textBox3_2.Text = null;
        }
        private void button3_4_Click(object sender, EventArgs e)        // 주문버튼
        {
            Inventory.order_inv();
            textBox3_2.Text = null;
        }

        private void button3_5_Click(object sender, EventArgs e)        // 결재버튼
        {
            MessageBox.Show(Form_login.form_main.dataGridView5.Columns[0].ToString());
            MessageBox.Show(Form_login.form_main.dataGridView5.Columns[1].ToString());
            // 장바구니의 결재버튼을 누르면
            // 현재 장바구니에 들어있는 내용을 텍스트 파일에 전송
            // 관리자에서 결재 메뉴에서 텍스트파일 출력
            // 장바구니 초기화
            Inventory.app_inv();
        }
        private async void button3_6_Click(object sender, EventArgs e)          // 장바구니 마이너스 버튼
        {
            for (int i = 0; i < dataGridView5.Rows.Count; i++)
            {
                if (IsNumeric(textBox3_3.Text) && (Convert.ToBoolean(dataGridView5.Rows[i].Cells[3].Value) == true))
                {
                    int x = int.Parse((string)dataGridView5.Rows[i].Cells[1].Value);
                    if (x >= Int32.Parse(textBox3_3.Text))
                    {
                        dataGridView5.Rows[i].Cells[1].Value = (x - Int32.Parse(textBox3_3.Text)).ToString();
                    }
                    else
                    {
                        textBox3_3.Text = null;
                        label3_4.Text = "잘못된 값입니다.";
                        await Task.Delay(1000);                     // 일정시간후 에러메시지 삭제
                        label3_4.Text = null;
                        break;
                    }
                }
            }
        }

        private async void button3_7_Click(object sender, EventArgs e)          // 장바구니 플러스 버튼
        {
            for (int i = 0; i < dataGridView5.Rows.Count; i++)
            {
                if (IsNumeric(textBox3_3.Text) && (Convert.ToBoolean(dataGridView5.Rows[i].Cells[3].Value) == true))
                {
                    int x = int.Parse((string)dataGridView5.Rows[i].Cells[1].Value);
                    dataGridView5.Rows[i].Cells[1].Value = (x + Int32.Parse(textBox3_3.Text)).ToString();
                }
                else if (!IsNumeric(textBox3_3.Text))
                {
                    textBox3_3.Text = null;
                    label3_4.Text = "잘못된 값입니다.";
                    await Task.Delay(1000);                     // 일정시간후 에러메시지 삭제
                    label3_4.Text = null;
                    break;
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int count = 0;
            // 카테고리 행을 제외하고, 체크박스가 있는 열이고, 체크박스가 false일때
            if (e.RowIndex >= 0 && e.ColumnIndex == 3 && Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[3].Value) == false)
            {
                dataGridView1.Rows[e.RowIndex].Cells[3].Value = true;       // 체크박스를 true
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == 3 && Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[3].Value) == true)
            {
                dataGridView1.Rows[e.RowIndex].Cells[3].Value = false;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[3].Value) == true)        // 선택되어있는 체크박스가 하나라도 있는 경우
                {
                    textBox3_2.Visible = true;
                    textBox3_2.Focus();
                    // textBox3_2.Text = null;
                    count++;
                }
                else if (count == 0)         // 선택되어있는 체크박스가 하나도 없는 경우
                {
                    textBox3_2.Visible = false;
                }
            }
        }
        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int count = 0;
            if (e.RowIndex >= 0 && e.ColumnIndex == 2 && Convert.ToBoolean(dataGridView5.Rows[e.RowIndex].Cells[2].Value) == false)      // 선택을 눌렀을때 삭제버튼이 false이기 때문에 삭제됨
            {
                dataGridView5.Rows.RemoveAt(e.RowIndex);
            }
            if (e.RowIndex >= 0 && e.ColumnIndex == 3 && Convert.ToBoolean(dataGridView5.Rows[e.RowIndex].Cells[3].Value) == false)
            {
                dataGridView5.Rows[e.RowIndex].Cells[3].Value = true;       // 체크박스를 true
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == 3 && Convert.ToBoolean(dataGridView5.Rows[e.RowIndex].Cells[3].Value) == true)
            {
                dataGridView5.Rows[e.RowIndex].Cells[3].Value = false;
            }
            for (int i = 0; i < dataGridView5.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView5.Rows[i].Cells[3].Value) == true)
                {
                    button3_6.Visible = true;
                    button3_7.Visible = true;
                    textBox3_3.Visible = true;
                    textBox3_3.Focus();
                    count++;
                }
                else if (count == 0)         // 선택되어있는 체크박스가 하나도 없는 경우
                {
                    button3_6.Visible = false;
                    button3_7.Visible = false;
                    textBox3_3.Visible = false;
                }
            }
        }

        // ----------------------------------------------------------------------------재고




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
            /*textBox10.Clear();
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
            }*/
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


        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            label3.Text = textBox4.Text;
            char[] inputchars = textBox4.Text.ToCharArray();          //한글만 들어가게
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

        private void panel1_1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel1.ClientRectangle, Color.DarkGray, ButtonBorderStyle.Solid);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel1.ClientRectangle, Color.DarkGray, ButtonBorderStyle.Solid);
        }

        private void panel1_2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel1.ClientRectangle, Color.DarkGray, ButtonBorderStyle.Solid);
        }

        private void panel1_3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel1.ClientRectangle, Color.DarkGray, ButtonBorderStyle.Solid);
        }

        private void label3_Click(object sender, EventArgs e)
        {

            textBox4.Focus();

            textBox4.MaxLength = 7;
            label3.Text = null;
            textBox4.Text = null;
        }

        private void label4_Click(object sender, EventArgs e)
        {

            textBox5.Focus();
            textBox5.MaxLength = 10;
            label4.Text = null;
            textBox5.Text = null;

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            label4.Text = textBox5.Text;

        }

        private void label11_Click(object sender, EventArgs e)
        {
            textBox6.Focus();
            textBox6.MaxLength = 13;
            label11.Text = null;
            textBox6.Text = null;

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            label11.Text = textBox6.Text;

        }

        private void label12_Click(object sender, EventArgs e)
        {
            textBox7.Focus();
            textBox7.MaxLength = 13;
            label12.Text = null;
            textBox7.Text = null;

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            label12.Text = textBox7.Text;

        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            label3.Text = string.Empty;
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            label4.Text = string.Empty;
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            label11.Text = string.Empty;
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            label12.Text = string.Empty;
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button6_Click_1(sender, e);
            }

        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button6_Click_1(sender, e);
            }
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button6_Click_1(sender, e);
            }
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button6_Click_1(sender, e);
            }
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            panel8.Visible = false;
        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel12.ClientRectangle, Color.DarkSlateBlue, ButtonBorderStyle.Solid);

        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel14.ClientRectangle, Color.DarkSlateBlue, ButtonBorderStyle.Solid);

        }



        private void panel13_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel13.ClientRectangle, Color.DarkSlateBlue, ButtonBorderStyle.Solid);

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel13.ClientRectangle, Color.DarkOrange, ButtonBorderStyle.Solid);
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel13.ClientRectangle, Color.DarkOrange, ButtonBorderStyle.Solid);

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel13.ClientRectangle, Color.DarkOrange, ButtonBorderStyle.Solid);

        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            Manage.management();
        }


/*        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            this.main_textBox1.Focus();
        }

        private void richTextBox2_Enter(object sender, EventArgs e)
        {
            this.main_textBox1.Focus();
        }*/

        private void selectedRowsButton_Click(object sender, System.EventArgs e)
        {
            Int32 selectedRowCount =
                dataGridView4.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                for (int i = 0; i < selectedRowCount; i++)
                {
                    sb.Append("Row: ");
                    sb.Append(dataGridView4.SelectedRows[i].Index.ToString());
                    sb.Append(Environment.NewLine);
                }

                sb.Append("Total: " + selectedRowCount.ToString());
                MessageBox.Show(sb.ToString(), "Selected Rows");
            }
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show(Form_login.form_main.dataGridView4.SelectedRows[0].Cells[0].Value.ToString());
 
        }

       /* private void button17_Click(object sender, EventArgs e)
        {
            Daycontainer.Controls.Clear();
            month++;
            if (month > 12)
            {
                month = 1;
                year++;
            }
            DateTime startofthemonth = new DateTime(year, month, 1);// 다음달 첫날의 정보를 가져옴
            int days = DateTime.DaysInMonth(year, month); //다음달 일 수 계산
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")); // 다음 달의 시작 요일을 계산
            for (int i = 0; i < dayoftheweek; i++)  // 다음 달의 시작 요일까지 공백 패널을 추가
            {
                UserControl ucblank = new UserControlBlank(); //공백 패널 생성
                Daycontainer.Controls.Add(ucblank); //공백 패널 추가
            }
            for (int i = 1; i <= days; i++)// 다음 달의 각 날짜에 해당하는 패널을 추가
            {
                UserControldays ucdays = new UserControldays(); //날짜 패널 생성
                ucdays.days(i); //패널에 날짜 설정
                Daycontainer.Controls.Add(ucdays); //날짜 패널 추가
            }
            label18.Text = $"{year}년 {month}월"; // 라벨에 다음 달의 연도와 월을 표시
            foreach (Control control in Daycontainer.Controls) // 패널에 있는 UserControlDays의 클릭 이벤트 핸들러를 등록
            {
                if (control is UserControldays)
                {
                    (control as UserControldays).OnPanelClick += UserControlDays_OnPanelClick; // 해당 패널의 클릭 이벤트 핸들러 등록
                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Daycontainer.Controls.Clear();
            month--;
            if (month < 1)
            {
                month = 12;
                year--;
            }
            DateTime startofthemonth = new DateTime(year, month, 1); //지난달 첫날의 정보를 가져옴
            int days = DateTime.DaysInMonth(year, month);//지난달 일 수 계산
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d"));// 지난 달의 시작 요일을 계산
            for (int i = 0; i < dayoftheweek; i++)// 지난 달의 시작 요일까지 공백 패널을 추가
            {
                UserControl ucblank = new UserControlBlank();//공백 패널 생성
                Daycontainer.Controls.Add(ucblank); //공백 패널 추가
            }
            for (int i = 1; i <= days; i++)// 지난 달의 각 날짜에 해당하는 패널을 추가
            {
                UserControldays ucdays = new UserControldays();//날짜 패널 생성
                ucdays.days(i);//날짜 패널 설정
                Daycontainer.Controls.Add(ucdays);//날짜 패널 추가
            }
            label18.Text = $"{year}년 {month}월";// 라벨에 지난 달의 연도와 월을 표시
            foreach (Control control in Daycontainer.Controls) // 패널에 있는 UserControlDays의 클릭 이벤트 핸들러를 등록
            {
                if (control is UserControldays)
                {
                    (control as UserControldays).OnPanelClick += UserControlDays_OnPanelClick; // 해당 패널의 클릭 이벤트 핸들러 등록
                }
            }
        }*/

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }


        private void button19_Click(object sender, EventArgs e) //출근 버튼
        {
            Form_vacation formVacation = new Form_vacation(form_login, form_login.Name, form_login.UserBirth, form_login.UserRent);


            // 생성된 인스턴스를 보여줌
            formVacation.Show();
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

        string patient = "Patient.txt";

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
class Pay
{

}

class Manage
{
    public static void management()  //직원 메모장 불러오기
    {
        /*  string path = "Info.txt";
          // Info 파일 경로
          List<string> lines = File.ReadAllLines(path).ToList();
          return lines;*/
        try
        {
            string ManageFilePath = "Info.txt"; // 환자 정보 파일 경로
            string[] lines = File.ReadAllLines(ManageFilePath); // 파일의 모든 줄 읽기

            // 데이터그리드뷰 초기화
            Form_login.form_main.dataGridView4.Rows.Clear();


            for (int i = 0; i < lines.Length; i++)
            {
                string[] columns = lines[i].Split('\t'); // 탭으로 구분된 열을 분리
                if (i > 0 && columns[9] != "O")
                {
                    Form_login.form_main.dataGridView4.Rows.Add(columns[0],columns[11]);
                }

            }
        }


        catch
        {

        }
    }


}


/* public static void man_emp()  //직원 불러오기
 {
     List<string> lines = Manage.management();

     for (int i = 1; i < lines.Count; i++)          // 파일 내용을 한 줄씩 읽어가며 처리
     {
         for(int j=0; j < lines.Count; j++)
         {
             List<string> man_label = lines[i].Split('\t').ToList();

             string a = man_label;
             MessageBox.Show(a);
         }

     }

 }*/







class Inventory
{
    private static bool IsNumeric(string input)     // 숫자인지 확인
    {
        foreach (char c in input)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }
        return true;
    }
    public static List<string> text_inv()               // 재고
    {
        string I_path = "Inventory Manager.txt";      // 재고 정보 텍스트 파일
        List<string> lines = File.ReadAllLines(I_path).ToList();       // 파일의 모든 줄을 읽고 리스트화
        return lines;
    }
    public static void line_inv()         // 제품 검색
    {
        List<string> inventory_data = new List<string>(); // 각 줄의 데이터를 저장할 리스트 생성
        List<List<string>> inventory_list = new List<List<string>>(); // 각 줄의 데이터를 저장한 리스트를 저장할 리스트
        List<string> lines = Inventory.text_inv();
        for (int i = 0; i < lines.Count; i++)          // 파일 내용을 한 줄씩 읽어가며 처리
        {
            inventory_data = lines[i].Split('\t').ToList();      // Inventory Manager의 각 줄
            if (i > 0)     // 카테고리 라인 제외
            {
                // Inventory Manager의 i번째 재품의 이름와 textBox3_1의 글자가 같다면
                if (inventory_data[0].Substring(0, Form_login.form_main.textBox3_1.Text.Length) == Form_login.form_main.textBox3_1.Text)
                {
                    // dataGridView1에 행 추가
                    Form_login.form_main.dataGridView1.Rows.Add(inventory_data[0], inventory_data[1], inventory_data[2]);
                }
            }
        }
    }
    public static async void use_inv()         // 사용버튼
    {
        int rowIndex = Form_login.form_main.dataGridView1.CurrentRow.Index;          // 선택된 행
        List<string> inventory_data = new List<string>(); // 각 줄의 데이터를 저장할 리스트 생성
        List<string> lines = Inventory.text_inv();
        if (Form_login.form_main.textBox3_2 != null)
        {
            int x = 0;
            int y = 0;
            for (int i = 0; i < Form_login.form_main.dataGridView1.Rows.Count; i++)     // 체크박스가 선택되어있는 모든 행
            {
                if (IsNumeric(Form_login.form_main.textBox3_2.Text) && (Convert.ToBoolean(Form_login.form_main.dataGridView1.Rows[i].Cells[3].Value) == true))         // 수량입력창에 숫자가 오고 체크박스에 선택이 되어있으면
                {
                    x = Int32.Parse(Form_login.form_main.textBox3_2.Text);                                       // x = 수량 입력
                    y = Int32.Parse(Form_login.form_main.dataGridView1.Rows[i].Cells[2].Value.ToString());       // y = 재고 수량
                    if (y >= x)         // 재고가 입력수량보다 크거나 같을경우
                    {
                        Form_login.form_main.dataGridView1.Rows[i].Cells[2].Value = y - x;       // 재고 - 입력한 값
                    }
                    else                // 입력 수량이 재고보다 큰경우
                    {
                        Form_login.form_main.label3_3.Text = "잘못된 값입니다.";
                        Form_login.form_main.textBox3_2.Focus();        // 수량입력칸으로 포커스
                    }
                }
                else if (!IsNumeric(Form_login.form_main.textBox3_2.Text))       // 수량입력창에 숫자가 아닌 다른 문자가 온경우
                {
                    Form_login.form_main.label3_3.Text = "잘못된 값입니다.";
                    Form_login.form_main.textBox3_2.Focus();        // 수량입력칸으로 포커스
                }
            }

            if (Form_login.form_main.label3_3.Text != "잘못된 값입니다.")
            {
                // Inventory Manager파일에 변화 적용
                int code = 0, name = 0, value = 0;
                for (int i = 0; i < lines.Count; i++)          // 파일 내용을 한 줄씩 읽어가며 처리
                {
                    inventory_data = lines[i].Split('\t').ToList();            // Inventory Manager텍스트 파일의 각 줄을 탭으로 구분
                    if (i == 0)                                                // 카테고리 행 일때
                    {
                        code = inventory_data.IndexOf("코드");                 // 코드의 열번호
                        name = inventory_data.IndexOf("제품명");               // 제품명의 열번호
                        value = inventory_data.IndexOf("재고");                // 재고의 열번호
                    }
                    else
                    {
                        if (Form_login.form_main.dataGridView1.Rows[rowIndex].Cells[code].Value.ToString() == inventory_data[code])    // 선택한 행의 제품코드와 Inventory Manager의 제품코드와 같다면
                        {
                            int result = Int32.Parse(inventory_data[value]);        // 현재 텍스트파일에 있는 재고값
                            result -= x;                                            // 재고값 - 사용값
                            lines[i] = lines[i].Replace(inventory_data[value], result.ToString()); // 재고 열 값 변경
                        }
                    }
                }
            }
            await Task.Delay(1000);                     // 일정시간후 에러메시지 삭제
            Form_login.form_main.label3_3.Text = null; ;
            // 파일에 변경된 값을 적용
            File.WriteAllLines("Inventory Manager.txt", lines);
        }
        else        // 수량입력창이 null일경우
        {
            Form_login.form_main.label3_3.Text = "잘못된 값입니다.";
        }
    }
    public static async void order_inv()            // 주문버튼
    {
        if (Form_login.form_main.textBox3_2 != null)
        {
            for (int i = 0; i < Form_login.form_main.dataGridView1.Rows.Count; i++)         // 데이터그리드뷰1내의 제품수만큼 반복
            {
                if (IsNumeric(Form_login.form_main.textBox3_2.Text) && (Convert.ToBoolean(Form_login.form_main.dataGridView1.Rows[i].Cells[3].Value) == true))        // 수량입력창에 숫자가 입력되고, 체크박스에 선택이 되어있다면
                {
                    if (Form_login.form_main.dataGridView5.Rows.Count == 0)         // 장바구니에 아무것도 없으면
                    {
                        Form_login.form_main.dataGridView5.Rows.Add(Form_login.form_main.dataGridView1.Rows[i].Cells[1].Value, Form_login.form_main.textBox3_2.Text);
                    }
                    else        // 장바구니에 제품이 있으면
                    {
                        int count = 0;     // 중복횟수
                        for (int j = 0; j < Form_login.form_main.dataGridView5.Rows.Count; j++)   // 장바구니의 제품만큼 반복
                        {
                            // 데이터그리드뷰1에 선택된 제품명과 장바구니에 있는 제품명이 같다면
                            if (Form_login.form_main.dataGridView1.Rows[i].Cells[1].Value.ToString() == Form_login.form_main.dataGridView5.Rows[j].Cells[0].Value.ToString())
                            {
                                int x = int.Parse((string)(Form_login.form_main.dataGridView5.Rows[j].Cells[1].Value));     // 장바구니에 추가한 수량
                                Form_login.form_main.dataGridView5.Rows[j].Cells[1].Value = (x + Int32.Parse(Form_login.form_main.textBox3_2.Text)).ToString();
                                count++;
                                break;     // for문을 빠져나감
                            }
                        }
                        if (count == 0)     // 장바구니에 제품이 있고 중복이 없을때
                        {
                            Form_login.form_main.dataGridView5.Rows.Add(Form_login.form_main.dataGridView1.Rows[i].Cells[1].Value, Form_login.form_main.textBox3_2.Text);
                        }
                    }
                }
                else if (!IsNumeric(Form_login.form_main.textBox3_2.Text))       // 수량입력창에 숫자가 아닌 다른 값이 들어온경우
                {
                    Form_login.form_main.label3_3.Text = "잘못된 값입니다.";
                }
            }
        }
        else       // 수량입력창이 null일 경우
        {
            Form_login.form_main.label3_3.Text = "잘못된 값입니다.";
        }
        await Task.Delay(1000);                     // 일정시간후 에러메시지 삭제
        Form_login.form_main.label3_3.Text = null; ;
    }
    // 장바구니에 제품이 있는 경우
    // 1. 중복된 제품이 새로 추가할 제품보다 위에 있는 경우, 주문버튼을 누르면 장바구니에서 중복된 제품의 수량은 올라가지만, 새로추가되야할 제품은 추가되지 않음
    //    - 중복된 제품들 사이에 새로 추가할 제품이 있으면 중복된 제품들만 추가됨
    //       -> count = 0의 위치를 장바구니에 제품이 있을경우 아래로 옮김

    // 2. 새로 추가할 제품이 중복된 제품보다 위에 있는 경우, 주문버튼을 누르면 처음에는 둘 다 들어가지만 두번째부터는 중복된 제품중 위에 있는 것만 추가
    //       -> return를 제거해서 해결

    // 수량입력창을 비우고 버튼을 눌러도 label3_3이 나오지 않고, 주문버튼을 누르면 장바구니에 수량이 빈채로 들어감
    public static void app_inv()
    {
        List<string> lines = new List<string>();
        List<List<string>> app = new List<List<string>>();
        if (Form_login.form_main.dataGridView5.Rows.Count != 0)
        {
            // approval.txt만들어서 장바구니의 0번째 열값 1번째 열값을 제일 윗줄에 \t으로 구분
            // 그다음부터 Rows.Count만큼 0번째 열값, 1번째 열값 \t으로 구분에서 입력,Environment.NewLine
            for (int i = 0; i < Form_login.form_main.dataGridView5.Rows.Count; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < Form_login.form_main.dataGridView5.Columns.Count; j++)
                    {
                        lines[i] = Form_login.form_main.dataGridView5.Columns[j].ToString();
                    }
                    app.Add(lines);
                }

            }
        }
    }
}