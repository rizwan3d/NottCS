﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NottCS.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NottCS.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistrationPage : ContentPage
	{
	    private RegistrationViewModel viewModel;
        public RegistrationPage ()
		{
		    BindingContext = viewModel = new RegistrationViewModel();

            InitializeComponent();
		}
	}
}