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
using Newtonsoft.Json;

namespace NottCS.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public string PageTitle1 { get; set; } = "News Feed";
        public string PageTitle2 { get; set; } = "Clubs & Society";
        public string PageTitle3 { get; set; } = "Profile";
        private string _selectedClubType;
        #region HomeViewModel Constructor

        public HomeViewModel()
        {
            Title = "NottCS";
            SelectedClubType = ClubListTypePickerList[0];
        }

        #endregion

        public ICommand SettingsPageNavigationCommand => new Command(async() => await NavigationService.NavigateToAsync<SettingsViewModel>());

        #region Event List
        #region ListViewNavigation
        public ICommand EventListNavigationCommand => new Command(async (object p) => await EventListNavigation(p));
        private async Task EventListNavigation(object p)
        {
            //Label = $"Hello World {Count}";
            //Count++;
            try
            {
                await NavigationService.NavigateToAsync<EventRegistrationViewModel>(p);
                DebugService.WriteLine("Button pressed");
            }
            catch (Exception e)
            {
                DebugService.WriteLine(e.Message);
            }
        }
        #endregion
        #region Disable ItemSelectedCommand
        public ICommand DisableItemSelectedCommand => new Command(DisableItemSelected);
        public void DisableItemSelected()
        {
        }
        #endregion
        #region Temporary EventList
        public ObservableCollection<Item> EventLists { get; set; } = new ObservableCollection<Item>()
        {
            new Item()
            {
                EventName = "I'm just a title",
                ImageURL = "http://icons.iconarchive.com/icons/graphicloads/100-flat/24/home-icon.png"
            },
            new Item(),
            new Item(),
            new Item()
        };


        #endregion
        #endregion

        #region Club List

        #region Picker

        public List<string> ClubListTypePickerList { get; set; } =
            new List<string> { "My Clubs Only", "All Clubs"};

        public string SelectedClubType
        {
            get => _selectedClubType;
            set
            {
                ChangeLabel1(value);
                _selectedClubType = value;
                switch (value)
                {
                    case "My Clubs Only":
                        ClubList = MyClubList;
                        break;
                    case "All Clubs":
                        ClubList = AllClubList;
                        break;
                }
            }
        }
        private void ChangeLabel1(object e)
        {
            //DebugService.WriteLine("Picker Changed");
            string picked = e.ToString();
            //DebugService.WriteLine(picked);
        }

        #endregion
        #region EventListNavigation
        public ICommand ClubListNavigationCommand => new Command(async (object p) => await ClubListNavigation(p));
        private async Task ClubListNavigation(object p)
        {
            try
            {
                await NavigationService.NavigateToAsync<ClubViewModel>(p);
                DebugService.WriteLine("Item Tapped");

            }
            catch (Exception e)
            {
                DebugService.WriteLine(e.Message);
            }

        }
        #endregion
        #region Temporary ClubList
        private ObservableCollection<Item> _clubList = new ObservableCollection<Item>();
        public ObservableCollection<Item> ClubList
        {
            get => _clubList;
            set => SetProperty(ref _clubList, value);
        }

        public ObservableCollection<Item> AllClubList { get; set; } = new ObservableCollection<Item>()
        {
            new Item()
            {
                ClubName = "ClubName",
                ImageURL = "http://icons.iconarchive.com/icons/graphicloads/100-flat/24/home-icon.png"
            },
            new Item()
            {
                ClubName = "ClubName1",
                ImageURL = "http://icons.iconarchive.com/icons/graphicloads/100-flat/24/home-icon.png"
            },
            new Item()
            {
                ClubName = "ClubName2",
                ImageURL = "http://icons.iconarchive.com/icons/graphicloads/100-flat/24/home-icon.png"
            },
            new Item()
            {
                ClubName = "ClubName3",
                ImageURL = "http://icons.iconarchive.com/icons/graphicloads/100-flat/24/home-icon.png"
            },
            new Item()
            {
                ClubName = "ClubName4",
                ImageURL = "http://icons.iconarchive.com/icons/graphicloads/100-flat/24/home-icon.png"
            },
            new Item()
            {
                ClubName = "ClubName5",
                ImageURL = "http://icons.iconarchive.com/icons/graphicloads/100-flat/24/home-icon.png"
            },
            new Item()
            {
                ClubName = "ClubName6",
                ImageURL = "http://icons.iconarchive.com/icons/graphicloads/100-flat/24/home-icon.png"
            }
        };
        public ObservableCollection<Item> MyClubList { get; set; } = new ObservableCollection<Item>()
        {
            new Item()
            {
                ClubName = "ClubName",
                ImageURL = "http://icons.iconarchive.com/icons/graphicloads/100-flat/24/home-icon.png"
            },
            new Item()
            {
                ClubName = "ClubName1",
                ImageURL = "http://icons.iconarchive.com/icons/graphicloads/100-flat/24/home-icon.png"
            },
            new Item()
            {
                ClubName = "ClubName2",
                ImageURL = "http://icons.iconarchive.com/icons/graphicloads/100-flat/24/home-icon.png"
            },
            new Item()
            {
                ClubName = "ClubName3",
                ImageURL = "http://icons.iconarchive.com/icons/graphicloads/100-flat/24/home-icon.png"
            },
            new Item()
            {
                ClubName = "ClubName4",
                ImageURL = "http://icons.iconarchive.com/icons/graphicloads/100-flat/24/home-icon.png"
            }
        };

        #endregion
        #endregion

        #region Profile
        #region ViewModalAdditionalClass

        public class UserDataObject
        {
            public string DataName { get; set; }
            public string DataValue { get; set; }
        }

        #endregion

        #region PageProperties
        private User _loginUser;
        private List<UserDataObject> _dataList;

        public User LoginUser
        {
            get => _loginUser;
            set => SetProperty(ref _loginUser, value);
        }

        public List<UserDataObject> DataList
        {
            get => _dataList;
            set => SetProperty(ref _dataList, value);
        }

        #endregion

        /// <summary>
        /// Sets the data for the page
        /// </summary>
        /// <param name="userData">Username for the account data</param>
        private void SetPageDataAsync(User userData)
        {
            LoginUser = userData;
            DebugService.WriteLine($"HomeViewModel navigationData serialized: {JsonConvert.SerializeObject(LoginUser)}");
            DataList = new List<UserDataObject>()
            {
                new UserDataObject(){DataName = "Name", DataValue = LoginUser.Name},
                new UserDataObject(){DataName = "Email", DataValue = LoginUser.Email},
                new UserDataObject(){DataName = "Student ID", DataValue = LoginUser.StudentId},
                new UserDataObject(){DataName = "Library Number", DataValue = LoginUser.LibraryNumber},
                new UserDataObject(){DataName = "Course", DataValue = LoginUser.Course},
                new UserDataObject(){DataName = "Year of Study", DataValue = LoginUser.YearOfStudy.ToString()}
            };
        }

        public ICommand SignOutCommand => new Command(SignOut);

        private static async void SignOut()
        {
            await LoginService.SignOutAndNavigateAsync();
        }

        #endregion

        /// <summary>
        /// Initializes the page
        /// </summary>
        /// <param name="navigationData">Data passed from the previous page</param>
        /// <returns></returns>
        public override Task InitializeAsync(object navigationData)
        {

            try
            {
                var userData = navigationData as User;
                Task.Run(() => SetPageDataAsync(userData));
            }
            catch (Exception e)
            {
                DebugService.WriteLine(e);
            }

            return base.InitializeAsync(navigationData);
        }
    }
}