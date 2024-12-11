using System.Text;

namespace DevelopsTodayTestTask;

public class CsvDataDumper(IEnumerable<CabDataDto> duplicates)
{
    private readonly Dictionary<string, int> _columnNameToIndex = new();

    private string CreateHeader()
    {
        var headerBuilder = new StringBuilder();

        var properties = typeof(CabDataDto).GetProperties();
        for (var i = 0; i < properties.Length; i++)
        {
            headerBuilder.Append(properties[i].Name);
            _columnNameToIndex.Add(properties[i].Name, i);

            if (i != properties.Length - 1)
            {
                headerBuilder.Append(',');
            }
        }

        return headerBuilder.ToString();
    }

    public void Dump(string csvFilePath)
    {
        using var file = File.Open(csvFilePath, FileMode.Create);

        var header = CreateHeader();
        file.Write(Encoding.ASCII.GetBytes(header));
        file.Write(Encoding.ASCII.GetBytes(Environment.NewLine));

        var lineColumns = new string[typeof(CabDataDto).GetProperties().Length];
        foreach (var duplicate in duplicates)
        {
            foreach (var columnToIndexPair in _columnNameToIndex)
            {
                var property = typeof(CabDataDto).GetProperty(columnToIndexPair.Key);
                lineColumns[columnToIndexPair.Value] = property!.GetValue(duplicate)!.ToString()!;
            }

            var lineBuilder = new StringBuilder();
            for (var i = 0; i < lineColumns.Length; i++)
            {
                var lineColumn = lineColumns[i];
                lineBuilder.Append(lineColumn);
                if (i != lineColumns.Length - 1)
                {
                    lineBuilder.Append(',');
                }
            }

            file.Write(Encoding.ASCII.GetBytes(lineBuilder.ToString()));
            file.Write(Encoding.ASCII.GetBytes(Environment.NewLine));
        }
    }
}
