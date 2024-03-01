/*
 * Metoda rozhoduje, zda se predana hodnota x nachazi v predanem serazenem poli
 */

using static System.Runtime.InteropServices.JavaScript.JSType;

public class Ukol3
{

    // FUNKCE PRO GENEROVÁNÍ NÁHODNÝCH DAT
    static int[] dataGenerator()
    {
        Random random = new Random();
        int[] test = new int[random.Next(1, 1000)];
        for (int i = 0; i < test.Length; i++)
        {
            if (i == 0)
            {
                test[i] = random.Next(1, 100);
            }
            else
            {
                test[i] = random.Next(test[i - 1] + 1, test[i - 1] + 100);
            }
        }
        return test;
    }

    // FUNKCE, KTERÁ ZKOUŠÍ JESTLI OBĚ HLEDACÍ METODY DÁVAJÍ STEJNÝ VÝSLEDEK
    static bool testFinderMatch(IFinder finderIS, IFinder finderS)
    {
        Random random = new Random();
        int[] testMatch = dataGenerator();
        for (int i = 0; i < 10000; i ++) { 
            int num = random.Next(0, testMatch[testMatch.Length]);
            bool IS = finderIS.Find(num, testMatch);
            bool S = finderS.Find(num, testMatch);
            if (IS != S)
            {
                Console.WriteLine("Máš to špatně!");
                return false;
            }
        }
        return true;
    }

    // FUNKCE, KTERÁ ZJISTÍ RYCHLOST OBOU METOD A POROVNÁ JE (PRO ÚLOHU 3. DOPLNÍM DO PARAMETRU I DATA, KTERÁ BUDU POROVNÁVAT)
    static string testFinderSpeed(IFinder finderIS, IFinder finderS, int[] testSpeed)
    {
        Random random = new Random();
        DateTime startIS = DateTime.Now;
        for (int i = 0; i < 10000; i++) {
            int num = random.Next(0, testSpeed[testSpeed.Length - 1]);
            bool found = finderIS.Find(num, testSpeed);
        }
        DateTime stopIS = DateTime.Now;
        Console.WriteLine("Čas testování binárního hledání: " + (stopIS - startIS).TotalMilliseconds * 1000 + " ns");

        DateTime startS = DateTime.Now;
        for (int i = 0; i < 10000; i++)
        {
            int num = random.Next(0, testSpeed[testSpeed.Length - 1]);
            bool found = finderS.Find(num, testSpeed);
        }
        DateTime stopS = DateTime.Now;
        Console.WriteLine("Čas testování sekvenčního hledání: " + (stopS - startS).TotalMilliseconds * 1000 + " ns");

        if (stopIS - startIS < stopS - startS)
        {
            return "Binární Finder je rychlejší o " + ((stopS - startS).TotalMilliseconds / (stopIS - startIS).TotalMilliseconds) + " násobek";
        }
        else if (stopIS - startIS == stopS - startS)
        {
            return "Rychlost obou způsobů vyhledávání si je rovna";
        }
        else
        {
            return "Sekvenční Finder je rychlejší o " + ((stopIS - startIS).TotalMilliseconds / (stopS - startS).TotalMilliseconds) + " násobek";
        }
    }

    // FUNKCE, KTERÁ ZJISTÍ JESTLI JSOU DATA SEŘAZENA
    static bool isSorted(int[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == data[data.Length - 1]) { 
            }
            else if (data[i] > data[i + 1])
            {
                return false;
            }
        }
        return true;
    }

    // FUNKCE, KTERÁ NAČTE DATA Z TEXTOVÉHO DOKUMENT
    static int[] readData(string path)
    {
        string line;
        int[] data = new int[0];
        StreamReader sr = new StreamReader(path); // Čtení ze souboru s názvem z argumentu (soubour se nachází ve složce \bin\Debug, aby se načítala lehčím způsobem);
        line = sr.ReadLine();
        while (line != null)
        {
            data = data.Append(int.Parse(line)).ToArray();
            line = sr.ReadLine();
        }
        return data;
    }


    public static void Main(System.String[] args)
    {
        IFinder IS = new IntervalSubdivisionFinder();
        IFinder S = new SequentialFinder();

        // int[] data = new int[] { 1, 3, 5, 41, 48, 52, 63, 71 };

        /* IntervalSubdivisionFinder - Užití
        DateTime start = DateTime.Now;
        bool found = IS.Find(53, data);
        DateTime stop = DateTime.Now;
        Console.WriteLine("Interval subdivision finished in " + (stop - start).TotalMilliseconds * 1000 + " ns");
        Console.WriteLine();
        */

        /* Sequential Finder - Užití
        start = DateTime.Now;
        found = S.Find(53, data);
        stop = DateTime.Now;
        Console.WriteLine("Sequential finished in " + (stop - start).TotalMilliseconds * 1000 + " ns");
        Console.WriteLine();
   
        Console.WriteLine("Number found: " + found);
        */

        // Zkouška testů
        for (int i=0; i< 9; i++) {
            string src = "seq" + i + ".txt";
            int[] info = readData(src);
            Console.WriteLine("Název souboru: " + src);
            if (isSorted(info) )
            {
                Console.WriteLine("Data jsou řádně seřazeny");
                Console.WriteLine("Počet dat v souboru: " + info.Length);
                Console.WriteLine(testFinderSpeed(IS, S, info));
            }
            else
            {
                Console.WriteLine("Data nejsou seřazeny, ověřte zdroj informací");
            }
            
            Console.WriteLine();
        }

       // bool vyhodnoceni = testFinderMatch(IS, S);
       // Console.WriteLine(testFinderSpeed(IS, S));
        

        /*
         * TEST!
         * for (int i = 0; i < data[data.Lenght - 1]; i++) {
         *  bool found = intervalSubdivision(data, i);
         *  if (bool) { pocet += 1}
         *  if (pocet != data.Lenght) {
         *      Console.WriteLine("Error")
         *      }
         *  }
         */
    }
}


// VYTVOŘENÍ INTERFACE PRO FINDER
public interface IFinder
{
    bool Find(int x, int[] data);
}

// BINÁRNÍ FINDER
public class IntervalSubdivisionFinder : IFinder
{
    public bool Find(int x, int[] data)
    {
        int left = 0; //leva hranice intervalu
        int right = data.Length - 1; //prava hranice intervalu
        int mid = (left + right) / 2; //index uprostred intervalu
        while (data[mid] != x)
        {
            if (left >= right)
            {
                return false;
            }
            //nyni zmensime interval	
            if (data[mid] > x)
            {
                right = mid - 1;
            }
            else
            {
                left = mid + 1;
            }
            mid = (left + right) / 2;
        }
        return true;
    }
}

// SEKVENČNÍ FINDER
public class SequentialFinder : IFinder
{
    public bool Find(int x, int[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == x)
            {
                return true;
            }
        }
        return false;
    }
}
