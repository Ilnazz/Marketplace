using System.Collections.Generic;
using Marketplace.Database.Models;
using Marketplace.PageViewModels;

namespace Marketplace.WindowViewModels;

public partial class MakeOrderWindowVm : WindowVmBase
{
    public IEnumerable<Product> Products { get; set; }

    public MakeOrderWindowVm()
    {

    }
}
