namespace TreeLogic.Model

open Gjallarhorn.Bindable
open Gjallarhorn.Bindable.Framework

module Program =

    // "VM" types can be used in XAML designer, and allow simplified component construction
    type TreeVM =
        {
            Tree            : Tree
            Decorate        : VmCmd<TreeMessage>
            Light           : VmCmd<TreeMessage>
            DecorateOrLight : VmCmd<TreeMessage>
        }
    let treeDesign = { Tree = { Position = { X = 0.0 ; Y = 0.0 } ; Height = 1.0 ; Decorated = true ; Lit = true } ; Decorate = Vm.cmd Decorate ; Light = Vm.cmd Light ; DecorateOrLight = Vm.cmd DecorateOrLight }

    type ForestVM =
        {
            Forest     : Forest
            Add        : VmCmd<ForestMessage>            
        }
    let forestDesign = { Forest = Forest.empty ; Add = Vm.cmd (Add None) }

    // Create binding for a single tree.  This will output Decorate and Light messages
    let treeComponent =
        Component.create<Tree,unit,TreeMessage> [
            <@ treeDesign.Tree @>            |> Bind.oneWay id
            <@ treeDesign.Decorate @>        |> Bind.cmd
            <@ treeDesign.Light @>           |> Bind.cmd
            <@ treeDesign.DecorateOrLight @> |> Bind.cmd
        ]

    // Create binding for entire application.  This will output all of our messages.
    let forestComponent =
        Component.create<Forest,unit,ForestMessage> [
            <@ forestDesign.Forest @> |> Bind.collection id treeComponent UpdateTree
            <@ forestDesign.Add @>    |> Bind.cmdParam Add
        ]
    
    let pruneHandler (dispatch : Dispatch<_>) token =        
        // Handle pruning of the forest - 
        // Twice per second, send a prune message to remove a tree if there are more than max
        let rec pruneForever max =
            async {
                do! Async.Sleep 500                
                Prune max |> dispatch
                return! pruneForever max 
            }
    
        // Start prune loop in the background asynchronously
        Async.Start(pruneForever 10, token)

    let application nav =       
        // Start pruning "loop"
        let prune = new Executor<_,_>(pruneHandler)
        prune.Start()
        // Start our application
        Framework.application Forest.empty Forest.update forestComponent nav
        |> Framework.withDispatcher prune