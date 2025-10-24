using BookingService.Model.Dto;
using BookingService.Model.Entity;
using BookingService.Repository.Contract;

namespace BookingService.Mapper.ReservationMapper;

public class ReservationToReservationDtoMapper(IRepositoryManager repositoryManager) : BaseMapper<Reservation, ReservationDto>
{
    public override async Task<ReservationDto> Map(Reservation source)
    {
        var grades = (await repositoryManager.ReviewRepository
                .GetByEntityInfo(source.Accommodation.ExternalId.ToString()))
            .Select(x => x.Grade);
        
        

        var enumerable = grades as int[] ?? grades.ToArray();
        var hasReview = enumerable.Length != 0;
        var averageGrade = enumerable.Length == 0 ? 0 : enumerable.Average();
        
        var dto = new ReservationDto
        {
            Id = source.Id,
            AccommodationExternalId = source.Accommodation.ExternalId,
            GuestNumber = source.GuestNumber,
            GuestUsername = source.GuestUsername,
            FinalPrice = source.FinalPrice,
            StartDate = source.StartDate.ToString("yyyy-MM-dd"),
            EndDate = source.EndDate.ToString("yyyy-MM-dd"),
            AccommodationName = source.Accommodation.Name,
            HostUsername = source.Accommodation.Owner,
            AverageGrade = averageGrade,
            HasReview = hasReview
        };

        return dto;
    }
}