open System.Text.RegularExpressions

let fragmentPattern = @"(\d+(?:\.\d+)?)\s+(\d+(?:\.\d+)?)\s+([+-/*xX%])"
let singleNumberPattern = @"^(\d+(?:\.\d+)?)$"
let sqrtPattern = @"(\d+)\ssqrt"

let getOp = function
    | "+" -> (+)
    | "-" -> (-)
    | "/" -> (/)
    | "X"
    | "x"
    | "*" -> (*)
    | "%" -> (%)
    | _ as op -> failwith (sprintf "Invalid operation: %s" op)

let (|Regex|_|) pattern equation =
    let matches = Regex.Match(equation, pattern)
    if matches.Success then Some(List.tail [ for g in matches.Groups -> g.Value ])
    else None

let rec solve equation =
    match equation with
    | Regex sqrtPattern [num] -> 
        let result = sqrt (float num)
        let newEquation = Regex.Replace(equation, sqrtPattern, sprintf "%f" result)
        solve newEquation
    | Regex fragmentPattern [num1; num2; op] ->
        let result = (getOp op) (float num1) (float num2)
        let newEquation = Regex.Replace(equation, fragmentPattern, sprintf "%f" result)
        solve newEquation
    | Regex singleNumberPattern [number] -> Some number
    | _ -> None

[<EntryPoint>]
let main argv =
    let solution = argv |> String.concat " " |> solve

    match solution with
    | Some solution -> printfn "%s" solution
    | None -> printfn "An error has occured"

    0