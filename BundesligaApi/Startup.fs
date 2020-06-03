namespace BundesligaApi

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Server.Kestrel.Core;
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open GraphQL
open GraphQL.Server
open GraphQL.Types

type Startup() =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    member this.ConfigureServices(services: IServiceCollection) =
        services.AddEntityFrameworkSqlite().AddDbContext<ApiDbContext>()
        services.AddScoped<ApiSchema>()

        services.AddScoped<IDependencyResolver>(
            fun (serviceProvider : IServiceProvider) -> FuncDependencyResolver(
                fun (t : Type) -> serviceProvider.GetRequiredService(t)) :> IDependencyResolver)

        services.Configure<KestrelServerOptions>(
            fun (options : KestrelServerOptions) -> options.AllowSynchronousIO <- true)

        services.AddGraphQL().AddGraphTypes(ServiceLifetime.Scoped)
        |> ignore

    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        app.UseGraphQL<ApiSchema>()
        |> ignore

    member val Configuration : IConfiguration = null with get, set