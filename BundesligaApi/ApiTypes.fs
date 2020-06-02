namespace BundesligaApi

type Player() =
    member val Id = 0 with get, set
    member val TeamId = 0 with get, set
    member val Name = "" with get, set
    member val Goals = 0 with get, set

type Team() =
    member val Id = 0 with get, set
    member val Code = "" with get, set
    member val Name = "" with get, set