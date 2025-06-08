# FizzBuzzWhizz

**FizzBuzzWhizz** is a C# source generator that automatically creates methods for the classic FizzBuzz problem (and similar patterns) using a simple attribute. By annotating your partial class with the `FizzBuzzWhizz` attribute, you can generate methods that return custom strings for numbers divisible by specified values.

## Repository

[GitHub - Jodre11/FizzBuzzWhizz](https://github.com/Jodre11/FizzBuzzWhizz)

## Purpose

This project demonstrates how to use C# source generators to reduce boilerplate code for repetitive logic, such as FizzBuzz. It allows you to define the rules declaratively via an attribute, and the generator will create the necessary methods at compile time.

## Usage

1. **Install the NuGet Package**

   Add the `FizzBuzzWhizz` package to your project:

2. **Annotate Your Partial Class**

   Use the `FizzBuzzWhizz` attribute on a partial class to specify the words and divisors:

   ```csharp
   using FizzBuzzWhizz;

   [FizzBuzzWhizz("Fizz", "3", "Buzz", "5")]
   public partial class FizzBuzz
   {
   }
   ```

    - The first argument is the word for the first divisor (e.g., `"Fizz"` for 3).
    - The second argument is the first divisor (e.g., `"3"`).
    - The third argument is the word for the second divisor (e.g., `"Buzz"` for 5).
    - The fourth argument is the second divisor (e.g., `"5"`").

3. **Use the Generated Method**

   After building your project, the generator will add an `Identity` method to your class:

   ```csharp
   var fizzBuzz = new FizzBuzz();
   Console.WriteLine(fizzBuzz.Identity(15)); // Output: FizzBuzz
   Console.WriteLine(fizzBuzz.Identity(3));  // Output: Fizz
   Console.WriteLine(fizzBuzz.Identity(5));  // Output: Buzz
   Console.WriteLine(fizzBuzz.Identity(7));  // Output: 7
   ```

## How It Works

- The source generator scans your code for partial classes decorated with the `FizzBuzzWhizz` attribute.
- For each matching class, it generates an `Identity(long value)` method at compile time.
- The generated method checks if the input value is divisible by the specified divisors:
    - If divisible by both, it returns the concatenated words.
    - If divisible by only one, it returns the corresponding word.
    - If divisible by neither, it returns the number as a string.

## Build & Run

1. **Clone the Repository**

   ```sh
   git clone https://github.com/Jodre11/FizzBuzzWhizz.git
   cd FizzBuzzWhizz
   ```
2. **Build the Project**

   The tests and sample require [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0).

   ```sh
   dotnet build
   ```
3. **Run the Tests**

   ```sh
   cd FizzBuzzWhizz.Tests
   dotnet test
   ```
4. **Sample**

    The sample project demonstrates how to use the source generator in a console application.
You can run it to see the generated methods in action.
   
## Contributing

Contributions are welcome! Please open issues or pull requests for improvements or bug fixes.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
