using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSClass.API.Consumer.Models
{
    public class IP
    {
        /// <summary>
        /// ID do IP no banco de dados.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Endereço de IP Cadastrado.
        /// </summary>
        public string IPAdress { get; set; }
        /// <summary>
        /// Indica o nível de confiança do sistema neste IP. 
        /// </summary>
        public Enums.Confiance Confiance { get; set; }
        /// <summary>
        /// Indica se este IP já foi banido.
        /// </summary>
        public bool AlreadyBeenBanned { get; set; }
    }
}