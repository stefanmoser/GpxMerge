using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace GpxMerge
{
    public class GpxActivity
    {
        private XmlDocument _gpxXmlDocument;
        private XmlNamespaceManager _xmlNamespaceManager;

        public string ActivityName { get; private set; }
        public DateTime StartTime { get; private set; }

        public GpxActivity(FileInfo file)
        {
            _gpxXmlDocument = new XmlDocument();
            _gpxXmlDocument.Load(file.FullName);

            _xmlNamespaceManager = new XmlNamespaceManager(_gpxXmlDocument.NameTable);
            _xmlNamespaceManager.AddNamespace("gpx", "http://www.topografix.com/GPX/1/1");
        } 

        public IEnumerable<GpxGpsPoint> GetAllPoints()
        {
            XmlNodeList gpsNodes = _gpxXmlDocument.SelectNodes("/gpx:gpx/gpx:trk/gpx:trkseg/gpx:trkpt", _xmlNamespaceManager);
            foreach (XmlNode gpsNode in gpsNodes)
            {
                GpxGpsPoint gpsPoint = new GpxGpsPoint(gpsNode, _xmlNamespaceManager);
                yield return gpsPoint;
            }
        }

        public GpxGpsPoint GetPointAtTime(DateTime time)
        {
            return null;
        }

        public void RemovePointAtTime(DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}