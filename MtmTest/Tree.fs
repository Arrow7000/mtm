module Tree

open FSharp.Data

type DataProvider = CsvProvider<"pairs.csv">


let parseCsv str =
    let parsed = DataProvider.Parse str
    parsed.Rows
    |> Seq.map (fun row -> row.)

type HierarchyTree<'T> =
    | Branch of 'T * HierarchyTree<'T> list
    | Leaf of 'T

    static member map (mapping : 'a -> 'b) =
        function
        | Leaf str -> Leaf (mapping str)
        | Branch (str, children) -> Branch (mapping str, List.map (HierarchyTree<_>.map mapping) children)

    static member bind (binder : 'a -> HierarchyTree<'a>) =
        function
        | Leaf str -> binder str
        | Branch (str, children) -> Branch (str, List.map (HierarchyTree<_>.bind binder) children)


(*
At each step one of 3 possibilities:
 a. one or more of previous top branches are children of the new branch
 b. new branch is a child of one of the previous branches
 c. both above cases are true for different branches
 d. all branches found so far are disjoint, the merging will come later
*)

let makeTree (parentChildRows : (string * string) seq) =
    parentChildRows
    |> Seq.groupBy fst // group by parent
    |> Seq.map
        (fun (parent, pairs) ->
            let children = Seq.map snd pairs
            parent, children)
    |> Map.ofSeq
    |> Map.fold
        (fun state key children ->
            match state with
            | [] -> // first run
                Branch (key, children |> Seq.map Leaf |> List.ofSeq) |> List.singleton
            | hierarchiesSoFar ->
                hierarchiesSoFar
                |> List.fold // hm
        )
        List.empty



