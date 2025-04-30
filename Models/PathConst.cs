using System.Drawing.Imaging;

public class PathConstant
{
    public static string file_loc(string ComID)
    {

        // return "E:\\easysoftware\\easy School\\easysch\\" + ComID + "\\";    //LIVE URL
        return "C:\\Users\\Anamol\\Pictures\\EasySoft" + ComID + "\\";
    }

    public static string file_url()
    {

        // return "https://esnep.com/easysch/" + ComID + "/";    //LIVE URL
        return "C:\\Users\\Anamol\\Pictures\\EasySoft\\";
    }
}