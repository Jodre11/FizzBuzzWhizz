namespace FizzBuzzWhizz.Sample;

public class FizzBuzzManualSingleModuloManyPatterns
{
    public string Identity(long n)
    {
        if (n == 0L)
        {
            return "0";
        }

        var d = n % 15;
        return d switch
        {
            0 => "FizzBuzz",
            3 or 6 or 9 or 12 => "Fizz",
            5 or 10 => "Buzz",
            _ => n.ToString()
        };
    }
}
