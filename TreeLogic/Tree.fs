namespace TreeLogic.Model

open System.Runtime.InteropServices

// Our tree types
type Location = { X: float; Y: float }
type Tree = { Position : Location ; Height : float ; Decorated : bool ; Lights : bool ; Lit : bool }

// Update types allowed on a tree
type TreeMessage = | Decorate | Light | Blink

// Module showing allowed operations on an existing tree
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Tree =
    let private rnd = System.Random()
    let private makeHeight () = 8.0 + rnd.NextDouble() * 4.0

    let create location = 
        { Position = location ; Height = makeHeight () ; Decorated = false ; Lights = false ; Lit = false }

    let blink tree = 
        match tree.Lights with 
        | true  -> { tree with Lit = not tree.Lit} 
        | false -> { tree with Lit = false}

    let update msg tree =
        match msg with
        | Decorate -> { tree with Decorated = not tree.Decorated }
        | Light    -> { tree with Lights = not tree.Lights }
        | Blink  -> tree |> blink
