using Marketplace.WindowViewModels;
using Wpf.Ui.Controls;

namespace Marketplace.WindowViews;

public partial class TitledContainerWindow : UiWindow
{
    public TitledContainerWindow(WindowVmBase windowVm)
    {
        InitializeComponent();

        DataContext = windowVm;

        windowVm.CloseWindowMethod = this.Close;
        this.Closing += (_, e) => e.Cancel = windowVm.OnClosing() == false;

        Loaded += (_, _) =>
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
        };
    }
}
