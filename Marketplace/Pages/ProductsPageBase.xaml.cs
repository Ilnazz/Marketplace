using Marketplace.DataTypes.Enums;
using Marketplace.PageViewModels;
using Wpf.Ui.Controls;

namespace Marketplace.Pages;

public abstract partial class ProductsPageBase : UiPage
{
    public ProductsPageBase(ProductCategory category)
    {
        DataContext = new ProductsPageVm(category);
        InitializeComponent();
    }
}