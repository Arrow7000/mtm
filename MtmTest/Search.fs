module Search

open HierarchyTree


type SearchResultString =
    | Unmarked of string
    | Marked of string

/// String consisting of some marked and some unmarked parts,
/// the marked parts correlating to the query string
type MarkedString = SearchResultString list


let printMarkedString (markedString : MarkedString) =
    markedString
    |> List.fold
        (fun strSoFar thisChunk ->
            match thisChunk with
            | Unmarked str -> strSoFar + str
            | Marked str -> strSoFar + (sprintf "<mark>%s</mark>" str))
        ""


type SearchResultTree =
    | Branch of MarkedString * SearchResultTree list
    | Leaf of MarkedString
