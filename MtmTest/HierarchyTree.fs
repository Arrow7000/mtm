module HierarchyTree

open FSharp.Data

type DataProvider = CsvProvider<"pairs.csv">

let parseCsv str =
    let parsed = DataProvider.Parse str
    parsed.Rows
    |> Seq.map (fun row -> row.Parent.Trim(), row.Child.Trim())





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






let makeTree (parentChildRows : (string * string) seq) =
    let parentChildRelations =
        parentChildRows
        |> Seq.groupBy fst // group by parent
        |> Seq.map
            (fun (parent, pairs) ->
                let children = Seq.map snd pairs
                parent, children)

    let parentChildRelationsMap = Map.ofSeq parentChildRelations

    let allChildren =
        parentChildRelations
        |> Seq.map snd
        |> Seq.concat
        |> Set.ofSeq

    let allParents =
        parentChildRelations
        |> Seq.map fst
        |> Set.ofSeq

    /// Could be more than one root parent
    let rootParents =
        Set.difference allParents allChildren

    let rec findAllDescendants word =
        let childrenOpt = Map.tryFind word parentChildRelationsMap
        match childrenOpt with
        | None -> Leaf word
        | Some children -> Branch (word, Seq.map findAllDescendants children |> List.ofSeq |> List.distinct)

    rootParents
    |> Set.map findAllDescendants
    |> List.ofSeq
