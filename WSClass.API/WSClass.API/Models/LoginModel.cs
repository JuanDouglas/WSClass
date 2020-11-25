using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WSClass.API.Models
{
    /// <summary>
    /// Classe Modelo de Logins.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// ID do login.
        /// </summary>
        internal int ID { get; set; }
        /// <summary>
        /// Nome de usuário usado para logar.
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Nome de Usuário")]
        [StringLength(maximumLength: 100, MinimumLength = 3, ErrorMessage = "O texto do campo {0} está fora dos limites de caracteres ({1},{2}). ")]
        public string UserName { get; set; }
        /// <summary>
        /// Senha de usuário usada no Login.
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Senha")]
        [StringLength(maximumLength: 200, MinimumLength = 6)]
        public string Password { get; set; }
        /// <summary>
        /// Senha de confirmação.
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Confirme sua Senha")]
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// Email do usuário.
        /// </summary>
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress]
        [StringLength(maximumLength: 300, MinimumLength = 3)]
        public string Email { get; set; }
        public UserModel User { get; set; }
        public LoginModel(string password, string email)
        {
            User = new UserModel();
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }

        /// <summary>
        /// Construtor a partir do Entity FreameWork 
        /// </summary>
        /// <param name="login">Classe de Login.</param>
        public LoginModel(Login login)
        {
            User = new UserModel();
            ID = login.ID;
            Password = login.Password;
            Email = login.Email;
        }

        public LoginModel()
        {
            User = new UserModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Login GetLogin()
        {
            Login lg = new Login();
            lg.UserName = UserName;
            lg.Password = Password;
            lg.Email = Email;
            return lg;
        }
        public User GetUserModel()
        {
            return User.GetUser();
        }
    }
}