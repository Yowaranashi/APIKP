using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1.Controller.organizator
{
    public partial class ChempionatForm : Form
    {
        public ChempionatForm()
        {
            InitializeComponent();

            string wellcomeText;

            if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 11)
            {
                wellcomeText = "Доброе утро, ";
            }
            else if (DateTime.Now.Hour >= 11 && DateTime.Now.Hour <= 17)
            {
                wellcomeText = "Добрый день, ";
            }
            else
            {
                wellcomeText = "Доброй ночи, ";
            }
            wellcomeText += Login.Session.UserFIO; //фио организатора
            wellcome.Text = wellcomeText;

            label3.Text = Login.Session.CempionatName;
        }

        private void chempionat_Click(object sender, EventArgs e)
        {
            UchastnikListForm form = new UchastnikListForm();
            form.ShowDialog();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chempionatOptions_Click(object sender, EventArgs e)
        {
            OptionsForm form = new OptionsForm();
            form.ShowDialog();
        }
    }
}
