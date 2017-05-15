using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;

public class ExcelFileResult : FileResult
{
    private object _model;
    Dictionary<string, string> _dicExpoModels;
    public ExcelFileResult(object model, Dictionary<string, string> dicExpoModels) :
        base(new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
    {
        _model = model;
        _dicExpoModels = dicExpoModels;
    }

    protected override Task WriteFileAsync(HttpResponse response)
    {
        var enumerable = _model as System.Collections.IEnumerable;
        if (enumerable == null)
        {
            throw new ArgumentException("IEnumerable type required");
        }

        byte[] FileContents = null;
        using (MemoryStream mem = new MemoryStream())
        {
            using (var workbook = SpreadsheetDocument.Create(mem, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                uint sheetId = 1;
                if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                {
                    sheetId = sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }

                DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = "Sheet1" };
                sheets.Append(sheet);

                DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                List<String> columns = new List<string>();
                //var properties = typeof(AllergyViewModel).GetProperties();
                var properties = enumerable.GetType().GetGenericArguments()[0].GetProperties();
                properties = properties.Where(m => _dicExpoModels.Keys.Contains(m.Name)).ToArray();
                foreach (var property in properties)
                {
                    string str = _dicExpoModels[property.Name];
                    columns.Add(str);

                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(str);
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                foreach (var item in enumerable)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    foreach (var header in properties)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;

                        var value = header.GetValue(item);
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(value?.ToString()); //
                        newRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(newRow);
                }

                sheetPart.Worksheet.Save();
                workbook.WorkbookPart.Workbook.Save();
                workbook.Close();
                FileContents = mem.ToArray();
                return response.Body.WriteAsync(FileContents, 0, FileContents.Length);
            }
        }

    }
}