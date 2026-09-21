using inventory_management_system.Models;
using inventory_management_system.Services;

var inventoryService = new InventoryService();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=============================================");
    Console.WriteLine("        INVENTORY MANAGEMENT SYSTEM");
    Console.WriteLine("=============================================");
    Console.WriteLine("1  - Ürün Ekle");
    Console.WriteLine("2  - Ürünleri Listele");
    Console.WriteLine("3  - Ürün Ara");
    Console.WriteLine("4  - Ürün Sil");
    Console.WriteLine("5  - Depoya Manuel Stok Girişi");
    Console.WriteLine("6  - Depodan Manuel Stok Çıkışı");
    Console.WriteLine("7  - Kritik Stokları Listele");
    Console.WriteLine("8  - İndirimli Fiyat Hesapla");
    Console.WriteLine("9  - Stok Hareketlerini Listele");
    Console.WriteLine("10 - Kategori Ekle");
    Console.WriteLine("11 - Kategorileri Listele");
    Console.WriteLine("12 - Tedarikçi Ekle");
    Console.WriteLine("13 - Tedarikçileri Listele");
    Console.WriteLine("14 - Depo Ekle");
    Console.WriteLine("15 - Depoları Listele");
    Console.WriteLine("16 - Depo Stoklarını Listele");
    Console.WriteLine("17 - Alış Yap");
    Console.WriteLine("18 - Alışları Listele");
    Console.WriteLine("19 - Satış Yap");
    Console.WriteLine("20 - Satışları Listele");
    Console.WriteLine("0  - Çıkış");
    Console.WriteLine("=============================================");

    Console.Write("Seçiminiz: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddProduct();
            break;

        case "2":
            inventoryService.ListProducts();
            break;

        case "3":
            SearchProduct();
            break;

        case "4":
            DeleteProduct();
            break;

        case "5":
            ManualIncreaseStock();
            break;

        case "6":
            ManualDecreaseStock();
            break;

        case "7":
            inventoryService.ListLowStockProducts();
            break;

        case "8":
            CalculateDiscountedPrice();
            break;

        case "9":
            inventoryService.ListStockMovements();
            break;

        case "10":
            AddCategory();
            break;

        case "11":
            inventoryService.ListCategories();
            break;

        case "12":
            AddSupplier();
            break;

        case "13":
            inventoryService.ListSuppliers();
            break;

        case "14":
            AddWarehouse();
            break;

        case "15":
            inventoryService.ListWarehouses();
            break;

        case "16":
            inventoryService.ListWarehouseStocks();
            break;

        case "17":
            AddPurchase();
            break;

        case "18":
            inventoryService.ListPurchases();
            break;

        case "19":
            AddSale();
            break;

        case "20":
            inventoryService.ListSales();
            break;

        case "0":
            Console.WriteLine(
                "Program kapatılıyor...");

            return;

        default:
            Console.WriteLine(
                "Geçersiz seçim.");

            break;
    }
}


// =========================
// PRODUCT
// =========================

void AddProduct()
{
    Console.WriteLine("\n--- ÜRÜN EKLE ---");

    inventoryService.ListCategories();

    Console.Write("Ürün adı: ");
    var name = Console.ReadLine() ?? "";

    Console.Write("SKU: ");
    var sku = Console.ReadLine() ?? "";

    var price =
        ReadDecimal("Fiyat: ", 0);

    var minimumStock =
        ReadInt("Minimum stok: ", 0);

    var categoryId =
        ReadInt("Kategori ID: ", 1);

    inventoryService.AddProduct(
        name,
        sku,
        price,
        minimumStock,
        categoryId);
}


void SearchProduct()
{
    Console.Write(
        "Aranacak ürün veya SKU: ");

    var keyword =
        Console.ReadLine() ?? "";

    inventoryService.SearchProduct(
        keyword);
}


void DeleteProduct()
{
    var id =
        ReadInt(
            "Silinecek ürün ID: ",
            1);

    inventoryService.DeleteProduct(id);
}


// =========================
// STOCK
// =========================

void ManualIncreaseStock()
{
    if (!HasProductOrStop())
        return;

    if (!HasWarehouseOrStop())
        return;

    inventoryService.ListProducts();

    var productId =
        ReadInt(
            "Ürün ID: ",
            1);

    inventoryService.ListWarehouses();

    var warehouseId =
        ReadInt(
            "Depo ID: ",
            1);

    var amount =
        ReadInt(
            "Eklenecek miktar: ",
            1);

    if (inventoryService.IncreaseStock(
        productId,
        warehouseId,
        amount))
    {
        var product =
            inventoryService.GetProductById(
                productId);

        Console.WriteLine(
            $"{product?.Name} stokuna " +
            $"{amount} adet eklendi.");

        Console.WriteLine(
            $"Yeni depo stoku: " +
            $"{inventoryService.GetWarehouseStock(
                productId,
                warehouseId)}");
    }
}


void ManualDecreaseStock()
{
    if (!HasProductOrStop())
        return;

    if (!HasWarehouseOrStop())
        return;

    inventoryService.ListProducts();

    var productId =
        ReadInt(
            "Ürün ID: ",
            1);

    inventoryService.ListWarehouses();

    var warehouseId =
        ReadInt(
            "Depo ID: ",
            1);

    var amount =
        ReadInt(
            "Çıkarılacak miktar: ",
            1);

    if (inventoryService.DecreaseStock(
        productId,
        warehouseId,
        amount))
    {
        var product =
            inventoryService.GetProductById(
                productId);

        Console.WriteLine(
            $"{product?.Name} stokundan " +
            $"{amount} adet çıkarıldı.");

        Console.WriteLine(
            $"Yeni depo stoku: " +
            $"{inventoryService.GetWarehouseStock(
                productId,
                warehouseId)}");
    }
}


void CalculateDiscountedPrice()
{
    inventoryService.ListProducts();

    var productId =
        ReadInt(
            "Ürün ID: ",
            1);

    var discount =
        ReadDecimal(
            "İndirim yüzdesi: ",
            0,
            100);

    var product =
        inventoryService.GetProductById(
            productId);

    if (product == null)
    {
        Console.WriteLine(
            "Ürün bulunamadı.");

        return;
    }

    var discountedPrice =
        product.CalculateDiscountedPrice(
            discount);

    var discountAmount =
        product.Price - discountedPrice;

    Console.WriteLine();

    Console.WriteLine(
        $"Ürün: {product.Name}");

    Console.WriteLine(
        $"Normal fiyat: {product.Price:C}");

    Console.WriteLine(
        $"İndirim: %{discount}");

    Console.WriteLine(
        $"İndirim miktarı: " +
        $"{discountAmount:C}");

    Console.WriteLine(
        $"İndirimli fiyat: " +
        $"{discountedPrice:C}");
}


// =========================
// CATEGORY
// =========================

void AddCategory()
{
    Console.Write(
        "Kategori adı: ");

    var name =
        Console.ReadLine() ?? "";

    inventoryService.AddCategory(
        name);
}


// =========================
// SUPPLIER
// =========================

void AddSupplier()
{
    Console.WriteLine(
        "\n--- TEDARİKÇİ EKLE ---");

    Console.Write(
        "Tedarikçi adı: ");

    var name =
        Console.ReadLine() ?? "";

    Console.Write(
        "Yetkili kişi: ");

    var contactName =
        Console.ReadLine() ?? "";

    Console.Write(
        "Telefon: ");

    var phone =
        Console.ReadLine() ?? "";

    Console.Write(
        "E-posta: ");

    var email =
        Console.ReadLine() ?? "";

    inventoryService.AddSupplier(
        name,
        contactName,
        phone,
        email);
}


// =========================
// WAREHOUSE
// =========================

void AddWarehouse()
{
    Console.WriteLine(
        "\n--- DEPO EKLE ---");

    Console.Write(
        "Depo adı: ");

    var name =
        Console.ReadLine() ?? "";

    Console.Write(
        "Adres: ");

    var address =
        Console.ReadLine() ?? "";

    inventoryService.AddWarehouse(
        name,
        address);
}


// =========================
// PURCHASE
// =========================

void AddPurchase()
{
    inventoryService.ListSuppliers();

    var supplierId =
        ReadInt(
            "Tedarikçi ID: ",
            1);

    inventoryService.ListWarehouses();

    var warehouseId =
        ReadInt(
            "Mal girişi yapılacak depo ID: ",
            1);

    var items =
        new List<PurchaseItem>();

    while (true)
    {
        Console.WriteLine();

        inventoryService.ListProducts();

        var productId =
            ReadInt(
                "Ürün ID: ",
                1);

        var quantity =
            ReadInt(
                "Miktar: ",
                1);

        var unitPrice =
            ReadDecimal(
                "Alış birim fiyatı: ",
                0);

        var product =
            inventoryService.GetProductById(
                productId);

        if (product == null)
        {
            Console.WriteLine(
                "Ürün bulunamadı.");

            continue;
        }

        items.Add(
            new PurchaseItem
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = unitPrice
            });

        if (!AskContinue(
            "Başka ürün eklemek istiyor musunuz? (E/H): "))
        {
            break;
        }
    }

    inventoryService.AddPurchase(
        supplierId,
        warehouseId,
        items);
}


// =========================
// SALE
// =========================

void AddSale()
{
    inventoryService.ListWarehouses();

    var warehouseId =
        ReadInt(
            "Satış yapılacak depo ID: ",
            1);

    Console.Write(
        "Müşteri adı: ");

    var customerName =
        Console.ReadLine() ?? "";

    var items =
        new List<SaleItem>();

    while (true)
    {
        Console.WriteLine();

        inventoryService.ListProducts();

        var productId =
            ReadInt(
                "Ürün ID: ",
                1);

        var product =
            inventoryService.GetProductById(
                productId);

        if (product == null)
        {
            Console.WriteLine(
                "Ürün bulunamadı.");

            continue;
        }

        var available =
            inventoryService.GetWarehouseStock(
                productId,
                warehouseId);

        Console.WriteLine(
            $"Mevcut depo stoğu: {available}");

        var quantity =
            ReadInt(
                "Miktar: ",
                1);

        if (available < quantity)
        {
            Console.WriteLine(
                "Yetersiz stok.");

            continue;
        }

        var unitPrice =
            ReadDecimal(
                $"Satış birim fiyatı " +
                $"(önerilen {product.Price:C}): ",
                0);

        if (unitPrice == 0)
        {
            unitPrice =
                product.Price;
        }

        items.Add(
            new SaleItem
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = unitPrice
            });

        if (!AskContinue(
            "Başka ürün eklemek istiyor musunuz? (E/H): "))
        {
            break;
        }
    }

    inventoryService.AddSale(
        customerName,
        warehouseId,
        items);
}


// =========================
// HELPERS
// =========================

bool HasProductOrStop()
{
    return true;
}


bool HasWarehouseOrStop()
{
    return true;
}


int ReadInt(
    string message,
    int minValue,
    int maxValue = int.MaxValue)
{
    while (true)
    {
        Console.Write(message);

        if (int.TryParse(
            Console.ReadLine(),
            out var value))
        {
            if (value >= minValue &&
                value <= maxValue)
            {
                return value;
            }
        }

        Console.WriteLine(
            $"Lütfen {minValue} ile " +
            $"{maxValue} arasında " +
            $"geçerli bir sayı girin.");
    }
}


decimal ReadDecimal(
    string message,
    decimal minValue,
    decimal maxValue = decimal.MaxValue)
{
    while (true)
    {
        Console.Write(message);

        if (decimal.TryParse(
            Console.ReadLine(),
            out var value))
        {
            if (value >= minValue &&
                value <= maxValue)
            {
                return value;
            }
        }

        Console.WriteLine(
            "Geçerli bir sayı girin.");
    }
}


bool AskContinue(string message)
{
    Console.Write(message);

    var answer =
        (Console.ReadLine() ?? "")
        .Trim()
        .ToLowerInvariant();

    return answer == "e" ||
           answer == "evet";
}