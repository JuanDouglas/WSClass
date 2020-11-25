using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using WSClass.API.Models;
using WSClass.API.Models.Exceptions;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using System.Web.Http.Description;

namespace WSClass.API.Controllers
{
    /// <summary>
    /// Api Login Controller.
    /// </summary>
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        private TaskDatabaseEntities db = new TaskDatabaseEntities();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        #region GetLogin Informations
        [HttpGet]
        [Route("Informations")]
        public async Task<IHttpActionResult> GetAsync(string login_token)
        {
            //Obtém o contexto da solicitação 
            HttpRequest context = HttpContext.Current.Request;

            //Obtém a autenticação deste usuário
            Authentication logToken = await db.Authentication.Where(fs => fs.IP == context.UserHostAddress).FirstOrDefaultAsync(fs => fs.Token == login_token.ToString());

            //Obtém o usuário pelo nome de usuário
            Login logUser = await db.Login.FirstOrDefaultAsync(fs => fs.UserName == logToken.Login1.UserName);

            //Verifica se os objetos retornados são nulos e caso sejam retorna um 'NotFound'
            if (logUser == null)
            {
                HttpResponseMessage response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Read this request headers")
                };
                response.Headers.Add("X-Error", "The user name did not return any results.");
                return ResponseMessage(response);
            }

            if (logToken == null)
            {
                HttpResponseMessage response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Read the headers of this request")
                };
                response.Headers.Add("X-Error", "The login token did not return any results.");
                return ResponseMessage(response);
            }

            //Caso o ban esteja ativado irá verificar se já foi banido
            //if (GetConfigurations.ActiveBans)
            //{
            //    Ban ban = await db.Bans.Where(fs=>fs.BlackList.IP==context.UserHostAddress).
            //            FirstOrDefaultAsync(fs=>fs.AppliedDate.Add(fs.Time)<GetConfigurations.Now);
            //    if (ban != null)
            //    {
            //        HttpResponseMessage response = new HttpResponseMessage {
            //            StatusCode = HttpStatusCode.Unauthorized,
            //            Content = new StringContent(
            //            JsonConvert.SerializeObject(
            //                new BlackListProfile(ban)))
            //        };
            //        response.Headers.Add("X-Error", "You are banned!");
            //        return ResponseMessage(response);
            //    }
            //}

            //Verifica se o token corresponde ao 'UserName'
            if (logToken.Login1.UserName != logUser.UserName)
            {
                HttpResponseMessage response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Content = new StringContent("Read the headers of this request")
                };
                response.Headers.Add("X-Error", "Some requisition fields return different values.");
                response.Headers.Add("Conflict-Field", "user = UserName");
                response.Headers.Add("Conflicting-Field", "token = Authentication Token");
                return ResponseMessage(response);
            }
            var model = new LoginModel(logUser);
            model.Password = "********";
            //Retorna o login 
            return Ok(model);
        }
        [Route("Informations/GetLoginImage")]
        public async Task<IHttpActionResult> GetAsync(Guid login_token, string user_key)
        {
            //HttpContext context = HttpContext.Current;
            ////Valida o usuário
            //try
            //{
            //    await ValidUserAsync(login_token, context, user_key);
            //}
            //catch (AuthenticationException e)
            //{
            //    return ResponseMessage(e.Response);
            //}
            //UriBuilder builder = new UriBuilder(Request.RequestUri);
            //try
            //{
            //    var login = await db.Authentication.FirstOrDefaultAsync(fs => fs.Token == login_token.ToString());
            //    builder.Path = "api/Files/Image";
            //    if (login.Login1.User.First().Image == null)
            //    {
            //        throw new Exception();
            //    }
            //    builder.Query = $"filename={login.Login1.User.First().Image.FileName}&filetype={(int)Models.Enums.FileType.ProfileImage}&extension={Models.Enums.ImageExtension.jpeg}";
            //    return Redirect(builder.Uri.ToString());
            //}
            //catch (Exception)
            //{
            //    builder.Path = "Resources/person.png";
            //    builder.Query = string.Empty;
            //    return Redirect(builder.Uri.ToString());
            //}
            throw new NotImplementedException();
        }
        #endregion

        #region Authentication
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login_token"></param>
        /// <param name="valid_key"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Authetication/Logout")]
        public async Task<IHttpActionResult> GetAsync(string login_token, string valid_key,string ip)
        {
            try
            {
                await ValidUserAsync(Guid.Parse(login_token),HttpContext.Current,valid_key);
            }
            catch (AuthenticationException e)
            {
                return InternalServerError(e);
            }
            Authentication auth = await db.Authentication.FirstOrDefaultAsync(fs=>fs.IP==ip);
            
            return Ok(new Authenticated(auth,valid_key));
        }
        [HttpGet]
        [ResponseType(typeof(Authenticated))]
        [Route("Authetication")]
        public async Task<IHttpActionResult> GetAsync(string user, string pas)
        {
            //obtém o contexto desta requisição 
            HttpRequest context = HttpContext.Current.Request;

            //Obtém o login no banco de dados
            Login result = await db.Login.FirstOrDefaultAsync(fs => fs.UserName == user);
            if (result == null)
            {//Obtém o login no banco de dados
                result = await db.Login.FirstOrDefaultAsync(fs => fs.Email == user);
            }
            //Verificar se o usuário existe.
            if (result != null)
            {
                //Caso o ban esteja ativado irá verificar se já foi banido
                //if (GetConfigurations.ActiveBans)
                //{
                //    Ban ban = await db.Bans.Where(fs=>fs.BlackList.IP==context.UserHostAddress).
                //        FirstOrDefaultAsync(fs=>fs.AppliedDate.Add(fs.Time)<GetConfigurations.Now);
                //    if (ban != null)
                //    {
                //        HttpResponseMessage response = new HttpResponseMessage();
                //        response.StatusCode = HttpStatusCode.Unauthorized;
                //        response.Content = new StringContent(
                //            JsonConvert.SerializeObject(
                //                new BlackListProfile(ban)));
                //        response.Headers.Add("X-Error", "You are banned!");
                //        return ResponseMessage(response);
                //    }
                //}

                //Verifica se a senha está correta
                if (!PasswordCompare(result.Password, pas))
                {
                    //Retorna um 'Unauthorized' caso a senha esteja errada 'UserName'
                    return Unauthorized();

                }
                else
                {
                    //Cria o Authenticate object
                    Authentication authentication;
                    try
                    {
                        //Verifica se ele já existe e alterar
                        authentication = await db.Authentication.Where(fs => fs.Login1.ID == result.ID).
                        FirstOrDefaultAsync(fs => fs.IP == context.UserHostAddress);
                        if (authentication == null)
                        {
                            throw new ArgumentNullException();
                        }
                    }
                    catch (ArgumentNullException)
                    {
                        //Cria um novo e adiona no banco caso não exista
                        authentication = new Authentication()
                        {
                            Date = DateTime.Now,
                            IP = context.UserHostAddress,
                            Token = Guid.NewGuid().ToString(),
                            User_Agent = context.UserAgent,
                            Login = result.ID,
                            Login1 = result
                        };
                        var existIP = await db.IP.FirstOrDefaultAsync(fs => fs.IP1 == context.UserHostAddress);
                        if (existIP == null)
                        {
                            existIP = new IP
                            {
                                IP1 = context.UserHostAddress,
                                AlreadyBeenBanned = false,
                                Confiance = 2
                            };
                            db.IP.Add(existIP);
                            await db.SaveChangesAsync();
                        }
                        db.Authentication.Add(authentication);
                        await db.SaveChangesAsync();
                    }

                    //Retorna o objeto da authenticação 
                    HttpResponseMessage response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(
                            JsonConvert.SerializeObject(
                                new Authenticated(authentication, 
                                (await db.User.FirstOrDefaultAsync(fs => fs.LoginID == result.ID)).ValidKey)))
                    };
                    return ResponseMessage(response);

                }
            }
            else
            {

                //Retorna um 'NotFound' caso não encontre nenhum login com este 'UserName'
                return NotFound();

            }
        }
        #endregion

        #region VerifyEmail
        [HttpGet]
        [Route("Authetication/ValidEmail")]
        public async Task<IHttpActionResult> GetAsync(Guid login_token, string user_key, string valid)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Delete
        /// <summary>
        /// Excluir um login
        /// </summary>
        /// <param name="user_key">Nome de Usuário</param>
        /// <param name="pas">Senha de login</param>
        /// <param name="login_token">Token de login</param>
        /// <returns></returns>
        [ResponseType(typeof(LoginModel))]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync(string user_key, string pas, Guid login_token)
        {
            //obtém o contexto desta requisição 
            HttpContext context = HttpContext.Current;

            //Obtém o login no banco de dados
            Login result = db.Authentication.FirstOrDefault(fs => fs.Token == login_token.ToString()).Login1;

            //Verificar se o usuário existe.
            if (result != null)
            {
                //Retorna um 'NotFound' caso não encontre nenhum login com este 'UserName'
                return NotFound();
            }

            //Valida o usuário
            try
            {
                await ValidUserAsync(login_token, context, user_key);
            }
            catch (AuthenticationException e)
            {
                return ResponseMessage(e.Response);
            }

            //Verifica se a senha está correta
            if (PasswordCompare(
                result.Password,
                pas))
            {
                return Unauthorized();
            }

            //Remove as autenticações deste usuário
            foreach (Authentication authentication in db.Authentication.Where(fs => fs.Login1.ID == result.ID))
            {
                db.Authentication.Remove(authentication);
            }


            db.Login.Remove(result);

            await db.SaveChangesAsync();
            return Ok(new LoginModel(result));
        }
        #endregion

        #region Put
        /// <summary>
        /// Adiciona novo Login.
        /// </summary>
        /// <param name="login">Modelo Contendo informações de login.</param>
        /// <returns>retorna o modelo com as informações adionadas.</returns>
        [ResponseType(typeof(LoginModel))]
        [Route("Authetication/Create")]
        [HttpPut]
        public async Task<IHttpActionResult> Put([FromBody]LoginModel login)
        {
            #region ValidModel
            await ValidModelAsync(login);
            //Verifica se o modelo é valido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #endregion

            #region LoginInsert
            //Criptografa a senha
            login.Password = CryptographyString(login.Password);

            //Trasforma a Classe LoginModel em um mOdelo do banco de dados.
            var loginModel = login.GetLogin();

            //Adiciona a data de criação do login
            loginModel.CreateDate = DateTime.UtcNow; ;
            try
            {
                //Adiciona ao DBContext
                db.Login.Add(loginModel);

                //Salva no banco
                await db.SaveChangesAsync();
            }
            catch (APIException)
            {
                return InternalServerError();
            }

            #endregion

            #region UserInsert
            //Obtém o Modelo do banco de dados.
            var userModel = login.GetUserModel();

            loginModel = await db.Login.FirstOrDefaultAsync(predicate: fs => fs.UserName == login.UserName);

            try
            {

                userModel.LoginID = loginModel.ID;

                //Obtém uma chave de usuário valida.
                bool validKey = false;
                do
                {
                    userModel.ValidKey = Regex.Replace(Guid.NewGuid().ToString(), "[A-Za-z0-9]", "");
                    var result = await db.User.FirstOrDefaultAsync(fs => fs.ValidKey == userModel.ValidKey);
                    if (result == null)
                    {
                        validKey = true;
                    }
                } while (!validKey);

                db.User.Add(userModel);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                db.Login.Remove(loginModel);
                await db.SaveChangesAsync();
                return InternalServerError();
            }
            #endregion
            return Ok(login);
        }
        #endregion

        #region Post
        /// <summary>
        /// Atualizar Login
        /// </summary>
        /// <param name="login_id">ID de usuário</param>
        /// <param name="model">Modelo com as informações para atualizar</param>
        /// <param name="login_token">Token de login</param>
        /// <param name="user_key">Chave de verificação do usuário</param>
        /// <returns>Novo Modelo com as informações atualizadas.</returns>
        [ResponseType(typeof(LoginModel))]
        [HttpPost]
        public async Task<IHttpActionResult> Post(int login_id, string user_key, [FromBody] LoginModel model, Guid login_token)
        {
            //Obtém o usuário pelo nome de usuário
            Login logUser = await db.Login.FirstOrDefaultAsync(fs => fs.ID == login_id);

            //Obtém o contexto da solicitação 
            HttpRequest context = HttpContext.Current.Request;

            //Valida o usuário
            try
            {
                await ValidUserAsync(login_token, HttpContext.Current, user_key);
            }
            catch (AuthenticationException e)
            {
                return ResponseMessage(e.Response);
            }

            //Verifica se o email atualizado já foi usado
            if (model.Email == null)
            {
                Login usedEmail = await db.Login.FirstOrDefaultAsync(fs => fs.Email == logUser.Email);
                if (usedEmail == null)
                {
                    HttpResponseMessage response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Conflict,
                        Content = new StringContent("Read the headers of this request")
                    };
                    response.Headers.Add("X-Error", "There is already a record with the same value as the 'UserName' field");
                    return ResponseMessage(response);
                }
            }

            //Verifica se o nome de usuário já foi usado
            if (model.UserName == null)
            {
                Login usedEmail = await db.Login.FirstOrDefaultAsync(fs => fs.Email == logUser.Email);
                if (usedEmail == null)
                {
                    HttpResponseMessage response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Conflict,
                        Content = new StringContent("Read the headers of this request")
                    };
                    response.Headers.Add("X-Error", "There is already a record with the same value as the 'UserName' field");
                    return ResponseMessage(response);
                }
            }

            model.ID = logUser.ID;
            db.Entry(model).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok(
                new LoginModel(
                    await db.Login.FirstOrDefaultAsync(fs => fs.ID == logUser.ID)));
        }
        #endregion

        #region Cryptography services

        public static string CryptographyString(string value)
        {
            return HashGeneration(value);
        }
        public static string HashGeneration(string password)
        {
            // Configurations
            int workfactor = 10; // 2 ^ (10) = 1024 iterations.

            string salt = BCrypt.Net.BCrypt.GenerateSalt(workfactor);
            string hash = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hash;
        }
        public static bool PasswordCompare(string hash, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        #endregion

        #region ValidUser

        public async static System.Threading.Tasks.Task ValidUserAsync(Guid token, HttpContext context, string user_valid_key)
        {
            TaskDatabaseEntities db = new TaskDatabaseEntities();

            //Caso o ban esteja ativado irá verificar se já foi banido
            //if (GetConfigurations.ActiveBans)
            //{
            //    Ban ban = await db.Bans.Where(fs => fs.BlackList.IP == context.UserHostAddress).
            //            FirstOrDefaultAsync(fs => fs.AppliedDate.Add(fs.Time) < GetConfigurations.Now);
            //    if (ban != null)
            //    {
            //        HttpResponseMessage response = new HttpResponseMessage
            //        {
            //            StatusCode = HttpStatusCode.Unauthorized,
            //            Content = new StringContent(
            //            JsonConvert.SerializeObject(
            //                new BlackListProfile(ban)))
            //        };
            //        response.Headers.Add("X-Error", "You are banned!");
            //        return ResponseMessage(response);
            //    }
            //}

            //Obtém a autenticação deste usuário
            Authentication logToken = await db.Authentication.FirstOrDefaultAsync(fs => fs.Token == token.ToString());
            if (logToken == null)
            {
                HttpResponseMessage response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Read the headers of this request")
                };
                response.Headers.Add("X-Error", "This token does not exist or does not exist on the system or is wrong.");

                throw new Models.Exceptions.AuthenticationException("This token does not exist or does not exist on the system or is wrong.", response, context);
            }

            //Valida o IP fornecido
            if (context.Request.UserHostAddress != logToken.IP)
            {
                HttpResponseMessage response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent("Read the headers of this request")
                };
                response.Headers.Add("X-Error", "This token is not valid for this request.");

                throw new Models.Exceptions.AuthenticationException("This token is not valid for this request.", response, context);
            }

            //Obtém o usuário deste login
            User user = logToken.Login1.User.FirstOrDefault(fs => true);

            //Valida a chave de verificação da tabela User.
            if (user_valid_key != user.ValidKey)
            {
                HttpResponseMessage response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Content = new StringContent("Read the headers of this request")
                };
                response.Headers.Add("X-Error", "This token does not exist or does not exist on the system or is wrong.");

                throw new Models.Exceptions.AuthenticationException("This token does not exist or does not exist on the system or is wrong.", response, context);
            }
        }
        public static void ValidUser(Guid token, HttpContext context, string user_valid_key)
        {
            TaskDatabaseEntities db = new TaskDatabaseEntities();

            //Caso o ban esteja ativado irá verificar se já foi banido
            //if (GetConfigurations.ActiveBans)
            //{
            //    Ban ban = await db.Bans.Where(fs=>fs.BlackList.IP==context.UserHostAddress).
            //            FirstOrDefaultAsync(fs=>fs.AppliedDate.Add(fs.Time)<GetConfigurations.Now);
            //    if (ban != null)
            //    {
            //        HttpResponseMessage response = new HttpResponseMessage {
            //            StatusCode = HttpStatusCode.Unauthorized,
            //            Content = new StringContent(
            //            JsonConvert.SerializeObject(
            //                new BlackListProfile(ban)))
            //        };
            //        response.Headers.Add("X-Error", "You are banned!");
            //        return ResponseMessage(response);
            //    }
            //}

            //Obtém a autenticação deste usuário
            Authentication logToken = db.Authentication.FirstOrDefault(fs => fs.Token == token.ToString());
            if (logToken == null)
            {
                HttpResponseMessage response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Read the headers of this request")
                };
                response.Headers.Add("X-Error", "This token does not exist or does not exist on the system or is wrong.");

                throw new Models.Exceptions.AuthenticationException("This token does not exist or does not exist on the system or is wrong.", response, context);
            }

            //Valida o IP fornecido
            if (context.Request.UserHostAddress != logToken.IP)
            {
                HttpResponseMessage response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent("Read the headers of this request")
                };
                response.Headers.Add("X-Error", "This token is not valid for this request.");

                throw new Models.Exceptions.AuthenticationException("This token is not valid for this request.", response, context);
            }

            //Obtém o usuário deste login
            User user = logToken.Login1.User.FirstOrDefault(fs => true);

            //Valida a chave de verificação da tabela User.
            if (user_valid_key != user.ValidKey)
            {
                HttpResponseMessage response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Content = new StringContent("Read the headers of this request")
                };
                response.Headers.Add("X-Error", "This token does not exist or does not exist on the system or is wrong.");

                throw new Models.Exceptions.AuthenticationException("This token does not exist or does not exist on the system or is wrong.", response, context);
            }
        }


        #endregion

        private async System.Threading.Tasks.Task ValidModelAsync(LoginModel login) {
            //verifica se o login e nulo
            if (login == null)
            {
                login = new LoginModel();
                //return BadRequest();
            }

            //Verifica se a senha e nula
            if (login.Password == null)
            {
                ModelState.AddModelError("Password", "Field is required");
                login.Password = string.Empty;
            }

            //Verifica se a senha de confirmação e nula
            if (login.ConfirmPassword == null)
            {
                ModelState.AddModelError("ConfirmPassword", "Field is required");
                login.ConfirmPassword = string.Empty;
            }

            if (login.Email == null)
            {
                ModelState.AddModelError("Email", "Field is required");
                login.Email = string.Empty;
            }

            if (login.UserName == null)
            {
                ModelState.AddModelError("UserName", "Field is required");
                login.UserName = string.Empty;
            }

            if (login.User ==null)
            {
                ModelState.AddModelError("User", "Field is required");
            }

            //Verifica se as senhas coincidem.
            if (login.Password != login.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords must match!");
            }

            //Valida se já existe um Login com o mesmo E-mail cadastrado.
            Login exists = await db.Login.FirstOrDefaultAsync(fs => fs.Email == login.Email);
            if (exists != null)
            {
                ModelState.AddModelError("Email", "There is already a registration with this 'E-mail'.");
            }

            //Valida se já existe um Login com o mesmo UserName cadastrado.
            exists = await db.Login.FirstOrDefaultAsync(fs => fs.UserName == login.UserName);
            if (exists != null)
            {
                ModelState.AddModelError("UserName", "There is already a registration with this 'UserName'.");
            }
            
        }
    }
}