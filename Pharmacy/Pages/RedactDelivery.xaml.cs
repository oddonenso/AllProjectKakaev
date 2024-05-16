using Pharmacy.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Pharmacy.Pages
{
    public partial class RedactDelivery : Page
    {
        Connection connection = new Connection();
        private Delivery delivery;
        private DeliveryService deliveryService;
        private PickupRequest pickupRequest;

        public RedactDelivery(Delivery delivery)
        {
            InitializeComponent();
            this.delivery = delivery;
            deliveryService = new DeliveryService(connection);
            pickupRequest = new PickupRequest();

            DeliveryIDTextBlock.Text = delivery.DeliveryID.ToString();
            DeliveryDateTextBlock.Text = delivery.DeliveryDate.ToString();
            PickupRequestIDTextBlock.Text = delivery.PickupRequestID.ToString();
            QuantityTextBlock.Text = delivery.Quantity.ToString();
            PriceTextBlock.Text = delivery.Price.ToString();
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите отклонить заказ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                deliveryService.DeleteDelivery(delivery.DeliveryID);
                MessageBox.Show("Заказ отклонен");
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите одобрить заказ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                deliveryService.AddProductQuantityToPharmacy(delivery.PickupRequestID, delivery.Quantity);
                deliveryService.DeleteDelivery(delivery.DeliveryID);

                MessageBox.Show("Заказ одобрен");
            }
        }
    }
}
