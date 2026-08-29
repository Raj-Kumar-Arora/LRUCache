using LRUCache;

namespace PaymentData
{
    public class PaymentProcessor
    {
        private readonly Dictionary<int, decimal> _payments = new();
        private readonly LRUCache<int, decimal> _cache = new LRUCache<int, decimal>(3);

        public string ProcessPayment(
            int paymentId,
            decimal amount,
            string currency,
            string customerType,
            bool isInternational)
        {
            if (!IsPaymentDataValid(paymentId, amount, currency))
                return "Invalid payment";

            var total = CalculateTotalPayment(amount, customerType, isInternational);
            if (total > 10000)
                return "Payment exceeds limit";

            if (!_payments.TryAdd(paymentId, total))
                return "Payment already exists";

            return "Payment processed";
        }
        private bool IsPaymentDataValid(int paymentId, decimal amount, string currency)
        {
            return paymentId > 0 && amount > 0 && !string.IsNullOrWhiteSpace(currency);
        }
        private decimal CalculateTotalPayment(decimal amount, string customerType, bool isInternational)
        {
            decimal fee = 0;

            switch (customerType)
            {
                case "PREMIUM": fee = amount * 0.01m; break;
                case "STANDARD": fee = amount * 0.02m; break;
                default: fee = amount * 0.03m; break;
            }

            if (isInternational)
                fee = fee + 10;

            return amount + fee;
        }

        public decimal GetPaymentAmount(int paymentId)
        {
            if (_cache.TryGet(paymentId, out var amount))
                return amount;

            if(_payments.TryGetValue(paymentId, out var dbAmount)
            {
                _cache.Put(paymentId, dbAmount);
                return dbAmount;
            }

            return 0;
        }
        public bool UpdatePayment(int paymentId, decimal amount)
        {
            if (_payments.TryGetValue(paymentId, out var currentAmount))
            {
                _payments[paymentId] = amount;
                _cache.Remove(paymentId);

                return true;
            }
            return false;
        }
        public bool DeletePayment(int paymentId)
        {
            if (_payments.Remove(paymentId))
            {
                _cache.Remove(paymentId);
                return true;
            }
            return false;
        }
    }
}

