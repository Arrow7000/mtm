module Serialisation

open FSharp.Data
open Search

let private serialiseList = List.toArray >> JsonValue.Array


let rec serialiseResponseTreeToJson (responseTree : ResponseTree) =
    match responseTree with
    | ChildResult str -> JsonValue.String str
    | ParentResult (str, children) ->
        [| "name", JsonValue.String str
           "children", List.map serialiseResponseTreeToJson children |> serialiseList |]
        |> JsonValue.Record



let serialiseQueryResponse queryStr responseTrees =
    [| "searchTerm", JsonValue.String queryStr 
       "results", responseTrees |> List.map serialiseResponseTreeToJson |> serialiseList |]
    |> JsonValue.Record


let jsonToStr (jsonValue : JsonValue) = jsonValue.ToString()
