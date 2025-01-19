using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ArbiBot
{
    public class BybitPareInfo
    {
        public int Id { get; set; }
        public string Pare { get; set; }
        public decimal AvgPrice {  get; set; }
        public DateTime Time { get; set; }
    }
} 
