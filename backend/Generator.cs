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
                    faker.Lorem.Sentences()
                );
                repo.AddUniversity( u );
            }
        }

        public static void populateFaculties()
        {
            FacultyRepository repo = new FacultyRepository();
            UniversityRepository repo2 = new UniversityRepository();

            var faker = new Faker("en_US");
            for (int i = 0; i < 10000; i++)
            {
                string city = faker.Address.City();
                var f = new Faculty(
                    1,
                    "Faculty of " + faker.Lorem.Words(),
                    faker.Random.Byte() * 10,
                    faker.Random.ListItem<UniversityMinimal>(repo2.GetUniversityNames()).Id
                );
                repo.AddFaculty(f);
            }
        }
    }
}
