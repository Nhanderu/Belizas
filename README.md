# Belizas (deprecated)

![Deprecated][badge-1-img]
[![License][badge-2-img]][badge-2-link]

Belizas is a simple .NET library for truth table calculus!

## Deprecated

Don't use it! I made this code when I was 17, on my freshman year, and I
haven't cared for it since then. It was my first open source project and
I'm keeping it just for my emotional attachment. So, again, don't use
this code! You've been warned!

## Index

1. [Usage](#usage)
  1. [How to start](#how-to-start)
  2. [TruthTable class' properties](#truthtable-class-properties)
  2. [TruthTable class' methods](#truthtable-class-methods)
  4. [Exceptions](#exceptions)
2. [To-do list](#to-do-list)
3. [License](#license)

## Usage

### How to start

Just make an instance of TruthTable, pass the formula and calculate.
```csharp
var table = new TruthTable("a.b'+(a-c)'");
table.Calculate();
```

Or you can calculate automatically from the constructor:
```csharp
var table = new TruthTable("a.b", true);
```

### TruthTable class' properties

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

After setting an operator, the old character(s) will need to be removed
from the formula, otherwise it'll be considered invalid. See the
situation below:

```csharp
table.ValidateFormula("a+b.c"); //true

table.And = '$';
table.Or = '%';

table.ValidateFormula("a+b.c"); //false
```

All the following properties are only set by the `Calculate` method, but
`Formula`.

* __`String` Formula__ *(get and set)*
  The formula that generated the table. Everytime it is set, the other
  properties are set as `null` and can only be updated when `Calculate`
  method is called.

* __`IList<Char>` Arguments__ *(get)*
  The arguments of the table (only letters, e.g. "a").

* __`Boolean[,]` ArgumentsValues__ *(get)*
  The binary values of the arguments.

* __`IList<String>` Expressions__ *(get)*
  The expressions in order to be calculated (e.g. "a+b").

* __`IList<Boolean[]>` ExpressionsValues__ *(get)*
  The binary values of the expressions.

### TruthTable class' methods

The methods will be explained and followed by their parameters.

* __`Boolean` ValidateFormula__
  Verifies if the formula is valid - if it is under all conditions to be
  a consistent truth table formula.
  Parameters:
  * __`String` formula__ *(optional)*
    The formula to be validated. If not defined, it validates the
    formula that rules the instance.

* __`static Boolean` ValidateFormula__
  Verifies if the formula is valid. A static version of the
  `ValidateFormula` above, to make a validation before making a
  instance.
  Parameters:
  * __`String` formula__
    The formula to be validated.
  * __`IEnumerable<Char>` characters__ *(optional)*
    The caracters that will represent the operators (same rules of the
    "characters" in the constructor).

* __`void` Calculate__
  Necessary to calculate the values of the arguments and the
  expressions. The `Arguments`, `ArgumentsValues`, `Expressions` and
  `ExpressionsValues` properties are only populated after this method is
  called. __Call this method after setting the formula!__

* __`IList<Char>` EnumerateOperators__
  Returns a list with all the operators.
  Parameters:
  * __`Boolean` includeNot__ *(optional)*
    If the "not" operator should be included in the list.
    Defaults to `true`.
  * __`Boolean` includeBrackets__ *(optional)*
    If the brackets should be included in the list.
    Defaults to `true`.

* __`Boolean` IsAnOperator__
  Verifies if the character passed is an operator.
  Parameters:
  * __`Char` character__
    The character to be verified.
  * __`Boolean` includeNot__ *(optional)*
    If the "not" operator should be counted in the verification.
    Defaults to `true`.
  * __`Boolean` includeBrackets__ *(optional)*
    If the brackets should be counted in the verification.
    Defaults to `true`.

* __`String` ToString__
  Converts the table to a text.

* __`String` ToHtmlTable__
  Converts the truth table to a HTML code.
  Parameters:
  * __`Object` tableAttributes__ *(optional)*
    The attributes of the tag `<table>`.
  * __`Object` theadAttributes__ *(optional)*
    The attributes of the tag `<thead>`.
  * __`Object` tbodyAttributes__ *(optional)*
    The attributes of the tag `<tbody>`.
  * __`Object` trAttributes__ *(optional)*
    The attributes of the tag `<tr>`.
  * __`Object` thAttributes__ *(optional)*
    The attributes of the tag `<th>`.
  * __`Object` tdAttributes__ *(optional)*
    The attributes of the tag `<td>`.

The usage of the parameters is exactly the same as in the ASP.NET HTML
helpers. For example, something like this:

```csharp
table.ToHtmlTable(new { foo = "bar", nhan = "deru" });
```

Will return this:

```html
<table foo="bar" nhan="deru">
...
</table>
```

If some tags will have attributes and others won't, you can use named
parameters or just pass `null` to some parameters. See below both ways:

```csharp
// Named parameters.
table.ToHtmlTable(
    tableAttributes: new { note_the_underline = "now-it-is-a-hyphen" },
    trAttributes: new { @class = "cool" },
    tdAttributes: new { vai = "curintia" }
);

// Null parameters.
table.ToHtmlTable(
  new { note_the_underline = "now-it-is-a-hyphen" },
  null,
  null,
  new { @class ="cool" },
  null,
  new { vai = "curintia" }
);
```

Which results to this:

```html
<table note-the-underline="now-it-is-a-hyphen">
    <thead>
        <tr class="cool">
            <th>...</th>
        </tr>
    </thead>
    <tbody>
        <tr class="cool">
            <td vai="curintia">...</td>
        </tr>
    </tbody>
</table>
```

Or, besides the example above, you can use this overload:

* __`String` ToHtmlTable__
  Converts the truth table to a HTML code.
  Parameters:
  * __`IDictionary<String, Object>` tableAttributes__ *(optional)*
    The attributes of the tag "table".
  * __`IDictionary<String, Object>` theadAttributes__ *(optional)*
    The attributes of the tag "thead".
  * __`IDictionary<String, Object>` tbodyAttributes__ *(optional)*
    The attributes of the tag "tbody".
  * __`IDictionary<String, Object>` trAttributes__ *(optional)*
    The attributes of the tag "tr".
  * __`IDictionary<String, Object>` thAttributes__ *(optional)*
    The attributes of the tag "th".
  * __`IDictionary<String, Object>` tdAttributes__ *(optional)*
    The attributes of the tag "td".

In practice:
```csharp
var attributes = new Dictionary<String, Object>();
attributes.Add("foo", "bar");
attributes.Add("nhan", "deru");

table.ToHtmlTable(attributes);
```

### Exceptions

* __InvalidFormulaException__
  Only thrown in `Calculate` method if the formula is not valid. That's
  why it's highly recommendable to validate your formula before
  calculate.
* __TableNotCalculatedException__
  Thrown when a porperty is called before the table was calculated, i.e.
  before the `Calculate` method is called and the "calculate" parameter
  in constructor is not set as true.
* __TooMuchArgumentsInTruthTableException__
  When the arguments and its values pass the limit of the memory.
* __TooMuchExpressionsInTruthTableException__
  When the expressions and its values pass the limit of the memory.
* __TooMuchInformationInTruthTableException__
  When another thing in the table pass the limit of the memory.

## To-do list

* Add comments and XML docs in all code
* Add a CSV file writer method
* Stop being lazy

## License

This project code is in the public domain. See the [LICENSE file][1].

### Contribution

Unless you explicitly state otherwise, any contribution intentionally
submitted for inclusion in the work by you shall be in the public
domain, without any additional terms or conditions.

[1]: ./LICENSE

[badge-1-img]: https://img.shields.io/badge/code-deprecated-critical?style=flat-square
[badge-2-img]: https://img.shields.io/github/license/Nhanderu/belizas?style=flat-square
[badge-2-link]: https://github.com/Nhanderu/belizas/blob/master/LICENSE
