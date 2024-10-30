using LiabraryApp.Models;
using LiabraryApp.Repositories.Interfaces;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace LiabraryApp.Repositories.Implementation
{
    public class CBorrowingRepository : IGenericRepository<CBorrowing>
    {

        private readonly IConfiguration _configuration;

        public CBorrowingRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CBorrowing> Edit(CBorrowing entity)
        {
            throw new NotImplementedException();
        }

        public async Task<CBorrowing> Save(CBorrowing entity)
        {
            throw new NotImplementedException();
        }
        public async Task<List<CBorrowing>> Get(Hashtable criteria)
        {
            try
            {
                do{
                    List<CBorrowing> list = new List<CBorrowing>();

                    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLConnection")))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("FETCH_BORROWINGS", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();

                        if(criteria.ContainsKey("BOOK_ID"))
                            cmd.Parameters.Add(new SqlParameter("@BOOK_ID", criteria["BOOK_ID"]));

                        if(criteria.ContainsKey("USER_ID"))
                            cmd.Parameters.Add(new SqlParameter("@USER_ID", criteria["USER_ID"]));

                        if (criteria.ContainsKey("BOOK_ISBN"))
                            cmd.Parameters.Add(new SqlParameter("@BOOK_ISBN", criteria["BOOK_ISBN"]));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                CBook book = new CBook();
                                book.ID = reader.GetInt32("BOOK_ID");
                                book.Name = reader.GetString("BOOK_NAME");
                                book.Author = reader.GetString("BOOK_AUTHOR");
                                book.ISBN = reader.GetString("BOOK_ISBN");
                                book.IsAvailable = reader.GetBoolean("BOOK_IS_AVAILABLE");

                                CUser user = new CUser();
                                user.ID = reader.GetInt32("USER_ID");
                                user.Name = reader.GetString("USER_NAME");
                                user.Email = reader.GetString("USER_EMAIL");

                                list.Add(new CBorrowing()
                                {
                                    ID = reader.GetInt32("ID"),
                                    Book = book,
                                    User = user,
                                });
                            }
                        }
                        connection.Close();
                    }
                    return list;
                } while (false) ;
            }
            catch (Exception ex)
            {
                Exception exception = new Exception(ex.Message, ex);
                throw exception;
            }
        }
    }
}