# Parsing
Parsing equations and things of the like with F#

# RPN
A parser for [Reverse Polish Notations](https://en.wikipedia.org/wiki/Reverse_Polish_notation)

## How to use?
- Clone the repo and run it in visual studio
- Clone the repo and run with the [dotnet CLI tools](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x)
- Copy and pase into [fsharp interactive](https://docs.microsoft.com/en-us/dotnet/fsharp/tutorials/fsharp-interactive/)

After you're set up you can call the `solve` function with the expression as your argument. You need to wrap the expression in quotations since the function takes in a single string
```fs
solve "100 sqrt 4 2 x %" // 2
```

If you're compiling the RPN parser into an exe, you can call it with the expression as the arguments. You do not need to wrap the expression in quotes.

Examples: 
```console
$ 8 8 x # 64

$ 100 sqrt 5 x # 50
```
or
```console
$ "10 10 x" # 100

$ "10 % 6" # 4
```
# Operators

### Binary Operators
- `+` For addition
- `-` For subtraction
- `/` For division
- `*` For multiplication - You may also use `x`
- `%` For modulus 

### Unary Operators
- `sqrt` To find the square root of a number
