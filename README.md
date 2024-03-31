# WOT 

Organiser les clan war  
Login: jetonpeche (admin)
Mdp: salut

# Installation front

## Prérequis

- Télécharger [nodeJS](https://nodejs.org/en/download) (dernière LTS)
- Dans le cmd taper `npm i -g @angular/cli` pour installer Angular

## Démarer le projet

- Deplacer vous dans le dossier `front` avec le cmd
- Executer la commande `npm i`
- Executer la commande `ng serve -o` (le projet sa s'ouvrir dans le navigateur par défaut)

# Installation back

- Telecharger visual studio 2022 (community) [ici](https://visualstudio.microsoft.com/fr/downloads/)
- Une fois installer, clicker sur le bouton `modifier` puis cocher `Développement web et ASP.NET` et valider en bas à droite

## Démarer le projet

- Dans le dossier `back` doublic click sur `back.sln` se qui va ouvrir visual studio 2022
  OU
  lancer visual studio 2022 et clicker sur `ouvrir un projet ou une solution` puis aller chercher `back.sln`
- Clicker sur le bouton play en haut pour lancer le back

## Installation Bdd

### Sql Server existant
- Si vous avez sql server créer la bdd `WOT` et copier coller le `script.sql`
- Oublier pas de changer dans `appsettings.json` la chaine de connexion (data source) par la votre
