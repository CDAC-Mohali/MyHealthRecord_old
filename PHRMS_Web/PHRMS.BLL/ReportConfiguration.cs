
using iTextSharp.text;
using System.Collections.Generic;

namespace PHRMS.BLL
{
    public class ReportColumn
    {
        public string ColumnName { get; set; }
        public int Width { get; set; }

        private string headerText;
        public string HeaderText
        {
            set { headerText = value; }
            get { return headerText ?? ColumnName; }
        }
    }

    public class ReportConfiguration
    {
        // Overall page layout, orientation and margins
        private Rectangle pageOrientation = PageSize.LETTER;
        private int marginLeft = 30;
        private int marginTop = 20;
        private int marginRight = 30;
        private int marginBottom = 30;

        public Rectangle PageOrientation
        {
            get { return pageOrientation; }
            set { pageOrientation = value; }
        }
        public int MarginLeft { get { return marginLeft; } set { marginLeft = value; } }
        public int MarginTop { get { return marginTop; } set { marginTop = value; } }
        public int MarginRight
        { get { return marginRight; } set { marginRight = value; } }
        public int MarginBottom
        { get { return marginBottom; } set { marginBottom = value; } }

        // Logo - Logo is always placed at the top left corner
        private int logImageScalePercent = 100;
        public string LogoPathLeft { get; set; }
        public string LogoPathRight { get; set; }
        public int LogImageScalePercent
        {
            get { return logImageScalePercent; }
            set { logImageScalePercent = value; }
        }

        // Title and subtitle. Titles are always center aligned at the top.
        public string ReportTitle { get; set; }
        public string ReportSubTitle { get; set; }

        //Profile Information Dictionary
        public Dictionary<string, string> dictProfileInfo { get; set; }
        public Dictionary<string, Dictionary<string,string>> dictTempProfileInfo { get; set; }
    }
}
