module MetaCritic

open FSharp.Data
open FSharpPlus
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

let private platformToString value =
    match value with
    | Platform.PS5 -> "playstation-5"
    | Platform.PS4 -> "playstation-4"
    | Platform.PS3 -> "playstation-3"
    | Platform.XboxOne -> "xbox-one"
    | Platform.Xbox360 -> "xbox-360"
    | Platform.PC -> "PC"
    | Platform.DS -> "DS"
    | Platform.N3DS -> "3DS"
    | Platform.PsVita -> "VITA"
    | Platform.PSP -> "PSP"
    | Platform.Wii -> "WII"
    | Platform.WiiU -> "WII-U"
    | Platform.Switch -> "Switch"
    | Platform.PS2 -> "PS2"
    | Platform.PS -> "PS"
    | Platform.GBA -> "GBA"
    | Platform.IOS -> "iOS"
    | Platform.Xbox -> "XBOX"
    | Platform.GameCube -> "GC"
    | Platform.N64 -> "N64"
    | Platform.Dreamcast -> "Dreamcast"
    | _ -> failwith "Internal Error"

let private platfromFromString value =
    match value with
    | "PlayStation 5"
    | "PS5" -> Platform.PS5
    | "PlayStation 4"
    | "PS4" -> Platform.PS4
    | "PlayStation 3"
    | "PS3" -> Platform.PS3
    | "Xbox One"
    | "XONE" -> Platform.XboxOne
    | "Xbox 360"
    | "X360" -> Platform.Xbox360
    | "PC" -> Platform.PC
    | "DS" -> Platform.DS
    | "3DS" -> Platform.N3DS
    | "VITA" -> Platform.PsVita
    | "PSP" -> Platform.PSP
    | "Wii" -> Platform.Wii
    | "WIIU"
    | "Wii U" -> Platform.WiiU
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

type FindResult =
    { Title: string
      MetaScore: Nullable<uint>
      Platform: Platform
      Uri: Uri }

type GameData =
    { Title: string
      MetaScore: Nullable<uint>
      UserScore: Nullable<float>
      Platform: Platform
      Uri: Uri }


let private score typeFunc text =
    match text with
    | "tbd" -> None
    | n -> n |> typeFunc |> Some

let private metaScore = score uint
let private userScore = score float

let private processResult (result: HtmlNode) =
    let text selector =
        result.CssSelect(selector).Head.InnerText().Trim()

    let title = text ".product_title > a"
    let platform = text ".platform" |> platfromFromString
    let score = text ".metascore_w" |> metaScore

    let uri =
        ("https://www.metacritic.com"
         + result
             .CssSelect(".product_title > a")
             .Head.AttributeValue("href"))
        |> Uri

    { Title = title
      MetaScore = score |> Option.toNullable
      Platform = platform
      Uri = uri }

let find (platform: Platform) (game: string) =
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

let gameUri (platform: Platform) (game: string): Uri =
    sprintf
        "https://www.metacritic.com/game/%s/%s"
        ((platformToString platform).ToLower())
        (game.ToLower().Replace("& ", "").Replace(" ", "-"))
    |> Uri

let get (gameUri: Uri): Async<GameData> =
    async {
        let! doc = HtmlDocument.AsyncLoad(gameUri.ToString())

        let text selector =
            let results = doc.CssSelect(selector)

            match results.Length with
            | 0 -> None
            | _ -> Some(results.Head.InnerText().Trim())

        let metaScore =
            text ".metascore_w.xlarge"
            |> Option.bind metaScore

        let userScore =
            text ".metascore_w.user" |> Option.bind userScore

        return
            { Title = text "h1" |> Option.get
              Platform =
                  text ".platform > a"
                  |> Option.get
                  |> platfromFromString
              MetaScore = (metaScore |> Option.toNullable)
              UserScore = (userScore |> Option.toNullable)
              Uri = gameUri }
    }
