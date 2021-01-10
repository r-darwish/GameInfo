module Option

open System

let toNullable (opt: option<'T>): Nullable<'T> =
    match opt with
    | Some value -> Nullable(value)
    | None -> Nullable()
