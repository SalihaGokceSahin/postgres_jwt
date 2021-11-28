using core_web_api_postgresql_crud.RequestModel;
using Data_Layer;
using Data_Layer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace core_web_api_postgresql_crud.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly PostgreSqlExampleDbContext db;
        private readonly IConfiguration configuration;
        public EmployeeController(PostgreSqlExampleDbContext db, IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;
        }
        [HttpPost("add")]
        public IActionResult Add([FromBody]EmployeeRequestModel request)
        {
            var model = new Employee();
            model.NameSurname = request.NameSurname;
            model.Title = request.Title;
            db.Add(model);
            db.SaveChanges();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier, model.NameSurname.ToString()),
                    new Claim(ClaimTypes.Name, model.Title)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok("The data successfully added to the database. Token is here: " + tokenString);
        }
    }
}
