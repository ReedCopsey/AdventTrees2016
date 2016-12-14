open System
open Gjallarhorn.Wpf
open Views
open TreeLogic.Model

[<STAThread>]
[<EntryPoint>]
let main _ =  
    // Run using the WPF wrappers around the basic application framework    
    Framework.runApplication System.Windows.Application MainWindow Program.application