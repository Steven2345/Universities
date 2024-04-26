using backend.Domain;
using backend.Repository;

namespace backend.Service
{
    public class FacultyService
    {
        private FacultyRepository facultyRepo;

        public FacultyService()
        {
            facultyRepo = new FacultyRepository();
        }

        public int AddFaculty(Faculty faculty)
        {
            return facultyRepo.AddFaculty(faculty);
        }

        public FacultyExtended getById(int id)
        {
            FacultyExtended? ret = facultyRepo.SearchFaculty(id);
            if (ret == null)
                return new FacultyExtended(0, "", -2, 0, "");
            return ret;
        }

        public int UpdateFaculty(Faculty faculty)
        {
            return facultyRepo.UpdateFaculty(faculty);
        }

        public int DeleteFaculty(int id)
        {
            return facultyRepo.DeleteFaculty(id);
        }

        public int GetSize()
        {
            return facultyRepo.GetSizeOfRepo();
        }

        public List<FacultyExtended> GetBatch(int start, int count) 
        {
            return facultyRepo.GetBatch(start, count);
        }
    }
}
