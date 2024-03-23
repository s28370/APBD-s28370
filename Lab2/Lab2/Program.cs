using Lab2;

Container container1 = new GasContainer(10, 5, 1.5, 4, 20, 4.5);
Container container2 = new LiquidContainer(5.6, 3, 0.5, 3, 25, true);
Container container3 = new RefrigeratedContainer(4.5, 4, 2, 5, 8, "Bananas", 13.3);

ContainerShip containerShip1 = new ContainerShip(25, 2, 15);
ContainerShip containerShip2 = new ContainerShip(20, 3, 50);

container1.LoadCargo(15);
container1.LoadCargo(5);
Console.WriteLine(container1);

container1.EmptyCargo();
container1.LoadCargo(10);

container2.LoadCargo(10);

containerShip1.AddContainer(container1);
containerShip1.AddContainer(container2);
containerShip1.AddContainer(container3);

List<Container> containers = [container1, container2, container3];
containerShip2.AddContainerList(containers);

containerShip1.TransferContainer(containerShip2, container1);

containerShip2.ReplaceContainer(containerShip2.Containers[0].serialNumber, container3);

Console.WriteLine(containerShip1);
Console.WriteLine(containerShip2);