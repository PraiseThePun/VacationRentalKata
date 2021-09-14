using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly RentalService rentalService;

        public RentalsController(IDictionary<int, RentalViewModel> rentals)
        {
            rentalService = new RentalService(rentals);
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            return rentalService.Find(rentalId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = rentalService.GetNextKey() };

            rentalService.Add(model, key);

            return key;
        }

        [HttpPut]
        [Route("api/v1/rentals/{id}")]
        public RentalViewModel Update(int id, RentalBindingModel model)
        {
            return rentalService.Update(id, model);
        }
    }
}
