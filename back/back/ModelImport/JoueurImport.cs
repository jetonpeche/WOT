﻿namespace back.ModelImport;

public sealed class JoueurImport
{
    /// <summary>
    /// Uniquement pour la modification du joueur
    /// </summary>
    public int? Id { get; set; }
    public required string IdDiscord { get; set; }
    public required string Pseudo { get; set; }
    public required string Mdp { get; set; }
    public bool EstStrateur { get; set; }
    public bool EstAdmin { get; set; }
}
