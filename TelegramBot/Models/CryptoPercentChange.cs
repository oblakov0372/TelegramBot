using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    public class CryptoPercentChange
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal PercentChange24H { get; set; }
        public decimal VolumeChange24H { get; set; }
    }
}
