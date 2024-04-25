namespace backend.Service
{
    using backend.Domain;
    using backend.Repository;

    // -2 -> error in repo
    // -1 -> error in service
    // 0 -> no rows affected
    // 1 -> ok
    // >1 -> problem
    public class UniversityService
    {
        private UniversityRepository uniRepo;

        public UniversityService()
        {
            uniRepo = new UniversityRepository();
        }

        public int AddUniversity(University university)
        {
            if (university.Score >= 0 && university.Score <= 100)
                return uniRepo.AddUniversity(university);
            return -1;
        }

        public University getById(int id) 
        {
            University? ret = uniRepo.SearchUniversity(id);
            if (ret == null)
                return new University(0, "", "", -2, "");
            return ret;
        }

        public int UpdateUniversity(University university)
        {
            if (university.Score >= 0 && university.Score <= 100)
                return uniRepo.UpdateUniversity(university);
            return -1;
        }

        public int DeleteUniversity(int id)
        {
            return uniRepo.DeleteUniversity(id);
        }

        public int GetSize()
        {
            return uniRepo.GetSizeOfRepo();
        }

        public List<University> GetBatch(int start, int count)
        {
            return uniRepo.GetBatch(start, count);
        }
    }
}
