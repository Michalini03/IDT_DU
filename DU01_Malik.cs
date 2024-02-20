using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IDT
{
    internal class DU01_Malik
    {
        static void Main(string[] args)
        {
            string line;
            StreamReader sr = new StreamReader("matrix.txt"); // Čtení ze souboru s názvem "matrix.txt" (soubour "matrix.txt" se nachází ve složce \bin\Debug, aby se načítala lehčím způsobem)
            line = sr.ReadLine();

            // Inicializace matice 'x' o rozměrech 3x3
                double[,] matrix = new double[3, 3];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        matrix[i, j] = Convert.ToInt32(line); // Naplnění matice hodnotami ze souboru
                        line = sr.ReadLine();
                    }
                }

            // Inicializace pole 'right' pro pravou stranu rovnic
                double[] right = new double[3];
                for (int finish = 0; finish < 3; finish++)
                {
                    right[finish] = Convert.ToInt32(line); // Naplnění pole hodnotami ze souboru
                    line = sr.ReadLine();
                }
                sr.Close();

            // Výpis matice 
                Console.WriteLine("Matice soustavy rovnic:");
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Console.Write(matrix[i, j] + " ");
                    }
                    Console.WriteLine();
                }

            // Výpočet a výpis determinantu matice
                Console.WriteLine();
                double hlavni_det = Determinant(matrix);
                Console.WriteLine("Determinant: " + hlavni_det);
                Console.WriteLine();

            // Výpis pravé strany
                Console.WriteLine("Řešení pravé strany:");
                for (int finish = 0; finish < 3; finish++)
                {
                    Console.WriteLine(right[finish]);
                }

                Console.WriteLine(line);

            // Inicializace pole 'vysledek' pro uložení řešení
                double[] vysledek = new double[right.Length];
                double[,] cramar_matrix = new double[3, 3];

            // Kopírování původní matice 'x' do 'cramar_matrix'
                Array.Copy(matrix, cramar_matrix, 9);

            // Řešení systému rovnic pomocí Cramerova pravidla
            for (int i = 0; i < 3; i++)
            {
                cramar_matrix[0, i] = right[0];
                cramar_matrix[1, i] = right[1];
                cramar_matrix[2, i] = right[2];

                // Výpis upravené Cramerovy matice
                Console.WriteLine(i + 1 + ". upravená matice pomocí Cramerova pravidla:");
                for (int a = 0; a < 3; a++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Console.Write(cramar_matrix[a, j] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();

                // Výpočet a výpis determinantu upravené matice
                    double poddet = Determinant(cramar_matrix);
                    Console.WriteLine("Sub-Determinant: " + poddet);
                    Console.WriteLine();

                // Výpočet a uložení řešení
                    vysledek[i] = poddet / hlavni_det;

                // Obnovení 'cramar_matrix' původní matice 'x'
                    Array.Copy(matrix, cramar_matrix, 9);
            }

            // Výpis řešení
                Console.WriteLine("X = " + Math.Round(vysledek[0], 2));
                Console.WriteLine("Y = " + Math.Round(vysledek[1], 2));
                Console.WriteLine("Z = " + Math.Round(vysledek[2], 2));
        }

        // Funkce pro výpočet Sarrussova pravidla
        static double Determinant(double[,] x)
        {
            double det1 = 1;
            double det2 = 0;
            
            // Výpočet součtu součinů pro horní trojúhelníkovou část
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int sloupec = i + j;
                    if (sloupec == 3)
                    {
                        sloupec = 0;
                    }
                    else if (i + j == 4)
                    {
                        sloupec = 1;
                    }
                    det1 *= x[j, sloupec];
                }
                det2 += det1;
                det1 = 1;
            }

            // Výpočet součtu součinů pro dolní trojúhelníkovou část
            for (int i = 2; i > -1; i--)
            {
                for (int j = 2; j > -1; j--)
                {
                    int sloupec = i - j;
                    if (sloupec == -1)
                    {
                        sloupec = 2;
                    }
                    else if (sloupec == -2)
                    {
                        sloupec = 1;
                    }
                    det1 *= x[j, sloupec];
                }
                det2 -= det1;
                det1 = 1;
            }

            return det2; // Vrácení determinantu
        }
    }
}

