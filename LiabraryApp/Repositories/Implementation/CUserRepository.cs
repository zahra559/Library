using LiabraryApp.Models;
using LiabraryApp.Repositories.Interfaces;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace LiabraryApp.Repositories.Implementation
{
    public class CUserRepository : IGenericRepository<CUser>
    {

        private readonly IConfiguration _configuration;

        public CUserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CUser> Edit(CUser entity)
        {
            throw new NotImplementedException();
        }
        public async Task<List<CUser>> Get(Hashtable criteria)
        {
            try
            {
                do
                {
                    List<CUser> list = new List<CUser>();

                    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLConnection")))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("FETCH_USERS", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();

                        if (criteria.ContainsKey("ID"))
                            cmd.Parameters.Add(new SqlParameter("@ID", criteria["ID"]));

                        if (criteria.ContainsKey("NAME"))
                            cmd.Parameters.Add(new SqlParameter("@NAME", criteria["NAME"]));

                        if (criteria.ContainsKey("EMAIL"))
                            cmd.Parameters.Add(new SqlParameter("@EMAIL", criteria["EMAIL"]));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                list.Add(new CUser()
                                {
                                    ID = reader.GetInt32("ID"),
                                    Name = reader.GetString("USER_NAME"),
                                    Email = reader.GetString("USER_EMAIL"),
                                });
                            }
                        }
                        connection.Close();
                        return list;
                    }
                } while (false);
            }catch (Exception ex)
            {
                Exception exception = new Exception(ex.Message, ex);
                throw exception;
            }
        }

        public async Task<CUser> Save(CUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
