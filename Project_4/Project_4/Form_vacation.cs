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
namespace Project_4
{
    public partial class Form_vacation : Form
    {
        private Timer timer;
        private Form_login form_login;

        string name;
        string birth;
        string Rent;
        public Form_vacation(Form_login form_login, string name, string birth, string Rent)
        {
            InitializeComponent();
            this.form_login = form_login;
            this.name = form_login.UserName;
            this.birth = birth;
            this.Rent = Rent;
            LoadDisplay(); // 월차/반차 신청을 새로고침 
            Vac_count(); //월차/반차 남은 갯수를 새로고침
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            if (monthCalendar1.SelectionRange.Start == monthCalendar1.SelectionRange.End) //달력에서 선택된 날을 텍스트박스에 삽입
                textBox1.Text = monthCalendar1.SelectionRange.Start.ToString("yyyy-MM-dd");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.SelectedItem.ToString(); //콤보박스에서 선택한것

            // 만약 특정 항목을 선택한 경우에만 두 번째 콤보 박스를 보이게 합니다.
            if (selectedItem == "반차")
            {
                comboBox2.Visible = true;
            }
            else
            {
                comboBox2.Visible = false;
                comboBox2.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string comboText = comboBox1.SelectedItem.ToString();
            string selectedDate = textBox1.Text;
            string filePath = "vac_req.txt";
            string fileVac = "Info.txt";
            string comboText2 = " ";
            DateTime selecteDate = monthCalendar1.SelectionStart;//달력에서 선택한 날을 삽입
            if (selecteDate >= DateTime.Now) //선택한 날짜가 오늘날보다 크면
            {
                if (comboText == "월차")
                {


                    if (int.Parse(Rent) > 0)
                    {
                        // 파일에 쓸 내용
                        string vac_reqText = $"{selectedDate}\t{comboText}\t{name}\t{birth}\t{comboText2}";
                        try
                        {
                            // 파일에 내용 추가
                            File.AppendAllText(filePath, vac_reqText + Environment.NewLine);

                            int rentValue = int.Parse(Rent);
                            rentValue -= 2; // rent 값을 2 감소
                            Rent = rentValue.ToString(); // 감소된 rent 값을 문자열로 변환하여 저장

                            // rent 값이 변경되었으므로 다시 화면에 표시
                            Vac_count();
                            List<string> lines = File.ReadAllLines(fileVac).ToList();
                            if (lines.Count > 0)
                            {
                                for (int i = 0; i < lines.Count; i++)
                                {
                                    string[] columns = lines[i].Split('\t');
                                    if (columns[0] == name) // 이름이 일치하는 경우 monthrent 값을 변경
                                    {
                                        columns[10] = Rent; // monthrent 값을 변경
                                        lines[i] = string.Join("\t", columns); // 변경된 값을 다시 합쳐서 해당 줄로 설정
                                        File.WriteAllLines(fileVac, lines); // 변경된 내용을 파일에 씀
                                        break; // 이름이 일치하는 행을 찾았으므로 루프를 중지합니다.
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                        // 파일에 수정된 내용을 씀

                        // 작업 완료 메시지 출력
                        MessageBox.Show("신청 되었습니다.");

                        LoadDisplay(); // 새로고침 메서드 호출

                    }
                    if (int.Parse(Rent) <= 0)
                    {
                        MessageBox.Show("사용할수있는 갯수가 모자랍니다");
                    }
                }

            }

            if (selecteDate >= DateTime.Now) //선택한 날짜가 오늘날보다 크면
            {
                if (comboText == "반차")
                {
                    comboText2 = comboBox2.SelectedItem.ToString();

                    if (int.Parse(Rent) > 0)
                    {
                        string vac_reqText = $"{selectedDate}\t{comboText}\t{name}\t{birth}\t{comboText2}"; // 파일에 쓸 내용
                        try
                        {
                            File.AppendAllText(filePath, vac_reqText + Environment.NewLine); // 파일에 내용 추가

                            double rentValue = double.Parse(Rent);
                            rentValue--; // rent 값을 1 감소
                            Rent = rentValue.ToString(); // 감소된 rent 값을 문자열로 변환하여 저장
                            Vac_count();// rent 값이 변경되었으므로 다시 화면에 표시
                            List<string> lines = File.ReadAllLines(fileVac).ToList();//fileVac을 읽어옴
                            if (lines.Count > 0) //휴가의 갯수가 0보다 크면
                            {
                                for (int i = 0; i < lines.Count; i++)
                                {
                                    string[] columns = lines[i].Split('\t');
                                    if (columns[0] == name) // 이름이 일치하는 경우 
                                    {
                                        columns[10] = Rent; // monthrent 값을 변경
                                        lines[i] = string.Join("\t", columns); // 변경된 값을 다시 합쳐서 해당 줄로 설정
                                        File.WriteAllLines(fileVac, lines); // 변경된 내용을 파일에 씀
                                        break; // 루프 중단
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                        MessageBox.Show("신청 되었습니다.");// 작업 완료 메시지 출력

                        LoadDisplay(); // 새로고침 메서드 호출
                    }
                }
                if (int.Parse(Rent) <= 0)
                {
                    MessageBox.Show("사용할수있는 갯수가 모자랍니다");
                }

            }
            else
            {
                MessageBox.Show("날짜가 지났습니다");
            }
        }
        private void LoadDisplay()
        {
            try
            {
                List<List<string>> vac_req_List = vacation.Line(); // 텍스트 파일의 내용을 읽어옴
                dataGridView1.Rows.Clear(); // DataGridView 초기화
                foreach (List<string> vac_req_Info in vac_req_List) // 읽어온 내용을 DataGridView에 표시
                {
                    int rowIndex = dataGridView1.Rows.Add(); // DataGridView에 행 추가

                    for (int i = 0; i < vac_req_Info.Count; i++) // 각 항목을 탭으로 나누어 DataGridView의 각 셀에 추가
                    {
                        dataGridView1.Rows[rowIndex].Cells[i].Value = vac_req_Info[i];
                    }
                }
            }
            catch { }

        }
        public void Vac_count()
        {
            dataGridView2.Rows.Clear(); // DataGridView 초기화
            int rent;
            rent = int.Parse(Rent);
            try
            {
                int rowIndex = dataGridView2.Rows.Add(); //행을 추가
                dataGridView2.Rows[rowIndex].Cells[0].Value = rent; //새로운 행에 값을 대체함

            }
            catch { }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
    class vacation
    {

        public static List<string> text_vac()
        {
            string P_path = "vac_req.txt";      // 환자 정보 텍스트 파일
            string[] lines = File.ReadAllLines(P_path);       // 파일의 모든 줄을 읽어옴
            List<string> vacList = lines.ToList();
            return vacList;
        }

        public static List<List<string>> Line()
        {
            List<List<string>> lines = new List<List<string>>();

            // 파일의 각 줄을 읽어와 리스트 형태로 변환
            foreach (string line in text_vac())
            {
                List<string> data = line.Split('\t').ToList(); // 각 줄을 탭으로 구분하여 리스트로 변환
                lines.Add(data);
            }

            return lines;
        }

    }
}

