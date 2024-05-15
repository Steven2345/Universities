using System.Data;

namespace backend.Domain
{
    public class University
    {
        public int Id { get; }
        public string Name { get; }
        public string Location { get; }
        public double Score { get; }
        public string Description { get; }
        public string Author { get; }

        public University(int id, string name, string location, double score, string description, string author)
        {
            Id = id;
            Name = name;
            Location = location;
            Score = score;
            Description = description;
            Author = author;
        }

        public University(DataRow row)
        {
            Id = (int)row["uni_id"];
            Name = (string)row["uni_name"];
            Location = (string)row["uni_location"];
            Score = (double)row["uni_score"];
            Description = (string)row["uni_descr"];
            Author = (string)row["user_id"];
        }
    }
}
