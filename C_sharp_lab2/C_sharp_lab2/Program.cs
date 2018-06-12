using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sharp_lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            Listen ex = new Listen();
            Series example = new Series();
            example.onChange += ex.Messege;
            for(int i =0;i<9;i++)
            {
                if (i % 2 == 0)
                {
                    example.AddElement(new Linear(i, 2 * i));
                }
                else
                {
                    example.AddElement(new Exponential(i, 3 * i));
                }
            }
            Console.Write(example.ToString());
            Series example2 = new Series();
            example2.onChange += ex.Messege;
            for (int i = 0; i < 9; i++)
            {
                if (i % 2 == 0)
                {
                    example2.AddElement(new Linear(i, 2 * i));
                }
                else
                {
                    example2.AddElement(new Exponential(i, 3 * i));
                }
            }
            if (example == example2)
                Console.WriteLine("Same");
            Console.ReadKey();
        }
    }
    abstract class Progression
    {
        protected float first;
        protected float param;
        abstract public float element(int n);
        abstract public float sum(int n);
        float get_first()
        {
            return first;
        }
        float get_param()
        {
            return param;
        }
        override
        public bool Equals(Object obj)
        {
            Progression temp = (Progression)obj;
            if (temp.first == first && temp.param == param)
                return true;
            else
                return false;
        }

        public static bool operator ==(Progression obj1, Progression obj2)
        {
            if (obj1.first == obj2.first && obj1.param == obj2.param)
                return true;
            else
                return false;
        }
        public static bool operator !=(Progression obj1, Progression obj2)
        {
            if (obj1.first == obj2.first && obj1.param == obj2.param)
                return false;
            else
                return true;
        }
        override
        public int GetHashCode()
        {
            string s1 = Convert.ToString(first);
            string s2 = Convert.ToString(param);
            string s = s1+" "+s2; 
            return s.GetHashCode();
        }
        abstract public Progression DeepCopy();
        override
        public string ToString()
        {
            string s1 = Convert.ToString(first);
            string s2 = Convert.ToString(param);
            string s = "first element: "+s1 + "\n parametr: " + s2+"\n";
            return s;
        }
    }
    class Linear : Progression
    {
        public Linear() { }
        public Linear(float a, float d)
        {
            first = a;
            param = d;
        }
        override
        public float element(int n)
        {
            return first+param*(n-1);
        }
        override
        public float sum(int n)
        {
            try
            {
                return (2 * first + (n - 1) * param) / 2;
            }
            catch(OverflowException)
            {
                Console.WriteLine("To many computing!");
                return 0;
            }
        }
        override
        public Progression DeepCopy()
        {
            return new Linear(first,param);
        }
    }
    class Exponential: Progression
    {
        public Exponential() { }
        public Exponential(float b, float q)
        {
            first = b;
            param = q;
        }

        override
        public float element(int n)
        {
            try
            {
                return first * (float)Math.Pow(param, n - 1);
            }
            catch (OverflowException)
            {
                Console.WriteLine("To many computing!");
                return 0;
            }
        }
        override
        public float sum(int n)
        {
            if (param != 1)
                return (first * (1 - (float)Math.Pow(param, n - 1))) / (1 - param);
            else
                return n * first;
        }
        public float full_sum()
        {
            if (param >= 1)
            {
                Console.WriteLine("Full sum is not exsist");
                return -1;
            }
            else
                return first / (1 - param);
        }
        override
        public Progression DeepCopy()
        {
            return new Exponential(first,param);
        }
    }
    class Series
    {
        public Progression[] massive;
        public int curr;
        public delegate void Coller();
        public event Coller onChange;
        public Series()
        {
            this.massive = new Progression[10];
            for (int i = 0; i < 10; i++)
                massive[i] = new Linear();
            curr = 0;
        }
        public Series(Progression obj)
        {
            massive = new Progression[10];
            for (int i = 0; i < 10; i++)
                massive[i] = new Linear();
            curr = 1;
            massive[0] = obj.DeepCopy();
        }
        public void AddElement(Progression obj)
        {
            if (massive.Length == curr)
            {
                Progression[] new_mas = new Progression[massive.Length * 2];
                for (int i = 0; i < massive.Length; i++)
                {
                    new_mas[i] = massive[i];
                }
                massive = new_mas;
            }
            curr++;
            this.massive[curr] = obj.DeepCopy();
            onChange();
    }
        public void DeleteElement(Progression obj)
        {
            int index=-1;
            for(int i =0; i<=curr;i++)
                if(massive[i]==obj)
                {
                    index = i;
                    break;
                }
            DeleteElement(index);
        }
        public void DeleteElement(int index)
        {
            if (index > -1)
            {
                massive[index] = null;
                for (int i = 0; i < curr; i++)
                    if (i > index)
                        massive[i] = massive[i + 1];
                onChange();
            }              
        }
        public Progression this[int index]
        {
            get
            {
                try
                {
                    return massive[index];
                }
                catch(IndexOutOfRangeException ex)
                {
                    Console.WriteLine("Dou!");
                    return 0;
                }

                
            }
            set
            {
                try
                {
                    massive[index] = value;
                    onChange();
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine("Dou!");
                }

            }
        }
        public float sum(int n)
        {
            float result = 0;
            for (int i = 0; i <= curr; i++)
            {
                result += massive[i].sum(n);
            }
            return result;
        }
        public void output()
        {
            for (int i = 0; i < curr; i++)
            {
                Console.WriteLine(massive[i].ToString());
            }
        }
        override
        public bool Equals(object obj)
        {
            Series temp = (Series)obj;
            if (temp.curr != curr)
                return false;
            else
            {
                for (int i = 0; i < curr; i++)
                {
                    if (temp[i] != massive[i])
                        return false;
                }
                return true;
            }
        }
        public static bool operator ==(Series obj1, Series obj2)
        {
            if (obj1.curr != obj2.curr)
                return false;
            else
            {
                for (int i = 0; i < obj1.curr; i++)
                {
                    if (obj1[i] != obj2[i])
                        return false;
                }
                return true;
            }
        }

        public static bool operator !=(Series obj1, Series obj2)
        {
            if (obj1.curr != obj2.curr)
                return true;
            else
            {
                for (int i = 0; i < obj1.curr; i++)
                {
                    if (obj1[i] != obj2[i])
                        return true;
                }
                return false;
            }
        }
        override
        public int GetHashCode()
        {
            string s = "";
            for (int i = 0; i < curr; i++)
            {
                s += massive[i].ToString();
                s += "\n";
            }
            return s.GetHashCode();
        }
        Series DeepCopy()
        {
            Series obj = new Series();
            for(int i =0; i<=curr;i++)
            {
                obj.massive[i] = massive[i].DeepCopy();
            }
            return obj;
        }
        override
        public string ToString()
        {
            if (massive == null)
                Console.WriteLine("O no");
            string s = "";
            for (int i = 0; i <= curr; i++)
            {
                this.massive[i].ToString();
                s+= massive[i].ToString();
                s += "\n";
            }
            return s;
        }
    }
    class Listen
    {
        public void Messege()
        {
            Console.WriteLine("Change!");
        }
    }

}
