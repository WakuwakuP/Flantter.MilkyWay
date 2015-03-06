﻿using Flantter.MilkyWay.Models;
using Microsoft.Practices.Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Navigation;

namespace Flantter.MilkyWay.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        private MainPageModel _MainPageModel { get; set; }

        public ReadOnlyReactiveCollection<AccountViewModel> Account { get; private set; }

        #region Constructor
        public MainPageViewModel()
        {
            this._MainPageModel = MainPageModel.Instance;
            this.Account = this._MainPageModel.ReadOnlyAccount.ToReadOnlyReactiveCollection(x => new AccountViewModel(x));
        }
        #endregion

        #region Destructor
        ~MainPageViewModel()
        {
        }
        #endregion

        #region Others
        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            this._MainPageModel.Initialize();
        }
        #endregion
    }
}
