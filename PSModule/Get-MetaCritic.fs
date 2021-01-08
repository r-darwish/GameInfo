namespace GameInfo.Powershell

open System.Management.Automation
open FSharpPlus
open System

[<Cmdlet("Get", "MetaCritic", DefaultParameterSetName = "Results")>]
[<OutputType(typeof<string>)>]
type GetMetaCritic() =
    inherit PSCmdlet()

    [<ValidateNotNullOrEmpty>]
    [<Parameter(ValueFromPipelineByPropertyName = true, Position = 0, ParameterSetName = "Results")>]
    member val Uri: Uri [] = [||] with get, set

    [<ValidateNotNullOrEmpty>]
    [<Parameter(ValueFromPipeline = true, Position = 0, ParameterSetName = "Flags")>]
    member val Game: string [] = [||] with get, set

    [<Parameter>]
    member val Throttle: int = 5 with get, set

    [<Parameter(ParameterSetName = "Flags", Mandatory = true)>]
    member val Platform = MetaCritic.Platform.All with get, set

    override x.ProcessRecord() =
        base.ProcessRecord()

        let input =
            if x.Game.Length > 0 then
                let gameUri = MetaCritic.gameUri x.Platform

                if x.Platform = MetaCritic.Platform.All then
                    do failwith "Cannot use the All platform with the command"

                x.Game |> Array.map gameUri
            else
                x.Uri

        input
        |> Array.map MetaCritic.get
        |> Async.ParallelThrottle x.Throttle
        |> Async.RunSynchronously
        |> Array.iter x.WriteObject
