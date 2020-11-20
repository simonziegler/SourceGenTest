using System;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// Represents a longitude/latitude coordinate of a geospatial point on the globe.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Global Coordinate")]
    public class Coordinate : VectisBase
    {
        private double longitude;
        /// <summary>
        /// Longitude in degrees.
        /// </summary>
        [MessagePack.Key(5)]
        [Display(Name = "Longitude", Prompt = "The coodinate's longitude point (degrees)")]
        public double Longitude { get => longitude; set => Setter(ref longitude, value); }


        private double latitude;
        /// <summary>
        /// Latitude in degrees.
        /// </summary>
        [MessagePack.Key(6)]
        [Display(Name = "Latitude", Prompt = "The coodinate's latitude point (degrees)")]
        public double Latitude { get => latitude; set => Setter(ref latitude, value); }


        /// <summary>
        /// Longitude in radians.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public double LongitudeRadians => Longitude * Math.PI / 180;


        /// <summary>
        /// Latitude in radians.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public double LatitudeRadians => Latitude * Math.PI / 180;


        /// <summary>
        /// Latitude in radians.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public bool CoordinateSet => Longitude != 0 && Latitude != 0;


        /// <summary>
        /// Default constructor for a (0, 0) point.
        /// </summary>
        public Coordinate() { }


        /// <summary>
        /// Constructor taking longitude and latitude points in degrees.
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        public Coordinate(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        /// <summary>
        /// Great circle distance in meters to another set  using the Haversine method.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double HaversineDistanceMeters(Coordinate other)
        {
            if (Latitude == other.Latitude && Longitude == other.Longitude)
            {
                return 0;
            }

            const double R = 6371e3; // circumference of the Earth in meters

            var deltaLatitude = other.LatitudeRadians - LatitudeRadians;
            var deltaLongitude = other.LongitudeRadians - LongitudeRadians;

            var a = Math.Pow(Math.Sin(deltaLatitude / 2), 2) +

                    Math.Cos(LatitudeRadians) * Math.Cos(other.LatitudeRadians) *
                    Math.Pow(Math.Sin(deltaLongitude / 2), 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }
    }
}
