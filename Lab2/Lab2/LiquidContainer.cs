namespace Lab2;

public class LiquidContainer : Container, IHazardNotifier
{
    private bool isHazardous { get; }
    
    
    public LiquidContainer(double cargoMass, double height, double tareWeight, double depth, double maxPayload, bool isHazardous) : base(GenerateSerialNumber("L"), cargoMass, height, tareWeight, depth, maxPayload)
    {
        this.isHazardous = isHazardous;
    }

    public override void LoadCargo(double mass)
    {
        NotifyHazard();
        if (isHazardous)
        {
            if (cargoMass + mass > maxPayload * 0.5)
            {
                Console.WriteLine("Hazardous cargo, we cannot fill it more than 50%");
            }
            else
            {
                cargoMass += mass;
            }
        }
        else
        {
            if (cargoMass + mass > maxPayload * 0.9)
            {
                Console.WriteLine("Not hazardous cargo, we cannot fill it more than 90%");
            }
            else
            {
                cargoMass += mass;
            }
        }
    }

    public override void EmptyCargo()
    {
        NotifyHazard();
        cargoMass = 0;
    }


    public void NotifyHazard()
    {
        Console.WriteLine($"HAZARD! {serialNumber}");
    }
}