using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database;
using Marketplace.Database.Models;
using Marketplace.DataTypes.Enums;
using Microsoft.Win32;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using Marketplace.WindowViews;

namespace Marketplace.WindowViewModels;

public partial class UserProfileWindowVm : WindowVmBase
{
    #region Properties
    [RegularExpression(@"[a-zA-Zа-яА-Я]+", ErrorMessage = "Может содержать только буквы")]
    [Required(ErrorMessage = "Обязательное поле")]
    public string Name
    {
        get => _user.Name;
        set
        {
            _user.Name = value;
            OnPropertyChanged();
            ValidateProperty(value);
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    [RegularExpression(@"[a-zA-Zа-яА-Я]+", ErrorMessage = "Может содержать только буквы")]
    [Required(ErrorMessage = "Обязательное поле")]
    public string Surname
    {
        get => _user.Surname;
        set
        {
            _user.Surname = value;
            OnPropertyChanged();
            ValidateProperty(value);
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    [RegularExpression(@"[a-zA-Zа-яА-Я]+", ErrorMessage = "Может содержать только буквы")]
    public string? Patronymic
    {
        get => _user.Patronymic;
        set
        {
            _user.Patronymic = value;
            OnPropertyChanged();
            ValidateProperty(value);
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    public string Login
    {
        get => _user.Login;
        set
        {
            _user.Login = value;
            OnPropertyChanged();
            ValidateProperty(value);
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    [Required(ErrorMessage = "Обязательное поле")]
    public string Password
    {
        get => _user.Password;
        set
        {
            _user.Password = value;
            ValidateProperty(value);
            OnPropertyChanged();
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
    }

    public BankCard? BankCard
    {
        get => _user.Client?.BankCard;
        set
        {
            if (_user.Client != null)
                _user.Client.BankCard = value;
            OnPropertyChanged();
        }
    }

    public bool CanAttachBankCard => Role == UserRole.Client;

    public bool IsBankCardAttached => BankCard != null;

    [ObservableProperty]
    private byte[]? _photo;

    public UserRole Role => _user.Role;
    #endregion

    #region Commands
    [RelayCommand]
    private void UploadPhoto()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Изображение (png, jpg, jpeg)|*.png;*.jpg;*.jpeg",
            CheckFileExists = true
        };

        if (openFileDialog.ShowDialog() != true)
            return;

        var photoFilePath = openFileDialog.FileName;
        var photoFileBytes = File.ReadAllBytes(photoFilePath);

        Photo = photoFileBytes;
    }

    [RelayCommand]
    private void ShowBankCardWindow()
    {
        var bankCard = BankCard;
        if (bankCard == null)
        {
            bankCard = new BankCard();
            bankCard.Clients.Add(_user.Client!);
        }

        var bankCardWindowVm = new BankCardWindowVm(bankCard);
        var bankCardWindowView = new BankCardWindowView() { DataContext = bankCardWindowVm };

        var dialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = bankCardWindowView,
            Width = bankCardWindowView.Width + 30,
            Height = bankCardWindowView.Height,
            SizeToContent = SizeToContent.Height,
            ResizeMode = ResizeMode.NoResize,
            Title = bankCardWindowVm.Title,
            Topmost = false,
            ShowFooter = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        bankCardWindowVm.CloseWindowMethod += dialogWindow.Close;
        dialogWindow.ShowDialog();

        OnPropertyChanged(nameof(BankCard));
        OnPropertyChanged(nameof(IsBankCardAttached));
        SaveChangesCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private void RemoveBankCard()
    {
        BankCard = null;
        OnPropertyChanged(nameof(IsBankCardAttached));
        SaveChangesCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanSaveChanges))]
    private void SaveChanges()
    {
        ValidateAllProperties();
        SaveChangesCommand.NotifyCanExecuteChanged();

        if (HasErrors || DatabaseContext.Entities.HasChanges() == false)
            return;

        DatabaseContext.Entities.SaveChanges();

        CloseWindow();
    }
    private bool CanSaveChanges() =>
        HasErrors == false && DatabaseContext.Entities.HasChanges();
    #endregion

    private User _user;

    public UserProfileWindowVm(User user)
    {
        Title = "Профиль";
        _user = user;
    }

    public override bool OnClosing()
    {
        if (DatabaseContext.Entities.HasChanges())
            DatabaseContext.Entities.CancelChanges();
        return true;
    }
}
