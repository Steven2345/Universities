using backend.Domain;
using backend.Repository;
using Bogus;

namespace backend
{
    public static class Generator
    {
        public static void populateUniversities()
        {
            UniversityRepository repo = new UniversityRepository();

            var faker = new Faker("en_US");
            for (int i = 0; i < 10000; i++)
            {
                string city = faker.Address.City();
                var u = new University(
                    1,
                    city + " University",
                    city + ", " + faker.Address.State(),
                    faker.Random.Double() * 100,
                    faker.Lorem.Sentences(),
                    "405a73b0-38b3-444c-8a31-3fb58728ed48"
                );
                repo.AddUniversity( u );
            }
        }

        public static void populateFaculties()
        {
            FacultyRepository repo = new FacultyRepository();
            UniversityRepository repo2 = new UniversityRepository();

            static string CapitalizeFirstLetter(string s)
            {
                if (s.Length < 4)
                    return s;
                return s.Remove(1).ToUpper() + s.Substring(1);
            }

            var faker = new Faker("en_US");
            for (int i = 0; i < 5000; i++)
            {
                var f = new Faculty(
                    1,
                    "Faculty of " + 
                        CapitalizeFirstLetter(faker.Lorem.Word()) + " " +
                        CapitalizeFirstLetter(faker.Lorem.Word()) + " " +
                        CapitalizeFirstLetter(faker.Lorem.Word()),
                    faker.Random.Byte() * 10,
                    faker.Random.ListItem<UniversityMinimal>(repo2.GetUniversityNames()).Id,
                    "405a73b0-38b3-444c-8a31-3fb58728ed48"
                );
                repo.AddFaculty(f);
            }
        }
    }
}
