
using System;
using System.Globalization;
using System.Linq;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;

namespace demo_project.app
{
    public static class StringExtensionExcel
    {
        public static string GetHyperlinkUrl(WorkbookPart workbookPart, string relationshipId)
        {
            HyperlinkRelationship hyperlinkRelationship = workbookPart.HyperlinkRelationships.FirstOrDefault(hr => hr.Id == relationshipId);
            return hyperlinkRelationship?.Uri.AbsoluteUri;
        }

        public static string GetStringOrEmpty(this IXLCell cell)
        {
            return cell.Value.ToString().Trim();
        }
        public static string? ToStringIgnoreRef(this string? value)
        {
            if (value == null)
            {
                return "";
            }
            value = value.Trim(' ');
            if (value == "#REF!")
            {
                return "";
            }

            return value;
        }

        public static string? ToDateString(this string? value)
        {
            if (value == null)
            {
                return "";
            }
            value = value.Trim(' ');
            value = value.Replace(" ", "");
            value = value.Replace($"\n", "");

            return value;
        }

        public static bool TryGetDateTime(this XLCellValue value, string[] formats, out DateTime? result)
        {
            if (value.IsDateTime)
            {
                result = value.GetDateTime();
                return true;
            }
            else
            {
                var hasDate = DateTime.TryParseExact(value.ToString().ToDateString(), formats,
                    new CultureInfo("vi-VN"),
                    DateTimeStyles.None,
                    out var getDate);
                if (hasDate)
                {
                    result = getDate;
                }
                else
                {
                    result = null;
                }
                return hasDate;
            }
        }

        public static string TrimS(this string? value)
        {
            if (value == null)
            {
                return "";
            }
            value = value.Trim(' ');
            value = value.Replace(" ", "");
            value = value.Replace($"\n", "");
            value = value.Replace($"\"", "");
            value = value.Replace($"\'", "");

            return value;
        }

        public static IXLCell CellFromMerge(this IXLWorksheet worksheet, int row, int column)
        {
            var cell = worksheet.Cell(row, column);

            if (cell.IsMerged())
            {
                var valueCell = worksheet.MergedRanges.FirstOrDefault(r => r.Contains(cell))?.FirstCell();
                return valueCell ?? cell;
            }
            else
            {
                return cell;
            }
        }

        public static IXLCell CellFromSelectMerge(this IXLWorksheet worksheet, int row, int column, int columnSelect)
        {
            var cellSelect = worksheet.Cell(row, columnSelect);
            var cell = worksheet.Cell(row, column);

            if (cellSelect.IsMerged())
            {
                var valueCell = worksheet.MergedRanges.FirstOrDefault(r => r.Contains(cellSelect))?.FirstCell();
                var cellNew = valueCell?.WorksheetRow().Cell(column);
                return cellNew ?? cell;
            }
            else
            {
                return cell;
            }
        }

        public static IXLCell CellTerritory(this IXLWorksheet worksheet, int row, int column, XLWorkbook a)
        {
            var cell = worksheet.Cell(row, column);

            if (cell.IsEmpty())
            {
                var worksheet_2 = a.Worksheet(2);
                cell = worksheet_2.Cell(4, 7);
            }

            return cell;
        }

        public static int CellIntOrZero(this IXLCell cell)
        {
            if (cell.TryGetValue(out int value))
            {
                return value;
            }
            return 0;
        }

        public static double CellDoubleOrZero(this IXLCell cell)
        {
            if (cell.TryGetValue(out double value))
            {
                return value;
            }
            return 0;
        }

        public static long CellLongOrZero(this IXLCell cell)
        {
            if (cell.TryGetValue(out long value))
            {
                return value;
            }
            return 0;
        }

        public static decimal CellDecimalOrZero(this IXLCell cell)
        {
            if (cell.TryGetValue(out decimal value))
            {
                return value;
            }
            return 0;
        }

        public static bool CellBoolOrFalse(this IXLCell cell)
        {
            // var a = cell.GetString();
            var stringBool = cell.GetString().Trim();
            // bool result = false;
            if (!string.IsNullOrWhiteSpace(stringBool))
            {
                if (stringBool == "1")
                {
                    // result = true;
                    return true;
                }
            }
            // bool result = !string.IsNullOrWhiteSpace(cell.GetString());
            // return result;
            return false;
        }

        public static string CellGenderKey(this IXLCell cell)
        {
            if (cell.TryGetValue(out int value))
            {
                if (value == 0) return "core.gender.female"; // female
                if (value == 1) return "core.gender.male"; // male
            }

            // others → default female
            return "core.gender.others";
        }
    }
}