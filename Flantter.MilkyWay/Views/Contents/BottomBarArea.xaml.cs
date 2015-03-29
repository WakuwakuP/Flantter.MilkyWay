﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// ユーザー コントロールのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace Flantter.MilkyWay.Views.Contents
{
    public sealed partial class BottomBarArea : UserControl
    {
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(BottomBarArea), new PropertyMetadata(0, SelectedIndex_Changed));

        private static void SelectedIndex_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bottomBar = d as BottomBarArea;
            bottomBar.BottomBar_SelectedIndexChanged();
        }

        public BottomBarArea()
        {
            this.InitializeComponent();

            this.SizeChanged += BottomBar_SizeChanged;

            this.BottomBar_OthersButton_Flyout.ItemSelected += BottomBar_OthersButton_Flyout_ItemSelected;
            this.BottomBar_OthersTextButton_Flyout.ItemSelected += BottomBar_OthersButton_Flyout_ItemSelected;

            this.BottomBar_OthersButton_Flyout.Source = new ObservableCollection<string>() { "Events", "Favorites" };
            this.BottomBar_OthersTextButton_Flyout.Source = new ObservableCollection<string>() { "Events", "Favorites" };

            this.BottomBar_SelectedIndexChanged();
        }

        private void BottomBar_OthersButton_Flyout_ItemSelected(object sender, Controls.ItemSelectedEventArgs e)
        {
            this.SelectedIndex = 3 + e.Index;
        }

        public void BottomBar_SelectedIndexChanged()
        {
            this.BottomBar_HomeButton.Background = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonUnselectedBackgroundBrush"]);
            this.BottomBar_HomeIcon.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonUnselectedForegroundBrush"]);
            this.BottomBar_MentionsButton.Background = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonUnselectedBackgroundBrush"]);
            this.BottomBar_MentionsIcon.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonUnselectedForegroundBrush"]);
            this.BottomBar_DirectMessagesButton.Background = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonUnselectedBackgroundBrush"]);
            this.BottomBar_DirectMessagesIcon.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonUnselectedForegroundBrush"]);
            this.BottomBar_OthersButton.Background = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonUnselectedBackgroundBrush"]);
            this.BottomBar_OthersIcon.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonUnselectedForegroundBrush"]);

            this.BottomBar_HomeTextButton.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarTextblockButtonUnselectedBrush"]);
            this.BottomBar_MentionsTextButton.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarTextblockButtonUnselectedBrush"]);
            this.BottomBar_DirectMessagesTextButton.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarTextblockButtonUnselectedBrush"]);
            this.BottomBar_OthersTextButton.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarTextblockButtonUnselectedBrush"]);

            if (this.SelectedIndex == 0)
            {
                this.BottomBar_HomeButton.Background = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonSelectedBackgroundBrush"]);
                this.BottomBar_HomeIcon.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonSelectedForegroundBrush"]);
                this.BottomBar_HomeTextButton.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarTextblockButtonSelectedBrush"]);
            }
            else if (this.SelectedIndex == 1)
            {
                this.BottomBar_MentionsButton.Background = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonSelectedBackgroundBrush"]);
                this.BottomBar_MentionsIcon.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonSelectedForegroundBrush"]);
                this.BottomBar_MentionsTextButton.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarTextblockButtonSelectedBrush"]);
            }
            else if (this.SelectedIndex == 2)
            {
                this.BottomBar_DirectMessagesButton.Background = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonSelectedBackgroundBrush"]);
                this.BottomBar_DirectMessagesIcon.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonSelectedForegroundBrush"]);
                this.BottomBar_DirectMessagesTextButton.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarTextblockButtonSelectedBrush"]);
            }
            else
            {
                this.BottomBar_OthersButton.Background = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonSelectedBackgroundBrush"]);
                this.BottomBar_OthersIcon.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarButtonSelectedForegroundBrush"]);
                this.BottomBar_OthersTextButton.Foreground = ((SolidColorBrush)Application.Current.Resources["BottomBarTextblockButtonSelectedBrush"]);
            }
        }

        void BottomBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.BottomBar_ProfileImageButton.Width = e.NewSize.Height;
            this.BottomBar_ProfileImageButton.Height = e.NewSize.Height;

            if (e.NewSize.Width < 384)
                VisualStateManager.GoToState(this, "Under384px", true);
            else if (e.NewSize.Width < 500)
                VisualStateManager.GoToState(this, "Under500px", true);
            else if (e.NewSize.Width < 700)
                VisualStateManager.GoToState(this, "Under700px", true);
            else
                VisualStateManager.GoToState(this, "Default", true);
        }

        private void BottomBar_HomeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SelectedIndex = 0;
        }

        private void BottomBar_MentionsButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SelectedIndex = 1;
        }

        private void BottomBar_DirectMessagesButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SelectedIndex = 2;
        }
    }
}
