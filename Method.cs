using System.Text;
namespace EGA_lab4;
public abstract class Method
{

    static string TAB = "      ";
    static string END_TAB = "    > ";
    public static BinaryString MonteCarlo(int L, BinaryString.MuMode MODE, int N)
    {
        //Console.WriteLine("------======= LANDSCAPE =======------\n");
        //BinaryString.CreateLandscape(32);

        //Console.WriteLine("\n------======= MONTE-CARLO METHOD =======------\n");
        Console.WriteLine("\n*** MONTE-CARLO ***\n");
        //Console.WriteLine("Enter N: ");
        //int N = Convert.ToInt32(Console.ReadLine());
        //Console.WriteLine();

        List<BinaryString> S = new List<BinaryString>();
        BinaryString maxS = new BinaryString(L, MODE);
        double max = -1;

        for (int i = 0; i < N; i++)
        {
            bool maxChanged = false;
            string reportString = $"{TAB}Step {i + 1,BinaryString.COUNTER_FORMAT_WIDTH}: ";

            S.Add(new BinaryString(L, MODE));
            reportString += $"S[i] = {S[i].StrVal}, mu = {S[i].GetMu():0.##}, ";

            if (max < S[i].GetMu())
            {
                max = S[i].GetMu();
                maxS = S[i];

                if (i != 0)
                    maxChanged = true;
            }

            reportString += $"maxS = {maxS.StrVal}, max = {max:0.##}";

            if (maxChanged && i != 0)
                reportString += " <- CHANGE";

            Console.WriteLine(reportString);
        }
        Console.WriteLine($"\n{END_TAB}M-C result: {maxS.StrVal}, mu = {max:0.##}.");
        return maxS;
    }
    public static BinaryString HillClimbingDepth(int L, BinaryString.MuMode MODE, int N)
    {
        //const int L = 5;
        //const BinaryString.MuMode MODE = BinaryString.MuMode.Rand;

        //Console.WriteLine("------======= LANDSCAPE =======------\n");
        //BinaryString.CreateLandscape(L, MODE, 32);       

        //Console.WriteLine("\n------======= HILL CLIMBING METHOD (DEPTH-FIRST SEARCH) =======------\n");
        Console.WriteLine("\n*** HILL CLIMBING (DEPTH) ***\n");
        //Console.WriteLine("Enter N: ");
        //int N = Convert.ToInt32(Console.ReadLine());
        //Console.WriteLine();

        int i = 0;
        BinaryString maxS = new BinaryString(L, MODE);
        double max = maxS.GetMu();

        while (i < N)
        {
            List<BinaryString> omega = Omega(maxS);

            while (omega.Count > 0 && i < N)
            {
                string omegaString = $"{TAB}Omega = {OmegaToString(omega)}\n";

                Random random = new Random();
                int randomIndex = random.Next(0, omega.Count);
                BinaryString S = omega[randomIndex];
                omega.RemoveAt(randomIndex);

                string reportString = $"{TAB}Step {i + 1,BinaryString.COUNTER_FORMAT_WIDTH}: ";
                reportString += $"maxS = {maxS.StrVal}, max = {max:0.##}, ";
                reportString += $"S = {S.StrVal}, mu = {S.GetMu():0.##} ";

                if (max < S.GetMu())
                {
                    maxS = S;
                    max = maxS.GetMu();
                    reportString += "<- CHANGE";
                    Console.WriteLine(reportString);
                    Console.WriteLine(omegaString);
                    break;
                }
                Console.WriteLine(reportString);
                Console.WriteLine(omegaString);
                i++;
            }

            if (omega.Count == 0)
            {
                break;
            }
        }

        Console.WriteLine($"{END_TAB}HC(D) result: {maxS.StrVal}, mu = {max:0.##}");
        return maxS;

    }
    public static BinaryString HillClimbingBreadth(int L, BinaryString.MuMode MODE, int N)
    {
        //const int L = 5;
        //const BinaryString.MuMode MODE = BinaryString.MuMode.Natural;

        ///Console.WriteLine("------======= LANDSCAPE =======------\n");
        //BinaryString.CreateLandscape(L, MODE, 32);

        //Console.WriteLine("\n------======= HILL CLIMBING METHOD (BREADTH-FIRST SEARCH) =======------\n");
        Console.WriteLine("\n*** HILL CLIMBING (BREADTH) ***\n");
        //Console.WriteLine("Enter N: ");
        //int N = Convert.ToInt32(Console.ReadLine());
        //Console.WriteLine();

        BinaryString maxS = new BinaryString(L, MODE);

        for (int i = 0; i < N; i++)
        {
            List<BinaryString> omega = Omega(maxS);
            string omegaString = $"{TAB}Omega = {OmegaToString(omega)}\n";

            BinaryString maxOmega = omega[0];

            for (int j = 1; j < omega.Count; j++)
            {
                if (maxOmega.GetMu() < omega[j].GetMu())
                {
                    maxOmega = omega[j];
                }
            }

            string reportString = $"{TAB}Step {i + 1,BinaryString.COUNTER_FORMAT_WIDTH}: ";
            reportString += $"maxS = {maxS.StrVal}, max = {maxS.GetMu():0.##}, ";
            reportString += $"maxOmega = {maxOmega.StrVal}, mu = {maxOmega.GetMu():0.##} ";

            if (maxS.GetMu() < maxOmega.GetMu())
            {
                maxS = maxOmega;
                reportString += "<- CHANGE";
                Console.WriteLine(reportString);
                Console.WriteLine(omegaString);
            }
            else
            {
                Console.WriteLine(reportString);
                Console.WriteLine(omegaString);
                break;
            }

        }

        Console.WriteLine($"{END_TAB}HC(B) result: {maxS.StrVal}, mu = {maxS.GetMu():0.##}");
        return maxS;
    }

    private static List<BinaryString> Omega(BinaryString currentString)
    {
        List<BinaryString> omega = new List<BinaryString>();
        for (int j = 0; j < currentString.length; j++)
        {
            StringBuilder nearbyString = new StringBuilder(currentString.StrVal);
            if (nearbyString[j] == '0')
            {
                nearbyString[j] = '1';
            }
            else if (nearbyString[j] == '1')
            {
                nearbyString[j] = '0';
            }
            omega.Add(new BinaryString(currentString.length, currentString.muMode, Convert.ToString(nearbyString)!));
        }
        return omega;
    }

    private static string OmegaToString(List<BinaryString> omega)
    {
        string result = "{ ";
        if (omega.Count > 0)
            result += $"{omega[0].StrVal} ({omega[0].GetMu():0.##})";
        for (int i = 1; i < omega.Count; i++)
        {
            result += $", {omega[i].StrVal} ({omega[i].GetMu():0.##})";
        }
        result += " }";
        return result;
    }

}