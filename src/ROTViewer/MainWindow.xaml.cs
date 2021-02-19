using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ROTViewer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VueModele vm;
        public MainWindow()
        {
            InitializeComponent();
             vm= new VueModele();
            this.DataContext=vm;
        }


        private void refreshTable_Click(object sender, RoutedEventArgs e)
        {
            vm.RefreshTable();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vm.RefreshTable();
        }
    }
}
