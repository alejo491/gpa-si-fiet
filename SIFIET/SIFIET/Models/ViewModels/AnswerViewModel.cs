using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIFIET.Models.ViewModels
{
    /// <summary>
    /// Modelo de Vista
    /// </summary>
    public class AnswerViewModel
    {
        
        
        /// <summary>
        /// Diccionario con preguntas
        /// </summary>
        public Dictionary<Question, String> Questions { get; set; }

        /// <summary>
        /// Metodo que asigna preguntas a un tema
        /// </summary>
        /// <param name="questions">Preguntas</param>
        public AnswerViewModel(IEnumerable<Question> questions)
        {    
            this.Questions = new Dictionary<Question, string>();
            foreach (var topic in questions)
            {
                this.Questions.Add(topic, "");
            }
        }
    }
}