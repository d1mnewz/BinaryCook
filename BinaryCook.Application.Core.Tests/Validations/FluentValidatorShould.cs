using System;
using System.Threading;
using System.Threading.Tasks;
using BinaryCook.Application.Core.Validations.Fluent;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace BinaryCook.Application.Core.Tests.Validations
{
    public class FluentValidatorShould
    {
        private readonly Core.Validations.IValidator<Item> _validator;

        public class Item
        {
            public Item(string name, string email)
            {
                Name = name;
                Email = email;
            }

            public string Name { get; }
            public string Email { get; }
        }

        public class ItemValidator : FluentValidator<Item>
        {
            public const string IncorrectLength = "Has incorrect length";
            public const string IncorrectFormat = "Has incorrect format";
            public const string IsNull = "IsNull";
            public const string NotEqualsAbc = "Not equals abc";

            public ItemValidator()
            {
                RuleFor(x => x.Name).Length(0, 10).WithMessage(IncorrectLength)
                    .NotNull().WithMessage(IsNull)
                    .MustAsync(BeValid).WithMessage(NotEqualsAbc);
                RuleFor(x => x.Email).EmailAddress().WithMessage(IncorrectFormat)
                    .NotNull().WithMessage(IsNull);
            }

            private async Task<bool> BeValid(string name, CancellationToken ct)
            {
                return string.Equals(name, "abc", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public FluentValidatorShould()
        {
            _validator = new ItemValidator();
        }

        [Fact]
        public async Task BeValid()
        {
            var item = new Item("abc", "name@name.com");
            var result = await _validator.ValidateAsync(item);

            result.IsValid.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.Result.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task BeNotValid()
        {
            var item = new Item("asdadasaddsadsasdasaddsada", "name");
            var result = await _validator.ValidateAsync(item);

            result.IsValid.Should().BeFalse();
            result.Result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task HaveCorrectMessage()
        {
            var item = new Item("asdadasaddsadsasdasaddsada", "name");
            var result = await _validator.ValidateAsync(item);

            result.IsValid.Should().BeFalse();
            result.Result.Should().NotBeNullOrEmpty();

            result.Result[nameof(Item.Name)][0].Should().Be(ItemValidator.IncorrectLength);
            result.Result[nameof(Item.Name)][1].Should().Be(ItemValidator.NotEqualsAbc);
            result.Result[nameof(Item.Email)][0].Should().Be(ItemValidator.IncorrectFormat);

            item = new Item(null, null);
            result = await _validator.ValidateAsync(item);

            result.IsValid.Should().BeFalse();
            result.Result.Should().NotBeNullOrEmpty();

            result.IsValid.Should().BeFalse();
            result.Result.Should().NotBeNullOrEmpty();

            result.Result[nameof(Item.Name)][0].Should().Be(ItemValidator.IsNull);
            result.Result[nameof(Item.Name)][1].Should().Be(ItemValidator.NotEqualsAbc);
            result.Result[nameof(Item.Email)][0].Should().Be(ItemValidator.IsNull);
        }
    }
}