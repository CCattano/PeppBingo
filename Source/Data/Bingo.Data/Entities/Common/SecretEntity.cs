namespace Pepp.Web.Apps.Bingo.Data.Entities.Common
{
    /// <summary>
    /// A generic ValueDetailDesc entity carrying information 
    /// about a context-specific data point
    /// </summary>
    public class SecretEntity
    {
        /// <summary>
        /// The source this value detail is associated with
        /// </summary>
        public string Source;
        /// <summary>
        /// The type of value detail this is.
        /// </summary>
        /// <remarks>
        /// The value of this property is contextual to the source
        /// </remarks>
        public string Type;
        /// <summary>
        /// The value of this value detail
        /// </summary>
        public string Value;
        /// <summary>
        /// The description of this value detail
        /// </summary>
        public string Description;
    }
}
