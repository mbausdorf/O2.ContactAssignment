using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2.ContactAssignment
{
    internal class Customer
    {
        [DisplayName("VVL")]
        public DateTime VVL { get; set; }

        [DisplayName("Läuft in Monat")]
        public int RunningInMonth { get; set; }
        public string Shop { get; set; }
        public string VO { get; set; }
        public string CustomerRate { get; set; }
        public string ContactNo { get; set; }
        public string TicketNo { get; set; }
        public string Tariff { get; set; }
        public string Status { get; set; }
        public int ContactCount { get; set; }
        public string AssignedTo { get; set; }
        public bool Called { get; set; }
    }

    internal sealed class CustomerMap : CsvClassMap<Customer>
    {
        public CustomerMap()
        {
            Map(m => m.RunningInMonth).Name("Läuft in Monat");
        }
    }
}
