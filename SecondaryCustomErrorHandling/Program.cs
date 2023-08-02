using System;
using NServiceBus;
using NServiceBus.Faults;
using NServiceBus.Features;
using NServiceBus.Logging;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.SecondaryCustomErrorHandling";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Warn);

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SecondaryCustomErrorHandling");
        busConfiguration.UsePersistence<InMemoryPersistence>();

        var sqlConfig = busConfiguration.UseTransport<SqlServerTransport>();
        sqlConfig.ConnectionString("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NSBRecoverabilitySample;Integrated Security=True;Connect Timeout=30;Encrypt=False;");

        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press [ESC] key to exit");

            while (true)
            {
                var input = Console.ReadKey();

                if (input.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}