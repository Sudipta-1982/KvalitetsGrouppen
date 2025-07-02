using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class DocumentRepository
{

  //  public record DocNode(int id, int pid, string name, bool hasChild, bool expanded, bool check);
  public List<object> GetDocumentsList()
  {
    var connectionString = "Server=192.168.0.214;Port=3308;Database=qw;Uid=Viorica;Pwd=vio;";
    using var cn = new MySqlConnection(connectionString);
    cn.Open();
    string query = "  SELECT lopNr AS id, PID, TEXT, grupp FROM qwoutln";
    using var cmd = new MySqlCommand(query, cn);
    using var reader = cmd.ExecuteReader();
    List<object> listdata = new List<object>();
    while (reader.Read())
    {
      // Console.WriteLine(reader["text"].ToString() + " " + reader["id"].ToString() + " " + reader["pid"].ToString());
      var id = reader["id"].ToString();
      var pid = reader["pid"].ToString();
      var idInt = Convert.ToInt32(id);
      var pidInt = Convert.ToInt32(pid);
      var name = reader["text"].ToString();

      bool hasC = false;
      if (pidInt == 0)
      {
        hasC = true;
      }
      var nodeID = reader["grupp"].ToString();
      var fullName = name + " (" + nodeID + ")";
      Console.WriteLine($"id: {id}, pid: {pid}, name: {name}, fullName: {fullName}");
      var pNodeID = !string.IsNullOrEmpty(nodeID) && nodeID.Contains('.') ? nodeID[..nodeID.LastIndexOf('.')] : nodeID;
      var nodeNr = !string.IsNullOrEmpty(nodeID) && nodeID.Contains('.') ? nodeID[(nodeID.LastIndexOf('.') + 1)..] : nodeID;
      listdata.Add(new { id = idInt, pid = pidInt, name = name, fullName = fullName, hasChild = hasC, expanded = false, pNodeID = pNodeID, nodeID = nodeID, nodeNr = nodeNr });

    }
    return listdata;

  }

}
