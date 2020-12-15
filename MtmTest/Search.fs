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
 a. what of query string we haven't been able to match yet
 b. marked target string so far
 c. the target string left
*)

/// Finds query string in target string.
/// Returns the unmatched chars in query string and the marked/unmarked chars of the target string.
let findInCharList queryStr targetStr =
    targetStr
    |> List.fold
        (fun (queryStrNotMatchedYet,markedString) targetStrChar ->
            match queryStrNotMatchedYet with
            | [] -> [], markedString @ [ Unmarked targetStrChar ]
            | queryStrChar :: rest ->
                if queryStrChar = targetStrChar then
                    rest, markedString @ [ Marked targetStrChar ]
                else
                    rest, markedString @ [ Unmarked targetStrChar ])
        (queryStr, List.empty)



/// WIP
let findInHierarchyTree queryStr (tree : HierarchyTree<string>) =
    let rec traverser remainingQueryCharList remainingTree =
        match remainingQueryCharList with
        | [] -> remainingTree // 
        | remainingQuery ->
            match remainingTree with
            | Leaf str ->
                findInCharList remainingQueryCharList


    traverser (Seq.toList queryStr) tree
