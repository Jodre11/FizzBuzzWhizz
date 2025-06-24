using System.Text;

namespace FizzBuzzWhizz.Sample;

public class FizzBuzzManualRavenBeetle
{
    public string Identity(long n)
    {
        // Handle non-positive numbers by returning as string
        if (n <= 0) return n.ToString();

        StringBuilder sb = new();
        if (n % 3 == 0) sb.Append("Fizz");
        if (n % 5 == 0) sb.Append("Buzz");
        // Return the built string if any rules matched, otherwise n as string
        return sb.Length > 0 ? sb.ToString() : n.ToString();
    }
}
