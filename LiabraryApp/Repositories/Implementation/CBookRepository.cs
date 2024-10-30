using LiabraryApp.Models;
using LiabraryApp.Repositories.Interfaces;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace LiabraryApp.Repositories.Implementation
{
    public class CBookRepository : IBookRepository
    {

        private readonly IConfiguration _configuration;

        public CBookRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CBook> Edit(CBook entity)
        {
            throw new NotImplementedException();
        }

        public Task<CBook> Save(CBook entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CBook>> Get(Hashtable criteria)
        {
            try
            {
                do
                {
                    List<CBook> list = new List<CBook>();

                    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLConnection")))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("FETCH_BOOKS", connection);

                        if (cmd == null)
                        {
                            Exception exception = new Exception("Server error");
                            throw exception;
                        }

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();

                        if (criteria.ContainsKey("ID"))
                            cmd.Parameters.Add(new SqlParameter("@ID", criteria["ID"]));

                        if (criteria.ContainsKey("NAME"))
                            cmd.Parameters.Add(new SqlParameter("@NAME", criteria["NAME"]));

                        if (criteria.ContainsKey("AUTHOR"))
                            cmd.Parameters.Add(new SqlParameter("@AUTHOR", criteria["AUTHOR"]));

                        if (criteria.ContainsKey("ISBN"))
                            cmd.Parameters.Add(new SqlParameter("@ISBN", criteria["ISBN"]));

                        if (criteria.ContainsKey("IS_AVAILABLE"))
                            cmd.Parameters.Add(new SqlParameter("@IS_AVAILABLE", criteria["IS_AVAILABLE"]));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                list.Add(new CBook()
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Name = reader.GetString("BOOK_NAME"),
                                    Author = reader.GetString("AUTHOR_NAME"),
                                    ISBN = reader.GetString("BOOK_ISBN"),
                                    IsAvailable = Convert.ToBoolean(reader["IS_AVAILABLE"]),
                                });
                            }
                        }
                        connection.Close();
                    } return list;
                } while (false);
             }
            catch (Exception ex)
            {
                Exception exception = new Exception(ex.Message, ex);
                throw exception;
            }
         }

        public async Task<bool> ReturningBook(Hashtable criteria)
        {
            try
            {
                do
                {

                    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLConnection")))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("RETURNING_BOOK", connection);

                        if (cmd == null)
                        {
                            Exception exception = new Exception("Server error");
                            throw exception;
                        }

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();

                        cmd.Parameters.Add(new SqlParameter("@BOOK_ID", criteria["BOOK_ID"]));
                        cmd.Parameters.Add(new SqlParameter("@USER_ID", criteria["USER_ID"]));

                        var affectedRows = await cmd.ExecuteNonQueryAsync();

                        connection.Close();

                        if (affectedRows > 0)
                            return true;

                        else return false;
                    }
                } while (false);
            }
            catch (Exception ex)
            {
                Exception exception = new Exception(ex.Message, ex);
                throw exception;
            }
        }

        public async Task<bool> BorrowingBook(Hashtable criteria)
        {
            try
            {
                do
                {

                    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLConnection")))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("BORROWING_BOOK", connection);

                        if (cmd == null)
                        {
                            Exception exception = new Exception("Server error");
                            throw exception;
                        }

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();

                        cmd.Parameters.Add(new SqlParameter("@BOOK_ID", criteria["BOOK_ID"]));
                        cmd.Parameters.Add(new SqlParameter("@USER_ID", criteria["USER_ID"]));

                        var affectedRows = await cmd.ExecuteNonQueryAsync();

                        connection.Close();

                        if (affectedRows > 0)
                            return true;

                        else return false;
                    }
                } while (false);
            }
            catch (Exception ex)
            {
                Exception exception = new Exception(ex.Message, ex);
                throw exception;
            }
        }
    }
}
