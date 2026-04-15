using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using demoExam.Model;
using demoExam.Windows;

namespace demoExam.Pages;

public partial class OrdersPage : Page
{
    private readonly User? _currentUser;

    public bool IsAdmin => _currentUser != null && IsAdminRole(_currentUser.Role);

    public OrdersPage(User? currentUser)
    {
        InitializeComponent();
        _currentUser = currentUser;
        DataContext = this;
        LoadOrders();
    }

    private void LoadOrders()
    {
        using var db = new DbshopbootsContext();

        var orders = db.Orders
            .Include(o => o.PickupPoint)
            .OrderByDescending(o => o.DateOrder)
            .Select(o => new OrderItem
            {
                Id = o.Id,
                Code = o.Code,
                StatusOrder = o.StatusOrder,
                DateOrder = o.DateOrder,
                DateDelivery = o.DateDelivery,
                PickupAddress = $"{o.PickupPoint.AddressIndex}, {o.PickupPoint.AddressCity}, {o.PickupPoint.AddressStreet}, {o.PickupPoint.AddressNumberHouse}"
            })
            .ToList();

        OrdersList.ItemsSource = orders;
        OrdersList.SelectedItem = null;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (NavigationService?.CanGoBack == true)
        {
            NavigationService.GoBack();
        }
        else
        {
            NavigationService?.Navigate(new ProductsPage(_currentUser));
        }
    }

    private void AddOrderButton_Click(object sender, RoutedEventArgs e)
    {
        if (!IsAdmin || _currentUser == null)
            return;

        var editWindow = new OrderEditWindow(null, _currentUser);
        if (editWindow.ShowDialog() == true)
            LoadOrders();
    }

    private void OrderItemBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (!IsAdmin || _currentUser == null)
            return;

        if (FindParent<Button>(e.OriginalSource as DependencyObject) != null)
            return;

        if (sender is not Border { DataContext: OrderItem selectedOrder })
            return;

        using var db = new DbshopbootsContext();
        var order = db.Orders.FirstOrDefault(o => o.Id == selectedOrder.Id);
        if (order == null)
            return;

        var editWindow = new OrderEditWindow(order, _currentUser);
        if (editWindow.ShowDialog() == true)
            LoadOrders();
    }

    private void DeleteOrderButton_Click(object sender, RoutedEventArgs e)
    {
        if (!IsAdmin || sender is not Button button || button.Tag is not int orderId)
            return;

        var result = MessageBox.Show("Удалить заказ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result != MessageBoxResult.Yes)
            return;

        using var db = new DbshopbootsContext();
        var order = db.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefault(o => o.Id == orderId);

        if (order == null)
            return;

        if (order.OrderDetails.Count > 0)
            db.OrderDetails.RemoveRange(order.OrderDetails);

        db.Orders.Remove(order);
        db.SaveChanges();
        LoadOrders();
    }

    private static bool IsAdminRole(string? role)
    {
        if (string.IsNullOrWhiteSpace(role))
            return false;

        return role.Equals("администратор", StringComparison.OrdinalIgnoreCase)
               || role.Equals("Р°РґРјРёРЅРёСЃС‚СЂР°С‚РѕСЂ", StringComparison.OrdinalIgnoreCase);
    }

    private static T? FindParent<T>(DependencyObject? child) where T : DependencyObject
    {
        while (child != null)
        {
            if (child is T match)
                return match;

            child = VisualTreeHelper.GetParent(child);
        }

        return null;
    }

    private sealed class OrderItem
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string StatusOrder { get; set; } = "";
        public DateOnly DateOrder { get; set; }
        public DateOnly DateDelivery { get; set; }
        public string PickupAddress { get; set; } = "";
    }
}
