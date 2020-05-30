namespace BundesligaApi.Controllers

open System.Linq
open Microsoft.AspNetCore.Mvc
open BundesligaApi

[<ApiController>]
[<Route("[controller]")>]
type PlayersController (dbContext : ApiDbContext) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() : Player[] =
        let players = query {
            for player in dbContext.Players do
            select player
        }
        players.ToArray()
