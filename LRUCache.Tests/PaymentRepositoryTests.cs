using PaymentData;

namespace LRUCache.Tests
{
    public class PaymentRepositoryTests
    {
        [Fact]
        public void GetById_ExistingPayment_ReturnsPayment()
        {
            var repository = new PaymentRepository();

            var payment = repository.GetById(1);

            Assert.NotNull(payment);
            Assert.Equal(1, payment.Id);
            Assert.Equal(100, payment.Amount);
            Assert.Equal("INR", payment.Currency);
        }

        [Fact]
        public void GetById_MissingPayment_ReturnsNull()
        {
            var repository = new PaymentRepository();

            var payment = repository.GetById(999);

            Assert.Null(payment);
        }

        [Fact]
        public void Update_ExistingPayment_UpdatesValue()
        {
            var repository = new PaymentRepository();

            var result = repository.Update(1, 500);
            var payment = repository.GetById(1);

            Assert.True(result);
            Assert.NotNull(payment);
            Assert.Equal(500, payment.Amount);
        }

        [Fact]
        public void Update_MissingPayment_ReturnsFalse()
        {
            var repository = new PaymentRepository();

            var result = repository.Update(999, 500);

            Assert.False(result);
        }

        [Fact]
        public void Delete_ExistingPayment_RemovesPayment()
        {
            var repository = new PaymentRepository();

            var result = repository.Delete(1);
            var payment = repository.GetById(1);

            Assert.True(result);
            Assert.Null(payment);
        }

        [Fact]
        public void Delete_MissingPayment_ReturnsFalse()
        {
            var repository = new PaymentRepository();

            var result = repository.Delete(999);

            Assert.False(result);
        }

        [Fact]
        public void Update_InvalidatesCache()
        {
            var repository = new PaymentRepository();

            // Populate cache with old value
            var original = repository.GetById(1);
            Assert.Equal(100, original!.Amount);

            // Update database and invalidate cache
            repository.Update(1, 500);

            // Should get new value
            var updated = repository.GetById(1);

            Assert.Equal(500, updated!.Amount);
        }

        [Fact]
        public void Delete_InvalidatesCache()
        {
            var repository = new PaymentRepository();

            // Populate cache
            var payment = repository.GetById(1);
            Assert.NotNull(payment);

            // Delete from DB and cache
            repository.Delete(1);

            // Must not get stale cached payment
            var result = repository.GetById(1);

            Assert.Null(result);
        }
    }
}