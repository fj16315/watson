# Code Standards

In general, try and keep the code looking the same throughout.
What follows is the import parts, but for full details see the following guide.
https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/index

## Naming
Everything should use PascalCase (capital letters for each word including the
first, and no underscores) with the following exceptions:
- Variables and function parameters use camelCasing

## Layout
Lines start with two spaces and no tabs. You can configure visual studio to do
this for you.
One statement per line.
Opening and closing parentheses appear on their own lines.
Try and use var for local variables where the type is clear, eg. when using new.

## Documentation
All 'public' code should be documented with C#'s annoying XML tags. Information
about that be found here:
https://docs.microsoft.com/en-us/dotnet/csharp/codedoc
