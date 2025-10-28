using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.config
{
    public class Session
    {
        public Login LoginForm { get; set; }
        public Session(Login login) { 
            LoginForm = login;
        }
        public string CempionatName { get; set; }
        public string UserFIO { get; set; }
        public string SkillName { get; set; }
        public int CempoinatId { get; set; }
    }
}
