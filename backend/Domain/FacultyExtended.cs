namespace backend.Domain
{
    public class FacultyExtended
    {
        public int Id { get; }
        public string Name { get; }
        public int NoOfStudents { get; }
        public int UniversityID { get; }
        public string University { get; }
        public string Author { get; }

        public FacultyExtended(int id, string name, int noOfStudents, int universityID, string university, string author)
        {
            Id = id;
            Name = name;
            NoOfStudents = noOfStudents;
            UniversityID = universityID;
            University = university;
            Author = author;
        }
    }
}
