namespace GameInfo.Powershell

open System.Management.Automation
open FSharpPlus

[<Cmdlet("Get", "MetaCritic", DefaultParameterSetName = "Results")>]
[<OutputType(typeof<string>)>]
type GetMetaCritic() =
    inherit PSCmdlet()

    [<ValidateNotNullOrEmpty>]
    [<Parameter(ValueFromPipeline = true, Position = 0, ParameterSetName = "Results")>]
    member val Result: MetaCritic.FindResult [] = [||] with get, set

    [<ValidateNotNullOrEmpty>]
    [<Parameter(ValueFromPipeline = true, Position = 0, ParameterSetName = "Flags")>]
    member val Game: string [] = [||] with get, set

    [<Parameter>]
    member val Throttle: int = 5 with get, set

    [<Parameter(ParameterSetName = "Flags")>]
    member val Platform = MetaCritic.Platform.All with get, set

    override x.ProcessRecord() =
        base.ProcessRecord()

        let input =
            if x.Game.Length > 0 then
                if x.Platform = MetaCritic.Platform.All then
                    do failwith "Cannot use the All platform with the command"

                x.Game
                |> Array.map (fun game -> (game, x.Platform))
            else
                x.Result
                |> Array.map (fun result -> (result.Title, result.Platform))

        input
        |> Array.map (fun (title, platform) -> MetaCritic.get platform title)
        |> Async.ParallelThrottle x.Throttle
        |> Async.RunSynchronously
        |> Array.iter x.WriteObject
