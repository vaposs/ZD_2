using System;
using System.Collections.Generic;

namespace yrsyrq
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
            if (shop.Warehouse._goods.TryGetValue(good, out int currentCount))
            {
                if (currentCount >= count)
                {
                    shop.Card.Delive(good, count);
                    shop.Warehouse.CellGood(good, currentCount - count);
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
            _goods[good] = count;
        }
    }

    class Shop
    {
        public Warehouse Warehouse { get; private set; }
        public Card Card { get; private set; }

        public Shop(Warehouse warehouse)
        {
            Warehouse = warehouse;
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
        }

        public Good Clone()
        {
            return new Good(Name);
        }
    }

    abstract class Show
    {
        public Dictionary<Good, int> _goods { get; private set; }

        public Show()
        {
            _goods = new Dictionary<Good, int>();
        }

        public void ShowGoods()
        {
            foreach (var good in _goods)
            {
                Console.WriteLine($"{good.Key.Name} - {good.Value}");
            }
        }

        public void Delive(Good good, int count)
        {
            _goods.Add(good, count);
        }
    }
}