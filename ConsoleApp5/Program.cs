using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ContactManager
{
    [Serializable]
    public class Contact
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }

        public Contact()
        {

        }
        public Contact(string name, string lastname, string number, string address = "")
        {
            this.Name = name;
            this.LastName = lastname;
            this.Number = number;
            this.Address = address;

        }
    }

    class ContactBook
    {
        public List<Contact> addresses;

        public ContactBook()
        {
            if(File.Exists("persons.xml"))
            addresses = XMLFILE.READXML();
                else
                addresses = new List<Contact>();
        }

        public bool add(string name, string lastname, string number, string address)
        {
            Contact addr = new Contact(name, lastname, number, address);
            Contact result = find(number);

            if (result == null) //if null = not found
            {
                addresses.Add(addr);
                XMLFILE.WriteXML(addresses);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool remove(string phone)
        {
            Contact addr = find(phone);

            if (addr != null)
            {
                addresses.Remove(addr);
                XMLFILE.WriteXML(addresses);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void update(Contact addr)
        {
            Console.WriteLine("Edit contact: ");
            Console.WriteLine("Enter Name: ");
            addr.Name = Console.ReadLine();
            Console.WriteLine("Enter Last Name: ");
            addr.LastName = Console.ReadLine();
            Console.WriteLine("Enter phone number: ");
            addr.Number = Console.ReadLine();
            Console.WriteLine("Address: ");
            addr.Address = Console.ReadLine();
            Console.WriteLine("Contact updated");

            XMLFILE.WriteXML(addresses);
        }
        public void ShowAll()
        {
            foreach (Contact addr in addresses)
            {
                Console.WriteLine("name: {0}, lastname: {1}, number: {2}, address: {3}", addr.Name, addr.LastName, addr.Number, addr.Address);
            }
        }

        public bool isEmpty()
        {
            return (addresses.Count == 0);
        }

        public Contact find(string number)
        {
            Contact addr = addresses.Find((a) => a.Number == number);
            return addr;
        }
    }


    public class ContactPrompt
    {
        ContactBook book;

        public ContactPrompt()
        {
            book = new ContactBook();
        }

        static void Main(string[] args)
        {
            string selection = "";
            ContactPrompt prompt = new ContactPrompt();

            prompt.displayMenu();
            while (!selection.Equals("5"))// 5 to quit
            {
                Console.WriteLine("Selection: ");
                selection = Console.ReadLine();
                prompt.performAction(selection);
            }
        }

        void displayMenu()
        {
            Console.WriteLine("Main Menu");
            Console.WriteLine("1 - Add an Contact");
            Console.WriteLine("2 - Delete an Contact");
            Console.WriteLine("3 - Edit an Contact");
            Console.WriteLine("4 - List All Contacts");
            Console.WriteLine("5 - Quit");
        }

        void performAction(string selection)
        {
            string name = "";
            string lastname = "";
            string phone = "";
            string address = "";

            switch (selection)
            {
                case "1":
                    Console.WriteLine("Enter Name: ");
                    name = Console.ReadLine();
                    Console.WriteLine("Enter Last Name: ");
                    lastname = Console.ReadLine();
                    Console.WriteLine("Enter phone number: ");
                    phone = Console.ReadLine();
                    Console.WriteLine("Address: ");
                    address = Console.ReadLine();
                    if (book.add(name, lastname, phone, address))// call book.add
                    {
                        Console.WriteLine("Contact successfully added!");
                    }
                    else
                    {
                        Console.WriteLine("Numbers is already used.");
                    }
                    break;
                case "2":
                    Console.WriteLine("Enter phone to Delete: ");
                    phone = Console.ReadLine();
                    if (book.remove(phone))
                    {
                        Console.WriteLine("Contact successfully removed");
                    }
                    else
                    {
                        Console.WriteLine("Phone could not be found.", name);
                    }
                    break;
                case "4":
                    if (book.isEmpty())
                    {
                        Console.WriteLine("There are no entries.");
                    }
                    else
                    {
                        Console.WriteLine("Contacts:");
                        book.ShowAll();
                    }
                    break;
                case "3":
                    Console.WriteLine("Enter phone number to Edit: ");
                    phone = Console.ReadLine();
                    Contact addr = book.find(phone);
                    if (addr == null)
                    {
                        Console.WriteLine("Contact could not be found.", phone);
                    }
                    else
                    {
                        book.update(addr);
                    }

                    break;
            }
        }
    }

    public static class XMLFILE

    {
        internal static void WriteXML(List<Contact> Contacts) // Write XML
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Contact>));
            using (FileStream fs = new FileStream("persons.xml", FileMode.Create))
            {
                formatter.Serialize(fs, Contacts);
            }
        }
        internal static List<Contact> READXML() // Read XML
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Contact>));
            List<Contact> tempContacts = new List<Contact>();
            using (FileStream fs = new FileStream("persons.xml", FileMode.OpenOrCreate))
            {
                tempContacts = (List<Contact>)formatter.Deserialize(fs);
            }
            return tempContacts;
        }
    }
}
