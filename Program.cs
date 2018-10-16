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
                    for (int col = 1; detailsheet.Cells[1, col].Value != null; col++)
                    {
                        colnames[detailsheet.Cells[1, col].GetValue<string>()] = col;
                    }

                    T GetCol<T>(int row, string colname)
                    {
                        return detailsheet.Cells[row, colnames[colname]].GetValue<T>();
                    }

                    var rowidmap = new Dictionary<int, int>();
                    
                    for (int row = 2; !String.IsNullOrWhiteSpace(GetCol<string>(row, "ID")); row++)
                    {
                        var story = new StoryDetails
                        {
                            StoryId = GetCol<int>(row, "ID"),
                            Title = GetCol<string>(row, "Title"),
                            Author = GetCol<string>(row, "Author"),
                            Summary = GetCol<string>(row, "Summary"),
                            Characters = GetCol<string>(row, "Characters"),
                            Chapters = GetCol<short>(row, "Chapters"),
                            Words = GetCol<int>(row, "Words"),
                            Reviews = GetCol<int>(row, "Reviews"),
                            Favs = GetCol<int>(row, "Favs"),
                            Follows = GetCol<int>(row, "Follows"),
                            Published = GetCol<DateTime>(row, "Published"),
                            Url = GetCol<string>(row, "Url")
                        };
                        var complete = GetCol<string>(row, "Complete");
                        if (complete == "Complete")
                            story.Complete = true;
                        else if (complete == "Incomplete")
                            story.Complete = false;
                        else
                            story.Complete = null;

                        Console.WriteLine(story.StoryId);
                        
                        rowidmap[GetCol<int>(row, "Row")] = GetCol<int>(row, "ID");
                    }

                    var matrixsheet = p.Workbook.Worksheets["Adjacency matrix"];
                    for (int row = 1, col = 1; matrixsheet.Cells[row, col].Value != null; row++, col = 1)
                    {
                        for (; matrixsheet.Cells[row, col].Value != null; col++)
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
