using System;
using System.Net;
using System.Windows.Forms;
using DotRas;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Dialer
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Defines the name of the entry being used by the example.
        /// </summary>
        public const string EntryName = "VPN Connection";

        public MainForm()
        {
            Program.logWriting("InitializeComponent");
            this.InitializeComponent();

            Program.logWriting("fillListView");
            fillListView();
        }

        public static List<vpn_Entry> getRemoteVpn()
        {
            string Url =  System.Configuration.ConfigurationManager.AppSettings["Url"].ToString();
            string _keys = System.Configuration.ConfigurationManager.AppSettings["Keys"].ToString();
            string data = "Keys="+_keys;
            string result = HttpGetPost.PostDataToUrl(data, Url);
            Program.logWriting(result);
            List<vpn_Entry> r = JsonConvert.DeserializeObject<List<vpn_Entry>>(result);
            return r;
        }
        private void getPhoneEntries() {
            List<vpn_Entry> RemoteVpns = getRemoteVpn();
            AllUsersPhoneBook.Open();
            foreach (vpn_Entry vpn in RemoteVpns)
            {
                if (!this.AllUsersPhoneBook.Entries.Contains(vpn.Name) )
                {
                    if (string.IsNullOrEmpty(vpn.vpnIP)) continue;
                    RasEntry re = RasEntry.CreateVpnEntry(vpn.Name,vpn.vpnIP,
                        RasVpnStrategy.PptpOnly,
                        RasDevice.Create("VPN_PPTP", RasDeviceType.Vpn));
                    re.EncryptionType = RasEncryptionType.Optional;
                    this.AllUsersPhoneBook.Entries.Add(re);
                    re.UpdateCredentials(new NetworkCredential(vpn.vpnID, vpn.vpnPW));
                    re.Update();
                }
                else {
                    RasEntry re = this.AllUsersPhoneBook.Entries[vpn.Name];
                    re.IPAddress = IPAddress.Parse(vpn.vpnIP); 
                    re.PhoneNumber = re.IPAddress.ToString();
                    NetworkCredential nc = re.GetCredentials();
                    nc.UserName = vpn.vpnID;
                    nc.Password = vpn.vpnPW;
                    re.UpdateCredentials(nc);
                    re.Update();
                }
            }
        }
        public void fillListView() {
            gvEntries.Rows.Clear();
            getPhoneEntries();
            DataGridViewRow gvR;
            int rowIdx = -1;
            foreach (RasEntry re in this.AllUsersPhoneBook.Entries)
            {
                Object[] r = new object[] {"",re.Name
                    ,re.PhoneNumber
                    ,""};
                rowIdx = gvEntries.Rows.Add(r);
                gvR = gvEntries.Rows[rowIdx];
                gvR.Tag = re;
            }
        }
        /// <summary>
        /// Occurs when the user clicks the Create Entry button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void CreateEntryButton_Click(object sender, EventArgs e)
        {
            RasEntryDialog red = new RasEntryDialog() ;
            red.PhoneBookPath = this.AllUsersPhoneBook.Path;
            red.EntryName = EntryName;
            DialogResult dr = red.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
                fillListView();          
        }

        /// <summary>
        /// Occurs when the user clicks the Dial button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void DialButton_Click(object sender, EventArgs e)
        {
            this.StatusTextBox.Clear();
            RasEntry entry = (RasEntry)gvEntries.SelectedRows[0].Tag;
            // This button will be used to dial the connection.
            //this.Dialer.EntryName = EntryName;
            this.Dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers);
            this.Dialer.EntryName = entry.Name;
            Program.logWriting(" DialButton_Click "+entry.Name);
            try
            {
                // Set the credentials the dialer should use.
                //this.Dialer.Credentials = new NetworkCredential("Test", "User");
                this.Dialer.Credentials = entry.GetCredentials();

                // NOTE: The entry MUST be in the phone book before the connection can be dialed.
                // Begin dialing the connection; this will raise events from the dialer instance.
                this.Dialer.DialAsync();

                // Enable the disconnect button for use later.
                this.DisconnectButton.Enabled = true;
            }
            catch (Exception ex)
            {
                this.StatusTextBox.AppendText(ex.ToString());
                Program.logWriting(ex.ToString());
            }
        }

        /// <summary>
        /// Occurs when the dialer state changes during a connection attempt.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An <see cref="DotRas.StateChangedEventArgs"/> containing event data.</param>
        private void Dialer_StateChanged(object sender, StateChangedEventArgs e)
        {
            this.StatusTextBox.AppendText(e.State.ToString() + "\r\n");
            Program.logWriting(e.State.ToString());
        }

        /// <summary>
        /// Occurs when the dialer has completed a dialing operation.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An <see cref="DotRas.DialCompletedEventArgs"/> containing event data.</param>
        private void Dialer_DialCompleted(object sender, DialCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.StatusTextBox.AppendText("Cancelled! \r\n");
                DataGridViewCell dgc = gvEntries.SelectedRows[0].Cells[0];
                dgc.Value = "Off";
                dgc = gvEntries.SelectedRows[0].Cells[3];
                dgc.Value = "拨号中止";
                Program.logWriting("Cancelled!");
            }
            else if (e.TimedOut)
            {
                this.StatusTextBox.AppendText("Connection attempt timed out! \r\n");
                DataGridViewCell dgc = gvEntries.SelectedRows[0].Cells[0];
                dgc.Value = "Off";
                dgc = gvEntries.SelectedRows[0].Cells[3];
                dgc.Value = "拨号超时";
                Program.logWriting("Connection attempt timed out! ");
            }
            else if (e.Error != null)
            {
                this.StatusTextBox.AppendText(e.Error.ToString() + " \r\n");
                DataGridViewCell dgc = gvEntries.SelectedRows[0].Cells[0];
                dgc.Value = "Err";
                dgc = gvEntries.SelectedRows[0].Cells[3];
                dgc.Value = "错误";
                Program.logWriting(e.Error.ToString());
            }
            else if (e.Connected)
            {
                this.StatusTextBox.AppendText("Connection successful! \r\n");
                DataGridViewCell dgc = gvEntries.SelectedRows[0].Cells[0];
                dgc.Value = "Ok";
                dgc = gvEntries.SelectedRows[0].Cells[3];
                dgc.Value = "拨号连接成功";
                this.DisconnectButton.Enabled = true;
                this.DialButton.Enabled = false;
                Program.logWriting("Connection successful!");
            }

            if (!e.Connected)
            {
                // The connection was not connected, disable the disconnect button.

                this.StatusTextBox.AppendText("Connection Failured! \r\n");
                DataGridViewCell dgc = gvEntries.SelectedRows[0].Cells[0];
                dgc.Value = "Fail";
                dgc = gvEntries.SelectedRows[0].Cells[3];
                dgc.Value = "拨号连接失败";
                this.DialButton.Enabled = true;
                this.DisconnectButton.Enabled = false;
                Program.logWriting("Connection Failured!");
            }
        }

        /// <summary>
        /// Occurs when the user clicks the Disconnect button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            Program.logWriting("DisconnectButton_Click");
            if (this.Dialer.IsBusy)
            {
                // The connection attempt has not been completed, cancel the attempt.
                this.Dialer.DialAsyncCancel();
            }
            else
            {
                // The connection attempt has completed, attempt to find the connection in the active connections.

                string entryName = gvEntries.SelectedRows[0].Cells[1].Value.ToString();
                RasConnection connection = getRasConn(entryName);
                
                //.GetActiveConnectionByHandle(this.handle);
                if (connection != null )
                {
                    // The connection has been found, disconnect it.
                    connection.HangUp(true);

                    //断开后设置提示
                    this.StatusTextBox.AppendText("Disconnection! \r\n");
                    DataGridViewCell dgc = gvEntries.SelectedRows[0].Cells[0];
                    dgc.Value = "Off";
                    dgc = gvEntries.SelectedRows[0].Cells[3];
                    dgc.Value = "拨号断开连接";
                    this.DialButton.Enabled = true;
                    this.DisconnectButton.Enabled = false;
                }
            }
        }
        private RasConnection getRasConn(string RasEntryName)
        {
            RasConnection r = null;
            var Conns = RasConnection.GetActiveConnections();
            foreach (RasConnection c in Conns)
            {
                if (c.EntryName == RasEntryName)
                { r = c; break; }
            }
            return r;
        }

        private void gvEntries_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            string entryName = gvEntries.Rows[e.RowIndex].Cells[1].Value.ToString();
            RasConnection c = getRasConn(entryName);
            if (c != null)
            {
                DialButton.Enabled = false;
                DisconnectButton.Enabled = true;
            }
            else
            {
                DialButton.Enabled = true;
                DisconnectButton.Enabled = false;
            }
        }

        private void buttonFlushGV_Click(object sender, EventArgs e)
        {
            fillListView();
        }

        private void RemoveEntryButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow gvR = gvEntries.SelectedRows[0];
            RasEntry r = (RasEntry)gvR.Tag;
            this.AllUsersPhoneBook.Entries.Remove(r);
            gvEntries.Rows.Remove(gvR);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.logWriting("MainForm_FormClosed");
        }
    }
}
