using Microsoft.AspNetCore.Mvc;
using movie_rental_api.Context;
using movie_rental_api.Models;

namespace movie_rental_api.Controllers
{
    [Route("v1/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly MovieRentalContext _rentalContext;
        public CustomerController(MovieRentalContext rentalContext)
        {
            _rentalContext = rentalContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetCustomers()
        {
            var customerList = _rentalContext.Customer.ToList();

            return Ok(customerList);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult> GetCustomersByName(string name)
        {
            var customerList = _rentalContext.Customer.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
            if (customerList.Count <= 0)
                return NotFound("Nenhum cliente encontrado pelo nome");

            return Ok(customerList);
        }

        [HttpGet("{customerId:int}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int customerId)
        {
            var customer = _rentalContext.Customer.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer == null)
                return NotFound("Cliente não encontrado");

            return customer;
        }

        [HttpPost]
        public async Task<ActionResult<CreateCustomerModel>> CreateCustomer(CreateCustomerModel createCustomerModel)
        {
            var emailAlreadyExist = _rentalContext.Customer.FirstOrDefault(x => x.Email == createCustomerModel.Email);
            if (emailAlreadyExist != null)
                return Conflict("Email já cadastrado");

            var customer = new Customer
            {
                Name = createCustomerModel.Name,
                CPF = createCustomerModel.CPF,
                BirthDate = createCustomerModel.BirthDate,
                Email = createCustomerModel.Email,
                TelephoneNumber = createCustomerModel.TelephoneNumber,
            };

            _rentalContext.Customer.Add(customer);

            _rentalContext.SaveChanges();

            return Created($"v1/customer/{customer.CustomerId}", customer);
        }

        [HttpPatch("telephone-number")]
        public async Task<ActionResult<Customer>> UpdateCustomerTelephoneNumber(UpdateCustomerPhoneNumberModel updateCustomerNumberModel)
        {
            var customer = _rentalContext.Customer.FirstOrDefault(x => x.CustomerId == updateCustomerNumberModel.CustomerId);
            if (customer == null)
                return NotFound("Cliente não encontrado");

            customer.TelephoneNumber = updateCustomerNumberModel.TelephoneNumber;
            _rentalContext.SaveChanges();

            return Ok(customer);
        }

        [HttpDelete("{customerId:int}")]
        public async Task<ActionResult> DeleteCustomer(int customerId)
        {
            var customer = _rentalContext.Customer.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer == null)
                return NotFound("Cliente não encontrado");

            var customerMovieRent = _rentalContext.RentalMovie.Any(x => x.CustomerId == customer.CustomerId);

            if (customerMovieRent)
                return StatusCode(StatusCodes.Status403Forbidden, "Cliente possui filme alugado, não é possivel deletar");

            _rentalContext.Remove(customer);
            _rentalContext.SaveChanges();

            return NoContent();
        }
    }
}