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
                    var colnames = new Dictionary<string, int>();
                    for (int col = 1; detailsheet.Cells[col, 1].Value != null; col++)
                    {
                        colnames[detailsheet.Cells[col, 1].GetValue<string>()] = col;
                    }

                    T GetCol<T>(string colname, int row)
                    {
                        return detailsheet.Cells[colnames[colname], row].GetValue<T>();
                    }

                    var rowidmap = new Dictionary<int, int>();
                    
                    for (int row = 2; !String.IsNullOrWhiteSpace(GetCol<string>("ID", row)); row++)
                    {
                        var story = new StoryDetails
                        {
                            StoryId = GetCol<int>("ID", row),
                            Title = GetCol<string>("Title", row),
                            Author = GetCol<string>("Author", row),
                            Summary = GetCol<string>("Summary", row),
                            Characters = GetCol<string>("Characters", row),
                            Chapters = GetCol<short>("Chapters", row),
                            Words = GetCol<int>("Words", row),
                            Reviews = GetCol<int>("Reviews", row),
                            Favs = GetCol<int>("Favs", row),
                            Follows = GetCol<int>("Follows", row),
                            Published = GetCol<DateTime>("Published", row),
                            Url = GetCol<string>("Url", row)
                        };
                        var complete = GetCol<string>("Complete", row);
                        if (complete == "Complete")
                            story.Complete = true;
                        else if (complete == "Incomplete")
                            story.Complete = false;
                        else
                            story.Complete = null;

                        Console.WriteLine(story.StoryId);
                        
                        rowidmap[GetCol<int>("Row", row)] = GetCol<int>("ID", row);
                    }

                    var matrixsheet = p.Workbook.Worksheets["Adjacency matrix"];
                    for (int row = 1, col = 1; matrixsheet.Cells[col, row].Value != null; row++, col = 1)
                    {
                        for (; matrixsheet.Cells[col, row].Value != null; col++)
                        {
                            var matrix = new StoryMatrix()
                            {
                                StoryA = rowidmap[row],
                                StoryB = rowidmap[col],
                                Similarity = matrixsheet.Cells[col, row].GetValue<float>()
                            };

                            Console.WriteLine("{0} {1} {2}", matrix.StoryA, matrix.StoryB, matrix.Similarity);
                        }
                    }
                }
            }
        }
    }
}
