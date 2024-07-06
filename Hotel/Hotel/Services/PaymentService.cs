using Stripe;
using System.Threading.Tasks;

namespace Hotel.Services
{
    public class PaymentService
    {
        public async Task<Refund> RefundPaymentAsync(string chargeId, decimal amount)
        {
            var refundOptions = new RefundCreateOptions
            {
                Charge = chargeId,
                Amount = (long)(amount * 100)
            };

            var refundService = new RefundService();
            var refund = await refundService.CreateAsync(refundOptions);

            return refund;
        }
    }
}
