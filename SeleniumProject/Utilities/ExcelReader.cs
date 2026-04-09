using System.Data;
using ExcelDataReader;

namespace SeleniumProject.Utilities
{
    public static class ExcelReader
    {
        public static DataTable GetSheet(string filePath, string sheetName)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);

            var result = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            });

            if (!result.Tables.Contains(sheetName))
            {
                throw new Exception($"Không tìm thấy sheet: {sheetName}");
            }

            return result.Tables[sheetName]!;
        }
    }
}