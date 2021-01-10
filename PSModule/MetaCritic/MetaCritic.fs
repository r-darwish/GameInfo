module MetaCritic

open FSharp.Data
open FSharpPlus
open System

type Platform =
    | PS4 = 72496
    | PS5 = 100000 // Still unknown
    | Stadia = 100001 // Still unknown
    | XboxSeriesX = 100002 // Still unknown
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
    | Platform.Stadia -> "Stadia"
    | Platform.XboxSeriesX -> "Xbox Series X"
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
    | "PlayStation Vita"
    | "VITA" -> Platform.PsVita
    | "PSP" -> Platform.PSP
    | "WII" -> Platform.Wii
    | "Wii" -> Platform.Wii
    | "WIIU"
    | "Wii U" -> Platform.WiiU
    | "Switch" -> Platform.Switch
    | "PlayStation 2"
    | "PS2" -> Platform.PS2
    | "PlayStation"
    | "PS" -> Platform.PS
    | "Game Boy Advance"
    | "GBA" -> Platform.GBA
    | "iOS" -> Platform.IOS
    | "Xbox"
    | "XBOX" -> Platform.Xbox
    | "GameCube"
    | "GC" -> Platform.GameCube
    | "Nintendo 64"
    | "N64" -> Platform.N64
    | "Dreamcast" -> Platform.Dreamcast
    | "STA"
    | "Stadia" -> Platform.Stadia
    | "XBSX"
    | "Xbox Series X" -> Platform.XboxSeriesX
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
      Uri: Uri
      Developer: String
      Publisher: String
      Rating: String }


let private score typeFunc text =
    match text with
    | "tbd" -> None
    | n -> n |> typeFunc |> Some

let private metaScore = score uint
let private userScore = score float

let private processResult (result: HtmlNode) =
    let title =
        result.ElementText ".product_title > a"
        |> Option.get

    let platform =
        result.ElementText ".platform"
        |> Option.get
        |> platfromFromString

    let score =
        result.ElementText ".metascore_w"
        |> Option.get
        |> metaScore

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

        let metaScore =
            doc.ElementText ".metascore_w.xlarge"
            |> Option.bind metaScore

        let userScore =
            doc.ElementText ".metascore_w.user"
            |> Option.bind userScore

        let title = doc.ElementText "h1" |> Option.get

        let publisher =
            doc.ElementText ".publisher > span > a"
            |> Option.get

        let developer =
            doc.ElementText ".developer > .data"
            |> Option.defaultValue ""

        let platform =
            doc.ElementText ".platform"
            |> Option.get
            |> platfromFromString

        let rating =
            doc.ElementText ".product_rating > .data"
            |> Option.defaultValue ""

        return
            { Title = title
              Publisher = publisher
              Developer = developer
              Rating = rating
              Platform = platform
              MetaScore = (metaScore |> Option.toNullable)
              UserScore = (userScore |> Option.toNullable)
              Uri = gameUri }
    }
