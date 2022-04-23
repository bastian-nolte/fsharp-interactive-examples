open System

let incrementByOne n = n + 1

let digits integer =
    match integer with
    | 0 -> 0
    | _ ->
        integer
        |> float
        |> log10
        |> truncate
        |> int
        |> incrementByOne

let symbolTriple digits =
    let symbols = [ "I"; "V"; "X"; "L"; "C"; "D"; "M" ]
    let index = (digits - 1) * 2
    let outOfRange d = d > 4 || d < 1
    let lastInRange d = d = 4

    match digits with
    | d when d |> outOfRange -> ("", "", "")
    | d when d |> lastInRange -> (symbols[index], "", "")
    | _ -> (symbols[index], symbols[index + 1], symbols[index + 2])

let rec toRoman n =
    let outOfRange n = n < 0 || n > 3999

    match n with
    | n when n |> outOfRange -> ""
    | n when n = 0 -> "nulla"
    | _ ->
        let digits = digits n
        let factor = pown 10 (digits - 1) // 1, 10, 100, 1000
        let (one, half, next) = symbolTriple digits

        let fillWithSymbols subtrahend =
            String.replicate ((n - subtrahend) / factor) one

        let left =
            match n with
            | n when n < 4 * factor -> fillWithSymbols 0
            | n when n < 5 * factor -> one + half
            | n when n < 6 * factor -> half
            | n when n < 9 * factor -> half + fillWithSymbols (5 * factor)
            | n -> one + next

        let right =
            match n % factor with
            | 0 -> ""
            | m -> toRoman m

        left + right


// Test Section
// ------------
let printConsoleRed =
    fun (msg: string) ->
        System.Console.ForegroundColor <- ConsoleColor.Red
        System.Console.WriteLine msg
        System.Console.ResetColor()

let inline printRed msg = Printf.ksprintf printConsoleRed msg

let testConversion () =
    [ (0, "nulla")
      (1, "I")
      (2, "II")
      (3, "III")
      (4, "IV")
      (5, "V")
      (6, "VI")
      (7, "VII")
      (8, "VIII")
      (9, "IX")
      (10, "X")
      (23, "XXIII")
      (34, "XXXIV")
      (35, "XXXV")
      (36, "XXXVI")
      (44, "XLIV")
      (55, "LV")
      (66, "LXVI")
      (480, "CDLXXX")
      (489, "CDLXXXIX")
      (999, "CMXCIX")
      (3999, "MMMCMXCIX")
      (4000, "")
      (9999, "")
      (99999, "") ]
    |> List.map (fun (n, e) -> (n, e, toRoman n))
    |> List.iter (fun (n, e, r) ->
        match r with
        | r when r = e -> printfn "OK: %d -> %s" n r
        | r -> printRed "Not OK: %d -> %s but %s expected!" n r e)

testConversion ()
