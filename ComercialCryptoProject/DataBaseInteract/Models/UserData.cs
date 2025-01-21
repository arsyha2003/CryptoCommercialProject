using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoPtoject.DataBaseInteract.Models
{
    public class UserData
    {
        public int Id { get; set; }
        public long TelegramId { get; set; }
        public DateTime SubscriptionEnd { get; set; }
        public int SubTypeId { get; set; }
        public TypesOfSubscribe SubType { get; set; }
    }

}
