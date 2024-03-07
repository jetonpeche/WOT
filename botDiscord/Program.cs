using Microsoft.Extensions.Hosting;
using NetCord;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.ComponentInteractions;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.ComponentInteractions;

var builder = Host.CreateDefaultBuilder(args)
    .UseComponentInteractions<ButtonInteraction, ButtonInteractionContext>()
    .UseComponentInteractions<RoleMenuInteraction, RoleMenuInteractionContext>()
    .UseApplicationCommands<SlashCommandInteraction, SlashCommandContext>()
    .UseComponentInteractions<ModalInteraction, ModalSubmitInteractionContext>()
    .UseDiscordGateway();

var host = builder.Build()
    .AddModules(typeof(Program).Assembly)
    .UseGatewayEventHandlers();

await host.RunAsync();
