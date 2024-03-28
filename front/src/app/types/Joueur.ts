export type Joueur =
{
    Id: number,
    IdDiscord: string,
    Pseudo: string,
    Jwt?: string,
    EstAdmin: boolean,
    EstStrateur: boolean,
    EstActiver: boolean,
    ListeIdTank:number[]
}