using Company.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IMongoCollection<CompanyData> _companies;

        public CompanyService(IOptions<CompanyDatabaseSettings> setting, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(setting.Value.DatabaseName);
            _companies = database.GetCollection<CompanyData>(setting.Value.CompanyCollectionName);
        }
        public CompanyData Create(CompanyData company)
        {
            try
            {
                string companycode = company.Code;
                var code = _companies.Find(company => company.Code == companycode).FirstOrDefault();
                if (code == null)
                {
                    company.Stocks = new List<Stock>();
                    _companies.InsertOne(company);
                    return code;
                }
                return code;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public Stock CreateStock(Stock stock, string companycode)
        {
            try
            {                                
                var code = _companies.Find(company => company.Code == companycode).FirstOrDefault();
                if (code != null)
                {
                    stock.DateAndTime=DateTime.Now;
                    
                    code.Stocks.Add(stock);   
                    
                    var filter = Builders<CompanyData>.Filter.Eq(x => x.Code, companycode);
                    var updateData = Builders<CompanyData>.Update.Set(x => x.Stocks, code.Stocks);
                    _companies.UpdateOne(filter, updateData);

                    return stock;
                }                
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CompanyData Get(string companycode)
        {
            try
            {
                var data = _companies.Find(company=>company.Code==companycode).FirstOrDefault();
                if (data.Stocks.Count == 0)
                    return data;
                var latestDate = data.Stocks?.Max(x => x.DateAndTime);
                data.Stocks = data.Stocks.Where(y => y.DateAndTime == latestDate).ToList();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<object> GetAll()
        {
            try
            {
                var alldata = _companies.Find(company => true).ToList();
                var filteredData = alldata.Select(x => new 
                { 
                 code=x.Code, name=x.Name, stocks=x.Stocks.
                 Where(y=>y.DateAndTime == x.Stocks.Max(z=>z.DateAndTime)).ToList()
                }
                ).ToList();
                return new List<object>(filteredData);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Stock> GetAllStock(string companycode, string startDate, string endDate)
        {            
            try
            {
                var StartDate = Convert.ToDateTime(startDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat).Date;
                var EndDate = Convert.ToDateTime(endDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat).Date;
                var data = _companies.Find(company => company.Code == companycode).FirstOrDefault();
                if (data == null)
                    return null;

                var stocks = new List<Stock>();
                for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
                {                    
                    stocks.AddRange(data.Stocks.Where(y => y.DateAndTime.Date == date).ToList());
                }
                return stocks;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public bool Remove(string companycode)
        {
            try
            {
                var id = _companies.Find(company => company.Code == companycode).FirstOrDefault().Id;
                _companies.DeleteOne(company => company.Id == id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
