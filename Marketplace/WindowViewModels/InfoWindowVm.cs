using CommunityToolkit.Mvvm.Input;

namespace Marketplace.WindowViewModels;

public partial class InfoWindowVm : WindowVmBase
{
    public string Text { get; init; }

    [RelayCommand]
    private void Ok() => CloseWindow();

    public InfoWindowVm(string text, string title)
    {
        Title = title;
        Text = text;
    }
}
