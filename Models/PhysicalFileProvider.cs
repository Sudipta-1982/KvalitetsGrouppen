using Syncfusion.EJ2.FileManager.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AspnetCoreMvcStarter // Change this namespace to match your project
{
  public class PhysicalFileProvider
  {
    private string rootFolder;

    public void RootFolder(string rootPath)
    {
      rootFolder = rootPath;
    }

    public object PerformFileOperation(FileManagerDirectoryContent args)
    {
      switch (args.Action)
      {
        case "read":
          return GetFiles(args.Path);
        case "delete":
          DeleteFiles(args.Names, args.Path);
          break;
        case "copy":
          CopyFiles(args.Names, args.Path, args.TargetPath);
          break;
        case "move":
          MoveFiles(args.Names, args.Path, args.TargetPath);
          break;
        case "details":
          return GetDetails(args.Names, args.Path);
        case "create":
          CreateFolder(args.Path, args.Name);
          break;
        case "search":
          return SearchFiles(args.Path, args.SearchString);
      }
      return null;
    }

    private object GetFiles(string path)
    {
      var directoryPath = Path.Combine(rootFolder, path ?? string.Empty);

      if (!Directory.Exists(directoryPath))
        return new { files = new List<object>(), dirs = new List<object>() };

      var directories = Directory.GetDirectories(directoryPath).Select(dir => new
      {
        Name = Path.GetFileName(dir),
        IsFile = false,
        Size = 0,
        DateCreated = Directory.GetCreationTime(dir),
        DateModified = Directory.GetLastWriteTime(dir)
      });

      var files = Directory.GetFiles(directoryPath).Select(file => new
      {
        Name = Path.GetFileName(file),
        IsFile = true,
        Size = new FileInfo(file).Length,
        DateCreated = File.GetCreationTime(file),
        DateModified = File.GetLastWriteTime(file)
      });

      return new { files = files.ToList(), dirs = directories.ToList() };
    }

    private void DeleteFiles(string[] names, string path)
    {
      foreach (var name in names)
      {
        var fullPath = Path.Combine(rootFolder, path, name);
        if (Directory.Exists(fullPath))
          Directory.Delete(fullPath, true);
        else if (File.Exists(fullPath))
          File.Delete(fullPath);
      }
    }

    private void CopyFiles(string[] names, string path, string targetPath)
    {
      foreach (var name in names)
      {
        var sourcePath = Path.Combine(rootFolder, path, name);
        var destPath = Path.Combine(rootFolder, targetPath, name);

        if (Directory.Exists(sourcePath))
          CopyDirectory(sourcePath, destPath);
        else if (File.Exists(sourcePath))
          File.Copy(sourcePath, destPath, true);
      }
    }

    private void CopyDirectory(string sourceDir, string destinationDir)
    {
      Directory.CreateDirectory(destinationDir);
      foreach (var file in Directory.GetFiles(sourceDir))
        File.Copy(file, Path.Combine(destinationDir, Path.GetFileName(file)), true);

      foreach (var directory in Directory.GetDirectories(sourceDir))
        CopyDirectory(directory, Path.Combine(destinationDir, Path.GetFileName(directory)));
    }

    private void MoveFiles(string[] names, string path, string targetPath)
    {
      foreach (var name in names)
      {
        var sourcePath = Path.Combine(rootFolder, path, name);
        var destPath = Path.Combine(rootFolder, targetPath, name);

        if (Directory.Exists(sourcePath))
          Directory.Move(sourcePath, destPath);
        else if (File.Exists(sourcePath))
          File.Move(sourcePath, destPath);
      }
    }

    private object GetDetails(string[] names, string path)
    {
      var details = new List<object>();

      foreach (var name in names)
      {
        var fullPath = Path.Combine(rootFolder, path, name);
        if (File.Exists(fullPath))
        {
          var info = new FileInfo(fullPath);
          details.Add(new
          {
            Name = name,
            Size = info.Length,
            DateCreated = info.CreationTime,
            DateModified = info.LastWriteTime,
            IsFile = true
          });
        }
        else if (Directory.Exists(fullPath))
        {
          var dirInfo = new DirectoryInfo(fullPath);
          details.Add(new
          {
            Name = name,
            Size = 0,
            DateCreated = dirInfo.CreationTime,
            DateModified = dirInfo.LastWriteTime,
            IsFile = false
          });
        }
      }

      return details;
    }

    private void CreateFolder(string path, string folderName)
    {
      var newFolderPath = Path.Combine(rootFolder, path, folderName);
      if (!Directory.Exists(newFolderPath))
      {
        Directory.CreateDirectory(newFolderPath);
      }
    }

    private object SearchFiles(string path, string searchString)
    {
      var directoryPath = Path.Combine(rootFolder, path);
      if (!Directory.Exists(directoryPath))
        return new { files = new List<object>(), dirs = new List<object>() };

      var matchingFiles = Directory.GetFiles(directoryPath, $"*{searchString}*").Select(file => new
      {
        Name = Path.GetFileName(file),
        IsFile = true,
        Size = new FileInfo(file).Length,
        DateCreated = File.GetCreationTime(file),
        DateModified = File.GetLastWriteTime(file)
      });

      var matchingDirs = Directory.GetDirectories(directoryPath, $"*{searchString}*").Select(dir => new
      {
        Name = Path.GetFileName(dir),
        IsFile = false,
        Size = 0,
        DateCreated = Directory.GetCreationTime(dir),
        DateModified = Directory.GetLastWriteTime(dir)
      });

      return new { files = matchingFiles.ToList(), dirs = matchingDirs.ToList() };
    }

    public void Upload(string path, IList<IFormFile> uploadFiles, string action, object param)
    {
      var uploadPath = Path.Combine(rootFolder, path);

      if (!Directory.Exists(uploadPath))
        Directory.CreateDirectory(uploadPath);

      foreach (var file in uploadFiles)
      {
        var filePath = Path.Combine(uploadPath, file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          file.CopyTo(stream);
        }
      }
    }

    public IActionResult Download(string path, string[] names)
    {
      var fileName = names.FirstOrDefault();
      if (string.IsNullOrEmpty(fileName))
        return new NotFoundResult();

      var fullPath = Path.Combine(rootFolder, path, fileName);
      if (!System.IO.File.Exists(fullPath))
        return new NotFoundResult();

      var contentType = "application/octet-stream";
      return new FileStreamResult(new FileStream(fullPath, FileMode.Open, FileAccess.Read), contentType)
      {
        FileDownloadName = fileName
      };
    }

    public IActionResult GetImage(string path, string id)
    {
      var fullPath = Path.Combine(rootFolder, path, id);
      if (!System.IO.File.Exists(fullPath))
        return new NotFoundResult();

      var contentType = "image/jpeg"; // Or detect content type based on file extension
      return new FileStreamResult(new FileStream(fullPath, FileMode.Open, FileAccess.Read), contentType);
    }
  }
}
