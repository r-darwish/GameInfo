namespace GameInfo.Powershell

open System.Management.Automation
open FSharpPlus

module Async =
    /// A wrapper around the throttling overload of Parallel to allow easier pipelining.
    let ParallelThrottle throttle workflows = Async.Parallel(workflows, throttle)

[<Cmdlet("Find", "MetaCritic")>]
[<OutputType(typeof<string>)>]
type FindMetaCritic() =
    inherit PSCmdlet()

    [<ValidateNotNullOrEmpty>]
    [<Parameter(ValueFromPipeline = true, Position = 0)>]
    member val Game: string [] = [||] with get, set

    [<Parameter>]
    member val Throttle: int = 5 with get, set

    override x.ProcessRecord() =
        base.ProcessRecord()

        x.Game
        |> Array.map MetaCritic.find
        |> Async.ParallelThrottle x.Throttle
        |> Async.RunSynchronously
        |> Seq.ofArray
        |> Seq.concat
        |> Seq.iter x.WriteObject
