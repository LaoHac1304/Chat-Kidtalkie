using AutoMapper;

namespace ChatKid.Api.Services.Mapping
{
    public static class MappingExtension
    {
        public static void IgnoreNull<TSource, TDestination> (this IMappingExpression<TSource, TDestination> map)
        {
            map.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != default));
        }
    }
}
