using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Database;
using Marketplace.Database.Models;

namespace Marketplace.Services;

public class ClientBasketService : IBasketService<Product>
{
    public int TotalItemsCount =>
        _client.Basket.Sum(prodAndCount => prodAndCount.Quantity);

    public event Action? StateChanged;

    private Client _client = null!;

    public ClientBasketService(Client client)
    {
        _client = client;
    }

    public void AddToBasket(Product product, int count)
    {
        ArgumentNullException.ThrowIfNull(product, nameof(product));
        if (count <= 0)
            throw new ArgumentException("Should be greater than zero", nameof(count));

        var prodAndCount = _client.Basket
            .FirstOrDefault(pac => pac.Product == product);
        if (prodAndCount != null)
            prodAndCount.Quantity += count;
        else
            _client.Basket.Add(new ClientProduct
            {
                Product = product,
                Quantity = count
            });

        DatabaseContext.Entities.SaveChanges();

        StateChanged?.Invoke();
    }

    public IReadOnlyDictionary<Product, int> GetItemAndCounts() =>
        _client.Basket.ToDictionary(pac => pac.Product, pac => pac.Quantity);

    public void RemoveFromBasket(Product product, int count)
    {
        ArgumentNullException.ThrowIfNull(product, nameof(product));
        if (count <= 0)
            throw new ArgumentException("Should be greater than zero", nameof(count));

        var prodAndCount = _client.Basket.FirstOrDefault(pac => pac.Product == product);
        if (prodAndCount == null)
            throw new ArgumentException("Product is not in basket to remove it", nameof(product));
        else if (prodAndCount.Quantity < count)
            throw new ArgumentException("Product count in basket lesser than given count to remove", nameof(product));

        prodAndCount.Quantity -= count;
        if (prodAndCount.Quantity == 0)
            _client.Basket.Remove(prodAndCount);

        DatabaseContext.Entities.SaveChanges();

        StateChanged?.Invoke();
    }
}
