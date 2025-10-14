namespace BookingService.Mapper;

public interface IBaseMapper<TSource, TDestination>
{
    Task<TDestination> Map(TSource source);
    TSource ReverseMap(TDestination destination);

}