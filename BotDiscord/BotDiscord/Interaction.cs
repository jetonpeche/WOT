using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;

namespace BotDiscord
{
    public class Interaction
    {
        private readonly DiscordSocketClient client;
        private readonly InteractionService commande;
        private readonly IServiceProvider serviceProvider;

        public Interaction(DiscordSocketClient _client, InteractionService _commande, IServiceProvider _serviceProvider)
        {
            client = _client;
            commande = _commande;
            serviceProvider = _serviceProvider;
        }

        public async Task InitAsync()
        {
            await commande.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);

            // slach commande
            client.InteractionCreated += SlashComande;
        }

        private async Task SlashComande(SocketInteraction arg)
        {
            var context = new SocketInteractionContext(client, arg);
            await commande.ExecuteCommandAsync(context, serviceProvider);
        }
    }
}
