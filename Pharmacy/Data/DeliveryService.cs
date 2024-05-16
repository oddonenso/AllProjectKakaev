using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacy.Data
{
    public class DeliveryService : IDeliveryService
    {
        private readonly Connection _context;

        public DeliveryService(Connection context)
        {
            _context = context;
        }

        public void CreatePickupRequest(int productId, int quantity, int warehouseId, int requesterId)
        {
            try
            {
                var request = new PickupRequest
                {
                    ProductID = productId,
                    Quantity = quantity,
                    RequestDate = DateTime.UtcNow,
                    RequesterID = requesterId,
                    Status = "Pending",
                    WarehouseID = warehouseId // Добавляем присвоение warehouseId
                };

                _context.PickupRequest.Add(request);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the pickup request.", ex);
            }
        }



        public IEnumerable<PickupRequest> GetPendingPickupRequests()
        {
            return _context.PickupRequest.Where(r => r.Status == "Pending").ToList();
        }

        public void ApprovePickupRequest(int requestId, int warehouseId)
        {
            var request = _context.PickupRequest.Find(requestId);
            if (request != null)
            {
                request.Status = "Approved";
                _context.SaveChanges();
            }
        }

        public void RejectPickupRequest(int requestId)
        {
            var request = _context.PickupRequest.Find(requestId);
            if (request != null)
            {
                request.Status = "Rejected";
                _context.SaveChanges();
            }
        }
       public void DeleteDelivery(int deliveryId)
        {
            var delivery = _context.Delivery.Find(deliveryId);
            if (delivery !=null)
            {
                var pickupRequest = _context.PickupRequest.Find(delivery.PickupRequestID);

                if (pickupRequest!=null)
                {
                    _context.PickupRequest.Remove(pickupRequest);
                }
                _context.Delivery.Remove(delivery);
                _context.SaveChanges(); 
            }
           
        }


        public void AddProductQuantityToPharmacy(int pickupRequsstId, int quantity)
        {
            var pickupRequest = _context.PickupRequest.Find(pickupRequsstId);
            if (pickupRequest != null)
            {
                var product = _context.Product.Find(pickupRequest.ProductID);
                if (product != null)
                {
                    product.Quantity += quantity;
                    _context.SaveChanges();
                }
            }
        }

    }
}
