namespace FizzBuzzWhizz.Sample;

public class FizzBuzzManualManyModuloUpFront
{
    public string Identity(long n)
    {
        if (n == 0)
        {
            return "0";
        }

        return (n % 5, n % 3) switch
        {
            (0, 0) => "FizzBuzz",
            (0, _) => "Buzz",
            (_, 0) => "Fizz",
            _ => n.ToString()
        };
    }
}
