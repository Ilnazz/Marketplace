﻿using Marketplace.WindowViewModels;
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
        };
    }
}
