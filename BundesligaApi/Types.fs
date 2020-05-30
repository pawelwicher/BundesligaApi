namespace BundesligaApi

type Team() =
    member val Id = 0 with get, set
    member val Code = "" with get, set
    member val Name = "" with get, set

type Player() =
    member val Id = 0 with get, set
    member val TeamId = 0 with get, set
    member val Name = "" with get, set
    member val GoalsScored = 0 with get, set