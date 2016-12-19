using O2.ContactAssignment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Win32;
using CsvHelper;

namespace O2.ContactAssignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel vm;

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            vm = new MainViewModel();
            InitializeComponent();
            this.DataContext = vm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportListBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                string text = Clipboard.GetText();
                string[] split = text.Split('\n');
                int lineNo = 1;
                Customer c = new Customer();
                foreach (string line in split)
                {
                    string[] split2 = line.Split('\t');
                    if(lineNo == 1)
                    {
                        c.VVL = DateTime.Parse(split2[0]);
                        c.RunningInMonth = int.Parse(split2[1]);
                        lineNo = 2;
                    }
                    else
                    {
                        c.Shop = split2[0];
                        c.VO = split2[1];
                        c.CustomerRate = split2[2];
                        c.ContactNo = split2[3];
                        c.TicketNo = split2[4];
                        c.Tariff = split2[5];
                        c.Status = split2[6];
                        c.ContactCount = int.Parse(split2[9]);
                        vm.AllCustomers.Add(c);
                        lineNo = 1;
                        c = new Customer();
                    }
                }
                CustomerList.ItemsSource = null;
                CustomerList.ItemsSource = vm.AllCustomers;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Closed(object sender, EventArgs e)
        {
            File.WriteAllText("customers.json",JsonConvert.SerializeObject(vm.AllCustomers));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetHakenBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Customer c in vm.AllCustomers.Where(ac => ac.AssignedTo == ColleagueBox.SelectedValue.ToString()))
            {
                c.Called = false;
            }
            CustomerList.ItemsSource = null;
            CustomerList.ItemsSource = vm.AllCustomers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColleagueBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ColleagueBox.SelectedValue.ToString() != "-")
            {
                ResetHakenBtn.IsEnabled = true;
                vm.CstView.Filter = new Predicate<object>(AssignedTo);
            }
            else
            {
                ResetHakenBtn.IsEnabled = false;
                vm.CstView.Filter = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public bool AssignedTo(object de)
        {
            Customer cst = de as Customer;
            return (cst.AssignedTo == vm.DudeSelected);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintListButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.DefaultExt = ".csv";
            openFileDialog.CheckFileExists = false;
            if (openFileDialog.ShowDialog() == true)
            {
                using(TextWriter textWriter = File.CreateText(openFileDialog.FileName))
                {
                    CsvWriter csv = new CsvWriter(textWriter);
                    csv.Configuration.Delimiter = ";";
                    if (vm.DudeSelected != "-")
                    {
                        csv.WriteRecords(vm.AllCustomers.Where(c => c.AssignedTo == vm.DudeSelected));
                    }
                    else
                    {
                        csv.WriteRecords(vm.AllCustomers);
                    }
                }
            }
                
        }
    }
}
