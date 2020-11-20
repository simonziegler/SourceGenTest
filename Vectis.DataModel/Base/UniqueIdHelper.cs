using System;

namespace Vectis.DataModel
{
    /// <summary>
    /// A helper class to create new unique strings for use either as partition keys or object ids.
    /// </summary>
    public static class UniqueIdHelper
    {
        /// <summary>
        /// Returns a new id in the format "[timestamp ticks]|[guid]"
        /// </summary>
        /// <returns></returns>
        public static string NewId() => $"{DateTime.UtcNow.Ticks}|{Guid.NewGuid()}";
    }
}
