﻿namespace AdventOfCode2022

module Day06 = 
    open Expecto

    module Part01 = 

        let allDifferent (elements: 'a list) = 
            let n = 
                elements
                |> Set.ofList
                |> Set.count
            n = elements.Length

        let processSignal (input: 'a list) = 
            let rec helper k rest = 
                match rest with 
                | x::(y::m::n::_ as tail) -> 
                    match allDifferent [x; y; m; n] with 
                    | true -> Some (k + 3)
                    | false -> helper (k + 1) tail
                | _ -> 
                    None
            match helper 0 input with 
            | Some n -> n + 1
            | None -> failwith "Failed to lock signal"

        let lockSignal input = 
            input 
            |> List.ofSeq
            |> processSignal

        let test01 =
            testCase "part 01 baseline"
            <| fun _ ->
                let sig01 = lockSignal "bvwbjplbgvbhsrlpgdmjqwftvncz"
                let sig02 = lockSignal "nppdvjthqldpwncqszvftbrmjlhg"
                let sig03 = lockSignal "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg"
                let sig04 = lockSignal "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"

                Expect.equal sig01 5 "01"
                Expect.equal sig02 6 "02"
                Expect.equal sig03 10 "03"
                Expect.equal sig04 11 "04"

        let test02 = 
            testCase "part 01 input"
            <| fun _ ->
                let input = 
                    AdventOfCode.Common.readInput "2022/input/day06.txt"
                    |> Array.head

                Expect.equal (lockSignal input) 1109 "part 01 input"


    [<Tests>]
    let tests = testList "Day06" [Part01.test01; Part01.test02]

