﻿using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;

namespace Marketplace.WindowViewModels;

public partial class AddEditManufacturerWindowVm : WindowVmBase
{
    public ProductManufacturer Manufacturer { get; set; }

    public bool IsNew => Manufacturer.Id == 0;

    [Required(ErrorMessage = "Обязательное поле")]
    public string Name
    {
        get => Manufacturer.Name;
        set
        {
            ValidateProperty(value);
            Manufacturer.Name = value;
            OnPropertyChanged();
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        ValidateAllProperties();
        SaveCommand.NotifyCanExecuteChanged();
        if (HasErrors)
            return;

        _save = true;
        CloseWindow();
    }
    private bool CanSave()
    {
        if (Manufacturer.Id == 0)
            return HasErrors == false;

        return HasErrors == false && DatabaseContext.Entities.HasChanges();
    }

    private bool _save;

    public AddEditManufacturerWindowVm(ProductManufacturer manufacturer)
    {   
        Title = manufacturer == null ? "Добавление производителя" : "Редактирование производителя";
        Manufacturer = manufacturer ?? new ProductManufacturer();
    }

    public override bool OnClosing()
    {
        if (_save)
        {
            if (Manufacturer.Id == 0)
                DatabaseContext.Entities.ProductManufacturers.Local.Add(Manufacturer);
            DatabaseContext.Entities.SaveChanges();
        }
        else
        {
            DatabaseContext.Entities.CancelChanges();
        }

        return true;
    }
}