using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSClass.API.Consumer.Models
{
    public class ModelState
    {

        public List<FieldState> Errors { get; set; }
        public bool ContainError => Errors == null || Errors.Count < 0;
    }
    public class State
    {
        public string Message { get; set; }
        public ModelState ModelState { get; set; }
    }
}