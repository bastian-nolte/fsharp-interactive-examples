// The http client library: https://fsprojects.github.io/FsHttp/Overview.html
// A video about async in f#: https://www.youtube.com/watch?v=eMSZSUbeexc
#r "nuget: FsHttp"
open FsHttp

// {"fact":"Both humans and cats have identical regions in the brain responsible for emotion.","length":81}
type Fact = { Fact: string; Length: int }

let getACatFact index =
    async {
        let rnd = System.Random().Next(1, 9) * 100
        do! Async.Sleep rnd

        let response =
            http { GET "https://catfact.ninja/fact" }
            |> Request.send
            |> Response.deserializeJson<Fact>

        printfn "Internal: %i -> %s" index response.Fact
        return (index, response)
    }



[ 1..10 ]
|> List.map getACatFact
|> Async.Parallel
|> Async.RunSynchronously
|> Seq.iter (fun (i, response) -> printfn "External: The %i fact about cats is that %A" i response.Fact)
