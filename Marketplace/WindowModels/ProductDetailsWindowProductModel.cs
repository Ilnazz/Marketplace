using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Database.Models;
using Marketplace.WindowModels.Base;

namespace Marketplace.WindowModels;

public partial class ProductDetailsWindowProductModel : WindowProductModelBase
{
    #region Photo
    private IEnumerable<byte[]> _photos => _product.ProductPhotos.Select(pp => pp.Data);
    public byte[]? CurrentPhoto => _photos.Count() > 0 ? _photos.ElementAt(_currentPhotoIndex) : null;

    private int _currentPhotoIndex = 0;
    public int CurrentPhotoNumber => _currentPhotoIndex + 1;
    public int TotalPhotosNumber => _photos.Count();


    [RelayCommand(CanExecute = nameof(CanShowPrevPhoto))]
    private void ShowPrevPhoto()
    {
        _currentPhotoIndex -= 1;
        ShowPrevPhotoCommand.NotifyCanExecuteChanged();
        ShowNextPhotoCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(CurrentPhoto));
        OnPropertyChanged(nameof(CurrentPhotoNumber));
    }
    private bool CanShowPrevPhoto() => _currentPhotoIndex > 0;


    [RelayCommand(CanExecute = nameof(CanShowNextPhoto))]
    private void ShowNextPhoto()
    {
        _currentPhotoIndex += 1;
        ShowPrevPhotoCommand.NotifyCanExecuteChanged();
        ShowNextPhotoCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(CurrentPhotoNumber));
    }
    private bool CanShowNextPhoto() => _currentPhotoIndex < TotalPhotosNumber - 1;
    #endregion

    public ProductDetailsWindowProductModel(Product product) : base(product)
    {
    }
}
