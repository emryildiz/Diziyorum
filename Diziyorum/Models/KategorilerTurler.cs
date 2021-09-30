using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diziyorum.Models
{
    public class KategorilerTurler
    {
        public int selectedKategoriId { get; set; }

        public IEnumerable<SelectListItem> kategoriler;
        public int selectedTurId { get; set; }

        public IEnumerable<SelectListItem> turler;







    }
}