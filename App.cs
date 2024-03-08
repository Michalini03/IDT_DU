using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;


//Malík, Brabec
namespace FAV_03
{
    /**
 * Hlavni trida programu pro numericky vypocet urciteho integralu
 */
    public class Exercise04
    {
        public static void Main(String[] args)
        {
            IFunction lf = new LinearFunction(2, 1);
            IFunction quadraticPolynomial = new QuadraticPolynomial(1, 2, 5);

            Integrator integrator = new Integrator();
            integrator.SetDelta(0.01);

            double integral = integrator.Integrate(lf, 0, 10);
            double integralQuadraticPolynomial = integrator.Integrate(quadraticPolynomial, 0, 1);

            lf.SetEpsilon(0.001);
            quadraticPolynomial.SetEpsilon(0.001);

            Console.WriteLine(lf.Differentiate(1));
            Console.WriteLine(quadraticPolynomial.Differentiate(1));

            Console.WriteLine(integral);
            Console.WriteLine(integralQuadraticPolynomial);

            double[] coefs = { 2, 3, 5 };
            IFunction quadra = new GeneralPolynomial(coefs);
            Console.WriteLine(quadra.ValueAt(2));

            IFunction myPolynomial = new GeneralPolynomial(new double[] { 7, -5, 3, -15 });
            myPolynomial.SetEpsilon(0.01);
            IFunction firstDerivative = new Derivative(myPolynomial);
            firstDerivative.SetEpsilon(0.01);
            IFunction secondDerivative = new Derivative(firstDerivative);
            secondDerivative.SetEpsilon(0.01);
            IFunction thirdDerivative = new Derivative(secondDerivative);
            thirdDerivative.SetEpsilon(0.01);

            for (double x = 0; x < 10.1; x += 0.2)
            {
                Console.WriteLine(thirdDerivative.ValueAt(x));
            }
        }
    }

    /**
     * Obecna matematicka funkce
     */
    interface IFunction
    {
        /**
         * Vypocte a vrati hodnotu funkce v zadanem bode
         */
        double ValueAt(double p);
        double Differentiate(double x);

        void SetEpsilon(double k);
    }

    abstract class AbstractFunction : IFunction
    {
        double h = 0.1;
        double epsilon;
        public double Differentiate(double x)
        {
            double last_dx = this.dx(x,h);
            h /= 2;
            double dx = this.dx(x, h);
            
            
            while (Math.Abs(dx-last_dx) >= epsilon) {
                last_dx = dx;
                h /= 2;
                dx = this.dx(x, h);
            }
            return Math.Round(dx,2);
        }

        public void SetEpsilon(double ep)
        {
            this.epsilon = ep;
        }

        public double dx(double x, double h)
        {
            return (this.ValueAt(x + h) - this.ValueAt(x)) / h;
        }

        public abstract double ValueAt(double p);
    }
    /**
     * Trida pro numericky vypocet urciteho integralu funkce
     */
    class Integrator
    {
        /** Krok pro vypocet integralu */
        double delta;

        /**
         * Numbericky vypocte a vrati urcity integral zadane fukce f od a do b
         */
        public double Integrate(IFunction f, double a, double b)
        {
            double result = 0;
            double v = f.ValueAt(a);
            while (a + delta < b)
            {
                //obdelniky sirky delta
                result += delta * v;
                a += delta;
                v = f.ValueAt(a);
            }
            // jeste posledni obdelnik, ktery bude uzsi nez delta
            result += (b-a) * v;
            return result;
        }

        /**
         * Nastavi krok pro vypocet integralu
         * @param d krok pro vypocet integralu
         */
        public void SetDelta(double d)
        {
            this.delta = d;
        }

    }

    /**
     * Linerani funkce
     * @author Libor Vasa
     */
    class LinearFunction : AbstractFunction
    {
        /** Smernice funkce */
        double k;
        /** Posun funkce */
        double q;

        /**
         * Vytvori novou linearni funkci se zadanymi koeficienty
         */
        public LinearFunction(double k, double q)
        {
            this.k = k;
            this.q = q;
        }

        public override double ValueAt(double p)
        {
            return (k * p + q);
        }
    }

    class QuadraticPolynomial : AbstractFunction
    {
        double a, b, c;
        public QuadraticPolynomial(double a, double b, double c)
        {
            this.a = a; this.b = b; this.c = c;
        }
        public override double ValueAt(double p)
        {
            return(this.a*p*p + this.b * p + this.c);
        }
    }

    class GeneralPolynomial : AbstractFunction
    {
        double[] koef;

        public GeneralPolynomial(double[] coefs)
        {
            this.koef = coefs;
        }

        public override double ValueAt(double p)
        {
            double vysledek = 0;
            for (int i = 0; i < koef.Length; i++)
            {
                vysledek += koef[i] * Math.Pow(p,(koef.Length-1-i));
            }
            return vysledek;
        }
    }

    class Sine : AbstractFunction
    {
        double x;

        public Sine(double x)
        {
            this.x = x;
        }

        public override double ValueAt(double p)
        {
            // Math.Sin v C# očekává úhel v radiánech
            return Math.Sin(p);
        }
    }

    class Derivative : AbstractFunction
    {
        IFunction f;

        public Derivative(IFunction function)
        {
            this.f = function;
        }

        public override double ValueAt(double p)
        {
           return f.Differentiate(p);
        }
    }

}

