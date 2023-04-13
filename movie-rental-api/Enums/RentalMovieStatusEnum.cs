using System.ComponentModel;

namespace movie_rental_api.Enums
{
    public enum RentalMovieStatusEnum
    {
        [Description("ativo")]
        ACTIVE = 1,
        [Description("encerrado")]
        FINISHED = 2,
        [Description("atrasado")]
        OVERDUE = 3
    }
}