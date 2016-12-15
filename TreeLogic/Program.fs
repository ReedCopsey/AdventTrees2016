namespace TreeLogic.Model

open Gjallarhorn
open Gjallarhorn.Bindable

module Program =

    // Create binding for a single tree.  This will output Decorate messages
    let treeComponent source (model : ISignal<Tree>) =
        // Bind the tree itself to the view
        model |> Binding.toView source "Tree"

        [
            // Create a command that turns into the Decorate message
            source |> Binding.createMessage "Decorate" Decorate 
        ]

    // Create binding for entire application.  This will output all of our messages.
    let forestComponent source (model : ISignal<Forest>) =             
        // Bind our collection to "Forest"
        let forest = BindingCollection.toView source "Forest" model treeComponent             

        [
            // Map Decorate messages in the treeComponent to UpdateTree messages
            forest |> Observable.map UpdateTree
            // Create a command that routes to Add messages
            source |> Binding.createMessageParam "Add" Add
        ]

    // Handle pruning of the forest - 
    // Once per second, send a prune message to remove a tree if there are more than max
    let rec pruneForever max update =
        async {
            do! Async.Sleep 500
                
            Prune max |> update

            do! pruneForever max update
        }
    
    let application = 
        // Create our forest, wrapped in a mutable with an atomic update function
        let forest = new AsyncMutable<_>(Forest.empty)

        // Create our 3 functions for the application framework

        // Start with the function to create our model (as an ISignal<'a>)
        let createModel () : ISignal<_> = forest :> _

        // Create a function that updates our state given a message
        let update (msg : ForestMessage) : unit = Forest.update msg |> forest.Update |> ignore

        // An init function that occurs once everything's created, but before it starts
        let init () : unit = 
            // Start prune loop in the background asynchronously
            pruneForever 10 update |> Async.Start 

        // Start our application
        Framework.application createModel init update forestComponent 