using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ArbiBot
{
    public partial class Form1 : Form
    {
        private RegistrationBot registrationBot;
        private ArbitrageBot arbitrageBot;
        private PumpAndDumpBot pumpAndDumpBot;

        public Action<decimal, decimal> setRanges;
        public Action<string> showPND;

        private decimal spreadRange1 = 0;
        private decimal spreadRange2 = 10;

        private string[] bybitPares;
        public Form1()
        {
            InitializeComponent();
            setRanges = (decimal range1, decimal range2) =>
            {
                spreadRange1 = range1;
                spreadRange2 = range2;
            };
            showPND = (string msg) => { label8.Text = msg; };
            registrationBot = new RegistrationBot();
            pumpAndDumpBot = new PumpAndDumpBot(showPND);
            arbitrageBot = new ArbitrageBot();
        }
        private void ClearTable(object sender, EventArgs e)
        {
            using (var db = new Context())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                db.Database.ExecuteSqlRaw("insert into Types (TypeOfSubscribe) values ('Arbi'),('PumpDump'),('Both')");
            }
        }
        private async void StartPumpAndDump(object sender, EventArgs e)
        {
            label6.Text = "Запущен";
            pumpAndDumpBot.StopBot();
            try
            {
                await Task.Run(() => pumpAndDumpBot.StartBot());
            }
            catch (Exception ex) { label6.Text = ex.Message; }
        }
        private void StopPumpAndDumpBot(object sender, EventArgs e)
        {
            label6.Text = "Отключен";
            pumpAndDumpBot.StopBot();
        }
        private async void OnFormLoadEvent(object sender, EventArgs e)
        {
            label5.Text = "Запущен";
            label6.Text = "Запущен";
            await Task.Run(() => pumpAndDumpBot.StartBot());
            if (spreadRange2 != 0)
                arbitrageBot = new ArbitrageBot(spreadRange1, spreadRange2);
            else
                arbitrageBot = new ArbitrageBot();
            await Task.Run(() => arbitrageBot.StartBot());
        }
        private async void StartArbitrageEvent(object sender, EventArgs e)
        {
            label5.Text = "Запущен";
            arbitrageBot.StopBot();
            if (spreadRange2 != 0) arbitrageBot = new ArbitrageBot(spreadRange1, spreadRange2);
            await Task.Run(() => arbitrageBot.StartBot());
        }
        private void StopArbitrageEvent(object sender, EventArgs e)
        {
            label5.Text = "Отключен";
            arbitrageBot.StopBot();
        }
        private void OpenOptionsEvent(object sender, EventArgs e)
        {
            OptionsForm form = new OptionsForm(setRanges);
            form.ShowDialog();
        }
        private void SelectAllRegisteredUsersEvent(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            using (Context db = new Context())
            {
                foreach (var user in db.Users)
                {
                    listBox1.Items.Add($"id = {user.Id}. tgId = {user.TelegramId} subId = {user.SubTypeId} subEndDate - {user.SubscriptionEnd.ToShortDateString()}");
                }
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
