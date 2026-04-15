using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using demoExam.Model;

namespace demoExam.Windows
{
    public partial class ProductEditWindow : Window
    {
        private Product _product;
        private bool _isEditMode;
        private string? _newImagePath;
        private User _currentUser;

        public ProductEditWindow(Product? product, User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _product = product ?? new Product();
            _isEditMode = product != null;

            LoadComboBoxes();

            if (_isEditMode)
            {
                LoadProductData();
                Title = "Редактирование товара";
                DeleteButton.Visibility = Visibility.Visible;
                IdTextBox.Text = _product.Id.ToString();
            }
            else
            {
                Title = "Добавление товара";
                DeleteButton.Visibility = Visibility.Collapsed;
                IdTextBox.Text = "Автоматически";
            }
        }

        private void LoadComboBoxes()
        {
            using var db = new DbshopbootsContext();
            CategoryComboBox.ItemsSource = db.Categories.ToList();
            ManufacturerComboBox.ItemsSource = db.Manufacturers.ToList();
            SupplierComboBox.ItemsSource = db.Suppliers.ToList();
        }

        private void LoadProductData()
        {
            using var db = new DbshopbootsContext();
            
            var p = db.Products
                .Include(p => p.Images)
                .Include(p => p.Storages)
                .FirstOrDefault(x => x.Id == _product.Id);

            if (p == null) return;
            _product = p;

            NameTextBox.Text = _product.Name;
            DescriptionTextBox.Text = _product.Description;
            PriceTextBox.Text = _product.Price.ToString("F2");
            DiscountTextBox.Text = _product.Discount.ToString();
            
            CategoryComboBox.SelectedValue = _product.CategoryId;
            ManufacturerComboBox.SelectedValue = _product.ManufactureId;
            SupplierComboBox.SelectedValue = _product.SupplierId;

            var storage = _product.Storages.FirstOrDefault();
            if (storage != null)
            {
                StockCountTextBox.Text = storage.Count.ToString();
                UnitTextBox.Text = storage.Unit;
            }
            else
            {
                StockCountTextBox.Text = "0";
                UnitTextBox.Text = "шт.";
            }

            var img = _product.Images.FirstOrDefault();
            if (img != null && !string.IsNullOrEmpty(img.Image1))
            {
                LoadImageFromFile(img.Image1);
            }
        }

        private void LoadImageFromFile(string imageName)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource", "Products", imageName);
                
                if (!File.Exists(path))
                {
                     
                }

                if (File.Exists(path))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; 
                    bitmap.UriSource = new Uri(path);
                    bitmap.EndInit();
                    ProductImage.Source = bitmap;
                }
                else
                {
                    try
                    {
                        ProductImage.Source = new BitmapImage(new Uri($"pack://application:,,,/Resource/Products/{imageName}"));
                    }
                    catch
                    {
                         ProductImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resource/Icons/picture.png"));
                    }
                }
            }
            catch
            {
                 ProductImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resource/Icons/picture.png"));
            }
        }

        private void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                _newImagePath = openFileDialog.FileName;
                
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_newImagePath);
                bitmap.EndInit();
                
                if (bitmap.PixelWidth > 300 || bitmap.PixelHeight > 200)
                {
                    MessageBox.Show("Размер изображения не должен превышать 300x200 пикселей!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _newImagePath = null;
                    return;
                }

                ProductImage.Source = bitmap;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Введите наименование товара");
                return;
            }
            if (CategoryComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }
            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Некорректная цена");
                return;
            }
            if (!int.TryParse(StockCountTextBox.Text, out int count) || count < 0)
            {
                MessageBox.Show("Некорректное количество");
                return;
            }
             if (!byte.TryParse(DiscountTextBox.Text, out byte discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Некорректная скидка (0-100)");
                return;
            }

            try
            {
                using var db = new DbshopbootsContext();
                Product productToSave;

                if (_isEditMode)
                {
                    productToSave = db.Products
                        .Include(p => p.Images)
                        .Include(p => p.Storages)
                        .FirstOrDefault(p => p.Id == _product.Id);
                }
                else
                {
                    productToSave = new Product();
                    db.Products.Add(productToSave);
                }

                productToSave.Name = NameTextBox.Text;
                productToSave.Description = DescriptionTextBox.Text;
                productToSave.CategoryId = (int)CategoryComboBox.SelectedValue;
                productToSave.ManufactureId = (int)ManufacturerComboBox.SelectedValue;
                productToSave.SupplierId = (int)SupplierComboBox.SelectedValue;
                productToSave.Price = (double)price;
                productToSave.Discount = discount; 

                if (_newImagePath != null)
                {
                    string destFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource", "Products");
                    if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(_newImagePath);
                    string destPath = Path.Combine(destFolder, fileName);

                    File.Copy(_newImagePath, destPath, true);

                    var oldImg = productToSave.Images.FirstOrDefault();
                    if (oldImg != null)
                    {
                        oldImg.Image1 = fileName;
                    }
                    else
                    {
                        // ProductId пока 0 у нового товара — задаём связь через коллекцию, EF проставит id при SaveChanges.
                        productToSave.Images.Add(new demoExam.Model.Image { Image1 = fileName });
                    }
                }

                var storage = productToSave.Storages.FirstOrDefault();
                if (storage == null)
                {
                    storage = new Storage { Product = productToSave, Unit = UnitTextBox.Text };
                    productToSave.Storages.Add(storage);
                }
                storage.Count = count;
                storage.Unit = UnitTextBox.Text;

                db.SaveChanges();
                
                MessageBox.Show("Товар успешно сохранен");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
             if (MessageBox.Show("Вы уверены, что хотите удалить этот товар?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
             {
                 using var db = new DbshopbootsContext();
                 var product = db.Products.Include(p => p.OrderDetails).FirstOrDefault(p => p.Id == _product.Id);

                 if (product != null)
                 {
                     if (product.OrderDetails.Any())
                     {
                         MessageBox.Show("Нельзя удалить товар, который присутствует в заказах!");
                         return;
                     }

                     db.Storages.RemoveRange(db.Storages.Where(s => s.ProductId == product.Id));
                     db.Images.RemoveRange(db.Images.Where(i => i.ProductId == product.Id));
                     db.Products.Remove(product);
                     
                     db.SaveChanges();
                     MessageBox.Show("Товар удален");
                     Close();
                 }
             }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
             e.Handled = !e.Text.All(c => char.IsDigit(c) || c == ',' || c == '.');
        }
    }
}
