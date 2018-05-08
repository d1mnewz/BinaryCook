using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BinaryCook.Core.Extensions;
using ProtoBuf.Meta;

namespace BinaryCook.Infrastructure.Caching.Configurations
{
    public interface ICacheConfigurationProperty
    {
        int Order { get; }
        string Name { get; }
        void SetOrder(int order);
    }

    public class CacheConfigurationProperty : ICacheConfigurationProperty,
        IEqualityComparer<ICacheConfigurationProperty>
    {
        protected readonly PropertyInfo PropertyInfo;

        internal CacheConfigurationProperty(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }


        public virtual int Order { get; private set; }
        public virtual string Name => PropertyInfo.Name;

        public virtual void SetOrder(int order)
        {
            Order = order;
        }

        public bool Equals(ICacheConfigurationProperty x, ICacheConfigurationProperty y) =>
            string.Equals(x?.Name, y?.Name, StringComparison.InvariantCultureIgnoreCase);

        public int GetHashCode(ICacheConfigurationProperty obj) => GetType().GetHashCode() * 907 + (Name?.GetHashCode() ?? 0);
    }

    public class CacheConfigurationProperty<TProp> : CacheConfigurationProperty
    {
        private CacheConfigurationProperty(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public static CacheConfigurationProperty<TProp> Create<TModel>(Expression<Func<TModel, TProp>> expression) =>
            new CacheConfigurationProperty<TProp>(expression.GetPropertyInfo());
    }

    public interface ICacheConfiguration
    {
        Type Type { get; }
        void Initialize(MetaType meta);
    }

    public abstract class CacheConfiguration<T> : ICacheConfiguration where T : class
    {
        private readonly HashSet<ICacheConfigurationProperty> _configurations = new HashSet<ICacheConfigurationProperty>();

        protected CacheConfiguration(bool extractAllProperties = false)
        {
            if (extractAllProperties)
                ExtractAllProperties();
        }

        protected void ExtractAllProperties()
        {
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var propertyInfo in properties)
            {
                if (!propertyInfo.CanRead || !propertyInfo.CanWrite) continue;

                var property = new CacheConfigurationProperty(propertyInfo);
                property.SetOrder(_configurations.Count + 1);
                _configurations.Add(property);
            }
        }

        protected ICacheConfigurationProperty For<TProp>(Expression<Func<T, TProp>> expression)
        {
            var prop = CacheConfigurationProperty<TProp>.Create(expression);
            prop.SetOrder(_configurations.Count + 1);
            _configurations.Add(prop);
            return prop;
        }

        public Type Type => typeof(T);

        public void Initialize(MetaType meta)
        {
            foreach (var configuration in _configurations.OrderBy(x => x.Order))
            {
                meta.Add(configuration.Order, configuration.Name);
            }
        }
    }
}