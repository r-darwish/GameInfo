namespace GameInfo.Powershell

open System.Management.Automation
open FSharpPlus
open System

[<Cmdlet("Get", "MetaCritic", DefaultParameterSetName = "Url")>]
[<OutputType(typeof<MetaCritic.GameData>)>]
type GetMetaCritic() =
    inherit PSCmdlet()

    [<ValidateNotNullOrEmpty>]
    [<Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0, ParameterSetName = "Url")>]
    member val Uri: Uri [] = [||] with get, set

    [<ValidateNotNullOrEmpty>]
    [<Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, ParameterSetName = "GameTitle")>]
    member val Title: string [] = [||] with get, set

    [<Parameter(ParameterSetName = "GameTitle", Mandatory = true)>]
    member val Platform = MetaCritic.Platform.All with get, set

    [<Parameter>]
    member val Throttle: int = 5 with get, set

    override x.ProcessRecord() =
        base.ProcessRecord()

        let input =
            if x.Title.Length > 0 then
                let gameUri = MetaCritic.gameUri x.Platform

                if x.Platform = MetaCritic.Platform.All then
                    do failwith "Cannot use the All platform with the command"

                x.Title |> Array.map gameUri
            else
                x.Uri

        input
        |> Array.map (Async.protect MetaCritic.get)
        |> Async.ParallelThrottle x.Throttle
        |> Async.RunSynchronously
        |> Array.iter
            (fun result ->
                match result with
                | Ok (result) -> x.WriteObject result
                | Error (ex) ->
                    x.WriteError(ErrorRecord(ex, "MetaCritic error", ErrorCategory.InvalidResult, Nullable())))
