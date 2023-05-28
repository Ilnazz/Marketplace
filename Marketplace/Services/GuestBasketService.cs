using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Marketplace.Services;

public class GuestBasketService<T> : IBasketService<T> where T : notnull
{
    public int TotalItemsCount => _itemAndCounts.Values.Sum();

    public event Action? StateChanged;

    private readonly IDictionary<T, int> _itemAndCounts = new Dictionary<T, int>();

    public void AddToBasket(T item, int count)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        if (_itemAndCounts.ContainsKey(item))
            _itemAndCounts[item] += count;
        else
            _itemAndCounts[item] = count;

        StateChanged?.Invoke();
    }

    public void RemoveFromBasket(T item, int count)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        if (_itemAndCounts.ContainsKey(item) == false)
            throw new ArgumentException("Item is not in basket to remove it", nameof(item));
        else if (_itemAndCounts[item] < count)
            throw new ArgumentException("Item count in basket lesser than given count to remove", nameof(item));

        _itemAndCounts[item] -= count;

        StateChanged?.Invoke();
    }

    public IReadOnlyDictionary<T, int> GetItemAndCounts() =>
        new ReadOnlyDictionary<T, int>(_itemAndCounts);
}
    