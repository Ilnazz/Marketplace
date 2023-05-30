using System;
using System.Windows.Controls;

namespace Marketplace.Services;

public class SearchService
{
    public string SearchText => _searchTextBox.Text ?? string.Empty;

    public bool IsEnabled
    {
        get => _searchTextBox.IsEnabled;
        set => _searchTextBox.IsEnabled = value;
    }

    public event Action<string>? SearchTextChanged;

    private readonly TextBox _searchTextBox = null!;

    public SearchService(TextBox searchTextBox)
    {
        _searchTextBox = searchTextBox ?? throw new ArgumentNullException(nameof(searchTextBox));

        _searchTextBox.TextChanged += (_, _) => SearchTextChanged?.Invoke(SearchText);
    }
}
