using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace tictactoe
{
    public enum tileStates
    {
        empty, o, x
    }

    public class fieldContainer
    {
        /*
         * VARIABLES
         */

        public struct Tiles
        {
            public Texture2D texture;
            public Vector2 position;
            public tileStates state;
        }
        public Tiles[,] tiles;
        private Game1 m_parent;
        protected Vector2 spacing, font_vec, fontOrigin;
        protected MouseState mouseCurrent, mousePrevious;
        protected string fontString;

        /* 
         * FUNCTIONS
         */

        public fieldContainer(Game1 parent) {
            m_parent = parent;
            this.Init();
        }

        public void Init() {
            tiles = new Tiles[3, 3];
            this.spacing.X = 80;
            this.spacing.Y = 80;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    this.setTexture(i, j, m_parent.blank_texture);
                    this.tiles[i, j].position.X = this.spacing.X + (this.tiles[i, j].texture.Width * i);
                    this.tiles[i, j].position.Y = this.spacing.Y + (this.tiles[i, j].texture.Height * j);
                    this.tiles[i, j].state = tileStates.empty;
                }
            }
            this.mouseCurrent = new MouseState();
            this.mousePrevious = new MouseState();
            this.font_vec = new Vector2();
            m_parent.currentGameState = Game1.gameStates.GAME_PROG;
        }

        public void setTexture(int i, int j, Texture2D text) {
            this.tiles[i, j].texture = text;
        }

        public void Update() {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (this.tiles[i, j].state == tileStates.o) this.tiles[i, j].texture = m_parent.o_texture;
                    else if (this.tiles[i, j].state == tileStates.x) this.tiles[i, j].texture = m_parent.x_texture;
                }
            }

            this.mouseCurrent = Mouse.GetState();

            if (this.mouseCurrent.LeftButton == ButtonState.Pressed) {
                this.mousePrevious = this.mouseCurrent;
            }
            else if (this.mouseCurrent.LeftButton == ButtonState.Released) {
                if (this.mousePrevious.LeftButton == ButtonState.Pressed) {
                    this.MouseClick();
                }
                this.mousePrevious = this.mouseCurrent;
            }

            // Font work
            this.font_vec.X = this.tiles[0, 0].position.X + ((this.tiles[0, 0].texture.Width * 3) / 2);
            this.font_vec.Y = this.tiles[0, 0].position.Y - (spacing.Y / 2);
            if (m_parent.getPlayerTurn() == 1) this.fontString = "Player one's turn!";
            else if (m_parent.getPlayerTurn() == 2) this.fontString = "Player two's turn!";
            else this.fontString = "blank";
            fontOrigin = (m_parent.spriteFont.MeasureString(fontString) / 2);
        }

        public void Draw() {
            m_parent.GraphicsDevice.Clear(Color.DarkCyan);
            // Draw tiles
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    m_parent.spriteBatch.Draw(this.tiles[i, j].texture, this.tiles[i, j].position, Color.White);
                }
            }

            // Draw game board
            m_parent.spriteBatch.DrawString(m_parent.spriteFont, this.fontString, font_vec, Color.Red, 0, fontOrigin, 1f, SpriteEffects.None, .5f);
            m_parent.spriteBatch.Draw(m_parent.smallRect, new Rectangle(((int)this.tiles[0, 0].position.X + m_parent.blank_texture.Width + 3), (int)(this.tiles[0, 0].position.Y), 3, (m_parent.blank_texture.Width * 3)), Color.Red);
            m_parent.spriteBatch.Draw(m_parent.smallRect, new Rectangle(((int)this.tiles[0, 1].position.Y + m_parent.blank_texture.Width + 3), (int)(this.tiles[0, 0].position.Y), 3, (m_parent.blank_texture.Width * 3)), Color.Red);
            m_parent.spriteBatch.Draw(m_parent.smallRect, new Rectangle((int)(this.tiles[0, 0].position.Y), ((int)this.tiles[0, 0].position.X + m_parent.blank_texture.Width + 3), (m_parent.blank_texture.Height * 3), 3), Color.Red);
            m_parent.spriteBatch.Draw(m_parent.smallRect, new Rectangle((int)(this.tiles[0, 0].position.X), ((int)this.tiles[1, 0].position.X + m_parent.blank_texture.Width + 3), (m_parent.blank_texture.Height * 3), 3), Color.Red);


        }

        public void MouseClick() {
            float tvx, tvy;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    tvx = this.tiles[i, j].position.X;
                    tvy = this.tiles[i, j].position.Y;
                    if ((tvx <= mouseCurrent.X) && (mouseCurrent.X <= (tvx + (float)this.tiles[i, j].texture.Width)) && ((tvy <= mouseCurrent.Y) && (mouseCurrent.Y <= (tvy + (float)this.tiles[i, j].texture.Height)))) {
                        this.clickedTile(ref tiles[i, j]);
                    }
                }
            }
        }


        public void clickedTile(ref Tiles tile) {
            if (tile.state == tileStates.empty) {
                int i = m_parent.getPlayerTurn();
                if (i == 1) {
                    tile.state = tileStates.x;
                    m_parent.nextPlayerTurn();
                    gameCheck();
                }
                else if (i == 2) {
                    tile.state = tileStates.o;
                    m_parent.nextPlayerTurn();
                    gameCheck();
                }
            }
            else { };
        }

        public void gameCheck() {
            // First row
            if ((this.tiles[0, 0].state == tileStates.x) && (this.tiles[0, 1].state == tileStates.x) && (this.tiles[0, 2].state == tileStates.x)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_ONE;
                m_parent.gameEnded();

            }
            // Down diagnol
            else if ((this.tiles[0, 0].state == tileStates.x) && (this.tiles[1, 1].state == tileStates.x) && (this.tiles[2, 2].state == tileStates.x)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_ONE;
                m_parent.gameEnded();

            }
            // First column
            else if ((this.tiles[0, 0].state == tileStates.x) && (this.tiles[1, 0].state == tileStates.x) && (this.tiles[2, 0].state == tileStates.x)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_ONE;
                m_parent.gameEnded();

            }
            // Third row
            else if ((this.tiles[2, 0].state == tileStates.x) && (this.tiles[2, 1].state == tileStates.x) && (this.tiles[2, 2].state == tileStates.x)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_ONE;
                m_parent.gameEnded();

            }
            // Up diagnol
            else if ((this.tiles[2, 0].state == tileStates.x) && (this.tiles[1, 1].state == tileStates.x) && (this.tiles[0, 2].state == tileStates.x)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_ONE;
                m_parent.gameEnded();

            }
            // Second column
            else if ((this.tiles[0, 1].state == tileStates.x) && (this.tiles[1, 1].state == tileStates.x) && (this.tiles[2, 1].state == tileStates.x)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_ONE;
                m_parent.gameEnded();

            }
            // Second row
            else if ((this.tiles[1, 0].state == tileStates.x) && (this.tiles[1, 1].state == tileStates.x) && (this.tiles[1, 2].state == tileStates.x)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_ONE;
                m_parent.gameEnded();

            }
            // Third column
            else if ((this.tiles[0, 2].state == tileStates.x) && (this.tiles[1, 2].state == tileStates.x) && (this.tiles[2, 2].state == tileStates.x)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_ONE;
                m_parent.gameEnded();

            }

            // SECOND PLAYER

             // First row
            else if ((this.tiles[0, 0].state == tileStates.o) && (this.tiles[0, 1].state == tileStates.o) && (this.tiles[0, 2].state == tileStates.o)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_TWO;
                m_parent.gameEnded();

            }
            // Down diagnol
            else if ((this.tiles[0, 0].state == tileStates.o) && (this.tiles[1, 1].state == tileStates.o) && (this.tiles[2, 2].state == tileStates.o)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_TWO;
                m_parent.gameEnded();

            }
            // First column
            else if ((this.tiles[0, 0].state == tileStates.o) && (this.tiles[1, 0].state == tileStates.o) && (this.tiles[2, 0].state == tileStates.o)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_TWO;
                m_parent.gameEnded();

            }
            // Third row
            else if ((this.tiles[2, 0].state == tileStates.o) && (this.tiles[2, 1].state == tileStates.o) && (this.tiles[2, 2].state == tileStates.o)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_TWO;
                m_parent.gameEnded();

            }
            // Up diagnol
            else if ((this.tiles[2, 0].state == tileStates.o) && (this.tiles[1, 1].state == tileStates.o) && (this.tiles[0, 2].state == tileStates.o)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_TWO;
                m_parent.gameEnded();

            }
            // Second column
            else if ((this.tiles[0, 1].state == tileStates.o) && (this.tiles[1, 1].state == tileStates.o) && (this.tiles[2, 1].state == tileStates.o)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_TWO;
                m_parent.gameEnded();

            }
            // Second row
            else if ((this.tiles[1, 0].state == tileStates.o) && (this.tiles[1, 1].state == tileStates.o) && (this.tiles[1, 2].state == tileStates.o)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_TWO;
                m_parent.gameEnded();

            }
            // Third column
            else if ((this.tiles[0, 2].state == tileStates.o) && (this.tiles[1, 2].state == tileStates.o) && (this.tiles[2, 2].state == tileStates.o)) {
                m_parent.currentGameState = Game1.gameStates.PLAY_TWO;
                m_parent.gameEnded();

            }
            else {
                int count = 0;
                foreach (Tiles item in tiles) {
                    if (item.state != tileStates.empty) count++;
                }
                if (count == 9) {
                    m_parent.currentGameState = Game1.gameStates.PLAY_TIE;
                    m_parent.gameEnded();
                }
            }
        }

    }
}
