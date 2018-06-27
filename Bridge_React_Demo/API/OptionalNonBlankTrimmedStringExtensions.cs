using System;
using ProductiveRage.Immutable;

namespace Bridge_React_Demo.API
{
    public static class OptionalNonBlankTrimmedStringExtensions
    {
        /// <summary>
        /// If the Optional NonBlankTrimmedString has a value then it will be unwrapped directly
        /// into a string - if not, the null will be returned (this is one of the few places
        /// where null will be an acceptable value in the app and it should be only used when
        /// integrating with code that expects nulls - such as when setting attributes via
        /// React html element factories)
        /// </summary>
        public static string ToStringIfDefined(this Optional<NonBlankTrimmedString> source)
        {
            return source.IsDefined ? source.Value : null;
        }

        /// <summary>
        /// This will join two Optional NonBlankTrimmedString with a specified delimiter if
        /// they both have values. If only one of them has a value then this will be returned
        /// unaltered. If neither of them have a value then a Missing value will be returned.
        /// </summary>
        public static Optional<NonBlankTrimmedString> Add(
          this Optional<NonBlankTrimmedString> source,
          string delimiter,
          Optional<NonBlankTrimmedString> other)
        {
            if (delimiter == null)
                throw new ArgumentNullException("delimiter");

            if (!source.IsDefined && !other.IsDefined)
                return Optional<NonBlankTrimmedString>.Missing;
            else if (!source.IsDefined)
                return other;
            else if (!other.IsDefined)
                return source;

            return new NonBlankTrimmedString(source.Value.Value + delimiter + other.Value.Value);
        }
    }
}
