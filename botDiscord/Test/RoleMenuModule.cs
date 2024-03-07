using NetCord.Rest;
using NetCord.Services.ComponentInteractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botDiscord.Test;

public class RoleMenuModule : ComponentInteractionModule<RoleMenuInteractionContext>
{
    [ComponentInteraction("menu")]
    public Task MenuAsync()
    {
        return RespondAsync(InteractionCallback.Message($"You selected: {string.Join(", ", Context.SelectedRoles)}"));
    }
}
