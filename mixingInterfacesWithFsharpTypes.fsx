type IMeansOfTransport =
    abstract member Move: unit -> string

type Aircraft =
    | Airbus
    | Boing

type Car =
    | VW
    | Toyota

type Aircraft with
    member this.AsMeansOfTransport =
        { new IMeansOfTransport with
            member a.Move() = "fly" }

type Car with
    member this.AsMeansOfTransport =
        { new IMeansOfTransport with
            member c.Move() = "drive" }

let myAircraft = Airbus
let myCar = VW

myAircraft.AsMeansOfTransport.Move()
myCar.AsMeansOfTransport.Move()
