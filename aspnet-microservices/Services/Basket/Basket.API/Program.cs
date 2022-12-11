using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.gRPC.Protos;
using MassTransit;

namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IBasketRepository, BasketRespository>();

            // Redis
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });

            // gRPC
            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
            {
                options.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl"));
            });
            builder.Services.AddScoped<DiscountGrpcService>();

            // MassTransit
            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, rabbitMqConfig) =>
                {
                    rabbitMqConfig.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));
                });
            });

            // Mapper
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}