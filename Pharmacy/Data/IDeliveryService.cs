namespace Pharmacy.Data
{
    public interface IDeliveryService
    {
        void CreatePickupRequest(int productId, int quantity, int warehouseId, int requesterId);
        IEnumerable<PickupRequest> GetPendingPickupRequests();
        void ApprovePickupRequest(int requestId, int warehouseId);
        void RejectPickupRequest(int requestId);
    }
}
