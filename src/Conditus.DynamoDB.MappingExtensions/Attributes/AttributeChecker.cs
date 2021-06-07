using System.Reflection;

namespace Conditus.DynamoDB.MappingExtensions.Attributes
{
    public static class AttributeChecker
    {
        internal static bool IsMapList(PropertyInfo propertyInfo)
            => propertyInfo.GetCustomAttribute(typeof(DynamoDBMapListAttribute), false) != null;
        internal static bool IsCompositeKey(PropertyInfo propertyInfo)
            => propertyInfo.GetCustomAttribute(typeof(DynamoDBCompositeKeyAttribute), false) != null;
        internal static bool IsSelfContainingCompositeKey(PropertyInfo propertyInfo)
            => propertyInfo.GetCustomAttribute(typeof(DynamoDBSelfContainingCompositeKeyAttribute), false) != null;
    }
}