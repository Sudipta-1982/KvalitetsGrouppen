using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcStarter.Models;
using System.Text;
using System.Globalization;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace AspnetCoreMvcStarter.Controllers
{
  public class Page4Controller : Controller
  {
    public IActionResult Index()
    {
      return View();
    }

    [HttpPost]
    public IActionResult AddCustomer(CustomerModel customer)
    {
      if (!ModelState.IsValid)
        return View("Index");

      // TODO: Save the customer to DB

      return RedirectToAction("Index");
    }

    public IActionResult ExportToCsv()
    {
      var customers = GetMockCustomers();

      var csv = new StringBuilder();
      csv.AppendLine("CustomerName,CustomerID,ContactPerson,Email,Website,Classification,SatisfactionLevel,EvaluationDate");

      foreach (var customer in customers)
      {
        csv.AppendLine($"{customer.CustomerName},{customer.CustomerID},{customer.ContactPerson},{customer.Email},{customer.Website},{customer.Classification},{customer.SatisfactionLevel},{customer.EvaluationDate?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
      }

      var bytes = Encoding.UTF8.GetBytes(csv.ToString());
      return File(bytes, "text/csv", "kundlista.csv");
    }

    public IActionResult ExportToExcel()
    {
      var customers = GetMockCustomers();

      using var workbook = new XLWorkbook();
      var worksheet = workbook.Worksheets.Add("Kundlista");

      worksheet.Cell(1, 1).Value = "Kund";
      worksheet.Cell(1, 2).Value = "Klassificering";
      worksheet.Cell(1, 3).Value = "Kontakta";
      worksheet.Cell(1, 4).Value = "E-post";
      worksheet.Cell(1, 5).Value = "Tillfredsställelse";

      int row = 2;
      foreach (var c in customers)
      {
        worksheet.Cell(row, 1).Value = c.CustomerName;
        worksheet.Cell(row, 2).Value = c.Classification;
        worksheet.Cell(row, 3).Value = c.ContactPerson;
        worksheet.Cell(row, 4).Value = c.Email;
        worksheet.Cell(row, 5).Value = c.SatisfactionLevel;
        row++;
      }

      using var stream = new MemoryStream();
      workbook.SaveAs(stream);
      stream.Position = 0;

      return File(stream.ToArray(),
          "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          "Kundlista.xlsx");
    }

    public IActionResult ExportToPdf()
    {
      var customers = GetMockCustomers();

      var stream = new MemoryStream();

      Document.Create(container =>
      {
        container.Page(page =>
              {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                          .Text("Kundlista")
                          .FontSize(20)
                          .SemiBold()
                          .FontColor(Colors.Blue.Medium);

                page.Content()
                          .Table(table =>
                          {
                            table.ColumnsDefinition(columns =>
                              {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                              });

                            table.Header(header =>
                              {
                                header.Cell().Element(CellStyle).Text("Kund");
                                header.Cell().Element(CellStyle).Text("Klassificering");
                                header.Cell().Element(CellStyle).Text("Kontakt");
                                header.Cell().Element(CellStyle).Text("E-post");
                                header.Cell().Element(CellStyle).Text("Tillfredsställelse");

                                static IContainer CellStyle(IContainer container) =>
                                      container.Padding(5).Background(Colors.Grey.Lighten2).BorderBottom(1).BorderColor(Colors.Grey.Medium);
                              });

                            foreach (var c in customers)
                            {
                              table.Cell().Element(CellStyle).Text(c.CustomerName);
                              table.Cell().Element(CellStyle).Text(c.Classification);
                              table.Cell().Element(CellStyle).Text(c.ContactPerson);
                              table.Cell().Element(CellStyle).Text(c.Email);
                              table.Cell().Element(CellStyle).Text(c.SatisfactionLevel);

                              static IContainer CellStyle(IContainer container) =>
                                    container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                            }
                          });
              });
      }).GeneratePdf(stream);

      stream.Position = 0;
      return File(stream.ToArray(), "application/pdf", "kundlista.pdf");
    }

    private List<CustomerModel> GetMockCustomers()
    {
      return new List<CustomerModel>
            {
                new CustomerModel
                {
                    CustomerName = "LOGS TERMINAL AB",
                    CustomerID = "551894187",
                    ContactPerson = "Nils Jansson",
                    Email = "info@logsterminal.se",
                    Website = "https://logsterminal.se",
                    Classification = "B Kund",
                    SatisfactionLevel = "Inte nöjd",
                    EvaluationDate = DateTime.Now.AddMonths(-2)
                },
                new CustomerModel
                {
                    CustomerName = "JOVI Konsult",
                    CustomerID = "55184329",
                    ContactPerson = "Ingrid Andersson",
                    Email = "info@jovikonsult.se",
                    Website = "https://jovikonsult.se",
                    Classification = "C Kund",
                    SatisfactionLevel = "Inte nöjd",
                    EvaluationDate = DateTime.Now.AddMonths(-2)
                },
                new CustomerModel
                {
                    CustomerName = "Kylmontage Redotem AB",
                    CustomerID = "94187321",
                    ContactPerson = "Ingrid Karlsson",
                    Email = "info@noamekaniska.se",
                    Website = "https://kylmontage.se",
                    Classification = "A Kund",
                    SatisfactionLevel = "Nöjd",
                    EvaluationDate = DateTime.Now.AddMonths(-2)
                },
                new CustomerModel
                {
                    CustomerName = "LTI Isolering AB",
                    CustomerID = "1894187",
                    ContactPerson = "Björn Jansson",
                    Email = "info@ltiisolering.se",
                    Website = "https://ltiisolering.se",
                    Classification = "A Kund",
                    SatisfactionLevel = "Nöjd",
                    EvaluationDate = DateTime.Now.AddMonths(-2)
                },
                new CustomerModel
                {
                    CustomerName = "Kylmontage Swedac",
                    CustomerID = "189554187",
                    ContactPerson = "Erik Karlsson",
                    Email = "info@kylmontageswedac.se",
                    Website = "https://kylmontageswedac.se",
                    Classification = "A Kund",
                    SatisfactionLevel = "Nöjd",
                    EvaluationDate = DateTime.Now.AddMonths(-2)
                },
                new CustomerModel
                {
                    CustomerName = "PR Development AB",
                    CustomerID = "55189647383",
                    ContactPerson = "Kristina Andersson",
                    Email = "info@prdevelopment.se",
                    Website = "https://prdevelopment.se",
                    Classification = "B Kund",
                    SatisfactionLevel = "Inte nöjd",
                    EvaluationDate = DateTime.Now.AddMonths(-2)
                }
            };
    }
  }
}
