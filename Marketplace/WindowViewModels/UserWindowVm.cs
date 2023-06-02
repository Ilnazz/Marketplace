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

    [Required(ErrorMessage = "Обязательное поле")]
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

    [RelayCommand(CanExecute = nameof(CanSaveChanges))]
    private void SaveChanges()
    {
        SaveChangesCommand.NotifyCanExecuteChanged();

        ValidateAllProperties();
        if (HasErrors || DatabaseContext.Entities.HasChanges() == false)
            return;

        DatabaseContext.Entities.SaveChanges();
        SaveChangesCommand.NotifyCanExecuteChanged();
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
        if (HasErrors)
        {
            DatabaseContext.Entities.CancelChanges();
            return true;
        }
        else if (DatabaseContext.Entities.HasChanges() == false)
            return true;

        var dialogWindow = new Wpf.Ui.Controls.MessageBox
        {
            Content = new TextBlock
            {
                Text = "Сохранить изменения?",
                TextAlignment = TextAlignment.Center,
                FontSize = 18,
                FontWeight = FontWeights.Medium
            },
            ResizeMode = ResizeMode.NoResize,
            Title = "Подтверждение",
            ButtonLeftName = "Да",
            ButtonLeftAppearance = Wpf.Ui.Common.ControlAppearance.Primary,
            ButtonRightName = "Нет"
        };
        dialogWindow.Loaded += (_, _) =>
            dialogWindow.SizeToContent = SizeToContent.WidthAndHeight;
        dialogWindow.ButtonLeftClick += (_, _) =>
        {
            DatabaseContext.Entities.SaveChanges();
            dialogWindow.Close();
        };
        dialogWindow.ButtonRightClick += (_, _) =>
        {
            DatabaseContext.Entities.CancelChanges();
            dialogWindow.Close();
        };
        dialogWindow.ShowDialog();

        return true;
    }
}
