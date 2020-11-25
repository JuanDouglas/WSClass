using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSClass.API.Consumer.Models
{
    public class User
    {
        internal int ID { get; set; }
        /// <summary>
        /// Primeiro Nome do usuário.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Segundo Nome do usuário.
        /// </summary>
        public string SecondName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal Image ProfileImage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime BirthDay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Indica se o usuário aceitou os termos.
        /// </summary>
        public bool AcceptedTerms { get; set; }
    }
}