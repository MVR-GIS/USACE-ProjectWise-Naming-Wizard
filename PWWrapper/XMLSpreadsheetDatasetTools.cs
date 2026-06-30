using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;

public class XMLSpreadsheetDatasetTools
{
    private const string XML_HEADER = "<?xml version=\"1.0\"?><?mso-application progid=\"Excel.Sheet\"?><Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:html=\"http://www.w3.org/TR/REC-html40\">";
    private const string XML_HEADER1 = "<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\"><LastAuthor></LastAuthor><Created></Created><Version>11.6568</Version></DocumentProperties><ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\"><WindowHeight>12525</WindowHeight><WindowWidth>18075</WindowWidth><WindowTopX>0</WindowTopX>";
    private const string XML_HEADER2 = "<WindowTopY>15</WindowTopY><ProtectStructure>False</ProtectStructure><ProtectWindows>False</ProtectWindows></ExcelWorkbook><Styles><Style ss:ID=\"Default\" ss:Name=\"Normal\"><Alignment ss:Vertical=\"Bottom\"/><Borders/><Font/><Interior/><NumberFormat/><Protection/></Style><Style ss:ID=\"s22\"><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\"/><Font x:Family=\"Swiss\" ss:Bold=\"1\"/></Style><Style ss:ID=\"s23\"><Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Bottom\"/><Font x:Family=\"Swiss\" ss:Bold=\"1\"/></Style></Styles>";
    private const string XML_WORKSHEETHEADER = "<Worksheet ss:Name=\"{0}\"><Table x:FullColumns=\"1\" x:FullRows=\"1\">\n";
    private const string XML_WORKSHEETFOOTER = "</Table><WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\"><Selected/><ProtectObjects>False</ProtectObjects><ProtectScenarios>False</ProtectScenarios></WorksheetOptions></Worksheet>";
    private const string XML_WORKBOOKFOOTER = "</Workbook>";
    private const string XML_ROW_FORMAT = "<Row><Cell ss:StyleID=\"s22\"><Data ss:Type=\"{2}\">{0}</Data></Cell><Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">{1}</Data></Cell></Row>";
    private const string XML_HEADER_URL = "<?xml version=\"1.0\"?><?mso-application progid=\"Excel.Sheet\"?><Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:html=\"http://www.w3.org/TR/REC-html40\">";
    private const string XML_HEADER1_URL = "<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\"><LastAuthor></LastAuthor><Created></Created><Version>11.6568</Version></DocumentProperties><ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\"><WindowHeight>12525</WindowHeight><WindowWidth>18075</WindowWidth><WindowTopX>0</WindowTopX>";
    private const string XML_HEADER2_URL = "<WindowTopY>15</WindowTopY><ProtectStructure>False</ProtectStructure><ProtectWindows>False</ProtectWindows></ExcelWorkbook><Styles><Style ss:ID=\"Default\" ss:Name=\"Normal\"><Alignment ss:Vertical=\"Bottom\"/><Borders/><Font/><Interior/><NumberFormat/><Protection/></Style><Style ss:ID=\"s22\"><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\"/><Font x:Family=\"Swiss\" ss:Bold=\"1\"/></Style><Style ss:ID=\"s23\"><Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Bottom\"/><Font x:Family=\"Swiss\" ss:Bold=\"1\"/></Style>";
    private const string XML_HEADER3_URL = "<Style ss:ID=\"s63\" ss:Name=\"Hyperlink\"><Font ss:FontName=\"Arial\" ss:Color=\"#0000FF\" ss:Underline=\"Single\"/></Style><Style ss:ID=\"s62\"><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\"/><Font ss:FontName=\"Arial\" x:Family=\"Swiss\" ss:Bold=\"1\"/></Style><Style ss:ID=\"s64\" ss:Parent=\"s63\"><Alignment ss:Vertical=\"Bottom\"/><Protection/></Style></Styles>";
    private const string XML_WORKSHEETHEADER_URL = "<Worksheet ss:Name=\"{0}\"><Table x:FullColumns=\"1\" x:FullRows=\"1\">\n";
    private const string XML_WORKSHEETFOOTER_URL = "</Table><WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\"><Selected/><ProtectObjects>False</ProtectObjects><ProtectScenarios>False</ProtectScenarios></WorksheetOptions></Worksheet>";
    private const string XML_WORKBOOKFOOTER_URL = "</Workbook>";
    private const string XML_ROW_FORMAT_URL = "<Row><Cell ss:StyleID=\"s22\"><Data ss:Type=\"{2}\">{0}</Data></Cell><Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">{1}</Data></Cell></Row>";

    private static string CleanUpString(string sInString)
    {
        string str = (string)sInString.Clone();
        string str2 = "";
        if (!str.Contains("&amp;"))
        {
            str2 = str.Replace("&", "&amp;");
        }
        return str2.Replace(">", "&gt;").Replace("<", "&lt;").Replace("\"", "&quot;").Replace("'", "&apos;");
    }

    private static string getColumnType(DataColumn dc)
    {
        string str2;
        if (((str2 = dc.DataType.ToString()) != null) && (((str2 == "System.UInt64") || (str2 == "System.UInt32")) || (((str2 == "System.Int64") || (str2 == "System.Double")) || (str2 == "System.Int32"))))
        {
            return "Number";
        }
        return "String";
    }

    private static void ReadDataSet(string sXmlPath, ref DataSet ds, bool bReadURLs)
    {
        int num = 0;
        if (File.Exists(sXmlPath))
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(sXmlPath);
                int num2 = -1;
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    if (node.Name == "Worksheet")
                    {
                        num2++;
                        foreach (XmlNode node2 in node.ChildNodes)
                        {
                            if (node2.Name == "Table")
                            {
                                num = 0;
                                foreach (XmlNode node3 in node2.ChildNodes)
                                {
                                    if (node3.Name == "Row")
                                    {
                                        switch (num)
                                        {
                                            case 0:
                                                num++;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }

    public static DataSet ReadDataSetFromXMLSpreadsheet(string sXmlPath)
    {
        DataSet ds = ReadXmlFileColumnNames(sXmlPath);
        ReadDataSet(sXmlPath, ref ds, false);
        return ds;
    }

    public static DataSet ReadDataSetFromXMLSpreadsheetWithURLs(string sXmlPath)
    {
        DataSet ds = ReadXmlFileColumnNames(sXmlPath);
        ReadDataSet(sXmlPath, ref ds, true);
        return ds;
    }

    private static DataSet ReadXmlFileColumnNames(string sXmlPath)
    {
        DataSet set = new DataSet();
        if (File.Exists(sXmlPath))
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(sXmlPath);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    if ((node.Name == "Worksheet") && (node.Attributes["ss:Name"] != null))
                    {
                        DataTable table = new DataTable(node.Attributes["ss:Name"].Value);
                        set.Tables.Add(table);
                        foreach (XmlNode node2 in node.ChildNodes)
                        {
                            if (node2.Name == "Table")
                            {
                                foreach (XmlNode node3 in node2.ChildNodes)
                                {
                                    if (node3.Name == "Row")
                                    {
                                        if (node3.HasChildNodes)
                                        {
                                            foreach (XmlNode node4 in node3.ChildNodes)
                                            {
                                                if (node4.Name == "Cell")
                                                {
                                                    string str3 = node4.InnerText.Replace("\r", "_").Replace("\n", "_").Replace(" ", "_");
                                                    if (!string.IsNullOrEmpty(str3))
                                                    {
                                                        DataColumn column = new DataColumn
                                                        {
                                                            DataType = Type.GetType("System.String"),
                                                            ColumnName = str3
                                                        };
                                                        table.Columns.Add(column);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
        return set;
    }

    public static bool WriteDatasetToXMLSpreadsheet(DataSet ds, string sXMLFile)
    {
        using (StreamWriter writer = new StreamWriter(sXMLFile, false, Encoding.Unicode))
        {
            writer.Write("<?xml version=\"1.0\"?><?mso-application progid=\"Excel.Sheet\"?><Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:html=\"http://www.w3.org/TR/REC-html40\">");
            writer.Write("<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\"><LastAuthor></LastAuthor><Created></Created><Version>11.6568</Version></DocumentProperties><ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\"><WindowHeight>12525</WindowHeight><WindowWidth>18075</WindowWidth><WindowTopX>0</WindowTopX>");
            writer.Write("<WindowTopY>15</WindowTopY><ProtectStructure>False</ProtectStructure><ProtectWindows>False</ProtectWindows></ExcelWorkbook><Styles><Style ss:ID=\"Default\" ss:Name=\"Normal\"><Alignment ss:Vertical=\"Bottom\"/><Borders/><Font/><Interior/><NumberFormat/><Protection/></Style><Style ss:ID=\"s22\"><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\"/><Font x:Family=\"Swiss\" ss:Bold=\"1\"/></Style><Style ss:ID=\"s23\"><Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Bottom\"/><Font x:Family=\"Swiss\" ss:Bold=\"1\"/></Style></Styles>");
            foreach (DataTable table in ds.Tables)
            {
                writer.Write(string.Format("<Worksheet ss:Name=\"{0}\"><Table x:FullColumns=\"1\" x:FullRows=\"1\">\n", table.TableName));
                writer.WriteLine("<Row>");
                ArrayList list = new ArrayList();
                foreach (DataColumn column in table.Columns)
                {
                    writer.WriteLine("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">{0}</Data></Cell>", CleanUpString(column.ColumnName));
                    list.Add(getColumnType(column));
                }
                writer.WriteLine("</Row>");
                foreach (DataRow row in table.Rows)
                {
                    writer.WriteLine("<Row>");
                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        writer.WriteLine("<Cell><Data ss:Type=\"{0}\">{1}</Data></Cell>", list[i], CleanUpString(row[i].ToString()));
                    }
                    writer.WriteLine("</Row>");
                }
                writer.Write("</Table><WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\"><Selected/><ProtectObjects>False</ProtectObjects><ProtectScenarios>False</ProtectScenarios></WorksheetOptions></Worksheet>");
            }
            writer.Write("</Workbook>");
        }
        return true;
    }

    public static bool WriteDatasetToXMLSpreadsheetWithURLs(DataSet ds, string sXMLFile)
    {
        using (StreamWriter writer = new StreamWriter(sXMLFile, false, Encoding.Unicode))
        {
            writer.Write("<?xml version=\"1.0\"?><?mso-application progid=\"Excel.Sheet\"?><Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:html=\"http://www.w3.org/TR/REC-html40\">");
            writer.Write("<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\"><LastAuthor></LastAuthor><Created></Created><Version>11.6568</Version></DocumentProperties><ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\"><WindowHeight>12525</WindowHeight><WindowWidth>18075</WindowWidth><WindowTopX>0</WindowTopX>");
            writer.Write("<WindowTopY>15</WindowTopY><ProtectStructure>False</ProtectStructure><ProtectWindows>False</ProtectWindows></ExcelWorkbook><Styles><Style ss:ID=\"Default\" ss:Name=\"Normal\"><Alignment ss:Vertical=\"Bottom\"/><Borders/><Font/><Interior/><NumberFormat/><Protection/></Style><Style ss:ID=\"s22\"><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\"/><Font x:Family=\"Swiss\" ss:Bold=\"1\"/></Style><Style ss:ID=\"s23\"><Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Bottom\"/><Font x:Family=\"Swiss\" ss:Bold=\"1\"/></Style>");
            writer.Write("<Style ss:ID=\"s63\" ss:Name=\"Hyperlink\"><Font ss:FontName=\"Arial\" ss:Color=\"#0000FF\" ss:Underline=\"Single\"/></Style><Style ss:ID=\"s62\"><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\"/><Font ss:FontName=\"Arial\" x:Family=\"Swiss\" ss:Bold=\"1\"/></Style><Style ss:ID=\"s64\" ss:Parent=\"s63\"><Alignment ss:Vertical=\"Bottom\"/><Protection/></Style></Styles>");
            foreach (DataTable table in ds.Tables)
            {
                writer.Write(string.Format("<Worksheet ss:Name=\"{0}\"><Table x:FullColumns=\"1\" x:FullRows=\"1\">\n", table.TableName));
                writer.WriteLine("<Row>");
                ArrayList list = new ArrayList();
                foreach (DataColumn column in table.Columns)
                {
                    writer.WriteLine("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">{0}</Data></Cell>", column.ColumnName);
                    list.Add(getColumnType(column));
                }
                writer.WriteLine("</Row>");
                foreach (DataRow row in table.Rows)
                {
                    writer.WriteLine("<Row>");
                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        try
                        {
                            if (row[i].ToString().StartsWith("pw://") || row[i].ToString().StartsWith(@"pw:\\"))
                            {
                                writer.WriteLine("<Cell ss:StyleID=\"s64\" ss:HRef=\"{1}\"><Data ss:Type=\"{0}\">{1}</Data></Cell>", list[i], row[i].ToString());
                            }
                            else if (row[i].ToString().StartsWith("http://") || row[i].ToString().StartsWith(@"http:\\"))
                            {
                                writer.WriteLine("<Cell ss:StyleID=\"s64\" ss:HRef=\"{1}\"><Data ss:Type=\"{0}\">{1}</Data></Cell>", list[i], row[i].ToString());
                            }
                            else if (row[i].ToString().StartsWith("file://") || row[i].ToString().StartsWith(@"file:\\"))
                            {
                                writer.WriteLine("<Cell ss:StyleID=\"s64\" ss:HRef=\"{1}\"><Data ss:Type=\"{0}\">{1}</Data></Cell>", list[i], row[i].ToString());
                            }
                            else if (row[i].ToString().StartsWith("ftp://") || row[i].ToString().StartsWith(@"ftp:\\"))
                            {
                                writer.WriteLine("<Cell ss:StyleID=\"s64\" ss:HRef=\"{1}\"><Data ss:Type=\"{0}\">{1}</Data></Cell>", list[i], row[i].ToString());
                            }
                            else
                            {
                                writer.WriteLine("<Cell><Data ss:Type=\"{0}\">{1}</Data></Cell>", list[i], row[i].ToString());
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    writer.WriteLine("</Row>");
                }
                writer.Write("</Table><WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\"><Selected/><ProtectObjects>False</ProtectObjects><ProtectScenarios>False</ProtectScenarios></WorksheetOptions></Worksheet>");
            }
            writer.Write("</Workbook>");
        }
        return true;
    }

    public static bool WriteDatasetToXMLSpreadsheetWithURLs2(DataSet ds, string sXMLFile)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(sXMLFile, false, Encoding.Unicode))
            {
                writer.Write("<?xml version=\"1.0\"?><?mso-application progid=\"Excel.Sheet\"?><Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:html=\"http://www.w3.org/TR/REC-html40\">");
                writer.Write("<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\"><LastAuthor></LastAuthor><Created></Created><Version>11.6568</Version></DocumentProperties><ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\"><WindowHeight>12525</WindowHeight><WindowWidth>18075</WindowWidth><WindowTopX>0</WindowTopX>");
                writer.Write("<WindowTopY>15</WindowTopY><ProtectStructure>False</ProtectStructure><ProtectWindows>False</ProtectWindows></ExcelWorkbook><Styles><Style ss:ID=\"Default\" ss:Name=\"Normal\"><Alignment ss:Vertical=\"Bottom\"/><Borders/><Font/><Interior/><NumberFormat/><Protection/></Style><Style ss:ID=\"s22\"><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\"/><Font x:Family=\"Swiss\" ss:Bold=\"1\"/></Style><Style ss:ID=\"s23\"><Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Bottom\"/><Font x:Family=\"Swiss\" ss:Bold=\"1\"/></Style>");
                writer.Write("<Style ss:ID=\"s63\" ss:Name=\"Hyperlink\"><Font ss:FontName=\"Arial\" ss:Color=\"#0000FF\" ss:Underline=\"Single\"/></Style><Style ss:ID=\"s62\"><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\"/><Font ss:FontName=\"Arial\" x:Family=\"Swiss\" ss:Bold=\"1\"/></Style><Style ss:ID=\"s64\" ss:Parent=\"s63\"><Alignment ss:Vertical=\"Bottom\"/><Protection/></Style></Styles>");
                foreach (DataTable table in ds.Tables)
                {
                    try
                    {
                        writer.Write(string.Format("<Worksheet ss:Name=\"{0}\"><Table x:FullColumns=\"1\" x:FullRows=\"1\">\n", table.TableName));
                        writer.WriteLine("<Row>");
                        ArrayList list = new ArrayList();
                        foreach (DataColumn column in table.Columns)
                        {
                            writer.WriteLine("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">{0}</Data></Cell>", column.ColumnName);
                            list.Add(getColumnType(column));
                        }
                        writer.WriteLine("</Row>");
                        foreach (DataRow row in table.Rows)
                        {
                            writer.WriteLine("<Row>");
                            for (int i = 0; i < row.ItemArray.Length; i++)
                            {
                                try
                                {
                                    if (((row[i].ToString().StartsWith("pw://") || row[i].ToString().StartsWith(@"pw:\\")) || (row[i].ToString().StartsWith("http://") || row[i].ToString().StartsWith(@"http:\\"))) || ((row[i].ToString().StartsWith("file://") || row[i].ToString().StartsWith(@"file:\\")) || (row[i].ToString().StartsWith("ftp://") || row[i].ToString().StartsWith(@"ftp:\\"))))
                                    {
                                        if (row[i].ToString().Contains("|"))
                                        {
                                            string[] strArray = row[i].ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                            if (strArray.Length == 2)
                                            {
                                                writer.WriteLine("<Cell ss:StyleID=\"s64\" ss:HRef=\"{1}\"><Data ss:Type=\"{0}\">{2}</Data></Cell>", list[i], strArray[0], strArray[1]);
                                            }
                                            else
                                            {
                                                writer.WriteLine("<Cell ss:StyleID=\"s64\" ss:HRef=\"{1}\"><Data ss:Type=\"{0}\">{1}</Data></Cell>", list[i], row[i].ToString());
                                            }
                                        }
                                        else
                                        {
                                            writer.WriteLine("<Cell ss:StyleID=\"s64\" ss:HRef=\"{1}\"><Data ss:Type=\"{0}\">{1}</Data></Cell>", list[i], row[i].ToString());
                                        }
                                    }
                                    else
                                    {
                                        writer.WriteLine("<Cell><Data ss:Type=\"{0}\">{1}</Data></Cell>", list[i], row[i].ToString());
                                    }
                                }
                                catch (Exception exception)
                                {
                                    BPSUtilities.WriteLog(exception.Message);
                                    BPSUtilities.WriteLog(exception.StackTrace);
                                }
                            }
                            writer.WriteLine("</Row>");
                        }
                        writer.Write("</Table><WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\"><Selected/><ProtectObjects>False</ProtectObjects><ProtectScenarios>False</ProtectScenarios></WorksheetOptions></Worksheet>");
                    }
                    catch (Exception exception2)
                    {
                        BPSUtilities.WriteLog(exception2.Message);
                        BPSUtilities.WriteLog(exception2.StackTrace);
                    }
                }
                writer.Write("</Workbook>");
            }
        }
        catch (Exception exception3)
        {
            BPSUtilities.WriteLog(exception3.Message);
            BPSUtilities.WriteLog(exception3.StackTrace);
        }
        return true;
    }
}
