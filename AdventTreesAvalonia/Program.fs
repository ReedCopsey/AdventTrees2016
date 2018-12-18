module Startup

open System

open TreeLogic.Model
open AdventTreesAvalonia

open Gjallarhorn.Avalonia
open Avalonia
open Avalonia.Logging.Serilog

// ----------------------------------  Application  ---------------------------------- 
[<STAThread>]
[<EntryPoint>]
let main _ =         
    let app () =
        AppBuilder.Configure<App>().UsePlatformDetect().LogToDebug().SetupWithoutStarting().Instance                

    let nav = Gjallarhorn.Avalonia.Navigation.singleView app MainWindow
    let app' = Program.application nav.Navigate
         
    Gjallarhorn.Avalonia.Framework.RunApplication<Forest,unit,ForestMessage> (nav, app')

    1