using System;
using System.Xml;

namespace GpxMerge
{
    public class GpxGpsPoint
    {
        private readonly XmlNode _gpsNode;
        private readonly XmlNamespaceManager _xmlNamespaceManager;

        public GpxGpsPoint(XmlNode gpsNode, XmlNamespaceManager xmlNamespaceManager)
        {
            _gpsNode = gpsNode;
            _xmlNamespaceManager = xmlNamespaceManager;
        }

        public GeographicCoordinate Coordinate { get; private set; }
        public decimal Elevation { get; private set; }

        public DateTime Time
        {
            get
            {
                string timeNodeString = _gpsNode.SelectSingleNode("./gpx:time", _xmlNamespaceManager).InnerText;
                DateTime time = DateTime.Parse(timeNodeString);
                return time;
            }
        }

        public int? HeartRate
        {
            get
            {
                var heartRateNode = GetHeartRateNode();
                if (heartRateNode != null)
                {
                    var heartRateString = heartRateNode.InnerText;
                    var heartRate = int.Parse(heartRateString);
                    return heartRate;
                }
                else
                {
                    return null;
                }
            }
        }

        public decimal? Temperature
        {
            get
            {
                var temperatureNode = GetTemperatureNode();
                if (temperatureNode != null)
                {
                    var temperatureString = temperatureNode.InnerText;
                    var temperature = decimal.Parse(temperatureString);
                    return temperature;
                }
                else
                {
                    return null;
                }
            }
        }

        public void SetHeartRate(int heartRate)
        {
            EnsureExtensionsNodeExists();

            EnsureHeartRateNodeExists();

            var heartRateNode = GetHeartRateNode();
            heartRateNode.InnerText = heartRate.ToString();
        }

        public void SetTemperature(decimal temperature)
        {
            EnsureExtensionsNodeExists();

            EnsureTemperatureNodeExists();

            var temperatureNode = GetTemperatureNode();
            temperatureNode.InnerText = temperature.ToString();
        }

        private void EnsureExtensionsNodeExists()
        {
            XmlNode extensionsNode = _gpsNode.SelectSingleNode("./gpx:extensions", _xmlNamespaceManager);
            if (extensionsNode == null)
            {
                extensionsNode = _gpsNode.OwnerDocument.CreateNode(XmlNodeType.Element, "extensions", "http://www.topografix.com/GPX/1/1");
                _gpsNode.AppendChild(extensionsNode);

                var trackPointExtensionsNode = _gpsNode.OwnerDocument.CreateNode(XmlNodeType.Element, "gpxtpx", "TrackPointExtension", "http://www.garmin.com/xmlschemas/TrackPointExtension/v1");
                extensionsNode.AppendChild(trackPointExtensionsNode);
            }
        }

        private void EnsureHeartRateNodeExists()
        {
            var trackPointExtensionNode = GetTrackPointExtensionNode();

            var heartRateNode = GetHeartRateNode();
            if (heartRateNode == null)
            {
                heartRateNode = trackPointExtensionNode.OwnerDocument.CreateNode(XmlNodeType.Element, "gpxtpx", "hr", "http://www.garmin.com/xmlschemas/TrackPointExtension/v1");
                trackPointExtensionNode.AppendChild(heartRateNode);
            }
        }

        private void EnsureTemperatureNodeExists()
        {
            var trackPointExtensionNode = GetTrackPointExtensionNode();

            var temperatureNode = GetTemperatureNode();
            if (temperatureNode == null)
            {
                temperatureNode = trackPointExtensionNode.OwnerDocument.CreateNode(XmlNodeType.Element, "gpxtpx", "atemp", "http://www.garmin.com/xmlschemas/TrackPointExtension/v1");
                trackPointExtensionNode.AppendChild(temperatureNode);
            }
        }

        private XmlNode GetTrackPointExtensionNode()
        {
            return _gpsNode.SelectSingleNode("./gpx:extensions", _xmlNamespaceManager);
        }

        private XmlNode GetHeartRateNode()
        {
            return _gpsNode.SelectSingleNode("./gpx:extensions/gpxtpx:hr", _xmlNamespaceManager);
        }

        private XmlNode GetTemperatureNode()
        {
            return _gpsNode.SelectSingleNode("./gpx:extensions/gpxtpx:atemp", _xmlNamespaceManager);
        }
    }
}