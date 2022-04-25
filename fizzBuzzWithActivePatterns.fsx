open System.Text.RegularExpressions

let (|MultipleOf|_|) divisor value =
    match value with
    | v when v % divisor = 0 -> Some v
    | _ -> None

let print (int, str) = printfn "%i -> %s" int str

let fizzBuzz value =
    match value with
    | MultipleOf 15 _ -> (value, "FizzBuzz")
    | MultipleOf 3 _ -> (value, "Fizz")
    | MultipleOf 5 _ -> (value, "Buzz")
    | _ -> (value, $"{value}")

[ 1..16 ] //
|> List.map fizzBuzz
|> List.iter print
