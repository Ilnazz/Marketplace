﻿using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System;
using Marketplace.Controls;
using Marketplace.Pages;
using Wpf.Ui.Controls;
using Marketplace.Services;
using System.Linq;
using Marketplace.Database.Models;

namespace Marketplace.WindowViews;

[ObservableObject]
public partial class NavigationWindowView : UserControl
{
    #region Properties
    [ObservableProperty]
    private string _currentPageTitle;

    [ObservableProperty]
    private string _title;
    #endregion

    public NavigationWindowView()
    {
        InitializeComponent();

        Title = "Маркетплэйс";
        DataContext = this;

        InitServices();
    }

    private void InitServices()
    {
        App.NavigationService = NavigationSideBar;
        App.NavigationService.PageService = new PageService();
        App.NavigationService.Navigated += (_, _) =>
        {
            UserNavButton.IsActive = BasketNavButton.IsActive = false;

            CurrentPageTitle = $"{NavigationSideBar.Current?.Content}";
            App.SearchService.IsEnabled = true;
        };

        App.SearchService = new SearchService(SearchBox);
        App.AuthRegService = new AuthRegService();

        App.BasketService = new GuestBasketService<Product>();
        App.BasketService.StateChanged += () =>
            BasketNavButton.ItemsCount = App.BasketService.TotalItemsCount;
    }

    private void TopNavButtonClick(object sender, RoutedEventArgs e)
    {
        var btn = (Wpf.Ui.Controls.Button)sender;
        var pageType = (Type)btn.Tag;
        
        App.NavigationService.Navigate(pageType);

        if (pageType == typeof(BasketPage))
        {
            UserNavButton.IsActive = false;
            BasketNavButton.IsActive = true;
            CurrentPageTitle = $"Корзина ({BasketNavButton.ItemsCount})";
        }
        else if (pageType == typeof(UserPage))
        {
            UserNavButton.IsActive = true;
            BasketNavButton.IsActive = false;
            CurrentPageTitle = $"Личный кабинет";
        }

        App.SearchService.IsEnabled = false;
    }
}