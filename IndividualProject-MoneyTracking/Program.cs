// See https://aka.ms/new-console-template for more information
using IndividualProject_MoneyTracking.Classes;
using System;
using System.Diagnostics;
using System.Linq;

Console.WriteLine("Welcome to TrackMoney");
Account account = new Account();
account.LoadItemsFromFile();
//account.accountList.Add(new IncomeItem("Aalary", new DateTime(2001,02,13), 1));  //Added items for testing purposes
//account.accountList.Add(new IncomeItem("qalary", new DateTime(2001, 02, 13), 3));  //Added items for testing purposes
//account.accountList.Add(new ExpenseItem("Aar payment", new DateTime(2001, 02, 13), 2));//Added items for testing purposes 
//account.accountList.Add(new ExpenseItem("qaar payment", new DateTime(2001, 02, 13), 4));//Added items for testing purposes 

Console.WriteLine($"Your current balance is {account.CalculateBalance()}\n");


string option = "";
do
{
  
    Console.WriteLine("Select one of the following options \n");
    Console.WriteLine("(1) Show items All/Expenses/Incomes ");
    Console.WriteLine("(2) Add a new item  Expense/Income");
    Console.WriteLine("(3) Edit an item");
    Console.WriteLine("(4) Delete an item");
    Console.WriteLine("(5) Save your items");
    Console.WriteLine("(6) Load your items");
    Console.WriteLine("(7) Quit the application");
    option = Console.ReadLine();    
    
    switch (option)
    {
        case "1":
            account.ChooseItemsToDisplay();
            break;
        case "2":    
            account.AddAnItem();
            break;
        case "3":
            account.EditItem();
            break;
        case "4":
            account.DeleteItem();
            break;
        case "5":
            account.SaveItemsToFile();
             break;
        case "6":
            account.LoadItemsFromFile();
            break;
        case "7":
            Console.WriteLine("Quit");
            break;
        default:
            Console.WriteLine("Please enter a valid number");
            break;
    }

}
while(option != "7");


Console.WriteLine("Thank you for using the application");




/*
 Agenda:
Create list class which we add then add the individual items with identifers if they are expense or income / Create child classes that we add into a list so we know what type of item it is
Create a way to display the list items. First all and then make it so that it displays based on what the user wants - also make sorting method for sorting based on whatever the kravspec is.
Create method for editing with acceptable UI
Create method for saving to file and reading from file to create some sort of state.
Create method that gets the balance of the user. 
*/