// See https://aka.ms/new-console-template for more information
using IndividualProject_MoneyTracking.Classes;
using System;
using System.Diagnostics;
using System.Linq;

Console.WriteLine("Welcome to TrackMoney");
Account account = new Account();
//Uncomment these lines for fast testing of application
//account.accountList.Add(new IncomeItem("Salary", new DateTime(2001,02,13),25000,"Income"));  //Added items for testing purposes
//account.accountList.Add(new IncomeItem("Swish payment", new DateTime(2001,02,13), 250,"Income"));  //Added items for testing purposes
//account.accountList.Add(new ExpenseItem("Car payment", new DateTime(2001,02,13), 1000,"Expense"));//Added items for testing purposes 
//account.accountList.Add(new ExpenseItem("Groceries", new DateTime(2001,02,13), 1568, "Expense"));//Added items for testing purposes 





string option = "";
do
{
    Console.WriteLine($"Your current balance is {account.CalculateBalance()}\n");
    Console.WriteLine("Select one of the following options \n");
    Console.WriteLine("(1) Show items All/Expenses/Incomes ");
    Console.WriteLine("(2) Add a new item Expense/Income");
    Console.WriteLine("(3) Edit an item");
    Console.WriteLine("(4) Delete an item");
    Console.WriteLine("(5) Save your items");
    Console.WriteLine("(6) Load your items");
    Console.WriteLine("(7) Quit the application");
    Console.Write("Your choice: ");
    option = Console.ReadLine();    
    
    switch (option)
    {
        case "1":
            account.ChooseItemsToDisplay(); // This function starts the process to view your items
            break;
        case "2":    
            account.ChooseTypeOfItemToAdd(); //Function to add items to the list
            break;
        case "3":
            account.CheckEdit();//Function to edit existing items in the list
            break;
        case "4":
            account.CheckDelete(); //Function to delete existing items in the list
            break;
        case "5":
            account.SaveItemsToFile(); //Function to save existing items in the list to a text file
            break;
        case "6":
            account.LoadItemsFromFile();//Function to load existing from a text file
            break;
        case "7":
            Console.WriteLine("Quit"); //Quit the application
            break;
        default:
            account.PrintTextMsg("Please enter a valid option",ConsoleColor.Red);
            break;
    }

}
while(option != "7");


Console.WriteLine("Thank you for using the application");

