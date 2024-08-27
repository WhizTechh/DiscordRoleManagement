//By WhizTech

using System;
using System.Windows.Forms;
using Discord;
using Discord.WebSocket;

namespace DiscordGivingRemovingRoles
{
    public partial class MainWindow : Form
    {
        readonly string bottoken = "";

        bool botsonly = false, allincluded = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            //Start bot client
            await client.LoginAsync(TokenType.Bot, bottoken);
            await client.StartAsync();
        }

        public void ShowMembersOnlyLbl() {
            membersonlyLbl.Visible = true;
        }

        public void HideMembersOnlyLbl()
        {
            membersonlyLbl.Visible = false;
        }

        private void botsOnlyChkBox_CheckedChanged(object sender, EventArgs e)
        {
            allChkBox.Checked = false;

            if (botsOnlyChkBox.Checked)
            {
                botsonly = true;
                HideMembersOnlyLbl();
            }
            else if (!botsOnlyChkBox.Checked) {
                botsonly = false;
                ShowMembersOnlyLbl();
            }
        }

        private void allChkBox_CheckedChanged(object sender, EventArgs e)
        {
            botsOnlyChkBox.Checked = false;

            if (allChkBox.Checked)
            {
                allincluded = true;
                HideMembersOnlyLbl();
            }
            else if (!allChkBox.Checked)
            {
                allincluded = false;
                ShowMembersOnlyLbl();
            }
        }

        private async void AddRoleForEveryone() {

            string serverId = guildIdTextBox.Text;
            string roleId = roleIdTextBox.Text;

            try
            {
                if (ulong.TryParse(serverId, out ulong serverIdparsed) && ulong.TryParse(roleId, out ulong roleIdparsed))
                {
                    var server = client.GetGuild(serverIdparsed);

                    if (server!=null) {
                        var users = await server.GetUsersAsync().FlattenAsync();
                        var role = server.GetRole(roleIdparsed);

                        if (role != null)
                        {
                            foreach (var user in users)
                            {
                                if (user != null && user.IsBot == botsonly)
                                {
                                    await user.AddRoleAsync(role);
                                }

                                else if (allincluded)
                                {
                                    await user.AddRoleAsync(role);
                                }
                            }

                            MessageBox.Show("Role added to all users successfully!", "Success");
                        }
                        else
                        {
                            MessageBox.Show("Role not found in the server.", "Error");
                        }
                    }
                }

                else
                {
                    MessageBox.Show("Server not found.", "Error");
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private async void RemoveRoleForEveryone()
        {

            string serverId = guildIdTextBox.Text;
            string roleId = roleIdTextBox.Text;

            try
            {
                if (ulong.TryParse(serverId, out ulong serverIdparsed) && ulong.TryParse(roleId, out ulong roleIdparsed))
                {
                    var server = client.GetGuild(serverIdparsed);

                    if (server != null)
                    {
                        var users = await server.GetUsersAsync().FlattenAsync();
                        var role = server.GetRole(roleIdparsed);

                        if (role != null)
                        {
                            foreach (var user in users)
                            {
                                if (user != null && user.IsBot == botsonly)
                                {
                                    await user.RemoveRoleAsync(role);
                                }

                                else if (allincluded)
                                {
                                    await user.RemoveRoleAsync(role);
                                }
                            }
                            MessageBox.Show("Role removed from all users successfully!", "Success");
                        }
                        else
                        {
                            MessageBox.Show("Role not found in the server.", "Error");
                        }
                    }
                }

                else
                {
                    MessageBox.Show("Server not found.", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            await client.StopAsync();
        }

        private readonly DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info
        });

        private void addRoleBtn_Click(object sender, EventArgs e)
        {
            AddRoleForEveryone();
        }

        private void removeRoleBtn_Click(object sender, EventArgs e)
        {
            RemoveRoleForEveryone();
        }

    }
}
