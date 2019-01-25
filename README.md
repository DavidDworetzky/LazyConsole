# LazyConsole
Dead Simple .Net Standard Console Library.

This console can be used to write boilerplate console test programs for testing library code.

# Usage

Simply call LazyConsole.StartConsole with the type parameter of a class containing methods that you wish to run from the console.

```
 LazyConsole.LazyConsole.StartConsole(typeof(mytype));
```