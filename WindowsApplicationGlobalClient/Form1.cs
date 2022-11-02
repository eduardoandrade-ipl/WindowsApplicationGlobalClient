using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsApplicationGlobalClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HolidayService.HolidaySoapClient service = new HolidayService.HolidaySoapClient();

            int year = (int)numericUpDown1.Value;
            HolidayService.Holiday[] feriados = service.GetAllHolidays(year);

            foreach (HolidayService.Holiday item in feriados)
            {
                string valor = item.Date.ToShortDateString() + " -> " + item.Name;
                listBox1.Items.Add(valor);
            }
        }
    }
}
