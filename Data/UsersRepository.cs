using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AspnetCoreMvcStarter.Models;

public class UsersRepository
{
  private readonly string _connectionString;
  public UsersRepository(string connectionString)
  {
    _connectionString = connectionString;
  }
  public List<User> GetAllUsers()
  {
    var Users = new List<User>();

    using (var connection = new MySqlConnection(_connectionString))
    {
      var command = new MySqlCommand("  SELECT * FROM secure", connection);
      connection.Open();
      using (var reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          Users.Add(new User
          {
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            Name = reader.GetString(reader.GetOrdinal("Namn")),
            Picture = (byte[])reader.GetValue(reader.GetOrdinal("Picture")),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            Password = reader.GetString(reader.GetOrdinal("Password"))
          });
        }
      }
    }

    return Users;
  }

  public User GetUser(int id)
  {
    User user = null;

    using (var connection = new MySqlConnection(_connectionString))
    {
      var command = new MySqlCommand("  SELECT * FROM secure WHERE id = @id", connection);
      command.Parameters.AddWithValue("@id", id);
      connection.Open();
      using (var reader = command.ExecuteReader())
      {

        if (reader.Read())
        {
          user = new User
          {
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            Name = reader.GetString(reader.GetOrdinal("Namn")),
            Picture = (byte[])reader.GetValue(reader.GetOrdinal("Picture")),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            Password = reader.GetString(reader.GetOrdinal("Password"))

          };
        }
        else
        {
          throw new KeyNotFoundException($"User with ID {id} not found.");
        }

      }
    }

    return user;
  }

}
