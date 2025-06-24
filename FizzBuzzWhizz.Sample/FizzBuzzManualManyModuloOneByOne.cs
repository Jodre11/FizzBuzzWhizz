namespace FizzBuzzWhizz.Sample;

public class FizzBuzzManualManyModuloOneByOne
{
    public string Identity(long n)
    {
        if (n == 0) return "0";
        if (n % 3 == 0 && n % 5 == 0) return "FizzBuzz";
        if (n % 3 == 0) return "Fizz";
        if (n % 5 == 0) return "Buzz";
        return n.ToString();
    }
}
