namespace LiveCoding.Domain

open System
open System.Collections.Generic
type Capacity = {
    Value : int
    }

type Bar = {
    Name : string
    Capacity : Capacity
    OpenedDays : DayOfWeek[]
}

type Booking = {
    Bar : Bar
    Date : DateTime
}

type Dev = {
    Name : string
    OnSite : DateTime[]
}

type IBarRepository =
    abstract member Get : unit -> IEnumerable<Bar>
type IDevRepository =
    abstract member Get : unit -> IEnumerable<Dev>
type IBookingRepository =
    abstract member GetUpcomingBookings : unit -> IEnumerable<Booking>
    abstract member Save : Booking -> unit
    
module BarFunctions =
    let bookAt (bar:Bar) (date:DateTime) =
        printfn "Bar booked: %s at %s" bar.Name (date.ToString())
        {Bar = bar; Date = date}
    let hasEnoughCapacityFor (bar: Bar) (nbDevs:int) =
        bar.Capacity.Value >= nbDevs
    let isOpenedOn (bar: Bar) (day: DayOfWeek) =
        Array.contains day bar.OpenedDays
    
    let bookBarIfPossible (bars: Bar list) (nbDevs:int) (bestDate:DateTime) : Option<Booking> =
        let availableBars = bars
                            |> List.filter (fun bar -> isOpenedOn bar bestDate.DayOfWeek)
                            |> List.filter (fun bar -> hasEnoughCapacityFor bar nbDevs)
        match availableBars with
        | [] -> None
        | _ -> Some (bookAt availableBars.Head bestDate)