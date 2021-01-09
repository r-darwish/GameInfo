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

        x.Title
        |> Array.map (MetaCritic.find x.Platform)
        |> Async.ParallelThrottle x.Throttle
        |> Async.RunSynchronously
        |> Seq.ofArray
        |> Seq.concat
        |> Seq.iter x.WriteObject
