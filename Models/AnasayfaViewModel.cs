using System.Collections.Generic;

namespace SmartCityHub.Models
{
    public class AnasayfaViewModel
    {
        public List<Duyuru> sonDuyurular { get; set; }
        public List<Ilan> AktifIlanlar { get; set; }
        public string HavaDurumu { get; set; }
        public string OtobusSaatleri { get; set; }
        public string Menu { get; set; }
    }
}