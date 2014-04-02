using System;
using System.Collections.Generic;
using System.Xml;

//Xna Stuff

namespace PACMAN
{
    public class Question
    {
        private static XmlDocument _document;
        private static Random _random;

        private Question(Ghost ghost)
        {
            XmlNodeList questions = _document.GetElementsByTagName("Ejercicio");
            XmlNode question = questions[_random.Next(questions.Count)];
            foreach (XmlElement node in question.ChildNodes)
            {
                if (node.Name == "Pregunta")
                {
                    Text = node.InnerText;
                }
            }

            Responses = new List<string>();
            var respuestas = new List<string>();
            var correctas = new List<string>();
            foreach (XmlElement node in question)
            {
                if (node.Name == "Respuesta")
                {
                    respuestas.Add(node.InnerText);
                    if (node.Attributes[0].Name == "Correcto" &&
                        node.Attributes[0].Value == "si")
                    {
                        correctas.Add(node.InnerText);
                    }
                }
            }
            Responses.AddRange(respuestas);
            Corrects = new List<string>();
            Corrects.AddRange(correctas);
            Ghost = ghost;
        }

        public Ghost Ghost { get; set; }
        public string Text { get; set; }
        public List<string> Responses { get; set; }
        public List<string> Corrects { get; set; }

        public static Question GetQuestion(Ghost ghost)
        {
            if (_document == null)
            {
                _document = new XmlDocument();
                _random = new Random();
                _document.Load("Content/questions.xml");
            }
            return new Question(ghost);
        }

        public string[] GetItems()
        {
            var items = new List<string> {Text};
            items.AddRange(Responses);
            return items.ToArray();
        }
    }
}