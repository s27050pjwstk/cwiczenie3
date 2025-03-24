using Project.Containers;

namespace Project
{
    public class Ship
    {
        private static int counter = 1;

        private List<Container> Containers = new();

        public int Id { get; }
        public double MaxSpeed { get; }
        public int MaxContainerCount { get; }
        public double MaxTotalWeight { get; }

        public Ship(double maxSpeed, int maxContainerCount, double maxTotalWeight)
        {
            Id = counter++;
            MaxSpeed = maxSpeed;
            MaxContainerCount = maxContainerCount;
            MaxTotalWeight = maxTotalWeight;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Ship {Id} (speed={MaxSpeed}, maxContainerCount={MaxContainerCount}, maxTotalWeight={MaxTotalWeight})");
            Console.WriteLine("Containers:");
            foreach (var c in Containers)
                Console.WriteLine($"\t{c}");
        }

        public bool AddContainer(Container c)
        {
            if (Containers.Count + 1 > MaxContainerCount || Containers.Sum(c => c.TareWeight + c.LoadMass) + c.TareWeight + c.LoadMass > MaxTotalWeight * 1000)
                return false;
            Containers.Add(c);
            return true;
        }

        public bool AddContainerList(List<Container> c)
        {
            if (Containers.Count + c.Count > MaxContainerCount || Containers.Sum(c => c.TareWeight + c.LoadMass) + c.Sum(c => c.TareWeight + c.LoadMass) > MaxTotalWeight * 1000)
                return false;
            Containers.AddRange(c);
            return true;
        }

        private void RemoveContainer(Container c)
        {
            Containers.Remove(c);
        }

        public Container? UnloadContainer(string serial)
        {
            var container = Containers.Find(c => c.SerialNumber == serial);
            if (container == null)
            {
                Console.WriteLine("Container not found");
                return null;
            }
            Containers.Remove(container);
            return container;
        }

        public Container? ReplaceContainer(string serial, Container c)
        {
            var container = Containers.Find(c => c.SerialNumber == serial);
            if (container == null)
            {
                Console.Write("Container not found");
                return null;
            }
            if (!AddContainer(c))
            {
                Console.Write("Can't replace container due to max count or mass overload");
                return null;
            }
            RemoveContainer(container);
            return container;
        }

        public static bool TransferContainer(Ship from, Ship to, string serial)
        {
            var container = from.Containers.Find(c => c.SerialNumber == serial);
            if (container == null)
            {
                Console.Write("Container not found");
                return false;
            }
            if (!to.AddContainer(container))
            {
                Console.Write("Can't transfer containers due to max count or mass overload");
                return false;
            }
            from.RemoveContainer(container);
            return true;
        }

        public override string ToString()
        {
            return $"Ship {Id} (speed={MaxSpeed}, maxContainerCount={MaxContainerCount}, maxTotalWeight={MaxTotalWeight})";
        }
    }
}
