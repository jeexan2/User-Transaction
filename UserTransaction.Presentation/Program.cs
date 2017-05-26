using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserTransaction.Domain;
using UserTransaction.DAL;



namespace UserTransaction.Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Press Key to choose!");
                Console.WriteLine("1.Add to List!");
                Console.WriteLine("2.Modify user!");
                Console.WriteLine("3.Delete User");
                Console.WriteLine("4.Show User!");
                Console.WriteLine("5.Show Custom List!");
                Console.WriteLine("6.Want to Query!");
                Console.WriteLine("Press Any Key to break");
                UserEvent Event = new UserEvent(DALDelegate.GetfromJson);
                Event();
                int press = Convert.ToInt32(Console.ReadLine());

                if (press == 1)
                {
                    Event = DALDelegate.AddUser;
                    Event();
                }
                else if (press == 2)
                {
                    Event = DALDelegate.ModifyUser;
                    Event();
                }
                else if (press == 3)
                {
                    Event = DALDelegate.DeleteUser;
                    Event();
                }
                else if (press == 4)
                {
                    Event = DALDelegate.Show;
                    Event();
                }
                else if (press == 5)
                {
                    Event = DALDelegate.CustomList;
                    Event();
                }
                else if (press == 6)
                {
                    Query query = new Query(DALDelegate.ComplexQuery);
                    query();
                }
                else break;
            }
           
            Console.ReadLine();
        }
    }
}
