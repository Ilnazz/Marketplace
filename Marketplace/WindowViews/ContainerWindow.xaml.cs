using Marketplace.WindowViewModels;
using Wpf.Ui.Controls;

namespace Marketplace.WindowViews;

public partial class ContainerWindow : UiWindow
{
    public ContainerWindow(WindowVmBase windowVm)
    {
        InitializeComponent();

        DataContext = windowVm;

        windowVm.CloseWindowMethod = this.Close;
        this.Closing += (_, e) => e.Cancel = windowVm.OnClosing() == false;

        Loaded += (_, _) =>
        {
            //WindowState = System.Windows.WindowState.Maximized;
            //WindowState = System.Windows.WindowState.Normal;
            //SizeToContent = System.Windows.SizeToContent.Width;
            //SizeToContent = System.Windows.SizeToContent.Height;
            //SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
        };
    }
}
