using System.ComponentModel;

namespace movie_rental_api.Enums
{
    public enum RentalMovieStatusEnum
    {
        [Description("active")]
        ACTIVE = 1,
        [Description("finished")]
        FINISHED = 2,
        [Description("overdue")]
        OVERDUE = 3
    }
}