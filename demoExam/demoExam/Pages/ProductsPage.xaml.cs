using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using demoExam.Model;

namespace demoExam.Pages;

public partial class ProductsPage : Page
    {
        private readonly User? _currentUser;
        private List<ProductItem> _allProducts = new();
        private bool _suppressApply;

        public bool IsAdminOrManager => _currentUser != null && (_currentUser.Role == "менеджер" || _currentUser.Role == "администратор");

        public ProductsPage(User? currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            DataContext = this;

            if (IsAdminOrManager)
            {
                OrdersButton.Visibility = System.Windows.Visibility.Visible;
                AddProductButton.Visibility = System.Windows.Visibility.Visible;
                StockSortCombo.SelectedIndex = 0;
            }

            LoadProducts();
        }

        private static string ResolveProductPhotoUri(string? imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
                return "pack://application:,,,/Resource/Icons/picture.png";

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource", "Products", imageName);
            if (File.Exists(path))
                return new Uri(path).AbsoluteUri;

            return $"pack://application:,,,/Resource/Products/{imageName}";
        }

        private void LoadProducts()
        {
            using var db = new DbshopbootsContext();

            var products = db.Products.Select(p => new
                {
                    p.Id,
                    p.SupplierId,
                    p.Name,
                    p.Price,
                    p.Discount,
                    Description = p.Description,
                    CategoryName = p.Category.Name,
                    ManufacturerName = p.Manufacture.Name,
                    SupplierName = p.Supplier.Name,
                    Unit = db.Storages.Where(s => s.ProductId == p.Id).Select(s => s.Unit).FirstOrDefault(),
                    StockCountNullable = db.Storages.Where(s => s.ProductId == p.Id).Sum(s => (int?)s.Count),
                    ImageName = db.Images.Where(i => i.ProductId == p.Id).Select(i => i.Image1).FirstOrDefault()
                })
                .ToList();

            _allProducts = new List<ProductItem>();
            foreach (var b in products)
            {
                _allProducts.Add(new ProductItem
                {
                    Id = b.Id,
                    SupplierId = b.SupplierId,
                    Name = b.Name,
                    Price = b.Price,
                    Discount = b.Discount,
                    Description = b.Description ?? "",
                    CategoryName = b.CategoryName,
                    ManufacturerName = b.ManufacturerName,
                    SupplierName = b.SupplierName,
                    Unit = string.IsNullOrWhiteSpace(b.Unit) ? "шт." : b.Unit!,
                    StockCount = b.StockCountNullable ?? 0,
                    ImageName = b.ImageName,
                    FinalPrice = Math.Round(b.Price * (100 - b.Discount) / 100.0, 2),
                    PhotoUri = ResolveProductPhotoUri(b.ImageName),
                    CategoryAndName = $"{b.CategoryName} | {b.Name}",
                    IsBigDiscount = b.Discount > 15,
                    IsDiscounted = b.Discount > 0
                });
            }

            if (IsAdminOrManager)
            {
                _suppressApply = true;
                try
                {
                    var prevSupplierId = (SupplierFilterCombo.SelectedItem as SupplierListItem)?.Id ?? 0;
                    var supplierItems = new List<SupplierListItem>
                    {
                        new() { Id = 0, Name = "Все поставщики" }
                    };
                    supplierItems.AddRange(db.Suppliers.OrderBy(s => s.Name).Select(s => new SupplierListItem { Id = s.Id, Name = s.Name }));
                    SupplierFilterCombo.ItemsSource = supplierItems;
                    SupplierFilterCombo.SelectedItem = supplierItems.FirstOrDefault(x => x.Id == prevSupplierId) ?? supplierItems[0];
                }
                finally
                {
                    _suppressApply = false;
                }
            }

            ApplyView();
        }

        private void ApplyView()
        {
            IEnumerable<ProductItem> query = _allProducts;

            if (IsAdminOrManager)
            {
                var search = SearchTextBox.Text.Trim();
                if (search.Length > 0)
                {
                    query = query.Where(p =>
                        p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                        || p.CategoryName.Contains(search, StringComparison.OrdinalIgnoreCase)
                        || p.CategoryAndName.Contains(search, StringComparison.OrdinalIgnoreCase)
                        || p.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
            }

            if (IsAdminOrManager && SupplierFilterCombo.SelectedItem is SupplierListItem s && s.Id != 0)
                query = query.Where(p => p.SupplierId == s.Id);

            var list = query.ToList();

            if (IsAdminOrManager && StockSortCombo.SelectedIndex == 1)
                list = list.OrderBy(p => p.StockCount).ToList();
            else if (IsAdminOrManager && StockSortCombo.SelectedIndex == 2)
                list = list.OrderByDescending(p => p.StockCount).ToList();

            ProductsList.ItemsSource = list;
            RecordsCountText.Text = $"{list.Count} записей";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyView();

        private void SupplierFilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_suppressApply)
                ApplyView();
        }

        private void StockSortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_suppressApply)
                ApplyView();
        }

        private void AddProductButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var editWindow = new Windows.ProductEditWindow(null, _currentUser!);
            editWindow.ShowDialog();
            LoadProducts();
        }

        private void EditProductButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int productId)
            {
                using var db = new DbshopbootsContext();
                var product = db.Products.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                     var editWindow = new Windows.ProductEditWindow(product, _currentUser!);
                     editWindow.ShowDialog();
                     LoadProducts();
                }
            }
        }

        private void OrdersButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage(_currentUser));
        }

        private sealed class SupplierListItem
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
        }

        private class ProductItem
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string Name { get; set; } = "";
        public double Price { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string ManufacturerName { get; set; } = "";
        public string SupplierName { get; set; } = "";
        public string Unit { get; set; } = "шт.";
        public int StockCount { get; set; }
        public string? ImageName { get; set; }
        public string PhotoUri { get; set; } = "";
        public double FinalPrice { get; set; }
        public string CategoryAndName { get; set; } = "";
        public bool IsBigDiscount { get; set; }
        public bool IsDiscounted { get; set; }
    }
}
