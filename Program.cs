using Project;
using Project.Containers;

public class Program
{
    static readonly List<Ship> Ships = new();
    static readonly List<Container> Containers = new();

    public static void Main()
    {
        bool working = true;
        while(working)
        {
            Console.WriteLine("\nShips list:");
            if (Ships.Count > 0)
                foreach (var s in Ships)
                    Console.WriteLine(s);
            else
                Console.WriteLine("none");

            Console.WriteLine("\nContainer list:");
            if (Containers.Count > 0 )
                foreach(var c in Containers)
                    Console.WriteLine(c);
            else
                Console.WriteLine("none");

            Console.WriteLine("\nPossible Actions");
            Console.WriteLine("1. Create ship");
            Console.WriteLine("2. Create container");
            Console.WriteLine("3. Load container");
            Console.WriteLine("4. Load container on ship");
            Console.WriteLine("5. Load container list on ship");
            Console.WriteLine("6. Unload container from ship");
            Console.WriteLine("7. Unload container");
            Console.WriteLine("8. Change container on ship");
            Console.WriteLine("9. Move container between ships");
            Console.WriteLine("10. Container info");
            Console.WriteLine("11. Ship info");
            Console.WriteLine("0. Exit");
            Console.Write("Choose option: ");

            string choice = Console.ReadLine();

            Console.WriteLine("\n");
            switch (choice)
            {
                case "1":
                    CreateShip();
                    break;
                case "2":
                    CreateContainer();
                    break;
                case "3":
                    LoadContainer();
                    break;
                case "4":
                    LoadContainerOnShip();
                    break;
                case "5":
                    LoadContainerListOnShip();
                    break;
                case "6":
                    UnloadContainerFromShip();
                    break;
                case "7":
                    UnloadContainer();
                    break;
                case "8":
                    ChangeContainerOnShip();
                    break;
                case "9":
                    MoveContainerBetweenShips();
                    break;
                case "10":
                    ShowContainerInfo();
                    break;
                case "11":
                    ShowShipInfo();
                    break;
                case "0":
                    working = false;
                    break;
                default:
                    Console.WriteLine("Unknown option");
                    break;
            }
        }
    }

    static void CreateShip()
    {
        Console.Write("Enter max speed: ");
        double maxSpeed = double.Parse(Console.ReadLine());

        Console.Write("Enter max container count: ");
        int maxContainerCount = int.Parse(Console.ReadLine());

        Console.Write("Enter max weight: ");
        double maxTotalWeight = double.Parse(Console.ReadLine());

        Ships.Add(new Ship(maxSpeed, maxContainerCount, maxTotalWeight));
        Console.WriteLine("Ship created");
    }

    static void CreateContainer()
    {
        Console.WriteLine("Choose container type:");
        Console.WriteLine("\t1. Liquid");
        Console.WriteLine("\t2. Gas");
        Console.WriteLine("\t3. Refrigerated");
        Console.Write("Your option: ");
        string type = Console.ReadLine();

        Console.Write("Tare weight: ");
        double tare = double.Parse(Console.ReadLine());
        Console.Write("Height: ");
        int height = int.Parse(Console.ReadLine());
        Console.Write("Depth: ");
        int depth = int.Parse(Console.ReadLine());
        Console.Write("Max load: ");
        double maxLoad = double.Parse(Console.ReadLine());

        switch (type)
        {
            case "1":
                Console.Write("Is hazardous: ");
                bool hazardous = bool.Parse(Console.ReadLine());
                Containers.Add(new LiquidContainer(tare, maxLoad, height, depth, hazardous));
                Console.WriteLine("Container created");
                break;
            case "2":
                Console.Write("Pressure: ");
                double pressure = double.Parse(Console.ReadLine());
                Containers.Add(new GasContainer(tare, maxLoad, height, depth, pressure));
                Console.WriteLine("Container created");
                break;
            case "3":
                Console.Write("Product type: ");
                string productType = Console.ReadLine();
                Console.Write("Temperature: ");
                double temp = double.Parse(Console.ReadLine());
                try
                {
                    Containers.Add(new RefrigeratedContainer(tare, maxLoad, height, depth, productType, temp));
                    Console.WriteLine("Container created");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Invalid parameter");
                }
                break;
            default:
                Console.WriteLine("Invalid type");
                break;
        }
    }

    static void LoadContainer()
    {
        Console.Write("Enter container serial number: ");
        var cont = Containers.Find(c => c.SerialNumber == Console.ReadLine());
        if (cont == null)
        {
            Console.WriteLine("Container not found");
            return;
        }

        Console.Write("Enter mass to load (kg): ");
        double mass = double.Parse(Console.ReadLine());
        try
        {
            cont.Load(mass);
            Console.WriteLine("Container loaded");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void LoadContainerOnShip()
    {
        Console.Write("Enter ship id: ");
        var ship = Ships.Find(s => s.Id == int.Parse(Console.ReadLine()));
        if (ship == null)
        {
            Console.WriteLine("Ship not found");
            return;
        }

        Console.Write("Enter container serial number: ");
        var cont = Containers.Find(c => c.SerialNumber == Console.ReadLine());
        if (cont == null)
        {
            Console.WriteLine("Container not found");
            return;
        }

        if (ship.AddContainer(cont))
        {
            Containers.Remove(cont);
            Console.WriteLine("Container loaded onto ship");
        }
        else
            Console.Write("Can't load container due to max count or mass overload");
    }

    static void LoadContainerListOnShip()
    {
        Console.Write("Enter ship id: ");
        var ship = Ships.Find(s => s.Id == int.Parse(Console.ReadLine()));
        if (ship == null)
        {
            Console.WriteLine("Ship not found");
            return;
        }

        List<Container> toAdd = new();
        while(true)
        {
            Console.Write("Enter container serial number (0 to Exit): ");
            var serial = Console.ReadLine();
            if (serial == "0")
                break;
            var cont = Containers.Find(c => c.SerialNumber == serial);
            if (cont == null)
            {
                Console.WriteLine("Container not found");
                return;
            }
            toAdd.Add(cont);
        }
        if (ship.AddContainerList(toAdd))
        {
            toAdd.ForEach(c => Containers.Remove(c));
            Console.WriteLine("Containers loaded onto ship");
        }
        else
            Console.WriteLine("Containers coulnd be loaded due to max count or mass overload");
    }

    static void UnloadContainerFromShip()
    {
        Console.Write("Enter ship id: ");
        var ship = Ships.Find(s => s.Id == int.Parse(Console.ReadLine()));
        if (ship == null)
        {
            Console.WriteLine("Ship not found");
            return;
        }
        Console.Write("Enter container serial number: ");
        string serial = Console.ReadLine();

        var cont = ship.UnloadContainer(serial);
        if (cont != null)
        {
            Containers.Add(cont);
            Console.WriteLine("Container unloaded");
        }
    }

    static void UnloadContainer()
    {
        Console.Write("Enter container serial number: ");
        string serial = Console.ReadLine();
        var cont = Containers.Find(c => c.SerialNumber == serial);
        if (cont != null)
        {
            cont.Unload();
            Console.WriteLine("Container unloaded");
        }
        else
        {
            Console.WriteLine("Container not found");
        }
    }

    static void ChangeContainerOnShip()
    {
        Console.Write("Enter ship id: ");
        var ship = Ships.Find(s => s.Id == int.Parse(Console.ReadLine()));
        if (ship == null)
        {
            Console.WriteLine("Ship not found");
            return;
        }

        Console.Write("Enter old container serial number: ");
        string oldSerial = Console.ReadLine();
        Console.Write("Enter new container serial number: ");
        string newSerial = Console.ReadLine();

        var newCont = Containers.Find(c => c.SerialNumber == newSerial);
        if (newCont == null)
        {
            Console.WriteLine("New container not found");
            return;
        }

        var oldCont = ship.ReplaceContainer(oldSerial, newCont);
        if (oldCont != null)
        {
            Containers.Remove(newCont);
            Containers.Add(oldCont);
            Console.WriteLine("Container replaced");
        }
    }

    static void MoveContainerBetweenShips()
    {
        Console.Write("Enter source ship id: ");
        var from = Ships.Find(s => s.Id == int.Parse(Console.ReadLine()));
        Console.Write("Enter destination ship id: ");
        var to = Ships.Find(s => s.Id == int.Parse(Console.ReadLine()));
        Console.Write("Enter container serial number: ");
        string serial = Console.ReadLine();

        if (Ship.TransferContainer(from, to, serial))
            Console.Write("Container transfered between ships");
    }

    static void ShowContainerInfo()
    {
        Console.Write("Enter container serial number: ");
        string serial = Console.ReadLine();
        var container = Containers.Find(c => c.SerialNumber == serial);
        if (container != null)
            Console.WriteLine(container);
        else
            Console.WriteLine("Container not found");
    }

    static void ShowShipInfo()
    {
        Console.Write("Enter ship id: ");
        int id = int.Parse(Console.ReadLine());
        var ship = Ships.Find(s => s.Id == id);
        if (ship != null)
            ship.ShowInfo();
        else
            Console.WriteLine("Ship not found");
    }
}