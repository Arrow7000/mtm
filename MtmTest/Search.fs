module Search

open HierarchyTree


type SearchResultChar = Unmarked of char | Marked of char

/// String consisting of some marked and some unmarked parts,
/// the marked parts correlating to the query string
type MarkedString = SearchResultChar list


let printMarkedString (markedString : MarkedString) =
    markedString
    |> List.fold
        (fun strSoFar thisChunk ->
            match thisChunk with
            | Unmarked str -> strSoFar + string str
            | Marked str -> strSoFar + (sprintf "<mark>%c</mark>" str)) // this marks every char, even adjacent ones, but not a big deal; can optimise later
        ""


type SearchResultTree =
    | ResultBranch of MarkedString * SearchResultTree list
    | ResultLeaf of MarkedString




(*
We need to keep track of:
 a. how much of query string we've been able to match
 b. marked target string so far
 c. the target string left
*)


/// Very much in progress
let findInStr queryStr targetStr =
    targetStr
    |> List.fold
        (fun (coveredQueryStr,yetToDoQueryStr,markedString) targetStrChar ->
            match yetToDoQueryStr with
            | [] -> ()
            | list -> ()

        )
        ((List.empty,queryStr),List.empty)


let findInHierarchyTree queryStr (tree : HierarchyTree<string>) =
    let rec traverser remainingQueryStr remainingTree =
        match remainingTree with
        | Leaf str -> ()



