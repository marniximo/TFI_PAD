//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TFI_PAD.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Libro
    {
        public System.Guid ID { get; set; }
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string NombreArchivo { get; set; }
    
        public virtual Asignatura Asignatura { get; set; }
        public virtual Perfil Perfil { get; set; }
    }
}