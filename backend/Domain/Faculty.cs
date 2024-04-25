﻿using static System.Formats.Asn1.AsnWriter;
using System.Data;

namespace backend.Domain
{
    public class Faculty
    {
        public int Id { get; }
        public string Name { get; }
        public int NoOfStudents { get; }
        public int University { get; }

        public Faculty(int id, string name, int noOfStudents, int university)
        {
            Id = id;
            Name = name;
            NoOfStudents = noOfStudents;
            University = university;
        }

        public Faculty(DataRow row)
        {
            Id = (int)row["facult_id"];
            Name = (string)row["facult_name"];
            NoOfStudents = (int)row["facult_nostud"];
            University = (int)row["uni_id"];
        }
    }
}
