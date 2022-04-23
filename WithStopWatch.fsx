let withStopWatch functionToCall =
    [ 1..100000 ]
    |> List.map (fun n ->
        let stopWatch = System.Diagnostics.Stopwatch.StartNew()
        functionToCall ()
        stopWatch.Stop()
        stopWatch.Elapsed.TotalMilliseconds)
    |> List.average
