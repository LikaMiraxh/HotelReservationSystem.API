namespace HotelReservationSystem.API.Auth
{
    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel()
            {
                Username= "admin",
                Password= "admin",
                Email = "admin@admin.com",
                FullName = "Administrator",
                Role = "Administrator"
            },
            new UserModel()
            {
                Username = "user",
                Password = "user",
                Role= "user",
                FullName = "User",
                Email = "user@user.com"
            }
        };
    }
}
