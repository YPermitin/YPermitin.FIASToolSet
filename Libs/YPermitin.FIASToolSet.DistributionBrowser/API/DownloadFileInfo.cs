using System;

namespace YPermitin.FIASToolSet.DistributionBrowser.API
{
    internal sealed class DownloadFileInfo
    {
        public int VersionId { get; set; }
        public string TextVersion { get; set; }
        public Uri FiasCompleteDbfUrl { get; set; }
        public Uri FiasCompleteXmlUrl { get; set; }
        public Uri FiasDeltaDbfUrl { get; set; }
        public Uri FiasDeltaXmlUrl { get; set; }
        public Uri Kladr4ArjUrl { get; set; }
        public Uri Kladr47ZUrl { get; set; }
        public Uri GarXMLFullURL { get; set; }
        public Uri GarXMLDeltaURL { get; set; }
        public DateTime Date { get; set; }
    }
}
