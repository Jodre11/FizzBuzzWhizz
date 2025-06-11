using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace FizzBuzzWhizz.Tests;

public class FizzBuzzWhizzShould
{
    private static readonly VerifySettings VerifySettings;

    static FizzBuzzWhizzShould()
    {
        // Initialize Verify settings to use the C# language.
        VerifySettings = new VerifySettings();
        VerifySettings.UseDirectory("Snapshots");
    }

    private const string FizzBuzzClassText = @"
namespace TestNamespace;

[FizzBuzzWhizz.FizzBuzzWhizz(""Fizz"", ""3"", ""Buzz"", ""5"")]
public partial class FizzBuzz
{
}";

    private const string FizzBuzzWhizzedClassText = @"
namespace TestNamespace;

[FizzBuzzWhizz.FizzBuzzWhizz(""Fizz"", ""3"", ""Buzz"", ""5"", ""Whizz"", ""7"")]
public partial class FizzBuzzWhizzed
{
}";

    private const string SillyDuplicationsClassText = @"
namespace TestNamespace;

[FizzBuzzWhizz.FizzBuzzWhizz(""Two"", ""2"", ""Too"", ""2"", ""Four"", ""4"", ""For"", ""4"")]
public partial class SillyDuplications
{
}";

    private const string CollisionsClassText = @"
namespace TestNamespace;

[FizzBuzzWhizz.FizzBuzzWhizz(""Bang"", ""2"", ""Bang"", ""3"", ""Bang"", ""4"", ""Bang"", ""5"")]
public partial class Collisions
{
}";

    private static async Task<(object instance, MethodInfo method)> Generate(
        string classText,
        string className)
    {
        // Create an instance of the source generator.
        var generator = new FizzBuzzWhizz();

        // Source generators should be tested using 'GeneratorDriver'.
        var driver = CSharpGeneratorDriver.Create(generator);

        // We need to create a compilation with the required source code.
        var compilation = CSharpCompilation.Create(nameof(FizzBuzzWhizzShould),
            [CSharpSyntaxTree.ParseText(classText, cancellationToken: TestContext.Current.CancellationToken)],
            [
                // To support 'System.Attribute' inheritance, add reference to 'System.Private.CoreLib'.
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            ],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Run generators and retrieve all results.
        var runResult = driver.RunGenerators(compilation, TestContext.Current.CancellationToken).GetRunResult();

        // All generated files can be found in 'RunResults.GeneratedTrees'.
        var generatedAttributeSyntax = runResult.GeneratedTrees.Single(t => t.FilePath.EndsWith("FizzBuzzWhizzAttribute.g.cs"));
        var generatedClassSyntax = runResult.GeneratedTrees.Single(t => t.FilePath.EndsWith($"{className}.g.cs"));

        await Verify(new
        {
            Attribute = generatedAttributeSyntax.ToString(),
            Class = generatedClassSyntax.ToString()
        }, VerifySettings);

        // Add the generated tree to the compilation
        var updatedCompilation = compilation.AddSyntaxTrees(generatedAttributeSyntax, generatedClassSyntax);

        // Emit the assembly to a stream
        using var ms = new MemoryStream();
        var emitResult = updatedCompilation.Emit(ms, cancellationToken: TestContext.Current.CancellationToken);
        Assert.True(emitResult.Success, "Compilation failed");

        // Load the assembly
        ms.Seek(0, SeekOrigin.Begin);
        var assembly = Assembly.Load(ms.ToArray());

        // Get the generated type and instantiate it
        var type = assembly.GetType($"TestNamespace.{className}");
        Assert.NotNull(type);

        var instance = Activator.CreateInstance(type);
        Assert.NotNull(instance);

        var method = instance.GetType().GetMethod("Identity");
        Assert.NotNull(method);

        return (instance, method);
    }

    [Fact]
    public async Task GenerateFizzBuzz()
    {
        var (instance, method) = await Generate(FizzBuzzClassText, "FizzBuzz");

        Assert.Equal("0", method.Invoke(instance, [ 0L ] ));
        Assert.Equal("1", method.Invoke(instance, [ 1L ] ));
        Assert.Equal("Fizz", method.Invoke(instance, [ 3L ] ));
        Assert.Equal("Buzz", method.Invoke(instance, [ 5L ] ));
        Assert.Equal("FizzBuzz", method.Invoke(instance, [ 15L ] ));
        Assert.Equal("Fizz", method.Invoke(instance, [ 6L ] ));
        Assert.Equal("Buzz", method.Invoke(instance, [ 10L ] ));
        Assert.Equal("FizzBuzz", method.Invoke(instance, [ 30L ] ));
        Assert.Equal("31", method.Invoke(instance, [ 31L ] ));
    }

    [Fact]
    public async Task GenerateFizzBuzzWhizzed()
    {
        var (instance, method) = await Generate(FizzBuzzWhizzedClassText, "FizzBuzzWhizzed");

        Assert.Equal("0", method.Invoke(instance, [ 0L ] ));
        Assert.Equal("1", method.Invoke(instance, [ 1L ] ));
        Assert.Equal("Fizz", method.Invoke(instance, [ 3L ] ));
        Assert.Equal("Buzz", method.Invoke(instance, [ 5L ] ));
        Assert.Equal("Whizz", method.Invoke(instance, [ 7L ] ));
        Assert.Equal("FizzBuzz", method.Invoke(instance, [ 15L ] ));
        Assert.Equal("Fizz", method.Invoke(instance, [ 6L ] ));
        Assert.Equal("Buzz", method.Invoke(instance, [ 10L ] ));
        Assert.Equal("FizzWhizz", method.Invoke(instance, [ 21L ] ));
        Assert.Equal("FizzBuzz", method.Invoke(instance, [ 30L ] ));
        Assert.Equal("31", method.Invoke(instance, [ 31L ] ));
        Assert.Equal("FizzBuzzWhizz", method.Invoke(instance, [ 105L ] ));
        Assert.Equal("106", method.Invoke(instance, [ 106L ] ));
    }

    [Fact]
    public async Task GenerateSillyDuplications()
    {
        var (instance, method) = await Generate(SillyDuplicationsClassText, "SillyDuplications");

        Assert.Equal("0", method.Invoke(instance, [ 0L ] ));
        Assert.Equal("1", method.Invoke(instance, [ 1L ] ));
        Assert.Equal("TwoToo", method.Invoke(instance, [ 2L ] ));
        Assert.Equal("TwoTooFourFor", method.Invoke(instance, [ 4L ] ));
        Assert.Equal("5", method.Invoke(instance, [ 5L ] ));
        Assert.Equal("TwoTooFourFor", method.Invoke(instance, [ 8L ] ));
    }

    [Fact]
    public async Task GenerateCollisions()
    {
        var (instance, method) = await Generate(CollisionsClassText, "Collisions");

        Assert.Equal("0", method.Invoke(instance, [ 0L ] ));
        Assert.Equal("1", method.Invoke(instance, [ 1L ] ));
        Assert.Equal("Bang", method.Invoke(instance, [ 2L ] ));
        Assert.Equal("Bang", method.Invoke(instance, [ 3L ] ));
        Assert.Equal("BangBang", method.Invoke(instance, [ 4L ] ));
        Assert.Equal("BangBang", method.Invoke(instance, [ 6L ] ));
        Assert.Equal("7", method.Invoke(instance, [ 7L ] ));
        Assert.Equal("BangBang", method.Invoke(instance, [ 8L ] ));
        Assert.Equal("Bang", method.Invoke(instance, [ 9L ] ));
        Assert.Equal("BangBangBangBang", method.Invoke(instance, [ 120L ] ));
        Assert.Equal("121", method.Invoke(instance, [ 121L ] ));
    }
}
