using System;
using BinaryCook.Core.Code;
using BinaryCook.Core.Data.Entities;
using BinaryCook.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BinaryCook.Core.Domain.Model.Recipes
{
	[Serializable]
	public class RecipeStep : Entity
	{
		private RecipeStep()
		{
		}

		protected RecipeStep(string action, int priority, TimeSpan duration) : this()
		{
			Requires.NotNull(action, nameof(action));
			Requires.That(priority > 0, $"{nameof(priority)} must be more than zero.");
			Requires.NotNull(duration, nameof(duration));
			Requires.That(duration > TimeSpan.Zero, $"{nameof(duration)} must be more than zero.");

			Action = action;
			Priority = priority;
			DurationInSeconds = duration.TotalSeconds;
		}

		private Guid RecipeId { get; set; }

		public string Action { get; set; }
		public int Priority { get; set; }
		public TimeSpan Duration => TimeSpan.FromSeconds(DurationInSeconds);
		private double DurationInSeconds { get; set; }

		public void Update(Maybe<string> action = null)
		{
			action.Required()
				.NotEmpty(nameof(action))
				.Do(x => Action = x);
		}
	}

	public class RecipeStepEntityConfiguration : EntityConfiguration<RecipeStep>
	{
		protected override void Initialize(ModelBuilder builder, EntityTypeBuilder<RecipeStep> cfg)
		{
			cfg.Property("RecipeId");
			cfg.Ignore(x => x.Duration);
			cfg.Property("DurationInSeconds");

			base.Initialize(builder, cfg);
		}
	}
}