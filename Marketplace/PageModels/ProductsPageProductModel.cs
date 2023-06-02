using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Marketplace.Database.Models;
using Marketplace.PageModels.Base;
using Marketplace.Pages;
using Wpf.Ui.Controls;

namespace Marketplace.PageModels;

public partial class ProductsPageProductModel : PageProductModelBase
{
    public ProductsPageProductModel(Product product) : base(product)
    {
    }
}