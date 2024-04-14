namespace Task4.models
{
    public class Visit
    {
        public Guid IdVisit { get; set; } = Guid.NewGuid();
        public DateTime DateOfVisit { get; set; }
        public Guid AnimalId { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        // id of visists [Guid], datesofvisits [DateTime], AnimalId, Description, Price
    }
}
