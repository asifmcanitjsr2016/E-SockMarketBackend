using Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.Services
{
    public interface ICompanyService
    {
        Stock CreateStock(Stock stock, string companycode);
        CompanyData Create(CompanyData company);
        List<object> GetAll();
        List<Stock> GetAllStock(string companycode, string startDate, string endDate);
        CompanyData Get(string companycode);
        bool Remove(string companycode);        
    }
}
