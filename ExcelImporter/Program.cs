using System;
using System.Collections.Generic;
using System.IO;
using FicRecs.DatabaseLib;
using OfficeOpenXml;

namespace FicRecs.ExcelImporter
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
                using (var context = new FicRecsDbContext())
                {
                    var detailsheet = p.Workbook.Worksheets["Fic Info"];
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
                    var validids = new HashSet<int>();
                    
                    int stories = 0;
                    for (int row = 2; !String.IsNullOrWhiteSpace(GetCol<string>(row, "ID")); row++, stories++)
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
                            Reviews = GetCol<int>(row, "Review"),
                            Favs = GetCol<int>(row, "Favs"),
                            Follows = GetCol<int>(row, "Follows"),
                            Published = GetCol<DateTime>(row, "Published"),
                            Url = GetCol<string>(row, "URL")
                        };
                        var complete = GetCol<string>(row, "Complete");
                        if (complete == "Complete")
                            story.Complete = true;
                        else if (complete == "Incomplete")
                            story.Complete = false;
                        else
                            story.Complete = null;
                        
                        context.StoryDetails.Add(story);
                        
                        rowidmap[row - 1] = story.StoryId;
                        validids.Add(story.StoryId);

                        Console.Write($"\rImported info row {row - 1}, id {story.StoryId}");
                    }
                    Console.WriteLine($"\rImported {stories} stories");

                    var weightsheet = p.Workbook.Worksheets["Weights"];
                    var idsheet = p.Workbook.Worksheets["Nearest IDs"];
                    for (int row = 1, col = 1; weightsheet.Cells[row, col].Value != null; row++, col = 1)
                    {
                        for (; weightsheet.Cells[row, col].Value != null; col++)
                        {
                            try
                            {
                                var matrix = new StoryMatrix()
                                {
                                    StoryA = rowidmap[row],
                                    StoryB = idsheet.Cells[row, col].GetValue<int>(),
                                    Similarity = weightsheet.Cells[row, col].GetValue<float>()
                                };

                                Console.Write($"\rImported rec row {row}, col {col}");

                                if (!validids.Contains(matrix.StoryB))
                                {
                                    Console.WriteLine($"\rNo fic info for {matrix.StoryB}, ignoring");
                                    continue;
                                }

                                context.StoryMatrix.Add(matrix);
                            }
                            catch (InvalidCastException)
                            {
                                if (weightsheet.Cells[row, col].GetValue<ExcelErrorValue>().Type == eErrorType.Num)
                                {
                                    continue;
                                }
                                else
                                {
                                    Console.WriteLine("\r[{0}] ({1}) {2}",
                                        weightsheet.Cells[row, col].Value,
                                        weightsheet.Cells[row, col].Value.GetType(),
                                        weightsheet.Cells[row, col].Value.ToString());
                                    throw;
                                }
                            }
                        }
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
