# Belizas
Belizas is a simple .NET library for truth table calculus.

### Index
1. [Usage](#usage)
 1. [How to start](#how-to-start)
 2. [Properties of the TruthTable class](#properties-of-the-truthtable-class)
 3. [Methods of the TruthTable class](#methods-of-the-truthtable-class)
 4. [Exceptions](#exceptions)
2. [To-do list](#to-do-list)
3. [License](#license)

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
* `Char Not` *(get and set)*: The "not" operator. Defaults to `'`.
* `Char And` *(get and set)*: The "and" operator. Defaults to `.`.
* `Char Or` *(get and set)*: The "or" operator. Defaults to `+`.
* `Char Xor` *(get and set)*: The "xor" operator. Defaults to `:`.
* `Char IfThen` *(get and set)*: The "if-then" operator. Defaults to `>`.
* `Char ThenIf` *(get and set)*: The "then-if" operator. Defaults to `<`.
* `Char IfAndOnlyIf` *(get and set)*: The "if-and-only-if" operator. Defaults to `-`.
* `Char OpeningBracket` *(get and set)*: The opening bracket. Defaults to `(`.
* `Char ClosingBracket` *(get and set)*: The closing bracket. Defaults to `)`.

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

##### Methods of the TruthTable class
The methods will be explained and followed by their parameters.
* `Boolean ValidateFormula`: Verifies if the formula is valid - if it is under all conditions to be a consistent truth table formula.
 * `String formula` *(optional)*: The formula to be validated. If not defined, validate the formula that rules the instance.
* `static Boolean ValidateFormula`: Verifies if the formula is valid. A static version of the `ValidateFormula` above, to make a validation before making a instance.
 * `String formula`: The formula to be validated.
 * `IEnumerable<Char> characters` *(optional)*: The caracters that will represent the operators (same rules of the "characters" in the constructor).
* `void Calculate`: Necessary to calculate the values of the arguments and the expressions. The `Arguments`, `ArgumentsValues`, `Expressions` and `ExpressionsValues` properties are only populated after this method is called. **Call this method after setting the formula!**
* `IList<Char> EnumerateOperators`: Returns a list with all the operators.
 * `Boolean includeNot` *(optional)*: If the "not" operator should be included in the list. Defaults to `true`.
 * `Boolean includeBrackets` *(optional)*: If the brackets should be included in the list. Defaults to `true`.
* `Boolean IsAnOperator`: Verifies if the character passed is an operator.
 * `Char character`: The character to be verified.
 * `Boolean includeNot` *(optional)*: If the "not" operator should be counted in the verification. Defaults to `true`.
 * `Boolean includeBrackets` *(optional)*: If the "not" operator should be counted in the verification. Defaults to `true`.
* `String ToString`: Converts the table to a text.

The following method is under construction (in the branch "test").
* `ToHtmlTable`: Converts the truth table to a HTML code.
 * `Object tableAttributes` *(optional)*: The attributes of the tag "table".
 * `Object theadAttributes` *(optional)*: The attributes of the tag "thead".
 * `Object tbodyAttributes` *(optional)*: The attributes of the tag "tbody".
 * `Object trAttributes` *(optional)*: The attributes of the tag "tr".
 * `Object thAttributes` *(optional)*: The attributes of the tag "th".
 * `Object tdAttributes` *(optional)*: The attributes of the tag "td".

The usage of the parameters is exactly the same as in the ASP.NET HTML helpers. For example, something like this:
```c#
table.ToHtmlTable(new { foo = "bar", nhan = "deru" });
```
Will return this:
```html
<table foo="bar" nhan="deru">
...
</table>
```
If some tags will have attributes and others won't, I highly recommend you to use named parameters on it, but you can just pass `null` to some parameters. See below both ways:
```c#
// Named parameters.
table.ToHtmlTable(
    tableAttributes: new { note_the_underline = "now-it-is-a-hyphen" },
    trAttributes: new { @class = "cool" },
    tdAttributes: new { vai = "curintia" }
);

// Null parameters.
table.ToHtmlTable(new { note_the_underline = "now-it-is-a-hyphen" }, null, null, new { @class ="cool" }, null, new { vai = "curintia" });
```
Which results to this:
```html
<table note-the-underline="now-it-is-a-hyphen">
    <thead>
        <tr class="cool">
            <th>...</th>
        </tr>
        <tr class="cool">
            <td var="curintia">...</td>
        </tr>
    </thead>
</table>
```

Or, besides the example above, you can use this overload:
* `ToHtmlTable`: Converts the truth table to a HTML code.
 * `IDictionary<String, Object> tableAttributes` *(optional)*: The attributes of the tag "table".
 * `IDictionary<String, Object> theadAttributes` *(optional)*: The attributes of the tag "thead".
 * `IDictionary<String, Object> tbodyAttributes` *(optional)*: The attributes of the tag "tbody".
 * `IDictionary<String, Object> trAttributes` *(optional)*: The attributes of the tag "tr".
 * `IDictionary<String, Object> thAttributes` *(optional)*: The attributes of the tag "th".
 * `IDictionary<String, Object> tdAttributes` *(optional)*: The attributes of the tag "td".

In practice:
```c#
var attributes = new Dictionary<String, Object>();
attributes.Add("foo", "bar");
attributes.Add("nhan", "deru");

table.ToHtmlTable(attributes);
```

##### Exceptions
* `InvalidFormulaException`: Only thrown in `Calculate` method if the formula is not valid. That's why it's highly recommendable to validate your formula before calculate.
* `TableNotCalculatedException`: Thrown when a porperty is called before the table was calculated, i.e. before the `Calculate` method is called and the "calculate" parameter in constructor is not set as true.
* `TooMuchArgumentsInTruthTableException`: When the arguments and its values pass the limit of the memory.
* `TooMuchExpressionsInTruthTableException`: When the expressions and its values pass the limit of the memory.
* `TooMuchInformationInTruthTableException`: When another thing in the table pass the limit of the memory.

### To-do list
* Add comments and XML docs in all code.
* Make unit tests for every method.
* Finish read me file.
* TruthTable method to convert the table data to a HTML table (in construcion in branch "test").
* Formula D.

### License
This project is released under the terms of the [MIT](http://opensource.org/licenses/MIT) license.
