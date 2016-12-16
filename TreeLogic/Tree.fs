namespace TreeLogic.Model

open System.Runtime.InteropServices

// Our tree types
type Location = { X: float; Y: float }
type Tree = { Position : Location ; Height : float ; Decorated : bool ; Lit : bool }

// Update types allowed on a tree
type TreeMessage = | Decorate | Light

// Module showing allowed operations on an existing tree
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Tree =
    let private rnd = System.Random()
    let private makeHeight () = 8.0 + rnd.NextDouble() * 4.0

    let create location = 
        { Position = location ; Height = makeHeight () ; Decorated = false ; Lit = false }

    let update msg tree =
        match msg with
        | Decorate -> { tree with Decorated = not tree.Decorated }
        | Light    -> { tree with Lit = not tree.Lit }
