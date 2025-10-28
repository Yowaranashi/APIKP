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
    public partial class VozrastnCenzForm : Form
    {
        string skillGlob = "";
        string champGlob = "";
        public VozrastnCenzForm(string skill, string champ)
        {
            InitializeComponent();
            labelTitleSkill.Text = skill;
            labelTitleChamp.Text = champ;
            skillGlob = skill;
            champGlob = champ;
        }

        private void butBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
