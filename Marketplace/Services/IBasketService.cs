using System;
using System.Collections.Generic;
using Marketplace.Database.Models;

namespace Marketplace.Services;

public interface IBasketService<T>
{
    IEnumerable<Product> Items { get; }

    int ItemCount { get; }

    event Action? StateChanged;

    void AddToBasket(T item, int quantity);

    void RemoveFromBasket(T item, int quantity);

    void AddToBasket(T item)
        => AddToBasket(item, 1);

    void RemoveFromBasket(T item)
        => RemoveFromBasket(item, 1);
}
