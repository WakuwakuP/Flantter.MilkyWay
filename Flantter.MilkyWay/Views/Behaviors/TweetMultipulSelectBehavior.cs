﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Flantter.MilkyWay.Views.Behaviors
{
    public class TweetMultipulSelectBehavior
    {
        public static bool GetIsMultipulSelectOpened(DependencyObject obj) { return (bool)obj.GetValue(IsMultipulSelectOpenedProperty); }
        public static void SetIsMultipulSelectOpened(DependencyObject obj, bool value) { obj.SetValue(IsMultipulSelectOpenedProperty, value); }

        public static readonly DependencyProperty IsMultipulSelectOpenedProperty =
            DependencyProperty.Register("IsMultipulSelectOpened", typeof(bool), typeof(TweetMultipulSelectBehavior), new PropertyMetadata(false, IsMultipulSelectOpened_Changed));


        private static void IsMultipulSelectOpened_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if (grid == null)
                return;

            var tweetMultipulActionGrid = grid.FindName("TweetMultipulActionGrid") as Grid;
            if (tweetMultipulActionGrid == null)
                return;

            if ((bool)e.NewValue)
            {
                (tweetMultipulActionGrid.Resources["TweetMultipulActionGridOpenAnimation"] as Storyboard).Begin();
            }
            else
            {
                (tweetMultipulActionGrid.Resources["TweetMultipulActionGridCloseAnimation"] as Storyboard).Begin();
            }
        }
    }
}
