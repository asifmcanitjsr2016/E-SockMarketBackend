using Company.Models;
using Company.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Company.Controllers
{
    [Route("/api/v1.0/market/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public StockController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        // GET: api/<StockController>
        [HttpGet("get")]
        public List<Stock> Get(string companycode, string startdate, string enddate)
        {
            return _companyService.GetAllStock(companycode, startdate, enddate);
        }        

        // POST api/<StockController>
        [HttpPost("add")]
        public ActionResult<Stock> Post([FromBody] Stock stock, string companycode)
        {
            return _companyService.CreateStock(stock, companycode);
        }        
    }
}
