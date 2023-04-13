using System.ComponentModel;

namespace movie_rental_api.Enums
{
    public enum RentalMovieStatusEnum
    {
        [Description("ativo")]
        ATIVO = 1,
        [Description("encerrado")]
        ENCERRADO = 2,
        [Description("atrasado")]
        ATRASADO = 3
    }
}