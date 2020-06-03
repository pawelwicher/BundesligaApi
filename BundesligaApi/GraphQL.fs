namespace BundesligaApi

open System.Linq
open Microsoft.EntityFrameworkCore
open GraphQL
open GraphQL.Types

type PlayerType() as this =
    inherit ObjectGraphType<Player>() do
        this.Field(fun x -> x.Id) |> ignore
        this.Field(fun x -> x.TeamId) |> ignore
        this.Field(fun x -> x.Name) |> ignore
        this.Field(fun x -> x.Goals) |> ignore

type TeamType(dbContext : ApiDbContext) as this =
    inherit ObjectGraphType<Team>() do
        this.Field(fun x -> x.Id) |> ignore
        this.Field(fun x -> x.Code) |> ignore
        this.Field(fun x -> x.Name) |> ignore
        this.Field<ListGraphType<PlayerType>>(
            "players",
            resolve = fun context -> dbContext.Players.Where(fun player -> player.TeamId = context.Source.Id).ToArrayAsync() :> obj
        ) |> ignore

type ApiQuery(dbContext : ApiDbContext) as this =
    inherit ObjectGraphType() do
        this.Field<ListGraphType<TeamType>>(
            "teams",
            resolve = (fun _ -> dbContext.Teams.ToArrayAsync() :> obj)
        ) |> ignore
        this.Field<ListGraphType<PlayerType>>(
            "players",
            resolve = (fun _ -> dbContext.Players.ToArrayAsync() :> obj)
        ) |> ignore
        this.Field<TeamType>(
            "team",
             arguments = QueryArguments(QueryArgument<IntGraphType>(Name = "id")),
             resolve =  fun context -> dbContext.Teams.FirstOrDefaultAsync(fun team -> team.Id = context.GetArgument<int>("id")) :> obj
        ) |> ignore
        this.Field<PlayerType>(
            "player",
             arguments = QueryArguments(QueryArgument<IntGraphType>(Name = "id")),
             resolve =  fun context -> dbContext.Players.FirstOrDefaultAsync(fun player -> player.Id = context.GetArgument<int>("id")) :> obj
        ) |> ignore

type ApiSchema(resolver : IDependencyResolver) as this =
    inherit Schema(resolver) do
        this.Query <- resolver.Resolve<ApiQuery>()  