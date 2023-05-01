using Microsoft.EntityFrameworkCore;
using movie_rental_api.Context;
using movie_rental_api.Enums;
using movie_rental_api.Exceptions;
using movie_rental_api.Models;

namespace movie_rental_api.Services
{
    public class CustomerService
    {
        private readonly MovieRentalContext _rentalContext;
        public CustomerService(MovieRentalContext rentalContext)
        {
            _rentalContext = rentalContext;
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var customerList = await _rentalContext.Customer.ToListAsync();

            return customerList;
        }

        public async Task<IEnumerable<Customer>> GetCustomersByName(string name)
        {
            var customerList = await _rentalContext.Customer.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            if (customerList.Count <= 0)
                throw new NotFoundException("nenhum cliente encontrado pelo nome", "customer.not_found");

            return customerList;
        }

        public async Task<Customer> GetCustomerById(int customerId)
        {
            var customer = await _rentalContext.Customer.FirstOrDefaultAsync(x => x.CustomerId == customerId);
            if (customer == null)
                throw new NotFoundException("nenhum cliente encontrado pelo id", "customer.not_found");

            return customer;
        }

        public async Task<Customer> CreateCustomer(CreateCustomerModel createCustomerModel)
        {
            var cpfAlreadyExist = await _rentalContext.Customer.FirstOrDefaultAsync(x => x.CPF == createCustomerModel.CPF);
            if (cpfAlreadyExist != null)
                throw new ConflictException("cpf já cadastrado", "customer.cpf_already_exist");

            var emailAlreadyExist = await _rentalContext.Customer.FirstOrDefaultAsync(x => x.Email == createCustomerModel.Email);
            if (emailAlreadyExist != null)
                throw new ConflictException("email já cadastrado", "customer.email_already_exist");

            var customer = new Customer
            {
                Name = createCustomerModel.Name,
                CPF = createCustomerModel.CPF,
                BirthDate = createCustomerModel.BirthDate.Date,
                Email = createCustomerModel.Email,
                TelephoneNumber = createCustomerModel.TelephoneNumber,
                CreateDate = DateTime.UtcNow.Date
            };

            await _rentalContext.Customer.AddAsync(customer);

            await _rentalContext.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer> UpdateCustomerTelephoneNumber(UpdateCustomerPhoneNumberModel updateCustomerNumberModel)
        {
            var customer = await GetCustomerById(updateCustomerNumberModel.CustomerId);

            customer.TelephoneNumber = updateCustomerNumberModel.TelephoneNumber;
            await _rentalContext.SaveChangesAsync();

            return customer;
        }

        public async Task DeleteCustomer(int customerId)
        {
            var customer = await GetCustomerById(customerId);

            var customerMovieRent = await _rentalContext.RentalMovie.AnyAsync(x => x.CustomerId == customer.CustomerId && x.Status != RentalMovieStatusEnum.FINISHED);

            if (customerMovieRent)
                throw new ForbiddenException("Cliente possui filme alugado, não é possivel deletar", "customer.cannot_be_deleted");

            _rentalContext.Remove(customer);
            await _rentalContext.SaveChangesAsync();
        }
    }
}