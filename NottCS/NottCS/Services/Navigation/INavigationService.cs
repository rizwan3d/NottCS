﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NottCS.ViewModels;

namespace NottCS.Services.Navigation
{
    public interface INavigationService
    {
        Task NavigateToAsync<TViewModel>(object navigationParameter = null) where TViewModel : BaseViewModel;
        Task NavigateToAsync(Type viewModelType, object navigationParameter = null);
        Task SetDetailPageAsync<TViewModel>(object navigationParameter = null) where TViewModel : BaseViewModel;
        Task SetDetailPageAsync(Type viewModelType, object navigationParameter = null);

        Task SetMainPageAsync<TViewModel>(object navigationParameter = null) where TViewModel : BaseViewModel;
        Task SetMainPageAsync(Type viewModelType, object navigationParameter = null);

        Task BackUntilAsync<TViewModel>() where TViewModel : BaseViewModel, new();
    }
}