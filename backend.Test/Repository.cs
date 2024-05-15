using backend.Domain;
using backend.Repository;

namespace backend.Test
{
    public class TestUniversityRepository
    {
        private UniversityRepository repo;
        private int firstID, lastID;

        [SetUp]
        public void Setup()
        {
            repo = new UniversityRepository("Data source=localhost,1235;Initial Catalog=UnivDB;User Id=universityuser;Password=root;Encrypt=False");
        }

        [Test]
        public void AddToRepo_SuccessiveAdds_ReturnSuccessiveIDs()
        {
            University u1 = new University(1, "asdf", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");
            University u2 = new University(1, "qwer", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");
            University u3 = new University(1, "zxcv", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");

            int id1 = firstID = repo.AddUniversity(u1);
            int id2 = repo.AddUniversity(u2);
            int id3 = repo.AddUniversity(u3);
            
            Assert.IsTrue(id1 == id2 - 1);
            Assert.IsTrue(id2 == id3 - 1);
        }

        [Test]
        public void SearchInRepo()
        {
            University u1 = new University(1, "asdf", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");
            int id = repo.AddUniversity(u1);

            UniversityExtended? u = repo.SearchUniversity(id);

            Assert.IsNotNull(u);
            Assert.That(u.Name == "asdf", Is.True);
        }

        [Test]
        public void UpdateRepo()
        {
            University uu = new University(1, "qwer", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");
            int id2 = lastID = repo.AddUniversity(uu);
            UniversityExtended? u1 = repo.SearchUniversity(id2);

            Assert.IsNotNull(u1);
            University u2 = new University(u1.Id, "ASDF", u1.Location, u1.Score, u1.Description, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            repo.UpdateUniversity(u2);
            Assert.That(u1.Name == "qwer");

            UniversityExtended? u11 = repo.SearchUniversity(id2);
            Assert.IsNotNull(u11);
            Assert.That(u11.Name == "ASDF", Is.True);
        }

        [Test]
        public void DeleteFromRepo()
        {
            University u1 = new University(1, "asdf", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");
            University u2 = new University(1, "qwer", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");
            University u3 = new University(1, "zxcv", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");

            int id1 = repo.AddUniversity(u1);
            int id2 = repo.AddUniversity(u2);
            int id3 = repo.AddUniversity(u3);

            Assert.That(repo.DeleteUniversity(id1), Is.EqualTo(1));
            Assert.That(repo.DeleteUniversity(id2), Is.EqualTo(1));
            Assert.That(repo.DeleteUniversity(id3), Is.EqualTo(1));
            Assert.That(repo.DeleteUniversity(id1), Is.EqualTo(0));
            Assert.That(repo.DeleteUniversity(id2), Is.EqualTo(0));
            Assert.That(repo.DeleteUniversity(id3), Is.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {
            for(int id = firstID; id <= lastID; id++)
                repo.DeleteUniversity(id);
        }
    }

    public class TestFacultyRepository
    {
        private FacultyRepository repo;
        private int firstID, lastID;

        [SetUp]
        public void Setup()
        {
            repo = new FacultyRepository("Data source=localhost,1235;Initial Catalog=UnivDB;User Id=universityuser;Password=root;Encrypt=False");
        }

        [Test]
        public void AddToRepo_SuccessiveAdds_ReturnSuccessiveIDs()
        {
            Faculty u1 = new Faculty(1, "asdf", 500, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            Faculty u2 = new Faculty(1, "qwer", 600, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            Faculty u3 = new Faculty(1, "zxcv", 900, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");

            int id1 = firstID = repo.AddFaculty(u1);
            int id2 = repo.AddFaculty(u2);
            int id3 = repo.AddFaculty(u3);
            
            Assert.That(id2 - id1, Is.EqualTo(1));
            Assert.IsTrue(id2 == id3 - 1);
        }

        [Test]
        public void SearchInRepo()
        {
            Faculty u1 = new Faculty(1, "asdf", 500, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            int id = repo.AddFaculty(u1);

            FacultyExtended? u = repo.SearchFaculty(id);

            Assert.IsNotNull(u);
            Assert.That(u.Name == "asdf", Is.True);
        }

        [Test]
        public void UpdateRepo()
        {
            Faculty uu = new Faculty(1, "qwer", 600, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            int id2 = lastID = repo.AddFaculty(uu);
            FacultyExtended? u1 = repo.SearchFaculty(id2);

            Assert.IsNotNull(u1);
            Faculty u2 = new Faculty(u1.Id, "ASDF", u1.NoOfStudents, u1.UniversityID, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            repo.UpdateFaculty(u2);
            Assert.That(u1.Name == "qwer");

            FacultyExtended? u11 = repo.SearchFaculty(id2);
            Assert.IsNotNull(u11);
            Assert.That(u11.Name == "ASDF", Is.True);
        }

        [Test]
        public void DeleteFromRepo()
        {
            Faculty u1 = new Faculty(1, "asdf", 500, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            Faculty u2 = new Faculty(1, "qwer", 600, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            Faculty u3 = new Faculty(1, "zxcv", 900, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");

            int id1 = repo.AddFaculty(u1);
            int id2 = repo.AddFaculty(u2);
            int id3 = repo.AddFaculty(u3);

            Assert.That(repo.DeleteFaculty(id1), Is.EqualTo(1));
            Assert.That(repo.DeleteFaculty(id2), Is.EqualTo(1));
            Assert.That(repo.DeleteFaculty(id3), Is.EqualTo(1));
            Assert.That(repo.DeleteFaculty(id1), Is.EqualTo(0));
            Assert.That(repo.DeleteFaculty(id2), Is.EqualTo(0));
            Assert.That(repo.DeleteFaculty(id3), Is.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {
            for(int id = firstID; id <= lastID; id++)
                repo.DeleteFaculty(id);
        }
    }
}