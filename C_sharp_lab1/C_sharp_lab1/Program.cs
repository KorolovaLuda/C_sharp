using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sharp_lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter quantity of people:");
            string num = Console.ReadLine();
            int n;
            if (int.TryParse(num, out n))
            {
                my_matrix mas = new my_matrix();
                Console.WriteLine("If you want enter matrix, press 1, if you want to random matrix, press 2");
                int marker;
                int.TryParse(Console.ReadLine(),out marker);
                if(marker==1)
                {
                    mas.own_input(n);
                    mas.output();
                    if (mas.strangers())
                        Console.WriteLine("We can devide people");
                    else
                        Console.WriteLine("We can not devide people");
                    mas.output();
                }
                else if (marker==2)
                {
                    mas.random_input(n);
                    mas.output();
                    if (mas.strangers())
                        Console.WriteLine("We can devide people");
                    else
                        Console.WriteLine("We can not devide people");
                    mas.output();
                }
                else
                    Console.WriteLine("Wrong operation");
            }
            else
                Console.WriteLine("It is not a number");
            Console.ReadKey();
        }
    }

    class my_matrix
    {
        int[,] matrix;
        public void  random_input(int n)
        {
            Random rand = new Random();
            int[,] result = new int[n,n];
            for (int i = 0; i < n; i++)
                for (int j = i; j < n; j++)
                    if(i==j)
                        result[i, j] = 1;
                    else
                    {
                        result[i, j] = rand.Next(0, 2);
                        result[j, i] = result[i, j];
                    }
            matrix = result;
        }
         public void own_input(int n)
        {
            string[] temp;
            string t;
            int a;
            matrix = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrix[i,j] = -1;
            for (int i = 0; i < n; i++)
            {
                t = Console.ReadLine();
                temp = t.Split(' ');
                if (temp.Length != n)
                {
                    i--;
                    Console.WriteLine("Try again");
                }
                else
                {
                    for (int j = 0; j < n; j++)
                        if (int.TryParse(temp[j], out a))
                            if (a == 1 || a == 0)
                            {
                                if(i==j&&a==0)
                                {
                                    i--;
                                    Console.WriteLine("Try again");
                                    break;
                                }
                                if(matrix[j,i]!=-1&&matrix[j,i]!=a)
                                {
                                    i--;
                                    Console.WriteLine("Try again");
                                    break;
                                }
                                matrix[i, j] = a;
                            }
                            else
                            {
                                i--;
                                Console.WriteLine("Try again");
                                break;
                            }
                }
            }
            Console.WriteLine();
        }
        public void output()
        {
            for (int i = 0; i < Math.Sqrt(matrix.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(matrix.Length); j++)
                    Console.Write(matrix[i, j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine();
        }
       public  bool strangers()
        {
            for (int i = 0; i < Math.Sqrt(matrix.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(matrix.Length); j++)
                {
                    if (i != j)
                        if (matrix[i, j] == 1)                 //if people are familiar
                        {
                            if (matrix[j, j] == 2)                  //if familiar human in group I
                            {
                                if(matrix[i,i]==1)                      //if human without group
                                {
                                    matrix[i, i] = 3;
                                }
                                else if (matrix[i, i] == 2)             //if human in group I
                                {
                                    return false;
                                }
                            }
                            else if(matrix[j,j]==3)                 //if familiar human in group II
                            {
                                if (matrix[i, i] == 1)                  //if human without group
                                {
                                    matrix[i, i] = 2;
                                }
                                else if (matrix[i, i] == 3)             //if human in group II
                                {
                                    return false;
                                }
                            }
                        }        
                }
                if (matrix[i, i] == 1)
                    matrix[i, i] = 2;
             
            }
            return true;
        }
    }

}
