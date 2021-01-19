module GameInfo.Powershell.HowLongToBeat.HowLongToBeat

open FSharp.Data
open System.Text.RegularExpressions
open System

let private add a b = a + b

let private parseTime (text: string): TimeSpan option =
    let reMatch =
        Regex.Match(text, "(\d+)(½)? (Hours|Mins)")

    match reMatch.Success with
    | true ->
        let num = reMatch.Groups.[1].Value |> int

        match reMatch.Groups.[3].Value with
        | "Hours" ->
            let minutes =
                if reMatch.Groups.[2].Success then
                    30
                else
                    0

            TimeSpan(num, minutes, 0)
        | "Mins" -> TimeSpan(0, num, 0)
        | _ -> failwith "Unexpected result"
        |> Some
    | false -> None

let private processResult (result: HtmlNode) =
    let elements = result.CssSelect(".search_list_tidbit")
    let mainStory = elements.[1].InnerText() |> parseTime
    let mainAndExtra = elements.[3].InnerText() |> parseTime
    let completioninst = elements.[5].InnerText() |> parseTime

    let title =
        result.ElementText ".shadow_text" |> Option.get

    { Title = title
      MainStory = mainStory |> Option.toNullable
      MainAndExtra = mainAndExtra |> Option.toNullable
      Completioninst = completioninst |> Option.toNullable }

let findGame game =
    let findGame page =
        async {
            let! html =
                Http.AsyncRequestString(
                    "https://howlongtobeat.com/search_results?page=1",
                    body =
                        FormValues [ "queryString", game
                                     "t", "games"
                                     "sorthead", "popular"
                                     "sortd", "Normal Order"
                                     "plat", ""
                                     "length_type", "main"
                                     "length_min", ""
                                     "length_max", ""
                                     "detail", ""
                                     "randomize", "0" ]
                )

            do
                System.IO.File.WriteAllText("kaki.html", html)
                |> ignore

            let html = "<html>" + html + "</html>"
            let doc = HtmlDocument.Parse html

            return
                doc.CssSelect(".back_darkish")
                |> List.map processResult
        }

    findGame 1
