using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Windows.Data;

namespace O2.ContactAssignment.ViewModels
{
    class MainViewModel
    {
        private ICollectionView cstView;

        public List<string> AllColleagues { get; set; }
        public List<Customer> AllCustomers { get; set; }
        public string DudeSelected { get; set; }

        public ICollectionView CstView { get { return cstView; } }

        public MainViewModel()
        {
            try
            {
                AllCustomers = JsonConvert.DeserializeObject<List<Customer>>(File.ReadAllText("customers.json"));
            }
            catch
            {
                //file error
                AllCustomers = new List<Customer>();
            }
            cstView = CollectionViewSource.GetDefaultView(AllCustomers);
            AllColleagues = new List<string>();
            AllColleagues.AddRange(File.ReadLines("Kollegen.txt"));
            AllColleagues.Add("-");
            DudeSelected = "-";
        }
    }
}
