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
        private readonly Dictionary<int, Payment> paymentData = new Dictionary<int, Payment>();
        public PaymentRepository()
        {
            //simulated data
            paymentData.TryAdd(1, new Payment(1, 100, "INR"));
            paymentData.TryAdd(2, new Payment(2, 200, "USD"));
            paymentData.TryAdd(3, new Payment(3, 300, "PLN"));
        }

        public Payment? GetById(int paymentId)
        {
            return paymentData.TryGetValue(paymentId, out var payment)
                ? payment
                : null;
        }
        public bool Update(int paymentId, decimal amount)
        {
            var found = paymentData.TryGetValue(paymentId, out var payment);
            if (found)
                payment.Amount = amount;

            return found;
        }
        public bool Delete(int paymentId)
        {
            //var found = paymentData.TryGetValue(paymentId, out var payment);
            //if (found)
            //    paymentData.Remove(paymentId);

            //return found;

            return paymentData.Remove(paymentId);
        }
    }
}
