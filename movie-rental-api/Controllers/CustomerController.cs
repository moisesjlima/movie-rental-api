using Microsoft.AspNetCore.Mvc;
using movie_rental_api.Context;
using movie_rental_api.Exceptions;
using movie_rental_api.Models;
using movie_rental_api.Services;

namespace movie_rental_api.Controllers
{
    [Route("v1/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerServices;
        public CustomerController(CustomerService customerService)
        {
            _customerServices = customerService;
        }

        [HttpGet]
        public async Task<ActionResult> GetCustomers()
        {
            var customerList = _customerServices.GetCustomers();

            return Ok(customerList);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult> GetCustomersByName(string name)
        {
            try
            {
                var customerList = _customerServices.GetCustomersByName(name);

                return Ok(customerList);
            }
            catch (NotFoundException e)
            {
                return NotFound(new NotFoundException(e.Message, e.Parameter));
            }
        }

        [HttpGet("{customerId:int}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int customerId)
        {
            try
            {
                var customerList = _customerServices.GetCustomerById(customerId);

                return Ok(customerList);
            }
            catch (NotFoundException e)
            {
                return NotFound(new NotFoundException(e.Message, e.Parameter));
            }
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(CreateCustomerModel createCustomerModel)
        {
            try
            {
                var customer = _customerServices.CreateCustomer(createCustomerModel);

                return Created($"v1/customer/{customer.CustomerId}", customer);
            }
            catch (ConflictException e)
            {
                return Conflict(new ConflictException(e.Message, e.Parameter));
            }

        }

        [HttpPatch("telephone-number")]
        public async Task<ActionResult<Customer>> UpdateCustomerTelephoneNumber(UpdateCustomerPhoneNumberModel updateCustomerNumberModel)
        {
            try
            {
                var customer = _customerServices.UpdateCustomerTelephoneNumber(updateCustomerNumberModel);

                return Ok(customer);
            }
            catch (NotFoundException e)
            {
                return NotFound(new NotFoundException(e.Message, e.Parameter));
            }
        }

        [HttpDelete("{customerId:int}")]
        public async Task<ActionResult> DeleteCustomer(int customerId)
        {
            try
            {
                _customerServices.DeleteCustomer(customerId);

                return NoContent();
            }
            catch (NotFoundException e)
            {
                return NotFound(new NotFoundException(e.Message, e.Parameter));
            }
            catch (ForbiddenException e)
            {
                return NotFound(new ForbiddenException(e.Message, e.Parameter));
            }
        }
    }
}