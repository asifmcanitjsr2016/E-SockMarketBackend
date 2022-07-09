using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Company.Models
{
    public class CompanyData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("code")]
        [Required(AllowEmptyStrings =false)]        
        public string Code { get; set; }
        [BsonElement("name")]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        [BsonElement("ceo")]
        [Required(AllowEmptyStrings = false)]
        public string CEO { get; set; }
        [BsonElement("turnover")]
        [Required]
        [Range(100000001,int.MaxValue, ErrorMessage = "Turn over must be greater than 10Cr")]
        public long TurnOver { get; set; }
        [BsonElement("website")]
        [Required(AllowEmptyStrings = false)]
        public string Website { get; set; }
        [BsonElement("stockexchange")]
        [Required(AllowEmptyStrings = false)]
        public string StockExcehange { get; set; }
        [BsonElement("stocks")]
        public List<Stock> Stocks {get;set;}
    }

    public class Stock
    {
        [BsonElement("price")]
        public double Price { get; set; }
        [BsonElement("datetime")]
        public DateTime DateAndTime { get; set; }
    }
}
