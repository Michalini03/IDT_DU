using System.Linq;

public enum Subject { math, computers }

public class PlanEvent
{
    /* Jmeno tutora */
    public String tutor;
    /* Hodina pocatku doucovani (10 = 10:00 atd.) */
    public int start;
    /* Hodina konce doucovani (10 = 10:00 atd.) */
    public int end;
    /* Den tydne doucovani (0 = Pondeli, 1 = Utery atd.) */
    public int dayOfWeek;
    /* Doucovany predmet */
    public Subject subject;

    /*
	 * Vytvori novou nabidku tutora na doucovani
	 */
    public PlanEvent(string tutor, int dayOfWeek, int start, int end, Subject subject)
    {
        this.tutor = tutor;
        this.start = start;
        this.end = end;
        this.dayOfWeek = dayOfWeek;
        this.subject = subject;
    }

    /*
	 * Vrati true, pokud se tato udalost prekryva se zadanou udalosti, jinak vrati false
	 */
    public bool IsInConflict(PlanEvent other)
    {
        if (this.dayOfWeek != other.dayOfWeek)
        {
            return false;
        }
        if (this.start < other.end && this.end > other.end)
        {
            return true;
        }
        if (other.start > this.start && other.start < this.end)
        {
            return true;
        }
        if (other.start < this.start && other.end > this.end)
        {
            return true;
        }
        if (this.start < other.start && this.end > other.end)
        {
            return true;
        }
        return false;
    }
    public class Plan
    {
        public PlanEvent[] schedule;

        public Plan(PlanEvent[] rozvrh)
        {
            this.schedule = new PlanEvent[rozvrh.Length];
            for (int i = 0; i < rozvrh.Length; i++)
            {
                this.schedule[i] = rozvrh[i];
            }
        }

        /*
         * Vrati true, pokud se tato udalost prekryva se zadanou udalosti, jinak vrati false
         */
        public bool IsConflict()
        {
            for (int i = 0; i < this.schedule.Length; i++)
            {
                for (int j = i + 1; j < this.schedule.Length; j++)
                {
                    if (this.schedule[i].IsInConflict(this.schedule[j]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public bool IsOK()
        {
            int math = 0;
            int[] math_day = new int[5] { 0, 0, 0, 0, 0 };
            int pc = 0;
            int[] pc_day = new int[5] { 0, 0, 0, 0, 0 };



            foreach (PlanEvent e in this.schedule)
            {
                if (e.subject == Subject.math)
                {
                    math_day[e.dayOfWeek]++;
                    math++;
                }
                else if (e.subject == Subject.computers)
                {
                    pc_day[e.dayOfWeek]++;
                    pc++;
                }
            }

            if (pc >= 2 && math >= 3)
            {
                for (int i = 0; i < math_day.Length; i++)
                {
                    if ((math_day[i] > 1))
                    {
                        return false;
                    }
                    else if ((pc_day[i] > 1))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }


    public static void Main(String[] args)
    {
        PlanEvent[] list_plan = new PlanEvent[0];

        string line;
        string[] tutors = new string[0];
        StreamReader sr = new StreamReader("sscUTF8.txt"); // Čtení ze souboru s názvem "sscUTF8.txt" (soubour "matrix.txt" se nachází ve složce \bin\Debug, aby se načítala lehčím způsobem)
        while ((line = sr.ReadLine()) != null)
        {
            tutors = tutors.Append(line).ToArray();
        }
        sr.Close();
        PlanEvent p = null;
        for (int i = 0; i < tutors.Length / 5; i++)
        {
            if (tutors[i * 5 + 1] == "math")
            {
                p = new PlanEvent(tutors[i * 5],
                            Int32.Parse(tutors[i * 5 + 2]),
                            Int32.Parse(tutors[i * 5 + 3]),
                            Int32.Parse(tutors[i * 5 + 4]),
                            Subject.math
                            );
                list_plan = list_plan.Append(p).ToArray();
            }
            else if (tutors[i * 5 + 1] == "computers")
            {
                p = new PlanEvent(tutors[i * 5],
                            Int32.Parse(tutors[i * 5 + 2]),
                            Int32.Parse(tutors[i * 5 + 3]),
                            Int32.Parse(tutors[i * 5 + 4]),
                            Subject.computers
                            );
                list_plan = list_plan.Append(p).ToArray();
            }
        }
        PlanEvent[][] novy_rozvrh = Combinations(list_plan);
        for (int i = 0; i < novy_rozvrh.Length; i++)
        {
            Plan funkcni = new Plan(novy_rozvrh[i]);
            if (funkcni.IsOK() == true && funkcni.IsConflict() == false)
            {
                Console.WriteLine("Vyučující:  " + novy_rozvrh[i][0].tutor + ", začátek: " + novy_rozvrh[i][0].start + ":00 a konec: " + novy_rozvrh[i][0].end + ":00 v " + (novy_rozvrh[i][0].dayOfWeek + 1) + ". vyučující den, na předmět: " + novy_rozvrh[i][0].subject);
                Console.WriteLine("Vyučující:  " + novy_rozvrh[i][1].tutor + ", začátek: " + novy_rozvrh[i][1].start + ":00 a konec: " + novy_rozvrh[i][1].end + ":00 v " + (novy_rozvrh[i][1].dayOfWeek + 1) + ". vyučující den, na předmět: " + novy_rozvrh[i][1].subject);
                Console.WriteLine("Vyučující:  " + novy_rozvrh[i][2].tutor + ", začátek: " + novy_rozvrh[i][2].start + ":00 a konec: " + novy_rozvrh[i][2].end + ":00 v " + (novy_rozvrh[i][2].dayOfWeek + 1) + ". vyučující den, na předmět: " + novy_rozvrh[i][2].subject);
                Console.WriteLine("Vyučující:  " + novy_rozvrh[i][3].tutor + ", začátek: " + novy_rozvrh[i][3].start + ":00 a konec: " + novy_rozvrh[i][3].end + ":00 v " + (novy_rozvrh[i][3].dayOfWeek + 1) + ". vyučující den, na předmět: " + novy_rozvrh[i][3].subject);
                Console.WriteLine("Vyučující:  " + novy_rozvrh[i][4].tutor + ", začátek: " + novy_rozvrh[i][4].start + ":00 a konec: " + novy_rozvrh[i][4].end + ":00 v " + (novy_rozvrh[i][4].dayOfWeek + 1) + ". vyučující den, na předmět: " + novy_rozvrh[i][4].subject);
                Console.WriteLine();
            }
        }
    }


    // FUNKCE, KTERÁ FUNGUJE NA ZPŮSOB KOMBINATORIKY -> VYTVOŘÍ KOMBINACI VŠECH MOŽNÝCH ROZVRHŮ
	public static PlanEvent[][] Combinations(PlanEvent[] list)
	{
	    List<PlanEvent[]> vysledek = new List<PlanEvent[]>();
	    int n = list.Length;
	
	    for (int i = 0; i < n - 4; i++)
	    {
	        for (int j = i + 1; j < n - 3; j++)
	        {
	            for (int k = j + 1; k < n - 2; k++)
	            {
	                for (int l = k + 1; l < n - 1; l++)
	                {
	                    for (int m = l + 1; m < n; m++)
	                    {
	                        PlanEvent[] kombinace = new PlanEvent[] { list[i], list[j], list[k], list[l], list[m] };
	                        vysledek.Add(kombinace);
	                    }
	                }
	            }
	        }
	    }
	    return vysledek.ToArray();
	}
}
