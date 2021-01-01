module MetaCritic

open FSharp.Data
open System

type Result =
    { Title: string
      Score: Nullable<uint>
      Platform: string }

let private processResult (result: HtmlNode) =
    let text selector =
        result.CssSelect(selector).Head.InnerText().Trim()

    let title = text ".product_title > a"
    let platform = text ".platform"

    let score =
        match text (".metascore_w") with
        | "tbd" -> Nullable()
        | n -> n |> uint |> Nullable

    { Title = title
      Score = score
      Platform = platform }

let find game =
    let rec findPage page =
        async {
            let url =
                sprintf "https://www.metacritic.com/search/game/%s/results?page=%d" game page

            let! doc = HtmlDocument.AsyncLoad(url)

            let results =
                doc.CssSelect(".result_wrap")
                |> List.map processResult

            if doc.CssSelect("span.next").IsEmpty then
                return results
            else
                let! further = findPage (page + 1)
                return List.append results further
        }

    findPage 0
