using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

abstract class Delivery
{
    public string Address;
    /// <summary>
    /// Отправить посылку
    /// </summary>
    /// <param name="address"></param>
    public abstract void Send(string Address);
}

public class Courier
{
    string Name;
    public Courier(string name) { Name = name; }
}

class Buyer
{
    private string Name;
    private string LastName;
    public string Home_Address;

    public enum DelivType
    {
        Home = 0,
        Shop,
        PickPoint,
    }
    private HomeDelivery homeDelivery;
    private PickPointDelivery pickPointDelivery;
    private ShopDelivery shopDelivery;
    public Buyer(string name, string lastname) : this(name)
    { LastName = lastname; }

    public (string type, string number)[] phones;
    string email;

    Product[] product_list = new Product[10];
    public void AddToCart(Product product)
    {
        for (int i = 0; i < product_list.Length; i++)
        {
            if (product_list[i] is null)
            {
                product_list[i] = product;
            return;
        }
        }
    }
public void RemoveFromCart(Product product)
{
    for (int i = product_list.Length - 1; i >= 0; i--)
    {
        if (product_list[i] == product)
        {
            product_list[i] = null;
            return;
        }
    }
}

private void PrintOrder()
{
        Console.WriteLine(Name + " " + lastname + " заказал:");
    for (int i = 0; i < product_list.Length; i++)
        if (product_list[i] is not null)
            Console.WriteLine(product_list[i].Product_name);

}

/// <summary>
/// Заказать доставку
/// </summary>
public void CreateOrder<TDelivery>(string address, DelivType delivType) where TDelivery : Delivery
{
    Order<TDelivery> order;
    switch (delivType)
    {
        case delivType.Shop:
            order = new Order<TDelivery>(new ShopDelivery());
            break;
        case delivType.PickPoint:
            order = new Order(new PickPointDelivery());
                break;
         case delivType.Home:
                order = new Order(new HomeDelivery());
            break;
        default:
            Console.WriteLine("Не выбран корректный способ доставки");
            return;
            break;
    }
    order.Products = this.product_list;
    order.Address = address;
    PrintOrder();
    Console.WriteLine("Создан заказ с доставкой на адрес " + address);

}
}

class HomeDelivery : Delivery
{
    Courier courier = new Courier("Имя курьера");
    //Buyer buyer1 ;
    public override void Send(string address)
    {
        Console.WriteLine("Выполняю доставку на дом по адресу: \r\n" + address);

    }
}

class PickPointDelivery : Delivery
{
    public override void Send(string address)
    {
        Console.WriteLine("Выполняю доставку в магазин по адресу: \r\n" + address);
    }
}

class ShopDelivery : Delivery
{
    public override void Send(string address)
    {
        Console.WriteLine("Выполняю доставку в магазин по адресу: \r\n" + address);
    }
}

class Product
{
    public string Product_name;
    string Product_Description;
    /// <summary>
    /// Идентификатор товара
    /// </summary>
    string Product_ID;
    public Product(string product_name)
    { Product_name = product_name; }
}


class Order<TDelivery/*,TStruct*/> where TDelivery : Delivery

{
    private TDelivery _delivery;
    
    // Агрегация
    public Order(TDelivery delivery)
    {
        _delivery = delivery;
    }
    // композиция
    //private HomeDelivery _homeDelivery;
    //public Order()
    //{
    //    _homeDelivery = new HomeDelivery();
    //}

    public int Number;

    public string Address;

    public string Description;

    public Product[] Products;

    public void DisplayAddress()
    {
        Console.WriteLine(Delivery.Address);
    }
}
internal class Program
{
    static void Main(string[] args)
    {
        Buyer Peter = new Buyer("Griffin","Peter");
        //Peter.phones[] = Peter.phones[2];
        Peter.Home_Address = "123 Quahog";
        Peter.AddToCart(new Product("Кофе"));
        Peter.AddToCart(new Product("Сыр"));
        Peter.AddToCart(new Product("Молоко"));
        Peter.CreateOrder<HomeDelivery>(Peter.Home_Address, Buyer.DelivType.Home);

        Buyer Lois = new Buyer("Lois", "Griffin");
        Lois.Home_Address = "123 Quahog";
        Lois.AddToCart(new Product("Колбаса"));
        Lois.AddToCart(new Product("Хлеб"));
        Lois.AddToCart(new Product("Бумага"));
        Lois.CreateOrder<ShopDelivery>("Street av. 2", Buyer.DelivType.Shop);

        Buyer Joe = new Buyer("Joe", "Swanson");
        Joe.Home_Address = "312 Quahog";
        Joe.AddToCart(new Product("Пончик"));
        Joe.AddToCart(new Product("Кофе"));
        Joe.CreateOrder<PickPointDelivery>("3rd str. 32", Buyer.DelivType.PickPoint);


        Console.ReadKey();

    }
}