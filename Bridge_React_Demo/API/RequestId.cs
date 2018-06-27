using ProductiveRage.Immutable;
using System;

namespace Bridge_React_Demo.API
{
    public class RequestId
    {
        private static DateTime _timeOfLastId = DateTime.MinValue;
        private static int _offsetOfLastId = 0;

        private readonly DateTime _requestTime;
        private readonly int _requestOffset;

        public RequestId()
        {
            _requestTime = DateTime.Now;

            if (_timeOfLastId < _requestTime)
            {
                _offsetOfLastId = 0;
                _timeOfLastId = _requestTime;
            }
            else
            {
                _offsetOfLastId++;
            }
                
            _requestOffset = _offsetOfLastId;
        }

        public bool ComesAfter(RequestId other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (_requestTime == other._requestTime)
            {
                return _requestOffset > other._requestOffset;
            }
                
            return (_requestTime > other._requestTime);
        }

        public bool IsEqualToOrComesAfter(Optional<RequestId> other)
        {
            // If the "other" reference is no-RequestId then the "source" may be considered to
            // come after it
            if (!other.IsDefined)
            {
                return true;
            }

            return (this == other.Value) || this.ComesAfter(other.Value);
        }
    }
}