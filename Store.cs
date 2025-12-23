using System;
using System.Collections.Generic;

namespace DZ_2
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            new Solution().Work();
        }
    }

    class Solution
    {
        public void Work()
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");
            //Good iPhone13 = new Good(""); // вызывает ошибку

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            Console.WriteLine("остаток на складе");
            warehouse.ShowGoods();

            // блок входящих данных от пользователя
            int firstCountBuy = 4;
            int secondCountBuy = 3;
            //-------------------------------------

            OperationBuy(shop, iPhone12, firstCountBuy);
            OperationBuy(shop, iPhone11, secondCountBuy);

            Console.WriteLine("корзина покупателя");
            shop.Card.ShowGoods();

            shop.Card.ShowDelivery();

            Console.WriteLine("остаток на складе");
            warehouse.ShowGoods();

            Console.WriteLine("end");
            Console.ReadLine();
        }

        private void OperationBuy(Shop shop, Good good, int count)
        {
            if (shop != null && good != null && count > 0)
            {
                if (shop.Warehouse.Goods.TryGetValue(good, out int currentCount))
                {
                    if (currentCount >= count)
                    {
                        shop.Card.Delive(good, count);
                        shop.Warehouse.CellGood(good, currentCount - count);
                    }
                }
            }
            else
            {
                if (shop == null)
                {
                    throw new ArgumentNullException("переменная не инициализирована", nameof(shop));
                }
                else if (good == null)
                {
                    throw new ArgumentNullException("переменная не инициализирована", nameof(good));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("количество не может быть 0 или меньше", nameof(count));
                }
            }
        }
    }

    class Warehouse : Show
    {
        public Warehouse() : base()
        {

        }

        public void CellGood(Good good, int count)
        {
            Goods[good] = count;
        }
    }

    class Shop
    {
        public Warehouse Warehouse { get; private set; }
        public Card Card { get; private set; }

        public Shop(Warehouse warehouse)
        {
            if (warehouse != null)
            {
                Warehouse = warehouse;
            }
            else
            {
                throw new ArgumentNullException("переменная не инициализирована", nameof(warehouse));
            }

            Card = new Card();
        }
    }

    class Card : Show
    {
        public Card() : base()
        {

        }

        public void ShowDelivery()
        {
            Console.WriteLine("покупка");
        }
    }

    class Good
    {
        public string Name { get; private set; }

        public Good(string name)
        {
            Name = name;
            Name = name != string.Empty ? name : throw new ArgumentNullException("поле не может быть пустым", nameof(name));
        }

        public Good Clone()
        {
            return new Good(Name);
        }
    }

    abstract class Show
    {
        public Dictionary<Good, int> Goods { get; private set; }

        public Show()
        {
            Goods = new Dictionary<Good, int>();
        }

        public void ShowGoods()
        {
            if (Goods != null)
            {
                foreach (var good in Goods)
                {
                    Console.WriteLine($"{good.Key.Name} - {good.Value}");
                }
            }
        }

        public void Delive(Good good, int count)
        {
            if (good != null && count > 0)
            {
                Goods.Add(good, count);
            }
            else
            {
                if (count <= 0)
                {
                    throw new ArgumentOutOfRangeException("колечество не может быть 0 или меньше", nameof(count));
                }
                else
                {
                    throw new ArgumentNullException("переменная не инициализирована", nameof(good));
                }
            }
        }
    }
}