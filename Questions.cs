using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Xna Stuff

namespace XNAPacMan
{

    public class Question
    {
        private static XmlDocument _document;
        private static Random _random;

        public Ghost Ghost { get; set; }
        public string Text { get; set; }
        public List<string> Responses { get; set; }
        public List<string> Corrects { get; set; }

        private Question(Ghost ghost)
        {
            _document.Load("Content/questions.xml");
            var questions = _document.GetElementsByTagName("Ejercicio");
            var question = questions[_random.Next(questions.Count)];
            foreach (XmlElement node in question.ChildNodes)
            {
                if (node.Name == )
                {
                    
                }
            }
            Text = "Text very large on the fucking question";
            Responses = new List<string>();
            Responses.AddRange(new []{"0","1","2","3"});
            Corrects= new List<string>();
            Corrects.AddRange(new []{"1","2"});
            Ghost = ghost;
        }

        public static Question GetQuestion(Ghost ghost)
        {
            if (_document == null)
            {
                _document = new XmlDocument();
                _random = new Random();
            }
            return new Question(ghost);
        }

        public string[] GetItems()
        {
            List<string>items = new List<string>();
            items.Add(Text);
            items.AddRange(Responses);
            return items.ToArray();
        }
    }
}
