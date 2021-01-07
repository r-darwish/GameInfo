module Async

let ParallelThrottle throttle workflows = Async.Parallel(workflows, throttle)
