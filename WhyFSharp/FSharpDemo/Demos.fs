﻿namespace WhyFSharp

module Demo01 =
    // Eliminate duplicates
    // Shows pattern matching is powerful
    // Shows it is generic by default
    // Shows it doesn't accept null
    // Shows it prefer recursive over loop 
    let rec compress =
        function
        | a :: (b :: _ as tail) ->
            // Instead of keep the first and ignore following repeated as I first though
            // It continue to see next until meet different element to keep the last one !
            if a = b then
                compress tail
            else
                a :: (compress tail)
        | x -> x

    let demo01 () =
        compress [ "a"; "a"; "a"; "a"; "b"; "c"; "c"; "a"; "a"; "d"; "e"; "e"; "e"; "e" ]

    let demo02 () = 
        compress [1; 1; 2; 2; 3; 3; 3; 3; 4]


module Demo02 = 
    // Shows the pipeline operator
    // Shows the lambda 
    // Shows again loop like for i = 0; i <= 100; i++ is barely used in F#
    // The point is F# (FP) focus on the transformation of input to output
    let demo() = 
        let fizzBuzz x = 
            match x with 
            | _ when x % 3 = 0 && x % 5 = 0 -> 
                "FizzBuzz"
            | _ when x % 3 = 0 -> 
                "Fizz"
            | _ when x % 5 = 0 -> 
                "Buzz"
            | _ ->
                $"{x}"

        [1..100]
        |> List.iter (fun x -> fizzBuzz x |> printfn "%A")


module Demo03 = 
    // Shows DU used as inheritance. Inheritance usually can be considered as composition
    // Shows explict convert from int to float
    // Show they are just functions
    type Point = {x: int; y: int}

    type Shape = 
        | Rectangle of width: int * height: int * point: Point
        | Square of edge: int * point: Point
        | Circle of radius: int * point: Point

    let rectangle = Rectangle (20, 10, {x = 1; y = 2})
    let squre = Square (10, {x = 0; y = 0})
    let circle = Circle (10, {x = 0; y = 2})

    let area shape = 
        match shape with 
        | Rectangle (x, y, _) -> float (x * y )
        | Square (e, _) -> float (e * e)
        | Circle (r, _) -> 3.14 * float r * float r 

    let demo() = 
        [rectangle; squre; circle]
        |> List.map (fun x -> area x)
        |> List.sum


module Demo04 = 
    // Shows DU as better domain modeling
    type Player = {name: string; score: int}

    type Game = 
        | NotStarted
        | InProcess of Player * Player 
        | Finished of Player

    let gameStatus (game: Game) = 
        match game with 
        | NotStarted -> "Game not started: Waiting for players to join"
        | InProcess (player01, player02) -> 
            $"Game is on: {player01} vs {player02}"
        | Finished player -> 
            $"Game is finished: {player} is the winner!"

    let demo() = 
        let game01 = NotStarted
        let game02 = InProcess ({name = "player01"; score = 0}, {name = "player02"; score = 0})
        let game03 = Finished {name = "player01"; score = 100}
        printfn $"{gameStatus game01}"
        printfn $"{gameStatus game02}"
        printfn $"{gameStatus game03}"

        

/// Something F# feature that is hard to achieve in C#
// Active pattern 01
module Demo05 = 
    // What is active pattern?
    // “Active patterns” means the pattern can be parsed or detected dynamically.
    open System.Text.RegularExpressions

    // Here, we defined FirstRegexGroup as partial active pattern 
    // which could return Some value or None.
    let (|FirstRegexGroup|_|) pattern input = 
            let m = Regex.Match(input, pattern)
            if (m.Success) then Some m.Groups.[1].Value else None 

    // Here, in this function we pattern matching dynamically with different input 
    // and based on result (Some value or None) we do further processing.
    let extractHost str = 
        match str with 
        | FirstRegexGroup "http://(.*?)/(.*)" host -> 
            host 
        | FirstRegexGroup ".*?@(.*)" host -> 
            host
        | _ -> str

    let demo() = 
        extractHost "http://google.com/test" |> printfn "%A"
        extractHost "alice@hotmail.com" |> printfn "%A"
        extractHost "unknown" |> printfn "%A"


// Active pattern 02 
module Demo06 = 
    
    type Temperature =
    | Celsius of float
    | Fahrenheit of int

    // Define an active pattern
    let (|IsWarm|IsCold|) temperature = 
        match temperature with 
        | Celsius c when c > 25.0 -> IsWarm
        | Celsius _ -> IsCold
        | Fahrenheit f when f > 77 -> IsWarm
        | Fahrenheit _ -> IsCold

    // Use it dynamically
    let isItWarm temperature = 
        match temperature with 
        | IsWarm -> true 
        | IsCold -> false

    let demo () = 
        isItWarm (Celsius 32.0) |> printfn "%A"
        isItWarm (Fahrenheit 88) |> printfn "%A"