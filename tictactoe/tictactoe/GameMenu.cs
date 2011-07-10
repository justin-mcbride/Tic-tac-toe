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
    class GameMenu
    {
        /*
         * VARIABLES
         */
        Game1 m_parent;
        Vector2 title_vector_pos, new_vector_pos, status_vector_pos, exit_vector_pos;
        MouseState currentMouse, previousMouse;
        public Texture2D currentState;


        /*
         * FUNCTIONS
         */

        public GameMenu(Game1 parent) {
            this.m_parent = parent;
            this.Init();
        }

        public void Init() {
            this.title_vector_pos = new Vector2(125, 40);
            this.new_vector_pos = new Vector2(this.title_vector_pos.X, this.title_vector_pos.Y + 85);
            this.status_vector_pos = new Vector2(this.new_vector_pos.X, this.new_vector_pos.Y + 85);
            this.exit_vector_pos = new Vector2(this.status_vector_pos.X, this.status_vector_pos.Y + 85);
            this.currentState = new Texture2D(m_parent.GraphicsDevice,1,1);
        }

        public void Update() {

            this.currentMouse = Mouse.GetState();

            if (this.currentMouse.LeftButton == ButtonState.Pressed) {
                this.previousMouse = this.currentMouse;
            }
            else if (this.currentMouse.LeftButton == ButtonState.Released) {
                if (this.previousMouse.LeftButton == ButtonState.Pressed) {
                    this.MouseClick();
                }
                this.previousMouse = this.currentMouse;
            }

            if (m_parent.currentGameState == Game1.gameStates.GAME_PROG) this.currentState = m_parent.resume;
            else if (m_parent.currentGameState == Game1.gameStates.PLAY_ONE) this.currentState = m_parent.play1;
            else if (m_parent.currentGameState == Game1.gameStates.PLAY_TWO) this.currentState = m_parent.play2;
            else if (m_parent.currentGameState == Game1.gameStates.PLAY_TIE) this.currentState = m_parent.cats;

        }

        public void Draw() {
            SpriteBatch sprite =  m_parent.spriteBatch;
            m_parent.GraphicsDevice.Clear(Color.Red);

            sprite.Draw(m_parent.title, this.title_vector_pos, Color.White);
            sprite.Draw(m_parent.newGameT, this.new_vector_pos, Color.White);
            if (m_parent.gameCount > 0) sprite.Draw(this.currentState, this.status_vector_pos, Color.White);
            sprite.Draw(m_parent.exit, this.exit_vector_pos, Color.White);

        }

        protected void MouseClick() {
            float tvx, tvy;
            tvx = currentMouse.X;
            tvy = currentMouse.Y;
            if ((tvx >= this.new_vector_pos.X) && (tvx <= this.new_vector_pos.X + m_parent.newGameT.Width) && (tvy >= this.new_vector_pos.Y) && (tvy <= this.new_vector_pos.Y + m_parent.newGameT.Height)) m_parent.newGame();
            else if ((tvx >= this.title_vector_pos.X) && (tvx <= this.title_vector_pos.X + m_parent.title.Width) && (tvy >= this.title_vector_pos.Y) && (tvy <= this.title_vector_pos.Y + m_parent.title.Height)) ;
            else if ((tvx >= this.exit_vector_pos.X) && (tvx <= this.exit_vector_pos.X + m_parent.exit.Width) && (tvy >= this.exit_vector_pos.Y) && (tvy <= this.exit_vector_pos.Y + m_parent.exit.Height)) m_parent.Exit();
        }


        protected void resumeGame() {
            m_parent.currentGameState = Game1.gameStates.GAME_PROG;
        }

        protected void exitGame() {
            m_parent.Exit();
        }
    }
}
