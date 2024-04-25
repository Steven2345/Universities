﻿using backend.Domain;
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

        public Faculty getById(int id)
        {
            Faculty? ret = facultyRepo.SearchFaculty(id);
            if (ret == null)
                return new Faculty(0, "", -1, 0);
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

        public List<Faculty> GetBatch(int start, int count) 
        {
            return facultyRepo.GetBatch(start, count);
        }
    }
}
