// https://fsprojects.github.io/FsHttp/Overview.html
#r "nuget: FsHttp"
open FsHttp

// {"fact":"Both humans and cats have identical regions in the brain responsible for emotion.","length":81}
type Fact = { Fact: string; Length: int }

let fact =
    http { GET "https://catfact.ninja/fact" }
    |> Request.send
    |> Response.deserializeJson<Fact>

printfn "A fact about cats is that %A" fact.Fact
