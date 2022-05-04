type Points = int

type Try =
    | Points of int
    | Foul

type RegularFrame =
    { FirstTry: Try
      SecondTry: Try option }

type LastFrame =
    { FirstTry: Try
      SecondTry: Try
      ThirdTry: Try option }

type Frame =
    | RegularFrame of RegularFrame
    | LastFrame of LastFrame

type FrameType =
    | OpenFrame
    | Spare
    | Strike

let Symbol =
    {| Strike = "X"
       Spare = "/"
       None = " "
       Foul = "-" |}

type FrameOnBoard =
    { Frame: Frame
      Type: FrameType
      Score: string
      Summery: Points }

type Game = Frame list

let tryToPoints ``try`` : Points =
    match ``try`` with
    | Points p -> p
    | Foul -> 0

let optionalTryToPoints o =
    match o with
    | Some (o) -> tryToPoints o
    | None -> 0

let getFirstTry frame =
    match frame with
    | RegularFrame f -> f.FirstTry
    | LastFrame f -> f.FirstTry

let getSecondTryOption frame =
    match frame with
    | RegularFrame f -> f.SecondTry
    | LastFrame f -> Some(f.SecondTry)

let getFirstTriesPoints = getFirstTry >> tryToPoints

let getSecondTriesPoints = getSecondTryOption >> optionalTryToPoints

let calculateFirstTry firstTry =
    match firstTry with
    | Foul -> Symbol.Foul, 0
    | Points p ->
        match p with
        | 10 -> Symbol.Strike, p
        | _ -> ($"{p}", p)

let calculateSecondTry firstPoints secondTry =
    match secondTry with
    | None -> Symbol.None, 0
    | Some Foul -> Symbol.Foul, 0
    | Some (Points p) ->
        match p + firstPoints with
        | 10 -> Symbol.Spare, p
        | _ when p = 10 -> Symbol.Strike, p // only relevant on last frame.
        | _ -> ($"{p}", p)

let calculateThirdTry thirdTry =
    match thirdTry with
    | None -> Symbol.None, 0
    | Some Foul -> Symbol.Foul, 0
    | Some (Points p) ->
        match p with
        | 10 -> Symbol.Strike, p
        | _ -> ($"{p}", p)

let ``calculate regular frame`` (frame: RegularFrame) : FrameOnBoard =
    let firstScore, firstPoints = calculateFirstTry frame.FirstTry
    let secondScore, secondPoints = calculateSecondTry firstPoints frame.SecondTry
    let summery = firstPoints + secondPoints
    let isStrike = firstScore = Symbol.Strike
    let isSpare = secondScore = Symbol.Spare

    let (frameType, score) =
        match frame with
        | _ when isStrike -> (Strike, sprintf "  %s" Symbol.Strike)
        | _ when isSpare -> (Spare, sprintf "%s %s" firstScore Symbol.Spare)
        | _ -> (OpenFrame, sprintf "%s %s" firstScore secondScore)

    { Frame = RegularFrame(frame)
      Type = frameType
      Score = score
      Summery = summery }

let ``calculate last frame`` frame =
    let firstScore, firstPoints = calculateFirstTry frame.FirstTry
    let someSecondTry = Some frame.SecondTry
    let secondScore, secondPoints = calculateSecondTry firstPoints someSecondTry
    let thirdScore, thirdPoints = calculateThirdTry frame.ThirdTry
    let summery = firstPoints + secondPoints + thirdPoints
    let isStrike = firstScore = Symbol.Strike
    let isSpare = secondScore = Symbol.Spare
    let score = sprintf "%s %s %s" firstScore secondScore thirdScore

    let frameType =
        match frame with
        | f when isStrike -> Strike
        | f when isSpare -> Spare
        | _ -> OpenFrame

    { Frame = LastFrame(frame)
      Type = frameType
      Score = score
      Summery = summery }

let ``calculate the current frame`` frame =
    match frame with
    | RegularFrame r -> ``calculate regular frame`` r
    | LastFrame l -> ``calculate last frame`` l

let recalculateTheFrameBefore frameBefore currentFrame =
    match frameBefore.Type with
    | Strike ->
        { frameBefore with
            Summery =
                frameBefore.Summery
                + getFirstTriesPoints currentFrame
                + getSecondTriesPoints currentFrame }
    | Spare ->
        { frameBefore with
            Summery =
                frameBefore.Summery
                + getFirstTriesPoints currentFrame }
    | OpenFrame -> frameBefore

let recalculateThePenultimateFrame penultimateFrame frameBefore currentFrame =
    if frameBefore.Type = Strike
       && penultimateFrame.Type = Strike then
        { penultimateFrame with
            Summery =
                penultimateFrame.Summery
                + getFirstTriesPoints currentFrame }
    else
        penultimateFrame

let recalculateStrikesAndSpares acc currentFrameOnBoard =
    let currentFrame = currentFrameOnBoard.Frame

    let recalculated =
        match acc with
        | [] -> []
        | before :: tail ->
            let frameBefore = recalculateTheFrameBefore before currentFrame

            match tail with
            | [] -> [ frameBefore ]
            | penultimate :: tail ->
                let penultimateFrame =
                    recalculateThePenultimateFrame penultimate frameBefore currentFrame

                frameBefore :: penultimateFrame :: tail

    currentFrameOnBoard :: recalculated

let summaizeBoard acc currentFrameOnBoard =
    match acc with
    | [] -> [ currentFrameOnBoard ]
    | frameBefore :: tail ->
        { currentFrameOnBoard with Summery = currentFrameOnBoard.Summery + frameBefore.Summery }
        :: acc

let initialBoard: FrameOnBoard list = []

let createBoard game =
    game
    |> List.map ``calculate the current frame``
    |> List.fold recalculateStrikesAndSpares initialBoard
    |> List.rev
    |> List.fold summaizeBoard initialBoard
    |> List.rev

let printBoard board =
    let (scores, summeries) =
        board
        |> List.fold
            (fun acc frame ->
                let scores, summeries = acc
                let newScores = sprintf "%s| %s " scores (frame.Score.PadLeft 7)
                let newSummeries = sprintf "%s| %s " summeries (frame.Summery.ToString().PadRight 7)
                (newScores, newSummeries))
            ("", "")

    printfn "| Round 1 |       2 |       3 |       4 |       5 |       6 |       7 |       8 |       9 |      10 |"
    printfn "|---------------------------------------------------------------------------------------------------|"
    printfn $"{scores}|"
    printfn $"{summeries}|"

// ------------------------------------------------------------------
// Let's play a game
// ------------------------------------------------------------------
let throwAStrike =
    RegularFrame(
        { FirstTry = Points 10
          SecondTry = None }
    )

let game: Game =
    [ throwAStrike
      RegularFrame(
          { FirstTry = Foul
            SecondTry = Some(Points 0) }
      )
      throwAStrike
      RegularFrame(
          { FirstTry = Points 3
            SecondTry = Some(Points 7) }
      )
      throwAStrike
      throwAStrike
      throwAStrike
      RegularFrame(
          { FirstTry = Points 3
            SecondTry = Some(Points 4) }
      )
      throwAStrike
      LastFrame(
          { FirstTry = Points 6
            SecondTry = Points 4
            ThirdTry = Some(Points 10) }
      ) ]

let board = createBoard game

printBoard board
