//using System;
//using System.Collections.Generic;
////Xna Stuff
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Graphics.PackedVector;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;

//namespace XNAPacMan
//{
//    public abstract class Sprite : DrawableGameComponent
//    {
//        #region Campos
//        // Puntos (puntuacion ke t da kada objeto)
//        public int scoreValue { get; protected set; }

//        // Posicion del sprite
//        protected Vector2 position;

//        protected const int max_scale=75;

//        public Vector2 Position
//        {
//            get { return position; }
//            set { position = value; }
//        }

//        // Foto del sprite
//        public Texture2D Texture { get; set; }
                
//        /// Frame actual
//        protected int _currentFrame;
                
//        /// Lista de Frames
//        protected List<Rectangle> _frames;
                
//        /// Tiempo en el kual se mostro el ultimo frame
        
//        private TimeSpan _lastFrame;
                
//        /// cantidad de frames por animacion
        
//        private int _cantFrames;
                
//        /// Tiempo entre cada frame
        
//        private readonly TimeSpan _frameTime;

//        /// SpriteBatch(en XNA todo lo ke se valla a ver en pantalla requere un objeto de este tipo para mostrarse)
        
//        protected static SpriteBatch _spriteBatch;

//        /// Banco de Sonidos (jamas se uso(no dio tiempo))
        
//        private static SoundBank _soundBank;

//        private int width;
//        private int height;
//        #endregion
//        #region Constructor de clase
//        public Sprite(Game game, Vector2 position, Texture2D texture, int width, int height, int animTime, int animYPos)
//            : base(game) 
//        {
//            if (game == null || texture == null || height > texture.Height || width > texture.Width || animTime < 50)            
//            throw new Exception("Los parametros no crean un objeto Sprite");

            
//            this.width = width;
//            this.height = height;

//            Position = position;
//            Texture = texture;
//            ReadFrames(height, width, animYPos);
//            _frameTime = TimeSpan.FromMilliseconds(animTime / _cantFrames);
//            _currentFrame = 0;
//            _lastFrame = TimeSpan.Zero;
//            // Get the current spritebatch
//            _spriteBatch = (SpriteBatch)
//                           Game.Services.GetService(typeof(SpriteBatch));
//            _soundBank = (SoundBank)
//                         Game.Services.GetService(typeof(SoundBank));
        
//        }
//#endregion
//        // ste metodo se korre una vez por frame asi ke en este caso se usa para cambiar de frame y poder lograr una animacion
//        public override void Update(GameTime gameTime)
//           {
//                _lastFrame += gameTime.ElapsedGameTime;
//                if (_lastFrame > _frameTime)
//                {
//                    _lastFrame -= _frameTime;
//                   if (_currentFrame < _cantFrames - 1)
//                   {
//                       _currentFrame++;
//                   }
//                   else
//                       _currentFrame = 0; 
//                }
//                base.Update(gameTime);
//            }
//        // ste metodo se korre una vez por frame y se enkarga de mostrar los objetos en pantalla para poder lograr una animacion
//        public override void Draw(GameTime gameTime)
//        {
//            Vector2 centro = new Vector2(0, 0);
//            _spriteBatch.Draw(Texture, Position,
//                              _frames[_currentFrame], Color.White,0, centro, (Game as XNAPacMan).Scale.X / max_scale, SpriteEffects.None, 0);
//        }
//       // pica las tiras de imagenes en "rectangulos" y los guarga en una lista
//        void ReadFrames(int height, int width, int animYPos)
//        {
//            _cantFrames = Texture.Width / width;
//            _frames = new List<Rectangle>();
//            for (int i = 0; i < _cantFrames; i++)
//            _frames.Add(new Rectangle(i * width, animYPos * height, width, height));
//        }
//         //se utiliza para obtener la posicion
//        public void Pos(Vector2 position)
//        {
//            Position = position;
//        }
//        // nos devuelve un rectangulo del tamano del sprite para poder detectar colisiones
//        public Rectangle GetBounds()
//        {
//            return new Rectangle(
//            (int)Position.X + width/10,
//            (int)Position.Y - height / 10,
//            (int)(width * (Game as XNAPacMan).Scale.X / max_scale) - width / 10,
//            (int)(height * (Game as XNAPacMan).Scale.Y / max_scale) - height / 10);
//        }
//    }

//}