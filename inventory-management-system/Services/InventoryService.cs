using inventory_management_system.Models;

namespace inventory_management_system.Services;

public class InventoryService
{
    private readonly List<Product> _products = new();

    private readonly List<Category> _categories = new();

    private readonly List<Supplier> _suppliers = new();

    private readonly List<Warehouse> _warehouses = new();

    private readonly List<WarehouseStock> _warehouseStocks = new();

    private readonly List<StockMovement> _stockMovements = new();

    private readonly List<Purchase> _purchases = new();

    private readonly List<Sale> _sales = new();

    private int _nextProductId = 1;

    private int _nextCategoryId = 1;

    private int _nextSupplierId = 1;

    private int _nextWarehouseId = 1;

    private int _nextWarehouseStockId = 1;

    private int _nextMovementId = 1;

    private int _nextPurchaseId = 1;

    private int _nextPurchaseItemId = 1;

    private int _nextSaleId = 1;

    private int _nextSaleItemId = 1;


    // =========================
    // PRODUCT
    // =========================

    public bool AddProduct(
        string name,
        string sku,
        decimal price,
        int minimumStock,
        int categoryId)
    {
        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(sku))
        {
            Console.WriteLine("Ürün adı ve SKU boş olamaz.");
            return false;
        }

        if (price < 0)
        {
            Console.WriteLine("Fiyat negatif olamaz.");
            return false;
        }

        if (minimumStock < 0)
        {
            Console.WriteLine("Minimum stok negatif olamaz.");
            return false;
        }

        if (GetCategoryById(categoryId) == null)
        {
            Console.WriteLine("Kategori bulunamadı.");
            return false;
        }

        if (_products.Any(x =>
            x.SKU.Equals(
                sku,
                StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Bu SKU zaten kullanılıyor.");
            return false;
        }

        var product = new Product
        {
            Id = _nextProductId++,
            Name = name.Trim(),
            SKU = sku.Trim().ToUpperInvariant(),
            Price = price,
            MinimumStock = minimumStock,
            CategoryId = categoryId
        };

        _products.Add(product);

        Console.WriteLine("Ürün başarıyla eklendi.");

        return true;
    }


    public void ListProducts()
    {
        if (_products.Count == 0)
        {
            Console.WriteLine("Henüz ürün bulunmuyor.");
            return;
        }

        Console.WriteLine("\n--- ÜRÜNLER ---");

        foreach (var product in _products)
        {
            var category =
                GetCategoryById(product.CategoryId);

            var totalStock =
                GetTotalStock(product.Id);

            Console.WriteLine(
                $"ID: {product.Id} | " +
                $"Ürün: {product.Name} | " +
                $"SKU: {product.SKU} | " +
                $"Kategori: {category?.Name ?? "Kategori Yok"} | " +
                $"Fiyat: {product.Price:C} | " +
                $"Toplam Stok: {totalStock} | " +
                $"Min: {product.MinimumStock}");
        }
    }


    public void SearchProduct(string keyword)
    {
        var results = _products
            .Where(x =>
                x.Name.Contains(
                    keyword,
                    StringComparison.OrdinalIgnoreCase) ||

                x.SKU.Contains(
                    keyword,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("Ürün bulunamadı.");
            return;
        }

        Console.WriteLine("\n--- ARAMA SONUÇLARI ---");

        foreach (var product in results)
        {
            Console.WriteLine(
                $"ID: {product.Id} | " +
                $"{product.Name} | " +
                $"SKU: {product.SKU} | " +
                $"Toplam Stok: {GetTotalStock(product.Id)}");
        }
    }


    public bool DeleteProduct(int id)
    {
        var product = GetProductById(id);

        if (product == null)
        {
            Console.WriteLine("Ürün bulunamadı.");
            return false;
        }

        if (GetTotalStock(id) > 0)
        {
            Console.WriteLine(
                "Stokta bulunan ürün silinemez.");

            return false;
        }

        _products.Remove(product);

        _warehouseStocks.RemoveAll(
            x => x.ProductId == id);

        Console.WriteLine("Ürün silindi.");

        return true;
    }


    public Product? GetProductById(int id)
    {
        return _products.FirstOrDefault(
            x => x.Id == id);
    }


    public int GetTotalStock(int productId)
    {
        return _warehouseStocks
            .Where(x => x.ProductId == productId)
            .Sum(x => x.Quantity);
    }


    // =========================
    // CATEGORY
    // =========================

    public bool AddCategory(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine(
                "Kategori adı boş olamaz.");

            return false;
        }

        if (_categories.Any(x =>
            x.Name.Equals(
                name.Trim(),
                StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine(
                "Bu kategori zaten mevcut.");

            return false;
        }

        _categories.Add(new Category
        {
            Id = _nextCategoryId++,
            Name = name.Trim()
        });

        Console.WriteLine(
            "Kategori başarıyla eklendi.");

        return true;
    }


    public void ListCategories()
    {
        if (_categories.Count == 0)
        {
            Console.WriteLine(
                "Henüz kategori bulunmuyor.");

            return;
        }

        Console.WriteLine(
            "\n--- KATEGORİLER ---");

        foreach (var category in _categories)
        {
            var productCount =
                _products.Count(
                    x => x.CategoryId == category.Id);

            Console.WriteLine(
                $"ID: {category.Id} | " +
                $"{category.Name} | " +
                $"Ürün Sayısı: {productCount}");
        }
    }


    public Category? GetCategoryById(int id)
    {
        return _categories.FirstOrDefault(
            x => x.Id == id);
    }


    // =========================
    // SUPPLIER
    // =========================

    public bool AddSupplier(
        string name,
        string contactName,
        string phone,
        string email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine(
                "Tedarikçi adı boş olamaz.");

            return false;
        }

        _suppliers.Add(new Supplier
        {
            Id = _nextSupplierId++,
            Name = name.Trim(),
            ContactName = contactName.Trim(),
            Phone = phone.Trim(),
            Email = email.Trim()
        });

        Console.WriteLine(
            "Tedarikçi başarıyla eklendi.");

        return true;
    }


    public void ListSuppliers()
    {
        if (_suppliers.Count == 0)
        {
            Console.WriteLine(
                "Henüz tedarikçi bulunmuyor.");

            return;
        }

        Console.WriteLine(
            "\n--- TEDARİKÇİLER ---");

        foreach (var supplier in _suppliers)
        {
            Console.WriteLine(
                $"ID: {supplier.Id} | " +
                $"{supplier.Name} | " +
                $"Yetkili: {supplier.ContactName} | " +
                $"Telefon: {supplier.Phone} | " +
                $"E-posta: {supplier.Email}");
        }
    }


    public Supplier? GetSupplierById(int id)
    {
        return _suppliers.FirstOrDefault(
            x => x.Id == id);
    }


    // =========================
    // WAREHOUSE
    // =========================

    public bool AddWarehouse(
        string name,
        string address)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine(
                "Depo adı boş olamaz.");

            return false;
        }

        _warehouses.Add(new Warehouse
        {
            Id = _nextWarehouseId++,
            Name = name.Trim(),
            Address = address.Trim()
        });

        Console.WriteLine(
            "Depo başarıyla eklendi.");

        return true;
    }


    public void ListWarehouses()
    {
        if (_warehouses.Count == 0)
        {
            Console.WriteLine(
                "Henüz depo bulunmuyor.");

            return;
        }

        Console.WriteLine(
            "\n--- DEPOLAR ---");

        foreach (var warehouse in _warehouses)
        {
            var stockCount =
                _warehouseStocks
                    .Where(x =>
                        x.WarehouseId == warehouse.Id)
                    .Sum(x => x.Quantity);

            Console.WriteLine(
                $"ID: {warehouse.Id} | " +
                $"{warehouse.Name} | " +
                $"Adres: {warehouse.Address} | " +
                $"Toplam Adet: {stockCount}");
        }
    }


    public Warehouse? GetWarehouseById(int id)
    {
        return _warehouses.FirstOrDefault(
            x => x.Id == id);
    }


    // =========================
    // WAREHOUSE STOCK
    // =========================

    private WarehouseStock
        GetOrCreateWarehouseStock(
            int productId,
            int warehouseId)
    {
        var stock =
            _warehouseStocks.FirstOrDefault(
                x =>
                    x.ProductId == productId &&
                    x.WarehouseId == warehouseId);

        if (stock != null)
        {
            return stock;
        }

        stock = new WarehouseStock
        {
            Id = _nextWarehouseStockId++,
            ProductId = productId,
            WarehouseId = warehouseId,
            Quantity = 0
        };

        _warehouseStocks.Add(stock);

        return stock;
    }


    public int GetWarehouseStock(
        int productId,
        int warehouseId)
    {
        return _warehouseStocks
            .FirstOrDefault(
                x =>
                    x.ProductId == productId &&
                    x.WarehouseId == warehouseId)
            ?.Quantity ?? 0;
    }


    public bool IncreaseStock(
        int productId,
        int warehouseId,
        int amount,
        string referenceType = "Manual",
        int? referenceId = null)
    {
        if (GetProductById(productId) == null)
        {
            Console.WriteLine(
                "Ürün bulunamadı.");

            return false;
        }

        if (GetWarehouseById(warehouseId) == null)
        {
            Console.WriteLine(
                "Depo bulunamadı.");

            return false;
        }

        if (amount <= 0)
        {
            Console.WriteLine(
                "Miktar 0'dan büyük olmalıdır.");

            return false;
        }

        var stock =
            GetOrCreateWarehouseStock(
                productId,
                warehouseId);

        stock.Quantity += amount;

        AddStockMovement(
            productId,
            warehouseId,
            amount,
            StockMovementType.Entry,
            referenceType,
            referenceId);

        return true;
    }


    public bool DecreaseStock(
        int productId,
        int warehouseId,
        int amount,
        string referenceType = "Manual",
        int? referenceId = null)
    {
        if (GetProductById(productId) == null)
        {
            Console.WriteLine(
                "Ürün bulunamadı.");

            return false;
        }

        if (GetWarehouseById(warehouseId) == null)
        {
            Console.WriteLine(
                "Depo bulunamadı.");

            return false;
        }

        if (amount <= 0)
        {
            Console.WriteLine(
                "Miktar 0'dan büyük olmalıdır.");

            return false;
        }

        var stock =
            GetOrCreateWarehouseStock(
                productId,
                warehouseId);

        if (stock.Quantity < amount)
        {
            Console.WriteLine(
                "Seçilen depoda yeterli stok bulunmuyor.");

            return false;
        }

        stock.Quantity -= amount;

        AddStockMovement(
            productId,
            warehouseId,
            amount,
            StockMovementType.Exit,
            referenceType,
            referenceId);

        return true;
    }


    private void AddStockMovement(
        int productId,
        int warehouseId,
        int quantity,
        StockMovementType type,
        string referenceType,
        int? referenceId)
    {
        _stockMovements.Add(new StockMovement
        {
            Id = _nextMovementId++,
            ProductId = productId,
            WarehouseId = warehouseId,
            Quantity = quantity,
            Type = type,
            ReferenceType = referenceType,
            ReferenceId = referenceId,
            CreatedAt = DateTime.Now
        });
    }


    public void ListWarehouseStocks()
    {
        if (_warehouses.Count == 0 ||
            _products.Count == 0)
        {
            Console.WriteLine(
                "Listelemek için en az bir ürün ve depo olmalı.");

            return;
        }

        Console.WriteLine(
            "\n--- DEPO STOKLARI ---");

        foreach (var warehouse in _warehouses)
        {
            Console.WriteLine(
                $"\n[{warehouse.Id}] {warehouse.Name}");

            foreach (var product in _products)
            {
                var quantity =
                    GetWarehouseStock(
                        product.Id,
                        warehouse.Id);

                Console.WriteLine(
                    $"Ürün: {product.Name} | " +
                    $"SKU: {product.SKU} | " +
                    $"Stok: {quantity}");
            }
        }
    }


    public void ListLowStockProducts()
    {
        var lowStockProducts =
            new List<(
                Product Product,
                Warehouse Warehouse,
                int Quantity)>();

        foreach (var warehouse in _warehouses)
        {
            foreach (var product in _products)
            {
                var quantity =
                    GetWarehouseStock(
                        product.Id,
                        warehouse.Id);

                if (quantity <= product.MinimumStock)
                {
                    lowStockProducts.Add(
                        (product, warehouse, quantity));
                }
            }
        }

        if (lowStockProducts.Count == 0)
        {
            Console.WriteLine(
                "Kritik stokta ürün bulunmuyor.");

            return;
        }

        Console.WriteLine(
            "\n--- KRİTİK STOKLAR ---");

        foreach (var item in lowStockProducts)
        {
            Console.WriteLine(
                $"Depo: {item.Warehouse.Name} | " +
                $"Ürün: {item.Product.Name} | " +
                $"Stok: {item.Quantity} | " +
                $"Minimum: {item.Product.MinimumStock}");
        }
    }


    public void ListStockMovements()
    {
        if (_stockMovements.Count == 0)
        {
            Console.WriteLine(
                "Henüz stok hareketi bulunmuyor.");

            return;
        }

        Console.WriteLine(
            "\n--- STOK HAREKETLERİ ---");

        foreach (var movement in _stockMovements)
        {
            var product =
                GetProductById(
                    movement.ProductId);

            var warehouse =
                GetWarehouseById(
                    movement.WarehouseId);

            var reference =
                movement.ReferenceId.HasValue
                    ? $"{movement.ReferenceType} #{movement.ReferenceId}"
                    : movement.ReferenceType;

            Console.WriteLine(
                $"ID: {movement.Id} | " +
                $"Ürün: {product?.Name ?? "Bilinmeyen"} | " +
                $"Depo: {warehouse?.Name ?? "Bilinmeyen"} | " +
                $"İşlem: {movement.Type} | " +
                $"Miktar: {movement.Quantity} | " +
                $"Kaynak: {reference} | " +
                $"Tarih: {movement.CreatedAt:dd.MM.yyyy HH:mm:ss}");
        }
    }


    // =========================
    // PURCHASE
    // =========================

    public bool AddPurchase(
        int supplierId,
        int warehouseId,
        List<PurchaseItem> items)
    {
        if (GetSupplierById(supplierId) == null)
        {
            Console.WriteLine(
                "Tedarikçi bulunamadı.");

            return false;
        }

        if (GetWarehouseById(warehouseId) == null)
        {
            Console.WriteLine(
                "Depo bulunamadı.");

            return false;
        }

        if (items.Count == 0)
        {
            Console.WriteLine(
                "Alışa en az bir ürün eklenmeli.");

            return false;
        }

        foreach (var item in items)
        {
            if (GetProductById(item.ProductId) == null ||
                item.Quantity <= 0 ||
                item.UnitPrice < 0)
            {
                Console.WriteLine(
                    "Alış kalemlerinden biri geçersiz.");

                return false;
            }
        }

        var purchase = new Purchase
        {
            Id = _nextPurchaseId++,
            SupplierId = supplierId,
            WarehouseId = warehouseId,
            CreatedAt = DateTime.Now
        };

        foreach (var item in items)
        {
            item.Id = _nextPurchaseItemId++;

            item.PurchaseId =
                purchase.Id;

            purchase.Items.Add(item);
        }

        _purchases.Add(purchase);

        foreach (var item in purchase.Items)
        {
            IncreaseStock(
                item.ProductId,
                warehouseId,
                item.Quantity,
                "Purchase",
                purchase.Id);
        }

        Console.WriteLine(
            $"Alış kaydı oluşturuldu. " +
            $"Alış No: {purchase.Id}");

        Console.WriteLine(
            $"Toplam: {purchase.TotalAmount:C}");

        return true;
    }


    public void ListPurchases()
    {
        if (_purchases.Count == 0)
        {
            Console.WriteLine(
                "Henüz alış kaydı bulunmuyor.");

            return;
        }

        Console.WriteLine(
            "\n--- ALIŞLAR ---");

        foreach (var purchase in _purchases)
        {
            var supplier =
                GetSupplierById(
                    purchase.SupplierId);

            var warehouse =
                GetWarehouseById(
                    purchase.WarehouseId);

            Console.WriteLine(
                $"No: {purchase.Id} | " +
                $"Tedarikçi: {supplier?.Name ?? "Bilinmeyen"} | " +
                $"Depo: {warehouse?.Name ?? "Bilinmeyen"} | " +
                $"Kalem: {purchase.Items.Count} | " +
                $"Toplam: {purchase.TotalAmount:C} | " +
                $"Tarih: {purchase.CreatedAt:dd.MM.yyyy HH:mm}");
        }
    }


    // =========================
    // SALE
    // =========================

    public bool AddSale(
        string customerName,
        int warehouseId,
        List<SaleItem> items)
    {
        if (GetWarehouseById(warehouseId) == null)
        {
            Console.WriteLine(
                "Depo bulunamadı.");

            return false;
        }

        if (items.Count == 0)
        {
            Console.WriteLine(
                "Satışa en az bir ürün eklenmeli.");

            return false;
        }

        var groupedItems = items
            .GroupBy(x => x.ProductId)
            .Select(x => new
            {
                ProductId = x.Key,
                Quantity = x.Sum(y => y.Quantity)
            })
            .ToList();

        foreach (var item in groupedItems)
        {
            var product =
                GetProductById(item.ProductId);

            if (product == null)
            {
                Console.WriteLine(
                    "Satış kalemlerinden biri geçersiz.");

                return false;
            }

            if (item.Quantity <= 0)
            {
                Console.WriteLine(
                    "Satış miktarı 0'dan büyük olmalı.");

                return false;
            }

            var warehouseQuantity =
                GetWarehouseStock(
                    item.ProductId,
                    warehouseId);

            if (warehouseQuantity < item.Quantity)
            {
                Console.WriteLine(
                    $"{product.Name} için yeterli stok yok. " +
                    $"Mevcut: {warehouseQuantity}");

                return false;
            }
        }

        foreach (var item in items)
        {
            if (item.UnitPrice < 0)
            {
                Console.WriteLine(
                    "Satış fiyatı negatif olamaz.");

                return false;
            }
        }

        var sale = new Sale
        {
            Id = _nextSaleId++,
            CustomerName =
                string.IsNullOrWhiteSpace(customerName)
                    ? "Genel Müşteri"
                    : customerName.Trim(),
            WarehouseId = warehouseId,
            CreatedAt = DateTime.Now
        };

        foreach (var item in items)
        {
            item.Id = _nextSaleItemId++;

            item.SaleId =
                sale.Id;

            sale.Items.Add(item);
        }

        _sales.Add(sale);

        foreach (var item in sale.Items)
        {
            DecreaseStock(
                item.ProductId,
                warehouseId,
                item.Quantity,
                "Sale",
                sale.Id);
        }

        Console.WriteLine(
            $"Satış kaydı oluşturuldu. " +
            $"Satış No: {sale.Id}");

        Console.WriteLine(
            $"Toplam: {sale.TotalAmount:C}");

        return true;
    }


    public void ListSales()
    {
        if (_sales.Count == 0)
        {
            Console.WriteLine(
                "Henüz satış kaydı bulunmuyor.");

            return;
        }

        Console.WriteLine(
            "\n--- SATIŞLAR ---");

        foreach (var sale in _sales)
        {
            var warehouse =
                GetWarehouseById(
                    sale.WarehouseId);

            Console.WriteLine(
                $"No: {sale.Id} | " +
                $"Müşteri: {sale.CustomerName} | " +
                $"Depo: {warehouse?.Name ?? "Bilinmeyen"} | " +
                $"Kalem: {sale.Items.Count} | " +
                $"Toplam: {sale.TotalAmount:C} | " +
                $"Tarih: {sale.CreatedAt:dd.MM.yyyy HH:mm}");
        }
    }
}