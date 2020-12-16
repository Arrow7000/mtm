open System

open System.IO
open HierarchyTree
open Search

[<EntryPoint>]
let main argv =
   
    File.ReadAllText "pairs.csv"
    |> parseCsv
    |> makeTree
    |> findInHierarchyTrees "fghfghfghfghfghfghgf"
    |> printfn "%A"

    0 // return an integer exit code
