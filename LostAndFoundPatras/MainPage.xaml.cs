using LostAndFoundPatras.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static LostAndFoundPatras.HeaderContentView;

namespace LostAndFoundPatras
{
    public partial class MainPage : Shell
    {
        public bool IsLogedIn { get; set; }
        public MainPage()
        {
            InitializeComponent();
        }
    }
}
