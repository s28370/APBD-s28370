namespace Lab2;

public class RefrigeratedContainer : Container
{
    private string ProductType { get; set; }
    private double Temperature { get; set; }
    
    public RefrigeratedContainer(double cargoMass, double height, double tareWeight, double depth, double maxPayload, string productType, double temperature) : base(GenerateSerialNumber("R"), cargoMass, height, tareWeight, depth, maxPayload)
    {
        ProductType = productType;
        Temperature = temperature;
    }

    public override void EmptyCargo()
    {
        cargoMass=0;
    }

    public override void LoadCargo(double mass)
    {
        if (cargoMass + mass > maxPayload)
        {
            Console.WriteLine("Max payload exceeded");
        }
        else
        {
            Console.WriteLine("Loading succeeded");
            cargoMass += mass;
        }
    }

    public override string ToString()
    {
        return base.ToString() + ", ProductType = " + ProductType + ", Temperature = " + Temperature;
    }
}