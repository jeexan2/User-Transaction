using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserTransaction.Domain;

namespace UserTransaction.DAL
{
    public static class ExtensionMethods
    {

    }
    public class DataLayer
    {
        
        public IList<User> UserList;
        //public Transaction trans = new Transaction();
        public DataLayer()
        {
            UserList = new List<User>();
           // Console.WriteLine("Constructor called!");

            GetfromJson();
        }
        public void SaveInJson()
        {
            
            File.WriteAllText(@"C:\Users\j.aziz\Desktop\user.json", JsonConvert.SerializeObject(UserList));
            using (StreamWriter file = File.CreateText(@"C:\Users\j.aziz\Desktop\user.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, UserList);
            }

        }
        public void Show()
        {
            if (UserList != null)
                foreach (var user in UserList)
                {
                    Console.WriteLine("Hi {0} {1}!!You have: {2} Taka!Your id is: {3}", user.FirstName, user.LastName, user.Amount, user.ID);
                }


        }
        public void GetfromJson()
        {

            UserList = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@"C:\Users\j.aziz\Desktop\user.json"));
            

        }

        public User SearchById(int id)
        {
            var user = UserList.SingleOrDefault(x => x.ID == id);
            if (user != null) return user;
            else return new User();
        }

        public void AddUser()
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
       
            if(UserList == null) UserList = new List<User>();
            UserList.Add(user);

           

            SaveInJson();
            GetfromJson();
            user.Transactions = new List<Transaction>();
        }
        public void DeleteUser(int id)
        {

            var item = SearchById(id);
            if (item != null)
                UserList.Remove(item);
            SaveInJson();
        }
        public void ModifyUser()
        {
            int id;
            Console.WriteLine("Enter the Specific Id to Update!");
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
        public void UpdateamountUser(User user, Decimal deposit)
        {
            user.Amount += deposit;
        }
        public void AddTransaction()
        {
            int id;
            Console.WriteLine("Enter Id to Add Transaction");
            id = Convert.ToInt32(Console.ReadLine());
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
                        UpdateamountUser(user, amount);
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


       public void QueryShowList(string input)
        {
             
           var users = UserList.Where(u => u.FirstName == input || u.LastName == input || u.Email == input);
            foreach (var user in users)
                Console.WriteLine(user.FirstName + " " + user.LastName);
        }

        public void QueryUserAmountRange(Decimal high,Decimal low)
        {
            var users = UserList.Where(u => u.Amount >= low && u.Amount <= high);
            foreach (var user in users)
                Console.WriteLine(user.FirstName + " " + user.LastName);
        }
        public void QueryUserTransaction()
        {
            DateTime start=DateTime.Today;
            DateTime end = DateTime.Today;

             var users = UserList.Where(u=> u.Transactions != null && u.Transactions.Any(t =>t.DateTransaction!=null && DateTime.Compare(t.DateTransaction,start)!=0));
                foreach (var user in users)
                    Console.WriteLine(user.FirstName + " " + user.LastName);

        }
        public void QueryUserNelement(int n)
        {
            var users = UserList.OrderBy(u => u.ID).Take(n);
            foreach (var user in users)
                Console.WriteLine(user.FirstName + " " + user.LastName);
        }
        public void QueryUserLastNelement(int n)
        {
            var users = UserList.OrderBy(u => u.ID).Skip(UserList.Count - n).ToList();
            foreach (var user in users)
                Console.WriteLine(user.FirstName + " " + user.LastName);
        }
        public void EmailTest()
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var users = UserList.Select(u => regex.Match(u.Email).Value);
            foreach (var user in users)
                if(user != "") Console.WriteLine(user);

        }
        
        public void ShowCustomList()
        {

            
        }
    }

}
