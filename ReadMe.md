# Belizas
Belizas is a simple .NET library for truth table calculus.

### Usage
##### How to start
Just make an instance of TruthTable, pass the formula and calculate.
```c#
var table = new TruthTable("a.b'+(a-c)'");
table.Calculate();
```
Or you can calculate automatically from the constructor:
```c#
var table = new TruthTable("a.b", true);
```

##### Properties of the TruthTable class
There's a property for every operator (and the brackets).
* `Char Not` *(get and set)*: The "not" operator. Default to `'`.
* `Char And` *(get and set)*: The "and" operator. Default to `.`.
* `Char Or` *(get and set)*: The "or" operator. Default to `+`.
* `Char Xor` *(get and set)*: The "xor" operator. Default to `:`.
* `Char IfThen` *(get and set)*: The "if-then" operator. Default to `>`.
* `Char ThenIf` *(get and set)*: The "then-if" operator. Default to `<`.
* `Char IfAndOnlyIf` *(get and set)*: The "if-and-only-if" operator. Default to `-`.
* `Char OpeningBracket` *(get and set)*: The opening bracket. Default to `(`.
* `Char ClosingBracket` *(get and set)*: The closing bracket. Default to `)`.

You can change the values of the operators due to your necessities, e.g.:
```c#
table.OpeningBracket = '[';
table.ClosingBracket = Console.Read();
```

All the following properties are only set by the `Calculate` method, but `Formula`.
* `String Formula` *(get and set)*: The formula that generated the table. Everytime it is set, the other properties are set as `null` and can only be updated when `Calculate` method is called.
* `IList<Char> Arguments` *(get)*: The arguments of the table (only letters, e.g. "a").
* `Boolean[,] ArgumentsValues` *(get)*: The binary values of the arguments.
* `IList<String> Expressions` *(get)*: The expressions in order to be calculated (e.g. "a+b").
* `IList<Boolean[]> ExpressionsValues` *(get)*: The binary values of the expressions.

### To-do list
* Add comments and XML docs in all code.
* Finish tests.
* Finish read me file.
* TruthTable method to convert the table data to a HTML table (in construcion in "test" branch).
* Add a exception for invalid character set to an operator.
* Formula D.

### Licence
This project is released under the terms of the [MIT](http://opensource.org/licenses/MIT) license.