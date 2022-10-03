using EGA_lab4;
internal class Program
{
    private static void Main(string[] args)
    {
        const int NUMBER_OF_METHODS = 3;

        const int L = 7;
        const BinaryString.MuMode MODE = BinaryString.MuMode.Custom;

        const int MONTE_CARLO_N = 10;
        const int HILL_CLIMBING_DEPTH_N = 10;
        const int HILL_CLIMBING_BREADTH_N = 10;
        
        Console.WriteLine("------======= LANDSCAPE =======------\n");
        BinaryString.CreateLandscape(L, MODE, 32);

        Console.WriteLine("\n------======= MULTIPLE LAUNCH METHOD =======------\n");
        Console.WriteLine("Enter N: ");
        int N = Convert.ToInt32(Console.ReadLine());
        
        BinaryString maxS = new BinaryString(L, MODE);
        double max = 0;

        for (int i = 0; i < N; i++) 
        {
            BinaryString maxS_i;
            double max_i;

            Random random = new Random();
            int k = random.Next(0, NUMBER_OF_METHODS);

            switch (k) {
                case 0:
                    maxS_i = Method.MonteCarlo(L, MODE, MONTE_CARLO_N);
                break;
                case 1:
                    maxS_i = Method.HillClimbingDepth(L, MODE, HILL_CLIMBING_DEPTH_N);
                break;
                default:
                    maxS_i = Method.HillClimbingBreadth(L, MODE, HILL_CLIMBING_BREADTH_N);
                break;
            }
            max_i = maxS_i.GetMu();

            if (max < max_i) {
                max = max_i;
                maxS = maxS_i;
                if (i != 0)
                    Console.WriteLine("\n!!^^ MAXS CHANGED ^^!!");
            }
        }
        Console.WriteLine($"\nFinal result: {maxS.StrVal}, mu = {maxS.GetMu():0.##}\n");

    }
}