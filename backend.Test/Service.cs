using backend.Domain;
using backend.Service;

namespace backend.Test
{
    public class TestUniversityService
    {
        private UniversityService serv;
        private List<int> ids;

        [SetUp]
        public void Setup()
        {
            serv = new UniversityService(new Repository.UniversityRepository("Data source=localhost,1235;Initial Catalog=UnivDB;User Id=universityuser;Password=root;Encrypt=False"));
            ids = [];
        }

        [Test]
        public void AddToService_SuccessiveAdds_ReturnSuccessiveIDs()
        {
            University u1 = new University(1, "asdf", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");
            University u2 = new University(1, "qwer", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");
            University u3 = new University(1, "zxcv", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");

            int id1 = serv.AddUniversity(u1);
            int id2 = serv.AddUniversity(u2);
            int id3 = serv.AddUniversity(u3);
            ids.Add(id1);
            ids.Add(id2);
            ids.Add(id3);

            Assert.IsTrue(id1 == id2 - 1);
            Assert.IsTrue(id2 == id3 - 1);
        }

        [Test]
        public void SearchInService()
        {
            University u1 = new University(1, "asdf", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");
            int id = serv.AddUniversity(u1);
            ids.Add(id);

            UniversityExtended u = serv.getById(id);

            Assert.That(u.Name, Is.Not.EqualTo(""));
            Assert.That(u.Name == "asdf", Is.True);
        }

        [Test]
        public void UpdateService()
        {
            University uu = new University(1, "qwer", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");
            int id2 = serv.AddUniversity(uu);
            ids.Add(id2);
            UniversityExtended u1 = serv.getById(id2);

            Assert.That(u1.Name, Is.Not.EqualTo(""));
            University u2 = new University(u1.Id, "ASDF", u1.Location, u1.Score, u1.Description, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            serv.UpdateUniversity(u2);
            Assert.That(u1.Name == "qwer");

            UniversityExtended u11 = serv.getById(id2);
            Assert.That(u11.Name, Is.Not.EqualTo(""));
            Assert.That(u11.Name == "ASDF", Is.True);
        }

        [Test]
        public void DeleteFromService()
        {
            University u1 = new University(1, "asdf", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");
            University u2 = new University(1, "qwer", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");
            University u3 = new University(1, "zxcv", "asdf", 34, "asdf", "405a73b0-38b3-444c-8a31-3fb58728ed48");

            int id1 = serv.AddUniversity(u1);
            int id2 = serv.AddUniversity(u2);
            int id3 = serv.AddUniversity(u3);

            Assert.That(serv.DeleteUniversity(id1, "405a73b0-38b3-444c-8a31-3fb58728ed48"), Is.EqualTo(1));
            Assert.That(serv.DeleteUniversity(id2, "405a73b0-38b3-444c-8a31-3fb58728ed48"), Is.EqualTo(1));
            Assert.That(serv.DeleteUniversity(id3, "405a73b0-38b3-444c-8a31-3fb58728ed48"), Is.EqualTo(1));
            Assert.That(serv.DeleteUniversity(id1, "405a73b0-38b3-444c-8a31-3fb58728ed48"), Is.EqualTo(0));
            Assert.That(serv.DeleteUniversity(id2, "405a73b0-38b3-444c-8a31-3fb58728ed48"), Is.EqualTo(0));
            Assert.That(serv.DeleteUniversity(id3, "405a73b0-38b3-444c-8a31-3fb58728ed48"), Is.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {
            foreach (int id in ids)
                serv.DeleteUniversity(id, "405a73b0-38b3-444c-8a31-3fb58728ed48");
        }
    }

    public class TestFacultyService
    {
        private FacultyService serv;
        private int firstID, lastID;

        [SetUp]
        public void Setup()
        {
            serv = new FacultyService(new Repository.FacultyRepository("Data source=localhost,1235;Initial Catalog=UnivDB;User Id=universityuser;Password=root;Encrypt=False"));
        }

        [Test]
        public void AddToService_SuccessiveAdds_ReturnSuccessiveIDs()
        {
            Faculty u1 = new Faculty(1, "asdf", 500, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            Faculty u2 = new Faculty(1, "qwer", 600, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            Faculty u3 = new Faculty(1, "zxcv", 900, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");

            int id1 = firstID = serv.AddFaculty(u1);
            int id2 = serv.AddFaculty(u2);
            int id3 = serv.AddFaculty(u3);

            Assert.That(id2 - id1, Is.EqualTo(1));
            Assert.IsTrue(id2 == id3 - 1);
        }

        [Test]
        public void SearchInService()
        {
            Faculty u1 = new Faculty(1, "asdf", 500, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            int id = serv.AddFaculty(u1);

            FacultyExtended u = serv.getById(id);

            Assert.That(u.Name, Is.Not.EqualTo(""));
            Assert.That(u.Name == "asdf", Is.True);
        }

        [Test]
        public void UpdateService()
        {
            Faculty uu = new Faculty(1, "qwer", 600, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            int id2 = lastID = serv.AddFaculty(uu);
            FacultyExtended u1 = serv.getById(id2);

            Assert.That(u1.Name, Is.Not.EqualTo(""));
            Faculty u2 = new Faculty(u1.Id, "ASDF", u1.NoOfStudents, u1.UniversityID, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            serv.UpdateFaculty(u2);
            Assert.That(u1.Name == "qwer");

            FacultyExtended u11 = serv.getById(id2);
            Assert.That(u11.Name, Is.Not.EqualTo(""));
            Assert.That(u11.Name == "ASDF", Is.True);
        }

        [Test]
        public void DeleteFromRepo()
        {
            Faculty u1 = new Faculty(1, "asdf", 500, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            Faculty u2 = new Faculty(1, "qwer", 600, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");
            Faculty u3 = new Faculty(1, "zxcv", 900, 2, "405a73b0-38b3-444c-8a31-3fb58728ed48");

            int id1 = serv.AddFaculty(u1);
            int id2 = serv.AddFaculty(u2);
            int id3 = serv.AddFaculty(u3);

            Assert.That(serv.DeleteFaculty(id1, "405a73b0-38b3-444c-8a31-3fb58728ed48"), Is.EqualTo(1));
            Assert.That(serv.DeleteFaculty(id2, "405a73b0-38b3-444c-8a31-3fb58728ed48"), Is.EqualTo(1));
            Assert.That(serv.DeleteFaculty(id3, "405a73b0-38b3-444c-8a31-3fb58728ed48"), Is.EqualTo(1));
            Assert.That(serv.DeleteFaculty(id1, "405a73b0-38b3-444c-8a31-3fb58728ed48"), Is.EqualTo(0));
            Assert.That(serv.DeleteFaculty(id2, "405a73b0-38b3-444c-8a31-3fb58728ed48"), Is.EqualTo(0));
            Assert.That(serv.DeleteFaculty(id3, "405a73b0-38b3-444c-8a31-3fb58728ed48"), Is.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {
            for (int id = firstID; id <= lastID; id++)
                serv.DeleteFaculty(id, "405a73b0-38b3-444c-8a31-3fb58728ed48");
        }
    }
}