![SimpleOption](https://raw.githubusercontent.com/alterius/SimpleOption/master/logo.png)

# SimpleOption

A simple and easy to use option type for C#.

## What is it?

SimpleOption is a strongly typed alternative to null that helps to avoid null-reference exceptions, model your data more explictly and cut down on manual null checks.

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

### Getting started

To use SimpleOption reference ```Alterius.SimpleOption.dll``` and import the following namespace:

```csharp
using Alterius.SimpleOption;
```

### Initialising an instance of Option\<T>

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

Using ```Option<T>``` as a method return type:

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

Retrieving values is achieved by using the ```Match()``` method and its various overloads.

A basic example:

```csharp
int x = option.Match(
    some => some + 1,
    () => -1);
```

A good use of ```Option<T>``` is when retuning an ```IActionResult``` in a WebApi controller:

```csharp
return option.Match<IActionResult>(
    some => Ok(some),
    () => NotFound());
```

> Please note that in this example I'm declaring ```TResult``` explicitly, as ```Ok()``` and ```NotFound()``` do not return the same type, even though they both return an implementation ```IActionResult```. This is not necessary under normal circumstances when the return types are the same.

Making use of the exception option can allow you to pass and handle application faults without the cost of throwing exceptions:

```csharp
return option.Match<IActionResult>(
    some => Ok(some),
    e => {
        if (e is NotFoundException) return NotFond();
        return BadRequest();
    });
```

> Warning! Accessing the value of Exception (e) can result in a ```NullReferenceException``` if there is no exception passed to the option and the result is none.

Using ```Option<T>``` as a method parameter:

```csharp
public bool HasString(Option<object> obj)
{
    return obj.Match(
        some => string.IsNullOrEmpty(some.ToString()),
        () => false);
}
```
### Fluent interface (v1.0.0.1+)

A fluent interface is available as an alternative to the ```Match()``` method in version 1.0.0.1 and upwards:

```csharp
int x = option
    .Some(some => some + 1)
    .None(() => -1);
```

```csharp
return option
    .Some<IActionResult>(some => Ok(some))
    .None(() => NotFound());
```

```csharp
return option
    .Some(some => Ok(some))
    .None(e => {
        if (e is NotFoundException) return NotFond();
        return BadRequest();
    });
```

> It's not recommended to mix the fluent interface with the ```Match()``` method as it'll probably get confusing. Pick one style and stick with it.