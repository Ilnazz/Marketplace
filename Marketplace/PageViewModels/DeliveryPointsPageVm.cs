using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.PageViewModels;

public partial class DeliveryPointsPageVm : PageVmBase
{
    public IEnumerable<DeliveryPoint> DeliveryPoints => DatabaseContext.Entities.DeliveryPoints.Local.ToList();

    [RelayCommand]
    private void Delete(DeliveryPoint dp)
    {
        if (dp.Orders.Any())
        {
            var infoWindowVm = new InfoWindowVm("Нельзя удалить - есть заказы с этим пунктом выдачи", "Ошибка");
            var infoWindowView = new InfoWindowView() { DataContext = infoWindowVm };

            var dw = new Wpf.Ui.Controls.MessageBox
            {
                Content = infoWindowView,
                Width = infoWindowView.Width + 30,
                Height = infoWindowView.Height,
                SizeToContent = SizeToContent.Height,
                ResizeMode = ResizeMode.NoResize,
                Title = infoWindowVm.Title,
                Topmost = false,
                ShowFooter = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            infoWindowVm.CloseWindowMethod = dw.Close;
            dw.ShowDialog();
            return;
        }

        DatabaseContext.Entities.Remove(dp);
        DatabaseContext.Entities.SaveChanges();

        OnPropertyChanged(nameof(DeliveryPoints));
    }

    [RelayCommand]
    private void Edit(DeliveryPoint dp)
    {
        var vm = new AddEditDeliveryPointWindowVm(dp);
        var view = new AddEditDeliveryPointWindowView() { DataContext = vm };

        var dialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = view,
            Width = view.Width + 30,
            Height = view.Height,
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
            Title = vm.Title,
            Topmost = false,
            ShowFooter = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        vm.CloseWindowMethod = dialogWindow.Close;
        dialogWindow.Closing += (_, e) => e.Cancel = vm.OnClosing() == false;
        dialogWindow.ShowDialog();

        OnPropertyChanged(nameof(DeliveryPoints));
    }

    [RelayCommand]
    private void Add()
    {
        var vm = new AddEditDeliveryPointWindowVm(new DeliveryPoint());
        var view = new AddEditDeliveryPointWindowView() { DataContext = vm };

        var dialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = view,
            Width = view.Width + 30,
            Height = view.Height,
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
            Title = vm.Title,
            Topmost = false,
            ShowFooter = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        vm.CloseWindowMethod = dialogWindow.Close;
        dialogWindow.Closing += (_, e) => e.Cancel = vm.OnClosing() == false;
        dialogWindow.ShowDialog();

        OnPropertyChanged(nameof(DeliveryPoints));
    }

    public DeliveryPointsPageVm()
    {
        DatabaseContext.Entities.DeliveryPoints.Load();
    }
}
