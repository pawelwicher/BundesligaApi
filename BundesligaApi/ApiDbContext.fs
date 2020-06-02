namespace BundesligaApi

open Microsoft.EntityFrameworkCore

type ApiDbContext() =
    inherit DbContext()

    [<DefaultValue>]
    val mutable private players : DbSet<Player>
    member this.Players with get() = this.players and set v = this.players <- v

    [<DefaultValue>]
    val mutable private teams : DbSet<Team>
    member this.Teams with get() = this.teams and set v = this.teams <- v

    override this.OnConfiguring optionsBuilder =
        optionsBuilder
            .UseSqlite("Data Source=DB.db") |> ignore
