open System

open System.IO
open HierarchyTree
open Search
open Serialisation
open Suave
open Suave.Operators
open Suave.Filters


let handler hierarchyTree (req : HttpRequest) =
    match req.queryParam "q" with
    | Choice1Of2 queryStr ->
        findInHierarchyTrees queryStr hierarchyTree
        |> serialiseQueryResponse queryStr
        |> jsonToStr
        |> Successful.OK
    | Choice2Of2 _ ->
        RequestErrors.BAD_REQUEST """A query parameter of "q" is required """



[<EntryPoint>]
let main _ =
    let hierarchyTree =
        File.ReadAllText "pairs.csv"
        |> parseCsv
        |> makeTree

    let handler' = handler hierarchyTree
    let endpoint : WebPart =
        path "/search/data-categories" >=> GET
        >=> Writers.setMimeType "application/json; charset=utf-8"
        >=> request handler'

    let config =
        { defaultConfig with
            bindings = [ HttpBinding.create HTTP Net.IPAddress.Any (uint16 4000) ] }

    startWebServer config endpoint

    0 // return an integer exit code
