# RPN
A calculator that uses [Infix Notation](https://en.wikipedia.org/wiki/Infix_notation)

## How to use?
- Clone the repo and run it in visual studio
- Clone the repo and run with the [dotnet CLI tools](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x)
- Copy and pase into [fsharp interactive](https://docs.microsoft.com/en-us/dotnet/fsharp/tutorials/fsharp-interactive/)

After you're set up you can call the `solve` function with the expression as the argument. You need to wrap the expression in quotations since the function takes in a single string
```fs
solve "1 + 1" // 2
```

If you're compiling the code into an exe, you can call it with the expression as the arguments. You do not need to wrap the expression in quotes.

Examples: 
```console
$ 9 x 9 # 81

$ (1 + 6 * 10) * 2  # 140  
```
or
```console
$ "(30 + 30 x (7 + 90) - 900)" # 2040

$ "10 % 6" # 4
```

# Operators

### Binary Operators
- `+` For addition
- `-` For subtraction
- `/` For division
- `*` For multiplication - You may also use `x`
- `%` For modulus 