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
    | SuccessLeaf of MarkedString
    | NoResultLeaf




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
            | queryStrChar :: rest as list ->
                if queryStrChar.ToString().ToLowerInvariant() = targetStrChar.ToString().ToLowerInvariant() then
                    rest, markedString @ [ Marked targetStrChar ]
                else
                    list, markedString @ [ Unmarked targetStrChar ])
        (queryStr, List.empty)



/// Straight conversion of a hierarchy tree to a search result tree, for when
/// all descendants of a tree need to be converted, e.g. when the whole rest
/// of the tree is to be included in the search results.
let rec convertHierarchyToSearchResult (tree : HierarchyTree<string>) =
    match tree with
    | Leaf str ->
        let markedStr = Seq.toList str |> List.map Unmarked
        SuccessLeaf markedStr
    | Branch (str, children) ->
        let markedStr = Seq.toList str |> List.map Unmarked
        ResultBranch (markedStr, List.map convertHierarchyToSearchResult children )


let rec trimNoResults (searchResult : SearchResultTree) =
    match searchResult with
    | NoResultLeaf -> None
    | SuccessLeaf str -> SuccessLeaf str |> Some
    | ResultBranch (markedString, children) ->
        let childResults =
            children
            |> List.choose trimNoResults
        match childResults with
        | [] -> None
        | results -> ResultBranch (markedString,results) |> Some






let findInHierarchyTree queryStr (tree : HierarchyTree<string>) =
    let rec traverser remainingQueryCharList remainingTree =
        match remainingQueryCharList with
        | [] -> convertHierarchyToSearchResult remainingTree // If everything in query string has been matched then return all descendants
        | remainingQuery ->
            match remainingTree with
            | Leaf str ->
                let targetCharList = Seq.toList str
                let (queryCharsLeft,markedString) = findInCharList remainingQuery targetCharList

                match queryCharsLeft with
                | [] -> // this branch matches!
                    SuccessLeaf markedString
                | _ -> NoResultLeaf // this branch doesn't match!
            | Branch (str,children) ->
                let targetCharList = Seq.toList str
                let (queryCharsLeft,markedString) = findInCharList remainingQuery targetCharList

                match queryCharsLeft with
                | [] -> // this branch matches!
                    ResultBranch (markedString, List.map convertHierarchyToSearchResult children)
                | someLeft ->
                    let childrenResults =
                        List.map (traverser someLeft) children
                    match childrenResults with
                    | [] -> NoResultLeaf
                    | childrenResults -> ResultBranch (markedString, childrenResults)

    traverser (Seq.toList queryStr) tree
    |> trimNoResults



let findInHierarchyTrees queryStr (trees : HierarchyTree<string> list) =
    trees
    |> List.choose (findInHierarchyTree queryStr)
