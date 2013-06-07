using System;
using System.Collections.Generic;
using System.Text;

namespace AC.AvalonControlsLibrary.Exception
{
    /// <summary>
    /// class definition holding all exception / error strings
    /// </summary>
    internal static class ExceptionStrings
    {
        /// <summary>
        /// Message when a user tries to manual populate the Ratingselector with invalid objects
        /// </summary>
        internal const string NOT_SUPPORTED_RATINGITEM = "The RatingSelector Only supports RatingSelectorItem in the items collection";

        /// <summary>
        /// Message used when the PropertyChangedEventArgs is null
        /// </summary>
        internal const string INVALID_PROPETY_CHANGED_EVENT_ARGS = "Invalid PropertyChangedEventArgs passed";

        /// <summary>
        /// Thrown when MinRange(for the range slider) is set less than 0
        /// </summary>
        internal const string MIN_RANGE_LESS_THAN_ZERO = "Value for MinRange cannot be less than 0";

        /// <summary>
        /// Thrown when a month is passed as less than 1 or greater than 12
        /// </summary>
        internal const string INVALID_MONTH_NUMBER = "Invalid month. Must be between 1 and 12";
    }
}
