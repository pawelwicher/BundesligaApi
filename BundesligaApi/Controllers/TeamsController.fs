namespace BundesligaApi.Controllers

open System.Linq
open Microsoft.AspNetCore.Mvc
open BundesligaApi

[<ApiController>]
[<Route("[controller]")>]
type TeamsController (dbContext : ApiDbContext) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Get() : Team[] =
        let teams = query {
            for team in dbContext.Teams do
            select team
        }
        teams.ToArray()
