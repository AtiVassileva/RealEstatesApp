using System.Xml.Serialization;

namespace RealEstates.Services.Models
{
    [XmlType("District")]
    public class DistrictInfoDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("AveragePricePerSquareMeter")]
        public decimal AveragePricePerSquareMeter { get; set; }

        [XmlElement("PropertiesCount")]
        public int PropertiesCount { get; set; }
    }
}
