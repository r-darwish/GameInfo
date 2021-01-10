namespace GameInfo.Powershell.MetaCritic

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
