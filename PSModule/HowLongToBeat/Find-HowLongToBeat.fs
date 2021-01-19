namespace GameInfo.Powershell.HowLongToBeat

open System.Management.Automation
open FSharpPlus

[<Cmdlet("Find", "HowLongToBeat")>]
[<OutputType(typeof<string>)>]
type FindHowLongToBeat() =
    inherit PSCmdlet()

    [<ValidateNotNullOrEmpty>]
    [<Parameter(ValueFromPipeline = true, Position = 0)>]
    member val Title: string [] = [||] with get, set

    [<Parameter>]
    member val Throttle: int = 5 with get, set

    override x.ProcessRecord() =
        base.ProcessRecord()

        x.Title
        |> Array.map HowLongToBeat.findGame
        |> Async.ParallelThrottle x.Throttle
        |> Async.RunSynchronously
        |> Seq.ofArray
        |> Seq.concat
        |> Seq.iter x.WriteObject
