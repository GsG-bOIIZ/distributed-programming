using Microsoft.AspNetCore.Hosting.Server;
using NATS.Client;
using StackExchange.Redis;
using System.Text;

using Valuator;

public class Program
{
    private static readonly IConnection _natsConnection = new ConnectionFactory().CreateConnection("127.0.0.1:4222");


    static void Main(string[] args)
    {
        var subscription = _natsConnection.SubscribeAsync("RankCalculator", async (sender, args) =>
        {
            IDatabase _redisDatabase = ConnectionMultiplexer.Connect($"{RedisService.SERVER}:{RedisService.PORT}").GetDatabase();

            var messageBytes = args.Message.Data;
            string id = Encoding.UTF8.GetString(messageBytes);
            string? text = _redisDatabase.StringGet("TEXT-" + id);

            double rank = CalculateRank(text);

            await _redisDatabase.StringSetAsync("RANK-" + id, rank.ToString());

            Console.WriteLine($"Обработан текст: {id}, Ранг: {rank}");
        });

        Console.WriteLine("Calculate rank...");
        Console.ReadLine();
    }

    private static double CalculateRank(string text)
    {
        return (text.Count(symbol => !Char.IsLetter(symbol))) / (double)text.Length;
    }
}

