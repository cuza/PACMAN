using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace PACMAN {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class XNAPacMan : Game {
        readonly GraphicsDeviceManager graphics_;
        SpriteBatch spriteBatch_;
        AudioEngine audioEngine_;
        WaveBank _waveBank;
        SoundBank soundBank_;

        public int ScreenWidth { get; set; }

        public int ScreenHeight { get; set; }

        public Vector2 Scale;

        public XNAPacMan() {
            // Pac Man 2 is somewhat resolution-independent, but runs best at 720x640.
            graphics_ = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 720,
                PreferredBackBufferWidth = 640
            };
            ScreenWidth = Window.ClientBounds.Width;
            ScreenHeight = Window.ClientBounds.Height;

            // Pac Man 2 always updates 1000 times per second. Framerate may vary.
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1);

            // The menu needs to be added in the Components list in the constructor.
            // Otherwise its Initialize() method is not called and everything blows up.
            Components.Add(new Menu(this, null));

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // This will be called before the Initialize() method of any component, which
            // all rely on these Services being available.
            audioEngine_ = new AudioEngine("Content/Audio/YEPAudio.xgs");
            _waveBank = new WaveBank(audioEngine_, "Content/Audio/Wave Bank.xwb");
            soundBank_ = new SoundBank(audioEngine_, "Content/Audio/Sound Bank.xsb");
            Services.AddService(typeof(AudioEngine), audioEngine_);
            Services.AddService(typeof(SoundBank), soundBank_);

            spriteBatch_ = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch_);
            Services.AddService(typeof(GraphicsDeviceManager), graphics_);
            Scale.X = ScreenWidth/(float) 10;
            Scale.Y = ScreenHeight/(float) 10;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // The components are automatically updated by XNA. The only relevant
            // task here is to update the AudioEngine.
            audioEngine_.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            // We _always_ clear to black, so we do this here.
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
