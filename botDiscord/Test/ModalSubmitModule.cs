using NetCord.Rest;
using NetCord.Services.ComponentInteractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botDiscord.Test;
public class ModalSubmitModule : ComponentInteractionModule<ModalSubmitInteractionContext>
{
    [ComponentInteraction("modal")]
    public Task ModalAsync()
    {
        return RespondAsync(InteractionCallback.Message(string.Join('\n', Context.Components.Select(c => $"{c.CustomId}: {c.Value}"))));
    }
}
