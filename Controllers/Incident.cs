//using System.Diagnostics;
//using Microsoft.AspNetCore.Mvc;
/*using AspnetCoreMvcStarter.Models;

namespace AspnetCoreMvcStarter.Controllers;

public class Incident : Controller
{
  public IActionResult Index() => View();
}*/
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcStarter.Models;
using AspnetCoreMvcStarter.Helpers;

namespace AspnetCoreMvcStarter.Controllers;

public class PageIncident : Controller
{
  public IActionResult Add() => View();
  private readonly IncidentRepository _incidentRepository;
  public PageIncident()
  {
    var config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
             .Build();

    var connectionString = config.GetConnectionString("DefaultConnection");
    Console.WriteLine($"Connection String: {connectionString}");
    if (string.IsNullOrEmpty(connectionString))
    {
      throw new InvalidOperationException("Database connection string 'DefaultConnection' is missing or empty in appsettings.json.");
    }
    _incidentRepository = new IncidentRepository(connectionString);
  }

  public IActionResult Index()
  {
    var incidents = _incidentRepository.GetAllIncidents();
    return View(incidents);
  }

  public IActionResult Edit(int id)
  {
    var incident = _incidentRepository.GetIncident(id);
    // Fetch the incident from your data source


    if (incident == null)
    {
      return NotFound();
    }
    Console.WriteLine($"Incident: Id={incident.Id}, Title={incident.Title}, Type={incident.Type}, Subject={incident.Subject}, Location={incident.Location}, Status={incident.Status}");
    return View(incident);

  }
  public IActionResult Details(int id)
  {
    // TODO: Replace with real data fetching logic
    var incident = new Incident
    {
      Id = id,
      Type = "Reklamation",
      Subject = "Power Outage",
      Location = "Office Building 1",
      DocName = "Updaterad rutin -7.3.1",
      SecretLabel = "Sekretess",
      Reference = "REF-12345",
      Title = "Sample Title",
      Customer = "Microsoft Corporation LTD",
      Supplier = "IKEA AB",
      Description = "On June 1st, 2024, a major network outage occurred in Building 1, disrupting connectivity for all employees on the first and second floors. The incident began at 8:15 AM and was reported by multiple users experiencing loss of access to internal systems and the internet. IT staff responded immediately, identifying a failed core switch as the root cause. Temporary measures were implemented to restore partial service while replacement hardware was sourced.",
      DateClosed = DateTime.Parse("2024-06-01"),
      DateReported = DateTime.Parse("2024-06-01"),
      DateFollowUp = DateTime.Parse("2024-06-02"),
      OperatedBy = "Sample Operated By",
      CustomerID = 1,
      SupplierID = 1,
      DocID = 1,
      Secret = false,
      PreventingMeasures = "Sample Preventing Measures",
      DecidedByUID = 2,
      FollowUp = "After the initial response, the IT team conducted a thorough investigation to determine the root cause of the outage. It was discovered that a power surge had damaged the core network switch, leading to widespread connectivity issues. The faulty hardware was promptly replaced, and additional surge protection was installed. A review meeting was held to discuss preventive measures and improve incident response protocols for future events.",
      FollowUID = 3,
      Notes = "Sample Notes",
      Status = "Pending"
    };

    if (incident == null)
    {
      return NotFound();
    }

    return View(incident);
  }
  public IActionResult Print() => View();

  public IActionResult Charts() => View();


}
