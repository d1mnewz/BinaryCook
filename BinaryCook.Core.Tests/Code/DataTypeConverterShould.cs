using System;
using System.Collections.Generic;
using BinaryCook.Core.Code;
using FluentAssertions;
using Xunit;

namespace BinaryCook.Core.Tests.Code
{
    public class DataTypeConverterShould
    {
        public enum TestEnum
        {
            A = 1,
            B = 2
        }

        [Theory]
        [InlineData("True", true)]
        [InlineData("true", true)]
        [InlineData("False", false)]
        [InlineData("false", false)]
        public void ConvertToBool(string value, bool expected)
        {
            var result = DataTypeConverter.Convert<bool?>(value, null);

            result.Should().NotBeNull()
                .And.Be(expected);
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("-1", -1)]
        [InlineData("0", 0)]
        public void ConvertToInt(string value, int expected)
        {
            var result = DataTypeConverter.Convert<int?>(value, null);

            result.Should().NotBeNull()
                .And.Be(expected);
        }

        [Theory]
        [InlineData("1", TestEnum.A)]
        [InlineData("a", TestEnum.A)]
        [InlineData("A", TestEnum.A)]
        [InlineData("2", TestEnum.B)]
        [InlineData("b", TestEnum.B)]
        [InlineData("B", TestEnum.B)]
        public void ConvertToEnum(string value, TestEnum expected)
        {
            var result = DataTypeConverter.Convert<TestEnum?>(value, null);

            result.Should().NotBeNull()
                .And.Be(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(10)]
        public void ReturnTheSameInt(int value)
        {
            var valueString = DataTypeConverter.ConvertAsString(value);
            var result = DataTypeConverter.Convert<int>(valueString);

            result.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ReturnTheSameBool(bool value)
        {
            var valueString = DataTypeConverter.ConvertAsString(value);
            var result = DataTypeConverter.Convert<bool>(valueString);
            result.Should().Be(value);
        }

        [Theory]
        [InlineData("aaaa")]
        [InlineData("sadasdas23214r32asdasdsas")]
        public void ReturnTheSameString(string value)
        {
            var valueString = DataTypeConverter.ConvertAsString(value);
            var result = DataTypeConverter.Convert<string>(valueString);
            result.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(GetTypedData))]
        public void NotConvertToWrongType(string value, Type type)
        {
            DataTypeConverter.TryConvert(type, value).Should().BeFalse();
        }

        public static IEnumerable<object[]> GetTypedData()
        {
            yield return new object[] {"yes", typeof(bool)};
            yield return new object[] {"no", typeof(bool)};
            yield return new object[] {"", typeof(int)};
            yield return new object[] {"", typeof(TestEnum)};
        }
    }
}