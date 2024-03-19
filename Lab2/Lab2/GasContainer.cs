namespace Lab2;

public class GasContainer : Container, IHazardNotifier
{
    private float pressure { get; set; }
    
    public GasContainer(string serialNumber, double cargoMass, double height, double tareWeight, double depth, double maxPayload, float pressure) : base(GenerateSerialNumber("G"), cargoMass, height, tareWeight, depth, maxPayload)
    {
        this.pressure = pressure;
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
}