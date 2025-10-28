using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldSkillsRussia
{
    public partial class FormSettingChamp : Form
    {
        DataBase _dataBase = new DataBase();
        int _idChamp = 0;
        public FormSettingChamp(int idChamp)
        {
            InitializeComponent();
            _idChamp = idChamp;
        }

        private void butListProtocol_Click(object sender, EventArgs e)
        {

        }
    }
}
