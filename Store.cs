using System;
using System.Collections.Generic;

namespace DZ_2
{
    class MainClass
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();
            Shop shop = new Shop(warehouse);
            Cart cart = shop.Cart();

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            //Вывод всех товаров на складе с их остатком
            warehouse.PrintStock();

            cart.Add(iPhone12, 4);
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 1);
            //cart.Add(iPhone11, 0); // вызывает ошибку, неверный ввод количества
            //cart.Add(iPhone11, -1);// вызывает ошибку, неверный ввод количества
            //cart.Add(iPhone12, 4); // вызывает ошибку, товара нет на складе
            //cart.Add(iPhone11, 3); // При такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            cart.PrintCart();

            Order order = cart.Order();
            Console.WriteLine($"Ссылка для оплаты: {order.Paylink}");

            Console.WriteLine("\nСостояние склада после заказа");
            warehouse.PrintStock();

            //cart.Add(iPhone12, 2); // Ошибка, после заказа со склада убираются заказанные товары
            Console.ReadKey();
        }
    }

    public class Good
    {
        public string Name { get; }

        public Good(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj is Good other)
            {
                return Name == other.Name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class StockItem
    {
        public Good Good { get; }
        public int Count { get; private set; }

        public StockItem(Good good, int count)
        {
            Good = good != null ? good : throw new ArgumentNullException("переменная не инициализирована", nameof(good));
            Count = count > 0 ? count : throw new ArgumentOutOfRangeException(" не может быть 0 или меньше 0", nameof(count));
        }
    }

    public class Warehouse
    {
        private Dictionary<Good, int> _goods = new Dictionary<Good, int>();

        public void Delive(Good good, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Количество должно быть больше 0", nameof(quantity));
            }

            if (_goods.ContainsKey(good))
            {
                _goods[good] += quantity;
            }
            else
            {
                _goods[good] = quantity;
            }
        }

        public bool HasGood(Good good, int count)
        {
            return _goods.ContainsKey(good) && _goods[good] >= count;
        }

        public void Take(Good good, int count)
        {
            if (HasGood(good, count) == false)
            {
                throw new InvalidOperationException($"Недостаточно товара {good.Name} на складе. Требуется: {count}, есть: {(_goods.ContainsKey(good) ? _goods[good] : 0)}");
            }

            _goods[good] -= count;

            if (_goods[good] == 0)
            {
                _goods.Remove(good);
            }
        }

        public int GetCount(Good good)
        {
            return _goods.ContainsKey(good) ? _goods[good] : 0;
        }

        public void PrintStock()
        {
            Console.WriteLine("Товары на складе:");

            if (_goods.Count == 0)
            {
                Console.WriteLine("  Склад пуст");

                return;
            }

            foreach (var item in _goods)
            {
                Console.WriteLine($"  {item.Key.Name}: {item.Value} шт.");
            }
        }
    }

    public class Cart
    {
        private Dictionary<Good, int> _items = new Dictionary<Good, int>();
        private Warehouse _warehouse;
        private bool _orderPlaced = false;

        public Cart(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public void Add(Good good, int quantity)
        {
            if (_orderPlaced)
            {
                throw new InvalidOperationException("Нельзя добавлять товары после оформления заказа");
            }

            if (quantity <= 0)
            {
                throw new ArgumentException("Количество должно быть больше 0", nameof(quantity));
            }

            if (_warehouse.HasGood(good, quantity) == false)
            {
                throw new InvalidOperationException($"Недостаточно товара {good.Name} на складе. Требуется: {quantity}, есть: {_warehouse.GetCount(good)}");
            }

            if (_items.ContainsKey(good))
            {
                _items[good] += quantity;
            }
            else
            {
                _items[good] = quantity;
            }
        }

        public Order Order()
        {
            if (_orderPlaced)
            {
                throw new InvalidOperationException("Заказ уже оформлен");
            }

            if (_items.Count == 0)
            {
                throw new InvalidOperationException("Корзина пуста");
            }

            foreach (var item in _items)
            {
                _warehouse.Take(item.Key, item.Value);
            }

            _orderPlaced = true;
            return new Order(_items);
        }

        public void PrintCart()
        {
            Console.WriteLine("Товары в корзине:");
            if (_items.Count == 0)
            {
                Console.WriteLine("  Корзина пуста");

                return;
            }

            foreach (var item in _items)
            {
                Console.WriteLine($"  {item.Key.Name}: {item.Value} шт.");
            }

            Console.WriteLine();
        }
    }

    public class Order
    {
        private Dictionary<Good, int> _items;

        public Order(Dictionary<Good, int> items)
        {
            _items = new Dictionary<Good, int>(items);
        }

        public string Paylink
        {
            get { return "оплаченно"; }
        }

        public Dictionary<Good, int> GetItems()
        {
            return new Dictionary<Good, int>(_items);
        }
    }

    public class Shop
    {
        private Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public Cart Cart()
        {
            return new Cart(_warehouse);
        }
    }
}