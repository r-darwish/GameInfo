module Async

let ParallelThrottle throttle workflows = Async.Parallel(workflows, throttle)

let protect f x =
    async {
        try
            let! result = f x
            return Ok(result)
        with ex -> return Error(ex)
    }
