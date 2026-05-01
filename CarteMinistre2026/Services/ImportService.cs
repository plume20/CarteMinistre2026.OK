using CarteMinistre2026.Models;
using ODSReaderWriter;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Zaretto.ODS;

namespace CarteMinistre2026.Services
{
    public class ImportService
    {
        public List<Employee> ImportFromFile(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();

            DataTable dataTable = new DataTable();

            if (extension == ".xlsx" || extension == ".xls")
            {
                dataTable = ImportFromExcel(filePath);
            }
            else if (extension == ".csv")
            {
                dataTable = ImportFromCsv(filePath);
            }
            else if (extension == ".ods")
            {
                dataTable = ImportFromOds(filePath);
            }
            else
            {
                throw new NotSupportedException($"Le format '{extension}' n'est pas supporté.");
            }

            return MapDataTableToEmployees(dataTable);
        }

        private DataTable ImportFromExcel(string filePath)
        {
            DataTable table = new DataTable();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                // Ajouter les colonnes
                for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                {
                    table.Columns.Add(worksheet.Cells[1, col].Text);
                }

                // Ajouter les lignes (à partir de la ligne 2)
                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    DataRow dataRow = table.NewRow();
                    for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                    {
                        dataRow[col - 1] = worksheet.Cells[row, col].Text;
                    }
                    table.Rows.Add(dataRow);
                }
            }

            return table;
        }

        private DataTable ImportFromCsv(string filePath)
        {
            DataTable table = new DataTable();
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length == 0) return table;

            // Première ligne = en-têtes
            string[] headers = lines[0].Split(',');
            foreach (string header in headers)
            {
                table.Columns.Add(header.Trim());
            }

            // Lignes suivantes = données
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                DataRow row = table.NewRow();
                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    row[j] = values[j].Trim();
                }
                table.Rows.Add(row);
            }

            return table;
        }

        private DataTable ImportFromOds(string filePath)
        {
            var odsReader = new ODSReaderWriter();
            var dataSet = odsReader.ReadOdsFile(filePath);

            if (dataSet.Tables.Count > 0)
                return dataSet.Tables[0];
            else
                return new DataTable();
        }

        private List<Employee> MapDataTableToEmployees(DataTable table)
        {
            var employees = new List<Employee>();

            foreach (DataRow row in table.Rows)
            {
                var emp = new Employee
                {
                    LastName = GetValue(row, 0),
                    PostName = GetValue(row, 1),
                    FirstName = GetValue(row, 2),
                    BirthPlace = GetValue(row, 3),
                    BirthDate = ParseDate(GetValue(row, 4)),
                    Ministry = GetValue(row, 5),
                    JobTitle = GetValue(row, 6),
                    OrdinationDate = ParseDate(GetValue(row, 7)),
                    IssueDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddYears(2)
                };

                employees.Add(emp);
            }

            return employees;
        }

        private string GetValue(DataRow row, int index)
        {
            if (row.Table.Columns.Count > index && row[index] != null)
                return row[index].ToString();
            return "";
        }

        private DateTime? ParseDate(string text)
        {
            if (DateTime.TryParse(text, out DateTime result))
                return result;
            return null;
        }
    }
}