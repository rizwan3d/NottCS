﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using NottCS.Models;
using NottCS.Services.REST;
using NottCS.ViewModels;
using Xamarin.Forms;

namespace NottCS.Services.Navigation
{
    public static class NavigationService
    {
        private static bool _isNavigating;

        /// <summary>
        /// Used to determine the correct first page on app startup
        /// Handles all authentication on app startup
        /// </summary>
        /// <returns></returns>
        internal static async Task InitializeAsync()
        {
            bool canAuthenticate = await LoginService.MicrosoftAuthenticateWithCacheAsync();
            DebugService.WriteLine($"Can authenticate with cached data: {canAuthenticate}");
            Stopwatch stopwatch = new Stopwatch();

            if (canAuthenticate)
            {
                var userData = await RestService.RequestGetAsync<User>();
                if (userData.Item1 == "OK") //first item represents whether the request is successful
                {
                    //if either studentId or librarynumber is not filled that means is new user
                    if (String.IsNullOrEmpty(userData.Item2.StudentId) ||
                        String.IsNullOrEmpty(userData.Item2.LibraryNumber))
                    {
                        stopwatch.Start();
                        await NavigateToAsync<RegistrationViewModel>(userData.Item2);
                        DebugService.WriteLine($"Navigation took {stopwatch.ElapsedMilliseconds}ms");
                    }
                    else
                    {
                        stopwatch.Start();
                        await NavigateToAsync<AccountViewModel>(userData.Item2);
                        DebugService.WriteLine($"Navigation took {stopwatch.ElapsedMilliseconds}ms");
                    }
                }
                else
                {
                    stopwatch.Start();
                    await NavigateToAsync<LoginViewModel>();
                    DebugService.WriteLine($"Navigation took {stopwatch.ElapsedMilliseconds}ms");
                }
            }
            else
            {
                stopwatch.Start();
                await NavigateToAsync<LoginViewModel>();
                DebugService.WriteLine($"Navigation took {stopwatch.ElapsedMilliseconds}ms");
            }
        }
        /// <summary>
        /// Navigates using viewmodel, preferred way of navigation due to type checks during compile time
        /// Calls InitializeAsync method with the passed parameter during navigation, override that method to use the parameter
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="navigationParameter">parameter to be passed during navigation</param>
        /// <returns></returns>
        internal static async Task NavigateToAsync<TViewModel>(object navigationParameter = null) where TViewModel : BaseViewModel, new()
        {
            await NavigateToAsync(typeof(TViewModel), navigationParameter);
        }

        internal static async Task NavigateToAsync(Type viewModelType, object navigationParameter = null)
        {
            if (!_isNavigating) //prevents simultaneous navigations
            {
                _isNavigating = true;

                Page page = null;
                var createPageTask = CreatePage(viewModelType);

                if (viewModelType == null || !viewModelType.IsSubclassOf(typeof(BaseViewModel)))
                {
                    DebugService.WriteLine("passed viewmodel type does not inherit BaseViewModel");
                    DebugService.WriteLine("Terminating navigation...");
                    _isNavigating = false;
                    return;
                }
                //Creating page
                try
                {
                    page = await createPageTask;
                }
                catch (Exception e)
                {
                    DebugService.WriteLine(e.Message);
                    DebugService.WriteLine(e.TargetSite);
                }

                if (page == null)
                {
                    DebugService.WriteLine("Page unable to be created.");
                    DebugService.WriteLine("Terminating navigation...");
                    _isNavigating = false;
                    return;
                }
                if (Application.Current.MainPage is NavigationPage navigationPage)
                {

                    Task pushPageTask = navigationPage.PushAsync(page);
                    Task initializeAsyncTask = null;
                    if (page.BindingContext is BaseViewModel viewModel)
                    {
                        initializeAsyncTask = viewModel.InitializeAsync(navigationParameter);
                    };

                    DebugService.WriteLine($"Previous page is: {navigationPage.CurrentPage}");
                    DebugService.WriteLine($"Now navigating to:{page}");
                    await pushPageTask;
                    if (initializeAsyncTask != null) await initializeAsyncTask;
                }
                else
                {
                    DebugService.WriteLine($"MainPage is not Navigation page, now replacing it with new page");
                    Task initializeAsyncTask = null;
                    if (page.BindingContext is BaseViewModel viewModel)
                    {
                        initializeAsyncTask = viewModel.InitializeAsync(navigationParameter);
                    };
                    Application.Current.MainPage = new NavigationPage(page);
                    if (initializeAsyncTask != null) await initializeAsyncTask;
                }

                _isNavigating = false;
            }

        }

        internal static async Task GoBackAsync()
        {
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.PopAsync();
            }
        }

        private static Type GetPageTypeForViewModel(Type viewModelType)
        {
            string viewName = (viewModelType.FullName.Replace("ViewModels", "Views")).Replace("ViewModel", "Page");
            string viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            string viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            Type viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }
        private static Task<Page> CreatePage(Type viewModelType)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = null;
            try
            {

                page = Activator.CreateInstance(pageType) as Page;
            }
            catch (Exception e)
            {
                DebugService.WriteLine($"Exception thrown at CreatePage {e}");
                DebugService.WriteLine($"{e.Message}");
            }
            if (page == null)
            {
                DebugService.WriteLine($"Page not created");
                return null;
            }
            else
            {
                DebugService.WriteLine($"{page} successfully created");
                return Task.FromResult(page);
            }
        }
        
    }
}
