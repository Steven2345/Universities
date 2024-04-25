namespace backend.Repository
{
    using backend.Domain;
    using Microsoft.Data.SqlClient;

    public class FacultyRepository
    {
        private SqlConnection conn;

        private string getConnectionString()
        {
            return "Data source=localhost,1235;Initial Catalog=UnivDB;" +
                "User Id=universityuser;Password=root;Encrypt=False";
        }

        public FacultyRepository()
        {
            conn = new SqlConnection(getConnectionString());
        }

        public int AddFaculty(Faculty faculty)
        {
            const string query = "INSERT INTO Faculties(facult_name, facult_nostud, uni_id) " +
                                 "VALUES(@name, @nostud, @uniid);" +
                                 "SELECT SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", faculty.Name);
            cmd.Parameters.AddWithValue("@nostud", faculty.NoOfStudents);
            cmd.Parameters.AddWithValue("@uniid", faculty.University);
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
            finally
            {
                conn.Close();
            }

            return index;
        }

        public Faculty? SearchFaculty(int id)
        {
            List<Faculty?> ret = [];
            const string query = "SELECT * FROM Faculties WHERE facult_id=@id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ret.Add(new Faculty((int)reader[0],
                                        (string)reader[1],
                                        (int)reader[2],
                                        (int)reader[3]));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ret.Add(null);
            }
            finally
            {
                conn.Close();
            }

            return ret[0];
        }

        public int UpdateFaculty(Faculty faculty)
        {
            const string query = "UPDATE Faculties SET " +
                                 "facult_name=@name, facult_nostud=@nostud, uni_id=@uniid " +
                                 "WHERE facult_id=@id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", faculty.Id);
            cmd.Parameters.AddWithValue("@name", faculty.Name);
            cmd.Parameters.AddWithValue("@nostud", faculty.NoOfStudents);
            cmd.Parameters.AddWithValue("@uniid", faculty.University);
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
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public int DeleteFaculty(int id)
        {
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
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public int GetSizeOfRepo()
        {
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
            finally
            {
                conn.Close();
            }

            return count;
        }

        public List<Faculty> GetBatch(int start, int count)
        {
            List<Faculty> ret = new List<Faculty>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Faculties " +
                                            "ORDER BY facult_name " +
                                            "OFFSET " + start + " ROWS " +
                                            "FETCH NEXT " + count + " ROWS ONLY", conn);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ret.Add(new Faculty((int)reader[0],
                                        (string)reader[1],
                                        (int)reader[2],
                                        (int)reader[3]));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return ret;
        }

        /*private SqlConnection conn;
        private SqlDataAdapter daFaculties;
        private DataSet dataset;
        private SqlCommandBuilder cmdBuilderFaculties;

        private string getConnectionString()
        {
            return "Data source=localhost,1235;Initial Catalog=UnivDB;" +
                "User Id=universityuser;Password=root;Encrypt=False";
        }

        public FacultyRepository()
        {
            conn = new SqlConnection(getConnectionString());

            daFaculties = new SqlDataAdapter("select * from Faculties", conn);
            dataset = new DataSet();

            daFaculties.Fill(dataset);

            cmdBuilderFaculties = new SqlCommandBuilder(daFaculties);
            daFaculties.DeleteCommand = cmdBuilderFaculties.GetDeleteCommand();
            daFaculties.InsertCommand = cmdBuilderFaculties.GetInsertCommand();
            daFaculties.UpdateCommand = cmdBuilderFaculties.GetUpdateCommand();
        }

        public void AddFaculty(Faculty faculty)
        {
            DataTable table = dataset.Tables[0];
            DataRow row = table.NewRow();
            row["facult_name"] = faculty.Name;
            row["facult_nostud"] = faculty.NoOfStudents;
            row["uni_id"] = faculty.University;
            table.Rows.Add(row);
            daFaculties.Update(dataset);
        }

        public Faculty? SearchFaculty(int id)
        {
            DataTable table = dataset.Tables[0];
            var elems = from DataRow row in table.Rows
                        where (int)row["facult_id"] == id
                        select row;
            if (elems == null)
                return null;

            DataRow? elem = elems.FirstOrDefault();
            if (elem == null) 
                return null;

            return new Faculty(elem);
        }

        public void UpdateFaculty(Faculty faculty)
        {
            DataTable table = dataset.Tables[0];
            var elems = from DataRow row in table.Rows
                        where (int)row["facult_id"] == faculty.Id
                        select row;
            if (elems == null)
                return;

            DataRow? elem = elems.FirstOrDefault();
            if (elem == null)
                return;

            elem["facult_name"] = faculty.Name;
            elem["facult_nostud"] = faculty.NoOfStudents;
            elem["uni_id"] = faculty.University;

            daFaculties.Update(dataset);
        }

        public void DeleteUniversity(int id)
        {
            DataTable table = dataset.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                if ((int)row["facult_id"] == id)
                    row.Delete();
            }
            dataset.AcceptChanges();
            daFaculties.Update(dataset);
        }

        public List<Faculty> GetBatch(int start, int count)
        {
            List<Faculty> ret = new List<Faculty>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Faculties " +
                                            "ORDER BY facult_name " +
                                            "OFFSET " + start + " ROWS " +
                                            "FETCH NEXT " + count + " ROWS ONLY", conn);
            conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ret.Add(new Faculty((int)reader[0],
                                        (string)reader[1],
                                        (int)reader[2],
                                        (int)reader[3]));
                }
            }
            return ret;
        }*/

        /*public List<Faculty> GroupByUniId()
        {
            List<Faculty> ret = new List<Faculty>();
            SqlCommand cmd = new SqlCommand("SELECT U.uni_name AS uni_name, U.uni_location AS uni_location, COUNT(*) as faculties " + 
                                            "FROM Faculties F INNER JOIN Universities U ON F.uni_id = U.uni_id " +
                                            "GROUP BY F.uni_id", conn);
            conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ret.Add(new Faculty((int)reader[0],
                                        (string)reader[1],
                                        (int)reader[2],
                                        (int)reader[3]));
                }
            }
            return ret;
        }*/
    }
}
