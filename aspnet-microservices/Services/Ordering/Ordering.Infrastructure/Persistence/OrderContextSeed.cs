using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrder());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}",
                    typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrder()
        {
            return new List<Order>
            {
                new Order
                {
                    UserName = "kobura",
                    FirstName = "Ivan",
                    LastName = "Quiroz",
                    EmailAddress = "ivanquiroz@gmail.com",
                    AddressLine = "Torreon",
                    Country = "Mexico",
                    TotalPrice = 350,

                    CardName = "BBVA",
                    CardNumber = "1234 1234 1234 1234",
                    Expiration = "03/30",
                    CVV = "123",
                    PaymentMethod = 1
                }
            };
        }
    }
}
