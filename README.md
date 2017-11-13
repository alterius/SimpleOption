# SimpleOption
A simple option type.

## What the hell is this?

SimpleOption is a strongly typed alternative to null that helps to avoid null-reference exceptions, model your data more explictly and
cut down on manual null checks.

## How do I use it?

### Using the library

To use SimpleOption simply import the following namespace:

```csharp
using Alterius.SimpleOption;
```

### Initialising an instance of the option type

Using static constructors:

```csharp
var none = Option.None<string>();
var noneWithException = Option.None<string>(new Exception());
var some = Option.Some("Something");
```

Using implicit casting:

```csharp
Option<string> option;

option = (string)null;
option = new Exception();
option = "Something";
```

### Retrieving values

A basic example when retuning an IActionResult in a WebApi controller:

```csharp
return option.Match<IActionResult>(
    some => Ok(some),
    NotFound());
```

Taking advantage of the exception option:

```csharp
return option.Match<IActionResult>(
    some => Ok(some),
    e => {
        if (e is NotFoundException) return NotFond();
        return BadRequest();
    });
```

Please bear in mind that when accessing the value of the Exception (e) that this can result in a NullReferenceException being thrown if there was no exception passed to the Option and the option is none.
