using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Discord.Commands;
using BotDiscord;

public class Program
{
    public static Task Main() => new Program().MainAsync();

    public async Task MainAsync()
    {
        // creation d'un config.json
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("config.json")
            .Build();

        //creation de l'injecteur de dependance et injection de dependance
        using IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, service) =>
            service
            .AddSingleton(config)
            .AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged,
                AlwaysDownloadUsers = true
            }))
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<Interaction>()
            .AddSingleton(x => new CommandService())
            )
            .Build();

        await DemarrerBot(host);
    }

    public async Task DemarrerBot(IHost _host)
    {
        using IServiceScope serviceScope = _host.Services.CreateScope();
        IServiceProvider serviceProvider = serviceScope.ServiceProvider;

        // recuperer les dependances
        var client = serviceProvider.GetRequiredService<DiscordSocketClient>();
        var sCommande = serviceProvider.GetRequiredService<InteractionService>();
        var config = serviceProvider.GetRequiredService<IConfigurationRoot>();

        await serviceProvider.GetRequiredService<Interaction>().InitAsync();       

        client.Log += (LogMessage _msg) =>
        {
            Console.WriteLine(_msg.Message);
            return Task.CompletedTask;
        };

        sCommande.Log += (LogMessage _msg) =>
        {
            Console.WriteLine(_msg.Message);
            return Task.CompletedTask;
        };

        //event apres lancement
        client.Ready += async () =>
        {
            Console.WriteLine("Pret !");
            try
            {
                // id serveur discord
                // ajoute les commandes dans le serveur discord
                await sCommande.RegisterCommandsToGuildAsync(config.GetValue<ulong>("idServeur"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        };

        // demare le bot
        await client.LoginAsync(TokenType.Bot, config.GetValue<string>("token"));
        await client.StartAsync();

        // -1 pour pas que le bot s'arrete
        await Task.Delay(-1);
    }
}
