using PaymentData;
using System;
using System.Collections.Generic;
using System.Text;

namespace LRUCache.Tests
{
    public class PaymentProcessorTests
    {
        [Fact]
        public void ProcessPayment_ValidPayment_ReturnsProcessed()
        {
            var processor = new PaymentProcessor();

            var result = processor.ProcessPayment(
                1, 100, "INR", "PREMIUM", false);

            Assert.Equal("Payment processed", result);
        }

        [Fact]
        public void GetPaymentAmount_ReturnsProcessedPayment()
        {
            var processor = new PaymentProcessor();

            processor.ProcessPayment(
                1, 100, "INR", "PREMIUM", false);

            var amount = processor.GetPaymentAmount(1);

            // 100 + 1% premium fee
            Assert.Equal(101, amount);
        }

        [Fact]
        public void GetPaymentAmount_MissingPayment_ReturnsZero()
        {
            var processor = new PaymentProcessor();

            var amount = processor.GetPaymentAmount(999);

            Assert.Equal(0, amount);
        }

        [Fact]
        public void UpdatePayment_InvalidatesCache()
        {
            var processor = new PaymentProcessor();

            processor.ProcessPayment(
                1, 100, "INR", "PREMIUM", false);

            // Populate cache with 101
            var originalAmount = processor.GetPaymentAmount(1);
            Assert.Equal(101, originalAmount);

            // Update underlying data
            var result = processor.UpdatePayment(1, 500);

            Assert.True(result);

            // Must not return stale 101 from cache
            var updatedAmount = processor.GetPaymentAmount(1);

            Assert.Equal(500, updatedAmount);
        }

        [Fact]
        public void DeletePayment_InvalidatesCache()
        {
            var processor = new PaymentProcessor();

            processor.ProcessPayment(
                1, 100, "INR", "PREMIUM", false);

            // Populate cache
            var amount = processor.GetPaymentAmount(1);
            Assert.Equal(101, amount);

            // Delete underlying data
            var result = processor.DeletePayment(1);

            Assert.True(result);

            // Must not return stale cached value
            var deletedAmount = processor.GetPaymentAmount(1);

            Assert.Equal(0, deletedAmount);
        }

        [Fact]
        public void UpdatePayment_MissingPayment_ReturnsFalse()
        {
            var processor = new PaymentProcessor();

            var result = processor.UpdatePayment(999, 500);

            Assert.False(result);
        }

        [Fact]
        public void DeletePayment_MissingPayment_ReturnsFalse()
        {
            var processor = new PaymentProcessor();

            var result = processor.DeletePayment(999);

            Assert.False(result);
        }
    }
}
