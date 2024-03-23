namespace Lab2;

public class GasContainer : Container, IHazardNotifier
{
    private double Pressure { get; set; }
    
    public GasContainer(double cargoMass, double height, double tareWeight, double depth, double maxPayload, double pressure) : base(GenerateSerialNumber("G"), cargoMass, height, tareWeight, depth, maxPayload)
    {
        this.Pressure = pressure;
    }

    public override void LoadCargo(double mass)
    {
        NotifyHazard();
        
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

    public override void EmptyCargo()
    {
        NotifyHazard();

        cargoMass *= 0.05;
    }

    public void NotifyHazard()
    {
        Console.WriteLine($"HAZARD! {serialNumber}");
    }

    public override string ToString()
    {
        return base.ToString() + ", pressure = " + Pressure;
    }
}