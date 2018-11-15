open System.Text.RegularExpressions

let numberPattern = @"\d+(?:\.\d+)?"
let solvedPattern = sprintf @"^(%s)$" numberPattern
let expressionPattern = sprintf @"(%s)\s*([+-/*xX%%])\s*(%s)" numberPattern numberPattern

type Order =
| First
| Second

let orderOperationPattern = function
| First -> sprintf @"(%s)\s*([/*xX%%])\s*(%s)" numberPattern numberPattern
| Second -> sprintf @"(%s)\s*([+-])\s*(%s)" numberPattern numberPattern

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
| _ as op -> failwithf "Invalid operation: %s" op

let validateExpression expression =
    let openBracketCount = expression |> Seq.filter (fun c -> c = ')') |> Seq.length
    let closingBracketCount = expression |> Seq.filter (fun c -> c = '(') |> Seq.length

    match (openBracketCount, closingBracketCount) with
    | (openC, closeC) when openC = closeC -> expression
    | _ -> failwithf "Invalid expression - Bracket mismatch: %s" expression

/// Recursively solves an expression in the correct order
let rec calculate expression =
    // Solves a simple binary expression within a larger expression
    let solveFragment operator a b  =
        let solution = sprintf "%f" (getBinaryOp operator <| float a <| float b)
        Regex.Replace(expression, sprintf "(\s*%s\s*[%s]\s*%s\s*)" a operator b, solution)

    match expression with
    | Regex (orderOperationPattern First) [num1; op; num2]  ->
        calculate (solveFragment op num1 num2)
    | Regex (orderOperationPattern Second) [num1; op; num2] ->
        calculate (solveFragment op num1 num2)
    | Regex solvedPattern [solution] ->
        solution
    | _ -> failwithf "Could not solve expression: %s" expression

/// Gets and solves expressions in brackets from left to right
let rec solveExpression (expression: string) =
    let someEndIndex = expression |> Seq.tryFindIndex (fun c -> c = ')')

    match someEndIndex with
    | Some endIndex ->
        let startIndex = expression.[0..endIndex - 1] |> Seq.findIndexBack (fun c -> c = '(')
        let expressionFragment = expression.[startIndex + 1..endIndex - 1]
        let solution = calculate expressionFragment
        let newExpression = expression.Replace(sprintf "(%s)" expressionFragment, solution)
        solveExpression newExpression
    | None -> calculate expression

let solve = validateExpression >> solveExpression

[<EntryPoint>]
let main argv =
    let solution = argv |> String.concat " " |> solve
    printfn "%s" solution
    0