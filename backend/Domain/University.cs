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

        public University(int id, string name, string location, double score, string description)
        {
            Id = id;
            Name = name;
            Location = location;
            Score = score;
            Description = description;
        }

        public University(DataRow row)
        {
            Id = (int)row["uni_id"];
            Name = (string)row["uni_name"];
            Location = (string)row["uni_location"];
            Score = (double)row["uni_score"];
            Description = (string)row["uni_descr"];
        }
    }
}
