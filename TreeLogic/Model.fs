namespace TreeLogic.Model

open System.Runtime.InteropServices

// Our tree model types
type Location = { X: float; Y: float }
type Tree = { Position : Location ; Height : float ; Decorated : bool }

// Update types allowed on a tree
type TreeMessage = | Decorate

// Module showing allowed operations on an existing tree
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Tree =
    let update msg tree =
        match msg with
        | Decorate -> { tree with Decorated = true }

// Our main forest model
type Forest = Tree list

// Update types allowed on a forest
type ForestMessage =
    | Add of Location // Add new tree at a location
    | UpdateTree of msg : TreeMessage * tree : Tree // Update an existing tree
    | Prune of maxTrees : int  // Prune the trees

// Module with allowed operations on a forest
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Forest =
    let private rnd = System.Random()
    let private makeHeight () = 8.0 + rnd.NextDouble() * 4.0

    let empty : Forest = []

    let private add location (forest : Forest) : Forest = 
        let tree = { Position = location ; Height = makeHeight () ; Decorated = false }
        tree :: forest    

    // Prune one tree if we're over the max size
    let private prune max (forest : Forest) : Forest = 
        let l = List.length forest
        if max < l then
            let indexToRemove = rnd.Next l
            forest 
            |> List.mapi (fun i t -> (i <> indexToRemove, t))
            |> List.filter fst
            |> List.map snd
        else
            forest         

    let update msg forest =
        match msg with
            | Add(tree)             -> add tree forest
            | UpdateTree(msg, tree) -> Tree.update msg tree :: List.except [ tree ] forest
            | Prune(maxTrees)       -> prune maxTrees forest
