using System;
using Conditus.DynamoDB.MappingExtensions.Mappers;
using Conditus.DynamoDB.MappingExtensions.UnitTests.MapperTests.CompositeKeyMapperTests.TestClasses;
using FluentAssertions;
using Xunit;

using static Conditus.DynamoDB.MappingExtensions.Constants.MappingConstants;

namespace Conditus.DynamoDB.MappingExtensions.UnitTests.MapperTests.CompositeKeyMapperTests
{
    public class GetSelfContainingCompositeKeyTests
    {
        [Fact]
        public void GetSelfContainingCompositeKey_WithOnlyStringKeys_ShouldReturnCompositeKeyOfTheValuesOfTheKeyProperties()
        {
            //Given
            var entity = new ClassWithSelfContainingCompositeStringKey
            {
                SelfContainingCompositeKey = "CompKeyValue",
                KeyProperty1 = "Key1Value",
                KeyProperty2 = "Key2Value"
            };

            //When
            var compositeKey = SelfContainingCompositeKeyMapper.GetSelfContainingCompositeKey(entity, nameof(ClassWithSelfContainingCompositeStringKey.SelfContainingCompositeKey));

            //Then
            var expectedKeyParts = new string[]
            {
                entity.SelfContainingCompositeKey,
                entity.KeyProperty1,
                entity.KeyProperty2
            };
            var expectedCompositeKey = string.Join(COMPOSITE_KEY_SEPARATOR, expectedKeyParts);

            compositeKey.Should().Be(expectedCompositeKey);
        }

        [Fact]
        public void GetSelfContainingCompositeKey_WithDateTimePropertyAndStringKeys_ShouldReturnCompositeKeyOfTheStringValueOfTheKeyProperties()
        {
            //Given
            var entity = new ClassWithSelfContainingCompositeDateTimeKey
            {
                SelfContainingCompositeKey = DateTime.UtcNow,
                KeyProperty1 = "Key1Value",
                KeyProperty2 = "Key2Value"
            };

            //When
            var compositeKey = SelfContainingCompositeKeyMapper.GetSelfContainingCompositeKey(entity, nameof(ClassWithSelfContainingCompositeStringKey.SelfContainingCompositeKey));

            //Then
            var expectedKeyParts = new string[]
            {
                StringMapper.ConvertToDynamoDBStringValue(entity.SelfContainingCompositeKey),
                entity.KeyProperty1,
                entity.KeyProperty2
            };
            var expectedCompositeKey = string.Join(COMPOSITE_KEY_SEPARATOR, expectedKeyParts);
            
            compositeKey.Should().Be(expectedCompositeKey);
        }

        [Fact]
        public void GetSelfContainingCompositeKey_WithStringPropertyAndDateTimeKey_ShouldReturnCompositeKeyOfTheStringValueOfTheKeyProperties()
        {
            //Given
            var entity = new ClassWithSelfContainingCompositeStringKeyIncludingDateTimeKey
            {
                SelfContainingCompositeKey = "PropertyValue",
                DateTimeKeyProperty1 = DateTime.UtcNow,
                KeyProperty2 = "Key2Value"
            };

            //When
            var compositeKey = SelfContainingCompositeKeyMapper.GetSelfContainingCompositeKey(entity, nameof(ClassWithSelfContainingCompositeStringKey.SelfContainingCompositeKey));

            //Then
            var expectedKeyParts = new string[]
            {
                entity.SelfContainingCompositeKey,
                StringMapper.ConvertToDynamoDBStringValue(entity.DateTimeKeyProperty1),
                entity.KeyProperty2
            };
            var expectedCompositeKey = string.Join(COMPOSITE_KEY_SEPARATOR, expectedKeyParts);
            
            compositeKey.Should().Be(expectedCompositeKey);
        }

        [Fact]
        public void GetSelfContainingCompositeKey_WithNullableKeys_ShouldReturnCompositeKeyOfTheStringValueOfTheKeyProperties()
        {
            //Given
            var entity = new ClassWithSelfContainingCompositeNullableKeys
            {
                SelfContainingCompositeKey = "PropertyValue",
                DateTimeKeyProperty1 = DateTime.UtcNow,
                IntProperty1 = 1
            };

            //When
            var compositeKey = SelfContainingCompositeKeyMapper.GetSelfContainingCompositeKey(entity, nameof(ClassWithSelfContainingCompositeStringKey.SelfContainingCompositeKey));

            //Then
            var expectedKeyParts = new string[]
            {
                entity.SelfContainingCompositeKey,
                StringMapper.ConvertToDynamoDBStringValue(entity.DateTimeKeyProperty1),
                entity.IntProperty1.ToString()
            };
            var expectedCompositeKey = string.Join(COMPOSITE_KEY_SEPARATOR, expectedKeyParts);
            
            compositeKey.Should().Be(expectedCompositeKey);
        }
    }
}