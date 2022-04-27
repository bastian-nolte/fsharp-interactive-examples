open System

// int example
let quantity =
    match Int32.TryParse("123") with
    | (b, i) when b -> Some(i)
    | _ -> None

match quantity with
| Some (i) -> printfn "You sucessfully ordered %i ducks." i
| None -> printfn "Your shopping cart is empty."


// date example
let (dateSuccess, date) = System.DateTime.TryParse("1/1/1980")

if dateSuccess then
    printfn "The given date is %s" (date.ToLongDateString())
