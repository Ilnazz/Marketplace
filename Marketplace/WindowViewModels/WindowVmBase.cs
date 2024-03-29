﻿using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Marketplace.WindowViewModels;

public abstract partial class WindowVmBase : ObservableValidator
{
    [ObservableProperty]
    private string _title = string.Empty;

    public Action? CloseWindowMethod { get; set; }

    public void CloseWindow() => CloseWindowMethod?.Invoke();

    public virtual bool OnClosing() => true;
}
