using System;
using System.Collections.Generic;

namespace Marketplace.Services;

public interface IBasketService<T> where T : notnull
{
    int TotalItemsCount { get; }

    event Action? StateChanged;

    void AddToBasket(T item, int count);

    void RemoveFromBasket(T item, int count);

    IReadOnlyDictionary<T, int> GetItemAndCounts();

    void AddToBasket(T item)
        => AddToBasket(item, 1);

    void ClearBasket()
    {
        var itemAndCounts = GetItemAndCounts();
        foreach (var itemAndCount in itemAndCounts)
            RemoveFromBasket(itemAndCount.Key, itemAndCount.Value);
    }

    void RemoveFromBasket(T item)
        => RemoveFromBasket(item, 1);

    int GetCount(T item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        var itemAndCounts = GetItemAndCounts();
        return itemAndCounts.TryGetValue(item, out var count) ? count : 0;
    }
}
