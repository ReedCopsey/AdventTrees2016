namespace TreeLogic.Model

open System.Runtime.InteropServices

// Our model types
type Location = { X: float; Y: float }
type Tree = { Position : Location ; Height : float ; Decorated : bool }
type Forest = Tree list

type ForestMessage =
    | Add of Location // Add new tree at a location
    | DecorateTree of Tree  // Decorate an existing tree
    | Prune of maxTrees : int  // Prune the trees

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Forest =
    let private rnd = System.Random()
    let private makeHeight () = 8.0 + rnd.NextDouble() * 4.0

    let empty : Forest = []

    let private add location (forest : Forest) : Forest = 
        let tree = { Position = location ; Height = makeHeight () ; Decorated = false }
        tree :: forest

    let private decorate tree (forest : Forest) : Forest = 
        let allButTree = 
            forest |> List.except [ tree ]
        { tree with Decorated = true } :: allButTree

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
        forest
        |>  match msg with
            | Add(tree) -> add tree
            | DecorateTree(tree) -> decorate tree
            | Prune(maxTrees) -> prune maxTrees
