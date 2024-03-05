// See https://aka.ms/new-console-template for more information

Console.WriteLine("Enter integer: ");
if(int.TryParse(Console.ReadLine(), out var a))
{
    switch (a)
    {
        case > 0:
            Console.WriteLine("Int is bigger than 0");
            break;
        case < 0:
            Console.WriteLine("Int is smaller than 0");
            break;
        default:
            Console.WriteLine("Int equals 0");
            break;
    }
}
else
{
    Console.WriteLine("Invalid input. Please enter an integer");
}