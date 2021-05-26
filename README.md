# Conditus DynamoDBMapper
A NuGet package that helps convert AttributeValues to entities and vice versa.  

The package includes:
- Extension methods for common types that maps their value to a AttributeValue and AttributeValue maps
- Extension methods which can be called on a DynamoDB map attribute (M) value to map it to a entity
- PropertyConverters for IDynamoDBContext for commonly converted types

**Supports nested mapping**

## Testing
As testing could be quite extensive on this package, tests will be added if errors occur in systems using this package or if new logic is added.  
The logic of the package started as part of the Conditus Order service and has been tested in it's initial fase.

## Nested mapping
The package uses reflection to map nested lists and objects.

**Sources:**
- https://stackoverflow.com/questions/25757121/c-sharp-how-to-set-propertyinfo-value-when-its-type-is-a-listt-and-i-have-a-li
- https://stackoverflow.com/questions/14888075/create-generic-listt-with-reflection

## Terminology
**Entity:** a class or primitive instance (object, MyClass, string, int, decimal, Enum...).