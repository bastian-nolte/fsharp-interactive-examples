let l = [ "Hello"; "World!"; "123" ] // 5, 6, 3 |> sum = 14

let sumLengths strList =
    strList |> List.map String.length |> List.sum

l |> sumLengths


let product n =
    [ 1..n ] |> Seq.fold (fun a n -> a * n) 1

product 3

let sumOfOdds n =
    [ 1..n ]
    |> Seq.filter (fun n -> n % 2 <> 0)
    |> Seq.sum

sumOfOdds 5

let alternatingSum n =
    [ 1..n ]
    |> Seq.fold
        (fun a n ->
            let (isPositiv, value) = a

            let newValue =
                match isPositiv with
                | true -> value + n
                | false -> value - n

            (not isPositiv, newValue))
        (true, 0)
    |> (fun (_, value) -> value)

alternatingSum 3
