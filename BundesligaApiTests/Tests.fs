namespace BundesligaApiTests

open System.Text
open System.Net.Http

module Tests =

    open Xunit
    open FsUnit.Xunit

    let makeQuery (query : string) : string =
        use httpClient = new HttpClient()
        use request = new StringContent(query, Encoding.UTF8, "application/json")
        let response = httpClient.PostAsync("https://localhost:5001/graphql", request) |> Async.AwaitTask |> Async.RunSynchronously
        let json = response.Content.ReadAsStringAsync() |> Async.AwaitTask |> Async.RunSynchronously
        json

    [<Fact>]
    let ``Get all teams`` () =
        let query = @"
        { 
	        ""query"": ""{
		        teams {
			        id
			        code
			        name
                    players {
                        name
                        goals
                    }
		        }
            }""
        }"
        let json = makeQuery query
        json |> should not' (be NullOrEmptyString)

    [<Fact>]
    let ``Get team by id`` () =
        let query = @"
        { 
	        ""query"": ""{
		        team(id: 1) {
			        id
			        code
			        name
                    players {
                        name
                        goals
                    }
		        }
            }""
        }"
        let json = makeQuery query
        json |> should not' (be NullOrEmptyString)