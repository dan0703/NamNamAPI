namespace Newtonsoft.utility;

public class GenerateRandomID{
    public static string GenerateID()
    {
        Random random = new Random();
        int length = 10;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Caracteres permitidos en el ID
        char[] id = new char[length];

        for (int i = 0; i < length; i++)
        {
            id[i] = chars[random.Next(chars.Length)];
        }

        return new string(id);
    }
}