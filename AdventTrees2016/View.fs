namespace Views

open System.Windows
open System.Windows.Controls
open System.Windows.Input

open FsXaml

open TreeLogic.Model

type MainWindow = XAML<"MainWindow.xaml"> 

module internal MouseConverters =
    // Create a converter from mouse clicks on a Canvas to Some(location), and clicks elsewhere to None
    let locationConverter (args : MouseEventArgs) =
        match args.OriginalSource with
        | :? Canvas ->
            let source = args.OriginalSource :?> IInputElement
            let pt = args.GetPosition(source)
            Some { X = pt.X; Y = pt.Y }
        | _ -> None

type LocationConverter() = inherit EventArgsConverter<MouseEventArgs, Location option>(MouseConverters.locationConverter, None)