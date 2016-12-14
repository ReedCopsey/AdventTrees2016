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
    let rec pruneForever max (state : State<_,_>) =
        async {
            do! Async.Sleep 1000
                
            Prune max
            |> state.Update 
            |> ignore

            do! pruneForever max state
        }
    
    let application = 
        // Create our state
        let state = new State<_,_>(Forest.empty, Forest.update) 

        // Prune it in the background asynchronously
        pruneForever 10 state |> Async.Start 


        // Start our application
        Framework.application (fun _ -> state :> _) ignore (state.Update >> ignore) forestComponent 