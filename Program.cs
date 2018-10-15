using System;
using System.Collections.Generic;
using System.IO;
using FicRecs_ExcelImporter.Models;
using OfficeOpenXml;

namespace FicRecs_ExcelImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine($"USAGE: {Environment.GetCommandLineArgs()[0]} importfile.xlsx");
                return;
            }

            var file = new FileInfo(args[0]);
            if (!file.Exists)
            {
                Console.WriteLine($"{args[0]} does not exist");
                return;
            }

            
            using (var p = new ExcelPackage(file))
            {
                using (var context = new FicrecDbContext())
                {
                    var detailsheet = p.Workbook.Worksheets["Fic info"];
                    var cols = new Dictionary<string, int>();
                    Console.WriteLine(detailsheet.Cells[1, 1]);
                }
            }
        }
    }
}
