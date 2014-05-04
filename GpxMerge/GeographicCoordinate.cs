namespace GpxMerge
{
    public class GeographicCoordinate
    {
        private readonly decimal _latitude;
        private readonly decimal _longitude;

        public GeographicCoordinate(decimal latitude, decimal longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        public decimal Latitude
        {
            get { return _latitude; }
        }

        public decimal Longitude
        {
            get { return _longitude; }
        }
    }
}