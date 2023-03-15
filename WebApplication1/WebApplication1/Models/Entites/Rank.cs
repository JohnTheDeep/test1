namespace WebApplication1.Models.Entites
{
    public class Rank
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public Rank() { }
        public Rank(string name)
        {
            Name = name;
        }
    }
}
