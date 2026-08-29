using LRUCache;

namespace PaymentData
{
    public class Payment(int id, decimal amount, string currency)
    {
        public int Id { get; set; } = id;
        public decimal Amount { get; set; } = amount;
        public string Currency { get; set; } = currency;
    }

    public class PaymentRepository
    {
        //Simulating DB data with in-memory to save time to setup SQL Server
        private readonly Dictionary<int, Payment> _paymentData = new Dictionary<int, Payment>();
        private readonly LRUCache<int, Payment> _cache = new LRUCache<int, Payment>(3);    //** ToDo - use DI  **//

        public PaymentRepository()
        {
            //simulated data
            _paymentData.TryAdd(1, new Payment(1, 100, "INR"));
            _paymentData.TryAdd(2, new Payment(2, 200, "USD"));
            _paymentData.TryAdd(3, new Payment(3, 300, "PLN"));
        }

        public Payment? GetById(int paymentId)
        {
            if (_cache.TryGet(paymentId, out var payment))
                return payment;

            if (_paymentData.TryGetValue(paymentId, out var dbPayment))
            {
                _cache.Put(paymentId, dbPayment);
                return dbPayment;
            }

            return null;                
        }
        public bool Update(int paymentId, decimal amount)
        {
            var found = _paymentData.TryGetValue(paymentId, out var payment);
            if (found)
            {
                payment.Amount = amount;
                _cache.Remove(paymentId);
            }

            return found;
        }
        public bool Delete(int paymentId)
        {
            if (_paymentData.Remove(paymentId))
            {
                _cache.Remove(paymentId);
                return true;
            }
            return false;
        }
    }
}
