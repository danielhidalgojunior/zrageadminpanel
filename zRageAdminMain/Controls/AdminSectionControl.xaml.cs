using MongoDBHelper.Models;
using StaticResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace zRageAdminMain.Controls
{
    /// <summary>
    /// Interação lógica para AdminSectionControl.xam
    /// </summary>
    public partial class AdminSectionControl : UserControl
    {
        public AdminSectionControl()
        {
            var user = Variables.LoggedUser as UserModel;

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()) ||
                user.HighestRank >= 9 ||
                user.GodMode)
            {
                InitializeComponent();
            }
        }
    }
}
