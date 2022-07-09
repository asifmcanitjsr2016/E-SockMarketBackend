using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.Models;
using Company.Services;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Company.Controllers
{
    [Route("/api/v1.0/market/company")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        // GET: CompaniesController       
        [HttpPost("register")]
        public ActionResult<CompanyData> Register([FromBody] CompanyData company)
        {            
            var created = _companyService.Create(company);
            if (created==null)
            {
                return company;                
            }
            return StatusCode(409, $"Company with Code={created.Code} already exists!");

        }

        // GET: CompaniesController/Info/5        
        [HttpGet("info")]
        public ActionResult<CompanyData> Info(string companycode)
        {
            
            var comp = _companyService.Get(companycode);
            if (comp == null)
            {
                return NotFound($"Company with Code={companycode} not found!");
            }
            return comp;
        }

        // GET: CompaniesController/GetAllCompanyList        
        [HttpGet("getall")]
        public ActionResult<List<object>> GetAll()
        {
            var alldata = _companyService.GetAll();
            if (alldata.Count() == 0)
            {
                return NotFound("No records found!");
            }
            return alldata;
        }

        // GET: CompaniesController/Delete/5       
        [HttpDelete("delete")]
        public ActionResult<bool> Delete(string companycode)
        {
            if (!_companyService.Remove(companycode))
            {
                return NotFound($"Company with Code={companycode} not found!");
            }
            return true;
        }
    }
}
