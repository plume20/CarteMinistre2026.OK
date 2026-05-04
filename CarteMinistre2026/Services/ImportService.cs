using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Data;
using System.IO;
using CarteMinistre2026.Models;

namespace CarteMinistre2026.Services
{
    public class ImportService
    {
        public List<Employee> ImportFromFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();

            if (extension == ".xlsx" || extension == ".xls")
                return ImportFromExcel(filePath);
            else if (extension == ".csv")
                return ImportFromCsv(filePath);
            else if (extension == ".ods")
                return ImportFromOds(filePath);
            else
                throw new NotSupportedException($"Le format '{extension}' n'est pas supporté.");
        }

        private List<Employee> ImportFromExcel(string filePath)
        {
            var employees = new List<Employee>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++)
                {
                    var emp = new Employee
                    {
                        LastName = worksheet.Cells[row, 1].Text,
                        PostName = worksheet.Cells[row, 2].Text,
                        FirstName = worksheet.Cells[row, 3].Text,
                        BirthPlace = worksheet.Cells[row, 4].Text,
                        BirthDate = ParseDate(worksheet.Cells[row, 5].Text),
                        Ministry = worksheet.Cells[row, 6].Text,
                        JobTitle = worksheet.Cells[row, 7].Text,
                        OrdinationDate = ParseDate(worksheet.Cells[row, 8].Text),
                        IssueDate = DateTime.Now,
                        ExpiryDate = DateTime.Now.AddYears(2)
                    };
                    employees.Add(emp);
                }
            }
            return employees;
        }

        private List<Employee> ImportFromCsv(string filePath)
        {
            var employees = new List<Employee>();
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');

                var emp = new Employee
                {
                    LastName = values.Length > 0 ? values[0] : "",
                    PostName = values.Length > 1 ? values[1] : "",
                    FirstName = values.Length > 2 ? values[2] : "",
                    BirthPlace = values.Length > 3 ? values[3] : "",
                    BirthDate = ParseDate(values.Length > 4 ? values[4] : ""),
                    Ministry = values.Length > 5 ? values[5] : "",
                    JobTitle = values.Length > 6 ? values[6] : "",
                    OrdinationDate = ParseDate(values.Length > 7 ? values[7] : ""),
                    IssueDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddYears(2)
                };
                employees.Add(emp);
            }
            return employees;
        }

        private List<Employee> ImportFromOds(string filePath)
        {
            // Méthode alternative sans OdsReaderWriter
            // Un fichier ODS est un ZIP contenant content.xml
            var employees = new List<Employee>();

            try
            {
                using (var zip = System.IO.Compression.ZipFile.OpenRead(filePath))
                {
                    var contentEntry = zip.GetEntry("content.xml");
                    if (contentEntry != null)
                    {
                        using (var stream = contentEntry.Open())
                        {
                            var doc = new System.Xml.XmlDocument();
                            doc.Load(stream);

                            var nsManager = new System.Xml.XmlNamespaceManager(doc.NameTable);
                            nsManager.AddNamespace("table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
                            nsManager.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");

                            var rows = doc.SelectNodes("//table:table-row", nsManager);

                            if (rows != null && rows.Count > 1)
                            {
                                for (int i = 1; i < rows.Count; i++)
                                {
                                    var cells = rows[i].SelectNodes("table:table-cell/text:p", nsManager);
                                    if (cells != null && cells.Count >= 6)
                                    {
                                        var emp = new Employee
                                        {
                                            LastName = cells[0]?.InnerText ?? "",
                                            PostName = cells.Count > 1 ? cells[1]?.InnerText ?? "" : "",
                                            FirstName = cells.Count > 2 ? cells[2]?.InnerText ?? "" : "",
                                            BirthPlace = cells.Count > 3 ? cells[3]?.InnerText ?? "" : "",
                                            BirthDate = ParseDate(cells.Count > 4 ? cells[4]?.InnerText ?? "" : ""),
                                            Ministry = cells.Count > 5 ? cells[5]?.InnerText ?? "" : "",
                                            JobTitle = cells.Count > 6 ? cells[6]?.InnerText ?? "" : "",
                                            OrdinationDate = ParseDate(cells.Count > 7 ? cells[7]?.InnerText ?? "" : ""),
                                            IssueDate = DateTime.Now,
                                            ExpiryDate = DateTime.Now.AddYears(2)
                                        };
                                        employees.Add(emp);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Impossible de lire le fichier ODS. Vérifiez le format.");
            }

            return employees;
        }

        private DateTime? ParseDate(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            if (DateTime.TryParse(text, out DateTime result))
                return result;

            return null;
        }
    }
}