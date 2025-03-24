using Project.Exceptions;
using Project.Interfaces;

namespace Project.Containers
{
    public class GasContainer : Container, IHazardNotifier
    {
        public double Pressure { get; }

        public GasContainer(double tareWeight, double maxLoad, int height, int depth, double pressure)
            : base("G", tareWeight, maxLoad, height, depth)
        {
            Pressure = pressure;
        }

        public override void Load(double mass)
        {
            if (mass > MaxLoad)
            {
                NotifyHazard("container overloaded");
                throw new OverfillException($"Load limit for container [{SerialNumber}] is {MaxLoad}kg - tried to load {mass}kg");
            }
            LoadMass = mass;
        }

        public override void Unload()
        {
            LoadMass *= 0.05;
        }

        public void NotifyHazard(string message)
        {
            Console.WriteLine($"Detected hazard in container [{SerialNumber}]: {message}");
        }
    }
}
