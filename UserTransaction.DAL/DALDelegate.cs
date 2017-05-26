using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserTransaction.Domain;

namespace UserTransaction.DAL
{
    public delegate void UserEvent();
    public delegate void UserParamEvent(int n);
    public delegate List<User> Query();
    public static class Extension
    {
        public static void ShowTitle(this User user)
        {
            TextInfo text = new CultureInfo("en-US", false).TextInfo;
            Console.WriteLine("{0}",text.ToTitleCase(user.FirstName+" "+user.LastName));
        }
    }
    public class DALDelegate
    {
        public static IList<User> UserList;

        
        public static void SaveInJson()
        {

            File.WriteAllText(@"C:\Users\j.aziz\Desktop\user.json", JsonConvert.SerializeObject(UserList));
            using (StreamWriter file = File.CreateText(@"C:\Users\j.aziz\Desktop\user.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, UserList);
            }

        }

        public static void GetfromJson()
        {

            UserList = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@"C:\Users\j.aziz\Desktop\user.json"));


        }
        public static void Show()
        {
            GetfromJson();
            if (UserList != null)
                foreach (var user in UserList)
                {
                    Console.WriteLine("Hi {0} {1}!!You have: {2} Taka!Your id is: {3}", user.FirstName, user.LastName, user.Amount, user.ID);
                }


        }
        public static void AddUser()
        {

            SaveInJson();
            GetfromJson();

            User user = new User();
            Console.WriteLine("Enter First Name");
            user.FirstName = Console.ReadLine();
            Console.WriteLine("Enter Last Name");
            user.LastName = Console.ReadLine();
            Console.WriteLine("Enter Email");
            user.Email = Console.ReadLine();
            Console.WriteLine("Enter Amount of Taka!");
            user.Amount = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Enter id(in integer)!");
            user.ID = Convert.ToInt32(Console.ReadLine());

            if (UserList == null) UserList = new List<User>();
            UserList.Add(user);

           

            SaveInJson();
            GetfromJson();
            user.Transactions = new List<Transaction>();
        }
        public static User SearchById(int id)
        {
            var user = UserList.SingleOrDefault(x => x.ID == id);
            if (user != null) return user;
            else return new User();
        }
        public static void DeleteUser()
        {
            int id;
            Console.WriteLine("Enter id to delete");
            id = Convert.ToInt32(Console.ReadLine());
            var item = SearchById(id);
            if (item != null)
                UserList.Remove(item);
            SaveInJson();
        }
        public static void ModifyUser()
        {

            int id;
            Console.WriteLine("Enter id to modify");
            id = Convert.ToInt32(Console.ReadLine());
            var item = SearchById(id);
            //Console.WriteLine("{0}", item.FirstName);
            foreach (var user in UserList)
            {
                if (item == user)
                {
                    string firstname; string lastname;
                    Console.WriteLine("Enter First Name of user");
                    firstname = Console.ReadLine();
                    Console.WriteLine("Enter Last Name of user");
                    lastname = Console.ReadLine();
                    user.FirstName = firstname;
                    user.LastName = lastname;
                }
                SaveInJson();
            }
        }
        public static void UpdateamountUser(User user, Decimal deposit)
        {
            user.Amount += deposit;
        }
        public static void AddTransaction(int id)
        {
            
            var item = SearchById(id);
            if (item != null)
            {
                foreach (var user in UserList)
                {
                    if (item == user)
                    {
                        Decimal amount;


                        Console.WriteLine("Enter the amount");
                        amount = Convert.ToDecimal(Console.ReadLine());
                        user.Amount += amount;
                        int mx = 0;
                        if (user.Transactions != null)
                        {
                            foreach (var transactionu in user.Transactions)
                                if (mx < transactionu.ID)
                                    mx = transactionu.ID;
                        }
                        Transaction transaction = new Transaction();
                        transaction.Amount = amount;
                        transaction.ID = mx + 1;
                        transaction.UserId = user.ID;
                        transaction.DateTransaction = DateTime.Now;
                        if (user.Transactions == null)
                            user.Transactions = new List<Transaction>();
                        user.Transactions.Add(transaction);
                        SaveInJson();


                    }

                }
            }
        }
        public static List<User> QueryByString()
        {
            string input;
            Console.WriteLine("Give input string!");
            input = Console.ReadLine();
            List<User> users = UserList.Where(u => u.FirstName == input || u.LastName == input || u.Email == input).ToList();
            return users;
        }
        public static List<User> QueryUserAmountRange()
        {
            Decimal high; Decimal low;
            Console.WriteLine("Enter highest and lowest value!");
            high = Convert.ToDecimal(Console.ReadLine());
            low = Convert.ToDecimal(Console.ReadLine());
            List<User> users = UserList.Where(u => u.Amount >= low && u.Amount <= high).ToList();
            return users;
        }
        public static List<User> QueryUserTransactionDate()
        {
           // Console.WriteLine("Give start input in dd/mm/yyyy");
            string st;
            st = "01/08/2016 14:50:50.42";
            DateTime start = Convert.ToDateTime(st);
            DateTime end = DateTime.Today;

            List<User> users = UserList.Where(u => u.Transactions != null && u.Transactions.Any(t => t.DateTransaction != null && DateTime.Compare(t.DateTransaction, start) >= 0 && DateTime.Compare(t.DateTransaction,end) <= 0)).ToList();
            return users;

        }
        public static List<User> QueryUserNelement()
        {
            int n;
            Console.WriteLine("Enter number to see first N elements!");
            n = Convert.ToInt32(Console.ReadLine());
            List<User> users = UserList.OrderBy(u => u.ID).Take(n).ToList();
            return users;
        }
        public static List<User> QueryUserLastNelement()
        {
            int n;
            Console.WriteLine("Enter number to see last N elements!");
            n = Convert.ToInt32(Console.ReadLine());
            List<User> users = UserList.OrderBy(u => u.ID).Skip(UserList.Count - n).ToList();
            return users;
        }
        public static List<User> EmailTest()
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            List<User> users = UserList.Where(u => regex.IsMatch(u.Email)).ToList();
            Console.WriteLine("Email Test!");
            return users;
        }
        public static List<User> ComplexQuery()
        {
            List<User> users = new List<User>();
            Console.WriteLine("QueryList!");
            Console.WriteLine("1.Search By String");
            Console.WriteLine("2.Search By Amount Range!!");
            Console.WriteLine("3:Search By Transaction Date!");
            Console.WriteLine("4.Search  First N Element in List By Id");
            Console.WriteLine("5.Search By Last N Element in List By Id");
            Console.WriteLine("6.Show the valid email users!");
            int querynum,press;
            //users = users.Union(QueryUserLastNelement()).ToList();
            Console.WriteLine("How many query you want to run?Enter the number!");
            querynum = Convert.ToInt32(Console.ReadLine());
           
            for (int i = 0; i < querynum; i++)
            {
                Console.WriteLine("Press the query number");
                press = Convert.ToInt32(Console.ReadLine());
                if (press == 1)
                    users = users.Union(QueryByString()).ToList();
                else if (press == 2)
                    users = users.Union(QueryUserAmountRange()).ToList();
                else if (press == 3)
                    users = users.Union(QueryUserTransactionDate()).ToList();
                else if (press == 4)
                    users = users.Union(QueryUserNelement()).ToList();
                else if (press == 5)
                    users = users.Union(QueryUserLastNelement()).ToList();
                else if (press == 6)
                    users = users.Union(EmailTest()).ToList();
                

            }
            foreach (User user in users)
                user.ShowTitle();
            return users;
        }
        
        public static void CustomList()
        {
            GetfromJson();
            IList<CustomUser> customUsers = new List<CustomUser>();
            foreach(User user in UserList)
            {
                CustomUser cUser = new CustomUser();
                cUser.FullName = user.FirstName+" "+user.LastName;
                cUser.Email = user.Email;
               if(user.Transactions != null) cUser.Amount = user.Transactions.Sum(t =>t.Amount);
                cUser.Amount += user.Amount;
                customUsers.Add(cUser);
            }
            foreach (var customUser in customUsers)
                Console.WriteLine(customUser.FullName + " " + customUser.Email + " " + customUser.Amount);           


        }

    }
}
