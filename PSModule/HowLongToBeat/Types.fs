namespace GameInfo.Powershell.HowLongToBeat

open System

type Record =
    { Title: string
      MainStory: Nullable<TimeSpan>
      MainAndExtra: Nullable<TimeSpan>
      Completioninst: Nullable<TimeSpan> }
