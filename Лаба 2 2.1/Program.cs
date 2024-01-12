
using System;
using System.Collections.Generic;
using static Restaurant;

public class TableReservationApp
{
    static void Main(string[] args)
    {
        ReservationManager manage = new ReservationManager();
        manage.AddRestaurant("A", 10);
        manage.AddRestaurant("B", 5);

        Console.WriteLine(manage.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(manage.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
    }
}

public class ReservationManager
{
    public List<Restaurant> res;

    public ReservationManager()
    {
        res = new List<Restaurant>();
    }

    public void AddRestaurant(string name, int table)
    {
        try
        {
            Restaurant r = new Restaurant();
            r.name = name;
            r.table = new RestaurantTable[table];
            for (int i = 0; i < table; i++)
            {
                r.table[i] = new RestaurantTable();
            }
            res.Add(r);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    private void LoadRestaurantsFromFile(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                {
                    AddRestaurant(parts[0], tableCount);
                }
                else
                {
                    Console.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine ( "Error LoadRestaurantsFromFile");
        }
    }

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
                    throw new Exception(null);
                }

                return restaurant.table[tNumber].Book(d);
            }
        }

        throw new Exception(null);
    }

    public void SortRestaurantsByAvaila(DateTime dt)
    {
        try
        {
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < res.Count - 1; i++)
                {
                    int avTc = CountAvailableTables(res[i], dt);
                    int avTn = CountAvailableTables(res[i + 1], dt);

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
    public int CountAvailableTables(Restaurant r, DateTime dt)
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

public class Restaurant
{
    public string name;
    public RestaurantTable[] table;

    public class RestaurantTable
    {
        private List<DateTime> bd;


        public RestaurantTable()
        {
            bd = new List<DateTime>();
        }

        public bool Book(DateTime d)
        {
            try
            {
                if (bd.Contains(d))
                {
                    return false;
                }
                bd.Add(d);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                return false;
            }
        }

        public bool IsBooked(DateTime d)
        {
            return bd.Contains(d);
        }
    }
}
