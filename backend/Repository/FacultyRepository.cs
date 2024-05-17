namespace backend.Repository
{
    using backend.Domain;
    using Microsoft.Data.SqlClient;

    public class FacultyRepository
    {
        private readonly string connectionString;

        public FacultyRepository(string? connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            this.connectionString = connectionString;
        }
        
        private string getConnectionString()
        {
            return connectionString;
        }

        public int AddFaculty(Faculty faculty)
        {
            using SqlConnection conn = new SqlConnection(getConnectionString());
            const string query = "INSERT INTO Faculties(facult_name, facult_nostud, uni_id, [user_id]) " +
                                 "VALUES(@name, @nostud, @uniid, @userid);" +
                                 "SELECT SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", faculty.Name);
            cmd.Parameters.AddWithValue("@nostud", faculty.NoOfStudents);
            cmd.Parameters.AddWithValue("@uniid", faculty.UniversityID);
            cmd.Parameters.AddWithValue("@userid", faculty.Author);
            int index;

            try
            {
                conn.Open();
                index = decimal.ToInt32((decimal)cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                index = -2;
            }

            return index;
        }

        public FacultyExtended? SearchFaculty(int id)
        {
            List<FacultyExtended?> ret = [];
            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                const string query = "SELECT t.*, U.uni_name " +
                                     "FROM Universities U INNER JOIN (SELECT * " + 
                                                                     "FROM Faculties " + 
                                                                     "WHERE facult_id=@id) t ON U.uni_id = t.uni_id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new FacultyExtended((int)reader[0],
                                                    (string)reader[1],
                                                    (int)reader[2],
                                                    (int)reader[3],
                                                    (string)reader[5],
                                                    (string)reader[4]));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ret.Add(null);
                }
            }

            if (ret.Count > 0)
                return ret[0];
            return null;
        }

        public int UpdateFaculty(Faculty faculty)
        {
            using SqlConnection conn = new SqlConnection(getConnectionString());
            const string query = "UPDATE Faculties SET " +
                                 "facult_name=@name, facult_nostud=@nostud, uni_id=@uniid " +
                                 "WHERE facult_id=@id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", faculty.Id);
            cmd.Parameters.AddWithValue("@name", faculty.Name);
            cmd.Parameters.AddWithValue("@nostud", faculty.NoOfStudents);
            cmd.Parameters.AddWithValue("@uniid", faculty.UniversityID);
            int rows;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                rows = -2;
            }

            return rows;
        }

        public int DeleteFaculty(int id)
        {
            using SqlConnection conn = new SqlConnection(getConnectionString());
            const string query = "DELETE FROM Faculties WHERE facult_id=@id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            int rows;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                rows = -2;
            }

            return rows;
        }

        public int GetSizeOfRepo()
        {
            using SqlConnection conn = new SqlConnection(getConnectionString());
            const string query = "SELECT COUNT(*) FROM Faculties";
            SqlCommand cmd = new SqlCommand(query, conn);
            int count;

            try
            {
                conn.Open();
                count = (int)cmd.ExecuteScalar();
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                count = -2;
            }

            return count;
        }

        public List<FacultyExtended> GetBatch(int start, int count)
        {
            List<FacultyExtended> ret = new List<FacultyExtended>();
            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT F.*, U.uni_name " + 
                                                "FROM Faculties F INNER JOIN Universities U ON F.uni_id = U.uni_id " +
                                                "ORDER BY facult_name " +
                                                "OFFSET " + start + " ROWS " +
                                                "FETCH NEXT " + count + " ROWS ONLY", conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new FacultyExtended((int)reader[0],
                                                    (string)reader[1],
                                                    (int)reader[2],
                                                    (int)reader[3],
                                                    (string)reader[5],
                                                    (string)reader[4]));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return ret;
        }
    }
}
