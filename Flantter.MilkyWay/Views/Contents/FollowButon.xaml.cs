﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Flantter.MilkyWay.Views.Contents
{
    public sealed partial class FollowButon : UserControl
    {
        private bool pointerOver = false;
        public FollowButon()
        {
            this.InitializeComponent();

            this.Loaded += (s, e) => 
            {
                this.FollowButton.PointerEntered += (_, __) => { pointerOver = true; this.ContentChange(); };
                this.FollowButton.PointerExited += (_, __) => { pointerOver = false; this.ContentChange(); };
                this.ContentChange();
            };
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(FollowButon), new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(FollowButon), new PropertyMetadata(null));

        public object ButtonContent
        {
            get { return (object)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }
        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent", typeof(object), typeof(FollowButon), new PropertyMetadata("Follow", ButtonContentChanged));

        public object ButtonPointerOverContent
        {
            get { return (object)GetValue(ButtonPointerOverContentProperty); }
            set { SetValue(ButtonPointerOverContentProperty, value); }
        }
        public static readonly DependencyProperty ButtonPointerOverContentProperty =
            DependencyProperty.Register("ButtonPointerOverContent", typeof(object), typeof(FollowButon), new PropertyMetadata("Follow", ButtonContentChanged));

        private static void ButtonContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var followButton = d as FollowButon;
            followButton.ContentChange();
        }

        public void ContentChange()
        {
            if (!pointerOver)
                this.FollowButton.Content = ButtonContent;
            else
                this.FollowButton.Content = ButtonPointerOverContent;
        }

        private void FollowButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Command != null && this.Command.CanExecute(this.CommandParameter))
                this.Command.Execute(this.CommandParameter);
        }
    }
}
