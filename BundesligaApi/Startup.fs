namespace BundesligaApi

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection

type Startup () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    member this.ConfigureServices(services: IServiceCollection) =
        services.AddControllers()
        services.AddEntityFrameworkSqlite().AddDbContext<ApiDbContext>()
        |> ignore

    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        app.UseHttpsRedirection()
        app.UseRouting()
        app.UseEndpoints(fun endpoints -> endpoints.MapControllers() |> ignore)
        |> ignore

    member val Configuration : IConfiguration = null with get, set