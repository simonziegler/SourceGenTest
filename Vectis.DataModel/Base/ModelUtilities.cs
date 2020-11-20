using Vectis.DataModel;

namespace Vectis.DataModel
{
    /// <summary>
    /// Constants for use in the model.
    /// </summary>
    public class ModelUtilities
    {
        /// <summary>
        /// The number of square feet per square meter.
        /// </summary>
        public const decimal SqFeetPerSqMeter = 10.763910m;


        /// <summary>
        /// Converts the given area to square meters.
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaUnits"></param>
        /// <returns></returns>
        public static decimal ConvertToSquareMeters(decimal area, AreaUnits areaUnits = AreaUnits.SquareFeet) => (areaUnits == AreaUnits.SquareMeters) ? area : area / SqFeetPerSqMeter;


        /// <summary>
        /// Converts the given area to square feet.
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaUnits"></param>
        /// <returns></returns>
        public static decimal ConvertToSquareFeet(decimal area, AreaUnits areaUnits = AreaUnits.SquareMeters) => (areaUnits == AreaUnits.SquareFeet) ? area : area * SqFeetPerSqMeter;
        

        /// <summary>
        /// Converts the given rate to a rate per square meters.
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="areaUnits"></param>
        /// <returns></returns>
        public static decimal RatePerSquareMeter(decimal rate, AreaUnits areaUnits = AreaUnits.SquareFeet) => (areaUnits == AreaUnits.SquareMeters) ? rate : rate * SqFeetPerSqMeter;
        

        /// <summary>
        /// Converts the given rate to a rate per square feet.
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="areaUnits"></param>
        /// <returns></returns>
        public static decimal RatePerSquareFoot(decimal rate, AreaUnits areaUnits = AreaUnits.SquareMeters) => (areaUnits == AreaUnits.SquareFeet) ? rate : rate / SqFeetPerSqMeter;
    }
}
