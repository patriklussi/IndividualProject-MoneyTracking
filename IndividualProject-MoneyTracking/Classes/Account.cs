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
                amount = value; ;
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

        public void ChooseTypeOfItemToAdd()  // Choose the type of item to add
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


                if (choice == "1" || choice == "2")
                {
                    AddItemBody(choice);
                    PrintTextMsg("\nYour item has been added succesfully", ConsoleColor.Green);
                }
                else if (choice == "3")
                {
                    break;
                }
                else
                {
                    PrintTextMsg("Please only enter one of the available options", ConsoleColor.Red);
                }

            }


        }

        public void AddItemBody(string choice) //Add the items to the account list.
        {

            while (true)
            {
                double amount;
                Console.Write("Title: ");
                string title = Console.ReadLine();
                if (title == "")
                {
                    PrintTextMsg("Title cannot be empty", ConsoleColor.Red);
                    continue;
                }
                else if (title.Length > 15)
                {
                    PrintTextMsg("Title cannot be longer than 15 characters", ConsoleColor.Red);
                    continue;
                }
                Console.Write("Amount: ");
                bool doubleTest = double.TryParse(Console.ReadLine(), out amount);
                if (!doubleTest)
                {
                    PrintTextMsg("Please add a numerical value", ConsoleColor.Red);
                    continue;
                }

                DateTime datePurchased;
                Console.Write("Date purchased (YYYY-MM-DD format): ");
                bool dateTest = DateTime.TryParse(Console.ReadLine(), out datePurchased);
                if (!dateTest)
                {
                    PrintTextMsg("Please enter a correct date", ConsoleColor.Red);
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
        }
        public void CheckEdit()
            //Function to make sure that user can go back to menu if they changed their mind about wanting to edit/delete
        {
            if (!accountList.Any())
            {
                PrintTextMsg("You don't have any saved items", ConsoleColor.Red);
            } else
            {
                while (true) { 
                Console.WriteLine("(1) Edit an item");
                Console.WriteLine("(2) Go back to the main menu");
                string choice = Console.ReadLine();
                if(choice == "1")
                {
                    EditItem();
                } else if(choice == "2")
                {
                    break;
                } else
                    {
                        PrintTextMsg("Please only enter an available option", ConsoleColor.Red);
                    }
                }
            }

        }
        public void EditItem()
        //Function to edit an existing item.
        //If the list is empty no operations work.
        {
            int choice = 0;
            int indexOfItemToBeEdited = 0;
           
            
                while (true)
                {

                    ChooseEditItem(choice, indexOfItemToBeEdited); //Function to choose what specific item to edit



                    PrintTextMsg("Your item has been changed", ConsoleColor.Yellow);
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
                    PrintTextMsg("You have entered the wrong command", ConsoleColor.Red);
                    }
                }
            
        }
        public void ChooseEditItem(int choice, int indexOfItemToBeEdited) //Function to choose what specific item to edit
        {
            while (true)
            {
                int Id = 0;

                PrintTextMsg("Your current items: ", ConsoleColor.Yellow);
                PrintItemsWithForLoop(Id);
                Console.Write("\nNumber of the item you wish to edit: ");
                bool test = int.TryParse(Console.ReadLine(), out choice);
                if (!test)
                {
                    PrintTextMsg("Please only enter one of the numbers", ConsoleColor.Red);
                    continue;
                }
                indexOfItemToBeEdited = choice - 1;
                if (choice > accountList.Count) //Check if user inupt is higher than avaliable options.
                {
                    PrintTextMsg($"Please only choose one of the {accountList.Count} options \n", ConsoleColor.Red);
                    continue;
                }
                else
                {
                    break;
                }

            }
            EditItemBody(indexOfItemToBeEdited);
        }
        public void EditItemBody(int indexOfItemToBeEdited) //To get the new values to be added to the chosen item in the list
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
                    PrintTextMsg("Please enter a numerical value", ConsoleColor.Red);
                    continue;
                }

                Console.Write("Month: ");
                DateTime month;
                bool dateTest = DateTime.TryParse(Console.ReadLine(), out month);
                if (!dateTest)
                {
                    PrintTextMsg("Please enter a correct date", ConsoleColor.Red);
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
                /*
                 Switch the existing item with the new one by using the index of the old item.
                 */
            }
        }
        public void PrintItemsWithForLoop(int Id)
        {
            Console.WriteLine("Id ".PadRight(5) + "Title".PadRight(16) + "Amount".PadRight(16) + "Month".PadRight(19) + "Item type");
            for (int i = 0; i < accountList.Count; i++) //Loop out the items in the list and show them to the user
            {
                Id = i + 1;

                DisplayCardWithId(accountList[i].Title, accountList[i].amount, accountList[i].Month, accountList[i].ItemType, Id);
            }
        }
        public void PrintTextMsg(string text, ConsoleColor color) //Function to print console messages that uses color changes.
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }


        public void CheckDelete()
        //Function to make sure that user can go back to menu if they changed their mind about wanting to edit/delete
        {

            if (!accountList.Any())
            {
                PrintTextMsg("You don't have any saved items", ConsoleColor.Red);
            } else
            {
                while (true)
                {
                    Console.WriteLine("(1) Delete an item");
                    Console.WriteLine("(2) Go back to main menu");
                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        DeleteItem();
                    }
                    else if (choice.Trim() == "2")
                    {
                        break;
                    } else
                    {
                        PrintTextMsg("Please only enter an available option", ConsoleColor.Red);
                    }
                }
             
            }
        }

        public void DeleteItem()
        //This function delets a singluar item in the list. 
        //If the list is empty no operations work.
        {

            int Id = 0;
            int choice = 0;
            Console.WriteLine("Your items: \n");
            PrintItemsWithForLoop(Id);
            while (true)
            {
                Console.Write("\nNumber of the item you wish to delete: ");
                bool test = int.TryParse(Console.ReadLine(), out choice);
                if (choice > accountList.Count)

                {
                    PrintTextMsg($"Please only choose one of the {accountList.Count} options \n", ConsoleColor.Red);
                }
                else
                {
                    break;
                }
            }


            accountList.RemoveAt(choice - 1); //Based on the index of the selected item we delete the item from the list.

            PrintTextMsg("Your items have been deleted succesfully", ConsoleColor.Green);






        }


        public void SaveItemsToFile()
        //The function takes the list and json serializes it using a json library
        // The jsonstring then gets added to a text file which gets saved in a directory
        //If the list is empty no operations work.
        {
            if (!accountList.Any())
            {
                PrintTextMsg("You don't have any saved items", ConsoleColor.Red);
            }
            else
            {
                string filePath = $"{folderPath}{fileName}";
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                string jsonString = JsonConvert.SerializeObject(accountList, settings);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                PrintTextMsg("Your items have been saved", ConsoleColor.Green);

                File.WriteAllText(filePath, jsonString);
            }


        }

        public void LoadItemsFromFile()
        //Function loads items from a text file 
        //If the list is empty no operations work.
        //The function deserilalizes the jsonstring in the file and then adds the values back into the account list.
        {

            string fullPath = $"{folderPath}{fileName}";
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("No directory exists");
            }
            if (File.Exists(fullPath))
            {
                string lines = File.ReadAllText(fullPath);
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                var records = JsonConvert.DeserializeObject<List<Account>>(lines, settings);
                accountList = records;



                PrintTextMsg("Your items have been loaded!", ConsoleColor.Green);

            }
            else
            {
                PrintTextMsg("File does not exist", ConsoleColor.Red);
            }



        }
        public double CalculateBalance()
        //Function to calculate the current balance based on the expenses and incomes
        {
            var incomeList = accountList.OfType<IncomeItem>().ToList();
            var expenseList = accountList.OfType<ExpenseItem>().ToList();

            double expenseTotal = expenseList.Sum(x => x.Amount);
            double incomeTotal = incomeList.Sum(x => x.Amount);

            this.Balance = incomeTotal - expenseTotal;

            return Balance;
        }

        public void ChooseItemsToDisplay()
        //Choose the items to be displayed
        //If the account list is empty no operation will happen.

        {
            string choice = "";
            if (!accountList.Any())
            {
                PrintTextMsg("You don't have any saved items", ConsoleColor.Red);
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
                        PrintTextMsg("Wrong input", ConsoleColor.Red);
                        continue;
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
        //Choose the way of sorting the items to be displayed with a switch case and a readline
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
                    PrintTextMsg("Please only choose one of the 6 options\n", ConsoleColor.Red);
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
        //Display the items using the previous choice the user did
        {
            while (true)
            {
                var expenseList = sortedList.OfType<ExpenseItem>().ToList();
                var incomeList = sortedList.OfType<IncomeItem>().ToList();
                Console.WriteLine("Your items: \n");
                Console.WriteLine("Title".PadRight(16) + "Amount".PadRight(16) + "Month".PadRight(19) + "Item type");
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
                    PrintTextMsg("You have entered the wrong command", ConsoleColor.Red);
                }
            }
        }
        public void DisplayCard(string title, double amount, DateTime month, string itemType, ConsoleColor color)
        //Function to display the items for the normal display items function.
        //Based on the type of item the text color is either blue or cyan.
        {
            if (itemType == "Expense")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{title.PadRight(16)}{amount.ToString().PadRight(16)}{month.ToString("MMMM").PadRight(19)}{itemType.PadRight(16)}");
                Console.ResetColor();
            }
            else if (itemType == "Income")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{title.PadRight(16)}{amount.ToString().PadRight(16)}{month.ToString("MMMM").PadRight(19)}{itemType.PadRight(16)}");
                Console.ResetColor();
            }

        }
        public void DisplayCardWithId(string title, double amount, DateTime month, string itemType, int Id)
        //Function to display the list items with id included for edit and delete functionality
        //Based on type of item the text gets either a blue or cyan color.

        {
            if (itemType == "Expense")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{Id.ToString().PadRight(5)}{title.PadRight(16)}{amount.ToString().PadRight(16)}{month.ToString("MMMM").PadRight(19)}{itemType.PadRight(16)}");
                Console.ResetColor();
            }
            else if (itemType == "Income")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{Id.ToString().PadRight(5)}{title.PadRight(16)}{amount.ToString().PadRight(16)}{month.ToString("MMMM").PadRight(19)}{itemType.PadRight(16)}");
                Console.ResetColor();
            }

        }

    }
}

