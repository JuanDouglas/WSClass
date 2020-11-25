using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WSClass.API.Models.Enums
{
    /// <summary>
    /// Define o tipo de arquivo que está sendo buscado. 
    /// </summary>
    public enum FileType
    {
        CompactFile = 1,
        ImageFile = 2,
        Undefined = 3
    }
}