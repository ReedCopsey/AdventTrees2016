namespace TreeLogic.Model

open System.Runtime.InteropServices

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

    let empty : Forest = []
    
    // Prune one tree if we're over the max size
    let private prune max (forest : Forest) : Forest = 
        let l = List.length forest
        if max < l then
            // Remove an "older" tree, from the 2nd half of the list
            let indexToRemove = rnd.Next ( l / 2, l)
            forest 
            |> List.mapi (fun i t -> (i <> indexToRemove, t))
            |> List.filter fst
            |> List.map snd
        else
            forest         

    let update msg forest =
        match msg with
            | Add(location)         -> Tree.create location :: forest    
            | UpdateTree(msg, tree) -> Tree.update msg tree :: List.except [ tree ] forest
            | Prune(maxTrees)       -> prune maxTrees forest
