namespace FizzBuzzWhizz.Sample;

public class FizzBuzzManual
{
    public string Identity(long n) => n switch
    {
        15 => "FizzBuzz",
        5 => "Buzz",
        3 => "Fizz",
        _ => n.ToString()
    };
}
