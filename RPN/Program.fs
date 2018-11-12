open System.Text.RegularExpressions

let fragmentPattern = @"(\d+)\s+(\d+)\s+([+-/*xX])"

let getOp = function
    | "+" -> (+)
    | "-" -> (-)
    | "/" -> (/)
    | "X"
    | "x"
    | "*" -> (*)
    | _ as op -> failwith (sprintf "Invalid operation: %s" op)

let (|Regex|_|) pattern equation =
    let matches = Regex.Match(equation, pattern)
    if matches.Success then Some(List.tail [ for g in matches.Groups -> g.Value ])
    else None

let rec solve equation =
    match equation with
    | Regex fragmentPattern [num1; num2; op] ->
        let result = (getOp op) (int num1) (int num2)
        let newEquation = Regex.Replace(equation, fragmentPattern, sprintf "%i" result)
        solve newEquation
    | Regex @"^(\d+)$" [number] -> Some number
    | _ -> None

[<EntryPoint>]
let main argv =
    let solution =
        argv
        |> Array.toSeq
        |> String.concat " "
        |> solve

    match solution with
    | Some solution -> printfn "%s" solution
    | None -> printfn "An error has occured"

    0