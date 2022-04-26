// https://fsharpforfunandprofit.com/posts/concurrency-async-and-parallel/
let sleepWorkflowMs ms =
    async {
        printfn "%i ms workflow started..." ms
        do! Async.Sleep ms
        printfn "...%i workflow finished." ms
    }

let workflowInSeries =
    async {
        let! sleep1 = sleepWorkflowMs 1000
        printfn "Finished the first workflow."
        let! sleep2 = sleepWorkflowMs 2000
        printfn "Finished the second workflow."
    }

#time
Async.RunSynchronously workflowInSeries
#time
