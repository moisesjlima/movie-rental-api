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
        public async Task<ActionResult> GetCustomers([FromQuery] string name)
        {
            try
            {
                IEnumerable<Customer> customerList;

                if (!string.IsNullOrEmpty(name))
                    customerList = _customerServices.GetCustomersByName(name);
                else
                    customerList = _customerServices.GetCustomers();

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

        [HttpPatch("{customerId:int}/telephone-number")]
        public async Task<ActionResult<Customer>> UpdateCustomerTelephoneNumber(int customerId, UpdateCustomerPhoneNumberModel updateCustomerNumberModel)
        {
            try
            {
                if (customerId != updateCustomerNumberModel.CustomerId)
                    return BadRequest(new BadRequestException("id da uri diferente do passado no corpo", "customer.bad_request"));

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
                return StatusCode(StatusCodes.Status403Forbidden, new ForbiddenException(e.Message, e.Parameter));
            }
        }
    }
}