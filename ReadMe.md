# Belizas

Belizas is a simple .NET library for truth table calculus.

## Index

1. [Usage](#usage)
 1. [How to start](#how-to-start)
 2. [Properties of the TruthTable class](#properties-of-the-truthtable-class)
 3. [Methods of the TruthTable class](#methods-of-the-truthtable-class)
 4. [Exceptions](#exceptions)
2. [To-do list](#to-do-list)
3. [License](#license)

## Usage

### How to start

Just make an instance of TruthTable, pass the formula and calculate.
```c#
var table = new TruthTable("a.b'+(a-c)'");
table.Calculate();
```

Or you can calculate automatically from the constructor:
```c#
var table = new TruthTable("a.b", true);
```

### Properties of the TruthTable class

There's a property for every operator (and the brackets).

* __`Char` Not__ *(get and set)*  
  The character that represents the "not" operator.  
  Defaults to `'`.

* __`Char` And__ *(get and set)*  
  The character that represents the "and" operator.  
  Defaults to `.`.

* __`Char` Or__ *(get and set)*  
  The character that represents the "or" operator.  
  Defaults to `+`.

* __`Char` Xor__ *(get and set)*  
  The character that represents the "xor" operator.  
  Defaults to `:`.

* __`Char` IfThen__ *(get and set)*  
  The character that represents the "if then" operator.  
  Defaults to `>`.

* __`Char` ThenIf__ *(get and set)*  
  The character that represents the "then if" operator.  
  Defaults to `<`.

* __`Char` IfAndOnlyIf__ *(get and set)*  
  The character that represents the "if and only if" operator.  
  Defaults to `-`.

* __`Char` OpeningBracket__ *(get and set)*  
  The character that represents the opening bracket.  
  Defaults to `(`.

* __`Char` ClosingBracket__ *(get and set)*  
  The character that represents the closing bracket.  
  Defaults to `)`.

You can change the values of the operators due to your necessities, e.g.:

```c#
table.OpeningBracket = '[';
table.ClosingBracket = '}';
```

All the following properties are only set by the `Calculate` method, but `Formula`.

* __`String` Formula__ *(get and set)*  
  The formula that generated the table. Everytime it is set, the other properties are set as `null` and can only be updated when `Calculate` method is called.

* __`IList<Char>` Arguments__ *(get)*  
  The arguments of the table (only letters, e.g. "a").

* __`Boolean[,]` ArgumentsValues__ *(get)*  
  The binary values of the arguments.

* __`IList<String>` Expressions__ *(get)*  
  The expressions in order to be calculated (e.g. "a+b").

* __`IList<Boolean[]>` ExpressionsValues__ *(get)*  
  The binary values of the expressions.

### Methods of the TruthTable class

The methods will be explained and followed by their parameters.

* __`Boolean` ValidateFormula__  
  Verifies if the formula is valid - if it is under all conditions to be a consistent truth table formula.  
  Parameters:
  * __`String` formula__ *(optional)*  
    The formula to be validated. If not defined, it validates the formula that rules the instance.

* __`static Boolean` ValidateFormula__  
  Verifies if the formula is valid. A static version of the `ValidateFormula` above, to make a validation before making a instance.  
  Parameters:
  * __`String` formula__  
    The formula to be validated.
  * __`IEnumerable<Char>` characters__ *(optional)*  
    The caracters that will represent the operators (same rules of the "characters" in the constructor).

* __`void` Calculate__  
  Necessary to calculate the values of the arguments and the expressions. The `Arguments`, `ArgumentsValues`, `Expressions` and `ExpressionsValues` properties are only populated after this method is called. __Call this method after setting the formula!__

* __`IList<Char>` EnumerateOperators__  
  Returns a list with all the operators.  
  Parameters:
  * `Boolean` includeNot *(optional)*  
    If the "not" operator should be included in the list.  
    Defaults to `true`.
  * `Boolean` includeBrackets *(optional)*  
    If the brackets should be included in the list.  
    Defaults to `true`.

* __`Boolean` IsAnOperator__  
  Verifies if the character passed is an operator.  
  Parameters:
  * `Char` character *(optional)*  
    The character to be verified.
  * `Boolean` includeNot *(optional)*  
    If the "not" operator should be counted in the verification.  
    Defaults to `true`.
  * `Boolean` includeBrackets *(optional)*  
    If the "not" operator should be counted in the verification.  
    Defaults to `true`.

* __`String` ToString__  
  Converts the table to a text. 

The following method is under construction (in the branch "test").

* __ToHtmlTable__  
  Converts the truth table to a HTML code.  
  Parameters:
  * `Object` tableAttributes *(optional)*  
    The attributes of the tag `<table>`.
  * `Object` theadAttributes *(optional)*  
    The attributes of the tag `<thead>`.
  * `Object` tbodyAttributes *(optional)*  
    The attributes of the tag `<tbody>`.
  * `Object` trAttributes *(optional)*  
    The attributes of the tag `<tr>`.
  * `Object` thAttributes *(optional)*  
    The attributes of the tag `<th>`.
  * `Object` tdAttributes *(optional)*  
    The attributes of the tag `<td>`.

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

If some tags will have attributes and others won't, you can use named parameters or just pass `null` to some parameters. See below both ways:

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
            <td vai="curintia">...</td>
        </tr>
    </thead>
</table>
```

Or, besides the example above, you can use this overload:

* __ToHtmlTable__  
  Converts the truth table to a HTML code.  
  Parameters:
  * `IDictionary<String, Object>` tableAttributes *(optional)*  
    The attributes of the tag "table".
  * `IDictionary<String, Object>` theadAttributes *(optional)*  
    The attributes of the tag "thead".
  * `IDictionary<String, Object>` tbodyAttributes *(optional)*  
    The attributes of the tag "tbody".
  * `IDictionary<String, Object>` trAttributes *(optional)*  
    The attributes of the tag "tr".
  * `IDictionary<String, Object>` thAttributes *(optional)*  
    The attributes of the tag "th".
  * `IDictionary<String, Object>` tdAttributes *(optional)*  
    The attributes of the tag "td".

In practice:
```c#
var attributes = new Dictionary<String, Object>();
attributes.Add("foo", "bar");
attributes.Add("nhan", "deru");

table.ToHtmlTable(attributes);
```

#### Exceptions
* __InvalidFormulaException__  
  Only thrown in `Calculate` method if the formula is not valid. That's why it's highly recommendable to validate your formula before calculate.
* __TableNotCalculatedException__  
  Thrown when a porperty is called before the table was calculated, i.e. before the `Calculate` method is called and the "calculate" parameter in constructor is not set as true.
* __TooMuchArgumentsInTruthTableException__  
  When the arguments and its values pass the limit of the memory.
* __TooMuchExpressionsInTruthTableException__  
  When the expressions and its values pass the limit of the memory.
* __TooMuchInformationInTruthTableException__  
  When another thing in the table pass the limit of the memory.

## To-do list
* Add comments and XML docs in all code.
* Make unit tests for every method.
* Finish read me file.
* TruthTable method to convert the table data to a HTML table (in construcion in "test" branch).
* TruthTable method to convert the table data to a CSV text.
* Formula D.

## License
This project is released under the terms of the [MIT](http://opensource.org/licenses/MIT) license.
