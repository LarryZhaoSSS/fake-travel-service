using System.Collections.Generic;

namespace travel_service.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool IsMappingExists<TSource, TDestination>(string fields);
        bool IsPropertiesExists<T>(string fields);
    }
}