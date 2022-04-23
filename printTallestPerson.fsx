open System

type NameAndSize = { Name: string; Size: int }

let maxByOrNone list maxByFn =
    match list with
    | [] -> None
    | _ -> Some(list |> List.maxBy maxByFn)

let maxPersonBySize list =
    maxByOrNone list (fun item -> item.Size)

let printPerson person =
    match person with
    | Some person -> printfn "The tallest person is %s with a size of %d." person.Name person.Size
    | None -> printfn "No Person found."

let people =
    [ { Name = "Alice"; Size = 18 }
      { Name = "Bob"; Size = 1 }
      { Name = "Carol"; Size = 12 }
      { Name = "David"; Size = 5 } ]

people |> maxPersonBySize |> printPerson
[] |> maxPersonBySize |> printPerson
