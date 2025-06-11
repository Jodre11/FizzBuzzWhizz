namespace FizzBuzzWhizz.Sample;

public class FizzBuzzManualManyModuloOneByOne
{
    public string Identity(long n)
    {
        if (n % 15 == 0) return "FizzBuzz";
        if (n % 3 == 0) return "Fizz";
        return n % 5 == 0 ? "Buzz" : n.ToString();
    }
}
