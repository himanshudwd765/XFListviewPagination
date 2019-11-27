using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XFListviewPagination.ViewModels;

namespace XFListviewPagination.Views
{
    public partial class EmpListView : ContentPage
    {
        public EmpListView()
        {
            InitializeComponent();
            this.BindingContext = new EmpListViewModel();
        }
    }
}
