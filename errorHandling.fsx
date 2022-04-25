open System.IO
open System.Security
// From https://fsharpforfunandprofit.com/posts/correctness-exhaustive-pattern-matching/#exhaustive-pattern-matching-as-an-error-handling-technique

// Implement a file handler
type Result<'a, 'b> =
    | Success of 'a
    | Failure of 'b

type FileErrorReason =
    | FileNotFound of string
    | UnautorizedAccess of string * System.Exception

let performActionOnFile action filePath =
    try
        use sr = new StreamReader(filePath: string)
        let result = action sr
        Success(result)
    with
    | :? FileNotFoundException -> Failure(FileNotFound filePath)
    | :? SecurityException as e -> Failure(UnautorizedAccess(filePath, e))


// Some usage
let middleLayerDo action filePath =
    let fileResult = performActionOnFile action filePath
    // do something
    fileResult

let topLayerDo action filePath =
    let fileResult = middleLayerDo action filePath
    fileResult

let doSomethingWithTheFile filePath =
    let fileResult = topLayerDo (fun fs -> fs.ReadLine()) filePath

    match fileResult with
    | Success result -> printfn "The first line of the file is \"%s\"." result
    | Failure reason ->
        match reason with
        | FileNotFound file -> printfn "File not found: %s" file
        | UnautorizedAccess (file, _) -> printfn "You don't have access to the file: %s" file
