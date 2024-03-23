namespace Lab2;

public class ContainerShip
{
    public List<Container> Containers { get; set; } = [];
    public int MaxSpeed { get; }
    public int MaxContainerNumber { get; }
    public double MaxWeight { get; }
    
    public ContainerShip(int maxSpeed, int maxContainerNumber, double maxWeight)
    {
        MaxSpeed = maxSpeed;
        MaxContainerNumber = maxContainerNumber;
        MaxWeight = maxWeight;
    }
    
    public void AddContainer(Container newContainer)
    {
        double currentWeight = 0;
        foreach (var container in Containers)
        {
            currentWeight += container.cargoMass + container.tareWeight;
        }

        if (currentWeight + newContainer.cargoMass + newContainer.tareWeight > MaxWeight)
        {
            Console.WriteLine("Max ship payload exceeded");
            return;
        }

        if (Containers.Count + 1 > MaxContainerNumber)
        {
            Console.WriteLine("Max container number exceeded");
            return;
        }

        Console.WriteLine("Added successfully");
        Containers.Add(newContainer);
    }
    
    public void AddContainerList(List<Container> newContainers)
    {
        double currentWeight = 0;
        foreach (var container in Containers)
        {
            currentWeight += container.cargoMass + container.tareWeight;
        }
        
        double newWeight = 0;
        foreach (var container in newContainers)
        {
            newWeight += container.cargoMass + container.tareWeight;
        }

        if (currentWeight + newWeight > MaxWeight)
        {
            Console.WriteLine("Max ship payload exceeded");
            return;
        }
        
        if (Containers.Count + 1 > MaxContainerNumber)
        {
            Console.WriteLine("Max container number exceeded");
            return;
        }
        
        Console.WriteLine("Added successfully");
        Containers.AddRange(newContainers);
    }

    public void RemoveContainer(Container container)
    {
            Containers.Remove(container);
    }

    public void ReplaceContainer(String serialNumber, Container container)
    {
        foreach (var varContainer in Containers)
        {
            if (varContainer.serialNumber.Equals(serialNumber))
            {
                Containers.Remove(varContainer);
                Containers.Add(container);
                Console.WriteLine("Replace successfully");
                break;
            }
        }
    }

    public void TransferContainer(ContainerShip containerShip, Container container)
    {
        if (!Containers.Remove(container))
        {
            Console.WriteLine("There is no such container in this ship");
            return;
        }
        
        containerShip.AddContainer(container);
    }

    public String PrintContainers()
    {
        String res = "";

        foreach (var container in Containers)
        {
            res += container + "\n";
        }

        return res;
    }

    public override string ToString()
    {
        return "MaxSpeed = " + MaxSpeed + ", MaxWeight = " + MaxWeight + ", MaxContainerNumber = " + MaxContainerNumber + ", " + PrintContainers();
    }
}