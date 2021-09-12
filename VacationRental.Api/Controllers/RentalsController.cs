using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Repos;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly RentalsRepository rentalsRepository;

        public RentalsController(IDictionary<int, RentalViewModel> rentals)
        {
            rentalsRepository = new RentalsRepository(rentals);
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            return rentalsRepository.Find(rentalId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = rentalsRepository.GetNextKey() };

            rentalsRepository.Add(model, key);

            return key;
        }

        [HttpPut]
        [Route("api/v1/rentals/{id}")]
        public RentalViewModel Update(int id, RentalBindingModel model)
        {
            return rentalsRepository.Update(id, model);
        }
    }
}
