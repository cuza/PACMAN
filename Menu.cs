using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PACMAN
{
    /// <summary>
    ///     This is a game component that implements IUpdateable.
    /// </summary>
    /// Optionally takes a GameLoop argument, when the menu must be able to
    /// resume the current GameLoop. Otherwise, the reference would be lost
    /// and the gameLoop garbage collected.
    public class Menu : DrawableGameComponent
    {
        private readonly Ghost _ghost;
        private readonly bool _question;
        private readonly GameLoop gameLoop_;
        private bool gameStart_;
        private GraphicsDeviceManager graphics_;
        private string[] items_;
        private SpriteFont menuItem_;
        private KeyboardState oldState_;
        protected Question question;
        private Texture2D selectionArrow_;
        private int selection_;
        private SoundBank soundBank_;
        private SpriteBatch spriteBatch_;
        private Texture2D title_;

        public Menu(Game game, GameLoop gameLoop, Ghost ghost = null)
            : base(game)
        {
            gameLoop_ = gameLoop;
            gameStart_ = (gameLoop == null);
            _question = ghost != null;
            _ghost = ghost;
        }

        /// <summary>
        ///     Allows the game component to perform any initialization it needs to before starting
        ///     to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            spriteBatch_ = (SpriteBatch) Game.Services.GetService(typeof (SpriteBatch));
            graphics_ = (GraphicsDeviceManager) Game.Services.GetService(typeof (GraphicsDeviceManager));
            soundBank_ = (SoundBank) Game.Services.GetService(typeof (SoundBank));
            selection_ = 0;
            if (gameLoop_ == null)
            {
                items_ = new[] {"New Game", "High Scores", "Quit"};
            }
            else if (!_question)
            {
                items_ = new[] {"Resume", "Quit Game"};
            }
            else
            {
                question = Question.GetQuestion(_ghost);
                items_ = question.GetItems();
            }
            menuItem_ = Game.Content.Load<SpriteFont>("MenuItem");
            title_ = Game.Content.Load<Texture2D>("sprites/Title");
            selectionArrow_ = Game.Content.Load<Texture2D>("sprites/Selection");
            oldState_ = Keyboard.GetState();
            base.Initialize();
        }

        /// <summary>
        ///     Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Wonder why we test for this condition? Just replace gameStart_ by true and
            // try running the game. The answer should be instantaneous.
            if (gameStart_)
            {
                soundBank_.PlayCue("NewLevel");
                gameStart_ = false;
            }

            KeyboardState newState = Keyboard.GetState();

            // Get keys pressed now that weren't pressed before
            IEnumerable<Keys> newPressedKeys = from k in newState.GetPressedKeys()
                where !(oldState_.GetPressedKeys().Contains(k))
                select k;

            // Scroll through menu items
            if (newPressedKeys.Contains(Keys.Down))
            {
                selection_++;
                selection_ %= items_.Length;
                soundBank_.PlayCue("PacMAnEat1");
                if (selection_ == 0 && _question)
                    selection_++;
            }
            else if (newPressedKeys.Contains(Keys.Up))
            {
                selection_--;
                if (selection_ == 0 && _question)
                    selection_--;
                selection_ = (selection_ < 0 ? items_.Length - 1 : selection_);
                soundBank_.PlayCue("PacManEat2");
            }
            else if (newPressedKeys.Contains(Keys.Enter))
            {
                menuAction();
            }
            if (selection_ == 0 && _question)
                selection_++;

            // Update keyboard state for next update
            oldState_ = newState;
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // The menu is a main component, so it is responsible for initializing the sprite batch each frame
            spriteBatch_.Begin();

            // Draw title
            spriteBatch_.Draw(title_, new Vector2((graphics_.PreferredBackBufferWidth/2) - (title_.Width/2), 75),
                Color.White);

            // Draw items
            Vector2 itemPosition;
            itemPosition.X = (graphics_.PreferredBackBufferWidth/2) - 100;
            for (int i = 0; i < items_.Length; i++)
            {
                itemPosition.Y = (graphics_.PreferredBackBufferHeight/2) - 60 + (60*i);
                if (i == selection_)
                {
                    spriteBatch_.Draw(selectionArrow_, new Vector2(itemPosition.X - 50, itemPosition.Y), Color.White);
                }
                spriteBatch_.DrawString(menuItem_, items_[i], itemPosition, Color.Yellow);
            }

            spriteBatch_.End();
        }

        private void menuAction()
        {
            Game.Components.Remove(this);
            switch (items_[selection_])
            {
                case ("Resume"):
                    Game.Components.Add(gameLoop_);
                    break;
                case ("New Game"):
                    Game.Components.Add(new GameLoop(Game));
                    break;
                case ("High Scores"):
                    Game.Components.Add(new HighScores(Game));
                    break;
                case ("Quit"):
                    Game.Exit();
                    break;
                case ("Quit Game"):
                    Game.Components.Add(new Menu(Game, null));
                    SaveHighScore(gameLoop_.Score);
                    break;
                default:
                    if (question.Corrects.Contains(items_[selection_]))
                    {
                        Game.Components.Add(gameLoop_);
                        gameLoop_.KillGosht(_ghost);
                    }
                    else
                    {
                        Game.Components.Add(gameLoop_);
                        gameLoop_.KillPacMan();
                    }
                    break;
            }
        }

        /// <summary>
        ///     Keep a history of the best 10 scores
        /// </summary>
        /// <param name="highScore">New score to save, might make it inside the list, might not.</param>
        public static void SaveHighScore(int highScore)
        {
            const string fileName = "highscores.txt";
            if (!File.Exists(fileName))
            {
                File.WriteAllLines(fileName, new[] {highScore.ToString(CultureInfo.InvariantCulture)});
            }
            else
            {
                List<string> contents = File.ReadAllLines(fileName).ToList();
                contents.Add(highScore.ToString(CultureInfo.InvariantCulture));
                if (contents.Count >= 10)
                {
                    contents.Sort((a, b) => Convert.ToInt32(a).CompareTo(Convert.ToInt32(b)));
                    while (contents.Count > 10)
                    {
                        contents.RemoveAt(0);
                    }
                }
                File.WriteAllLines(fileName, contents.ToArray());
            }
        }
    }
}