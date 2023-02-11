import { Joueur } from "src/app/types/Joueur";
import { StatutTank } from "src/app/types/StatutTank";
import { Tier } from "src/app/types/Tier";
import { TypeTank } from "src/app/types/TypeTank";

export var environment = {
    urlApi: "http://localhost:5086",

    infoJoueur: null as Joueur,

    listeTier: [
      { Id: 1, Nom: "Tier 6" },
      { Id: 2, Nom: "Tier 8"},
      { Id: 3, Nom: "Tier 10"}
    ] as Tier[],

    listeTypeTank: [
      { Id: 1, Nom: "Léger", Image: "iconLeger.png"},
      { Id: 2, Nom: "Medium", Image: "iconMedium.png" },
      { Id: 3, Nom: "Lourd", Image: "iconLourd.png"},
      { Id: 4, Nom: "Chasseur de char", Image: "iconTd.png" },
      { Id: 5, Nom: "Artillerie", Image: "iconArty.png"}
    ] as TypeTank[],

    listeStatutTank: [
      { Id: 1, Nom: "Méta" },
      { Id: 2, Nom: "Accepté" },
      { Id: 3, Nom: "Toléré" }
    ] as StatutTank[],

    // contient pas que des espaces
    regexVide: /^(\s+\S+\s*)*(?!\s).*$/,

    tailleEcran375: 375
  };