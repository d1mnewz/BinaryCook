using BinaryCook.Core.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BinaryCook.Core.Data.Extensions
{
    public static class EntityTypeConfigurationExtensions
    {
        public static PropertyBuilder NVarCharMax(this PropertyBuilder cfg) => cfg.NVarChar();

        public static PropertyBuilder NVarChar(this PropertyBuilder cfg, int? maxLength = null)
        {
            cfg = maxLength.HasValue ? cfg.HasMaxLength(maxLength.Value) : cfg;
            return cfg.Required();
        }

        public static PropertyBuilder Optional(this PropertyBuilder cfg) => cfg.IsRequired(false);

        public static PropertyBuilder Required(this PropertyBuilder cfg) => cfg.IsRequired(true);

        //TODO: try to add IUnremovable constraint 
        public static EntityTypeBuilder<T> WithDefaultUnremovableFilter<T>(this EntityTypeBuilder<T> cfg) where T : class =>
            cfg.HasQueryFilter(UnremovableExtensions.DefaultNotDeletedExpression<T>());
    }
}