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

        private ILoginManeger _loginManeger;
        private IMessageManeger _messageManeger;

        public InstaApiInstance(ILoginManeger loginManeger, IMessageManeger messageManeger)
        {
            _loginManeger = loginManeger;
            _messageManeger = messageManeger;
            InitializeComponent();
        }


        private EInstaUser activeAccount;
        private string _activeMessageUser;
        public string activeMessageUser
        {
            get { return _activeMessageUser; }
            set
            {
                _activeMessageUser = value;
                lbl_SendingMessage.Text = _activeMessageUser;
            }
        }

        private async void InstaApiInstance_Load(object sender, EventArgs e)
        {
            var _loginedUsers = await _loginManeger.LoginedUsers();

            //UI
            SetView.DgvAddRowsAccount<string>(ref dgvInstaAccounts, _loginedUsers.Select(x => x.UserName).ToList<string>(),
                new List<string>(new string[] { "Accounts" }));
        }

      

        private async void btnSingIn_Click(object sender, EventArgs e)
        {
            await _loginManeger.LoginInsta(txtUserName.Text, txtPassword.Text);
            var _loginedUsers = await _loginManeger.LoginedUsers();

            //UI
            SetView.DgvAddRowsAccount<string>(ref dgvInstaAccounts, _loginedUsers.Select(x => x.UserName).ToList<string>(), 
                new List<string>(new string[]{"Accounts"}));
        }




        private async void dgvInstaAccounts_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                activeAccount = EInstaUser.instaUsers.SingleOrDefault(x =>
                    x.UserName == dgvInstaAccounts.Rows[e.RowIndex].Cells[0].Value.ToString());



                var messageBoxs = await _messageManeger.GetMessageBox(activeAccount._instaApi);

                //UI

                SetView.DgvAddRowsMessageBox(ref dgvInstaMessageUser, ref dgvPendingMessage, messageBoxs,
                    new List<string>(new string[] {"Message Users"}));
            }
        }


        private async void dgvInstaMessageUser_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >=0)
            {
                activeMessageUser = dgvInstaMessageUser.Rows[e.RowIndex].Cells[0].Value.ToString();



                var messages =
                    await _messageManeger.GetMessageByUserName(activeAccount._instaApi, activeMessageUser, false);


                //UI

                SetView.DgvMessageGetMessage(ref dgvInstaMessages, messages, "Messages");
            }
            
        }

        private async void dgvPendingMessage_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                activeMessageUser = dgvPendingMessage.Rows[e.RowIndex].Cells[0].Value.ToString();

                var messages =
                    await _messageManeger.GetMessageByUserName(activeAccount._instaApi, activeMessageUser, true);

                //UI

                SetView.DgvMessageGetMessage(ref dgvPendingMessage, messages, "Messages");
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            await _messageManeger.SendMessageByUsername(activeAccount._instaApi, activeMessageUser,
                txtMessageText.Text);


            //UI

            txtMessageText.Text = "";

            var messages = await _messageManeger.GetMessageByUserName(activeAccount._instaApi, activeMessageUser, false);

            SetView.DgvMessageGetMessage(ref dgvInstaMessages, messages, "Messages");

           
        }
    }


}
