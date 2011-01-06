﻿#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.Collections.Generic;

namespace ClearCanvas.ImageViewer.Web.Client.Silverlight.Helpers
{
    public class PopupHelper
    {
        static ChildWindow _currentWindow;

        static object _syncLock = new object();

        public static bool IsPopupActive
        {
            get { return _currentWindow != null; }
        }

        private static ChildWindow CurrentPopup
        {
            get
            {
                lock (_syncLock)
                { return _currentWindow; }
            }
            set
            {
                lock (_syncLock)
                {
                    if (_currentWindow != value)
                    {
                        if (_currentWindow != null)
                            _currentWindow.Close();

                        _currentWindow = value;

                        if (_currentWindow != null)
                        {
                            _currentWindow.Closed += (s, e) => {
                                if (CurrentPopup == s)
                                {
                                    _currentWindow = null; 
                                }
                                
                            };
                        }
                    }
                }
            }
        }

        public static ChildWindow PopupContent(string title, object content)
        {
            return PopupContent(title, content, null);
        }

        public static ChildWindow PopupContent(string title, object content, IEnumerable<Button> buttons)
        {
            var msgBox = new ChildWindow();
            CurrentPopup = msgBox;
            msgBox.Style = System.Windows.Application.Current.Resources["PopupMessageWindow"] as Style;
            msgBox.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xBA, 0xD2, 0xEC));
            msgBox.Title = title;

            msgBox.MaxWidth = Application.Current.Host.Content.ActualWidth * 0.5;

            StackPanel panel = new StackPanel();
            panel.Children.Add(new ContentPresenter() { Content = content });
            
            StackPanel buttonPanel = new StackPanel() { Margin = new Thickness(20), Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
                
            if (buttons != null)
            {
                foreach (Button b in buttons)
                {
                    b.Click += (s, e) => { msgBox.Close(); };
                    panel.Children.Add(b);
                }

            }
            else
            {
                var closeButton = new Button { Content = "Close", HorizontalAlignment = HorizontalAlignment.Center,  };
                closeButton.Click += (s, e) => { msgBox.Close(); };
                buttonPanel.Children.Add(closeButton);

            }

            panel.Children.Add(buttonPanel);
            msgBox.Content = panel;

            msgBox.Show();
            return msgBox;
        }

        public static ChildWindow PopupMessage( string title, string message)
        {
            var msgBox = new ChildWindow();
            CurrentPopup = msgBox;
            
            msgBox.Style = System.Windows.Application.Current.Resources["PopupMessageWindow"] as Style;
            msgBox.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xBA, 0xD2, 0xEC));
            msgBox.Title = title;

            msgBox.MaxWidth = Application.Current.Host.Content.ActualWidth * 0.5; 
            
            msgBox.Content = new TextBlock() { Text = message, Margin = new Thickness(20), Foreground = new SolidColorBrush(Colors.White), FontSize = 14 };
            msgBox.Show();

            _currentWindow = msgBox;
            return msgBox;
        }

        public static ChildWindow PopupMessage(string title, string message, string closeButtonLabel)
        {
            var msgBox = new ChildWindow();
            CurrentPopup = msgBox;
            
            msgBox.Style = System.Windows.Application.Current.Resources["PopupMessageWindow"] as Style;
            msgBox.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xBA, 0xD2, 0xEC));
            msgBox.Title = title;
            msgBox.MaxWidth = Application.Current.Host.Content.ActualWidth * 0.5;

            StackPanel content = new StackPanel();
            content.Children.Add(new TextBlock() { Text = message, Margin = new Thickness(20), Foreground = new SolidColorBrush(Colors.White), FontSize = 14, HorizontalAlignment=HorizontalAlignment.Center });

            Button closeButton = new Button() { Content = closeButtonLabel, FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(20) };
            closeButton.Click += (s, o) => { msgBox.Close(); };
            content.Children.Add(closeButton);
            msgBox.Content = content;
            msgBox.Show();

            _currentWindow = msgBox;
            return msgBox;
        }
    }
}
