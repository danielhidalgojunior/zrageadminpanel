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
using System.Windows.Shapes;
using zRageAdminMain.ViewModels;

namespace zRageAdminMain.Views
{
    /// <summary>
    /// Lógica interna para WinMainView.xaml
    /// </summary>
    public partial class WinMainView : Window
    {
        public WinMainView(LoginViewModel loginvm)
        {
            InitializeComponent();
        }
    }
}
