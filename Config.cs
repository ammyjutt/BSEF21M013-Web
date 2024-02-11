using System.Configuration;




///////                       Created this class just to globally define "ConnectionString" so that we can avoid that 
///                           ugly string appear everywhere

public class Config
{

    public static string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"MyDB\";Integrated Security=True;";

}
