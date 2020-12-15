open System

open System.IO
open HierarchyTree

[<EntryPoint>]
let main argv =
   
    File.ReadAllText "pairs.csv"
    |> parseCsv
    |> makeTree
    |> printfn "%A"

    0 // return an integer exit code
