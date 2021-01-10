module FSharp.Data

open FSharp.Data
open System.Runtime.CompilerServices

let private firstResultText (results: HtmlNode list) =
    match results.Length with
    | 0 -> None
    | _ -> Some(results.Head.InnerText().Trim())

[<Extension>]
type CssSelectorExtensions =
    [<Extension>]
    static member ElementText(node: HtmlNode, selector: string): string option =
        node.CssSelect(selector) |> firstResultText

    [<Extension>]
    static member ElementText(doc: HtmlDocument, selector: string): string option =
        doc.CssSelect(selector) |> firstResultText
