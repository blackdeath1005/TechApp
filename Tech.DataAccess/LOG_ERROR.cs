//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tech.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class LOG_ERROR
    {
        public int Co_LogError { get; set; }
        public int Co_Error { get; set; }
        public int Co_Equipo { get; set; }
        public System.DateTime Fe_LogError { get; set; }
        public string Tx_Detalle { get; set; }
        public string Tx_Voltaje { get; set; }
        public string Tx_Alineacion { get; set; }
        public string Tx_Visualizacion { get; set; }
        public string Tx_VGA { get; set; }
        public string Tx_COM { get; set; }
        public string Tx_Energia { get; set; }
        public string Tx_Sistema { get; set; }
        public string Tx_Archivo { get; set; }
    
        public virtual EQUIPO EQUIPO { get; set; }
    }
}
