using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ejer02
{
    [XmlRoot("Raiz")]
    public class ListaProducto
    {

        [XmlArrayItem(typeof(Producto))]
        public List<Producto> lista = new List<Producto>();

    }
}
