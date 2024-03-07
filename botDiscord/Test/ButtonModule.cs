using NetCord.Rest;
using NetCord.Services.ComponentInteractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botDiscord.Test;

public class ButtonModule : ComponentInteractionModule<ButtonInteractionContext>
{
    [ComponentInteraction("btn")]
    public Task ButtonAsync(string data)
    {
        return RespondAsync(InteractionCallback.Message(new InteractionMessageProperties
        {
            Content = data,
            
        }));
    }
}
