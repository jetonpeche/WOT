export type Tank =
{
    Id: number,

    IdTier: number,
    IdTypeTank: number,
    IdStatut: number,
    
    Nom: string,
    EstVisible: boolean,

    // pas dans appel API
    estPosseder: boolean

}