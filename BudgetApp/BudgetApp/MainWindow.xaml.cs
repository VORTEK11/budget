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


namespace BudgetApp
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.PreviousDate();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.NextDate();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddRecord();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.UpdateRecord();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DeleteRecord();
        }
    }
}

