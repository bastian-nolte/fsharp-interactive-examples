// https://fsharpforfunandprofit.com/posts/concurrency-async-and-parallel/
open System
open System.Threading

let testLoop =
    async {
        for i in [ 1..100 ] do
            // do something
            printf "%i before.." i

            // sleep a bit
            do! Async.Sleep 75
            printfn "..after"
    }

let cancellationSource = new CancellationTokenSource()
// start the task, but this time pass in a cancellation token
Async.Start(testLoop, cancellationSource.Token)

// wait a bit
Thread.Sleep(1200)

// cancel after 200ms
cancellationSource.Cancel()
