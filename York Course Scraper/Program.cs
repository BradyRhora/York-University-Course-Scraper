using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Data.SQLite;

namespace York_Course_Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] Faculties = { "AP", "ED", "ES", "FA", "GL", "GS", "HH", "LE", "LW", "SB", "SC" };
            HtmlWeb web = new HtmlWeb();
            foreach(var fac in Faculties)
            {
                string url = $"https://apps1.sis.yorku.ca/WebObjects/cdm.woa/Contents/WebServerResources/FW2019{fac}.html";

                Console.Write($"Loading {url}... ");
                var page = web.Load(url);
                Console.WriteLine("Loaded.");
                var table = page.DocumentNode.SelectSingleNode("/html[1]/body[1]/table[1]");
                var rows = table.ChildNodes.Where(x => x.Name == "tr").ToArray();
                for(int i = 1; i < rows.Count(); i++)
                {
                    try
                    {
                        if (rows[i].ChildNodes[1].InnerText == "&nbsp;") continue;
                        var dep = rows[i].ChildNodes[3].InnerText;
                        var title = rows[i].ChildNodes[7].InnerText;
                        i++;
                        var data = rows[i].ChildNodes[3].InnerText.Split(new string[] { "&nbsp;" }, StringSplitOptions.RemoveEmptyEntries);
                        string level = data[0].Trim(' ');
                        double credit = Convert.ToDouble(data[1]);
                        string loi = rows[i].ChildNodes[5].InnerText;
                        string type = rows[i].ChildNodes[7].InnerText.Replace("&nbsp;", "");

                        using (var con = new SQLiteConnection(Constants.CONNECTION_STRING))
                        {
                            con.Open();
                            var stm = "INSERT INTO COURSES(FACULTY, DEPARTMENT, LEVEL, CODE, CREDIT, TITLE, LANGUAGE, TYPE) " +
                                "VALUES(@fac, @dep, @lvl, @code, @credit, @title, @lang, @type)";
                            using (var cmd = new SQLiteCommand(stm, con))
                            {
                                cmd.Parameters.AddWithValue("@fac", fac);
                                cmd.Parameters.AddWithValue("@dep", dep);
                                cmd.Parameters.AddWithValue("@lvl", level);
                                cmd.Parameters.AddWithValue("@code", dep + " " + level);
                                cmd.Parameters.AddWithValue("@credit", credit);
                                cmd.Parameters.AddWithValue("@title", title);
                                cmd.Parameters.AddWithValue("@lang", loi);
                                cmd.Parameters.AddWithValue("@type", type);

                                Console.Write($"Adding {dep} {level}... ");
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Success.");
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Failed");
                                }

                            }
                        }
                    } catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }
            Console.WriteLine("Done. Press any button to close the program.");
            Console.ReadKey();
        }
    }
}
