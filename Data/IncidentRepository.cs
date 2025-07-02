using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AspnetCoreMvcStarter.Models;

public class IncidentRepository
{
  private readonly string _connectionString;

  public IncidentRepository(string connectionString)
  {
    _connectionString = connectionString;
  }

  public List<Incident> GetAllIncidents()
  {
    var incidents = new List<Incident>();

    using (var connection = new MySqlConnection(_connectionString))
    {
      var command = new MySqlCommand("  SELECT AvvikelseIndexNr,Subject, location, avvdatum, investigator , flaggor, AktivitetsIndexNr FROM qwavv", connection);
      connection.Open();
      using (var reader = command.ExecuteReader())
      {
        while (reader.Read())
        {
          var statusValue = reader.GetString(5);
          string status;
          switch (statusValue)
          {
            case "A":
              status = "Inlämnad";
              break;
            case "B":
              status = "Avslutad";
              break;
            case "C":
              status = "Bevakning";
              break;
            case "D":
              status = "Öppen";
              break;
            default:
              status = statusValue;
              break;
          }
          var incidentType = reader.IsDBNull(6) ? string.Empty : reader.GetInt32(6).ToString();
          var incident = new Incident
          {
            Id = reader.GetInt32(0),
            Subject = reader.GetString(1),
            Location = reader.GetString(2),
            DateReported = reader.GetDateTime(3),
            OperatedBy = reader.GetString(4),
            Type = incidentType,
            Status = status
          };

          // Console write the incident
          Console.WriteLine($"Id: {incident.Id}, Subject: {incident.Subject}, Location: {incident.Location}, DateReported: {incident.DateReported}, OperatedBy: {incident.OperatedBy}, Type: {incident.Type}, Status: {incident.Status}");

          incidents.Add(incident);
        }
      }
    }

    return incidents;
  }

  public Incident GetIncident(int id)
  {
    Incident incident = null;

    using (var connection = new MySqlConnection(_connectionString))
    {
      var command = new MySqlCommand("  SELECT * FROM qwavv WHERE AvvikelseIndexNr = @id", connection);
      command.Parameters.AddWithValue("@id", id);
      connection.Open();
      using (var reader = command.ExecuteReader())
      {

        if (reader.Read())
        {
          incident = new Incident
          {
            Id = reader.GetInt32(reader.GetOrdinal("AvvikelseIndexNr")),
            Customer = reader.GetString(reader.GetOrdinal("kundnamn")),
            Subject = reader.GetString(reader.GetOrdinal("Subject")),
            DateReported = reader.GetDateTime(reader.GetOrdinal("avvdatum")),
            Location = reader.GetString(reader.GetOrdinal("Location")),
            Status = reader.GetString(reader.GetOrdinal("flaggor"))

          };
        }
        else
        {
          throw new KeyNotFoundException($"Incident with ID {id} not found.");
        }

      }
    }

    return incident;
  }
}
