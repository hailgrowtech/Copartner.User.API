
using CopartnerUser.DataAccessLayer.Models;
using CopartnerUser.DataAccessLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CopartnerUserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IMongoRepository<Person> _peopleRepository;

        public PersonController(IMongoRepository<Person> peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }

        [HttpPost("registerPerson")]
        public async Task AddPerson(string firstName, string lastName)
        {
            var person = new Person()
            {
                FirstName = firstName,
                LastName = lastName
            };

            await _peopleRepository.InsertOneAsync(person);
        }

        [HttpGet("getPeopleData")]
        public IEnumerable<string> GetPeopleData()
        {
            var people = _peopleRepository.FilterBy(
                filter => filter.FirstName != "test",
                projection => projection.FirstName
            );
            return people;
        }
    }
}
