namespace Project.Containers
{
    public abstract class Container
    {
        private static int counter = 1;

        public double TareWeight { get; }
        public double LoadMass { get; protected set; }
        public double MaxLoad { get; }
        public string SerialNumber { get; }
        public double Height { get; }
        public double Depth { get; }

        public Container(string type, double tareWeight, double maxLoad, double height, double depth)
        {
            SerialNumber = $"KON-{type}-{counter++}";
            TareWeight = tareWeight;
            MaxLoad = maxLoad;
            Height = height;
            Depth = depth;
        }

        public abstract void Load(double mass);
        public abstract void Unload();

        public override string ToString()
        {
            return $"Container {SerialNumber} (tareWeight={TareWeight}, loadMass={LoadMass}, maxLoad={MaxLoad}, height={Height}, depth={Depth})";
        }
    }
}
