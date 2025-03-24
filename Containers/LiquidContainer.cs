using Project.Exceptions;
using Project.Interfaces;

namespace Project.Containers
{
    public class LiquidContainer : Container, IHazardNotifier
    {
        public bool IsHazard { get; }

        public LiquidContainer(double tareWeight, double maxLoad, int height, int depth, bool isHazard)
            : base("L", tareWeight, maxLoad, height, depth)
        {
            IsHazard = isHazard;
        }

        public override void Load(double mass)
        {
            double massLimit = MaxLoad * (IsHazard ? 0.5 : 0.9);
            if (mass > massLimit)
            {
                NotifyHazard("container overloaded");
                throw new OverfillException($"Load limit for container [{SerialNumber}] is {massLimit}kg - tried to load {mass}kg");
            }
            LoadMass = mass;
        }

        public override void Unload()
        {
            LoadMass = 0;
        }

        public void NotifyHazard(string message)
        {
            Console.WriteLine($"Detected hazard in container [{SerialNumber}]: {message}");
        }
    }
}
