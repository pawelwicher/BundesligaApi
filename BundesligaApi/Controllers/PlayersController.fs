namespace BundesligaApi.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open BundesligaApi

[<ApiController>]
[<Route("[controller]")>]
type PlayersController (dbContext : ApiDbContext) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        async {
            let players = query {
                for player in dbContext.Players do
                select player
            }
            let! result = players.ToArrayAsync() |> Async.AwaitTask
            return result
        } |> Async.StartAsTask