using Npgsql;

namespace Discount.Grpc.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int ? retry=0)
        {
            int retryForAvailibility = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("Migrating Postgre database.");
                    
                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();
                    
                    using var command = new NpgsqlCommand { Connection = connection };
                    command.CommandText = "Drop table if exists Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"Create table coupon(Id SERIAL PRIMARY KEY,
                                                                ProductName Varchar(24) Not Null,
                                                                Description Text,
                                                                Amount Int)";
                    command.ExecuteNonQuery();

                    command.CommandText = "Insert into coupon (ProductName, Description, Amount) Values ('IPhone X', 'IPhone X Desc', 199)";
                    command.ExecuteNonQuery();

                    command.CommandText = "Insert into coupon (ProductName, Description, Amount) Values ('Samsung 10', 'Samsung 10 Desc', 99)";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migration of Postgre database completed.");

                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occured while migrating the database");
                    if(retryForAvailibility < 50)
                    {
                        retryForAvailibility++;
                        System.Threading.Thread.Sleep(200);
                        MigrateDatabase<TContext>(host, retryForAvailibility);
                    }
                }
                return host;
            }
        }
            
    }
}
