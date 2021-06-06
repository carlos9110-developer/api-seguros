using ApiSeguros.Models.DBP;
using ApiSeguros.Models.WS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiSeguros.Controllers
{
    public class SegurosController : ApiController
    {
        [HttpGet]
        public Reply GetBYCedula(int cedula)
        {
            Reply oR = new Reply();
            try
            {
                var oAfliados = ConsultarAfiliado(cedula);
                if (oAfliados != null)
                {
                    if(oAfliados.estado==1)
                    {
                        DatosConsultadosAfiliado oDatosConsultadosAfiliado = new DatosConsultadosAfiliado();
                        oDatosConsultadosAfiliado.id = oAfliados.id;
                        oDatosConsultadosAfiliado.nombres = oAfliados.nombres;
                        oDatosConsultadosAfiliado.apellidos = oAfliados.apellidos;
                        oDatosConsultadosAfiliado.fecha_nacimiento = oAfliados.fecha_nacimiento;
                        oDatosConsultadosAfiliado.fecha_registro = oAfliados.fecha;
                        oDatosConsultadosAfiliado.valor_plan = oAfliados.valor_plan;
                        oDatosConsultadosAfiliado.monto_asegurado = oAfliados.monto_asegurado;
                        oR.result = 1;
                        oR.data = oDatosConsultadosAfiliado;
                    }
                    else
                    {
                        oR.result = 0;
                        oR.message = "El cliente registrado se encuentra inactivo";
                    }
                }
                else
                {
                    oR.result = -2;
                    oR.message = "No se encontro ningún usuario con la cedula registrada";
                }
            }
            catch (Exception ex)
            {
                oR.result = -1;
                oR.message = "Ocurrio un error en el servidor, intentalo de nuevo";
            }
            return oR;
        }

        [HttpPost]
        public Reply DesactivarAfiliado([FromBody] DesactivarDTO model)
        {
            Reply oR = new Reply();
           
            using (BdsegurosUltimaEntities db = new BdsegurosUltimaEntities())
            {
                using (DbContextTransaction objConexionTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // DESACTIVAMOS EL USUARIO PASANDO SU ESTADO A 0
                        afiliados oAfiliados = db.afiliados.Find(model.id);
                        oAfiliados.estado = 0;
                        db.Entry(oAfiliados).State = System.Data.Entity.EntityState.Modified;
                        
                        // REALIZAMOS EL REGISTRO DE LA DESACTIVACIÓN
                        desactivaciones oDesactivaciones = new desactivaciones();
                        oDesactivaciones.id_afiliado = model.id;
                        oDesactivaciones.fecha = DateTime.Now;
                        db.desactivaciones.Add(oDesactivaciones);

                        db.SaveChanges();
                        objConexionTransaction.Commit();
                        oR.result = 1;
                        oR.message = "Cliente inactivado con exito";
                    }
                    catch (Exception ex)
                    {
                        objConexionTransaction.Rollback();
                        oR.result = -1;
                        oR.message = "Ocurrio un error en el servidor, intentalo de nuevo";
                    }

                }
            }
            
            
            return oR;
        }

        [HttpPost]
        public Reply RegistrarAfiliado([FromBody] AfiliadosDTO model)
        {
            Reply oR = new Reply();
                
            if ( ConsultarAfiliado(model.documento) == null  )
            {
                try
                {
                    using (BdsegurosUltimaEntities db = new BdsegurosUltimaEntities())
                    {
                        afiliados oAfiliados = new afiliados();
                        oAfiliados.cedula = model.documento.ToString();
                        oAfiliados.nombres = model.nombres;
                        oAfiliados.apellidos = model.apellidos;
                        oAfiliados.fecha_nacimiento = model.fecha_nacimiento;
                        oAfiliados.lugar_residencia = model.lugar_residencia;
                        oAfiliados.telefono = model.telefono;
                        oAfiliados.correo = model.correo;
                        oAfiliados.fecha = DateTime.Now;
                        oAfiliados.valor_plan = model.valor_plan;
                        oAfiliados.monto_asegurado = model.monto_asegurado;
                        oAfiliados.estado = 1;
                        db.afiliados.Add(oAfiliados);
                        db.SaveChanges();

                        oR.result = 1;
                        oR.message = "Cliente registrado con exito en el sistema";
                    }
                }
                catch (Exception ex)
                {
                    oR.result = 0;
                    oR.message = "Ocurrio un error en el servidor, intentalo de nuevo";
                }
            }
            else
            {
                oR.result = -1;
                oR.message = "Error, se encontró un cliente con la misma cédula en el sistema";
            }

            
            return oR;
        }


       
        #region HELPERS
        private afiliados ConsultarAfiliado(int cedula)
        {
            var cedulaString = cedula.ToString();
            using (BdsegurosUltimaEntities db = new BdsegurosUltimaEntities())
            {
                var afiliado = db.afiliados.Where(u => u.cedula == cedulaString).FirstOrDefault();
                if (afiliado != null)
                {
                    return afiliado;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

    }
}
