using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using demoExam.Model;

namespace demoExam.Windows;

public partial class OrderEditWindow : Window
{
    private readonly int? _orderId;
    private readonly int _userId;

    public OrderEditWindow(Order? order, User currentUser)
    {
        InitializeComponent();

        _orderId = order?.Id;
        _userId = order?.UserId ?? currentUser.Id;

        LoadPickupPoints();
        LoadStatuses();

        DateOrderPicker.SelectedDate = DateTime.Today;
        DateDeliveryPicker.SelectedDate = DateTime.Today;

        if (order != null)
            FillForm(order);
    }

    private void LoadPickupPoints()
    {
        using var db = new DbshopbootsContext();
        var pickupPoints = db.PickupPoints
            .AsEnumerable()
            .Select(p => new PickupPointItem
            {
                Id = p.Id,
                Address = $"{p.AddressIndex}, {p.AddressCity}, {p.AddressStreet}, {p.AddressNumberHouse}"
            })
            .OrderBy(p => p.Address)
            .ToList();

        PickupPointComboBox.ItemsSource = pickupPoints;
    }

    private void LoadStatuses()
    {
        using var db = new DbshopbootsContext();
        var statuses = db.Orders
            .Select(o => o.StatusOrder)
            .Distinct()
            .ToList();

        AddStatusIfMissing(statuses, "Новый");
        AddStatusIfMissing(statuses, "Оформлен");
        AddStatusIfMissing(statuses, "Завершен");

        StatusComboBox.ItemsSource = statuses.OrderBy(s => s).ToList();
        if (StatusComboBox.Items.Count > 0)
            StatusComboBox.SelectedIndex = 0;
    }

    private static void AddStatusIfMissing(ICollection<string> statuses, string status)
    {
        if (!statuses.Any(s => s.Equals(status, StringComparison.OrdinalIgnoreCase)))
            statuses.Add(status);
    }

    private void FillForm(Order order)
    {
        CodeTextBox.Text = order.Code.ToString();
        StatusComboBox.SelectedItem = StatusComboBox.Items.Cast<string>()
            .FirstOrDefault(s => s.Equals(order.StatusOrder, StringComparison.OrdinalIgnoreCase))
            ?? order.StatusOrder;
        PickupPointComboBox.SelectedItem = PickupPointComboBox.Items.Cast<PickupPointItem>()
            .FirstOrDefault(p => p.Id == order.PickupPointId);
        DateOrderPicker.SelectedDate = order.DateOrder.ToDateTime(TimeOnly.MinValue);
        DateDeliveryPicker.SelectedDate = order.DateDelivery.ToDateTime(TimeOnly.MinValue);
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        ErrorTextBlock.Text = "";

        if (!int.TryParse(CodeTextBox.Text.Trim(), out var code))
        {
            ErrorTextBlock.Text = "Введите артикул числом.";
            return;
        }

        if (StatusComboBox.SelectedItem is not string status)
        {
            ErrorTextBlock.Text = "Выберите статус заказа.";
            return;
        }

        if (PickupPointComboBox.SelectedItem is not PickupPointItem pickupPoint)
        {
            ErrorTextBlock.Text = "Выберите адрес пункта выдачи.";
            return;
        }

        if (DateOrderPicker.SelectedDate == null || DateDeliveryPicker.SelectedDate == null)
        {
            ErrorTextBlock.Text = "Выберите дату заказа и дату выдачи.";
            return;
        }

        using var db = new DbshopbootsContext();
        Order order;

        if (_orderId.HasValue)
        {
            order = db.Orders.First(o => o.Id == _orderId.Value);
        }
        else
        {
            order = new Order();
            db.Orders.Add(order);
        }

        order.Code = code;
        order.StatusOrder = status;
        order.PickupPointId = pickupPoint.Id;
        order.DateOrder = DateOnly.FromDateTime(DateOrderPicker.SelectedDate.Value);
        order.DateDelivery = DateOnly.FromDateTime(DateDeliveryPicker.SelectedDate.Value);
        order.UserId = _userId;

        db.SaveChanges();
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private sealed class PickupPointItem
    {
        public int Id { get; set; }
        public string Address { get; set; } = "";
    }
}
