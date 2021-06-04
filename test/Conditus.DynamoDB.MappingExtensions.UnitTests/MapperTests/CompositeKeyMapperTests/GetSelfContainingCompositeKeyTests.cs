using System;
using System.Collections.Generic;
using Conditus.DynamoDB.MappingExtensions.Mappers;
using Conditus.DynamoDB.MappingExtensions.UnitTests.MapperTests.CompositeKeyMapperTests.TestClasses;
using FluentAssertions;
using Xunit;

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
            var compositeKey = CompositeKeyMapper.GetSelfContainingCompositeKey(entity, nameof(ClassWithSelfContainingCompositeStringKey.SelfContainingCompositeKey));

            //Then
            var expectedKeyParts = new string[]
            {
                entity.SelfContainingCompositeKey,
                entity.KeyProperty1,
                entity.KeyProperty2
            };
            var expectedCompositeKey = string.Join(CompositeKeyMapper.KEY_SEPARATOR, expectedKeyParts);

            compositeKey.Should().Be(expectedCompositeKey);
        }

        [Fact]
        public void GetSelfContainingCompositeKey_WithDateTimeAndString_ShouldReturnCompositeKeyOfTheStringValueOfTheKeyProperties()
        {
            //Given
            var entity = new ClassWithSelfContainingCompositeDateTimeKey
            {
                SelfContainingCompositeKey = DateTime.UtcNow,
                KeyProperty1 = "Key1Value",
                KeyProperty2 = "Key2Value"
            };

            //When
            var compositeKey = CompositeKeyMapper.GetSelfContainingCompositeKey(entity, nameof(ClassWithSelfContainingCompositeStringKey.SelfContainingCompositeKey));

            //Then
            var expectedKeyParts = new string[]
            {
                StringMapper.ConvertToString(entity.SelfContainingCompositeKey, entity.SelfContainingCompositeKey.GetType()),
                entity.KeyProperty1,
                entity.KeyProperty2
            };
            var expectedCompositeKey = string.Join(CompositeKeyMapper.KEY_SEPARATOR, expectedKeyParts);
            
            compositeKey.Should().Be(expectedCompositeKey);
        }
    }
}