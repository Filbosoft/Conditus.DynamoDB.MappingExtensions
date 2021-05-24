using System;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBMapper.Mappers
{
    public static class DateTimeMapper
    {
        public static AttributeValue GetAttributeValue(this DateTime dateTime)
        {
            var unixTime = dateTime.ToUtcUnixTimeMilliseconds();
            var value = new AttributeValue { N = unixTime.ToString() };

            return value;
        }

        public static AttributeValue GetAttributeValue(this DateTime? dateTime)
            => ((DateTime) dateTime).GetAttributeValue();

        public static long ToUtcUnixTimeMilliseconds(this DateTime dateTime)
        {
            var offset = new DateTimeOffset(dateTime, new TimeSpan());
            var unixTime = offset.ToUnixTimeMilliseconds();

            return unixTime;
        }

        public static DateTime FromUtcUnixTimeMilliseconds(string unixTime)
        {
            try
            {
                var longValue = long.Parse(unixTime);
                var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(longValue);
                return dateTimeOffset.UtcDateTime;
            }
            catch (ArgumentOutOfRangeException)
            {
                return new DateTime();
            }
        }
    }
}