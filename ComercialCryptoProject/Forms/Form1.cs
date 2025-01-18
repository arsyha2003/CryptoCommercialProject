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
        public Action<string> showSpread;
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
            registrationBot = new RegistrationBot();
            pumpAndDumpBot = new PumpAndDumpBot();
            arbitrageBot = new ArbitrageBot();
        }
        private void ClearTable(object sender, EventArgs e)
        {
            using(var db = new UsersContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                db.Database.ExecuteSqlRaw("insert into Types (TypeOfSubscribe) values ('Arbi'),('PumpDump'),('Both')");
            }
        }
        private async void StartPumpAndDump(object sender, EventArgs e)
        {
            pumpAndDumpBot.StopBot();
            await Task.Run(()=>pumpAndDumpBot.StartBot());
        }
        private void StopPumpAndDumpBot (object sender, EventArgs e)
        {
            pumpAndDumpBot.StopBot();
        }
        private async void OnFormLoadEvent(object sender, EventArgs e)
        {
            await Task.Run(() => pumpAndDumpBot.StartBot());
            if (spreadRange2 != 0) 
                arbitrageBot = new ArbitrageBot(spreadRange1, spreadRange2);
            else
                arbitrageBot = new ArbitrageBot();
            await Task.Run(() => arbitrageBot.StartBot());
        }
        private async void StartArbitrageEvent(object sender, EventArgs e)
        {
            arbitrageBot.StopBot();
            if (spreadRange2!=0) arbitrageBot = new ArbitrageBot(spreadRange1, spreadRange2);
            await Task.Run(()=>arbitrageBot.StartBot());
        }
        private void StopArbitrageEvent(object sender, EventArgs e)
        {
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
            using (UsersContext db = new UsersContext())
            {
                foreach (var user in db.Users)
                {
                    listBox1.Items.Add($"id = {user.Id}. tgId = {user.TelegramId} subId = {user.SubTypeId} subEndDate - {user.SubscriptionEnd.ToShortDateString()}");
                }
            }
        }
    }
}
