using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstaApi.Business.Abstract;
using InstaApi.Business.Concrate;
using InstaApi.Entities.Concrate;

namespace InstaApiInstance
{
    public partial class InstaApiInstance : Form
    {

        ILoginManeger _loginManeger;
        public InstaApiInstance(ILoginManeger loginManeger)
        {
            _loginManeger = loginManeger;
            InitializeComponent();
        }

        private async void btnSingIn_Click(object sender, EventArgs e)
        {
            await _loginManeger.LoginInsta(txtUserName.Text, txtPassword.Text);
            var _loginedUsers = await _loginManeger.LoginedUsers();
            SetView<string>.DgvAddRows(ref dgvInstaAccounts, _loginedUsers.Select(x => x.UserName).ToList<string>(), 
                new List<string>(new string[]{"User Name"}));
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
