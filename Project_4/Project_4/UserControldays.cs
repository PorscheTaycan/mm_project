using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_4
{
    public partial class UserControldays : UserControl
    {
        public string DayLabelText
        {
            get { return lbdays.Text; }
        }
        public static Form_main form_main;
        public UserControldays()
        {
            InitializeComponent();
        }
        private void UserControlDays_Load(object sender, EventArgs e)
        {

        }
        public void days(int numday)
        {
            lbdays.Text = numday + "";
            if (form_main != null)
            {
                form_main.SetDayLabel(lbdays.Text);
            }
        }

        private void lbdays_Click(object sender, EventArgs e)
        {
            OnPanelClick.Invoke(this, EventArgs.Empty);
        }
        public EventHandler OnPanelClick;
    }

}
