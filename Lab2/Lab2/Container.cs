namespace Lab2;

public abstract class Container
{
    private static int id = 0;
    public string serialNumber { get; }
    public double cargoMass { get; protected set; }
    public double height { get; }
    public double tareWeight { get; }
    public double depth { get; }
    public double maxPayload { get; }
    

    protected Container(string serialNumber, double cargoMass, double height, double tareWeight, double depth, double maxPayload)
    {
        this.serialNumber = serialNumber;
        this.cargoMass = cargoMass;
        this.height = height;
        this.tareWeight = tareWeight;
        this.depth = depth;
        this.maxPayload = maxPayload;
    }

    public abstract void LoadCargo(double mass);

    public abstract void EmptyCargo();

    protected static string GenerateSerialNumber(string type) {
        return $"KON-{type}-{id++}";
    }

    public override string ToString()
    {
        return "serialNumber = " + serialNumber + ", " +  ", cargoMass = " + cargoMass + ", height = " + height + ", tareWeight = " + tareWeight + ", depth = " + depth + ", maxPayload = " + maxPayload;
    }
}