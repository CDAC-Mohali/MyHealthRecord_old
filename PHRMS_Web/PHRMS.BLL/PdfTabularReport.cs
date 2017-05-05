using System;
using System.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Collections.Generic;

namespace PHRMS.BLL
{
    // The main class used to generate Pdf tabular report.
    public class PdfTabularReport
    {
        // Configurations
        public ReportConfiguration ReportConfiguration { get; set; }

        // Internal properties, not set outside of the class
        public Image LogoImageLeft { get; private set; }
        public Image LogoImageRight { get; private set; }
        public PdfPTable Title { get; private set; }
        public PdfPTable ProfileInfo { get; private set; }
        public PdfPTable PageNumberLabel { get; private set; }
        public float HeaderSectionHeight { get; private set; }
        public static float PageNumberLabelHeight { get; set; }
        public float profileHeight { get; private set; }
        public int PageCount { get; private set; }

        // Private instance variables
        Document PdfDocument = null;
        PdfWriter PdfWriter = null;
        MemoryStream PdfStream = null;

        // Constructor
        public PdfTabularReport(ReportConfiguration configuration = null)
        {
            // If configuration is not provided, the default will be used
            ReportConfiguration = configuration ?? new ReportConfiguration();
        }

        private void InitiateDocument()
        {
            PdfStream = new MemoryStream();
            PdfDocument = new Document(ReportConfiguration.PageOrientation);
            PdfDocument.SetMargins(ReportConfiguration.MarginLeft,
                ReportConfiguration.MarginRight,
                ReportConfiguration.MarginTop, ReportConfiguration.MarginBottom);
            PdfWriter = PdfWriter.GetInstance(PdfDocument, PdfStream);

            // create logo, header and page number objects
            PdfPCell cell;
            HeaderSectionHeight = 0;
            LogoImageLeft = null;
            LogoImageRight = null;
            if (ReportConfiguration.LogoPathLeft != null)
            {
                LogoImageLeft = Image.GetInstance(ReportConfiguration.LogoPathLeft);
                LogoImageLeft.ScalePercent(ReportConfiguration.LogImageScalePercent);
                LogoImageLeft.SetAbsolutePosition(PdfDocument.LeftMargin,
                    PdfDocument.PageSize.Height - PdfDocument.TopMargin
                        - LogoImageLeft.ScaledHeight);

                HeaderSectionHeight = LogoImageLeft.ScaledHeight;
            }
            if (ReportConfiguration.LogoPathRight != null)
            {
                LogoImageRight = Image.GetInstance(ReportConfiguration.LogoPathRight);
                LogoImageRight.ScalePercent(ReportConfiguration.LogImageScalePercent);
                LogoImageRight.SetAbsolutePosition(PdfDocument.PageSize.Width - PdfDocument.RightMargin - LogoImageRight.ScaledWidth, PdfDocument.PageSize.Height - PdfDocument.TopMargin
                        - LogoImageRight.ScaledHeight);

                HeaderSectionHeight = HeaderSectionHeight > LogoImageRight.ScaledHeight ? HeaderSectionHeight : LogoImageRight.ScaledHeight;
            }

            Title = null;
            float titleHeight = 0;
            if ((ReportConfiguration.ReportTitle != null) ||
                (ReportConfiguration.ReportSubTitle != null))
            {
                Title = new PdfPTable(1);
                Title.TotalWidth = PdfDocument.PageSize.Width
                    - (PdfDocument.LeftMargin + PdfDocument.RightMargin);

                if (ReportConfiguration.ReportTitle != null)
                {
                    cell = new PdfPCell(new Phrase(ReportConfiguration.ReportTitle,
                                               new Font(ReportFonts.HelveticaBold, 12)));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 0;
                    Title.AddCell(cell);
                }

                if (ReportConfiguration.ReportSubTitle != null)
                {
                    cell = new PdfPCell(new Phrase(ReportConfiguration.ReportSubTitle,
                                               new Font(ReportFonts.Helvetica, 10)));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.PaddingTop = 5;
                    cell.Border = 0;
                    Title.AddCell(cell);
                }

                // Get the height of the title section
                for (int i = 0; i < Title.Rows.Count; i++)
                {
                    titleHeight = titleHeight + Title.GetRowHeight(i);
                }
            }
            HeaderSectionHeight = (HeaderSectionHeight > titleHeight)
                ? HeaderSectionHeight : titleHeight;


            PageNumberLabel = new PdfPTable(2);
            PageNumberLabel.TotalWidth = PdfDocument.PageSize.Width
                               - (PdfDocument.LeftMargin + PdfDocument.RightMargin);
            cell = new PdfPCell(new Phrase("Page Label", new Font(ReportFonts.Helvetica, 8)));
            cell.Border = 0;
            float pagenumberHeight = PageNumberLabel.GetRowHeight(0);
            HeaderSectionHeight = (HeaderSectionHeight > pagenumberHeight)
                ? HeaderSectionHeight : pagenumberHeight;

            //if (ReportConfiguration.dictTempProfileInfo != null && ReportConfiguration.dictTempProfileInfo.Count > 0)
            //{
            //    ProfileInfo = new PdfPTable(2);
            //    ProfileInfo.TotalWidth = PdfDocument.PageSize.Width
            //                       - (PdfDocument.LeftMargin + PdfDocument.RightMargin);
            //    //leave a gap before and after the table
            //    ProfileInfo.SpacingBefore = 20f;
            //    ProfileInfo.SpacingAfter = 20f;
            //    PdfPCell tempCell1 = null;
            //    PdfPCell tempCell2 = null;
            //    Phrase phrase = null;
            //    Dictionary<string, string> tempDict = null;
            //    for (int i = 0; i < ReportConfiguration.dictTempProfileInfo.Count; i++)
            //    {
            //        tempDict = ReportConfiguration.dictTempProfileInfo.ElementAt(i).Value;
            //        if (tempDict != null && tempDict.Count > 0)
            //        {
            //            ReportTable.AddCell(ProfileInfo, " ", new Font(ReportFonts.HelveticaBold, 12), BaseColor.WHITE, 3f, 2, Element.ALIGN_MIDDLE);
            //            ReportTable.AddCell(ProfileInfo, ReportConfiguration.dictTempProfileInfo.ElementAt(i).Key, new Font(ReportFonts.HelveticaBold, 12), BaseColor.WHITE, 3f, 2, Element.ALIGN_MIDDLE, 2f);
            //            tempCell1 = new PdfPCell();
            //            tempCell2 = new PdfPCell();
            //            phrase = null;
            //            phrase = new Phrase();
            //            tempCell1.BackgroundColor = BaseColor.WHITE;
            //            for (int j = 0; j < tempDict.Count; j++)
            //            {
            //                if (j % 2 == 0)
            //                {
            //                    phrase = new Phrase();
            //                    phrase.Add(new Chunk(tempDict.ElementAt(j).Key + ": ", new Font(ReportFonts.HelveticaBold, 10)));
            //                    phrase.Add(new Chunk(tempDict.ElementAt(j).Value, new Font(ReportFonts.Helvetica, 10)));
            //                    tempCell1.AddElement(phrase);
            //                }
            //                else
            //                {
            //                    phrase = new Phrase();
            //                    phrase.Add(new Chunk(tempDict.ElementAt(j).Key + ": ", new Font(ReportFonts.HelveticaBold, 10)));
            //                    phrase.Add(new Chunk(tempDict.ElementAt(j).Value, new Font(ReportFonts.Helvetica, 10)));
            //                    tempCell2.AddElement(phrase);
            //                }
            //                phrase = null;
            //            }
            //            tempCell1.VerticalAlignment = Element.ALIGN_LEFT;
            //            tempCell2.VerticalAlignment = Element.ALIGN_RIGHT;
            //            tempCell1.Border = 0;
            //            tempCell2.Border = 0;
            //            //PdfPRow row = new PdfPRow(new PdfPCell[] { tempCell1, tempCell2 });
            //            ProfileInfo.AddCell(tempCell1);
            //            ProfileInfo.AddCell(tempCell2);
            //            tempDict = null;
            //            tempCell1 = null;
            //            tempCell2 = null;
            //        }
            //    }

            //ReportTable.AddCell(ProfileInfo, " ", new Font(ReportFonts.HelveticaBold, 12), BaseColor.WHITE, 3f, 2, Element.ALIGN_MIDDLE);
            //ReportTable.AddCell(ProfileInfo, "Personal Information", new Font(ReportFonts.HelveticaBold, 12), BaseColor.WHITE, 3f, 2, Element.ALIGN_MIDDLE, 2f);
            //PdfPCell tempCell1 = new PdfPCell(new Phrase("Profile", new Font(ReportFonts.HelveticaBold, 12)));
            ////tempCell1.BackgroundColor = iTextSharp.text.html.WebColors.GetRGBColor("#A00000");
            //tempCell1.Colspan = 2;
            //tempCell1.Border = 0;
            //tempCell1.BorderWidthBottom = 1f;
            //ProfileInfo.AddCell(tempCell1);
            //tempCell1 = null;

            //PdfPCell tempCell1 = new PdfPCell();
            //PdfPCell tempCell2 = new PdfPCell();
            //Phrase phrase = null;
            //phrase = new Phrase();
            //tempCell1.BackgroundColor = BaseColor.WHITE;
            //for (int i = 0; i < ReportConfiguration.dictProfileInfo.Count; i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //        phrase = new Phrase();
            //        phrase.Add(new Chunk(ReportConfiguration.dictProfileInfo.ElementAt(i).Key + ": ", new Font(ReportFonts.HelveticaBold, 10)));
            //        phrase.Add(new Chunk(ReportConfiguration.dictProfileInfo.ElementAt(i).Value, new Font(ReportFonts.Helvetica, 10)));
            //        tempCell1.AddElement(phrase);
            //    }
            //    else
            //    {
            //        phrase = new Phrase();
            //        phrase.Add(new Chunk(ReportConfiguration.dictProfileInfo.ElementAt(i).Key + ": ", new Font(ReportFonts.HelveticaBold, 10)));
            //        phrase.Add(new Chunk(ReportConfiguration.dictProfileInfo.ElementAt(i).Value, new Font(ReportFonts.Helvetica, 10)));
            //        tempCell2.AddElement(phrase);
            //    }
            //    phrase = null;
            //}
            //tempCell1.VerticalAlignment = Element.ALIGN_LEFT;
            //tempCell2.VerticalAlignment = Element.ALIGN_RIGHT;
            //tempCell1.Border = 0;
            //tempCell2.Border = 0;
            ////PdfPRow row = new PdfPRow(new PdfPCell[] { tempCell1, tempCell2 });
            //ProfileInfo.AddCell(tempCell1);
            //ProfileInfo.AddCell(tempCell2);
            //ProfileInfo.Rows.Add(row);
            //for (int i = 0; i < ProfileInfo.Rows.Count; i++)
            //{
            //    profileHeight = profileHeight + ProfileInfo.GetRowHeight(i);
            //}
            //}

        }

        private MemoryStream RenderDocument(ReportTable reportTable)
        {
            PdfWriter.PageEvent = new PageEventHelper { Report = this };
            PdfDocument.Open();
            reportTable.RenderProfileInfo(PdfDocument, PdfWriter);
            reportTable.RenderTable(PdfDocument, PdfWriter);
            PdfDocument.Close();
            PdfWriter.Flush();
            return PdfStream;
        }

        // Method to get the pdf stream
        public MemoryStream GetPdf<T>(Dictionary<string, List<T>> data, Dictionary<string, List<ReportColumn>> displayColumns, Dictionary<string, Dictionary<string, string>> dictTempProfileInfo)
        {
            InitiateDocument();

            // Add the report data
            //var top = (HeaderSectionHeight == 0 && profileHeight == 0)
            //              ? PdfDocument.PageSize.Height - PdfDocument.TopMargin
            //              : (HeaderSectionHeight != 0 && profileHeight == 0) ? PdfDocument.PageSize.Height - PdfDocument.TopMargin
            //              - HeaderSectionHeight - 10 : (HeaderSectionHeight == 0 && profileHeight != 0)
            //              ? PdfDocument.PageSize.Height - PdfDocument.TopMargin
            //              - profileHeight - 10 : PdfDocument.PageSize.Height - PdfDocument.TopMargin
            //              - HeaderSectionHeight - profileHeight - 10;
            var top = (HeaderSectionHeight == 0)
                          ? PdfDocument.PageSize.Height - PdfDocument.TopMargin
                          : PdfDocument.PageSize.Height - PdfDocument.TopMargin
                          - HeaderSectionHeight - 10;
            var reportTable = ReportTable.CreateReportTable<T>(data,
                dictTempProfileInfo, displayColumns, top, PdfDocument);
            PageCount = reportTable.PageCount;

            return RenderDocument(reportTable);
        }

        // Overloaded method to get the pdf stream. It takes that data as DataTable
        //public MemoryStream GetPdf(DataTable data, List<ReportColumn> displayColumns, Dictionary<string, Dictionary<string, string>> dictTempProfileInfo)
        public MemoryStream GetPdf(Dictionary<string, List<DataRow>> data, Dictionary<string, List<ReportColumn>> displayColumns, Dictionary<string, Dictionary<string, string>> dictTempProfileInfo)
        {
            //List<DataRow> list = data.Select().ToList();
            //List<DataRow> list = new List<DataRow>();
            //foreach (DataRow row in data.Rows)
            //{
            //    list.Add(row);
            //}
            return GetPdf<DataRow>(data, displayColumns, dictTempProfileInfo);
        }
    }

    // Fonts used by the tabular report
    public class ReportFonts
    {
        public static BaseFont Helvetica
        {
            get
            {
                return BaseFont.CreateFont(BaseFont.HELVETICA,
              BaseFont.CP1252, false);
            }
        }
        public static BaseFont HelveticaBold
        {
            get
            {
                return BaseFont.CreateFont(BaseFont.HELVETICA_BOLD,
              BaseFont.CP1252, false);
            }
        }
    }

    // Utility class to render the Pdf table to the report document
    internal class ReportTable
    {
        private PdfPTable headerTable;
        private PdfPTable dataTable;
        private Dictionary<string, PdfPTable> dHeaderTable;
        private Dictionary<string, PdfPTable> dDataTable;
        private Dictionary<string, PdfPTable> dTitleTable;
        private PdfPTable profileTable;
        private PdfPTable titleTable;
        private Dictionary<string, List<Tuple<int, int>>> pageSplitter;
        private Dictionary<string, int> dicTotalPages;
        private Dictionary<string, float> dicResPageSize;
        private float width;
        private float top;
        private float height;

        public int PageCount
        {
            get { return (pageSplitter.Count == 0) ? 1 : pageSplitter.Count; }
        }

        // Private constructor. The instances need to use "CreateReportTable"
        // method to create.
        private ReportTable(Dictionary<string, List<ReportColumn>> displayColumns,
            Document document, float top)
        {
            pageSplitter = new Dictionary<string, List<Tuple<int, int>>>();
            dicTotalPages = new Dictionary<string, int>();
            this.top = top;
            width = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            height = top - document.BottomMargin;
            dDataTable = new Dictionary<string, PdfPTable>();
            dHeaderTable = new Dictionary<string, PdfPTable>();
            dTitleTable = new Dictionary<string, PdfPTable>();
            foreach (KeyValuePair<string, List<ReportColumn>> entry in displayColumns)
            {
                float[] columnWidths = (from c in entry.Value select (float)c.Width).ToArray();
                titleTable = new PdfPTable(columnWidths);
                titleTable.TotalWidth = width;
                AddCell(titleTable, " ", new Font(ReportFonts.Helvetica), BaseColor.WHITE, 0f, columnWidths.Count());
                AddCell(titleTable, entry.Key, new Font(ReportFonts.HelveticaBold, 12), BaseColor.WHITE, 3f, columnWidths.Count(), Element.ALIGN_RIGHT, 2f);
                AddCell(titleTable, " ", new Font(ReportFonts.Helvetica), BaseColor.WHITE, 0f, columnWidths.Count());
                headerTable = new PdfPTable(columnWidths);
                headerTable.TotalWidth = width;
                //AddCell(headerTable, " ", new Font(ReportFonts.Helvetica), BaseColor.WHITE,3f,columnWidths.Count());
                dataTable = new PdfPTable(columnWidths);
                dataTable.TotalWidth = width;

                foreach (var column in entry.Value)
                {
                    AddCell(headerTable, column.HeaderText,
                        new Font(ReportFonts.HelveticaBold, 10), BaseColor.WHITE, 5f);
                }

                dHeaderTable.Add(entry.Key, headerTable);
                dDataTable.Add(entry.Key, dataTable);
                dTitleTable.Add(entry.Key, titleTable);
            }
        }

        public static void AddCell(PdfPTable table, string Text, Font font,
            BaseColor backgroundColor = null, float padding = 3f, int colspan = 1, int valign = Element.ALIGN_MIDDLE, float borderBottom = 0f)
        {
            PdfPCell cell = new PdfPCell(new Paragraph(Text, font));
            cell.Padding = padding;
            cell.Border = 0;
            cell.VerticalAlignment = valign; //Element.ALIGN_MIDDLE;
            cell.BackgroundColor = backgroundColor ?? BaseColor.WHITE;
            cell.Colspan = colspan;
            cell.BorderWidthBottom = borderBottom;
            table.AddCell(cell);
        }

        private static void AddRow(Object dataitem, System.Type type,
            List<ReportColumn> displayColumns, BaseColor color, PdfPTable table)
        {
            foreach (var column in displayColumns)
            {
                var text = string.Empty;

                if (type.FullName == "System.Data.DataRow")
                {
                    text = ((DataRow)dataitem)[column.ColumnName].ToString();
                }
                else
                {
                    var propertyInfo = type.GetProperty(column.ColumnName);
                    text = (propertyInfo.GetValue(dataitem, null) == null)
                               ? ""
                               : propertyInfo.GetValue(dataitem, null).ToString();
                }

                AddCell(table, text, new Font(ReportFonts.Helvetica, 8), color);
            }
        }

        public static float GetTableHeight(PdfPTable pdfPTable)
        {
            float height = 0;
            for (int i = 0; i < pdfPTable.Rows.Count; i++)
            {
                height = height + pdfPTable.GetRowHeight(i);
            }

            return height;
        }

        public static int GetTotalRowCount<T>(Dictionary<string, List<T>> data)
        {
            int count = 0;
            foreach (KeyValuePair<string, List<T>> entry in data)
            {
                count = count + entry.Value.Count;
            }
            return count;
        }

        // Static method to create & return an instance object
        public static ReportTable CreateReportTable<T>(Dictionary<string, List<T>> data,
           Dictionary<string, Dictionary<string, string>> dictTempProfileInfo, Dictionary<string, List<ReportColumn>> displayColumns, float top, Document document)
        {
            // Construct an instance object
            var reportTable = new ReportTable(displayColumns, document, top);
            var type = typeof(T);
            reportTable.dicResPageSize = new Dictionary<string, float>();
            reportTable.CreateProfileInfo(dictTempProfileInfo, top, document);
            float headerHeight = 0;
            float gridHeaderHeight = reportTable.dHeaderTable.Count > 0 ? GetTableHeight(reportTable.dHeaderTable.ElementAt(0).Value) : 0;
            float actualHeight = GetTableHeight(reportTable.profileTable) + gridHeaderHeight + document.BottomMargin + PdfTabularReport.PageNumberLabelHeight;
            bool lastRowReached = false;
            if (gridHeaderHeight > 0)
            {
                foreach (KeyValuePair<string, List<T>> entry in data)
                {
                    // Add each data item into the PdfPTable.
                    int srartRow = 0;
                    int pageRowIndex = 0;
                    headerHeight = GetTableHeight(reportTable.dHeaderTable[entry.Key]);//reportTable.headerTable.GetRowHeight(0);
                                                                                       //actualHeight = actualHeight + headerHeight + document.BottomMargin + PdfTabularReport.PageNumberLabelHeight;
                    reportTable.dicTotalPages.Add(entry.Key, 0);
                    reportTable.pageSplitter.Add(entry.Key, new List<Tuple<int, int>>());
                    for (int i = 0; i < entry.Value.Count; i++)
                    {
                        var dataItem = entry.Value[i];
                        BaseColor color = (pageRowIndex++ % 2 == 0) ? BaseColor.LIGHT_GRAY : BaseColor.WHITE;

                        AddRow(dataItem, type, displayColumns[entry.Key], color, reportTable.dDataTable[entry.Key]);

                        actualHeight = actualHeight + reportTable.dDataTable[entry.Key].GetRowHeight(i);
                        //var lastRowReached = i == entry.Value.Count - 1;
                        //if (entry.Equals(data.Last()))
                        //{
                        lastRowReached = i == entry.Value.Count - 1;
                        //}
                        if ((actualHeight > reportTable.height) || lastRowReached)
                        {
                            reportTable.pageSplitter[entry.Key].Add(new Tuple<int, int>(srartRow, lastRowReached ? -1 : i));
                            if ((actualHeight > reportTable.height))
                            {
                                if (reportTable.dicTotalPages.ContainsKey(entry.Key))
                                    reportTable.dicTotalPages[entry.Key]++;
                                reportTable.dDataTable[entry.Key].DeleteLastRow();
                                AddRow(dataItem, type, displayColumns[entry.Key], BaseColor.LIGHT_GRAY,
                                    reportTable.dDataTable[entry.Key]);
                                pageRowIndex = 1;
                                actualHeight = headerHeight + reportTable.dDataTable[entry.Key].GetRowHeight(i) + document.BottomMargin + PdfTabularReport.PageNumberLabelHeight;
                                if (lastRowReached)
                                {
                                    reportTable.dicResPageSize.Add(entry.Key, actualHeight);
                                }
                            }
                            else
                            {
                                reportTable.dicResPageSize.Add(entry.Key, actualHeight);
                                actualHeight = actualHeight + reportTable.dDataTable[entry.Key].GetRowHeight(i) + document.BottomMargin + PdfTabularReport.PageNumberLabelHeight;
                            }
                            srartRow = i;
                        }
                    }
                }
            }
            return reportTable;
        }

        public void CreateProfileInfo(Dictionary<string, Dictionary<string, string>> dictTempProfileInfo, float top, Document document)
        {
            if (dictTempProfileInfo != null && dictTempProfileInfo.Count > 0)
            {
                profileTable = new PdfPTable(2);
                profileTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                //leave a gap before and after the table
                profileTable.SpacingBefore = 20f;
                profileTable.SpacingAfter = 20f;
                PdfPCell tempCell1 = null;
                PdfPCell tempCell2 = null;
                Phrase phrase = null;
                Dictionary<string, string> tempDict = null;
                for (int i = 0; i < dictTempProfileInfo.Count; i++)
                {
                    tempDict = dictTempProfileInfo.ElementAt(i).Value;
                    if (tempDict != null && tempDict.Count > 0)
                    {
                        AddCell(profileTable, " ", new Font(ReportFonts.HelveticaBold, 12), BaseColor.WHITE, 3f, 2, Element.ALIGN_MIDDLE);
                        AddCell(profileTable, dictTempProfileInfo.ElementAt(i).Key, new Font(ReportFonts.HelveticaBold, 12), BaseColor.WHITE, 3f, 2, Element.ALIGN_MIDDLE, 2f);
                        tempCell1 = new PdfPCell();
                        tempCell2 = new PdfPCell();
                        phrase = null;
                        phrase = new Phrase();
                        tempCell1.BackgroundColor = BaseColor.WHITE;
                        for (int j = 0; j < tempDict.Count; j++)
                        {
                            if (j % 2 == 0)
                            {
                                phrase = new Phrase();
                                phrase.Add(new Chunk(tempDict.ElementAt(j).Key + ": ", new Font(ReportFonts.HelveticaBold, 10)));
                                phrase.Add(new Chunk(tempDict.ElementAt(j).Value, new Font(ReportFonts.Helvetica, 10)));
                                tempCell1.AddElement(phrase);
                            }
                            else
                            {
                                phrase = new Phrase();
                                phrase.Add(new Chunk(tempDict.ElementAt(j).Key + ": ", new Font(ReportFonts.HelveticaBold, 10)));
                                phrase.Add(new Chunk(tempDict.ElementAt(j).Value, new Font(ReportFonts.Helvetica, 10)));
                                tempCell2.AddElement(phrase);
                            }
                            phrase = null;
                        }
                        tempCell1.VerticalAlignment = Element.ALIGN_LEFT;
                        tempCell2.VerticalAlignment = Element.ALIGN_RIGHT;
                        tempCell1.Border = 0;
                        tempCell2.Border = 0;
                        //PdfPRow row = new PdfPRow(new PdfPCell[] { tempCell1, tempCell2 });
                        profileTable.AddCell(tempCell1);
                        profileTable.AddCell(tempCell2);
                        tempDict = null;
                        tempCell1 = null;
                        tempCell2 = null;
                    }
                }
            }
        }

        public void RenderProfileInfo(Document document, PdfWriter writer)
        {
            profileTable.WriteSelectedRows(0, -1, document.LeftMargin,
       top, writer.DirectContent);
        }

        // Render the table to the Pdf document.
        public void RenderTable(Document document, PdfWriter writer)
        {
            RenderProfileInfo(document, writer);
            if (dHeaderTable.Count > 0)
            {
                float left = (document.PageSize.Width - headerTable.TotalWidth) / 2;

                var pageCount = pageSplitter.Count;
                float profileHeight = GetTableHeight(profileTable);
                int pCounter = 0;
                string strPrevKey = "";
                foreach (KeyValuePair<string, PdfPTable> entry in dDataTable)
                {
                    float titleHeight = GetTableHeight(dTitleTable[entry.Key]);
                    float headerHeight = GetTableHeight(dHeaderTable[entry.Key]);
                    for (int i = 0; i < pageSplitter[entry.Key].Count; i++)
                    {
                        var rownumbers = pageSplitter[entry.Key][i];
                        if (i == 0)
                        {
                            if (pCounter == 0)
                            {
                                dTitleTable[entry.Key].WriteSelectedRows(0, -1, left, top - profileHeight, writer.DirectContent);
                                dHeaderTable[entry.Key].WriteSelectedRows(0, -1, left, top - profileHeight - titleHeight, writer.DirectContent);
                                entry.Value.WriteSelectedRows(rownumbers.Item1, rownumbers.Item2,
                                        left, top - profileHeight - headerHeight - titleHeight, writer.DirectContent);
                            }
                            else
                            {
                                //profileHeight = dicResPageSize[strPrevKey];
                                dTitleTable[entry.Key].WriteSelectedRows(0, -1, left, top - dicResPageSize[strPrevKey], writer.DirectContent);
                                dHeaderTable[entry.Key].WriteSelectedRows(0, -1, left, top - dicResPageSize[strPrevKey] - titleHeight, writer.DirectContent);
                                entry.Value.WriteSelectedRows(rownumbers.Item1, rownumbers.Item2,
                                        left, top - dicResPageSize[strPrevKey] - titleHeight, writer.DirectContent);
                            }
                        }
                        else
                        {
                            dHeaderTable[entry.Key].WriteSelectedRows(0, -1, left, top, writer.DirectContent);
                            entry.Value.WriteSelectedRows(rownumbers.Item1, rownumbers.Item2,
                                    left, top - headerHeight, writer.DirectContent);
                        }

                        if (i != pageSplitter[entry.Key].Count - 1)
                        {
                            document.NewPage();
                            pCounter++;
                        }
                    }
                    strPrevKey = entry.Key;
                }
            }
        }
    }

    // PdfPageEventHelper: logo, title, sub-title, and page numbers.
    public class PageEventHelper : PdfPageEventHelper
    {
        public PdfTabularReport Report { get; set; }

        private void AddHeader(Document document, PdfWriter writer)
        {
            // Add logo
            if (Report.LogoImageLeft != null)
            {
                document.Add(Report.LogoImageLeft);
            }
            if (Report.LogoImageRight != null)
            {
                document.Add(Report.LogoImageRight);
            }

            // Add titles
            if (Report.Title != null)
            {
                Report.Title.WriteSelectedRows(0, -1, document.LeftMargin,
                    document.PageSize.Height - document.TopMargin, writer.DirectContent);
            }
            // Add page number
            Report.PageNumberLabel.DeleteLastRow();
            var cell = new PdfPCell(new Phrase("Printed on " + DateTime.Now.ToString("dd/MM/yyyy"), new Font(ReportFonts.Helvetica, 8)));
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            Report.PageNumberLabel.AddCell(cell);
            cell = null;
            cell = new PdfPCell(new Phrase("Page " + document.PageNumber.ToString()
                + " of "
                + Report.PageCount.ToString(), new Font(ReportFonts.Helvetica, 8)));
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Report.PageNumberLabel.AddCell(cell);
            var cellHeight = Report.PageNumberLabel.GetRowHeight(0);
            PdfTabularReport.PageNumberLabelHeight = cellHeight;
            //    Report.PageNumberLabel.WriteSelectedRows(0, -1, document.LeftMargin,
            //    document.PageSize.Height - document.TopMargin - Report.HeaderSectionHeight + cellHeight, writer.DirectContent);
            Report.PageNumberLabel.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin, writer.DirectContent);
            //     if (Report.ProfileInfo.Rows.Count > 0)
            //     {
            //         Report.ProfileInfo.WriteSelectedRows(0, -1, document.LeftMargin,
            //document.PageSize.Height - document.TopMargin - Report.HeaderSectionHeight, writer.DirectContent);
            //         Report.ProfileInfo.FlushContent();
            //     }
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            AddHeader(document, writer);
        }
    }
}
