// https://fsharpforfunandprofit.com/posts/concurrency-async-and-parallel/
let sleepWorkflowMs ms =
    async {
        printfn "%i ms workflow started..." ms
        do! Async.Sleep ms
        printfn "...%i workflow finished." ms
    }

let sleep1 = sleepWorkflowMs 1000
let sleep2 = sleepWorkflowMs 2000

#time
// Example of a “fork/join” approach, where a number of child tasks are spawned and then the parent waits for them all to finish.
[ sleep1; sleep2 ]
|> Async.Parallel
|> Async.RunSynchronously

#time
