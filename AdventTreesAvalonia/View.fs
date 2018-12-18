namespace Views

open System
open Avalonia
open Avalonia.Controls
open Avalonia.Input
open TreeLogic.Model
open Avalonia.Data.Converters
open Avalonia.Input.Raw

module internal MouseConverters =
    // Create a converter from mouse clicks on a Canvas to Some(location), and clicks elsewhere to None
    let locationConverter (args : PointerPressedEventArgs) =
        match args.Source with
        | :? Canvas as canvas ->
            let pt = args.GetPosition(canvas)
            Some { X = pt.X; Y = pt.Y }
        | _ -> None
 
// Create our converter from MouseEventArgs -> Location
type LocationConverter () = 
    interface IValueConverter with
        member __.Convert (value, targetType, param, culture) =
            match value with
            | :? PointerPressedEventArgs as args -> MouseConverters.locationConverter args |> box
            | _ -> null
        member __.ConvertBack (value, targetType, param, culture) =
            null

