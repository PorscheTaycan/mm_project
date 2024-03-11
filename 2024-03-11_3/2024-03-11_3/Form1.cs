using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2024_03_11_3
{
    public partial class Form1 : Form
    {
        private Dictionary<string, Button> buttonDict = new Dictionary<string, Button>();  //딕셔너리 함수 선언
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string tag = button.Tag.ToString();
            string basename = button.Name.Replace("button_", "");
            update_top(basename, tag);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string tag = button.Tag.ToString();
            string basename = button.Name.Replace("button_", "");
            update_top(basename, tag);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string tag = button.Tag.ToString();
            string basename = button.Name.Replace("button_", "");
            update_top(basename, tag);

        }
        private void update_top(object baseName, string buttonText)
        {
            string buttonName = "button_" + baseName; //버튼 뒤에 이름을 basename
            string buttonXName = buttonName + "_X"; //x버튼 이름 정하기
            string panelName = "panel_" + baseName; //내가 뜨게 할 패널 이름 basename으로 똑같이 정해놓기

            if (buttonDict.ContainsKey(buttonName))  //중복 생성 방지
            {
                Control[] targetPanels = this.Controls.Find(panelName, true);
                if (targetPanels.Length > 0)
                {
                    Panel targetPanel = targetPanels[0] as Panel;
                    if (targetPanels != null)
                    {
                        targetPanel.Visible = true;
                        targetPanel.BringToFront();
                    }
                }
                return;
            }
            Button lastButton = buttonDict.Values.LastOrDefault();
            int nextXPosition = lastButton != null ? lastButton.Right + 30 : 10; //마지막 버튼

            Button newButton = new Button //여기서 딕셔너리에 넣음
            {
                Name = buttonName,
                Text = buttonText,
                Location = new Point(nextXPosition, 5),
                AutoSize = true
            };
            panel_TOP.Controls.Add(newButton);
            buttonDict[buttonName] = newButton;

            newButton.Click += (sender, e) =>
            {
                Control[] targetPanels = this.Controls.Find(panelName, true);
                if (targetPanels.Length > 0)
                {
                    Panel targetPanel = targetPanels[0] as Panel;
                    if (targetPanels != null)
                    {
                        targetPanel.Visible = true;
                        targetPanel.BringToFront();
                    }
                }
            };
            Button newXButton = new Button
            {
                Name = buttonName,
                Text = "X",
                Location = new Point(newButton.Right, 5),
                AutoSize = true
            };
            Control[] panels = this.Controls.Find(panelName, true);

            if (panels.Length > 0)
            {
                Panel panel = panels[0] as Panel;
                if (panel != null)
                {
                    panel.Visible = true;
                    panel.BringToFront();
                }
            }
            newXButton.Click += (sender, e) =>
            {
                panel_TOP.Controls.Remove(newButton);
                panel_TOP.Controls.Remove(newXButton);
                buttonDict.Remove(buttonName);

                Control[] relatedpanels = this.Controls.Find(panelName, true);

                if (relatedpanels.Length > 0)
                {
                    Panel panel = panels[0] as Panel;
                    if (panel != null)
                    {
                        panel.Visible = true;
                    }
                }
            };
            panel_TOP.Controls.Add(newXButton);
        }


    }
}
