using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections;
using System.Text.Json.Serialization;
using System.Runtime.ConstrainedExecution;
using Newtonsoft.Json;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace IndividualProject_MoneyTracking.Classes
{

    enum ItemType
    {
        IncomeItem,
        ExpenseItem
    }

    internal class Account
    {

        private double balance;
        private double amount;

        [JsonIgnore]
        public double Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        public string ItemType { get; set; }
        public string Title
        {
            get;
            set;
        }
        public DateTime Month
        {
            get;
            set;
        }


        public double Amount
        {
            get { return amount; }
            set
            {
                if (amount < 0)
                {
                    amount = 0;
                }
                else
                {
                    amount = value;
                }
            }
        }

        readonly string folderPath = @"C:\MoneyTracking\data\";
        readonly string fileName = "data.txt";

        public List<Account> accountList = new List<Account>();

        public Account()
        {

        }

        public Account(string title, DateTime month, double amount, string itemType)
        {
            Title = title;
            Month = month;
            Amount = amount;
            ItemType = itemType;
        }


        public void AddAnItem()
        {

            string choice = "";
            while (true)
            {
                Console.WriteLine("Select one of the following item types");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("(1) Add an expense ");
                Console.WriteLine("(2) Add an income");
                Console.WriteLine("(3) Go back to the main menu");
                choice = Console.ReadLine();


                if (!(choice == "1" || choice == "2" || choice == "3"))
                {
                    Console.WriteLine("Please only enter the available options");
                    continue;
                }
                else
                {
                    break;
                }

            }

            while (true)
            {
                double amount;
                Console.Write("Title: ");
                string title = Console.ReadLine();
                Console.Write("Amount: ");
                bool doubleTest = double.TryParse(Console.ReadLine(), out amount);
                if (!doubleTest)
                {
                    Console.WriteLine("Please enter a numerical value");
                    continue;
                }

                DateTime datePurchased;
                Console.Write("Date purchased (YYYY-MM-DD format): ");
                bool dateTest = DateTime.TryParse(Console.ReadLine(), out datePurchased);
                if (!dateTest)
                {
                    Console.WriteLine("Please enter a correct date");
                    continue;
                }

                if (choice == "1")
                {
                    accountList.Add(new IncomeItem(title, datePurchased, amount, ItemType = "Income"));
                    break;
                }
                else if (choice == "2")
                {
                    accountList.Add(new ExpenseItem(title, datePurchased, amount, ItemType = "Expense"));
                    break;
                }

            }
            printTextMsg("\nYour item has been added succesfully", ConsoleColor.Green);
            SaveItemsToFile();
        }

        public void EditItem()
        {
            int choice = 0;
            int indexOfItemToBeEdited = 0;
            if (!accountList.Any())
            {
                printTextMsg("You don't have any saved items", ConsoleColor.Red);
            }
            else
            {
                while (true)
                {

                    ChooseEditItem(choice, indexOfItemToBeEdited);

                    EditItemBody(indexOfItemToBeEdited);

                    printTextMsg("Your item has been changed", ConsoleColor.Yellow);
                    Console.WriteLine("\nEnter q to quit | Enter r to restart");
                    string exitChoice = Console.ReadLine();
                    if (exitChoice.Trim().ToLower() == "q")
                    {
                        break;
                    }
                    else if (exitChoice.Trim().ToLower() == "r")
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("You have entered the wrong command");
                    }
                }
            }
        }
        public void ChooseEditItem(int choice, int indexOfItemToBeEdited)
        {
            while (true)
            {
                int Id = 0;
                printTextMsg("Your current items", ConsoleColor.Yellow);
                Console.WriteLine("Id ".PadRight(5) + "Title".PadRight(10) + "Amount".PadRight(10) + "Month".PadRight(13) + "Item type");
                for (int i = 0; i < accountList.Count; i++)
                {
                    Id = i + 1;

                    DisplayCardWithId(accountList[i].Title, accountList[i].amount, accountList[i].Month, accountList[i].ItemType, Id);
                }
                Console.Write("Enter the number of the item you wish to edit: ");
                choice = int.Parse(Console.ReadLine());
                if (choice > accountList.Count)
                {
                    printTextMsg($"Please only choose one of the {accountList.Count} options \n", ConsoleColor.Red);
                    continue;
                }
                else
                {
                    break;
                }
                indexOfItemToBeEdited = choice - 1;
            }
        }
        public void EditItemBody(int indexOfItemToBeEdited)
        {
            while (true)
            {
                Console.Write("Title: ");
                string title = Console.ReadLine();
                Console.Write("Amount: ");
                double amount;
                bool doubleTest = double.TryParse(Console.ReadLine(), out amount);
                if (!doubleTest)
                {
                    printTextMsg("Please enter a numerical value", ConsoleColor.Red);
                    continue;
                }

                Console.Write("Month: ");
                DateTime month;
                bool dateTest = DateTime.TryParse(Console.ReadLine(), out month);
                if (!dateTest)
                {
                    printTextMsg("Please enter a correct date", ConsoleColor.Red);
                    continue;
                }


                if (accountList[indexOfItemToBeEdited].ItemType == "Income")
                {
                    accountList[indexOfItemToBeEdited] = new IncomeItem(title, month, amount, ItemType = "Income");
                    break;
                }
                else if (accountList[indexOfItemToBeEdited].ItemType == "Expense")
                {
                    accountList[indexOfItemToBeEdited] = new ExpenseItem(title, month, amount, ItemType = "Expense");
                    break;
                }
            }
        }
        public void printTextMsg(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public void DeleteItem()
        {

            while (true)
            {
                int Id = 0;
                int choice = 0;
                Console.WriteLine("Your items: \n");
                Console.WriteLine("Id ".PadRight(5) + "Title".PadRight(10) + "Amount".PadRight(10) + "Month".PadRight(13) + "Item type");
                for (int i = 0; i < accountList.Count; i++)
                {
                    Id = i + 1;
                    Console.WriteLine($"({i + 1}) Title: {accountList[i].Title}\n Amount: {accountList[i].Amount}\n Month: {accountList[i].Month.ToLongDateString()} ");
                    DisplayCardWithId(accountList[i].Title, accountList[i].amount, accountList[i].Month, accountList[i].ItemType, Id);
                }
                Console.WriteLine("Choose the number of the item you wish to delete: ");



                bool test = int.TryParse(Console.ReadLine(), out choice);

                if (choice + 1 > accountList.Count)

                {
                    printTextMsg($"Please only choose one of the {accountList.Count} options \n", ConsoleColor.Red);
                    continue;
                }

                accountList.RemoveAt(choice - 1);

                printTextMsg("Your items have been deleted succesfully", ConsoleColor.Green);


            }
        }


        public void SaveItemsToFile()
        {

            string filePath = $"{folderPath}{fileName}";
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string jsonString = JsonConvert.SerializeObject(accountList, settings);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            printTextMsg("Your items have been saved", ConsoleColor.Green);

            File.WriteAllText(filePath, jsonString);

        }

        public void LoadItemsFromFile()
        {

            string fullPath = $"{folderPath}{fileName}";
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("No files saved");
            }
            if (File.Exists(fullPath))
            {
                string lines = File.ReadAllText(fullPath);
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                var records = JsonConvert.DeserializeObject<List<Account>>(lines, settings);
                accountList = records;
                if (!accountList.Any())
                {
                    printTextMsg("Your items have been loaded!", ConsoleColor.Green);
                }
            }
            else
            {
                Console.WriteLine("File does not exist");
            }



        }
        public double CalculateBalance()
        {
            var incomeList = accountList.OfType<IncomeItem>().ToList();
            var expenseList = accountList.OfType<ExpenseItem>().ToList();

            double expenseTotal = expenseList.Sum(x => x.Amount);
            double incomeTotal = incomeList.Sum(x => x.Amount);

            this.Balance = incomeTotal - expenseTotal;

            return Balance;
        }

        public void ChooseItemsToDisplay()
        {
            string choice = "";
            if (!accountList.Any())
            {
                printTextMsg("You don't have any saved items", ConsoleColor.Red);
            }
            else
            {
                while (true)
                {
                    Console.WriteLine("Select one of the following options");
                    Console.WriteLine("(1) Show all");
                    Console.WriteLine("(2) Show incomes");
                    Console.WriteLine("(3) Show expenses");
                    Console.WriteLine("(4) Go back to main menu");
                    choice = Console.ReadLine();


                    if (!(choice == "1" || choice == "2" || choice == "3" || choice == "4"))
                    {
                        Console.WriteLine("Wrong input");
                    }
                    else if (choice == "4")
                    {
                        break;
                    }
                    else
                    {

                    }
                    ChooseSorting(choice);
                    Console.ResetColor();

                }



            }
        }
        public void ChooseSorting(string choice)
        {

            string sortChoice = "";
            while (true)
            {

                Console.WriteLine("How do you want your list to be sorted");
                Console.WriteLine("(1) By Ascending Title ");
                Console.WriteLine("(2) By Descending Title ");
                Console.WriteLine("(3) By Ascending Amount ");
                Console.WriteLine("(4) By Descending Amount ");
                Console.WriteLine("(5) By Ascending Month ");
                Console.WriteLine("(6) By Descending Month ");
                sortChoice = Console.ReadLine();
                if (!(sortChoice == "1" || sortChoice == "2" || sortChoice == "3" || sortChoice == "4" || sortChoice == "5" || sortChoice == "6"))
                {
                    Console.WriteLine("Please only choose one of the six options");
                }
                else
                {
                    break;
                }

            }
            List<Account> listToBeSorted = new List<Account>();

            switch (sortChoice)
            {
                case "1":
                    listToBeSorted = accountList.OrderBy(x => x.Title).ToList();
                    break;
                case "2":
                    listToBeSorted = accountList.OrderByDescending(x => x.Title).ToList();
                    break;
                case "3":
                    listToBeSorted = accountList.OrderBy(x => x.Amount).ToList();
                    break;
                case "4":
                    listToBeSorted = accountList.OrderByDescending(x => x.Amount).ToList();
                    break;
                case "5":
                    listToBeSorted = accountList.OrderBy(x => x.Month).ToList();
                    break;
                case "6":
                    listToBeSorted = accountList.OrderByDescending(x => x.Month).ToList();
                    break;
            }

            DisplayItems(choice, sortChoice, listToBeSorted);
        }



        public void DisplayItems(string choice, string sortChoice, List<Account> sortedList)
        {
            while (true)
            {
                var expenseList = sortedList.OfType<ExpenseItem>().ToList();
                var incomeList = sortedList.OfType<IncomeItem>().ToList();
                Console.WriteLine("Your items: \n");
                Console.WriteLine("Title".PadRight(10) + "Amount".PadRight(10) + "Month".PadRight(13) + "Item type");
                if (choice == "1")
                {
                    foreach (Account item in sortedList)
                    {
                        DisplayCard(item.Title, item.Amount, item.Month, item.ItemType, ConsoleColor.Gray);
                    }
                }
                else if (choice == "2")
                {

                    foreach (ExpenseItem item in expenseList)
                    {
                        DisplayCard(item.Title, item.Amount, item.Month, item.ItemType, ConsoleColor.Cyan);
                    }
                }
                else if (choice == "3")
                {

                    foreach (IncomeItem item in incomeList)
                    {
                        DisplayCard(item.Title, item.Amount, item.Month, item.ItemType, ConsoleColor.Blue);
                    }
                }


                Console.WriteLine("\nEnter q to quit");
                string exitChoice = Console.ReadLine();
                if (exitChoice.Trim().ToLower() == "q")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("You have entered the wrong command");
                }
            }
        }
        public void DisplayCard(string title, double amount, DateTime month, string itemType, ConsoleColor color)

        {
            if (itemType == "Expense")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{title.PadRight(10)}{amount.ToString().PadRight(10)}{month.ToString("MMMM").PadRight(13)}{itemType.PadRight(10)}");
                Console.ResetColor();
            }
            else if (itemType == "Income")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{title.PadRight(10)}{amount.ToString().PadRight(10)}{month.ToString("MMMM").PadRight(13)}{itemType.PadRight(10)}");
                Console.ResetColor();
            }

        }
        public void DisplayCardWithId(string title, double amount, DateTime month, string itemType, int Id)

        {
            if (itemType == "Expense")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{Id.ToString().PadRight(5)}{title.PadRight(10)}{amount.ToString().PadRight(10)}{month.ToString("MMMM").PadRight(13)}{itemType.PadRight(10)}");
                Console.ResetColor();
            }
            else if (itemType == "Income")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{Id.ToString().PadRight(5)}{title.PadRight(10)}{amount.ToString().PadRight(10)}{month.ToString("MMMM").PadRight(13)}{itemType.PadRight(10)}");
                Console.ResetColor();
            }

        }

    }
}
