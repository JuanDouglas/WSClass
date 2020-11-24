﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSClass.API.Models
{
    /// <summary>
    /// Modelo de Autenticação
    /// </summary>
    public class Authenticated
    {
        private TaskDatabaseEntities db = new TaskDatabaseEntities();
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
        public IPModel IP { get; set; }
        /// <summary>
        /// Token de autenticação
        /// </summary>
        public string Token { get; set; }
        public Authenticated(Authentication aut)
        {
            ID = aut.ID;
            UserAgent = aut.User_Agent;
            Token = aut.Token;
            IPAddress = aut.IP;
            IP = new IPModel(db.IP.FirstOrDefault(fs => fs.IP1 == IPAddress));
        }
    }
}