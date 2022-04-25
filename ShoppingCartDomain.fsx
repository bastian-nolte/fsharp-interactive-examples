// https://fsharpforfunandprofit.com/posts/designing-for-correctness/

// Domain of a purchasing system
// =============================
type CartItem = string
type EmptyState = NoItems
type ActiveState = { UnpaidItems: CartItem list }

type PaidForState =
    { PaidItems: CartItem list
      Payment: decimal }

type Cart =
    | Empty of EmptyState
    | Active of ActiveState
    | PaidFor of PaidForState

// Operations an empty state
// =========================
let addToEmptyState itemToAdd =
    Cart.Active { UnpaidItems = [ itemToAdd ] }

// Operations on active state
// ==========================
let addToActiveState state itemToAdd =
    Cart.Active { state with UnpaidItems = itemToAdd :: state.UnpaidItems }

let removeFromActiveState state itemToRemove =
    let shortenedItemList =
        state.UnpaidItems
        |> List.filter (fun i -> i <> itemToRemove)

    match shortenedItemList with
    | [] -> Cart.Empty NoItems
    | _ -> Cart.Active { state with UnpaidItems = shortenedItemList }

let payForActiveState state amount =
    Cart.PaidFor
        { PaidItems = state.UnpaidItems
          Payment = amount }

// Attach operations to the states as methods
// ==========================================
type EmptyState with
    member this.Add = addToEmptyState

type ActiveState with
    member this.Add = addToActiveState this
    member this.Remove = removeFromActiveState this
    member this.Pay = payForActiveState this

// Operations on cart
// ==================
let addItemToCart cart item =
    match cart with
    | Empty state -> state.Add item
    | Active state -> state.Add item
    | PaidFor state ->
        printfn "ERROR: The cart is paid for."
        cart

let removeItemFromCart cart item =
    match cart with
    | Empty state ->
        printfn "ERROR: The cart is empty"
        cart
    | Active state -> state.Remove item
    | PaidFor state ->
        printfn "ERROR: The cart is paid for"
        cart

let displayCart cart =
    match cart with
    | Empty state -> printfn "The cart is empty."
    | Active state -> printfn "The cart contains %A unpaid items" state.UnpaidItems
    | PaidFor state -> printfn "The cart cointains %A paid items. Amount paid: %f" state.PaidItems state.Payment

// Attach operations to the cart as methods
// ========================================
type Cart with
    static member NewCart = Cart.Empty NoItems
    member this.Add = addItemToCart this
    member this.Remove = removeItemFromCart this
    member this.Display = displayCart this


// Test the domain
// ===============
let emptyCart = Cart.NewCart
printf "emptyCart="
emptyCart.Display

let cartA1 = emptyCart.Add "Dragon fruit"
printf "cartA1="
cartA1.Display

let cartA2 = cartA1.Add "Kiwi"
printf "cartA2="
cartA2.Display

let cartA3 = cartA2.Remove "Dragon fruit"
let emtpyCart2 = cartA3.Remove "Kiwi"
printf "emptyCart2="
emtpyCart2.Display

let emptyCart3 = emtpyCart2.Remove "Banana"
printf "emptyCart3="
emptyCart3.Display

let cartA2Paid =
    match cartA2 with
    | Empty _
    | PaidFor _ -> cartA2
    | Active state -> state.Pay 8.23m

printf "cartA2Paid="
cartA2Paid.Display

// The compiler prevent us from doing this.
// match cartABPaid with
// | Empty state -> state.Pay 100m
// | PaidFor state -> state.Pay 100m
// | Active state -> state.Pay 100m
