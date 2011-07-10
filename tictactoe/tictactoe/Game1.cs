using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace tictactoe
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum gameStates
        {
            GAME_NOT_ON, GAME_PROG, PLAY_ONE, PLAY_TWO, PLAY_TIE
        }

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public fieldContainer field;
        int playerTurn;
        public int gameCount;
        public gameStates currentGameState;
        public Texture2D smallRect, x_texture, o_texture, blank_texture, cats, exit, newGameT, play1, play2, resume, title;
        public SpriteFont spriteFont;
        GameMenu gameMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 410;
            graphics.PreferredBackBufferHeight = 410;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;
            base.Initialize();
            playerTurn = 1;
            gameCount = 0;
            currentGameState = gameStates.GAME_NOT_ON;
            gameMenu = new GameMenu(this);
            gameMenu.Update();
        }

        protected override void LoadContent()
        {
            smallRect = new Texture2D(GraphicsDevice, 1, 1);
            smallRect.SetData(new[] { Color.White });

            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.x_texture = Content.Load<Texture2D>("X");
            this.o_texture = Content.Load<Texture2D>("O");
            this.blank_texture = Content.Load<Texture2D>("blank");
            this.spriteFont = Content.Load<SpriteFont>("Pericles");
            this.cats = Content.Load<Texture2D>("cats");
            this.exit = Content.Load<Texture2D>("exit");
            this.newGameT = Content.Load<Texture2D>("newGame");
            this.play1 = Content.Load<Texture2D>("player1");
            this.play2 = Content.Load<Texture2D>("player2");
            this.resume = Content.Load<Texture2D>("resume");
            this.title = Content.Load<Texture2D>("title");
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (currentGameState != gameStates.GAME_PROG) gameMenu.Update();
            else if (currentGameState == gameStates.GAME_PROG) field.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            if (currentGameState != gameStates.GAME_PROG) gameMenu.Draw();
            else if (currentGameState == gameStates.GAME_PROG) field.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public int getPlayerTurn()
        {
            if (this.playerTurn == 1) return 1;
            else if (this.playerTurn == -1) return 2;
            else return 0;
        }

        public void nextPlayerTurn()
        {
            if (this.playerTurn == 0) this.playerTurn = 1;
            else this.playerTurn *= -1;
        }

        public void gameEnded() {
            this.gameCount++;
            this.playerTurn = 1; // Reset player turn
            Console.WriteLine("game ended {0}",this.currentGameState);
            gameMenu = new GameMenu(this);
        }

        public void newGame() {
            this.field = new fieldContainer(this);
            this.playerTurn = 1;
            this.field.Update();
        }
    }
}
