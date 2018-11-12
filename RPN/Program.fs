open System.Text.RegularExpressions

let binaryOpPattern = @"(\d+(?:\.\d+)?)\s+(\d+(?:\.\d+)?)\s+([+-/*xX%])"
let unaryOpPattern = @"(\d+(?:\.\d+)?)\s(sqrt)"
let singleNumberPattern = @"^(\d+(?:\.\d+)?)$"

let getBinaryOp = function
    | "+" -> (+)
    | "-" -> (-)
    | "/" -> (/)
    | "X"
    | "x"
    | "*" -> (*)
    | "%" -> (%)
    | _ as op -> failwith (sprintf "Invalid operation: %s" op)

let getUnaryOp = function 
    | "sqrt" -> (sqrt)
    | _ as op -> failwith (sprintf "Invalid operation: %s" op)

let (|Regex|_|) pattern equation =
    let matches = Regex.Match(equation, pattern)
    if matches.Success then Some(List.tail [ for g in matches.Groups -> g.Value ])
    else None

let rec solve equation =
    match equation with
    | Regex unaryOpPattern [num; op] -> 
        let result = getUnaryOp op (float num)
        let newEquation = Regex.Replace(equation, unaryOpPattern, sprintf "%f" result)
        solve newEquation
    | Regex binaryOpPattern [num1; num2; op] ->
        let result = (getBinaryOp op) (float num1) (float num2)
        let newEquation = Regex.Replace(equation, binaryOpPattern, sprintf "%f" result)
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