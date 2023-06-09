using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.Models;
using Marketplace.WindowViewModels;
using Marketplace.WindowViews;

namespace Marketplace.PageViewModels;

public partial class CategoriesPageVm : PageVmBase
{
    public IEnumerable<ProductManufacturer> Manufacturers => DatabaseContext.Entities.ProductManufacturers.Local.ToList();

    [RelayCommand]
    private void Delete(ProductManufacturer manufacturer)
    {
        if (manufacturer.Products.Any())
        {
            var infoWindowVm = new InfoWindowVm("Имеются продукты этого производителя", "Ошибка");
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
            infoWindowVm.CloseWindowMethod += dw.Close;
            dw.ShowDialog();
            return;
        }

        DatabaseContext.Entities.Remove(manufacturer);
        DatabaseContext.Entities.SaveChanges();

        OnPropertyChanged(nameof(Manufacturers));
    }

    [RelayCommand]
    private void Edit(ProductManufacturer manufacturer)
    {
        var vm = new AddEditManufacturerWindowVm(manufacturer);
        var view = new AddEditManufacturerWindowView() { DataContext = vm };

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
        vm.CloseWindowMethod += dialogWindow.Close;
        dialogWindow.ShowDialog();

        OnPropertyChanged(nameof(Manufacturers));
    }

    [RelayCommand]
    private void Add()
    {
        var vm = new AddEditManufacturerWindowVm(new ProductManufacturer());
        var view = new AddEditManufacturerWindowView() { DataContext = vm };

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
        vm.CloseWindowMethod += dialogWindow.Close;
        dialogWindow.ShowDialog();

        OnPropertyChanged(nameof(Manufacturers));
    }

    public CategoriesPageVm()
    {
    }
}
