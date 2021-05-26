using System;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Conditus.DynamoDBMapper.Mappers;

namespace Conditus.DynamoDBMapper.PropertyConverters
{
    /***
    * DateTimePropertyConverter:
    * This converter converts DateTime properties to UTC Epoch and vice versa.
    *
    * Use cases:
    * - As DynamoDB sorts string attributes alphabetically, having the datetime as a number attribute 
    *   will allow for faster searches and correct sorting.
    ***/
    public class DateTimePropertyConverter : IPropertyConverter
    {
        public object FromEntry(DynamoDBEntry entry)
        {
            var primitive = entry as Primitive;

            if (primitive == null)
                throw new ArgumentOutOfRangeException();

            var value = DateTimeMapper.FromUtcUnixTimeMilliseconds((string)primitive.Value);
            return value;
        }

        public DynamoDBEntry ToEntry(object value)
        {
            if (value == null)
                return new Primitive { Value = null, Type = DynamoDBEntryType.Numeric };

            try
            {
                var dateValue = ((DateTime)value)
                    .ToUniversalTime()
                    .ToUtcUnixTimeMilliseconds();
                var stringValue = dateValue.ToString();

                return new Primitive { Value = stringValue, Type = DynamoDBEntryType.Numeric };
            }
            catch (ArgumentOutOfRangeException)
            {
                return new Primitive { Value = null, Type = DynamoDBEntryType.Numeric };
            }

        }
    }
}