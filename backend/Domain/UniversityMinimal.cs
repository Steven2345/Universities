namespace backend.Domain
{
    public class UniversityMinimal
    {
        public int Id { get; }
        public string Name { get; }

        public UniversityMinimal(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
