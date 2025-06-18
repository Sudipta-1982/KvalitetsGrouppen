using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.FileManager.Base;
using Syncfusion.EJ2.FileManager.PhysicalFileProvider;
using System;
using System.Collections.Generic;

namespace YourNamespace.Controllers
{
  public class FileExplorerController : Controller
  {
    private readonly PhysicalFileProvider _fileProvider;
    private readonly string _rootPath;

    public FileExplorerController(IWebHostEnvironment hostingEnvironment)
    {
      _rootPath = hostingEnvironment.WebRootPath + "/content/";
      _fileProvider = new PhysicalFileProvider();
      _fileProvider.RootFolder(_rootPath);
    }

    [HttpPost]
    public IActionResult FileOperations([FromBody] FileManagerDirectoryContent args)
    {
      try
      {
        switch (args.Action)
        {
          case "read":
            return Json(_fileProvider.GetFiles(args.Path, args.ShowHiddenItems));

          case "delete":
            return Json(_fileProvider.Delete(args.Path, args.Names));

          case "copy":
            return Json(_fileProvider.Copy(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData));

          case "move":
            return Json(_fileProvider.Move(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData));

          case "details":
            return Json(_fileProvider.Details(args.Path, args.Names));

          case "create":
            return Json(_fileProvider.Create(args.Path, args.Name));

          case "search":
            return Json(_fileProvider.Search(args.Path, args.SearchString, args.ShowHiddenItems, false));

          case "rename":
            return Json(_fileProvider.Rename(args.Path, args.Name, args.NewName));

          case "download":
            return Download(System.Text.Json.JsonSerializer.Serialize(new
            {
              path = args.Path,
              names = args.Names
            }));
        }
      }
      catch (Exception ex)
      {
        return Json(new { error = ex.Message });
      }

      return Json(new { error = "Invalid operation" });
    }

    [HttpPost]
    public IActionResult Upload(string path, IList<IFormFile> uploadFiles, string action)
    {
      try
      {
        return Json(_fileProvider.Upload(path, uploadFiles, action, 0));
      }
      catch (Exception ex)
      {
        return Json(new { error = ex.Message });
      }
    }

    [HttpGet]
    public IActionResult Download(string downloadInput)
    {
      try
      {
        var args = System.Text.Json.JsonSerializer.Deserialize<FileManagerDirectoryContent>(downloadInput);
        return _fileProvider.Download(args.Path, args.Names);
      }
      catch (Exception ex)
      {
        return BadRequest(new { error = ex.Message });
      }
    }

    [HttpGet]
    public IActionResult GetImage(FileManagerDirectoryContent args)
    {
      try
      {
        return _fileProvider.GetImage(args.Path, args.Id, false, null, null);
      }
      catch (Exception ex)
      {
        return BadRequest(new { error = ex.Message });
      }
    }

    public IActionResult Index()
    {
      return View();
    }
  }
}
