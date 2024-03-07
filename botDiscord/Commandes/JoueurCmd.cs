using botDiscord.Enums;
using BotDiscord.Models;
using BotDiscord.ModelsExport;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace botDiscord.Commandes;

[SlashCommand("joueur", "commande joueur")]
public class JoueurCmd: ApplicationCommandModule<SlashCommandContext>
{
    [SubSlashCommand("lister", "Lister les joueurs")]
    public async Task Lister([SlashCommandParameter(Name = "role_joueur")] ERoleJoueur _roleJoueur)
    {
        Joueur[]? tabJoueur = await ApiService.GetAsync(EApiType.joueur, 
                                                        $"lister/{(int)_roleJoueur}", 
                                                        JoueurContext.Default.JoueurArray);

        if (tabJoueur is null)
        { 
            await RespondAsync(InteractionCallback.Message("Erreur réseau"));
            return;
        }
        else if (tabJoueur.Length == 0)
        {
            await RespondAsync(InteractionCallback.Message("Aucun joueur"));
            return;
        }

        List<EmbedFieldProperties> liste = new();

        foreach (var element in tabJoueur)
        {
            liste.Add(new()
            {
                Name = element.Pseudo,
                Value = $"Strateur: {(element.EstStrateur ? "Oui" : "Non")} | Admin: {(element.EstAdmin ? "Oui" : "Non")}"
            });
        }

        EmbedProperties embed = new()
        {
            Title = "Liste des joueurs",
            Color = new Color(255, 127, 0),
            Fields = liste
        };

        await RespondAsync(InteractionCallback.Message(new() { Embeds = [embed] }));
    }

    [SubSlashCommand("inscription", "s'inscrire")]
    public async Task Ajouter([SlashCommandParameter(Name = "pseudo")] string _pseudo)
    {
        var reponse = await ApiService.PostAsync(EApiType.joueur, "ajouter", new JoueurExport
        {
            IdDiscord = Context.User.Id.ToString(),
            Pseudo = _pseudo,
            EstStrateur = false,
            EstAdmin = false
        }, JoueurExportContext.Default.JoueurExport);

        if (reponse is null)
            await RespondAsync(InteractionCallback.Message("Erreur réseau"));

        else if (reponse.IsSuccessStatusCode)
            await RespondAsync(InteractionCallback.Message("Tu as été ajouté"));

        else
        {
            string msg = await reponse.Content.ReadAsStringAsync();
            await RespondAsync(InteractionCallback.Message(msg.Replace('"', ' ')));
        }
    }

    [SubSlashCommand("inscrire", "Inscrire un nouveau joueur")]
    public async Task Ajouter([SlashCommandParameter(Name = "joueur")] User _joueur,
                             [SlashCommandParameter(Name = "pseudo")] string _pseudo,
                             [SlashCommandParameter(Name = "est_un_strateur")] bool _estStrateur,
                             [SlashCommandParameter(Name = "est_un_admin")] bool _estAdmin)
    {
        if(_joueur.IsBot)
            await RespondAsync(InteractionCallback.Message("Tu ne peux pas inscrire un bot"));

        var reponse = await ApiService.PostAsync(EApiType.joueur, "ajouter", new JoueurExport
        {
            IdDiscord = _joueur.Id.ToString(),
            Pseudo = _pseudo,
            EstStrateur = _estStrateur,
            EstAdmin = _estAdmin
        }, JoueurExportContext.Default.JoueurExport);

        if (reponse is null)
            await RespondAsync(InteractionCallback.Message("Erreur réseau"));

        else if (reponse.IsSuccessStatusCode)
            await RespondAsync(InteractionCallback.Message("Le joueur a été ajouté"));
        
        else
            await RespondAsync(InteractionCallback.Message($"Le joueur {_joueur.Username} existe déjà"));
    }

    [SubSlashCommand("supprimer", "Supprime le joueur choisi")]
    public async Task Supprimer([SlashCommandParameter(Name = "joueur")] User _joueur)
    {
        if (_joueur.IsBot)
            await RespondAsync(InteractionCallback.Message("Tu ne peux pas supprimer un bot"));

        await SupprimerAsync(_joueur.Id);
    }

    [SubSlashCommand("supprime_moi", "Supprime le joueur qui éxecute la commande de la base de donnée")]
    public async Task Supprimer() => await SupprimerAsync(Context.User.Id);

    private async Task SupprimerAsync(ulong _idDiscord)
    {
        var reponse = await ApiService.DeleteAsync(EApiType.joueur, $"supprimer/{_idDiscord}");

        if (reponse is null)
            await RespondAsync(InteractionCallback.Message("Erreur de suppression"));

        else if (reponse.IsSuccessStatusCode)
            await RespondAsync(InteractionCallback.Message($"Le joueur a été supprimé {new EmojiProperties("👋").Unicode}"));

        else
        {
            string msg = await reponse.Content.ReadAsStringAsync();
            await RespondAsync(InteractionCallback.Message(msg.Replace('"', ' ')));
        }
    }

    [SubSlashCommand("test", "a")]
    public async Task Test()
    {
        EmbedProperties embed = new()
        {
            Title = "Titre",
            Fields = [new EmbedFieldProperties { Name = "", Value = "ligne 1" }]
        };        

        ButtonProperties button = new("btn:salut", "click", ButtonStyle.Primary);

        await RespondAsync(InteractionCallback.Message(new InteractionMessageProperties
        {
            Embeds = [embed],
            Components = [new ActionRowProperties([button])]
            
        }));

    }

    [SubSlashCommand("test2", "liste des roles du discord")]
    public async Task Test2()
    {
        RoleMenuProperties roleMenu = new("menu");

        await RespondAsync(InteractionCallback.Message(new InteractionMessageProperties
        {
            Components = [roleMenu]
        }));
    }

    [SubSlashCommand("test3", "AA")]
    public async Task Test3()
    {
        TextInputProperties input = new("nom", TextInputStyle.Short, "Nom");
        
        ModalProperties modal = new("modal", "titre", [input]);
    }

}
