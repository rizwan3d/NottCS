﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using NottCS.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using NottCS.Services;
using NottCS.Services.Navigation;
using NottCS.Views;

namespace NottCS.ViewModels
{
    class CreateEventConfirmationViewModel:BaseViewModel
    {
        public ICommand Confirmation=>new Command(DoNothingForNow);
        public void DoNothingForNow()
        {

        }

        private ObservableCollection<Item> _list;
        public ObservableCollection<Item> List
        {
            get => _list;
            set => SetProperty(ref _list, value);
        }

        public int ButtonHeight { get; set; }
        #region CreateEventConfirmationViewModel Constructor
        public CreateEventConfirmationViewModel()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                {
                    ButtonHeight = 60;
                }
                    break;
                default:
                {
                    ButtonHeight = 40;
                }
                    break;
            }
        }
        #endregion
        #region Disable ItemSelectedCommand
        public ICommand DisableItemSelectedCommand => new Command(DisableItemSelected);
        public void DisableItemSelected()
        {
        }
        #endregion
        public override Task InitializeAsync(object navigationData)
        {
            DebugService.WriteLine(navigationData);

            if (navigationData is ObservableCollection<Item> entryList)
            {
                foreach (var item in entryList)
                {
                    DebugService.WriteLine(item.Entry);
                }

                List = entryList;
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
