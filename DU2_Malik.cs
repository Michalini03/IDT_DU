using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDT_DU2
{
    internal class DU2_Malik
    {
        static void Main(string[] args)
        {
            string line;
            string[] tutors = new string[0];
            PlantEvent[] ucitele = new PlantEvent[0];
            StreamReader sr = new StreamReader("sscUTF8.txt"); // Čtení ze souboru s názvem "sscUTF8.txt" (soubour "matrix.txt" se nachází ve složce \bin\Debug, aby se načítala lehčím způsobem)
            while ((line = sr.ReadLine()) != null)
            {
                tutors = tutors.Append(line).ToArray();
            }
            sr.Close();
            for (int i = 0; i < tutors.Length / 5; i++)
            {
                PlantEvent p = new PlantEvent(tutors[i * 5], tutors[i * 5 + 1], Int32.Parse(tutors[i * 5 + 2]), Int32.Parse(tutors[i * 5 + 3]), Int32.Parse(tutors[i * 5 + 4]));
                ucitele = ucitele.Append(p).ToArray();
            }
            // Console.WriteLine(ucitele[0].IsInConflict(ucitele[9]));


        }
        static Plan PossibleCombination(PlantEvent[] nabidka)
        {
            Plan plan = new Plan(nabidka);
            return plan;
        }
    }

    public class PlantEvent
    {
        public string name;
        public string subject;
        public int day;
        public int begin;
        public int end;

        public PlantEvent(string jmeno, string predmet, int den, int zacatek, int konec) {
        name = jmeno;
        subject = predmet;
        day = den;
        begin = zacatek;
        end = konec;
    }
        public bool IsInConflict(PlantEvent other)
        {
            if (this.day != other.day)
            {
                return false;
            }

            if (this.begin >= other.end)
            {
                return false;
            }
            if (other.begin >= this.end)
            {
                return false;
            }
            return true;
        }
    }

    public class Plan
    {
        public PlantEvent[] novy_plan;
        public Plan(PlantEvent[] data)
        {
            novy_plan = data;
        }
        public bool IsConflict() 
        { 
            return false; 
        }
        public bool IsOK()
        {
            int pocet_informatiky = 0;
            int pocet_matematiky = 0;
            for (int i = 0; i < novy_plan.Length; i++)
            {
                if (novy_plan[i].subject == "math")
                {
                    pocet_matematiky += 1;
                }
                else if (novy_plan[i].subject == "computers")
                {
                    pocet_informatiky += 1;
                }
            }
            if (pocet_informatiky >= 2)
            {
               if (pocet_matematiky >= 3)
               {
                    for (int i = 0;i < novy_plan.Length; i ++)
                    {
                        for (int j = i + 1; j < novy_plan.Length; j++)
                        {
                            if (novy_plan[i].subject == novy_plan[j].subject && novy_plan[i].day != novy_plan[j].day)
                            {
                                return true;
                            }
                        }
                    }
               }  
            }
            return false;
            
        }
    }
}
