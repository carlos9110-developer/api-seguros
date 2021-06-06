using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiSeguros.Models.WS
{
    public class AfiliadosDTO
    {
        public int documento { get; set; }
        public string nombres { get; set; }

        public string apellidos { get; set; }
        public System.DateTime fecha_nacimiento { get; set; }
        public string lugar_residencia { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public int valor_plan { get; set; }
        public int monto_asegurado { get; set; }
        
    }

    public class DatosConsultadosAfiliado
    {
        public int id { get; set; }
        public string nombres { get; set; }

        public string apellidos { get; set; }
        public System.DateTime fecha_nacimiento { get; set; }

        public System.DateTime fecha_registro { get; set; }
        public string lugar_residencia { get; set; }
        public int valor_plan { get; set; }
        public int monto_asegurado { get; set; }

    }


    public class DesactivacionesDTO
    {
        public int id_afiliado { get; set; }
        public System.DateTime fecha { get; set; }
    }

    public class DesactivarDTO
    {
        public int id { get; set; }
    }


}