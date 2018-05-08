using System;
using BinaryCook.Core.Code;
using BinaryCook.Core.Data.Entities;
using BinaryCook.Core.Data.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BinaryCook.Core.Domain.Model.Common
{
	[Serializable]
	public class Measure : ValueObject<Measure>
	{
		private Measure()
		{
		}

		private Measure(decimal amount, string unit)
		{
			Requires.NotEmpty(unit, nameof(unit));
			Requires.That(amount >= 0, $"{nameof(amount)} must be greater or equals to 0.");

			Amount = amount;
			Unit = unit.ToUpper();
		}

		public decimal Amount { get; private set; }

		public string Unit { get; private set; }

		public static Measure Oz(decimal amount) => new Measure(amount, "OZ");
		public static Measure Cup(decimal amount) => new Measure(amount, "CUP");
		public static Measure Tbsp(decimal amount) => new Measure(amount, "TBSP");
	}

	public class MeasureConfiguration
	{
		public static void Configure<TOwner>(ReferenceOwnershipBuilder<TOwner, Measure> builder) where TOwner : class
		{
			builder.Property(x => x.Amount);
			builder.Property(x => x.Unit).NVarChar();
		}
	}
}