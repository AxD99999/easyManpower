using System.Drawing.Imaging;

public class PathConstant
{
    public static string file_loc(string ComID)
    {

         return "https://testing.esnep.com/assets/manpower/" + ComID + "/";    //LIVE URL
        //return "C:\\Users\\Anamol\\Pictures\\EasySoft" + ComID + "\\";
    }

    public static string file_url()
    {

        return "https://testing.esnep.com/assets/manpower/";    //LIVE URL
        //return "C:\\Users\\Anamol\\Pictures\\EasySoft\\";
    }
}