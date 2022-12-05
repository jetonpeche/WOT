using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotDiscord.Commande
{
    public class Commande : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("100", "ping pong !")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("salut !");
        }
    }
}
