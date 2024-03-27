
using QueenShop;


class Program
{


    static void Main(string[] args)
    {
        try
        {

            QueensShop queenShop = new QueensShop();
            queenShop.Menu();

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }



}






