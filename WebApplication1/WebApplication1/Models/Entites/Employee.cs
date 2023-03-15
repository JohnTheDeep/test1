namespace WebApplication1.Models.Entites
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public Rank Rank { get; set; }
        public Employee() { }
        public Employee(string fullName, Rank rank)
        {
            FullName = fullName;
            Rank = rank;
        }
    }
}
