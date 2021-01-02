module MetaCritic

open FSharp.Data
open System

type Platform =
    | PS4 = 72496
    | PS5 = 99999 // Still unknown
    | PS3 = 1
    | XboxOne = 80000
    | Xbox360 = 2
    | PC = 3
    | DS = 4
    | N3DS = 16
    | PsVita = 67365
    | PSP = 7
    | Wii = 8
    | WiiU = 68410
    | Switch = 268409
    | PS2 = 6
    | PS = 10
    | GBA = 11
    | IOS = 9
    | Xbox = 12
    | GameCube = 13
    | N64 = 14
    | Dreamcast = 15
    | All = 0

let private platfromFromString value =
    match value with
    | "PS5" -> Platform.PS5
    | "PS4" -> Platform.PS4
    | "PS3" -> Platform.PS3
    | "XONE" -> Platform.XboxOne
    | "X360" -> Platform.Xbox360
    | "PC" -> Platform.PC
    | "DS" -> Platform.DS
    | "3DS" -> Platform.N3DS
    | "VITA" -> Platform.PsVita
    | "PSP" -> Platform.PSP
    | "WII" -> Platform.Wii
    | "WIIU" -> Platform.WiiU
    | "Switch" -> Platform.Switch
    | "PS2" -> Platform.PS2
    | "PS" -> Platform.PS
    | "GBA" -> Platform.GBA
    | "iOS" -> Platform.IOS
    | "XBOX" -> Platform.Xbox
    | "GC" -> Platform.GameCube
    | "N64" -> Platform.N64
    | "Dreamcast" -> Platform.Dreamcast
    | _ -> failwith (sprintf "Unknown platform %s" value)

type Result =
    { Title: string
      Score: Nullable<uint>
      Platform: Platform }

let private processResult (result: HtmlNode) =
    let text selector =
        result.CssSelect(selector).Head.InnerText().Trim()

    let title = text ".product_title > a"
    let platform = text ".platform" |> platfromFromString

    let score =
        match text (".metascore_w") with
        | "tbd" -> Nullable()
        | n -> n |> uint |> Nullable

    { Title = title
      Score = score
      Platform = platform }

let find (platform: Platform) game =
    let rec findPage page =
        async {
            let url =
                match platform with
                | Platform.All -> sprintf "https://www.metacritic.com/search/game/%s/results?page=%d" game page
                | _ ->
                    sprintf
                        "https://www.metacritic.com/search/game/%s/results?page=%d&search_type=advanced&plats[%d]=1"
                        game
                        page
                        (int platform)

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
