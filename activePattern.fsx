let (|Double|_|) str =
    match System.Double.TryParse(str: string) with
    | (true, int) -> Some(int)
    | _ -> None


let parse str =
    match str with
    | Double d -> printfn "The value is an double '%f'" d
    | _ -> printfn "The value '%s' is something else" str

parse "1.156e6"
parse "x1.156e6"
