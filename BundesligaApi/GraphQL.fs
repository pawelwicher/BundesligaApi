namespace BundesligaApi

open System.Linq
open Microsoft.EntityFrameworkCore
open GraphQL
open GraphQL.Types

type PlayerType() as this =
    inherit ObjectGraphType<Player>() do
        this.Name <- "Player"
        this.Field(fun x -> x.Id) |> ignore
        this.Field(fun x -> x.TeamId) |> ignore
        this.Field(fun x -> x.Name) |> ignore
        this.Field(fun x -> x.Goals) |> ignore


type TeamType(dbContext : ApiDbContext) as this =
    inherit ObjectGraphType<Team>() do
        this.Name <- "Team"
        this.Field(fun x -> x.Id) |> ignore
        this.Field(fun x -> x.Code) |> ignore
        this.Field(fun x -> x.Name) |> ignore
        this.Field<ListGraphType<PlayerType>>(
            "players",
            resolve = fun context -> dbContext.Players.Where(fun player -> player.TeamId = context.Source.Id).ToArrayAsync() :> obj
        ) |> ignore

type ApiQuery(dbContext : ApiDbContext) as this =
    inherit ObjectGraphType() do
        this.Field<ListGraphType<TeamType>>("Teams", resolve = (fun _ -> dbContext.Teams.ToArrayAsync() :> obj)) |> ignore

type ApiSchema(resolver : IDependencyResolver) as this =
    inherit Schema(resolver) do
        this.Query <- resolver.Resolve<ApiQuery>()  