// https://fsharpforfunandprofit.com/posts/concurrency-actor-model/#the-message-based-approach-to-shared-state
open System
open System.Threading

type Utility() =
    static let rand = Random()

    static member RandomSleep() =
        let ms = rand.Next(1, 10)
        Thread.Sleep ms

type MessageBasedCounter() =
    // the business logic!
    static let updateState (count, sum) msg =

        // increment the counters and...
        let newSum = sum + msg
        let newCount = count + 1
        printfn "Count is: %i. Sum is: %i" newCount newSum

        // ...emulate a short delay
        Utility.RandomSleep()

        // return the new state
        (newCount, newSum)

    // create the agent
    static let agent =
        MailboxProcessor.Start (fun inbox ->

            // the message processing function
            let rec messageLoop oldState =
                async {
                    printfn "Next round!"
                    // read a message
                    let! msg = inbox.Receive()

                    // do the core logic
                    let newState = updateState oldState msg

                    // loop to top
                    return! messageLoop newState
                }

            // start the loop
            messageLoop (0, 0))

    // public interface to hide the implementation
    static member Add i = agent.Post i

let makeCountingTask addFunction taskId =
    async {
        let name = sprintf "Task%i" taskId

        for i in [ 1..3 ] do
            addFunction i
    }


let messageExample5 =
    [ 1..5 ]
    |> List.map (fun i -> makeCountingTask MessageBasedCounter.Add i)
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
