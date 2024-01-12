
using System;
using System.Collections.Generic;

// Main Application Class
public class TableReservationApp
{
    static void Main(string[] args)
    {
        ReservationManagerClass manage = new ReservationManagerClass();
        manage.AddRestaurantMethod("A", 10);
        manage.AddRestaurantMethod("B", 5);

        Console.WriteLine(manage.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(manage.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
    }
}

// Reservation Manager Class
public class ReservationManagerClass
{
    // res
    public List<RestaurantClass> res;

    public ReservationManagerClass()
    {
        res = new List<RestaurantClass>();
    }

    // Add Restaurant Method
    public void AddRestaurantMethod(string name, int table)
    {
        try
        {
            RestaurantClass r = new RestaurantClass();
            r.name = name;
            r.table = new RestaurantTableClass[table];
            for (int i = 0; i < table; i++)
            {
                r.table[i] = new RestaurantTableClass();
            }
            res.Add(r);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    // Load Restaurants From
    // File
    private void LoadRestaurantsFromFileMethod(string fileP)
    {
        try
        {
            string[] ls = File.ReadAllLines(fileP);
            foreach (string l in ls)
            {
                var parts = l.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                {
                    AddRestaurantMethod(parts[0], tableCount);
                }
                else
                {
                    Console.WriteLine(l);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    //Find All Free Tables
    public List<string> FindAllFreeTables(DateTime dt)
    {
        try
        {
            List<string> free = new List<string>();
            foreach (var restaurant in res)
            {
                for (int i = 0; i < restaurant.table.Length; i++)
                {
                    if (!restaurant.table[i].IsBooked(dt))
                    {
                        free.Add($"{restaurant.name} - Table {i + 1}");
                    }
                }
            }
            return free;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return new List<string>();
        }
    }

    public bool BookTable(string rName, DateTime d, int tNumber)
    {
        foreach (var restaurant in res)
        {
            if (restaurant.name == rName)
            {
                if (tNumber < 0 || tNumber >= restaurant.table.Length)
                {
                    throw new Exception(null); //Invalid table number
                }

                return restaurant.table[tNumber].Book(d);
            }
        }

        throw new Exception(null); //Restaurant not found
    }

    public void SortRestaurantsByAvailabilityForUsersMethod(DateTime dt)
    {
        try
        {
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < res.Count - 1; i++)
                {
                    int avTc = CountAvailableTablesForRestaurantClassAndDateTimeMethod(res[i], dt); // available tables current
                    int avTn = CountAvailableTablesForRestaurantClassAndDateTimeMethod(res[i + 1], dt); // available tables next

                    if (avTc < avTn)
                    {
                        // Swap restaurants
                        var temp = res[i];
                        res[i] = res[i + 1];
                        res[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    // count available tables in a restaurant
    public int CountAvailableTablesForRestaurantClassAndDateTimeMethod(RestaurantClass r, DateTime dt)
    {
        try
        {
            int count = 0;
            foreach (var t in r.table)
            {
                if (!t.IsBooked(dt))
                {
                    count++;
                }
            }
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return 0;
        }
    }
}

// Restaurant Class
public class RestaurantClass
{
    public string name; //name
    public RestaurantTableClass[] table; // tables
}

// Table Class
public class RestaurantTableClass
{
    private List<DateTime> bd; //booked dates


    public RestaurantTableClass()
    {
        bd = new List<DateTime>();
    }

    // book
    public bool Book(DateTime d)
    {
        try
        {
            if (bd.Contains(d))
            {
                return false;
            }
            //add to bd
            bd.Add(d);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return false;
        }
    }

    // is booked
    public bool IsBooked(DateTime d)
    {
        return bd.Contains(d);
    }
}
