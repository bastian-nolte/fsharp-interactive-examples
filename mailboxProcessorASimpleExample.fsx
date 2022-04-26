let inboxAgent =
    MailboxProcessor.Start (fun inbox ->
        let rec messageLoop () =
            async {
                let! msg = inbox.Receive()
                printfn "Got a new message: %s" msg
                return! messageLoop () // Loop to top
            }

        messageLoop () // start the loop
    )

inboxAgent.Post "hello"
inboxAgent.Post "hello again"
inboxAgent.Post "I'm still here!"
