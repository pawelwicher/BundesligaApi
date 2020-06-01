namespace BundesligaApi

open Microsoft.EntityFrameworkCore
open GraphQL
open GraphQL.Types

type TeamType() as this =
    inherit ObjectGraphType<Team>() do
        this.Name <- "Team"
        this.Field(fun x -> x.Id) |> ignore
        this.Field(fun x -> x.Code) |> ignore
        this.Field(fun x -> x.Name) |> ignore

type ApiQuery(dbContext : ApiDbContext) as this =
    inherit ObjectGraphType() do
        this.Field<ListGraphType<TeamType>>("Teams", resolve = (fun _ -> dbContext.Teams.ToArrayAsync() :> obj)) |> ignore

type ApiSchema(resolver : IDependencyResolver) as this =
    inherit Schema(resolver) do
        this.Query <- resolver.Resolve<ApiQuery>()  