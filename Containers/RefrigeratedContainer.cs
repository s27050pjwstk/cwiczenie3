using Project.Exceptions;

namespace Project.Containers
{
    public class RefrigeratedContainer : Container
    {
        private static readonly Dictionary<string, double> ProductsRequirements = new()
        {
            { "Bananas", 13.3 },
            { "Chocolate", 18.0 },
            { "Fish", 2.0 },
            { "Meat", -15.0 },
            { "Ice Cream", -18.0 },
            { "Frozen pizza", -30.0 },
            { "Cheese", 7.2 },
            { "Sausages", 5.0 },
            { "Butter", 20.5 },
            { "Eggs", 19.0 }
        };

        public string ProductType { get; }
        public double Temperature { get; }

        public RefrigeratedContainer(double tareWeight, double maxLoad, int height, int depth, string productType, double temperature)
            : base("C", tareWeight, maxLoad, height, depth)
        {
            if (!ProductsRequirements.ContainsKey(productType))
                throw new ArgumentException($"Unknown product: {productType}");
            if (temperature < ProductsRequirements[productType])
                throw new ArgumentException($"Too low temperature - min. {ProductsRequirements[productType]}C");

            ProductType = productType;
            Temperature = temperature;
        }

        public override void Load(double mass)
        {
            if (mass > MaxLoad)
                throw new OverfillException($"Load limit for container [{SerialNumber}] is {MaxLoad}kg - tried to load {mass}kg");
            LoadMass = mass;
        }

        public override void Unload()
        {
            LoadMass = 0;
        }
    }
}
