using System;
using System.IO;
using System.Reflection;
using System.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace GpxMerge.Tests
{
    [TestFixture]
    public class GpxGpsPointFixture
    {
        private XmlDocument _gpxXmlDocument;
        private XmlNamespaceManager _xmlNamespaceManager;
        private XmlNode _gpsNode;

        [SetUp]
        public void SetUp()
        {
            string xmlString;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "GpxMerge.Tests.TestGpxFiles.Android.gpx";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    xmlString = reader.ReadToEnd();
                }
            }

            _gpxXmlDocument = new XmlDocument();
            _gpxXmlDocument.LoadXml(xmlString);

            _xmlNamespaceManager = new XmlNamespaceManager(_gpxXmlDocument.NameTable);
            _xmlNamespaceManager.AddNamespace("gpx", "http://www.topografix.com/GPX/1/1");
            _xmlNamespaceManager.AddNamespace("gpxtpx", "http://www.garmin.com/xmlschemas/TrackPointExtension/v1");

            _gpsNode = _gpxXmlDocument.SelectSingleNode("/gpx:gpx/gpx:trk/gpx:trkseg/gpx:trkpt", _xmlNamespaceManager);
        }

        [Test]
        public void should_get_time()
        {
            DateTime expectedTime = DateTime.Parse("2014-04-27T15:02:41Z");
            GpxGpsPoint gpsPoint = new GpxGpsPoint(_gpsNode, _xmlNamespaceManager);

            var time = gpsPoint.Time;

            time.Should().Be(expectedTime);
        }

        [Test]
        public void should_not_return_heart_rate_when_not_set()
        {
            GpxGpsPoint gpsPoint = new GpxGpsPoint(_gpsNode, _xmlNamespaceManager);

            var heartRate = gpsPoint.HeartRate;

            heartRate.Should().Be(null);
        }

        [Test]
        public void should_set_heart_rate()
        {
            var expectedHeartRate = 130;
            GpxGpsPoint gpsPoint = new GpxGpsPoint(_gpsNode, _xmlNamespaceManager);

            gpsPoint.SetHeartRate(expectedHeartRate);
            var heartRate = gpsPoint.HeartRate;

            heartRate.Should().Be(expectedHeartRate);
        }

        [Test]
        public void should_not_return_temperature_when_not_set()
        {
            GpxGpsPoint gpsPoint = new GpxGpsPoint(_gpsNode, _xmlNamespaceManager);

            var temperature = gpsPoint.Temperature;

            temperature.Should().Be(null);
        }

        [Test]
        public void should_set_temperature()
        {
            decimal expectedTemperature = 25.7m;
            GpxGpsPoint gpsPoint = new GpxGpsPoint(_gpsNode, _xmlNamespaceManager);

            gpsPoint.SetTemperature(expectedTemperature);
            var temperature = gpsPoint.Temperature;

            temperature.Should().Be(expectedTemperature);
        }
    }
}