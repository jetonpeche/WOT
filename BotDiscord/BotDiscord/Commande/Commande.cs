using BotDiscord.Models;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotDiscord.Commande
{
    public class Commande : InteractionModuleBase<SocketInteractionContext>
    {        
        [SlashCommand("lister_tank_joueur", "Liste les tanks du joueur")]
        public async Task Ping(SocketUser user)
        {
            List<Tank> liste = await ApiService.GetAsync<List<Tank>>(EApiType.tank, $"listerViaDiscord/{user.Id}");

            await Context.Channel.SendMessageAsync(liste.Count.ToString());
        }
    }
}
