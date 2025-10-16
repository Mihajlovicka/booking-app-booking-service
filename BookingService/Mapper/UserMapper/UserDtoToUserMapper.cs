using BookingService.Model.Entity;
using BookingService.Model.Messages;

namespace BookingService.Mapper.UserMapper;

public class UserDtoToUserMapper : BaseMapper<UserDto, User>
{
    public override User Map(UserDto source)
    {
        if (!Enum.TryParse<Role>(source.Role, true, out var roleEnum))
        {
            roleEnum = Role.GUEST;
        }

        return new User
        {
            ExternalId = source.Id,
            Username = source.Username,
            Role = roleEnum
        };
    }
}