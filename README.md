# ![SimpleOption](https://raw.githubusercontent.com/alterius/SimpleOption/master/logo.png) SimpleOption
A simple and easy to use option type for C#.

## What the hell is this?

SimpleOption is a strongly typed alternative to null that helps to avoid null-reference exceptions, model your data more explictly and
cut down on manual null checks.

## How do I get it?

SimpleOption is available via NuGet package manager:

```
PM> Install-Package Alterius.SimpleOption
```

Or via .NET CLI:

```
> dotnet add package Alterius.SimpleOption
```

Or visit [https://www.nuget.org/packages/Alterius.SimpleOption](https://www.nuget.org/packages/Alterius.SimpleOption)

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

Usage as a method return type:

```csharp
public Option<string> GetString(object obj)
{
    if (obj == null)
    {
        return new ArgumentNullException(nameof(obj));
    }
    
    var str = _someRepo.GetString(obj);
    
    if (str == null)
    {
        return Option.None<string>();
    }

    return str;
}
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

Please bear in mind that accessing the value of Exception (e) can result in a NullReferenceException being thrown if there is no exception passed to the Option and the option is none.

Usage as a method parameter:

```csharp
public bool HasString(Option<object> obj)
{
    return obj.Match(
        some => string.IsNullOrEmpty(some.ToString()),
        none => false);
}
```
