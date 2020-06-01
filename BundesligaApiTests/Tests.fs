namespace BundesligaApiTests

open System.Linq
open System.Text
open System.Net
open System.Net.Http
open BundesligaApi

module Tests =

    open Xunit
    open FsUnit.Xunit

    [<Fact>]
    let ``Get Teams list test`` () =
        use dbContext = new ApiDbContext()
        let teams = dbContext.Teams.ToList()
        teams |> should haveCount 3

        use httpClient = new HttpClient()
        use request = new StringContent(@"
        { 
	        ""query"": ""{
		        teams {
			        id
			        code
			        name
		        }
            }""
        }", Encoding.UTF8, "application/json")
        let response = httpClient.PostAsync("https://localhost:5001/graphql", request) |> Async.AwaitTask |> Async.RunSynchronously
        let json = response.Content.ReadAsStringAsync() |> Async.AwaitTask |> Async.RunSynchronously

        json |> should not' (be NullOrEmptyString)
        response.StatusCode |> should equal HttpStatusCode.OK