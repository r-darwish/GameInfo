namespace GameInfo.Powershell

open System.Management.Automation
open FSharpPlus

[<Cmdlet("Find", "MetaCritic")>]
[<OutputType(typeof<MetaCritic.FindResult>)>]
type FindMetaCritic() =
    inherit PSCmdlet()

    [<ValidateNotNullOrEmpty>]
    [<Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)>]
    member val Title: string [] = [||] with get, set

    [<Parameter>]
    member val Platform = MetaCritic.Platform.All with get, set

    [<Parameter>]
    member val Throttle: int = 5 with get, set

    override x.ProcessRecord() =
        base.ProcessRecord()

        (Seq.collect Result.get (x.Title
        |> Array.map (Async.protect (MetaCritic.find x.Platform))
        |> Async.ParallelThrottle x.Throttle
        |> Async.RunSynchronously
        |> Seq.ofArray
        |> Seq.filter
            (fun result ->
                match result with
                | Ok (result) -> true
                | Error (ex) ->
                    x.WriteError(ErrorRecord(ex, "MetaCritic error", ErrorCategory.InvalidResult, System.Nullable()))
                    false)))
        |> Seq.iter x.WriteObject
