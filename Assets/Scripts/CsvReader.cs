using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class CsvReader : MonoBehaviour
{
    public string resourceName = "Questions";

    private void Start()
    {
        var csvFile = Resources.Load<TextAsset>(resourceName);
        var csv = ReadCsv(csvFile.text);

        Debug.Log(JsonConvert.SerializeObject(csv, Formatting.Indented));
    }

    public static CsvDocument ReadCsv(string text)
    {
        var lines = text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0)
        {
            return new CsvDocument();
        }

        var headers = CsvFields(lines[0]).ToArray();
        var rows = new List<CsvRow>(lines.Length-1);

        foreach (var line in lines.Skip(1))
        {
            var values = CsvFields(line).ToArray();
            var pairs = new CsvCell[Mathf.Max(values.Length, headers.Length)];
            for (var i = 0; i < pairs.Length; i++)
            {
                string col = null, val = null;
                if (i < headers.Length)
                {
                    col = headers[i];
                }

                if (i < values.Length)
                {
                    val = values[i];
                }

                pairs[i] = new CsvCell(col,val);
            }
            rows.Add(new CsvRow(pairs));
        }

        return new CsvDocument
        {
            Headers = headers,
            Rows = rows.ToArray(),
        };
    }

    private static IEnumerable<string> CsvFields(string line)
    {
        var sb = new StringBuilder();
        var hasQuote = false;
        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];
            if (c == '"')
            {
                if (sb.Length == 0)
                {
                    hasQuote = true;
                    continue;
                }

                if (i < line.Length - 1 && line[i + 1] == '"')
                {
                    sb.Append(c);
                    i++; // skip the next quote
                    continue;
                }

                hasQuote = false;
                continue;
            }
            else if (c == ',' && !hasQuote)
            {
                yield return sb.ToString();
                sb.Clear();
                hasQuote = false;
                continue;
            }

            sb.Append(c);
        }

        if (sb.Length > 0)
        {
            yield return sb.ToString();
        }
    }
}

public class CsvDocument
{
    public string[] Headers { get; set; } = Array.Empty<string>();
    public CsvRow[] Rows { get; set; } = Array.Empty<CsvRow>();
}

public struct CsvRow
{
    public CsvCell[] Cells { get; set; }

    public CsvRow(CsvCell[] cells)
    {
        Cells = cells;
    }
}

public struct CsvCell
{
    public string Header { get; set; }
    public string Value { get; set; }

    public CsvCell(string header, string value)
    {
        Header = header;
        Value = value;
    }
}
