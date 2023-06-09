using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;

namespace Marketplace.WindowViewModels;

public partial class AddEditDeliveryPointWindowVm : WindowVmBase
{
    public DeliveryPoint DeliveryPoint { get; set; }

    public bool IsNew => DeliveryPoint.Id == 0;

    [Required(ErrorMessage = "Обязательное поле")]
    public string Name
    {
        get => DeliveryPoint.Name;
        set
        {
            ValidateProperty(value);
            DeliveryPoint.Name = value;
            OnPropertyChanged();
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    [Required(ErrorMessage = "Обязательное поле")]
    public string Address
    {
        get => DeliveryPoint.Address;
        set
        {
            ValidateProperty(value);
            DeliveryPoint.Address = value;
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
        if (DeliveryPoint.Id == 0)
            return HasErrors == false;

        return HasErrors == false && DatabaseContext.Entities.HasChanges();
    }

    private bool _save;

    public AddEditDeliveryPointWindowVm(DeliveryPoint dp)
    {
        Title = dp == null ? "Добавление производителя" : "Редактирование производителя";
        DeliveryPoint = dp ?? new DeliveryPoint();
    }

    public override bool OnClosing()
    {
        if (_save)
        {
            if (DeliveryPoint.Id == 0)
                DatabaseContext.Entities.DeliveryPoints.Local.Add(DeliveryPoint);
            DatabaseContext.Entities.SaveChanges();
        }
        else
            DatabaseContext.Entities.CancelChanges();

        return true;
    }
}
