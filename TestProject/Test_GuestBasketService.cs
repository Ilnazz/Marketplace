using Marketplace.Database.Models;
using Marketplace.Services;

namespace TestProject
{
    public class Test_GuestBasketService
    {
        private readonly IBasketService<Product> _basketService = new GuestBasketService<Product>();

        [Fact]
        public void TestAdd()
        {
            _basketService.AddToBasket(new Product());
            Assert.True(_basketService.TotalItemsCount == 1);
            _basketService.ClearBasket();
        }

        [Fact]
        public void TestRemove()
        {
            var product = new Product();
            _basketService.AddToBasket(product);
            _basketService.RemoveFromBasket(product);
            Assert.True(_basketService.TotalItemsCount == 0);
            _basketService.ClearBasket();
        }

        [Fact]
        public void TestClear()
        {
            _basketService.AddToBasket(new Product());
            _basketService.ClearBasket();
            Assert.True(_basketService.TotalItemsCount == 0);
        }

        [Fact]
        public void TestGetCount()
        {
            var product = new Product();
            _basketService.AddToBasket(product, 10);
            Assert.True(_basketService.GetCount(product) == 10);
            _basketService.ClearBasket();
        }

        [Fact]
        public void TestGetTotalItemsCount()
        {
            Product p1 = new Product(),
                    p2 = new Product();

            _basketService.AddToBasket(p1, 5);
            _basketService.AddToBasket(p2, 20);
            Assert.True(_basketService.TotalItemsCount == 25);
            _basketService.ClearBasket();
        }

        [Fact]
        public void TestInvokesStateChangedEvent()
        {
            var stateWasChanged = false;
            var stateChangedEventHandler = () => { stateWasChanged = true; };

            _basketService.StateChanged += stateChangedEventHandler;

            var product = new Product();

            _basketService.AddToBasket(product);
            Assert.True(stateWasChanged);
            stateWasChanged = false;

            _basketService.RemoveFromBasket(product);
            Assert.True(stateWasChanged);

            _basketService.StateChanged -= stateChangedEventHandler;
            _basketService.ClearBasket();
        }
    }
}