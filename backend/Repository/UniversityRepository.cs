namespace backend.Repository
{
    using backend.Domain;
    using Microsoft.Data.SqlClient;

    public class UniversityRepository
    {
        private string getConnectionString()
        {
            return "Data source=localhost,1235;Initial Catalog=UnivDB;" +
                "User Id=universityuser;Password=root;Encrypt=False";
        }

        public int AddUniversity(University uni)
        {
            using SqlConnection conn = new SqlConnection(getConnectionString());
            const string query = "INSERT INTO Universities(uni_name, uni_location, uni_score, uni_descr) " +
                                 "VALUES(@name, @location, @score, @descr);" +
                                 "SELECT SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", uni.Name);
            cmd.Parameters.AddWithValue("@location", uni.Location);
            cmd.Parameters.AddWithValue("@score", uni.Score);
            cmd.Parameters.AddWithValue("@descr", uni.Description);
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

        public UniversityExtended? SearchUniversity(int id)
        {
            List<UniversityExtended?> ret = [];
            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                const string query = "SELECT U.*, ISNULL(t.faculties, 0) " +
                                     "FROM Universities U LEFT JOIN (SELECT uni_id, COUNT(*) AS faculties " +
                                                                    "FROM Faculties " +
                                                                    "GROUP BY uni_id " +
                                                                    "HAVING uni_id=@id) t ON t.uni_id = U.uni_id ";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new UniversityExtended((int)reader[0],
                                                       (string)reader[1],
                                                       (string)reader[2],
                                                       (int)reader[5],
                                                       (double)reader[3],
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

        public int UpdateUniversity(University university)
        {
            using SqlConnection conn = new SqlConnection(getConnectionString());
            const string query = "UPDATE Universities SET " + 
                                 "uni_name=@name, uni_location=@location, uni_score=@score, uni_descr=@descr " +
                                 "WHERE uni_id=@id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", university.Id);
            cmd.Parameters.AddWithValue("@name", university.Name);
            cmd.Parameters.AddWithValue("@location", university.Location);
            cmd.Parameters.AddWithValue("@score", university.Score);
            cmd.Parameters.AddWithValue("@descr", university.Description);
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

        public int DeleteUniversity(int id)
        {
            using SqlConnection conn = new SqlConnection(getConnectionString());
            const string query = "DELETE FROM Universities WHERE uni_id=@id";
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
            const string query = "SELECT COUNT(*) FROM Universities";
            SqlCommand cmd = new SqlCommand(query, conn);
            int count;

            try
            {
                conn.Open();
                count = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                count = -2;
            }

            return count;
        }

        public List<University> GetBatch(int start, int count)
        {
            List<University> ret = new List<University>();
            using (SqlConnection cnct = new SqlConnection(getConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Universities " +
                                                "ORDER BY uni_score DESC " +
                                                "OFFSET " + start + " ROWS " +
                                                "FETCH NEXT " + count + " ROWS ONLY", cnct);
                try
                {
                    cnct.Open();
                    Console.WriteLine("Tried");
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new University((int)reader[0],
                                                (string)reader[1],
                                                (string)reader[2],
                                                (double)reader[3],
                                                (string)reader[4]));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.Write("Uni repo getbatch: ");
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine("Close");
                    Console.WriteLine();
                }

            }
            return ret;
        }

        public List<UniversityExtended> GroupByUniId(int start, int count)
        {
            List<UniversityExtended> ret = new List<UniversityExtended>();
            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                const string query = "SELECT U.*, ISNULL(t.faculties, 0) " +
                                     "FROM Universities U LEFT JOIN (SELECT uni_id, COUNT(*) AS faculties " +
                                                                    "FROM Faculties " +
                                                                    "GROUP BY uni_id) t ON t.uni_id = U.uni_id " + 
                                     "ORDER BY U.uni_score DESC " +
                                     "OFFSET @start ROWS " +
                                     "FETCH NEXT @count ROWS ONLY";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@count", count);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new UniversityExtended((int)reader[0],
                                                       (string)reader[1],
                                                       (string)reader[2],
                                                       (int)reader[5],
                                                       (double)reader[3],
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

        public List<UniversityMinimal> GetUniversityNames()
        {
            List<UniversityMinimal> ret = new List<UniversityMinimal>();
            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                const string query = "SELECT U.uni_id, U.uni_name " +
                                     "FROM Universities U " +
                                     "ORDER BY U.uni_name ASC";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new UniversityMinimal((int)reader[0],
                                                      (string)reader[1]));
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
