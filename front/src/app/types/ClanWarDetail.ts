export type ClanWarDetail =
{
    Id: number,
    Date: string,
    ListePersonne: ClanWarParticipant[]
}

export type ClanWarParticipant =
{
    Id: number,
    Pseudo: string,
    NomTank: string
}