using movie_rental_api.Context;
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

        public IEnumerable<Customer> GetCustomers()
        {
            var customerList = _rentalContext.Customer.ToList();

            return customerList;
        }

        public IEnumerable<Customer> GetCustomersByName(string name)
        {
            var customerList = _rentalContext.Customer.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
            if (customerList.Count <= 0)
                throw new NotFoundException("nenhum cliente encontrado pelo nome", "customer.not_found");

            return customerList;
        }

        public Customer GetCustomerById(int customerId)
        {
            var customer = _rentalContext.Customer.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer == null)
                throw new NotFoundException("nenhum cliente encontrado pelo id", "customer.not_found");

            return customer;
        }

        public Customer CreateCustomer(CreateCustomerModel createCustomerModel)
        {
            var cpfAlreadyExist = _rentalContext.Customer.FirstOrDefault(x => x.CPF == createCustomerModel.CPF);
            if (cpfAlreadyExist != null)
                throw new ConflictException("cpf já cadastrado", "customer.cpf_already_exist");

            var emailAlreadyExist = _rentalContext.Customer.FirstOrDefault(x => x.Email == createCustomerModel.Email);
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

            _rentalContext.Customer.Add(customer);

            _rentalContext.SaveChanges();

            return customer;
        }

        public Customer UpdateCustomerTelephoneNumber(UpdateCustomerPhoneNumberModel updateCustomerNumberModel)
        {
            var customer = GetCustomerById(updateCustomerNumberModel.CustomerId);

            customer.TelephoneNumber = updateCustomerNumberModel.TelephoneNumber;
            _rentalContext.SaveChanges();

            return customer;
        }

        public void DeleteCustomer(int customerId)
        {
            var customer = GetCustomerById(customerId);

            var customerMovieRent = _rentalContext.RentalMovie.Any(x => x.CustomerId == customer.CustomerId);

            if (customerMovieRent)
                throw new ForbiddenException("Cliente possui filme alugado, não é possivel deletar", "customer.cannot_be_deleted");

            _rentalContext.Remove(customer);
            _rentalContext.SaveChanges();
        }
    }
}