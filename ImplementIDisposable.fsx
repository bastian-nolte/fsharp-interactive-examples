open System

let createADisposableResource name =
    { new IDisposable with
        member this.Dispose() = printfn "%s disposed" name } // Clean up before the object is collected.

let doSomethingImportant i =
    use res = createADisposableResource i
    printfn "Init disposable resource %s" i


[ 1..100 ]
|> Seq.map (fun i -> $"{i}")
|> Seq.iter doSomethingImportant
