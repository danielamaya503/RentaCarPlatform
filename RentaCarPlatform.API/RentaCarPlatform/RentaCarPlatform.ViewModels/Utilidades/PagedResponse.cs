using System;
using System.Collections.Generic;
using System.Text;

namespace RentaCarPlatform.ViewModels.Utilidades
{
    public class PagedResponse<T>
    {
        public List<T> Items { get; set; } = [];
        public int TotalRegistros { get; set; }
        public int Pagina { get; set; }
        public int TamanoPagina { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamanoPagina);
        public bool TienePaginaAnterior => Pagina > 1;
        public bool TienePaginaSiguiente => Pagina < TotalPaginas;
    }
}
