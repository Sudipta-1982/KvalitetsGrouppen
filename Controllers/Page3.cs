using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcStarter.Models;
using System.Collections.Generic;
using MySql.Data.MySqlClient;



public class Page3 : Controller
{
  private readonly DocumentRepository _connectTest;
  public Page3(DocumentRepository connectTest)
  {
    _connectTest = connectTest;
  }
  public IActionResult Index()
  {
    var listdata = _connectTest.GetDocumentsList();
    ViewBag.TreeViewData = listdata;
    return View();

  }


  public IActionResult Treeview() => View();
  public ActionResult ToggleButton()
  {
    return View();
  }

}
