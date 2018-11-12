open System.Text.RegularExpressions

let numberPattern = @"\d+(?:\.\d+)?"
let solvedPattern = sprintf @"^(%s)$" numberPattern
let expressionPattern = sprintf @"(%s)\s+([+-/*xX%%])\s+(%s)" numberPattern numberPattern
    
let (|Regex|_|) pattern equation =
    let matches = Regex.Match(equation, pattern)
    if matches.Success then Some(List.tail [ for g in matches.Groups -> g.Value ])
    else None

let getBinaryOp = function
| "+" -> (+)
| "-" -> (-)
| "/" -> (/)
| "X"
| "x"
| "*" -> (*)
| "%" -> (%)
| _ as op -> failwith (sprintf "Invalid operation: %s" op)
    
let calculate = function 
| Regex expressionPattern [num1; op; num2] -> sprintf "%i" (getBinaryOp op (int num1) (int num2))
| _ as invalid -> failwithf "Invalid expression: %s" invalid

/// Gets the innermost expression in brackets from left to right
let rec solve (expression: string) = 
    match expression with 
    | Regex solvedPattern [solution] -> 
        solution
    | _ ->
        let someEndIndex = expression |> Seq.tryFindIndex (fun c -> c = ')')

        match someEndIndex with
        | Some endIndex ->
            let someStartIndex = expression.[0..endIndex - 1] |> Seq.tryFindIndexBack (fun c -> c = '(')

            match someStartIndex with 
            | Some startIndex -> 
                let expressionFragment = expression.[startIndex + 1.. endIndex - 1]
                let solution = calculate expressionFragment
                let newExpression = expression.Replace(sprintf "(%s)" expressionFragment, solution)
                solve newExpression
            | None -> failwith "Cannot solve expression. Bracket mismatch"
        | None -> "Error" // No ending brackets found check for opening brackets or solve normally?

[<EntryPoint>]
let main argv =
    let solution = argv |> String.concat " " |> solve
    printfn "%s" (solve "(((1 + 7) * 8) + 6)")
    0 // return an integer exit code

