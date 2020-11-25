using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSClass.API.Consumer.Models
{
    public class Authenticated
    {
        /// <summary>
        /// Numéro de identificação da autenticação.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Agente utilizado na requisição.
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// Endereço da Autenticação.
        /// </summary>
        private string IPAddress { get; set; }
        /// <summary>
        /// IP da autenticação.
        /// </summary>
        public IP IP { get; set; }
        /// <summary>
        /// Token de autenticação
        /// </summary>
        public string Token { get; set; }
        public string ValidKey { get; set; }
    }
}