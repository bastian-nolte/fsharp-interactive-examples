// create some types
type Address = { Street: string; City: string }

type Customer =
    { ID: int
      Name: string
      Address: Address }

// create a customer
let customer1 =
    { ID = 1
      Name = "Bob"
      Address = { Street = "123 Main"; City = "NY" } }

// extract name only
let { Name = name1 } = customer1
printfn "The customer is called %s" name1

// extract name and id
let { ID = id2; Name = name2 } = customer1
printfn "The customer called %s has id %i" name2 id2

// extract name and address
let { Name = name3
      Address = { Street = street3 } } =
    customer1

printfn "The customer is called %s and lives on %s" name3 street3
