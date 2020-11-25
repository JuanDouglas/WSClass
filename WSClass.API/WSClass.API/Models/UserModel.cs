using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WSClass.API.Models
{
    /// <summary>
    /// Modelo de usuário
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Identificação do usuário.
        /// </summary>
        internal int ID { get; set; }
        /// <summary>
        /// Primeiro Nome do usuário.
        /// </summary>
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 3)]
        public string FirstName { get; set; }
        /// <summary>
        /// Segundo Nome do usuário.
        /// </summary>
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 3)]
        public string SecondName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<int> Image { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public DateTime BirthDay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Phone]
        [Required]
        [StringLength(maximumLength: 24, MinimumLength = 14)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Indica se o usuário aceitou os termos.
        /// </summary>
        [Required]
        public bool AcceptedTerms { get; set; }
        /// <summary>
        /// Indica o tipo de conta
        /// </summary>
        [Required]
        public Enums.AccountType AccountType { get; set; }

        public User GetUser()
        {
            User re = new User
            {
                FirstName = FirstName,
                SecondName = SecondName,
                PhoneNumber = PhoneNumber,
                BirthDay = BirthDay,
                AccountType = (int)AccountType 
            };
            return re;
        }

        public UserModel(User user)
        {

        }

        public UserModel(int iD, string firstName, string secondName, DateTime birthDay, string phoneNumber)
        {
            ID = iD;
            FirstName = firstName;
            SecondName = secondName;
            BirthDay = birthDay;
            PhoneNumber = phoneNumber;
        }

        public UserModel()
        {
        }
    }

}