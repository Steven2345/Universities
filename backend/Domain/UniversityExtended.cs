namespace backend.Domain
{
    public class UniversityExtended
    {
        public int Id { get; }
        public string Name { get; }
        public string Location { get; }
        public int NoOfFaculties { get; }
        public double Score { get; }
        public string Description { get; }
        public string Author { get; }

        public UniversityExtended(int id, string name, string location, int noOfFaculties, double score, string description, string author)
        {
            Id = id;
            Name = name;
            Location = location;
            NoOfFaculties = noOfFaculties;
            Score = score;
            Description = description;
            Author = author;
        }
    }
}
