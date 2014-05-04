using System;
using System.IO;
using System.Xml;

namespace GpxMerge
{
    public class GpxMerger
    {
        public void MergeFiles(FileInfo gpsFile, FileInfo heartRateFile, string outputFileName)
        {
            var gpsDoc = new XmlDocument();
            gpsDoc.Load(gpsFile.FullName);

            var heartRateDoc = new XmlDocument();
            heartRateDoc.Load(heartRateFile.FullName);

            var mgr = new XmlNamespaceManager(gpsDoc.NameTable);
            mgr.AddNamespace("gpx", "http://www.topografix.com/GPX/1/1");   

            XmlNodeList gpsNodes = gpsDoc.SelectNodes("/gpx:gpx/gpx:trk/gpx:trkseg/gpx:trkpt", mgr);
            foreach (XmlNode gpsNode in gpsNodes)
            {
                var timeNode = gpsNode.SelectSingleNode("./gpx:time", mgr);
                var timeString = timeNode.InnerText;

                var garminTimeString = timeString.Substring(0, timeString.Length - 1) + ".000Z";
                //var garminTimeString = timeString;

                var matchingNode = heartRateDoc.SelectSingleNode(string.Format("/gpx:gpx/gpx:trk/gpx:trkseg/gpx:trkpt[gpx:time='{0}']", garminTimeString), mgr);
                if (matchingNode != null)
                {
                    var extensionsNode = matchingNode.SelectSingleNode("./gpx:extensions", mgr);
                    var importedNode = gpsDoc.ImportNode(extensionsNode, true);
                    gpsNode.AppendChild(importedNode);
                }
                else
                {
                    gpsNode.ParentNode.RemoveChild(gpsNode);
                }
            }

            var metadataTimeNode = gpsDoc.SelectSingleNode("/gpx:gpx/gpx:metadata/gpx:time", mgr);
            metadataTimeNode.InnerText = gpsDoc.SelectNodes("/gpx:gpx/gpx:trk/gpx:trkseg/gpx:trkpt/gpx:time", mgr)[0].InnerText;

            gpsDoc.DocumentElement.SetAttribute("xmlns:gpxtpx", "http://www.garmin.com/xmlschemas/TrackPointExtension/v1");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
            using (XmlWriter writer = XmlWriter.Create(outputFileName, settings))
            {
                gpsDoc.WriteTo(writer);
            }
        }
    }
}