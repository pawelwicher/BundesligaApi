namespace BundesligaApi.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open BundesligaApi

[<ApiController>]
[<Route("[controller]")>]
type TeamsController (dbContext : ApiDbContext) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() =
        async {
            let teams = query {
                for team in dbContext.Teams do
                select team
            }
            let! result = teams.ToArrayAsync() |> Async.AwaitTask
            return result
        } |> Async.StartAsTask